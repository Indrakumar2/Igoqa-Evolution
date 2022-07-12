using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Evolution.Common.Models.Responses;
using domModel = Evolution.Master.Domain.Models;
using Evolution.Master.Domain.Interfaces.Services;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Api.Controllers.Base;
using Evolution.Logging.Interfaces;

namespace Evolution.Api.Controllers.Master
{
    [Route("api/master/{masterType}/data")]
    public class MasterController : BaseController
    {
        private readonly IMasterService _masterService = null;
        private readonly IAppLogger<MasterController> _logger = null;

        public MasterController(IMasterService masterService, IAppLogger<MasterController> logger)
        {
            _masterService = masterService;
            this._logger = logger;
        }

        [HttpGet]
        public Response Get([FromRoute]string masterType, [FromQuery]domModel.MasterData search)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                 search.MasterType=masterType;
                 return _masterService.SearchMasterData(search);
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), search);               
            }

            return new Response().ToPopulate(responseType, null, null, null, null, exception); 
          
        }
        [HttpPost]
        [Route("systemSetting")]
        public Response Get([FromBody]List<string> keyValues)
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
                Console.WriteLine(ex.ToFullString());
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
        public Response Put([FromBody]IList<domModel.MasterData> datas)
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
                Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), datas);               
            }

            return new Response().ToPopulate(responseType, null, null, null, null, exception); 
            
        }


        // [HttpPut]
        // public Response Put( [FromBody]IList<domModel.MasterData> datas)
        // {
        //     return _masterService.Modify(datas);
        // }

       
        // [HttpDelete]
        // public Response Delete([FromBody]IList<domModel.MasterData> datas)
        // {
        //     return _masterService.Delete(datas);
        // }
    }
}
