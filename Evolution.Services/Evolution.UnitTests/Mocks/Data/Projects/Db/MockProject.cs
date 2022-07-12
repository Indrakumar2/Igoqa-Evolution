using Evolution.Common.Models.Documents;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DbModel = Evolution.DbRepository.Models;

namespace Evolution.UnitTests.Mocks.Data.Projects.Db
{
    public static class MockProject
    {
        #region MockProject
        public static IQueryable<DbModel.Project> GetprojectMockData()
        {
            return new List<DbModel.Project>()
            {
                new DbModel.Project()
                {
                    Id=1,ContractId=1,Contract=new DbModel.Contract(){Id=1,ContractNumber="SU02412/0001",CustomerId=1,Customer=new DbModel.Customer(){ Id=1} },StartDate=DateTime.UtcNow,Status="O",
                    EndDate=DateTime.UtcNow,ProjectNumber=1,Budget=0.01M,BudgetHours=0.01M,BudgetWarning=80,BudgetHoursWarning=75,WorkFlowType="V",
                    CoordinatorId=1,Coordinator=new DbModel.User(){Id=1,Name="Mark Peacock"},ProjectTypeId=1,ProjectType=new DbModel.Data(){Id=1,Name="TSS (Technical Staffing Services)"},
                    IndustrySector="Mining",IsNewFacility=false,CreationDate=DateTime.UtcNow,CompanyDivisionId=1,CompanyDivision=new DbModel.CompanyDivision(){Id=1,Division=new DbModel.Data(){Id=1,Name="AIM"}},
                    LastModification=DateTime.UtcNow,ModifiedBy="test",UpdateCount=0,CustomerContactId=1,CustomerContact=new DbModel.CustomerContact(){ Id=1,CustomerAddressId=1,CustomerAddress=new DbModel.CustomerAddress(){ Id=1,Address="Tullow Group Services Ltd."} },CustomerProjectAddressId=1

                },

                 new DbModel.Project()
                {
                    Id=2,ContractId=2,Contract=new DbModel.Contract(){Id=2,ContractNumber="SU02412/0002"},StartDate=DateTime.UtcNow,Status="O",
                    EndDate=DateTime.UtcNow,ProjectNumber=2,Budget=0.02M,BudgetHours=0.02M,BudgetWarning=80,BudgetHoursWarning=80,WorkFlowType="V",
                    CoordinatorId=2,Coordinator=new DbModel.User(){Id=2,Name="Nigel Manners"},ProjectTypeId=2,ProjectType=new DbModel.Data(){Id=2,Name="TNDT (Non Destructive Testing)"},
                    IndustrySector="Transport (Vehicles)",IsNewFacility=true,CreationDate=DateTime.UtcNow,CompanyDivisionId=2,CompanyDivision=new DbModel.CompanyDivision(){Id=2,Division=new DbModel.Data(){Id=2,Name="C&T"}},
                    LastModification=DateTime.UtcNow,ModifiedBy="test",UpdateCount=0,CustomerContactId=2,CustomerContact=new DbModel.CustomerContact(){Id=1,CustomerAddressId=1,CustomerAddress=new DbModel.CustomerAddress(){Id=2,Address="Subsea 7 Ltd  Greenwell Base"}},CustomerProjectAddressId=2

                 }
            }.AsQueryable();


        }

        public static Mock<DbSet<DbModel.Project>> GetprojectMockData(IQueryable<DbModel.Project> data)
        {
            var mockSet = new Mock<DbSet<DbModel.Project>>();
            mockSet.As<IQueryable<DbModel.Project>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<DbModel.Project>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<DbModel.Project>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<DbModel.Project>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            return mockSet;
        }
        #endregion

