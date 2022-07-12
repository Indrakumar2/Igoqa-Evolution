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
    public class TimesheetTechnicalSpecialistTimeRepository : GenericRepository<DbModel.TimesheetTechnicalSpecialistAccountItemTime>, ITechSpecAccountItemTimeRepository
    {
        private readonly DbModel.EvolutionSqlDbContext _dbContext = null;
        private readonly IMapper _mapper = null;
        public TimesheetTechnicalSpecialistTimeRepository(DbModel.EvolutionSqlDbContext dbContext, IMapper mapper) : base(dbContext)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public IList<DomainModel.TimesheetSpecialistAccountItemTime> Search(DomainModel.TimesheetSpecialistAccountItemTime searchModel,
                                                                            params string[] includes)
        {
            var dbSearchModel = _mapper.Map<DomainModel.TimesheetSpecialistAccountItemTime, DbModel.TimesheetTechnicalSpecialistAccountItemTime>(searchModel);
            var expression = dbSearchModel.ToExpression((new List<string> { nameof(dbSearchModel.IsInvoicePrintExpenseDescription), nameof(dbSearchModel.IsInvoicePrintPayRateDescrition), nameof(dbSearchModel.InvoicingStatus) }));
            var timesheetTechnicalSpecialistTime = _dbContext.TimesheetTechnicalSpecialistAccountItemTime;
            IQueryable<DbModel.TimesheetTechnicalSpecialistAccountItemTime> whereClause = null;

            if (searchModel.TimesheetId > 0)
                whereClause = timesheetTechnicalSpecialistTime.Where(x => x.Timesheet.Id == searchModel.TimesheetId);

            if (expression != null)
                return timesheetTechnicalSpecialistTime.Where(expression).ProjectTo<DomainModel.TimesheetSpecialistAccountItemTime>().ToList();
            return timesheetTechnicalSpecialistTime.ProjectTo<DomainModel.TimesheetSpecialistAccountItemTime>().ToList();
        }

        public IList<DbModel.TimesheetTechnicalSpecialistAccountItemTime> IsUniqueTimesheetTSTime(IList<DomainModel.TimesheetSpecialistAccountItemTime> timesheetTsTime,
                                                            IList<DbModel.TimesheetTechnicalSpecialistAccountItemTime> dbTimesheetTsTime,
                                                            ValidationType validationType)
        {
            IList<DbModel.TimesheetTechnicalSpecialistAccountItemTime> dbSpecificTime = null;
            IList<DbModel.TimesheetTechnicalSpecialistAccountItemTime> dbTime = null;
            var timesheetTime = timesheetTsTime?.Where(x => !string.IsNullOrEmpty(x.Pin))?
                                                                  .Select(x => x.TimesheetId)?
                                                                  .ToList();

            if (dbTimesheetTsTime == null && validationType != ValidationType.Add)
            {
                dbSpecificTime = _dbContext.TimesheetTechnicalSpecialistAccountItemTime?.Where(x => timesheetTime.Contains(x.TimesheetId)).ToList();
            }
            else
                dbSpecificTime = dbTimesheetTsTime;

            if (dbSpecificTime?.Count > 0)
                dbTime = dbSpecificTime.Join(timesheetTsTime.ToList(),
                                                dbTimesheetsTime => new { TimesheetID = dbTimesheetsTime.TimesheetId, Pin = dbTimesheetsTime.TimesheetTechnicalSpeciallist.TechnicalSpecialistId },
                                                domTimesheetsTime => new { TimesheetID = domTimesheetsTime.TimesheetId.GetValueOrDefault(), Pin = Convert.ToInt32(domTimesheetsTime.Pin) },
                                               (dbTimesheetsTime, domTimesheetsTime) => new { dbTimesheetsTime, domTimesheetsTime })
                                               .Where(x => x.dbTimesheetsTime.Id != x.domTimesheetsTime.TimesheetTechnicalSpecialistAccountTimeId)
                                               .Select(x => x.dbTimesheetsTime)
                                               .ToList();

            return dbTime;
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
            this._dbContext.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}

