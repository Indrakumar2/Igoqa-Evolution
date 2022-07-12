using AutoMapper;
using AutoMapper.QueryableExtensions;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Common.Models.Base;
using Evolution.DbRepository.Models.SqlDatabaseContext;
using Evolution.Document.Domain.Interfaces.Data;
using Evolution.Document.Domain.Models.Document;
using Evolution.GenericDbRepository.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Transactions;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;

namespace Evolution.Document.Infrastructure.Data
{
    public class DocumentRepository : GenericRepository<DbModel.Document>, IDocumentRepository
    {
        private IMapper _mapper = null;
        private EvolutionSqlDbContext _dbContext = null;
        private readonly AppEnvVariableBaseModel _environment = null;

        public DocumentRepository(EvolutionSqlDbContext dbContext, IMapper mapper, IOptions<AppEnvVariableBaseModel> environment) : base(dbContext)
        {
            this._mapper = mapper;
            _dbContext = dbContext;
            _environment = environment.Value;
            _dbContext.Database.SetCommandTimeout(Convert.ToInt32(_environment.SQLConnectionTimeout));
        }

        public IList<ModuleDocument> Get(IList<string> documentUniqueNames)
        {
            IList<ModuleDocument> moduleDocuments = null;
            IQueryable<DbModel.Document> whereClause = null;

            if (documentUniqueNames?.Count > 0)
            {
                whereClause = _dbContext.Document?.Where(x => documentUniqueNames.Contains(x.DocumentUniqueName) && x.IsDeleted != true);
                moduleDocuments = whereClause.ProjectTo<ModuleDocument>().ToList();
            }

            return moduleDocuments;
        }

        public IList<ModuleDocument> Get(ModuleDocument searchModel)
        {
            IQueryable<DbModel.Document> whereClause = null;
            whereClause = FilterRecords(searchModel);
            return whereClause.ProjectTo<ModuleDocument>().ToList();
        }

        private IQueryable<DbModel.Document> FilterRecords(ModuleDocument searchModel)
        {
            var dbSearchModel = _mapper.Map<ModuleDocument, DbModel.Document>(searchModel);
            Expression<Func<DbModel.Document, bool>> expression = null;

            // Added to remove default value assigned for SubModuleRefCode TS module
            if (searchModel.IsForApproval == true)
                expression = dbSearchModel.ToExpression(new List<string> { nameof(dbSearchModel.SubModuleRefCode), nameof(dbSearchModel.ModuleCode) });
            else
                expression = dbSearchModel.ToExpression();

            IQueryable<DbModel.Document> whereClause = _dbContext.Document;
            IQueryable<DbModel.Document> whereCotractDocument = null;

            if (searchModel.ModuleRefCode.HasEvoWildCardChar())
                whereClause = _dbContext.Document?.WhereLike(x => x.ModuleRefCode, searchModel.ModuleRefCode, '*');

            if (expression != null)
                whereClause = whereClause.Where(expression);

            if (searchModel.SubModuleRefCode.HasEvoWildCardChar())
                whereClause = whereClause?.WhereLike(x => x.SubModuleRefCode, searchModel.SubModuleRefCode, '*');

            //Search for Module Code In
            if (searchModel.ModuleCode != null)
            {
                var moduleCodes = searchModel.ModuleCode.Split(",");
                whereClause = whereClause?.Where(x => moduleCodes.Contains(x.ModuleCode.Trim()));
            }

            //Added for Document Approval Screen
            if (searchModel.IsForApproval == true)
                whereCotractDocument = whereClause?.Where(x => x.ModuleCode == "CNT");

            //CoordinatorName - only for document Approval Screen
            if (!string.IsNullOrEmpty(searchModel.CoordinatorName))
                whereClause = whereClause?.Where(x => x.Coordinator.SamaccountName.Trim() == searchModel.CoordinatorName.Trim());

            //Added for Document Approval Screen
            if (whereClause.Any() && (whereCotractDocument != null && whereCotractDocument.Any()) && searchModel.IsForApproval == true)
            {
                var documentId = whereClause.Select(x => x.Id);
                whereCotractDocument = whereCotractDocument.Where(x => !documentId.Contains(x.Id));
            }

            if (searchModel.IsForApproval == true)
                whereClause = whereCotractDocument != null && whereCotractDocument.Any() ? (whereClause.Any() ? whereClause.Concat(whereCotractDocument) : whereCotractDocument) : whereClause;

            // Fetch only completed records if Status is not passed
            if (string.IsNullOrEmpty(searchModel.Status))
                whereClause = whereClause.Where(x => x.Status.Trim() == DocumentStatusType.C.ToString());

            //Added for module wise document tab where it should exclude records which are not approved or rejected
            if (searchModel.IsForApproval == null)
                whereClause = whereClause?.Where(x => x.IsForApproval == searchModel.IsForApproval || x.IsForApproval == false);

            // Fetch only active and completed records always
            whereClause = whereClause.Where(x => x.IsDeleted != true);

            return whereClause;
        }

