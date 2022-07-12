using Evolution.Customer.Domain.Interfaces.Data;
using Evolution.Customer.Domain.Models.Customers;
using Evolution.GenericDbRepository.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;

namespace Evolution.Customer.Infrastructure.Data
{
    public class CustomerAssignmentReferenceRepository : GenericRepository<DbModel.CustomerAssignmentReferenceType>, ICustomerAssignmentReferenceRepository
    {
        private readonly DbModel.EvolutionSqlDbContext _dbContext = null;

        public CustomerAssignmentReferenceRepository(DbModel.EvolutionSqlDbContext dbContext) : base(dbContext)
        {
            this._dbContext = dbContext;
        }

        public IList<CustomerAssignmentReference> Search(string customerCode, CustomerAssignmentReference searchModel)
        {
            if (this._dbContext == null)
                throw new System.InvalidOperationException("Datacontext is not intitialized.");

            return _dbContext.CustomerAssignmentReferenceType
                        .Where(x => (string.IsNullOrEmpty(searchModel.AssignmentRefType) || x.AssignmentReference.Name == searchModel.AssignmentRefType)
                        && (string.IsNullOrEmpty(customerCode) || x.Customer.Code == customerCode))
                        .Select(x => new CustomerAssignmentReference()
                        {
                            CustomerAssignmentReferenceId = x.Id,
                            AssignmentRefType = x.AssignmentReference.Name,
                            ModifiedBy = x.ModifiedBy,
                            UpdateCount = x.UpdateCount,
                            LastModifiedOn = (DateTime)x.LastModification,
                        }).ToList();
        }
    }
}
