using AutoMapper;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Common.Models.Responses;
using Evolution.DbRepository.Models.SqlDatabaseContext;
using Evolution.Logging.Interfaces;
using Evolution.Master.Domain.Interfaces.Data;
using Evolution.Master.Domain.Interfaces.Services;
using Evolution.Master.Domain.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Transactions;

namespace Evolution.Master.Core
{
    public class SalutationService : MasterService,ISalutation
    {
        private readonly IAppLogger<MasterService> _logger = null;
        private readonly IMasterRepository _repository = null;
        private readonly IMapper _mapper = null;
        private readonly JObject _messages = null;

        public SalutationService(IMapper mapper, IAppLogger<MasterService> logger, IMasterRepository repository,JObject messages) : base(mapper, logger, repository,messages)
        {
            this._mapper = mapper;
            this._repository = repository;
            this._logger = logger;
            this._messages = messages;
        }

        public  Response Search (Salutation search)
        {
            IList<Salutation> result = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                if (search == null)
                    search = new Salutation();

                var masterData = _mapper.Map<Salutation, MasterData>(search);
                //If condition added on 24-Sep2020 to find how complied queries perform
                using (var tranScope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))                
                {
                    if (search.Code.HasEvoWildCardChar() || search.Name.HasEvoWildCardChar())
                        result = this._repository.FindBy(_mapper.Map<MasterData, Data>(masterData).ToExpression())
                                                .Where(x => x.MasterDataTypeId == Convert.ToInt32(MasterType.Salutation) && (search.IsActive == null || x.IsActive == search.IsActive))
                                                .Select(x => _mapper.Map<MasterData, Salutation>(_mapper.Map<Data, MasterData>(x))).OrderBy(x => x.Name).ToList();
                    else
                    {
                        masterData.MasterDataTypeId = Convert.ToInt32(MasterType.Salutation);
                        result = _repository.GetMasterData(masterData).Select(x => _mapper.Map<MasterData, Salutation>(x)).OrderBy(x => x.Name).ToList();
                    }
                    tranScope.Complete();
                }
            }
            catch (SqlException sqlE)
            {
                responseType = ResponseType.DbException;
                this._logger.LogError(responseType.ToId(), this._messages[responseType.ToId()].ToString(), sqlE);
            }
            catch (Exception ex)
            {
                responseType = ResponseType.Exception;
                this._logger.LogError(responseType.ToId(), this._messages[responseType.ToId()].ToString(), ex);
            }
            return new Response().ToPopulate(responseType, result,result?.Count);

        }
    }
}