        public IList<ModuleDocument> Get(ModuleCodeType moduleCodeType, IList<string> moduleRefCodes = null, IList<string> subModuleRefCodes = null)
        {
            IQueryable<DbModel.Document> whereClause = _dbContext.Document.Where(x => x.ModuleCode == moduleCodeType.ToString());

            if (moduleRefCodes?.Count > 0)
            {
                whereClause = whereClause.Where(x => moduleRefCodes.Contains(x.ModuleRefCode));
            }

            if (subModuleRefCodes?.Count > 0)
            {
                whereClause = whereClause.Where(x => subModuleRefCodes.Contains(x.SubModuleRefCode));
            }
            // Fetch only completed records if Status is not passed
            whereClause = whereClause.Where(x => x.Status.Trim() == DocumentStatusType.C.ToString());
            // Fetch only active and completed records always
            whereClause = whereClause.Where(x => x.IsDeleted != true);

            return whereClause.ProjectTo<ModuleDocument>().ToList();
        }

        public string[] GetCustomerName(string moduleCode, string moduleRefCode)
        {
            string[] customer = null;
            if (moduleCode == ModuleCodeType.CNT.ToString())
                customer = _dbContext.Contract?.Where(x => x.ContractNumber == moduleRefCode)?.Select(x => new string[] { x.Customer.Code, x.Customer.Name })?.FirstOrDefault();

            if (moduleCode == ModuleCodeType.PRJ.ToString())
                customer = _dbContext.Project?.Where(x => x.ProjectNumber == Convert.ToInt32(moduleRefCode))?.Select(x => new string[] { x.Contract.Customer.Code, x.Contract.Customer.Name })?.FirstOrDefault();

            if (moduleCode == ModuleCodeType.ASGMNT.ToString())
                customer = _dbContext.Assignment?.Where(x => x.Id == Convert.ToInt32(moduleRefCode))?.ToList().Select(x1 => new string[] { x1.Project.Contract.Customer.Code, x1.Project.Contract.Customer.Name })?.FirstOrDefault();

            if (moduleCode == ModuleCodeType.SUPPO.ToString())
                customer = _dbContext.SupplierPurchaseOrder?.Where(x => x.Id == Convert.ToInt32(moduleRefCode))?.ToList().Select(x1 => new string[] { x1.Project.Contract.Customer.Code, x1.Project.Contract.Customer.Name })?.FirstOrDefault();

            if (moduleCode == ModuleCodeType.VST.ToString())
                customer = _dbContext.Visit?.Where(x => x.Id == Convert.ToInt32(moduleRefCode))?.Select(x => new string[] { x.Assignment.Project.Contract.Customer.Code, x.Assignment.Project.Contract.Customer.Name })?.FirstOrDefault();

            if (moduleCode == ModuleCodeType.TIME.ToString())
                customer = _dbContext.Timesheet?.Where(x => x.Id == Convert.ToInt32(moduleRefCode))?.Select(x => new string[] { x.Assignment.Project.Contract.Customer.Code, x.Assignment.Project.Contract.Customer.Name })?.FirstOrDefault();

            return customer;
        }

        public DocumentUploadPath GetDocumentUploadPath()
        {
            var DocumentUploadPath = _dbContext.DocumentUploadPath?.Where(x => x.IsFull == false && x.IsActive == true)?.OrderBy(x => x.Id).FirstOrDefault();
            return DocumentUploadPath;
        }

        public DocumentUploadPath UpdateDocumentUploadFull(int id)
        {
            var DocumentUploadPath = _dbContext.DocumentUploadPath?.Where(x => x.Id == id)?.FirstOrDefault();
            if (DocumentUploadPath != null)
            {
                DocumentUploadPath.IsFull = true;
                _dbContext.DocumentUploadPath.Update(DocumentUploadPath);
                return this.GetDocumentUploadPath();
            }
            else
            {
                return null;
            }
        }

