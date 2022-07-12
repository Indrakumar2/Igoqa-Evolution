using DbModel = Evolution.DbRepository.Models;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Evolution.UnitTest.Mocks.Data.Masters.Db.Masters
{
    public static partial class MockMaster
    {
        public static IQueryable<DbModel.Country> GetCountryMockData()
        {
            return new List<DbModel.Country>
            {
                new DbModel.Country { Id=1,Code="UK",Name="United Kingdom",RegonId=1,IsEumember=true,Euvatprefix="GB",IsGccmember=false,UpdateCount=0,ModifiedBy=null},
                new DbModel.Country { Id=2,Code="AD",Name="Andorra, Principality of",RegonId=null,IsEumember=false,Euvatprefix=null,IsGccmember=false,UpdateCount=0,ModifiedBy=null},

            }.AsQueryable();

        }

        public static Mock<DbSet<DbModel.Country>> GetCountryMockDbSet(IQueryable<DbModel.Country> data)
        {
            var mockSet = new Mock<DbSet<DbModel.Country>>();
            mockSet.As<IQueryable<DbModel.Country>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<DbModel.Country>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<DbModel.Country>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<DbModel.Country>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            return mockSet;
        }

        public static IQueryable<DbModel.City> GetCityMockData()
        {
            return new List<DbModel.City>
            {
                new DbModel.City { Id=1,CountyId=1185,Name="Brighton",LastModification=DateTime.UtcNow,UpdateCount=0,ModifiedBy=null,County=new DbModel.County{Name="Brighton and Hove"}, },

                new DbModel.City { Id=2,CountyId=3771,Name="Houston",LastModification=DateTime.UtcNow,UpdateCount=0,ModifiedBy=null,County=new DbModel.County{Name="Brighton and Hove"} },
            }.AsQueryable();

        }

        public static Mock<DbSet<DbModel.City>> GetCountryMockDbSet(IQueryable<DbModel.City> data)
        {
            var mockSet = new Mock<DbSet<DbModel.City>>();
            mockSet.As<IQueryable<DbModel.City>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<DbModel.City>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<DbModel.City>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<DbModel.City>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            return mockSet;
        }

        public static IQueryable<DbModel.Tax> GetTaxMockData()
        {
            return new List<DbModel.Tax>
            {
                new DbModel.Tax { Id=1,Code="GST",Name="GST-0%",Rate=5.00M,TaxType="S",IsIcinv=null,IsActive=false,LastModification=DateTime.UtcNow,UpdateCount=0,ModifiedBy=null },
                new DbModel.Tax { Id=2,Code="GST",Name="GST-10%",Rate=0.00M,TaxType="N",IsIcinv=null,IsActive=false,LastModification=DateTime.UtcNow,UpdateCount=0,ModifiedBy=null  },
            }.AsQueryable();

        }
        public static Mock<DbSet<DbModel.Tax>> GetTaxMockDbSet(IQueryable<DbModel.Tax> data)
        {
            var mockSet = new Mock<DbSet<DbModel.Tax>>();
            mockSet.As<IQueryable<DbModel.Tax>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<DbModel.Tax>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<DbModel.Tax>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<DbModel.Tax>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            return mockSet;
        }

        public static IQueryable<DbModel.County> GetCountyMockData()
        {
            return new List<DbModel.County>
            {
                new DbModel.County { Id=1,CountryId=20,Name="Andorra la Vella",LastModification=DateTime.UtcNow,UpdateCount=0,ModifiedBy=null,Country=new DbModel.Country{ Id=20,Name="Andorra, Principality of"} },
                new DbModel.County { Id=2,CountryId=20, Name="Canillo",LastModification=DateTime.UtcNow,UpdateCount=0,ModifiedBy=null,Country=new DbModel.Country{ Id=20,Name="Andorra, Principality of"}},

            }.AsQueryable();

        }
        public static Mock<DbSet<DbModel.County>> GetCountyMockDbSet(IQueryable<DbModel.County> data)
        {
            var mockSet = new Mock<DbSet<DbModel.County>>();
            mockSet.As<IQueryable<DbModel.County>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<DbModel.County>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<DbModel.County>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<DbModel.County>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            return mockSet;
        }
        public static IQueryable<DbModel.EmailPlaceHolder> GetEmailPlaceholderMockData()
        {
            return new List<DbModel.EmailPlaceHolder>
            {
                new DbModel.EmailPlaceHolder {  DisplayName="Coordinator Name",Name="CoordinatorName",ModuleName=null,IsActive=true,LastModification=DateTime.UtcNow,ModifiedBy=null,UpdateCount=0},
                new DbModel.EmailPlaceHolder {  DisplayName="Project Number",Name="ProjectNumber",ModuleName=null,IsActive=true,LastModification=DateTime.UtcNow,ModifiedBy=null,UpdateCount=0},
                new DbModel.EmailPlaceHolder {  DisplayName="Visit Number",Name="VisitNumber",ModuleName="Visit",IsActive=true,LastModification=DateTime.UtcNow,ModifiedBy=null,UpdateCount=0},
            }.AsQueryable();

        }
        public static Mock<DbSet<DbModel.EmailPlaceHolder>> GetEmailPlaceholderMockDbSet(IQueryable<DbModel.EmailPlaceHolder> data)
        {
            var mockSet = new Mock<DbSet<DbModel.EmailPlaceHolder>>();
            mockSet.As<IQueryable<DbModel.EmailPlaceHolder>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<DbModel.EmailPlaceHolder>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<DbModel.EmailPlaceHolder>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<DbModel.EmailPlaceHolder>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            return mockSet;

        }

        public static IQueryable<DbModel.Data> GetInvoicePaymentTermsMockData()
        {
            return new List<DbModel.Data>
            {
                new DbModel.Data { Id=1, Name="Due on receipt of invoice",MasterDataTypeId=10,LastModification=DateTime.UtcNow,UpdateCount=0,ModifiedBy=null  },

                new DbModel.Data { Id=2, Name="10 days from invoice date",MasterDataTypeId=10,LastModification=DateTime.UtcNow,UpdateCount=0,ModifiedBy=null },
            }.AsQueryable();

        }

        public static Mock<DbSet<DbModel.Data>> GetInvoicePaymentTermsMockDbSet(IQueryable<DbModel.Data> data)
        {
            var mockSet = new Mock<DbSet<DbModel.Data>>();
            mockSet.As<IQueryable<DbModel.Data>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<DbModel.Data>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<DbModel.Data>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<DbModel.Data>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            return mockSet;
        }


        public static IQueryable<DbModel.Data> GetCurrencyMockData()
        {
            return new List<DbModel.Data>
            {
                new DbModel.Data { Id=1,Code="GBP", Name="United Kingdom, Pounds",MasterDataTypeId=29,LastModification=DateTime.UtcNow,UpdateCount=0,ModifiedBy=null  },
                new DbModel.Data { Id=2,Code="AED", Name="United Arab Emirates, Dirhams",MasterDataTypeId=29,LastModification=DateTime.UtcNow,UpdateCount=0,ModifiedBy=null },
                new DbModel.Data{ Id=3,Code="AFA",Name="Afghanistan, Afghanis", MasterDataTypeId=29,LastModification=DateTime.UtcNow,UpdateCount=0,ModifiedBy=null },
               new DbModel.Data{ Id=4,Code="ALL",Name="Albania, Leke", MasterDataTypeId=29,LastModification=DateTime.UtcNow,UpdateCount=0,ModifiedBy=null }
            }.AsQueryable();

        }

        public static Mock<DbSet<DbModel.Data>> GetCurrencyMockDbSet(IQueryable<DbModel.Data> data)
        {
            var mockSet = new Mock<DbSet<DbModel.Data>>();
            mockSet.As<IQueryable<DbModel.Data>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<DbModel.Data>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<DbModel.Data>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<DbModel.Data>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            return mockSet;
        }


        public static IQueryable<DbModel.CompanyInspectionTypeChargeRate> GetCompanyInspectionTypeChargeRateMockData()
        {
            return new List<DbModel.CompanyInspectionTypeChargeRate>
            {
                new DbModel.CompanyInspectionTypeChargeRate { Id=1,LastModification=DateTime.UtcNow,UpdateCount=0,ModifiedBy=null,
                CompanyChgSchInspGrpInspectionType=new DbModel.CompanyChgSchInspGrpInspectionType{
                    CompanyChgSchInspGroup=new DbModel.CompanyChgSchInspGroup {
                        CompanyChargeSchedule=new DbModel.CompanyChargeSchedule{
                            CompanyId=1,
                        }
                    }
                }
                },
                
            }.AsQueryable();

        }

        public static Mock<DbSet<DbModel.CompanyInspectionTypeChargeRate>> GetCompanyInspectionTypeChargeRateMockDbSet(IQueryable<DbModel.CompanyInspectionTypeChargeRate> data)
        {
            var mockSet = new Mock<DbSet<DbModel.CompanyInspectionTypeChargeRate>>();
            mockSet.As<IQueryable<DbModel.CompanyInspectionTypeChargeRate>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<DbModel.CompanyInspectionTypeChargeRate>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<DbModel.CompanyInspectionTypeChargeRate>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<DbModel.CompanyInspectionTypeChargeRate>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            return mockSet;
        }

        public static IQueryable<DbModel.Data> GetProjectTypeMockData()
        {
            return new List<DbModel.Data>
            {
                new DbModel.Data { Id=1, Code=null, Name="AIM (Asset Integrity Management)",MasterDataTypeId=12,LastModification=DateTime.UtcNow,UpdateCount=0,ModifiedBy=null  },
                new DbModel.Data { Id=2,Code=null, Name="C&T (Consulting & Training)",MasterDataTypeId=12,LastModification=DateTime.UtcNow,UpdateCount=0,ModifiedBy=null },
                new DbModel.Data{ Id=3,Code=null,Name="MSC (Management System Certification)", MasterDataTypeId=12,LastModification=DateTime.UtcNow,UpdateCount=0,ModifiedBy=null },
               new DbModel.Data{ Id=4,Code=null,Name="NDT (Non Destructive Testing)", MasterDataTypeId=12,LastModification=DateTime.UtcNow,UpdateCount=0,ModifiedBy=null }
            }.AsQueryable();

        }

        public static Mock<DbSet<DbModel.Data>> GetProjectTypeMockDbSet(IQueryable<DbModel.Data> data)
        {
            var mockSet = new Mock<DbSet<DbModel.Data>>();
            mockSet.As<IQueryable<DbModel.Data>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<DbModel.Data>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<DbModel.Data>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<DbModel.Data>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            return mockSet;
        }

        public static IQueryable<DbModel.Data> GetIndustrySectorMockData()
        {
            return new List<DbModel.Data>
            {
                new DbModel.Data { Id=1, Code=null, Name="Oil and Gas - Midstream",MasterDataTypeId=8,LastModification=DateTime.UtcNow,UpdateCount=0,ModifiedBy=null  },
                new DbModel.Data { Id=2,Code=null, Name="Power Generation/Fossil Fuels",MasterDataTypeId=8,LastModification=DateTime.UtcNow,UpdateCount=0,ModifiedBy=null },
                new DbModel.Data{ Id=3,Code=null,Name="Mining", MasterDataTypeId=8,LastModification=DateTime.UtcNow,UpdateCount=0,ModifiedBy=null },
               new DbModel.Data{ Id=4,Code=null,Name="Transport (Vehicles)", MasterDataTypeId=8,LastModification=DateTime.UtcNow,UpdateCount=0,ModifiedBy=null }
            }.AsQueryable();

        }

        public static Mock<DbSet<DbModel.Data>> GetIndustrySectorMockData(IQueryable<DbModel.Data> data)
        {
            var mockSet = new Mock<DbSet<DbModel.Data>>();
            mockSet.As<IQueryable<DbModel.Data>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<DbModel.Data>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<DbModel.Data>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<DbModel.Data>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            return mockSet;
        }
        public static IQueryable<DbModel.Data> GetLogoMockData()
        {
            return new List<DbModel.Data>
            {
                new DbModel.Data { Id=1, Code=null, Name="Intertek",MasterDataTypeId=40,LastModification=DateTime.UtcNow,UpdateCount=0,ModifiedBy=null  },
                new DbModel.Data { Id=2,Code="Company", Name="Intertek Moody",MasterDataTypeId=40,LastModification=DateTime.UtcNow,UpdateCount=0,ModifiedBy=null },
                new DbModel.Data{ Id=3,Code="Project",Name="Intertek - Inspec", MasterDataTypeId=8,LastModification=DateTime.UtcNow,UpdateCount=0,ModifiedBy=null },
              
            }.AsQueryable();

        }

        public static Mock<DbSet<DbModel.Data>> GetLogoMockData(IQueryable<DbModel.Data> data)
        {
            var mockSet = new Mock<DbSet<DbModel.Data>>();
            mockSet.As<IQueryable<DbModel.Data>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<DbModel.Data>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<DbModel.Data>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<DbModel.Data>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            return mockSet;
        }

        public static IQueryable<DbModel.Data> GetDivisonMockData()
        {
            return new List<DbModel.Data>
            {
                new DbModel.Data { Id=1, Code=null, Name="Inspection",MasterDataTypeId=40,LastModification=DateTime.UtcNow,UpdateCount=0,ModifiedBy=null  },
                new DbModel.Data { Id=2,Code=null, Name="ISP",MasterDataTypeId=40,LastModification=DateTime.UtcNow,UpdateCount=0,ModifiedBy=null },
                new DbModel.Data{ Id=3,Code=null,Name="Intertek - Inspec", MasterDataTypeId=8,LastModification=DateTime.UtcNow,UpdateCount=0,ModifiedBy=null },

            }.AsQueryable();

        }

        public static Mock<DbSet<DbModel.Data>> GetDivisonMockData(IQueryable<DbModel.Data> data)
        {
            var mockSet = new Mock<DbSet<DbModel.Data>>();
            mockSet.As<IQueryable<DbModel.Data>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<DbModel.Data>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<DbModel.Data>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<DbModel.Data>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            return mockSet;
        }

        public static IQueryable<DbModel.Data> GetLanguageData()
        {
            return new List<DbModel.Data>
            {
                new DbModel.Data { Id=3795, Code=null, Name="BENGALI",MasterDataTypeId=53,LastModification=DateTime.UtcNow,UpdateCount=0,ModifiedBy=null  },
                new DbModel.Data { Id=3793,Code=null, Name="HINDI",MasterDataTypeId=53,LastModification=DateTime.UtcNow,UpdateCount=0,ModifiedBy=null },
                new DbModel.Data{ Id=3794,Code=null,Name="ENGLISH", MasterDataTypeId=53,LastModification=DateTime.UtcNow,UpdateCount=0,ModifiedBy=null },

            }.AsQueryable();

        }
        public static IQueryable<DbModel.Data> GetCodeStandardData()
        {
            return new List<DbModel.Data>
            {
                new DbModel.Data { Id=3765, Code=null, Name="ACI",MasterDataTypeId=47,LastModification=DateTime.UtcNow,UpdateCount=0,ModifiedBy=null  },
                new DbModel.Data { Id=3766,Code=null, Name="ADA",MasterDataTypeId=47,LastModification=DateTime.UtcNow,UpdateCount=0,ModifiedBy=null },
            }.AsQueryable();

        }
        public static IQueryable<DbModel.Data> GetCertificateName()
        {
            return new List<DbModel.Data>
            {
                new DbModel.Data { Id=3770, Code=null, Name="ACA",MasterDataTypeId=49,LastModification=DateTime.UtcNow,UpdateCount=0,ModifiedBy=null  }
                
            }.AsQueryable();

        }
        public static Mock<DbSet<DbModel.Data>> GetLanguageMockData(IQueryable<DbModel.Data> data)
        {
            var mockSet = new Mock<DbSet<DbModel.Data>>();
            mockSet.As<IQueryable<DbModel.Data>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<DbModel.Data>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<DbModel.Data>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<DbModel.Data>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            return mockSet;
        }

        public static IQueryable<DbModel.Data> GetComputerElectronic()
        {
            return new List<DbModel.Data>
            {
                new DbModel.Data { Id=3767, Code=null, Name="Adobe Flash",MasterDataTypeId=48,LastModification=DateTime.UtcNow,UpdateCount=0,ModifiedBy=null  },
                new DbModel.Data { Id=3768,Code=null, Name="Adobe Photoshop",MasterDataTypeId=48,LastModification=DateTime.UtcNow,UpdateCount=0,ModifiedBy=null },               

            }.AsQueryable();

        }

        public static Mock<DbSet<DbModel.Data>> GetComputerElectronic(IQueryable<DbModel.Data> data)
        {
            var mockSet = new Mock<DbSet<DbModel.Data>>();
            mockSet.As<IQueryable<DbModel.Data>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<DbModel.Data>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<DbModel.Data>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<DbModel.Data>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            return mockSet;
        }
    }
}
