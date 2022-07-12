using System;
using System.Collections.Generic;
using Evolution.Admin.Api.Controllers.Base;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Common.Models.Responses;
using Evolution.Logging.Interfaces;
using Evolution.Master.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using domModel = Evolution.Master.Domain.Models;

namespace Evolution.Admin.Api.Controllers.Master
{
    [Route("api/master/{masterType}/data")]
    public class MasterController : BaseController
    {
        private readonly IAppLogger<MasterController> _logger;
        private readonly IMasterService _masterService;

        public MasterController(IMasterService masterService, IAppLogger<MasterController> logger)
        {
            _masterService = masterService;
            _logger = logger;
        }

        [HttpGet]
        public Response Get([FromRoute] string masterType, [FromQuery] domModel.MasterData search)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                search.MasterType = masterType;
                return _masterService.SearchMasterData(search);
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                //Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), search);
            }

            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }

        [HttpPost]
        [Route("systemSetting")]
        public Response Get([FromBody] List<string> keyValues)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                return _masterService.GetCommonSystemSetting(keyValues);
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                //Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), keyValues);
            }
            finally
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }

            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }

        // PUT api/values
        [HttpPut]
        public Response Put([FromBody] IList<domModel.MasterData> datas)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                return _masterService.MasterSave(datas);
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                //Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), datas);
            }

            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }
    }
}