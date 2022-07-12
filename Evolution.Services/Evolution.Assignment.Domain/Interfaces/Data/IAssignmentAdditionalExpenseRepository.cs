using Evolution.Common.Enums;
using Evolution.GenericDbRepository.Interfaces;
using System;
using System.Collections.Generic;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.Assignment.Domain.Models.Assignments;

namespace Evolution.Assignment.Domain.Interfaces.Data
{
    public interface IAssignmentAdditionalExpenseRepository : IGenericRepository<DbModel.AssignmentAdditionalExpense>, IDisposable
    {
        IList<DomainModel.AssignmentAdditionalExpense> Search(DomainModel.AssignmentAdditionalExpense model);

        IList<DbModel.AssignmentAdditionalExpense> IsUniqueAssignmentExpense(IList<DomainModel.AssignmentAdditionalExpense> assignmentExpenses,
                                                                             IList<DbModel.AssignmentAdditionalExpense> dbAssignmentExpenses,
                                                                             ValidationType validationType);

    }
}