using AutoMapper;
using Evolution.Assignment.Domain.Interfaces.Assignments;
using Evolution.Assignment.Domain.Interfaces.Data;
using Evolution.Assignment.Domain.Interfaces.Validations;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Common.Models.Messages;
using Evolution.Common.Models.Responses;
using Evolution.Logging.Interfaces;
using Evolution.Master.Domain.Interfaces.Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Transactions;
using DbModels = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModels = Evolution.Assignment.Domain.Models.Assignments;

namespace Evolution.Assignment.Core.Services
{
    public class AssignmentTaxonomyService : IAssignmentTaxonomyService
    {
        private IAssignmentTaxonomyRepository _repository = null;
        private IAppLogger<AssignmentTaxonomyService> _logger = null;
        private IAssignmentTaxonomyValidationService _validationService = null;
        private readonly JObject _messageDescriptions = null;
        private ITaxonomyServices _taxonomyService = null;
        private IAssignmentService _assignmentService = null;
        private IMapper _mapper = null;

        public AssignmentTaxonomyService(IAssignmentTaxonomyRepository repository,
                                        IAppLogger<AssignmentTaxonomyService> logger,
                                        IAssignmentTaxonomyValidationService validationService,
                                        ITaxonomyServices taxonomyService,
                                        IAssignmentService assignmentService,
                                        IMapper mapper,
                                        JObject messages)
        {
            _logger = logger;
            _mapper = mapper;
            _repository = repository;
            _validationService = validationService;
            _messageDescriptions = messages;
            _assignmentService = assignmentService;
            _taxonomyService = taxonomyService;
        }

        #region Public Methods

        public Response Add(IList<DomainModels.AssignmentTaxonomy> assignmentTaxonomies,
                            bool commitChange = true,
                            bool isValidationRequire = true,
                            int? assignmentId = null)
        {
            if (assignmentId.HasValue)
                assignmentTaxonomies?.ToList().ForEach(x => { x.AssignmentId = assignmentId.Value; });
            IList<DbModels.AssignmentTaxonomy> dbAssignmentTaxonomy = null;
            IList<DbModels.TaxonomyService> dbTaxonomyService = null;
            IList<DbModels.Assignment> dbAssignment = null;

            return AddTaxonomies(assignmentTaxonomies,
                                 ref dbAssignmentTaxonomy,
                                 ref dbAssignment,
                                 ref dbTaxonomyService,
                                 commitChange,
                                 isValidationRequire);
        }

        public Response Add(IList<DomainModels.AssignmentTaxonomy> assignmentTaxonomies,
                            ref IList<DbModels.AssignmentTaxonomy> dbAssignmentTaxonomies,
                            ref IList<DbModels.Assignment> dbAssignment,
                            ref IList<DbModels.TaxonomyService> dbTaxonomyService,
                            bool commitChange = true,
                            bool isValidationRequire = true,
                            int? assignmentId = null)
        {
            if (assignmentId.HasValue && !isValidationRequire)
                assignmentTaxonomies?.ToList().ForEach(x => { x.AssignmentId = assignmentId.Value; });
            return AddTaxonomies(assignmentTaxonomies,
                                 ref dbAssignmentTaxonomies,
                                 ref dbAssignment,
                                 ref dbTaxonomyService,
                                 commitChange,
                                 isValidationRequire);
        }

        public Response Update(IList<DomainModels.AssignmentTaxonomy> assignmentTaxonomies,
                               bool commitChange = true,
                               bool isValidationRequire = true,
                               int? assignmentId = null)
        {
            IList<DbModels.AssignmentTaxonomy> dbAssignmentTaxonomy = null;
            IList<DbModels.Assignment> dbAssignments = null;
            IList<DbModels.TaxonomyService> dbTaxonomyService = null;
            if (assignmentId.HasValue)
                assignmentTaxonomies?.ToList().ForEach(x => { x.AssignmentId = assignmentId.Value; });
            return UpdateTaxonomies(assignmentTaxonomies,
                                    ref dbAssignmentTaxonomy,
                                    ref dbAssignments,
                                    ref dbTaxonomyService,
                                    commitChange,
                                    isValidationRequire);
        }