        public string GetDocumentPathByName(string documentUniqueName)
        {
            string result = string.Empty;
            try
            {
                using (var tranScope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    result = _dbContext.Document?.Where(x => x.DocumentUniqueName == documentUniqueName && x.IsDeleted != true)?.Select(x => x.FilePath)?.FirstOrDefault();
                    tranScope.Complete();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public IList<ModuleDocument> FilterDocumentsByCompany(ModuleDocument model)
        {
            List<string> moduleRefCode = new List<string>();
            IQueryable<DbModel.Document> documentList = FilterRecords(model);
            IQueryable<DbModel.Document> document = null;
            var documentModuleList = documentList?.ToList();
            if (model.IsForApproval != null && model.IsForApproval == true)
            {
                var contractDocuments = documentModuleList?.Where(x => x.ModuleCode == Convert.ToString(ModuleCodeType.CNT))?.Select(x => x.ModuleRefCode)?.ToList();
                var projectDocuments = documentModuleList?.Where(x => x.ModuleCode == Convert.ToString(ModuleCodeType.PRJ))?.Select(x => x.ModuleRefCode)?.ToList();
                var assignmentDocuments = documentModuleList?.Where(x => x.ModuleCode == Convert.ToString(ModuleCodeType.ASGMNT))?.Select(x => x.ModuleRefCode)?.ToList();
                var supplierPODocuments = documentModuleList?.Where(x => x.ModuleCode == Convert.ToString(ModuleCodeType.SUPPO))?.Select(x => x.ModuleRefCode)?.ToList();
                var visitDocuments = documentModuleList?.Where(x => x.ModuleCode == Convert.ToString(ModuleCodeType.VST))?.Select(x => x.ModuleRefCode)?.ToList();
                var timesheetDocuments = documentModuleList?.Where(x => x.ModuleCode == Convert.ToString(ModuleCodeType.TIME))?.Select(x => x.ModuleRefCode)?.ToList();

                if (contractDocuments.Any() || projectDocuments.Any() || assignmentDocuments.Any() || supplierPODocuments.Any() || visitDocuments.Any() || timesheetDocuments.Any())
                {
                    int? companyId = _dbContext.Company.AsNoTracking()?.Where(com => com.Code == model.CompanyCode)?.Select(x => x.Id)?.FirstOrDefault();
                    List<DbModel.Document> filteredDocuments = new List<DbModel.Document>();
                    if (companyId != null)
                    {
                        if (contractDocuments.Any())
                        {
                            var moduleRef = _dbContext.Contract.Where(x => contractDocuments.Contains(x.ContractNumber) && x.ContractHolderCompanyId == companyId)?.Select(x => x.ContractNumber)?.ToList();
                            if (moduleRef?.Count > 0)
                                moduleRefCode.AddRange(moduleRef);
                        }
                        if (projectDocuments.Any())
                        {
                            var moduleRef = _dbContext.Project.Where(x => projectDocuments.Contains(x.Id.ToString()) && x.Contract.ContractHolderCompanyId == companyId)?.Select(x => x.Id.ToString())?.ToList();
                            if (moduleRef?.Count > 0)
                                moduleRefCode.AddRange(moduleRef);
                        }
                        if (assignmentDocuments.Any())
                        {
                            var moduleRef = _dbContext.Assignment.Where(x => assignmentDocuments.Contains(x.Id.ToString()) && (x.ContractCompanyId == companyId || x.OperatingCompanyId == companyId))?.Select(x => x.Id.ToString())?.ToList();
                            if (moduleRef?.Count > 0)
                                moduleRefCode.AddRange(moduleRef);
                        }
                        if (supplierPODocuments.Any())
                        {
                            var moduleRef = _dbContext.SupplierPurchaseOrder.Where(x => supplierPODocuments.Contains(x.Id.ToString()) && (x.Project.Contract.ContractHolderCompanyId == companyId))?.Select(x => x.Id.ToString())?.ToList();
                            if (moduleRef?.Count > 0)
                                moduleRefCode.AddRange(moduleRef);
                        }
                        if (visitDocuments.Any())
                        {
                            var moduleRef = _dbContext.Visit.Where(x => visitDocuments.Contains(x.Id.ToString()) && (x.Assignment.ContractCompanyId == companyId || x.Assignment.OperatingCompanyId == companyId))?.Select(x => x.Id.ToString())?.ToList();
                            if (moduleRef?.Count > 0)
                                moduleRefCode.AddRange(moduleRef);
                        }
                        if (timesheetDocuments.Any())
                        {
                            var moduleRef = _dbContext.Timesheet.Where(x => timesheetDocuments.Contains(x.Id.ToString()) && (x.Assignment.ContractCompanyId == companyId || x.Assignment.OperatingCompanyId == companyId))?.Select(x => x.Id.ToString())?.ToList();
                            if (moduleRef?.Count > 0)
                                moduleRefCode.AddRange(moduleRef);
                        }
                        if (moduleRefCode?.Any() == true)
                            document = documentList?.Where(x => moduleRefCode.Contains(x.ModuleRefCode));
                    }
                    else
                         document = documentList;
                }
            }
            else //Added for IGO QC D963
                document = documentList;
            return document?.ProjectTo<ModuleDocument>()?.OrderByDescending(x => x.CreatedOn)?.ToList();
        }

        public IList<ModuleDocument> GetDocumentsToBeProcessed(bool isFetchNewDoc)
        {
            IQueryable<DbModel.Document> documents = null;
            string status = DocumentStatusType.C.ToString();
            IQueryable<DbModel.DocumentMongoSync> documentMongoSyncs = _dbContext.DocumentMongoSync;
            if (isFetchNewDoc)
            {
                documents = _dbContext.Document
                            .Where(doc => doc.CreatedDate>= DateTime.Now.AddDays(-1) && doc.Status == status && !string.IsNullOrEmpty(doc.ModuleRefCode)
                           && !documentMongoSyncs.Any(sync => sync.DocumentUniqueName == doc.DocumentUniqueName
                           && (sync.IsCompleted == true || (sync.IsCompleted == false
                           && (sync.Status == MongoSyncStatusType.In_Progress.ToString()
                           || sync.Status == MongoSyncStatusType.Failed_Retryable.ToString())))));

            }
            else
            {
                documents = _dbContext.Document
                            .Where(doc => doc.CreatedDate >= DateTime.Now.AddDays(-1) && doc.Status == status && !string.IsNullOrEmpty(doc.ModuleRefCode)
                            && documentMongoSyncs.Any(sync => sync.DocumentUniqueName == doc.DocumentUniqueName && sync.IsCompleted == false && sync.FailCount < 5 && (MongoSyncStatusType.In_Progress.ToString() != sync.Status || MongoSyncStatusType.Failed_Non_Retryable.ToString() != sync.Status)));
            }

            long? maxId = documents?.DefaultIfEmpty()?.Max(x => x.Id);

            if (maxId <= 0) return null;

            return _dbContext.Document.Where(e => e.Id == maxId)
                .Select(x1 => new ModuleDocument
                {
                    DocumentUniqueName = x1.DocumentUniqueName,
                    DocumentName = x1.DocumentName,
                    ModuleRefCode = x1.ModuleRefCode,
                    SubModuleRefCode = x1.SubModuleRefCode,
                    ModuleCode = x1.ModuleCode,
                    DocumentType = x1.DocumentType,
                    FilePath = x1.FilePath,
                    DocumentSize = x1.Size,
                    Id = x1.Id
                }).AsNoTracking().ToList();
        }

        public ModuleDocument GetDocumentsToBeProcessed(string documentTitle, string modifiedBy)
        {
            string status = DocumentStatusType.C.ToString();
            string progressStatus = DocumentStatusType.IN.ToString();
            DbModel.Document document = _dbContext.Document
                .Where(doc => doc.Status == status
                        && doc.DocumentTitle == documentTitle
                        && doc.ModifiedBy == modifiedBy)?
                        .OrderByDescending(a => a.Id)?
                        .Take(1)?.FirstOrDefault();

            if (document != null && document.Id != 0)
            {
                //After pulling status should be updated as in progress
                document.Status = progressStatus;
                _dbContext.Document.Update(document);
                _dbContext.SaveChanges();

                return new ModuleDocument
                {
                    DocumentUniqueName = document.DocumentUniqueName,
                    DocumentName = document.DocumentName,
                    ModuleRefCode = document.ModuleRefCode,
                    SubModuleRefCode = document.SubModuleRefCode,
                    ModuleCode = document.ModuleCode,
                    DocumentType = document.DocumentType,
                    FilePath = document.FilePath,
                    DocumentSize = document.Size,
                    Id = document.Id
                };
            }
            else
                return null;
        }

        public IList<DbModel.Document> GetOrphandRecords()
        {
            string completedStatus = DocumentStatusType.C.ToString().ToLower();
            string crStatus = DocumentStatusType.CR.ToString().ToLower();
            string rStatus = DocumentStatusType.R.ToString().ToLower();
            IQueryable<DbModel.Document> orphandRecords = _dbContext.Document?.Where(x => (x.IsDeleted == true)
            || (DateTime.UtcNow.Date != x.CreatedDate.Date && x.Status.Trim().ToLower() == crStatus)
            || (DateTime.UtcNow.Date != x.CreatedDate.Date && x.Status.Trim().ToLower() == completedStatus
            && string.IsNullOrEmpty(x.ModuleRefCode)) || (x.Status.Trim().ToLower() == rStatus));
            return orphandRecords?.ToList();
        }
    }
}
