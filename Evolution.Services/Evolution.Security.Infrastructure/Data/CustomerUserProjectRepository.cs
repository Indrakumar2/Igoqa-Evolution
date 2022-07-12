using Evolution.Common.Models.Responses;
using Evolution.GenericDbRepository.Services;
using Evolution.Security.Domain.Interfaces.Data;
using System;
using System.Collections.Generic;
using System.Text;
using DomainModel = Evolution.Security.Domain.Models.Security;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using AutoMapper;
using Evolution.DbRepository.Models.SqlDatabaseContext;

namespace Evolution.Security.Infrastructure.Data
{
    public class CustomerUserProjectRepository : GenericRepository<DbModel.CustomerUserProjectAccess>,ICustomerUserProjectRepository
    {
        private readonly DbModel.EvolutionSqlDbContext _dbContext = null;
        public readonly IMapper _mapper = null;

        public CustomerUserProjectRepository(IMapper mapper, EvolutionSqlDbContext dbContext) : base(dbContext)
        {
            this._dbContext = dbContext;
            this._mapper = mapper;
        }
    }
}