        #region ProjectDocument
        public static IQueryable<DbModel.Document> GetProjectDocumentsMockData()
        {
            return new List<DbModel.Document>()
            {
                new DbModel.Document()
                {
                    Id=1,DocumentName="Document",DocumentType="Customer Report Forms",
                    IsVisibleToCustomer=false,IsVisibleToOutsideOfCompany=false,IsVisibleToTechSpecialist=false,CreatedDate=DateTime.UtcNow,
                    LastModification=DateTime.UtcNow,ModifiedBy="test",UpdateCount=0,Status="CR",DocumentUniqueName="UniqueName",
                    ModuleCode="PRJ",ModuleRefCode="1",CreatedBy="Indu",Size=100
                    

                },
                new DbModel.Document()
                {
                     Id=2,DocumentName="Document",DocumentType="Evolution Email",
                    IsVisibleToCustomer=false,IsVisibleToOutsideOfCompany=false,IsVisibleToTechSpecialist=false,CreatedDate=DateTime.UtcNow,
                    LastModification=DateTime.UtcNow,ModifiedBy="test",UpdateCount=0,Status="CR",DocumentUniqueName="PROJ-18-12-2018-9cb18148-b3d2-4eb7-b144-c4c039d6a874.temp",
                    ModuleCode="PRJ",ModuleRefCode="1",CreatedBy="Indu.N",Size=120

                }
            }.AsQueryable();
        }
        public static Mock<DbSet<DbModel.Document>> GetProjectDocumentsMockData(IQueryable<DbModel.Document> data)
        {
            var mockSet = new Mock<DbSet<DbModel.Document>>();
            mockSet.As<IQueryable<DbModel.Document>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<DbModel.Document>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<DbModel.Document>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<DbModel.Document>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            return mockSet;
        }
        #endregion

        #region ProjectNotes
        public static IQueryable<DbModel.ProjectNote> GetProjectNotesMockedData()
        {
            return new List<DbModel.ProjectNote>()
            {
                new DbModel.ProjectNote()
                {
                    Id=1,ProjectId=1,Project=new DbModel.Project(){Id=1,ProjectNumber=1},CreatedDate=DateTime.UtcNow,CreatedBy="M.peacock",
                    Note="Re-issue of assignment in EVO",LastModification=DateTime.UtcNow,UpdateCount=0,ModifiedBy="test"
                },
                new DbModel.ProjectNote()
                {
                    Id=2,ProjectId=2,Project=new DbModel.Project(){Id=2,ProjectNumber=2},CreatedDate=DateTime.UtcNow,CreatedBy="Nigel Manners",
                    Note="Start date of project:TBA",LastModification=DateTime.UtcNow,UpdateCount=0,ModifiedBy="test"
                }

            }.AsQueryable();
        }

        public static Mock<DbSet<DbModel.ProjectNote>> GetProjectNotesMockedData(IQueryable<DbModel.ProjectNote> data)
        {
            var mockSet = new Mock<DbSet<DbModel.ProjectNote>>();
            mockSet.As<IQueryable<DbModel.ProjectNote>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<DbModel.ProjectNote>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<DbModel.ProjectNote>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<DbModel.ProjectNote>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            return mockSet;
        }
        #endregion

        #region ProjectInvoiceAssignmentReference
        public static IQueryable<DbModel.ProjectInvoiceAssignmentRefence> GetprojectInvoiceAssignmentRefencesMockedData()
        {
            return new List<DbModel.ProjectInvoiceAssignmentRefence>()
            {
                new DbModel.ProjectInvoiceAssignmentRefence()
                {
                    Id=1,ProjectId=1,Project=new DbModel.Project(){Id=1,ProjectNumber=1},AssignmentReferenceTypeId=1,
                    AssignmentReferenceType=new DbModel.Data(){Id=1,Name="# of Observations Raised"},SortOrder=1,IsAssignment=true,IsTimesheet=true,
                    IsVisit=true,LastModification=DateTime.UtcNow,ModifiedBy="test",UpdateCount=0
                },
                new DbModel.ProjectInvoiceAssignmentRefence()
                {
                    Id=2,ProjectId=2,Project=new DbModel.Project(){Id=2,ProjectNumber=2},AssignmentReferenceTypeId=2,
                    AssignmentReferenceType=new DbModel.Data(){Id=2,Name="# of Quality Saves made this Visit"},SortOrder=1,IsAssignment=true,IsTimesheet=true,
                    IsVisit=true,LastModification=DateTime.UtcNow,ModifiedBy="test",UpdateCount=0
                }
            }.AsQueryable();
        }
        public static Mock<DbSet<DbModel.ProjectInvoiceAssignmentRefence>> GetProjectNotesMockedData(IQueryable<DbModel.ProjectInvoiceAssignmentRefence> data)
        {
            var mockSet = new Mock<DbSet<DbModel.ProjectInvoiceAssignmentRefence>>();
            mockSet.As<IQueryable<DbModel.ProjectInvoiceAssignmentRefence>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<DbModel.ProjectInvoiceAssignmentRefence>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<DbModel.ProjectInvoiceAssignmentRefence>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<DbModel.ProjectInvoiceAssignmentRefence>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            return mockSet;
        }
        #endregion

