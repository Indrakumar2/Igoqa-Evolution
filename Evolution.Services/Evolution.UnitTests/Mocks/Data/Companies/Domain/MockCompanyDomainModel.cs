using Evolution.Common.Models.Documents;
using Evolution.Document.Domain.Models.Document;
using System;
using System.Collections.Generic;
using System.Text;
using DomModel = Evolution.Company.Domain.Models.Companies;

namespace Evolution.UnitTests.Mocks.Data.Companies
{
    public static class MockCompanyDomainModel
    {
        public static IList<DomModel.Company> GetCompanyMockedDomainModelData()
        {
            return new List<DomModel.Company>
            {
                new DomModel.Company{ CompanyCode= "DZ", CompanyName= "Algeria MI", InvoiceName= "Algeria MI", IsActive= true, Currency= "DZD", SalesTaxDescription= null, WithholdingTaxDescription= null, InterCompanyExpenseAccRef= null, InterCompanyRateAccRef= null, InterCompanyRoyaltyAccRef= null, CompanyMiiwaid= 2, OperatingCountry= "Algeria", CompanyMiiwaref= 2, IsUseIctms=false, IsFullUse= true, GfsCoa= "ITKD-404", GfsBu= "E04", Region= "5. Africa", IsCOSEmailOverrideAllow= false,  AvgTSHourlyCost= null, VatTaxRegNo= "000230049009354", EUVatPrefix= "      ", IARegion= "4", CognosNumber= "404", UpdateCount= null, RecordStatus= null, LastModification=DateTime.UtcNow, ModifiedBy= null },
                new DomModel.Company{ CompanyCode= "AR", CompanyName= "Argentina MI", InvoiceName= "Argentina MI", IsActive= true, Currency= "USD", SalesTaxDescription= null, WithholdingTaxDescription= null, InterCompanyExpenseAccRef= null, InterCompanyRateAccRef= null, InterCompanyRoyaltyAccRef= null, CompanyMiiwaid= 3, OperatingCountry= "Argentina", CompanyMiiwaref= 3, IsUseIctms=true, IsFullUse= true, GfsCoa= "ITKD-0406", GfsBu= "C01", Region= "1. Americas", IsCOSEmailOverrideAllow= false,  AvgTSHourlyCost= 15.5M, VatTaxRegNo= "", EUVatPrefix= "      ", IARegion= "6", CognosNumber= "0406", UpdateCount= null, RecordStatus= null, LastModification=DateTime.UtcNow, ModifiedBy= null },
                new DomModel.Company{ CompanyCode= "AU", CompanyName= "Australia - Intertek Technical Services PTY Ltd", InvoiceName= "Australia - Intertek Technical Services PTY Ltd", IsActive= true, Currency= "AUD", SalesTaxDescription= null, WithholdingTaxDescription= null, InterCompanyExpenseAccRef= null, InterCompanyRateAccRef= null, InterCompanyRoyaltyAccRef= null, CompanyMiiwaid= 4, OperatingCountry= "Australia", CompanyMiiwaref= 4, IsUseIctms=false, IsFullUse= true, GfsCoa= "ITKD-408", GfsBu= "D07", Region= "2. Asia Pacific", IsCOSEmailOverrideAllow= false,  AvgTSHourlyCost= 60, VatTaxRegNo= "", EUVatPrefix= "      ", IARegion= "1", CognosNumber= "408", UpdateCount= null, RecordStatus= null, LastModification=DateTime.UtcNow, ModifiedBy= null },
            };
        }

        public static IList<DomModel.CompanyDivisionCostCenter> GetCompanyCostCenterMockedDomainModelData()
        {
            return new List<DomModel.CompanyDivisionCostCenter>
            {
                new DomModel.CompanyDivisionCostCenter{ CompanyDivisionCostCenterId=1, CompanyCode= "UK050",Division="Inspection",CostCenterCode="1",CostCenterName="Burgess Hill",UpdateCount= null, RecordStatus= null, LastModification=DateTime.UtcNow, ModifiedBy=null },
                new DomModel.CompanyDivisionCostCenter{ CompanyDivisionCostCenterId=2, CompanyCode= "UK050",Division="PSO",CostCenterCode="2",CostCenterName="Azerbaijan",UpdateCount= null, RecordStatus= null, LastModification=DateTime.UtcNow, ModifiedBy= null },
                new DomModel.CompanyDivisionCostCenter{ CompanyDivisionCostCenterId=3, CompanyCode= "UK051",Division="PSO",CostCenterCode="CC4",CostCenterName="Azerbaijan",UpdateCount= null, RecordStatus= null, LastModification=DateTime.UtcNow, ModifiedBy= null },
            };
        }

