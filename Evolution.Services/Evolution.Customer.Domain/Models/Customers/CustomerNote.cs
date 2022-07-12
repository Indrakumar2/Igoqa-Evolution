using System;
using System.Collections.Generic;
using System.Text;

namespace Evolution.Customer.Domain.Models.Customers
{
    /// <summary>
    /// This will hold the information related to note.
    /// </summary>
    public class CustomerNote : BaseCustomer
    {
        [AuditNameAttribute("Note")]
        /// <summary>
        /// User note for a customer
        /// </summary>
        public string Note { get; set; }
        [AuditNameAttribute("Created By ")]
        /// <summary>
        /// Who created the Note.
        /// </summary>
        public string CreatedBy { get; set; }
        [AuditNameAttribute("Created On", "dd-MMM-yyyy", AuditNameformatDataType.DateTime)]
        /// <summary>
        /// Date of Note creation.
        /// </summary>
        public DateTime? CreatedOn { get; set; }
        [AuditNameAttribute("Customer Note Id ")]
        /// <summary>
        /// CustomerNote Id
        /// </summary>
        public int CustomerNoteId { get; set; }
        public string UserDisplayName { get; set; }
    }
}
