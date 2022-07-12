using AutoMapper;
using Evolution.Common.Enums;
using Evolution.Common.Models.Responses;
using Evolution.Logging.Interfaces;
using Evolution.Master.Domain.Interfaces.Data;
using Evolution.Master.Domain.Interfaces.Services;
using Evolution.Master.Domain.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using Dbmodel=Evolution.DbRepository.Models.SqlDatabaseContext;
using Evolution.Common.Extensions;
using Evolution.Common.Models.ExchangeRate;
using System.Linq;

namespace Evolution.Master.Core
{
    public class CurrencyExchangeRateService : MasterService, ICurrencyExchangeRateService
    {
        private readonly IMasterRepository _masterRepository = null;
        private readonly IAppLogger<MasterService> _logger = null;
        private readonly ICurrencyExchangeRateRepository _repository = null;
        private readonly IMapper _mapper = null;
        private readonly JObject _messages = null;

        public CurrencyExchangeRateService(IMasterRepository masterRepository, IAppLogger<MasterService> logger,
                                            ICurrencyExchangeRateRepository repository, IMapper mapper,JObject messages) : base(mapper, logger, masterRepository,messages)
        {
            _masterRepository = masterRepository;
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
            _messages = messages;
        }

        public Response Search(Domain.Models.CurrencyExchangeRates search)
        {
            IList<CurrencyExchangeRates> result = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                if (search == null)
                    search = new Domain.Models.CurrencyExchangeRates();

                result = this._repository.Search(search);


            }
            catch (Exception ex)
            {
                responseType = ResponseType.Exception;
                this._logger.LogError(responseType.ToId(), this._messages[responseType.ToId()].ToString(), ex);
            }
            return new Response().ToPopulate(responseType, result, result?.Count);
        }

        public Response GetExchangeRates(IList<ExchangeRate> models,IList<Dbmodel.Data> dbCurrency=null)
        {
            List<ExchangeRate> result = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                var currenciesValueNeedToFetch = models?.Where(x => x.CurrencyFrom != x.CurrencyTo).ToList();
                if (currenciesValueNeedToFetch != null && currenciesValueNeedToFetch?.Count > 0)
                    result = _repository.GetExchangeRates(currenciesValueNeedToFetch, dbCurrency).ToList();

                var sameCurrencies = models?.Where(x => x.CurrencyFrom == x.CurrencyTo).ToList();

                if (sameCurrencies != null && sameCurrencies.Count > 0)
                   sameCurrencies.ForEach(x1 => { x1.Rate = 1; });

                if (result == null)
                    result = new List<ExchangeRate>();

                if (sameCurrencies?.Count > 0)
                    result.AddRange(sameCurrencies);
            }
            catch (Exception ex)
            {
                responseType = ResponseType.Exception;
                this._logger.LogError(responseType.ToId(), this._messages[responseType.ToId()].ToString(), ex);
            }
            return new Response().ToPopulate(responseType, result, result?.Count);
        }

        public Response GetCurrencyExchangeRates(IList<SearchCurrencyExchangeRate> models)
        {
            IList<CurrencyExchangeRates> result = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                result = _repository.GetCurrencyExchangeRates(models);
            }
            catch (Exception ex)
            {
                responseType = ResponseType.Exception;
                this._logger.LogError(responseType.ToId(), this._messages[responseType.ToId()].ToString(), ex);
            }
            return new Response().ToPopulate(responseType, result, result?.Count);
        }
        
        public Response GetMiiwaExchangeRates(IList<ExchangeRate> models)
        {
            List<ExchangeRate> result = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                var currenciesValueNeedToFetch = models?.Where(x => x.CurrencyFrom != x.CurrencyTo).ToList();
                if (currenciesValueNeedToFetch != null && currenciesValueNeedToFetch?.Count > 0)
                    result = _repository.GetMiiwaExchangeRates(currenciesValueNeedToFetch).ToList();

                var sameCurrencies = models?.Where(x => x.CurrencyFrom == x.CurrencyTo).ToList();
                sameCurrencies.ForEach(x1 => { x1.Rate = 1; });

                if (result == null)
                    result = new List<ExchangeRate>();

                if (sameCurrencies?.Count > 0)
                    result.AddRange(sameCurrencies);
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
