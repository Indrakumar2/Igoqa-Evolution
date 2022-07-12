using System;
using System.Collections.Generic;
using System.Text;
using Evolution.Common.Enums;

namespace Evolution.Master.Domain.Models
{
    public class MasterData :BaseMasterModel
    {
        public int? MasterDataTypeId { get; set; }

        public string MasterType {get; set;}

        public string Code { get; set; }
        
        public string DisplayName { get; set; }

        public string Description { get; set; }

        public bool? IsActive { get; set; } = true;

        public bool? IsAlchiddenForNewFacility { get; set; }

        public bool? IsEmployed { get; set; }

        public string Type { get; set; }

        public string InterCompanyType { get; set; }

        public string ChargeReference { get; set; }

        public string PayReference { get; set; }

        public string InvoiceType { get; set; }

        public int? Precedence { get; set; }

        public string PayrollExportPrefix { get; set; }

        public int? Hour { get; set; }
                
        public int? Evolution1Id { get; set; }

        public bool? IsARS { get; set; }
    }
}
