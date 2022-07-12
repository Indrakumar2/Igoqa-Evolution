using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DbModel = Evolution.DbRepository.Models;

namespace Evolution.UnitTests.Mocks.Data.Supplier.Db
{
    public static class MockSupplier
    {
        #region Suppllier
        public static IQueryable<DbModel.Supplier> GetSupplierMockedData()
        {
            return new List<DbModel.Supplier>()
            {
                new DbModel.Supplier()
                {
                    Id=1,SupplierName="CAMERON FRANCE",Address="CAMERON FRANCE",CityId=1,
                    City =new DbModel.City(){Id=1,CountyId=1,Name="Brighton"},
                    PostalCode ="00123",IsActive=true,LastModification=DateTime.UtcNow,UpdateCount=0,ModifiedBy=null

                },
                new DbModel.Supplier()
                {
                    Id=2,SupplierName="Dresser Produits Industriels",Address="Dresser Produits Industriels ,Italy",CityId=2,
                    City =new DbModel.City(){Id=2,CountyId=2,Name="Houston"},
                    PostalCode ="001112",IsActive=false,LastModification=DateTime.UtcNow,UpdateCount=0,ModifiedBy=null,
                    SupplierPurchaseOrder= new List<DbModel.SupplierPurchaseOrder>(){ new DbModel.SupplierPurchaseOrder() { SupplierId=2,SupplierPonumber="1",Status="C"} }
                }
            }.AsQueryable();
        }
        public static Mock<DbSet<DbModel.Supplier>> GetSupplierMockedData(IQueryable<DbModel.Supplier> data)
        {
            var mockSet = new Mock<DbSet<DbModel.Supplier>>();
            mockSet.As<IQueryable<DbModel.Supplier>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<DbModel.Supplier>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<DbModel.Supplier>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<DbModel.Supplier>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            return mockSet;
        }
        #endregion

        #region SupplierDocument
        public static IQueryable<DbModel.SupplierDocument> GetSupplierDocumentsMockedData()
        {
            return new List<DbModel.SupplierDocument>()
            {
                new DbModel.SupplierDocument()
                {
                    Id=1,SupplierId=1,Supplier=new DbModel.Supplier(){Id=1,SupplierName="CAMERON FRANCE"},
                    Name="Document",DocumentType="Email",DocumentSize=100,UploadedOn=DateTime.UtcNow,IsInProgress=true,
                    IsvisibleToCustomer=true,IsVisibleToTechnicalSpecialist=true,LastModification=DateTime.UtcNow,UpdateCount=0,
                    ModifiedBy=null,UploadedDataId=null
                },
                new DbModel.SupplierDocument()
                {
                    Id=2,SupplierId=2,Supplier=new DbModel.Supplier(){Id=2,SupplierName="Dresser Produits Industriels"},
                    Name="Document Report",DocumentType="Genral",DocumentSize=120,UploadedOn=DateTime.UtcNow,IsInProgress=true,
                    IsvisibleToCustomer=true,IsVisibleToTechnicalSpecialist=true,LastModification=DateTime.UtcNow,UpdateCount=0,
                    ModifiedBy=null,UploadedDataId=null
                }

            }.AsQueryable();
        }
        public static Mock<DbSet<DbModel.SupplierDocument>> GetSupplierMockedData(IQueryable<DbModel.SupplierDocument> data)
        {
            var mockSet = new Mock<DbSet<DbModel.SupplierDocument>>();
            mockSet.As<IQueryable<DbModel.SupplierDocument>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<DbModel.SupplierDocument>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<DbModel.SupplierDocument>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<DbModel.SupplierDocument>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            return mockSet;
        }
        #endregion

        #region SupplierContact
        public static IQueryable<DbModel.SupplierContact> GetSupplierContactMockedData()
        {
            return new List<DbModel.SupplierContact>()
            {
                new DbModel.SupplierContact()
                {
                    Id=1,SupplierId=1,Supplier=new DbModel.Supplier(){Id=1,SupplierName="CAMERON FRANCE"},
                    SupplierContactName="John Cleary",TelephoneNumber="0132456",FaxNumber="12345",MobileNumber="123456",
                    EmailId="Jean-Luc.Llorca@c-a-m.com",OtherContactDetails=null,LastModification=DateTime.UtcNow,UpdateCount=0,ModifiedBy=null
                },
                new DbModel.SupplierContact()
                {
                    Id=2,SupplierId=2,Supplier=new DbModel.Supplier(){Id=2,SupplierName="Dresser Produits Industriels"},
                    SupplierContactName="Sofia Redford",TelephoneNumber="01324567",FaxNumber="123456",MobileNumber="1234567",
                    EmailId="john.cleary@lff.co.uk",OtherContactDetails=null,LastModification=DateTime.UtcNow,UpdateCount=0,ModifiedBy=null
                }
            }.AsQueryable();
        }
        public static Mock<DbSet<DbModel.SupplierContact>> GetSupplierContactMockedData(IQueryable<DbModel.SupplierContact> data)
        {
            var mockSet = new Mock<DbSet<DbModel.SupplierContact>>();
            mockSet.As<IQueryable<DbModel.SupplierContact>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<DbModel.SupplierContact>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<DbModel.SupplierContact>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<DbModel.SupplierContact>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            return mockSet;
        }
        #endregion

        #region SupplierNote
        public static IQueryable<DbModel.SupplierNote> GetSupplierNoteMockedData()
        {
            return new List<DbModel.SupplierNote>()
            {
                new DbModel.SupplierNote()
                {
                    Id=1,SupplierId=1,Supplier=new DbModel.Supplier(){Id=1,SupplierName="CAMERON FRANCE"},CreatedDate=DateTime.UtcNow,
                    CreatedBy="M.Peacock",Note="Notes",LastModification=DateTime.UtcNow,ModifiedBy=null,UpdateCount=0
                },
                new DbModel.SupplierNote()
                {
                    Id=2,SupplierId=2,Supplier=new DbModel.Supplier(){Id=2,SupplierName="Dresser Produits Industriels"},CreatedDate=DateTime.UtcNow,
                    CreatedBy="M.Peacock",Note="Notes Reports",LastModification=DateTime.UtcNow,ModifiedBy=null,UpdateCount=0
                }
              }.AsQueryable();
        }
        public static Mock<DbSet<DbModel.SupplierNote>> GetSupplierNoteMockedData(IQueryable<DbModel.SupplierNote> data)
        {
            var mockSet = new Mock<DbSet<DbModel.SupplierNote>>();
            mockSet.As<IQueryable<DbModel.SupplierNote>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<DbModel.SupplierNote>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<DbModel.SupplierNote>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<DbModel.SupplierNote>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            return mockSet;
        }
        #endregion
    }
}
