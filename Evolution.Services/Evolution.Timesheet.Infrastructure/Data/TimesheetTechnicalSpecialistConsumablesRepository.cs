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
    public class TimesheetTechnicalSpecialistConsumablesRepository : GenericRepository<DbModel.TimesheetTechnicalSpecialistAccountItemConsumable>, ITechSpecAccountItemConsumableRepository
    {
        private readonly DbModel.EvolutionSqlDbContext _dbContext = null;
        private readonly IMapper _mapper = null;
        public TimesheetTechnicalSpecialistConsumablesRepository(DbModel.EvolutionSqlDbContext dbContext, IMapper mapper) : base(dbContext)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public IList<DomainModel.TimesheetSpecialistAccountItemConsumable> Search(DomainModel.TimesheetSpecialistAccountItemConsumable searchModel,
                                                                            params string[] includes)
        {
            var dbSearchModel = _mapper.Map<DomainModel.TimesheetSpecialistAccountItemConsumable, DbModel.TimesheetTechnicalSpecialistAccountItemConsumable>(searchModel);
            var expression = dbSearchModel.ToExpression((new List<string> { nameof(dbSearchModel.IsInvoicePrintChargeDescription), nameof(dbSearchModel.InvoicingStatus) }));
            var timesheetTechnicalSpecialistConsumables = _dbContext.TimesheetTechnicalSpecialistAccountItemConsumable;
            IQueryable<DbModel.TimesheetTechnicalSpecialistAccountItemConsumable> whereClause = null;

            if (searchModel.TimesheetId > 0)
                whereClause = timesheetTechnicalSpecialistConsumables.Where(x => x.Timesheet.Id == searchModel.TimesheetId);

            if (expression != null)
                return timesheetTechnicalSpecialistConsumables.Where(expression).ProjectTo<DomainModel.TimesheetSpecialistAccountItemConsumable>().ToList();
            return timesheetTechnicalSpecialistConsumables.ProjectTo<DomainModel.TimesheetSpecialistAccountItemConsumable>().ToList();
        }

        public IList<DbModel.TimesheetTechnicalSpecialistAccountItemConsumable> IsUniqueTimesheetTSConsumables(IList<DomainModel.TimesheetSpecialistAccountItemConsumable> timesheetTsConsumables,
                                                                    IList<DbModel.TimesheetTechnicalSpecialistAccountItemConsumable> dbTimesheetTsConsumables,
                                                                    ValidationType validationType)
        {
            IList<DbModel.TimesheetTechnicalSpecialistAccountItemConsumable> dbSpecificTimeCon = null;
            IList<DbModel.TimesheetTechnicalSpecialistAccountItemConsumable> dbTimeCon = null;
            var timesheetConsumables = timesheetTsConsumables?.Where(x => !string.IsNullOrEmpty(x.Pin))?
                                                                  .Select(x => x.TimesheetId)?
                                                                  .ToList();

            if (dbTimesheetTsConsumables == null && validationType != ValidationType.Add)
            {
                dbSpecificTimeCon = _dbContext.TimesheetTechnicalSpecialistAccountItemConsumable?.Where(x => timesheetConsumables.Contains(x.TimesheetId)).ToList();
            }
            else
                dbSpecificTimeCon = dbTimesheetTsConsumables;

            if (dbSpecificTimeCon?.Count > 0)
                dbTimeCon = dbSpecificTimeCon.Join(timesheetTsConsumables.ToList(),
                                               dbTimesheetsCon => new { TimesheetID = dbTimesheetsCon.TimesheetId, Pin = dbTimesheetsCon.TimesheetTechnicalSpecialist.TechnicalSpecialist.Pin },
                                               domTimesheetsCon => new { TimesheetID = domTimesheetsCon.TimesheetId.GetValueOrDefault(), Pin = Convert.ToInt32(domTimesheetsCon.Pin) },
                                              (dbTimesheetsCon, domTimesheetsCon) => new { dbTimesheetsCon, domTimesheetsCon })
                                               .Where(x => x.dbTimesheetsCon.Id != x.domTimesheetsCon.TimesheetTechnicalSpecialistAccountConsumableId)
                                               .Select(x => x.dbTimesheetsCon)
                                              .ToList();


            return dbTimeCon;
        }

        //To be deleted later. Added only for sync purpose
        //public long? GetMaxEvoId()
        //{
        //    long? visitTSId=_dbContext.VisitTechnicalSpecialistAccountItemConsumable.FromSql("SELECT TOP 1 * FROM visit.VisitTechnicalSpecialistAccountItemConsumable ORDER By ID DESC")?.FirstOrDefault()?.Evoid ?? 0;
        //    var dbTimesheetTSC=_dbContext.TimesheetTechnicalSpecialistAccountItemConsumable.FromSql("SELECT TOP 1 * FROM timesheet.TimesheetTechnicalSpecialistAccountItemConsumable ORDER By ID DESC")?.Select(x=> new DbModel.TimesheetTechnicalSpecialistAccountItemConsumable() {Id=x.Id,Evoid=x.Evoid});
        //    long? timesheetTSId= dbTimesheetTSC.ToList().FirstOrDefault().Evoid ?? 0;
        //    if (visitTSId == 0 && timesheetTSId == 0)
        //        return  dbTimesheetTSC.ToList().FirstOrDefault().Id;
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
            this._dbContext.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}

