using Evolution.Admin.Domain.Interfaces.Admins;
using Evolution.Admin.Domain.Interfaces.Data;
using Evolution.Admin.Domain.Models.Admins;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Common.Models.Responses;
using Evolution.Logging.Interfaces;
using System;
using System.Collections.Generic;
using System.Transactions;

namespace Evolution.Admin.Core.Services
{
    public class AnnouncementService : IAnnouncementService
    {
        private readonly IAnnouncementRepository _repository = null;
        private readonly IAppLogger<AnnouncementService> _logger = null;
        //private readonly IMemoryCache _memoryCache = null;
        //private readonly AppEnvVariableBaseModel _environment = null;

        public AnnouncementService(IAnnouncementRepository repository, IAppLogger<AnnouncementService> logger
            //,IMemoryCache memoryCache, 
            //IOptions<AppEnvVariableBaseModel> environment
            )
        {
            _repository = repository;
            _logger = logger;
            //_memoryCache = memoryCache;
            //_environment = environment.Value;
        }
        public Response GetAnnouncement(Announcement searchModel)
        {
            Exception exception = null;
            IList<Announcement> result = null;
            try
            {
                //var cacheKey = "Announcement";
                //if (!_memoryCache.TryGetValue(cacheKey, out result))
                //{
                    using (var tranScope = new TransactionScope(TransactionScopeOption.RequiresNew,
                                                   new TransactionOptions
                                                   {
                                                       IsolationLevel = IsolationLevel.ReadUncommitted
                                                   }))
                    {
                        result = _repository.Search(searchModel);
                       // memoryCache.Set(cacheKey, result, ObjectExtension.CacheExpiry(_environment.AbsoluteExpiration, environment.SlidingExpiration));
                        tranScope.Complete();
                    }
                //}
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), searchModel);
            }
            finally
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception, result?.Count);
        }
    }
}