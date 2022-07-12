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
    public class SupplierNoteRepository : GenericRepository<DbModel.SupplierNote>, ISupplierNoteRepository
    {
        private readonly DbModel.EvolutionSqlDbContext _dbContext = null;
        private readonly IMapper _mapper = null;

        public SupplierNoteRepository(EvolutionSqlDbContext dbContext, IMapper mapper) : base(dbContext)
        {
            this._dbContext = dbContext;
            this._mapper = mapper;
        }
        public IList<DomainModel.SupplierNote> Search(DomainModel.SupplierNote model,
                                                  params Expression<Func<DbModel.SupplierNote, object>>[] includes)
        {
            var tsAsQueryable = this.PopulateTsAsQuerable(model);
            return this.Get(tsAsQueryable, null, null, includes);
        }

        private IList<DomainModel.SupplierNote> Get(IQueryable<DbModel.SupplierNote> suppAsQuerable = null,
                                       IList<int> ids = null,
                                       IList<string> supplierNames = null,
                                       params Expression<Func<DbModel.SupplierNote, object>>[] includes)
        {
            if (suppAsQuerable == null)
                suppAsQuerable = _dbContext.SupplierNote;

            if (includes.Any())
                suppAsQuerable = includes.Aggregate(suppAsQuerable, (current, include) => current.Include(include));

            if (ids?.Count > 0)
                suppAsQuerable = suppAsQuerable.Where(x => ids.Contains(x.Id));

            if (supplierNames?.Count > 0)
                suppAsQuerable = suppAsQuerable.Where(x => supplierNames.Contains(x.Supplier.SupplierName.ToString()));

            return suppAsQuerable.GroupJoin(_dbContext.User,
            Note => Note.CreatedBy,
            User => User.SamaccountName,
            (Note, User) => new DomainModel.SupplierNote()
            {
                SupplierNoteId = Note.Id,
                Notes = Note.Note,
                CreatedBy = Note.CreatedBy,
                CreatedOn = Note.CreatedDate,
                UpdateCount = Note.UpdateCount,
                LastModification = Note.LastModification,
                ModifiedBy = Note.ModifiedBy,
                UserDisplayName = User != null && User.Count() > 0 ? User.FirstOrDefault().Name : string.Empty,
            })?.ToList();
        }

        //public IList<DomainModel.SupplierNote> Search(DomainModel.SupplierNote searchModel)
        //{
        //    var dbSearchModel = this._mapper.Map<DbModel.SupplierNote>(searchModel);
        //    IQueryable<DbModel.SupplierNote> whereClause = null;

        //    //Wildcard Search for Supplier Name
        //    if (searchModel.SupplierName.HasEvoWildCardChar())
        //        whereClause = _dbContext.SupplierNote.WhereLike(x => x.Supplier.SupplierName, searchModel.SupplierName, '*');
        //    else
        //        whereClause = _dbContext.SupplierNote.Where(x => string.IsNullOrEmpty(searchModel.SupplierName) || x.Supplier.SupplierName == searchModel.SupplierName);

        //    var expression = dbSearchModel.ToExpression();
        //    if (expression == null)
        //        return whereClause.ProjectTo<DomainModel.SupplierNote>().ToList();
        //    else
        //        return whereClause.Where(expression).ProjectTo<DomainModel.SupplierNote>().ToList();
        //}

        private IQueryable<DbModel.SupplierNote> PopulateTsAsQuerable(DomainModel.SupplierNote model)
        {
            var dbSearchModel = this._mapper.Map<DbModel.SupplierNote>(model);
            IQueryable<DbModel.SupplierNote> tsAsQueryable = _dbContext.SupplierNote;

            #region Wildcard Search for SupplierName
            if (model.SupplierName.HasEvoWildCardChar())
                tsAsQueryable = tsAsQueryable.WhereLike(x => x.Supplier.SupplierName.ToString(), model.SupplierName, '*');
            else if (!string.IsNullOrEmpty(model.SupplierName))
                tsAsQueryable = tsAsQueryable.Where(x => x.Supplier.SupplierName.ToString() == model.SupplierName);
            #endregion

            var defWhereExpr = dbSearchModel.ToExpression();
            if (defWhereExpr != null)
                tsAsQueryable = tsAsQueryable.Where(defWhereExpr);

            return tsAsQueryable;
        }
    }
}
