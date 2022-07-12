using AutoMapper;
using Evolution.AuditLog.Domain.Enums;
using Evolution.AuditLog.Domain.Extensions;
using Evolution.AuditLog.Domain.Functions;
using Evolution.AuditLog.Domain.Interfaces.Audit;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Common.Models.Messages;
using Evolution.Common.Models.Responses;
using Evolution.DbRepository.Models.SqlDatabaseContext;
using Evolution.Document.Domain.Interfaces.Documents;
using Evolution.Document.Domain.Models.Document;
using Evolution.Logging.Interfaces;
using Evolution.SupplierPO.Domain.Interfaces.Data;
using Evolution.SupplierPO.Domain.Interfaces.SupplierPO;
using Evolution.SupplierPO.Domain.Models.SupplierPO;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.SupplierPO.Domain.Models.SupplierPO;

namespace Evolution.SupplierPO.Core.Services
{
    public class SupplierPODetailService : ISupplierPODetailService
    {
        private readonly IAppLogger<SupplierPODetailService> _logger = null;
        private readonly ISupplierPOService _supplierPOService = null;
        private readonly ISupplierPOSubSupplierService _supplierPOSubSupplierService = null;
        private readonly IDocumentService _supplierPODcoumentService = null;
        private readonly ISupplierPONoteService _supplierPONoteService = null;
        private readonly EvolutionSqlDbContext _dbContext = null;
        private readonly ISupplierPORepository _supplierPORepository = null;
        private readonly IAuditSearchService _auditSearchService = null;
        private readonly IMapper _mapper = null;
        private readonly JObject _messageDescriptions = null;

        #region Constructor

        public SupplierPODetailService(IAppLogger<SupplierPODetailService> logger,
                                        EvolutionSqlDbContext dbContext,
                                       ISupplierPOService supplierPOService,
                                       ISupplierPOSubSupplierService supplierSubSupplierService,
                                       IDocumentService supplierPODocumentService,
                                       ISupplierPONoteService supplierPONoteService,
                                       ISupplierPORepository supplierPORepository,
                                       IAuditSearchService auditSearchService,
                                       IMapper mapper,
                                       JObject messages)
        {
            this._logger = logger;
            this._dbContext = dbContext;
            this._supplierPOService = supplierPOService;
            this._supplierPOSubSupplierService = supplierSubSupplierService;
            this._supplierPODcoumentService = supplierPODocumentService;
            this._supplierPONoteService = supplierPONoteService;
            this._supplierPORepository = supplierPORepository;
            this._auditSearchService = auditSearchService;
            _mapper = mapper;
            _messageDescriptions = messages;
        }

        #endregion