        #region ProjectInvoiceAttachment
        public static IQueryable<DbModel.ProjectInvoiceAttachment> GetProjectInvoiceAttachmentsMockedData()
        {
            return new List<DbModel.ProjectInvoiceAttachment>()
            {
                new DbModel.ProjectInvoiceAttachment()
                {
                    Id=1,ProjectId=1,Project=new DbModel.Project(){Id=1,ProjectNumber=1},DocumentTypeId=1,
                    DocumentType =new DbModel.Data(){Id=1,Name="contract"},LastModification=DateTime.UtcNow,ModifiedBy="test",UpdateCount=0

                },

                 new DbModel.ProjectInvoiceAttachment()
                {
                    Id=2,ProjectId=2,Project=new DbModel.Project(){Id=2,ProjectNumber=2},DocumentTypeId=2,
                    DocumentType =new DbModel.Data(){Id=2,Name="Contract Report"},LastModification=DateTime.UtcNow,ModifiedBy="test",UpdateCount=0

                 }


            }.AsQueryable();
        }

        public static Mock<DbSet<DbModel.ProjectInvoiceAttachment>> GetProjectInvoiceAttachmentsMockedData(IQueryable<DbModel.ProjectInvoiceAttachment> data)
        {
            var mockSet = new Mock<DbSet<DbModel.ProjectInvoiceAttachment>>();
            mockSet.As<IQueryable<DbModel.ProjectInvoiceAttachment>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<DbModel.ProjectInvoiceAttachment>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<DbModel.ProjectInvoiceAttachment>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<DbModel.ProjectInvoiceAttachment>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            return mockSet;
        }
        #endregion

        #region ProjectMessagType
        public static IQueryable<DbModel.ProjectMessageType> GetProjectMessageTypesMockedData()
        {
            return new List<DbModel.ProjectMessageType>()
             {
                new DbModel.ProjectMessageType()
                {
                    Id=1,Name="OperationalNotes",LastModification=DateTime.UtcNow,UpdateCount=0,ModifiedBy=null
                },

                 new DbModel.ProjectMessageType()
                 {
                    Id=2,Name="InvoiceNotes",LastModification=DateTime.UtcNow,UpdateCount=0,ModifiedBy=null
                 },

                 new DbModel.ProjectMessageType()
                 {
                    Id=3,Name="InvoiceFreeText",LastModification=DateTime.UtcNow,UpdateCount=0,ModifiedBy=null
                 },

                  new DbModel.ProjectMessageType()
                  {
                    Id=4,Name="ReportingRequirements",LastModification=DateTime.UtcNow,UpdateCount=0,ModifiedBy=null
                  }

              }.AsQueryable();

        }

        public static Mock<DbSet<DbModel.ProjectMessageType>> GetProjectMessageTypesMockedData(IQueryable<DbModel.ProjectMessageType> data)
        {
            var mockSet = new Mock<DbSet<DbModel.ProjectMessageType>>();
            mockSet.As<IQueryable<DbModel.ProjectMessageType>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<DbModel.ProjectMessageType>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<DbModel.ProjectMessageType>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<DbModel.ProjectMessageType>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            return mockSet;
        }
        #endregion