        public Response Update(IList<DomainModels.AssignmentTaxonomy> assignmentTaxonomies,
                               ref IList<DbModels.AssignmentTaxonomy> dbAssignmentTaxonomies,
                               ref IList<DbModels.Assignment> dbAssignments,
                               ref IList<DbModels.TaxonomyService> dbTaxonomyService,
                               bool commitChange = true,
                               bool isValidationRequire = true,
                               int? assignmentId = null)
        {
            if (assignmentId.HasValue && !isValidationRequire)
                assignmentTaxonomies?.ToList().ForEach(x => { x.AssignmentId = assignmentId.Value; });
            return UpdateTaxonomies(assignmentTaxonomies,
                                    ref dbAssignmentTaxonomies,
                                    ref dbAssignments,
                                    ref dbTaxonomyService,
                                    commitChange,
                                    isValidationRequire);

        }

        public Response Delete(IList<DomainModels.AssignmentTaxonomy> assignmentTaxonomies,
                               ref IList<DbModels.AssignmentTaxonomy> dbAssignmentTaxonomies,
                               ref IList<DbModels.Assignment> dbAssignment,
                               ref IList<DbModels.TaxonomyService> dbTaxonomyService,
                               bool commitChange = true,
                               bool isValidationRequire = true,
                               int? assignmentId = null)
        {
            if (assignmentId.HasValue)
                assignmentTaxonomies?.ToList().ForEach(x => { x.AssignmentId = assignmentId.Value; });
            return DeleteTaxonomies(assignmentTaxonomies,
                                    ref dbAssignmentTaxonomies,
                                    ref dbAssignment,
                                    ref dbTaxonomyService,
                                    commitChange,
                                    isValidationRequire);
        }

        public Response Delete(IList<DomainModels.AssignmentTaxonomy> assignmentTaxonomies,
                               bool commitChange = true,
                               bool isValidationRequire = true,
                               int? assignmentId = null)
        {
            IList<DbModels.AssignmentTaxonomy> dbAssignmentTaxonomy = null;
            IList<DbModels.Assignment> dbAssignment = null;
            IList<DbModels.TaxonomyService> dbTaxonomyService = null;
            if (assignmentId.HasValue && !isValidationRequire)
                assignmentTaxonomies?.ToList().ForEach(x => { x.AssignmentId = assignmentId.Value; });
            return DeleteTaxonomies(assignmentTaxonomies,
                                    ref dbAssignmentTaxonomy,
                                    ref dbAssignment,
                                    ref dbTaxonomyService,
                                    commitChange,
                                    isValidationRequire);
        }

