using Evolution.Common.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Evolution.Common.Models.Notes;

namespace Evolution.Company.Domain.Models.Companies
{
    public class CompanyNote : Note
    {
        [AuditNameAttribute("Company Note Id")]
        public int CompanyNoteId { get; set; }

        [AuditNameAttribute("Company Code")]
        public string CompanyCode { get; set; }
        
    }
}
