using Evolution.Common.Models.Documents;
using Evolution.DbRepository.Models;
using Evolution.Document.Domain.Models.Document;
using System;
using System.Collections.Generic;
using System.Text;
using DomModel = Evolution.Contract.Domain.Models.Contracts;

namespace Evolution.UnitTests.Mocks.Data.Contracts.Domain
{
    public static class MockContractDomainModel
    {
        public static IList<DomModel.Contract> GetDomainModelContract()
        {
            return new List<DomModel.Contract>()
            {
                new DomModel.Contract(){
                    ContractNumber ="SU02412/0001",ContractBudgetMonetaryValue= 0,ContractBudgetMonetaryCurrency= "GBP",
                    ContractBudgetMonetaryWarning= null,ContractBudgetHours= 0,ContractBudgetHoursWarning= 75,IsParentContract= null,
                    IsParentContractInvoiceUsed= false,IsChildContract= null,ParentContractNumber= null,ParentCompanyOffice= null,
                    ParentContractDiscount= null,ParentContractHolder= null,IsFrameworkContract= null,FrameworkCompanyOfficeName= null,
                    IsRelatedFrameworkContract= null,FrameworkContractNumber= null,FrameworkContractHolder= null,IsCRM= false,
                    ContractCRMReference= null,ContractCRMReason= "Legacy Data",ContractClientReportingRequirement= null,
                    ContractOperationalNote= null,ContractInvoicePaymentTerms= "Due on receipt of invoice",ContractCustomerContact= "Karen Innes",
                    ContractCustomerContactAddress= "Subsea 7 Ltd  Greenwell Base  Greenwell Road  Aberdeen  AB12 3AX",
                    ContractCustomerInvoiceContact= "ELC Process Support Team Accounts",ContractCustomerInvoiceAddress= "Subsea 7 Ltd  Peregrine Road  Westhill Business Park  Westhill  Aberdeenshire  AB32 6JL",
                    ContractInvoiceRemittanceIdentifier= null,ContractSalesTax= "VAT - 17.5%",ContractWithHoldingTax= null,ContractInvoicingCurrency= "GBP",ContractInvoiceGrouping= "Assignment",
                    ContractInvoiceFooterIdentifier= null,ContractInvoiceInstructionNotes= "Limited Company and Day Rate : Day rates are calculated on the basis of a 9 hour day and all day rated personnel should note that a minimum of 8 hours working constitutes a working day and 4 hours working constitutes a half (0.5) day.\r\n\r\n3rd Party Agency / Temporary : Hourly rate is paid for actual hours worked and hourly rated personnel should note that a minimum of 7.5 hours working constitutes a working day and 4 hours working constitutes a half (0.5) day.\r\n\r\nSAP ID number to be quoted on all invoices.",
                    ContractInvoiceFreeText= null,ContractConflictOfInterest= null,IsFixedExchangeRateUsed= false,IsRemittanceText= null,
                    ContractHoldingCompanyCode= "DZ",ContractHoldingCompanyName= "UK - Intertek Inspection Services UK Ltd",
                    ContractInvoicingCompanyCode= "DZ",ContractInvoicingCompanyName= "UK - Intertek Inspection Services UK Ltd",
                    ContractCustomerCode = "SU02412",ContractCustomerName= "SUBSEA 7",CustomerContractNumber= "Supply Agreement",ContractStartDate= DateTime.UtcNow,ContractEndDate= DateTime.UtcNow,
                    ContractType= null,ContractStatus= "O",UpdateCount= null,LastModification=  DateTime.UtcNow,ModifiedBy= null
                },
                new DomModel.Contract(){ContractNumber="SU02412/0002",ContractCustomerCode="AB00002",CustomerContractNumber="Supply Agreement Reports",ContractHoldingCompanyCode="UK051",ContractBudgetHours=0.004M,ContractBudgetMonetaryCurrency="EUR",ContractBudgetHoursWarning=80,ContractStatus="C",ContractCustomerContactAddress="ABB K.K.  26-1, Sakuragaoka-cho  Cerulean Tower  Shibuya-ku, Tokyo 150-8512  Japan",ContractCustomerInvoiceAddress="ABB FRANCE  ATPA-AXB  ZA Des Combaruches  73100 AIX-Les-Bains  ",ContractStartDate=DateTime.Now, ContractEndDate=DateTime.Now,LastModification=DateTime.UtcNow,UpdateCount=0,ContractType="PAR" },
                 new DomModel.Contract(){ContractNumber="SU02412/0003",ContractCustomerCode="AB00002",CustomerContractNumber="Supply Agreement Reports",ContractHoldingCompanyCode="UK051",ContractBudgetHours=0.004M,ContractBudgetMonetaryCurrency="EUR",ContractBudgetHoursWarning=80,ContractStatus="O",ContractCustomerContactAddress="ABB K.K.  26-1, Sakuragaoka-cho  Cerulean Tower  Shibuya-ku, Tokyo 150-8512  Japan",ContractCustomerInvoiceAddress="ABB FRANCE  ATPA-AXB  ZA Des Combaruches  73100 AIX-Les-Bains  ",ContractStartDate=DateTime.Now, ContractEndDate=DateTime.Now,LastModification=DateTime.UtcNow,UpdateCount=0,ContractType="PAR" }
            };
        }