        public Response Get(DomainModels.AssignmentTaxonomy searchModel)
        {
            Exception exception = null;
            IList<DomainModels.AssignmentTaxonomy> result = null;
            try
            {
                using (var tranScope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions {IsolationLevel = IsolationLevel.ReadUncommitted}))
                {
                    result = _repository.search(searchModel);
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

        #region Validation Check

        public Response IsRecordValidForProcess(IList<DomainModels.AssignmentTaxonomy> assignmentTaxonomies,
                                                ValidationType validationType)
        {
            IList<DbModels.AssignmentTaxonomy> dbAssignmentTaxonomy = null;
            IList<DbModels.Assignment> dbAssignments = null;
            IList<DbModels.TaxonomyService> dbTaxonomyService = null;
            return IsRecordValidForProcess(assignmentTaxonomies,
                                            validationType,
                                            ref dbAssignmentTaxonomy,
                                            ref dbTaxonomyService,
                                            ref dbAssignments);
        }

        public Response IsRecordValidForProcess(IList<DomainModels.AssignmentTaxonomy> assignmentTaxonomies,
                                                ValidationType validationType,
                                                ref IList<DbModels.AssignmentTaxonomy> dbAssignmentTaxonomies,
                                                ref IList<DbModels.TaxonomyService> dbTaxonomyService,
                                                ref IList<DbModels.Assignment> dbAssignments)
        {
            IList<DomainModels.AssignmentTaxonomy> filteredAssignmentTaxonomies = null;
            return IsRecordValidForProcess(validationType,
                                            assignmentTaxonomies,
                                            ref filteredAssignmentTaxonomies,
                                            ref dbAssignmentTaxonomies,
                                            ref dbTaxonomyService,
                                            ref dbAssignments
                                          );
        }

        public Response IsRecordValidForProcess(IList<DomainModels.AssignmentTaxonomy> assignmentTaxonomies,
                                                ValidationType validationType,
                                                IList<DbModels.AssignmentTaxonomy> dbAssignmenttaxonomies,
                                                ref IList<DbModels.TaxonomyService> dbTaxonomyservices,
                                                ref IList<DbModels.Assignment> dbAssignments)
        {
            return IsRecordValidForProcess(assignmentTaxonomies,
                                            validationType,
                                            ref dbAssignmenttaxonomies,
                                            ref dbTaxonomyservices,
                                            ref dbAssignments);
        }

        #endregion

        #endregion

        #region Private Methods

        private Response AddTaxonomies(IList<DomainModels.AssignmentTaxonomy> assignmentTaxonomies,
                                       ref IList<DbModels.AssignmentTaxonomy> dbAssignmentTaxonomies,
                                       ref IList<DbModels.Assignment> dbAssignments,
                                       ref IList<DbModels.TaxonomyService> dbTaxonomyService,
                                       bool commitChange = true,
                                       bool isDbValidationRequire = true)
        {
            Exception exception = null;
            IList<ValidationMessage> validationMessages = null;
            try
            {
                IList<DbModels.TaxonomyService> dbTaxonomyServices = null;
                Response valdResponse = null;
                var recordToBeAdd = FilterRecords(assignmentTaxonomies, ValidationType.Add);
                if (isDbValidationRequire)
                    valdResponse = IsRecordValidForProcess(ValidationType.Add,
                                                           assignmentTaxonomies,
                                                           ref recordToBeAdd,
                                                           ref dbAssignmentTaxonomies,
                                                           ref dbTaxonomyService,
                                                           ref dbAssignments);
                if ((!isDbValidationRequire || Convert.ToBoolean(valdResponse.Result)) && recordToBeAdd?.Count > 0)
                {
                    _repository.AutoSave = false;
                    dbTaxonomyServices = dbTaxonomyService;
                    dbAssignmentTaxonomies = _mapper.Map<IList<DbModels.AssignmentTaxonomy>>(recordToBeAdd, opt =>
                    {
                        opt.Items["isAssignId"] = false;
                        opt.Items["TaxonomyService"] = dbTaxonomyServices;
                    });
                    _repository.Add(dbAssignmentTaxonomies);
                    if (commitChange && recordToBeAdd.Count > 0)
                        _repository.ForceSave();
                }
                else
                    return valdResponse;
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), assignmentTaxonomies);
            }
            //finally { _repository.Dispose(); }

            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), null, exception);
        }

        private Response UpdateTaxonomies(IList<DomainModels.AssignmentTaxonomy> assignmentTaxonomies,
                                          ref IList<DbModels.AssignmentTaxonomy> dbAssignmentTaxonomy,
                                          ref IList<DbModels.Assignment> dbAssignment,
                                          ref IList<DbModels.TaxonomyService> dbTaxonomyService,
                                          bool commitChange,
                                          bool isDbValidationRequire)
        {

            Exception exception = null;
            IList<ValidationMessage> validationMessages = null;
            IList<DbModels.Assignment> dbAssignments = null;

            try
            {
                Response valdResponse = null;
                var filteredRecords = FilterRecords(assignmentTaxonomies, ValidationType.Update);
                if (isDbValidationRequire)
                    valdResponse = IsRecordValidForProcess(ValidationType.Update,
                                                           assignmentTaxonomies,
                                                           ref filteredRecords,
                                                           ref dbAssignmentTaxonomy,
                                                           ref dbTaxonomyService,
                                                           ref dbAssignments);
                    if ((!isDbValidationRequire || Convert.ToBoolean(valdResponse.Result)) && filteredRecords?.Count > 0)
                    {
                        if (dbAssignmentTaxonomy == null)
                            dbAssignmentTaxonomy = GetAssignmentTaxonomies(assignmentTaxonomies);

                        IList<DbModels.AssignmentTaxonomy> recordToUpdate = new List<DbModels.AssignmentTaxonomy>();
                        foreach (var record in filteredRecords)
                        {
                            var dbrecord = dbAssignmentTaxonomy.Where(x => x.Id == record.AssignmentTaxonomyId).FirstOrDefault();
                            if (dbrecord != null)
                            {
                                dbrecord.TaxonomyServiceId = dbTaxonomyService.Where(x => x.TaxonomyServiceName == record.TaxonomyService).FirstOrDefault().Id;
                                dbrecord.LastModification = DateTime.UtcNow;
                                dbrecord.UpdateCount = record.UpdateCount.CalculateUpdateCount();
                                dbrecord.ModifiedBy = record.ModifiedBy;
                                recordToUpdate.Add(dbrecord);
                            }
                        }
                        _repository.AutoSave = false;
                        _repository.Update(recordToUpdate);

                        if (commitChange)
                            _repository.ForceSave();
                    }
                    else
                        return valdResponse;

            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), assignmentTaxonomies);
            }
            finally
            {
                _repository.AutoSave = true;
                //_repository.Dispose();
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), null, exception);

        }

        private Response DeleteTaxonomies(IList<DomainModels.AssignmentTaxonomy> assignmentTaxonomies,
                                          ref IList<DbModels.AssignmentTaxonomy> dbAssignmentTaxonomy,
                                          ref IList<DbModels.Assignment> dbAssignment,
                                          ref IList<DbModels.TaxonomyService> dbTaxonomyService,
                                          bool commitChange,
                                          bool isDbValidationRequire)
        {

            Exception exception = null;
            IList<ValidationMessage> validationMessages = null;
            try
            {
                Response valdResponse = null;
                var filteredRecords = FilterRecords(assignmentTaxonomies, ValidationType.Delete);
                if (isDbValidationRequire)
                    valdResponse = IsRecordValidForProcess(ValidationType.Delete,
                                                            assignmentTaxonomies,
                                                            ref filteredRecords,
                                                            ref dbAssignmentTaxonomy,
                                                            ref dbTaxonomyService,
                                                            ref dbAssignment);
                    if ((!isDbValidationRequire || Convert.ToBoolean(valdResponse.Result)) && filteredRecords?.Count > 0)
                    {
                        var recordToDelete = dbAssignmentTaxonomy.Where(x => filteredRecords.Select(x1 => x1.AssignmentTaxonomyId).Contains(x.Id)).ToList();
                        _repository.AutoSave = false;
                        _repository.Delete(recordToDelete);

                        if (commitChange)
                            _repository.ForceSave();
                    }
                    else
                        return valdResponse;
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), assignmentTaxonomies);
            }
            finally
            {
                _repository.AutoSave = true;
               // _repository.Dispose();
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), null, exception);
        }

        private Response IsRecordValidForProcess(ValidationType validationType,
                                                IList<DomainModels.AssignmentTaxonomy> assignmentTaxonomies,
                                                ref IList<DomainModels.AssignmentTaxonomy> filteredTaxonomies,
                                                ref IList<DbModels.AssignmentTaxonomy> dbAssignmentTaxonomies,
                                                ref IList<DbModels.TaxonomyService> dbTaxonomyServices,
                                                ref IList<DbModels.Assignment> dbAssignments)

        {
            Exception exception = null;
            bool result = true;
            IList<ValidationMessage> validationMessages = null;
            try
            {
                if (filteredTaxonomies == null || filteredTaxonomies.Count <= 0)
                    filteredTaxonomies = FilterRecords(assignmentTaxonomies, validationType);

                if (filteredTaxonomies != null && filteredTaxonomies.Count > 0)
                {
                    if (validationMessages == null)
                        validationMessages = new List<ValidationMessage>();

                    result = IsValidPayLoad(filteredTaxonomies,
                                            validationType,
                                            ref validationMessages);
                    if (result && filteredTaxonomies?.Count > 0)
                    {
                        var assignmentSubSupplierIds = filteredTaxonomies.Where(x => x.AssignmentTaxonomyId != null).Select(x => x.AssignmentTaxonomyId).ToList();
                        if ((dbAssignmentTaxonomies == null || dbAssignmentTaxonomies.Count <= 0) && validationType != ValidationType.Add)
                            dbAssignmentTaxonomies = GetAssignmentTaxonomies(filteredTaxonomies);

                        if (validationType == ValidationType.Delete || validationType == ValidationType.Update)
                        {
                            result = IsRecordExistsInDb(assignmentSubSupplierIds,
                                                        dbAssignmentTaxonomies,
                                                        ref validationMessages);
                            if (result && validationType == ValidationType.Delete)
                                result = true;
                            else if (result && validationType == ValidationType.Update)
                                result = IsRecordValidForUpdate(filteredTaxonomies,
                                                                dbAssignmentTaxonomies,
                                                                ref dbTaxonomyServices,
                                                                ref validationMessages,
                                                                ref dbAssignments);
                        }
                        else if (validationType == ValidationType.Add)
                            result = IsRecordValidForAdd(filteredTaxonomies,
                                                         dbAssignmentTaxonomies,
                                                         ref dbTaxonomyServices,
                                                         ref validationMessages,
                                                         ref dbAssignments);
                    }
                }
            }
            catch (Exception ex)
            {
                result = false;
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), assignmentTaxonomies);
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), result, exception);
        }

        private IList<DbModels.AssignmentTaxonomy> GetAssignmentTaxonomies(IList<DomainModels.AssignmentTaxonomy> assignmentTaxonomies)
        {
            IList<DbModels.AssignmentTaxonomy> dbAssignmentTaxonomies = null;
            if (assignmentTaxonomies?.Count > 0)
                dbAssignmentTaxonomies = _repository.FindBy(x => assignmentTaxonomies.Select(x1 => x1.AssignmentTaxonomyId).Contains(x.Id)).ToList();

            return dbAssignmentTaxonomies;
        }

        private IList<DomainModels.AssignmentTaxonomy> FilterRecords(IList<DomainModels.AssignmentTaxonomy> assignmentTaxonomies,
                                                                     ValidationType filterType)
        {
            IList<DomainModels.AssignmentTaxonomy> filteredRecords = null;
            if (filterType == ValidationType.Add)
                filteredRecords = assignmentTaxonomies.Where(x => x.RecordStatus.IsRecordStatusNew()).ToList();
            if (filterType == ValidationType.Update)
                filteredRecords = assignmentTaxonomies.Where(x => x.RecordStatus.IsRecordStatusModified()).ToList();
            if (filterType == ValidationType.Delete)
                filteredRecords = assignmentTaxonomies.Where(x => x.RecordStatus.IsRecordStatusDeleted()).ToList();
            return filteredRecords;
        }

        private bool IsValidPayLoad(IList<DomainModels.AssignmentTaxonomy> assignmentTaxonomies,
                                    ValidationType validationType,
                                    ref IList<ValidationMessage> validationMessages)
        {
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            var validMessages = validationMessages;

            var validationResults = _validationService.Validate(JsonConvert.SerializeObject(assignmentTaxonomies), validationType);
            if (validationResults?.Count > 0)
                validationMessages.Add(_messageDescriptions, ModuleType.Assignment, validationResults);

            validationMessages = validMessages;
            return validationMessages?.Count <= 0;
        }

        private bool IsValidTaxonomyCategroy(IList<DomainModels.AssignmentTaxonomy> assignmentTaxonomies,
                                             ref IList<DbModels.TaxonomyService> dbTaxonomyServiceNames,
                                             ref IList<ValidationMessage> validationMessages,
                                             params Expression<Func<DbModels.TaxonomyService, object>>[] includes)
        {
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            var validMessages = validationMessages;
            var dbcategorynames = dbTaxonomyServiceNames?.Select(x => x.TaxonomySubCategory.TaxonomyCategory).ToList();
            var categoryNotExists = assignmentTaxonomies.Where(x => !dbcategorynames.Any(x1 => x1.Name == x.TaxonomyCategory)).ToList();

            categoryNotExists?.ForEach(x =>
            {
                validMessages.Add(_messageDescriptions, x, MessageType.InvalidTaxonomyCategory, x);
            });

            validationMessages = validMessages;
            return validationMessages?.Count <= 0;
        }

        private bool IsValidTaxonomyService(IList<string> serviceNames, IList<DbModels.TaxonomyService> dbTaxonomyService,
                                            ref IList<ValidationMessage> validationMessages)
        {
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            var validMessages = validationMessages;
            var serviceNotExists = serviceNames.Where(x => !dbTaxonomyService.Any(x1 => x1.TaxonomyServiceName == x)).ToList();

            serviceNotExists?.ForEach(x =>
            {
                validMessages.Add(_messageDescriptions, x, MessageType.InvalidTaxonomyService, x);
            });

            validationMessages = validMessages;
            return validationMessages?.Count <= 0;
        }

        private bool IsRecordExistsInDb(IList<int?> serviceIds,
                                        IList<DbModels.AssignmentTaxonomy> dbAssignmentTaxonomy,
                                        ref IList<ValidationMessage> validationMessages)
        {
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            var validMessages = validationMessages;
            var recordNotExistsInDb = serviceIds.Where(x => !dbAssignmentTaxonomy.Any(x1 => x1.Id == x)).ToList();

            recordNotExistsInDb?.ForEach(x =>
            {
                validMessages.Add(_messageDescriptions, x, MessageType.TaxonomyDoesntExists, x);

            });

            validationMessages = validMessages;
            return validationMessages?.Count <= 0;
        }

        private bool IsRecordValidForAdd(IList<DomainModels.AssignmentTaxonomy> assignmentTaxonomies,
                                         IList<DbModels.AssignmentTaxonomy> dbAssignmentTaxonomies,
                                         ref IList<DbModels.TaxonomyService> dbTaxonomyService,
                                         ref IList<ValidationMessage> validationMessages,
                                         ref IList<DbModels.Assignment> dbAssignments)
        {

            IList<ValidationMessage> messages = new List<ValidationMessage>();
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            var assignmentIds = assignmentTaxonomies.Where(x => x.AssignmentId > 0).Select(x1 => (int)x1.AssignmentId).ToList();
            var taxonomyService = assignmentTaxonomies.Select(x => x.TaxonomyService).ToList();
            if (_assignmentService.IsValidAssignment(assignmentIds, ref dbAssignments, ref validationMessages)
                && _taxonomyService.IsValidTaxonomyService(taxonomyService, ref dbTaxonomyService, ref validationMessages,
                                                                  taxo => taxo.TaxonomySubCategory, Cat => Cat.TaxonomySubCategory.TaxonomyCategory)
                && IsValidTaxonomyCategroy(assignmentTaxonomies, ref dbTaxonomyService, ref validationMessages)
                && IsValidTaxonomySubCategory(assignmentTaxonomies, ref dbTaxonomyService, ref validationMessages))
            {
                IsUniqueAssignmentTaxonomy(assignmentTaxonomies, dbAssignments,ref dbTaxonomyService, ref validationMessages);
            }


            messages = validationMessages;
            return messages?.Count <= 0;
        }

        private bool IsRecordValidForUpdate(IList<DomainModels.AssignmentTaxonomy> assignmentTaxonomy,
                                            IList<DbModels.AssignmentTaxonomy> dbAssignmentTaxonomy,
                                            ref IList<DbModels.TaxonomyService> dbTaxonomyService,
                                            ref IList<ValidationMessage> validationMessages,
                                            ref IList<DbModels.Assignment> dbAssignments)
        {
            IList<ValidationMessage> messages = new List<ValidationMessage>();
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();
            var taxonomyService = assignmentTaxonomy.Select(x => x.TaxonomyService).ToList();

            assignmentTaxonomy.Select(x => x.AssignmentTaxonomyId).ToList()
                   .ForEach(x1 =>
                   {
                       var isExist = dbAssignmentTaxonomy.Any(x2 => x2.Id == x1);
                       if (!isExist)
                           messages.Add(_messageDescriptions, x1, MessageType.TaxonomyDoesntExists, x1);
                   });

            if (messages?.Count <= 0)
            {
                if (IsRecordUpdateCountMatching(assignmentTaxonomy,
                                                dbAssignmentTaxonomy,
                                                ref messages))
                {
                    var assignmentIds = assignmentTaxonomy.Where(x => x.AssignmentId > 0).Select(x => (int)x.AssignmentId).Distinct().ToList();
                    if (_assignmentService.IsValidAssignment(assignmentIds, ref dbAssignments, ref validationMessages)
                         && _taxonomyService.IsValidTaxonomyService(taxonomyService, ref dbTaxonomyService, ref validationMessages, taxo => taxo.TaxonomySubCategory,
                                                                                                                                    cat => cat.TaxonomySubCategory.TaxonomyCategory)
                         && IsValidTaxonomySubCategory(assignmentTaxonomy, ref dbTaxonomyService, ref validationMessages)
                         && IsValidTaxonomyCategroy(assignmentTaxonomy, ref dbTaxonomyService, ref validationMessages))
                        IsUniqueAssignmentTaxonomy(assignmentTaxonomy, dbAssignments, ref dbTaxonomyService, ref validationMessages);
                }
            }

            if (messages.Count > 0)
                validationMessages.AddRange(messages);

            return validationMessages?.Count <= 0;
        }
        private bool IsRecordUpdateCountMatching(IList<DomainModels.AssignmentTaxonomy> assignmentTaxonomy,
                                                 IList<DbModels.AssignmentTaxonomy> dbAssignmentTaxonomy,
                                                 ref IList<ValidationMessage> validationMessages)
        {
            List<ValidationMessage> messages = new List<ValidationMessage>();
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            var notMatchedRecords = assignmentTaxonomy.Where(x => !dbAssignmentTaxonomy.ToList().Any(x1 => x1.UpdateCount.ToInt() == x.UpdateCount.ToInt() && x1.Id == x.AssignmentTaxonomyId)).ToList();
            notMatchedRecords.ForEach(x =>
            {
                messages.Add(_messageDescriptions, x, MessageType.TaxonomyAlreadyUpdated, x.AssignmentTaxonomyId);
            });

            if (messages.Count > 0)
                validationMessages.AddRange(messages);

            return messages?.Count <= 0;
        }

        private bool IsValidTaxonomySubCategory(IList<DomainModels.AssignmentTaxonomy> assignmentTaxonomies,
                                                ref IList<DbModels.TaxonomyService> dbTaxonomyServiceNames,
                                                ref IList<ValidationMessage> validationMessages,
                                                params Expression<Func<DbModels.TaxonomyService, object>>[] includes)
        {
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            var validMessages = validationMessages;
            var dbcategorynames = dbTaxonomyServiceNames?.Select(x => x.TaxonomySubCategory).ToList();
            var categoryNotExists = assignmentTaxonomies.Where(x => !dbcategorynames.Any(x1 => x1.TaxonomySubCategoryName == x.TaxonomySubCategory)).ToList();

            categoryNotExists?.ForEach(x =>
            {
                validMessages.Add(_messageDescriptions, x, MessageType.InvalidTaxonomySubCategory, x);
            });

            validationMessages = validMessages;
            return validationMessages?.Count <= 0;

        }
        private bool IsUniqueAssignmentTaxonomy(IList<DomainModels.AssignmentTaxonomy> assignmentTaxonomies,
                                                IList<DbModels.Assignment> dbAssignments,
                                                ref IList<DbModels.TaxonomyService> dbTaxonomyService,
                                                ref IList<ValidationMessage> validationMessages)
        {
            List<ValidationMessage> messages = new List<ValidationMessage>();
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            var assignmentPartialTaxonomies = assignmentTaxonomies.Select(x => new { x.AssignmentId, x.TaxonomyService, x.AssignmentTaxonomyId }).ToList();
            var duplicateRecords = dbAssignments.SelectMany(x => x.AssignmentTaxonomy).ToList().Where(x => assignmentPartialTaxonomies.Any(x1 => x1.AssignmentId == x.AssignmentId &&
                                                                                                            x1.TaxonomyService == x.TaxonomyService.TaxonomyServiceName
                                                                                                            && x1.AssignmentTaxonomyId != x.Id)).ToList();

            assignmentPartialTaxonomies.Where(x => duplicateRecords.Any(x1 => x1.AssignmentId == x.AssignmentId && x1.TaxonomyService.TaxonomyServiceName == x.TaxonomyService))?.ToList().ForEach(x =>
            {
                messages.Add(_messageDescriptions, x.TaxonomyService, MessageType.TaxonomyDuplicateRecord, x.TaxonomyService);
            });
            if (messages.Count > 0)
                validationMessages.AddRange(messages);

            return messages?.Count <= 0;
        }
    }

}

#endregion


