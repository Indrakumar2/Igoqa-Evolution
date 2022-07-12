using System;
using System.Collections.Generic;

namespace Evolution.DbRepository.Models.SqlDatabaseContext
{
    public partial class TechnicalSpecialistCustomers
    {
        public TechnicalSpecialistCustomers()
        {
            CustomerCommodity = new HashSet<CustomerCommodity>();
            TechnicalSpecialistCustomerApproval = new HashSet<TechnicalSpecialistCustomerApproval>();
        }

        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public DateTime? LastModification { get; set; }
        public bool? IsActive { get; set; }
        public byte? UpdateCount { get; set; }
        public string ModifiedBy { get; set; }

        public virtual ICollection<CustomerCommodity> CustomerCommodity { get; set; }
        public virtual ICollection<TechnicalSpecialistCustomerApproval> TechnicalSpecialistCustomerApproval { get; set; }
    }
}
