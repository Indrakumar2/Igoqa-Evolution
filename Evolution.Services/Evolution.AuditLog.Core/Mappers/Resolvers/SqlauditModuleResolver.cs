using AutoMapper;
using Evolution.AuditLog.Domain.Interfaces.Data;
using Evolution.DbRepository.Models.SqlDatabaseContext;
using System.Collections.Generic;
using System.Linq;

namespace Evolution.AuditLog.Core.Mappers.Resolvers
{
    //public class SqlauditModuleResolver : IValueResolver<object, object, int>
    //{
    //    private readonly ISqlAuditModuleRepository _repository = null;

    //    public SqlauditModuleResolver(ISqlAuditModuleRepository repository)
    //    {
    //        this._repository = repository;
    //    }

    //    public int Resolve(object source, object destination, int destinationMember, ResolutionContext context)
    //    {
    //        IList<SqlauditModule> dbModules = null;

    //        if (context.Options.Items.ContainsKey("dbModules"))
    //            dbModules = ((List<SqlauditModule>)context.Options.Items["dbModules"]);
    //        else
    //            dbModules = _repository.GetAll().ToList();

    //        var moduleName = (string)source.GetPropertyValue("ApplicationName");
    //        if (!string.IsNullOrEmpty(moduleName))
    //        {
    //            var dbModule = dbModules.Where(x => x.ModuleName == moduleName).FirstOrDefault();
    //            if (dbModule != null)
    //                return dbModule.Id;
    //        }
    //        return 0;
    //    }
    //}

    public class SqlauditModuleResolver : IMemberValueResolver<object, object, string, int>
    {
        private readonly ISqlAuditModuleRepository _repository = null;

        public SqlauditModuleResolver(ISqlAuditModuleRepository repository)
        {
            this._repository = repository;
        }

        public int Resolve(object source, object destination, string sourceMember, int destMember, ResolutionContext context)
        {
            IList<SqlauditModule> dbModules = null;

            if (context.Options.Items.ContainsKey("dbModules"))
                dbModules = ((List<SqlauditModule>)context.Options.Items["dbModules"]);

            if (!string.IsNullOrEmpty(sourceMember))
            {
                if (dbModules == null)
                    dbModules = _repository.FindBy(x => x.ModuleName == sourceMember).ToList();

                var dbModule = dbModules.Where(x => x.ModuleName == sourceMember).FirstOrDefault();
                if (dbModule != null)
                    return dbModule.Id;
            }
            return 0;
        }
    }
}
