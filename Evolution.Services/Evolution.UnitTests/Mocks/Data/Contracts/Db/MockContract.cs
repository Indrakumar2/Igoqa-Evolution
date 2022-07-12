using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DbModel = Evolution.DbRepository.Models;
using DomModel = Evolution.Contract.Domain.Models.Contracts;

namespace Evolution.UnitTests.Mocks.Data.Contracts.Db
{
    public static class MockContract
    {
        #region Contract
        public static IQueryable<DbModel.Contract> GetDbModelContract()
        {
            return new List<DbModel.Contract>()
            {
                new DbModel.Contract(){
                    Id=1,ContractNumber="SU02412/0001",CustomerId=1,CustomerContractNumber="Supply Agreement",ContractHolderCompanyId=1,Budget=0.002M,BudgetCurrency="GBP",BudgetWarning=80,Status="O",DefaultCustomerContractAddressId=57,DefaultCustomerInvoiceAddressId=57,BudgetHours=0.00M,LastModification=DateTime.UtcNow,UpdateCount=0,ContractType="PAR",
                    ContractHolderCompany =new DbModel.Company{Id=1,Code="DZ",CompanyDivision=new List<DbModel.CompanyDivision>(){ new DbModel.CompanyDivision { Id= 1,DivisionId=2304,Company=new DbModel.Company {Id=1,Code="DZ" },CompanyDivisionCostCenter=new List<DbModel.CompanyDivisionCostCenter> { new DbModel.CompanyDivisionCostCenter { Id = 1, CompanyDivisionId = 1, Code = "1", Name = "Burgess Hill", CompanyDivision = new DbModel.CompanyDivision { CompanyId = 1, Id = 1, DivisionId = 2304, Division = new DbModel.Data { Id = 2304, Code = "1", Name = "Inspection" },Company=new DbModel.Company { Id=1,Code="DZ"} } } }, Division = new DbModel.Data { Id = 2304, Code = "116", Name = "Inspection" } } },
                     CompanyOffice=new List<DbModel.CompanyOffice>{new DbModel.CompanyOffice {Id=1,OfficeName= "Haywards Heath", Company=new DbModel.Company { Id=1,Code="DZ"} } },
                     User=new List<DbModel.User>{ new DbModel.User { Id=1,Name = "June Palmer", Company = new DbModel.Company { Id=1,Code="DZ"}  } },
                     CompanyMessage=new List<DbModel.CompanyMessage>{ new DbModel.CompanyMessage {Id=1,Identifier= "Remittance",Company=new DbModel.Company { Id=1,Code="DZ"} } },
                     CompanyTax=new List<DbModel.CompanyTax>{ new DbModel.CompanyTax {Id=1,TaxId=1, Company=new DbModel.Company { Id=1,Code="DZ"},Tax=new DbModel.Tax {Id=1,Code="GST",Name= "GST-0%",TaxType="S" } } }
                    },
                    Customer=new DbModel.Customer{CustomerAddress=new List<DbModel.CustomerAddress>{new DbModel.CustomerAddress {Id=1,Address= "Tullow Group Services Ltd.",CustomerContact=new List<DbModel.CustomerContact> { new DbModel.CustomerContact { Id=1,ContactName= "Manfred Hoffman" } } } }},
                    Project=new List<DbModel.Project>{
                         new DbModel.Project {
                              Coordinator=new DbModel.User{ Name ="M.peacock"}
                         }
                    }
                   
                },

                new DbModel.Contract(){
                   Id=2,ContractNumber="SU02412/0002",CustomerId=2,CustomerContractNumber="Supply Agreement Report",ContractHolderCompanyId=2,Budget=0.012M,BudgetCurrency="GBP",BudgetWarning=82,Status="C",DefaultCustomerContractAddressId=58,DefaultCustomerInvoiceAddressId=58,BudgetHours=0.00M,LastModification=DateTime.UtcNow,UpdateCount=0,ContractType="PAR",ContractHolderCompany=new DbModel.Company{Id=2,Code="UK" }

                },
                new DbModel.Contract(){
                   Id=2,ContractNumber="SU02412/0003",CustomerId=2,CustomerContractNumber="Supply Agreement Report",ContractHolderCompanyId=2,Budget=0.012M,BudgetCurrency="GBP",BudgetWarning=82,Status="O",DefaultCustomerContractAddressId=58,DefaultCustomerInvoiceAddressId=58,BudgetHours=0.00M,LastModification=DateTime.UtcNow,UpdateCount=0,ContractType="PAR",ContractHolderCompany=new DbModel.Company{Id=2,Code="UK" }

                }
            }.AsQueryable();
        }

