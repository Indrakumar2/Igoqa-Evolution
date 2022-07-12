using Evolution.Admin.Domain.Interfaces.Admins;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Common.Models.Responses;
using Evolution.Logging.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using DomainModels = Evolution.Admin.Domain.Models.Admins;

namespace Evolution.Api.Controllers.Admin
{
    [Route("api/admin/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private IUserService _service = null;
        private readonly IAppLogger<UserController> _logger = null;

        public UserController(IUserService service, IAppLogger<UserController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet]
        public Response Get([FromQuery]DomainModels.User searchModel)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                return _service.GeUser(searchModel);
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), searchModel);
            }

            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }

        [HttpPost("micoordinators")]
        public Response Get([FromBody]IList<string> contractHoldingCompanyCodes)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                return _service.GetMICoordinators(contractHoldingCompanyCodes);
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), contractHoldingCompanyCodes);
            }

            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }

        [HttpPost("micoordinators/status")]
        public Response Get([FromBody]IList<int> contractHoldingCompanyIds)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                return _service.GetMICoordinators(null,contractHoldingCompanyIds);
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), contractHoldingCompanyIds);
            }

            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }

        [HttpPost]
        [Route("coordinators")]
        public Response GetCoordinators([FromBody]CoordinatorParams coordinatorParams)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                return this._service.GetByUserType(coordinatorParams.CompanyCode, coordinatorParams.UserTypes, coordinatorParams.IsActiveCoordinators);
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), coordinatorParams);
            }

            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }

        [HttpPost]
        [Route("reportCoordinators")]
        public Response ReportCoordinators([FromBody]CoordinatorParams coordinatorParams)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                return this._service.GetByUserType(coordinatorParams.CompanyCodes, coordinatorParams.UserTypes, coordinatorParams.IsActiveCoordinators);
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), coordinatorParams);
            }

            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }

        [HttpPost("ssrsCoordinators")]
        public Response GetReportCoordniators([FromBody]ReportCoordinatorParams reportCoordinatorParams)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                return _service.GetReportCoordniators(reportCoordinatorParams.CompanyCode, reportCoordinatorParams.IsVisit, reportCoordinatorParams.IsOperating);
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), reportCoordinatorParams.CompanyCode);
            }

            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }

        public class CoordinatorParams
        {
            public string CompanyCode { get; set; }
            public IList<string> UserTypes { get; set; }
            public bool IsActiveCoordinators { get; set; }
            public IList<string> CompanyCodes { get; set; }
        }

        public class ReportCoordinatorParams
        {
            public string CompanyCode { get; set; }
            public bool IsVisit { get; set; }
            public bool IsOperating { get; set; }
        }
    }
}