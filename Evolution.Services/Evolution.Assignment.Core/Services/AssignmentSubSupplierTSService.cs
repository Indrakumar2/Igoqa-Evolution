using AutoMapper;
using Evolution.Assignment.Domain.Interfaces.Assignments;
using Evolution.Assignment.Domain.Interfaces.Data;
using Evolution.Assignment.Domain.Interfaces.Validations;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Common.Models.Messages;
using Evolution.Common.Models.Responses;
using Evolution.DbRepository.Models.SqlDatabaseContext;
using Evolution.Logging.Interfaces;
using Evolution.SupplierPO.Domain.Interfaces.SupplierPO;
using Evolution.TechnicalSpecialist.Domain.Interfaces.TechnicalSpecialist;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.Assignment.Domain.Models.Assignments;

namespace Evolution.Assignment.Core.Services
{
    public class AssignmentSubSupplierTSService: IAssignmentSubSupplierTSService
    {
        private readonly IAppLogger<AssignmentSubSupplierTSService> _logger = null;
        private readonly IMapper _mapper = null;
        private readonly IAssignmentSubSupplierTSValidationService _validationService = null;
        private readonly JObject _messageDescriptions = null;
        private readonly ITechnicalSpecialistService _technicalSpecialistService = null;
        private readonly IAssignmentSubSupplerTSRepository _assignmentSubSupplierTSRepository = null;
        private readonly IAssignmentService _assignmentService = null;
        private readonly ISupplierPOSubSupplierService _subSupplierService = null;

        public AssignmentSubSupplierTSService(IAppLogger<AssignmentSubSupplierTSService> logger,
                                                        IAssignmentSubSupplerTSRepository assignmentSubSupplierTSRepository,
                                                        IAssignmentService assignmentService,
                                                        ISupplierPOSubSupplierService subSupplierService,
                                                        ITechnicalSpecialistService technicalSpecialistServices,
                                                        IAssignmentSubSupplierTSValidationService validationService,
                                                        IMapper mapper, JObject messageDescriptions)
        {
            _logger = logger;
            _mapper = mapper;
            _validationService = validationService;
            _messageDescriptions = messageDescriptions;
            _assignmentService = assignmentService;
            _assignmentSubSupplierTSRepository = assignmentSubSupplierTSRepository;
            _subSupplierService = subSupplierService;
            _technicalSpecialistService = technicalSpecialistServices;
        }


        #region Public Methods
        public Response Add(IList<DomainModel.AssignmentSubSupplierTechnicalSpecialist> assignmentSubSupplierTechnicalSpecialist, 
                            bool commitChange = true, 
                            bool isDbValidationRequired = true,
                            int? assignmentId=null)
        {
            if (assignmentId.HasValue)
                assignmentSubSupplierTechnicalSpecialist?.ToList().ForEach(x => { x.AssignmentId = assignmentId; });
            IList<DbModel.AssignmentSubSupplierTechnicalSpecialist> dbAssignmentSubSupplierTechnicalSpecialists = null;

            return AddAssignmentSubSupplierTechnicalSpecialist(assignmentSubSupplierTechnicalSpecialist, 
                                                                ref dbAssignmentSubSupplierTechnicalSpecialists, 
                                                                commitChange, 
                                                                isDbValidationRequired);
        }

        public Response Add(IList<DomainModel.AssignmentSubSupplierTechnicalSpecialist> AssignmentSubSupplierTechnicalSpecialist, 
                            ref IList<AssignmentSubSupplierTechnicalSpecialist> dbAssignmentSubSupplierTechnicalSpecialist, 
                            bool commitChange = true, 
                            bool isDbValidationRequired = true,
                            int? assignmentId=null)
        {
            if (assignmentId.HasValue)
                AssignmentSubSupplierTechnicalSpecialist?.ToList().ForEach(x => { x.AssignmentId = assignmentId; });

            return AddAssignmentSubSupplierTechnicalSpecialist(AssignmentSubSupplierTechnicalSpecialist, 
                                                               ref dbAssignmentSubSupplierTechnicalSpecialist, 
                                                               commitChange, 
                                                               isDbValidationRequired);
        }

