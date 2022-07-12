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
    public class ProfileActionService : MasterService, IProfileActionService
    {
        private readonly IAppLogger<MasterService> _logger = null;
        private readonly IMasterRepository _repository = null;
        private readonly IMapper _mapper = null;
        private readonly JObject _messages = null;

        public ProfileActionService(IAppLogger<MasterService> logger, IMasterRepository repository, IMapper mapper,JObject messages) : base(mapper, logger, repository,messages)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
            _messages = messages;
        }

        public Response Search(ProfileAction search)
        {
            IList<ProfileAction> result = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                if (search == null)
                    search = new ProfileAction();

                var masterData = _mapper.Map<ProfileAction, MasterData>(search);
                using (var tranScope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))                
                {
                    result = this._repository.FindBy(_mapper.Map<MasterData, Data>(masterData).ToExpression())
                                                .Where(x => x.MasterDataTypeId == Convert.ToInt32(MasterType.ProfileAction) && (search.IsActive == null || x.IsActive == search.IsActive))
                                                .Select(x => _mapper.Map<MasterData, ProfileAction>(_mapper.Map<Data, MasterData>(x))).OrderBy(x => x.Name).ToList();
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

        public bool IsValidProfileActionName(IList<string> names, ref IList<DbModel.Data> dbActions, ref IList<ValidationMessage> valdMessages)
        {
            bool? result = false;
            var messages = new List<ValidationMessage>();
            if (names?.Count() > 0)
            {
                result = _repository?.IsRecordValidByName(MasterType.ProfileAction, names, ref dbActions);
                IList<DbModel.Data> dbDatas = dbActions;
                var subDivisionNotExists = names?.Where(x => !dbDatas.Any(x2 => x2.Name == x))?.ToList();
                subDivisionNotExists?.ForEach(x =>
                {
                    messages.Add(_messages, x, MessageType.InvalidProfileAction, x);
                });
                valdMessages = messages;
            }
            return (bool)result;
        }
    }
}
