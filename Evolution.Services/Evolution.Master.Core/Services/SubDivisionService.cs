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
    public class SubDivisionService : MasterService, ISubDivisionService
    {
        private readonly IAppLogger<MasterService> _logger = null;
        private readonly IMasterRepository _repository = null;
        private readonly IMapper _mapper = null;
        private readonly JObject _messages = null;

        public SubDivisionService(IAppLogger<MasterService> logger,
                                    IMasterRepository repository,
                                    IMapper mapper,
                                    JObject messages) : base(mapper, logger, repository, messages)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
            _messages = messages;
        }

        public bool IsValidSubDivisionName(IList<string> names,
                                           ref IList<DbModel.Data> dbSubDivisions,
                                           ref IList<ValidationMessage> messages)
        {
            bool? result = false;
            IList<ValidationMessage> valdMessage = new List<ValidationMessage>();
            if (names?.Count() > 0)
            {
                result = _repository?.IsRecordValidByName(MasterType.SubDivision, names, ref dbSubDivisions);
                IList<DbModel.Data> dbDatas = dbSubDivisions;
                var subDivisionNotExists = names?.Where(x => !dbDatas.Any(x2 => x2.Name == x))?.ToList();
                subDivisionNotExists?.ForEach(x =>
                {
                    valdMessage.Add(_messages, x, MessageType.InvalidSubDivision, x);
                });
                messages = valdMessage;
            }
            return (bool)result;
        }

        public Response Search(SubDivision search)
        {
            IList<SubDivision> result = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                if (search == null)
                    search = new SubDivision();

                var masterData = _mapper.Map<SubDivision, MasterData>(search);
                using (var tranScope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))                
                {
                    result = this._repository.FindBy(_mapper.Map<MasterData, Data>(masterData).ToExpression())
                                                .Where(x => x.MasterDataTypeId == Convert.ToInt32(MasterType.SubDivision) && (search.IsActive == null || x.IsActive == search.IsActive))
                                                .Select(x => _mapper.Map<MasterData, SubDivision>(_mapper.Map<Data, MasterData>(x))).OrderBy(x => x.Name).ToList();
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
    }
}