        public static IList<DomModel.CompanyDivision> GetCompanyDivisionMockedDomainModelData()
        {
            return new List<DomModel.CompanyDivision>
            {
                new DomModel.CompanyDivision{ CompanyDivisionId=1, CompanyCode= "UK050", DivisionName="Inspection",DivisionAcReference="1",UpdateCount= null, RecordStatus= null, LastModification=DateTime.UtcNow, ModifiedBy= null },
                new DomModel.CompanyDivision{CompanyDivisionId=2, CompanyCode= "UK051",DivisionName="PSO",DivisionAcReference="1",UpdateCount= null, RecordStatus= null, LastModification=DateTime.UtcNow, ModifiedBy= null },
                new DomModel.CompanyDivision{ CompanyDivisionId=3,CompanyCode= "UK052", DivisionName="Inspection",DivisionAcReference="1",UpdateCount= null, RecordStatus= null, LastModification=DateTime.UtcNow, ModifiedBy= null },
            };
        }

        public static IList<ModuleDocument> GetCompanyDocumentMockedDomainModelData()
        {
            return new List<ModuleDocument>
            {
                new ModuleDocument{ DocumentName="Invoice",DocumentType="Email",IsVisibleToCustomer=true,IsVisibleToTS=true,DocumentSize=1000 ,UpdateCount= null, RecordStatus= null, LastModification=DateTime.UtcNow, ModifiedBy= null },
                new ModuleDocument{DocumentName="Assignment File",DocumentType="Assignment",IsVisibleToCustomer=true,IsVisibleToTS=true,DocumentSize=1000,LastModification=null,RecordStatus= null, UpdateCount=null,ModifiedBy=null },
                new ModuleDocument{DocumentName="Contract Copy",DocumentType="Contract",IsVisibleToCustomer=false,IsVisibleToTS=false,DocumentSize=1000,LastModification=null,RecordStatus= null, UpdateCount=null,ModifiedBy=null },
                new ModuleDocument{DocumentName="Certificate File",DocumentType="Email",IsVisibleToCustomer=true,IsVisibleToTS=true,DocumentSize=1000,LastModification=null,RecordStatus= null, UpdateCount=null,ModifiedBy=null}
            };
        }

