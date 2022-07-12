using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DbModel = Evolution.DbRepository.Models;

namespace Evolution.UnitTests.Mocks.Data.SupplierPO.Db
{
    public class MockDbSupplierPO
    {

        #region SupplierPO

        public static Mock<DbSet<DbModel.SupplierPurchaseOrder>> GetSupplierPOMockData(IQueryable<DbModel.SupplierPurchaseOrder> data)
        {
            var mockSet = new Mock<DbSet<DbModel.SupplierPurchaseOrder>>();
            mockSet.As<IQueryable<DbModel.SupplierPurchaseOrder>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<DbModel.SupplierPurchaseOrder>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<DbModel.SupplierPurchaseOrder>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<DbModel.SupplierPurchaseOrder>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());
            return mockSet;
        }

        public static IQueryable<DbModel.SupplierPurchaseOrder> GetSupplierPOMockDbData()
        {
            return new List<DbModel.SupplierPurchaseOrder>()
            {
                new DbModel.SupplierPurchaseOrder()
                {
                    BudgetValue = 10.000M,BudgetHoursUnit = 10.000M,BudgetWarning = 78,BudgetHoursUnitWarning = 75,CompletionDate = DateTime.Now.AddDays(30),DeliveryDate = DateTime.Now.AddDays(30),
                    Id = 1,LastModification = DateTime.Now,MaterialDescription = "New Material Description",ModifiedBy = "Gordan Mccallum",ProjectId = 1,SupplierId=1,SupplierPonumber = "New Supplier PO Number 1",UpdateCount = 1,
                    Project = new DbModel.Project(){ ProjectNumber = 1,Id =1 ,Contract = new DbModel.Contract(){ ContractNumber = "1",Customer = new DbModel.Customer(){ Code ="CSDSDS",Name="New Customer"} },CustomerProjectName="New Customer ProjectName",CustomerProjectNumber = "CustomerProjectNumber" }, Status = "o",Assignment = new List<DbModel.Assignment>(){ new DbModel.Assignment() { Id= 1, AssignmentNumber = 1} },Supplier = new DbModel.Supplier(){ Id = 1,SupplierName ="New Supplier"},
                },
                new DbModel.SupplierPurchaseOrder()
                {
                    BudgetHoursUnit = 10.000M,BudgetHoursUnitWarning = 75,BudgetValue = 10.000M,BudgetWarning = 78,CompletionDate = DateTime.Now.AddDays(30),DeliveryDate = DateTime.Now.AddDays(30),
                    Id = 2,LastModification = DateTime.Now,MaterialDescription = "New Material Description 2",ModifiedBy = "Gordan Mccallum",ProjectId = 1,SupplierId=1,SupplierPonumber = "New Supplier PO Number 1",UpdateCount = 1,
                    Project = new DbModel.Project(){ ProjectNumber = 1,Id =1 ,Contract = new DbModel.Contract(){ ContractNumber = "1",Customer = new DbModel.Customer(){ Code ="CSDSDS",Name="New Customer"} },CustomerProjectName="New Customer ProjectName",CustomerProjectNumber = "CustomerProjectNumber" }, Status = "o",Assignment = new List<DbModel.Assignment>(){ new DbModel.Assignment() { Id= 1, AssignmentNumber = 1} },Supplier = new DbModel.Supplier(){ Id = 1,SupplierName ="New Supplier" }
                }
            }.AsQueryable();
        }

        #endregion

        #region SupplierPO Sub Supplier

