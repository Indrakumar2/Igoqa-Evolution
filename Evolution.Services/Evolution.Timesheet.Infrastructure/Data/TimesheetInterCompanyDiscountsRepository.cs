using AutoMapper;
using Evolution.GenericDbRepository.Services;
using Evolution.Logging.Interfaces;
using Evolution.Timesheet.Domain.Interfaces.Data;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;

namespace Evolution.Timesheet.Infrastructure.Data
{
    public class TimesheetInterCompanyDiscountsRepository : GenericRepository<DbModel.TimesheetInterCompanyDiscount>, ITimesheetInterCompanyDiscountsRepository
    {
        private readonly DbModel.EvolutionSqlDbContext _dbContext = null;
        private readonly IMapper _mapper = null;
        private readonly IAppLogger<TimesheetInterCompanyDiscountsRepository> _logger = null;

        public TimesheetInterCompanyDiscountsRepository(DbModel.EvolutionSqlDbContext dbContext, IMapper mapper, IAppLogger<TimesheetInterCompanyDiscountsRepository> logger) : base(dbContext)
        {
            this._dbContext = dbContext;
            this._mapper = mapper;
            this._logger = logger;
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }
    }
}
