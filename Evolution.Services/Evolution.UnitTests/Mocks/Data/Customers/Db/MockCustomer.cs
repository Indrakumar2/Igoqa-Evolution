using DbModel = Evolution.DbRepository.Models;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Evolution.UnitTest.Mocks.Data.Customers.Db
{
    public static class MockCustomer
    {
        public static IQueryable<DbModel.Customer> GetCustomerMockData()
        {
            return new List<DbModel.Customer>
            {
                new DbModel.Customer { Id=1,Code="TU03659",Name="ABB",ParentName="ABB",Miiwaid=1,MiiwaparentId=0,LastModification=DateTime.UtcNow,IsActive=true,UpdateCount=0,ModifiedBy=null,
                 CustomerAddress =new  List< DbModel.CustomerAddress>{
                                      new DbModel.CustomerAddress {
                                         Id=1, CustomerId=1, Address ="Tullow Group Services Ltd.",CityId =5819,PostalCode="150-8512",VatTaxRegistrationNo=null,LastModification= DateTime.Now
                                     },
                                     new DbModel.CustomerAddress {
                                         Id=2, CustomerId=2, Address ="ABB FRANCE  ATPA-AXB  ZA Des Combaruches  73100 AIX-Les-Bains  ",CityId =1321,PostalCode="73100",VatTaxRegistrationNo="FR 95 335 C146 312",LastModification= DateTime.Now
                                     },
                 },
                },
                new DbModel.Customer { Id=2,Code="AB00007",Name="SUBSEA 7",ParentName="SUBSEA 7",Miiwaid=22412,MiiwaparentId=0,LastModification=DateTime.UtcNow,IsActive=true,UpdateCount=0,ModifiedBy=null,
                CustomerAddress =new  List< DbModel.CustomerAddress>{
                                      new DbModel.CustomerAddress {
                                         Id=1, CustomerId=2, Address ="Subsea 7 Ltd  Greenwell Base  Greenwell Road  Aberdeen  AB12 3AX",CityId =80,PostalCode="AB12 3AX",VatTaxRegistrationNo=null,LastModification= DateTime.Now,
                                          CustomerContact =new List<DbModel.CustomerContact> {
                                              new DbModel.CustomerContact { ContactName="Karen Innes" }
                                          }
                                     },
                                     new DbModel.CustomerAddress {
                                         Id=2, CustomerId=2402, Address ="Subsea 7 Ltd  Peregrine Road  Westhill Business Park  Westhill  Aberdeenshire  AB32 6JL",CityId =80,PostalCode="73100",VatTaxRegistrationNo="FR 95 335 C146 312",LastModification= DateTime.Now,
                                          CustomerContact =new List<DbModel.CustomerContact> {
                                              new DbModel.CustomerContact { ContactName="ELC Process Support Team Accounts" }
                                          }
                                     },
                 },
                },
                new DbModel.Customer { Id=3,Code="AB00003",Name="ABB Elektrik Sanayi",ParentName="ABB",Miiwaid=3,MiiwaparentId=1,LastModification=DateTime.UtcNow,IsActive=true,UpdateCount=1,ModifiedBy="Jenna Blyth" },
                new DbModel.Customer { Id=4,Code="AB00004",Name="AB Joy",ParentName="ABB",Miiwaid=4,MiiwaparentId=1,LastModification=DateTime.UtcNow,IsActive=true,UpdateCount=1,ModifiedBy="Jenna Blyth" }
            }.AsQueryable();

        }

        public static Mock<DbSet<DbModel.Customer>> GetCustomerMockDbSet(IQueryable<DbModel.Customer> data)
        {
            var mockSet = new Mock<DbSet<DbModel.Customer>>();
            mockSet.As<IQueryable<DbModel.Customer>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<DbModel.Customer>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<DbModel.Customer>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<DbModel.Customer>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            return mockSet;
        }
        public static IQueryable<DbModel.CustomerContact> GetCustomerContactMockData()
        {
            return new List<DbModel.CustomerContact>
            {
                new DbModel.CustomerContact()
                {
                    Id=1,CustomerAddressId=1,CustomerAddress=new DbModel.CustomerAddress(){Id=1,CustomerId=1,Address="Tullow Group Services Ltd.",Customer=new DbModel.Customer(){Id=1,Code="TU03659"} },
                                              Position="Commercial Representative",Salutation="Mr.",ContactName="Manfred Hoffman",LastModification=DateTime.UtcNow,ModifiedBy="test",UpdateCount=0,
                                            ProjectClientNotification=new List<DbModel.ProjectClientNotification>
                                            {
                                                new DbModel.ProjectClientNotification(){Id=1,CustomerContactId=1,}
                                            }                  
                                           
                                            
                },
                 new DbModel.CustomerContact{ Id=2,CustomerAddressId=2,CustomerAddress=new DbModel.CustomerAddress(){Id=2,CustomerId=2,Address="Subsea 7 Ltd  Greenwell Base",Customer=new DbModel.Customer(){Id=2,Code="AB00007"} } ,
                                              Position="HR Department",Salutation="Attention :",ContactName="Layla Gill",LastModification=DateTime.UtcNow,ModifiedBy="test",UpdateCount=0,
                                               ProjectClientNotification=new List<DbModel.ProjectClientNotification>
                                               {
                                                new DbModel.ProjectClientNotification(){Id=2,CustomerContactId=2,}
                                               }

                }
            }.AsQueryable();
        }
        public static Mock<DbSet<DbModel.CustomerContact>> GetCustomerMockDbSet(IQueryable<DbModel.CustomerContact> data)
        {
            var mockSet = new Mock<DbSet<DbModel.CustomerContact>>();
            mockSet.As<IQueryable<DbModel.CustomerContact>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<DbModel.CustomerContact>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<DbModel.CustomerContact>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<DbModel.CustomerContact>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            return mockSet;
        }

        public static IQueryable<DbModel.CustomerAddress> GetCustomerAddressMockData()
        {
            return new List<DbModel.CustomerAddress>
            {
                new DbModel.CustomerAddress()
                {
                    Id=1,CustomerId=1,Address="Tullow Group Services Ltd.",CustomerContact=new List<DbModel.CustomerContact>
                    {
                        new DbModel.CustomerContact{Id=1,CustomerAddressId=1,ContactName="Manfred Hoffman"}
                    },Customer=new DbModel.Customer(){Id=1,Code="TU03659"}
                }
            }.AsQueryable();
       }
        public static Mock<DbSet<DbModel.CustomerAddress>> GetCustomerMockDbSet(IQueryable<DbModel.CustomerAddress> data)
        {
            var mockSet = new Mock<DbSet<DbModel.CustomerAddress>>();
            mockSet.As<IQueryable<DbModel.CustomerAddress>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<DbModel.CustomerAddress>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<DbModel.CustomerAddress>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<DbModel.CustomerAddress>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            return mockSet;
        }

    }
}
