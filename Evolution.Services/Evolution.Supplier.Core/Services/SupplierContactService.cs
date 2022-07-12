using AutoMapper;
using Evolution.AuditLog.Domain.Enums;
using Evolution.AuditLog.Domain.Extensions;
using Evolution.AuditLog.Domain.Functions;
using Evolution.AuditLog.Domain.Interfaces.Audit;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Common.Models.Messages;
using Evolution.Common.Models.Responses;
using Evolution.Logging.Interfaces;
using Evolution.Supplier.Domain.Interfaces.Data;
using Evolution.Supplier.Domain.Interfaces.Suppliers;
using Evolution.Supplier.Domain.Interfaces.Validations;
using Evolution.SupplierContacts.Domain.Interfaces.Suppliers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Transactions;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.Supplier.Domain.Models.Supplier;

namespace Evolution.Supplier.Core.Services
{
    public class SupplierContactService : ISupplierContactService
    {
        private readonly IMapper _mapper = null;
        private readonly IAppLogger<SupplierContactService> _logger = null;
        private readonly ISupplierContactRepository _repository = null;
        private readonly ISupplierService _supplierService = null;
        private readonly JObject _messageDescriptions = null;
        private readonly IAppLogger<LogEventGeneration> _applogger = null;
        private readonly ISupplierContactValidationService _validationService = null;
        private readonly IAuditSearchService _auditSearchService = null;

        #region Constructor

        public SupplierContactService(IMapper mapper,
                                      IAppLogger<SupplierContactService> logger,
                                      ISupplierContactRepository repository,
                                      ISupplierService supplierService,
                                     // IAuditLogger auditLogger,
                                      IAppLogger<LogEventGeneration> applogger,
                                      ISupplierContactValidationService validationService,
                                      JObject messages,
                                       IAuditSearchService auditSearchService)
        {
            this._mapper = mapper;
            this._logger = logger;
            this._repository = repository;
            this._supplierService = supplierService;
           // this._auditLogger = auditLogger;
            this._applogger = applogger;
            this._validationService = validationService;
            this._messageDescriptions = messages;
            _auditSearchService = auditSearchService;
        }
        #endregion

