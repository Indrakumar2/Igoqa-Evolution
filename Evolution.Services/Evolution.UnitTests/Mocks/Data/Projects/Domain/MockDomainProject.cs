using Evolution.Common.Models.Documents;
using Evolution.Document.Domain.Models.Document;
using System;
using System.Collections.Generic;
using System.Text;
using DomModel = Evolution.Project.Domain.Models.Projects;

namespace Evolution.UnitTests.Mocks.Data.Projects.Domain
{
    public static class MockProjectDomainModel
    {
        #region Project
        public static IList<DomModel.Project> GetProjectsMockedDomainData()
        {
            return new List<DomModel.Project>()
            {
                new DomModel.Project()
                {
                  ContractNumber="SU02412/0001",ProjectStartDate=DateTime.UtcNow,ProjectStatus="O",ProjectEndDate=DateTime.UtcNow,ProjectNumber=1,
                   ProjectBudgetValue=0.50M,ProjectBudgetWarning=80,ProjectBudgetHoursUnit=0.01M,ProjectBudgetHoursWarning=0,WorkFlowType="V",
                   ProjectCoordinatorName="June Palmer",ProjectType="AIM (Asset Integrity Management)",IndustrySector="Oil and Gas - Midstream",IsProjectForNewFacility=true,
                   CreationDate=DateTime.UtcNow,IsManagedServices=true,ManagedServiceType="Integrated Svcs Coordination",ManagedServiceCoordinatorName="June Palmer",
                   IsExtranetSummaryVisibleToClient=false,IsRemittanectext=true,IsEReportProjectMapped=false,CompanyDivision="Inspection",CompanyCostCenterCode="1",CompanyCostCenterName="Burgess Hill",
                   CompanyOffice="Haywards Heath",CustomerProjectNumber="MI Agency Agreement",CustomerProjectName="Agency Worker Supply Agreement",ProjectCustomerContact="Manfred Hoffman",ProjectCustomerInvoiceAddress="Tullow Group Services Ltd.",ProjectCustomerContactAddress="Tullow Group Services Ltd.",
                   ProjectSalesTax="GST-0%",ProjectInvoicingCurrency="GBP",ProjectInvoiceGrouping="Project",ProjectInvoiceRemittanceIdentifier="Remittance",ProjectInvoiceFooterIdentifier="Remittance",ProjectInvoicePaymentTerms="Due on receipt of invoice",LastModification=DateTime.UtcNow,ModifiedBy="test",UpdateCount=0,LogoText="Intertek",
                   ProjectAssignmentOperationNotes="Notes",ProjectInvoiceInstructionNotes="Instruction Notes",ProjectCustomerInvoiceContact="Manfred Hoffman",ContractHoldingCompanyCode="DZ",ContractHoldingCompanyName="Algeria MI",ContractCustomerCode="TU03659",ContractCustomerName="ABB"

                },
                new DomModel.Project()
                {
                    ContractNumber ="SU02412/0002",ProjectStartDate=DateTime.UtcNow,ProjectStatus="C",
                    ProjectEndDate=DateTime.UtcNow,ProjectNumber=2,ProjectBudgetHoursUnit=0.02M,ProjectBudgetWarning=75,ProjectBudgetHoursWarning=98,WorkFlowType="V",
                   ProjectType="PC (Product Certification)",IndustrySector="Transport (Vehicles)",IsProjectForNewFacility=false,CreationDate=DateTime.UtcNow,CompanyDivision="AIM (Tas ISI)",
                    LastModification=DateTime.UtcNow,ModifiedBy="test",UpdateCount=0
                },
            };
        }
        #endregion

        #region Project Notes
        public static IList<DomModel.ProjectNote> GetProjectNotesMockedDomainData()
        {
            return new List<DomModel.ProjectNote>()
            {
                new DomModel.ProjectNote()
                {
                    ProjectNoteId=1,ProjectNumber=1,CreatedOn=DateTime.UtcNow,CreatedBy="M.peacock",
                    Notes="Re-issue of assignment in EVO",LastModification=DateTime.UtcNow,UpdateCount=0,ModifiedBy="test"
                },


                new DomModel.ProjectNote()
                {
                    ProjectNoteId=2,ProjectNumber=2,CreatedOn=DateTime.UtcNow,CreatedBy="Nigel Manners",
                    Notes="Draft 2293 R21 181.25",LastModification=DateTime.UtcNow,UpdateCount=0,ModifiedBy="test"
                }

            };

        }
        #endregion

