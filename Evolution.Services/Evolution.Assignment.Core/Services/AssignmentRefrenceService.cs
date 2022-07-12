using AutoMapper;
using Evolution.Assignment.Domain.Interfaces.Assignments;
using Evolution.Assignment.Domain.Interfaces.Data;
using Evolution.Assignment.Domain.Interfaces.Validations;
using Evolution.Assignment.Domain.Models.Assignments;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Common.Models.Messages;
using Evolution.Common.Models.Responses;
using Evolution.DbRepository.Interfaces.Master;
using Evolution.Logging.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.Assignment.Domain.Models.Assignments;

namespace Evolution.Assignment.Core.Services
{
    public class AssignmentRefrenceService : IAssignmentReferenceService
    {
        private readonly IAppLogger<AssignmentRefrenceService> _logger = null;
        private readonly IMapper _mapper = null;
        private readonly IAssignmentReferenceTypeRepository _assignmentRefrenceRepository = null;
        private readonly JObject _messageDescriptions = null;
        private readonly IAssignmentReferenceTypeValidationService _assignmentRefrenceValidationService = null;
        private readonly IDataRepository _dataRepository = null;
        private readonly IAssignmentService _assignmentService = null;

        public AssignmentRefrenceService(IAppLogger<AssignmentRefrenceService> logger,
                                         IMapper mapper,
                                         IAssignmentReferenceTypeRepository assignmentRefrenceRepository,
                                         JObject messageDescriptions,
                                         IAssignmentReferenceTypeValidationService assignmentRefrenceValidationService,
                                         IAssignmentService assignmentService,
                                         IDataRepository dataRepository)
        {
            _logger = logger;
            _mapper = mapper;
            _assignmentRefrenceRepository = assignmentRefrenceRepository;
            _messageDescriptions = messageDescriptions;
            _assignmentRefrenceValidationService = assignmentRefrenceValidationService;
            _assignmentService = assignmentService;
            _dataRepository = dataRepository;
        }

        #region Public Methods


        #region add
        public Response Add(IList<AssignmentReferenceType> assignmentReferenceTypes,
                            bool commitChange = true,
                            bool isDbValidationRequired = true,
                            int? assignmentIds = null)
        {
            IList<DbModel.Data> dbReferenceType = null;
            IList<DbModel.Assignment> dbAssignment = null;
            if (assignmentIds.HasValue)
                assignmentReferenceTypes?.ToList().ForEach(x => { x.AssignmentId = assignmentIds; });
            IList<DbModel.AssignmentReference> dbAssignmentRefrenceType = null;
            return AddAssignmentRefrence(assignmentReferenceTypes,
                                         ref dbAssignmentRefrenceType,
                                         ref dbAssignment,
                                         ref dbReferenceType,
                                         commitChange,
                                         isDbValidationRequired);
        }

        public Response Add(IList<AssignmentReferenceType> assignmentReferenceTypes,
                            ref IList<DbModel.AssignmentReference> dbAssignmentReferenceTypes,
                            ref IList<DbModel.Assignment> dbAssignment,
                            ref IList<DbModel.Data> dbReferenceType,
                            bool commitChange = true,
                            bool isDbValidationRequired = true,
                            int? assignmentIds = null)
        {
            if (assignmentIds.HasValue && !isDbValidationRequired)
                assignmentReferenceTypes?.ToList().ForEach(x => { x.AssignmentId = assignmentIds; });
            return AddAssignmentRefrence(assignmentReferenceTypes,
                                         ref dbAssignmentReferenceTypes,
                                         ref dbAssignment,
                                         ref dbReferenceType,
                                         commitChange,
                                         isDbValidationRequired);
        }
        #endregion

        #region Delete
        public Response Delete(IList<AssignmentReferenceType> assignmentReferenceTypes,
                               bool commitChange = true,
                               bool isDbValidationRequired = true,
                               int? assignmentIds = null)
        {
            IList<DbModel.Assignment> dbAssignment = null;
            IList<DbModel.Data> dbReferenceType = null;
            IList<DbModel.AssignmentReference> dbAssignmentReferenceTypes = null;
            if (assignmentIds.HasValue)
                assignmentReferenceTypes?.ToList().ForEach(x => { x.AssignmentId = assignmentIds; });
            return this.RemoveAssignmentReference(assignmentReferenceTypes,
                                                  ref dbAssignmentReferenceTypes,
                                                  ref dbAssignment,
                                                  ref dbReferenceType,
                                                  commitChange,
                                                  isDbValidationRequired);
        }

