using Evolution.Common.Extensions;
using Evolution.DbRepository.Models.SqlDatabaseContext;
using Evolution.GenericDbRepository.Services;
using Evolution.Master.Domain.Interfaces.Data;
using System.Collections.Generic;
using System.Linq;

namespace Evolution.Master.Infrastructure.Data
{
    public class TaxTypeRepository : GenericRepository<Tax>, ITaxTypeRepository
    {
        private readonly EvolutionSqlDbContext _dbContext = null;

        public TaxTypeRepository(EvolutionSqlDbContext dbContext) : base(dbContext)
        {
            this._dbContext = dbContext;
        }

        public IList<Tax> Search(Tax search)
        {
            return this.FindBy(search.ToExpression()).ToList();
        }
    }
}