using Evolution.Common.Models.Documents;
using Evolution.Document.Domain.Models.Document;
using System;
using System.Collections.Generic;
using System.Text;
using DBModel = Evolution.DbRepository.Models.SqlDatabaseContext;

namespace Evolution.Customer.Domain.Models.Customers
{
   public class CustomerDetail
    {
        public Customer Detail { get; set; }
        public IList<CustomerAddress> Addresses { get; set; }
        public IList<ModuleDocument> Documents { get; set; }
        public IList<CustomerNote> Notes { get; set; }
        public IList<CustomerAssignmentReference> AssignmentReferences { get; set; }
        public IList<CustomerCompanyAccountReference> AccountReferences { get; set; }
        public IList<Contact> Contact { get; set; }

        public long? EventId { get; set; }

        public string ActionByUser { get; set; }

        public IList<DBModel.SqlauditModule> dbModule = null;
    }
}
