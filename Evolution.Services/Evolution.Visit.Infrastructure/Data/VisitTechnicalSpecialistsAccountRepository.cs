using AutoMapper.QueryableExtensions;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.GenericDbRepository.Services;
using Evolution.Visit.Domain.Interfaces.Data;
using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.Visit.Domain.Models.Visits;
using Microsoft.EntityFrameworkCore;

namespace Evolution.Visit.Infrastructure.Data
{
    public class VisitTechnicalSpecialistsAccountRepository : GenericRepository<DbModel.VisitTechnicalSpecialist>, IVisitTechnicalSpecialistsAccountRepository
    {
        private readonly DbModel.EvolutionSqlDbContext _dbContext = null;
        private readonly IMapper _mapper = null;

        public VisitTechnicalSpecialistsAccountRepository(DbModel.EvolutionSqlDbContext dbContext, IMapper mapper) : base(dbContext)
        {
            this._dbContext = dbContext;
            this._mapper = mapper;
        }

        public IList<DomainModel.VisitTechnicalSpecialist> Search(DomainModel.VisitTechnicalSpecialist searchModel,
                                                                      params string[] includes)
        {
            var dbSearchModel = _mapper.Map<DomainModel.VisitTechnicalSpecialist, DbModel.VisitTechnicalSpecialist>(searchModel);
            var expression = dbSearchModel.ToExpression();
            var visitTechnicalSpecialist = _dbContext.VisitTechnicalSpecialist;
            IQueryable<DbModel.VisitTechnicalSpecialist> whereClause = null;

            if (searchModel.VisitId > 0)
                whereClause = visitTechnicalSpecialist.Where(x => x.Visit.Id == searchModel.VisitId);

            if (expression != null)
                return visitTechnicalSpecialist.Where(expression).ProjectTo<DomainModel.VisitTechnicalSpecialist>().ToList();
            return visitTechnicalSpecialist.ProjectTo<DomainModel.VisitTechnicalSpecialist>().ToList();
        }

        public IList<DbModel.VisitTechnicalSpecialist> IsUniqueVisitReference(IList<DomainModel.VisitTechnicalSpecialist> visitTechSpecTypes,
                                                                            IList<DbModel.VisitTechnicalSpecialist> dbVisitTecSpec,
                                                                            ValidationType validationType)
        {
            IList<DbModel.VisitTechnicalSpecialist> dbSpecificTimeTechSpecRef = null;
            IList<DbModel.VisitTechnicalSpecialist> dbTimeTechSpecRef = null;
            var visitTechSpec = visitTechSpecTypes?.Where(x => x.GrossMargin > 0)?
                                                                  .Select(x => x.VisitId)?
                                                                  .ToList();

            if (dbVisitTecSpec == null)
            {
                dbSpecificTimeTechSpecRef = _dbContext.VisitTechnicalSpecialist?.Where(x => visitTechSpec.Contains(x.VisitId)).ToList();
            }
            else
                dbSpecificTimeTechSpecRef = dbVisitTecSpec;

            if (dbSpecificTimeTechSpecRef?.Count > 0)
                dbTimeTechSpecRef = dbSpecificTimeTechSpecRef.Join(visitTechSpecTypes.ToList(),
                                                 dbVisitTechSpecRef => new { VisitID = dbVisitTechSpecRef.VisitId },
                                                 domVisitTechSpecRef => new { VisitID = (long)domVisitTechSpecRef.VisitId },
                                                (dbVisitTSRef, domVisitTSRef) => new { dbVisitTSRef, domVisitTSRef })
                                                .Where(x => x.dbVisitTSRef.Id != x.domVisitTSRef.VisitTechnicalSpecialistId)
                                                .Select(x => x.dbVisitTSRef)
                                                .ToList();

            return dbTimeTechSpecRef;
        }

