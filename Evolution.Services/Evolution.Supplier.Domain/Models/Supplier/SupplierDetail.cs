using Evolution.Document.Domain.Models.Document;
using System;
using System.Collections.Generic;
using System.Text;
using DBModel = Evolution.DbRepository.Models.SqlDatabaseContext;

namespace Evolution.Supplier.Domain.Models.Supplier
{
    public class SupplierDetail
    {
        public Supplier SupplierInfo { get; set; }

        public IList<SupplierContact> SupplierContacts { get; set; }
          
        public IList<ModuleDocument> SupplierDocuments { get; set; }

        public IList<SupplierNote> SupplierNotes { get; set; }

        public IList<DBModel.SqlauditModule> dbModule = null;
    }
}
