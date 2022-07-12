using AutoMapper.QueryableExtensions;
using Evolution.Common.Extensions;
using Evolution.TechnicalSpecialist.Domain.Interfaces.Data;
using Evolution.DbRepository.Models.SqlDatabaseContext;
using Evolution.GenericDbRepository.Services;
using System.Collections.Generic;
using System.Linq;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.TechnicalSpecialist.Domain.Models.TechSpecialist;
using AutoMapper;
using Evolution.Admin.Domain.Models.Admins;

namespace Evolution.TechnicalSpecialist.Infrastructure.Data
{
    public class TechnicalSpecialistUserRepository : GenericRepository<DbModel.User>, ITechnicalSpecialistUsersRepository
    {

        private readonly DbModel.EvolutionSqlDbContext _dbContext = null;
        private readonly IMapper _mapper = null;
        /// <summary>
        /// TODO :  Replace Object to DBContext
        /// </summary>
        /// <param name="dbContext"></param>
        public TechnicalSpecialistUserRepository(EvolutionSqlDbContext dbContext, IMapper mapper) : base(dbContext)
        {
            this._dbContext = dbContext;
            this._mapper = mapper;
        }

        public IList<Admin.Domain.Models.Admins.User> Search(Admin.Domain.Models.Admins.User searchModel)
        {
            var dbSearchModel = this._mapper.Map<DbModel.User>(searchModel);
            IQueryable<DbModel.User> whereClause = null;
            var payScheduleDetails = _dbContext.User;

            //Wildcard Search for customer Approval
            if (searchModel.Username.HasEvoWildCardChar())
                whereClause = payScheduleDetails.WhereLike(x => x.Name, searchModel.Username, '*');
            else
                whereClause = payScheduleDetails.Where(x => string.IsNullOrEmpty(searchModel.Username) || x.Name == searchModel.Username);

            var expression = dbSearchModel.ToExpression();
            if (expression == null)
                return whereClause.ProjectTo<Admin.Domain.Models.Admins.User>().ToList();
            else
                return whereClause.Where(expression).ProjectTo<Admin.Domain.Models.Admins.User>().ToList();

        }
    }
}


