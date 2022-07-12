using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DbModel = Evolution.DbRepository.Models;

namespace Evolution.UnitTests.Mocks.Data.SupplierPO.Db
{
    public static class MockSupplierPODBData
    {
        #region SupplierPO
        public static IQueryable<DbModel.SupplierPurchaseOrder> GetSupplierPOMockedData()
        {
            return new List<DbModel.SupplierPurchaseOrder>()
            {
                new DbModel.SupplierPurchaseOrder()
                {
                    Id=1,Project=new DbModel.Project(){Id=1,ProjectNumber=1,},ProjectId=1,SupplierPonumber="TOUK/05/00323",
                    MaterialDescription ="Xmas Trees",DeliveryDate=DateTime.UtcNow,Status="O",BudgetValue=0.01M,BudgetWarning=12,BudgetHoursUnit=0.012M,
                    BudgetHoursUnitWarning=12,SupplierId=1,LastModification=DateTime.UtcNow,ModifiedBy="test",UpdateCount=0
                },
                new DbModel.SupplierPurchaseOrder()
                {
                    Id=2,Project=new DbModel.Project(){Id=2,ProjectNumber=2,},ProjectId=2,SupplierPonumber="TOUK/08/00324",
                    MaterialDescription ="JT Valves",DeliveryDate=DateTime.UtcNow,Status="O",BudgetValue=0.02M,BudgetWarning=15,BudgetHoursUnit=0.2M,
                    BudgetHoursUnitWarning=15,SupplierId=2,LastModification=DateTime.UtcNow,ModifiedBy="test",UpdateCount=0
                }
            }.AsQueryable();

          
        }
        public static Mock<DbSet<DbModel.SupplierPurchaseOrder>> GetSupplierPOMockedData(IQueryable<DbModel.SupplierPurchaseOrder> data)
        {
            var mockSet = new Mock<DbSet<DbModel.SupplierPurchaseOrder>>();
            mockSet.As<IQueryable<DbModel.SupplierPurchaseOrder>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<DbModel.SupplierPurchaseOrder>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<DbModel.SupplierPurchaseOrder>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<DbModel.SupplierPurchaseOrder>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            return mockSet;
        }
        #endregion

        #region SupplierPONote
        public static IQueryable<DbModel.SupplierPurchaseOrderNote> GetSupplierPONoteMockedData()
        {
            return new List<DbModel.SupplierPurchaseOrderNote>()
            {
                new DbModel.SupplierPurchaseOrderNote()
                {
                    Id=1,SupplierPurchaseOrderId=1,CreatedDate=DateTime.UtcNow,CreatedBy="M.Peacock",Note="PO to follow",
                    LastModification =DateTime.UtcNow,UpdateCount=0,ModifiedBy="test"
                },
                new DbModel.SupplierPurchaseOrderNote()
                {
                    Id=2,SupplierPurchaseOrderId=2,CreatedDate=DateTime.UtcNow,CreatedBy="M.Peacock",Note="380 Man hours budget",
                    LastModification =DateTime.UtcNow,UpdateCount=0,ModifiedBy="test"
                }
            }.AsQueryable();
        }
        public static Mock<DbSet<DbModel.SupplierPurchaseOrderNote>> GetSupplierPONoteMockedData(IQueryable<DbModel.SupplierPurchaseOrderNote> data)
        {
            var mockSet = new Mock<DbSet<DbModel.SupplierPurchaseOrderNote>>();
            mockSet.As<IQueryable<DbModel.SupplierPurchaseOrderNote>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<DbModel.SupplierPurchaseOrderNote>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<DbModel.SupplierPurchaseOrderNote>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<DbModel.SupplierPurchaseOrderNote>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            return mockSet;
        }
        #endregion

        #region SupplierSubSupplier
        public static IQueryable<DbModel.SupplierPurchaseOrderSubSupplier> GetSupplierPOSubSupplierMockedData()
        {
            return new List<DbModel.SupplierPurchaseOrderSubSupplier>()
            {
                  new DbModel.SupplierPurchaseOrderSubSupplier()
                  {
                      Id=1,SupplierPurchaseOrderId=1,SupplierPurchaseOrder=new DbModel.SupplierPurchaseOrder(){Id=1,ProjectId=1},SupplierId=1,
                      Supplier = new DbModel.Supplier()
                      {
                          Id=1,SupplierName="CAMERON FRANCE",City=new DbModel.City(){Name="Brighton",Id=1}
                      }
                  },

                  new DbModel.SupplierPurchaseOrderSubSupplier()
                  {
                      Id=2,SupplierPurchaseOrderId=2,SupplierPurchaseOrder=new DbModel.SupplierPurchaseOrder(){Id=2,ProjectId=2},SupplierId=2,
                      Supplier = new DbModel.Supplier()
                      {
                          Id=2,SupplierName="Dresser Produits Industriels",City=new DbModel.City(){Name="Houston",Id=2}
                      }
                  }
            }.AsQueryable();
        }
        public static Mock<DbSet<DbModel.SupplierPurchaseOrderSubSupplier>> GetSupplierPOSubSupplierMockedData(IQueryable<DbModel.SupplierPurchaseOrderSubSupplier> data)
        {
            var mockSet = new Mock<DbSet<DbModel.SupplierPurchaseOrderSubSupplier>>();
            mockSet.As<IQueryable<DbModel.SupplierPurchaseOrderSubSupplier>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<DbModel.SupplierPurchaseOrderSubSupplier>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<DbModel.SupplierPurchaseOrderSubSupplier>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<DbModel.SupplierPurchaseOrderSubSupplier>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            return mockSet;
        }
        #endregion
    }
}
