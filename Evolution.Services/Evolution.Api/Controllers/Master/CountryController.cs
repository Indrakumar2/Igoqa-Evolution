using AutoMapper;
using Evolution.Common.Models.Responses;
using Evolution.Master.Domain.Interfaces.Services;
using Evolution.Master.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Evolution.Logging.Interfaces;
using System;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;

namespace Evolution.Api.Controllers.Master
{
    [Route("api/master/countries")]
    public class CountryController : Controller
    {
        private readonly ICountryService _service = null;
        private readonly IAppLogger<CountryController> _logger = null; 

        public CountryController (ICountryService service,IAppLogger<CountryController> logger)
        {
            this._service = service;
            this._logger = logger;
        }

        //get
        [HttpGet]
        public Response Get([FromQuery]Country search)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try{
                return this._service.Search(search);
            }
            catch(Exception ex){
                exception = ex;
                responseType = ResponseType.Exception;
                Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), search);
            }
            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }


        [HttpPost]
        public Response Post([FromBody]IList<Country> datas)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try{
                return this._service.Save(datas);
            }
            catch(Exception ex){
                exception = ex;
                responseType = ResponseType.Exception;
                Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), datas);
            }
            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }

        [HttpPut]
        public Response Put([FromBody]IList<Country> datas)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try{
                return this._service.Modify(datas);
            }
            catch(Exception ex){
                exception = ex;
                responseType = ResponseType.Exception;
                Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), datas);
            }
            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }


        [HttpDelete]
        public Response Delete([FromBody]IList<Country> datas)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try{
                return this._service.Delete(datas);
            }
            catch(Exception ex){
                exception = ex;
                responseType = ResponseType.Exception;
                Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), datas);
            }
            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }

    }
}