        #region Public Methods
        public Response Add(SupplierPODetail supplierPODetail, bool commitChange = true)
        {
            Exception exception = null;
            IList<DbModel.SupplierPurchaseOrder> dbSupplierPos = null;
            IList<DbModel.SupplierPurchaseOrderSubSupplier> dbSubSuppliers = null;
            IList<DbModel.Supplier> dbSuppliers = null;
            IList<DbModel.Project> dbProjects = null;
            Response response = null;
            IList<ValidationMessage> validationMessages = null;
            int? supplierPOId = 0;

            try
            {
                if (supplierPODetail != null && supplierPODetail.SupplierPOInfo != null)
                {
                    response = ProcessSupplierPODetail(supplierPODetail, ref dbSupplierPos, ref dbSubSuppliers, ref dbSuppliers, ref dbProjects, ref supplierPOId, ValidationType.Add, commitChange);
                    if (response.Code != MessageType.Success.ToId())
                        return response;
                }
                else if (supplierPODetail == null || supplierPODetail?.SupplierPOInfo == null)
                {
                    var message = new List<ValidationMessage>
                        {
                            { _messageDescriptions, supplierPODetail, MessageType.InvalidPayLoad, supplierPODetail }
                        };
                    return new Response().ToPopulate(ResponseType.Warning, null, null, message, null, exception, null);
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), supplierPODetail);
            }
            finally
            {
                _supplierPORepository.AutoSave = true;
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), dbSupplierPos?.ToList().FirstOrDefault().Id, exception);
        }

        public Response Modify(SupplierPODetail supplierPODetail, bool commitChange = true)
        {
            Exception exception = null;
            IList<DbModel.SupplierPurchaseOrder> dbSupplierPos = null;
            IList<DbModel.SupplierPurchaseOrderSubSupplier> dbSubSuppliers = null;
            IList<DbModel.Supplier> dbSuppliers = null;
            IList<DbModel.Project> dbProjects = null;
            Response response = null;
            IList<ValidationMessage> validationMessages = null;
            int? id = 0;
            try
            {
                if (supplierPODetail != null && supplierPODetail.SupplierPOInfo != null)
                {
                    response = ProcessSupplierPODetail(supplierPODetail, ref dbSupplierPos, ref dbSubSuppliers, ref dbSuppliers, ref dbProjects, ref id, ValidationType.Update, commitChange);
                    if (response.Code == MessageType.Success.ToId())
                    {
                        _supplierPORepository.ForceSave();
                    }
                    else
                        return response;
                }
                else if (supplierPODetail == null || supplierPODetail?.SupplierPOInfo == null)
                {
                    var message = new List<ValidationMessage>
                        {
                            { _messageDescriptions, supplierPODetail, MessageType.InvalidPayLoad, supplierPODetail }
                        };
                    return new Response().ToPopulate(ResponseType.Warning, null, null, message, null, exception, null);
                }

            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), supplierPODetail);
            }
            finally
            {
                _supplierPORepository.AutoSave = true;
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), dbSupplierPos?.ToList().FirstOrDefault().Id, exception);
        }

        public Response Delete(SupplierPODetail supplierPODetail, bool commitChange = true)
        {
            Exception exception = null;
            IList<DbModel.SupplierPurchaseOrder> dbSupplierPo = null;
            Response response = null;
            IList<ValidationMessage> validationMessages = null;
            try
            {
                if (supplierPODetail != null && supplierPODetail.SupplierPOInfo != null)
                {
                    _supplierPORepository.AutoSave = true;
                    response = ProcessSupplierDeleteDetail(supplierPODetail, ref dbSupplierPo, ValidationType.Delete, true);
                    if (response.Code != MessageType.Success.ToId())
                        return response;
                }
                else if (supplierPODetail == null || supplierPODetail?.SupplierPOInfo == null)
                {
                    var message = new List<ValidationMessage>
                        {
                            { _messageDescriptions, supplierPODetail, MessageType.InvalidPayLoad, supplierPODetail }
                        };
                    return new Response().ToPopulate(ResponseType.Warning, null, null, message, null, exception, null);
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), supplierPODetail);
            }
            finally
            {
                _supplierPORepository.AutoSave = true;
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), null, exception);
        }

        #endregion

        #region Private Methods

        private Response ValidateSupplierPOInfo(DomainModel.SupplierPODetail supplierPODetail,
                                 ValidationType validationType,
                                 ref IList<DbModel.SupplierPurchaseOrder> dbSupplierPos,
                                 ref IList<DbModel.Supplier> dbSuppliers,
                                 ref IList<DbModel.Project> dbProjects)
        {
            Response response = null;
            if (validationType == ValidationType.Add)
                response = _supplierPOService.IsRecordValidForProcess(new List<DomainModel.SupplierPO> { supplierPODetail?.SupplierPOInfo }, validationType, ref dbSupplierPos, ref dbSuppliers, ref dbProjects);

            if (validationType == ValidationType.Update)
                response = _supplierPOService.IsRecordValidForProcess(new List<DomainModel.SupplierPO> { supplierPODetail?.SupplierPOInfo }, validationType, ref dbSupplierPos, ref dbSuppliers, ref dbProjects);

            if (validationType == ValidationType.Delete)
                response = _supplierPOService.IsRecordValidForProcess(new List<DomainModel.SupplierPO> { supplierPODetail?.SupplierPOInfo }, validationType, ref dbSupplierPos, ref dbSuppliers, ref dbProjects);

            return response;
        }

        private Response ValidateSupplierPOChild(int supplierPO,
                                DomainModel.SupplierPODetail supplierPODetail,
                                ref IList<DbModel.SupplierPurchaseOrderSubSupplier> dbSubSuppliers,
                                ref IList<DbModel.SupplierPurchaseOrder> dbSupplierPos,
                                ref IList<DbModel.Supplier> dbSuppliers,
                                ref IList<DbModel.SupplierPurchaseOrderNote> dbSupplierNotes)
        {
            Response response = new Response(ResponseType.Success.ToId(), ResponseType.Success.ToString());
            response = ValidSubSuppliers(supplierPO, supplierPODetail?.SupplierPOSubSupplier, ref dbSubSuppliers, ref dbSupplierPos, ref dbSuppliers);

            if (response != null && response.Code == ResponseType.Success.ToId() && Convert.ToBoolean(response.Result))
                response = ValidSupplierPONotes(supplierPODetail?.SupplierPONotes, ref dbSupplierPos, ref dbSupplierNotes);

            return response;
        }

        public Response ValidSubSuppliers(int supplierPOid,
                                           IList<DomainModel.SupplierPOSubSupplier> supplierPOSubSuppliers,
                                           ref IList<DbModel.SupplierPurchaseOrderSubSupplier> dbSubSuppliers,
                                           ref IList<DbModel.SupplierPurchaseOrder> dbSupplierPos,
                                           ref IList<DbModel.Supplier> dbSuppliers
                                           )
        {
            Response response = new Response(ResponseType.Success.ToId(), ResponseType.Success.ToString());
            if (supplierPOSubSuppliers != null)
            {
                // As Main Supplier and Sub Supplier varies
                dbSuppliers = null;
                var addSubSuppliers = supplierPOSubSuppliers?.Where(x => x.RecordStatus.IsRecordStatusNew()).ToList();
                var modSubSuppliers = supplierPOSubSuppliers?.Where(x => x.RecordStatus.IsRecordStatusModified()).ToList();
                var delSubSuppliers = supplierPOSubSuppliers?.Where(x => x.RecordStatus.IsRecordStatusDeleted()).ToList();

                if (addSubSuppliers?.Count > 0)
                {
                    dbSuppliers = null;
                    response = _supplierPOSubSupplierService.IsRecordValidForProcess(supplierPOid, addSubSuppliers, ValidationType.Add, ref dbSubSuppliers, ref dbSupplierPos, ref dbSuppliers);
                }

                if (modSubSuppliers?.Count > 0)
                {
                    dbSuppliers = null;
                    response = _supplierPOSubSupplierService.IsRecordValidForProcess(supplierPOid, modSubSuppliers, ValidationType.Update, ref dbSubSuppliers, ref dbSupplierPos, ref dbSuppliers);
                }

                if (delSubSuppliers?.Count > 0)
                {
                    dbSuppliers = null;
                    response = _supplierPOSubSupplierService.IsRecordValidForProcess(supplierPOid, delSubSuppliers, ValidationType.Delete, ref dbSubSuppliers, ref dbSupplierPos, ref dbSuppliers);

                }
            }

            return response;
        }

        public Response ValidSupplierPONotes(IList<DomainModel.SupplierPONote> supplierPONotes,
                                              ref IList<DbModel.SupplierPurchaseOrder> dbSupplierPos,
                                              ref IList<DbModel.SupplierPurchaseOrderNote> dbSupplierPoNotes)
        {
            Response response = new Response(ResponseType.Success.ToId(), ResponseType.Success.ToString());
            if (supplierPONotes != null)
            {
                var addNotes = supplierPONotes?.Where(x => x.RecordStatus.IsRecordStatusNew()).ToList();
                var delNotes = supplierPONotes?.Where(x => x.RecordStatus.IsRecordStatusDeleted()).ToList();
                if (addNotes?.Count > 0)
                    response = _supplierPONoteService.IsRecordValidForProcess(addNotes, ValidationType.Add, ref dbSupplierPoNotes, ref dbSupplierPos);

                if (delNotes?.Count > 0)
                    response = _supplierPONoteService.IsRecordValidForProcess(delNotes, ValidationType.Delete, ref dbSupplierPoNotes, ref dbSupplierPos);

            }

            return response;
        }


        private Response ProcessSupplierPODetail(DomainModel.SupplierPODetail supplierPODetail,
                                               ref IList<DbModel.SupplierPurchaseOrder> dbSupplierPos,
                                               ref IList<DbModel.SupplierPurchaseOrderSubSupplier> dbSubSuppliers,
                                               ref IList<DbModel.Supplier> dbSuppliers,
                                               ref IList<DbModel.Project> dbProjects,
                                               ref int? supplierPoID,
                                               ValidationType validationType,
                                               bool commitChange)
        {
            Response response = null;
            Exception exception = null;
            IList<DbModel.SupplierPurchaseOrderNote> dbSupplierPONotes = null;
            long? eventId = null;

            int? supplierPoId = supplierPoID;
            try
            {
                if (supplierPODetail != null)
                {
                    response = ValidateSupplierPOInfo(supplierPODetail, validationType, ref dbSupplierPos, ref dbSuppliers, ref dbProjects);
                    if (response.Code == MessageType.Success.ToId())
                    {
                        var dbModule = _auditSearchService.GetAuditModule(new List<string>() {
                                                SqlAuditModuleType.SupplierPO.ToString(),
                                                SqlAuditModuleType.SupplierPOSubSupplier.ToString(),
                                                SqlAuditModuleType.SupplierPODocument.ToString(),
                                                SqlAuditModuleType.SupplierPONote.ToString()

                                            });
                        //To-Do: Will create helper method get TransactionScope instance based on requirement
                        using (var tranScope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.Snapshot }))
                        //using (var tranScope = new TransactionScope())
                        {
                            supplierPoId = (supplierPODetail?.SupplierPOInfo?.SupplierPOId) ?? 0;
                            response = this.ProcessSupplierPOInfo(new List<DomainModel.SupplierPO> { supplierPODetail.SupplierPOInfo }, ref dbSupplierPos, ref dbSuppliers, ref dbProjects, ref eventId, validationType, commitChange, dbModule);

                            if (response != null && response.Code == MessageType.Success.ToId())
                                supplierPoId = dbSupplierPos?.ToList().FirstOrDefault().Id;

                            if (supplierPoId > 0)
                            {
                                supplierPODetail?.SupplierPOSubSupplier?.ToList().ForEach(x => x.SupplierPOId = supplierPoId);
                                supplierPODetail?.SupplierPONotes?.ToList().ForEach(x => { x.SupplierPOId = supplierPoId; });

                                response = ValidateSupplierPOChild((int)supplierPoId, supplierPODetail, ref dbSubSuppliers, ref dbSupplierPos, ref dbSuppliers, ref dbSupplierPONotes);
                                AppendEvent(supplierPODetail, eventId);
                                if (response != null && response.Code == ResponseType.Success.ToId())
                                    response = ProcessSubSupplier(supplierPODetail?.SupplierPOSubSupplier, dbSubSuppliers, dbSupplierPos, dbSuppliers, validationType, commitChange, (int)supplierPoId, dbModule);

                                if (response != null && response.Code == ResponseType.Success.ToId())
                                    response = ProcessSupplierPONote(supplierPODetail?.SupplierPONotes, dbSupplierPONotes, dbSupplierPos, validationType, commitChange, (int)supplierPoId, dbModule);

                                if (response != null && response.Code == ResponseType.Success.ToId())
                                    response = ProcessSupplierPODocument((int)supplierPoId, supplierPODetail?.SupplierPODocuments, validationType, commitChange, supplierPODetail, validationType.ToAuditActionType(), ref eventId, dbModule);

                                if (response.Code == ResponseType.Success.ToId())
                                {
                                    _supplierPORepository.AutoSave = false;
                                    _supplierPORepository.ForceSave();
                                    tranScope.Complete();

                                }
                            }
                        }
                    }
                    return response;
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), supplierPODetail);
            }
            finally
            {
                _supplierPORepository.AutoSave = true;
            }

            return response;
        }


        private Response ProcessSupplierDeleteDetail(DomainModel.SupplierPODetail supplierPODetail,
                                         ref IList<DbModel.SupplierPurchaseOrder> dbSupplierPos,
                                         ValidationType validationType,
                                         bool commitChange)
        {
            Response response = null;
            Exception exception = null;
            IList<DbModel.Supplier> dbSuppliers = null;
            IList<DbModel.SupplierPurchaseOrderSubSupplier> dbSubSuppliers = null;
            IList<DbModel.Project> dbProjects = null;
            long? eventId = null;
            try
            {
                if (supplierPODetail != null)
                {
                    response = ValidateSupplierPOInfo(supplierPODetail, ValidationType.Delete, ref dbSupplierPos, ref dbSuppliers, ref dbProjects);
                    supplierPODetail.SupplierPONotes = supplierPODetail?.SupplierPONotes?.Select(x => { x.RecordStatus = "D"; return x; }).ToList();
                    supplierPODetail.SupplierPOSubSupplier = supplierPODetail?.SupplierPOSubSupplier?.Select(x => { x.RecordStatus = "D"; return x; }).ToList();
                    supplierPODetail.SupplierPODocuments = supplierPODetail?.SupplierPODocuments?.Select(x => { x.RecordStatus = "D"; return x; }).ToList();

                    if (response != null && response.Code == ResponseType.Success.ToId())
                    {
                        var dbModule = _auditSearchService.GetAuditModule(new List<string>() {
                                                SqlAuditModuleType.SupplierPO.ToString(),
                                                SqlAuditModuleType.SupplierPOSubSupplier.ToString(),
                                                SqlAuditModuleType.SupplierPODocument.ToString(),
                                                SqlAuditModuleType.SupplierPONote.ToString()

                                            });

                        response = _supplierPOSubSupplierService.IsRecordValidForProcess((int)supplierPODetail.SupplierPOInfo.SupplierPOId,
                                                                                        supplierPODetail.SupplierPOSubSupplier?.ToList(),
                                                                                        ValidationType.Delete,
                                                                                        ref dbSubSuppliers,
                                                                                        ref dbSupplierPos,
                                                                                        ref dbSuppliers);
                        if (response != null && response.Code == ResponseType.Success.ToId())
                        {
                            AppendEvent(supplierPODetail, eventId);
                            response = ProcessSupplierPODocument((int)supplierPODetail.SupplierPOInfo.SupplierPOId, supplierPODetail?.SupplierPODocuments, ValidationType.Delete, commitChange, supplierPODetail, validationType.ToAuditActionType(), ref eventId, dbModule);
                            if (response != null && response.Code == ResponseType.Success.ToId())
                            {
                                //To-Do: Will create helper method get TransactionScope instance based on requirement
                                using (var tranScope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.Snapshot }))
                                //using (var tranScope = new TransactionScope())
                                {
                                    var count = _supplierPORepository.DeleteSupplierPO((int)supplierPODetail.SupplierPOInfo.SupplierPOId);
                                    if (count > 0)
                                    {
                                        response = _auditSearchService.AuditLog(supplierPODetail, ref eventId, supplierPODetail.SupplierPOInfo?.ActionByUser?.ToString(), "{" + AuditSelectType.Id + ":" + supplierPODetail?.SupplierPOInfo?.SupplierPOId + "}${" + AuditSelectType.Number + ":" + supplierPODetail?.SupplierPOInfo?.SupplierPONumber?.Trim() + "}${"
                                                                         + AuditSelectType.ProjectNumber + ":" + supplierPODetail?.SupplierPOInfo?.SupplierPOProjectNumber + "}${" + AuditSelectType.CustomerProjectName + ":" + supplierPODetail?.SupplierPOInfo?.SupplierPOCustomerProjectName?.Trim() + "}", SqlAuditActionType.D,
                                                                          SqlAuditModuleType.SupplierPO, supplierPODetail.SupplierPOInfo, null,dbModule);
                                        tranScope.Complete();
                                        response = new Response(ResponseType.Success.ToId(), ResponseType.Success.ToString());
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), supplierPODetail);
            }
            finally
            {
                _supplierPORepository.AutoSave = true;
            }

            return response;
        }

        private Response ProcessSupplierPOInfo(IList<DomainModel.SupplierPO> supplierPOs,
                                             ref IList<DbModel.SupplierPurchaseOrder> dbSupplierPOs,
                                             ref IList<DbModel.Supplier> dbSuppliers,
                                             ref IList<DbModel.Project> dbProjects,
                                             ref long? eventId,
                                             ValidationType validationType,
                                             bool commitChanges,
                                             IList<DbModel.SqlauditModule> dbModule)
        {
            Exception exception = null;
            try
            {
                if (supplierPOs != null)
                {
                    if (validationType == ValidationType.Delete)
                        return this._supplierPOService.Delete(supplierPOs, dbModule, ref dbSupplierPOs, ref dbSuppliers, ref dbProjects, ref eventId, commitChanges, false);
                    else if (validationType == ValidationType.Add)
                        return this._supplierPOService.Add(supplierPOs, dbModule, ref dbSupplierPOs, ref dbSuppliers, ref dbProjects, ref eventId, commitChanges, false);
                    else if (validationType == ValidationType.Update)
                        return this._supplierPOService.Modify(supplierPOs, dbModule, ref dbSupplierPOs, ref dbSuppliers, ref dbProjects, ref eventId, commitChanges, false);
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), supplierPOs);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, null, exception);
        }

        private Response ProcessSubSupplier(IList<DomainModel.SupplierPOSubSupplier> subSuppliers,
                                                 IList<DbModel.SupplierPurchaseOrderSubSupplier> dbSubSuppliers,
                                                 IList<DbModel.SupplierPurchaseOrder> dbSupplierPos,
                                                 IList<DbModel.Supplier> dbSuppliers,
                                                 ValidationType validationType,
                                                 bool commitChanges,
                                                 int supplierPoId,
                                                 IList<DbModel.SqlauditModule> dbModule)
        {
            Response response = new Response(ResponseType.Success.ToId(), ResponseType.Success.ToString());
            Exception exception = null;
            try
            {
                if (subSuppliers != null)
                {
                    var addSubSupplier = subSuppliers?.Where(x => x.RecordStatus.IsRecordStatusNew()).ToList();
                    var modSubSupplier = subSuppliers?.Where(x => x.RecordStatus.IsRecordStatusModified()).ToList();
                    var delSubSupplier = subSuppliers?.Where(x => x.RecordStatus.IsRecordStatusDeleted()).ToList();

                    if (addSubSupplier?.Count > 0)
                    {
                        if (supplierPoId > 0)
                            subSuppliers?.ToList().ForEach(x => { x.SupplierPOId = supplierPoId; });
                        response = this._supplierPOSubSupplierService.Add(supplierPoId, addSubSupplier, dbModule, ref dbSubSuppliers, ref dbSupplierPos, ref dbSuppliers, commitChanges, false);
                    }

                    if (modSubSupplier?.Count > 0 && response.Code == MessageType.Success.ToId())
                    {
                        dbSubSuppliers = dbSupplierPos?.SelectMany(x => x.SupplierPurchaseOrderSubSupplier).ToList().Where(x1 => modSubSupplier.Any(x2 => x1.Id == x2.SubSupplierId)).ToList();
                        if (supplierPoId > 0)
                            subSuppliers?.ToList().ForEach(x => { x.SupplierPOId = supplierPoId; });
                        response = this._supplierPOSubSupplierService.Modify(supplierPoId, modSubSupplier, subSuppliers, dbModule, ref dbSubSuppliers, ref dbSupplierPos, ref dbSuppliers, commitChanges, false);
                    }

                    if (delSubSupplier?.Count > 0 && response.Code == MessageType.Success.ToId())
                    {
                        dbSubSuppliers = dbSupplierPos?.SelectMany(x => x.SupplierPurchaseOrderSubSupplier).ToList().Where(x1 => delSubSupplier.Any(x2 => x1.Id == x2.SubSupplierId)).ToList();
                        if (supplierPoId > 0)
                            subSuppliers?.ToList().ForEach(x => { x.SupplierPOId = supplierPoId; });
                        response = this._supplierPOSubSupplierService.Delete(supplierPoId, delSubSupplier, dbSubSuppliers, dbModule, ref dbSupplierPos, ref dbSuppliers, commitChanges, false);
                    }

                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), subSuppliers);
            }

            return response;
        }

        private Response ProcessSupplierPONote(IList<DomainModel.SupplierPONote> supplierPONotes,
                                             IList<DbModel.SupplierPurchaseOrderNote> dbSupplierPONotes,
                                             IList<DbModel.SupplierPurchaseOrder> dbSupplierPOs,
                                             ValidationType validationType,
                                             bool commitChanges,
                                             int supplierPOId,
                                             IList<DbModel.SqlauditModule> dbModule)
        {
            Response response = new Response(ResponseType.Success.ToId(), ResponseType.Success.ToString());
            Exception exception = null;
            try
            {
                if (supplierPONotes != null)
                {
                    var addNotes = supplierPONotes?.Where(x => x.RecordStatus.IsRecordStatusNew()).ToList();
                    var delNotes = supplierPONotes?.Where(x => x.RecordStatus.IsRecordStatusDeleted()).ToList();
                    var updateNotes = supplierPONotes?.Where(x => x.RecordStatus.IsRecordStatusModified()).ToList();       //D661 issue 8 

                    if (addNotes?.Count > 0)
                    {
                        if (supplierPOId > 0)
                            supplierPONotes?.ToList().ForEach(x => { x.SupplierPOId = supplierPOId; });
                        response = _supplierPONoteService.Add(addNotes, dbModule, ref dbSupplierPONotes, ref dbSupplierPOs, commitChanges, false);
                    }

                    if (delNotes?.Count > 0 && response.Code == MessageType.Success.ToId())
                    {
                        dbSupplierPONotes = dbSupplierPOs?.SelectMany(x => x.SupplierPurchaseOrderNote).ToList().Where(x1 => delNotes.Any(x2 => x1.Id == x2.SupplierPONoteId)).ToList();
                        if (supplierPOId > 0)
                            supplierPONotes?.ToList().ForEach(x => { x.SupplierPOId = supplierPOId; });
                        response = this._supplierPONoteService.Delete(delNotes, dbSupplierPONotes, dbModule, ref dbSupplierPOs, commitChanges, false);
                    }
                    //D661 issue 8 Start
                    if (updateNotes?.Count > 0 && response.Code == MessageType.Success.ToId())
                    {
                        dbSupplierPONotes = dbSupplierPOs?.SelectMany(x => x.SupplierPurchaseOrderNote).ToList().Where(x1 => updateNotes.Any(x2 => x1.Id == x2.SupplierPONoteId)).ToList();
                        if (supplierPOId > 0)
                            supplierPONotes?.ToList().ForEach(x => { x.SupplierPOId = supplierPOId; });
                        response = this._supplierPONoteService.Update(updateNotes, dbModule, ref dbSupplierPONotes, ref dbSupplierPOs, commitChanges, false);
                    }
                    //D661 issue 8 End
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), supplierPONotes);
            }

            return response;
        }

        private Response ProcessSupplierPODocument(int supplierPOId, IList<ModuleDocument> supplierPODocuments, ValidationType validationType, bool commitChanges, DomainModel.SupplierPODetail supplierPODetail, SqlAuditActionType sqlAuditActionType, ref long? eventId, IList<DbModel.SqlauditModule> dbModule)
        {
            Response response = new Response(ResponseType.Success.ToId(), ResponseType.Success.ToString());
            Exception exception = null;
            List<DbModel.Document> dbDocuments = null;
            try
            {
                if (supplierPODocuments != null)
                {
                    supplierPODocuments?.ToList().ForEach(x => { x.ModuleRefCode = supplierPOId.ToString(); });
                    supplierPODetail.SupplierPODocuments = supplierPODocuments;
                    var auditSupplierPODetails = ObjectExtension.Clone(supplierPODetail);

                    var addDocuments = supplierPODocuments?.Where(x => x.RecordStatus.IsRecordStatusNew()).ToList();
                    var modDocuments = supplierPODocuments?.Where(x => x.RecordStatus.IsRecordStatusModified()).ToList();
                    var delDocuments = supplierPODocuments?.Where(x => x.RecordStatus.IsRecordStatusDeleted()).ToList();

                    if (addDocuments.Count > 0)
                    {
                        addDocuments?.ToList().ForEach(x => { x.ModuleRefCode = supplierPOId.ToString(); });
                        response = this._supplierPODcoumentService.Save(addDocuments, ref dbDocuments, commitChanges);
                        auditSupplierPODetails.SupplierPODocuments = addDocuments.Select(x =>
                        {
                            x.CreatedOn = dbDocuments?.FirstOrDefault(y => y.DocumentUniqueName == x.DocumentUniqueName)?.CreatedDate;
                            return x;
                        }).ToList(); // audit create date issue fix
                    }

                    if (modDocuments.Count > 0)
                    {
                        modDocuments?.ToList().ForEach(x => { x.ModuleRefCode = supplierPOId.ToString(); });
                        response = this._supplierPODcoumentService.Modify(modDocuments, ref dbDocuments, commitChanges);
                    }

                    if (delDocuments.Count > 0)
                    {
                        delDocuments?.ToList().ForEach(x => { x.ModuleRefCode = supplierPOId.ToString(); });
                        response = this._supplierPODcoumentService.Delete(delDocuments, commitChanges);
                    }
                    if (response.Code == MessageType.Success.ToId())
                    {
                        DocumentAudit(auditSupplierPODetails.SupplierPODocuments, sqlAuditActionType, auditSupplierPODetails, ref eventId, ref dbDocuments, dbModule);
                    }

                    return response;
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), supplierPODocuments);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, null, exception);
        }

        #endregion

        #region Private Exposed Methods

        private void DocumentAudit(IList<ModuleDocument> supplierPODocuments, SqlAuditActionType sqlAuditActionType, DomainModel.SupplierPODetail supplierPODetail, ref long? eventId, ref List<DbModel.Document> dbDocuments, IList<DbModel.SqlauditModule> dbModule)
        {
            //For Document Audit
            if (supplierPODocuments.Count > 0)
            {
                object newData;
                object oldData;
                var newDocument = supplierPODocuments?.Where(x => x.RecordStatus.IsRecordStatusNew())?.ToList();
                var modifiedDocument = supplierPODocuments?.Where(x => x.RecordStatus.IsRecordStatusModified())?.ToList();
                var deletedDocument = supplierPODocuments?.Where(x => x.RecordStatus.IsRecordStatusDeleted())?.ToList();
                if (newDocument.Count > 0)
                {
                    newData = newDocument;
                    _auditSearchService.AuditLog(supplierPODetail, ref eventId, supplierPODetail?.SupplierPOInfo?.ActionByUser?.ToString(), null, sqlAuditActionType, SqlAuditModuleType.SupplierPODocument, null, newData,dbModule);
                }
                if (modifiedDocument.Count > 0)
                {
                    newData = modifiedDocument?.OrderBy(x => x.Id)?.ToList();
                    oldData = (dbDocuments != null && dbDocuments.Count > 0) ? _mapper.Map<List<ModuleDocument>>(dbDocuments)?.OrderBy(x => x.Id)?.ToList() : null;
                    _auditSearchService.AuditLog(supplierPODetail, ref eventId, supplierPODetail?.SupplierPOInfo?.ActionByUser?.ToString(), null, sqlAuditActionType, SqlAuditModuleType.SupplierPODocument, oldData, newData,dbModule);
                }
                if (deletedDocument.Count > 0)
                {
                    oldData = deletedDocument;
                    _auditSearchService.AuditLog(supplierPODetail, ref eventId, supplierPODetail?.SupplierPOInfo?.ActionByUser?.ToString(), null, sqlAuditActionType, SqlAuditModuleType.SupplierPODocument, oldData, null,dbModule);
                }
            }
        }

        private void AppendEvent(DomainModel.SupplierPODetail supplierPODetail,
                         long? eventId)
        {
            ObjectExtension.SetPropertyValue(supplierPODetail.SupplierPOInfo, "EventId", eventId);
            ObjectExtension.SetPropertyValue(supplierPODetail.SupplierPOSubSupplier, "EventId", eventId);
            ObjectExtension.SetPropertyValue(supplierPODetail.SupplierPONotes, "EventId", eventId);
            ObjectExtension.SetPropertyValue(supplierPODetail.SupplierPODocuments, "EventId", eventId);
        }

        private void RollbackTransaction()
        {
            if (_dbContext.Database.CurrentTransaction != null)
                _dbContext.Database.RollbackTransaction();
        }
        #endregion
    }
}