        public static Mock<DbSet<DbModel.Contract>> GetContractMockDbSet(IQueryable<DbModel.Contract> data)
        {
            var mockSet = new Mock<DbSet<DbModel.Contract>>();
            mockSet.As<IQueryable<DbModel.Contract>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<DbModel.Contract>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<DbModel.Contract>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<DbModel.Contract>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());
            return mockSet;
        }

        #endregion

        #region Contract Exchange Rate

        public static IQueryable<DbModel.ContractExchangeRate> GetContratctExchangeRates()
        {
            return new List<DbModel.ContractExchangeRate>()
            {
                new DbModel.ContractExchangeRate(){
                Id=1,ContractId=1,CurrencyFrom="GBP",CurrencyTo="AFA",EffectiveFrom=DateTime.Now,ExchangeRate=1.000000m,LastModification=DateTime.Now,ModifiedBy="Current User",UpdateCount =1,
                Contract = new DbModel.Contract(){Id=1, ContractNumber="SU02412/0001"}
                }, 
                new DbModel.ContractExchangeRate(){
                   Id=2,ContractId=2 ,CurrencyFrom="AED",CurrencyTo="ALL",EffectiveFrom=DateTime.Now,ExchangeRate=1.567891m,LastModification=DateTime.Now,ModifiedBy="Current User",UpdateCount =1
                   ,Contract = new DbModel.Contract(){Id=2, ContractNumber="SU02412/0002"}
                },
            }.AsQueryable();
        }

