using AutoMapper;
using Evolution.AuditLog.Domain.Enums;
using Evolution.AuditLog.Domain.Extensions;
using Evolution.AuditLog.Domain.Interfaces.Audit;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Common.Models.Messages;
using Evolution.Common.Models.Responses;
using Evolution.Company.Domain.Interfaces.Companies;
using Evolution.Company.Domain.Interfaces.Data;
using Evolution.Company.Domain.Models.Companies;
using Evolution.Document.Domain.Interfaces.Documents;
using Evolution.Document.Domain.Models.Document;
using Evolution.Logging.Interfaces;
using Evolution.Master.Domain.Interfaces.Services;
using Evolution.Master.Domain.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;

namespace Evolution.Company.Core.Services
{
    public class CompanyDetailService : ICompanyDetailService
    {
        private readonly IAppLogger<CompanyDetailService> _logger = null;
        private readonly JObject _messageDescriptions = null;

        private readonly ICompanyRepository _repository = null;
        private readonly IAuditSearchService _auditSearchService = null;
        private readonly ICompanyService _companyService = null;
        private readonly ICompanyOfficeService _companyOfficeService = null;
        private readonly IDivisionService _divisionService = null;
        private readonly ICompanyDivisionService _companyDivisionService = null;
        private readonly ICompanyDivisionCostCenterService _companyDivisionCostCenterService = null;
        private readonly IDocumentService _companyDocumentService = null;
        private readonly ICompanyEmailTemplateService _companyEmailTemplateService = null;
        private readonly ICompanyExpectedMarginService _companyExpectedMarginService = null;
        private readonly ICompanyInvoiceService _companyInvoiceService = null;
        private readonly ICompanyNoteService _companyNoteService = null;
        private readonly ICompanyPayrollService _companyPayrollService = null;
        private readonly IExportPrefixService _exportPrefixService = null;
        private readonly IPayrollTypeService _payrollTypeService = null;
        private readonly ICompanyPayrollPeriodService _companyPayrollPeriodService = null;
        private readonly ICompanyTaxService _companyTaxService = null;
        private readonly DbModel.EvolutionSqlDbContext _dbContext = null;
        private readonly IMapper _mapper = null;


        public CompanyDetailService(DbModel.EvolutionSqlDbContext dbContext,
                                    ICompanyService companyService,
                                    ICompanyOfficeService companyOfficeService,
                                    ICompanyDivisionService companyDivisionService,
                                    ICompanyDivisionCostCenterService companyDivisionCostCenterService,
                                    IDocumentService documentService,
                                    ICompanyEmailTemplateService companyEmailTemplateService,
                                    ICompanyExpectedMarginService companyExpectedMarginService,
                                    ICompanyInvoiceService companyInvoiceService,
                                    ICompanyNoteService companyNoteService,
                                    ICompanyPayrollService companyPayrollService,
                                    IExportPrefixService _exportPrefixService,
                                    IPayrollTypeService payrollTypeService,
                                    ICompanyPayrollPeriodService companyPayrollPeriodService,
                                    ICompanyTaxService companyTaxService,
                                    IDivisionService divisionService,
                                    IAppLogger<CompanyDetailService> logger, JObject messages,
                                   IAuditSearchService auditSearchService,
                                    IMapper mapper,
                                    ICompanyRepository repository)
        {
            this._companyService = companyService;
            this._companyOfficeService = companyOfficeService;
            this._companyDivisionService = companyDivisionService;
            this._companyDivisionCostCenterService = companyDivisionCostCenterService;
            this._companyDocumentService = documentService;
            this._companyEmailTemplateService = companyEmailTemplateService;
            this._companyExpectedMarginService = companyExpectedMarginService;
            this._companyInvoiceService = companyInvoiceService;
            this._companyNoteService = companyNoteService;
            this._companyPayrollService = companyPayrollService;
            this._exportPrefixService = _exportPrefixService;
            this._payrollTypeService = payrollTypeService;
            this._companyPayrollPeriodService = companyPayrollPeriodService;
            this._companyTaxService = companyTaxService;
            this._divisionService = divisionService;
            this._dbContext = dbContext;
            this._logger = logger;
            this._messageDescriptions = messages;
            this._auditSearchService = auditSearchService;
            this._mapper = mapper;
            this._repository = repository;
        }

