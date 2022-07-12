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
using System.Reflection;
using System.Transactions;

namespace Evolution.Master.Core
{
    public class TaxService : MasterService, ITaxTypeService
    {
        private readonly IAppLogger<MasterService> _logger = null;
        private readonly ITaxTypeRepository _repository = null;
        private readonly IMapper _mapper = null;
        private readonly JObject _messages = null;
        public TaxService(IMapper mapper, IAppLogger<MasterService> logger, ITaxTypeRepository repository, IMasterRepository masterRepository,JObject messages) : base(mapper, logger, masterRepository,messages)
        {
            this._logger = logger;
            this._mapper = mapper;
            this._repository = repository;
           this._messages =messages;
        }

        public Response Search(TaxType search)
        {
            IList<TaxType> result = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                if (search == null)
                    search = new TaxType();

                var taxSearch = _mapper.Map<TaxType, Tax>(search);
                using (var tranScope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))                
                {
                    result = _mapper.Map<IList<Tax>, IList<TaxType>>(this._repository.Search(taxSearch));
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
