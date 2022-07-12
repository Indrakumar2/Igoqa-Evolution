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
using Evolution.Supplier.Domain.Interfaces.Suppliers;
using Evolution.SupplierPO.Domain.Interfaces.Data;
using Evolution.SupplierPO.Domain.Interfaces.SupplierPO;
using Evolution.SupplierPO.Domain.Interfaces.Validations;
using Evolution.Assignment.Domain.Interfaces.Assignments;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Transactions;
using DbModels = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModels = Evolution.SupplierPO.Domain.Models.SupplierPO;
using Evolution.DbRepository.Models.SqlDatabaseContext;

namespace Evolution.SupplierPO.Core.Services
{
    public class SupplierPOSubSupplierService : ISupplierPOSubSupplierService
    {
        private ISupplierPOSubSupplierRepository _repository = null;
        private IAppLogger<SupplierPOSubSupplierService> _logger = null;
        private ISupplierPOService _supplierPOService = null;
        private ISupplierService _supplierService = null;
        private IMapper _mapper = null;
        private readonly JObject _messageDescriptions = null;
        private ISupplierPoSubSupplierValidationService _validationService = null;
        private readonly IAppLogger<LogEventGeneration> _applogger = null;
        private readonly IAuditSearchService _auditSearchService = null;
        private IAssignmentService _assignmentService = null;

        #region Constructor

        public SupplierPOSubSupplierService(ISupplierPOSubSupplierRepository repository,
                                            ISupplierService supplierService,
                                            ISupplierPOService supplierPOService,
                                            ISupplierPoSubSupplierValidationService validationService,
                                            IMapper mapper,
                                            IAppLogger<SupplierPOSubSupplierService> logger,
                                          //  IAuditLogger auditLogger,
                                            IAppLogger<LogEventGeneration> applogger,
                                            JObject messages,
                                            IAuditSearchService auditSearchService , IAssignmentService assignmentService)
        {
            _repository = repository;
            _supplierPOService = supplierPOService;
            _supplierService = supplierService;
            _logger = logger;
            _mapper = mapper;
            _validationService = validationService;
           // _auditLogger = auditLogger;
            _applogger = applogger;
            _messageDescriptions = messages;
            _auditSearchService = auditSearchService;
            _assignmentService = assignmentService;
        }

        #endregion

        #region Public Methods

