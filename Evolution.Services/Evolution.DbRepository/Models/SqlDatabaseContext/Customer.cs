using System;
using System.Collections.Generic;

namespace Evolution.DbRepository.Models.SqlDatabaseContext
{
    public partial class Customer
    {
        public Customer()
        {
            Contract = new HashSet<Contract>();
            CustomerAddress = new HashSet<CustomerAddress>();
            CustomerAssignmentReferenceType = new HashSet<CustomerAssignmentReferenceType>();
            CustomerCompanyAccountReference = new HashSet<CustomerCompanyAccountReference>();
            CustomerNote = new HashSet<CustomerNote>();
            ResourceSearch = new HashSet<ResourceSearch>();
        }

        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string ParentName { get; set; }
        public int Miiwaid { get; set; }
        public int? MiiwaparentId { get; set; }
        public DateTime? LastModification { get; set; }
        public bool IsActive { get; set; }
        public byte? UpdateCount { get; set; }
        public string ModifiedBy { get; set; }

        public virtual ICollection<Contract> Contract { get; set; }
        public virtual ICollection<CustomerAddress> CustomerAddress { get; set; }
        public virtual ICollection<CustomerAssignmentReferenceType> CustomerAssignmentReferenceType { get; set; }
        public virtual ICollection<CustomerCompanyAccountReference> CustomerCompanyAccountReference { get; set; }
        public virtual ICollection<CustomerNote> CustomerNote { get; set; }
        public virtual ICollection<ResourceSearch> ResourceSearch { get; set; }
    }
}
