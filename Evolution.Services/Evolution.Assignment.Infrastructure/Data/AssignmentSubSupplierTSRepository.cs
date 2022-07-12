using AutoMapper;
using AutoMapper.QueryableExtensions;
using Evolution.Assignment.Domain.Interfaces.Data;
using Evolution.Common.Extensions;
using Evolution.GenericDbRepository.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.Assignment.Domain.Models.Assignments;

namespace Evolution.Assignment.Infrastructure.Data
{
    public class AssignmentSubSupplierTSRepository : GenericRepository<DbModel.AssignmentSubSupplierTechnicalSpecialist>, IAssignmentSubSupplerTSRepository
    {
        private  DbModel.EvolutionSqlDbContext _dbContext = null;
        private  IMapper _mapper = null;

        public AssignmentSubSupplierTSRepository(DbModel.EvolutionSqlDbContext dbContext, IMapper mapper) : base(dbContext)
        {
            this._dbContext = dbContext;
            this._mapper = mapper;
        }

        public IList<DomainModel.AssignmentSubSupplierTechnicalSpecialist> Search(DomainModel.AssignmentSubSupplierTechnicalSpecialist model, params Expression<Func<DbModel.AssignmentSubSupplierTechnicalSpecialist, object>>[] includes)
        {
            var dbSearchModel = _mapper.Map<DbModel.AssignmentSubSupplierTechnicalSpecialist>(model);
            var expression = dbSearchModel.ToExpression();

            IQueryable<DbModel.AssignmentSubSupplierTechnicalSpecialist> whereClause = GetAll();
            if (includes.Any())
                whereClause = includes.Aggregate(whereClause, (current, include) => current.Include(include));

            //if (model.SupplierName.HasEvoWildCardChar())
            //    whereClause = _dbContext.AssignmentSubSupplierTechnicalSpecialist.WhereLike(x => x.SubSupplier.Supplier.SupplierName, model.SupplierName, '*');
            //else
            //    whereClause = _dbContext.AssignmentSubSupplierTechnicalSpecialist.Where(x => string.IsNullOrEmpty(model.SupplierName) || x.SubSupplier.Supplier.SupplierName == model.SupplierName);

            //if (model.SubSupplierId > 0)
            //    whereClause = _dbContext.AssignmentSubSupplierTechnicalSpecialist.Where(x => x.SubSupplierId == model.SubSupplierId);

            //if (model.AssignmentId.HasValue)
            //    whereClause = _dbContext.AssignmentSubSupplierTechnicalSpecialist.Where(x => x.AssignmentId == model.AssignmentId);

            if (model.TechnicalSpecialistName.HasEvoWildCardChar())
                whereClause = _dbContext.AssignmentSubSupplierTechnicalSpecialist.WhereLike(x => x.TechnicalSpecialist.TechnicalSpecialist.LastName + x.TechnicalSpecialist.TechnicalSpecialist.FirstName, model.TechnicalSpecialistName, '*');
            else
                whereClause = _dbContext.AssignmentSubSupplierTechnicalSpecialist.Where(x => string.IsNullOrEmpty(model.TechnicalSpecialistName) || x.TechnicalSpecialist.TechnicalSpecialist.LastName + x.TechnicalSpecialist.TechnicalSpecialist.FirstName == model.TechnicalSpecialistName);


            if (expression == null)
                return whereClause.ProjectTo<DomainModel.AssignmentSubSupplierTechnicalSpecialist>().ToList();
            else
                return whereClause.Where(expression).ProjectTo<DomainModel.AssignmentSubSupplierTechnicalSpecialist>().ToList();
        }


        //public int DeleteSupplierTS(List<DbModel.AssignmentSubSupplierTechnicalSpecialist> dbassignmentSubSupplierTS)
        //{
        //    var assignmentSubSupplierTSId = dbassignmentSubSupplierTS?.Where(x => x.Id > 0).Select(x => (int)x.Id)?.Distinct().ToList();
        //    return Delete(assignmentSubSupplierTSId);
        //}

        //public int Delete(List<int> assignmentSubSupplierTSId)
        //{
        //    int count = 0;
        //    try
        //    {
        //        if (assignmentSubSupplierTSId.Count > 0)
        //        {
        //            var deleteStatement = string.Format(Utility.GetSqlQuery(SQLModuleType.Assignment_SubSupplierTS, SQLModuleActionType.Delete), string.Join(",", assignmentSubSupplierTSId.ToString<int>()));
        //            count = _dbContext.Database.ExecuteSqlCommand(deleteStatement);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), "AssignmentSubSupplierTSId=" + assignmentSubSupplierTSId.ToString<int>());
        //    }

        //    return count;
        //}

        public void Dispose()
        {
            _dbContext.Dispose();
            _mapper = null;
        }
    }
}
