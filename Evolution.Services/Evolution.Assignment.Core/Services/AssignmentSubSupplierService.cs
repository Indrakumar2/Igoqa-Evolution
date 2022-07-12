using AutoMapper;
using Evolution.Assignment.Domain.Interfaces.Assignments;
using Evolution.Assignment.Domain.Interfaces.Data;
using Evolution.Assignment.Domain.Interfaces.Validations;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Common.Models.Messages;
using Evolution.Common.Models.Responses;
using Evolution.Logging.Interfaces;
using Evolution.SupplierContacts.Domain.Interfaces.Suppliers;
using Evolution.SupplierPO.Domain.Interfaces.SupplierPO;
using Evolution.TechnicalSpecialist.Domain.Interfaces.TechnicalSpecialist;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Transactions;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.Assignment.Domain.Models.Assignments;

namespace Evolution.Assignment.Core.Services
{
    public class AssignmentSubSupplierService : IAssignmentSubSupplierService
    {
        private readonly IAppLogger<AssignmentSubSupplierService> _logger = null;
        private readonly IMapper _mapper = null;
        private readonly JObject _messageDescriptions = null;
        private readonly IAssignmentService _assignmentService = null;
        private readonly IAssignmentSubSupplerRepository _assignmentSubSupplierRepository = null;
        private readonly IAssignmentSubSupplierValidationService _assignmentSubSupplierValidationService = null;
        private readonly IAssignmentRepository _assignmentRepository = null;
        private readonly ITechnicalSpecialistService _technicalSpecialistService = null;
        private readonly IAssignmentSubSupplerTSRepository _assignmentSubSupplerTSRepository = null;


        #region Constructor 

        public AssignmentSubSupplierService(IMapper mapper, IAppLogger<AssignmentSubSupplierService> logger, IAssignmentService assignmentService,
                                            IAssignmentSubSupplerRepository assignmentSubSupplierRepository,
                                            IAssignmentSubSupplierValidationService assignmentSubSupplierValidationService,
                                            IAssignmentRepository assignmentRepository,
                                            ITechnicalSpecialistService technicalSpecialistService,
                                            IAssignmentSubSupplerTSRepository assignmentSubSupplerTSRepository,
                                            JObject messages)
        {
            _logger = logger;
            _mapper = mapper;
            _assignmentService = assignmentService;
            _assignmentSubSupplierRepository = assignmentSubSupplierRepository;
            _assignmentSubSupplierValidationService = assignmentSubSupplierValidationService;
            _technicalSpecialistService = technicalSpecialistService;
            _messageDescriptions = messages;
            _assignmentRepository = assignmentRepository;
            _assignmentSubSupplerTSRepository = assignmentSubSupplerTSRepository;
        }

        #endregion

        #region Public Methods

        #region Add

        public Response Add(IList<DomainModel.AssignmentSubSupplier> assignmentSubSupplier,
                            bool commitChange = true,
                            bool isDbValidationRequired = true,
                            int? assignmentId = null)
        {
            if (assignmentId.HasValue)
                assignmentSubSupplier?.ToList().ForEach(x => { x.AssignmentId = assignmentId.Value; });

            IList<DbModel.AssignmentSubSupplier> dbAssignmentSubSupplier = null;
            IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialist = null;
            IList<DbModel.Assignment> dbAssignment = null;
            return AddAssignmentSubSupplier(assignmentSubSupplier,
                                            ref dbAssignmentSubSupplier,
                                            ref dbTechnicalSpecialist,
                                            ref dbAssignment,
                                            commitChange,
                                            isDbValidationRequired);
        }

        public Response Add(IList<DomainModel.AssignmentSubSupplier> assignmentSubSupplier,
                            ref IList<DbModel.AssignmentSubSupplier> dbAssignmentSubSuppliers,
                            ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialist,
                            ref IList<DbModel.Assignment> dbAssignment,
                            bool commitChange = true,
                            bool isDbValidationRequired = true,
                            int? assignmentId = null)
        {
            if (assignmentId.HasValue && !isDbValidationRequired)
                assignmentSubSupplier?.ToList().ForEach(x => { x.AssignmentId = assignmentId.Value; });

            return AddAssignmentSubSupplier(assignmentSubSupplier,
                                            ref dbAssignmentSubSuppliers,
                                            ref dbTechnicalSpecialist,
                                            ref dbAssignment,
                                            commitChange,
                                            isDbValidationRequired);
        }

        //public Response SaveMainSupplierContact(IList<DomainModel.AssignmentSubSupplier> assignmentSubSuppliers,
        //                                        ref IList<DbModel.Assignment> dbAssignments,
        //                                        bool commitChange = true,
        //                                        bool isDbValidationRequired = true)
        //{

        //    Exception exception = null;
        //    try
        //    {
        //        _assignmentRepository.AutoSave = false;

        //        dbAssignments.ToList().ForEach(dbAssignmentsToBeUpdate =>
        //        {
        //            var assignmentMainSupplierContactToModify = assignmentSubSuppliers.Where(x => x.MainSupplierContactId > 0)
        //                                                                              .FirstOrDefault(x => x.AssignmentId == dbAssignmentsToBeUpdate.Id);
        //            if (assignmentMainSupplierContactToModify != null)
        //                dbAssignmentsToBeUpdate.MainSupplierContactId = assignmentMainSupplierContactToModify.MainSupplierContactId;
        //        });
        //        _assignmentRepository.Update(dbAssignments);
        //        _assignmentRepository.ForceSave();
        //    }
        //    catch (Exception ex)
        //    {
        //        exception = ex;
        //        _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), assignmentSubSuppliers);
        //    }
        //    return new Response().ToPopulate(ResponseType.Success, null, null, null, null, exception);

        //}


        #endregion

        #region Modify

