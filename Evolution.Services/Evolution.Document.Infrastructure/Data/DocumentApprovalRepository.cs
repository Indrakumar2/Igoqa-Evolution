using AutoMapper;
using AutoMapper.QueryableExtensions;
using Evolution.Common.Extensions;
using Evolution.DbRepository.Models.SqlDatabaseContext;
using Evolution.GenericDbRepository.Services;
using Evolution.Document.Domain.Interfaces.Data;
using System.Collections.Generic;
using System.Linq;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.Document.Domain.Models.Document;

namespace Evolution.Document.Infrastructure.Data
{
    public class DocumentApprovalRepository : GenericRepository<DbModel.DocumentApproval>, IDocumentApprovalRepository
    {
        private readonly DbModel.EvolutionSqlDbContext _dbContext = null;
        private readonly IMapper _mapper = null;

        public DocumentApprovalRepository(EvolutionSqlDbContext dbContext, IMapper mapper) : base(dbContext)
        {
            this._dbContext = dbContext;
            this._mapper = mapper;
        }

        public IList<DomainModel.DocumentApproval> Search(DomainModel.DocumentApproval searchModel)
        {
            var dbSearchModel = this._mapper.Map<DbModel.DocumentApproval>(searchModel);
            IQueryable<DbModel.DocumentApproval> whereClause = null;

            var compNotes = _dbContext.DocumentApproval;
            
            if (searchModel.DocumentApprovedBy.HasEvoWildCardChar())
                whereClause = compNotes.WhereLike(x => x.Coordinator.Name, searchModel.DocumentApprovedBy, '*');
            else
                whereClause = compNotes.Where(x => string.IsNullOrEmpty(searchModel.DocumentApprovedBy) || x.Coordinator.Name == searchModel.DocumentApprovedBy);
            
            //if (searchModel.DocumentUploadedBy.HasEvoWildCardChar())
            //    whereClause = whereClause.WhereLike(x => x.UploadedUser.Name, searchModel.DocumentUploadedBy, '*');
            //else
            //    whereClause = whereClause.Where(x => string.IsNullOrEmpty(searchModel.DocumentUploadedBy) || x.UploadedUser.Name == searchModel.DocumentUploadedBy);

            var expression = dbSearchModel.ToExpression();
            if (expression == null)
                return whereClause.ProjectTo<DomainModel.DocumentApproval>().ToList();
            else
                return whereClause.Where(expression).ProjectTo<DomainModel.DocumentApproval>().ToList();
        }
    }
}