        #region Get
        public Response Get(DomainModels.SupplierPOSubSupplier searchModel)
        {
            Exception exception = null;
            IList<DomainModels.SupplierPOSubSupplier> result = null;
            try
            {
                using (var tranScope = new TransactionScope(TransactionScopeOption.RequiresNew,
                                              new TransactionOptions
                                              {
                                                  IsolationLevel = IsolationLevel.ReadUncommitted
                                              }))
                {
                    result = _mapper.Map<IList<DomainModels.SupplierPOSubSupplier>>(this._repository.Search(searchModel, a => a.Supplier.Country.County,
                                                                                                                    b => b.Supplier.City));
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
        public Response Add(int supplierPoId,
                            IList<DomainModels.SupplierPOSubSupplier> subSuppliers,
                            bool commitChange = true,
                            bool isDbValildationRequired = true)
        {
            IList<DbModels.SupplierPurchaseOrderSubSupplier> dbSubSuppliers = null;
            IList<DbModels.SupplierPurchaseOrder> dbSupplierPos = null;
            IList<DbModels.Supplier> dbSuppliers = null;
            return AddSubSupplier(supplierPoId, subSuppliers,null, ref dbSubSuppliers, ref dbSupplierPos, ref dbSuppliers, commitChange, isDbValildationRequired);
        }

        public Response Add(int supplierPoId,
                            IList<DomainModels.SupplierPOSubSupplier> subSuppliers,
                            IList<DbModels.SqlauditModule> dbModule,
                            ref IList<DbModels.SupplierPurchaseOrderSubSupplier> dbSubSuppliers,
                            ref IList<DbModels.SupplierPurchaseOrder> dbSupplierPos,
                            ref IList<DbModels.Supplier> dbSuppliers,
                            bool commitChange = true,
                            bool isDbValidationRequired = true)
        {
            return AddSubSupplier(supplierPoId, subSuppliers, dbModule, ref dbSubSuppliers, ref dbSupplierPos, ref dbSuppliers, commitChange, isDbValidationRequired);
        }
        #endregion

        #region Modify
        public Response Modify(int supplierPoId,
                               IList<DomainModels.SupplierPOSubSupplier> subSuppliers,
                               bool commitChange = true,
                               bool isDbvalidationRequired = true)
        {
            IList<DbModels.SupplierPurchaseOrderSubSupplier> dbSubSuppliers = null;
            IList<DbModels.SupplierPurchaseOrder> dbSupplierPos = null;
            IList<DbModels.Supplier> dbSuppliers = null;
            IList<DomainModels.SupplierPOSubSupplier> subSupplierlist = null;
            return UpdateSubSupplier(supplierPoId, subSuppliers, subSupplierlist,null, ref dbSubSuppliers, ref dbSupplierPos, ref dbSuppliers, commitChange, isDbvalidationRequired);
        }

        public Response Modify(int supplierPoId,
                               IList<DomainModels.SupplierPOSubSupplier> subSuppliers,
                               IList<DbModels.SqlauditModule> dbModule,
                               ref IList<DbModels.SupplierPurchaseOrderSubSupplier> dbSubSuppliers,
                               ref IList<DbModels.SupplierPurchaseOrder> dbSupplierPos,
                               ref IList<DbModels.Supplier> dbSuppliers,
                               bool commitChange = true,
                               bool isDbvalidationRequired = true)
        {
            IList<DomainModels.SupplierPOSubSupplier> subSupplierlist = null;
            return UpdateSubSupplier(supplierPoId, subSuppliers, subSupplierlist, dbModule, ref dbSubSuppliers, ref dbSupplierPos, ref dbSuppliers, commitChange, isDbvalidationRequired);
        }

        #endregion

        #region Delete

        public Response Delete(int supplierPoId,
                               IList<DomainModels.SupplierPOSubSupplier> subSuppliers,
                               bool commitChange = true,
                               bool isDbvalidationRequired = true)
        {
            IList<DbModels.SupplierPurchaseOrderSubSupplier> dbSubSuppliers = null;
            IList<DbModels.SupplierPurchaseOrder> dbSupplierPos = null;
            IList<DbModels.Supplier> dbSuppliers = null;
            return RemoveSubSupplier(supplierPoId, subSuppliers,null, ref dbSubSuppliers, ref dbSupplierPos, ref dbSuppliers, commitChange, isDbvalidationRequired);
        }

        public Response Delete(int supplierPoId,
                             IList<DomainModels.SupplierPOSubSupplier> subSuppliers,
                             IList<DbModels.SupplierPurchaseOrderSubSupplier> dbSubSuppliers,
                             IList<DbModels.SqlauditModule> dbModule,
                             ref IList<DbModels.SupplierPurchaseOrder> dbSupplierPos,
                             ref IList<DbModels.Supplier> dbSuppliers,
                             bool commitChange = true,
                             bool isDbvalidationRequired = true)
        {
            return RemoveSubSupplier(supplierPoId, subSuppliers, dbModule, ref dbSubSuppliers, ref dbSupplierPos, ref dbSuppliers, commitChange, isDbvalidationRequired);
        }

        #endregion

        #region Record valid Check

        public Response IsRecordValidForProcess(int supplierPoId,
                                                IList<DomainModels.SupplierPOSubSupplier> subSuppliers,
                                                ValidationType validationType)
        {
            IList<DbModels.SupplierPurchaseOrderSubSupplier> dbSubSuppliers = null;
            IList<DbModels.SupplierPurchaseOrder> dbSupplierPos = null;
            IList<DbModels.Supplier> dbSuppliers = null;
            return IsRecordValidForProcess(supplierPoId, subSuppliers, validationType, ref dbSubSuppliers, ref dbSupplierPos, ref dbSuppliers);
        }

        public Response IsRecordValidForProcess(int supplierPoId,
                                                IList<DomainModels.SupplierPOSubSupplier> subSuppliers,
                                                ValidationType validationType,
                                                ref IList<DbModels.SupplierPurchaseOrderSubSupplier> dbSubSuppliers,
                                                ref IList<DbModels.SupplierPurchaseOrder> dbSupplierPos,
                                                ref IList<DbModels.Supplier> dbSuppliers)
        {
            IList<DomainModels.SupplierPOSubSupplier> filteredSubSuppliers = null;
            return CheckRecordValidForProcess(supplierPoId, subSuppliers, validationType, ref filteredSubSuppliers, ref dbSubSuppliers, ref dbSupplierPos, ref dbSuppliers);
        }

        public Response IsRecordValidForProcess(int supplierPoId,
                                                IList<DomainModels.SupplierPOSubSupplier> subSuppliers,
                                                ValidationType validationType,
                                                IList<DbModels.SupplierPurchaseOrderSubSupplier> dbSubSuppliers,
                                                IList<DbModels.SupplierPurchaseOrder> dbSupplierPos,
                                                IList<DbModels.Supplier> dbSuppliers)
        {
            return IsRecordValidForProcess(supplierPoId, subSuppliers, validationType, ref dbSubSuppliers, ref dbSupplierPos, ref dbSuppliers);
        }

        #endregion

        #region Common

        public bool IsValidSubSupplier(IList<int> subSupplierId,
                                       ref IList<DbModels.Assignment> dbAssignments,
                                       ref IList<DbModels.SupplierPurchaseOrderSubSupplier> dbSubSuppliers,
                                       ref IList<DbModels.Supplier> dbSuppliers,
                                       ref IList<ValidationMessage> messages,
                                       params Expression<Func<DbModels.SupplierPurchaseOrderSubSupplier, object>>[] includes)
        {
            IList<ValidationMessage> message = new List<ValidationMessage>();
            if (subSupplierId != null)
            {
                var dbSubSupplier = GetSubSuppliersById(subSupplierId, dbAssignments, contact => contact.Supplier.SupplierContact, sup => sup.Supplier);
                var subSupplierNotExists = subSupplierId?.Where(x => !dbSubSupplier.Any(x2 => x2.Id == x))?.ToList();
                subSupplierNotExists?.ForEach(x =>
                {
                    string errorCode = MessageType.SubSupplierDoesNotExist.ToId();
                    message.Add(_messageDescriptions, x, MessageType.SubSupplierDoesNotExist, x);
                });
                dbSubSuppliers = dbSubSupplier;
                dbSuppliers = dbSubSupplier.ToList().Select(x => x.Supplier).ToList();
                messages = message;
            }

            return messages?.Count <= 0;
        }

        public bool IsValidSubSupplierID(IList<int> subSupplierId,
                                     ref IList<DbModels.Assignment> dbAssignments,
                                     ref IList<DbModels.SupplierPurchaseOrderSubSupplier> dbSubSuppliers,
                                     ref IList<DbModels.Supplier> dbSuppliers,
                                     ref IList<ValidationMessage> messages,
                                     params Expression<Func<DbModels.SupplierPurchaseOrderSubSupplier, object>>[] includes)
        {
            IList<ValidationMessage> message = new List<ValidationMessage>();
            if (subSupplierId != null)
            {
                var dbSubSupplier = GetSubSuppliers(subSupplierId, dbAssignments, contact => contact.Supplier.SupplierContact, sup => sup.Supplier);
                var subSupplierNotExists = subSupplierId?.Where(x => dbSubSupplier.Any(x2 => x2.Id == x))?.ToList();

                if (subSupplierNotExists?.Count > 0)
                {
                    subSupplierNotExists?.ForEach(x =>
                    {
                        string errorCode = MessageType.SubSupplierDoesNotExist.ToId();
                        message.Add(_messageDescriptions, x, MessageType.SubSupplierDoesNotExist, x);
                    });
                    
                }
                dbSubSuppliers = dbSubSupplier;
                dbSuppliers = dbSubSupplier.ToList().Select(x => x.Supplier).ToList();
                messages = message;
            }

            return messages?.Count <= 0;
        }
        #endregion

        #endregion

        #region Private Methods

        #region Add
        private bool IsRecordValidForAdd(int supplierPoId,
                                         IList<DomainModels.SupplierPOSubSupplier> subSuppliers,
                                         ref IList<DomainModels.SupplierPOSubSupplier> filteredSubSuppliers,
                                         ref IList<DbModels.SupplierPurchaseOrderSubSupplier> dbSubSuppliers,
                                         ref IList<DbModels.SupplierPurchaseOrder> dbSupplierPos,
                                         ref IList<DbModels.Supplier> dbSuppliers,
                                         ref IList<ValidationMessage> validationMessages)
        {
            Exception exception = null;
            bool result = false;
            try
            {
                if (subSuppliers != null & subSuppliers.Count > 0)
                {
                    if (validationMessages == null)
                        validationMessages = new List<ValidationMessage>();

                    if (filteredSubSuppliers == null || filteredSubSuppliers.Count <= 0)
                        filteredSubSuppliers = FilterRecord(subSuppliers, ValidationType.Add);

                    var supplierPoIds = filteredSubSuppliers.Where(x => x.SupplierPOId > 0).Select(x =>(int) x.SupplierPOId).Distinct().ToList();
                    if (filteredSubSuppliers?.Count > 0 && IsValidPayload(filteredSubSuppliers, ValidationType.Add, ref validationMessages) &&
                                                           Convert.ToBoolean(_supplierPOService.IsRecordExistInDb(supplierPoIds, ref dbSupplierPos, ref validationMessages).Result))
                    {
                        var supplierIds = filteredSubSuppliers.Where(x => x.SupplierId > 0).Select(x =>(int) x.SupplierId).Distinct().ToList();

                        if (Convert.ToBoolean(_supplierService.IsRecordExistInDb(supplierIds, ref dbSuppliers, ref validationMessages).Result))
                            result = !IsSupplierAlreadyAssociated(filteredSubSuppliers, dbSupplierPos, ref validationMessages, ValidationType.Add);
                    }
                }
            }
            catch (Exception ex)
            {
                result = false;
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), subSuppliers);

            }
            return result;
        }

        private Response AddSubSupplier(int supplierPoId,
                                        IList<DomainModels.SupplierPOSubSupplier> subSuppliers,
                                        IList<DbModels.SqlauditModule> dbModule,
                                        ref IList<DbModels.SupplierPurchaseOrderSubSupplier> dbSubSuppliers,
                                        ref IList<DbModels.SupplierPurchaseOrder> dbSupplierPos,
                                        ref IList<DbModels.Supplier> dbSuppliers,
                                        bool commitChange = true,
                                        bool isDbValidationRequire = true)
        {
            Exception exception = null;
            IList<ValidationMessage> validationMessages = null;
            long? eventId = 0;
            try
            {
                Response valdResponse = null;
                var recordToAdd = FilterRecord(subSuppliers, ValidationType.Add);
                eventId = subSuppliers?.FirstOrDefault()?.EventId;
                if (isDbValidationRequire)
                    valdResponse = CheckRecordValidForProcess(supplierPoId, subSuppliers, ValidationType.Add, ref recordToAdd, ref dbSubSuppliers, ref dbSupplierPos, ref dbSuppliers);
                if (!isDbValidationRequire || Convert.ToBoolean(valdResponse.Result))
                {
                    _repository.AutoSave = false;
                    recordToAdd = recordToAdd.Select(x => { x.SubSupplierId = 0; x.SupplierPOId = supplierPoId; return x; }).ToList();
                    var mappedValues=_mapper.Map<IList<DbModels.SupplierPurchaseOrderSubSupplier>>(recordToAdd);
                    _repository.Add(mappedValues);
                    if (commitChange)
                    {
                        int value=_repository.ForceSave();
                        if (value > 0)
                        {
                            mappedValues?.ToList().ForEach(x1 => _auditSearchService.AuditLog(x1, ref eventId, subSuppliers?.FirstOrDefault()?.ActionByUser,
                                                                                                 null,
                                                                                                  ValidationType.Add.ToAuditActionType(),
                                                                                                  SqlAuditModuleType.SupplierPOSubSupplier,
                                                                                                   null,
                                                                                                    _mapper.Map<DomainModels.SupplierPOSubSupplier>(x1),
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
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), subSuppliers);
            }
            finally
            {
                _repository.AutoSave = true;
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), null, exception);
        }

        #endregion

        #region Modify
        private bool IsRecordValidForUpdate(int supplierPoId,
                                            IList<DomainModels.SupplierPOSubSupplier> subSuppliers,
                                            ref IList<DomainModels.SupplierPOSubSupplier> filteredSubSuppliers,
                                            ref IList<DbModels.SupplierPurchaseOrderSubSupplier> dbSubSuppliers,
                                            ref IList<DbModels.SupplierPurchaseOrder> dbSupplierPos,
                                            ref IList<DbModels.Supplier> dbSuppliers,
                                            ref IList<ValidationMessage> validationMessages)
        {

            bool result = false;
            if (subSuppliers != null && subSuppliers.Count > 0)
            {
                ValidationType validationType = ValidationType.Update;
                IList<ValidationMessage> messages = new List<ValidationMessage>();
                if (validationMessages == null)
                    validationMessages = new List<ValidationMessage>();

                if (filteredSubSuppliers == null || filteredSubSuppliers.Count <= 0)
                    filteredSubSuppliers = FilterRecord(subSuppliers, validationType);

                var supplierPoIds = filteredSubSuppliers.Where(x => x.SupplierPOId > 0).Select(x => (int)x.SupplierPOId).Distinct().ToList();
                if (filteredSubSuppliers?.Count > 0 && IsValidPayload(filteredSubSuppliers, validationType, ref validationMessages) &&
                                                       Convert.ToBoolean(_supplierPOService.IsRecordExistInDb(supplierPoIds, ref dbSupplierPos, ref validationMessages).Result))
                {
                    var supplierIds = filteredSubSuppliers.Where(x => x.SupplierId > 0).Select(x => (int)x.SupplierId).Distinct().ToList();
                    var subSupplierIds = filteredSubSuppliers.Where(x => x.SubSupplierId > 0).Select(x => (int)x.SubSupplierId).Distinct().ToList();
                    dbSubSuppliers = GetSubSuppliersById(subSupplierIds, null);

                    if (IsRecordExistsInDb(subSupplierIds, dbSubSuppliers, ref validationMessages))
                    {
                        if (Convert.ToBoolean(_supplierService.IsRecordExistInDb(supplierIds, ref dbSuppliers, ref validationMessages).Result))
                            if (!IsSupplierAlreadyAssociated(filteredSubSuppliers, dbSupplierPos, ref validationMessages, ValidationType.Update))
                                result = IsRecordUpdateCountMatching(filteredSubSuppliers, dbSubSuppliers, ref validationMessages);

                        //Hot Fix Id 669
                        if(result)
                        {
                            result = IsSubSupplierAlreadyAssociated(dbSubSuppliers, ref validationMessages);
                        }
                    }
                }
            }
            return result;
        }

        private Response UpdateSubSupplier(int supplierPoId,
                                           IList<DomainModels.SupplierPOSubSupplier> subSuppliers,
                                           IList<DomainModels.SupplierPOSubSupplier> subSupplierlist ,
                                           IList<DbModels.SqlauditModule> dbModule,
                                           ref IList<DbModels.SupplierPurchaseOrderSubSupplier> dbSubSuppliers,
                                           ref IList<DbModels.SupplierPurchaseOrder> dbSupplierPos,
                                           ref IList<DbModels.Supplier> dbSuppliers,
                                           bool commitChange = true,
                                           bool isDbValidationRequire = true)
        {
            Exception exception = null;
            IList<ValidationMessage> validationMessages = null;
            long? eventId = 0;
            try
            {
                Response valdResponse = null;
                IList<DbModels.AssignmentSubSupplier> assignmentSubSuppliers= new List<AssignmentSubSupplier>();
                List<DbModels.Assignment> AssignmentIds = new List<DbModels.Assignment>();
                IList<DomainModels.SupplierPOSubSupplier> subSuppliersdata = subSuppliers;
                IList<DomainModels.SupplierPOSubSupplier> dbExistingSupplierPoSubSupplier = new List<DomainModels.SupplierPOSubSupplier>();
                var recordToModify = FilterRecord(subSuppliers, ValidationType.Update);
                if (isDbValidationRequire)
                    valdResponse = CheckRecordValidForProcess(supplierPoId, subSuppliers, ValidationType.Update, ref recordToModify, ref dbSubSuppliers, ref dbSupplierPos, ref dbSuppliers);
                eventId = subSuppliers?.FirstOrDefault()?.EventId;
                if (!isDbValidationRequire || Convert.ToBoolean(valdResponse.Result))
                {
                    _repository.AutoSave = false;
                    dbSubSuppliers.ToList().ForEach(x =>
                    {
                        dbExistingSupplierPoSubSupplier.Add(ObjectExtension.Clone(_mapper.Map<DomainModels.SupplierPOSubSupplier>(x)));
                    });
                    #region D995 Sup supplier Modify 
                    IList<DbModels.SupplierPurchaseOrderSubSupplier> dbExistingSubsuppliers;
                    dbExistingSubsuppliers = dbSubSuppliers;
                    dbExistingSubsuppliers = dbSubSuppliers?.Where(a => a.SupplierPurchaseOrder.Assignment.Count > 0
                    && a.SupplierPurchaseOrder.Assignment.Any(b => b.AssignmentSubSupplier
                    .Any(c => c.SupplierId == a.SupplierId && c.SupplierType == "S"  && c.IsDeleted == false))).ToList();//&& c.IsFirstVisit == true
                    List<int>ExistingSupplierid = dbExistingSubsuppliers.Select(a => a.SupplierId).ToList();
                    if(ExistingSupplierid.Count>0)
                    {
                        _assignmentService.RemoveAsssubsupplier(supplierPoId, ExistingSupplierid,"S");
                    }
                    #endregion
                    dbSubSuppliers.ToList().ForEach(x =>
                    {
                        var subSupplierToModify = recordToModify.FirstOrDefault(x1 => x1.SubSupplierId == x.Id);
                        x.SupplierId = (int)subSupplierToModify.SupplierId;
                        x.LastModification = DateTime.UtcNow;
                        x.ModifiedBy = subSupplierToModify.ModifiedBy;
                        x.UpdateCount = subSupplierToModify.UpdateCount.CalculateUpdateCount();
                    });
                    List<int> supplierIDList = new List<int>();
                    foreach (var supplierId in dbExistingSubsuppliers)
                    {
                        supplierIDList.Add((int)supplierId.SupplierId);
                    }
                    if (dbExistingSubsuppliers.Count > 0)
                    {
                      //  _assignmentService.addAssigenmentSubSupplier(supplierPoId, supplierIDList, subSuppliers[0].ModifiedBy,"S");
                    }
                    if (commitChange)
                    {
                        int value = _repository.ForceSave();
                        if (value > 0)
                        {
                            dbSubSuppliers?.ToList().ForEach(x =>
                                  recordToModify?.ToList().ForEach(x1 => _auditSearchService.AuditLog(x1, ref eventId, x1.ActionByUser,
                                                                      null,
                                                                     ValidationType.Update.ToAuditActionType(),
                                                                     SqlAuditModuleType.SupplierPOSubSupplier,
                                                                      _mapper.Map<DomainModels.SupplierPOSubSupplier>(dbExistingSupplierPoSubSupplier?.FirstOrDefault(x2 => x2.SubSupplierId == x1.SubSupplierId)),
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
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), subSuppliers);
            }
            finally
            {
                _repository.AutoSave = true;
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), null, exception);
        }

        #endregion

        #region Remove
        private bool IsRecordValidForRemove(int supplierPoId,
                                            IList<DomainModels.SupplierPOSubSupplier> subSuppliers,
                                            ref IList<DomainModels.SupplierPOSubSupplier> filteredSubSuppliers,
                                            ref IList<DbModels.SupplierPurchaseOrderSubSupplier> dbSubSuppliers,
                                            ref IList<DbModels.SupplierPurchaseOrder> dbSupplierPos,
                                            ref IList<DbModels.Supplier> dbSuppliers,
                                            ref IList<ValidationMessage> validationMessages)
        {
            Exception exception = null;
            bool result = true;
            try
            {
                if (subSuppliers != null && subSuppliers.Count > 0)
                {
                    if (validationMessages == null)
                        validationMessages = new List<ValidationMessage>();

                    if (filteredSubSuppliers == null || filteredSubSuppliers.Count <= 0)
                        filteredSubSuppliers = FilterRecord(subSuppliers, ValidationType.Delete);

                    var supplierPoIds = filteredSubSuppliers.Where(x => x.SupplierPOId > 0).Select(x => (int)x.SupplierPOId).Distinct().ToList();
                    if (filteredSubSuppliers?.Count > 0 && IsValidPayload(filteredSubSuppliers, ValidationType.Delete, ref validationMessages) &&
                        Convert.ToBoolean(_supplierPOService.IsRecordExistInDb(supplierPoIds, ref dbSupplierPos, ref validationMessages).Result))
                    {
                        var subSupplierIds = filteredSubSuppliers.Where(x => x.SubSupplierId > 0).Select(x => (int)x.SubSupplierId).Distinct().ToList();
                        dbSubSuppliers = GetSubSuppliersById(subSupplierIds, null);
                        result = IsRecordCanBeDeleted(dbSubSuppliers, ref validationMessages);
                    }
                }
            }
            catch (Exception ex)
            {
                result = false;
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), subSuppliers);
            }
            return result;
        }
        #endregion

        #region Common

        private Response CheckRecordValidForProcess(int supplierPoId,
                                                    IList<DomainModels.SupplierPOSubSupplier> supplierPOSubSuppliers,
                                                    ValidationType validationType,
                                                    ref IList<DomainModels.SupplierPOSubSupplier> filteredSupplierPOSubSuppliers,
                                                    ref IList<DbModels.SupplierPurchaseOrderSubSupplier> dbSubSuppliers,
                                                    ref IList<DbModels.SupplierPurchaseOrder> dbSupplierPos,
                                                    ref IList<DbModels.Supplier> dbSuppliers)
        {
            Exception exception = null;
            bool result = false;
            IList<ValidationMessage> validationMessages = null;
            try
            {
                if (validationType == ValidationType.Add)
                    result = IsRecordValidForAdd(supplierPoId, supplierPOSubSuppliers, ref filteredSupplierPOSubSuppliers, ref dbSubSuppliers, ref dbSupplierPos, ref dbSuppliers, ref validationMessages);
                else if (validationType == ValidationType.Delete)
                    result = IsRecordValidForRemove(supplierPoId, supplierPOSubSuppliers, ref filteredSupplierPOSubSuppliers, ref dbSubSuppliers, ref dbSupplierPos, ref dbSuppliers, ref validationMessages);
                else if (validationType == ValidationType.Update)
                    result = IsRecordValidForUpdate(supplierPoId, supplierPOSubSuppliers, ref filteredSupplierPOSubSuppliers, ref dbSubSuppliers, ref dbSupplierPos, ref dbSuppliers, ref validationMessages);
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), supplierPOSubSuppliers);
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), result, exception);
        }

        private Response RemoveSubSupplier(int supplierPoId,
                                           IList<DomainModels.SupplierPOSubSupplier> subSuppliers,
                                           IList<DbModels.SqlauditModule> dbModule,
                                           ref IList<DbModels.SupplierPurchaseOrderSubSupplier> dbSubSuppliers,
                                           ref IList<DbModels.SupplierPurchaseOrder> dbSupplierPos,
                                           ref IList<DbModels.Supplier> dbSuppliers,
                                           bool commitChange = true,
                                           bool isDbValidationRequire = true)
        {
            Exception exception = null;
            List<MessageDetail> errorMessages = null;
            List<ValidationMessage> validationMessages = null;
            long? eventId = 0;

            try
            {
                errorMessages = new List<MessageDetail>();
                validationMessages = new List<ValidationMessage>();
                eventId = subSuppliers?.FirstOrDefault()?.EventId;
                Response response = null;
                if (isDbValidationRequire)
                    response = IsRecordValidForProcess(supplierPoId, subSuppliers, ValidationType.Delete, ref dbSubSuppliers, ref dbSupplierPos, ref dbSuppliers);

                IList<DbModels.SupplierPurchaseOrderSubSupplier> dbExistingSubsuppliers;
                dbExistingSubsuppliers = dbSubSuppliers;
                dbExistingSubsuppliers = dbSubSuppliers?.Where(a => a.SupplierPurchaseOrder.Assignment.Count > 0
                && a.SupplierPurchaseOrder.Assignment.Any(b => b.AssignmentSubSupplier
                .Any(c => c.SupplierId == a.SupplierId && c.SupplierType == "S"  && c.IsDeleted == false))).ToList();//&& c.IsFirstVisit == true
                List<int> ExistingSupplierid = dbExistingSubsuppliers.Select(a => a.SupplierId).ToList();
                if (ExistingSupplierid.Count > 0)
                {
                    _assignmentService.RemoveAsssubsupplier(supplierPoId, ExistingSupplierid,"S");
                }
                if (!isDbValidationRequire || Convert.ToBoolean(response.Result)) 
                {
               
                    _repository.AutoSave = false;
                    _repository.Delete(dbSubSuppliers);
                    if (commitChange)
                    {
                        int value = _repository.ForceSave();
                        if (value > 0)
                        {
                            subSuppliers?.ToList().ForEach(x1 => _auditSearchService.AuditLog(x1, ref eventId, x1.ActionByUser,
                                                                                               null,
                                                                                             ValidationType.Delete.ToAuditActionType(),
                                                                                               SqlAuditModuleType.SupplierPOSubSupplier,
                                                                                               x1,
                                                                                                null,
                                                                                                dbModule
                                                                                              ));
                        }



                    }
                }
                else
                    return response;
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), subSuppliers);
            }
            finally
            {
                _repository.AutoSave = true;
            }

