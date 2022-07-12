using System.Collections.Generic;
using DbModel=Evolution.DbRepository.Models.SqlDatabaseContext;
using Evolution.GenericDbRepository.Services;
using Evolution.Project.Domain.Interfaces.Data;
using DomainModel = Evolution.Project.Domain.Models.Projects;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Evolution.Common.Extensions;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Evolution.Project.Infrastructure.Data
{ 
    public class ProjectInvoiceAttachmentRepository: GenericRepository<DbModel.ProjectInvoiceAttachment>, IProjectInvoiceAttachmentRepository
    {
        private IMapper _mapper = null;
        private DbModel.EvolutionSqlDbContext _dbContext = null;
        public ProjectInvoiceAttachmentRepository(IMapper mapper,DbModel.EvolutionSqlDbContext dbContext):base(dbContext)
        {
            _mapper = mapper;
            _dbContext = dbContext;
        }

        public IList<DomainModel.ProjectInvoiceAttachment> Search(DomainModel.ProjectInvoiceAttachment searchModel)
        {
            var dbSearchModel = _mapper.Map<DbModel.ProjectInvoiceAttachment>(searchModel);
            var expression = dbSearchModel.ToExpression();

            IQueryable<DbModel.ProjectInvoiceAttachment> whereClause = null;
            var ProjectInvoiceAttachment = _dbContext.ProjectInvoiceAttachment;

            if (searchModel.ProjectNumber.HasValue)
                whereClause = ProjectInvoiceAttachment.Where(x => x.Project.ProjectNumber == searchModel.ProjectNumber);

            if (searchModel.DocumentType.HasEvoWildCardChar())
                whereClause = whereClause.WhereLike(x => x.DocumentType.Name, searchModel.DocumentType, '*');
            else
                whereClause = whereClause.Where(x => string.IsNullOrEmpty(searchModel.DocumentType) || x.DocumentType.Name == searchModel.DocumentType);

            if (expression != null)
                return whereClause.Where(expression).ProjectTo<DomainModel.ProjectInvoiceAttachment>().ToList();
            return whereClause.ProjectTo<DomainModel.ProjectInvoiceAttachment>().ToList();
        }
    }
}