        #region ProjectMessage
        public static IQueryable<DbModel.ProjectMessage> GetProjectMessagesMockedData()
        {
            return new List<DbModel.ProjectMessage>()
            {
                 new DbModel.ProjectMessage()
                 {
                     Id=1,Identifier=null,ProjectId=1,Project=new DbModel.Project(){Id=1,ProjectNumber=1},MessageTypeId=1,
                     MessageType =new DbModel.ProjectMessageType(){Id=1,Name="OperationalNotes"},Message="Timesheets (Client specific) to be approved by the client prior to issuing to Moody International Ltd.",
                     IsDefaultMessage =true,LastModification=DateTime.UtcNow,IsActive=true,UpdateCount=0,ModifiedBy="test"
                 },
                  new DbModel.ProjectMessage()
                 {
                     Id=2,Identifier=null,ProjectId=2,Project=new DbModel.Project(){Id=2,ProjectNumber=2},MessageTypeId=2,
                     MessageType =new DbModel.ProjectMessageType(){Id=2,Name="InvoiceNotes"},Message="Invoice in Rate currency NOT default GBP  Expenses",
                     IsDefaultMessage =true,LastModification=DateTime.UtcNow,IsActive=true,UpdateCount=0,ModifiedBy="test"
                  },
                  new DbModel.ProjectMessage()
                 {
                     Id=3,Identifier=null,ProjectId=3,Project=new DbModel.Project(){Id=3,ProjectNumber=3},MessageTypeId=3,
                     MessageType =new DbModel.ProjectMessageType(){Id=3,Name="InvoiceFreeText"},Message="Invoice Queries  -  adminservices.uk@intertek.com",
                     IsDefaultMessage =true,LastModification=DateTime.UtcNow,IsActive=true,UpdateCount=0,ModifiedBy="test"
                  },

                   new DbModel.ProjectMessage()
                 {
                     Id=4,Identifier=null,ProjectId=4,Project=new DbModel.Project(){Id=4,ProjectNumber=4},MessageTypeId=4,
                     MessageType =new DbModel.ProjectMessageType(){Id=4,Name="ReportingRequirements"},Message="Moody Standard Report Formats",
                     IsDefaultMessage =true,LastModification=DateTime.UtcNow,IsActive=true,UpdateCount=0,ModifiedBy="test"
                  }

            }.AsQueryable();
        }
        public static Mock<DbSet<DbModel.ProjectMessage>> GetProjectMessageTypesMockedData(IQueryable<DbModel.ProjectMessage> data)
        {
            var mockSet = new Mock<DbSet<DbModel.ProjectMessage>>();
            mockSet.As<IQueryable<DbModel.ProjectMessage>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<DbModel.ProjectMessage>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<DbModel.ProjectMessage>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<DbModel.ProjectMessage>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            return mockSet;
        }
        #endregion

        #region ProjectClientNotification
        public static IQueryable<DbModel.ProjectClientNotification> GetProjectClientNotificationsMockedData()
        {
            return new List<DbModel.ProjectClientNotification>()
            {
                new DbModel.ProjectClientNotification()
                {
                    ProjectId=1,Project=new DbModel.Project(){ Id=1,ProjectNumber=1,ContractId=1,Contract=new DbModel.Contract(){Id=1, ContractNumber="SU02412/0001",
                    CustomerId=1,Customer=new DbModel.Customer(){Id=1,Code="AB00007" } } },
                    SendCustomerDirectReportingNotification=true,SendCustomerReportingNotification=true,SendFlashReportingNotification=true,SendInspectionReleaseNotesNotification=true,SendNcrreportingNotification=true,
                    UpdateCount=0,Id=1,CustomerContact=new DbModel.CustomerContact(){Id=1,ContactName="Layla Gill",CustomerAddressId=1},CustomerContactId=1,ModifiedBy="test"
             
                },

                 new DbModel.ProjectClientNotification()
                 {
                    ProjectId=2,Project=new DbModel.Project(){ Id=2,ProjectNumber=2,ContractId=2,Contract=new DbModel.Contract(){Id=2, ContractNumber="SU02412/0002",
                    CustomerId=2,Customer=new DbModel.Customer(){Id=2,Code="AB00008" } } },
                    SendCustomerDirectReportingNotification=true,SendCustomerReportingNotification=true,SendFlashReportingNotification=true,SendInspectionReleaseNotesNotification=true,SendNcrreportingNotification=true,
                    UpdateCount=0,Id=1,CustomerContact=new DbModel.CustomerContact(){ Id=2,ContactName="Layla Gill"},CustomerContactId=1,ModifiedBy="test"
                 }

            }.AsQueryable();
        }
        public static Mock<DbSet<DbModel.ProjectClientNotification>> GetProjectClientNotificationsMockedData(IQueryable<DbModel.ProjectClientNotification> data)
        {
            var mockSet = new Mock<DbSet<DbModel.ProjectClientNotification>>();
            mockSet.As<IQueryable<DbModel.ProjectClientNotification>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<DbModel.ProjectClientNotification>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<DbModel.ProjectClientNotification>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<DbModel.ProjectClientNotification>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            return mockSet;
        }
        #endregion

