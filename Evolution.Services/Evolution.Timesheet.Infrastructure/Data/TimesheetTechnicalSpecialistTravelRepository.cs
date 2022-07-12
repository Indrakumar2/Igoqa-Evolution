using AutoMapper;
using AutoMapper.QueryableExtensions;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.GenericDbRepository.Services;
using Evolution.Timesheet.Domain.Interfaces.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.Timesheet.Domain.Models.Timesheets;


namespace Evolution.Timesheet.Infrastructure.Data
{
    public class TimesheetTechnicalSpecialistTravelRepository : GenericRepository<DbModel.TimesheetTechnicalSpecialistAccountItemTravel>, ITechSpecAccountItemTravelRepository
    {
        private readonly DbModel.EvolutionSqlDbContext _dbContext = null;
        private readonly IMapper _mapper = null;
        public TimesheetTechnicalSpecialistTravelRepository(DbModel.EvolutionSqlDbContext dbContext, IMapper mapper) : base(dbContext)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public IList<DomainModel.TimesheetSpecialistAccountItemTravel> Search(DomainModel.TimesheetSpecialistAccountItemTravel searchModel,
                                                                            params string[] includes)
        {
            var dbSearchModel = _mapper.Map<DomainModel.TimesheetSpecialistAccountItemTravel, DbModel.TimesheetTechnicalSpecialistAccountItemTravel>(searchModel);
            var expression = dbSearchModel.ToExpression((new List<string> { nameof(dbSearchModel.IsInvoicePrintExpenseDescription), nameof(dbSearchModel.InvoicingStatus) }));
            var timesheetTechnicalSpecialistTravel = _dbContext.TimesheetTechnicalSpecialistAccountItemTravel;
            IQueryable<DbModel.TimesheetTechnicalSpecialistAccountItemTravel> whereClause = null;

            if (searchModel.TimesheetId > 0)
                whereClause = timesheetTechnicalSpecialistTravel.Where(x => x.Timesheet.Id == searchModel.TimesheetId);

            if (expression != null)
                return timesheetTechnicalSpecialistTravel.Where(expression).ProjectTo<DomainModel.TimesheetSpecialistAccountItemTravel>().ToList();
            return timesheetTechnicalSpecialistTravel.ProjectTo<DomainModel.TimesheetSpecialistAccountItemTravel>().ToList();
        }

        public IList<DbModel.TimesheetTechnicalSpecialistAccountItemTravel> IsUniqueTimesheetTSTravel(IList<DomainModel.TimesheetSpecialistAccountItemTravel> timesheetTsTravel,
                                                           IList<DbModel.TimesheetTechnicalSpecialistAccountItemTravel> dbTimesheetTsTravel,
                                                           ValidationType validationType)
        {
            IList<DbModel.TimesheetTechnicalSpecialistAccountItemTravel> dbSpecificTravel = null;
            IList<DbModel.TimesheetTechnicalSpecialistAccountItemTravel> dbTravel = null;
            var timesheetTime = timesheetTsTravel?.Where(x => !string.IsNullOrEmpty(x.Pin))?
                                                                  .Select(x => x.TimesheetId)?
                                                                  .ToList();

            if (dbTimesheetTsTravel == null && validationType != ValidationType.Add)
            {
                dbSpecificTravel = _dbContext.TimesheetTechnicalSpecialistAccountItemTravel?.Where(x => timesheetTime.Contains(x.TimesheetId)).ToList();
            }
            else
                dbSpecificTravel = dbTimesheetTsTravel;

            if (dbSpecificTravel?.Count > 0)
                dbTravel = dbSpecificTravel.Join(timesheetTsTravel.ToList(),
                                                dbTimesheetsTime => new { TimesheetID = dbTimesheetsTime.TimesheetId, Pin = dbTimesheetsTime.TimesheetTechnicalSpecialist.TechnicalSpecialistId },
                                                domTimesheetsTime => new { TimesheetID = domTimesheetsTime.TimesheetId.GetValueOrDefault(), Pin = Convert.ToInt32(domTimesheetsTime.Pin) },
                                               (dbTimesheetsTime, domTimesheetsTime) => new { dbTimesheetsTime, domTimesheetsTime })
                                               .Where(x => x.dbTimesheetsTime.Id != x.domTimesheetsTime.TimesheetTechnicalSpecialistAccountTravelId)
                                               .Select(x => x.dbTimesheetsTime)
                                               .ToList();

            return dbTravel;
        }

        //To be deleted later. Added only for sync purpose
        //public long? GetMaxEvoId()
        //{
        //    long? visitTSId = _dbContext.VisitTechnicalSpecialistAccountItemTravel.FromSql("SELECT TOP 1 * FROM visit.VisitTechnicalSpecialistAccountItemTravel ORDER By ID DESC")?.FirstOrDefault()?.Evoid ?? 0;
        //    var dbTimesheetTS = _dbContext.TimesheetTechnicalSpecialistAccountItemTravel.FromSql("SELECT TOP 1 * FROM timesheet.TimesheetTechnicalSpecialistAccountItemTravel ORDER By ID DESC")?.Select(x => new DbModel.TimesheetTechnicalSpecialistAccountItemTravel() { Id = x.Id, Evoid = x.Evoid });
        //    long? timesheetTSId = dbTimesheetTS.ToList().FirstOrDefault().Evoid ?? 0;

        //    if (dbTimesheetTS != null)

        //        if (visitTSId == 0 && timesheetTSId == 0)
        //            return dbTimesheetTS.ToList().FirstOrDefault().Id;
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
            return result?.EvoId;
        }
        public void Dispose()
        {
            this._dbContext.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}

