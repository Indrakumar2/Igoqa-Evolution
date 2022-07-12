using System;
using System.Collections.Generic;
using System.Text;

namespace Evolution.Master.Domain.Models
{
    public class Country : BaseMasterModel
    {
        public string EUVatPrefix { get; set; }

        public string Code { get; set; }

        public string Region { get; set; }

        public bool? IsEuMember { get; set; }

        public bool? IsGCCMember { get; set; }

        
    }
}
