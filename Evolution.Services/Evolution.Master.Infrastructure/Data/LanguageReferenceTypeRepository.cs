using AutoMapper;
using AutoMapper.QueryableExtensions;
using Evolution.Common.Extensions;
using Evolution.DbRepository.Models.SqlDatabaseContext;
using Evolution.GenericDbRepository.Services;
using Evolution.Master.Domain.Interfaces.Data;
using System.Collections.Generic;
using System.Linq;
using dbModel = Evolution.DbRepository.Models.SqlDatabaseContext;

namespace Evolution.Master.Infrastructure.Data
{
    public class LanguageReferenceTypeRepository :GenericRepository<dbModel.LanguageReferenceType>,ILanguageReferenceTypeRepository
    {
        private readonly EvolutionSqlDbContext _dbContext = null;
        private IMapper _mapper = null;

        public LanguageReferenceTypeRepository(EvolutionSqlDbContext dbContext, IMapper mapper) : base(dbContext)
        {
             this._dbContext = dbContext;
            _mapper = mapper;
        }

        public IList<Domain.Models.LanguageReferenceType> Search(Domain.Models.LanguageReferenceType searchModel)
        {
             var dbSearchModel = _mapper.Map<dbModel.LanguageReferenceType>(searchModel);
          
            IQueryable<dbModel.LanguageReferenceType> whereClause = null;
            var languagreRefType = _dbContext.LanguageReferenceType;

            

            //Wildcard Search for RefType
            if (searchModel.ReferenceType.HasEvoWildCardChar())
                whereClause = whereClause.WhereLike(x => x.ReferenceType.Name, searchModel.ReferenceType, '*');
            else
                whereClause = whereClause.Where(x => string.IsNullOrEmpty(searchModel.ReferenceType) || x.ReferenceType.Name == searchModel.ReferenceType);

             if (searchModel.Language.HasEvoWildCardChar())
                whereClause = whereClause.WhereLike(x => x.Language.Name, searchModel.Language, '*');
            else
                whereClause = whereClause.Where(x => string.IsNullOrEmpty(searchModel.Language) || x.Language.Name == searchModel.ReferenceType);
                        
            return whereClause.ProjectTo<Domain.Models.LanguageReferenceType>().ToList();
        }
    }
   
}