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
using System.Data.SqlClient;
using System.Linq;
using System.Transactions;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;

namespace Evolution.Master.Core.Services
{
    public class TechnicalSpecialistStampCountryCodeService : MasterService, ITechnicalSpecialistStampCountryCodeService
    {
        private readonly IAppLogger<MasterService> _logger = null;
        private readonly IMasterRepository _repository = null;
        private readonly IMapper _mapper = null;
        private readonly JObject _messages = null;

        public TechnicalSpecialistStampCountryCodeService(IMapper mapper, IAppLogger<MasterService> logger, IMasterRepository repository, JObject messages) : base(mapper, logger, repository, messages)
        {
            this._mapper = mapper;
            this._repository = repository;
            this._logger = logger;
            this._messages = messages;
        }

        public Response Search(TechnicalSpecialistStampCountryCode search)
        {
            IList<TechnicalSpecialistStampCountryCode> result = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                if (search == null)
                    search = new TechnicalSpecialistStampCountryCode();
                
                var masterData = _mapper.Map<TechnicalSpecialistStampCountryCode, MasterData>(search);
                using (var tranScope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))                
                {
                    result = this._repository.FindBy(_mapper.Map<MasterData, Data>(masterData).ToExpression())
                                                .Where(x => x.MasterDataTypeId == Convert.ToInt32(MasterType.TSStampCountryCode) && (search.IsActive == null || x.IsActive == search.IsActive))
                                                .Select(x => _mapper.Map<MasterData, TechnicalSpecialistStampCountryCode>(_mapper.Map<Data, MasterData>(x))).OrderBy(x => x.Name).ToList();
                    tranScope.Complete();
                }            
            }
            catch (SqlException sqlE)
            {
                responseType = ResponseType.DbException;
                this._logger.LogError(responseType.ToId(), this._messages[responseType.ToId()].ToString(), sqlE);
            }
            catch (Exception ex)
            {
                responseType = ResponseType.Exception;
                this._logger.LogError(responseType.ToId(), this._messages[responseType.ToId()].ToString(), ex);
            }
            return new Response().ToPopulate(responseType, result, result?.Count);

        }

        public bool IsValidTechnicalSpecialistStampCountryCode(IList<string> stampCountries,
                                         ref IList<DbModel.Data> dbTechSpecStampCountries,
                                         ref IList<ValidationMessage> messages)
        {
            bool? result = false;
            IList<ValidationMessage> valdMessage = new List<ValidationMessage>();
            if (stampCountries?.Count() > 0)
            {
                result = _repository?.IsRecordValidByName(MasterType.TSStampCountryCode, stampCountries, ref dbTechSpecStampCountries);
                IList<DbModel.Data> dbDatas = dbTechSpecStampCountries;
                var stampCountriesNotExists = stampCountries?.Where(x => !dbDatas.Any(x2 => x2.Name == x))?.ToList();
                stampCountriesNotExists?.ForEach(x =>
                {
                    valdMessage.Add(_messages, x, MessageType.MasterInvalidCurrency, x);
                });
                messages = valdMessage;
            }
            return (bool)result;
        }
    }
}
