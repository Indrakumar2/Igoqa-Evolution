using AutoMapper;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Common.Models.Base;
using Evolution.Common.Models.Responses;
using Evolution.DbRepository.Models.SqlDatabaseContext;
using Evolution.Logging.Interfaces;
using Evolution.Master.Domain.Interfaces.Data;
using Evolution.Master.Domain.Interfaces.Services;
using Evolution.Master.Domain.Models;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;

namespace Evolution.Master.Core.Services
{
    public class UnusedReasonService : MasterService, IUnusedReasonService
    {

        private readonly IAppLogger<MasterService> _logger = null;
        private readonly IMasterRepository _repository = null;
        private readonly IMapper _mapper = null;
        private readonly JObject _messages = null;
        private readonly IMemoryCache _memoryCache = null;
        private readonly AppEnvVariableBaseModel _environment = null;

        public UnusedReasonService(IAppLogger<MasterService> logger, IMasterRepository repository, IMapper mapper,
                                 JObject messages, IMemoryCache memoryCache, IOptions<AppEnvVariableBaseModel> environment) : base(mapper, logger, repository, messages)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
            _messages = messages;
            _memoryCache = memoryCache;
            _environment = environment.Value;
        }

        public Response UnusedReasonSearch(UnusedReason unusedReasons)
        {
            IList<UnusedReason> result = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                if (unusedReasons == null)
                    unusedReasons = new UnusedReason();

                var masterData = _mapper.Map<UnusedReason, MasterData>(unusedReasons);
                var cacheKey = "UnusedReason";
                if (!_memoryCache.TryGetValue(cacheKey, out result))
                {
                    using (var tranScope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                    {
                        result = this._repository.FindBy(_mapper.Map<MasterData, Data>(masterData).ToExpression())
                                            .Where(x => x.MasterDataTypeId == Convert.ToInt32(MasterType.UnusedReasons))
                                            .Select(x => _mapper.Map<MasterData, UnusedReason>(_mapper.Map<Data, MasterData>(x))).OrderBy(x => x.Name).ToList();

                        _memoryCache.Set(cacheKey, result, ObjectExtension.CacheExpiry(_environment.AbsoluteExpiration, _environment.SlidingExpiration));
                        tranScope.Complete();
                    }
                }
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