        public Response Delete(IList<AssignmentReferenceType> assignmentReferenceTypes,
                               ref IList<DbModel.AssignmentReference> dbAssignmentReferenceTypes,
                               ref IList<DbModel.Assignment> dbAssignment,
                               ref IList<DbModel.Data> dbReferenceType,
                               bool commitChange = true,
                               bool isDbValidationRequired = true,
                               int? assignmentIds = null)
        {
            if (assignmentIds.HasValue && !isDbValidationRequired)
                assignmentReferenceTypes?.ToList().ForEach(x => { x.AssignmentId = assignmentIds; });
            return this.RemoveAssignmentReference(assignmentReferenceTypes,
                                                  ref dbAssignmentReferenceTypes,
                                                  ref dbAssignment,
                                                  ref dbReferenceType,
                                                  commitChange,
                                                  isDbValidationRequired);
        }
        #endregion

        #region Get
        public Response Get(AssignmentReferenceType searchModel)
        {
            IList<DomainModel.AssignmentReferenceType> result = null;
            Exception exception = null;
            try
            {
                using (var tranScope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions {IsolationLevel = IsolationLevel.ReadUncommitted}))
                {
                    result = this._assignmentRefrenceRepository.Search(searchModel);
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
        #endregion

        #region Modify
        public Response Modify(IList<AssignmentReferenceType> assignmentReferenceTypes,
                                bool commitChange = true,
                                bool isDbValidationRequired = true,
                                int? assignmentIds = null)
        {
            IList<DbModel.Data> dbReferenceType = null;
            IList<DbModel.Assignment> dbAssignment = null;
            if (assignmentIds.HasValue)
                assignmentReferenceTypes?.ToList().ForEach(x => { x.AssignmentId = assignmentIds; });
            IList<DbModel.AssignmentReference> dbAssignmentReference = null;
            return UpdateAssignmentReference(assignmentReferenceTypes,
                                             ref dbAssignmentReference,
                                             ref dbAssignment,
                                             ref dbReferenceType,
                                             commitChange,
                                             isDbValidationRequired);
        }

        public Response Modify(IList<AssignmentReferenceType> assignmentReferenceTypes,
                               ref IList<DbModel.AssignmentReference> dbAssignmentReferenceTypes,
                               ref IList<DbModel.Assignment> dbAssignment,
                               ref IList<DbModel.Data> dbReferenceType,
                               bool commitChange = true,
                               bool isDbValidationRequired = true,
                               int? assignmentIds = null)
        {
            if (assignmentIds.HasValue && !isDbValidationRequired)
                assignmentReferenceTypes?.ToList().ForEach(x => { x.AssignmentId = assignmentIds; });
            return UpdateAssignmentReference(assignmentReferenceTypes,
                                             ref dbAssignmentReferenceTypes,
                                             ref dbAssignment,
                                             ref dbReferenceType,
                                             commitChange,
                                             isDbValidationRequired);
        }
        #endregion

        #region Validation Check

        public Response IsRecordValidForProcess(IList<AssignmentReferenceType> assignmentReferenceTypes,
                                                ValidationType validationType)
        {
            IList<DbModel.AssignmentReference> dbAssignementReference = null;
            IList<DbModel.Assignment> dbAssignment = null;
            IList<DbModel.Data> dbReferenceType = null;
            return IsRecordValidForProcess(assignmentReferenceTypes,
                                            validationType,
                                            ref dbAssignementReference,
                                            ref dbAssignment,
                                            ref dbReferenceType);
        }

        public Response IsRecordValidForProcess(IList<AssignmentReferenceType> assignmentReferenceTypes,
                                                ValidationType validationType,
                                                ref IList<DbModel.AssignmentReference> dbAssignmentReferenceTypes,
                                                ref IList<DbModel.Assignment> dbAssignment,
                                                ref IList<DbModel.Data> dbReferenceType)
        {
            IList<DomainModel.AssignmentReferenceType> filteredAssignemntReference = null;
            return IsRecordValidForProcess(assignmentReferenceTypes,
                                            validationType,
                                            ref filteredAssignemntReference,
                                            ref dbAssignmentReferenceTypes,
                                            ref dbAssignment,
                                            ref dbReferenceType);
        }

        public Response IsRecordValidForProcess(IList<AssignmentReferenceType> assignmentReferenceTypes,
                                                ValidationType validationType,
                                                IList<DbModel.AssignmentReference> dbAssignmentReferenceTypes,
                                                ref IList<DbModel.Assignment> dbAssignment,
                                                ref IList<DbModel.Data> dbReferenceType)
        {
            return IsRecordValidForProcess(assignmentReferenceTypes,
                                            validationType,
                                            ref dbAssignmentReferenceTypes,
                                            ref dbAssignment,
                                            ref dbReferenceType);
        }
        #endregion

        #endregion

        #region Private Region
        private Response AddAssignmentRefrence(IList<DomainModel.AssignmentReferenceType> assignmentRefrenceTypes,
                                                ref IList<DbModel.AssignmentReference> dbAssignmentRefrence,
                                                ref IList<DbModel.Assignment> dbAssignment,
                                                ref IList<DbModel.Data> dbReferenceType,
                                                bool commitChange = true,
                                                bool isDbValidationRequired = true)
        {
            Exception exception = null;
            IList<ValidationMessage> validationMessages = null;
            IList<DbModel.Data> dbReferenceTypes = null;
            try
            {
                Response valdResponse = null;
                var recordToBeAdd = FilterRecord(assignmentRefrenceTypes, ValidationType.Add);
                if (isDbValidationRequired)
                    valdResponse = IsRecordValidForProcess(assignmentRefrenceTypes,
                                                            ValidationType.Add,
                                                            ref recordToBeAdd,
                                                            ref dbAssignmentRefrence,
                                                            ref dbAssignment,
                                                            ref dbReferenceType);

                if (recordToBeAdd?.Count > 0)
                {
                    if (!isDbValidationRequired || Convert.ToBoolean(valdResponse.Result))
                    {
                        _assignmentRefrenceRepository.AutoSave = false;
                        dbReferenceTypes = dbReferenceType;
                        dbAssignmentRefrence = _mapper.Map<IList<DbModel.AssignmentReference>>(recordToBeAdd, opt =>
                          {
                              opt.Items["isAssignId"] = false;
                              opt.Items["ReferenceTypes"] = dbReferenceTypes;
                          });
                        _assignmentRefrenceRepository.Add(dbAssignmentRefrence);
                        if (commitChange)
                            _assignmentRefrenceRepository.ForceSave();
                    }
                    else
                        return valdResponse;
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), assignmentRefrenceTypes);
            }
           // finally { _assignmentRefrenceRepository.Dispose(); }
            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), null, exception);
        }

