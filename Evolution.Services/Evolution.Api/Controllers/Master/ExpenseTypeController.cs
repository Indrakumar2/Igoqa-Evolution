using Evolution.Common.Models.Responses;
using Evolution.Master.Domain.Interfaces.Services;
using Evolution.Master.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Logging.Interfaces;
using System;


namespace Evolution.Api.Controllers.Master
{
    [Route("api/master/PayRateExpenseTypes")]
    public class ExpenseTypeController : ControllerBase
    {
        private readonly IExpenseType _service = null;
        private readonly IAppLogger<ExpenseTypeController> _logger = null;

        public ExpenseTypeController(IExpenseType service, IAppLogger<ExpenseTypeController> logger)
        {
            this._service = service;
            this._logger = logger;
        }

        // GET: api/
        [HttpGet]
        public Response Get(ExpenseType search)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                 return this._service.Search(search);
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
    }
}