        public static IList<DomModel.CompanyEmailTemplate> GetCompanyEmailTemplateMockedDomainModelData()
        {
            return new List<DomModel.CompanyEmailTemplate>
            {
                new DomModel.CompanyEmailTemplate{ CompanyCode= "UK050",CustomerReportingNotificationEmailText="Customer : @CustomerName@\r\nSupplier : @Supplier@\r\nSupplier PO : @SupplierPO@\r\nProject Number : @ProjectNumber@\r\nAssignment Number : @AssignmentNumber@\r\n\r\n\r\nDear Sir / Madam, \r\n\r\nPlease find attached a copy of our report @ReportNumber@ which pertains to our visit on the @VisitDate@@VisitDatePeriod@.\r\n\r\nClick here to hyperlink to the visit on the Customer Extranet @VisitURL@\r\n\r\nShould you have any queries please do not hesitate to contact me. Thank you. \r\n\r\nBest regards\r\n\r\n@CoordinatorName@\r\n@Company@",CustomerDirectReportingEmailText="@ReportNumber@@VisitDatePeriod@@Supplier@@CustomerProjectName@@CustomerName@",RejectVisitTimesheetEmailText="@CustomerName@\r\n@ContractNumber@\r\n@ProjectNumber@\r\n@AssignmentNumber@\r\n@VisitDate@\r\n@ReportNumber@\r\n@CoordinatorName@\r\n\r\nYour visit / timesheet has been rejected for the following reasons:\r\n\r\n@Note@",VisitCompletedCoordinatorEmailText="@CustomerName@\r\n@ContractNumber@\r\n@ProjectNumber@\r\n@AssignmentNumber@\r\n@VisitDate@\r\n@ReportNumber@\r\n@CoordinatorName@",InterCompanyOperatingCoordinatorEmail="@CustomerName@\r\n@ContractNumber@\r\n@ProjectNumber@\r\n@AssignmentNumber@\r\n@VisitDate@\r\n@ReportNumber@\r\n@CoordinatorName@",UpdateCount= null, RecordStatus= null, LastModification=DateTime.UtcNow, ModifiedBy= null },
                new DomModel.CompanyEmailTemplate{ CompanyCode= "UK051",CustomerReportingNotificationEmailText="Dear Sir / Madam, \r\n\r\nPlease find attached a copy of our report @ReportNumber@ which pertains to our visit on the @VisitDate@@VisitDatePeriod@.\r\n\r\nClick here to hyperlink to the visit on the Customer Extranet @VisitURL@\r\n\r\nShould you have any queries please do not hesitate to contact me. Thank you. \r\n\r\nBest regards\r\n\r\n@CoordinatorName",CustomerDirectReportingEmailText="@ReportNumber@@VisitDatePeriod@@Supplier@@CustomerProjectName@@CustomerName@",RejectVisitTimesheetEmailText="@CustomerName@\r\n@ContractNumber@\r\n@ProjectNumber@\r\n@AssignmentNumber@\r\n@VisitDate@\r\n@ReportNumber@\r\n@CoordinatorName@\r\n\r\nYour visit / timesheet has been rejected for the following reasons:\r\n\r\n@Note@",VisitCompletedCoordinatorEmailText="@CustomerName@\r\n@ContractNumber@\r\n@ProjectNumber@\r\n@AssignmentNumber@\r\n@VisitDate@\r\n@ReportNumber@\r\n@CoordinatorName@",InterCompanyOperatingCoordinatorEmail="@CustomerName@\r\n@ContractNumber@\r\n@ProjectNumber@\r\n@AssignmentNumber@\r\n@VisitDate@\r\n@ReportNumber@\r\n@CoordinatorName@",UpdateCount= null, RecordStatus= null, LastModification=DateTime.UtcNow, ModifiedBy= null },
                new DomModel.CompanyEmailTemplate{ CompanyCode= "UK052",CustomerReportingNotificationEmailText="Dear Sir / Madam, \r\n\r\nPlease find attached a copy of our report @ReportNumber@ which pertains to our visit on the @VisitDate@@VisitDatePeriod@.\r\n\r\nClick here to hyperlink to the visit on the Customer Extranet @VisitURL@\r\n\r\nShould you have any queries please do not hesitate to contact me. Thank you. \r\n\r\nBest regards\r\n\r\n@CoordinatorName@\r\n@Company@",CustomerDirectReportingEmailText="@ReportNumber@@VisitDatePeriod@@Supplier@@CustomerProjectName@@CustomerName@",RejectVisitTimesheetEmailText="@CustomerName@\r\n@ContractNumber@\r\n@ProjectNumber@\r\n@AssignmentNumber@\r\n@VisitDate@\r\n@ReportNumber@\r\n@CoordinatorName@\r\n\r\nYour visit / timesheet has been rejected for the following reasons:\r\n\r\n@Note@",VisitCompletedCoordinatorEmailText="@CustomerName@\r\n@ContractNumber@\r\n@ProjectNumber@\r\n@AssignmentNumber@\r\n@VisitDate@\r\n@ReportNumber@\r\n@CoordinatorName@",InterCompanyOperatingCoordinatorEmail="@CustomerName@\r\n@ContractNumber@\r\n@ProjectNumber@\r\n@AssignmentNumber@\r\n@VisitDate@\r\n@ReportNumber@\r\n@CoordinatorName@",UpdateCount= null, RecordStatus= null, LastModification=DateTime.UtcNow, ModifiedBy= null },

            };
        }

