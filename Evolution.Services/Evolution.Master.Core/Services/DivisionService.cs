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
using System.IO;
using System.Linq;
using System.Reflection;

namespace Evolution.Master.Core
{
    public class DivisionService : MasterService, IDivisionService
    {
        private readonly IAppLogger<MasterService> _logger = null;
        private readonly IMasterRepository _repository = null;
        private readonly IMapper _mapper = null;
        private readonly JObject _messages = null;

        public DivisionService(IMapper mapper, IAppLogger<MasterService> logger, IMasterRepository repository,JObject messages) : base(mapper, logger, repository,messages)
        {
            this._mapper = mapper;
            this._repository = repository;
            this._logger = logger;
            this._messages = messages;
        }

        public Response SaveDivision(IList<Division> model, bool returnResultSet = false)
        {
            Exception exception = null;
            List<MessageDetail> errorMessages = null;
            try
            {
                if (model != null)
                {
                    IList<Division> divisionsExists = null;
                    IList<string> divisionNotExist = null;

                    this.IsValidDivision(model?.Where(x=> x.RecordStatus.IsRecordStatusNew()).Select(x => x.Name)?.ToList(), ref divisionNotExist, ref divisionsExists, ref errorMessages);
                    if (divisionNotExist?.Count > 0)
                    {
                        IList<DbRepository.Models.SqlDatabaseContext.Data> dbDivisions = new List<DbRepository.Models.SqlDatabaseContext.Data>();
                        foreach (var division in model.Where(x => divisionNotExist.Contains(x.Name)))
                        {
                            var dbDivision = _mapper.Map<MasterData, DbRepository.Models.SqlDatabaseContext.Data>(_mapper.Map<Division, MasterData>(division));
                            dbDivision.MasterDataTypeId = Convert.ToInt32(MasterType.Division);
                            dbDivisions.Add(dbDivision);
                        }
                        if (dbDivisions.Count > 0)
                        {
                            this._repository.Add(dbDivisions);
                            if (returnResultSet)
                                return this.Search(model.Select(x => x.Name).ToList());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), model);
            }
            return new Response().ToPopulate(ResponseType.Success, null, errorMessages, null, null, exception);
        }

        public Response Search(Division search)
        {
            IList<Division> result = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                if (search == null)
                    search = new Division();

                var masterData = _mapper.Map<Division, MasterData>(search);
                result = this._repository.FindBy(_mapper.Map<MasterData, Data>(masterData).ToExpression())
                                            .Where(x => x.MasterDataTypeId == Convert.ToInt32(MasterType.Division))
                                            .Select(x => _mapper.Map<MasterData, Division>(_mapper.Map<Data, MasterData>(x))).OrderBy(x => x.Name).ToList();
            }
            catch (Exception ex)
            {
                responseType = ResponseType.Exception;
                this._logger.LogError(responseType.ToId(), this._messages[responseType.ToId()].ToString(), ex);
            }
            return new Response().ToPopulate(responseType, result, result?.Count);
        }

        public Response Search(IList<string> divisionNames)
        {
            IList<Division> result = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                if (divisionNames != null)
                    result = this._repository.FindBy(x => x.MasterDataTypeId == Convert.ToInt32(MasterType.Division) && divisionNames.Contains(x.Name))
                                            .Select(x => _mapper.Map<MasterData, Division>(_mapper.Map<Data, MasterData>(x))).ToList();
            }
            catch (Exception ex)
            {
                responseType = ResponseType.Exception;
                this._logger.LogError(responseType.ToId(), this._messages[responseType.ToId()].ToString(), ex);
            }
            return new Response().ToPopulate(responseType, result, result?.Count);
        }

        public bool IsValidDivision(IList<string> divisionNames, ref IList<string> divisionNotExist, ref IList<Division> divisions, ref List<MessageDetail> errorMessages)
        {
            var messages = new List<MessageDetail>();
            if (errorMessages == null)
                errorMessages = new List<MessageDetail>();

            if (divisionNames?.Count > 0)
            {
                var searchResult = this.Search(divisionNames).Result?.Populate<IList<Division>>();
                var divisionNotExists = divisionNames.Where(x => !searchResult.Any(x1 => x1.Name.ToLower() == x.ToLower())).ToList();
                divisionNotExists.ForEach(x =>
                {
                    string errorCode = MessageType.DivisionNotExists.ToId();
                    messages.Add(new MessageDetail(ModuleType.Master, errorCode, string.Format(_messages[errorCode].ToString(), x)));
                });

                divisionNotExist = divisionNotExists;
                divisions = searchResult;

                if (messages.Count > 0)
                    errorMessages.AddRange(messages);
            }
            return errorMessages?.Count <= 0;
        }

        #region Delete
        public Response Delete(IList<int> divisionIds)
        {
            Exception exception = null;
            List<MessageDetail> errorMessages = null;
            try
            {
                if (divisionIds != null && divisionIds.Any())
                {
                    var dbDivisions = this._repository.FindBy(x => x.MasterDataTypeId == Convert.ToInt32(MasterType.Division) && divisionIds.Contains(x.Id)).ToList();
                         
                        if (dbDivisions!=null && dbDivisions.Any())
                        {
                            this._repository.Delete(dbDivisions); 
                        } 
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), divisionIds);
            }
            return new Response().ToPopulate(ResponseType.Success, null, errorMessages, null, null, exception);
        }
        #endregion
    }
}