        public Response Modify(IList<DomainModel.AssignmentSubSupplier> assignmentSubSupplier,
                               bool commitChange = true,
                               bool isDbValidationRequired = true,
                               int? assignmentId = null)
        {
            if (assignmentId.HasValue)
                assignmentSubSupplier?.ToList().ForEach(x => { x.AssignmentId = assignmentId.Value; });

            IList<DbModel.AssignmentSubSupplier> dbAssignmentSubSuppliers = null;
            IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialist = null;
            IList<DbModel.Assignment> dbAssignment = null;
            return UpdateAssignmentSubSupplier(assignmentSubSupplier,
                                               ref dbAssignmentSubSuppliers,
                                               ref dbTechnicalSpecialist,
                                               ref dbAssignment,
                                               commitChange,
                                               isDbValidationRequired);
        }

        public Response Modify(IList<DomainModel.AssignmentSubSupplier> assignmentSubSupplier,
                               ref IList<DbModel.AssignmentSubSupplier> dbAssignmentSubSuppliers,
                               ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialist,
                               ref IList<DbModel.Assignment> dbAssignment,
                               bool commitChange = true,
                               bool isDbValidationRequired = true,
                               int? assignmentId = null)
        {
            if (assignmentId.HasValue && !isDbValidationRequired)
                assignmentSubSupplier?.ToList().ForEach(x => { x.AssignmentId = assignmentId.Value; });

            return UpdateAssignmentSubSupplier(assignmentSubSupplier,
                                               ref dbAssignmentSubSuppliers,
                                               ref dbTechnicalSpecialist,
                                               ref dbAssignment,
                                               commitChange,
                                               isDbValidationRequired);
        }

        #endregion

        #region Delete

        public Response Delete(IList<DomainModel.AssignmentSubSupplier> assignmentSubSupplier,
                               bool commitChange = true,
                               bool isDbValidationRequired = true,
                               int? assignmentId = null)
        {
            IList<DbModel.Assignment> dbAssignment = null;
            if (assignmentId.HasValue)
                assignmentSubSupplier?.ToList().ForEach(x => { x.AssignmentId = assignmentId.Value; });

            return RemoveAssignmentSubSupplierServices(assignmentSubSupplier,
                                                       ref dbAssignment,
                                                       commitChange,
                                                       isDbValidationRequired);
        }

        public Response Delete(IList<DomainModel.AssignmentSubSupplier> assignmentSubSupplier,
                            ref IList<DbModel.Assignment> dbAssignment,
                            bool commitChange = true,
                            bool isDbValidationRequired = true,
                            int? assignmentId = null)
        {
            if (assignmentId.HasValue && !isDbValidationRequired)
                assignmentSubSupplier?.ToList().ForEach(x => { x.AssignmentId = assignmentId.Value; });

            return RemoveAssignmentSubSupplierServices(assignmentSubSupplier,
                                                       ref dbAssignment,
                                                       commitChange,
                                                       isDbValidationRequired);
        }

        #endregion

        #region Get