            return new Response().ToPopulate(ResponseType.Success, null, errorMessages, validationMessages, null, exception);
        }

        private Response IsRecordValidForProcess(int supplierPoId,
                                                 IList<DomainModels.SupplierPOSubSupplier> subSuppliers,
                                                 ValidationType validationType,
                                                 ref IList<DomainModels.SupplierPOSubSupplier> filteredSubSuppliers,
                                                 ref IList<DbModels.SupplierPurchaseOrderSubSupplier> dbSubSuppliers,
                                                 ref IList<DbModels.SupplierPurchaseOrder> dbSupplierPos,
                                                 ref IList<DbModels.Supplier> dbSuppliers)
        {
            Exception exception = null;
            bool result = false;
            IList<ValidationMessage> validationMessages = null;
            try
            {
                if (subSuppliers != null & subSuppliers.Count > 0)
                {
                    if (validationMessages != null)
                        validationMessages = new List<ValidationMessage>();

                    if (filteredSubSuppliers == null || filteredSubSuppliers.Count <= 0)
                        filteredSubSuppliers = FilterRecord(subSuppliers, validationType);

                    var supplierPoIds = filteredSubSuppliers.Where(x => x.SupplierPOId > 0).Select(x => (int)x.SupplierPOId).Distinct().ToList();
                    if (filteredSubSuppliers?.Count > 0 && IsValidPayload(filteredSubSuppliers, validationType, ref validationMessages)
                                                        && Convert.ToBoolean(_supplierPOService.IsRecordExistInDb(supplierPoIds, ref dbSupplierPos, ref validationMessages).Result))
                    {
                        var supplierIds = filteredSubSuppliers.Where(x => x.SupplierId > 0).Select(x => (int)x.SupplierId).Distinct().ToList();
                        if (validationType == ValidationType.Add)
                        {
                            if (Convert.ToBoolean(_supplierService.IsRecordExistInDb(supplierIds, ref dbSuppliers, ref validationMessages).Result))
                                if (!IsSupplierAlreadyAssociated(filteredSubSuppliers, dbSupplierPos, ref validationMessages,ValidationType.Add))
                                    result = true;
                        }
                        else if (validationType == ValidationType.Update || validationType == ValidationType.Delete)
                        {
                            var subSupplierIds = filteredSubSuppliers.Where(x => x.SubSupplierId > 0).Select(x => (int)x.SubSupplierId).Distinct().ToList();
                            dbSubSuppliers = GetSubSuppliersById(subSupplierIds, null);
                            if (IsRecordExistsInDb(subSupplierIds, dbSubSuppliers, ref validationMessages))
                            {
                                if (validationType == ValidationType.Update)
                                {
                                    if (Convert.ToBoolean((_supplierService.IsRecordExistInDb(supplierIds, ref dbSuppliers, ref validationMessages).Result)))
                                        if (!IsSupplierAlreadyAssociated(filteredSubSuppliers, dbSupplierPos, ref validationMessages,ValidationType.Update))
                                            if (IsRecordUpdateCountMatching(filteredSubSuppliers, dbSubSuppliers, ref validationMessages))
                                                result = true;
                                }
                                else if (validationType == ValidationType.Delete)
                                {
                                    result = IsRecordCanBeDeleted(dbSubSuppliers, ref validationMessages);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result = false;
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), subSuppliers);
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), result, exception);
        }

        private IList<DbModels.Supplier> GetSuppliersById(IList<int> supplierIds)
        {
            IList<DbModels.Supplier> dbSuppliers = null;
            Response response = null;

            if (supplierIds?.Count > 0)
            {
                response = _supplierService.Get(supplierIds);
            }
            return dbSuppliers;
        }

        private IList<DbModels.SupplierPurchaseOrderSubSupplier> GetSubSuppliersById(IList<int> subSuplierIds, IList<DbModels.Assignment> dbAssignments, params Expression<Func<DbModels.SupplierPurchaseOrderSubSupplier, object>>[] includes)
        {
            IList<DbModels.SupplierPurchaseOrderSubSupplier> dbSubSuppliers = null;
            if (dbAssignments?.Count > 0)
                dbSubSuppliers = dbAssignments.SelectMany(x => x.SupplierPurchaseOrder.SupplierPurchaseOrderSubSupplier).ToList().Where(x => subSuplierIds.Contains(x.Id)).ToList();

            if (subSuplierIds?.Count > 0 && dbAssignments == null && dbSubSuppliers == null)
                dbSubSuppliers = _repository.FindBy(x => subSuplierIds.Contains(x.Id), includes).ToList();
            return dbSubSuppliers;
        }
        //Added for Assignment SubSupplier bug
        private IList<DbModels.SupplierPurchaseOrderSubSupplier> GetSubSuppliers(IList<int> subSuplierIds, IList<DbModels.Assignment> dbAssignments, params Expression<Func<DbModels.SupplierPurchaseOrderSubSupplier, object>>[] includes)
        {
            IList<DbModels.SupplierPurchaseOrderSubSupplier> dbSubSuppliers = null;
            if (dbAssignments?.Count > 0)
                dbSubSuppliers = dbAssignments.SelectMany(x => x.SupplierPurchaseOrder.SupplierPurchaseOrderSubSupplier).ToList().Where(x => subSuplierIds.Contains(x.SupplierId)).ToList();

            if (subSuplierIds?.Count > 0 && dbAssignments == null && dbSubSuppliers == null)
                dbSubSuppliers = _repository.FindBy(x => subSuplierIds.Contains(x.Id), includes).ToList();
            return dbSubSuppliers;
        }

        private IList<DomainModels.SupplierPOSubSupplier> FilterRecord(IList<DomainModels.SupplierPOSubSupplier> subSuppliers, ValidationType validationType)
        {
            IList<DomainModels.SupplierPOSubSupplier> filteredSubSuppliers = null;
            if (validationType == ValidationType.Add)
                filteredSubSuppliers = subSuppliers?.Where(x => x.RecordStatus.IsRecordStatusNew()).ToList();
            if (validationType == ValidationType.Update)
                filteredSubSuppliers = subSuppliers?.Where(x => x.RecordStatus.IsRecordStatusModified()).ToList();
            if (validationType == ValidationType.Delete)
                filteredSubSuppliers = subSuppliers?.Where(x => x.RecordStatus.IsRecordStatusDeleted()).ToList();
            return filteredSubSuppliers;
        }

        private bool IsValidPayload(IList<DomainModels.SupplierPOSubSupplier> subSuppliers, ValidationType validationType, ref IList<ValidationMessage> validationMessages)
        {
            var messages = new List<ValidationMessage>();

            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            var validationResults = _validationService.Validate(JsonConvert.SerializeObject(subSuppliers), validationType);

            if (validationResults?.Count > 0)
            {
                messages.Add(_messageDescriptions, ModuleType.Supplier, validationResults);
                validationMessages.AddRange(messages);
            }

            return validationMessages?.Count <= 0;
        }

        private bool IsValidSuppliers(IList<int> supplierIds,
                                      IList<DbModels.Supplier> dbSuppliers,
                                      ref IList<ValidationMessage> validationMessages,
                                      ref IList<int> supplierIdNotExists)
        {
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();
            if (dbSuppliers == null)
                dbSuppliers = new List<DbModels.Supplier>();

            var validMessages = validationMessages;

            supplierIdNotExists = supplierIds.Where(x => !dbSuppliers.Select(x1 => x1.Id).Contains(x)).ToList();
            supplierIdNotExists?.ToList().ForEach(x =>
            {
                validMessages.Add(_messageDescriptions, x, MessageType.InvalidSupplier, x);
            });

            validationMessages = validMessages;
            return validationMessages.Count <= 0;
        }        

        private bool IsSupplierAlreadyAssociated(IList<DomainModels.SupplierPOSubSupplier> subSuppliers,
                                                IList<DbModels.SupplierPurchaseOrder> dbSupplierPos,
                                                ref IList<ValidationMessage> validationMessages,
                                                ValidationType validationType
                                                )
        {
            var validMessages = validationMessages;
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();
            IList<int> supplierIdAlreadyExists = null;

            var dbSubSuppliers = dbSupplierPos.SelectMany(x => x.SupplierPurchaseOrderSubSupplier).ToList();

            if (validationType == ValidationType.Add)
                supplierIdAlreadyExists = subSuppliers?.Where(x => dbSubSuppliers.Any(x1 => x1.SupplierPurchaseOrderId == x.SupplierPOId
                                                            && x1.SupplierId == (int)x.SupplierId)).Select(x => (int)x.SupplierId).ToList();
            if (validationType == ValidationType.Update)
                supplierIdAlreadyExists = subSuppliers?.Where(x => dbSubSuppliers.Any(x1 => x1.SupplierPurchaseOrderId == x.SupplierPOId
                                                           && x1.SupplierId == (int)x.SupplierId
                                                           && x1.Id != x.SubSupplierId)).Select(x => (int)x.SupplierId).ToList();

            supplierIdAlreadyExists?.ToList().ForEach(x =>
            {
                validMessages.Add(_messageDescriptions, x, MessageType.SubSupplierAlreadyAssociated, x);
            });

            validationMessages = validMessages;

            return validationMessages.Count > 0;
        }

        //Hot Fix Id 669
        private bool IsSubSupplierAlreadyAssociated(IList<DbModels.SupplierPurchaseOrderSubSupplier> dbSubSuppliers, ref IList<ValidationMessage> validationMessages)
        {
            var validMessages = validationMessages;
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            var noUpdateRecords = dbSubSuppliers?.Where(a => a.SupplierPurchaseOrder.Assignment.Count > 0
              && a.SupplierPurchaseOrder.Assignment.Any(b => b.AssignmentSubSupplier
             .Any(c => c.AssignmentSubSupplierTechnicalSpecialist.Any(d => d.AssignmentSubSupplierId == c.Id)
              && c.SupplierId == a.SupplierId && c.SupplierType != "M"))).ToList();

            noUpdateRecords?.ForEach(x =>
            {
                validMessages.Add(_messageDescriptions, x.Id, MessageType.SubSupplierCannotBeUpdated, x.Id);
            });

            validationMessages = validMessages;

            return validationMessages.Count <= 0;
        }


        private bool IsRecordCanBeDeleted(IList<DbModels.SupplierPurchaseOrderSubSupplier> dbSubSuppliers, ref IList<ValidationMessage> validationMessages)
        {
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();
            var validMessages = validationMessages;
            //var notDeletableRecords = dbSubSuppliers?.Where(x => x.AssignmentSubSupplier.Count > 0).ToList();
            
            var notDeletableRecords = dbSubSuppliers?.Where(a => a.SupplierPurchaseOrder.Assignment.Count > 0 
              && a.SupplierPurchaseOrder.Assignment.Any(b => b.AssignmentSubSupplier
             .Any(c => c.AssignmentSubSupplierTechnicalSpecialist.Any(d => d.AssignmentSubSupplierId == c.Id) 
              && c.SupplierId == a.SupplierId && c.SupplierType != "M"))).ToList();//Scafold - MS-TS Link CR

            notDeletableRecords?.ForEach(x =>
            {
                validMessages.Add(_messageDescriptions, x.Id, MessageType.SubSupplierCannotBeDeleted, x.Id);
            });
            validationMessages = validMessages;

            return validationMessages.Count <= 0;
        }

        private bool IsRecordUpdateCountMatching(IList<DomainModels.SupplierPOSubSupplier> subSuppliers, IList<DbModels.SupplierPurchaseOrderSubSupplier> dbSubSuppliers, ref IList<ValidationMessage> validationMessages)
        {
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            var validMessages = validationMessages;

            var updateCountNotMatching = subSuppliers.Where(x => !(dbSubSuppliers.Any(x1 => x.UpdateCount.ToInt() == x1.UpdateCount.ToInt() && x.SubSupplierId == x1.Id))).ToList();

            updateCountNotMatching?.ForEach(x =>
            {
                validMessages.Add(_messageDescriptions, x, MessageType.SubSupplierAlreadyUpdated, x.SubSupplierName);
            });

            validationMessages = validMessages;

            return validationMessages.Count <= 0;
        }

        private bool IsRecordExistsInDb(IList<int> subSupplierIds, IList<DbModels.SupplierPurchaseOrderSubSupplier> dbSubSuppliers, ref IList<ValidationMessage> validationMessages)
        {
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            var validMessages = validationMessages;

            var supplierPoNotExists = subSupplierIds.Where(x => !dbSubSuppliers.Select(x1 => x1.Id).Contains(x)).ToList();

            supplierPoNotExists?.ForEach(x =>
            {
                validMessages.Add(_messageDescriptions, x, MessageType.SubSupplierDoesNotExist, x);
            });

            validationMessages = validMessages;

            return validationMessages.Count <= 0;
        }

        public Response Modify(int supplierPoId, IList<DomainModels.SupplierPOSubSupplier> subSuppliers, IList<DomainModels.SupplierPOSubSupplier> subSupplierlist, IList<SqlauditModule> dbModule, ref IList<SupplierPurchaseOrderSubSupplier> dbSubSuppliers, ref IList<SupplierPurchaseOrder> dbSupplierPos, ref IList<DbModels.Supplier> dbSuppliers, bool commitChange = true, bool isDbvalidationRequired = true)
        {
            return UpdateSubSupplier(supplierPoId, subSuppliers, subSupplierlist, dbModule, ref dbSubSuppliers, ref dbSupplierPos, ref dbSuppliers, commitChange, isDbvalidationRequired);
        }



        #endregion

        #endregion
    }
}
