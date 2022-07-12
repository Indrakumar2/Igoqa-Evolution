using AutoMapper;
using AutoMapper.QueryableExtensions;
using Evolution.Common.Extensions;
using Evolution.DbRepository.Models.SqlDatabaseContext;
using Evolution.GenericDbRepository.Services;
using Evolution.Project.Domain.Interfaces.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using DomainModel = Evolution.Project.Domain.Models.Projects;

namespace Evolution.Project.Infrastructure.Data
{ 
    public class ProjectInvoiceReferenceRepository : GenericRepository<ProjectInvoiceAssignmentReference>, IProjectInvoiceReferenceRepository
    { 
        private EvolutionSqlDbContext _dbContext = null;
        private IMapper _mapper = null;

        public ProjectInvoiceReferenceRepository(IMapper mapper, EvolutionSqlDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public IList<DomainModel.ProjectInvoiceReference> Search(DomainModel.ProjectInvoiceReference searchModel)
        {
              
            IQueryable<ProjectInvoiceAssignmentReference> whereClause = _dbContext.ProjectInvoiceAssignmentReference;

            if (searchModel.ProjectInvoiceReferenceTypeId.HasValue)
                whereClause = whereClause.Where(x => x.Id == searchModel.ProjectInvoiceReferenceTypeId);

            if (searchModel.ProjectNumber.HasValue)
                whereClause = whereClause.Where(x => x.ProjectId == searchModel.ProjectNumber);

            if (searchModel.ReferenceType.HasEvoWildCardChar())
                whereClause = whereClause.WhereLike(x => x.AssignmentReferenceType.Name, searchModel.ReferenceType, '*');

            else if(!string.IsNullOrEmpty(searchModel.ReferenceType) && !searchModel.ReferenceType.HasEvoWildCardChar())
                whereClause = whereClause.Where(x => string.IsNullOrEmpty(searchModel.ReferenceType) || x.AssignmentReferenceType.Name == searchModel.ReferenceType);

            if (searchModel.IsVisibleToAssignment.HasValue)
                whereClause = whereClause.Where(x => x.IsAssignment == searchModel.IsVisibleToAssignment);

            if (searchModel.IsVisibleToVisit.HasValue)
                whereClause = whereClause.Where(x => x.IsVisit == searchModel.IsVisibleToVisit);

            if (searchModel.IsVisibleToTimesheet.HasValue)
                whereClause = whereClause.Where(x => x.IsTimesheet == searchModel.IsVisibleToTimesheet);

            return whereClause.ProjectTo<DomainModel.ProjectInvoiceReference>().ToList();

        }
    }
}