        public Response Get(DomainModel.AssignmentSubSupplier searchModel)
        {
            IList<DomainModel.AssignmentSubSupplier> result = null;
            Exception exception = null;
            try
            {
                using (var tranScope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    result = _assignmentSubSupplierRepository.Search(searchModel, subSup => subSup.Supplier.Address,//MS-TS Link
                        supCon => supCon.SupplierContact,
                        assign => assign.Assignment,
                        tech => tech.AssignmentSubSupplierTechnicalSpecialist
                    );
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

        public Response GetSubSupplierForVisit(DomainModel.AssignmentSubSupplierVisit searchModel)
        {
            IList<DomainModel.AssignmentSubSupplierVisit> result = null;
            Exception exception = null;
            try
            {
                result = _assignmentSubSupplierRepository.GetSubSupplierForVisit(searchModel, subSup => subSup.Supplier.Address,//MS-TS Link
                                                                                 supCon => supCon.SupplierContact,
                                                                                 assign => assign.Assignment,
                                                                                 tech => tech.AssignmentSubSupplierTechnicalSpecialist
                                                                           );
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), searchModel);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception, result?.Count);
        }

        #endregion

        #region Validation Check

        public Response IsRecordValidForProcess(IList<DomainModel.AssignmentSubSupplier> assignmentSubSuppliers,
                                                ValidationType validationType)
        {
            IList<DbModel.AssignmentSubSupplier> dbAssignmentSubSupplier = null;
            IList<DbModel.Assignment> dbAssignments = null;
            IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialist = null;
            return IsRecordValidForProcess(assignmentSubSuppliers,
                                            validationType,
                                            ref dbAssignmentSubSupplier,
                                            ref dbTechnicalSpecialist,
                                            ref dbAssignments);
        }

        public Response IsRecordValidForProcess(IList<DomainModel.AssignmentSubSupplier> assignmentSubSuppliers,
                                                ValidationType validationType,
                                                ref IList<DbModel.AssignmentSubSupplier> dbAssignmentSubSupplier,
                                                ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialist,
                                                ref IList<DbModel.Assignment> dbAssignments)
        {
            IList<DomainModel.AssignmentSubSupplier> filteredAssignmentSubSupplier = null;
            return IsRecordValidForProcess(assignmentSubSuppliers,
                                            validationType,
                                            ref filteredAssignmentSubSupplier,
                                            ref dbAssignmentSubSupplier,
                                            ref dbTechnicalSpecialist,
                                            ref dbAssignments);
        }

        public Response IsRecordValidForProcess(IList<DomainModel.AssignmentSubSupplier> assignmentSubSuppliers,
                                                ValidationType validationType,
                                                IList<DbModel.AssignmentSubSupplier> dbAssignmentSubSupplier,
                                                ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialist,
                                                ref IList<DbModel.Assignment> dbAssignments)
        {
            return IsRecordValidForProcess(assignmentSubSuppliers,
                                            validationType,
                                            ref dbAssignmentSubSupplier,
                                            ref dbTechnicalSpecialist,
                                            ref dbAssignments);
        }

        #endregion

        #endregion

        #region Private Methods

        private Response AddAssignmentSubSupplier(IList<DomainModel.AssignmentSubSupplier> assignmentSubSuppliers,
                                                  ref IList<DbModel.AssignmentSubSupplier> dbAssignmentSubSuppliers,
                                                  ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialist,
                                                  ref IList<DbModel.Assignment> dbAssignment,
                                                  bool commitChange = true,
                                                  bool isDbValidationRequire = true)
        {
            Exception exception = null;
            IList<ValidationMessage> validationMessages = null;
            try
            {
                IList<DbModel.Assignment> dbAssignments = null;
                if (dbAssignment != null)
                    dbAssignments = dbAssignment;

                Response valdResponse = null;
                var recordToBeAdd = FilterRecord(assignmentSubSuppliers, ValidationType.Add);


                if (isDbValidationRequire)
                    valdResponse = IsRecordValidForProcess(assignmentSubSuppliers,
                                                            ValidationType.Add,
                                                            ref recordToBeAdd,
                                                            ref dbAssignmentSubSuppliers,
                                                            ref dbTechnicalSpecialist,
                                                            ref dbAssignments);
                if (recordToBeAdd?.Count > 0)
                {
                    var SupplierId = recordToBeAdd?.FirstOrDefault()?.SubSupplierId;//MS-TS Link
                    if (!isDbValidationRequire || Convert.ToBoolean(valdResponse.Result) && dbAssignmentSubSuppliers?.Count > 0)
                    {
                        if (SupplierId != null)
                        {
                            _assignmentSubSupplierRepository.AutoSave = false;
                            var dbAssignmentTs = dbAssignments.SelectMany(x => x.AssignmentTechnicalSpecialist).ToList();

                            if (dbAssignmentTs?.Count > 0)
                            {
                                recordToBeAdd.ToList().ForEach(x =>
                                {
                                    x.AssignmentSubSupplierTS = x.AssignmentSubSupplierTS?.Where(x1 => x1.RecordStatus.IsRecordStatusNew()).ToList();
                                });
                            }
                            dbAssignmentSubSuppliers = _mapper.Map<IList<DbModel.AssignmentSubSupplier>>(recordToBeAdd, opt =>
                            {
                                opt.Items["isAssignId"] = false;
                                opt.Items["AssignmentTsId"] = dbAssignmentTs;
                            });

                            _assignmentSubSupplierRepository.Add(dbAssignmentSubSuppliers);
                            if (commitChange && recordToBeAdd.Count > 0)
                                _assignmentSubSupplierRepository.ForceSave();
                        }

                        //this.SaveMainSupplierContact(recordToBeAdd, ref dbAssignments);
                    }
                    else
                        return valdResponse;
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), assignmentSubSuppliers);
            }
            // finally { _assignmentSubSupplierRepository.Dispose(); }

            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), null, exception);
        }

        private Response UpdateAssignmentSubSupplier(IList<DomainModel.AssignmentSubSupplier> assignmentSubSuppliers,
                                                     ref IList<DbModel.AssignmentSubSupplier> dbAssignmentSubSuppliers,
                                                     ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialist,
                                                     ref IList<DbModel.Assignment> dbAssignment,
                                                     bool commitChange = true,
                                                     bool isDbValidationRequire = true)
        {
            Exception exception = null;
            IList<AssignmentSubSupplierService> result = null;
            IList<ValidationMessage> validationMessages = null;
            try
            {
                IList<DbModel.Assignment> dbAssignments = null;
                if (dbAssignment != null)
                    dbAssignments = dbAssignment;

                Response valdResponse = null;
                var recordToBeModify = FilterRecord(assignmentSubSuppliers, ValidationType.Update);

                if (dbAssignmentSubSuppliers == null)
                    dbAssignmentSubSuppliers = GetAssignmentSubSuppliers(recordToBeModify);

                if (isDbValidationRequire)
                    valdResponse = IsRecordValidForProcess(assignmentSubSuppliers,
                                                            ValidationType.Update,
                                                            ref recordToBeModify,
                                                            ref dbAssignmentSubSuppliers,
                                                            ref dbTechnicalSpecialist,
                                                            ref dbAssignments);
                var SupplierId = recordToBeModify?.FirstOrDefault()?.SubSupplierId;//MS-TS Link
                if (recordToBeModify?.Count > 0)
                {
                    if (!isDbValidationRequire || (Convert.ToBoolean(valdResponse.Result)) & dbAssignmentSubSuppliers?.Count > 0)
                    {
                        if (SupplierId != null)
                        {
                            IList<string> ePins = recordToBeModify?.Where(x => x.AssignmentSubSupplierTS != null)
                                                                   .SelectMany(x => x.AssignmentSubSupplierTS)?
                                                                   .Where(x => x.Epin > 0).Select(x => x.Epin.ToString()).ToList();

                            var dbAssignmentTs = dbAssignments.SelectMany(x => x.AssignmentTechnicalSpecialist).ToList();
                            if (dbTechnicalSpecialist == null)
                                _technicalSpecialistService.IsRecordExistInDb(ePins, ref dbTechnicalSpecialist, ref validationMessages);

                            var dbTechSpecs = dbTechnicalSpecialist;
                            dbAssignmentSubSuppliers.ToList().ForEach(dbAssignmentSubSupplier =>
                            {
                                var assignmentSubSupplierToModify = recordToBeModify?.FirstOrDefault(x => x.AssignmentSubSupplierId == dbAssignmentSubSupplier.Id);
                                if (assignmentSubSupplierToModify != null)
                                {
                                    dbAssignmentSubSupplier.AssignmentId = (int)assignmentSubSupplierToModify.AssignmentId;
                                    dbAssignmentSubSupplier.SupplierId = (int)assignmentSubSupplierToModify.SubSupplierId;//MS-TS Link
                                    dbAssignmentSubSupplier.SupplierContactId = assignmentSubSupplierToModify.SubSupplierContactId;
                                    dbAssignmentSubSupplier.IsFirstVisit = assignmentSubSupplierToModify.IsSubSupplierFirstVisit;
                                    dbAssignmentSubSupplier.LastModification = DateTime.UtcNow;
                                    dbAssignmentSubSupplier.UpdateCount = assignmentSubSupplierToModify.UpdateCount.CalculateUpdateCount();
                                    dbAssignmentSubSupplier.ModifiedBy = assignmentSubSupplierToModify.ModifiedBy;
                                    assignmentSubSupplierToModify?.AssignmentSubSupplierTS?.ForEach(x =>
                                    {
                                        ProcessSubSupplierTechnicalSpecialist((int)assignmentSubSupplierToModify.AssignmentSubSupplierId,
                                                                                x,
                                                                                dbAssignmentSubSupplier.AssignmentSubSupplierTechnicalSpecialist.ToList(),
                                                                                dbAssignmentTs);
                                    });
                                }
                            });

                            _assignmentSubSupplierRepository.AutoSave = false;
                            _assignmentSubSupplierRepository.Update(dbAssignmentSubSuppliers);
                            if (commitChange)
                            {
                                _assignmentSubSupplierRepository.ForceSave();
                                _assignmentSubSupplerTSRepository.ForceSave();
                            }
                        }
                        //this.SaveMainSupplierContact(recordToBeModify, ref dbAssignments);
                    }
                    else
                        return valdResponse;
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), assignmentSubSuppliers);
            }
            finally
            {
                _assignmentSubSupplierRepository.AutoSave = true;
                _assignmentSubSupplerTSRepository.AutoSave = true;
                //  _assignmentSubSupplierRepository.Dispose();
                //  _assignmentSubSupplerTSRepository.Dispose();
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), result, exception);
        }

        private Response RemoveAssignmentSubSupplierServices(IList<DomainModel.AssignmentSubSupplier> assignmentSubSuppliers,
                                                             ref IList<DbModel.Assignment> dbAssignment,
                                                             bool commitChange,
                                                             bool isDbValidationRequire
                                                              )
        {
            Exception exception = null;
            List<MessageDetail> errorMessages = null;
            List<ValidationMessage> validationMessages = null;
            IList<DbModel.AssignmentSubSupplier> dbAssignmentSubSuppliers = null;
            IList<DbModel.Assignment> dbAssignemnts = null;
            IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialist = null;


            try
            {
                errorMessages = new List<MessageDetail>();
                validationMessages = new List<ValidationMessage>();
                Response valdResponse = null;

                IList<DbModel.Assignment> dbAssignments = null;
                if (dbAssignment != null)
                    dbAssignments = dbAssignment;

                var recordToBeDelete = FilterRecord(assignmentSubSuppliers, ValidationType.Delete);

                if (dbAssignmentSubSuppliers == null)
                    dbAssignmentSubSuppliers = GetAssignmentSubSuppliers(recordToBeDelete);

                if (isDbValidationRequire)
                    valdResponse = IsRecordValidForProcess(assignmentSubSuppliers,
                                                            ValidationType.Delete,
                                                            ref recordToBeDelete,
                                                            ref dbAssignmentSubSuppliers,
                                                            ref dbTechnicalSpecialist,
                                                            ref dbAssignemnts);

                if (recordToBeDelete?.Count > 0)
                {
                    if (!isDbValidationRequire || (Convert.ToBoolean(valdResponse.Result)) && dbAssignmentSubSuppliers?.Count > 0)
                    {
                        IList<int> subSupplierIds = assignmentSubSuppliers?.Where(x => x.AssignmentSubSupplierId != null).Distinct().Select(x => (int)x.AssignmentSubSupplierId).ToList(); ;
                        var dbTsRecordsToDelete = _assignmentSubSupplerTSRepository.FindBy(x => subSupplierIds.Contains(x.AssignmentSubSupplierId)).ToList();

                        _assignmentSubSupplerTSRepository.Delete(dbTsRecordsToDelete);
                        _assignmentSubSupplierRepository.AutoSave = false;
                        _assignmentSubSupplierRepository.Delete(dbAssignmentSubSuppliers);
                        if (commitChange)
                        {
                            _assignmentSubSupplerTSRepository.ForceSave();
                            _assignmentSubSupplierRepository.ForceSave();

                        }
                        //this.SaveMainSupplierContact(recordToBeDelete, ref dbAssignments);
                    }
                    else
                        return valdResponse;
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), assignmentSubSuppliers);
            }
            finally
            {
                _assignmentSubSupplierRepository.AutoSave = true;
                // _assignmentSubSupplierRepository.Dispose();
            }

            return new Response().ToPopulate(ResponseType.Success, null, errorMessages, validationMessages, null, exception);
        }

        private IList<DomainModel.AssignmentSubSupplier> FilterRecord(IList<DomainModel.AssignmentSubSupplier> assignmentSubSuppliers,
                                                                      ValidationType filterType)
        {
            IList<DomainModel.AssignmentSubSupplier> filteredRecords = null;

            if (filterType == ValidationType.Add)
                filteredRecords = assignmentSubSuppliers?.Where(x => x.RecordStatus.IsRecordStatusNew()).ToList();
            else if (filterType == ValidationType.Update)
                filteredRecords = assignmentSubSuppliers?.Where(x => x.RecordStatus.IsRecordStatusModified()).ToList();
            else if (filterType == ValidationType.Delete)
                filteredRecords = assignmentSubSuppliers?.Where(x => x.RecordStatus.IsRecordStatusDeleted()).ToList();

            return filteredRecords;
        }

        private IList<DbModel.AssignmentSubSupplier> GetAssignmentSubSuppliers(IList<DomainModel.AssignmentSubSupplier> assignmentSubSupplier)
        {
            IList<DbModel.AssignmentSubSupplier> dbAssignmentSubSuppliers = null;
            if (assignmentSubSupplier?.Count > 0)
            {
                var assignmentSubSupplierIds = assignmentSubSupplier?.Where(x => x.AssignmentSubSupplierId > 0).Select(x => x.AssignmentSubSupplierId).ToList();
                dbAssignmentSubSuppliers = _assignmentSubSupplierRepository.FindBy(x => assignmentSubSupplierIds.Contains(x.Id)).ToList();
            }

            return dbAssignmentSubSuppliers;
        }

        private Response IsRecordValidForProcess(IList<DomainModel.AssignmentSubSupplier> assignmentSubSuppliers,
                                                 ValidationType validationType,
                                                 ref IList<DomainModel.AssignmentSubSupplier> filteredAssignmentSubSupplier,
                                                 ref IList<DbModel.AssignmentSubSupplier> dbAssignmentSubSupplier,
                                                 ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialist,
                                                 ref IList<DbModel.Assignment> dbAssignments)
        {
            Exception exception = null;
            bool result = true;
            IList<ValidationMessage> validationMessages = null;
            List<int?> assignmentNotExists = null;
            try
            {
                if (filteredAssignmentSubSupplier == null || filteredAssignmentSubSupplier.Count <= 0)
                    filteredAssignmentSubSupplier = FilterRecord(assignmentSubSuppliers, validationType);

                if (filteredAssignmentSubSupplier != null && filteredAssignmentSubSupplier.Count > 0)
                {
                    if (validationMessages == null)
                        validationMessages = new List<ValidationMessage>();

                    result = IsValidPayload(filteredAssignmentSubSupplier, validationType, ref validationMessages);
                    if (result)
                    {
                        if ((dbAssignmentSubSupplier == null || dbAssignmentSubSupplier.Count <= 0) && validationType != ValidationType.Add)
                            dbAssignmentSubSupplier = GetAssignmentSubSuppliers(filteredAssignmentSubSupplier);

                        if (validationType == ValidationType.Delete || validationType == ValidationType.Update)
                        {
                            var assignmentSubSupplierIds = filteredAssignmentSubSupplier.Where(x => x.AssignmentSubSupplierId != null)
                                                                                        .Select(x => x.AssignmentSubSupplierId).ToList();

                            result = IsAssignmentSubSupplierExistInDb(assignmentSubSupplierIds,
                                                                      dbAssignmentSubSupplier,
                                                                      ref assignmentNotExists,
                                                                      ref validationMessages);

                            if (result && validationType == ValidationType.Delete)
                                result = IsChildRecordExistsInDb(assignmentSubSuppliers, dbAssignmentSubSupplier, ref validationMessages);

                            else if (result && validationType == ValidationType.Update)
                                result = IsRecordValidForUpdate(filteredAssignmentSubSupplier,
                                                                dbAssignmentSubSupplier,
                                                                ref dbTechnicalSpecialist,
                                                                ref validationMessages,
                                                                ref dbAssignments);
                        }
                        else if (validationType == ValidationType.Add)
                            result = IsRecordValidForAdd(filteredAssignmentSubSupplier,
                                                        ref dbAssignmentSubSupplier,
                                                         ref dbTechnicalSpecialist,
                                                         ref validationMessages,
                                                         ref dbAssignments);
                    }

                }
            }
            catch (Exception ex)
            {
                result = false;
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), assignmentSubSuppliers);
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), result, exception);
        }

        private bool IsRecordValidForAdd(IList<DomainModel.AssignmentSubSupplier> assignmentSubSupplier,
                                         ref IList<DbModel.AssignmentSubSupplier> dbAssignmentSubSupplier,
                                         ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialist,
                                         ref IList<ValidationMessage> validationMessages,
                                         ref IList<DbModel.Assignment> dbAssignments)
        {
            //IList<DbModel.Supplier> dbSuppliers = null;
            //IList<DbModel.SupplierPurchaseOrderSubSupplier> dbSubSuppliers = null;
            bool result = false;
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            var assignmentSubSupplierTs = assignmentSubSupplier.Where(x => x.AssignmentSubSupplierTS != null).SelectMany(x => x.AssignmentSubSupplierTS).ToList();
            var assignmentIds = assignmentSubSupplier.Where(x => x.AssignmentId > 0).Select(x1 => (int)x1.AssignmentId).Distinct().ToList();
            //var subSupplierIds = assignmentSubSupplier.Where(x => x.SubSupplierId > 0).Select(x1 => (int)x1.SubSupplierId).Distinct().ToList();
            var epin = assignmentSubSupplier.SelectMany(x => x.AssignmentSubSupplierTS).ToList().Select(x => x.Epin).ToList();
            List<string> epins = epin.ConvertAll(x => x.ToString());

            if (_assignmentService.IsValidAssignment(assignmentIds, ref dbAssignments, ref validationMessages, sub => sub.SupplierPurchaseOrder.SupplierPurchaseOrderSubSupplier,
                                                                                                                sup => sup.SupplierPurchaseOrder.Supplier.SupplierContact,
                                                                                                                assignSubSupplier => assignSubSupplier.AssignmentSubSupplier,
                                                                                                                assignTS => assignTS.AssignmentTechnicalSpecialist)

               // && _subSupplierService.IsValidSubSupplierID(subSupplierIds, ref dbAssignments, ref dbSubSuppliers, ref dbSuppliers, ref validationMessages, sup => sup.Supplier)
               //&& IsValidSupplierContact(subSupplierIds, assignmentSubSupplier, dbAssignments, ref validationMessages)
               // && IsUniqueAssignmentSubSupplier(assignmentSubSupplier, dbAssignments, ref dbAssignmentSubSupplier, ref validationMessages)
               && IsFirstVisitAlreadyAssocitedToAnotherSupplier(assignmentSubSupplier, dbAssignments, dbAssignmentSubSupplier, ref validationMessages)
               && Convert.ToBoolean(_technicalSpecialistService.IsRecordExistInDb(epins, ref dbTechnicalSpecialist, ref validationMessages).Result))
            {
                return !result;
            }

            return result;
        }

        private bool IsRecordValidForUpdate(IList<DomainModel.AssignmentSubSupplier> assignmentSubSupplier,
                                            IList<DbModel.AssignmentSubSupplier> dbAssignmentSubSupplier,
                                            ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialist,
                                            ref IList<ValidationMessage> validationMessages,
                                            ref IList<DbModel.Assignment> dbAssignments)
        {

            //IList<DbModel.Supplier> dbSuppliers = null;
            IList<ValidationMessage> messages = new List<ValidationMessage>();
            //IList<DbModel.SupplierPurchaseOrderSubSupplier> dbSubSuppliers = null;
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            if (messages?.Count <= 0)
            {
                if (IsRecordUpdateCountMatching(assignmentSubSupplier, dbAssignmentSubSupplier, ref messages))
                {
                    var assignmentIds = assignmentSubSupplier.Where(x => x.AssignmentId > 0).Select(x => (int)x.AssignmentId).ToList();
                    //  var SupplierIds = assignmentSubSupplier.Where(x => x.SupplierId > 0).Select(x => (int)x.SupplierId).ToList();//MS-TS Link
                    var epin = assignmentSubSupplier.Where(x => x.AssignmentSubSupplierTS != null).SelectMany(x1 => x1.AssignmentSubSupplierTS).Select(x2 => x2.Epin).ToList();
                    List<string> epins = epin.ConvertAll(x => x.ToString());

                    if (_assignmentService.IsValidAssignment(assignmentIds, ref dbAssignments, ref validationMessages, sub => sub.SupplierPurchaseOrder.SupplierPurchaseOrderSubSupplier,
                                                                                                               sup => sup.SupplierPurchaseOrder.Supplier.SupplierContact,
                                                                                                               assignSubSupplier => assignSubSupplier.AssignmentSubSupplier)

                        // && _subSupplierService.IsValidSubSupplierID(SupplierId, ref dbAssignments, ref dbSubSuppliers, ref dbSuppliers, ref validationMessages, sup => sup.Supplier)
                        // && IsValidSupplierContact(SupplierId, assignmentSubSupplier, dbAssignments, ref validationMessages)
                        && Convert.ToBoolean(_technicalSpecialistService.IsRecordExistInDb(epins, ref dbTechnicalSpecialist, ref validationMessages).Result)
                        && IsChildRecordExistsInDb(assignmentSubSupplier, dbAssignmentSubSupplier, ref validationMessages))
                         IsChildRecordUpdateCountMatching(assignmentSubSupplier, dbAssignmentSubSupplier, ref validationMessages);
                }
            }

            if (messages.Count > 0)
                validationMessages.AddRange(messages);

            return validationMessages?.Count <= 0;
        }

        private bool IsValidPayload(IList<DomainModel.AssignmentSubSupplier> assignmentSubSupplier,
                                    ValidationType validationType,
                                    ref IList<ValidationMessage> validationMessages)
        {
            var messages = new List<ValidationMessage>();

            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            var validationResults = _assignmentSubSupplierValidationService.Validate(JsonConvert.SerializeObject(assignmentSubSupplier), validationType);
            if (validationResults?.Count > 0)
                messages.Add(_messageDescriptions, ModuleType.Assignment, validationResults);

            if (validationResults.Count > 0)
                validationMessages.AddRange(messages);

            return validationMessages?.Count <= 0;
        }

        private bool IsAssignmentSubSupplierExistInDb(List<int?> assignmentSubSupplierIds,
                                                      IList<DbModel.AssignmentSubSupplier> dbAssignmentSubSupplier,
                                                      ref List<int?> assignmentSubSupplierNotExists,
                                                      ref IList<ValidationMessage> validationMessages)
        {
            List<ValidationMessage> messages = new List<ValidationMessage>();
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            if (dbAssignmentSubSupplier == null)
                dbAssignmentSubSupplier = new List<DbModel.AssignmentSubSupplier>();

            if (assignmentSubSupplierIds?.Count > 0)
            {
                assignmentSubSupplierNotExists = assignmentSubSupplierIds.Where(x => !dbAssignmentSubSupplier.Select(x1 => x1.Id).ToList().Contains((int)x)).ToList();
                assignmentSubSupplierNotExists?.ToList().ForEach(x =>
                {
                    messages.Add(_messageDescriptions, x, MessageType.AssignmentSubSupplierNotExists, x);
                });
            }

            if (messages.Count > 0)
                validationMessages.AddRange(messages);

            return messages.Count <= 0;
        }

        private bool IsRecordUpdateCountMatching(IList<DomainModel.AssignmentSubSupplier> assignmentSubSupplier,
                                                 IList<DbModel.AssignmentSubSupplier> dbAssignmentSubSupplier,
                                                 ref IList<ValidationMessage> validationMessages)
        {
            List<ValidationMessage> messages = new List<ValidationMessage>();
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();
            if (assignmentSubSupplier?.Select(x => x.SubSupplierId > 0).Count() > 0)//MS-TS Link
            {
                var notMatchedRecords = assignmentSubSupplier.Where(x => !dbAssignmentSubSupplier.ToList().Any(x1 => x1.UpdateCount.ToInt() == x.UpdateCount.ToInt() && x1.Id == x.AssignmentSubSupplierId)).ToList();
                notMatchedRecords.ForEach(x =>
                {
                    messages.Add(_messageDescriptions, x, MessageType.AssignmentSubSupplierUpdateCountMisMatch, x.AssignmentSubSupplierId);
                });
            }

            if (messages.Count > 0)
                validationMessages.AddRange(messages);

            return messages?.Count <= 0;
        }

        //private bool IsUniqueAssignmentSubSupplier(IList<DomainModel.AssignmentSubSupplier> assignmentSubSupplier,
        //                                           IList<DbModel.Assignment> dbAssignments,
        //                                           ref IList<DbModel.AssignmentSubSupplier> dbAssignmentSubSupplier,
        //                                           ref IList<ValidationMessage> validationMessages,
        //                                           params Expression<Func<DbModel.AssignmentSubSupplier, object>>[] includes)
        //{
        //    List<ValidationMessage> messages = new List<ValidationMessage>();
        //    if (validationMessages == null)
        //        validationMessages = new List<ValidationMessage>();

        //    var assignmentPartialSubSupplier = assignmentSubSupplier.Select(x => new { x.AssignmentId, x.SupplierId, x.AssignmentSubSupplierId, x.SupplierContactId }).ToList();//MS-TS Link
        //    var dbAssignmentSubSuppliers = dbAssignments.SelectMany(x => x.AssignmentSubSupplier).ToList();

        //    var duplicateRecords = dbAssignmentSubSuppliers.Where(x => assignmentPartialSubSupplier.Any(x1 => x1.AssignmentId == x.AssignmentId &&
        //                                                                                                      x1.SupplierId == x.SupplierId &&//MS-TS Link
        //                                                                                                      x1.SupplierContactId == x.SupplierContactId &&
        //                                                                                                      x1.AssignmentSubSupplierId != x.Id)).ToList();

        //    assignmentPartialSubSupplier.Where(x => duplicateRecords.Any(x1 => x1.AssignmentId == x.AssignmentId && x1.SupplierId == x.SupplierId))?.ToList().ForEach(x =>
        //    {
        //        messages.Add(_messageDescriptions, x.SupplierId, MessageType.AssignmentSubSupplierDuplicateRecord, x.SupplierId);//MS-TS Link
        //    });

        //    if (messages.Count > 0)
        //        validationMessages.AddRange(messages);

        //    dbAssignmentSubSupplier = dbAssignmentSubSuppliers;
        //    return messages?.Count <= 0;
        //}


        private bool IsValidSupplierContact(IList<int> subSupplierIds,
                                            IList<DomainModel.AssignmentSubSupplier> assignmentSubSupplier,
                                            IList<DbModel.Assignment> dbAssignments,
                                            ref IList<ValidationMessage> validationMessages)
        {
            List<ValidationMessage> messages = new List<ValidationMessage>();
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();
            if (subSupplierIds?.Count > 0)
            {
                var dbSubSupplierContacts = dbAssignments.SelectMany(x => x.SupplierPurchaseOrder.SupplierPurchaseOrderSubSupplier).ToList().Where(x => subSupplierIds.Contains(x.Id)).SelectMany(x => x.Supplier.SupplierContact).ToList();
                var subSupplierContactNotExists = assignmentSubSupplier.Where(x => x.SubSupplierContactId.HasValue).Where(x => !dbSubSupplierContacts.Any(x1 => x1.Id == x.SubSupplierContactId)).ToList();
                var mainSupplierContactNotExists = assignmentSubSupplier.Where(x => x.MainSupplierContactId.HasValue).Where(x => !dbSubSupplierContacts.Any(x1 => x1.Id == x.MainSupplierContactId)).ToList();

                subSupplierContactNotExists.ToList().ForEach(x =>
                {
                    messages.Add(_messageDescriptions, x, MessageType.AssignmentSubSupplierInvalidSupplierContact, x);
                });
                mainSupplierContactNotExists.ToList().ForEach(x =>
                {
                    messages.Add(_messageDescriptions, x, MessageType.AssignmentSubSupplierInvalidSupplierContact, x);
                });
                if (messages.Count > 0)
                    validationMessages.AddRange(messages);
            }
            return messages?.Count <= 0;
        }

        private IList<DbModel.Assignment> GetAssignments(IList<DomainModel.AssignmentSubSupplier> assignmentSubSupplierServices)
        {
            IList<DbModel.Assignment> dbAssignments = null;
            if (assignmentSubSupplierServices?.Count > 0)
            {
                var assignmentIds = assignmentSubSupplierServices.Select(x => x.AssignmentId).Distinct().ToList();
                dbAssignments = _assignmentRepository.FindBy(x => assignmentIds.Contains(x.Id)).ToList();
            }

            return dbAssignments;
        }

        private bool IsFirstVisitAlreadyAssocitedToAnotherSupplier(IList<DomainModel.AssignmentSubSupplier> subSupplier,
                                                                   IList<DbModel.Assignment> dbAssignment,
                                                                   IList<DbModel.AssignmentSubSupplier> dbSubSuplier,
                                                                   ref IList<ValidationMessage> validationMessages)
        {
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            var validMessages = validationMessages;
            var assignmentIds = subSupplier.ToList().Where(x => x.AssignmentId > 0).Select(x => x.AssignmentId).ToList();
            var mainSupplierAssignedToFirstVisit = assignmentIds.Where(x => dbAssignment.ToList().Any(x1 => x1.Id == x && x1.IsFirstVisit == true)).ToList();

            if (mainSupplierAssignedToFirstVisit.Count > 0)
            {
                mainSupplierAssignedToFirstVisit?.ForEach(x =>
                {
                    validMessages.Add(_messageDescriptions, x, MessageType.AssignmentSubSupplierHasFirstVisit, x);
                });
            }
            else
            {
                if (dbSubSuplier != null && dbSubSuplier.Count > 0)
                {
                    var subSupplierAlreadyHasFirstVisit = assignmentIds.Where(x => dbSubSuplier.Any(x1 => x1.AssignmentId == x && x1.IsFirstVisit == true))?.ToList();
                    if (subSupplierAlreadyHasFirstVisit.Count > 0)
                    {
                        subSupplierAlreadyHasFirstVisit.ForEach(x =>
                        {
                            validMessages.Add(_messageDescriptions, x, MessageType.AssignmentSubSupplierHasFirstVisit, x);
                        });
                    }
                }

            }
            validationMessages = validMessages;
            return validationMessages.Count <= 0;

        }

        private void ProcessSubSupplierTechnicalSpecialist(int assignmentSubSupplierId,
                                                            DomainModel.AssignmentSubSupplierTS assignmentSubSupplierTs,
                                                            IList<DbModel.AssignmentSubSupplierTechnicalSpecialist> dbAssignmentSubSupplierTs,
                                                            IList<DbModel.AssignmentTechnicalSpecialist> dbAssignmnetTechnicalSpecialists)
        {
            _assignmentSubSupplerTSRepository.AutoSave = false;
            if (assignmentSubSupplierTs.RecordStatus.IsRecordStatusNew())
            {
                if (dbAssignmnetTechnicalSpecialists?.Count > 0)
                {
                    var dbNewTS = _mapper.Map<DbModel.AssignmentSubSupplierTechnicalSpecialist>(assignmentSubSupplierTs, opt =>
                    {
                        opt.Items["isAssignId"] = false;
                        opt.Items["AssignmentTsId"] = dbAssignmnetTechnicalSpecialists;
                    });
                    if (dbNewTS != null)
                    {
                        _assignmentSubSupplerTSRepository.Add(dbNewTS);
                        _assignmentSubSupplerTSRepository.ForceSave();
                    }
                }
            }
            if (assignmentSubSupplierTs.RecordStatus.IsRecordStatusModified())
            {
                var recordToUpdate = dbAssignmentSubSupplierTs?.Where(x => x.Id == assignmentSubSupplierTs.AssignmentSubSupplierTSId)?.ToList();
                recordToUpdate.ForEach(TS =>
                {
                    _mapper.Map(assignmentSubSupplierTs, TS, opt =>
                    {
                        opt.Items["isAssignId"] = true;
                        opt.Items["AssignmentTsId"] = dbAssignmnetTechnicalSpecialists;

                    });
                    TS.LastModification = DateTime.UtcNow;
                    TS.UpdateCount = assignmentSubSupplierTs.UpdateCount.CalculateUpdateCount();
                    TS.ModifiedBy = assignmentSubSupplierTs.ModifiedBy;
                });
                _assignmentSubSupplerTSRepository.Update(recordToUpdate);
                _assignmentSubSupplerTSRepository.ForceSave();
            }

            if (assignmentSubSupplierTs.RecordStatus.IsRecordStatusDeleted())
            {
                var recordToDelete = dbAssignmentSubSupplierTs?.Where(x => x.Id == assignmentSubSupplierTs.AssignmentSubSupplierTSId)?.ToList();
                _assignmentSubSupplerTSRepository.Delete(recordToDelete);
            }
        }

        private bool IsChildRecordExistsInDb(IList<DomainModel.AssignmentSubSupplier> assignmentSubSupplier,
                                             IList<DbModel.AssignmentSubSupplier> dbAssignmentSubSupplier,
                                             ref IList<ValidationMessage> validationMessages)

        {
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();
            var messages = validationMessages;
            var subSupplierTsIds = assignmentSubSupplier.SelectMany(x => x.AssignmentSubSupplierTS)?
                                                               .Where(x => !x.RecordStatus.IsRecordStatusNew() && x.AssignmentSubSupplierTSId != null)
                                                               .Select(x => x.AssignmentSubSupplierTSId).ToList();

            var recordNotExists = subSupplierTsIds.Where(x => !dbAssignmentSubSupplier.SelectMany(x1 => x1.AssignmentSubSupplierTechnicalSpecialist).Any(x2 => x2.Id == x)).ToList();
            recordNotExists?.ForEach(x =>
            {
                messages.Add(_messageDescriptions, x, MessageType.AssignmentSubSupplierTechnicalSpecialistNotExists, x);
            });

            validationMessages = messages;
            return validationMessages?.Count <= 0;
        }

        private bool IsChildRecordUpdateCountMatching(IList<DomainModel.AssignmentSubSupplier> assignmentSubSuppliers,
                                                      IList<DbModel.AssignmentSubSupplier> dbAssignmentSubSupplier,
                                                      ref IList<ValidationMessage> validationMessages)
        {
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();
            var messages = validationMessages;
            var assignmentSubSupplierTS = assignmentSubSuppliers.SelectMany(x => x.AssignmentSubSupplierTS).ToList();
            var recordToValidate = assignmentSubSupplierTS.Where(x => x.RecordStatus.IsRecordStatusModified() && x.AssignmentSubSupplierTSId != null).ToList();
            var updateCountNotMatchingRecords = recordToValidate.Where(x => !dbAssignmentSubSupplier.SelectMany(x1 => x1.AssignmentSubSupplierTechnicalSpecialist).Any(x2 => x2.Id == x.AssignmentSubSupplierTSId && x2.UpdateCount == x.UpdateCount)).ToList();

            updateCountNotMatchingRecords?.ForEach(x =>
            {
                messages.Add(_messageDescriptions, x.AssignmentSubSupplierTSId, MessageType.AssignmentSubSupplierTechnicalSpecialistUpdateCountMisMatch, x.AssignmentSubSupplierTSId);
            });

            validationMessages = messages;

            return validationMessages?.Count <= 0;
        }


        #endregion
    }
}
