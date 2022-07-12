using Evolution.DbRepository.Models.SqlDatabaseContext;
using Evolution.GenericDbRepository.Services;
using Evolution.TechnicalSpecialist.Domain.Interfaces.Data;
using System;
using System.Collections.Generic;
using System.Text;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
namespace Evolution.TechnicalSpecialist.Infrastructure.Data
{
  public  class EmploymentRepository: GenericRepository<DbModel.TechnicalSpecialist>, IEmploymentRepository
    {
        /// <summary>
        /// TODO :  Replace Object to DBContext
        /// </summary>
        /// <param name="dbContext"></param>
        public EmploymentRepository(EvolutionSqlDbContext dbContext) : base(dbContext)
        {

        }
    }
}
