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

namespace Evolution.Master.Core
{
    public class CostOfSaleService : MasterService,ICostOfSaleService
    {
        private readonly IAppLogger<MasterService> _logger = null;
        private readonly IMasterRepository _repository = null;
        private readonly IMapper _mapper = null;
        private readonly JObject _messages = null;

        public CostOfSaleService(IMapper mapper, IAppLogger<MasterService> logger, IMasterRepository masterRepository,JObject messages) : base(mapper, logger, masterRepository,messages)
        {
            this._logger = logger;
            _repository = masterRepository;
            this._mapper = mapper;
            this._messages =messages;
        }

        public Response Search(CostOfSale search)
        {
            IList<CostOfSale> result = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                if (search == null)
                    search = new CostOfSale();

                var masterData = _mapper.Map<CostOfSale, MasterData>(search);
                result = this._repository.FindBy(_mapper.Map<MasterData, Data>(masterData).ToExpression())
                                            .Where(x => x.MasterDataTypeId == Convert.ToInt32(MasterType.ExpenseType))
                                            .Select(x => _mapper.Map<MasterData, CostOfSale>(_mapper.Map<Data, MasterData>(x))).OrderBy(x=>x.Name).ToList();
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
