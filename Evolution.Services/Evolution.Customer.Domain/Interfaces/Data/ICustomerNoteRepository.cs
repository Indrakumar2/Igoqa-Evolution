using Evolution.GenericDbRepository.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.Customer.Domain.Models.Customers;

namespace Evolution.Customer.Domain.Interfaces.Data
{ 
    public interface ICustomerNoteRepository : IGenericRepository<DbModel.CustomerNote>
    {
        ///TODO : Defined those function which is not covered in Generic Repository
        IList<DomainModel.CustomerNote> Get(DbModel.CustomerNote customerNotes);
    }
}
