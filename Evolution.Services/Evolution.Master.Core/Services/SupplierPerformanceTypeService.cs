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

namespace Evolution.Master.Core.Services
{
    class SupplierPerformanceTypeService : MasterService, ISupplierPerformanceTypeService
    {
        private readonly IAppLogger<MasterService> _logger = null;
        private readonly IMapper _mapper = null;
        private readonly IMasterRepository _repository = null;
        private readonly JObject _messages = null;

        public SupplierPerformanceTypeService(IMapper mapper, IAppLogger<MasterService> logger, IMasterRepository repository, JObject messages) : base(mapper, logger, repository, messages)
        {
            this._logger = logger;
            this._mapper = mapper;
            this._repository = repository;
            this._messages = messages;
        }

        public Response Search(SupplierPerformanceType search)
        {
            IList<SupplierPerformanceType> result = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                if (search == null)
                    search = new SupplierPerformanceType();

                var masterData = _mapper.Map<SupplierPerformanceType, MasterData>(search);
                using (var tranScope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))                
                {
                    result = this._repository.FindBy(_mapper.Map<MasterData, Data>(masterData).ToExpression())
                                                .Where(x => x.MasterDataTypeId == Convert.ToInt32(MasterType.SupplierPerformanceType))
                                                .Select(x => _mapper.Map<MasterData, SupplierPerformanceType>(_mapper.Map<Data, MasterData>(x))).OrderBy(x => x.Name).ToList();
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
            return new Response().ToPopulate(responseType, result, result?.Count);
        }
    }
}
