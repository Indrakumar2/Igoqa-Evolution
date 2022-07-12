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
    public class VisitTechnicalSpecialistTimeRepository : GenericRepository<DbModel.VisitTechnicalSpecialistAccountItemTime>, IVisitTechnicalSpecialistTimeRespository
    {
        private readonly DbModel.EvolutionSqlDbContext _dbContext = null;
        private readonly IMapper _mapper = null;
        public VisitTechnicalSpecialistTimeRepository(DbModel.EvolutionSqlDbContext dbContext, IMapper mapper) : base(dbContext)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public IList<DomainModel.VisitSpecialistAccountItemTime> Search(DomainModel.VisitSpecialistAccountItemTime searchModel,
                                                                            params string[] includes)
        {
            var dbSearchModel = _mapper.Map<DomainModel.VisitSpecialistAccountItemTime, DbModel.VisitTechnicalSpecialistAccountItemTime>(searchModel);
            var expression = dbSearchModel.ToExpression((new List<string> { nameof(dbSearchModel.IsInvoicePrintExpenseDescription), nameof(dbSearchModel.IsInvoicePrintPayRateDescrition), nameof(dbSearchModel.InvoicingStatus) }));
            var visitTechnicalSpecialistTime = _dbContext.VisitTechnicalSpecialistAccountItemTime;
            IQueryable<DbModel.VisitTechnicalSpecialistAccountItemTime> whereClause = null;

            if (searchModel.VisitId > 0)
                whereClause = visitTechnicalSpecialistTime.Where(x => x.Visit.Id == searchModel.VisitId);

            if (expression != null)
                return visitTechnicalSpecialistTime.Where(expression).ProjectTo<DomainModel.VisitSpecialistAccountItemTime>().ToList();
            return visitTechnicalSpecialistTime.ProjectTo<DomainModel.VisitSpecialistAccountItemTime>().ToList();
        }

        //To be deleted later. Added only for sync purpose
        //public long? GetMaxEvoId()
        //{
        //    long? visitTechSpecTimeId = _dbContext.VisitTechnicalSpecialistAccountItemTime.FromSql("SELECT TOP 1 * FROM visit.VisitTechnicalSpecialistAccountItemTime ORDER By ID DESC")?.FirstOrDefault()?.Evoid ?? 0;
        //    var dbTimesheetTST = _dbContext.TimesheetTechnicalSpecialistAccountItemTime.FromSql("SELECT TOP 1 * FROM timesheet.TimesheetTechnicalSpecialistAccountItemTime ORDER By ID DESC")?.Select(x => new DbModel.TimesheetTechnicalSpecialistAccountItemTime() { Id = x.Id, Evoid = x.Evoid });
        //    long? timesheetTechSpecTimeId = dbTimesheetTST.ToList().FirstOrDefault().Evoid ?? 0;

        //    if (visitTechSpecTimeId == 0 && timesheetTechSpecTimeId == 0)
        //        return dbTimesheetTST.ToList().FirstOrDefault().Id;
        //    if (visitTechSpecTimeId > timesheetTechSpecTimeId)
        //        return visitTechSpecTimeId;
        //    else
        //        return timesheetTechSpecTimeId;
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
