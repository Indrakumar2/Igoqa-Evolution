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
    public class VisitTechnicalSpecialistTravelRepository : GenericRepository<DbModel.VisitTechnicalSpecialistAccountItemTravel>, IVisitTechnicalSpecialistTravelRepository
    {
        private readonly DbModel.EvolutionSqlDbContext _dbContext = null;
        private readonly IMapper _mapper = null;
        public VisitTechnicalSpecialistTravelRepository(DbModel.EvolutionSqlDbContext dbContext, IMapper mapper) : base(dbContext)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public IList<DomainModel.VisitSpecialistAccountItemTravel> Search(DomainModel.VisitSpecialistAccountItemTravel searchModel,
                                                                            params string[] includes)
        {
            var dbSearchModel = _mapper.Map<DomainModel.VisitSpecialistAccountItemTravel, DbModel.VisitTechnicalSpecialistAccountItemTravel>(searchModel);
            var expression = dbSearchModel.ToExpression((new List<string> { nameof(dbSearchModel.IsInvoicePrintExpenseDescription), nameof(dbSearchModel.InvoicingStatus) }));
            var visitTechnicalSpecialistTravel = _dbContext.VisitTechnicalSpecialistAccountItemTravel;
            IQueryable<DbModel.VisitTechnicalSpecialistAccountItemTravel> whereClause = null;

            if (searchModel.VisitId > 0)
                whereClause = visitTechnicalSpecialistTravel.Where(x => x.Visit.Id == searchModel.VisitId);

            if (expression != null)
                return visitTechnicalSpecialistTravel.Where(expression).ProjectTo<DomainModel.VisitSpecialistAccountItemTravel>().ToList();
            return visitTechnicalSpecialistTravel.ProjectTo<DomainModel.VisitSpecialistAccountItemTravel>().ToList();
        }


        ////To be deleted later. Added only for sync purpose
        //public long? GetMaxEvoId()
        //{
        //    long? visitTechSpecTravelId = _dbContext.VisitTechnicalSpecialistAccountItemTravel.FromSql("SELECT TOP 1 * FROM visit.VisitTechnicalSpecialistAccountItemTravel ORDER By ID DESC")?.FirstOrDefault()?.Evoid ?? 0;
        //    var dbTimesheetTS = _dbContext.TimesheetTechnicalSpecialistAccountItemTravel.FromSql("SELECT TOP 1 * FROM timesheet.TimesheetTechnicalSpecialistAccountItemTravel ORDER By ID DESC")?.Select(x => new DbModel.TimesheetTechnicalSpecialistAccountItemTravel() { Id = x.Id, Evoid = x.Evoid });
        //    long? timesheetTechSpecTravelId = dbTimesheetTS.ToList().FirstOrDefault().Evoid ?? 0;

        //    if (dbTimesheetTS != null)

        //        if (visitTechSpecTravelId == 0 && timesheetTechSpecTravelId == 0)
        //            return dbTimesheetTS.ToList().FirstOrDefault().Id;
        //    if (visitTechSpecTravelId > timesheetTechSpecTravelId)
        //        return visitTechSpecTravelId;
        //    else
        //        return timesheetTechSpecTravelId;
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
