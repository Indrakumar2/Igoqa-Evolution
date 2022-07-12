using Evolution.Common.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Evolution.Company.Domain.Models.Companies
{
    public class CompanyTax : BaseModel
    {
        [AuditNameAttribute("Company Tax Id")]
        public int? CompanyTaxId { get; set; }

        [AuditNameAttribute("Company Code")]
        public string CompanyCode { get; set; }

        [AuditNameAttribute("Tax Name")]
        public string TaxName { get; set; }

        [AuditNameAttribute("Tax Type")]
        public string TaxType { get; set; }

        [AuditNameAttribute("Tax Rate")]
        public decimal TaxRate { get; set; }

        public string TaxCode { get; set; }

        public bool? IsIcInv { get; set; }

        public bool? IsActive { get; set; }
    } 
}
