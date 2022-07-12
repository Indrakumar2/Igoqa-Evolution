using Evolution.GenericDbRepository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.Supplier.Domain.Models.Supplier;

namespace Evolution.Supplier.Domain.Interfaces.Data
{
    public interface ISupplierNoteRepository: IGenericRepository<DbModel.SupplierNote>
    {
        IList<DomainModel.SupplierNote> Search(DomainModel.SupplierNote model,
                                                  params Expression<Func<DbModel.SupplierNote, object>>[] includes);
    }
}
