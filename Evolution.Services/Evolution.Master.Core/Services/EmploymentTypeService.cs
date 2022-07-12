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

namespace Evolution.Master.Core
{
    public class EmploymentTypeService : MasterService, IEmploymentTypeService
    {
        private readonly IAppLogger<MasterService> _logger = null;
        private readonly IMasterRepository _repository = null;
        private readonly IMapper _mapper = null;
        private readonly JObject _messages = null;

        public EmploymentTypeService(IAppLogger<MasterService> logger, IMasterRepository repository, IMapper mapper,JObject messages) : base(mapper, logger, repository,messages)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
            _messages = messages;
        }

        public Response Search(EmploymentType search)
        {

            IList<EmploymentType> result = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                if (search == null)
                    search = new EmploymentType();

                var masterData = _mapper.Map<EmploymentType, MasterData>(search);
                result = this._repository.FindBy(_mapper.Map<MasterData, Data>(masterData).ToExpression())
                                            .Where(x => x.MasterDataTypeId == Convert.ToInt32(MasterType.EmploymentType) && (search.IsActive == null || x.IsActive == search.IsActive))
                                            .Select(x => _mapper.Map<MasterData, EmploymentType>(_mapper.Map<Data, MasterData>(x))).OrderBy(x => x.Name).ToList();
            }

            catch (Exception ex)
            {
                responseType = ResponseType.Exception;
                this._logger.LogError(responseType.ToId(), this._messages[responseType.ToId()].ToString(), ex);
            }
            return new Response().ToPopulate(responseType, result, result?.Count);
        }

        public bool IsValidEmploymentName(IList<string> names, ref IList<DbModel.Data> dbEmploymentTypes, ref IList<ValidationMessage> valdMessages)
        {
            bool? result = true;
            var messages = new List<ValidationMessage>();
            var recordToValidate = names.Where(x => !string.IsNullOrEmpty(x)).Distinct().Select(x => x).ToList();
            if (recordToValidate?.Count() > 0)
            {
                result = _repository?.IsRecordValidByName(MasterType.EmploymentType, recordToValidate, ref dbEmploymentTypes);
                IList<DbModel.Data> dbDatas = dbEmploymentTypes;
                var subDivisionNotExists = recordToValidate?.Where(x => !dbDatas.Any(x2 => x2.Name == x))?.ToList();
                subDivisionNotExists?.ForEach(x =>
                {
                    messages.Add(_messages, x, MessageType.InvalidEmployementType, x);
                });
                valdMessages = messages;
                return (bool)result;
            }
            else
            {
                return (bool)result;
            }
          
        }
    }
}
