using Evolution.Customer.Domain.Interfaces.Data;
using Evolution.GenericDbRepository.Services;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.Customer.Domain.Models.Customers;
using Evolution.Common.Extensions;
using System.Linq.Expressions;
using System;

namespace Evolution.Customer.Infrastructure.Data
{
    public class CustomerNoteRepository : GenericRepository<DbModel.CustomerNote>, ICustomerNoteRepository
    {
        private readonly DbModel.EvolutionSqlDbContext _dbContext = null;

        /// <summary>
        /// TODO :  Replace Object to DBContext
        /// </summary>
        /// <param name="dbContext"></param>
        public CustomerNoteRepository(DbModel.EvolutionSqlDbContext dbContext) : base(dbContext)
        {
            this._dbContext = dbContext;
        }
        public IList<DomainModel.CustomerNote> Get(DbModel.CustomerNote dbCustomerNote)
        {
            // Generate dynamic filter expression
            Expression<Func<DbModel.CustomerNote, bool>> searchExpression = dbCustomerNote.ToExpression();
            var lstCustomerNotes = _dbContext.CustomerNote.Where(searchExpression).ToList();

            return lstCustomerNotes.GroupJoin(_dbContext.User,
                                              Note => Note.CreatedBy,
                                              User => User.SamaccountName,
                                              (Note, User) => new DomainModel.CustomerNote()
                                              {
                                                  CustomerNoteId = Note.Id,
                                                  Note = Note.Note,
                                                  CreatedBy = Note.CreatedBy,
                                                  CreatedOn = Note.CreatedDate,
                                                  UpdateCount = Note.UpdateCount,
                                                  LastModifiedOn = Note.LastModification,
                                                  ModifiedBy = Note.ModifiedBy,
                                                  UserDisplayName = User != null && User.Count() > 0 ? User?.FirstOrDefault().Name : string.Empty,
                                              })?.ToList();

        }
    }
}
