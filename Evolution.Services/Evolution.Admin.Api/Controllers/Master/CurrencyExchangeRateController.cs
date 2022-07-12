using System;
using System.Collections.Generic;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Common.Models.ExchangeRate;
using Evolution.Common.Models.Responses;
using Evolution.Logging.Interfaces;
using Evolution.Master.Domain.Interfaces.Services;
using Evolution.Master.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace Evolution.Admin.Api.Controllers.Master
{
    [Route("api/master/currency/exchangerates")]
    public class CurrencyExchangeRateController : Controller
    {
        private readonly IAppLogger<CurrencyExchangeRateController> _logger;
        private readonly ICurrencyExchangeRateService _service;

        public CurrencyExchangeRateController(ICurrencyExchangeRateService service,
            IAppLogger<CurrencyExchangeRateController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet]
        public Response Get(CurrencyExchangeRates currencyExchangeRates)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                return _service.Search(currencyExchangeRates);
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                //Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), currencyExchangeRates);
            }

            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }

        [HttpGet]
        [Route("GetExchangeRates")]
        public Response GetExchangeRates(ExchangeRate exchangeRates)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                IList<ExchangeRate> rates = new List<ExchangeRate>
                {
                    exchangeRates
                };
                return _service.GetExchangeRates(rates);
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                //Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), exchangeRates);
            }

            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }

        [HttpPost]
        [Route("GetExchangeRates")]
        public Response GetExchangeRates([FromBody] IList<ExchangeRate> exchangeRates)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                return _service.GetExchangeRates(exchangeRates);
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                //Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), exchangeRates);
            }

            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }
    }
}