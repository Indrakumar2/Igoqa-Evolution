using System;
using System.Collections.Generic;
using System.Text;

namespace Evolution.Master.Domain.Models
{
    public class ProjectType:BaseMasterModel
    {
        public string Type { get; set; }

        public string InterCompanyType { get; set; }

        public string InvoiceType { get; set; }
        public bool? IsARS { get; set; }

    }
}
