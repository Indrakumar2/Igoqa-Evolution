//using AutoMapper;
//using AutoMapper.QueryableExtensions;
//using Evolution.Common.Extensions;
//using Evolution.DbRepository.Models.SqlDatabaseContext;
//using Evolution.GenericDbRepository.Services;
//using Evolution.TechnicalSpecialist.Domain.Interfaces.Data;
//using System.Collections.Generic;
//using System.Linq;
//using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
//using DomainModel = Evolution.TechnicalSpecialist.Domain.Models.TechSpecialist;

//namespace Evolution.TechnicalSpecialist.Infrastructure.Data
//{
//   public class TechnicalSpecialistSensitiveDocumentRepository : GenericRepository<DbModel.TechnicalSpecialistSensitiveDocument>, ITechnicalSpecialistSensitiveDocumentRepository
//    {
//        private readonly DbModel.EvolutionSqlDbContext _dbContext = null;
//        private readonly IMapper _mapper = null;
//        /// <summary>
//        /// TODO :  Replace Object to DBContext
//        /// </summary>
//        /// <param name="dbContext"></param>
//        public TechnicalSpecialistSensitiveDocumentRepository(EvolutionSqlDbContext dbContext, IMapper mapper) : base(dbContext)
//        {
//            this._dbContext = dbContext;
//            this._mapper = mapper;
//        }

//        public IList<DomainModel.TechnicalSpecialistSensitiveDocument> Search(DomainModel.TechnicalSpecialistSensitiveDocument searchModel)
//        {
//            var dbSearchModel = this._mapper.Map<DbModel.TechnicalSpecialistSensitiveDocument>(searchModel);
//            IQueryable<DbModel.TechnicalSpecialistSensitiveDocument> whereClause = null;

//            var sensitiveDocument = _dbContext.TechnicalSpecialistSensitiveDocument;

//            //Wildcard Search for Comments
//            if (searchModel.Comments.HasEvoWildCardChar())
//                whereClause = sensitiveDocument.WhereLike(x => x.Comments, searchModel.Comments, '*');
//            else
//                whereClause = sensitiveDocument.Where(x => string.IsNullOrEmpty(searchModel.Comments) || x.Comments == searchModel.Comments);


//            var expression = dbSearchModel.ToExpression();
//            if (expression == null)
//                return whereClause.ProjectTo<DomainModel.TechnicalSpecialistSensitiveDocument>().ToList();
//            else
//                return whereClause.Where(expression).ProjectTo<DomainModel.TechnicalSpecialistSensitiveDocument>().ToList();
//        }
//    }
//}
