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
// public  class TechnicalSpecialistCurriculumVitaeRepository : GenericRepository<DbModel.TechnicalSpecialistCurriculumVitae>

//    {
//        private readonly DbModel.EvolutionSqlDbContext _dbContext = null;
//        private readonly IMapper _mapper = null;
//        /// <summary>
//        /// TODO :  Replace Object to DBContext
//        /// </summary>
//        /// <param name="dbContext"></param>
//        public TechnicalSpecialistCurriculumVitaeRepository(EvolutionSqlDbContext dbContext, IMapper mapper) : base(dbContext)
//        {
//            this._dbContext = dbContext;
//            this._mapper = mapper;
//        }

//        public IList<DomainModel.TechnicalSpecialistCurriculumVitae> Search(DomainModel.TechnicalSpecialistCurriculumVitae searchModel)
//        {

//            var dbSearchModel = this._mapper.Map<DbModel.TechnicalSpecialistCurriculumVitae>(searchModel);
//            IQueryable<DbModel.TechnicalSpecialistCurriculumVitae> whereClause = null;

//            var CvDetails = _dbContext.TechnicalSpecialistCurriculumVitae;

//            //Wildcard Search for ModifiedBy 
//            if (searchModel.ModifiedBy.HasEvoWildCardChar())
//                whereClause = CvDetails.WhereLike(x => x.ModifiedBy, searchModel.ModifiedBy, '*');
//            else
//                whereClause = CvDetails.Where(x => string.IsNullOrEmpty(searchModel.ModifiedBy) || x.ModifiedBy == searchModel.ModifiedBy);

//            //Wildcard Search for Project Name
           
//            var expression = dbSearchModel.ToExpression();
//            if (expression == null)
//                return whereClause.ProjectTo<DomainModel.TechnicalSpecialistCurriculumVitae>().ToList();
//            else
//                return whereClause.Where(expression).ProjectTo<DomainModel.TechnicalSpecialistCurriculumVitae>().ToList();

//        }
//    }
//}
