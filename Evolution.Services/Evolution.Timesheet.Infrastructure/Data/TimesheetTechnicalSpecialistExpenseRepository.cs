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
    public class TimesheetTechnicalSpecialistExpenseRepository : GenericRepository<DbModel.TimesheetTechnicalSpecialistAccountItemExpense>, ITechSpecAccountItemExpenseRepository
    {
        private readonly DbModel.EvolutionSqlDbContext _dbContext = null;
        private readonly IMapper _mapper = null;
        public TimesheetTechnicalSpecialistExpenseRepository(DbModel.EvolutionSqlDbContext dbContext, IMapper mapper) : base(dbContext)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public IList<DomainModel.TimesheetSpecialistAccountItemExpense> Search(DomainModel.TimesheetSpecialistAccountItemExpense searchModel,
                                                                            params string[] includes)
        {
            var dbSearchModel = _mapper.Map<DomainModel.TimesheetSpecialistAccountItemExpense, DbModel.TimesheetTechnicalSpecialistAccountItemExpense>(searchModel);
            var expression = dbSearchModel.ToExpression((new List<string> { nameof(dbSearchModel.IsContractHolderExpense), nameof(dbSearchModel.InvoicingStatus) }));
            var timesheetTechnicalSpecialistExpense = _dbContext.TimesheetTechnicalSpecialistAccountItemExpense;
            IQueryable<DbModel.TimesheetTechnicalSpecialistAccountItemExpense> whereClause = null;

            if (searchModel.TimesheetId > 0)
                whereClause = timesheetTechnicalSpecialistExpense.Where(x => x.Timesheet.Id == searchModel.TimesheetId);

            if (expression != null)
                return timesheetTechnicalSpecialistExpense.Where(expression).ProjectTo<DomainModel.TimesheetSpecialistAccountItemExpense>().ToList();
            return timesheetTechnicalSpecialistExpense.ProjectTo<DomainModel.TimesheetSpecialistAccountItemExpense>().ToList();
        }

        public IList<DbModel.TimesheetTechnicalSpecialistAccountItemExpense> IsUniqueTimesheetTechSpecialistExpense(IList<DomainModel.TimesheetSpecialistAccountItemExpense> timesheetTsExpenses,
                                                                   IList<DbModel.TimesheetTechnicalSpecialistAccountItemExpense> dbTimesheetTsExpenses,
                                                                   ValidationType validationType)
        {
            IList<DbModel.TimesheetTechnicalSpecialistAccountItemExpense> dbSpecificTimeExpense = null;
            IList<DbModel.TimesheetTechnicalSpecialistAccountItemExpense> dbTimeExpense = null;
            var timesheetExpenses = timesheetTsExpenses?.Where(x => !string.IsNullOrEmpty(x.Pin))?
                                                                  .Select(x => x.TimesheetId)?
                                                                  .ToList();

            if (dbTimesheetTsExpenses == null && validationType != ValidationType.Add)
            {
                dbSpecificTimeExpense = _dbContext.TimesheetTechnicalSpecialistAccountItemExpense?.Where(x => timesheetExpenses.Contains(x.TimesheetId)).ToList();
            }
            else
                dbSpecificTimeExpense = dbTimesheetTsExpenses;

            if (dbSpecificTimeExpense?.Count > 0)
                dbTimeExpense = dbSpecificTimeExpense.Join(timesheetTsExpenses.ToList(),
                                                dbTimesheetsEx => new { TimesheetID = dbTimesheetsEx.TimesheetId, Pin = dbTimesheetsEx.TimesheetTechnicalSpeciallist.TechnicalSpecialistId },
                                                domTimesheetsEx => new { TimesheetID = domTimesheetsEx.TimesheetId.GetValueOrDefault(), Pin = Convert.ToInt32(domTimesheetsEx.Pin) },
                                               (dbTimesheetsEx, domTimesheetsEx) => new { dbTimesheetsEx, domTimesheetsEx })
                                               .Where(x => x.dbTimesheetsEx.Id != x.domTimesheetsEx.TimesheetTechnicalSpecialistAccountExpenseId)
                                               .Select(x => x.dbTimesheetsEx)
                                               .ToList();

            return dbTimeExpense;
        }

        //To be deleted later. Added only for sync purpose
        // public long? GetMaxEvoId()
        //{
        //    long? visitTechSpecExId=_dbContext.VisitTechnicalSpecialistAccountItemExpense.FromSql("SELECT TOP 1 * FROM visit.VisitTechnicalSpecialistAccountItemExpense ORDER By ID DESC")?.FirstOrDefault()?.Evoid ?? 0;
        //    var dbTimesheetTSE=_dbContext.TimesheetTechnicalSpecialistAccountItemExpense.FromSql("SELECT TOP 1 * FROM timesheet.TimesheetTechnicalSpecialistAccountItemExpense ORDER By ID DESC")?.Select(x=> new DbModel.TimesheetTechnicalSpecialistAccountItemExpense() {Id=x.Id,Evoid=x.Evoid});
        //    long? timesheetTechSpecExId= dbTimesheetTSE.ToList().FirstOrDefault().Evoid ?? 0;

        //    if (visitTechSpecExId == 0 && timesheetTechSpecExId == 0)
        //        return  dbTimesheetTSE.ToList().FirstOrDefault().Id;
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
            this._dbContext.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}

