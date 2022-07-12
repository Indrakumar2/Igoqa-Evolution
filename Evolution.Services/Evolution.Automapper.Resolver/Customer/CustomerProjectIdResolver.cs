using AutoMapper;
using Evolution.DbRepository.Models.SqlDatabaseContext;
using Evolution.Security.Domain.Models.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;

namespace Evolution.Automapper.Resolver.Customer
{
    public class CustomerProjectIdResolver : IMemberValueResolver<object, object, IList<CustomerUserProject>, ICollection<DbModel.CustomerUserProjectAccess>>
    {
        private readonly EvolutionSqlDbContext _dbContext = null;

        public CustomerProjectIdResolver(EvolutionSqlDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public ICollection<DbModel.CustomerUserProjectAccess> Resolve(object source, object destination, IList<CustomerUserProject> sourceMember, ICollection<DbModel.CustomerUserProjectAccess> destMember, ResolutionContext context)
        {
            IList<DbModel.Project> dbCustomerProjects = null;
            if (context.Options.Items.ContainsKey("dbCustomerProject"))
                dbCustomerProjects = ((List<DbModel.Project>)context.Options.Items["dbCustomerProject"]);

            if (sourceMember!=null && sourceMember.Any())
            { 
                if (dbCustomerProjects == null)
                {
                    return _dbContext.Project.Where(x => sourceMember.Select(x1 => x1.ProjectNumber).Contains(x.ProjectNumber.Value))
                       .Select(x => new DbModel.CustomerUserProjectAccess
                       {
                           ProjectId = x.Id,
                           LastModification = DateTime.UtcNow,
                       }).ToList();
                }
                else
                {
                    return dbCustomerProjects.Where(x => sourceMember.Select(x1 => x1.ProjectNumber).Contains(x.ProjectNumber.Value)).Select(x => new DbModel.CustomerUserProjectAccess
                    {
                        ProjectId = x.Id,
                        LastModification=DateTime.UtcNow,
                    }).ToList();
                }
            }
            return null;
        }
    }
}
