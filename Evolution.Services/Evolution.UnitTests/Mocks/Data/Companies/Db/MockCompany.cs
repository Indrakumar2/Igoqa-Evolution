using DbModel = Evolution.DbRepository.Models;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Evolution.UnitTests.Mocks.Data.Companies.Db
{
    public static class MockCompany
    {
        #region Company
        public static IQueryable<DbModel.Company> GetCompanyMockData()
        {
            return new List<DbModel.Company>
            {
                new DbModel.Company{Id=1,Code="DZ",Name="Algeria MI",InvoiceCompanyName= "MI Algeria",IsActive=true,NativeCurrency="DZD",CompanyMiiwaid=1,OperatingCountry="Algeria",UpdateCount=null,
                CompanyTax=new List<DbModel.CompanyTax> {  new DbModel.CompanyTax{ Id=1,CompanyId=1,TaxId=643,LastModification=DateTime.UtcNow,UpdateCount=null,ModifiedBy=null,
                    Company =new DbModel.Company { Id=116,Code="DZ" },
                    Tax =new DbModel.Tax { Id=643,Code="VAT",Name="VAT - 17.5%",Rate=17.5000M,TaxType="S",IsIcinv=null,IsActive=true,LastModification=DateTime.UtcNow,UpdateCount=null,ModifiedBy=null}
                } }
                },
                new DbModel.Company{Id=67, Code="BR007" ,Name="Brasil MI", InvoiceCompanyName="Intertek Industry Services Brasil Ltda",UpdateCount=null}
            }.AsQueryable();
        }

        public static Mock<DbSet<DbModel.Company>> GetCompanyMockDbSet(IQueryable<DbModel.Company> data)
        {
            var mockSet = new Mock<DbSet<DbModel.Company>>();
            mockSet.As<IQueryable<DbModel.Company>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<DbModel.Company>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<DbModel.Company>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<DbModel.Company>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            return mockSet;
        }
        #endregion

        #region CompanyTax
        public static IQueryable<DbModel.CompanyTax> GetCompanyTaxMockData()
        {
            return new List<DbModel.CompanyTax>
            {
                new DbModel.CompanyTax{ Id=1,CompanyId=116,TaxId=643,LastModification=DateTime.UtcNow,UpdateCount=null,ModifiedBy=null,
                    Company =new DbModel.Company { Id=116,Code="UK050" },
                    Tax =new DbModel.Tax { Id=643,Code="VAT",Name="VAT - 17.5%",Rate=17.5000M,TaxType="S",IsIcinv=null,IsActive=true,LastModification=DateTime.UtcNow,UpdateCount=null,ModifiedBy=null}
                },
                new DbModel.CompanyTax{ Id=2,CompanyId=116,TaxId=654,LastModification=DateTime.UtcNow,UpdateCount=null,ModifiedBy=null,
                    Company =new DbModel.Company { Id=116,Code="UK050" },
                    Tax =new DbModel.Tax { Id=654,Code="VAT",Name="VAT - Exempt",Rate=0.0000M,TaxType="S",IsIcinv=null,IsActive=true,LastModification=DateTime.UtcNow,UpdateCount=null,ModifiedBy=null}
                },
                 new DbModel.CompanyTax{ Id=3,CompanyId=117,TaxId=654,LastModification=DateTime.UtcNow,UpdateCount=null,ModifiedBy=null,
                    Company =new DbModel.Company { Id=117,Code="UK051" },
                    Tax =new DbModel.Tax { Id=654,Code="VAT",Name="VAT - Exempt",Rate=0.0000M,TaxType="S",IsIcinv=null,IsActive=true,LastModification=DateTime.UtcNow,UpdateCount=null,ModifiedBy=null}
                },
                  new DbModel.CompanyTax{ Id=4,CompanyId=118,TaxId=654,LastModification=DateTime.UtcNow,UpdateCount=null,ModifiedBy=null,
                    Company =new DbModel.Company { Id=118,Code="UK052" },
                    Tax =new DbModel.Tax { Id=654,Code="VAT",Name="VAT - Exempt",Rate=0.0000M,TaxType="S",IsIcinv=null,IsActive=true,LastModification=DateTime.UtcNow,UpdateCount=null,ModifiedBy=null}
                },
                new DbModel.CompanyTax{ Id=6,CompanyId=70,TaxId=516,LastModification=DateTime.UtcNow,UpdateCount=null,ModifiedBy=null,
                    Company =new DbModel.Company { Id=116,Code="CA" },
                    Tax =new DbModel.Tax { Id=516,Code="GST  ",Name="GST",Rate=5.0000M,TaxType="S",IsIcinv=null,IsActive=true,LastModification=DateTime.UtcNow,UpdateCount=null,ModifiedBy=null}
                },
            }.AsQueryable();
        }

        public static Mock<DbSet<DbModel.CompanyTax>> GetCompanyTaxMockDbSet(IQueryable<DbModel.CompanyTax> data)
        {
            var mockSet = new Mock<DbSet<DbModel.CompanyTax>>();
            mockSet.As<IQueryable<DbModel.CompanyTax>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<DbModel.CompanyTax>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<DbModel.CompanyTax>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<DbModel.CompanyTax>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            return mockSet;
        }
        #endregion

        #region CompanyOffice

        public static IQueryable<DbModel.CompanyOffice> GetCompanyAddressMockData()
        {
            return new List<DbModel.CompanyOffice>
            {
                new DbModel.CompanyOffice{ Id=1,CompanyId=1,OfficeName="Haywards Heath",AccountRef="HH01",Address="Intertek Inspection Services UK Limited (Formerly Moody International Limited)  The Forum,  277 London Road,  Burgess Hill,  West Sussex  RH15 9QU  UK",CityId=2976,PostalCode="RH16 3BW",LastModification=null,UpdateCount=null,ModifiedBy=null,
                    City=new DbModel.City { Id=2976,CountyId=1390,Name="Haywards Heath",LastModification=null, UpdateCount=null,ModifiedBy=null,
                            County = new DbModel.County { Id=1390,Name="West Sussex",
                                Country=new DbModel.Country{ Id=1,Code ="UK",Name="United Kingdom"}
                            }
                    },
                    Company =new DbModel.Company { Id=116,Code="UK050" }
                },
                new DbModel.CompanyOffice{ Id=2,CompanyId=116,OfficeName="Aberdeen",AccountRef="Aber01",Address="SIntertek Inspection Services UK Limited(Form  Excel Centre  Exploration Drive  Aberdeen Science and Energy Park  Bridge of Don  Aberdeen   AB23 8HZ  ",CityId=80,PostalCode="AB23 8HZ",LastModification=null,UpdateCount=null,ModifiedBy=null,
                    City=new DbModel.City {Id=80,CountyId=1162,Name="Aberdeen",LastModification=null, UpdateCount=null,ModifiedBy=null,
                            County = new DbModel.County { Id=1162,Name="Aberdeen",
                                Country=new DbModel.Country{ Id=1,Code ="UK",Name="United Kingdom"}
                            }
                    },
                    Company =new DbModel.Company { Id=116,Code="UK050" }
                },
            }.AsQueryable();
        }

        public static Mock<DbSet<DbModel.CompanyOffice>> GetCompanyAddressMockDbSet(IQueryable<DbModel.CompanyOffice> data)
        {
            var mockSet = new Mock<DbSet<DbModel.CompanyOffice>>();
            mockSet.As<IQueryable<DbModel.CompanyOffice>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<DbModel.CompanyOffice>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<DbModel.CompanyOffice>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<DbModel.CompanyOffice>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            return mockSet;
        }
        #endregion

        #region CompanyDivisionCostCenter

        public static IQueryable<DbModel.CompanyDivisionCostCenter> GetCompanyDivisionCostCenterMockData()
        {
            return new List<DbModel.CompanyDivisionCostCenter>
            {
                new DbModel.CompanyDivisionCostCenter{ Id=1,CompanyDivisionId=1,Code="1",Name="Burgess Hill",LastModification=null,UpdateCount=null,ModifiedBy=null,
                    CompanyDivision=new DbModel.CompanyDivision
                    {
                      Company=new DbModel.Company {Id=1,Code="DZ",Name ="Algeria MI" },
                      Division =new DbModel.Data { Id =2304,Code="116", Name="Inspection" },
                    },
                     Project=new List<DbModel.Project>(){new DbModel.Project { Id=1,ProjectNumber=12,CompanyanyDivCostCentreId=1 } }

                },
                 new DbModel.CompanyDivisionCostCenter{ Id=2,CompanyDivisionId=92,Code="4",Name="Azerbaijan",LastModification=null,UpdateCount=null,ModifiedBy=null,
                    CompanyDivision=new DbModel.CompanyDivision
                    {
                      Company=new DbModel.Company {Id=116,Code="UK050",Name ="UK - Intertek Inspection Services UK Ltd" },
                      Division =new DbModel.Data { Id =2305,Code="116", Name="PSO" }
                    },

                },
                 new DbModel.CompanyDivisionCostCenter{ Id=3,CompanyDivisionId=92,Code="CC4",Name="Azerbaijan",LastModification=null,UpdateCount=null,ModifiedBy=null,
                    CompanyDivision=new DbModel.CompanyDivision
                    {
                      Company=new DbModel.Company {Id=117,Code="UK051",Name ="UK - MIC Derby" },
                      Division =new DbModel.Data { Id =2305,Code="116", Name="PSO" }
                    }
                }
            }.AsQueryable();
        }

        public static Mock<DbSet<DbModel.CompanyDivisionCostCenter>> GetCompanyDivisionCostCenterMockDbSet(IQueryable<DbModel.CompanyDivisionCostCenter> data)
        {
            var mockSet = new Mock<DbSet<DbModel.CompanyDivisionCostCenter>>();
            mockSet.As<IQueryable<DbModel.CompanyDivisionCostCenter>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<DbModel.CompanyDivisionCostCenter>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<DbModel.CompanyDivisionCostCenter>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<DbModel.CompanyDivisionCostCenter>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            return mockSet;
        }
        #endregion


        #region CompanyDivision

        public static IQueryable<DbModel.CompanyDivision> GetCompanyDivisionMockData()
        {
            return new List<DbModel.CompanyDivision>
            {
               new DbModel.CompanyDivision{
                   Id=1,CompanyId=1,DivisionId=2304,AccountReference="05",LastModification=DateTime.UtcNow,UpdateCount=0,ModifiedBy=null,
                   Company=new DbModel.Company{Id=1,Code="DZ",Name="Algeria MI"},
                   Division=new DbModel.Data{Id=2304,Code="1",Name="Inspection"},
                   CompanyDivisionCostCenter=new List<DbModel.CompanyDivisionCostCenter>
                   { new DbModel.CompanyDivisionCostCenter { Id=1,CompanyDivisionId=1,Code="1",Name="Burgess Hill",
                       CompanyDivision =new DbModel.CompanyDivision{ CompanyId=1,Id=1,DivisionId=2304,Division=new DbModel.Data{Id=2304,Code="1",Name="Inspection"}} }
                   }
                   
               },

                 new DbModel.CompanyDivision{ Id=2,CompanyId=116,DivisionId=2304,AccountReference="06",LastModification=null,UpdateCount=null,ModifiedBy=null,
                       Company=new DbModel.Company {Id=116,Code="UK050",Name ="UK - Intertek Inspection Services UK Ltd" },
                       Division =new DbModel.Data { Id =2304,Code="115", Name="Inspection" }
                },
                  new DbModel.CompanyDivision{ Id=3,CompanyId=117,DivisionId=2305,AccountReference="07",LastModification=null,UpdateCount=null,ModifiedBy=null,
                       Company=new DbModel.Company {Id=117,Code="UK051",Name ="UK - MIC Derby" },
                      Division =new DbModel.Data { Id =2305,Code="116", Name="PSO" }
                }
            }.AsQueryable();
        }

        public static Mock<DbSet<DbModel.CompanyDivision>> GetCompanyDivisionMockDbSet(IQueryable<DbModel.CompanyDivision> data)
        {
            var mockSet = new Mock<DbSet<DbModel.CompanyDivision>>();
            mockSet.As<IQueryable<DbModel.CompanyDivision>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<DbModel.CompanyDivision>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<DbModel.CompanyDivision>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<DbModel.CompanyDivision>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            return mockSet;
        }
        #endregion


        #region CompanyDocument
        public static IQueryable<DbModel.Document> GetCompanyDocumentMockData()
        {
            return new List<DbModel.Document>
            {
                new DbModel.Document{ Id=1,DocumentName="Invoice",DocumentType="Email",IsVisibleToCustomer=true,Size=1000,LastModification=null,UpdateCount=null,ModifiedBy=null,                  
                } ,
                new DbModel.Document{ Id=2,DocumentName="Assignment File",DocumentType="Assignment",IsVisibleToCustomer=true,Size=1000,LastModification=null,UpdateCount=null,ModifiedBy=null,                        
                },
                new DbModel.Document{ Id=3,DocumentName="Contract Copy",DocumentType="Contract",IsVisibleToCustomer=false,Size=1000,LastModification=null,UpdateCount=null,ModifiedBy=null,
                       
                },
                new DbModel.Document{ Id=4,DocumentName="Certificate File",DocumentType="Email",IsVisibleToCustomer=true,Size=1000,LastModification=null,UpdateCount=null,ModifiedBy=null,
                     
                },
            }.AsQueryable();
        }

        public static Mock<DbSet<DbModel.Document>> GetCompanyDocumentMockDbSet(IQueryable<DbModel.Document> data)
        {
            var mockSet = new Mock<DbSet<DbModel.Document>>();
            mockSet.As<IQueryable<DbModel.Document>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<DbModel.Document>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<DbModel.Document>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<DbModel.Document>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            return mockSet;
        }
        #endregion


        #region CompanyMessage
        public static IQueryable<DbModel.CompanyMessage> GetEmailTemplateMockData()
        {
            return new List<DbModel.CompanyMessage>
            {
                new DbModel.CompanyMessage{ Id=1,CompanyId=116,Identifier=null,MessageTypeId=1,Message="",IsDefaultMessage=true, LastModification=null,IsActive=true,UpdateCount=null,ModifiedBy=null,
                                    MessageType=new DbModel.CompanyMessageType {Id=1, Name ="EmailCustomerReportingNotification" },
                                    Company=new DbModel.Company {Id=116,Code="UK050",Name ="UK - Intertek Inspection Services UK Ltd" },
                } ,
                new DbModel.CompanyMessage{ Id=2,CompanyId=116,Identifier=null,MessageTypeId=2,Message="",IsDefaultMessage=true, LastModification=null,IsActive=true,UpdateCount=null,ModifiedBy=null,
                                    MessageType=new DbModel.CompanyMessageType {Id=2, Name ="EmailCustomerDirectReporting" },
                                    Company=new DbModel.Company {Id=116,Code="UK050",Name ="UK - Intertek Inspection Services UK Ltd" },
                } ,
                new DbModel.CompanyMessage{ Id=3,CompanyId=116,Identifier=null,MessageTypeId=3,Message="",IsDefaultMessage=true, LastModification=null,IsActive=true,UpdateCount=null,ModifiedBy=null,
                                    MessageType=new DbModel.CompanyMessageType {Id=3, Name ="EmailRejectedVisit" },
                                    Company=new DbModel.Company {Id=116,Code="UK050",Name ="UK - Intertek Inspection Services UK Ltd" },
                } ,
                new DbModel.CompanyMessage{ Id=4,CompanyId=116,Identifier=null,MessageTypeId=4,Message="",IsDefaultMessage=true, LastModification=null,IsActive=true,UpdateCount=null,ModifiedBy=null,
                                    MessageType=new DbModel.CompanyMessageType {Id=4, Name ="EmailVisitCompletedToCoordinator" },
                                    Company=new DbModel.Company {Id=116,Code="UK050",Name ="UK - Intertek Inspection Services UK Ltd" },
                } ,
            }.AsQueryable();
        }

        public static Mock<DbSet<DbModel.CompanyMessage>> GetEmailTemplateMockDbSet(IQueryable<DbModel.CompanyMessage> data)
        {
            var mockSet = new Mock<DbSet<DbModel.CompanyMessage>>();
            mockSet.As<IQueryable<DbModel.CompanyMessage>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<DbModel.CompanyMessage>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<DbModel.CompanyMessage>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<DbModel.CompanyMessage>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            return mockSet;
        }
        #endregion



      # region CompanyEamil
        public static IQueryable<DbModel.Company> GetCompanyEmailTemplatesMockData()
        {
            return new List<DbModel.Company> {
                 new DbModel.Company { Code="UK050",
                    CompanyMessage= new List<DbModel.CompanyMessage>{
                               new DbModel.CompanyMessage{ Id=1,CompanyId=116,Identifier=null,MessageTypeId=1,Message="",IsDefaultMessage=true, LastModification=null,IsActive=true,UpdateCount=null,ModifiedBy=null,
                                    MessageType=new DbModel.CompanyMessageType {Id=1, Name ="EmailCustomerReportingNotification" },
                                    Company=new DbModel.Company {Id=116,Code="UK050",Name ="UK - Intertek Inspection Services UK Ltd" },
                                } ,
                                new DbModel.CompanyMessage{ Id=2,CompanyId=116,Identifier=null,MessageTypeId=2,Message="",IsDefaultMessage=true, LastModification=null,IsActive=true,UpdateCount=null,ModifiedBy=null,
                                                    MessageType=new DbModel.CompanyMessageType {Id=2, Name ="EmailCustomerDirectReporting" },
                                                    Company=new DbModel.Company {Id=116,Code="UK050",Name ="UK - Intertek Inspection Services UK Ltd" },
                                } ,
                                new DbModel.CompanyMessage{ Id=3,CompanyId=116,Identifier=null,MessageTypeId=3,Message="",IsDefaultMessage=true, LastModification=null,IsActive=true,UpdateCount=null,ModifiedBy=null,
                                                    MessageType=new DbModel.CompanyMessageType {Id=3, Name ="EmailRejectedVisit" },
                                                    Company=new DbModel.Company {Id=116,Code="UK050",Name ="UK - Intertek Inspection Services UK Ltd" },
                                } ,
                                new DbModel.CompanyMessage{ Id=4,CompanyId=116,Identifier=null,MessageTypeId=4,Message="",IsDefaultMessage=true, LastModification=null,IsActive=true,UpdateCount=null,ModifiedBy=null,
                                                    MessageType=new DbModel.CompanyMessageType {Id=4, Name ="EmailVisitCompletedToCoordinator" },
                                                    Company=new DbModel.Company {Id=116,Code="UK050",Name ="UK - Intertek Inspection Services UK Ltd" },
                                } ,
                    }
                 }
            }.AsQueryable();
        }

        public static Mock<DbSet<DbModel.Company>> GetCompanyEmailTemplatesMockDbSet(IQueryable<DbModel.Company> data)
        {
            var mockSet = new Mock<DbSet<DbModel.Company>>();
            mockSet.As<IQueryable<DbModel.Company>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<DbModel.Company>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<DbModel.Company>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<DbModel.Company>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            return mockSet;
        }
        #endregion

        #region Expectedmargin

        public static IQueryable<DbModel.CompanyExpectedMargin> GetCompanyExpectedMarginMockData()
        {
            return new List<DbModel.CompanyExpectedMargin>
            {
                new DbModel.CompanyExpectedMargin{ Id=1,CompanyId=116,MinimumMargin=15.000000M,MarginTypeId=3166,LastModification=null,UpdateCount=null,ModifiedBy=null,
                    Company=new DbModel.Company {Id=116,Code="UK050",Name ="UK - Intertek Inspection Services UK Ltd" },
                    MarginType=new DbModel.Data { Id =3166, Name="TIS (Technical Inspection Services)" }
                } ,
                new DbModel.CompanyExpectedMargin{ Id=2,CompanyId=116,MinimumMargin=7.000000M,MarginTypeId=3167,LastModification=null,UpdateCount=null,ModifiedBy=null,
                    Company=new DbModel.Company {Id=116,Code="UK050",Name ="UK - Intertek Inspection Services UK Ltd" },
                    MarginType=new DbModel.Data { Id =3167, Name="TSS (Technical Staffing Services)" }
                } ,
                new DbModel.CompanyExpectedMargin{ Id=3,CompanyId=233,MinimumMargin=12.000000M,MarginTypeId=3161,LastModification=null,UpdateCount=null,ModifiedBy=null,
                    Company=new DbModel.Company {Id=233,Code="MX167",Name ="Mexico - Intertek Testing Services de Mexico SA" },
                    MarginType=new DbModel.Data { Id =3161, Name="AIM (Asset Integrity Management)" }
                } ,
                new DbModel.CompanyExpectedMargin{ Id=4,CompanyId=233,MinimumMargin=40.000000M,MarginTypeId=3166,LastModification=null,UpdateCount=null,ModifiedBy=null,
                    Company=new DbModel.Company {Id=233,Code="MX167",Name ="Mexico - Intertek Testing Services de Mexico SA" },
                    MarginType=new DbModel.Data { Id =3166, Name="TIS (Technical Inspection Services)" }
                } ,
                new DbModel.CompanyExpectedMargin{ Id=5,CompanyId=233,MinimumMargin=20.000000M,MarginTypeId=3167,LastModification=null,UpdateCount=null,ModifiedBy=null,
                    Company=new DbModel.Company {Id=233,Code="MX167",Name ="Mexico - Intertek Testing Services de Mexico SA" },
                    MarginType=new DbModel.Data { Id =3167, Name="TSS (Technical Staffing Services)" }
                }
            }.AsQueryable();
        }

        public static Mock<DbSet<DbModel.CompanyExpectedMargin>> GetCompanyExpectedMarginMockDbSet(IQueryable<DbModel.CompanyExpectedMargin> data)
        {
            var mockSet = new Mock<DbSet<DbModel.CompanyExpectedMargin>>();
            mockSet.As<IQueryable<DbModel.CompanyExpectedMargin>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<DbModel.CompanyExpectedMargin>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<DbModel.CompanyExpectedMargin>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<DbModel.CompanyExpectedMargin>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            return mockSet;
        }
        #endregion

        #region companymessage1

        public static IQueryable<DbModel.CompanyMessage> GetCompanyInvoiceMockData()
        {
            return new List<DbModel.CompanyMessage>
            {
                new DbModel.CompanyMessage{ Id=1,CompanyId=116,Identifier=null,MessageTypeId=1,Message="",IsDefaultMessage=true, LastModification=null,IsActive=true,UpdateCount=null,ModifiedBy=null,
                                    MessageType=new DbModel.CompanyMessageType {Id=1, Name ="InvoiceRemittanceText" },
                                    Company=new DbModel.Company {Id=116,Code="UK050",Name ="UK - Intertek Inspection Services UK Ltd" },
                } ,
                new DbModel.CompanyMessage{ Id=2,CompanyId=116,Identifier=null,MessageTypeId=2,Message="",IsDefaultMessage=true, LastModification=null,IsActive=true,UpdateCount=null,ModifiedBy=null,
                                    MessageType=new DbModel.CompanyMessageType {Id=2, Name ="InvoiceFooterText" },
                                    Company=new DbModel.Company {Id=116,Code="UK050",Name ="UK - Intertek Inspection Services UK Ltd" },
                } ,
                new DbModel.CompanyMessage{ Id=3,CompanyId=116,Identifier=null,MessageTypeId=3,Message="",IsDefaultMessage=true, LastModification=null,IsActive=true,UpdateCount=null,ModifiedBy=null,
                                    MessageType=new DbModel.CompanyMessageType {Id=3, Name ="InvoiceDraftText" },
                                    Company=new DbModel.Company {Id=116,Code="UK050",Name ="UK - Intertek Inspection Services UK Ltd" },
                } ,
                new DbModel.CompanyMessage{ Id=4,CompanyId=116,Identifier=null,MessageTypeId=4,Message="",IsDefaultMessage=true, LastModification=null,IsActive=true,UpdateCount=null,ModifiedBy=null,
                                    MessageType=new DbModel.CompanyMessageType {Id=4, Name ="InvoiceDescriptionText" },
                                    Company=new DbModel.Company {Id=116,Code="UK050",Name ="UK - Intertek Inspection Services UK Ltd" },
                } ,
            }.AsQueryable();
        }

        public static Mock<DbSet<DbModel.CompanyMessage>> GetCompanyInvoiceMockDbSet(IQueryable<DbModel.CompanyMessage> data)
        {
            var mockSet = new Mock<DbSet<DbModel.CompanyMessage>>();
            mockSet.As<IQueryable<DbModel.CompanyMessage>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<DbModel.CompanyMessage>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<DbModel.CompanyMessage>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<DbModel.CompanyMessage>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            return mockSet;
        }
        #endregion


        #region CompanyInvoice
        public static IQueryable<DbModel.Company> GetCompanyInvoiceMessagessMockData()
        {
            return new List<DbModel.Company> {
                 new DbModel.Company { Code="UK050",
                    CompanyMessage= new List<DbModel.CompanyMessage>{
                            new DbModel.CompanyMessage{ Id=1,CompanyId=116,Identifier=null,MessageTypeId=1,Message="Remittance",IsDefaultMessage=true, LastModification=null,IsActive=true,UpdateCount=null,ModifiedBy=null,
                                                MessageType=new DbModel.CompanyMessageType {Id=1, Name ="InvoiceRemittanceText" },
                                                Company=new DbModel.Company {Id=116,Code="UK050",Name ="UK - Intertek Inspection Services UK Ltd" },
                            } ,
                            new DbModel.CompanyMessage{ Id=2,CompanyId=116,Identifier=null,MessageTypeId=2,Message="Footer",IsDefaultMessage=true, LastModification=null,IsActive=true,UpdateCount=null,ModifiedBy=null,
                                                MessageType=new DbModel.CompanyMessageType {Id=2, Name ="InvoiceFooterText" },
                                                Company=new DbModel.Company {Id=116,Code="UK050",Name ="UK - Intertek Inspection Services UK Ltd" },
                            } ,
                            new DbModel.CompanyMessage{ Id=3,CompanyId=116,Identifier=null,MessageTypeId=3,Message="",IsDefaultMessage=true, LastModification=null,IsActive=true,UpdateCount=null,ModifiedBy=null,
                                                MessageType=new DbModel.CompanyMessageType {Id=3, Name ="InvoiceDraftText" },
                                                Company=new DbModel.Company {Id=116,Code="UK050",Name ="UK - Intertek Inspection Services UK Ltd" },
                            } ,
                            new DbModel.CompanyMessage{ Id=4,CompanyId=116,Identifier=null,MessageTypeId=4,Message="",IsDefaultMessage=true, LastModification=null,IsActive=true,UpdateCount=null,ModifiedBy=null,
                                                MessageType=new DbModel.CompanyMessageType {Id=4, Name ="InvoiceDescriptionText" },
                                                Company=new DbModel.Company {Id=116,Code="UK050",Name ="UK - Intertek Inspection Services UK Ltd" },
                            } ,
                    }
                 }
            }.AsQueryable();
        }

        public static Mock<DbSet<DbModel.Company>> GetCompanyInvoiceMessagesMockDbSet(IQueryable<DbModel.Company> data)
        {
            var mockSet = new Mock<DbSet<DbModel.Company>>();
            mockSet.As<IQueryable<DbModel.Company>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<DbModel.Company>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<DbModel.Company>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<DbModel.Company>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            return mockSet;
        }
        #endregion

        #region  CompanyNote
        public static IQueryable<DbModel.CompanyNote> GetCompanyNoteMockData()
        {
            return new List<DbModel.CompanyNote>
            {
                  new DbModel.CompanyNote{ Id=1,CompanyId=116,Note="changed admin address to reception H as per Nigel request",CreatedBy="Jennn.Blyth",CreatedDate=DateTime.UtcNow,LastModification=null,UpdateCount=null,ModifiedBy=null,
                         Company=new DbModel.Company {Id=116,Code="UK050",Name ="UK - Intertek Inspection Services UK Ltd" },
                } ,
                new DbModel.CompanyNote{ Id=2,CompanyId=117,Note="Average TS hourly cost taken from StarBitz tool used to determine 2010 standard rates.",CreatedBy="Joe.Row",CreatedDate=DateTime.UtcNow,LastModification=null,UpdateCount=null,ModifiedBy=null,
                         Company=new DbModel.Company {Id=117,Code="UK051",Name ="UK - MIC Derby" },
                },
                new DbModel.CompanyNote{ Id=3,CompanyId=118,Note="Welcome to MI Extranet!  Please note MI Company Inspection forms and company notices may be obtained under Company Documents portion of this Extranet.  IMPORTANT NOTICE:  US Nuclear Regulatory Commission 10CFR 21: Notification of Defects  If you suspect that any item of equipment that is destined for a nuclear facility in the USA is defective, contact your Project Coordinator immediately!  This applies to you whether or not you are assigned to work on that specific item of equipment.  “Defective” means deviates from the technical requirements.",CreatedBy="Jennn.Blyth",CreatedDate=DateTime.UtcNow,LastModification=null,UpdateCount=null,ModifiedBy=null,
                         Company=new DbModel.Company {Id=118,Code="UK052",Name ="UK - MIC Cuckfield-DO NOT USE" },
                },
                new DbModel.CompanyNote{ Id=4,CompanyId=119,Note="Name Change to PIpe payroll due to incorrect naming of payroll schedules.  There had been an additional payroll added into the system 01/2011b which was incorrect.  01/2011b became 02/2011 and the additional pay periods changed accordingly.",CreatedBy="Jeennn.Crystien",CreatedDate=DateTime.UtcNow,LastModification=null,UpdateCount=null,ModifiedBy=null,
                         Company=new DbModel.Company {Id=119,Code="UK056",Name ="UK Russia MI" },
                },
            }.AsQueryable();
        }

        public static Mock<DbSet<DbModel.CompanyNote>> GetCompanyNoteMockDbSet(IQueryable<DbModel.CompanyNote> data)
        {
            var mockSet = new Mock<DbSet<DbModel.CompanyNote>>();
            mockSet.As<IQueryable<DbModel.CompanyNote>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<DbModel.CompanyNote>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<DbModel.CompanyNote>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<DbModel.CompanyNote>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            return mockSet;
        }
        #endregion


        #region CompanyPayrollPeriod
        public static IQueryable<DbModel.CompanyPayrollPeriod> GetCompanyPayrollPeriodMockData()
        {
            return new List<DbModel.CompanyPayrollPeriod>
            {
                  new DbModel.CompanyPayrollPeriod{ Id=1,CompanyPayrollId=66,PeriodName="October",StartDate=Convert.ToDateTime("2007-09-29 00:00:00.000"),EndDate=Convert.ToDateTime("2007-10-26 00:00:00.000"),PeriodStatus="N",IsActive=true,LastModification=null,UpdateCount=null,ModifiedBy=null,
                            CompanyPayroll= new DbModel.CompanyPayroll { Id=66,CompanyId=116,PayrollTypeId=2898,
                                    Company=new DbModel.Company { Id=116,Code="UK050",Name ="UK - Intertek Inspection Services UK Ltd" },
                                    PayrollType=new DbModel.Data{ Id=2898,Name="LTD/VAT/SE" }
                            }
                  },
                   new DbModel.CompanyPayrollPeriod{ Id=2,CompanyPayrollId=67,PeriodName="07Month 12",StartDate=Convert.ToDateTime("2007-02-17 00:00:00.000"),EndDate=Convert.ToDateTime("2007-03-23 00:00:00.000"),PeriodStatus="N",IsActive=true,LastModification=null,UpdateCount=null,ModifiedBy=null,
                            CompanyPayroll= new DbModel.CompanyPayroll { Id=67,CompanyId=116,PayrollTypeId=2899,
                                    Company=new DbModel.Company { Id=116,Code="UK050",Name ="UK - Intertek Inspection Services UK Ltd" },
                                    PayrollType=new DbModel.Data{ Id=2899,Name="PAYE Monthly" }
                            }
                  },
                    new DbModel.CompanyPayrollPeriod{ Id=1,CompanyPayrollId=69,PeriodName="Week 12",StartDate=Convert.ToDateTime("2007-06-18 00:00:00.000"),EndDate=Convert.ToDateTime("2007-06-24 00:00:00.000"),PeriodStatus="T",IsActive=true,LastModification=null,UpdateCount=null,ModifiedBy=null,
                            CompanyPayroll= new DbModel.CompanyPayroll { Id=69,CompanyId=116,PayrollTypeId=2900,
                                    Company=new DbModel.Company { Id=116,Code="UK050",Name ="UK - Intertek Inspection Services UK Ltd" },
                                    PayrollType=new DbModel.Data{ Id=2900,Name="PAYE Weekly" }
                            }
                  },
            }.AsQueryable();
        }

        public static Mock<DbSet<DbModel.CompanyPayrollPeriod>> GetCompanyPayrollPeriodMockDbSet(IQueryable<DbModel.CompanyPayrollPeriod> data)
        {
            var mockSet = new Mock<DbSet<DbModel.CompanyPayrollPeriod>>();
            mockSet.As<IQueryable<DbModel.CompanyPayrollPeriod>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<DbModel.CompanyPayrollPeriod>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<DbModel.CompanyPayrollPeriod>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<DbModel.CompanyPayrollPeriod>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            return mockSet;
        }
        #endregion

# region CompanyPayroll

        public static IQueryable<DbModel.CompanyPayroll> GetCompanyPayrollMockData()
        {
            return new List<DbModel.CompanyPayroll>
            {
                new DbModel.CompanyPayroll { Id=66,CompanyId=116,PayrollTypeId=2898,Currency="USD",LastModification=null,UpdateCount=null,ModifiedBy=null,
                                    Company=new DbModel.Company { Id=116,Code="UK050",Name ="UK - Intertek Inspection Services UK Ltd" },
                                    PayrollType=new DbModel.Data{ Id=2898,Name="LTD/VAT/SE" },
                                    ExportPrefix=new DbModel.Data{ Id=3169,Name="AIM", MasterDataTypeId=77},
                            } ,
                new DbModel.CompanyPayroll { Id=67,CompanyId=116,PayrollTypeId=2899,Currency="USD",LastModification=null,UpdateCount=null,ModifiedBy=null,
                                    Company=new DbModel.Company { Id=116,Code="UK050",Name ="UK - Intertek Inspection Services UK Ltd" },
                                    PayrollType=new DbModel.Data{ Id=2899,Name="PAYE Monthly" },
                                    ExportPrefix= new DbModel.Data{ Id=3180,Name="C&T" , MasterDataTypeId=77},
                            } ,
                new DbModel.CompanyPayroll { Id=69,CompanyId=1,PayrollTypeId=2898,Currency="USD",LastModification=null,UpdateCount=null,ModifiedBy=null,
                                    Company=new DbModel.Company { Id=1,Code="DZ",Name ="Algeria MI" },
                                    PayrollType=new DbModel.Data{ Id=2898,Name="LTD/VAT/SE" },
                                    ExportPrefix=new DbModel.Data{ Id=3279,Name="TIS Staff", MasterDataTypeId=77 }
                            }
            }.AsQueryable();
        }

        public static Mock<DbSet<DbModel.CompanyPayroll>> GetCompanyPayrollMockDbSet(IQueryable<DbModel.CompanyPayroll> data)
        {
            var mockSet = new Mock<DbSet<DbModel.CompanyPayroll>>();
            mockSet.As<IQueryable<DbModel.CompanyPayroll>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<DbModel.CompanyPayroll>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<DbModel.CompanyPayroll>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<DbModel.CompanyPayroll>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            return mockSet;
        }

        #endregion

        #region CompanyQualification

        public static IQueryable<DbModel.CompanyQualificationType> GetCompanyQualificationMockData()
        {
            return new List<DbModel.CompanyQualificationType>
            {
                new DbModel.CompanyQualificationType { Id=1,CompanyId=116,QualificationTypeId=325 ,LastModification=null,UpdateCount=null,ModifiedBy=null,
                                    Company=new DbModel.Company { Id=116,Code="UK050",Name ="UK - Intertek Inspection Services UK Ltd" },
                                    QualificationType=new DbModel.Data{Id=325,Name="City & Guild Craft - Mechnical" }
                 },
                new DbModel.CompanyQualificationType { Id=2,CompanyId=116,QualificationTypeId=328 ,LastModification=null,UpdateCount=null,ModifiedBy=null,
                                    Company=new DbModel.Company { Id=116,Code="UK050",Name ="UK - Intertek Inspection Services UK Ltd" },
                                    QualificationType=new DbModel.Data{Id=328,Name="City & Guild Technician - Mechanical" }
                 },
                new DbModel.CompanyQualificationType { Id=3,CompanyId=116,QualificationTypeId=597 ,LastModification=null,UpdateCount=null,ModifiedBy=null,
                                    Company=new DbModel.Company { Id=116,Code="UK050",Name ="UK - Intertek Inspection Services UK Ltd" },
                                    QualificationType=new DbModel.Data{Id=597,Name="HNC / HND - Mechanical" }
                 },
                new DbModel.CompanyQualificationType { Id=4,CompanyId=116,QualificationTypeId=480 ,LastModification=null,UpdateCount=null,ModifiedBy=null,
                                    Company=new DbModel.Company { Id=116,Code="UK050",Name ="UK - Intertek Inspection Services UK Ltd" },
                                    QualificationType=new DbModel.Data{Id=480,Name="ONC / OND - Mechnical" }
                 },
            }.AsQueryable();
        }

        public static Mock<DbSet<DbModel.CompanyQualificationType>> GetCompanyQualificationMockDbSet(IQueryable<DbModel.CompanyQualificationType> data)
        {
            var mockSet = new Mock<DbSet<DbModel.CompanyQualificationType>>();
            mockSet.As<IQueryable<DbModel.CompanyQualificationType>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<DbModel.CompanyQualificationType>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<DbModel.CompanyQualificationType>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<DbModel.CompanyQualificationType>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            return mockSet;
        }
        #endregion

        public static IQueryable<DbModel.Data> GetPayrollTypeMockData()
        {
            return new List<DbModel.Data>
            {
                new DbModel.Data{ Id=2898,Name="LTD/VAT/SE" , MasterDataTypeId=31},
               new DbModel.Data{ Id=2899,Name="PAYE Monthly", MasterDataTypeId=31 },
              new DbModel.Data{ Id=2900,Name="PAYE Weekly", MasterDataTypeId=31 },
              new DbModel.Data{ Id=2897,Name="Ltd Monthly TS", MasterDataTypeId=31 }
            }.AsQueryable();
        }

        public static IQueryable<DbModel.Data> GetExportPrefixMockData()
        {
            return new List<DbModel.Data>
            {
                new DbModel.Data{ Id=3169,Name="AIM", MasterDataTypeId=77},
               new DbModel.Data{ Id=3180,Name="C&T" , MasterDataTypeId=77},
              new DbModel.Data{ Id=3279,Name="TIS Staff", MasterDataTypeId=77 },
               new DbModel.Data{ Id=3171,Name="Amelia", MasterDataTypeId=77 }
            }.AsQueryable();
        }

        public static IQueryable<DbModel.Data> GetDivisionMockData()
        {
            return new List<DbModel.Data>
            {
                new DbModel.Data{ Id=2304,Code="116",Name="Inspection", MasterDataTypeId=32},
               new DbModel.Data{ Id=2302,Code="116",Name="PSO" , MasterDataTypeId=32},
              new DbModel.Data{ Id=2303,Code="115",Name="Technical Training", MasterDataTypeId=32}
            }.AsQueryable();
        }

        public static IQueryable<DbModel.Data> GetMarginTypeMockData()
        {
            return new List<DbModel.Data>
            {
                new DbModel.Data { Id =3167, Name="TSS (Technical Staffing Services)" , MasterDataTypeId=33 },
                new DbModel.Data { Id =3161, Name="AIM (Asset Integrity Management)" , MasterDataTypeId=33},
                new DbModel.Data { Id =3166, Name="TIS (Technical Inspection Services)", MasterDataTypeId=33 },
                new DbModel.Data { Id =3167, Name="TSS (Technical Staffing Services)" , MasterDataTypeId=33},
                 new DbModel.Data { Id =3164, Name="NDT (Non Destructive Testing)" , MasterDataTypeId=33}
            }.AsQueryable();
        }

        public static IQueryable<DbModel.Tax> GetTaxTypeMockData()
        {
            return new List<DbModel.Tax>
            {
                    new DbModel.Tax { Id=643,Code="VAT",Name="VAT - 17.5%",Rate=17.5000M,TaxType="S",IsIcinv=null,IsActive=true,LastModification=DateTime.UtcNow,UpdateCount=null,ModifiedBy=null},
                     new DbModel.Tax { Id=654,Code="VAT",Name="VAT - Exempt",Rate=0.0000M,TaxType="S",IsIcinv=null,IsActive=true,LastModification=DateTime.UtcNow,UpdateCount=null,ModifiedBy=null},
                     new DbModel.Tax { Id=516,Code="GST  ",Name="GST",Rate=5.0000M,TaxType="S",IsIcinv=null,IsActive=true,LastModification=DateTime.UtcNow,UpdateCount=null,ModifiedBy=null},
                     new DbModel.Tax { Id=639,Code="VAT",Name="VAT - 12.5%",Rate=12.5000M,TaxType="S",IsIcinv=null,IsActive=true,LastModification=DateTime.UtcNow,UpdateCount=null,ModifiedBy=null}

            }.AsQueryable();

        }
        
    }
}
