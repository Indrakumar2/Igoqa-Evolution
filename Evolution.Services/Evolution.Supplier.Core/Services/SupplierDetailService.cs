using AutoMapper;
using Evolution.AuditLog.Domain.Enums;
using Evolution.AuditLog.Domain.Extensions;
using Evolution.AuditLog.Domain.Interfaces.Audit;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Common.Models.Messages;
using Evolution.Common.Models.Responses;
using Evolution.DbRepository.Models.SqlDatabaseContext;
using Evolution.Document.Domain.Interfaces.Documents;
using Evolution.Document.Domain.Models.Document;
using Evolution.Logging.Interfaces;
using Evolution.Supplier.Domain.Interfaces.Data;
using Evolution.Supplier.Domain.Interfaces.Suppliers;
using Evolution.Supplier.Domain.Models.Supplier;
using Evolution.SupplierContacts.Domain.Interfaces.Suppliers;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.Supplier.Domain.Models.Supplier;

namespace Evolution.Supplier.Core.Services
{
    public class SupplierDetailService : ISupplierDetailService
    {
        private readonly IAppLogger<SupplierDetailService> _logger = null;
        private readonly ISupplierService _supplierService = null;
        private readonly ISupplierContactService _supplierContactService = null;
        private readonly IDocumentService _supplierDocumentService = null;
        private readonly ISupplierNoteService _supplierNoteService = null;
        private readonly EvolutionSqlDbContext _dbContext = null;
        private readonly ISupplierRepository _repository = null;
        private readonly IAuditSearchService _auditSearchService = null;
        private readonly IMapper _mapper = null;
        private readonly JObject _messageDescriptions = null;

        #region Constructor
        public SupplierDetailService(IAppLogger<SupplierDetailService> logger,
                                     EvolutionSqlDbContext dbContext,
                                     ISupplierService supplierService,
                                     ISupplierContactService supplierContactService,
                                     IDocumentService supplierDocumentService,
                                     ISupplierNoteService supplierNoteService,
                                     ISupplierRepository repository,
                                     IAuditSearchService auditSearchService,
                                     IMapper mapper,
                                     JObject messages)
        {
            this._logger = logger;
            this._dbContext = dbContext;
            this._supplierService = supplierService;
            this._supplierContactService = supplierContactService;
            this._supplierDocumentService = supplierDocumentService;
            this._supplierNoteService = supplierNoteService;
            this._repository = repository;
            this._auditSearchService = auditSearchService;
            _mapper = mapper;
            _messageDescriptions = messages;
        }
        #endregion
        #region Public Methods

        public Response Get(DomainModel.SupplierDetail searchModel)
        {
            throw new NotImplementedException();
        }

