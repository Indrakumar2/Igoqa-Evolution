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
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using System.Transactions;

namespace Evolution.Master.Core.Services
{
    public class TrainingsService : MasterService, ITrainingsService
    {
        private readonly IAppLogger<MasterService> _logger = null;
        private readonly IMasterRepository _repository = null;
        private readonly IMapper _mapper = null;
        private readonly JObject _messages = null;

        public TrainingsService(IAppLogger<MasterService> logger, IMasterRepository repository, IMapper mapper, JObject messages) : base(mapper, logger, repository, messages)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
            _messages = messages;
        }

        public Response Search(Trainings search)
        {
            IList<Trainings> result = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                if (search == null)
                    search = new Trainings();
                var masterData = _mapper.Map<Trainings, MasterData>(search);                        
                using (var tranScope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    result = this._repository.FindBy(_mapper.Map<MasterData, Data>(masterData).ToExpression())
                                            .Where(x => x.MasterDataTypeId == Convert.ToInt32(MasterType.Trainings) && (search.IsActive == null || x.IsActive == search.IsActive))
                                            .Select(x => _mapper.Map<MasterData, Trainings>(_mapper.Map<Data, MasterData>(x))).OrderBy(x => x.Name.ToLower()).ToList(); //DEF 424 Issue 6 fix
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


        public bool IsValidTraining(IList<string> trainingNames,
                                        ref IList<DbModel.Data> dbTrainings,
                                        ref IList<ValidationMessage> messages)
        {
            bool? result = false;
            IList<ValidationMessage> valdMessage = new List<ValidationMessage>();
            if (trainingNames?.Count() > 0)
            {
                result = _repository?.IsRecordValidByName(MasterType.Trainings, trainingNames, ref dbTrainings);
                IList<DbModel.Data> dbDatas = dbTrainings;
                var certificateNotExists = trainingNames?.Where(x => !dbDatas.Any(x2 => x2.Name == x))?.ToList();
                certificateNotExists?.ForEach(x =>
                {
                    valdMessage.Add(_messages, x, MessageType.MasterInvalidTraining, x);
                });
                messages = valdMessage;
            }
            return (bool)result;
        }

    }
}