        public static IList<DomModel.CompanyExpectedMargin> GetCompanyExpectedMarginMockedDomainModelData()
        {
            return new List<DomModel.CompanyExpectedMargin>
            {
                new DomModel.CompanyExpectedMargin{ CompanyExpectedMarginId=1, CompanyCode= "UK050",MarginType="TIS (Technical Inspection Services)",MinimumMargin=15.000000M,UpdateCount= null, RecordStatus= null, LastModification=DateTime.UtcNow, ModifiedBy= null },
                new DomModel.CompanyExpectedMargin{CompanyExpectedMarginId=2, CompanyCode= "UK050",MarginType="TSS (Technical Staffing Services)",MinimumMargin=7.000000M,UpdateCount= null, RecordStatus= null, LastModification=DateTime.UtcNow, ModifiedBy= null },
                new DomModel.CompanyExpectedMargin{CompanyExpectedMarginId=4, CompanyCode= "MX167",MarginType="AIM (Asset Integrity Management)",MinimumMargin=12.000000M,UpdateCount= null, RecordStatus= null, LastModification=DateTime.UtcNow, ModifiedBy= null },
            };
        }

        public static IList<DomModel.CompanyInvoice> GetCompanyInvoiceMockedDomainModelData()
        {
            return new List<DomModel.CompanyInvoice>
            {
                new DomModel.CompanyInvoice{ CompanyCode= "UK050",InvoiceDraftText="DRAFT",InvoiceInterCompText=null,InvoiceInterCompDraftText=null,InvoiceSummarryText=null,UpdateCount= null, RecordStatus= null, LastModification=DateTime.UtcNow, ModifiedBy= null,
                InvoiceRemittances=new List<DomModel.CompanyMessage>{ new DomModel.CompanyMessage { MsgIdentifier= "Default Remittance", MsgType = "InvoiceRemittanceText", MsgText = "All Remittances to be made payable to Intertek Inspection Services UK Limited ", IsDefaultMsg= true, IsActive =true} },
                InvoiceFooters=new List<DomModel.CompanyMessage>{ new DomModel.CompanyMessage { MsgIdentifier= "Default Footer", MsgType = "InvoiceFooterText", MsgText = "Intertek Inspection Services UK Limited ( formerly Moody International Limited).", IsDefaultMsg= true, IsActive =true} }
                }, 
                 new DomModel.CompanyInvoice{ CompanyCode= "UK051",InvoiceDraftText="DRAFT",InvoiceInterCompText=null,InvoiceInterCompDraftText=null,InvoiceSummarryText=null,UpdateCount= null, RecordStatus= null, LastModification=DateTime.UtcNow, ModifiedBy= null,
                InvoiceRemittances=new List<DomModel.CompanyMessage>{ new DomModel.CompanyMessage { MsgIdentifier= "Default Remittance", MsgType = "InvoiceRemittanceText", MsgText = "All Remittances to be made payable to Intertek Inspection Services UK Limited ", IsDefaultMsg= true, IsActive =true} },
                InvoiceFooters=new List<DomModel.CompanyMessage>{ new DomModel.CompanyMessage { MsgIdentifier= "Default Footer", MsgType = "InvoiceFooterText", MsgText = "Intertek Inspection Services UK Limited ( formerly Moody International Limited).", IsDefaultMsg= true, IsActive =true} }
                } 
            };
        }

        public static IList<DomModel.CompanyNote> GetCompanyNoteMockedDomainModelData()
        {
            return new List<DomModel.CompanyNote>
            {
                new DomModel.CompanyNote{CompanyNoteId=1, CompanyCode= "UK050",Notes="Invoice",UpdateCount= null, RecordStatus= null, LastModification=DateTime.UtcNow, ModifiedBy= null },
                new DomModel.CompanyNote{ CompanyNoteId=2,CompanyCode= "UK051",Notes="Assignment File",UpdateCount= null, RecordStatus= null, LastModification=DateTime.UtcNow, ModifiedBy= null },
                new DomModel.CompanyNote{ CompanyNoteId=3,CompanyCode= "UK052",Notes="Contract Copy",UpdateCount= null, RecordStatus= null, LastModification=DateTime.UtcNow, ModifiedBy= null },
                new DomModel.CompanyNote{ CompanyNoteId=4, CompanyCode = "UK052",Notes="Certificate File",UpdateCount= null, RecordStatus= null, LastModification=DateTime.UtcNow, ModifiedBy= null },
            };
        }

