using Evolution.Web.Gateway.Models.Customers;
using System.Collections.Generic;

namespace Evolution.Web.Gateway.Models
{
    public class CustomerAggregatorResponse
    {
        public object Detail { get; set; }

        public IList<AddressDetail> Addresses { get; set; }

       // public IList<Contact> Contacts { get; set; }

        public object AccountReferences { get; set; }

        public object AssignmentReferences { get; set; }

        public object Documents { get; set; }

        public object Notes { get; set; }
    }
}
