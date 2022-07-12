using AutoMapper;
using Evolution.AuditLog.Domain.Enums;
using Evolution.AuditLog.Domain.Extensions;
using Evolution.AuditLog.Domain.Functions;
using Evolution.AuditLog.Domain.Interfaces.Audit;
using Evolution.Common.Constants;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Common.Helpers;
using Evolution.Common.Models.Base;
using Evolution.Common.Models.Filters;
using Evolution.Common.Models.Messages;
using Evolution.Common.Models.Responses;
using Evolution.Company.Domain.Enums;
using Evolution.Document.Domain.Interfaces.Documents;
using Evolution.Logging.Interfaces;
using Evolution.Master.Domain.Interfaces.Data;
using Evolution.Master.Domain.Models;
using Evolution.NumberSequence.InfraStructure.Interface;
using Evolution.Security.Domain.Interfaces.Data;
using Evolution.Visit.Domain.Interfaces.Data;
using Evolution.Visit.Domain.Interfaces.Validations;
using Evolution.Visit.Domain.Interfaces.Visits;
using Evolution.Visit.Domain.Models.Visits;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.Visit.Domain.Models.Visits;

namespace Evolution.Visit.Core.Services
{
    public class VisitService : IVisitService
    {
        private readonly IMapper _mapper = null;
        private readonly IAppLogger<VisitService> _logger = null;
        private readonly IVisitRepository _repository = null;
        private readonly IVisitValidationService _visitValidationService = null;
        private readonly JObject _messages = null;
        private readonly IAuditSearchService _auditSearchService = null;
        private readonly INumberSequenceRepository _numberSequenceRepository = null;
        private readonly IMongoDocumentService _mongoDocumentService = null;
        private readonly IModuleRepository _moduleRepository = null;
        private readonly IMasterRepository _masterRepository = null;
        private readonly AppEnvVariableBaseModel _environment = null;
        private readonly IAuditLogger _auditlogger = null;

        public VisitService(IAppLogger<VisitService> logger,
                                IVisitRepository visitRepository,
                                IVisitValidationService visitValidationService,
                                IMapper mapper,
                                JObject messages,
                                IAuditSearchService auditSearchService,
                                IMongoDocumentService mongoDocumentService,
                                INumberSequenceRepository numberSequenceRepository,
                                IModuleRepository moduleRepository,
                                IMasterRepository masterRepository,
                                IOptions<AppEnvVariableBaseModel> environment,
                                IAuditLogger auditLogger
                                )
        {
            _mongoDocumentService = mongoDocumentService;
            _mapper = mapper;
            _logger = logger;
            _repository = visitRepository;
            _visitValidationService = visitValidationService;
            this._messages = messages;
            _auditSearchService = auditSearchService;
            _numberSequenceRepository = numberSequenceRepository;
            _moduleRepository = moduleRepository;
            _masterRepository = masterRepository;
            _environment = environment.Value;
            _auditlogger = auditLogger;
        }

        public Response GetVisit(DomainModel.BaseVisit searchModel, AdditionalFilter filter = null)
        {
            IList<DomainModel.VisitSearchResults> result = null;
            Int32? count = null;
            Exception exception = null;
            try
            {
                using (var tranScope = new TransactionScope(TransactionScopeOption.RequiresNew,
                                              new TransactionOptions
                                              {
                                                  IsolationLevel = IsolationLevel.ReadUncommitted
                                              }))
                {
                    if (filter != null && !filter.IsRecordCountOnly)
                        result = this._repository.Search(searchModel);
                    else
                        count = this._repository.GetCount(searchModel);
                    tranScope.Complete();
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), searchModel);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception, count);
        }

