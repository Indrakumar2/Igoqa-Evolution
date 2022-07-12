using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DomainModel = Evolution.SupplierPO.Domain.Models.SupplierPO;

namespace Evolution.UnitTests.Mocks.Data.SupplierPO.Domain
{
    public class MockDomainSupplierPO
    {
        #region SupplierPO

        public static IList<DomainModel.SupplierPO> GetSupplierPoMockedDomainData()
        {
            return new List<DomainModel.SupplierPO>()
            {
                new DomainModel.SupplierPO()
                {
                    LastModification = DateTime.Now, ModifiedBy ="Gordan mccallum",SupplierPOBudget =70.00M,SupplierPOBudgetHours = 10.00M,SupplierPOBudgetHoursInvoicedToDate = 100,SupplierPOBudgetHoursUnInvoicedToDate = 100,SupplierPOBudgetHoursWarning = 75,
                    SupplierPOBudgetInvoicedToDate = 100, SupplierPOBudgetUninvoicedToDate = 100,SupplierPOBudgetWarning = 75,SupplierPOCompletedDate = DateTime.Now.AddDays(30),SupplierPOContractNumber = "CZD0001/9090",SupplierPOCustomerCode ="CZD0001",SupplierPOCustomerName ="New Customer",
                    SupplierPOCustomerProjectName ="New Customer ProjectName", SupplierPOCustomerProjectNumber = "New Customer Project number",SupplierPODeliveryDate = DateTime.Now.AddDays(30),SupplierPOId = 1,SupplierPOMainSupplierId=1,
                    SupplierPOMainSupplierName = "new Supplier name ",SupplierPONumber = "New supplier Purchase order",SupplierPOMaterialDescription = "New Supplier PO Material Description",SupplierPOProjectNumber =1,SupplierPOStatus = "C",UpdateCount =1
                },
                new DomainModel.SupplierPO()
                {
                     LastModification = DateTime.Now, ModifiedBy ="Gordan mccallum",SupplierPOBudget =70.00M,SupplierPOBudgetHours = 10.00M,SupplierPOBudgetHoursInvoicedToDate = 100,SupplierPOBudgetHoursUnInvoicedToDate = 100,SupplierPOBudgetHoursWarning = 75,
                    SupplierPOBudgetInvoicedToDate = 100, SupplierPOBudgetUninvoicedToDate = 100,SupplierPOBudgetWarning = 75,SupplierPOCompletedDate = DateTime.Now.AddDays(30),SupplierPOContractNumber = "CZD0001/9090",SupplierPOCustomerCode ="CZD0001",SupplierPOCustomerName ="New Customer",
                    SupplierPOCustomerProjectName ="New Customer ProjectName", SupplierPOCustomerProjectNumber = "New Customer Project number",SupplierPODeliveryDate = DateTime.Now.AddDays(30),SupplierPOId = 2,SupplierPOMainSupplierId=1,
                    SupplierPOMainSupplierName = "new Supplier name ",SupplierPONumber = "New supplier Purchase order",SupplierPOMaterialDescription = "New Supplier PO Material Description",SupplierPOProjectNumber =1,SupplierPOStatus = "C",UpdateCount =1
                }
            };
        }

        #endregion

        #region Supplier PO Sub Supplier 

        public static IList<DomainModel.SupplierPOSubSupplier> GetSubSupplierMockedData()
        {
            return new List<DomainModel.SupplierPOSubSupplier>()
            {
                new DomainModel.SupplierPOSubSupplier()
                {
                    LastModification = DateTime.Now,ModifiedBy = "Gordan Mccallum",SubSupplierAddress = "1",SubSupplierId = 1,SubSupplierName = "New Sub Supplier",SupplierId= 1,SupplierPOId = 1,SupplierPONumber ="new Supplier Po number",UpdateCount = 1
                },
                new DomainModel.SupplierPOSubSupplier()
                {
                    LastModification = DateTime.Now,ModifiedBy = "Gordan Mccallum",SubSupplierAddress = "1",SubSupplierId = 1,SubSupplierName = "New Sub Supplier 2",SupplierId= 2,SupplierPOId = 1,SupplierPONumber ="new Supplier Po number",UpdateCount = 1
                }
            };
        }

        #endregion


        #region Supplier PO Notes

        public static IList<DomainModel.SupplierPONote> GetSupplierPONotesMockedData()
        {
            return new List<DomainModel.SupplierPONote>()
            {
                new DomainModel.SupplierPONote()
                {
                    CreatedBy = "gordan Mccallum",CreatedOn = DateTime.Now.AddDays(-2),LastModification = DateTime.Now,ModifiedBy = "gordan Mccallum",Notes ="New supplier Po Notes 1",SupplierPOId = 1,SupplierPONoteId = 1,SupplierPONumber = "New Supplier PO Number ",UpdateCount = 1
                },
                new DomainModel.SupplierPONote()
                {
                    CreatedBy = "gordan Mccallum",CreatedOn = DateTime.Now.AddDays(-2),LastModification = DateTime.Now,ModifiedBy = "gordan Mccallum",Notes ="New supplier Po Notes 2",SupplierPOId = 1,SupplierPONoteId = 2,SupplierPONumber = "New Supplier PO Number ",UpdateCount = 1
                }
            };
        }

        #endregion 
    }
}