        #region Public Methods
        #region Get
        public Response Get(DomainModel.SupplierContact searchModel)
        {
            IList<DomainModel.SupplierContact> result = null;
            Exception exception = null;
            try
            {
                using (var tranScope = new TransactionScope(TransactionScopeOption.RequiresNew,
                                              new TransactionOptions
                                              {
                                                  IsolationLevel = IsolationLevel.ReadUncommitted
                                              }))
                {
                    result = _mapper.Map<IList<DomainModel.SupplierContact>>(this._repository.Search(searchModel));
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

        #region Add
        public Response Add(IList<DomainModel.SupplierContact> supplierContacts,
                            bool commitChange = true,
                            bool isDbValidationRequire = true)
        {
            IList<DbModel.SupplierContact> dbSupplierContacts = null;
            IList<DbModel.Supplier> dbSuppliers = null;
            return AddSupplierContact(supplierContacts,null, ref dbSupplierContacts, ref dbSuppliers, commitChange, isDbValidationRequire);
        }

        public Response Add(IList<DomainModel.SupplierContact> supplierContacts,
            IList<DbModel.SqlauditModule> dbModule,
                            ref IList<DbModel.SupplierContact> dbSupplierContacts,
                            ref IList<DbModel.Supplier> dbSuppliers,
                            bool commitChange = true,
                            bool isDbValidationRequire = true)
        {
            return AddSupplierContact(supplierContacts, dbModule, ref dbSupplierContacts, ref dbSuppliers, commitChange, isDbValidationRequire);
        }
        #endregion

        #region Modify

        public Response Modify(IList<DomainModel.SupplierContact> supplierContacts,
                                bool commitChange = true,
                                bool isDbValidationRequire = true)
        {
            IList<DbModel.SupplierContact> dbSupplierContacts = null;
            IList<DbModel.Supplier> dbSuppliers = null;
            return UpdateSupplierContact(supplierContacts,null, ref dbSupplierContacts, ref dbSuppliers, commitChange, isDbValidationRequire);
        }

        public Response Modify(IList<DomainModel.SupplierContact> supplierContacts,
            IList<DbModel.SqlauditModule> dbModule,
                                ref IList<DbModel.SupplierContact> dbSupplierContacts,
                                ref IList<DbModel.Supplier> dbSuppliers,
                                bool commitChange = true,
                                bool isDbValidationRequire = true)
        {
            return UpdateSupplierContact(supplierContacts, dbModule, ref dbSupplierContacts, ref dbSuppliers, commitChange, isDbValidationRequire);
        }

        #endregion

        #region Delete

        public Response Delete(IList<DomainModel.SupplierContact> supplierContacts,
                               bool commitChange = true,
                               bool isDbValidationRequire = true)
        {
            IList<DbModel.SupplierContact> dbSupplierContacts = null;
            IList<DbModel.Supplier> dbSuppliers = null;
            return this.RemoveSupplierContacts(supplierContacts,null, ref dbSupplierContacts, ref dbSuppliers, commitChange, isDbValidationRequire);
        }

        public Response Delete(IList<DomainModel.SupplierContact> supplierContacts,
            IList<DbModel.SqlauditModule> dbModule,
                               ref IList<DbModel.SupplierContact> dbSupplierContacts,
                               ref IList<DbModel.Supplier> dbSuppliers,
                               bool commitChange = true,
                               bool isDbValidationRequire = true)
        {
            return this.RemoveSupplierContacts(supplierContacts, dbModule, ref dbSupplierContacts, ref dbSuppliers, commitChange, isDbValidationRequire);
        }

        #endregion

        #region Record Valid Check
        public Response IsRecordValidForProcess(IList<DomainModel.SupplierContact> supplierContacts,
                                              ValidationType validationType)
        {
            IList<DbModel.SupplierContact> dbSupplierContacts = null;
            IList<DbModel.Supplier> dbSuppliers = null;
            return IsRecordValidForProcess(supplierContacts, validationType, ref dbSupplierContacts, ref dbSuppliers);
        }

        public Response IsRecordValidForProcess(IList<DomainModel.SupplierContact> supplierContacts,
                                                ValidationType validationType,
                                                ref IList<DbModel.SupplierContact> dbSupplierContacts,
                                                ref IList<DbModel.Supplier> dbSuppliers)
        {
            IList<DomainModel.SupplierContact> filteredSupplierContact = null;
            return this.CheckRecordValidForProcess(supplierContacts, validationType, ref filteredSupplierContact, ref dbSupplierContacts, ref dbSuppliers);
        }

        public Response IsRecordValidForProcess(IList<DomainModel.SupplierContact> supplierContacts,
                                                ValidationType validationType,
                                                IList<DbModel.SupplierContact> dbSupplierContacts,
                                                IList<DbModel.Supplier> dbSuppliers)
        {
            return IsRecordValidForProcess(supplierContacts, validationType, ref dbSupplierContacts, ref dbSuppliers);
        }
        #endregion
        #endregion

        #region Private Metods
        #region Get
        private IList<DbModel.SupplierContact> GetSupplierContactById(IList<int> supplierContactIds,
                                                                      params Expression<Func<DbModel.SupplierContact, object>>[] includes)
        {
            IList<DbModel.SupplierContact> dbSupplierContacts = null;
            if (supplierContactIds?.Count > 0)
                dbSupplierContacts = _repository.FindBy(x => supplierContactIds.Contains(x.Id), includes).ToList();

            return dbSupplierContacts;
        }

        #endregion

        #region Add
        private bool IsRecordValidForAdd(IList<DomainModel.SupplierContact> supplierContacts,
                                ref IList<DomainModel.SupplierContact> filteredSupplierContacts,
                                ref IList<DbModel.SupplierContact> dbSupplierContacts,
                                ref IList<DbModel.Supplier> dbSuppliers,
                                ref IList<ValidationMessage> validationMessages)
        {
            bool result = false;
            if (supplierContacts != null && supplierContacts.Count > 0)
            {
                ValidationType validationType = ValidationType.Add;
                if (validationMessages == null)
                    validationMessages = new List<ValidationMessage>();

                if (filteredSupplierContacts == null || filteredSupplierContacts.Count <= 0)
                    filteredSupplierContacts = FilterRecord(supplierContacts, validationType);

                if (filteredSupplierContacts?.Count > 0 && IsValidPayload(filteredSupplierContacts, validationType, ref validationMessages))
                {
                    IList<int> supplierIds = filteredSupplierContacts.Select(x => x.SupplierId ?? 0).ToList();
                    result = Convert.ToBoolean(_supplierService.IsRecordExistInDb(supplierIds, ref dbSuppliers, ref validationMessages).Result);
                }
            }
            return result;
        }

        private Response AddSupplierContact(IList<DomainModel.SupplierContact> supplierContacts,
            IList<DbModel.SqlauditModule> dbModule,
                                           ref IList<DbModel.SupplierContact> dbSupplierContacts,
                                           ref IList<DbModel.Supplier> dbSuppliers,
                                           bool commitChange,
                                           bool isDbValidationRequire)
        {
            Exception exception = null;
            IList<ValidationMessage> validationMessages = null;
            IList<DomainModel.SupplierContact> recordToBeAdd = FilterRecord(supplierContacts, ValidationType.Add);
            long? eventId = 0;
            try
            {
                Response valdResponse = null;

                eventId = supplierContacts?.FirstOrDefault()?.EventId;
                if (isDbValidationRequire)
                    valdResponse = CheckRecordValidForProcess(supplierContacts, ValidationType.Add, ref recordToBeAdd, ref dbSupplierContacts, ref dbSuppliers);

                if ((!isDbValidationRequire || Convert.ToBoolean(valdResponse.Result)) && recordToBeAdd?.Count > 0)
                {
                    _repository.AutoSave = false;
                    IList<DbModel.Supplier> dbSuppliersToBeAdd = dbSuppliers;

                    var mappedRecords = recordToBeAdd?.ToList().Select(x => new DbModel.SupplierContact()
                    {
                        SupplierId = (int)dbSuppliersToBeAdd?.FirstOrDefault(x1 => x1.Id == x.SupplierId)?.Id,
                        SupplierContactName = x.SupplierContactName,
                        TelephoneNumber = x.SupplierTelephoneNumber,
                        EmailId = x.SupplierEmail,
                        FaxNumber = x.SupplierFaxNumber,
                        MobileNumber = x.SupplierMobileNumber,
                        OtherContactDetails = x.OtherContactDetails,
                        UpdateCount = 0,
                        ModifiedBy = x.ModifiedBy
                    }).ToList();

                    _repository.Add(mappedRecords);

                    if (commitChange)
                    {
                        int value = _repository.ForceSave();
                        if (value > 0)
                        {
                            mappedRecords?.ToList().ForEach(x1 => _auditSearchService.AuditLog(x1, ref eventId, supplierContacts?.FirstOrDefault()?.ActionByUser,
                                                                                                  null,
                                                                                                   ValidationType.Add.ToAuditActionType(),
                                                                                                   SqlAuditModuleType.SupplierContact,
                                                                                                    null,
                                                                                                     _mapper.Map<DomainModel.SupplierContact>(x1),
                                                                                                     dbModule
                                                                                                    ));
                        }
                    }


                }
                else
                    return valdResponse;

            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), supplierContacts);
            }
            finally
            {
                _repository.AutoSave = true;
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), null, exception);
        }

