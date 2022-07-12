using AutoMapper;
using Evolution.Common.Extensions;
using Evolution.GenericDbRepository.Services;
using Evolution.TechnicalSpecialist.Domain.Interfaces.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.TechnicalSpecialist.Domain.Models.TechSpecialist;

namespace Evolution.TechnicalSpecialist.Infrastructure.Data
{
    public class TechnicalSpecialistTaxonomyHistoryRepository : GenericRepository<DbModel.TechnicalSpecialistTaxonomyHistory>,ITechnicalSpecialistTaxonomyHistoryRepository
    {
        private DbModel.EvolutionSqlDbContext _dbContext = null;

        public TechnicalSpecialistTaxonomyHistoryRepository(DbModel.EvolutionSqlDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
