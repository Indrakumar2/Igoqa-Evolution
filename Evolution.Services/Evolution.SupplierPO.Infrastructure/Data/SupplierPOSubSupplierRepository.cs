using AutoMapper;
using AutoMapper.QueryableExtensions;
using Evolution.Common.Extensions;
using Evolution.GenericDbRepository.Services;
using Evolution.SupplierPO.Domain.Interfaces.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using DbModels = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModels = Evolution.SupplierPO.Domain.Models.SupplierPO;

namespace Evolution.SupplierPO.Infrastructure.Data
{
    public class SupplierPOSubSupplierRepository : GenericRepository<DbModels.SupplierPurchaseOrderSubSupplier>, ISupplierPOSubSupplierRepository
    {
        private readonly DbModels.EvolutionSqlDbContext _dbContext = null;
        private readonly IMapper _mapper = null;

        public SupplierPOSubSupplierRepository(DbModels.EvolutionSqlDbContext dbContext, IMapper mapper) : base(dbContext)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public IList<DbModels.SupplierPurchaseOrderSubSupplier> Search(DomainModels.SupplierPOSubSupplier model,
                                                       params Expression<Func<DbModels.SupplierPurchaseOrderSubSupplier, object>>[] includes)
        {
            var tsAsQueryable = this.PopulateTsAsQuerable(model);
            return this.Get(tsAsQueryable, null, null, includes);
        }

        private IList<DbModels.SupplierPurchaseOrderSubSupplier> Get(IQueryable<DbModels.SupplierPurchaseOrderSubSupplier> suppAsQuerable = null,
                                         IList<int> ids = null,
                                         IList<string> supplierNames = null,
                                         params Expression<Func<DbModels.SupplierPurchaseOrderSubSupplier, object>>[] includes)
        {
            if (suppAsQuerable == null)
                suppAsQuerable = _dbContext.SupplierPurchaseOrderSubSupplier;

            if (includes.Any())
                suppAsQuerable = includes.Aggregate(suppAsQuerable, (current, include) => current.Include(include));

            if (ids?.Count > 0)
                suppAsQuerable = suppAsQuerable.Where(x => ids.Contains(x.Id));
            if (supplierNames?.Count > 0)
                suppAsQuerable = suppAsQuerable.Where(x => supplierNames.Contains(x.Supplier.SupplierName.ToString()));

            return suppAsQuerable.ToList();
        }

        private IQueryable<DbModels.SupplierPurchaseOrderSubSupplier> PopulateTsAsQuerable(DomainModels.SupplierPOSubSupplier model)
        {
            var dbSearchModel = this._mapper.Map<DbModels.SupplierPurchaseOrderSubSupplier>(model);
            IQueryable<DbModels.SupplierPurchaseOrderSubSupplier> tsAsQueryable = _dbContext.SupplierPurchaseOrderSubSupplier;

            #region Wildcard Search for SupplierPONumber
            if (model.SupplierPONumber.HasEvoWildCardChar())
                tsAsQueryable = tsAsQueryable.WhereLike(x => x.SupplierPurchaseOrder.SupplierPonumber.ToString(), model.SubSupplierName, '*');
            else if (!string.IsNullOrEmpty(model.SupplierPONumber))
                tsAsQueryable = tsAsQueryable.Where(x => x.SupplierPurchaseOrder.SupplierPonumber.ToString() == model.SupplierPONumber);
            #endregion

            #region Wildcard Search for SupplierPONumber
            if (model.SupplierPONumber.HasEvoWildCardChar())
                tsAsQueryable = tsAsQueryable.WhereLike(x => x.Supplier.SupplierName.ToString(), model.SubSupplierName, '*');
            else if (!string.IsNullOrEmpty(model.SubSupplierName))
                tsAsQueryable = tsAsQueryable.Where(x => string.IsNullOrEmpty(model.SubSupplierName) || x.Supplier.SupplierName == model.SubSupplierName);
            #endregion

            var defWhereExpr = dbSearchModel.ToExpression();
            if (defWhereExpr != null)
                tsAsQueryable = tsAsQueryable.Where(defWhereExpr);

            return tsAsQueryable;
        }

        public bool IsValidSupplierContact(int supplierPoId, List<int?> supplierIds, List<int?> supplierContactIds)
        {
           var supplierId= _dbContext.SupplierPurchaseOrder.Where(x => x.Id == supplierPoId)?
                                                                       // && supplierIds.Contains(x.SupplierId))?
                                                                        .Select(x=>x.SupplierId)?.ToList();

            var supplierContactId = _dbContext.SupplierContact.Where(x => supplierId.Contains(x.SupplierId) 
                                                                    && supplierContactIds.Contains(x.Id))?.Select(x => x.Id)?.ToList();

            if (supplierId.Any() && supplierContactId.Any())
                return true;
            else
                return false;
        }
    }
}
