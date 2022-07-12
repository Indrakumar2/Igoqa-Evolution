using Evolution.Common.Enums;
using System;

namespace Evolution.Customer.Domain.Models.Customers
{
    public class BaseCustomer
    {
        /// <summary>
        /// Last update count
        /// </summary>
        public Byte? UpdateCount { get; set; }

        /// <summary>
        /// Last Modified By
        /// </summary>
        public string ModifiedBy { get; set; }

        /// <summary>
        /// Last Modified On
        /// </summary>
        public DateTime? LastModifiedOn { get; set; }

        /// <summary>
        /// Record Status which will indicate wether record is N : New , M : Modified , D : Deleted
        /// </summary>
        public string RecordStatus { get; set; }
    }
}
