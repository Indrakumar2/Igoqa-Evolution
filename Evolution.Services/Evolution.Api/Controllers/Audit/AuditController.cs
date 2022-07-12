using Evolution.AuditLog.Domain.Interfaces.Audit;
using Evolution.AuditLog.Domain.Models.Audit;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Common.Models.Responses;
using Evolution.Logging.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Evolution.Api.Controllers.Audit
{
    [Route("api/audit")]
    public class AuditController : ControllerBase
    {
        private readonly ISqlAuditLogEventInfoService _sqlAuditLogEventInfoService;

        private readonly IAuditSearchService _auditSearchService;
        private readonly IAppLogger<AuditController> _logger;

        public AuditController(ISqlAuditLogEventInfoService sqlAuditLogEventInfoService, IAuditSearchService auditSearchService, IAppLogger<AuditController> logger)
        {
            this._sqlAuditLogEventInfoService = sqlAuditLogEventInfoService;
            this._auditSearchService = auditSearchService;
            _logger = logger;
        }

        [HttpPost]
        public Response Get([FromBody] SqlAuditLogEventSearchInfo sqlAuditLogEventSearchInfo)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                return this._sqlAuditLogEventInfoService.Get(sqlAuditLogEventSearchInfo);
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                //Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), sqlAuditLogEventSearchInfo);
            }

            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }

        [HttpPost]
        [Route("Search")]
        public Response GetAll([FromBody] SqlAuditLogEventSearchInfo sqlAuditLogEventSearchInfo)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                return this._sqlAuditLogEventInfoService.GetAll(sqlAuditLogEventSearchInfo);
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                //Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), sqlAuditLogEventSearchInfo);
            }

            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }

        [HttpGet]
        [Route("Module")]
        public Response Get(string module)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                return this._auditSearchService.GetModuleAndSearchType(module);
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                //Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), module);
            }

            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }

        [HttpPost]
        [Route("AuditEvent")]
        public Response GetAuditEvent([FromBody] SqlAuditLogEventSearchInfo sqlAuditLogEventSearchInfo)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                return this._sqlAuditLogEventInfoService.GetAuditEvent(sqlAuditLogEventSearchInfo);
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                //Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), sqlAuditLogEventSearchInfo);
            }

            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }

        [HttpPost]
        [Route("AuditLog")]
        public Response GetAuditLog([FromBody] SqlAuditLogEventSearchInfo sqlAuditLogEventSearchInfo)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                return this._sqlAuditLogEventInfoService.GetAuditLog(sqlAuditLogEventSearchInfo);
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                //Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), sqlAuditLogEventSearchInfo);
            }

            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }
    }
}
