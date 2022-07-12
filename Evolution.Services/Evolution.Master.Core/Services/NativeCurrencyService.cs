using AutoMapper;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Common.Models.Messages;
using Evolution.Common.Models.Responses;
using Evolution.DbRepository.Models.SqlDatabaseContext;
using Evolution.Logging.Interfaces;
using Evolution.Master.Domain.Interfaces.Data;
using Evolution.Master.Domain.Interfaces.Services;
using Evolution.Master.Domain.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;

namespace Evolution.Master.Core
{
    public class NativeCurrencyService : MasterService, INativeCurrencyService
    {
        private readonly IAppLogger<MasterService> _logger = null;
        private readonly IMasterRepository _repository = null;
        private readonly IMapper _mapper = null;
        private readonly JObject _messages = null;
        public NativeCurrencyService(IMapper mapper, IAppLogger<MasterService> logger, IMasterRepository MasterRepository,JObject messages) : base(mapper, logger, MasterRepository,messages)
        {
            this._logger = logger;
            this._repository = MasterRepository;
            this._mapper = mapper;
            this._messages = messages;
        }

        public Response Search(Currency search)
        {
            IList<Currency> result = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                if (search == null)
                    search = new Currency();

                var masterData = _mapper.Map<Currency, MasterData>(search);
                //If condition added on 24-Sep2020 to find how complied queries perform
                using (var tranScope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))                
                {
                    if (search.Code.HasEvoWildCardChar() || search.Name.HasEvoWildCardChar())
                        result = this._repository.FindBy(_mapper.Map<MasterData, Data>(masterData).ToExpression())
                                            .Where(x => x.MasterDataTypeId == Convert.ToInt32(MasterType.Currency))
                                            .Select(x => _mapper.Map<MasterData, Currency>(_mapper.Map<Data, MasterData>(x))).OrderBy(x => x.Name).ToList();
                    else
                    {
                        masterData.MasterDataTypeId = Convert.ToInt32(MasterType.Currency);
                        result = _repository.GetMasterData(masterData).Select(x => _mapper.Map<MasterData, Currency>(x)).OrderBy(x => x.Name).ToList();
                    }
                    tranScope.Complete();                
                }
            }
            catch (Exception ex)
            {
                responseType = ResponseType.Exception;
                this._logger.LogError(responseType.ToId(), this._messages[responseType.ToId()].ToString(), ex);
            }
            return new Response().ToPopulate(responseType, result, result?.Count);
        }

        public bool IsValidCurrency(IList<string> currencyCodes,
                                          ref IList<DbModel.Data> dbCurrencies,
                                          ref IList<ValidationMessage> messages)
        {
            bool? result = false;
            IList<ValidationMessage> valdMessage = new List<ValidationMessage>();
            if (currencyCodes?.Count() > 0)
            {
                result = _repository?.IsRecordValidByCode(MasterType.Currency, currencyCodes, ref dbCurrencies);
                IList<DbModel.Data> dbDatas = dbCurrencies;
                var currencyNotExists = currencyCodes?.Where(x => !dbDatas.Any(x2 => x2.Code == x))?.ToList();
                currencyNotExists?.ForEach(x =>
                {
                    valdMessage.Add(_messages, x, MessageType.MasterInvalidCurrency, x);
                });
                messages = valdMessage;
            }
            return (bool)result;
        }
    }
}

