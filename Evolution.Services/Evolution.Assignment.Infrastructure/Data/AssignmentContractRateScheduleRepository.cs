using AutoMapper;
using AutoMapper.QueryableExtensions;
using Evolution.Assignment.Domain.Interfaces.Data;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.GenericDbRepository.Services;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.Assignment.Domain.Models.Assignments;

namespace Evolution.Assignment.Infrastructure.Data
{
    public class AssignmentContractRateScheduleRepository : GenericRepository<DbModel.AssignmentContractSchedule>, IAssignmentContractRateScheduleRepository
    {
        private  DbModel.EvolutionSqlDbContext _dbContext = null;
        private  IMapper _mapper = null;

        public AssignmentContractRateScheduleRepository(DbModel.EvolutionSqlDbContext dbContext, IMapper mapper) : base(dbContext)
        {
            this._dbContext = dbContext;
            this._mapper = mapper;
        }

        public IList<DomainModel.AssignmentContractRateSchedule> Search(DomainModel.AssignmentContractRateSchedule model)
        {
            var dbSearchModel = _mapper.Map<DbModel.AssignmentContractSchedule>(model);
            IQueryable<DbModel.AssignmentContractSchedule> whereClause = null;

            if (model.ContractScheduleName.HasEvoWildCardChar())
                whereClause = _dbContext.AssignmentContractSchedule.WhereLike(x => x.ContractSchedule.Name, model.ContractScheduleName, '*');
            else
                whereClause = _dbContext.AssignmentContractSchedule.Where(x => string.IsNullOrEmpty(model.ContractScheduleName) || x.ContractSchedule.Name == model.ContractScheduleName);

            var expression = dbSearchModel.ToExpression();

            if (expression == null)
                return whereClause.ProjectTo<DomainModel.AssignmentContractRateSchedule>().ToList();
            else
                return whereClause.Where(expression).ProjectTo<DomainModel.AssignmentContractRateSchedule>().ToList();
        }


        public IList<DbModel.AssignmentContractSchedule> IsUniqueAssignmentContractSchedules(IList<DomainModel.AssignmentContractRateSchedule> assignmentContractRateSchedules,
                                                                                    IList<DbModel.AssignmentContractSchedule> dbAssignmentContractSchedule,
                                                                                     ValidationType validationType)
        {
            IList<DbModel.AssignmentContractSchedule> dbSpecificAssgmtContract = null;
            IList<DbModel.AssignmentContractSchedule> dbAssgmtContract = null;
            var assignmentContractSch = assignmentContractRateSchedules?.Where(x => !string.IsNullOrEmpty(x.ContractScheduleName) && x.AssignmentId > 0)?
                                                                  .Select(x => x.AssignmentId)?
                                                                  .ToList();
            if (assignmentContractSch != null)
            {
                if (dbAssignmentContractSchedule == null && validationType != ValidationType.Add)
                {
                    dbSpecificAssgmtContract = _dbContext.AssignmentContractSchedule?.Where(x => assignmentContractSch.Contains(x.AssignmentId)).AsNoTracking().ToList();
                }
                else
                    dbSpecificAssgmtContract = dbAssignmentContractSchedule;

                if (dbSpecificAssgmtContract?.Count > 0)
                    dbAssgmtContract = dbSpecificAssgmtContract.Join(assignmentContractRateSchedules.ToList(),
                                                     dbAssigmtCon => new { AssignmentID = dbAssigmtCon.AssignmentId, ScheduleName = dbAssigmtCon.ContractSchedule.Name.Trim().ToLower() },
                                                     domAssigmtCon => new { AssignmentID = (int)domAssigmtCon.AssignmentId, ScheduleName = domAssigmtCon.ContractScheduleName.Trim().ToLower() },
                                                    (dbAssigmtCon, domAssigmtCon) => new { dbAssigmtCon, domAssigmtCon })
                                                    .Where(x => x.dbAssigmtCon.Id != x.domAssigmtCon.AssignmentContractRateScheduleId)
                                                    .Select(x => x.dbAssigmtCon)
                                                    .ToList();
            }

            return dbAssgmtContract;
        }


        public void Dispose()
        {
            _dbContext.Dispose();
            _mapper = null;
        }

    }
}
