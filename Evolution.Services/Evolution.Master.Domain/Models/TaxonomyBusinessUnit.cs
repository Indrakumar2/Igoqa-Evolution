using Evolution.Common.Models.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Evolution.Master.Domain.Models
{
   public class TaxonomyBusinessUnit  
   {
        public int Id { get; set; }

        public string ProjectType { get; set; }

        public int? ProjectTypeId { get; set; }
        
        public int? CategoryId { get; set; }

        public string Category { get; set; }

        public string LastModification { get; set; }

        public string Type { get; set; }

        public string InterCompanyType { get; set; }

        public string InvoiceType { get; set; }

        public string ModifiedBy { get; set; }

        public string UpdateCount { get; set; }
   }
}
