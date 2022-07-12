using AutoMapper;
using AutoMapper.QueryableExtensions;
using Evolution.Assignment.Domain.Interfaces.Data;
using Evolution.Assignment.Domain.Models.Assignments;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.DbRepository.Models.SqlDatabaseContext;
using Evolution.GenericDbRepository.Services;
using System.Collections.Generic;
using System.Linq;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.Assignment.Domain.Models.Assignments;

namespace Evolution.Assignment.Infrastructure.Data
{
    public class AssignmentReferenceRepository : GenericRepository<DbModel.AssignmentReference>, IAssignmentReferenceTypeRepository
    {
        private  EvolutionSqlDbContext _dbContext = null;
        private  IMapper _mapper = null;

        public AssignmentReferenceRepository(EvolutionSqlDbContext dbContext, IMapper mapper) : base(dbContext)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public IList<AssignmentReferenceType> Search(AssignmentReferenceType searchModel)
        {
            var dbSearchModel = _mapper.Map<DomainModel.AssignmentReferenceType, DbModel.AssignmentReference>(searchModel);
            var expression = dbSearchModel.ToExpression();
            var assignmentReference = _dbContext.AssignmentReference;
            IQueryable<DbModel.AssignmentReference> whereClause = null;


            if (searchModel.AssignmentId.HasValue)
            {
                whereClause = assignmentReference.Where(x => x.Assignment.Id == searchModel.AssignmentId);
            }

            //ToDo with assignmentReferenceName

            if (expression != null)
                return assignmentReference.Where(expression).ProjectTo<DomainModel.AssignmentReferenceType>().ToList();
            return assignmentReference.ProjectTo<DomainModel.AssignmentReferenceType>().ToList();
        }

        public IList<AssignmentReference> IsUniqueAssignmentReference(IList<AssignmentReferenceType> assignmentReferenceTypes,
                                                                      IList<DbModel.AssignmentReference> dbAssignmentRefrenceType,
                                                                      ValidationType validationType)
        {
            IList<DbModel.AssignmentReference> dbSpecificAssgmtRef = null;
            IList<DbModel.AssignmentReference> dbAssgmtRef = null;
            var assignmentReferences = assignmentReferenceTypes?.Where(x => !string.IsNullOrEmpty(x.ReferenceType))?
                                                                  .Select(x => x.AssignmentId)?
                                                                  .ToList();

            if (dbAssignmentRefrenceType == null && validationType != ValidationType.Add)
            {
                dbSpecificAssgmtRef = _dbContext.AssignmentReference?.Where(x => assignmentReferences.Contains(x.AssignmentId)).ToList();
            }
            else
                dbSpecificAssgmtRef = dbAssignmentRefrenceType;

            if(dbSpecificAssgmtRef?.Count > 0)
                dbAssgmtRef= dbSpecificAssgmtRef.Join(assignmentReferenceTypes.ToList(),
                                                 dbAssigmtRef => new { AssignmentID = dbAssigmtRef.AssignmentId, Reference = dbAssigmtRef.AssignmentReferenceType.Name },
                                                 domAssigmtRef => new { AssignmentID = (int)domAssigmtRef.AssignmentId, Reference = domAssigmtRef.ReferenceType },
                                                (dbAssigmtRef, domAssigmtRef) => new { dbAssigmtRef, domAssigmtRef })
                                                .Where(x => x.dbAssigmtRef.Id != x.domAssigmtRef.AssignmentReferenceTypeId)
                                                .Select(x => x.dbAssigmtRef)
                                                .ToList();

            return dbAssgmtRef;
        }

        public void Dispose()
        {
            _dbContext.Dispose();
            _mapper = null;
        }
    }
}