        #endregion

        #region Modify
        private bool IsRecordValidForUpdate(IList<DomainModel.SupplierContact> supplierContacts,
                                           ref IList<DomainModel.SupplierContact> filteredSupplierContacts,
                                           ref IList<DbModel.SupplierContact> dbSupplierContacts,
                                           ref IList<DbModel.Supplier> dbSuppliers,
                                           ref IList<ValidationMessage> validationMessages)
        {
            bool result = false;
            if (supplierContacts != null && supplierContacts.Count > 0)
            {
                ValidationType validationType = ValidationType.Update;

                if (validationMessages == null)
                    validationMessages = new List<ValidationMessage>();

                if (filteredSupplierContacts == null || filteredSupplierContacts.Count <= 0)
                    filteredSupplierContacts = FilterRecord(supplierContacts, validationType);

                if (filteredSupplierContacts?.Count > 0 && IsValidPayload(filteredSupplierContacts, validationType, ref validationMessages))
                {
                    if (this.IsValidSupplier(filteredSupplierContacts, ref dbSuppliers, ref validationMessages))
                    {
                        this.GetSupplierContacts(filteredSupplierContacts, ref dbSupplierContacts);
                        IList<int> supplierContactIdsNotExists = null;
                        var supplierContactIds = filteredSupplierContacts.Select(x => x.SupplierContactId ?? 0).Distinct().ToList();

                        if (IsSupplierContactExistInDb(supplierContactIds, dbSupplierContacts, ref supplierContactIdsNotExists, ref validationMessages))
                            result = this.IsRecordUpdateCountMatching(filteredSupplierContacts, dbSupplierContacts, ref validationMessages);
                    }
                }
            }
            return result;
        }

