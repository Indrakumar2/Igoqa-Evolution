using Evolution.GenericDbRepository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using DomainModel = Evolution.Assignment.Domain.Models.Assignments;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using Evolution.Assignment.Domain.Models.Assignments;

namespace Evolution.Assignment.Domain.Interfaces.Data
{
    public interface IAssignmentSubSupplerRepository : IGenericRepository<DbModel.AssignmentSubSupplier>, IDisposable
    {
        IList<DomainModel.AssignmentSubSupplier> Search(DomainModel.AssignmentSubSupplier model, params Expression<Func<DbModel.AssignmentSubSupplier, object>>[] includes);
        IList<DomainModel.AssignmentSubSupplierVisit> GetSubSupplierForVisit(DomainModel.AssignmentSubSupplierVisit model, params Expression<Func<DbModel.AssignmentSubSupplier, object>>[] includes);
        List<AssignmentSubSupplier> GetAssignmentSubSuppliers(int AssigenmnetID);
        void RemoveAssignementSubSupplier(List<int> assignmentIds,List<int> SupplierId,string SupplierType);
        void AddAssignmentsubsupplier( IList<DomainModel.AssignmentSubSupplier> assigenmentsubsupplierdata);
        
    }
}
