using Evolution.Customer.Domain.Interfaces.Data;
using Evolution.DbRepository.Models.SqlDatabaseContext;
using Evolution.GenericDbRepository.Services;
using System;
using System.Collections.Generic;
using System.Text;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
namespace Evolution.TechnicalSpecialist.Infrastructure.Data
{
   public class PayRateRepository: GenericRepository<DbModel.TechnicalSpecialistPayRate>, IPayRateRepository
    {
        /// <summary>
        /// TODO :  Replace Object to DBContext
        /// </summary>
        /// <param name="dbContext"></param>
        public PayRateRepository(EvolutionSqlDbContext dbContext) : base(dbContext)
        {

        }
    }
}
