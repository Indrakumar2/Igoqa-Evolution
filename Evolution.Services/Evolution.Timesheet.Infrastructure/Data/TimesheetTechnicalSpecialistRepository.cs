using AutoMapper;
using AutoMapper.QueryableExtensions;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.DbRepository.Models.SqlDatabaseContext;
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
    public class TimesheetTechnicalSpecialistRepository : GenericRepository<DbModel.TimesheetTechnicalSpecialist>, ITimesheetTechnicalSpecialistRepository
    {
        private readonly EvolutionSqlDbContext _dbContext = null;
        private readonly IMapper _mapper = null;
        public TimesheetTechnicalSpecialistRepository(EvolutionSqlDbContext dbContext, IMapper mapper) : base(dbContext)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public IList<DomainModel.TimesheetTechnicalSpecialist> Search(DomainModel.TimesheetTechnicalSpecialist searchModel,
                                                                      params string[] includes)
        {
            var dbSearchModel = _mapper.Map<DomainModel.TimesheetTechnicalSpecialist, DbModel.TimesheetTechnicalSpecialist>(searchModel);
            var expression = dbSearchModel.ToExpression();
            var timesheetTechnicalSpecialist = _dbContext.TimesheetTechnicalSpecialist;
            IQueryable<DbModel.TimesheetTechnicalSpecialist> whereClause = null;

            if (searchModel.TimesheetId > 0)
                whereClause = timesheetTechnicalSpecialist.Where(x => x.Timesheet.Id == searchModel.TimesheetId);

            if (expression != null)
                return timesheetTechnicalSpecialist.Where(expression).ProjectTo<DomainModel.TimesheetTechnicalSpecialist>().ToList();
            return timesheetTechnicalSpecialist.ProjectTo<DomainModel.TimesheetTechnicalSpecialist>().ToList();
        }


        public IList<DbModel.TimesheetTechnicalSpecialist> IsUniqueTimesheetTechspec(IList<DomainModel.TimesheetTechnicalSpecialist> timesheetTechSpecTypes,
                                                                            IList<DbModel.TimesheetTechnicalSpecialist> dbTimesheetTecSpec,
                                                                            ValidationType validationType)
        {
            IList<DbModel.TimesheetTechnicalSpecialist> dbSpecificTimeTechSpecRef = null;
            IList<DbModel.TimesheetTechnicalSpecialist> dbTimeTechSpecRef = null;
            var timesheetTechSpec = timesheetTechSpecTypes?.Where(x => x.Pin > 0)?
                                                                  .Select(x => x.TimesheetId)?
                                                                  .ToList();

            if (dbTimesheetTecSpec == null && validationType != ValidationType.Add)
            {
                dbSpecificTimeTechSpecRef = _dbContext.TimesheetTechnicalSpecialist?.Where(x => timesheetTechSpec.Contains(x.TimesheetId)).ToList();
            }
            else
                dbSpecificTimeTechSpecRef = dbTimesheetTecSpec;

            if (dbSpecificTimeTechSpecRef?.Count > 0)
                dbTimeTechSpecRef = dbSpecificTimeTechSpecRef.Join(timesheetTechSpecTypes.ToList(),
                                                 dbTimesheetTechSpecRef => new { TimesheetID = dbTimesheetTechSpecRef.TimesheetId, Pin = dbTimesheetTechSpecRef.TechnicalSpecialist.Pin },
                                                 domTimesheetTechSpecRef => new { TimesheetID = (long)domTimesheetTechSpecRef.TimesheetId, Pin = domTimesheetTechSpecRef.Pin },
                                                (dbTimesheetTSRef, domTimesheetTSRef) => new { dbTimesheetTSRef, domTimesheetTSRef })
                                                .Where(x => x.dbTimesheetTSRef.Id != x.domTimesheetTSRef.TimesheetTechnicalSpecialistId)
                                                .Select(x => x.dbTimesheetTSRef)
                                                .ToList();

            return dbTimeTechSpecRef;
        }

        public List<DomainModel.ResourceInfo> IsEpinAssociated(DomainModel.TimesheetEmailData timesheetEmailData)
        {
            List<DomainModel.ResourceInfo> resourceInfo = null;
            var ePins = timesheetEmailData.TimesheetDetail.TimesheetInfo.TechSpecialists.Select(x => (int)x.Pin).ToList();

            var dbTimesheetTechSpec = _dbContext.Timesheet.Join(_dbContext.TimesheetTechnicalSpecialist,
                                                                dbTimesheets => new { TimesheetId = dbTimesheets.Id },
                                                                dbTimesheetTechSpecs => new { TimesheetId = dbTimesheetTechSpecs.TimesheetId },
                                                                (dbTimesheets, dbTimesheetTechSpecs) => new { dbTimesheets, dbTimesheetTechSpecs })
                                                 .Where(x => x.dbTimesheets.FromDate == timesheetEmailData.TimesheetDetail.TimesheetInfo.TimesheetStartDate && x.dbTimesheets.Id != timesheetEmailData.TimesheetDetail.TimesheetInfo.TimesheetId)
                                                 .Select(x => new DomainModel.TimesheetTechInfo()
                                                 {
                                                     FirstName = x.dbTimesheetTechSpecs.TechnicalSpecialist.FirstName,
                                                     MiddleName = x.dbTimesheetTechSpecs.TechnicalSpecialist.MiddleName,
                                                     LastName = x.dbTimesheetTechSpecs.TechnicalSpecialist.LastName,
                                                     Pin = x.dbTimesheetTechSpecs.TechnicalSpecialist.Pin,
                                                     resourceAdditionalInfo = new DomainModel.ResourceAdditionalInfo()
                                                     {
                                                         TimesheetProjectNumber = x.dbTimesheets.Assignment.Project.ProjectNumber,
                                                         TimesheetAssignmentId = x.dbTimesheets.Assignment.Id,
                                                         TimesheetAssignmentNumber = x.dbTimesheets.Assignment.AssignmentNumber,
                                                         TimesheetNumber = x.dbTimesheets.TimesheetNumber,
                                                         TimesheetId = x.dbTimesheets.Id
                                                     },
                                                 })
                                                 .ToList();

            var ePinAlreadyAssociated = dbTimesheetTechSpec?.ToList().Select(x => x.Pin).Intersect(ePins)?.ToList();
            if (ePinAlreadyAssociated?.Count > 0)
            {
                if (resourceInfo == null)
                    resourceInfo = new List<DomainModel.ResourceInfo>();

                ePinAlreadyAssociated.ToList().ForEach(x =>
                {
                    DomainModel.ResourceInfo tempResourceInfo = new DomainModel.ResourceInfo();
                    var dbData = dbTimesheetTechSpec.ToList().Where(x1 => x1.Pin == x);
                    tempResourceInfo.TechSpecName = dbData.FirstOrDefault().FullName;
                    tempResourceInfo.Pin = x;
                    tempResourceInfo.ResourceAdditionalInfos = dbData.Select(x1 => x1.resourceAdditionalInfo).ToList();
                    resourceInfo.Add(tempResourceInfo);
                });
            }

            return resourceInfo;
        }

        public IList<DomainModel.TimesheetTechnicalSpecialist> GetTechSpecForAssignment(List<long?> timesheetIds)
        {            
            var timesheetTechnicalSpecialist = _dbContext.TimesheetTechnicalSpecialist;
            IQueryable<DbModel.TimesheetTechnicalSpecialist> whereClause = null;
            whereClause = timesheetTechnicalSpecialist.Where(x => timesheetIds.Contains(x.Timesheet.Id));            
            return whereClause.ProjectTo<DomainModel.TimesheetTechnicalSpecialist>().ToList();
        }

        //To be deleted later. Added only for sync purpose
        public long? GetMaxEvoId()
        {
            var visitTechSpec = _dbContext.VisitTechnicalSpecialist.FromSql("SELECT TOP 1 Id,EvoId FROM visit.VisitTechnicalSpecialist ORDER By ID DESC")?.AsNoTracking().Select(x => new DbModel.VisitTechnicalSpecialist() { Id = x.Id, Evoid = x.Evoid });
            long? visitTechSpecId = visitTechSpec?.FirstOrDefault()?.Evoid ?? 0;
            //long? visitTechSpecId = _dbContext.VisitTechnicalSpecialist.FromSql("SELECT TOP 1 * FROM visit.VisitTechnicalSpecialist ORDER By ID DESC")?.FirstOrDefault()?.Evoid ?? 0;
            var dbTimesheetTS = _dbContext.TimesheetTechnicalSpecialist.FromSql("SELECT TOP 1 Id,EvoId FROM timesheet.TimesheetTechnicalSpecialist ORDER By ID DESC")?.AsNoTracking().Select(x => new DbModel.TimesheetTechnicalSpecialist() { Id = x.Id, Evoid = x.Evoid })?.FirstOrDefault();
            long? timesheetTechSpecId = dbTimesheetTS?.Evoid ?? 0;

            if (visitTechSpecId == 0 && timesheetTechSpecId == 0)
                return dbTimesheetTS?.Id;
            if (visitTechSpecId > timesheetTechSpecId)
                return visitTechSpecId;
            else
                return timesheetTechSpecId;
        }
        public void Dispose()
        {
            this._dbContext.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
