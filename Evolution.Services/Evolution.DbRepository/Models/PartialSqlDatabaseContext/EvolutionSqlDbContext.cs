using Evolution.DbRepository.Models.PartialSqlDatabaseContext;
using Microsoft.EntityFrameworkCore;

namespace Evolution.DbRepository.Models.SqlDatabaseContext
{
    public partial class EvolutionSqlDbContext : DbContext
    { 
        public virtual DbQuery<GetAccountItemDetailResult> GetAccountItemDetailResult { get; set; }
        public virtual DbQuery<EvoIDGeneration> EvoIDGeneration { get; set; } 

    }
}
