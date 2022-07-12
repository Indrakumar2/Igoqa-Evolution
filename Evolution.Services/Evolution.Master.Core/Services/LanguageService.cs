using AutoMapper;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Common.Models.Responses;
using Evolution.DbRepository.Models.SqlDatabaseContext;
using Evolution.Logging.Interfaces;
using Evolution.Master.Domain.Interfaces.Data;
using Evolution.Master.Domain.Interfaces.Services;
using Evolution.Master.Domain.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using System.Reflection;
using Evolution.Common.Models.Messages;

namespace Evolution.Master.Core.Services
{
    public class LanguageService : MasterService, ILanguageService
    {
        private readonly IAppLogger<MasterService> _logger = null;
        private readonly IMasterRepository _repository = null;
        private readonly IMapper _mapper = null;
        private readonly JObject _messages = null;

        public LanguageService(IAppLogger<MasterService> logger, IMasterRepository repository, IMapper mapper, JObject messages) : base(mapper, logger, repository, messages)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
            _messages = messages;
        }

        public Response Search(Language search)
        {
            IList<Language> result = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                if (search == null)
                    search = new Language();

                var masterData = _mapper.Map<Language, MasterData>(search);
                //If condition added on 24-Sep2020 to find how complied queries perform
                if (search.Code.HasEvoWildCardChar() || search.Name.HasEvoWildCardChar())
                    result = this._repository.FindBy(_mapper.Map<MasterData, Data>(masterData).ToExpression())
                                            .Where(x => x.MasterDataTypeId == Convert.ToInt32(MasterType.Language) && (search.IsActive == null || x.IsActive == search.IsActive))
                                            .Select(x => _mapper.Map<MasterData, Language>(_mapper.Map<Data, MasterData>(x))).OrderBy(x => x.Name).ToList();
                else
                {
                    masterData.MasterDataTypeId = Convert.ToInt32(MasterType.Language);
                    result = _repository.GetMasterData(masterData).Select(x => _mapper.Map<MasterData, Language>(x)).OrderBy(x => x.Name).ToList();
                }
            }
            catch (Exception ex)
            {
                responseType = ResponseType.Exception;
                this._logger.LogError(responseType.ToId(), this._messages[responseType.ToId()].ToString(), ex);
            }
            return new Response().ToPopulate(responseType, result, result?.Count);
        }
        public bool IsValidLanguage(IList<string> language, ref IList<DbModel.Data> dbLanguages, ref IList<ValidationMessage> messages)
        {
            IList<ValidationMessage> valdMessage = new List<ValidationMessage>();
            if (language?.Count() > 0)
            {
                if (dbLanguages == null || dbLanguages?.Count <= 0)
                    dbLanguages = _repository?.FindBy(x => language.Contains(x.Name))?.ToList();

                var dbLanguage = dbLanguages;
                var languageNotExists = language?.Where(x => !dbLanguage.Any(x2 => x2.Name == x))?.ToList();
                languageNotExists?.ForEach(x =>
                {
                    string errorCode = MessageType.InvalidLanguageCode.ToId();
                    valdMessage.Add(_messages, x, MessageType.InvalidLanguageCode, x);
                });

                messages = valdMessage;
            }

            return messages?.Count <= 0;
        }
    }
}
