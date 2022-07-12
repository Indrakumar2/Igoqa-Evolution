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
    public class ExportPrefixService : MasterService, IExportPrefixService
    {
        private readonly IAppLogger<MasterService> _logger = null;
        private readonly IMasterRepository _repository = null;
        private readonly IMapper _mapper = null;
        private readonly JObject _messages = null;

        public ExportPrefixService(IMapper mapper, IAppLogger<MasterService> logger, IMasterRepository repository,JObject messages) : base(mapper, logger, repository,messages)
        {
            this._mapper = mapper;
            this._repository = repository;
            this._logger = logger;
            this._messages = messages;
        }

        public bool IsValidExportPrefixes(IList<string> exportPrefixNames, ref IList<string> exportPrefixNotExists, ref IList<ExportPrefix> exportPrefixes, ref List<MessageDetail> errorMessages)
        {
            var messages = new List<MessageDetail>();
            if (errorMessages == null)
                errorMessages = new List<MessageDetail>();

            if (exportPrefixNames?.Count > 0)
            {
                var searchResult = this.Search(exportPrefixNames).Result?.Populate<IList<ExportPrefix>>();
                var payrollNotExist = exportPrefixNames.Where(x => !searchResult.Any(x1 => x1.Name.ToLower() == x.ToLower())).ToList();
                payrollNotExist.ForEach(x =>
                {
                    string errorCode = MessageType.PayrollNotExists.ToId();
                    messages.Add(new MessageDetail(ModuleType.Master, errorCode, string.Format(_messages[errorCode].ToString(), x)));
                });

                exportPrefixNotExists = payrollNotExist;
                exportPrefixes = searchResult;

                if (messages.Count > 0)
                    errorMessages.AddRange(messages);
            }
            return errorMessages?.Count <= 0;
        }

        public Response SaveExportPrefix(IList<ExportPrefix> model, bool returnResultSet = false)
        {
            Exception exception = null;
            List<MessageDetail> errorMessages = null;
            try
            {
                if (model != null)
                {
                    IList<ExportPrefix> payrollExists = null;
                    IList<string> payrollNotExist = null;

                    this.IsValidExportPrefixes(model?.Where(x => x.RecordStatus.IsRecordStatusNew())?.Select(x => x.Name)?.ToList(), ref payrollNotExist, ref payrollExists, ref errorMessages);
                    if (payrollNotExist?.Count > 0)
                    {
                        IList<DbRepository.Models.SqlDatabaseContext.Data> dbExportPrefixes = new List<DbRepository.Models.SqlDatabaseContext.Data>();
                        foreach (var exportPrefix in model.Where(x => payrollNotExist.Contains(x.Name)))
                        {
                            var dbExportPrefix = _mapper.Map<MasterData, DbRepository.Models.SqlDatabaseContext.Data>(_mapper.Map<ExportPrefix, MasterData>(exportPrefix));
                            dbExportPrefix.MasterDataTypeId = Convert.ToInt32(MasterType.PayrollExportPrefix);
                            dbExportPrefixes.Add(dbExportPrefix);
                        }
                        if (dbExportPrefixes.Count > 0)
                        {
                            this._repository.Add(dbExportPrefixes);
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

        public Response Search(ExportPrefix search)
        {
            IList<ExportPrefix> result = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                if (search == null)
                    search = new ExportPrefix();

                var masterData = _mapper.Map<ExportPrefix, MasterData>(search);
                result = this._repository.FindBy(_mapper.Map<MasterData, Data>(masterData).ToExpression())
                                            .Where(x => x.MasterDataTypeId == Convert.ToInt32(MasterType.PayrollExportPrefix))
                                            .Select(x => _mapper.Map<MasterData, ExportPrefix>(_mapper.Map<Data, MasterData>(x))).OrderBy(x => x.Name).ToList();
            }
            catch (Exception ex)
            {
                responseType = ResponseType.Exception;
                this._logger.LogError(responseType.ToId(), this._messages[responseType.ToId()].ToString(), ex);
            }
            return new Response().ToPopulate(responseType, result, result?.Count);

        }

        public Response Search(IList<string> prefixesNames)
        {
            IList<ExportPrefix> result = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                if (prefixesNames != null)
                    result = this._repository.FindBy(x => x.MasterDataTypeId == Convert.ToInt32(MasterType.PayrollExportPrefix) && prefixesNames.Contains(x.Name))
                                            .Select(x => _mapper.Map<MasterData, ExportPrefix>(_mapper.Map<Data, MasterData>(x))).OrderBy(x => x.Name).ToList();
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
