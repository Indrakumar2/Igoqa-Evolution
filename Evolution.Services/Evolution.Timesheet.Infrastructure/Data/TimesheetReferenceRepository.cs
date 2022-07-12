using AutoMapper;
using AutoMapper.QueryableExtensions;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.DbRepository.Models.SqlDatabaseContext;
using Evolution.GenericDbRepository.Services;
using Evolution.Timesheet.Domain.Interfaces.Data;
using Evolution.Timesheet.Domain.Models.Timesheets;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.Timesheet.Domain.Models.Timesheets;

namespace Evolution.Timesheet.Infrastructure.Data
{
    public class TimesheetReferenceRepository : GenericRepository<DbModel.TimesheetReference>, ITimesheetReferenceRepository
    {
        private readonly EvolutionSqlDbContext _dbContext = null;
        private readonly IMapper _mapper = null;
        public TimesheetReferenceRepository(EvolutionSqlDbContext dbContext, IMapper mapper) : base(dbContext)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public IList<TimesheetReferenceType> Search(TimesheetReferenceType searchModel)
        {
            var dbSearchModel = _mapper.Map<DomainModel.TimesheetReferenceType, DbModel.TimesheetReference>(searchModel);
            var expression = dbSearchModel.ToExpression();
            // var timesheetReference = _dbContext.TimesheetReference;
            IQueryable<DbModel.TimesheetReference> whereClause = _dbContext.TimesheetReference;

            if (searchModel.TimesheetId.HasValue)
                whereClause = whereClause.Where(x => x.Timesheet.Id == searchModel.TimesheetId);

            if (expression != null)
                return whereClause.Where(expression).ProjectTo<DomainModel.TimesheetReferenceType>().ToList();
            return whereClause.ProjectTo<DomainModel.TimesheetReferenceType>().ToList();
        }

        public IList<DbModel.TimesheetReference> IsUniqueTimesheetReference(IList<DomainModel.TimesheetReferenceType> timesheetReferenceTypes,
                                                                            IList<DbModel.TimesheetReference> dbTimesheetReference,
                                                                            ValidationType validationType)
        {
            IList<DbModel.TimesheetReference> dbSpecificTimeRef = null;
            IList<DbModel.TimesheetReference> dbTimeRef = null;
            var timesheetReferences = timesheetReferenceTypes?.Where(x => !string.IsNullOrEmpty(x.ReferenceType))?
                                                                  .Select(x => x.TimesheetId)?
                                                                  .ToList();

            if (dbTimesheetReference == null && validationType != ValidationType.Add)
            {
                dbSpecificTimeRef = _dbContext.TimesheetReference?.Where(x => timesheetReferences.Contains(x.TimesheetId)).ToList();
            }
            else
                dbSpecificTimeRef = dbTimesheetReference;

            if (dbSpecificTimeRef?.Count > 0)
                dbTimeRef = dbSpecificTimeRef.Join(timesheetReferenceTypes.ToList(),
                                                 dbTimesheetRef => new { TimesheetID = dbTimesheetRef.TimesheetId, Reference = dbTimesheetRef.AssignmentReferenceType.Name },
                                                 domTimesheetRef => new { TimesheetID = (long)domTimesheetRef.TimesheetId, Reference = domTimesheetRef.ReferenceType },
                                                (dbTimesheetRef, domTimesheetRef) => new { dbTimesheetRef, domTimesheetRef })
                                                .Where(x => x.dbTimesheetRef.Id != x.domTimesheetRef.TimesheetReferenceId)
                                                .Select(x => x.dbTimesheetRef)
                                                .ToList();

            return dbTimeRef;
        }

        //To be deleted later. Added only for sync purpose
        public long? GetMaxEvoId()
        {
            //long? visitRefId = _dbContext.VisitReference.FromSql("SELECT TOP 1 * FROM visit.VisitReference ORDER By ID DESC")?.FirstOrDefault()?.Evoid ?? 0;
            var dbVisitRef = _dbContext.VisitReference.FromSql("SELECT TOP 1 Id,EvoId FROM visit.VisitReference with(nolock) ORDER By ID DESC")?.AsNoTracking()?.Select(x => new DbModel.VisitReference() { Id = x.Id, Evoid = x.Evoid });
            long? visitRefId = dbVisitRef?.ToList()?.FirstOrDefault()?.Evoid ?? 0;
            var dbTimesheetRef = _dbContext.TimesheetReference.FromSql("SELECT TOP 1 Id,EvoId FROM timesheet.TimesheetReference with(nolock) ORDER By ID DESC")?.AsNoTracking()?.Select(x => new DbModel.TimesheetReference() { Id = x.Id, Evoid = x.Evoid })?.FirstOrDefault();
            long? timesheetRefId = dbTimesheetRef?.Evoid ?? 0;

            if (visitRefId == 0 && timesheetRefId == 0)
                return dbTimesheetRef?.Id;
            if (visitRefId > timesheetRefId)
                return visitRefId;
            else
                return timesheetRefId;
        }
        public void Dispose()
        {
            this._dbContext.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
