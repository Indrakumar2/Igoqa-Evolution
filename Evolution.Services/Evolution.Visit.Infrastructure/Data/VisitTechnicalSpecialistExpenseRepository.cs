using AutoMapper;
using AutoMapper.QueryableExtensions;
using Evolution.Common.Extensions;
using Evolution.GenericDbRepository.Services;
using Evolution.Visit.Domain.Interfaces.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.Visit.Domain.Models.Visits;

namespace Evolution.Visit.Infrastructure.Data
{
    public class VisitTechnicalSpecialistExpenseRepository : GenericRepository<DbModel.VisitTechnicalSpecialistAccountItemExpense>, IVisitTechnicalSpecialistExpenseRepository
    {
        private readonly DbModel.EvolutionSqlDbContext _dbContext = null;
        private readonly IMapper _mapper = null;
        public VisitTechnicalSpecialistExpenseRepository(DbModel.EvolutionSqlDbContext dbContext, IMapper mapper) : base(dbContext)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public IList<DomainModel.VisitSpecialistAccountItemExpense> Search(DomainModel.VisitSpecialistAccountItemExpense searchModel,
                                                                            params string[] includes)
        {
            var dbSearchModel = _mapper.Map<DomainModel.VisitSpecialistAccountItemExpense, DbModel.VisitTechnicalSpecialistAccountItemExpense>(searchModel);
            var expression = dbSearchModel.ToExpression((new List<string> { nameof(dbSearchModel.IsContractHolderExpense), nameof(dbSearchModel.InvoicingStatus) }));
            var visitTechnicalSpecialistExpense = _dbContext.VisitTechnicalSpecialistAccountItemExpense;
            IQueryable<DbModel.VisitTechnicalSpecialistAccountItemExpense> whereClause = null;

            if (searchModel.VisitId > 0)
                whereClause = visitTechnicalSpecialistExpense.Where(x => x.Visit.Id == searchModel.VisitId);

            if (expression != null)
                return visitTechnicalSpecialistExpense.Where(expression).ProjectTo<DomainModel.VisitSpecialistAccountItemExpense>().ToList();
            return visitTechnicalSpecialistExpense.ProjectTo<DomainModel.VisitSpecialistAccountItemExpense>().ToList();
        }
        //To be deleted later. Added only for sync purpose
        //public long? GetMaxEvoId()
        //{
        //    long? visitTechSpecExId = _dbContext.VisitTechnicalSpecialistAccountItemExpense.FromSql("SELECT TOP 1 * FROM visit.VisitTechnicalSpecialistAccountItemExpense ORDER By ID DESC")?.FirstOrDefault()?.Evoid ?? 0;
        //    var dbTimesheetTSE = _dbContext.TimesheetTechnicalSpecialistAccountItemExpense.FromSql("SELECT TOP 1 * FROM timesheet.TimesheetTechnicalSpecialistAccountItemExpense ORDER By ID DESC")?.Select(x => new DbModel.TimesheetTechnicalSpecialistAccountItemExpense() { Id = x.Id, Evoid = x.Evoid });
        //    long? timesheetTechSpecExId = dbTimesheetTSE.ToList().FirstOrDefault().Evoid ?? 0;

        //    if (visitTechSpecExId == 0 && timesheetTechSpecExId == 0)
        //        return dbTimesheetTSE.ToList().FirstOrDefault().Id;
        //    if (visitTechSpecExId > timesheetTechSpecExId)
        //        return visitTechSpecExId;
        //    else
        //        return timesheetTechSpecExId;
        //}

        //To be deleted later. Added only for sync purpose
        public long? GetMaxEvoId()
        {
            var result = _dbContext.EvoIDGeneration.FromSql(@"SELECT MAX(EVOID) AS EvoId FROM (
                                                                SELECT MAX(EVOID) AS EVOID FROM timesheet.TimesheetTechnicalSpecialistAccountItemConsumable with(nolock) WHERE ISNULL(EVOID, 0) != 0
                                                                UNION
                                                                SELECT MAX(EVOID) AS EVOID FROM timesheet.TimesheetTechnicalSpecialistAccountItemExpense with(nolock) WHERE ISNULL(EVOID, 0) != 0
                                                                UNION
                                                                SELECT MAX(EVOID) AS EVOID FROM timesheet.TimesheetTechnicalSpecialistAccountItemTime with(nolock) WHERE ISNULL(EVOID, 0) != 0
                                                                UNION
                                                                SELECT MAX(EVOID) AS EVOID FROM timesheet.TimesheetTechnicalSpecialistAccountItemTravel with(nolock) WHERE ISNULL(EVOID, 0) != 0
                                                                UNION
                                                                SELECT MAX(EVOID) AS EVOID FROM visit.VisitTechnicalSpecialistAccountItemConsumable with(nolock) WHERE ISNULL(EVOID, 0) != 0
                                                                UNION
                                                                SELECT MAX(EVOID) AS EVOID FROM visit.VisitTechnicalSpecialistAccountItemExpense with(nolock) WHERE ISNULL(EVOID, 0) != 0
                                                                UNION
                                                                SELECT MAX(EVOID) AS EVOID FROM visit.VisitTechnicalSpecialistAccountItemTime with(nolock) WHERE ISNULL(EVOID, 0) != 0
                                                                UNION
                                                                SELECT MAX(EVOID) AS EVOID FROM visit.VisitTechnicalSpecialistAccountItemTravel with(nolock) WHERE ISNULL(EVOID, 0) != 0) TEMP")
                                                                ?.AsNoTracking()?.ToList()?.First();
            return result?.EvoId;
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }
    }
}