        public Response Add(SupplierDetail supplierDetail, bool commitChange = true)
        {
            Exception exception = null;
            IList<DbModel.Supplier> dbSupplier = null;
            Response response = new Response(ResponseType.Success.ToId(), ResponseType.Success.ToString());
            IList<ValidationMessage> validationMessages = null;
            try
            {
                if (supplierDetail != null && supplierDetail.SupplierInfo != null)
                {
                    _repository.AutoSave = true;
                    response = ProcessSupplierDetail(supplierDetail, ref dbSupplier, ValidationType.Add, commitChange);
                    if (response.Code != MessageType.Success.ToId())
                    {
                        return response;
                    }
                }
                else if (supplierDetail == null || supplierDetail?.SupplierInfo == null)
                {
                    var message = new List<ValidationMessage>
                        {
                            { _messageDescriptions, supplierDetail, MessageType.InvalidPayLoad, supplierDetail }
                        };
                    return new Response().ToPopulate(ResponseType.Warning, null, null, message, null, exception, null);
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), supplierDetail);
            }
            finally
            {
                _repository.AutoSave = true;
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), dbSupplier?.ToList()?.FirstOrDefault()?.Id, exception);
        }

        public Response Modify(SupplierDetail supplierDetail, bool commitChange = true)
        {
            Exception exception = null;
            IList<DbModel.Supplier> dbSupplier = null;
            Response response = new Response(ResponseType.Success.ToId(), ResponseType.Success.ToString());
            IList<ValidationMessage> validationMessages = null;
            try
            {
                if (supplierDetail != null && supplierDetail.SupplierInfo != null)
                {
                    _repository.AutoSave = true;
                    response = ProcessSupplierDetail(supplierDetail, ref dbSupplier, ValidationType.Update, commitChange);
                    if (response.Code != MessageType.Success.ToId())
                    {
                        return response;
                    }
                    return response;
                }
                else if (supplierDetail == null || supplierDetail?.SupplierInfo == null)
                {
                    var message = new List<ValidationMessage>
                        {
                            { _messageDescriptions, supplierDetail, MessageType.InvalidPayLoad, supplierDetail }
                        };
                    return new Response().ToPopulate(ResponseType.Warning, null, null, message, null, exception, null);
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), supplierDetail);
            }
            finally
            {
                _repository.AutoSave = true;
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), dbSupplier?.ToList()?.FirstOrDefault()?.Id, exception);
        }

        public Response Delete(DomainModel.SupplierDetail supplierDetail, bool commitChange = true)
        {
            Exception exception = null;
            IList<DbModel.Supplier> dbSupplier = null;
            Response response = new Response(ResponseType.Success.ToId(), ResponseType.Success.ToString());

            try
            {
                if (supplierDetail != null && supplierDetail.SupplierInfo != null)
                {
                    _repository.AutoSave = true;
                    response = ProcessSupplierDeleteDetail(supplierDetail, ref dbSupplier, ValidationType.Delete, commitChange);
                }
                else if (supplierDetail == null || supplierDetail?.SupplierInfo == null)
                {
                    var message = new List<ValidationMessage>
                        {
                            { _messageDescriptions, supplierDetail, MessageType.InvalidPayLoad, supplierDetail }
                        };
                    return new Response().ToPopulate(ResponseType.Warning, null, null, message, null, exception, null);
                }

            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), supplierDetail);
            }
            finally
            {
                _repository.AutoSave = true;
            }

            return response;
        }

        #endregion

        #region Private Metods

        private Response ValidateSupplierInfo(DomainModel.SupplierDetail supplierDetail,
                                  ValidationType validationType,
                                  ref IList<DbModel.Supplier> dbSuppliers,
                                  ref IList<DbModel.Country> dbCountries,
                                  ref IList<DbModel.County> dbCounty,
                                  ref IList<DbModel.City> dbCities)
        {
            Response response = null;
            if (validationType == ValidationType.Add)
                response = _supplierService.IsRecordValidForProcess(new List<DomainModel.Supplier> { supplierDetail?.SupplierInfo }, validationType, ref dbSuppliers, ref dbCountries, ref dbCounty, ref dbCities);

            if (validationType == ValidationType.Update)
                response = _supplierService.IsRecordValidForProcess(new List<DomainModel.Supplier> { supplierDetail?.SupplierInfo }, validationType, ref dbSuppliers, ref dbCountries, ref dbCounty, ref dbCities);

            if (validationType == ValidationType.Delete)
                response = _supplierService.IsRecordValidForProcess(new List<DomainModel.Supplier> { supplierDetail?.SupplierInfo }, validationType, ref dbSuppliers, ref dbCountries, ref dbCounty, ref dbCities);

            return response;
        }

        private Response ValidateSupplierChild(int supplierId,
                                    DomainModel.SupplierDetail supplierDetail,
                                  ref IList<DbModel.Supplier> dbSuppliers,
                                  ref IList<DbModel.SupplierContact> dbSupplierContacts,
                                  ref IList<DbModel.SupplierNote> dbSupplierNotes)
        {
            Response response = new Response(ResponseType.Success.ToId(), ResponseType.Success.ToString());
            response = ValidSupplierContacts(supplierId, supplierDetail.SupplierContacts, ref dbSuppliers, ref dbSupplierContacts);

            if (response != null && response.Code == ResponseType.Success.ToId() && Convert.ToBoolean(response.Result))
                response = ValidSupplierNotes(supplierId, supplierDetail.SupplierNotes, ref dbSuppliers, ref dbSupplierNotes);

            return response;
        }

        public Response ValidSupplierContacts(int supplierid,
                                              IList<DomainModel.SupplierContact> supplierContacts,
                                              ref IList<DbModel.Supplier> dbSuppliers,
                                              ref IList<DbModel.SupplierContact> dbSupplierContacts)
        {
            Response response = new Response(ResponseType.Success.ToId(), ResponseType.Success.ToString());

            var addContacts = supplierContacts?.Where(x => x.RecordStatus.IsRecordStatusNew()).ToList();
            var modContacts = supplierContacts?.Where(x => x.RecordStatus.IsRecordStatusModified()).ToList();
            var delContacts = supplierContacts?.Where(x => x.RecordStatus.IsRecordStatusDeleted()).ToList();

            if (addContacts?.Count > 0)
            {
                addContacts = addContacts?.Select(x => { x.SupplierId = supplierid; return x; }).ToList();
                response = _supplierContactService.IsRecordValidForProcess(addContacts, ValidationType.Add, ref dbSupplierContacts, ref dbSuppliers);
            }
            if (modContacts?.Count > 0)
            {
                modContacts = modContacts?.Select(x => { x.SupplierId = supplierid; return x; }).ToList();
                response = _supplierContactService.IsRecordValidForProcess(modContacts, ValidationType.Update, ref dbSupplierContacts, ref dbSuppliers);
            }

            if (delContacts?.Count > 0)
            {
                delContacts = delContacts?.Select(x => { x.SupplierId = supplierid; return x; }).ToList();
                response = _supplierContactService.IsRecordValidForProcess(delContacts, ValidationType.Delete, ref dbSupplierContacts, ref dbSuppliers);
            }


            return response;
        }

        public Response ValidSupplierNotes(int supplierId,
                                              IList<DomainModel.SupplierNote> supplierNotes,
                                              ref IList<DbModel.Supplier> dbSuppliers,
                                              ref IList<DbModel.SupplierNote> dbSupplierNotes)
        {
            Response response = new Response(ResponseType.Success.ToId(), ResponseType.Success.ToString());
            var addNotes = supplierNotes?.Where(x => x.RecordStatus.IsRecordStatusNew()).ToList();
            var delNotes = supplierNotes?.Where(x => x.RecordStatus.IsRecordStatusDeleted()).ToList();
            if (addNotes?.Count > 0)
            {
                addNotes = addNotes.Select(x => { x.SupplierId = supplierId; return x; }).ToList();
                response = _supplierNoteService.IsRecordValidForProcess(addNotes, ValidationType.Add, ref dbSupplierNotes, ref dbSuppliers);
            }

            if (delNotes?.Count > 0)
            {
                delNotes = delNotes.Select(x => { x.SupplierId = supplierId; return x; }).ToList();
                response = _supplierNoteService.IsRecordValidForProcess(delNotes, ValidationType.Delete, ref dbSupplierNotes, ref dbSuppliers);
            }

            return response;
        }

        private Response ProcessSupplierDetail(DomainModel.SupplierDetail supplierDetail,
                                               ref IList<DbModel.Supplier> dbSuppliers,
                                               ValidationType validationType,
                                               bool commitChange)
        {
            Response response = new Response(ResponseType.Success.ToId(), ResponseType.Success.ToString());
            Exception exception = null;
            IList<DbModel.SupplierContact> dbSupplierContacts = null;
            IList<DbModel.SupplierNote> dbSupplierNotes = null;
            IList<DbModel.City> dbCities = null;
            IList<DbModel.Country> dbCountry = null;
            IList<DbModel.County> dbCounty = null;
            long? eventId = null;
            int supplierId = 0;
            try
            {
                if (supplierDetail != null)
                {
                    response = ValidateSupplierInfo(supplierDetail, validationType, ref dbSuppliers, ref dbCountry, ref dbCounty, ref dbCities);
                    if (response.Code == MessageType.Success.ToId())
                    {
                        var dbModule = _auditSearchService.GetAuditModule(new List<string>() {
                                                SqlAuditModuleType.Supplier.ToString(),
                                                SqlAuditModuleType.SupplierContact.ToString(),
                                                SqlAuditModuleType.SupplierNote.ToString(),
                                                SqlAuditModuleType.SupplierDocument.ToString(),
                                            });
                        //To-Do: Will create helper method get TransactionScope instance based on requirement
                        using (var tranScope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.Snapshot }))
                        //using (var tranScope = new TransactionScope())
                        {
                            supplierId = (supplierDetail?.SupplierInfo?.SupplierId) ?? 0;
                            response = this.ProcessSupplierInfo(new List<DomainModel.Supplier> { supplierDetail.SupplierInfo }, dbModule, ref dbSuppliers, ref dbCountry, ref dbCounty, ref dbCities, ref eventId, validationType, commitChange, ref supplierId);

                            if (response != null && response.Code == MessageType.Success.ToId())
                                supplierId = dbSuppliers.ToList().FirstOrDefault().Id;

                            if (supplierId > 0)
                            {
                                response = ValidateSupplierChild(supplierId, supplierDetail, ref dbSuppliers, ref dbSupplierContacts, ref dbSupplierNotes);
                                AppendEvent(supplierDetail, eventId);
                                if (response != null && response.Code == MessageType.Success.ToId())
                                    response = ProcessSupplierContacts(supplierDetail.SupplierContacts, dbSupplierContacts, dbSuppliers, validationType, commitChange, supplierId, dbModule);

                                if (response != null && response.Code == MessageType.Success.ToId())
                                    response = ProcessSupplierNote(supplierDetail.SupplierNotes, dbSupplierNotes, dbSuppliers, validationType, commitChange, supplierId, dbModule);

                                if (response != null && response.Code == MessageType.Success.ToId())
                                    response = ProcessSupplierDocument(supplierDetail.SupplierDocuments, supplierId, validationType, commitChange, supplierDetail, validationType.ToAuditActionType(), ref eventId, dbModule);

                                if (response != null && response.Code == MessageType.Success.ToId())
                                {

                                    _repository.AutoSave = false;
                                    _repository.ForceSave();
                                    tranScope.Complete();
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                supplierId = 0;
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), supplierDetail);
            }
            finally
            {
                _repository.AutoSave = true;
            }

            return response;
        }


        private Response ProcessSupplierDeleteDetail(DomainModel.SupplierDetail supplierDetail,
                                               ref IList<DbModel.Supplier> dbSuppliers,
                                               ValidationType validationType,
                                               bool commitChange)
        {
            Response response = new Response(ResponseType.Success.ToId(), ResponseType.Success.ToString());
            Exception exception = null;
            IList<DbModel.SupplierContact> dbSupplierContacts = null;
            IList<DbModel.SupplierNote> dbSupplierNotes = null;
            IList<DbModel.City> dbCities = null;
            IList<DbModel.Country> dbCountry = null;
            IList<DbModel.County> dbCounty = null;
            int supplierId = 0;
            long? eventId = null;
            try
            {
                if (supplierDetail != null)
                {
                    supplierDetail.SupplierContacts = supplierDetail.SupplierContacts?.Select(x => { x.RecordStatus = "D"; return x; })?.ToList();
                    supplierDetail.SupplierNotes = supplierDetail.SupplierNotes?.Select(x => { x.RecordStatus = "D"; return x; })?.ToList();
                    supplierDetail.SupplierDocuments = supplierDetail.SupplierDocuments?.Select(x => { x.RecordStatus = "D"; return x; })?.ToList();

                    response = ValidateSupplierInfo(supplierDetail, validationType, ref dbSuppliers, ref dbCountry, ref dbCounty, ref dbCities);
                    if (response.Code == MessageType.Success.ToId())
                    {
                        supplierId = (supplierDetail?.SupplierInfo?.SupplierId) ?? 0;
                        if (supplierId > 0)
                        {
                            response = ValidateSupplierChild(supplierId, supplierDetail, ref dbSuppliers, ref dbSupplierContacts, ref dbSupplierNotes);
                            AppendEvent(supplierDetail, eventId);
                            if (response != null && response.Code == MessageType.Success.ToId())
                            {
                                //To-Do: Will create helper method get TransactionScope instance based on requirement
                                using (var tranScope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.Snapshot }))
                                //using (var tranScope = new TransactionScope())
                                {
                                    response = ProcessSupplierDocument(supplierDetail.SupplierDocuments, supplierId, ValidationType.Delete, commitChange, supplierDetail, validationType.ToAuditActionType(), ref eventId,null);
                                    if (response != null && response.Code == ResponseType.Success.ToId())
                                    {
                                        var count = _repository.DeleteSupplier((int)supplierDetail.SupplierInfo.SupplierId);
                                        if (count > 0)
                                        {
                                            supplierDetail.dbModule = _auditSearchService.GetAuditModule(new List<string>() {
                                                SqlAuditModuleType.Supplier.ToString()
                                            });
                                            response = _auditSearchService.AuditLog(supplierDetail, ref eventId, supplierDetail?.SupplierInfo?.ActionByUser?.ToString(),
                                                                         "{"+ AuditSelectType.Id+":" + dbSuppliers?.FirstOrDefault().Id + "}${" + AuditSelectType.Name +":"+ supplierDetail.SupplierInfo.SupplierName.Trim()+"}",
                                                                         SqlAuditActionType.D, SqlAuditModuleType.Supplier, supplierDetail.SupplierInfo, null, supplierDetail?.dbModule);

                                            //response = ProcessSupplierDelete(supplierDetail, ValidationType.Delete.ToAuditActionType(),
                                            //                                 SqlAuditModuleType.Supplier, null, null, ref eventId);
                                            tranScope.Complete();
                                            response = new Response(ResponseType.Success.ToId(), ResponseType.Success.ToString());
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //{
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), supplierDetail);
            }
            finally
            {
                _repository.AutoSave = true;
            }

            return response;
        }

        private Response ProcessSupplierInfo(IList<DomainModel.Supplier> suppliers,
            IList<DbModel.SqlauditModule> dbModule,
                                             ref IList<DbModel.Supplier> dbSuppliers,
                                             ref IList<DbModel.Country> dbCountries,
                                             ref IList<DbModel.County> dbCounties,
                                             ref IList<DbModel.City> dbCities,
                                             ref long? eventId,
                                             ValidationType validationType,
                                             bool commitChanges,
                                             ref int supplierId)
        {
            Exception exception = null;
            try
            {
                if (suppliers != null)
                {
                    if (validationType == ValidationType.Delete)
                        return this._supplierService.Delete(suppliers, dbModule, ref dbSuppliers, ref eventId, commitChanges, false);
                    else if (validationType == ValidationType.Add)
                        return this._supplierService.Add(suppliers, dbModule, ref dbSuppliers, ref dbCountries, ref dbCounties, ref dbCities, ref eventId, commitChanges, false);
                    else if (validationType == ValidationType.Update)
                        return this._supplierService.Modify(suppliers, dbModule, ref dbSuppliers, ref dbCountries, ref dbCounties, ref dbCities, ref eventId, commitChanges, false);
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), suppliers);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, null, exception);
        }

        private Response ProcessSupplierContacts(IList<DomainModel.SupplierContact> supplierContacts,
                                                 IList<DbModel.SupplierContact> dbSupplierContacts,
                                                 IList<DbModel.Supplier> dbSuppliers,
                                                 ValidationType validationType,
                                                 bool commitChanges,
                                                 int supplierId,
                                                 IList<DbModel.SqlauditModule> dbModule)
        {
            Response response = new Response(ResponseType.Success.ToId(), ResponseType.Success.ToString());
            Exception exception = null;
            try
            {
                if (supplierContacts != null)
                {
                    var addContacts = supplierContacts?.Where(x => x.RecordStatus.IsRecordStatusNew()).ToList();
                    var modContacts = supplierContacts?.Where(x => x.RecordStatus.IsRecordStatusModified()).ToList();
                    var delContacts = supplierContacts?.Where(x => x.RecordStatus.IsRecordStatusDeleted()).ToList();

                    if (addContacts?.Count > 0)
                    {
                        if (supplierId > 0)
                            supplierContacts?.ToList().ForEach(x => { x.SupplierId = supplierId; });
                        response = this._supplierContactService.Add(addContacts,dbModule, ref dbSupplierContacts, ref dbSuppliers, commitChanges, false);
                    }

                    if (modContacts?.Count > 0 && response.Code == MessageType.Success.ToId())
                    {
                        dbSupplierContacts = dbSuppliers?.SelectMany(x => x.SupplierContact).ToList().Where(x1 => modContacts.Any(x2 => x1.Id == x2.SupplierContactId)).ToList();
                        if (supplierId > 0)
                            supplierContacts?.ToList().ForEach(x => { x.SupplierId = supplierId; });
                        response = this._supplierContactService.Modify(modContacts, dbModule, ref dbSupplierContacts, ref dbSuppliers, commitChanges, false);
                    }

                    if (delContacts?.Count > 0 && response.Code == MessageType.Success.ToId())
                    {
                        dbSupplierContacts = dbSuppliers?.SelectMany(x => x.SupplierContact).ToList().Where(x1 => delContacts.Any(x2 => x1.Id == x2.SupplierContactId)).ToList();
                        if (supplierId > 0)
                            supplierContacts?.ToList().ForEach(x => { x.SupplierId = supplierId; });
                        response = this._supplierContactService.Delete(delContacts, dbModule, ref dbSupplierContacts, ref dbSuppliers, commitChanges, false);
                    }

                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), supplierContacts);
            }

            return response;
        }

        private Response ProcessSupplierNote(IList<DomainModel.SupplierNote> supplierNotes,
                                             IList<DbModel.SupplierNote> dbSupplierNotes,
                                             IList<DbModel.Supplier> dbSuppliers,
                                             ValidationType validationType,
                                             bool commitChanges,
                                             int supplierId,
                                             IList<DbModel.SqlauditModule> dbModule)
        {
            Response response = new Response(ResponseType.Success.ToId(), ResponseType.Success.ToString());
            Exception exception = null;
            try
            {
                if (supplierNotes != null)
                {
                    var addNotes = supplierNotes?.Where(x => x.RecordStatus.IsRecordStatusNew()).ToList();
                    var delNotes = supplierNotes?.Where(x => x.RecordStatus.IsRecordStatusDeleted()).ToList();
                    var updateNotes = supplierNotes?.Where(x => x.RecordStatus.IsRecordStatusModified()).ToList();       //D661 issue 8 

                    if (addNotes?.Count > 0)
                    {
                        if (supplierId > 0)
                            supplierNotes?.ToList().ForEach(x => { x.SupplierId = supplierId; });
                        response = _supplierNoteService.Add(addNotes, dbModule, ref dbSupplierNotes, ref dbSuppliers, commitChanges, false);
                    }

                    if (delNotes?.Count > 0 && response.Code == MessageType.Success.ToId())
                    {
                        dbSupplierNotes = dbSuppliers?.SelectMany(x => x.SupplierNote).ToList().Where(x1 => delNotes.Any(x2 => x1.Id == x2.SupplierNoteId)).ToList();
                        if (supplierId > 0)
                            supplierNotes?.ToList().ForEach(x => { x.SupplierId = supplierId; });
                        response = this._supplierNoteService.Delete(supplierNotes, dbModule, ref dbSupplierNotes, ref dbSuppliers, commitChanges, false);
                    }
       //D661 issue 8 Start
                    if(updateNotes?.Count > 0 && response.Code == MessageType.Success.ToId())
                    {
                        dbSupplierNotes = dbSuppliers?.SelectMany(x => x.SupplierNote).ToList().Where(x1 => updateNotes.Any(x2 => x1.Id == x2.SupplierNoteId)).ToList();
                        if (supplierId > 0)
                            supplierNotes?.ToList().ForEach(x => { x.SupplierId = supplierId; });
                        response = this._supplierNoteService.Update(supplierNotes, dbModule, ref dbSupplierNotes, ref dbSuppliers, commitChanges, false);
                    }
        //D661 issue 8 End
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), supplierNotes);
            }

            return response;
        }

        private Response ProcessSupplierDocument(IList<ModuleDocument> supplierDocuments, int supplierId, ValidationType validationType, bool commitChanges, DomainModel.SupplierDetail supplierDetail, SqlAuditActionType sqlAuditActionType, ref long? eventId, IList<DbModel.SqlauditModule> dbModule)
        {
            Response response = new Response(ResponseType.Success.ToId(), ResponseType.Success.ToString());
            Exception exception = null;
            List<DbModel.Document> dbDocuments = null;
            try
            {
                if (supplierDocuments?.Count > 0 && supplierId > 0)
                {
                    supplierDocuments?.ToList().ForEach(x => { x.ModuleRefCode = supplierId.ToString(); });
                    supplierDetail.SupplierDocuments = supplierDocuments;
                    var auditSupplierDetails = ObjectExtension.Clone(supplierDetail);
                    if (supplierDocuments.Any(x => x.RecordStatus.IsRecordStatusNew()))
                    {
                        response = this._supplierDocumentService.Save(supplierDocuments,ref dbDocuments, commitChanges);
                        auditSupplierDetails.SupplierDocuments = supplierDocuments.Select(x =>
                        {
                            x.CreatedOn = dbDocuments?.FirstOrDefault(y => y.DocumentUniqueName == x.DocumentUniqueName)?.CreatedDate;
                            return x;
                        }).ToList(); // audit create date issue fix
                    } 
                    if (supplierDocuments.Any(x => x.RecordStatus.IsRecordStatusModified()))
                    {
                        response = this._supplierDocumentService.Modify(supplierDocuments,ref dbDocuments, commitChanges);
                    }
                    if (supplierDocuments.Any(x => x.RecordStatus.IsRecordStatusDeleted()) )
                    {
                        response = this._supplierDocumentService.Delete(supplierDocuments, commitChanges);
                    }  
                    if (response.Code == MessageType.Success.ToId())
                    {
                        DocumentAudit(auditSupplierDetails.SupplierDocuments, sqlAuditActionType, auditSupplierDetails, dbModule, ref eventId, ref dbDocuments);
                    }
                    return response;
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), supplierDocuments);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, null, exception);
        }

        private void DocumentAudit(IList<ModuleDocument> supplierDocuments, SqlAuditActionType sqlAuditActionType, DomainModel.SupplierDetail supplierDetail, IList<DbModel.SqlauditModule> dbModule, ref long? eventId, ref List<DbModel.Document> dbDocuments)
        {
            //For Document Audit
            if (supplierDocuments.Count > 0)
            {
                object newData;
                object oldData;
                var newDocument = supplierDocuments?.Where(x => x.RecordStatus.IsRecordStatusNew())?.ToList();
                var modifiedDocument = supplierDocuments?.Where(x => x.RecordStatus.IsRecordStatusModified())?.ToList();
                var deletedDocument = supplierDocuments?.Where(x => x.RecordStatus.IsRecordStatusDeleted())?.ToList();
                if (newDocument.Count > 0)
                {
                    newData = newDocument; 
                    _auditSearchService.AuditLog(supplierDetail, ref eventId, supplierDetail?.SupplierInfo?.ActionByUser?.ToString(), null, sqlAuditActionType, SqlAuditModuleType.SupplierDocument, null, newData, dbModule);
                }
                if (modifiedDocument.Count > 0)
                {
                    newData = modifiedDocument?.OrderBy(x => x.Id)?.ToList();
                    oldData = (dbDocuments != null && dbDocuments.Count > 0) ? _mapper.Map<List<ModuleDocument>>(dbDocuments)?.OrderBy(x => x.Id)?.ToList() : null;
                    _auditSearchService.AuditLog(supplierDetail, ref eventId, supplierDetail?.SupplierInfo?.ActionByUser?.ToString(), null, sqlAuditActionType, SqlAuditModuleType.SupplierDocument, oldData, newData, dbModule);
                }
                if (deletedDocument.Count > 0)
                {
                    oldData = deletedDocument;
                    _auditSearchService.AuditLog(supplierDetail, ref eventId, supplierDetail?.SupplierInfo?.ActionByUser?.ToString(), null, sqlAuditActionType, SqlAuditModuleType.SupplierDocument, oldData, null, dbModule);
                }
            }
        }

        private void AppendEvent(DomainModel.SupplierDetail supplierDetail,
                                 long? eventId)
        {
            ObjectExtension.SetPropertyValue(supplierDetail.SupplierInfo, "EventId", eventId);
            ObjectExtension.SetPropertyValue(supplierDetail.SupplierContacts, "EventId", eventId);
            ObjectExtension.SetPropertyValue(supplierDetail.SupplierNotes, "EventId", eventId);
            ObjectExtension.SetPropertyValue(supplierDetail.SupplierDocuments, "EventId", eventId);
        }

        //private Response ProcessSupplierDelete(DomainModel.SupplierDetail supplierDetail, SqlAuditActionType sqlAuditActionType,
        //                     SqlAuditModuleType sqlAuditModuleType, object oldData,
        //                     object newData,
        //                     ref long? eventId)
        //{

        //    Response result = null;
        //    Exception exception = null;

        //    LogEventGeneration logEventGeneration = new LogEventGeneration(_auditLogger);


        //    if (supplierDetail != null && !string.IsNullOrEmpty(supplierDetail.SupplierInfo.ActionByUser))
        //    {

        //        string actionBy = supplierDetail.SupplierInfo.ActionByUser;
        //        eventId = logEventGeneration.GetEventLogId(eventId,
        //                                                       sqlAuditActionType,
        //                                                       actionBy, supplierDetail.SupplierInfo.SupplierId + "$" + supplierDetail.SupplierInfo.SupplierName.Trim(),
        //                                                      SqlAuditModuleType.Supplier.ToString());
        //        if (supplierDetail.SupplierInfo.RecordStatus.IsRecordStatusDeleted())
        //        {
        //            if (supplierDetail?.SupplierInfo != null)
        //            {
        //                var DeleetdSupplierInfo = supplierDetail.SupplierInfo.RecordStatus.IsRecordStatusDeleted();
        //                if (DeleetdSupplierInfo)
        //                {
        //                    oldData = supplierDetail?.SupplierInfo;
        //                    result = _auditLogger.LogAuditData((long)eventId, SqlAuditModuleType.Supplier, oldData, null);
        //                }
        //            }
        //            if (supplierDetail.SupplierContacts.Count > 0)
        //            {
        //                var DeletedSupplierContact = supplierDetail.SupplierContacts.Where(x => x.RecordStatus.IsRecordStatusDeleted()).ToList();
        //                if (DeletedSupplierContact.Count > 0)
        //                {
        //                    oldData = DeletedSupplierContact;
        //                    result = _auditLogger.LogAuditData((long)eventId, SqlAuditModuleType.SupplierContact, oldData, null);
        //                }
        //            }
        //            if (supplierDetail.SupplierNotes.Count > 0)
        //            {
        //                var DeletedSupplierNotes = supplierDetail.SupplierNotes.Where(x => x.RecordStatus.IsRecordStatusDeleted()).ToList();
        //                if (DeletedSupplierNotes.Count > 0)
        //                {
        //                    oldData = DeletedSupplierNotes;
        //                    result = _auditLogger.LogAuditData((long)eventId, SqlAuditModuleType.SupplierNote, oldData, null);
        //                }
        //            }

        //        }

        //    }
        //      return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception);
        //}


        #endregion
    }
}