        private Response UpdateAssignmentReference(IList<DomainModel.AssignmentReferenceType> assignmentReferences,
                                                     ref IList<DbModel.AssignmentReference> dbAssignmentRefrence,
                                                     ref IList<DbModel.Assignment> dbAssignment,
                                                     ref IList<DbModel.Data> dbReferenceType,
                                                     bool commitChange = true,
                                                     bool isDbValidationRequire = true)
        {
            Exception exception = null;
            IList<DomainModel.AssignmentTechnicalSpecialist> result = null;
            IList<DbModel.Data> dbReferenceTypes = null;
            IList<ValidationMessage> validationMessages = null;
            try
            {
                Response valdResponse = null;
                var recordToBeModify = FilterRecord(assignmentReferences, ValidationType.Update);
                if (isDbValidationRequire)
                    valdResponse = IsRecordValidForProcess(assignmentReferences,
                                                            ValidationType.Update,
                                                            ref recordToBeModify,
                                                            ref dbAssignmentRefrence,
                                                            ref dbAssignment,
                                                            ref dbReferenceType);
                else if (dbAssignmentRefrence?.Count <= 0)
                    dbAssignmentRefrence = GetAssignmentRefrenceType(recordToBeModify);

                if (recordToBeModify?.Count > 0)
                {
                    if (!isDbValidationRequire || (Convert.ToBoolean(valdResponse.Result)) && dbAssignmentRefrence?.Count > 0)
                    {
                        dbReferenceTypes = dbReferenceType;
                        dbAssignmentRefrence.ToList().ForEach(dbAssignmentRefrenceType =>
                        {
                            var assignmentTechnicalSpecialistToBeModify = recordToBeModify.FirstOrDefault(x => x.AssignmentReferenceTypeId == dbAssignmentRefrenceType.Id);
                            if (assignmentTechnicalSpecialistToBeModify != null)
                            {
                                dbAssignmentRefrenceType.AssignmentReferenceTypeId = dbReferenceTypes.FirstOrDefault(x1 => x1.Name == assignmentTechnicalSpecialistToBeModify.ReferenceType).Id; //ToDo refrencetype
                                dbAssignmentRefrenceType.AssignmentId = (int)assignmentTechnicalSpecialistToBeModify.AssignmentId;
                                dbAssignmentRefrenceType.ReferenceValue = assignmentTechnicalSpecialistToBeModify.ReferenceValue;
                                dbAssignmentRefrenceType.LastModification = DateTime.UtcNow;
                                dbAssignmentRefrenceType.UpdateCount = assignmentTechnicalSpecialistToBeModify.UpdateCount.CalculateUpdateCount();
                                dbAssignmentRefrenceType.ModifiedBy = assignmentTechnicalSpecialistToBeModify.ModifiedBy;
                            }
                        });
                        _assignmentRefrenceRepository.AutoSave = false;
                        _assignmentRefrenceRepository.Update(dbAssignmentRefrence);
                        if (commitChange)
                            _assignmentRefrenceRepository.ForceSave();
                    }
                    else
                        return valdResponse;
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), assignmentReferences);
            }
            finally
            {
                _assignmentRefrenceRepository.AutoSave = true;
              //  _assignmentRefrenceRepository.Dispose();
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), result, exception);
        }

        private Response RemoveAssignmentReference(IList<DomainModel.AssignmentReferenceType> assignmentRefrenceType,
                                                   ref IList<DbModel.AssignmentReference> dbAssignmentReferenceTypes,
                                                   ref IList<DbModel.Assignment> dbAssignment,
                                                   ref IList<DbModel.Data> dbReferenceType,
                                                   bool commitChange,
                                                   bool isDbValidationRequire)
        {
            Exception exception = null;
            List<MessageDetail> errorMessages = null;
            List<ValidationMessage> validationMessages = null;
            //IList<DbModel.AssignmentReference> dbAssignmentReferenceType = null;
            try
            {
                errorMessages = new List<MessageDetail>();
                validationMessages = new List<ValidationMessage>();
                Response response = null;
                var recordToBeDelete = FilterRecord(assignmentRefrenceType, ValidationType.Delete);

                if (isDbValidationRequire)
                    response = IsRecordValidForProcess(assignmentRefrenceType,
                                                       ValidationType.Delete,
                                                       ref recordToBeDelete,
                                                       ref dbAssignmentReferenceTypes,
                                                       ref dbAssignment,
                                                       ref dbReferenceType);

                if (recordToBeDelete?.Count > 0)
                {
                    if (!isDbValidationRequire || (Convert.ToBoolean(response.Result)) && dbAssignmentReferenceTypes?.Count > 0)
                    {
                        _assignmentRefrenceRepository.AutoSave = false;
                        _assignmentRefrenceRepository.Delete(dbAssignmentReferenceTypes);
                        if (commitChange)
                            _assignmentRefrenceRepository.ForceSave();
                    }
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), assignmentRefrenceType);
            }
            finally
            {
                _assignmentRefrenceRepository.AutoSave = true;
               // _assignmentRefrenceRepository.Dispose();
            }

            return new Response().ToPopulate(ResponseType.Success, null, errorMessages, validationMessages, null, exception);
        }

        private IList<DomainModel.AssignmentReferenceType> FilterRecord(IList<DomainModel.AssignmentReferenceType> assignmentRefrenceTypes,
                                                                        ValidationType filterType)
        {
            IList<DomainModel.AssignmentReferenceType> filteredAssignmentRefrenceType = null;

            if (filterType == ValidationType.Add)
                filteredAssignmentRefrenceType = assignmentRefrenceTypes?.Where(x => x.RecordStatus.IsRecordStatusNew()).ToList();
            else if (filterType == ValidationType.Update)
                filteredAssignmentRefrenceType = assignmentRefrenceTypes?.Where(x => x.RecordStatus.IsRecordStatusModified()).ToList();
            else if (filterType == ValidationType.Delete)
                filteredAssignmentRefrenceType = assignmentRefrenceTypes?.Where(x => x.RecordStatus.IsRecordStatusDeleted()).ToList();

            return filteredAssignmentRefrenceType;
        }

        private Response IsRecordValidForProcess(IList<DomainModel.AssignmentReferenceType> assignmentRefrenceTypes,
                                                ValidationType validationType,
                                                ref IList<DomainModel.AssignmentReferenceType> filteredAssignmentRefrenceTypes,
                                                ref IList<DbModel.AssignmentReference> dbAssignmentRefrence,
                                                ref IList<DbModel.Assignment> dbAssignment,
                                                ref IList<DbModel.Data> dbReferenceType)
        {
            Exception exception = null;
            bool result = true;
            IList<ValidationMessage> validationMessages = null;
            try
            {
                if (assignmentRefrenceTypes != null && assignmentRefrenceTypes.Count > 0)
                {
                    if (validationMessages == null)
                        validationMessages = new List<ValidationMessage>();

                    if (filteredAssignmentRefrenceTypes == null || filteredAssignmentRefrenceTypes.Count <= 0)
                        filteredAssignmentRefrenceTypes = FilterRecord(assignmentRefrenceTypes, validationType);

                    if (filteredAssignmentRefrenceTypes != null && filteredAssignmentRefrenceTypes?.Count > 0)
                    {
                        result = IsValidPayload(filteredAssignmentRefrenceTypes,
                                                validationType,
                                                ref validationMessages);
                        if (filteredAssignmentRefrenceTypes?.Count > 0 && result)
                        {
                            IList<int> moduleNotExists = null;
                            var assignmentRefrenceTypeId = filteredAssignmentRefrenceTypes.Where(x => x.AssignmentReferenceTypeId.HasValue).Select(x => x.AssignmentReferenceTypeId.Value).Distinct().ToList();

                            if ((dbAssignmentRefrence == null || dbAssignmentRefrence.Count <= 0)  && validationType != ValidationType.Add)
                                dbAssignmentRefrence = GetAssignmentRefrenceType(filteredAssignmentRefrenceTypes);

                            if (validationType == ValidationType.Delete || validationType == ValidationType.Update)
                            {
                                result = IsAssignmentRefrenceExistInDb(assignmentRefrenceTypeId,
                                                                        dbAssignmentRefrence,
                                                                        ref moduleNotExists,
                                                                        ref validationMessages);
                                if (result && validationType == ValidationType.Update)
                                    result = IsRecordValidaForUpdate(filteredAssignmentRefrenceTypes,
                                                                    dbAssignmentRefrence,
                                                                    ref dbAssignment,
                                                                    ref dbReferenceType,
                                                                    ref validationMessages);
                            }
                            else if (validationType == ValidationType.Add)
                                result = IsRecordValidaForAdd(filteredAssignmentRefrenceTypes,
                                                                dbAssignmentRefrence,
                                                                ref dbAssignment,
                                                                ref dbReferenceType,
                                                                ref validationMessages);
                        }
                        else
                            result = false;
                    }

                }
            }
            catch (Exception ex)
            {
                result = false;
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), assignmentRefrenceTypes);
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), result, exception);
        }
        private bool IsValidPayload(IList<DomainModel.AssignmentReferenceType> assignmentRefrenceTypes,
                                    ValidationType validationType,
                                    ref IList<ValidationMessage> validationMessages)
        {
            var messages = new List<ValidationMessage>();
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            var validationResults = _assignmentRefrenceValidationService.Validate(JsonConvert.SerializeObject(assignmentRefrenceTypes), validationType);
            if (validationResults?.Count > 0)
                messages.Add(_messageDescriptions, ModuleType.Assignment, validationResults);

            if (validationResults.Count > 0)
                validationMessages.AddRange(messages);

            return validationMessages?.Count <= 0;
        }

        private IList<DbModel.AssignmentReference> GetAssignmentRefrenceType(IList<DomainModel.AssignmentReferenceType> assignmentRefrenceType)
        {
            IList<DbModel.AssignmentReference> dbAssignmentRefrenceType = null;
            if (assignmentRefrenceType?.Count > 0)
            {
                var assignmentRefrenceTypeId = assignmentRefrenceType.Select(x => x.AssignmentReferenceTypeId).Distinct().ToList();
                dbAssignmentRefrenceType = _assignmentRefrenceRepository.FindBy(x => assignmentRefrenceTypeId.Contains(x.Id)).ToList();
            }

            return dbAssignmentRefrenceType;
        }

        private bool IsAssignmentRefrenceExistInDb(IList<int> AssignmentRefrenceTypeIds,
                                                   IList<DbModel.AssignmentReference> dbAssignmentRefrenceTypes,
                                                   ref IList<int> assignmentRefrenceNotExists,
                                                   ref IList<ValidationMessage> validationMessages)
        {
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            if (dbAssignmentRefrenceTypes == null)
                dbAssignmentRefrenceTypes = new List<DbModel.AssignmentReference>();

            var validMessages = validationMessages;

            if (AssignmentRefrenceTypeIds?.Count > 0)
            {
                assignmentRefrenceNotExists = AssignmentRefrenceTypeIds.Where(x => !dbAssignmentRefrenceTypes.Select(x1 => x1.Id).ToList().Contains(x)).ToList();
                assignmentRefrenceNotExists?.ToList().ForEach(x =>
                {
                    validMessages.Add(_messageDescriptions, x, MessageType.AssignmentRefrenceNotExists, x);
                });
            }



            if (validMessages.Count > 0)
                validationMessages = validMessages;

            return validationMessages.Count <= 0;
        }

        private bool IsRecordValidaForUpdate(IList<DomainModel.AssignmentReferenceType> assignmentRefrenceType,
                                             IList<DbModel.AssignmentReference> dbAssignmentRefrenceType,
                                             ref IList<DbModel.Assignment> dbAssignment,
                                             ref IList<DbModel.Data> dbRefrenceType,
                                             ref IList<ValidationMessage> validationMessages)
        {
            IList<ValidationMessage> messages = new List<ValidationMessage>();
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();
            IList<DbModel.Assignment> dbAssignments = null;
            if (dbAssignment != null)
                dbAssignments = dbAssignment;

            assignmentRefrenceType.Select(x => x.AssignmentReferenceTypeId).ToList()
                   .ForEach(x1 =>
                   {
                       var isExist = dbAssignmentRefrenceType.Any(x2 => x2.Id == x1);
                       if (!isExist)
                           messages.Add(_messageDescriptions, x1, MessageType.AssignmentRefrenceNotExists, x1);
                   });
            var assignmentIds = assignmentRefrenceType.Where(x => x.AssignmentId > 0).Select(u => (int)u.AssignmentId).Distinct().ToList();

            if (messages?.Count <= 0)
            {
                if (IsRecordUpdateCountMatching(assignmentRefrenceType, dbAssignmentRefrenceType, ref messages))
                {
                    if (_assignmentService.IsValidAssignment(assignmentIds, ref dbAssignments, ref messages))
                    {
                        if (IsValidReferenceType(assignmentRefrenceType, ref dbRefrenceType, ref messages))
                        {
                            IsUniqueAssignmentRefrenceType(assignmentRefrenceType, dbAssignmentRefrenceType, ValidationType.Update, ref messages);
                        }
                    }
                }
            }

            if (messages.Count > 0)
                validationMessages.AddRange(messages);

            return messages?.Count <= 0;
        }
        private bool IsRecordUpdateCountMatching(IList<DomainModel.AssignmentReferenceType> assignmentRefrenceType,
                                                 IList<DbModel.AssignmentReference> dbAssignmentRefrenceType,
                                                 ref IList<ValidationMessage> validationMessages)
        {
            List<ValidationMessage> messages = new List<ValidationMessage>();
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            var notMatchedRecords = assignmentRefrenceType.Where(x => !dbAssignmentRefrenceType.ToList().Any(x1 => x1.UpdateCount.ToInt() == x.UpdateCount.ToInt() && x1.Id == x.AssignmentReferenceTypeId)).ToList();
            notMatchedRecords.ForEach(x =>
            {
                messages.Add(_messageDescriptions, x.AssignmentReferenceTypeId, MessageType.AssignmentRefrenceUpdateCountMisMatch, x.AssignmentReferenceTypeId);
            });

            if (messages.Count > 0)
                validationMessages.AddRange(messages);

            return messages?.Count <= 0;
        }


        private bool IsValidReferenceType(IList<AssignmentReferenceType> assignmentReferenceTypes,
                                          ref IList<DbModel.Data> dbReferenceTypes,
                                          ref IList<ValidationMessage> validationMessages)
        {
            IList<ValidationMessage> messages = new List<ValidationMessage>();
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            if (dbReferenceTypes == null && dbReferenceTypes?.Count == 0)
            {
                var referenceTypeNames = assignmentReferenceTypes.Select(x => x.ReferenceType).ToList();
                var dbData = this._dataRepository.FindBy(x => x.MasterDataTypeId == Convert.ToInt32(MasterType.AssignmentReferenceType) &&
                                                                     referenceTypeNames.Contains(x.Name)).ToList();

                var referenceTypeNotExists = assignmentReferenceTypes.Where(x => !dbData.Any(x1 => x1.Name == x.ReferenceType)).ToList();
                referenceTypeNotExists.ToList().ForEach(x =>
                {
                    messages.Add(_messageDescriptions, x.ReferenceType, MessageType.AssignmentRefrenceInvalidRefrence, x.ReferenceType);
                });

                if (dbReferenceTypes?.Count == 0)
                    dbReferenceTypes = dbData;

                if (messages.Count > 0)
                    validationMessages.AddRange(messages);
            }

            return messages?.Count <= 0;
        }

        private bool IsUniqueAssignmentRefrenceType(IList<AssignmentReferenceType> assignmentReferenceTypes,
                                                    IList<DbModel.AssignmentReference> dbAssignmentRefrenceType,
                                                    ValidationType validationType,
                                                    ref IList<ValidationMessage> validationMessages)
        {
            IList<DbModel.AssignmentReference> ReferenceTypeExists = null;
            IList<ValidationMessage> messages = new List<ValidationMessage>();
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            ReferenceTypeExists = _assignmentRefrenceRepository.IsUniqueAssignmentReference(assignmentReferenceTypes, dbAssignmentRefrenceType, validationType);

            ReferenceTypeExists?.ToList().ForEach(x =>
                {
                    messages.Add(_messageDescriptions, x.AssignmentReferenceType.Name, MessageType.AssignmentRefrenceDuplicateRecord, x.AssignmentReferenceType.Name);
                });

            if (messages.Count > 0)
                validationMessages.AddRange(messages);

            return messages?.Count > 0;
        }

        private bool IsRecordValidaForAdd(IList<DomainModel.AssignmentReferenceType> assignmentReferenceType,
                                          IList<DbModel.AssignmentReference> dbAssignmentReference,
                                          ref IList<DbModel.Assignment> dbAssignment,
                                          ref IList<DbModel.Data> dbReferenceType,
                                          ref IList<ValidationMessage> validationMessages)
        {

            IList<ValidationMessage> messages = new List<ValidationMessage>();
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();
            IList<DbModel.Assignment> dbAssignments = null;
            if (dbAssignment != null)
                dbAssignments = dbAssignment;

            var assignmentIds = assignmentReferenceType.Where(x => x.AssignmentId > 0).Select(u => (int)u.AssignmentId).Distinct().ToList();

            if (_assignmentService.IsValidAssignment(assignmentIds, ref dbAssignments, ref messages))

            {
                // IsValidReferenceType(assignmentReferenceType, ref dbReferenceType, ref messages);
                if (IsValidReferenceType(assignmentReferenceType, ref dbReferenceType, ref messages))
                {
                    IsUniqueAssignmentRefrenceType(assignmentReferenceType, dbAssignmentReference, ValidationType.Add, ref messages);
                }
            }

            if (messages.Count > 0)
                validationMessages.AddRange(messages);

            return messages?.Count <= 0;
        }



        #endregion
    }
}
