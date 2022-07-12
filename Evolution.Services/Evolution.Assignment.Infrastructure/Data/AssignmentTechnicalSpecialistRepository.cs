using AutoMapper;
using AutoMapper.QueryableExtensions;
using Evolution.Assignment.Domain.Interfaces.Data;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.DbRepository.Models.SqlDatabaseContext;
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
    public class AssignmentTechnicalSpecialistRepository:GenericRepository<DbModel.AssignmentTechnicalSpecialist>, IAssignmentTechnicalSpecilaistRepository
   {
        private EvolutionSqlDbContext _dbContext = null;
        private IMapper _mapper = null;

        public AssignmentTechnicalSpecialistRepository(EvolutionSqlDbContext dbContext, IMapper mapper):base(dbContext)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public IList<DomainModel.AssignmentTechnicalSpecialist> Search(DomainModel.AssignmentTechnicalSpecialist searchModel, 
                                                                       params Expression<Func<DbModel.AssignmentTechnicalSpecialist, object>>[] includes)
        {
            var dbSearchModel = _mapper.Map<DomainModel.AssignmentTechnicalSpecialist,DbModel.AssignmentTechnicalSpecialist>(searchModel);
            var expression = dbSearchModel.ToExpression(); 

            IQueryable<DbModel.AssignmentTechnicalSpecialist> whereClause = GetAll();
              
           if(searchModel.Epin.HasValue)
                whereClause = whereClause.Where(x => x.TechnicalSpecialistId == searchModel.Epin);

            if (includes.Any())
                whereClause = includes.Aggregate(whereClause, (current, include) => current.Include(include));
 
            if (expression != null)
                return whereClause.Where(expression).ProjectTo<DomainModel.AssignmentTechnicalSpecialist>().OrderBy(x => x.Epin).ToList();   

            return whereClause.ProjectTo<DomainModel.AssignmentTechnicalSpecialist>().OrderBy(x => x.Epin).ToList();
        }

        public IList<DbModel.AssignmentTechnicalSpecialist> IsUniqueAssignmentTS(IList<DomainModel.AssignmentTechnicalSpecialist> assignmentTS,
                                                                                   IList<DbModel.AssignmentTechnicalSpecialist> dbAssignmentTS,
                                                                                   ValidationType validationType)
        {
            IList<DbModel.AssignmentTechnicalSpecialist> dbSpecificAssgmtTS = null;
            IList<DbModel.AssignmentTechnicalSpecialist> dbAssgmtTS = null;
            var assignmentTechSpecialist = assignmentTS?.Where(x => x.Epin>0)?
                                                                  .Select(x => x.AssignmentId)?
                                                                  .ToList();

            if (dbAssignmentTS == null && validationType!=ValidationType.Add)
            {
                dbSpecificAssgmtTS = _dbContext.AssignmentTechnicalSpecialist?.Where(x => assignmentTechSpecialist.Contains(x.AssignmentId)).ToList();
            }
            else
                dbSpecificAssgmtTS = dbAssignmentTS;

            if (dbSpecificAssgmtTS?.Count > 0)
                dbAssgmtTS = dbSpecificAssgmtTS.Join(assignmentTS.ToList(),
                                                 dbAssigmtTS => new { AssignmentID = dbAssigmtTS.AssignmentId, EpinID = dbAssigmtTS.TechnicalSpecialistId },
                                                 domAssigmtTS => new { AssignmentID =(int)domAssigmtTS.AssignmentId, EpinID =(int)domAssigmtTS.Epin },
                                                (dbAssigmtTS, domAssigmtTS) => new { dbAssigmtTS, domAssigmtTS })
                                                .Where(x => x.dbAssigmtTS.Id != x.domAssigmtTS.AssignmentTechnicalSplId)
                                                .Select(x => x.dbAssigmtTS)
                                                .ToList();

            return dbAssgmtTS;
        }

        public void Dispose()
        {
            _dbContext.Dispose();
            _mapper = null;
        }
    }
}
