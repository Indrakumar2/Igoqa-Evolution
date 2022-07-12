using AutoMapper;
using AutoMapper.QueryableExtensions;
using Evolution.Common.Extensions;
using Evolution.DbRepository.Models.SqlDatabaseContext;
using Evolution.GenericDbRepository.Services;
using Evolution.Supplier.Domain.Interfaces.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.Supplier.Domain.Models.Supplier;

namespace Evolution.Contract.Infrastructure.Data
{
    public class SupplierContactRepository : GenericRepository<SupplierContact>, ISupplierContactRepository
    {
        private readonly DbModel.EvolutionSqlDbContext _dbContext = null;
        private readonly IMapper _mapper = null;

        public SupplierContactRepository(EvolutionSqlDbContext dbContext, IMapper mapper) : base(dbContext)
        {
            this._dbContext = dbContext;
            this._mapper = mapper;
        }

        public IList<DbModel.SupplierContact> Search(DomainModel.SupplierContact model,
                                                         params Expression<Func<DbModel.SupplierContact, object>>[] includes)
        {
            var supplierAsQueryable = this.PopulateSupplierAsQuerable(model);
            return this.Get(supplierAsQueryable, null, includes);
        }

        public IList<DbModel.SupplierContact> Get(IList<int> ids, params Expression<Func<DbModel.SupplierContact, object>>[] includes)
        {
            return Get(null, ids, includes);
        }

        private IList<DbModel.SupplierContact> Get(IQueryable<DbModel.SupplierContact> suppAsQuerable = null,
                                                   IList<int> ids = null,
                                                   params Expression<Func<DbModel.SupplierContact, object>>[] includes)
        {
            if (suppAsQuerable == null)
                suppAsQuerable = _dbContext.SupplierContact;

            if (includes.Any())
                suppAsQuerable = includes.Aggregate(suppAsQuerable, (current, include) => current.Include(include));

            if (ids?.Count > 0)
                suppAsQuerable = suppAsQuerable.Where(x => ids.Contains(x.Id));

            return suppAsQuerable.ToList();
        }

        private IQueryable<SupplierContact> PopulateSupplierAsQuerable(DomainModel.SupplierContact model)
        {
            var dbSearchModel = this._mapper.Map<DbModel.SupplierContact>(model);
            IQueryable<DbModel.SupplierContact> supplierAsQueryable = _dbContext.SupplierContact;

            #region Wildcard Search for Supplier Name
            if (model.SupplierName.HasEvoWildCardChar())
                supplierAsQueryable = supplierAsQueryable.WhereLike(x => x.Supplier.SupplierName.ToString(), model.SupplierName, '*');
            else if (!string.IsNullOrEmpty(model.SupplierName))
                supplierAsQueryable = supplierAsQueryable.Where(x => x.Supplier.SupplierName.ToString() == model.SupplierName);
            #endregion

            var defWhereExpr = dbSearchModel.ToExpression();
            if (defWhereExpr != null)
                supplierAsQueryable = supplierAsQueryable.Where(defWhereExpr);

            return supplierAsQueryable;
        }
    }
}