        public static Mock<DbSet<DbModel.SupplierPurchaseOrderSubSupplier>> GetSubSupplierMockedDbSet(IQueryable<DbModel.SupplierPurchaseOrderSubSupplier> data)
        {
            var mockSet = new Mock<DbSet<DbModel.SupplierPurchaseOrderSubSupplier>>();
            mockSet.As<IQueryable<DbModel.SupplierPurchaseOrderSubSupplier>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<DbModel.SupplierPurchaseOrderSubSupplier>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<DbModel.SupplierPurchaseOrderSubSupplier>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<DbModel.SupplierPurchaseOrderSubSupplier>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());
            return mockSet;
        }

        public static IQueryable<DbModel.SupplierPurchaseOrderSubSupplier> GetSubSupplierMockData()
        {
            return new List<DbModel.SupplierPurchaseOrderSubSupplier>()
            {
                new DbModel.SupplierPurchaseOrderSubSupplier()
                {
                    Id = 1, LastModification = DateTime.Now,  ModifiedBy = "gordan Mccallum", SupplierId = 1,SupplierPurchaseOrderId = 1,UpdateCount = 1,SupplierPurchaseOrder= new DbModel.SupplierPurchaseOrder(){ SupplierPonumber = "NEw Supplier PO number ",Id = 1,Supplier = new DbModel.Supplier(){ SupplierName = "Main Supplier 1", Id = 1}},Supplier = new DbModel.Supplier(){ SupplierName = "Main Supplier 1", Id = 1}
                },
                new DbModel.SupplierPurchaseOrderSubSupplier()
                {
                    Id = 2, LastModification = DateTime.Now,  ModifiedBy = "gordan Mccallum", SupplierId = 2,SupplierPurchaseOrderId = 1,UpdateCount = 1,SupplierPurchaseOrder= new DbModel.SupplierPurchaseOrder(){ SupplierPonumber = "NEw Supplier PO number ",Id = 1,Supplier = new DbModel.Supplier(){ SupplierName = "Main Supplier 1", Id = 1}},Supplier = new DbModel.Supplier(){ SupplierName = "Main Supplier 1", Id = 1}

                }
            }.AsQueryable();
        }

        #endregion

        #region SupplierPODocuments

        public static IQueryable<DbModel.SupplierPurchaseOrderDocument> GetSupplierPODdocumentsMockData()
        {
            return new List<DbModel.SupplierPurchaseOrderDocument>()
            {
                new DbModel.SupplierPurchaseOrderDocument()
                {
                    DocumentSize = 100,DocumentStatus ="U",DocumentType = "Email",Id = 1,IsInProgress = false,IsVisibleToCustomer = true, IsVisibleToTechnicalSpecialist = false,LastModification = DateTime.Now,ModifiedBy = "Gordan Mccallum",Name ="Supplier PO Document 1",SupplierPurchaseOrderId = 1,UpdateCount = 1,UploadedDataId =" Uploaded Data Id",UploadedOn = DateTime.Now.AddDays(-1)
                },
                new DbModel.SupplierPurchaseOrderDocument()
                {
                    DocumentSize = 100,DocumentStatus ="U",DocumentType = "Email",Id = 2,IsInProgress = false,IsVisibleToCustomer = true, IsVisibleToTechnicalSpecialist = false,LastModification = DateTime.Now,ModifiedBy = "Gordan Mccallum",Name ="Supplier PO Document 2",SupplierPurchaseOrderId = 1,UpdateCount = 1,UploadedDataId =" Uploaded Data Id",UploadedOn = DateTime.Now.AddDays(-1)
                }
            }.AsQueryable();
        }

        #endregion

        #region SupplierPONotes

        public static Mock<DbSet<DbModel.SupplierPurchaseOrderNote>> GetSupplierPONotesMockData(IQueryable<DbModel.SupplierPurchaseOrderNote> data)
        {
            var mockSet = new Mock<DbSet<DbModel.SupplierPurchaseOrderNote>>();
            mockSet.As<IQueryable<DbModel.SupplierPurchaseOrderNote>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<DbModel.SupplierPurchaseOrderNote>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<DbModel.SupplierPurchaseOrderNote>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<DbModel.SupplierPurchaseOrderNote>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());
            return mockSet;
        }

        public static IQueryable<DbModel.SupplierPurchaseOrderNote> GetSupplierPONotesMockData()
        {
            return new List<DbModel.SupplierPurchaseOrderNote>()
            {
                new DbModel.SupplierPurchaseOrderNote()
                {
                    CreatedBy = "Gordan Mccallum",CreatedDate = DateTime.Now.AddDays(-2),Id = 1,LastModification = DateTime.Now,ModifiedBy = "Gordan Mccallum",Note ="New Note 1",SupplierPurchaseOrderId = 1,UpdateCount = 1,SupplierPurchaseOrder = new DbModel.SupplierPurchaseOrder(){ Id= 1, SupplierPonumber ="New Supplier PO Number"}
                },
                new DbModel.SupplierPurchaseOrderNote()
                {
                    CreatedBy = "Gordan Mccallum",CreatedDate = DateTime.Now.AddDays(-2),Id = 2,LastModification = DateTime.Now,ModifiedBy = "Gordan Mccallum",Note ="New Note 2",SupplierPurchaseOrderId = 1,UpdateCount = 1,SupplierPurchaseOrder = new DbModel.SupplierPurchaseOrder(){ Id= 1, SupplierPonumber ="New Supplier PO Number"}
                }
            }.AsQueryable();
        }

        #endregion

       
    }
}
