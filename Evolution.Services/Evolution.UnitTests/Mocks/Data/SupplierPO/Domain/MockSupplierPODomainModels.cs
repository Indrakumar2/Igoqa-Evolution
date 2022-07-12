using System;
using System.Collections.Generic;
using System.Text;
using DomModel = Evolution.SupplierPO.Domain.Models.SupplierPO;

namespace Evolution.UnitTests.Mocks.Data.SupplierPO.Domain
{
   public static class MockSupplierPODomainModels
    {
        #region SupplierPO
        public static IList<DomModel.SupplierPO> GetSupplierPODomModel()
        {
            return new List<DomModel.SupplierPO>()
            {
                new DomModel.SupplierPO()
                {
                     SupplierPOId=1,SupplierPOProjectNumber=1,SupplierPONumber="TOUK/05/00323",
                    SupplierPOMaterialDescription ="Xmas Trees",SupplierPODeliveryDate=DateTime.UtcNow,SupplierPOStatus="O",SupplierPOBudget=0.01M,SupplierPOBudgetHoursWarning=12,SupplierPOBudgetHours=0.012M,
                    SupplierPOBudgetWarning=12,SupplierPOMainSupplierId=1,LastModification=DateTime.UtcNow,ModifiedBy="test",UpdateCount=0
                },
                 new DomModel.SupplierPO()
                {
                     SupplierPOId=2,SupplierPOProjectNumber=2,SupplierPONumber="TOUK/08/00325",
                    SupplierPOMaterialDescription ="JT Valves",SupplierPODeliveryDate=DateTime.UtcNow,SupplierPOStatus="O",SupplierPOBudget=0.01M,SupplierPOBudgetHoursWarning=15,SupplierPOBudgetHours=0.15M,
                    SupplierPOBudgetWarning=15,SupplierPOMainSupplierId=2,LastModification=DateTime.UtcNow,ModifiedBy="test",UpdateCount=0
                }
            };
        }
        #endregion

        #region SupplierPONote
        public static IList<DomModel.SupplierPONote> GetSupplierPONoteDomModel()
        {
            return new List<DomModel.SupplierPONote>()
            {
                new DomModel.SupplierPONote()
                {
                     SupplierPONoteId=1,SupplierPOId=1,CreatedOn=DateTime.UtcNow,CreatedBy="M.Peacock",Notes="PO to follow",
                    LastModification =DateTime.UtcNow,UpdateCount=0,ModifiedBy="test"
                },
                 new DomModel.SupplierPONote()
                {
                     SupplierPONoteId=1,SupplierPOId=1,CreatedOn=DateTime.UtcNow,CreatedBy="M.Peacock",Notes="PO to follow",
                    LastModification =DateTime.UtcNow,UpdateCount=0,ModifiedBy="test"
                }
            };
        }
        #endregion

        #region
        public static IList<DomModel.SupplierPOSubSupplier> GetSupplierPOSubSupplierDomModel()
        {
            return new List<DomModel.SupplierPOSubSupplier>()
            {
                new DomModel.SupplierPOSubSupplier()
                {
                   SubSupplierId=1,SupplierPOId=1,SupplierId=1,SupplierPONumber="TOUK/05/00323", LastModification =DateTime.UtcNow,UpdateCount=0,
                    ModifiedBy ="test"

                },
                 new DomModel.SupplierPOSubSupplier()
                {
                     SubSupplierId =1,SupplierPOId=1,SupplierId=1,SupplierPONumber="TOUK/08/00325", LastModification =DateTime.UtcNow,UpdateCount=0,
                    ModifiedBy ="test"
                }
            };
        }
        #endregion
    }
}
