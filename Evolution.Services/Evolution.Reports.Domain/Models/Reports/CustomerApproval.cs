using System;
using System.Collections.Generic;
using System.Text;

namespace Evolution.Reports.Domain.Models.Reports
{
    public class CustomerApproval: CustomerApprovalSearch
    {
        public string CustomerApprovalName { get; set; }

        public string LastName { get; set; }

        public string CustomerSapID { get; set; }

        public int? EpinId { get; set; }

        public DateTime? EffectiveFrom { get; set; }

        public DateTime? EffectiveTo { get; set; }

        public string ProfileStatus { get; set; }

        public string EmploymentType { get; set; }

        public string Company { get; set; }

        public string Country { get; set; }

        public string County { get; set; }

        public string City { get; set; }

        public string ZipCode { get; set; }

        public string Comments { get; set; }

        public string SubDivision { get; set; }
    }

    public class CustomerApprovalSearch 
    {

        public string FirstName { get; set; }

    }
}
