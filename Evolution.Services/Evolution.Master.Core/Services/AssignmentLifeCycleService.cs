using AutoMapper;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Common.Models.Base;
using Evolution.Common.Models.Responses;
using Evolution.Logging.Interfaces;
using Evolution.Master.Domain.Interfaces.Data;
using Evolution.Master.Domain.Interfaces.Services;
using Evolution.Master.Domain.Models;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Transactions;

namespace Evolution.Master.Core
{
    public class AssignmentLifeCycleService : MasterService, IAssignmentLifeCycleService
    {
        private readonly IAppLogger<MasterService> _logger = null;
        private readonly IMasterRepository _repository = null;
        private readonly IMapper _mapper = null;
        private readonly JObject _messages = null;
        private readonly IMemoryCache _memoryCache = null;
        private readonly AppEnvVariableBaseModel _environment = null;

        public AssignmentLifeCycleService(IMapper mapper,
                                            IAppLogger<MasterService> logger,
                                            IMasterRepository masterRepository, 
                                            JObject messages, IMemoryCache memoryCache,
                                            IOptions<AppEnvVariableBaseModel> environment) : base(mapper, logger, masterRepository, messages)
        {
            this._logger = logger;
            this._repository = masterRepository;
            this._mapper = mapper;
            this._messages = messages;
            _memoryCache = memoryCache;
            _environment = environment.Value;
        }

        public Response Search(MasterData search)
        {
            IList<MasterData> result = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                if (search == null)
                    search = new MasterData();

                //var cacheKey = "AssignmentLifeCycle";
                //if (!_memoryCache.TryGetValue(cacheKey, out result))
                //{
                    using (var tranScope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                    {
                        search.MasterDataTypeId = Convert.ToInt32(MasterType.AssignmentLifeCycle);
                        result = _repository.GetMasterData(search);
                        //_memoryCache.Set(cacheKey, result, ObjectExtension.CacheExpiry(_environment.AbsoluteExpiration, _environment.SlidingExpiration));
                        tranScope.Complete();
                    }
                //}
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