        #region ModuleDocumentType
        public static IQueryable<DbModel.ModuleDocumentType> GetmoduleDocumentTypeMockData()
        {
            return new List<DbModel.ModuleDocumentType>
            {
                new DbModel.ModuleDocumentType{ Id=1,ModuleId=3260,Module=new DbModel.Data{ Id=3260,Name="Project",MasterDataTypeId=38},
                                                DocumentType =new DbModel.Data{Id=3164,Name="contract",MasterDataTypeId=37}
                },
               new DbModel.ModuleDocumentType{ Id=2,ModuleId=3260,Module=new DbModel.Data{ Id=3260,Name="project",MasterDataTypeId=38},
                                                DocumentType =new DbModel.Data{Id=3165,Name="Contract Report",MasterDataTypeId=37}
                },
                new DbModel.ModuleDocumentType{ Id=2,ModuleId=3260,Module=new DbModel.Data{ Id=3260,Name="project",MasterDataTypeId=38},
                                                DocumentType =new DbModel.Data{Id=3165,Name="Evolution Email",MasterDataTypeId=37},
                },
                new DbModel.ModuleDocumentType{ Id=2,ModuleId=3260,Module=new DbModel.Data{ Id=3260,Name="project",MasterDataTypeId=38},
                                                DocumentType =new DbModel.Data{Id=3165,Name="Email",MasterDataTypeId=37},
                },

            }.AsQueryable();
        }
        #endregion
        #region AssignmentType
        public static IQueryable<DbModel.Data> GetmoduleAssignmentTypeMockData()
        {
            return new List<DbModel.Data>
            {
                new DbModel.Data{ Id=1,Name="Call Off Number", MasterDataTypeId=30},
                new DbModel.Data{ Id=2,Name="Charge Code" ,MasterDataTypeId=30},
                new DbModel.Data{ Id=3,Name="Cost Centre", MasterDataTypeId=30 },
                new DbModel.Data{ Id=4,Name="Cost Element", MasterDataTypeId=30 }
            }.AsQueryable();
        }
        #endregion
        #region CustomerContact
        public static IQueryable<DbModel.CustomerContact> GetCustomerContactMockData()
        {
            return new List<DbModel.CustomerContact>
            {
                new DbModel.CustomerContact{ Id=1,CustomerAddressId=1,CustomerAddress=new DbModel.CustomerAddress(){CustomerId=1,Address="Tullow Group Services Ltd.",Customer=new DbModel.Customer(){Id=1,Code="TU03659"} },
                                              Position="Commercial Representative",Salutation="Mr.",ContactName="Manfred Hoffman",LastModification=DateTime.UtcNow,ModifiedBy="test",UpdateCount=0

                },
                new DbModel.CustomerContact{ Id=2,CustomerAddressId=2,CustomerAddress=new DbModel.CustomerAddress(){CustomerId=2,Address="Subsea 7 Ltd  Greenwell Base",Customer=new DbModel.Customer(){Id=2,Code="AB00007"} } ,
                                              Position="HR Department",Salutation="Attention :",ContactName="Layla Gill",LastModification=DateTime.UtcNow,ModifiedBy="test",UpdateCount=0

                },
                 new DbModel.CustomerContact{ Id=1,CustomerAddressId=1,CustomerAddress=new DbModel.CustomerAddress(){CustomerId=3,Address="Subsea 7 Ltd  Peregrine Road ",Customer=new DbModel.Customer(){ Id=3,Code="AB00012"} } ,
                                              Position="Procurement Manager",Salutation="Attention :",ContactName="Peter Glynn",LastModification=DateTime.UtcNow,ModifiedBy="test",UpdateCount=0

                },
                 new DbModel.CustomerContact{ Id=1,CustomerAddressId=1,CustomerAddress=new DbModel.CustomerAddress(){CustomerId=1,Address="Tullow Group Services Ltd."} ,
                                              Position="Procurement Department",Salutation="Mr.",ContactName="James Swankie",LastModification=DateTime.UtcNow,ModifiedBy="test",UpdateCount=0

                },

            }.AsQueryable();
        }
        #endregion

        #region User
        public static IQueryable<DbModel.User> GetUserMockData()
        {
            return new List<DbModel.User>
            {
                new DbModel.User{
                    Id =1,Name="June Palmer"}
                
               

            }.AsQueryable();
        }

        #endregion


    }
} 
