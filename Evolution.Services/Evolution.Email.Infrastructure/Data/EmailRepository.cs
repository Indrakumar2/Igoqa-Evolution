using AutoMapper;
using Evolution.DbRepository.Models.SqlDatabaseContext;
using Evolution.Email.Domain.Interfaces.Data;
using Evolution.GenericDbRepository.Services;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext; 

namespace Evolution.Email.Infrastructure.Data
{
    public class EmailRepository : GenericRepository<DbModel.Email>, IEmailRepository
    {
        private readonly DbModel.EvolutionSqlDbContext _dbContext = null;
        private readonly IMapper _mapper = null;

        public EmailRepository(EvolutionSqlDbContext dbContext, IMapper mapper) : base(dbContext)
        {
            this._dbContext = dbContext;
            this._mapper = mapper;
        }
    }
}
