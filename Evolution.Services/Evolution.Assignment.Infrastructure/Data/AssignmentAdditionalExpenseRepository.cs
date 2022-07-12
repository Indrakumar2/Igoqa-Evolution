using AutoMapper;
using AutoMapper.QueryableExtensions;
using Evolution.Assignment.Domain.Interfaces.Data;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.DbRepository.Models.SqlDatabaseContext;
using Evolution.GenericDbRepository.Services;
using System.Collections.Generic;
using System.Linq;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.Assignment.Domain.Models.Assignments;

namespace Evolution.Assignment.Infrastructure.Data
{
    public class AssignmentAdditionalExpenseRepository : GenericRepository<DbModel.AssignmentAdditionalExpense>, IAssignmentAdditionalExpenseRepository
    {
        private  EvolutionSqlDbContext _dbContext = null;
        private  IMapper _mapper = null;

        public AssignmentAdditionalExpenseRepository(EvolutionSqlDbContext dbContext, IMapper mapper) : base(dbContext)
        {
            this._dbContext = dbContext;
            this._mapper = mapper;
        }

        public IList<DomainModel.AssignmentAdditionalExpense> Search(DomainModel.AssignmentAdditionalExpense model)
        {
            var dbSearchModel = this._mapper.Map<DbModel.AssignmentAdditionalExpense>(model);
            IQueryable<DbModel.AssignmentAdditionalExpense> whereClause = null;

            if (model.CompanyName.HasEvoWildCardChar())
                whereClause = _dbContext.AssignmentAdditionalExpense.WhereLike(x => x.Company.Name.ToString(), model.CompanyName, '*');
            else
                whereClause = _dbContext.AssignmentAdditionalExpense.Where(x => string.IsNullOrEmpty(model.CompanyName) || x.Company.Name.ToString() == model.CompanyName);

            if (model.CompanyCode.HasEvoWildCardChar())
                whereClause = whereClause.WhereLike(x => x.Company.Code.ToString(), model.CompanyCode, '*');
            else
                whereClause = whereClause.Where(x => string.IsNullOrEmpty(model.CompanyCode) || x.Company.Code.ToString() == model.CompanyCode);

            if (model.ExpenseType.HasEvoWildCardChar())
                whereClause = whereClause.WhereLike(x => x.ExpenseType.Name.ToString(), model.ExpenseType, '*');
            else
                whereClause = whereClause.Where(x => string.IsNullOrEmpty(model.ExpenseType) || x.ExpenseType.Name.ToString() == model.ExpenseType);

            var expression = dbSearchModel.ToExpression();

            if (expression == null)
            {
                return whereClause.ProjectTo<DomainModel.AssignmentAdditionalExpense>().ToList();
            }
            else
                return whereClause.Where(expression).ProjectTo<DomainModel.AssignmentAdditionalExpense>().ToList();
        }

        public IList<DbModel.AssignmentAdditionalExpense> IsUniqueAssignmentExpense(IList<DomainModel.AssignmentAdditionalExpense> assignmentExpenses,
                                                                                 IList<DbModel.AssignmentAdditionalExpense> dbAssignmentExpenses,
                                                                                 ValidationType validationType)
        {
            IList<DbModel.AssignmentAdditionalExpense> dbSpecificAssgmtExpense = null;
            IList<DbModel.AssignmentAdditionalExpense> dbAssgmtExpense = null;
            var assignmentExpense = assignmentExpenses?.Where(x => !string.IsNullOrEmpty(x.ExpenseType))?
                                                                  .Select(x => x.AssignmentId)?
                                                                  .ToList();

            if (dbAssignmentExpenses == null && validationType!=ValidationType.Add)
            {
                dbSpecificAssgmtExpense = _dbContext.AssignmentAdditionalExpense?.Where(x => assignmentExpense.Contains(x.AssignmentId)).ToList();
            }
            else
                dbSpecificAssgmtExpense = dbAssignmentExpenses;

            if (dbSpecificAssgmtExpense?.Count > 0)
                dbAssgmtExpense = dbSpecificAssgmtExpense.Join(assignmentExpenses.ToList(),
                                                 dbAssigmtExpense => new { AssignmentID = dbAssigmtExpense.AssignmentId, Expense = dbAssigmtExpense.ExpenseType.Name.Trim(), CompanyCode= dbAssigmtExpense.Company.Code.Trim() },
                                                 domAssigmtExpense => new { AssignmentID = (int)domAssigmtExpense.AssignmentId, Expense = domAssigmtExpense.ExpenseType.Trim(), CompanyCode = domAssigmtExpense.CompanyCode.Trim() },
                                                (dbAssigmtExpense, domAssigmtExpense) => new { dbAssigmtExpense, domAssigmtExpense })
                                                .Where(x => x.dbAssigmtExpense.Id != x.domAssigmtExpense.AssignmentAdditionalExpenseId)
                                                .Select(x => x.dbAssigmtExpense)
                                                .ToList();

            return dbAssgmtExpense;
        }

        public void Dispose()
        {
            _dbContext.Dispose();
            _mapper = null;
        }
    }
}