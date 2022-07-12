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

namespace Evolution.Master.Core.Services
{
    public class CertificationService : MasterService, ICertificationsService
    {
        private readonly IAppLogger<MasterService> _logger = null;
        private readonly IMasterRepository _repository = null;
        private readonly IMapper _mapper = null;
        private readonly JObject _messages = null;

        public CertificationService(IAppLogger<MasterService> logger, IMasterRepository repository, IMapper mapper,JObject messages) : base(mapper, logger, repository,messages)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
            _messages = messages;
           
        }

        public Response Search(Certifications search)
        {
            IList<Certifications> result = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                if (search == null)
                    search = new Certifications();

                var masterData = _mapper.Map<Certifications, MasterData>(search);
                using (var tranScope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    //If condition added on 23-Sep2020 to find how complied queries perform
                    if (search.Name.HasEvoWildCardChar() || search.Code.HasEvoWildCardChar())
                        result = this._repository.FindBy(_mapper.Map<MasterData, Data>(masterData).ToExpression())
                                                .Where(x => x.MasterDataTypeId == Convert.ToInt32(MasterType.Certifications) && (search.IsActive == null || x.IsActive == search.IsActive))
                                                .Select(x => _mapper.Map<MasterData, Certifications>(_mapper.Map<Data, MasterData>(x))).OrderBy(x => x.Name).ToList();
                    else
                    {
                        masterData.MasterDataTypeId = Convert.ToInt32(MasterType.Certifications);
                        result = _repository.GetMasterData(masterData).Select(x => _mapper.Map<MasterData, Certifications>(x)).OrderBy(x => x.Name).ToList();
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

        public bool IsValidCertification(IList<string> certificationNames,
                                         ref IList<DbModel.Data> dbCertifications,
                                         ref IList<ValidationMessage> messages)
        {
            bool? result = false;
            IList<ValidationMessage> valdMessage = new List<ValidationMessage>();
            if (certificationNames?.Count() > 0)
            {
                result = _repository.IsRecordValidByName(MasterType.Certifications, certificationNames, ref dbCertifications);
                IList<DbModel.Data> dbDatas = dbCertifications;
                var certificateNotExists = certificationNames?.Where(x => !dbDatas.Any(x2 => x2.Name == x))?.ToList();
                certificateNotExists?.ForEach(x =>
                {
                    valdMessage.Add(_messages, x, MessageType.MasterInvalidCertification, x);
                });
                messages = valdMessage;
            }
            return (bool)result;
        }
    }
}