        public static IList<DomModel.ContractExchangeRate> GetDomainModelContractExchangeRates()
        {
            return new List<DomModel.ContractExchangeRate>()
            {
                new DomModel.ContractExchangeRate(){ExchangeRateId=1,ContractNumber="SU02412/0001",EffectiveFrom= DateTime.Now,ExchangeRate=1.000000M,FromCurrency="GBP",ToCurrency="AFA",LastModification=DateTime.Now,ModifiedBy="Current User",UpdateCount = 1 },
                new DomModel.ContractExchangeRate(){ ExchangeRateId=2,ContractNumber="SU02412/0002",EffectiveFrom= DateTime.Now,ExchangeRate=1.000000M,FromCurrency="AED",ToCurrency="ALL",LastModification=DateTime.Now,ModifiedBy="Current User",UpdateCount = 1 }
            };
        }

        public static IList<DomModel.ContractInvoiceAttachment> GetDomainModelInvoiceAttachments()
        {
            return new List<DomModel.ContractInvoiceAttachment>()
            {
                new DomModel.ContractInvoiceAttachment(){ContractInvoiceAttachmentId=1,ContractNumber="SU02412/0001",DocumentType="contract",LastModification=DateTime.Now,ModifiedBy="Current User",RecordStatus=null,UpdateCount=1},
                new DomModel.ContractInvoiceAttachment(){ContractInvoiceAttachmentId=2,ContractNumber="SU02412/0002",DocumentType="Contract Report",LastModification=DateTime.Now,ModifiedBy="Current User",RecordStatus=null,UpdateCount=1}
            };
        }

        public static IList<ModuleDocument> GetDomainModelContractDocument()
        {
            return new List<ModuleDocument>()
            {
                new ModuleDocument(){Id=1 ,DocumentType="Certificate",DocumentSize=100,IsVisibleToCustomer=false,DocumentName="Contract",IsVisibleToTS=false,LastModification=DateTime.Now,ModifiedBy="Current User",RecordStatus=null,UpdateCount=null},
                new ModuleDocument(){Id=2,DocumentType="Certificate",DocumentSize=120,IsVisibleToCustomer=false,DocumentName="Contract Report",IsVisibleToTS=false,LastModification=DateTime.Now,ModifiedBy="Current User",RecordStatus=null,UpdateCount=null},
            };
        }

        public static IList<DomModel.ContractInvoiceReferenceType> GetDomainModelInvoiceReference()
        {
            return new List<DomModel.ContractInvoiceReferenceType>()
            {
                new DomModel.ContractInvoiceReferenceType()
                {
                    ContractNumber="SU02412/0001",DisplayOrder=1,IsVisibleToAssignment= false,IsVisibleToTimesheet=false,IsVisibleToVisit=false,LastModification=DateTime.Now,ModifiedBy="CurrentUser",RecordStatus=null,ReferenceType="Charge Code",UpdateCount=1,ContractInvoiceReferenceTypeId=1
                },
                  new DomModel.ContractInvoiceReferenceType()
                {
                    ContractNumber="SU02412/0002",DisplayOrder=1,IsVisibleToAssignment= false,IsVisibleToTimesheet=false,IsVisibleToVisit=false,LastModification=DateTime.Now,ModifiedBy="CurrentUser",RecordStatus=null,ReferenceType="Cost Element",UpdateCount=1,ContractInvoiceReferenceTypeId=2
                }
            };
        }

        public static IList<DomModel.ContractNote> GetContractNoteMockedDomainModelData()
        {
            return new List<DomModel.ContractNote>
            {
                new DomModel.ContractNote{ContractNoteId=1, ContractNumber="SU02412/0001",Notes="This is a JV between Aker Solutions/ Santos/ O&G.",UpdateCount= 0, RecordStatus= null, LastModification=DateTime.UtcNow, ModifiedBy= "test" },
                new DomModel.ContractNote{ ContractNoteId=2,ContractNumber= "SU02412/0002",Notes="Current Rates expire on 30th April 2008",UpdateCount= 0, RecordStatus= null, LastModification=DateTime.UtcNow, ModifiedBy= "test" },

            };
        }

        public static IList<DomModel.ContractSchedule> GetContractScheduleMockedDomainModelData()
        {
            return new List<DomModel.ContractSchedule>
            {
                new DomModel.ContractSchedule{ ContractNumber="SU02412/0001",ScheduleName="Norway",ScheduleNameForInvoicePrint="Logistic Coordinator",ChargeCurrency="AED",BaseScheduleName=null,LastModification=DateTime.UtcNow,ModifiedBy="test",UpdateCount=0,ScheduleId=1},
                new DomModel.ContractSchedule{ ContractNumber="SU02412/0002",ScheduleName="Fox",ScheduleNameForInvoicePrint="Eng Consultant",ChargeCurrency="ALL",BaseScheduleName=null,LastModification=DateTime.UtcNow,ModifiedBy="test",UpdateCount=0,ScheduleId=2},

            };
        }

        public static IList<DomModel.ContractScheduleRate> GetContractScheduleRateMockedDomainModelData()
        {
            return new List<DomModel.ContractScheduleRate>
            {
                new DomModel.ContractScheduleRate{RateId =1, ContractNumber="SU02412/0001",ScheduleName="Rocks, Gerry",
                     ChargeType="Lump Sum",
                    Description="JUNIOR API INSPECTOR",ChargeValue=1,
                     EffectiveFrom =DateTime.Now,EffectiveTo=DateTime.Now,
                     StandardInspectionTypeChargeRateId=1,LastModification=DateTime.UtcNow,ModifiedBy="test",UpdateCount=0},
                new DomModel.ContractScheduleRate{ RateId =2,ContractNumber="SU02412/0002",ScheduleName="Algeria"},
            };
        }
    }
}
