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
    public class UserRoleRepository : GenericRepository<DbModel.UserRole>, IUserRoleRepository
    {
        private readonly EvolutionSqlDbContext _dbContext = null;
        private readonly IMapper _mapper = null;

        public UserRoleRepository(DbModel.EvolutionSqlDbContext dbContext, IMapper mapper) : base(dbContext)
        {
            this._dbContext = dbContext;
            this._mapper = mapper;
        }

        public IList<DbModel.UserRole> Get(DomainModel.UserRoleInfo searchModel, params string[] includes)
        {
            IQueryable<DbModel.UserRole> whereClause = _dbContext.UserRole;

            if (!string.IsNullOrEmpty(searchModel.ApplicationName))
                whereClause = whereClause.Where(x => x.Application.Name == searchModel.ApplicationName);

            if (!string.IsNullOrEmpty(searchModel.UserLogonName))
                whereClause = whereClause.Where(x => x.User.SamaccountName == searchModel.UserLogonName);

            if (!string.IsNullOrEmpty(searchModel.RoleName))
                whereClause = whereClause.Where(x => x.Role.Name == searchModel.RoleName);

            if (!string.IsNullOrEmpty(searchModel.CompanyCode))//def 957: select only active company permissions
                whereClause = whereClause
                    .Where(x => x.User.UserType.Any(x1=>x1.Company.Code== searchModel.CompanyCode && x1.IsActive==true) && x.Company.Code == searchModel.CompanyCode); 

            if (includes?.Length > 0)
                whereClause = includes.Aggregate(whereClause, (current, include) => current.Include(include));

            return whereClause.ToList();

        }

    }
}
