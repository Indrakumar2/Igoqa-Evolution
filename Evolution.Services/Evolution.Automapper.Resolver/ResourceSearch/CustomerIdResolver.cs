using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;

namespace Evolution.Automapper.Resolver.ResourceSearch
{
    public class CustomerIdResolver : IMemberValueResolver<object, object, string, int?>
    {
        private readonly DbModel.EvolutionSqlDbContext _dbContext = null;

        public CustomerIdResolver(DbModel.EvolutionSqlDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public int? Resolve(object source, object destination, string sourceMember, int? destMember, ResolutionContext context)
        {
            IList<DbModel.Customer> dbCustomers = null;

            if (context.Options.Items.ContainsKey("dbCustomer"))
                dbCustomers = ((List<DbModel.Customer>)context.Options.Items["dbCustomer"]);

            if (!string.IsNullOrEmpty(sourceMember))
            {
                if (dbCustomers == null)
                    dbCustomers = this._dbContext.Customer.Where(x => x.Name == sourceMember || x.Code == sourceMember).ToList();

                var dbModule = dbCustomers.FirstOrDefault(x => x.Name == sourceMember || x.Code == sourceMember);
                if (dbModule != null)
                    return dbModule.Id;
            }
            return default(int?);
        }
    }
}