        public Response Delete(IList<DomainModel.AssignmentSubSupplierTechnicalSpecialist> AssignmentSubSupplierTechnicalSpecialist, 
                               bool commitChange = true, 
                               bool isDbValidationRequired = true, 
                               int? assignmentId = null)
        {
            if (assignmentId.HasValue)
                AssignmentSubSupplierTechnicalSpecialist?.ToList().ForEach(x => { x.AssignmentId = assignmentId; });

            return this.RemoveAssignmentSubSupplierTechnicalSpecialist(AssignmentSubSupplierTechnicalSpecialist, 
                                                                        commitChange, 
                                                                        isDbValidationRequired);
        }

        public Response Get(DomainModel.AssignmentSubSupplierTechnicalSpecialist searchModel)
        {
            IList<DomainModel.AssignmentSubSupplierTechnicalSpecialist> result = null;
            Exception exception = null;
            try
            {
                result = this._assignmentSubSupplierTSRepository.Search(searchModel,
                                                                                    tech=>tech.TechnicalSpecialist,
                                                                                    sup => sup.AssignmentSubSupplier);
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), searchModel);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception, result?.Count);
        }

        public Response IsRecordValidForProcess(IList<DomainModel.AssignmentSubSupplierTechnicalSpecialist> assignmentSubSupplierTechnicalSpecialist, 
                                                ValidationType validationType)
        {
            IList<DbModel.AssignmentSubSupplierTechnicalSpecialist> dbAssignmentSubSupplierTechnicalSpecialists = null;
            IList<DbModel.TechnicalSpecialist> technicalSpecialists = null;
            return IsRecordValidForProcess(assignmentSubSupplierTechnicalSpecialist, 
                                            validationType, 
                                            ref dbAssignmentSubSupplierTechnicalSpecialists,
                                            ref technicalSpecialists);
        }

        public Response IsRecordValidForProcess(IList<DomainModel.AssignmentSubSupplierTechnicalSpecialist> assignmentSubSupplierTechnicalSpecialist, 
                                                ValidationType validationType, 
                                                ref IList<AssignmentSubSupplierTechnicalSpecialist> dbAssignmentSubSupplierTechnicalSpecialist,
                                                ref IList<DbModel.TechnicalSpecialist>technicalSpecialists)
        {
            IList<DomainModel.AssignmentSubSupplierTechnicalSpecialist> filteredAssignmentSubSupplierTechnicalSpecialist = null;
            
            return IsRecordValidForProcess(assignmentSubSupplierTechnicalSpecialist, 
                                           validationType, 
                                           ref filteredAssignmentSubSupplierTechnicalSpecialist, 
                                           ref dbAssignmentSubSupplierTechnicalSpecialist,
                                           ref technicalSpecialists);
        }

        public Response IsRecordValidForProcess(IList<DomainModel.AssignmentSubSupplierTechnicalSpecialist> assignmentSubSupplierTechnicalSpecialist, 
                                                ValidationType validationType, 
                                                IList<AssignmentSubSupplierTechnicalSpecialist> dbAssignmentSubSupplierTechnicalSpecialist,
                                                IList<DbModel.TechnicalSpecialist> technicalSpecialists)
        {
            return IsRecordValidForProcess(assignmentSubSupplierTechnicalSpecialist, 
                                            validationType, 
                                            ref dbAssignmentSubSupplierTechnicalSpecialist,
                                            ref technicalSpecialists);
        }

        public Response Modify(IList<DomainModel.AssignmentSubSupplierTechnicalSpecialist> assignmentSubSupplierTechnicalSpecialist, 
                                bool commitChange = true, 
                                bool isDbValidationRequired = true,
                                int?assignmentId=null)
        {
            if (assignmentId.HasValue)
                assignmentSubSupplierTechnicalSpecialist?.ToList().ForEach(x => { x.AssignmentId = assignmentId; });

            IList<DbModel.AssignmentSubSupplierTechnicalSpecialist> dbAssignmentSubSupplierTechnicalSpecialist = null;
            return UpdateAssignmentSubSupplierTechnicalSpecialist(assignmentSubSupplierTechnicalSpecialist, 
                                                                  ref dbAssignmentSubSupplierTechnicalSpecialist, 
                                                                  commitChange, 
                                                                  isDbValidationRequired);
        }

        public Response Modify(IList<Domain.Models.Assignments.AssignmentSubSupplierTechnicalSpecialist> assignmentSubSupplierTechnicalSpecialist, 
                                ref IList<AssignmentSubSupplierTechnicalSpecialist> dbAssignmentSubSupplierTechnicalSpecialist, 
                                bool commitChange = true, 
                                bool isDbValidationRequired = true,
                                int?assignmentId=null)
        {
            if (assignmentId.HasValue)
                assignmentSubSupplierTechnicalSpecialist?.ToList().ForEach(x => { x.AssignmentId = assignmentId; });

            return UpdateAssignmentSubSupplierTechnicalSpecialist(assignmentSubSupplierTechnicalSpecialist, 
                                                                  ref dbAssignmentSubSupplierTechnicalSpecialist, 
                                                                  commitChange, 
                                                                  isDbValidationRequired);
        }

        #endregion

        #region Private Methods
        private Response AddAssignmentSubSupplierTechnicalSpecialist(IList<DomainModel.AssignmentSubSupplierTechnicalSpecialist> assignmentSubSupplierTechnicalSpecialist, 
                                                                    ref IList<DbModel.AssignmentSubSupplierTechnicalSpecialist> dbAssignmentSubSupplierTechnicalSpecialist, 
                                                                    bool commitChange = true, 
                                                                    bool isDbValidationRequired = true)
        {
            Exception exception = null;
            IList<ValidationMessage> validationMessages = null;
            IList<DbModel.TechnicalSpecialist> technicalSpecialists = null;
            try
            {
                Response valdResponse = null;
                var recordToBeAdd = FilterRecord(assignmentSubSupplierTechnicalSpecialist, ValidationType.Add);

                if (isDbValidationRequired)
                    valdResponse = IsRecordValidForProcess(assignmentSubSupplierTechnicalSpecialist, 
                                                           ValidationType.Add, 
                                                           ref recordToBeAdd, 
                                                           ref dbAssignmentSubSupplierTechnicalSpecialist,
                                                           ref technicalSpecialists);


                if (!isDbValidationRequired || Convert.ToBoolean(valdResponse.Result))
                {
                    _assignmentSubSupplierTSRepository.AutoSave = false;
                    recordToBeAdd = recordToBeAdd.Select(x => { x.AssignmentSubSupplierTechnicalSpecialistId = null;   return x; }).ToList();
                    _assignmentSubSupplierTSRepository.Add(_mapper.Map<IList<DbModel.AssignmentSubSupplierTechnicalSpecialist>>(recordToBeAdd));


                    if (commitChange)
                        _assignmentSubSupplierTSRepository.ForceSave();
                }
                else
                    return valdResponse;
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), assignmentSubSupplierTechnicalSpecialist);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), null, exception);
        }

        private Response UpdateAssignmentSubSupplierTechnicalSpecialist(IList<DomainModel.AssignmentSubSupplierTechnicalSpecialist> assignmentSubSupplierTechnicalSpecialist, 
                                                                        ref IList<DbModel.AssignmentSubSupplierTechnicalSpecialist> dbAssignmentSubSupplierTechnicalSpecialist, 
                                                                        bool commitChange = true, 
                                                                        bool isDbValidationRequire = true)
        {
            Exception exception = null;
            IList<DomainModel.AssignmentSubSupplierTechnicalSpecialist> result = null;
            IList<ValidationMessage> validationMessages = null;
            IList<DbModel.TechnicalSpecialist> technicalSpecialists = null;
            try
            {
                Response valdResponse = null;
                var recordToBeModify = FilterRecord(assignmentSubSupplierTechnicalSpecialist, ValidationType.Update);
                var technicalSpecialist = assignmentSubSupplierTechnicalSpecialist.Where(x => (x.Epin.HasValue))?.Select(x => x.Epin).ToList();

                if (isDbValidationRequire)
                    valdResponse = IsRecordValidForProcess(assignmentSubSupplierTechnicalSpecialist, 
                                                            ValidationType.Update, 
                                                            ref recordToBeModify, 
                                                            ref dbAssignmentSubSupplierTechnicalSpecialist,
                                                            ref technicalSpecialists);

                else if (dbAssignmentSubSupplierTechnicalSpecialist?.Count <= 0)
                    dbAssignmentSubSupplierTechnicalSpecialist = GetAssignmentSubSupplierTechnicalspecialist(recordToBeModify);

                if (!isDbValidationRequire || (Convert.ToBoolean(valdResponse.Result) && dbAssignmentSubSupplierTechnicalSpecialist?.Count > 0))
                {
                    dbAssignmentSubSupplierTechnicalSpecialist.ToList().ForEach(dbSubSupplierTS =>
                    {
                        var assignmentSubSupplierTSToBeModify = recordToBeModify.FirstOrDefault(x => x.AssignmentSubSupplierTechnicalSpecialistId == dbSubSupplierTS.Id);
                        dbSubSupplierTS.TechnicalSpecialistId = technicalSpecialists.FirstOrDefault(x1 => x1.Pin == assignmentSubSupplierTSToBeModify.Epin).Id;
                        dbSubSupplierTS.LastModification = DateTime.UtcNow;
                        dbSubSupplierTS.UpdateCount = assignmentSubSupplierTSToBeModify.UpdateCount.CalculateUpdateCount();
                        dbSubSupplierTS.ModifiedBy = assignmentSubSupplierTSToBeModify.ModifiedBy;
                    });
                    _assignmentSubSupplierTSRepository.AutoSave = false;
                    _assignmentSubSupplierTSRepository.Update(dbAssignmentSubSupplierTechnicalSpecialist);
                    if (commitChange)
                        _assignmentSubSupplierTSRepository.ForceSave();
                }
                else
                    return valdResponse;
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), assignmentSubSupplierTechnicalSpecialist);
            }
            finally
            {
                _assignmentSubSupplierTSRepository.AutoSave = true;
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), result, exception);
        }

        private Response RemoveAssignmentSubSupplierTechnicalSpecialist(IList<DomainModel.AssignmentSubSupplierTechnicalSpecialist> assignmentSubSupplierTechnicalSpecialist, 
                                                                        bool commitChange, 
                                                                        bool isDbValidationRequire)
        {
            Exception exception = null;
            List<MessageDetail> errorMessages = null;
            List<ValidationMessage> validationMessages = null;
            IList<DbModel.AssignmentSubSupplierTechnicalSpecialist> dbAssignmentSubSupplierTechnicalSpecialist = null;
            IList<DbModel.TechnicalSpecialist> technicalSpecialists = null;
            
            try
            {
                errorMessages = new List<MessageDetail>();
                validationMessages = new List<ValidationMessage>();
                Response response = null;
                var recordToBeDelete = FilterRecord(assignmentSubSupplierTechnicalSpecialist, ValidationType.Delete);

                if (isDbValidationRequire)
                    response = IsRecordValidForProcess(assignmentSubSupplierTechnicalSpecialist, 
                                                        ValidationType.Delete, 
                                                        ref recordToBeDelete, 
                                                        ref dbAssignmentSubSupplierTechnicalSpecialist,
                                                        ref technicalSpecialists);

                if (!isDbValidationRequire || (Convert.ToBoolean(response.Result) && dbAssignmentSubSupplierTechnicalSpecialist?.Count > 0))
                {
                    _assignmentSubSupplierTSRepository.AutoSave = false;
                    _assignmentSubSupplierTSRepository.Delete(dbAssignmentSubSupplierTechnicalSpecialist);
                    if (commitChange)
                        _assignmentSubSupplierTSRepository.ForceSave();
                }
                else
                    return response;
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), assignmentSubSupplierTechnicalSpecialist);
            }
            finally
            {
                _assignmentSubSupplierTSRepository.AutoSave = true;
            }

            return new Response().ToPopulate(ResponseType.Success, null, errorMessages, validationMessages, null, exception);
        }
        private IList<DomainModel.AssignmentSubSupplierTechnicalSpecialist> FilterRecord(IList<DomainModel.AssignmentSubSupplierTechnicalSpecialist> assignmentSubSupplierTechnicalSpecialists, 
                                                                                        ValidationType filterType)
        {
            IList<DomainModel.AssignmentSubSupplierTechnicalSpecialist> filteredAssignmentSubSupplierTechnicalSpecialist = null;

            if (filterType == ValidationType.Add)
                filteredAssignmentSubSupplierTechnicalSpecialist = assignmentSubSupplierTechnicalSpecialists?.Where(x => x.RecordStatus.IsRecordStatusNew()).ToList();
            else if (filterType == ValidationType.Update)
                filteredAssignmentSubSupplierTechnicalSpecialist = assignmentSubSupplierTechnicalSpecialists?.Where(x => x.RecordStatus.IsRecordStatusModified()).ToList();
            else if (filterType == ValidationType.Delete)
                filteredAssignmentSubSupplierTechnicalSpecialist = assignmentSubSupplierTechnicalSpecialists?.Where(x => x.RecordStatus.IsRecordStatusDeleted()).ToList();

            return filteredAssignmentSubSupplierTechnicalSpecialist;
        }

        private Response IsRecordValidForProcess(IList<DomainModel.AssignmentSubSupplierTechnicalSpecialist> assignmentSubSupplierTechnicalSpecialists,
                                              ValidationType validationType,
                                              ref IList<DomainModel.AssignmentSubSupplierTechnicalSpecialist> filteredAssignmentSubSupplierTechnicalSpecialists,
                                              ref IList<DbModel.AssignmentSubSupplierTechnicalSpecialist> dbAssignmentSubSupplierTechnicalSpecialists,
                                              ref IList<DbModel.TechnicalSpecialist> technicalSpecialists)
        {
            Exception exception = null;
            bool result = true;
            IList<ValidationMessage> validationMessages = null;
            try
            {
                if (assignmentSubSupplierTechnicalSpecialists != null && assignmentSubSupplierTechnicalSpecialists.Count > 0)
                {
                    if (validationMessages == null)
                        validationMessages = new List<ValidationMessage>();

                    if (filteredAssignmentSubSupplierTechnicalSpecialists == null || filteredAssignmentSubSupplierTechnicalSpecialists.Count <= 0)
                        filteredAssignmentSubSupplierTechnicalSpecialists = FilterRecord(assignmentSubSupplierTechnicalSpecialists, validationType);

                    if (filteredAssignmentSubSupplierTechnicalSpecialists?.Count > 0 && IsValidPayload(filteredAssignmentSubSupplierTechnicalSpecialists, validationType, ref validationMessages))
                    {
                        IList<int> moduleNotExists = null;
                        var assignmentSubSupplierTechnicalSpecialistId = filteredAssignmentSubSupplierTechnicalSpecialists.Where(x => x.AssignmentSubSupplierTechnicalSpecialistId.HasValue).Select(x => x.AssignmentSubSupplierTechnicalSpecialistId.Value).Distinct().ToList();

                        if (dbAssignmentSubSupplierTechnicalSpecialists == null || dbAssignmentSubSupplierTechnicalSpecialists.Count <= 0)
                            dbAssignmentSubSupplierTechnicalSpecialists = GetAssignmentSubSupplierTechnicalspecialist(filteredAssignmentSubSupplierTechnicalSpecialists);

                        if (validationType == ValidationType.Delete || validationType == ValidationType.Update)
                        {
                            result = IsAssignmentSubSupplierTechnicalSpecialistExistInDb(assignmentSubSupplierTechnicalSpecialistId, 
                                                                                        dbAssignmentSubSupplierTechnicalSpecialists, 
                                                                                        ref moduleNotExists,
                                                                                        ref validationMessages);
                            if (result && validationType == ValidationType.Update)
                                result = IsRecordValidaForUpdate(filteredAssignmentSubSupplierTechnicalSpecialists, 
                                                                 dbAssignmentSubSupplierTechnicalSpecialists, 
                                                                 ref validationMessages,
                                                                 ref technicalSpecialists);
                        }
                        else if (validationType == ValidationType.Add)
                            result = IsRecordValidaForAdd(filteredAssignmentSubSupplierTechnicalSpecialists, 
                                                          dbAssignmentSubSupplierTechnicalSpecialists, 
                                                          ref validationMessages,
                                                          ref technicalSpecialists);
                    }
                    else
                        result = false;
                }
            }
            catch (Exception ex)
            {
                result = false;
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), assignmentSubSupplierTechnicalSpecialists);
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), result, exception);
        }

        private bool IsValidPayload(IList<DomainModel.AssignmentSubSupplierTechnicalSpecialist> assignmenSubSupplierTechnicalSpecialists,
                                    ValidationType validationType,
                                    ref IList<ValidationMessage> validationMessages)
        {
            var messages = new List<ValidationMessage>();
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            var validationResults = _validationService.Validate(JsonConvert.SerializeObject(assignmenSubSupplierTechnicalSpecialists), validationType);
            if (validationResults?.Count > 0)
                messages.Add(_messageDescriptions, ModuleType.Assignment, validationResults);

            if (validationResults.Count > 0)
                validationMessages.AddRange(messages);

            return validationMessages?.Count <= 0;
        }

        private IList<DbModel.AssignmentSubSupplierTechnicalSpecialist> GetAssignmentSubSupplierTechnicalspecialist(IList<DomainModel.AssignmentSubSupplierTechnicalSpecialist> assignmentSubSupplierTechnicalSpecialist)
        {
            IList<DbModel.AssignmentSubSupplierTechnicalSpecialist> dbAssignmentSubSupplierTechnicalSpecialist = null;
            if (assignmentSubSupplierTechnicalSpecialist?.Count > 0)
            {
                var assignmentSubSupplierTechnicalSpecialistId = assignmentSubSupplierTechnicalSpecialist.Select(x => x.AssignmentSubSupplierTechnicalSpecialistId).Distinct().ToList();
                dbAssignmentSubSupplierTechnicalSpecialist = _assignmentSubSupplierTSRepository.FindBy(x => assignmentSubSupplierTechnicalSpecialistId.Contains(x.Id)).ToList();
            }

            return dbAssignmentSubSupplierTechnicalSpecialist;
        }

        private bool IsAssignmentSubSupplierTechnicalSpecialistExistInDb(IList<int> assignmentSubSupplierTechnicalSpecialistIds,
                                                                         IList<DbModel.AssignmentSubSupplierTechnicalSpecialist> dbAssignmentSubSupplierTechnicalSpecialist,
                                                                         ref IList<int> assignmentSubSupplierTechnicalSpecialistNotExists,
                                                                         ref IList<ValidationMessage> validationMessages)
        {
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            if (dbAssignmentSubSupplierTechnicalSpecialist == null)
                dbAssignmentSubSupplierTechnicalSpecialist = new List<DbModel.AssignmentSubSupplierTechnicalSpecialist>();

            var validMessages = validationMessages;

            if (assignmentSubSupplierTechnicalSpecialistIds?.Count > 0)
            {
                assignmentSubSupplierTechnicalSpecialistNotExists = assignmentSubSupplierTechnicalSpecialistIds.Where(x => !dbAssignmentSubSupplierTechnicalSpecialist.Select(x1 => x1.Id).ToList().Contains(x)).ToList();
                assignmentSubSupplierTechnicalSpecialistNotExists?.ToList().ForEach(x =>
                {
                    validMessages.Add(_messageDescriptions, x, MessageType.AssignmentSubSupplierTechnicalSpecialistNotExists, x);
                });
            }

            if (validMessages.Count > 0)
                validationMessages = validMessages;

            return validationMessages.Count <= 0;
        }

        private bool IsRecordValidaForUpdate(IList<DomainModel.AssignmentSubSupplierTechnicalSpecialist> assignmentSubSupplierTechnicalSpecialist, 
                                             IList<DbModel.AssignmentSubSupplierTechnicalSpecialist> dbAssignmentSubSupplierTechnicalSpecialist, 
                                             ref IList<ValidationMessage> validationMessages,
                                             ref IList<DbModel.TechnicalSpecialist> technicalSpecialists)
        {
            IList<ValidationMessage> messages = new List<ValidationMessage>();
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();
            IList<DbModel.Assignment> dbAssignments = null;
            IList<DbModel.SupplierPurchaseOrderSubSupplier> supplierPurchaseOrderSubSuppliers = null;
            IList<DbModel.Supplier> dbSupplier = null;

            assignmentSubSupplierTechnicalSpecialist.Select(x => x.AssignmentSubSupplierTechnicalSpecialistId).ToList()
                   .ForEach(x1 =>
                   {
                       var isExist = dbAssignmentSubSupplierTechnicalSpecialist.Any(x2 => x2.Id == x1);
                       if (!isExist)
                           messages.Add(_messageDescriptions, x1, MessageType.AssignmentSubSupplierTechnicalSpecialistNotExists, x1);
                   });

            if (messages?.Count <= 0)
            {
                if (IsRecordUpdateCountMatching(assignmentSubSupplierTechnicalSpecialist, dbAssignmentSubSupplierTechnicalSpecialist, ref messages))
                {
                    var assignmentIds = assignmentSubSupplierTechnicalSpecialist.Where(x => x.AssignmentId > 0).Select(u => (int)u.AssignmentId).Distinct().ToList();
                    var subSupplierIds = assignmentSubSupplierTechnicalSpecialist.Where(x => x.SubSupplierId > 0).Select(u => (int)u.SubSupplierId).Distinct().ToList();
                    var technicalSpecilaistIds = assignmentSubSupplierTechnicalSpecialist.Where(x => x.Epin > 0).Select(u => (int)u.Epin).Distinct().ToList();
                    if (_assignmentService.IsValidAssignment(assignmentIds, ref dbAssignments, ref messages)
                        && _subSupplierService.IsValidSubSupplier(subSupplierIds, ref dbAssignments, ref supplierPurchaseOrderSubSuppliers,ref dbSupplier, ref messages))
                            IsUniqueAssignmentTechnicalSpecialist(assignmentSubSupplierTechnicalSpecialist, ValidationType.Update, dbAssignmentSubSupplierTechnicalSpecialist, ref messages);
                            
                }
            }

            if (messages.Count > 0)
                validationMessages.AddRange(messages);

            return messages?.Count <= 0;
        }
        private bool IsRecordUpdateCountMatching(IList<DomainModel.AssignmentSubSupplierTechnicalSpecialist> assignmentSubSupplierTechnicalSpecialist, 
                                                 IList<DbModel.AssignmentSubSupplierTechnicalSpecialist> dbAssignmentSubSupplierTechnicalSpecialist, 
                                                 ref IList<ValidationMessage> validationMessages)
        {
            List<ValidationMessage> messages = new List<ValidationMessage>();
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            var notMatchedRecords = assignmentSubSupplierTechnicalSpecialist.Where(x => !dbAssignmentSubSupplierTechnicalSpecialist.ToList().Any(x1 => x1.UpdateCount.ToInt() == x.UpdateCount.ToInt() && x1.Id == x.AssignmentSubSupplierTechnicalSpecialistId)).ToList();
            notMatchedRecords.ForEach(x =>
            {
                messages.Add(_messageDescriptions, x.AssignmentSubSupplierTechnicalSpecialistId, MessageType.AssignmentSubSupplierTechnicalSpecialistNotExists, x.AssignmentSubSupplierTechnicalSpecialistId);
            });

            if (messages.Count > 0)
                validationMessages.AddRange(messages);

            return messages?.Count <= 0;
        }
       
        //private bool IsValidTechnicalSpecialist(IList<DomainModel.AssignmentSubSupplierTechnicalSpecialist> assignmentSubSupplierTechnicalSpecialists, ref IList<ValidationMessage> validationMessages)
        //{
        //    List<ValidationMessage> messages = new List<ValidationMessage>();
        //    if (validationMessages == null)
        //        validationMessages = new List<ValidationMessage>();

        //    var Epin = assignmentSubSupplierTechnicalSpecialists.Where(x => x.Epin.HasValue).Select(x => x.Epin.Value).Distinct().ToList();
        //    var dbEpin = _technicalSpecialistRepository.FindBy(x => Epin.Contains((int)x.Pin)).Select(x => x.Pin);
        //    var invalidContractScheduleIds = Epin.Where(x => !dbEpin.Any(x1 => x1 == x));
        //    invalidContractScheduleIds.ToList().ForEach(x =>
        //    {
        //        messages.Add(_messageDescriptions, x, MessageType.AssignmentSubSupplierTechnicalSpecialistInvalidTechnicalSpecialist, x);
        //    });

        //    if (messages.Count > 0)
        //        validationMessages.AddRange(messages);

        //    return messages?.Count <= 0;
        //}
         
        private bool IsUniqueAssignmentTechnicalSpecialist(IList<DomainModel.AssignmentSubSupplierTechnicalSpecialist> assignmentSubSupplierTechnicalSpecialists, 
                                                           ValidationType validationType, 
                                                           IList<DbModel.AssignmentSubSupplierTechnicalSpecialist> dbAssignmentSubSupplierTechnicalSpecialists, 
                                                           ref IList<ValidationMessage> validationMessages)
        {

            return false;
            //List<ValidationMessage> messages = new List<ValidationMessage>();
            //if (validationMessages == null)
            //    validationMessages = new List<ValidationMessage>();
            //if (validationType == ValidationType.Update || validationType == ValidationType.Delete)
            //{
            //    var assingmentSubSupplierTechnicalSpecialists = assignmentSubSupplierTechnicalSpecialists.Select(x => new { x.Epin, x.AssignmentId, x.AssignmentSubSupplierTechnicalSpecialistId,x.SubSupplierId }).ToList();
            //    var duplicateRecords = _assignmentSubSupplierTSRepository.FindBy(x => assingmentSubSupplierTechnicalSpecialists.Any(x1 => x1.Epin == x.TechnicalSpecialist.Pin && x1.AssignmentId == x.AssignmentId &&x1.SubSupplierId==x.SubSupplierId &&x1.AssignmentSubSupplierTechnicalSpecialistId != x.Id));
            //    duplicateRecords.ToList().ForEach(x =>
            //    {
            //        messages.Add(_messageDescriptions, x.TechnicalSpecialistId, MessageType.AssignmentSubSupplierTechnicalSpecialistDuplicateRecord, x.TechnicalSpecialistId);
            //    });
            //}

            //if (validationType == ValidationType.Add)
            //{
            //    var assingmentSubSupplierTechnicalSpecialists = assignmentSubSupplierTechnicalSpecialists.Select(x => new { x.Epin, x.AssignmentId, x.AssignmentSubSupplierTechnicalSpecialistId,x.SubSupplierId }).ToList();
            //    var duplicateRecords = _assignmentSubSupplierTSRepository.FindBy(x => assingmentSubSupplierTechnicalSpecialists.Any(x1 => x1.Epin == x.TechnicalSpecialist.Pin && x1.AssignmentId == x.AssignmentId && x1.SubSupplierId==x.SubSupplierId));
            //    duplicateRecords.ToList().ForEach(x =>
            //    {
            //        messages.Add(_messageDescriptions, x.TechnicalSpecialistId, MessageType.AssignmentSubSupplierTechnicalSpecialistDuplicateRecord, x.TechnicalSpecialistId);
            //    });
            //}

            //  if (messages.Count > 0)
            //    validationMessages.AddRange(messages);

            //return messages?.Count <= 0;
        }
       
        private bool IsRecordValidaForAdd(IList<DomainModel.AssignmentSubSupplierTechnicalSpecialist> assignmentSubSupplierTechnicalSpecialist, 
                                          IList<DbModel.AssignmentSubSupplierTechnicalSpecialist> dbAssignmentSubSupplierTechnicalSpecialist, 
                                          ref IList<ValidationMessage> validationMessages,ref IList<DbModel.TechnicalSpecialist> technicalSpecialists)
        {
           
            IList<ValidationMessage> messages = new List<ValidationMessage>();
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();
            IList<DbModel.Assignment> dbAssignments = null;
            IList<DbModel.SupplierPurchaseOrderSubSupplier> supplierPurchaseOrderSubSuppliers = null;
            IList<DbModel.Supplier> dbSuppliers = null;
            var assignmentIds = assignmentSubSupplierTechnicalSpecialist.Where(x => x.AssignmentId > 0).Select(u => (int)u.AssignmentId).Distinct().ToList();
            var subSupplierIds = assignmentSubSupplierTechnicalSpecialist.Where(x => x.SubSupplierId > 0).Select(u => (int)u.SubSupplierId).Distinct().ToList();
            var technicalSpecialistIds = assignmentSubSupplierTechnicalSpecialist.Where(x => x.Epin > 0).Select(u => (int)u.Epin).Distinct().ToList();
            if (_assignmentService.IsValidAssignment(assignmentIds, ref dbAssignments, ref messages)
                && _subSupplierService.IsValidSubSupplier(subSupplierIds, ref dbAssignments, ref supplierPurchaseOrderSubSuppliers,ref dbSuppliers, ref messages))
                //&& _technicalSpecialistService.IsValidTechnicalSpecialist(technicalSpecialistIds, ref technicalSpecialists, ref messages))
                    IsUniqueAssignmentTechnicalSpecialist(assignmentSubSupplierTechnicalSpecialist, ValidationType.Add, dbAssignmentSubSupplierTechnicalSpecialist, ref messages);

            if (messages.Count > 0)
                validationMessages.AddRange(messages);

            return messages?.Count <= 0;
        }

        #endregion
    }
}
