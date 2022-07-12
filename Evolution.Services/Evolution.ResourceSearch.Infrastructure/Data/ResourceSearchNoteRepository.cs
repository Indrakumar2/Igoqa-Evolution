using AutoMapper;
using AutoMapper.QueryableExtensions;
using Evolution.Common.Extensions;
using Evolution.DbRepository.Models.SqlDatabaseContext;
using Evolution.GenericDbRepository.Services;
using Evolution.ResourceSearch.Domain.Interfaces.Data;
using System.Collections.Generic;
using System.Linq;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.ResourceSearch.Domain.Models.ResourceSearch;

namespace Evolution.ResourceSearch.Infrastructure.Data
{
    public class ResourceSearchNoteRepository : GenericRepository<DbModel.ResourceSearchNote>, IResourceSearchNoteRepository
    {
        private readonly DbModel.EvolutionSqlDbContext _dbContext = null;
        private readonly IMapper _mapper = null;

        public ResourceSearchNoteRepository(EvolutionSqlDbContext dbContext, IMapper mapper) : base(dbContext)
        {
            this._dbContext = dbContext;
            this._mapper = mapper;
        }

        public IList<DomainModel.ResourceSearchNote> Search(DomainModel.ResourceSearchNote searchModel)
        {
            var resourceSearchIds = _dbContext.ResourceSearch.Where(x => x.AssignmentId == searchModel.AssignmentId)?.ToList()?.Select(x => x.Id)?.ToList();
            return _dbContext.ResourceSearchNote.Where(x => resourceSearchIds.Contains(x.ResourceSearchId))?.ProjectTo<DomainModel.ResourceSearchNote>().ToList();
            //var dbSearchModel = this._mapper.Map<DbModel.ResourceSearchNote>(searchModel);
            //var expression = dbSearchModel.ToExpression();
            //return _dbContext.ResourceSearchNote.Where(expression).ProjectTo<DomainModel.ResourceSearchNote>().ToList();
        }

        public IList<DomainModel.ResourceSearchNote> Get(IList<int> ResourceSearchIds, bool fetchLatest = false)
        {
            IQueryable<DbModel.ResourceSearchNote> noteAsQuerable = null;
             
            if (fetchLatest)
            {
                noteAsQuerable = _dbContext.ResourceSearchNote.GroupJoin(_dbContext.ResourceSearchNote,
                                                           ln => new { ln.ResourceSearchId },
                                                           rn => new { rn.ResourceSearchId },
                                                           (ln, rn) => new { ln, rn })
                                                           .Where(x => x.ln.CreatedDate == x.rn.Max(x1 => x1.CreatedDate) && ResourceSearchIds.Contains(x.ln.ResourceSearchId))
                                                           .Select(x => x.ln);
            }
            else
            {
                noteAsQuerable = _dbContext.ResourceSearchNote.Where(x => ResourceSearchIds.Contains(x.ResourceSearchId));
            }
             
            return noteAsQuerable.ProjectTo<DomainModel.ResourceSearchNote>().ToList();
        }
    }
}
