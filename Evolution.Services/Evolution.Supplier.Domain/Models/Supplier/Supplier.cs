using Evolution.Common.Models.Base;
using System.Collections.Generic;

namespace Evolution.Supplier.Domain.Models.Supplier
{
    public class Supplier :BaseModel
    {
        [AuditNameAttribute("Supplier Id ")]
        public int? SupplierId { get; set; }
        [AuditNameAttribute("Supplier Name")]
        public string SupplierName { get; set; }
         [AuditNameAttribute("Supplier Address")]
        public string SupplierAddress { get; set; }
        [AuditNameAttribute("Country")]
        public string Country { get; set; }

        public int? CountryId { get; set; } //Added for D-1076
        [AuditNameAttribute("State")]
        public string State { get; set; }

        public int? StateId { get; set; }   //Added for D-1076
        [AuditNameAttribute("City")]
        public string City{ get; set; }

        public int? CityId { get; set; }   //Added for D-1076
       [AuditNameAttribute("Postal Code")]
        public string PostalCode { get; set; }
         [AuditNameAttribute("Is Active")]
        public bool? IsActive { get; set; }

      
    }

    public class SupplierSearch : Supplier
    {
        public IList<string> SupplierIds { get; set; }

        public string SearchDocumentType { get; set; }

        public string DocumentSearchText { get; set; }
    }
}
