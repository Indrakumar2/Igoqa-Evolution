using AutoMapper;
using Evolution.AuditLog.Domain.Enums;
using Evolution.AuditLog.Domain.Interfaces.Audit;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Common.Models.Messages;
using Evolution.Common.Models.Responses;
using Evolution.Logging.Interfaces;
using Evolution.AuditLog.Domain.Functions;
using Evolution.ResourceSearch.Domain.Interfaces.Data;
using Evolution.ResourceSearch.Domain.Interfaces.ResourceSearch;
using Evolution.ResourceSearch.Domain.Models.ResourceSearch;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using Evolution.AuditLog.Domain.Extensions;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;

namespace Evolution.ResourceSearch.Core.Services
{
    public class ResourceSearchNoteService : IResourceSearchNoteService
    {
        private readonly IAppLogger<ResourceSearchNoteService> _logger = null;
        private readonly JObject _messageDescriptions = null;
        private readonly IMapper _mapper = null;
        public readonly IResourceSearchNoteRepository _resourceSearchNoteRepository = null;
        private readonly IAuditLogger _auditLogger = null;

        public ResourceSearchNoteService(JObject messages,
                                    IAppLogger<ResourceSearchNoteService> logger,
                                    IMapper mapper,
                                    IAuditLogger auditLogger,
                                    IResourceSearchNoteRepository resourceSearchNoteRepository)
        {
            _logger = logger;
            _mapper = mapper;
            _messageDescriptions = messages;
            _auditLogger = auditLogger;
            _resourceSearchNoteRepository = resourceSearchNoteRepository;
        }

        public Response Get(ResourceSearchNote searchModel)
        {
            IList<ResourceSearchNote> result = null;
            Exception exception = null;

            try
            {
                result = this._resourceSearchNoteRepository.Search(searchModel);
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), searchModel);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception, result?.Count);
        }

        public Response Get(IList<int> ResourceSearchIds, bool fetchLatest = false)
        {
            IList<ResourceSearchNote> result = null;
            Exception exception = null;

            try
            {
                result = this._resourceSearchNoteRepository.Get(ResourceSearchIds, fetchLatest);
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), ResourceSearchIds);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception, result?.Count);
        }

        public Response Save(IList<ResourceSearchNote> resourceSearchNotes, bool commitchange = true)
        {
            Exception exception = null;
            List<ValidationMessage> validationMessages = null;
            List<MessageDetail> errorMessages = null;
            try
            { 
                IList<ResourceSearchNote> recordToBeInserted = null;

                if (this.IsRecordValidForProcess(resourceSearchNotes, ValidationType.Add, ref recordToBeInserted, ref errorMessages, ref validationMessages))
                { 
                    var dbNotesToBeInserted = recordToBeInserted.Select(x => new DbModel.ResourceSearchNote()
                    {
                        ResourceSearchId= x.ResourceSearchId,
                        CreatedDate = DateTime.UtcNow,
                        CreatedBy = x.CreatedBy,
                        Note = x.Notes, 
                    }).ToList();

                    _resourceSearchNoteRepository.AutoSave = false;
                    _resourceSearchNoteRepository.Add(dbNotesToBeInserted);

                    if (commitchange)
                    {
                       int value= _resourceSearchNoteRepository.ForceSave();
                        if (value > 0)
                            recordToBeInserted?.ToList().ForEach(x => AuditLog(recordToBeInserted.FirstOrDefault(),
                                                                    ValidationType.Add.ToAuditActionType(),
                                                                    SqlAuditModuleType.ResourceSearchNote,
                                                                    null,
                                                                    _mapper.Map<ResourceSearchNote>(x)));
                    }
                }


            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), resourceSearchNotes);

            }
            finally
            {
                _resourceSearchNoteRepository.AutoSave = true;
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages, null, exception);
        }

        private bool IsRecordValidForProcess(IList<ResourceSearchNote>  resourceSearchNotes, ValidationType validationType, ref IList<ResourceSearchNote> filteredNotes, ref List<MessageDetail> errorMessages, ref List<ValidationMessage> validationMessages)
        {
            if (validationType == ValidationType.Add)
                filteredNotes = resourceSearchNotes?.Where(x => x.RecordStatus.IsRecordStatusNew()).ToList();
            else if (validationType == ValidationType.Delete)
                filteredNotes = resourceSearchNotes?.Where(x => x.RecordStatus.IsRecordStatusDeleted()).ToList();
             
            if (filteredNotes?.Count <= 0)
                return false; 
            return true;
        }

        private Response AuditLog(ResourceSearchNote resourceSearchNote,
                     SqlAuditActionType sqlAuditActionType,
                     SqlAuditModuleType sqlAuditModuleType,
                     object oldData,
                     object newData)
        {
            LogEventGeneration logEventGeneration = new LogEventGeneration(_auditLogger);
            Exception exception = null;
            long? eventId = 0;
            if (resourceSearchNote != null && !string.IsNullOrEmpty(resourceSearchNote.ActionByUser))
            {
                string actionBy = resourceSearchNote.ActionByUser;
                if (resourceSearchNote.EventId > 0)
                    eventId = resourceSearchNote.EventId;
                else
                    eventId = logEventGeneration.GetEventLogId(eventId,
                                                                  sqlAuditActionType,
                                                                  actionBy,
                                                                  resourceSearchNote.ResourceSearchId.ToString(),
                                                                  "ResourceSearchNote");

                return _auditLogger.LogAuditData((long)eventId, sqlAuditModuleType, oldData, newData);
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, null, null, exception);
        }

    }
 
}