        #region Project Documents
        public static IList<ModuleDocument> GetProjectDocumentsMockedDomainData()
        {
            return new List<ModuleDocument>()
            {
                new ModuleDocument()
                {

                    Id=1,DocumentName="Document",DocumentType="Evolution Email",
                    IsVisibleToCustomer=false,IsVisibleToTS=false,DocumentSize=100,Status="CR",
                    LastModification=DateTime.UtcNow,ModifiedBy="test",UpdateCount=0,DocumentUniqueName="UniqueName",
                    CreatedBy="Indu",CreatedOn=DateTime.UtcNow,IsVisibleOutOfCompany=true,ModuleCode="PRJ",ModuleRefCode="1"
                    
                },

                new ModuleDocument()
                {

                    Id=2,DocumentName="Project",DocumentType="Email",
                    IsVisibleToCustomer=false,IsVisibleToTS=false,DocumentSize=100,Status="C",
                    LastModification=DateTime.UtcNow,ModifiedBy="test",UpdateCount=0,DocumentUniqueName="PROJ-17-12-2018-c6e9bd13-448d-4c34-93a7-2f06ab1ac162.temp",
                      CreatedBy="Indu",CreatedOn=DateTime.UtcNow,IsVisibleOutOfCompany=true,ModuleCode="PROJ",ModuleRefCode="1"

                }
            };
        }

        #endregion
        #region ProjectInvoiceAttachment
        public static IList<DomModel.ProjectInvoiceAttachment> GetProjectInvoiceAttachmentsMockedDomaindata()
        {
            return new List<DomModel.ProjectInvoiceAttachment>()
            {
                new DomModel.ProjectInvoiceAttachment()
                {
                      ProjectInvoiceAttachmentId=1,ProjectNumber=1,DocumentType ="Evolution Email",LastModification=DateTime.UtcNow,
                    ModifiedBy ="test",UpdateCount=0

                },

                new DomModel.ProjectInvoiceAttachment()
                {
                      ProjectInvoiceAttachmentId=2,ProjectNumber=2,DocumentType ="Email",LastModification=DateTime.UtcNow,
                    ModifiedBy ="test",UpdateCount=0

                }
            };

        }
        #endregion

        #region ProjectInvoiceReference
        public static IList<DomModel.ProjectInvoiceReference> GetProjectInvoiceReferencesMockedDomainData()
        {
            return new List<DomModel.ProjectInvoiceReference>()
            {
                new DomModel.ProjectInvoiceReference()
                {
                    ProjectInvoiceReferenceTypeId=1,ProjectNumber=1,
                    ReferenceType="Charge Code",DisplayOrder=1,IsVisibleToAssignment=true,IsVisibleToTimesheet=true,
                    IsVisibleToVisit=true,LastModification=DateTime.UtcNow,ModifiedBy="test",UpdateCount=0
                },

                new DomModel.ProjectInvoiceReference()
                {
                    ProjectInvoiceReferenceTypeId=2,ProjectNumber=2,
                    ReferenceType="Call Off Number",DisplayOrder=2,IsVisibleToAssignment=true,IsVisibleToTimesheet=true,
                    IsVisibleToVisit=true,LastModification=DateTime.UtcNow,ModifiedBy="test",UpdateCount=0
                }
            };
        }

        #endregion

        #region ProjectClientNotification
        public static IList<DomModel.ProjectClientNotification> GetProjectClientNotificationsMockeDomainData()

        {
            return new List<DomModel.ProjectClientNotification>()
            {
                new DomModel.ProjectClientNotification()
                {
                   ProjectNumber=1, ProjectClientNotificationId=1,CustomerContact="Manfred Hoffman",IsSendInspectionReleaseNotesNotification=false,IsSendFlashReportingNotification=false,IsSendNCRReportingNotification=false,IsSendCustomerReportingNotification=false,
                    IsSendCustomerDirectReportingNotification=false,UpdateCount=0,ModifiedBy="test",LastModification=DateTime.UtcNow
                },
                 new DomModel.ProjectClientNotification()
                {
                   ProjectNumber=2, ProjectClientNotificationId=2,CustomerContact="Layla Gill",IsSendInspectionReleaseNotesNotification=true,IsSendFlashReportingNotification=true,IsSendNCRReportingNotification=true,IsSendCustomerReportingNotification=true,
                    IsSendCustomerDirectReportingNotification=false,UpdateCount=0,ModifiedBy="test"
                }
            };
        }
        #endregion

        

    }
}
