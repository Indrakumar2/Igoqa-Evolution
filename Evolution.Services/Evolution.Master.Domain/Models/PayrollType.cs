using Evolution.Common.Models.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Evolution.Master.Domain.Models
{
    public class PayrollType :BaseModel
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public bool? IsActive { get; set; }

    }
}