        public Response GetVisitData(DomainModel.BaseVisit searchModel)
        {
            IList<DomainModel.BaseVisit> result = null;
            Exception exception = null;
            try
            {
                result = this._repository.GetVisit(searchModel);
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), searchModel);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception, result.Count);
        }

        public async Task<Response> GetSearchVisits(DomainModel.VisitSearch searchModel)
        {
            IList<DomainModel.BaseVisit> result = null;
            Result export = null;
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            IList<string> mongoSearch = null;
            try
            {
                searchModel.FetchCount = _environment.VisitRecordSize;
                searchModel.OrderBy = VisitTimesheetConstants.ORDER_BY;
                //Mongo Doc Search
                if (!string.IsNullOrEmpty(searchModel.SearchDocumentType) || !string.IsNullOrEmpty(searchModel.DocumentSearchText))
                {
                    var evoMongoDocSearch = _mapper.Map<Document.Domain.Models.Document.EvolutionMongoDocumentSearch>(searchModel);
                    mongoSearch = await this._mongoDocumentService.SearchDistinctFieldAsync(evoMongoDocSearch);
                    if (mongoSearch != null && mongoSearch.Count > 0)
                    {
                        var ids = mongoSearch.Where(x => !string.IsNullOrEmpty(x) && x != "0" && x != "null" && x != "undefined").Distinct().ToList();
                        searchModel.VisitIds = ids.Select(x => Convert.ToInt64(x)).ToList();
                        if (searchModel.IsExport == true)
                            export = SearchVisits(searchModel, _environment.ChunkSize);
                        else
                            result = SearchVisits(searchModel, _environment.VisitRecordSize)?.BaseVisit;
                    }
                    else
                        result = new List<DomainModel.BaseVisit>();
                }
                else
                {
                    if (searchModel.IsExport == true)
                        export = SearchVisits(searchModel, _environment.ChunkSize);
                    else
                    {
                        List<BaseVisit> visitDetails = SearchVisits(searchModel, _environment.VisitRecordSize)?.BaseVisit?.ToList();
                        result = visitDetails ?? new List<BaseVisit>();
                    }
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                _logger.LogError(responseType.ToId(), ex.ToFullString(), searchModel);
            }

            if (searchModel.IsExport == true)
                return new Response().ToPopulate(responseType, null, null, null, export, exception, export?.BaseVisit?.FirstOrDefault()?.TotalCount);
            else
                return new Response().ToPopulate(responseType, null, null, null, result, exception, result?.FirstOrDefault()?.TotalCount);
        }

        private Result SearchVisits(DomainModel.VisitSearch searchModel, int fetchCount)
        {
            Result result = null;
            using (var tranScope = new TransactionScope(TransactionScopeOption.RequiresNew,
                                               new TransactionOptions
                                               {
                                                   IsolationLevel = IsolationLevel.ReadUncommitted
                                               }))
            {
                searchModel.FetchCount = fetchCount;
                result = this._repository.SearchVisits(searchModel);
                tranScope.Complete();
            }
            return result;
        }

        public Response GetVisitByID(DomainModel.BaseVisit searchModel)
        {
            DomainModel.Visit result = null;
            Exception exception = null;
            try
            {
                using (var tranScope = new TransactionScope(TransactionScopeOption.RequiresNew,
                                                   new TransactionOptions
                                                   {
                                                       IsolationLevel = IsolationLevel.ReadUncommitted
                                                   }))
                {
                    result = this._repository.GetVisitByID(searchModel);
                    tranScope.Complete();
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), searchModel);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception);
        }

        public Response GetHistoricalVisits(DomainModel.BaseVisit searchModel)
        {
            IList<DomainModel.Visit> result = null;
            Exception exception = null;
            try
            {
                result = this._repository.GetHistoricalVisits(searchModel);
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), searchModel);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception, result.Count);
        }

        /*This is used for Document Approval Dropdown binding*/
        public Response GetVisitForDocumentApproval(DomainModel.BaseVisit searchModel)
        {
            IList<DomainModel.BaseVisit> result = null;
            Exception exception = null;
            try
            {
                using (var tranScope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    result = _repository.GetVisitForDocumentApproval(searchModel)?.Select(x => new DomainModel.BaseVisit
                    {
                        ReportNumber = x.ReportNumber,
                        VisitNumber = x.VisitNumber,
                        VisitId = x.Id,
                        DocumentApprovalVisitValue = string.IsNullOrEmpty(x.ReportNumber) ? Convert.ToString(x.Id) : x.ReportNumber
                    })?.ToList();
                    tranScope.Complete();
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), searchModel);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception, result?.Count);
        }

        public Response GetTemplate(string companyCode, CompanyMessageType companyMessageType, EmailKey emailKey)
        {
            string emailContent = string.Empty;
            Exception exception = null;
            try
            {
                emailContent = _repository.GetTemplate(companyCode, companyMessageType, emailKey.ToString());
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), emailContent);
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, null, emailContent, exception, null);
        }

        public Response GetIntertekWorkHistoryReport(int epin)
        {
            object reportData = null;
            Exception exception = null;
            try
            {
                reportData = _repository.GetIntertekWorkHistoryReport(epin);
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), epin);
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, null, reportData, exception, null);
        }

        public Response AddSkeletonVisit(DbModel.Visit dbVisit, ref DbModel.Visit dbSavedVisit, bool commitChange)
        {
            Exception exception = null;
            try
            {
                _repository.AutoSave = false;
                if (dbVisit != null)
                    _repository.Add(dbVisit);

                if (commitChange)
                    _repository.ForceSave();

                dbSavedVisit = dbVisit;
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), dbVisit);
            }
            finally
            {
                _repository.AutoSave = true;
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, null, exception);
        }


        public Response ModifyVisit(DomainModel.Visit visits, bool commitChange = true)
        {
            List<MessageDetail> errorMessages = null;
            Exception exception = null;
            List<ValidationMessage> validationMessages = null;
            try
            {
                DomainModel.BaseVisit searchModel = new DomainModel.BaseVisit
                {
                    VisitId = visits.VisitId
                };
                DbModel.Visit dbVisit = _repository.GetDBVisitByID(searchModel);
                var dbContractToBeUpdated = AssignVisitValues(visits, dbVisit);
                _repository.Update(dbContractToBeUpdated);
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), visits);
            }

            return new Response().ToPopulate(ResponseType.Success, null, errorMessages, validationMessages, null, exception);
        }

        public Response DeleteVisit(DomainModel.Visit visits, bool commitChange = true)
        {
            Response result = null;
            Exception exception = null;
            try
            {
                //result = this._repository.DeleteVisit(visits, commitChange);
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), visits);
            }

            return result;
        }

        public Response SaveVisit(DomainModel.Visit visits, bool commitChange = true)
        {
            Response result = null;
            Exception exception = null;
            try
            {
                //result = this._repository.SaveVisit(visits, commitChange);
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), visits);
            }

            return result;
        }

        public Response GetSupplierList(DomainModel.BaseVisit searchModel)
        {
            IList<DomainModel.Visit> result = null;
            Exception exception = null;
            try
            {
                result = this._repository.GetSupplierList(searchModel);
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), searchModel);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception, result.Count);
        }

        public Response GetTechnicalSpecialistList(DomainModel.BaseVisit searchModel)
        {
            IList<DomainModel.Visit> result = null;
            Exception exception = null;
            try
            {
                result = this._repository.GetTechnicalSpecialistList(searchModel);
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), searchModel);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception, result.Count);
        }

        public Response GetVistListByIds(IList<long> visitIds)
        {
            IList<DbModel.Visit> result = null;
            string[] includes = null;
            Exception exception = null;
            try
            {
                result = this._repository.FindBy(x => visitIds.Contains(x.Id), includes).ToList();
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), visitIds);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception, result.Count);
        }

        #region Add
        public Response Add(IList<DomainModel.Visit> visits,
                           ref long? eventId,
                            bool commitChange = true,
                            bool isValidationRequire = true,
                            bool isProcessNumberSequence = false)
        {
            IList<DbModel.Visit> dbVisit = null;
            IList<DbModel.Assignment> dbAssignment = null;
            IList<DbModel.SqlauditModule> dbModule = null;
            return AddVisit(visits,
                               ref dbVisit,
                               ref dbAssignment,
                               dbModule,
                               ref eventId,
                                commitChange,
                                isValidationRequire,
                                isProcessNumberSequence);
        }

        public Response Add(IList<DomainModel.Visit> visits,
                             ref IList<DbModel.Visit> dbVisit,
                             ref IList<DbModel.Assignment> dbAssignment,
                             IList<DbModel.SqlauditModule> dbModule,
                             ref long? eventId,
                             bool commitChange = true,
                             bool isValidationRequire = true,
                             bool isProcessNumberSequence = false)
        {
            return AddVisit(visits,
                                ref dbVisit,
                                ref dbAssignment,
                                dbModule,
                                ref eventId,
                                commitChange,
                                isValidationRequire,
                                isProcessNumberSequence);
        }
        #endregion

        #region Modify
        public Response Modify(IList<DomainModel.Visit> visits,
                              ref long? eventId,
                            bool commitChange = true,
                            bool isValidationRequire = true)
        {
            IList<DbModel.Visit> dbVisit = null;
            IList<DbModel.Assignment> dbAssignment = null;
            IList<DbModel.SqlauditModule> dbModule = null;
            return UpdateVisit(visits,
                                    ref dbVisit,
                                    dbModule,
                                    ref eventId,
                                    ref dbAssignment,
                                    commitChange,
                                    isValidationRequire);

        }



        public Response Modify(IList<DomainModel.Visit> visits,

                           ref IList<DbModel.Visit> dbVisit,
                           ref IList<DbModel.Assignment> dbAssignment,
                           IList<DbModel.SqlauditModule> dbModule,
                           ref long? eventId,
                           bool commitChange = true,
                           bool isValidationRequire = true)
        {
            return UpdateVisit(visits,
                               ref dbVisit,
                               dbModule,
                               ref eventId,
                               ref dbAssignment,
                               commitChange,
                               isValidationRequire);
        }

        #endregion

        #region Delete
        public Response Delete(IList<DomainModel.Visit> visits,
                               ref long? eventId,
                               bool commitChange = true,
                               bool isValidationRequire = true)
        {
            IList<DbModel.SqlauditModule> dbModule = null;
            return this.RemoveVisit(visits,
                                     dbModule,
                                      ref eventId,
                                      commitChange,
                                      isValidationRequire);
        }
        public Response Delete(IList<DomainModel.Visit> visits,
                               IList<DbModel.SqlauditModule> dbModule,
                               ref long? eventId,
                               bool commitChange = true,
                               bool isValidationRequire = true)
        {

            return this.RemoveVisit(visits,
                                     dbModule,
                                      ref eventId,
                                      commitChange,
                                      isValidationRequire);
        }



        #endregion

        public Response IsRecordValidForProcess(IList<DomainModel.Visit> visits,
                                                ValidationType validationType)
        {
            IList<DbModel.Visit> dbVisits = null;
            IList<DbModel.Assignment> dbAssignments = null;
            return IsRecordValidForProcess(visits,
                                           validationType,
                                           ref dbVisits,
                                           ref dbAssignments);
        }

        public Response IsRecordValidForProcess(IList<DomainModel.Visit> visits,
                                                ValidationType validationType,
                                                ref IList<DbModel.Visit> dbVisits,
                                                ref IList<DbModel.Assignment> dbAssignments)
        {
            IList<DomainModel.Visit> filteredVisits = null;
            return IsRecordValidForProcess(visits,
                                           validationType,
                                           ref filteredVisits,
                                           ref dbVisits,
                                           ref dbAssignments);
        }



        public Response IsRecordValidForProcess(IList<DomainModel.Visit> visits,
                                                ValidationType validationType,
                                                IList<DbModel.Visit> dbVisits,
                                                IList<DbModel.Assignment> dbAssignments)
        {
            return IsRecordValidForProcess(visits,
                                             validationType,
                                             ref dbVisits,
                                             ref dbAssignments);
        }

        public bool IsValidVisit(IList<long> visitId,
                                    ref IList<DbModel.Visit> dbVisits,
                                    ref IList<ValidationMessage> messages,
                                    params string[] includes)
        {
            IList<ValidationMessage> message = new List<ValidationMessage>();
            if (dbVisits == null)
            {
                var dbVisit = _repository?.FindBy(x => visitId.Contains(x.Id), includes)?.ToList();
                var visitNotExists = visitId?.Where(x => !dbVisit.Any(x2 => x2.Id == x))?.ToList();
                visitNotExists?.ForEach(x =>
                {
                    string errorCode = MessageType.VisitNotExists.ToId();
                    message.Add(_messages, x, MessageType.VisitNotExists, x);
                });
                dbVisits = dbVisit;
                messages = message;
            }

            return messages?.Count <= 0;
        }

        public bool IsValidVisitData(IList<long> visitId,
                                   ref IList<DbModel.Visit> dbVisits,
                                   ref IList<ValidationMessage> messages)
        {
            IList<ValidationMessage> message = new List<ValidationMessage>();
            if (dbVisits == null)
            {
                //var myData = _repository.Get(x => visitId.Contains(x.Id), x => new { x.Id });
                var dbVisit = _repository.Get(x => visitId.Contains(x.Id), x => new DbModel.Visit { Id = x.Id })?.ToList();
                var visitNotExists = visitId?.Where(x => !dbVisit.Any(x2 => x2.Id == x))?.ToList();
                visitNotExists?.ForEach(x =>
                {
                    string errorCode = MessageType.VisitNotExists.ToId();
                    message.Add(_messages, x, MessageType.VisitNotExists, x);
                });
                dbVisits = dbVisit;
                messages = message;
            }

            return messages?.Count <= 0;
        }

        public void AddNumberSequence(DbModel.NumberSequence data, int parentId, int parentRefId, int assignmentId, ref List<DbModel.NumberSequence> numberSequence)
        {
            NumberSequence(data, parentId, parentRefId, assignmentId, ref numberSequence);
        }

        private int ProcessNumberSequence(int assignmentId,
                                        ref List<DbModel.NumberSequence> lstVisitNumberSequenceAdd,
                                        ref List<DbModel.NumberSequence> lstVisitNumberSequenceUpdate)
        {
            DbModel.NumberSequence visitNumberSequence = null;
            int visitNumber = ProcessNumberSequence(assignmentId, ref visitNumberSequence);
            if (visitNumberSequence.LastSequenceNumber == 1)
            {
                lstVisitNumberSequenceAdd.Add(visitNumberSequence);
            }
            else if (visitNumberSequence.LastSequenceNumber > 1)
            {
                lstVisitNumberSequenceUpdate.Add(visitNumberSequence);
            }
            return visitNumber;
        }

        public int ProcessNumberSequence(int assignmentId, ref DbModel.NumberSequence visitNumberSequence)
        {
            var modules = _moduleRepository.FindBy(x => x.Name == "Assignment" || x.Name == "Visit").ToList();

            List<DbModel.NumberSequence> dbVisitNumberSequence = _numberSequenceRepository.FindBy(x => x.ModuleData == assignmentId && x.Module.Name == "Assignment").ToList();
            if (dbVisitNumberSequence.Count > 0)
            {
                visitNumberSequence = dbVisitNumberSequence?.FirstOrDefault(x1 => x1.ModuleData == assignmentId);
                visitNumberSequence.LastSequenceNumber = visitNumberSequence.LastSequenceNumber + 1;
            }
            else
            {
                visitNumberSequence = new DbModel.NumberSequence()
                {
                    LastSequenceNumber = 1,
                    ModuleId = modules.FirstOrDefault(x1 => x1.Name == "Assignment").Id,
                    ModuleData = assignmentId,
                    ModuleRefId = modules.FirstOrDefault(x1 => x1.Name == "Visit").Id,
                };
            }
            return visitNumberSequence.LastSequenceNumber;
        }

        public Response SaveNumberSequence(DbModel.NumberSequence visitNumberSequence)
        {
            Exception exception = null;
            try
            {
                _numberSequenceRepository.AutoSave = false;

                if (visitNumberSequence.LastSequenceNumber == 1)
                    _numberSequenceRepository.Add(visitNumberSequence);
                if (visitNumberSequence.LastSequenceNumber > 1)
                    _numberSequenceRepository.Update(visitNumberSequence, x => x.LastSequenceNumber);
                _numberSequenceRepository.ForceSave();
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), visitNumberSequence);
            }
            finally
            {
                _numberSequenceRepository.AutoSave = true;
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, null, visitNumberSequence, exception);
        }

        private Response IsRecordValidForProcess(IList<DomainModel.Visit> visits,
                                                 ValidationType validationType,
                                                 ref IList<DomainModel.Visit> filteredVisits,
                                                 ref IList<DbModel.Visit> dbVisits,
                                                 ref IList<DbModel.Assignment> dbAssignments)
        {
            Exception exception = null;
            bool result = true;
            IList<ValidationMessage> messages = null;
            try
            {
                if (filteredVisits == null || filteredVisits.Count <= 0)
                    filteredVisits = FilterRecord(visits, validationType);

                if (filteredVisits != null && filteredVisits.Count > 0)
                {
                    if (messages == null)
                        messages = new List<ValidationMessage>();
                    result = IsValidPayload(filteredVisits, validationType, ref messages);

                    if (result)
                    {
                        List<long> visitNotExists = null;
                        var visitIds = filteredVisits.Where(x => x.VisitId > 0).Select(x => (long)x.VisitId).Distinct().ToList();
                        var assignmentIds = filteredVisits.Where(x => x.VisitAssignmentId > 0).Select(x => x.VisitAssignmentId).Distinct().ToList();
                        if ((dbVisits == null || dbVisits?.Count <= 0) && validationType != ValidationType.Add)
                            dbVisits = GetData(filteredVisits, "Assignment");

                        if (validationType == ValidationType.Delete || validationType == ValidationType.Update)
                        {
                            result = IsVisitExistInDb(visitIds,
                                                           dbVisits,
                                                           ref visitNotExists,
                                                           ref messages);

                            if (result && validationType == ValidationType.Delete)
                                result = IsVisitCanBeRemove(dbVisits, ref messages);

                            else if (result && validationType == ValidationType.Update)
                                result = IsRecordValidForUpdate(filteredVisits,
                                                                dbVisits,
                                                                ref dbAssignments,
                                                                ref messages);
                        }
                        else if (validationType == ValidationType.Add)
                            result = IsRecordValidForAdd(filteredVisits,
                                                         ref dbAssignments,
                                                         ref messages);
                    }
                }
            }
            catch (Exception ex)
            {
                result = false;
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), filteredVisits);
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, messages?.ToList(), result, exception);
        }

        private Response AddVisit(IList<DomainModel.Visit> visits,
                                     ref IList<DbModel.Visit> dbVisits,
                                     ref IList<DbModel.Assignment> dbAssignments,
                                     IList<DbModel.SqlauditModule> dbModule,
                                     ref long? eventId,
                                     bool commitChange,
                                     bool isValidationRequire,
                                     bool isProcessNumberSequence)
        {
            Exception exception = null;
            IList<ValidationMessage> validationMessages = null;
            IList<DbModel.Assignment> dbAssignment = null;
            List<DbModel.NumberSequence> numberSequenceToAdd = null;
            List<DbModel.NumberSequence> numberSequenceToUpdate = null;
            long? eventID = 0;

            try
            {
                Response valdResponse = null;
                var recordToBeAdd = FilterRecord(visits, ValidationType.Add);
                if (isValidationRequire)
                    valdResponse = IsRecordValidForProcess(visits,
                                                           ValidationType.Add,
                                                           ref dbVisits,
                                                           ref dbAssignments);

                if (!isValidationRequire || Convert.ToBoolean(valdResponse.Result))
                {
                    _repository.AutoSave = false;
                    dbAssignment = dbAssignments;
                    if (isProcessNumberSequence)
                    {
                        _numberSequenceRepository.AutoSave = false;
                        numberSequenceToAdd = new List<DbModel.NumberSequence>();
                        numberSequenceToUpdate = new List<DbModel.NumberSequence>();

                        recordToBeAdd.ToList()?.ForEach(x =>
                        {
                            x.VisitNumber = ProcessNumberSequence(x.VisitAssignmentId, ref numberSequenceToAdd, ref numberSequenceToUpdate);
                        });
                    }

                    var dbRecordToBeInserted = _mapper.Map<IList<DbModel.Visit>>(recordToBeAdd, opt =>
                    {
                        opt.Items["Assignment"] = dbAssignment;
                        opt.Items["isVisitId"] = false;
                        opt.Items["isVisitNumber"] = true;
                    });

                    _repository.Add(dbRecordToBeInserted);

                    if (commitChange)
                    {
                        var savCnt = _repository.ForceSave();
                        if (isProcessNumberSequence)
                        {
                            if (numberSequenceToAdd.Count > 0)
                                _numberSequenceRepository.Add(numberSequenceToAdd);
                            if (numberSequenceToUpdate.Count > 0)
                                _numberSequenceRepository.Update(numberSequenceToUpdate);
                            _numberSequenceRepository.ForceSave();
                        }
                        if (savCnt > 0)
                        {
                            if (dbRecordToBeInserted?.Count > 0 && recordToBeAdd?.Count > 0)
                            {
                                int? visitNumber = _numberSequenceRepository.FindBy(x => x.ModuleData == visits.FirstOrDefault().VisitAssignmentId && x.ModuleId == 5 && x.ModuleRefId == 17)?.FirstOrDefault()?.LastSequenceNumber;
                                dbRecordToBeInserted?.ToList().ForEach(x =>
                               recordToBeAdd?.ToList().ForEach(x1 => _auditSearchService.AuditLog(x1, ref eventID, x1.ActionByUser,
                                                                                                 "{" + AuditSelectType.Id + ":" + x?.Id + "}${" + AuditSelectType.ReportNumber + ":" + x?.ReportNumber?.Trim() + "}${" + AuditSelectType.JobReferenceNumber + ":" + x.Assignment?.Project.ProjectNumber + "-" + x.Assignment?.AssignmentNumber + "-" + visitNumber + "}${" +
                                                                                                 AuditSelectType.ProjectAssignment + ":" + x.Assignment?.Project.ProjectNumber + "-" + x.Assignment?.AssignmentNumber + "}",
                                                                                                 ValidationType.Add.ToAuditActionType(),
                                                                                                 SqlAuditModuleType.Visit,
                                                                                                 null,
                                                                                                 _mapper.Map<DomainModel.Visit>(x),
                                                                                                 dbModule
                                                                                                   )));
                                eventId = eventID;
                            }
                        }
                    }

                    dbVisits = dbRecordToBeInserted;
                }
                else
                    return valdResponse;
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), visits);
            }
            finally
            {
                dbAssignment = null;
                numberSequenceToAdd = null;
                numberSequenceToUpdate = null;
                _repository.AutoSave = true;
                _numberSequenceRepository.AutoSave = true;
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), dbVisits, exception);
        }

        private void NumberSequence(DbModel.NumberSequence data,
                               int parentId,
                               int parentRefId,
                               int assignmentId,
                               ref List<DbModel.NumberSequence> numberSequence)
        {
            if (numberSequence == null)
                numberSequence = new List<DbModel.NumberSequence>();
            if (data != null)
            {
                data.LastSequenceNumber = data.LastSequenceNumber + 1;
                numberSequence.Add(data);
            }
            else
            if (parentId > 0 && parentRefId > 0)
                numberSequence.Add(new DbModel.NumberSequence() { ModuleId = parentId, ModuleRefId = parentRefId, ModuleData = assignmentId, LastSequenceNumber = 1 });
        }

        private Response UpdateVisit(IList<DomainModel.Visit> visits,
                                         ref IList<DbModel.Visit> dbVisits,
                                         IList<DbModel.SqlauditModule> dbModule,
                                         ref long? eventId,
                                         ref IList<DbModel.Assignment> dbAssignments,
                                         bool commitChange,
                                         bool isValidationRequire)
        {
            Exception exception = null;
            IList<ValidationMessage> validationMessages = null;
            IList<DbModel.Assignment> dbAssignment = null;
            long? eventID = 0;
            try
            {
                var recordToBeModify = FilterRecord(visits, ValidationType.Update);
                Response valdResponse = null;

                if (isValidationRequire)
                    valdResponse = IsRecordValidForProcess(visits,
                                                           ValidationType.Update,
                                                           ref recordToBeModify,
                                                           ref dbVisits,
                                                           ref dbAssignments);

                if (!isValidationRequire || (Convert.ToBoolean(valdResponse.Result) && dbVisits?.Count > 0))
                {
                    dbAssignment = dbAssignments;
                    IList<DomainModel.Visit> domExistingVisits = new List<DomainModel.Visit>();
                    dbVisits?.ToList().ForEach(x =>
                    {
                        domExistingVisits.Add(ObjectExtension.Clone(_mapper.Map<DomainModel.Visit>(x)));
                    });
                    dbVisits.ToList().ForEach(visit =>
                    {
                        var visitToBeModify = recordToBeModify.FirstOrDefault(x => x.VisitId == visit.Id);
                        _mapper.Map(visitToBeModify, visit, opt =>
                        {
                            opt.Items["isVisitId"] = true;
                            opt.Items["isVisitNumber"] = false;
                            opt.Items["Assignment"] = dbAssignment;
                        });
                        visit.LastModification = DateTime.UtcNow;
                        visit.UpdateCount = Convert.ToByte(visitToBeModify.UpdateCount).CalculateUpdateCount();
                        visit.ModifiedBy = visitToBeModify.ModifiedBy;
                    });
                    int? proid = visits[0].VisitProjectNumber;
                    var CRN = dbAssignment[0].Project.ProjectClientNotification.FirstOrDefault(x => x.ProjectId == proid);
                    if (CRN != null)
                    {
                        if (CRN.SendCustomerReportingNotification == true && dbVisits[0].VisitStatus == "A")
                        {
                            if (recordToBeModify[0].IsContractHoldingCompanyActive == true)
                            {
                                dbVisits[0].ReportSentToCustomerDate = DateTime.UtcNow;
                            }
                        }
                    }
                    _repository.AutoSave = false;
                    _repository.Update(dbVisits);
                    if (commitChange)
                    {
                        var updCnt = _repository.ForceSave();
                        if (updCnt > 0)
                        {
                            if (dbVisits?.Count > 0 && recordToBeModify?.Count > 0)
                            {
                                dbVisits?.ToList().ForEach(x => recordToBeModify?.ToList().ForEach(x1 =>
                                {
                                    string result = string.Empty;
                                    string olddata = JsonConvert.SerializeObject(_mapper.Map<DomainModel.Visit>(domExistingVisits?.FirstOrDefault(x2 => x2.VisitId == x1.VisitId)));
                                    string newdata = JsonConvert.SerializeObject(x1);
                                    result = CompareJson.CompareVisitObject(olddata, newdata, new List<string>() { "TechSpecialists", "RecordStatus", "ModifiedBy", "ActionByUser" });

                                    if (result != "{}" && result?.Length > 0)
                                    {
                                        _auditSearchService.AuditLog(x1, ref eventID, x1.ActionByUser,
                                                                                                   "{" + AuditSelectType.Id + ":" + x?.Id + "}${" + AuditSelectType.ReportNumber + ":" + x?.ReportNumber?.Trim() + "}${" + AuditSelectType.JobReferenceNumber + ":" + x.Assignment?.Project.ProjectNumber + "-" + x.Assignment?.AssignmentNumber + "-" + x.VisitNumber + "}${" +
                                                                                                   AuditSelectType.ProjectAssignment + ":" + x.Assignment?.Project.ProjectNumber + "-" + x.Assignment?.AssignmentNumber + "}",
                                                                                                   ValidationType.Update.ToAuditActionType(),
                                                                                                   SqlAuditModuleType.Visit,
                                                                                                   _mapper.Map<DomainModel.Visit>(domExistingVisits?.FirstOrDefault(x2 => x2.VisitId == x1.VisitId)),
                                                                                                    x1, dbModule
                                                                                                     );
                                    }
                                    else
                                    {
                                        LogEventGeneration logEvent = new LogEventGeneration(_auditlogger);
                                        eventID = logEvent.GetEventLogId(eventID,
                                                ValidationType.Update.ToAuditActionType(),
                                                x1.ActionByUser,
                                                "{" + AuditSelectType.Id + ":" + x?.Id + "}${" + AuditSelectType.ReportNumber + ":" + x?.ReportNumber?.Trim() + "}${" + AuditSelectType.JobReferenceNumber + ":" + x.Assignment?.Project.ProjectNumber + "-" + x.Assignment?.AssignmentNumber + "-" + x.VisitNumber + "}${" +
                                                AuditSelectType.ProjectAssignment + ":" + x.Assignment?.Project.ProjectNumber + "-" + x.Assignment?.AssignmentNumber + "}",
                                                SqlAuditModuleType.Visit.ToString());
                                    }
                                }
                                ));
                                eventId = eventID;
                            }
                        }
                    }
                }
                else
                    return valdResponse;
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), visits);
            }
            finally
            {
                _repository.AutoSave = true;
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), null, exception);
        }



        private Response RemoveVisit(IList<DomainModel.Visit> visits,
                                        IList<DbModel.SqlauditModule> dbModule,
                                         ref long? eventId,
                                         bool commitChange,
                                         bool isValidationRequire)
        {
            Exception exception = null;
            List<MessageDetail> errorMessages = null;
            List<ValidationMessage> validationMessages = null;
            IList<DbModel.Visit> dbVisits = null;
            IList<DbModel.Assignment> dbAssignments = null;
            long? eventID = 0;

            try
            {
                errorMessages = new List<MessageDetail>();
                validationMessages = new List<ValidationMessage>();
                var recordToDelete = FilterRecord(visits, ValidationType.Delete);


                Response response = null;
                if (isValidationRequire)
                    response = IsRecordValidForProcess(visits,
                                                       ValidationType.Delete,
                                                       ref recordToDelete,
                                                       ref dbVisits,
                                                       ref dbAssignments);

                if (!isValidationRequire || (Convert.ToBoolean(response.Code == ResponseType.Success.ToId()) && dbVisits?.Count > 0))
                {
                    _repository.AutoSave = false;
                    _repository.Delete(dbVisits);
                    {
                        var dbAssignment = dbAssignments;
                        var delCnt = _repository.ForceSave();
                        if (dbVisits?.Count > 0 && visits?.Count > 0)
                        {
                            dbVisits?.ToList().ForEach(x =>
                            recordToDelete?.ToList().ForEach(x1 => _auditSearchService.AuditLog(x1, ref eventID, visits?.FirstOrDefault()?.ActionByUser,
                                                                                               "{" + AuditSelectType.Id + ":" + x?.Id + "}${" + AuditSelectType.ReportNumber + ":" + x?.ReportNumber?.Trim() + "}${" + AuditSelectType.JobReferenceNumber + ":" + x.Assignment?.Project.ProjectNumber + "-" + x.Assignment?.AssignmentNumber + "-" + x.VisitNumber + "}${" +
                                                                                                 AuditSelectType.ProjectAssignment + ":" + x.Assignment?.Project.ProjectNumber + "-" + x.Assignment?.AssignmentNumber + "}",
                                                                                                 ValidationType.Delete.ToAuditActionType(),
                                                                                                 SqlAuditModuleType.Supplier,
                                                                                                 x1,
                                                                                                 null,
                                                                                                 dbModule)));

                            eventId = eventID;
                        }

                    }
                }
                else
                    return response;
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), visits);
            }
            finally
            {
                _repository.AutoSave = true;
            }

            return new Response().ToPopulate(ResponseType.Success, null, errorMessages, validationMessages, null, exception);
        }

        private IList<DomainModel.Visit> FilterRecord(IList<DomainModel.Visit> visits,
                                                           ValidationType filterType)
        {
            IList<DomainModel.Visit> filteredModules = null;

            if (filterType == ValidationType.Add)
                filteredModules = visits?.Where(x => x.RecordStatus.IsRecordStatusNew()).ToList();
            else if (filterType == ValidationType.Update)
                filteredModules = visits?.Where(x => x.RecordStatus.IsRecordStatusModified()).ToList();
            else if (filterType == ValidationType.Delete)
                filteredModules = visits?.Where(x => x.RecordStatus.IsRecordStatusDeleted()).ToList();

            return filteredModules;
        }

        private IList<DbModel.Visit> GetData(IList<DomainModel.Visit> visits,
                                                        params string[] includes)
        {
            IList<DbModel.Visit> dbVisits = null;
            if (visits?.Count > 0)
            {
                var visitId = visits.Where(x => x.VisitId > 0).Select(x => x.VisitId).Distinct().ToList();

                if (visitId?.Count > 0)
                    dbVisits = _repository.FindBy(x => visitId.Contains(x.Id), includes).ToList();
            }
            return dbVisits;
        }

        private bool IsValidPayload(IList<DomainModel.Visit> visits,
                                    ValidationType validationType,
                                    ref IList<ValidationMessage> messages)
        {
            if (messages == null)
                messages = new List<ValidationMessage>();

            var validationResults = _visitValidationService.Validate(JsonConvert.SerializeObject(visits), validationType);
            if (validationResults?.Count > 0)
                messages.Add(_messages, ModuleType.Visit, validationResults);

            return messages?.Count <= 0;
        }


        private bool IsRecordValidForAdd(IList<DomainModel.Visit> filteredData,
                                         ref IList<DbModel.Assignment> dbAssignments,
                                         ref IList<ValidationMessage> messages)
        {
            IList<ValidationMessage> validationMessages = new List<ValidationMessage>();
            if (messages == null)
                messages = new List<ValidationMessage>();

            if (filteredData != null && filteredData.Count > 0)
            {
                var assignmentIds = filteredData.Where(x => x.VisitAssignmentId > 0).Select(x1 => x1.VisitAssignmentId).Distinct().ToList();
                if (messages?.Count <= 0)
                {
                    //_assignmentService.IsValidAssignment(assignmentIds, ref dbAssignments, ref messages);
                    IsValidAssignment(assignmentIds, ref dbAssignments, ref messages);
                }

                if (messages.Count > 0)
                    validationMessages.AddRange(messages);
            }

            return messages?.Count <= 0;

        }

        private bool IsValidAssignment(IList<int> assignmentId,
                                      ref IList<DbModel.Assignment> dbAssignments,
                                      ref IList<ValidationMessage> messages,
                                      params string[] includes)
        {
            IList<ValidationMessage> message = new List<ValidationMessage>();
            if (dbAssignments == null)
            {
                var dbAssignment = _repository.GetDBVisitAssignments(assignmentId, includes);
                var assignmentNotExists = assignmentId?.Where(x => !dbAssignment.Any(x2 => x2.Id == x))?.ToList();
                assignmentNotExists?.ForEach(x =>
                {
                    string errorCode = MessageType.AssignmentNotExists.ToId();
                    message.Add(_messages, x, MessageType.AssignmentNotExists, x);
                });
                dbAssignments = dbAssignment;
                messages = message;
            }

            return messages?.Count <= 0;
        }

        private bool IsRecordValidForUpdate(IList<DomainModel.Visit> filteredData,
                                           IList<DbModel.Visit> dbVisits,
                                           ref IList<DbModel.Assignment> dbAssignments,
                                           ref IList<ValidationMessage> messages)
        {
            IList<ValidationMessage> validationMessages = new List<ValidationMessage>();
            if (messages == null)
                messages = new List<ValidationMessage>();

            if (filteredData != null && filteredData.Count > 0)
            {
                var assignmentIds = filteredData.Where(x => x.VisitAssignmentId > 0).Select(x1 => x1.VisitAssignmentId).Distinct().ToList();
                if (messages?.Count <= 0)
                    if (IsRecordUpdateCountMatching(filteredData, dbVisits, ref messages))
                    {
                        //TO-DO Prathap: Testing for 500 issue fixes and remove                _assignmentService.IsValidAssignment
                        // _assignmentService.IsValidAssignment(assignmentIds, ref dbAssignments, ref messages);
                        IsValidAssignment(assignmentIds, ref dbAssignments, ref messages);
                    }

                if (messages.Count > 0)
                    validationMessages.AddRange(messages);
            }

            return messages?.Count <= 0;
        }


        private bool IsVisitExistInDb(List<long> visitIds,
                                           IList<DbModel.Visit> dbVisits,
                                           ref List<long> visitNotExists,
                                           ref IList<ValidationMessage> messages)
        {
            if (messages == null)
                messages = new List<ValidationMessage>();

            if (dbVisits == null)
                dbVisits = new List<DbModel.Visit>();

            var validMessages = messages;
            if (visitIds?.Count > 0)
            {
                visitNotExists = visitIds.Where(x => !dbVisits.Select(x1 => x1.Id).ToList().Contains(x)).ToList();
                visitNotExists?.ToList().ForEach(x =>
                {
                    validMessages.Add(_messages, x, MessageType.VisitNotExists, x);
                });
            }

            if (validMessages.Count > 0)
                messages = validMessages;

            return messages.Count <= 0;
        }


        private bool IsVisitCanBeRemove(IList<DbModel.Visit> dbVisits,
                                           ref IList<ValidationMessage> messages)
        {
            List<ValidationMessage> validationMessages = new List<ValidationMessage>();
            if (messages == null)
                messages = new List<ValidationMessage>();

            dbVisits.ToList().ForEach(x =>
            {
                bool result = x.IsAnyCollectionPropertyContainValue();
                if (result)
                    validationMessages.Add(_messages, x.Id, MessageType.VisitIsBeingUsed, x.VisitNumber + ":" + x.ReportNumber);
            });

            if (validationMessages.Count > 0)
                messages.AddRange(validationMessages);

            return messages.Count <= 0;
        }

        private bool IsRecordUpdateCountMatching(IList<DomainModel.Visit> filteredData,
                                                 IList<DbModel.Visit> dbVisits,
                                                 ref IList<ValidationMessage> messages)
        {
            List<ValidationMessage> validationMessages = new List<ValidationMessage>();
            if (messages == null)
                messages = new List<ValidationMessage>();

            if (filteredData != null && filteredData.Count > 0 && dbVisits != null && dbVisits.Count > 0)
            {
                var notMatchedRecords = filteredData.Where(x => !dbVisits.ToList().Any(x1 => x1.UpdateCount.ToInt() == x.UpdateCount && x1.Id == x.VisitId)).ToList();
                notMatchedRecords.ForEach(x =>
                {
                    validationMessages.Add(_messages, x, MessageType.VisitUpdateCountMismatch, string.Format("{0:D5} : {1}", x.VisitNumber, x.ReportNumber ?? "N/A"));
                });

                if (validationMessages.Count > 0)
                    messages.AddRange(validationMessages);
            }

            return messages?.Count <= 0;
        }

        public Response GetFinalVisitId(DomainModel.BaseVisit searchModel)
        {
            long? visitId = null;
            Exception exception = null;
            try
            {
                visitId = _repository.GetFinalVisitId(searchModel);
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString());
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, visitId, exception);
        }

        public Response GetVisitValidationData(DomainModel.BaseVisit searchModel)
        {
            DomainModel.VisitValidationData visitValidationData = new DomainModel.VisitValidationData();
            Exception exception = null;
            try
            {
                visitValidationData = _repository.GetVisitValidationData(searchModel);
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString());
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, visitValidationData, exception);
        }

        #region Private Methods

        private DbModel.Visit AssignVisitValues(DomainModel.Visit visits, DbModel.Visit dbVisit)
        {
            dbVisit.VisitStatus = visits.VisitStatus;
            dbVisit.ReportNumber = visits.VisitReportNumber;
            dbVisit.FromDate = visits.VisitStartDate;
            dbVisit.ToDate = visits.VisitEndDate;
            dbVisit.DatePeriod = visits.VisitDatePeriod;
            dbVisit.ReportSentToCustomerDate = visits.VisitReportSentToCustomerDate;
            //dbVisit.SupplierId = visits.VisitSupplier;
            dbVisit.IsFinalVisit = (string.IsNullOrEmpty(visits.FinalVisit) ? false : (visits.FinalVisit == "Yes" ? true : false));
            dbVisit.Reference1 = visits.VisitReference1;
            dbVisit.Reference2 = visits.VisitReference2;
            dbVisit.Reference3 = visits.VisitReference3;
            dbVisit.PercentageCompleted = visits.VisitCompletedPercentage;
            dbVisit.ExpectedCompleteDate = visits.VisitExpectedCompleteDate;
            dbVisit.NotificationReference = visits.VisitNotificationReference;
            //dbVisit.technicalspecilist

            return dbVisit;
        }

        public void AddVisitHistory(long visitId, string historyItemCode, string changedBy)
        {
            try
            {
                using (var tranScope = new TransactionScope())
                {
                    MasterData searchModel = new MasterData
                    {
                        MasterDataTypeId = (int)(MasterType.HistoryTable),
                        Code = historyItemCode.ToString()
                    };
                    var masterData = _masterRepository.Search(searchModel);
                    if (masterData != null && masterData.Count > 0)
                    {
                        _repository.AddVisitHistory(visitId, masterData[0].Id, changedBy);
                    }
                    tranScope.Complete();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString());
            }
        }

        #endregion
    }
}
