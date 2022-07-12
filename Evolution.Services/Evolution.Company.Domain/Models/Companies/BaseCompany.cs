using System;
using System.Collections.Generic;
using System.Text;

namespace Evolution.Company.Domain.Models.Companies
{
    public class BaseCompany
    {
        public int? UpdateCount { get; set; }
        public string RecordStatus { get; set; }
        public DateTime? LastModification { get; set; }
        public string ModifiedBy { get; set; }
        
       
    }
}
