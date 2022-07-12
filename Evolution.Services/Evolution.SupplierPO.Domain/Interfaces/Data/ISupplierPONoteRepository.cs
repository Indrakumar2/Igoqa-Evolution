using Evolution.GenericDbRepository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.SupplierPO.Domain.Models.SupplierPO;

namespace Evolution.SupplierPO.Domain.Interfaces.Data
{
   public  interface ISupplierPONoteRepository: IGenericRepository<DbModel.SupplierPurchaseOrderNote>
    {
        IList<DomainModel.SupplierPONote> Search(DomainModel.SupplierPONote searchModel);       
    }
}
