using AutoMapper.QueryableExtensions;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.GenericDbRepository.Services;
using Evolution.Visit.Domain.Enums;
using Evolution.Visit.Domain.Interfaces.Data;
using Evolution.Visit.Domain.Models.Visits;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.Visit.Domain.Models.Visits;
using AutoMapper;
using Evolution.Logging.Interfaces;
using Evolution.Common.Helpers;
using Microsoft.EntityFrameworkCore;

namespace Evolution.Visit.Infrastructure.Data
{
    public class VisitReferencesRepository : GenericRepository<DbModel.VisitReference>, IVisitReferencesRepository
    {
        private readonly DbModel.EvolutionSqlDbContext _dbContext = null;
        private readonly IMapper _mapper = null;
        private readonly IAppLogger<VisitReferencesRepository> _logger = null;

        public VisitReferencesRepository(DbModel.EvolutionSqlDbContext dbContext, IMapper mapper, IAppLogger<VisitReferencesRepository> logger) : base(dbContext)
        {
            this._dbContext = dbContext;
            this._mapper = mapper;
            this._logger = logger;
        }

        public IList<DomainModel.VisitReference> Search(DomainModel.VisitReference searchModel)
        {
            var dbSearchModel = _mapper.Map<DomainModel.VisitReference, DbModel.VisitReference>(searchModel);
            var expression = dbSearchModel.ToExpression();
            var visitReference = _dbContext.VisitReference;
            IQueryable<DbModel.VisitReference> whereClause = null;

            if (searchModel.VisitId.HasValue)
                whereClause = visitReference.Where(x => x.Visit.Id == searchModel.VisitId);

            if (expression != null)
                return visitReference.Where(expression).ProjectTo<DomainModel.VisitReference>().ToList();
            return visitReference.ProjectTo<DomainModel.VisitReference>().ToList();
        }

        public int DeleteVisitReference(List<DomainModel.VisitReference> visitReferences)
        {
            var referenceTypeIds = visitReferences?.Where(x => x.VisitReferenceId != null && x.VisitReferenceId > 0)?.Select(x => Convert.ToInt32(x.VisitReferenceId))?.Distinct().ToList();
            return DeleteInvoiceReference(referenceTypeIds);
        }

        public int DeleteInvoiceReference(List<int> referenceTypeIds)
        {
            int count = 0;
            try
            {
                if (referenceTypeIds.Count > 0)
                {
                    var deleteStatement = string.Format(Utility.GetSqlQuery(SQLModuleType.Visit_Reference, SQLModuleActionType.Delete), string.Join(",", referenceTypeIds.ToString<int>()));
                    count = _dbContext.Database.ExecuteSqlCommand(deleteStatement);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), "VisitReferenceIds=" + referenceTypeIds.ToString<int>());
            }

            return count;
        }

        public IList<DbModel.VisitReference> IsUniqueVisitReference(IList<DomainModel.VisitReference> visitReferenceTypes,
                                                                            IList<DbModel.VisitReference> dbVisitReference,
                                                                            ValidationType validationType)
        {
            IList<DbModel.VisitReference> dbSpecificTimeRef = null;
            IList<DbModel.VisitReference> dbTimeRef = null;
            var visitReferences = visitReferenceTypes?.Where(x => !string.IsNullOrEmpty(x.ReferenceType))?
                                                                  .Select(x => x.VisitId)?
                                                                  .ToList();

            if (dbVisitReference == null && validationType != ValidationType.Add)
            {
                dbSpecificTimeRef = _dbContext.VisitReference?.Where(x => visitReferences.Contains(x.VisitId)).ToList();
            }
            else
                dbSpecificTimeRef = dbVisitReference;

            if (dbSpecificTimeRef?.Count > 0)
                dbTimeRef = dbSpecificTimeRef.Join(visitReferenceTypes.ToList(),
                                                 dbVisitRef => new { VisitID = dbVisitRef.VisitId, Reference = dbVisitRef.AssignmentReferenceType.Name },
                                                 domVisitRef => new { VisitID = (long)domVisitRef.VisitId, Reference = domVisitRef.ReferenceType },
                                                (dbVisitRef, domVisitRef) => new { dbVisitRef, domVisitRef })
                                                .Where(x => x.dbVisitRef.Id != x.domVisitRef.VisitReferenceId)
                                                .Select(x => x.dbVisitRef)
                                                .ToList();

            return dbTimeRef;
        }

        //To be deleted later. Added only for sync purpose
        public long? GetMaxEvoId()
        {
            //var dbVisitRef = _dbContext.VisitReference.FromSql("SELECT TOP 1 * FROM visit.VisitReference ORDER By ID DESC")?.Select(x => new DbModel.VisitReference() { Id = x.Id, Evoid = x.Evoid });
            var dbVisitRef = _dbContext.VisitReference.OrderByDescending(x => x.Id)?.AsNoTracking()?.Select(x => new { x.Id, x.Evoid });
            long? visitRefId = dbVisitRef?.FirstOrDefault()?.Evoid ?? 0;
            //var dbTimesheetRef = _dbContext.TimesheetReference.FromSql("SELECT TOP 1 * FROM timesheet.TimesheetReference ORDER By ID DESC")?.Select(x => new DbModel.TimesheetReference() { Id = x.Id, Evoid = x.Evoid });
            var dbTimesheetRef = _dbContext.TimesheetReference.OrderByDescending(x => x.Id)?.AsNoTracking()?.Select(x => new { x.Id, x.Evoid });
            long? timesheetRefId = dbTimesheetRef?.FirstOrDefault()?.Evoid ?? 0;

            if (visitRefId == 0 && timesheetRefId == 0)
                return dbTimesheetRef?.FirstOrDefault()?.Id;
            if (visitRefId > timesheetRefId)
                return visitRefId;
            else
                return timesheetRefId;
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }
    }
}
