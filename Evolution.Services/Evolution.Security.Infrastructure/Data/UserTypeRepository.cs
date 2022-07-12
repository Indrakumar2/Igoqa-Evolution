using AutoMapper;
using Evolution.DbRepository.Models.SqlDatabaseContext;
using Evolution.GenericDbRepository.Services;
using Evolution.Security.Domain.Interfaces.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.Security.Domain.Models.Security;

namespace Evolution.Security.Infrastructure.Data
{
    public class UserTypeRepository : GenericRepository<UserType>, IUserTypeRepository
    {
        private readonly EvolutionSqlDbContext _dbContext = null;
        private readonly IMapper _mapper = null;

        public UserTypeRepository(EvolutionSqlDbContext dbContext, IMapper mapper) : base(dbContext)
        {
            this._dbContext = dbContext;
            this._mapper = mapper;
        }

        public IList<DbModel.UserType> Search(DomainModel.UserTypeInfo searchModel,params string[] includes)
        {  
            IQueryable<DbModel.UserType> whereClause = _dbContext.UserType;

            if (!string.IsNullOrEmpty(searchModel.CompanyCode))
                whereClause = whereClause.Where(x => x.Company.Code == searchModel.CompanyCode);

            if (!string.IsNullOrEmpty(searchModel.UserLogonName))
                whereClause = whereClause.Where(x => x.User.SamaccountName == searchModel.UserLogonName);

            if (!string.IsNullOrEmpty(searchModel.UserType))
                whereClause = whereClause.Where(x => x.UserTypeName == searchModel.UserType);

            if (includes?.Length > 0)
                whereClause = includes.Aggregate(whereClause, (current, include) => current.Include(include));

            return  whereClause.ToList(); 

        }

        public IList<DbModel.UserType> Get(string companyCode, IList<string> userTypes, params string[] includes)
        { 
            IQueryable<DbModel.UserType> whereClause = _dbContext.UserType;

            if (!string.IsNullOrEmpty(companyCode))
                whereClause = whereClause.Where(x => x.Company.Code == companyCode);

            if (userTypes?.Count > 0)
                whereClause = whereClause.Where(x => userTypes.Contains(x.UserTypeName));
              
            if (includes?.Length > 0)
                whereClause = includes.Aggregate(whereClause, (current, include) => current.Include(include));

            return whereClause.ToList();

        }

        public IList<DbModel.UserType> Get(int companyId, IList<string> userTypes, params string[] includes)
        {
            IQueryable<DbModel.UserType> whereClause = _dbContext.UserType;

            if (companyId>0)
                whereClause = whereClause.Where(x => x.CompanyId == companyId);

            if (userTypes?.Count > 0)
                whereClause = whereClause.Where(x => userTypes.Contains(x.UserTypeName));

            if (includes?.Length > 0)
                whereClause = includes.Aggregate(whereClause, (current, include) => current.Include(include));

            return whereClause.ToList();

        }
    }
}