        private Response UpdateSupplierContact(IList<DomainModel.SupplierContact> supplierContacts,
            IList<DbModel.SqlauditModule> dbModule,
                                             ref IList<DbModel.SupplierContact> dbSupplierContacts,
                                             ref IList<DbModel.Supplier> dbSuppliers,
                                             bool commitChange = true,
                                             bool isDbValidationRequire = true)
        {
            Exception exception = null;
            IList<DbModel.Supplier> dbSupplier = null;
            IList<ValidationMessage> validationMessages = null;
            long? eventId = 0;

            try
            {
                Response valdResponse = null;
                IList<DomainModel.SupplierContact> dbExistingSupplierContacts = new List<DomainModel.SupplierContact>();
                var recordToBeModify = FilterRecord(supplierContacts, ValidationType.Update);
                eventId = supplierContacts?.FirstOrDefault()?.EventId;

                if (isDbValidationRequire)
                    valdResponse = CheckRecordValidForProcess(supplierContacts, ValidationType.Update, ref recordToBeModify, ref dbSupplierContacts, ref dbSuppliers);

                if (dbSuppliers != null)
                    dbSupplier = dbSuppliers;

                if ((dbSupplierContacts == null || (dbSupplierContacts?.Count <= 0 && valdResponse == null)) && recordToBeModify?.Count > 0)
                    dbSupplierContacts = _repository.Get(recordToBeModify?.Select(x => x.SupplierContactId ?? 0).ToList());

                if (!isDbValidationRequire || (Convert.ToBoolean(valdResponse.Result) && dbSupplierContacts?.Count > 0))
                {
                    dbSupplierContacts.ToList().ForEach(x =>
                    {
                        dbExistingSupplierContacts.Add(ObjectExtension.Clone(_mapper.Map<DomainModel.SupplierContact>(x)));
                    });
                    dbSupplierContacts?.ToList().ForEach(x =>
                    {
                        var recordToBeModified = recordToBeModify?.FirstOrDefault(x1 => x1.SupplierContactId == x.Id);
                        x.SupplierId = (int)dbSupplier?.FirstOrDefault(x1 => x1.Id == recordToBeModified.SupplierId)?.Id;
                        x.SupplierContactName = recordToBeModified?.SupplierContactName;
                        x.EmailId = recordToBeModified?.SupplierEmail;
                        x.TelephoneNumber = recordToBeModified?.SupplierTelephoneNumber;
                        x.FaxNumber = recordToBeModified?.SupplierFaxNumber;
                        x.MobileNumber = recordToBeModified?.SupplierMobileNumber;
                        x.OtherContactDetails = recordToBeModified?.OtherContactDetails;
                        x.UpdateCount = recordToBeModify.Where(x1 => x1.SupplierContactId == x.Id).FirstOrDefault().UpdateCount.CalculateUpdateCount();
                        x.LastModification = DateTime.UtcNow;
                        x.ModifiedBy = recordToBeModified.ModifiedBy;
                    });

                    _repository.AutoSave = false;
                    _repository.Update(dbSupplierContacts);
                    if (commitChange)
                    {
                        int value = _repository.ForceSave();
                        if (value > 0)
                        {
                            dbSupplierContacts?.ToList().ForEach(x =>
                            recordToBeModify?.ToList().ForEach(x1 => _auditSearchService.AuditLog(x1, ref eventId, x1.ActionByUser,
                                                                                                  null,
                                                                                                   ValidationType.Update.ToAuditActionType(),
                                                                                                   SqlAuditModuleType.SupplierContact,
                                                                                                   _mapper.Map<DomainModel.SupplierContact>(dbExistingSupplierContacts?.FirstOrDefault(x2 => x2.SupplierContactId == x1.SupplierContactId)),
                                                                                                     x1,
                                                                                                     dbModule
                                                                                                        )));
                        }
                    }
                }
                else
                    return valdResponse;
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), supplierContacts);
            }
            finally
            {
                _repository.AutoSave = true;
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), null, exception);
        }
        #endregion

        #region Remove
        private bool IsRecordValidForRemove(IList<DomainModel.SupplierContact> supplierContacts,
                                            ref IList<DomainModel.SupplierContact> filteredSupplierContacts,
                                            ref IList<DbModel.SupplierContact> dbSupplierContacts,
                                            ref IList<DbModel.Supplier> dbSuppliers,
                                            ref IList<ValidationMessage> validationMessages)
        {
            bool result = false;

            if (supplierContacts != null && supplierContacts.Count > 0)
            {
                ValidationType validationType = ValidationType.Delete;
                IList<ValidationMessage> messages = new List<ValidationMessage>();
                if (validationMessages == null)
                    validationMessages = new List<ValidationMessage>();

                if (filteredSupplierContacts == null || filteredSupplierContacts.Count <= 0)
                    filteredSupplierContacts = FilterRecord(supplierContacts, validationType);

                if (filteredSupplierContacts?.Count > 0 && IsValidPayload(filteredSupplierContacts, validationType, ref validationMessages))
                {
                    this.GetSupplierContacts(filteredSupplierContacts, ref dbSupplierContacts);
                    IList<int> supplierContactIdsNotExists = null;
                    var supplierContactIds = filteredSupplierContacts.Select(x => x.SupplierContactId ?? 0).Distinct().ToList();

                    if (IsSupplierContactExistInDb(supplierContactIds, dbSupplierContacts, ref supplierContactIdsNotExists, ref validationMessages))
                        result = IsRecordCanBeDeleted(filteredSupplierContacts, dbSupplierContacts, ref validationMessages);
                }

                if (messages.Count > 0)
                    validationMessages.AddRange(messages);
            }
            return result;
        }

        private Response RemoveSupplierContacts(IList<DomainModel.SupplierContact> supplierContacts,
            IList<DbModel.SqlauditModule> dbModule,
                                            ref IList<DbModel.SupplierContact> dbSupplierContacts,
                                            ref IList<DbModel.Supplier> dbSuppliers,
                                            bool commitChange,
                                            bool isDbValidationRequire)
        {
            Exception exception = null;
            List<MessageDetail> errorMessages = null;
            List<ValidationMessage> validationMessages = null;
            long? eventId = 0;
           
            try
            {
                errorMessages = new List<MessageDetail>();
                validationMessages = new List<ValidationMessage>();
                eventId = supplierContacts?.FirstOrDefault()?.EventId;
                Response response = null;
                if (isDbValidationRequire)
                    response = IsRecordValidForProcess(supplierContacts, ValidationType.Delete, ref dbSupplierContacts, ref dbSuppliers);
                if (!isDbValidationRequire || (response.Code == ResponseType.Success.ToId() && Convert.ToBoolean(response.Result) && dbSupplierContacts?.Count > 0))
                {
                    _repository.AutoSave = false;
                    _repository.Delete(dbSupplierContacts);
                    if (commitChange)
                    {
                        int value = _repository.ForceSave();
                        if (value > 0)
                        {
                            dbSupplierContacts?.ToList().ForEach(x =>
                             supplierContacts?.ToList().ForEach(x1 => _auditSearchService.AuditLog(x1, ref eventId, x1.ActionByUser,
                                                                                                        null,
                                                                                                 ValidationType.Delete.ToAuditActionType(),
                                                                                                 SqlAuditModuleType.SupplierContact,
                                                                                                        x1,
                                                                                                       null,
                                                                                                       dbModule)));

                        }
                    }
                }
                else
                    return response;
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), supplierContacts);
            }
            finally
            {
                _repository.AutoSave = true;
            }

            return new Response().ToPopulate(ResponseType.Success, null, errorMessages, validationMessages, null, exception);
        }

        #endregion

        #region Common
        private Response CheckRecordValidForProcess(IList<DomainModel.SupplierContact> supplierContacts,
                                            ValidationType validationType,
                                            ref IList<DomainModel.SupplierContact> filteredSupplierContacts,
                                            ref IList<DbModel.SupplierContact> dbSupplierContacts,
                                            ref IList<DbModel.Supplier> dbSuppliers)
        {
            Exception exception = null;
            bool result = false;
            IList<ValidationMessage> validationMessages = null;
            try
            {
                if (validationType == ValidationType.Add)
                    result = IsRecordValidForAdd(supplierContacts, ref filteredSupplierContacts, ref dbSupplierContacts, ref dbSuppliers, ref validationMessages);
                else if (validationType == ValidationType.Delete)
                    result = IsRecordValidForRemove(supplierContacts, ref filteredSupplierContacts, ref dbSupplierContacts, ref dbSuppliers, ref validationMessages);
                else if (validationType == ValidationType.Update)
                    result = IsRecordValidForUpdate(supplierContacts, ref filteredSupplierContacts, ref dbSupplierContacts, ref dbSuppliers, ref validationMessages);
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), supplierContacts);
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), result, exception);
        }

        private IList<DomainModel.SupplierContact> FilterRecord(IList<DomainModel.SupplierContact> supplierContacts,
                                                        ValidationType filterType)
        {
            IList<DomainModel.SupplierContact> filterSupplierContacts = null;

            if (filterType == ValidationType.Add)
                filterSupplierContacts = supplierContacts?.Where(x => x.RecordStatus.IsRecordStatusNew()).ToList();
            else if (filterType == ValidationType.Update)
                filterSupplierContacts = supplierContacts?.Where(x => x.RecordStatus.IsRecordStatusModified()).ToList();
            else if (filterType == ValidationType.Delete)
                filterSupplierContacts = supplierContacts?.Where(x => x.RecordStatus.IsRecordStatusDeleted()).ToList();

            return filterSupplierContacts;
        }

        private bool IsValidPayload(IList<DomainModel.SupplierContact> supplierContacts,
                                    ValidationType validationType,
                                    ref IList<ValidationMessage> validationMessages)
        {
            IList<ValidationMessage> messages = new List<ValidationMessage>();
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            var validationResults = _validationService.Validate(JsonConvert.SerializeObject(supplierContacts), validationType);
            if (validationResults?.Count > 0)
                messages.Add(_messageDescriptions, ModuleType.Supplier, validationResults);

            if (validationResults.Count > 0)
                validationMessages.AddRange(messages);

            return validationMessages?.Count <= 0;
        }

        private bool IsValidSupplier(IList<DomainModel.SupplierContact> supplierContacts,
                                     ref IList<DbModel.Supplier> dbSupplier,
                                     ref IList<ValidationMessage> validationMessages)
        {
            bool result = true;
            if (dbSupplier?.Count > 0)
            {
                IList<int> supplierIds = supplierContacts.Select(x => x.SupplierId ?? 0).ToList();
                result = Convert.ToBoolean(_supplierService.IsRecordExistInDb(supplierIds, ref dbSupplier, ref validationMessages).Result);
            }

            return result;
        }

        private bool IsRecordUpdateCountMatching(IList<DomainModel.SupplierContact> supplierContacts,
                                                 IList<DbModel.SupplierContact> dbSupplierContacts,
                                                 ref IList<ValidationMessage> validationMessages)
        {
            List<ValidationMessage> messages = new List<ValidationMessage>();
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            var notMatchedRecords = supplierContacts.Where(x => !dbSupplierContacts.ToList().Any(x1 => x1.UpdateCount.ToInt() == x.UpdateCount.ToInt() && x1.Id == x.SupplierContactId)).ToList();
            notMatchedRecords.ForEach(x =>
            {
                messages.Add(_messageDescriptions, x.SupplierContactName, MessageType.SupplierContactUpdatedByOtherUser, x.SupplierContactName);
            });

            if (messages.Count > 0)
                validationMessages.AddRange(messages);

            return validationMessages?.Count <= 0;
        }

        private bool IsRecordCanBeDeleted(IList<DomainModel.SupplierContact> supplierContacts,
                                          IList<DbModel.SupplierContact> dbSupplierContacts,
                                          ref IList<ValidationMessage> validationMessages)
        {
            List<ValidationMessage> messages = new List<ValidationMessage>();

            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            supplierContacts.Where(x => dbSupplierContacts.Any(x1 => x.SupplierContactId == x1.Id && (x1.AssignmentSubSupplier.Count > 0)))
                            .ToList().ForEach(x2 =>
                            {
                                messages.Add(_messageDescriptions, x2.SupplierContactName, MessageType.SupplierContactCannotBeDeleted, x2.SupplierContactName);
                            });

            if (messages.Count > 0)
                validationMessages.AddRange(messages[0].ToArray()); //D305 

            return validationMessages.Count <= 0;
        }

        private bool IsSupplierContactExistInDb(IList<int> supplierContactIds,
                                                IList<DbModel.SupplierContact> dbSupplierContacts,
                                                ref IList<int> supplierContactIdsNotExists,
                                                ref IList<ValidationMessage> validationMessages)
        {
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            if (dbSupplierContacts == null)
                dbSupplierContacts = new List<DbModel.SupplierContact>();

            var validMessages = validationMessages;

            if (supplierContactIds?.Count > 0)
            {
                supplierContactIdsNotExists = supplierContactIds.Where(x => !dbSupplierContacts.Any(x1 => x1.Id == x)).Select(x => x).ToList();
                supplierContactIdsNotExists?.ToList().ForEach(x =>
                {
                    validMessages.Add(_messageDescriptions, x, MessageType.InvalidSupplierContact, x);
                });
            }

            if (validMessages.Count > 0)
                validationMessages = validMessages;

            return validationMessages.Count <= 0;
        }

        private void GetSupplierContacts(IList<DomainModel.SupplierContact> filteredSupplierContacts,
                                         ref IList<DbModel.SupplierContact> dbSupplierContacts,
                                         params Expression<Func<DbModel.SupplierContact, object>>[] includes)
        {
            IList<int> supplierContactIds = filteredSupplierContacts.Select(x => (int)x.SupplierContactId).Distinct().ToList();
            dbSupplierContacts = this.GetSupplierContactById(supplierContactIds, includes).ToList();
        }

        
        #endregion
        #endregion    
    }
}