        public static Mock<DbSet<DbModel.ContractExchangeRate>> GetContractExchangeRateMockDbSet(IQueryable<DbModel.ContractExchangeRate> data)
        {
            var mockSet = new Mock<DbSet<DbModel.ContractExchangeRate>>();
            mockSet.As<IQueryable<DbModel.ContractExchangeRate>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<DbModel.ContractExchangeRate>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<DbModel.ContractExchangeRate>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<DbModel.ContractExchangeRate>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());
            return mockSet;

        }



        #endregion

        #region ContractInvoiceAttachment

        public static IQueryable<DbModel.ContractInvoiceAttachment> GetDbModelInvoiceAttachments()
        {
            return new List<DbModel.ContractInvoiceAttachment>()
            {
                new DbModel.ContractInvoiceAttachment(){
                    Contract =new DbModel.Contract(){ ContractNumber="SU02412/0001"},

                    DocumentType =new DbModel.Data(){ Name="Contract"},
                    ContractId=1,DocumentTypeId=3164,Id=1,LastModification=DateTime.Now,ModifiedBy="Current User",UpdateCount=1
                    },
                 new DbModel.ContractInvoiceAttachment(){
                    Contract =new DbModel.Contract(){ ContractNumber="SU02412/0002"},

                    DocumentType =new DbModel.Data(){ Name="Contract Report"},
                    ContractId=2,DocumentTypeId=3165,Id=2,LastModification=DateTime.Now,ModifiedBy="Current User",UpdateCount=1
                    },
            }.AsQueryable();
        }

        public static Mock<DbSet<DbModel.ContractInvoiceAttachment>> GetContractInvoiceAttachmentMockDbSet(IQueryable<DbModel.ContractInvoiceAttachment> data)
        {
            var mockSet = new Mock<DbSet<DbModel.ContractInvoiceAttachment>>();
            mockSet.As<IQueryable<DbModel.ContractInvoiceAttachment>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<DbModel.ContractInvoiceAttachment>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<DbModel.ContractInvoiceAttachment>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<DbModel.ContractInvoiceAttachment>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());
            return mockSet;
        }


        #endregion

        #region ContractDocument

        public static IQueryable<DbModel.Document> GetDbModelContractDocuments()
        {
            return new List<DbModel.Document>()
            {
                new DbModel.Document(){

                    Id=1,CreatedDate = DateTime.Now,Size =100,DocumentType ="Email",IsVisibleToCustomer= false,IsVisibleToOutsideOfCompany = false,IsVisibleToTechSpecialist = false,LastModification=DateTime.Now,ModifiedBy="CurrentUser",DocumentName="Email",UpdateCount=null,
                    

                },
                new DbModel.Document(){
                   Id=2, CreatedDate = DateTime.Now,Size =120,DocumentType ="contract",IsVisibleToCustomer= false,IsVisibleToOutsideOfCompany = false,IsVisibleToTechSpecialist = false,LastModification=DateTime.Now,ModifiedBy="CurrentUser",DocumentName="Email",UpdateCount=null,
                  

                }
            }.AsQueryable();
        }

        public static Mock<DbSet<DbModel.Document>> GetContractDocumentsMockDbSet(IQueryable<DbModel.Document> data)
        {
            var mockSet = new Mock<DbSet<DbModel.Document>>();
            mockSet.As<IQueryable<DbModel.Document>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<DbModel.Document>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<DbModel.Document>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<DbModel.Document>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());
            return mockSet;
        }

        #endregion

        #region ContractInvoiceReferences

        public static IQueryable<DbModel.ContractInvoiceReference> GetDbModelContractInvoiceReferences()
        {
            return new List<DbModel.ContractInvoiceReference>()
            {
                new DbModel.ContractInvoiceReference(){
                    Contract = new DbModel.Contract(){ ContractNumber="SU02412/0001"},
                    AssignmentReferenceType = new DbModel.Data() {Name="Call Off Number",Id=1},
                    AssignmentReferenceTypeId= 1,ContractId = 1,Id=1,IsAssignment= false,IsTimesheet = false,IsVisit = false,LastModification = DateTime.Now,ModifiedBy = "Current User",SortOrder=1,UpdateCount=1
                },
                new DbModel.ContractInvoiceReference(){
                    Contract = new DbModel.Contract(){ ContractNumber="SU02412/0002"},
                    AssignmentReferenceType = new DbModel.Data() {Name="Cost Centre",Id=2},
                    AssignmentReferenceTypeId= 1,ContractId = 2,Id=2,IsAssignment= false,IsTimesheet = false,IsVisit = false,LastModification = DateTime.Now,ModifiedBy = "Current User",SortOrder=1,UpdateCount=1
                }
            }.AsQueryable();
        }

        public static Mock<DbSet<DbModel.ContractInvoiceReference>> GetContractInvoiceReferencesMockDbSet(IQueryable<DbModel.ContractInvoiceReference> data)
        {
            var mockSet = new Mock<DbSet<DbModel.ContractInvoiceReference>>();
            mockSet.As<IQueryable<DbModel.ContractInvoiceReference>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<DbModel.ContractInvoiceReference>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<DbModel.ContractInvoiceReference>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<DbModel.ContractInvoiceReference>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());
            return mockSet;
        }
        #endregion

        #region Contract Notes
        public static IQueryable<DbModel.ContractNote> GetContractNoteMockData()
        {
            return new List<DbModel.ContractNote>
            {
                new DbModel.ContractNote{Id=1,ContractId=1,CreatedBy="m.peacock",Note="This is a JV between Aker Solutions/ Santos/ O&G.",CreatedDate=DateTime.UtcNow,LastModification=DateTime.UtcNow,UpdateCount=0,ModifiedBy="Test",
                    Contract=new DbModel.Contract{ Id=1,ContractNumber="SU02412/0001"} },
                new DbModel.ContractNote{Id=2,ContractId=12,CreatedBy="jenna.byth",Note="Current Rates expire on 30th April 2008",CreatedDate=DateTime.UtcNow,LastModification=DateTime.UtcNow,UpdateCount=0,ModifiedBy="Test",
                Contract=new DbModel.Contract{ Id=2,ContractNumber="TU03659/0002"} }
            }.AsQueryable();
        }

        public static Mock<DbSet<DbModel.ContractNote>> GetContractNoteMockDbSet(IQueryable<DbModel.ContractNote> data)
        {
            var mockSet = new Mock<DbSet<DbModel.ContractNote>>();
            mockSet.As<IQueryable<DbModel.ContractNote>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<DbModel.ContractNote>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<DbModel.ContractNote>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<DbModel.ContractNote>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            return mockSet;
        }
        #endregion

        #region ContractSchedule

        public static IQueryable<DbModel.ContractSchedule> GetDbModelContractSchedule()
        {
            return new List<DbModel.ContractSchedule>()
            {
                new DbModel.ContractSchedule(){
                    Contract =new DbModel.Contract(){Id=1, ContractNumber="SU02412/0001"},ContractId=1,
                    Name="Rocks, Gerry",ScheduleNoteForInvoice="Logistic Coordinator",Currency="AED",BaseScheduleId=null,LastModification=DateTime.UtcNow,
                    ModifiedBy ="test",UpdateCount=0,Id=1
                    },
                new DbModel.ContractSchedule(){
                    Contract =new DbModel.Contract(){ ContractNumber="SU02412/0002"},ContractId=2,
                    Name="Norway",ScheduleNoteForInvoice="Eng Consultant",Currency="GBP",BaseScheduleId=null,LastModification=DateTime.UtcNow,
                    ModifiedBy ="test",UpdateCount=0,Id=2,
                    ContractRate=new List<DbModel.ContractRate>
                    {
                        new DbModel.ContractRate{Id=1,Description="Angola - 9 Hours and above"}
                    }
                  
                      

                    },
            }.AsQueryable();
        }

        public static Mock<DbSet<DbModel.ContractSchedule>> GetContractScheduleMockDbSet(IQueryable<DbModel.ContractSchedule> data)
        {
            var mockSet = new Mock<DbSet<DbModel.ContractSchedule>>();
            mockSet.As<IQueryable<DbModel.ContractSchedule>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<DbModel.ContractSchedule>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<DbModel.ContractSchedule>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<DbModel.ContractSchedule>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());
            return mockSet;
        }


        #endregion

        #region ContractScheduleRate
        public static IQueryable<DbModel.ContractRate> GetDbModelContractRate()
        {
            return new List<DbModel.ContractRate>()
            {
                 new DbModel.ContractRate()
                 {
                     Id=1,ContractScheduleId=1,
                     ContractSchedule=new DbModel.ContractSchedule(){Id=1,
                                Name ="Rocks, Gerry",
                                Contract=new DbModel.Contract {Id=1, ContractNumber="SU02412/0001" }
                     },
                     ExpenseType =new DbModel.Data(){Id=1,Name="Kilometers"},
                     Rate=695.50M,Description="Angola - 9 Hours and above",IsPrintDescriptionOnInvoice=false,
                     FromDate =DateTime.Now,
                     ToDate =DateTime.Now,
                     StandardInspectionTypeChargeRateId=1,
                     LastModification =DateTime.UtcNow,
                     ModifiedBy ="test",
                     UpdateCount =0

                 },
                 new DbModel.ContractRate()
                 {
                     Id=2,
                     ContractSchedule=new DbModel.ContractSchedule(){Name="Norway",
                     Contract=new DbModel.Contract {Id=2, ContractNumber="SU02412/0002" }
                     },
                     ExpenseType =new DbModel.Data(){Id=1,Name="Car Hire"},
                     Rate=695.50M,Description="Forest DB",IsPrintDescriptionOnInvoice=false,
                     FromDate =DateTime.Now,ToDate=DateTime.Now,
                     StandardInspectionTypeChargeRateId=2,LastModification=DateTime.UtcNow,ModifiedBy="test",UpdateCount=0

                 }


            }.AsQueryable();


        }
        public static Mock<DbSet<DbModel.ContractRate>> GetContractScheduleMockDbSet(IQueryable<DbModel.ContractRate> data)
        {
            var mockSet = new Mock<DbSet<DbModel.ContractRate>>();
            mockSet.As<IQueryable<DbModel.ContractRate>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<DbModel.ContractRate>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<DbModel.ContractRate>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<DbModel.ContractRate>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());
            return mockSet;
        }


        #endregion
         
        #region ModuleDocumentType
        public static IQueryable<DbModel.ModuleDocumentType> GetmoduleDocumentTypeMockData()
        {
            return new List<DbModel.ModuleDocumentType>
            {
                new DbModel.ModuleDocumentType{ Id=1,ModuleId=3257,Module=new DbModel.Data{ Id=3257,Name="Contract",MasterDataTypeId=38},
                                                DocumentType =new DbModel.Data{Id=3164,Name="contract",MasterDataTypeId=37}
                },
               new DbModel.ModuleDocumentType{ Id=2,ModuleId=3257,Module=new DbModel.Data{ Id=3257,Name="Contract",MasterDataTypeId=38},
                                                DocumentType =new DbModel.Data{Id=3165,Name="Contract Report",MasterDataTypeId=37}
               }

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

        #region ExpenseType
        public static IQueryable<DbModel.Data> GetExpenseTypeMockData()
        {
            return new List<DbModel.Data>
            {
                new DbModel.Data{ Id=1,Name="Kilometers", MasterDataTypeId=7},
                new DbModel.Data{ Id=2,Name="Car Hire" ,MasterDataTypeId=7},
                new DbModel.Data{ Id=3,Name="Communications", MasterDataTypeId=7},
                new DbModel.Data{ Id=4,Name="Lump Sum", MasterDataTypeId=7 }
            }.AsQueryable();
        }

        #endregion

        #region InspectionTypeChargeRate
        public static IQueryable<DbModel.CompanyInspectionTypeChargeRate> GetCompanyInspectionTypeMockData()
        {
            return new List<DbModel.CompanyInspectionTypeChargeRate>
            {
                new DbModel.CompanyInspectionTypeChargeRate{ Id=1,CompanyChgSchInspGrpInspectionTypeId=1,ItemDescription="JUNIOR API INSPECTOR",
                                                                  RateOffShoreOil=145.00M,RateOnShoreOil=145.00M,RateOnShoreNonOil=145.00M,
                                                                  ExpenseType=new DbModel.Data{ Id=1,Name="Kilometers", MasterDataTypeId=7},
                                                               LastModification=DateTime.UtcNow,UpdateCount=0,ModifiedBy="test"

                                                            },
                new DbModel.CompanyInspectionTypeChargeRate{ Id=2,CompanyChgSchInspGrpInspectionTypeId=2,ItemDescription="JUNIOR API CORDINATOR",
                                                                  RateOffShoreOil=150.00M,RateOnShoreOil=150.00M,RateOnShoreNonOil=150.00M,
                                                                  ExpenseType=new DbModel.Data{ Id=1,Name="Communications", MasterDataTypeId=7},
                                                                LastModification=DateTime.UtcNow,UpdateCount=0,ModifiedBy="test"},

            }.AsQueryable();
        }

        #endregion

        #region CompanyChgSchInspGrpInspectionType
        public static IQueryable<DbModel.CompanyChgSchInspGrpInspectionType> GetCompanyChgSchInspGrpInspectionTypeMockData()
        {
            return new List<DbModel.CompanyChgSchInspGrpInspectionType>
            {

                  new DbModel.CompanyChgSchInspGrpInspectionType{Id=1,StandardInspectionType=new DbModel.Data{ Id=1,Name="1-Man UT Thickness Unit"}
                      ,CompanyChgSchInspGroupId=1,LastModification=DateTime.UtcNow,UpdateCount=0,ModifiedBy="test"}

            }.AsQueryable();
        }

        #endregion

        #region StandardInspectionType
        public static IQueryable<DbModel.Data> GetStandardInspectionTypeMockData() {

            return new List<DbModel.Data>
            {
                new DbModel.Data{Id=1,Name="1-Man UT Thickness Unit",MasterDataTypeId=21},
                  new DbModel.Data{Id=2,Name="2018 Rates for Shell Pipeline",MasterDataTypeId=21}

            }.AsQueryable();
          
        }
        #endregion

        #region CompanyChgSchInspGroup
        public static IQueryable<DbModel.CompanyChgSchInspGroup> GetCompanyChgSchInspGroupMockData()
        {
            return new List<DbModel.CompanyChgSchInspGroup>
            {
                new DbModel.CompanyChgSchInspGroup{Id=1,CompanyChargeSchedule=new DbModel.CompanyChargeSchedule
                {
                    Company=new DbModel.Company{Id=1,Code="UK050"},StandardChargeSchedule=new DbModel.Data{Id=1,Name="2016 MASTER LIST",MasterDataTypeId=36},Currency="AE"
                }
                }
            }.AsQueryable();
        }

        #endregion
    }
}
