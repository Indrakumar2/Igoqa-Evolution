using Evolution.Api.Controllers.Base;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Common.Models.Responses;
using Evolution.Logging.Interfaces;
using Evolution.ResourceSearch.Domain.Interfaces.ResourceSearch;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using DomainModel = Evolution.ResourceSearch.Domain.Models.ResourceSearch;

namespace Evolution.Api.Controllers.ResourceSearch
{
    [Route("api/Resources")]
    public class ResourceSearchController : BaseController
    {
        private readonly IResourceSearchService _resourceSearchService = null;
        private readonly IResourceTechSpecSearchService _resourceTechSpecSearchService = null;
        private readonly IResourceSearchNoteService _resourceSearchNoteService = null;
        private readonly IAppLogger<ResourceSearchController> _logger = null;

        public ResourceSearchController(IResourceSearchService resourceSearchService,
                                        IResourceTechSpecSearchService resourceService,
                                        IResourceSearchNoteService resourceSearchNoteService,
                                        IAppLogger<ResourceSearchController> logger)
        {
            _logger = logger;
            _resourceSearchService = resourceSearchService;
            _resourceTechSpecSearchService = resourceService;
            _resourceSearchNoteService = resourceSearchNoteService;
        }

        [HttpPost]
        [Route("Search")]
        public Response Search([FromBody] DomainModel.ResourceSearch searchModel)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                return this._resourceTechSpecSearchService.Get(searchModel);
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                //Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), new { searchModel });
            }
            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }

        [HttpPost]
        [Route("GeoLocation")]
        public Response Get([FromBody] IList<DomainModel.ResourceTechSpecSearchResult> resourceResultInfo)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                return this._resourceTechSpecSearchService.GetGeoLocationInfo(resourceResultInfo);
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                //Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), new { resourceResultInfo });
            }
            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }

        [HttpGet]
        [Route("assignment/info")]
        public Response Get([FromQuery]int assignmentId)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                return _resourceSearchService.GetARSSearchAssignmentDetail(assignmentId);
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                //Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), new { assignmentId });
            }
            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }

        [HttpGet]
        public Response Get([FromQuery] DomainModel.BaseResourceSearch searchModel)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                return _resourceSearchService.Get(searchModel);
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                //Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), new { searchModel });
            }
            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }

        [HttpGet]
        [Route("Notes")]
        public Response Get([FromQuery] DomainModel.ResourceSearchNote searchModel)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                return _resourceSearchNoteService.Get(searchModel);
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                //Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), new { searchModel });
            }
            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }

        [HttpPost]
        public Response Post([FromBody] DomainModel.ResourceSearch resourceSearchModel)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                AssignValuesFromToken(ref resourceSearchModel);
                return _resourceSearchService.Save(resourceSearchModel);
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                //Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), new { resourceSearchModel });
            }
            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }

        [HttpPut]
        public Response Put([FromBody] DomainModel.ResourceSearch resourceSearchModel)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                AssignValuesFromToken(ref resourceSearchModel);
                return _resourceSearchService.Modify(resourceSearchModel);
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                //Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), new { resourceSearchModel });
            }
            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }

        [HttpDelete]
        public Response Delete([FromBody] int id)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                return _resourceSearchService.Delete(id);
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                //Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), new { id });
            }
            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }

        private void AssignValuesFromToken(ref DomainModel.ResourceSearch resourceSearchModel)
        {
            if (resourceSearchModel != null)
            {
                resourceSearchModel.ActionByUser = UserName;
                resourceSearchModel.UserTypes = UserType?.Split(',');
                resourceSearchModel.CreatedBy = UserName;
                resourceSearchModel.AssignedBy = UserName;
                resourceSearchModel.CreatedOn = DateTime.UtcNow;
                resourceSearchModel.ModifiedBy = UserName;
                resourceSearchModel.LastModification = DateTime.UtcNow;

            }

        }
    }
}
