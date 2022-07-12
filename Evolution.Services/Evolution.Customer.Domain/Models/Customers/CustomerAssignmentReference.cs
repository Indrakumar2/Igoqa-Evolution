using System;
using System.Collections.Generic;
using System.Text;

namespace Evolution.Customer.Domain.Models.Customers
{
    /// <summary>
    /// This will contain information related to Assignment Reference Type.
    /// </summary>
    public class CustomerAssignmentReference : BaseCustomer
    {
        [AuditNameAttribute("Customer Assignment Reference Id ")]

        /// <summary>
        /// Name of Assignment Reference Type Id
        /// </summary>
        public int CustomerAssignmentReferenceId { get; set; }
        [AuditNameAttribute("Assignment Reference Types ")]
        /// <summary>
        /// Name of Assignment Reference Type
        /// </summary>
        public string AssignmentRefType { get; set; }
    }
}