        public List<DomainModel.ResourceInfo> IsEpinAssociated(DomainModel.VisitEmailData visitEmailData)
        {
            List<DomainModel.ResourceInfo> resourceInfo = null;
            var ePins = visitEmailData.VisitDetail.VisitInfo.TechSpecialists.Select(x => (int)x.Pin).ToList();


            var dbVisitTechSpec = _dbContext.Visit.Join(_dbContext.VisitTechnicalSpecialist,
                                                                dbVisits => new { VisitId = dbVisits.Id },
                                                                dbVisitTechSpecs => new { VisitId = dbVisitTechSpecs.VisitId },
                                                                (dbVisits, dbVisitTechSpecs) => new { dbVisits, dbVisitTechSpecs })
                                                 .Where(x => x.dbVisits.FromDate == visitEmailData.VisitDetail.VisitInfo.VisitStartDate && x.dbVisits.Id != visitEmailData.VisitDetail.VisitInfo.VisitId)
                                                             
                                                 .Select(x => new DomainModel.VisitTechInfo()
                                                 {
                                                     FirstName = x.dbVisitTechSpecs.TechnicalSpecialist.FirstName,
                                                     MiddleName = x.dbVisitTechSpecs.TechnicalSpecialist.MiddleName,
                                                     LastName = x.dbVisitTechSpecs.TechnicalSpecialist.LastName,
                                                     Pin = x.dbVisitTechSpecs.TechnicalSpecialist.Pin,
                                                     resourceAdditionalInfo = new DomainModel.ResourceAdditionalInfo()
                                                     {
                                                         VisitProjectNumber = x.dbVisits.Assignment.Project.ProjectNumber,
                                                         VisitAssignmentId = x.dbVisits.Assignment.Id,
                                                         VisitAssignmentNumber = x.dbVisits.Assignment.AssignmentNumber,
                                                         VisitNumber = x.dbVisits.VisitNumber,
                                                         VisitId = x.dbVisits.Id,
                                                         VisitSupplierPOId = x.dbVisits.Assignment.SupplierPurchaseOrder.Id
                                                     },
                                                 })
                                                 .ToList();

            var ePinAlreadyAssociated = dbVisitTechSpec?.ToList().Select(x => x.Pin).Intersect(ePins)?.ToList();
            if (ePinAlreadyAssociated?.Count > 0)
            {
                if (resourceInfo == null)
                    resourceInfo = new List<DomainModel.ResourceInfo>();

                ePinAlreadyAssociated.ToList().ForEach(x =>
                {
                    DomainModel.ResourceInfo tempResourceInfo = new DomainModel.ResourceInfo();
                    var dbData = dbVisitTechSpec.ToList().Where(x1 => x1.Pin == x);
                    tempResourceInfo.TechSpecName = dbData.FirstOrDefault().FullName;
                    tempResourceInfo.Pin = x;
                    tempResourceInfo.ResourceAdditionalInfos = dbData.Select(x1 => x1.resourceAdditionalInfo).ToList();
                    resourceInfo.Add(tempResourceInfo);
                });
            }

            return resourceInfo;
        }

        public IList<DomainModel.VisitTechnicalSpecialist> GetTechSpecForAssignment(List<long?> visitIds)
        {
            var visitTechnicalSpecialist = _dbContext.VisitTechnicalSpecialist;
            IQueryable<DbModel.VisitTechnicalSpecialist> whereClause = null;
            whereClause = visitTechnicalSpecialist.Where(x => visitIds.Contains(x.Visit.Id));
            return whereClause.ProjectTo<DomainModel.VisitTechnicalSpecialist>().ToList();
        }

        //To be deleted later. Added only for sync purpose
        public long? GetMaxEvoId()
        {
            //var dbVisitTS = _dbContext.VisitTechnicalSpecialist.FromSql("SELECT TOP 1 * FROM visit.VisitTechnicalSpecialist ORDER By ID DESC")?.Select(x => new DbModel.VisitTechnicalSpecialist() { Id = x.Id, Evoid = x.Evoid });
            var dbVisitTS = _dbContext.VisitTechnicalSpecialist.OrderByDescending(x => x.Id)?.AsNoTracking()?.Select(x => new { Id = x.Id, Evoid = x.Evoid });
            long? visitTechSpecId = dbVisitTS?.FirstOrDefault()?.Evoid ?? 0;
            //var dbTimesheetTS = _dbContext.TimesheetTechnicalSpecialist.FromSql("SELECT TOP 1 * FROM timesheet.TimesheetTechnicalSpecialist ORDER By ID DESC")?.Select(x => new DbModel.TimesheetTechnicalSpecialist() { Id = x.Id, Evoid = x.Evoid });
            var dbTimesheetTS = _dbContext.TimesheetTechnicalSpecialist.OrderByDescending(x => x.Id)?.AsNoTracking()?.Select(x => new { Id = x.Id, Evoid = x.Evoid });
            long? timesheetTechSpecId = dbTimesheetTS?.FirstOrDefault()?.Evoid ?? 0;

            if (visitTechSpecId == 0 && timesheetTechSpecId == 0)
                return dbTimesheetTS?.FirstOrDefault()?.Id;
            if (visitTechSpecId > timesheetTechSpecId)
                return visitTechSpecId;
            else
                return timesheetTechSpecId;
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }
    }
}