        public Response SaveCompanyDetail(IList<Domain.Models.Companies.CompanyDetail> companyDetails)
        {
            string companyCode = string.Empty;
            bool commitChange = true; //Need to remove later
            Response response = null;
            Exception exception = null;
            List<MessageDetail> errorMessages = null;
            IList<DbModel.Company> dbCompaniesNew = null;
            IList<Domain.Models.Companies.CompanyDetail> exsistingCompanies = new List<Domain.Models.Companies.CompanyDetail>();
            long? eventID = 0;
            try
            {
                if (companyDetails != null && companyDetails.Any(x => x.CompanyInfo != null))
                {
                    IList<DbModel.CompanyMessage> msgToBeInsert = null; IList<DbModel.CompanyMessage> msgToBeUpdate = null; IList<DbModel.CompanyMessage> msgToBeDelete = null;
                    List<DbModel.CompanyMessage> msgToInsert = null; List<DbModel.CompanyMessage> msgToUpdate = null; List<DbModel.CompanyMessage> msgToDelete = null;
                    var divisions = companyDetails?.SelectMany(x => x.CompanyDivisions?.Where(f => f.RecordStatus.IsRecordStatusNew() || f.RecordStatus.IsRecordStatusModified())?.Select(x1 => x1.DivisionName)?.ToList())?.ToList();
                    var payrolls = companyDetails?.SelectMany(x => x.CompanyPayrolls?.Where(f => f.RecordStatus.IsRecordStatusNew())?.Select(x1 => x1.PayrollType)?.ToList()).ToList();
                    var payrollPrefixes = companyDetails?.SelectMany(x => x.CompanyPayrolls?.Where(f => f.RecordStatus.IsRecordStatusNew() || f.RecordStatus.IsRecordStatusModified())?.Select(x1 => x1.ExportPrefix)?.ToList()).ToList();
                    //Commented for defect 45 Payroll CR -No need to add the PayrollType and ExportPrefix in Master Data
                    //if (modifiedPayrolls?.Count > 0)
                    //{
                    //    response = this.CheckAndUpdateMasterData(modifiedPayrolls);
                    //}
                    response = this.CheckAndCreateMasterData(divisions, payrolls, payrollPrefixes);
                    if (response.Code != MessageType.Success.ToId())
                        return response;
                    //To-Do: Will create helper method get TransactionScope instance based on requirement
                    CompanyDetail auditCompanyDetails = null;
                    CompanyDetail company = null;
                    List<DbModel.Document> dbDocuments = null;
                    using (var tranScope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.Snapshot }))
                    {
                        this._repository.AutoSave = false;
                        response = this.ProcessCompanyInfo(companyDetails?.Select(x => x?.CompanyInfo).ToList(), ref exsistingCompanies, ref dbCompaniesNew, commitChange);
                        if (response.Code == MessageType.Success.ToId())
                        {
                            if (response.Result != null)
                            {
                                var newlyAddedDbCompanies = response.Result?.Populate<IList<Domain.Models.Companies.Company>>()?.ToList();
                                foreach (var dbCompany in newlyAddedDbCompanies)
                                {
                                    company = companyDetails.FirstOrDefault(x => x.CompanyInfo.CompanyMiiwaid == dbCompany.CompanyMiiwaid);
                                    if (company != null)
                                    {
                                        companyCode = company.CompanyInfo.CompanyCode;

                                        response = this.ProcessCompanyAddress(companyCode, company.CompanyOffices, commitChange);
                                        if (response.Code != MessageType.Success.ToId())
                                            return response;

                                        response = this.ProcessCompanyDivision(companyCode, company.CompanyDivisions, company.CompanyDivisionCostCenters, commitChange);
                                        if (response.Code != MessageType.Success.ToId())
                                            return response;

                                        response = this.ProcessCompanyDivisionCostCenter(companyCode, company.CompanyDivisionCostCenters, commitChange);
                                        if (response.Code != MessageType.Success.ToId())
                                            return response;

                                        response = this.ProcessCompanyEmailTemplate(companyCode, company.CompanyEmailTemplates, ref msgToBeInsert, ref msgToBeUpdate, ref msgToBeDelete, commitChange);
                                        if (response.Code != MessageType.Success.ToId())
                                            return response;

                                        response = this.ProcessCompanyExpectedMargin(companyCode, company.CompanyExpectedMargins, commitChange);
                                        if (response.Code != MessageType.Success.ToId())
                                            return response;

                                        response = this.ProcessCompanyInvoiceTemplate(companyCode, company.CompanyInvoiceInfo, ref msgToInsert, ref msgToUpdate, ref msgToDelete, commitChange);
                                        if (response.Code != MessageType.Success.ToId())
                                            return response;

                                        response = this.ProcessCompanyNote(companyCode, company.CompanyNotes, commitChange);
                                        if (response.Code != MessageType.Success.ToId())
                                            return response;

                                        response = this.ProcessCompanyPayroll(companyCode, company.CompanyPayrolls, company.CompanyPayrollPeriods, commitChange);
                                        if (response.Code != MessageType.Success.ToId())
                                            return response;
                                        response = this.ProcessCompanyPayrollPeriod(companyCode, company.CompanyPayrollPeriods, commitChange);
                                        if (response.Code != MessageType.Success.ToId())
                                            return response;

                                        response = this.ProcessCompanyQualification(companyCode, company.CompanyQualifications, commitChange);
                                        if (response.Code != MessageType.Success.ToId())
                                            return response;

                                        response = this.ProcessCompanyTax(companyCode, company.CompanyTaxes, commitChange);
                                        if (response.Code != MessageType.Success.ToId())
                                            return response;

                                        auditCompanyDetails = ObjectExtension.Clone(company);
                                       
                                        response = this.ProcessCompanyDocument(companyCode, company.CompanyDocuments, commitChange, ref dbDocuments);
                                        if (response.Code != MessageType.Success.ToId())
                                            return response;
                                        if (response != null && response.Code == MessageType.Success.ToId())
                                        {
                                            tranScope.Complete();
                                        }
                                    }
                                }
                            }
                        }
                        else
                            return response;
                    }
                    if (response.Code == MessageType.Success.ToId())
                    {
                        auditCompanyDetails.dbModule = _auditSearchService.GetAuditModule(new List<string>() { SqlAuditModuleType.Company.ToString(),
                                                                                                            SqlAuditModuleType.CompanyDivision.ToString(),
                                                                                                            SqlAuditModuleType.CompanyDivisionCostCenter.ToString(),
                                                                                                            SqlAuditModuleType.CompanyExpectedMargin.ToString(),
                                                                                                            SqlAuditModuleType.CompanyNote.ToString(),
                                                                                                            SqlAuditModuleType.CompanyOffice.ToString(),
                                                                                                            SqlAuditModuleType.CompanyPayroll.ToString(),
                                                                                                            SqlAuditModuleType.CompanyPayrollPeriod.ToString(),
                                                                                                            SqlAuditModuleType.CompanyQualification.ToString(),
                                                                                                            SqlAuditModuleType.CompanyTax.ToString(),
                                                                                                            SqlAuditModuleType.CompanyEmailTemplate.ToString(),
                                                                                                            SqlAuditModuleType.CompanyInvoice.ToString(),
                                                                                                            SqlAuditModuleType.CompanyDocument.ToString()
                                            });
                        if (company.CompanyInfo.RecordStatus.IsRecordStatusNew())
                        {
                            var auditresult = this.AuditLog(auditCompanyDetails,
                                                            dbCompaniesNew.FirstOrDefault(),
                                                          ValidationType.Add.ToAuditActionType(),
                                                          SqlAuditModuleType.Company,
                                                          null,
                                                          null,
                                                          ref eventID, exsistingCompanies.FirstOrDefault(), ref msgToBeInsert, ref msgToBeUpdate, ref msgToBeDelete,
                                                          ref msgToInsert, ref msgToUpdate, ref msgToDelete, dbDocuments);
                        }
                        else
                        {


                            var auditresult = this.AuditLog(auditCompanyDetails,
                                                  dbCompaniesNew.FirstOrDefault(),
                                                 ValidationType.Update.ToAuditActionType(),
                                                 SqlAuditModuleType.Company,
                                                 null,
                                                 null,
                                                 ref eventID, exsistingCompanies.FirstOrDefault(), ref msgToBeInsert, ref msgToBeUpdate, ref msgToBeDelete,
                                                 ref msgToInsert, ref msgToUpdate, ref msgToDelete, dbDocuments);



                        }

                    }
                }
                else if (companyDetails == null || companyDetails.Any(x => x.CompanyInfo == null))
                {
                    var message = new List<ValidationMessage>
                        {
                            { _messageDescriptions, companyDetails, MessageType.InvalidPayLoad, companyDetails }
                        };
                    return new Response().ToPopulate(ResponseType.Warning, null, null, message, null, exception, null);
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), companyDetails);
            }
            finally
            {
                this._repository.AutoSave = true;
            }

            return new Response().ToPopulate(ResponseType.Success, null, errorMessages, null, null, exception);
        }

        private Response ProcessCompanyInfo(IList<Domain.Models.Companies.Company> company, ref IList<Domain.Models.Companies.CompanyDetail> exsistingCompanies, ref IList<DbModel.Company> dbCompanies, bool commitChanges)
        {
            Response response = null;
            Exception exception = null;
            List<MessageDetail> errorMessages = null;

            try
            {
                errorMessages = new List<MessageDetail>();
                if (company != null)
                {
                    response = this._companyService.SaveCompany(company, ref dbCompanies, commitChanges);
                    if (response.Code == MessageType.Success.ToId())
                        response = this._companyService.ModifyCompany(company, ref exsistingCompanies, ref dbCompanies, commitChanges);
                }
                return response;
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), company);
            }

            return new Response().ToPopulate(ResponseType.Success, null, errorMessages, null, null, exception);
        }

        private Response ProcessCompanyAddress(string companyCode, IList<Domain.Models.Companies.CompanyAddress> models, bool commitChanges)
        {
            Response response = null;
            Exception exception = null;
            List<MessageDetail> errorMessages = null;
            try
            {
                errorMessages = new List<MessageDetail>();
                if (models != null && models.Count > 0)
                {
                    response = this._companyOfficeService.DeleteCompanyAddress(companyCode, models, commitChanges);
                    if (response.Code == MessageType.Success.ToId())
                    {
                        response = this._companyOfficeService.SaveCompanyAddress(companyCode, models, commitChanges);
                        if (response.Code == MessageType.Success.ToId())
                            response = this._companyOfficeService.ModifyCompanyAddress(companyCode, models, commitChanges);
                    }
                    return response;
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), models);
            }

            return new Response().ToPopulate(ResponseType.Success, null, errorMessages, null, null, exception);
        }

        private Response ProcessCompanyDivision(string companyCode, IList<Domain.Models.Companies.CompanyDivision> companyDivisions, IList<Domain.Models.Companies.CompanyDivisionCostCenter> companyDivisionCostCenters, bool commitChange)
        {
            Response response = null;
            Exception exception = null;
            List<MessageDetail> errorMessages = null;
            try
            {
                errorMessages = new List<MessageDetail>();

                //Removing Cost Center before deleting Division.
                if (companyDivisionCostCenters?.Count > 0)
                {
                    response = this.RemoveCompanyDivisionCostCenter(companyCode, companyDivisionCostCenters, commitChange);
                    if (response.Code != MessageType.Success.ToId())
                        return response;
                }

                if (companyDivisions?.Count > 0)
                {
                    response = this._companyDivisionService.DeleteCompanyDivision(companyCode, companyDivisions, commitChange);
                    if (response.Code == MessageType.Success.ToId())
                    {
                        response = this._companyDivisionService.SaveCompanyDivision(companyCode, companyDivisions, commitChange);
                        if (response.Code == MessageType.Success.ToId())
                            response = this._companyDivisionService.ModifyCompanyDivision(companyCode, companyDivisions, commitChange);
                    }
                    return response;
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), companyDivisions);
            }

            return new Response().ToPopulate(ResponseType.Success, null, errorMessages, null, null, exception);
        }

        private Response ProcessCompanyDivisionCostCenter(string companyCode, IList<Domain.Models.Companies.CompanyDivisionCostCenter> models, bool commitChanges)
        {
            Response response = null;
            Exception exception = null;
            List<MessageDetail> errorMessages = null;
            try
            {
                errorMessages = new List<MessageDetail>();
                if (models != null)
                {
                    var groupByDivision = models.GroupBy(x => x.Division);
                    if (groupByDivision.Count() > 0)
                    {
                        foreach (var divisionGroup in groupByDivision)
                        {
                            //response = this._companyDivisionCostCenterService.DeleteCompanyCostCenter(companyCode, divisionGroup.Key, divisionGroup.Select(x => x).ToList(), commitChanges);
                            //if (response.Code == MessageType.Success.ToId())
                            //{
                            response = this._companyDivisionCostCenterService.SaveCompanyCostCenter(companyCode, divisionGroup.Key, divisionGroup.Select(x => x).ToList(), commitChanges);
                            if (response.Code == MessageType.Success.ToId())
                            {
                                response = this._companyDivisionCostCenterService.ModifyCompanyCostCenter(companyCode, divisionGroup.Key, divisionGroup.Select(x => x).ToList(), commitChanges);
                                if (response.Code != MessageType.Success.ToId())
                                    break;
                            }
                            else
                                break;
                            //}
                            //else
                            //    break;
                        }
                        return response;
                    }
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), models);
            }

            return new Response().ToPopulate(ResponseType.Success, null, errorMessages, null, null, exception);
        }

        private Response ProcessCompanyDocument(string companyCode, IList<ModuleDocument> models, bool commitChanges, ref List<DbModel.Document> dbDocuments)
        {
            Response response = null;
            Exception exception = null;
            List<MessageDetail> errorMessages = null;
            try
            {
                errorMessages = new List<MessageDetail>();
                if (models != null)
                {
                    models?.ToList()?.ForEach(x => { x.ModuleRefCode = companyCode; });
                    response = this._companyDocumentService.Delete(models, commitChanges);
                    if (response.Code == MessageType.Success.ToId())
                    {
                        response = this._companyDocumentService.Save(models, ref dbDocuments, commitChanges);
                        if (response.Code == MessageType.Success.ToId())
                        {
                            response = this._companyDocumentService.Modify(models, ref dbDocuments, commitChanges);
                        }
                    }
                    return response;
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), models);
            }

            return new Response().ToPopulate(ResponseType.Success, null, errorMessages, null, null, exception);
        }

        private Response ProcessCompanyExpectedMargin(string companyCode, IList<Domain.Models.Companies.CompanyExpectedMargin> models, bool commitChanges)
        {
            Response response = null;
            Exception exception = null;
            List<MessageDetail> errorMessages = null;
            try
            {
                errorMessages = new List<MessageDetail>();
                if (models != null)
                {
                    response = this._companyExpectedMarginService.DeleteCompanyExpectedMargin(companyCode, models, commitChanges);
                    if (response.Code == MessageType.Success.ToId())
                    {
                        response = this._companyExpectedMarginService.SaveCompanyExpectedMargin(companyCode, models, commitChanges);
                        if (response.Code == MessageType.Success.ToId())
                            response = this._companyExpectedMarginService.ModifyCompanyExpectedMargin(companyCode, models, commitChanges);
                    }
                    return response;
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), models);
            }

            return new Response().ToPopulate(ResponseType.Success, null, errorMessages, null, null, exception);
        }

        private Response ProcessCompanyNote(string companyCode, IList<Domain.Models.Companies.CompanyNote> models, bool commitChanges)
        {
            Response response = null;
            Exception exception = null;
            List<MessageDetail> errorMessages = null;
            try
            {
                errorMessages = new List<MessageDetail>();
                if (models != null)
                {
                    response = this._companyNoteService.DeleteCompanyNote(companyCode, models, commitChanges);
                    if (response.Code == MessageType.Success.ToId())
                    {
                        response = this._companyNoteService.SaveCompanyNote(companyCode, models, commitChanges);
                        if (response.Code == MessageType.Success.ToId())
                            response = this._companyNoteService.ModifyCompanyNote(companyCode, models, commitChanges);
                    }
                    return response;
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), models);
            }

            return new Response().ToPopulate(ResponseType.Success, null, errorMessages, null, null, exception);
        }

        private Response ProcessCompanyPayroll(string companyCode, IList<Domain.Models.Companies.CompanyPayroll> models, IList<Domain.Models.Companies.CompanyPayrollPeriod> companyPayrollPeriods, bool commitChange)
        {
            Response response = null;
            Exception exception = null;
            List<MessageDetail> errorMessages = null;
            try
            {
                errorMessages = new List<MessageDetail>();

                //Removing Payroll Period before deleting Payroll.
                if (companyPayrollPeriods?.Count > 0)
                {
                    response = this.RemoveCompanyPayrollPeriod(companyCode, companyPayrollPeriods, commitChange);
                    if (response.Code != MessageType.Success.ToId())
                        return response;
                }

                if (models != null)
                {
                    response = this._companyPayrollService.DeleteCompanyPayroll(companyCode, models, commitChange);
                    if (response.Code == MessageType.Success.ToId())
                    {
                        response = this._companyPayrollService.SaveCompanyPayroll(companyCode, models, commitChange);
                        if (response.Code == MessageType.Success.ToId())
                            response = this._companyPayrollService.ModifyCompanyPayroll(companyCode, models, commitChange);
                    }
                    return response;
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), models);
            }

            return new Response().ToPopulate(ResponseType.Success, null, errorMessages, null, null, exception);
        }

        private Response ProcessCompanyPayrollPeriod(string companyCode, IList<Domain.Models.Companies.CompanyPayrollPeriod> models, bool commitChanges)
        {
            Response response = null;
            Exception exception = null;
            List<MessageDetail> errorMessages = null;
            try
            {
                errorMessages = new List<MessageDetail>();
                if (models != null)
                {
                    var groupByPayroll = models.GroupBy(x => x.CompanyPayrollId);
                    if (groupByPayroll.Count() > 0)
                    {
                        foreach (var divisionGroup in groupByPayroll)
                        {
                            //response = this._companyPayrollPeriodService.DeleteCompanyPayrollPeriod(companyCode, divisionGroup.Key, divisionGroup.Select(x => x).ToList(), commitChanges);
                            //if (response.Code == MessageType.Success.ToId())
                            //{
                            var payrollperiod = divisionGroup.Select(x => x).ToList();
                            var payrollName = payrollperiod[0].PayrollType;
                            response = this._companyPayrollPeriodService.SaveCompanyPayrollPeriod(companyCode, payrollName, divisionGroup.Key, divisionGroup.Select(x => x).ToList(), commitChanges);
                            if (response.Code == MessageType.Success.ToId())
                            {
                                response = this._companyPayrollPeriodService.ModifyCompanyPayrollPeriod(companyCode, payrollName, divisionGroup.Key, divisionGroup.Select(x => x).ToList(), commitChanges);
                                if (response.Code != MessageType.Success.ToId())
                                    break;
                            }
                            else
                                break;
                            //}
                        }
                        return response;
                    }
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), models);
            }

            return new Response().ToPopulate(ResponseType.Success, null, errorMessages, null, null, exception);
        }

        private Response ProcessCompanyTax(string companyCode, IList<Domain.Models.Companies.CompanyTax> models, bool commitChanges)
        {
            Response response = null;
            Exception exception = null;
            List<MessageDetail> errorMessages = null;
            try
            {
                errorMessages = new List<MessageDetail>();
                if (models != null)
                {
                    response = this._companyTaxService.DeleteCompanyTax(companyCode, models, commitChanges);
                    if (response.Code == MessageType.Success.ToId())
                    {
                        response = this._companyTaxService.SaveCompanyTax(companyCode, models, commitChanges);
                        if (response.Code == MessageType.Success.ToId())
                            response = this._companyTaxService.ModifyCompanyTax(companyCode, models, commitChanges);
                    }
                    return response;
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), models);
            }

            return new Response().ToPopulate(ResponseType.Success, null, errorMessages, null, null, exception);
        }

        private Response ProcessCompanyQualification(string companyCode, IList<Domain.Models.Companies.CompanyQualification> models, bool commitChanges)
        {
            Exception exception = null;
            List<MessageDetail> errorMessages = null;
            try
            {
                //TODO : Implement Later As Requirement is not clear.

                //errorMessages = new List<MessageDetail>();
                //if (models != null)
                //{
                //response = this._companyQualificationService.DeleteCompanyTax(companyCode, models, commitChanges);
                //if (response.Code == MessageType.Success.ToId())
                //{
                //    response = this._companyQualificationService.SaveCompanyTax(companyCode, models, commitChanges);
                //    if (response.Code == MessageType.Success.ToId())
                //        response = this._companyQualificationService.ModifyCompanyTax(companyCode, models, commitChanges);
                //      
                //}  
                //    return response;
                //}
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), models);
            }

            return new Response().ToPopulate(ResponseType.Success, null, errorMessages, null, null, exception);
        }

        private Response ProcessCompanyEmailTemplate(string companyCode, Domain.Models.Companies.CompanyEmailTemplate model, ref IList<DbModel.CompanyMessage> msgToBeInsert, ref IList<DbModel.CompanyMessage> msgToBeUpdate, ref IList<DbModel.CompanyMessage> msgToBeDelete, bool commitChanges)
        {
            Response response = null;
            Exception exception = null;
            List<MessageDetail> errorMessages = null;

            try
            {

                errorMessages = new List<MessageDetail>();

                if (model != null)
                {
                    //response = this._companyEmailTemplateService.AddCompanyEmailTemplate(companyCode, model, commitChanges);
                    //if (response.Code == MessageType.Success.ToId())
                    response = this._companyEmailTemplateService.ModifyCompanyEmailTemplate(companyCode, model, ref msgToBeInsert, ref msgToBeUpdate, ref msgToBeDelete, commitChanges);
                    //else
                    return response;
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), model);
            }

            return new Response().ToPopulate(ResponseType.Success, null, errorMessages, null, null, exception);
        }

        private Response ProcessCompanyInvoiceTemplate(string companyCode, Domain.Models.Companies.CompanyInvoice model, ref List<DbModel.CompanyMessage> msgToInsert, ref List<DbModel.CompanyMessage> msgToUpdate, ref List<DbModel.CompanyMessage> msgToDelete, bool commitChanges)
        {
            Response response = null;
            Exception exception = null;
            List<MessageDetail> errorMessages = null;
            try
            {
                errorMessages = new List<MessageDetail>();
                if (model != null)
                {
                    //response = this._companyInvoiceService.AddCompanyInvoice(companyCode, model, commitChanges);
                    //if (response.Code == MessageType.Success.ToId())
                    response = this._companyInvoiceService.ModifyCompanyInvoice(companyCode, model, ref msgToInsert, ref msgToUpdate, ref msgToDelete, commitChanges);
                    //else
                    return response;

                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), model);
            }

            return new Response().ToPopulate(ResponseType.Success, null, errorMessages, null, null, exception);
        }

        private Response RemoveCompanyDivisionCostCenter(string companyCode, IList<Domain.Models.Companies.CompanyDivisionCostCenter> companyCostCenters, bool commitChange)
        {
            Response response = null;
            var groupByDivision = companyCostCenters?.Where(x => x.RecordStatus.IsRecordStatusDeleted()).GroupBy(x => x.Division);
            if (groupByDivision.Count() > 0)
            {
                foreach (var divisionGroup in groupByDivision)
                {
                    response = this._companyDivisionCostCenterService.DeleteCompanyCostCenter(companyCode, divisionGroup.Key, divisionGroup.Select(x => x).ToList(), commitChange);
                    if (response.Code != MessageType.Success.ToId())
                        break;
                }
            }
            return response ?? new Response().ToPopulate(ResponseType.Success);
        }

        private Response RemoveCompanyPayrollPeriod(string companyCode, IList<Domain.Models.Companies.CompanyPayrollPeriod> companyPayrollPeriods, bool commitChange)
        {
            Response response = null;
            var groupByPayroll = companyPayrollPeriods?.Where(x => x.RecordStatus.IsRecordStatusDeleted()).GroupBy(x => x.CompanyPayrollId);
            if (groupByPayroll.Count() > 0)
            {
                foreach (var payrollGroup in groupByPayroll)
                {
                    var payrollPeriod = payrollGroup.Select(x => x).ToList();
                    var payrollName = payrollPeriod[0].PayrollType;
                    response = this._companyPayrollPeriodService.DeleteCompanyPayrollPeriod(companyCode, payrollName, payrollGroup.Key, payrollGroup.Select(x => x).ToList(), commitChange);
                    if (response.Code != MessageType.Success.ToId())
                        break;
                }
            }
            return response ?? new Response().ToPopulate(ResponseType.Success);
        }
        private Response CheckAndUpdateMasterData(IList<Domain.Models.Companies.CompanyPayroll> payrollNames)
        {
            Response response = null;
            List<MessageDetail> errorMessages = new List<MessageDetail>();
            response = this.CheckAndUpdatePayrollData(payrollNames);
            return response;
        }
        private Response CheckAndCreateMasterData(IList<string> divisionNames, IList<string> payrollNames, IList<string> payrollExportPrefixes)
        {
            Response response = null;
            List<MessageDetail> errorMessages = new List<MessageDetail>();

            response = this.CheckAndCreateDivisionData(divisionNames);
            //Commented for defect 45 Payroll CR -No need to add the PayrollType and ExportPrefix in Master Data
            //if (response.Code == ResponseType.Success.ToId())
            //{
            //    //response = this.CheckAndCreatePayrollData(payrollNames);
            //    if (response.Code == ResponseType.Success.ToId())
            //    {
            //        response = this.CheckAndCreatePayrollExportPrefixData(payrollExportPrefixes);
            //    }
            //}
            return response;
        }

        private Response CheckAndCreateDivisionData(IList<string> divisionNames)
        {
            Response response = null;
            List<MessageDetail> errorMessages = new List<MessageDetail>();
            if (divisionNames != null)
            {
                IList<Division> divisionToBeInsert = new List<Division>();
                divisionNames.ToList().ForEach(x =>
                {
                    divisionToBeInsert.Add(new Division() { Name = x, RecordStatus = "N" });
                });

                if (divisionToBeInsert.Count > 0)
                    response = this._divisionService.SaveDivision(divisionToBeInsert, true);
            }
            return response ?? new Response().ToPopulate(ResponseType.Success);
        }

        private Response CheckAndUpdatePayrollData(IList<Domain.Models.Companies.CompanyPayroll> payrollNames)
        {
            Response response = null;
            List<MessageDetail> errorMessages = new List<MessageDetail>();
            if (payrollNames != null)
            {
                IList<PayrollType> payrollToBeUpdated = new List<PayrollType>();
                payrollNames?.Distinct().ToList().ForEach(x =>
                {
                    payrollToBeUpdated.Add(new PayrollType() { Name = x.PayrollType, RecordStatus = "M", Id = x.CompanyPayrollId });
                });
                if (payrollToBeUpdated?.Count > 0)
                    response = this._payrollTypeService.UpdatePayrollType(payrollToBeUpdated, true);
            }
            return response ?? new Response().ToPopulate(ResponseType.Success);
        }
        private Response CheckAndCreatePayrollData(IList<string> payrollNames)
        {
            Response response = null;
            List<MessageDetail> errorMessages = new List<MessageDetail>();
            if (payrollNames != null)
            {
                IList<PayrollType> payrollToBeInsert = new List<PayrollType>();
                payrollNames.Distinct().ToList().ForEach(x =>
                {
                    payrollToBeInsert.Add(new PayrollType() { Name = x, RecordStatus = "N" });
                });

                if (payrollToBeInsert.Count > 0)
                    response = this._payrollTypeService.SavePayrollType(payrollToBeInsert, true);
            }
            return response ?? new Response().ToPopulate(ResponseType.Success);
        }

        private Response CheckAndCreatePayrollExportPrefixData(IList<string> payrollExportPrefixes)
        {
            Response response = null;
            List<MessageDetail> errorMessages = new List<MessageDetail>();
            if (payrollExportPrefixes != null)
            {
                IList<ExportPrefix> exportPrefixToBeInsert = new List<ExportPrefix>();
                payrollExportPrefixes.Distinct().ToList().ForEach(x =>
                {
                    exportPrefixToBeInsert.Add(new ExportPrefix() { Name = x, RecordStatus = "N" });
                });

                if (exportPrefixToBeInsert.Count > 0)
                    response = this._exportPrefixService.SaveExportPrefix(exportPrefixToBeInsert, true);
            }
            return response ?? new Response().ToPopulate(ResponseType.Success);
        }

        private void RollbackTransaction()
        {
            if (_dbContext.Database.CurrentTransaction != null)
                _dbContext.Database.RollbackTransaction();
        }

        private Response AuditLog(Domain.Models.Companies.CompanyDetail companyDetails,
                              DbModel.Company dbCompany,
                             SqlAuditActionType sqlAuditActionType,
                             SqlAuditModuleType sqlAuditModuleType,
                             object oldData,
                             object newData,
                             ref long? eventId,
                             Domain.Models.Companies.CompanyDetail exsistingCompanies, ref IList<DbModel.CompanyMessage> msgToBeInsert,
                             ref IList<DbModel.CompanyMessage> msgToBeUpdate, ref IList<DbModel.CompanyMessage> msgToBeDelete,
                             ref List<DbModel.CompanyMessage> msgToInsert,
                             ref List<DbModel.CompanyMessage> msgToUpdate, ref List<DbModel.CompanyMessage> msgToDelete,
                             List<DbModel.Document> dbDocuments = null)
        {

            Exception exception = null;
            Response result = new Response();
            result = _auditSearchService.AuditLog(companyDetails, ref eventId, companyDetails.CompanyInfo.ActionByUser.ToString(), "{" + AuditSelectType.Id + ":" + dbCompany.Id + "}${" + AuditSelectType.CompanyCode + ":" + dbCompany.Code.Trim() + "}${" + AuditSelectType.Name + ":" + dbCompany.Name.Trim() + "}", sqlAuditActionType, SqlAuditModuleType.Company, oldData, newData, companyDetails.dbModule);
            if (companyDetails.CompanyInfo.RecordStatus.IsRecordStatusNew())
            {
                this.ProcessAuditSave(companyDetails, dbCompany, sqlAuditActionType, sqlAuditModuleType, oldData, newData, ref eventId, exsistingCompanies,
                                      ref msgToBeInsert, ref msgToBeUpdate, ref msgToBeDelete,
                                      ref msgToInsert, ref msgToUpdate, ref msgToDelete, dbDocuments);
            }
            else
            {
                this.AuditUpdate(companyDetails, dbCompany, sqlAuditActionType, sqlAuditModuleType, oldData, newData, ref eventId, exsistingCompanies,
                                 ref msgToBeInsert, ref msgToBeUpdate, ref msgToBeDelete,
                                  ref msgToInsert, ref msgToUpdate, ref msgToDelete, dbDocuments);
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception);
        }

        private Response ProcessAuditSave(Domain.Models.Companies.CompanyDetail companyDetails,
                           DbModel.Company dbCompanies,
                            SqlAuditActionType sqlAuditActionType,
                            SqlAuditModuleType sqlAuditModuleType,
                            object oldData,
                            object newData,
                            ref long? eventId,
                            Domain.Models.Companies.CompanyDetail exsistingCompanies, ref IList<DbModel.CompanyMessage> msgToBeInsert,
                           ref IList<DbModel.CompanyMessage> msgToBeUpdate, ref IList<DbModel.CompanyMessage> msgToBeDelete
                           , ref List<DbModel.CompanyMessage> msgToInsert,
                           ref List<DbModel.CompanyMessage> msgToUpdate, ref List<DbModel.CompanyMessage> msgToDelete,
                           List<DbModel.Document> dbDocuments = null)
        {

            Exception exception = null;
            Response result = null;
            if (companyDetails.CompanyInfo.RecordStatus.IsRecordStatusNew() && companyDetails != null)
            {
                if (companyDetails.CompanyInfo != null)
                {
                    var newCompanies = companyDetails.CompanyInfo.RecordStatus.IsRecordStatusNew();
                    if (newCompanies)
                    {
                        newData = dbCompanies;
                        result = _auditSearchService.AuditLog(companyDetails, ref eventId, companyDetails?.CompanyInfo?.ActionByUser?.ToString(), null, sqlAuditActionType, SqlAuditModuleType.Company, oldData, newData, companyDetails?.dbModule);
                    }
                }
                if (companyDetails?.CompanyDivisions.Count > 0)
                {
                    var newDivisions = companyDetails.CompanyDivisions.Where(x => x.RecordStatus.IsRecordStatusNew()).ToList();

                    if (newDivisions.Count > 0)
                    {
                        var newaddDiv = _mapper.Map<List<Domain.Models.Companies.CompanyDivision>>(dbCompanies?.CompanyDivision);
                        newData = newaddDiv?.Where(x => !exsistingCompanies.CompanyDivisions.Any(x1 => x1.CompanyDivisionId == x.CompanyDivisionId)).ToList();
                        result = _auditSearchService.AuditLog(companyDetails, ref eventId, companyDetails?.CompanyInfo?.ActionByUser?.ToString(), null, sqlAuditActionType, SqlAuditModuleType.CompanyDivision, oldData, newData, companyDetails?.dbModule);
                    }
                }
                if (companyDetails?.CompanyDivisionCostCenters.Count > 0)
                {
                    var newDivisionCostCenters = companyDetails.CompanyDivisionCostCenters.Where(x => x.RecordStatus.IsRecordStatusNew()).ToList();


                    if (newDivisionCostCenters.Count > 0)
                    {
                        var newaddDivCost = _mapper.Map<List<Domain.Models.Companies.CompanyDivisionCostCenter>>(dbCompanies?.CompanyDivision.SelectMany(x => x.CompanyDivisionCostCenter));
                        newData = newaddDivCost?.Where(x => !exsistingCompanies.CompanyDivisionCostCenters.Any(x1 => x1.CompanyDivisionCostCenterId == x.CompanyDivisionCostCenterId)).ToList();
                        result = _auditSearchService.AuditLog(companyDetails, ref eventId, companyDetails?.CompanyInfo?.ActionByUser?.ToString(), null, sqlAuditActionType, SqlAuditModuleType.CompanyDivisionCostCenter, oldData, newData, companyDetails?.dbModule);
                    }
                }

                if (companyDetails?.CompanyExpectedMargins.Count > 0)
                {
                    var newCompanyExpectedMargins = companyDetails.CompanyExpectedMargins.Where(x => x.RecordStatus.IsRecordStatusNew()).ToList();
                    if (newCompanyExpectedMargins.Count > 0)
                    {
                        var newExpected = _mapper.Map<List<Domain.Models.Companies.CompanyExpectedMargin>>(dbCompanies?.CompanyExpectedMargin);
                        newData = newExpected?.Where(x => !exsistingCompanies.CompanyExpectedMargins.Any(x1 => x1.CompanyExpectedMarginId == x.CompanyExpectedMarginId)).ToList();
                        result = _auditSearchService.AuditLog(companyDetails, ref eventId, companyDetails?.CompanyInfo?.ActionByUser?.ToString(), null, sqlAuditActionType, SqlAuditModuleType.CompanyExpectedMargin, oldData, newData, companyDetails?.dbModule);
                    }
                }
                if (companyDetails?.CompanyNotes.Count > 0)
                {
                    var newCompanyNotes = companyDetails.CompanyNotes.Where(x => x.RecordStatus.IsRecordStatusNew()).ToList();
                    if (newCompanyNotes.Count > 0)
                    {
                        var newCompanyaNotes = _mapper.Map<List<Domain.Models.Companies.CompanyNote>>(dbCompanies?.CompanyNote);
                        newData = newCompanyaNotes?.Where(x => !exsistingCompanies.CompanyNotes.Any(x1 => x1.CompanyNoteId == x.CompanyNoteId)).ToList();
                        result = _auditSearchService.AuditLog(companyDetails, ref eventId, companyDetails?.CompanyInfo?.ActionByUser.ToString(), null, sqlAuditActionType, SqlAuditModuleType.CompanyNote, oldData, newData, companyDetails?.dbModule);
                    }
                }

                if (companyDetails?.CompanyOffices.Count > 0)
                {
                    var newCompanyOffices = companyDetails?.CompanyOffices.Where(x => x.RecordStatus.IsRecordStatusNew()).ToList();
                    if (newCompanyOffices.Count > 0)
                    {
                        var companyOffices = _mapper.Map<List<Domain.Models.Companies.CompanyAddress>>(dbCompanies?.CompanyOffice);
                        newData = companyOffices?.Where(x => !exsistingCompanies.CompanyOffices.Any(x1 => x1.AddressId == x.AddressId)).ToList();
                        result = _auditSearchService.AuditLog(companyDetails, ref eventId, companyDetails?.CompanyInfo?.ActionByUser?.ToString(), null, sqlAuditActionType, SqlAuditModuleType.CompanyOffice, oldData, newData, companyDetails?.dbModule);
                    }
                }
                if (companyDetails?.CompanyPayrolls?.Count > 0)
                {
                    var newCompanyPayrolls = companyDetails?.CompanyPayrolls?.Where(x => x.RecordStatus.IsRecordStatusNew()).ToList();
                    if (newCompanyPayrolls.Count > 0)
                    {
                        var CompanyPayrolls = _mapper.Map<List<Domain.Models.Companies.CompanyPayroll>>(dbCompanies?.CompanyPayroll);
                        newData = CompanyPayrolls?.Where(x => !exsistingCompanies.CompanyPayrolls.Any(x1 => x1.CompanyPayrollId == x.CompanyPayrollId)).ToList();
                        result = _auditSearchService.AuditLog(companyDetails, ref eventId, companyDetails?.CompanyInfo?.ActionByUser.ToString(), null, sqlAuditActionType, SqlAuditModuleType.CompanyPayroll, oldData, newData, companyDetails?.dbModule);
                    }
                }

                if (companyDetails?.CompanyPayrollPeriods.Count > 0)
                {
                    var newCompanyPayrollPeroids = companyDetails?.CompanyPayrollPeriods?.Where(x => x.RecordStatus.IsRecordStatusNew()).ToList();
                    if (newCompanyPayrollPeroids.Count > 0)
                    {
                        var CompanyPayrollPeroids = _mapper.Map<List<Domain.Models.Companies.CompanyPayrollPeriod>>(dbCompanies?.CompanyPayroll?.SelectMany(x => x.CompanyPayrollPeriod));
                        newData = CompanyPayrollPeroids?.Where(x => !exsistingCompanies.CompanyPayrollPeriods.Any(x1 => x1.CompanyPayrollPeriodId == x.CompanyPayrollPeriodId))?.ToList();
                        result = _auditSearchService.AuditLog(companyDetails, ref eventId, companyDetails?.CompanyInfo.ActionByUser.ToString(), null, sqlAuditActionType, SqlAuditModuleType.CompanyPayrollPeriod, oldData, newData, companyDetails?.dbModule);
                    }
                }
                //if (companyDetails?.CompanyQualifications.Count > 0)
                //{
                //    var newCompanyQualifications = companyDetails.CompanyQualifications.Where(x => x.RecordStatus.IsRecordStatusNew()).ToList();
                //    if (newCompanyQualifications.Count > 0)
                //    {
                //        var CompanyQualifications = _mapper.Map<List<Domain.Models.Companies.CompanyQualification>>(dbCompanies?.CompanyQualificationType);
                //        newData = CompanyQualifications?.Where(x => !exsistingCompanies.CompanyQualifications.Any(x1 => x1.CompanyQualificationId == x.CompanyQualificationId))?.ToList();
                //        result = _auditSearchService.AuditLog(companyDetails, ref eventId, companyDetails.CompanyInfo?.ActionByUser?.ToString(), null, sqlAuditActionType, SqlAuditModuleType.CompanyQualification, oldData, newData, companyDetails?.dbModule);
                //    }
                //}
                if (companyDetails?.CompanyTaxes.Count > 0)
                {
                    var newCompanyTaxes = companyDetails.CompanyTaxes?.Where(x => x.RecordStatus.IsRecordStatusNew())?.ToList();
                    if (newCompanyTaxes.Count > 0)
                    {
                        var CompanyTaxes = _mapper.Map<List<Domain.Models.Companies.CompanyTax>>(dbCompanies?.CompanyTax);
                        newData = CompanyTaxes?.Where(x => !exsistingCompanies.CompanyTaxes.Any(x1 => x1.CompanyTaxId == x.CompanyTaxId))?.ToList();
                        result = _auditSearchService.AuditLog(companyDetails, ref eventId, companyDetails.CompanyInfo?.ActionByUser?.ToString(), null, sqlAuditActionType, SqlAuditModuleType.CompanyTax, oldData, newData, companyDetails?.dbModule);
                    }
                }

                if (companyDetails?.CompanyEmailTemplates != null)
                {
                    if (msgToBeInsert?.Count > 0)
                    {
                        newData = companyDetails.CompanyEmailTemplates;
                        result = _auditSearchService.AuditLog(companyDetails, ref eventId, companyDetails.CompanyInfo?.ActionByUser?.ToString(), null, sqlAuditActionType, SqlAuditModuleType.CompanyEmailTemplate, oldData, newData, companyDetails?.dbModule);
                    }
                }

                if (companyDetails?.CompanyInvoiceInfo != null)
                {
                    if (msgToInsert?.Count > 0)
                    {
                        newData = companyDetails.CompanyInvoiceInfo;
                        result = _auditSearchService.AuditLog(companyDetails, ref eventId, companyDetails.CompanyInfo?.ActionByUser?.ToString(), null, sqlAuditActionType, SqlAuditModuleType.CompanyInvoice, oldData, newData, companyDetails?.dbModule);
                    }
                }

                if (companyDetails?.CompanyDocuments != null)
                {
                    if (msgToInsert?.Count > 0)
                    {
                        newData = companyDetails.CompanyDocuments.Select(x =>
                        {
                              x.CreatedOn = dbDocuments?.FirstOrDefault(y => y.DocumentUniqueName == x.DocumentUniqueName)?.CreatedDate;
                            return x;
                        }).ToList(); // audit create date issue fix
                        result = _auditSearchService.AuditLog(companyDetails, ref eventId, companyDetails.CompanyInfo?.ActionByUser?.ToString(), null, sqlAuditActionType, SqlAuditModuleType.CompanyDocument, oldData, newData, companyDetails?.dbModule);
                    }
                }
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception);
        }

        private Response AuditUpdate(Domain.Models.Companies.CompanyDetail companyDetails,
                               DbModel.Company dbCompanies,
                               SqlAuditActionType sqlAuditActionType,
                               SqlAuditModuleType sqlAuditModuleType,
                               object oldData,
                               object newData,
                               ref long? eventId,
                              Domain.Models.Companies.CompanyDetail exsistingCompanies, ref IList<DbModel.CompanyMessage> msgToBeInsert,
                             ref IList<DbModel.CompanyMessage> msgToBeUpdate, ref IList<DbModel.CompanyMessage> msgToBeDelete,
                             ref List<DbModel.CompanyMessage> msgToInsert,
                             ref List<DbModel.CompanyMessage> msgToUpdate, ref List<DbModel.CompanyMessage> msgToDelete,
                             List<DbModel.Document> dbDocuments = null)
        {
            Exception exception = null;
            Response result = null;
            if (companyDetails != null)
            {
                if (companyDetails.CompanyInfo != null)
                {
                    var modifiedCompanyInfo = companyDetails.CompanyInfo.RecordStatus.IsRecordStatusModified();
                    if (modifiedCompanyInfo)
                    {
                        newData = companyDetails.CompanyInfo;
                        oldData = exsistingCompanies?.CompanyInfo;
                        result = _auditSearchService.AuditLog(companyDetails, ref eventId, companyDetails.CompanyInfo?.ActionByUser?.ToString(), null, sqlAuditActionType, SqlAuditModuleType.Company, oldData, newData, companyDetails?.dbModule);
                    }
                }

                if (companyDetails?.CompanyDivisions.Count > 0)
                {
                    var newDivisions = companyDetails.CompanyDivisions.Where(x => x.RecordStatus.IsRecordStatusNew()).ToList();
                    var modifiedDivisions = companyDetails.CompanyDivisions.Where(x => x.RecordStatus.IsRecordStatusModified()).ToList();
                    var deletedDivisions = companyDetails.CompanyDivisions.Where(x => x.RecordStatus.IsRecordStatusDeleted()).ToList();

                    if (newDivisions.Count > 0)
                    {
                        var newaddDiv = _mapper.Map<List<Domain.Models.Companies.CompanyDivision>>(dbCompanies?.CompanyDivision);
                        newData = newaddDiv?.Where(x => !exsistingCompanies.CompanyDivisions.Any(x1 => x1.CompanyDivisionId == x.CompanyDivisionId)).ToList();
                        result = _auditSearchService.AuditLog(companyDetails, ref eventId, companyDetails.CompanyInfo?.ActionByUser?.ToString(), null, sqlAuditActionType, SqlAuditModuleType.CompanyDivision, null, newData, companyDetails?.dbModule);
                    }
                    if (modifiedDivisions.Count > 0)
                    {
                        newData = modifiedDivisions;
                        var Ids = modifiedDivisions.Select(x => x.CompanyDivisionId);
                        oldData = exsistingCompanies.CompanyDivisions.Where(x => Ids.Contains(x.CompanyDivisionId)).ToList();
                        result = _auditSearchService.AuditLog(companyDetails, ref eventId, companyDetails.CompanyInfo?.ActionByUser?.ToString(), null, sqlAuditActionType, SqlAuditModuleType.CompanyDivision, oldData, newData, companyDetails?.dbModule);
                    }
                    if (deletedDivisions.Count > 0)
                    {
                        oldData = deletedDivisions;
                        result = _auditSearchService.AuditLog(companyDetails, ref eventId, companyDetails.CompanyInfo?.ActionByUser?.ToString(), null, sqlAuditActionType, SqlAuditModuleType.CompanyDivision, oldData, null, companyDetails?.dbModule);
                    }
                }

                if (companyDetails?.CompanyDivisionCostCenters.Count > 0)
                {
                    var newDivisionCostCenters = companyDetails.CompanyDivisionCostCenters.Where(x => x.RecordStatus.IsRecordStatusNew()).ToList();
                    var modifiedDivisionCostCenters = companyDetails.CompanyDivisionCostCenters.Where(x => x.RecordStatus.IsRecordStatusModified()).ToList();
                    var deletedDivisionCostCenters = companyDetails.CompanyDivisionCostCenters.Where(x => x.RecordStatus.IsRecordStatusDeleted()).ToList();

                    if (newDivisionCostCenters.Count > 0)
                    {
                        var newaddDivCost = _mapper.Map<List<Domain.Models.Companies.CompanyDivisionCostCenter>>(dbCompanies?.CompanyDivision.SelectMany(x => x.CompanyDivisionCostCenter));
                        newData = newaddDivCost?.Where(x => !exsistingCompanies.CompanyDivisionCostCenters.Any(x1 => x1.CompanyDivisionCostCenterId == x.CompanyDivisionCostCenterId)).ToList();
                        result = _auditSearchService.AuditLog(companyDetails, ref eventId, companyDetails.CompanyInfo?.ActionByUser?.ToString(), null, sqlAuditActionType, SqlAuditModuleType.CompanyDivisionCostCenter, null, newData, companyDetails?.dbModule);
                    }
                    if (modifiedDivisionCostCenters.Count > 0)
                    {
                        newData = modifiedDivisionCostCenters;
                        var Ids = modifiedDivisionCostCenters.Select(x => x.CompanyDivisionCostCenterId);
                        oldData = exsistingCompanies.CompanyDivisionCostCenters.Where(x => Ids.Contains(x.CompanyDivisionCostCenterId)).ToList();
                        result = _auditSearchService.AuditLog(companyDetails, ref eventId, companyDetails.CompanyInfo?.ActionByUser?.ToString(), null, sqlAuditActionType, SqlAuditModuleType.CompanyDivisionCostCenter, oldData, newData, companyDetails?.dbModule);
                    }
                    if (deletedDivisionCostCenters.Count > 0)
                    {
                        oldData = deletedDivisionCostCenters;
                        result = _auditSearchService.AuditLog(companyDetails, ref eventId, companyDetails.CompanyInfo?.ActionByUser?.ToString(), null, sqlAuditActionType, SqlAuditModuleType.CompanyDivisionCostCenter, oldData, null, companyDetails?.dbModule);
                    }
                }

                if (companyDetails?.CompanyExpectedMargins.Count > 0)
                {
                    var newCompanyExpectedMargins = companyDetails.CompanyExpectedMargins.Where(x => x.RecordStatus.IsRecordStatusNew()).ToList();
                    var modifiedCompanyExpectedMargins = companyDetails.CompanyExpectedMargins.Where(x => x.RecordStatus.IsRecordStatusModified()).ToList();
                    var deletedCompanyExpectedMargins = companyDetails.CompanyExpectedMargins.Where(x => x.RecordStatus.IsRecordStatusDeleted()).ToList();

                    if (newCompanyExpectedMargins.Count > 0)
                    {
                        var newExpected = _mapper.Map<List<Domain.Models.Companies.CompanyExpectedMargin>>(dbCompanies?.CompanyExpectedMargin);
                        newData = newExpected?.Where(x => !exsistingCompanies.CompanyExpectedMargins.Any(x1 => x1.CompanyExpectedMarginId == x.CompanyExpectedMarginId)).ToList();
                        result = _auditSearchService.AuditLog(companyDetails, ref eventId, companyDetails.CompanyInfo?.ActionByUser?.ToString(), null, sqlAuditActionType, SqlAuditModuleType.CompanyExpectedMargin, null, newData, companyDetails?.dbModule);
                    }
                    if (modifiedCompanyExpectedMargins.Count > 0)
                    {
                        newData = modifiedCompanyExpectedMargins;
                        var Ids = modifiedCompanyExpectedMargins.Select(x => x.CompanyExpectedMarginId);
                        oldData = exsistingCompanies.CompanyExpectedMargins.Where(x => Ids.Contains(x.CompanyExpectedMarginId)).ToList();
                        result = _auditSearchService.AuditLog(companyDetails, ref eventId, companyDetails.CompanyInfo?.ActionByUser?.ToString(), null, sqlAuditActionType, SqlAuditModuleType.CompanyExpectedMargin, oldData, newData, companyDetails?.dbModule);
                    }
                    if (deletedCompanyExpectedMargins.Count > 0)
                    {
                        oldData = deletedCompanyExpectedMargins;
                        result = _auditSearchService.AuditLog(companyDetails, ref eventId, companyDetails.CompanyInfo?.ActionByUser?.ToString(), null, sqlAuditActionType, SqlAuditModuleType.CompanyExpectedMargin, oldData, null, companyDetails?.dbModule);
                    }
                }

                if (companyDetails?.CompanyNotes.Count > 0)
                {
                    var newCompanyNotes = companyDetails.CompanyNotes?.Where(x => x.RecordStatus.IsRecordStatusNew()).ToList();
                    var modifiedCompanyNotes = companyDetails.CompanyNotes?.Where(x => x.RecordStatus.IsRecordStatusModified()).ToList();
                    var deletedCompanyNotes = companyDetails.CompanyNotes?.Where(x => x.RecordStatus.IsRecordStatusDeleted()).ToList();

                    if (newCompanyNotes.Count > 0)
                    {
                        var newCompanyaNotes = _mapper.Map<List<Domain.Models.Companies.CompanyNote>>(dbCompanies?.CompanyNote);
                        newData = newCompanyaNotes?.Where(x => !exsistingCompanies.CompanyNotes.Any(x1 => x1.CompanyNoteId == x.CompanyNoteId)).ToList();
                        result = _auditSearchService.AuditLog(companyDetails, ref eventId, companyDetails.CompanyInfo?.ActionByUser?.ToString(), null, sqlAuditActionType, SqlAuditModuleType.CompanyNote, null, newData, companyDetails?.dbModule);
                    }
                    if (modifiedCompanyNotes.Count > 0)
                    {
                        newData = modifiedCompanyNotes;
                        var Ids = modifiedCompanyNotes.Select(x => x.CompanyNoteId);
                        oldData = exsistingCompanies.CompanyNotes.Where(x => Ids.Contains(x.CompanyNoteId)).ToList();
                        result = _auditSearchService.AuditLog(companyDetails, ref eventId, companyDetails.CompanyInfo?.ActionByUser?.ToString(), null, sqlAuditActionType, SqlAuditModuleType.CompanyNote, oldData, newData, companyDetails?.dbModule);
                    }
                    if (deletedCompanyNotes.Count > 0)
                    {
                        oldData = deletedCompanyNotes;
                        result = _auditSearchService.AuditLog(companyDetails, ref eventId, companyDetails.CompanyInfo?.ActionByUser?.ToString(), null, sqlAuditActionType, SqlAuditModuleType.CompanyNote, oldData, null, companyDetails?.dbModule);
                    }
                }

                if (companyDetails?.CompanyOffices.Count > 0)
                {
                    var newCompanyOffices = companyDetails.CompanyOffices.Where(x => x.RecordStatus.IsRecordStatusNew()).ToList();
                    var modifiedCompanyOffices = companyDetails.CompanyOffices.Where(x => x.RecordStatus.IsRecordStatusModified()).ToList();
                    var deletedCompanyOffices = companyDetails.CompanyOffices.Where(x => x.RecordStatus.IsRecordStatusDeleted()).ToList();

                    if (newCompanyOffices.Count > 0)
                    {
                        var companyOffices = _mapper.Map<List<Domain.Models.Companies.CompanyAddress>>(dbCompanies?.CompanyOffice);
                        newData = companyOffices?.Where(x => !exsistingCompanies.CompanyOffices.Any(x1 => x1.AddressId == x.AddressId)).ToList();
                        result = _auditSearchService.AuditLog(companyDetails, ref eventId, companyDetails.CompanyInfo?.ActionByUser?.ToString(), null, sqlAuditActionType, SqlAuditModuleType.CompanyOffice, null, newData, companyDetails?.dbModule);
                    }
                    if (modifiedCompanyOffices.Count > 0)
                    {
                        newData = modifiedCompanyOffices;
                        var Ids = modifiedCompanyOffices.Select(x => x.AddressId);
                        oldData = exsistingCompanies.CompanyOffices.Where(x => Ids.Contains(x.AddressId)).ToList();
                        result = _auditSearchService.AuditLog(companyDetails, ref eventId, companyDetails.CompanyInfo?.ActionByUser?.ToString(), null, sqlAuditActionType, SqlAuditModuleType.CompanyOffice, oldData, newData, companyDetails?.dbModule);
                    }
                    if (deletedCompanyOffices.Count > 0)
                    {
                        oldData = deletedCompanyOffices;
                        result = _auditSearchService.AuditLog(companyDetails, ref eventId, companyDetails.CompanyInfo.ActionByUser?.ToString(), null, sqlAuditActionType, SqlAuditModuleType.CompanyOffice, oldData, null, companyDetails?.dbModule);
                    }
                }

                if (companyDetails?.CompanyPayrolls?.Count > 0)
                {
                    var newCompanyPayrolls = companyDetails?.CompanyPayrolls?.Where(x => x.RecordStatus.IsRecordStatusNew()).ToList();
                    var modifiedCompanyPayrolls = companyDetails?.CompanyPayrolls?.Where(x => x.RecordStatus.IsRecordStatusModified()).ToList();
                    var deletedCompanyPayrolls = companyDetails?.CompanyPayrolls?.Where(x => x.RecordStatus.IsRecordStatusDeleted()).ToList();

                    if (newCompanyPayrolls.Count > 0)
                    {
                        var CompanyPayrolls = _mapper.Map<List<Domain.Models.Companies.CompanyPayroll>>(dbCompanies?.CompanyPayroll);
                        newData = CompanyPayrolls?.Where(x => !exsistingCompanies.CompanyPayrolls.Any(x1 => x1.CompanyPayrollId == x.CompanyPayrollId)).ToList();
                        result = _auditSearchService.AuditLog(companyDetails, ref eventId, companyDetails.CompanyInfo?.ActionByUser?.ToString(), null, sqlAuditActionType, SqlAuditModuleType.CompanyPayroll, null, newData, companyDetails?.dbModule);
                    }
                    if (modifiedCompanyPayrolls.Count > 0)
                    {
                        newData = modifiedCompanyPayrolls;
                        var Ids = modifiedCompanyPayrolls.Select(x => x.CompanyPayrollId);
                        oldData = exsistingCompanies.CompanyPayrolls.Where(x => Ids.Contains(x.CompanyPayrollId)).ToList();
                        result = _auditSearchService.AuditLog(companyDetails, ref eventId, companyDetails.CompanyInfo?.ActionByUser?.ToString(), null, sqlAuditActionType, SqlAuditModuleType.CompanyPayroll, oldData, newData, companyDetails?.dbModule);
                    }
                    if (deletedCompanyPayrolls.Count > 0)
                    {
                        oldData = deletedCompanyPayrolls;
                        result = _auditSearchService.AuditLog(companyDetails, ref eventId, companyDetails.CompanyInfo?.ActionByUser?.ToString(), null, sqlAuditActionType, SqlAuditModuleType.CompanyPayroll, oldData, null, companyDetails?.dbModule);
                    }
                }

                if (companyDetails?.CompanyPayrollPeriods.Count > 0)
                {
                    var newCompanyPayrollPeroids = companyDetails.CompanyPayrollPeriods?.Where(x => x.RecordStatus.IsRecordStatusNew()).ToList();
                    var modifiedCompanyPayrollPeriods = companyDetails.CompanyPayrollPeriods?.Where(x => x.RecordStatus.IsRecordStatusModified()).ToList();
                    var deletedCompanyPayrollPeriods = companyDetails.CompanyPayrollPeriods?.Where(x => x.RecordStatus.IsRecordStatusDeleted()).ToList();

                    if (newCompanyPayrollPeroids.Count > 0)
                    {
                        var CompanyPayrollPeroids = _mapper.Map<List<Domain.Models.Companies.CompanyPayrollPeriod>>(dbCompanies?.CompanyPayroll?.SelectMany(x => x.CompanyPayrollPeriod));
                        newData = CompanyPayrollPeroids?.Where(x => !exsistingCompanies.CompanyPayrollPeriods.Any(x1 => x1.CompanyPayrollPeriodId == x.CompanyPayrollPeriodId))?.ToList();
                        result = _auditSearchService.AuditLog(companyDetails, ref eventId, companyDetails.CompanyInfo?.ActionByUser?.ToString(), null, sqlAuditActionType, SqlAuditModuleType.CompanyPayrollPeriod, null, newData, companyDetails?.dbModule);
                    }
                    if (modifiedCompanyPayrollPeriods.Count > 0)
                    {
                        newData = modifiedCompanyPayrollPeriods;
                        var Ids = modifiedCompanyPayrollPeriods?.Select(x => x.CompanyPayrollPeriodId);
                        oldData = exsistingCompanies?.CompanyPayrollPeriods?.Where(x => Ids.Contains(x.CompanyPayrollPeriodId))?.ToList();
                        result = _auditSearchService.AuditLog(companyDetails, ref eventId, companyDetails.CompanyInfo?.ActionByUser?.ToString(), null, sqlAuditActionType, SqlAuditModuleType.CompanyPayrollPeriod, oldData, newData, companyDetails?.dbModule);
                    }
                    if (deletedCompanyPayrollPeriods.Count > 0)
                    {
                        oldData = deletedCompanyPayrollPeriods;
                        result = _auditSearchService.AuditLog(companyDetails, ref eventId, companyDetails.CompanyInfo?.ActionByUser?.ToString(), null, sqlAuditActionType, SqlAuditModuleType.CompanyPayrollPeriod, oldData, null, companyDetails?.dbModule);
                    }
                }

                //if (companyDetails?.CompanyQualifications.Count > 0)
                //{
                //    var newCompanyQualifications = companyDetails.CompanyQualifications.Where(x => x.RecordStatus.IsRecordStatusNew()).ToList();
                //    var modifiedCompanyQualifications = companyDetails.CompanyQualifications.Where(x => x.RecordStatus.IsRecordStatusModified()).ToList();
                //    var deletedCompanyQualifications = companyDetails.CompanyQualifications.Where(x => x.RecordStatus.IsRecordStatusDeleted()).ToList();

                //    if (newCompanyQualifications.Count > 0)
                //    {
                //        var CompanyQualifications = _mapper.Map<List<Domain.Models.Companies.CompanyQualification>>(dbCompanies?.CompanyQualificationType);
                //        newData = CompanyQualifications?.Where(x => !exsistingCompanies.CompanyQualifications.Any(x1 => x1.CompanyQualificationId == x.CompanyQualificationId))?.ToList();
                //        result = _auditSearchService.AuditLog(companyDetails, ref eventId, companyDetails.CompanyInfo?.ActionByUser?.ToString(), null, sqlAuditActionType, SqlAuditModuleType.CompanyQualification, null, newData, companyDetails?.dbModule);
                //    }
                //    if (modifiedCompanyQualifications.Count > 0)
                //    {
                //        newData = modifiedCompanyQualifications;
                //        var Ids = modifiedCompanyQualifications.Select(x => x.CompanyQualificationId);
                //        oldData = exsistingCompanies.CompanyQualifications.Where(x => Ids.Contains(x.CompanyQualificationId)).ToList();
                //        result = _auditSearchService.AuditLog(companyDetails, ref eventId, companyDetails.CompanyInfo?.ActionByUser?.ToString(), null, sqlAuditActionType, SqlAuditModuleType.CompanyQualification, oldData, newData, companyDetails?.dbModule);
                //    }
                //    if (deletedCompanyQualifications.Count > 0)
                //    {
                //        oldData = deletedCompanyQualifications;
                //        result = _auditSearchService.AuditLog(companyDetails, ref eventId, companyDetails.CompanyInfo?.ActionByUser?.ToString(), null, sqlAuditActionType, SqlAuditModuleType.CompanyQualification, oldData, null, companyDetails?.dbModule);
                //    }
                //}

                if (companyDetails?.CompanyTaxes.Count > 0)
                {
                    var newCompanyTaxes = companyDetails.CompanyTaxes?.Where(x => x.RecordStatus.IsRecordStatusNew())?.ToList();
                    var modifiedCompanyTaxes = companyDetails.CompanyTaxes?.Where(x => x.RecordStatus.IsRecordStatusModified())?.ToList();
                    var deletedCompanyTaxes = companyDetails.CompanyTaxes?.Where(x => x.RecordStatus.IsRecordStatusDeleted())?.ToList();

                    if (newCompanyTaxes.Count > 0)
                    {
                        var CompanyTaxes = _mapper.Map<List<Domain.Models.Companies.CompanyTax>>(dbCompanies?.CompanyTax);
                        newData = CompanyTaxes?.Where(x => !exsistingCompanies.CompanyTaxes.Any(x1 => x1.CompanyTaxId == x.CompanyTaxId))?.ToList();
                        result = _auditSearchService.AuditLog(companyDetails, ref eventId, companyDetails.CompanyInfo?.ActionByUser?.ToString(), null, sqlAuditActionType, SqlAuditModuleType.CompanyTax, null, newData, companyDetails?.dbModule);
                    }
                    if (modifiedCompanyTaxes.Count > 0)
                    {
                        newData = modifiedCompanyTaxes;
                        var Ids = modifiedCompanyTaxes.Select(x => x.CompanyTaxId);
                        oldData = exsistingCompanies.CompanyTaxes.Where(x => Ids.Contains(x.CompanyTaxId)).ToList();
                        result = _auditSearchService.AuditLog(companyDetails, ref eventId, companyDetails.CompanyInfo?.ActionByUser?.ToString(), null, sqlAuditActionType, SqlAuditModuleType.CompanyTax, oldData, newData, companyDetails?.dbModule);
                    }
                    if (deletedCompanyTaxes.Count > 0)
                    {
                        oldData = deletedCompanyTaxes;
                        result = _auditSearchService.AuditLog(companyDetails, ref eventId, companyDetails.CompanyInfo?.ActionByUser?.ToString(), null, sqlAuditActionType, SqlAuditModuleType.CompanyTax, oldData, null, companyDetails?.dbModule);
                    }
                }

                if (companyDetails?.CompanyEmailTemplates != null)
                {
                    if (msgToBeInsert?.Count > 0)
                    {

                        newData = companyDetails.CompanyEmailTemplates;
                        result = _auditSearchService.AuditLog(companyDetails, ref eventId, companyDetails.CompanyInfo?.ActionByUser?.ToString(), null, sqlAuditActionType, SqlAuditModuleType.CompanyEmailTemplate, null, newData, companyDetails?.dbModule);
                    }
                    if (msgToBeUpdate?.Count > 0)
                    {
                    //    newData = _mapper.Map<Domain.Models.Companies.CompanyEmailTemplate>(msgToBeUpdate);
                        newData = companyDetails.CompanyEmailTemplates; //Changes for IGO D905
                        newData.SetPropertyValue("CompanyCode", null); 
                        oldData = exsistingCompanies?.CompanyEmailTemplates;
                        result = _auditSearchService.AuditLog(companyDetails, ref eventId, companyDetails.CompanyInfo?.ActionByUser?.ToString(), null, sqlAuditActionType, SqlAuditModuleType.CompanyEmailTemplate, oldData, newData, companyDetails?.dbModule);
                    }
                    if (msgToBeDelete?.Count > 0)
                    {
                        oldData = _mapper.Map<Domain.Models.Companies.CompanyEmailTemplate>(msgToBeDelete);
                        result = _auditSearchService.AuditLog(companyDetails, ref eventId, companyDetails.CompanyInfo?.ActionByUser?.ToString(), null, sqlAuditActionType, SqlAuditModuleType.CompanyEmailTemplate, oldData, null, companyDetails?.dbModule);
                    }
                }

                if (companyDetails?.CompanyInvoiceInfo != null)
                {
                    //if (msgToInsert?.Count > 0) //Changes for IGO D905
                    //{
                    //    newData = _mapper.Map<CompanyPartialInvoice>(companyDetails.CompanyInvoiceInfo);
                    //    result = _auditSearchService.AuditLog(companyDetails, ref eventId, companyDetails.CompanyInfo?.ActionByUser?.ToString(), null, sqlAuditActionType, SqlAuditModuleType.CompanyInvoice, null, newData, companyDetails?.dbModule);
                    //}
                    if (msgToUpdate?.Count > 0 || msgToInsert?.Count > 0)
                    {
                    //    var DomMsgToBeUpdate = _mapper.Map<Domain.Models.Companies.CompanyInvoice>(msgToUpdate.ToList());
                        newData = _mapper.Map<CompanyPartialInvoice>(companyDetails.CompanyInvoiceInfo);
                        newData.SetPropertyValue("CompanyCode", null);
                        oldData = _mapper.Map<CompanyPartialInvoice>(exsistingCompanies.CompanyInvoiceInfo);

                        result = _auditSearchService.AuditLog(companyDetails, ref eventId, companyDetails.CompanyInfo?.ActionByUser?.ToString(), null, sqlAuditActionType, SqlAuditModuleType.CompanyInvoice, oldData, newData, companyDetails?.dbModule);
                    }
                    //if (msgToDelete?.Count > 0) //Changes for IGO D905
                    //{
                    //    var DomMsgDelete = _mapper.Map<Domain.Models.Companies.CompanyInvoice>(msgToDelete.ToList());
                    //    oldData = _mapper.Map<CompanyPartialInvoice>(DomMsgDelete);
                    //    result = _auditSearchService.AuditLog(companyDetails, ref eventId, companyDetails.CompanyInfo?.ActionByUser?.ToString(), null, sqlAuditActionType, SqlAuditModuleType.CompanyInvoice, oldData, null, companyDetails?.dbModule);
                    //}
                }

                if (companyDetails?.CompanyInvoiceInfo?.InvoiceRemittances.Count > 0)
                {
                    var newRemmitanceText = companyDetails?.CompanyInvoiceInfo?.InvoiceRemittances.Where(x => x.RecordStatus.IsRecordStatusNew()).ToList();
                    var modifyRemittanceText = companyDetails?.CompanyInvoiceInfo?.InvoiceRemittances.Where(x => x.RecordStatus.IsRecordStatusModified()).ToList();
                    var deleteRemittanceText = companyDetails?.CompanyInvoiceInfo?.InvoiceRemittances.Where(x => x.RecordStatus.IsRecordStatusDeleted()).ToList();
                    if (newRemmitanceText.Count > 0)
                    {
                        newData = newRemmitanceText;
                        result = _auditSearchService.AuditLog(companyDetails, ref eventId, companyDetails.CompanyInfo?.ActionByUser?.ToString(), null, sqlAuditActionType, SqlAuditModuleType.CompanyInvoice, null, newData, companyDetails?.dbModule);
                    }
                    if (modifyRemittanceText.Count > 0)
                    {

                        newData = modifyRemittanceText;
                        var Ids = modifyRemittanceText.Select(x => x.Id);
                        oldData = exsistingCompanies.CompanyInvoiceInfo.InvoiceRemittances.Where(x => Ids.Contains(x.Id)).ToList();
                        result = _auditSearchService.AuditLog(companyDetails, ref eventId, companyDetails.CompanyInfo?.ActionByUser?.ToString(), null, sqlAuditActionType, SqlAuditModuleType.CompanyInvoice, oldData, newData, companyDetails?.dbModule);
                    }
                    if (deleteRemittanceText.Count > 0)
                    {
                        oldData = deleteRemittanceText;
                        result = _auditSearchService.AuditLog(companyDetails, ref eventId, companyDetails.CompanyInfo?.ActionByUser?.ToString(), null, sqlAuditActionType, SqlAuditModuleType.CompanyInvoice, oldData, null, companyDetails?.dbModule);
                    }
                }

                if (companyDetails?.CompanyInvoiceInfo?.InvoiceFooters.Count > 0)
                {
                    var newFooter = companyDetails?.CompanyInvoiceInfo?.InvoiceFooters.Where(x => x.RecordStatus.IsRecordStatusNew()).ToList();
                    var modifiedFooter = companyDetails?.CompanyInvoiceInfo?.InvoiceFooters.Where(x => x.RecordStatus.IsRecordStatusModified()).ToList();
                    var deletedFooter = companyDetails?.CompanyInvoiceInfo?.InvoiceFooters.Where(x => x.RecordStatus.IsRecordStatusDeleted()).ToList();
                    if (newFooter.Count > 0)
                    {
                        newData = newFooter;
                        result = _auditSearchService.AuditLog(companyDetails, ref eventId, companyDetails.CompanyInfo?.ActionByUser?.ToString(), null, sqlAuditActionType, SqlAuditModuleType.CompanyInvoice, null, newData, companyDetails?.dbModule);
                    }
                    if (modifiedFooter.Count > 0)
                    {
                        newData = modifiedFooter;
                        var Ids = modifiedFooter.Select(x => x.Id);
                        oldData = exsistingCompanies.CompanyInvoiceInfo.InvoiceFooters.Where(x => Ids.Contains(x.Id)).ToList();
                        result = _auditSearchService.AuditLog(companyDetails, ref eventId, companyDetails.CompanyInfo?.ActionByUser?.ToString(), null, sqlAuditActionType, SqlAuditModuleType.CompanyInvoice, oldData, newData, companyDetails?.dbModule);
                    }
                    if (deletedFooter.Count > 0)
                    {
                        oldData = deletedFooter;
                        result = _auditSearchService.AuditLog(companyDetails, ref eventId, companyDetails.CompanyInfo?.ActionByUser?.ToString(), null, sqlAuditActionType, SqlAuditModuleType.CompanyInvoice, oldData, null, companyDetails?.dbModule);
                    }
                }

                //For Document Audit
                if (companyDetails?.CompanyDocuments.Count > 0)
                {
                    var newDocument = companyDetails?.CompanyDocuments?.Where(x => x.RecordStatus.IsRecordStatusNew())?.ToList();
                    var modifiedDocument = companyDetails?.CompanyDocuments?.Where(x => x.RecordStatus.IsRecordStatusModified())?.ToList();
                    var deletedDocument = companyDetails?.CompanyDocuments?.Where(x => x.RecordStatus.IsRecordStatusDeleted())?.ToList();
                    if (newDocument.Count > 0)
                    {
                        newData = newDocument.Select(x =>
                        {
                            x.CreatedOn = dbDocuments?.FirstOrDefault(y => y.DocumentUniqueName == x.DocumentUniqueName)?.CreatedDate;
                            return x;
                        }).ToList(); // audit create date issue fix
                        result = _auditSearchService.AuditLog(companyDetails, ref eventId, companyDetails.CompanyInfo?.ActionByUser?.ToString(), null, sqlAuditActionType, SqlAuditModuleType.CompanyDocument, null, newData, companyDetails?.dbModule);
                    }
                    if (modifiedDocument.Count > 0)
                    {
                        newData = modifiedDocument?.OrderBy(x => x.Id)?.ToList();
                        oldData = (dbDocuments != null && dbDocuments.Count > 0) ? _mapper.Map<List<ModuleDocument>>(dbDocuments)?.OrderBy(x => x.Id)?.ToList() : null;
                        result = _auditSearchService.AuditLog(companyDetails, ref eventId, companyDetails.CompanyInfo?.ActionByUser?.ToString(), null, sqlAuditActionType, SqlAuditModuleType.CompanyDocument, oldData, newData, companyDetails?.dbModule);
                    }
                    if (deletedDocument.Count > 0)
                    {
                        oldData = deletedDocument;
                        result = _auditSearchService.AuditLog(companyDetails, ref eventId, companyDetails.CompanyInfo?.ActionByUser?.ToString(), null, sqlAuditActionType, SqlAuditModuleType.CompanyDocument, oldData, null, companyDetails?.dbModule);
                    }
                }
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception);


        }
    }
}

