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
    public class VisitTechnicalSpecialistConsumablesRepository : GenericRepository<DbModel.VisitTechnicalSpecialistAccountItemConsumable>, IVisitTechnicalSpecialistConsumableRepository
    {
        private readonly DbModel.EvolutionSqlDbContext _dbContext = null;
        private readonly IMapper _mapper = null;
        public VisitTechnicalSpecialistConsumablesRepository(DbModel.EvolutionSqlDbContext dbContext, IMapper mapper) : base(dbContext)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public IList<DomainModel.VisitSpecialistAccountItemConsumable> Search(DomainModel.VisitSpecialistAccountItemConsumable searchModel,
                                                                            params string[] includes)
        {
            var dbSearchModel = _mapper.Map<DomainModel.VisitSpecialistAccountItemConsumable, DbModel.VisitTechnicalSpecialistAccountItemConsumable>(searchModel);
            var expression = dbSearchModel.ToExpression((new List<string> { nameof(dbSearchModel.IsInvoicePrintChargeDescription), nameof(dbSearchModel.InvoicingStatus) }));
            var visitTechnicalSpecialistConsumables = _dbContext.VisitTechnicalSpecialistAccountItemConsumable;
            IQueryable<DbModel.VisitTechnicalSpecialistAccountItemConsumable> whereClause = null;

            if (searchModel.VisitId > 0)
                whereClause = visitTechnicalSpecialistConsumables.Where(x => x.Visit.Id == searchModel.VisitId);

            if (expression != null)
                return visitTechnicalSpecialistConsumables.Where(expression).ProjectTo<DomainModel.VisitSpecialistAccountItemConsumable>().ToList();
            return visitTechnicalSpecialistConsumables.ProjectTo<DomainModel.VisitSpecialistAccountItemConsumable>().ToList();
        }


        //To be deleted later. Added only for sync purpose
        //public long? GetMaxEvoId()
        //{
        //    long? visitTSId = _dbContext.VisitTechnicalSpecialistAccountItemConsumable.FromSql("SELECT TOP 1 * FROM visit.VisitTechnicalSpecialistAccountItemConsumable ORDER By ID DESC")?.FirstOrDefault()?.Evoid ?? 0;
        //    var dbTimesheetTSC = _dbContext.TimesheetTechnicalSpecialistAccountItemConsumable.FromSql("SELECT TOP 1 * FROM timesheet.TimesheetTechnicalSpecialistAccountItemConsumable ORDER By ID DESC")?.Select(x => new DbModel.TimesheetTechnicalSpecialistAccountItemConsumable() { Id = x.Id, Evoid = x.Evoid });
        //    long? timesheetTSId = dbTimesheetTSC.ToList().FirstOrDefault().Evoid ?? 0;
        //    if (visitTSId == 0 && timesheetTSId == 0)
        //        return dbTimesheetTSC.ToList().FirstOrDefault().Id;
        //    if (visitTSId > timesheetTSId)
        //        return visitTSId;
        //    else
        //        return timesheetTSId;
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
            return result.EvoId;
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }
    }
}