        public static IList<DomModel.CompanyAddress> GetCompanyOfficeMockedDomainModelData()
        {
            return new List<DomModel.CompanyAddress>
            {
                new DomModel.CompanyAddress{ CompanyCode= "UK050",OfficeName="Haywards Heath",AccountRef="HH01",FullAddress="Intertek Inspection Services UK Limited (Formerly Moody International Limited)  The Forum,  277 London Road,  Burgess Hill,  West Sussex  RH15 9QU  UK",City="Haywards Heath",PostalCode="RH16 3BW",UpdateCount= null, RecordStatus= null, LastModification=DateTime.UtcNow, ModifiedBy= null },
                new DomModel.CompanyAddress{ CompanyCode= "UK051",OfficeName="Aberdeen",AccountRef="Aber01",FullAddress="SIntertek Inspection Services UK Limited(Form  Excel Centre  Exploration Drive  Aberdeen Science and Energy Park  Bridge of Don  Aberdeen   AB23 8HZ  ",City="Aberdeen",PostalCode="AB23 8HZ",LastModification=null,UpdateCount=null,ModifiedBy=null }
            };
        }

        public static IList<DomModel.CompanyPayrollPeriod> GetCompanyPayrollPeriodMockedDomainModelData()
        {
            return new List<DomModel.CompanyPayrollPeriod>
            {
                new DomModel.CompanyPayrollPeriod{ CompanyPayrollPeriodId=1, CompanyCode= "DZ",PayrollType="LTD/VAT/SE",PeriodName="October",StartDate=Convert.ToDateTime("2007-09-29 00:00:00.000"),EndDate=Convert.ToDateTime("2007-10-26 00:00:00.000"),PeriodStatus="N",IsActive=true,LastModification=null,UpdateCount=null,ModifiedBy=null},
                new DomModel.CompanyPayrollPeriod{ CompanyPayrollPeriodId=2, CompanyCode= "UK051",PayrollType="PAYE Monthly",PeriodName="07Month 12",StartDate=Convert.ToDateTime("2007-02-17 00:00:00.000"),EndDate=Convert.ToDateTime("2007-03-23 00:00:00.000"),PeriodStatus="N",IsActive=true,LastModification=null,UpdateCount=null,ModifiedBy=null}
            };
        }

        public static IList<DomModel.CompanyPayroll> GetCompanyPayrollMockedDomainModelData()
        {
            return new List<DomModel.CompanyPayroll>
            {
                new DomModel.CompanyPayroll{ CompanyPayrollId=66, CompanyCode= "UK050",PayrollType="LTD/VAT/SE", ExportPrefix="AIM",Currency="USD",LastModification=null,UpdateCount=null,ModifiedBy=null},
                new DomModel.CompanyPayroll{ CompanyPayrollId=67, CompanyCode= "UK051",PayrollType="PAYE Monthly",ExportPrefix="C&T",Currency="USD",LastModification=null,UpdateCount=null,ModifiedBy=null}
            };
        }

        public static IList<DomModel.CompanyQualification> GetCompanyQualificationMockedDomainModelData()
        {
            return new List<DomModel.CompanyQualification>
            {
                new DomModel.CompanyQualification{ CompanyCode= "UK050",Qualification="City & Guild Craft - Mechnical",LastModification=null,UpdateCount=null,ModifiedBy=null},
                new DomModel.CompanyQualification{ CompanyCode= "UK050",Qualification="City & Guild Technician - Mechanical",LastModification=null,UpdateCount=null,ModifiedBy=null},
                new DomModel.CompanyQualification{ CompanyCode= "UK050",Qualification="HNC / HND - Mechanical",LastModification=null,UpdateCount=null,ModifiedBy=null},
                new DomModel.CompanyQualification{ CompanyCode= "UK050",Qualification="ONC / OND - Mechnical",LastModification=null,UpdateCount=null,ModifiedBy=null}
            };
        }

        public static IList<DomModel.CompanyTax> GetCompanyTaxMockedDomainModelData()
        {
            return new List<DomModel.CompanyTax>
            {
                new DomModel.CompanyTax{ CompanyTaxId=1, CompanyCode= "UK050",Tax="VAT - 17.5%",LastModification=null,UpdateCount=null,ModifiedBy=null},
                new DomModel.CompanyTax{ CompanyTaxId=2, CompanyCode= "UK050",Tax="VAT - Exempt",LastModification=null,UpdateCount=null,ModifiedBy=null},
                new DomModel.CompanyTax{  CompanyTaxId=6,CompanyCode= "UK050",Tax="GST",LastModification=null,UpdateCount=null,ModifiedBy=null},
            };
        }

    }
}

