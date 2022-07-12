using AutoMapper;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Common.Models.Filters;
using Evolution.Common.Models.Responses;
using Evolution.Document.Domain.Interfaces.Documents;
using Evolution.Document.Domain.Models.Document;
using Evolution.Logging.Interfaces;
using Evolution.Visit.Domain.Interfaces.Data;
using Evolution.Visit.Domain.Interfaces.Visits;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.Visit.Domain.Models.Visits;

namespace Evolution.Visit.Core.Services
{
    public class VisitDocumentService : IVisitDocumentService
    {
        private readonly IMapper _mapper = null;
        private readonly IAppLogger<VisitDocumentService> _logger = null;
        private readonly IVisitRepository _repository = null;
        private readonly JObject _messages = null;
        private IDocumentService _service = null;

        public VisitDocumentService(IMapper mapper, IAppLogger<VisitDocumentService> logger, IVisitRepository repository, IDocumentService service, JObject messages)
        {
            this._mapper = mapper;
            this._logger = logger;
            this._repository = repository;
            this._service = service;
            this._messages = messages;

        }

        #region Public Methods

        public Response SaveVisitDocument(long visitId, IList<DomainModel.VisitDocuments> visitDocuments, bool commitChanges = true)
        {
            Exception exception = null;
            Response response = null;
            try
            {

            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString());
            }

            return response;
        }

        public Response ModifyVisitDocument(long visitId, IList<DomainModel.VisitDocuments> visitDocuments, bool commitChanges)
        {
            Exception exception = null;
            Response response = null;
            try
            {

            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString());
            }

            return response;
        }

        public Response DeleteVisitDocument(long visitId, IList<DomainModel.VisitDocuments> visitDocuments, bool commitChanges)
        {
            Exception exception = null;
            Response response = null;
            try
            {

            }
            catch(Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString());
            }
            return response;
        }

        public Response GetVisitDocument(DomainModel.VisitDocuments searchModel)
        {
            Exception exception = null;
            Response response = null;
            try
            {

            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString());
            }

            return response;
        }
        public Response GetSupplierPoVisitIds(int? supplierPoId)
        {
            Exception exception = null;
            List<DbModel.Visit> result = null;
            object response = null;
            List<string> strVisitIDs = new List<string>();
            try
            {
                result = this._repository.GetSupplierPoVisitIds(supplierPoId);
                if (result?.Count > 0)
                {
                    strVisitIDs = result?.ConvertAll<string>(x => x.Id.ToString());
                }
                List<ModuleDocument> visitDocuments = _service.Get(ModuleCodeType.VST, strVisitIDs).Result.Populate<List<ModuleDocument>>();
                response = visitDocuments?.Join(result,
                    vd => new {Id= vd.ModuleRefCode},
                    v=>new { Id = v.Id.ToString() },
                    (vd,v)=> new { vd,v})
                    .Select(x=>new  {
                        Id=x.vd.Id,
                        DocumentName=x.vd.DocumentName,
                        DocumentType=x.vd.DocumentType,
                        DocumentSize=x.vd.DocumentSize,
                        IsVisibleToTS=x.vd.IsVisibleToTS,
                        IsVisibleToCustomer=x.vd.IsVisibleToCustomer,
                        IsVisibleOutOfCompany=x.vd.IsVisibleOutOfCompany,
                        Status=x.vd.Status,
                        DocumentUniqueName=x.vd.DocumentUniqueName,
                        ModuleCode=x.vd.ModuleCode,
                        ModuleRefCode=x.vd.ModuleRefCode,
                        SubModuleRefCode=x.vd.SubModuleRefCode,
                        CreatedOn=x.vd.CreatedOn,
                        CreatedBy=x.vd.CreatedBy,
                        DisplayOrder=x.vd.DisplayOrder,
                        Comments=x.vd.Comments,
                        ExpiryDate=x.vd.ExpiryDate,
                        IsForApproval=x.vd.IsForApproval,
                        ApprovalDate=x.vd.ApprovalDate,
                        ApprovedBy=x.vd.ApprovedBy,
                        CoordinatorName=x.vd.CoordinatorName,
                        DocumentTitle=x.vd.DocumentTitle,
                        FilePath=x.vd.FilePath,
                        VisitReportNumber=x.v.ReportNumber
                    });
                
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString());
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, response, exception);
        }

        public Response GetAssignmentVisitDocuments(int? assignmentId)
        {
            Exception exception = null;
            List<DbModel.Visit> result = null;
            object response = null;
            List<string> strVisitIDs = new List<string>();
            try
            {
                result = this._repository.GetAssignmentVisitIds(assignmentId);
                if (result?.Count > 0)
                {
                    strVisitIDs = result?.ConvertAll<string>(x => x.Id.ToString());
                }
                if (strVisitIDs?.Count > 0)
                {
                    response = _service.Get(ModuleCodeType.VST, strVisitIDs).Result.Populate<List<ModuleDocument>>();
                    List<ModuleDocument> visitDocuments = _service.Get(ModuleCodeType.VST, strVisitIDs).Result.Populate<List<ModuleDocument>>();
                    response = visitDocuments?.Join(result,
                        vd => new { Id = vd.ModuleRefCode },
                        v => new { Id = v.Id.ToString() },
                        (vd, v) => new { vd, v })
                        .Select(x => new {
                            Id = x.vd.Id,
                            DocumentName = x.vd.DocumentName,
                            DocumentType = x.vd.DocumentType,
                            DocumentSize = x.vd.DocumentSize,
                            IsVisibleToTS = x.vd.IsVisibleToTS,
                            IsVisibleToCustomer = x.vd.IsVisibleToCustomer,
                            IsVisibleOutOfCompany = x.vd.IsVisibleOutOfCompany,
                            Status = x.vd.Status,
                            DocumentUniqueName = x.vd.DocumentUniqueName,
                            ModuleCode = x.vd.ModuleCode,
                            ModuleRefCode = x.vd.ModuleRefCode,
                            SubModuleRefCode = x.vd.SubModuleRefCode,
                            CreatedOn = x.vd.CreatedOn,
                            CreatedBy = x.vd.CreatedBy,
                            DisplayOrder = x.vd.DisplayOrder,
                            Comments = x.vd.Comments,
                            ExpiryDate = x.vd.ExpiryDate,
                            IsForApproval = x.vd.IsForApproval,
                            ApprovalDate = x.vd.ApprovalDate,
                            ApprovedBy = x.vd.ApprovedBy,
                            CoordinatorName = x.vd.CoordinatorName,
                            DocumentTitle = x.vd.DocumentTitle,
                            FilePath = x.vd.FilePath,
                            VisitReportNumber = x.v.ReportNumber
                        });
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString());
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, null, response, exception);
        }
        #endregion
    }
}
