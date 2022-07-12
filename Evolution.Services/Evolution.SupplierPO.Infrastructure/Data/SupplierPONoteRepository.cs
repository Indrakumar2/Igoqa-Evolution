using AutoMapper;
using AutoMapper.QueryableExtensions;
using Evolution.Common.Extensions;
using Evolution.GenericDbRepository.Services;
using Evolution.SupplierPO.Domain.Interfaces.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.SupplierPO.Domain.Models.SupplierPO;

namespace Evolution.SupplierPO.Infrastructure.Data
{
    public class SupplierPONoteRepository : GenericRepository<DbModel.SupplierPurchaseOrderNote>, ISupplierPONoteRepository
    {
        private readonly DbModel.EvolutionSqlDbContext _dbContext = null;
        private readonly IMapper _mapper = null;

        public SupplierPONoteRepository(DbModel.EvolutionSqlDbContext dbContext, IMapper mapper) : base(dbContext)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public IList<DomainModel.SupplierPONote> Search(DomainModel.SupplierPONote searchModel)
        {
            var dbSearchModel = this._mapper.Map<DbModel.SupplierPurchaseOrderNote>(searchModel);
            IQueryable<DbModel.SupplierPurchaseOrderNote> whereClause = null;

            if (searchModel.SupplierPONumber.HasEvoWildCardChar())
                whereClause = _dbContext.SupplierPurchaseOrderNote.WhereLike(x => x.SupplierPurchaseOrder.SupplierPonumber, searchModel.SupplierPONumber, '*');
            else
                whereClause = _dbContext.SupplierPurchaseOrderNote.Where(x => string.IsNullOrEmpty(searchModel.SupplierPONumber) || x.SupplierPurchaseOrder.SupplierPonumber == searchModel.SupplierPONumber);

            var expression = dbSearchModel.ToExpression();
            if (expression != null)
                whereClause = whereClause.Where(expression);

            return whereClause.GroupJoin(_dbContext.User,
            Note => Note.CreatedBy,
            User => User.SamaccountName,
            (Note, User) => new DomainModel.SupplierPONote()
            {
                Notes = Note.Note,
                CreatedBy = Note.CreatedBy,
                CreatedOn = Note.CreatedDate,
                SupplierPOId = Note.SupplierPurchaseOrderId,
                SupplierPONoteId = Note.Id,
                UserDisplayName = User != null && User.Count() > 0 ? User.FirstOrDefault().Name : string.Empty,
                UpdateCount = Note.UpdateCount,
                LastModification = Note.LastModification,
                ModifiedBy = Note.ModifiedBy,
                SupplierPONumber=Note.SupplierPurchaseOrder.SupplierPonumber
            })?.ToList();
        }
    }
}
