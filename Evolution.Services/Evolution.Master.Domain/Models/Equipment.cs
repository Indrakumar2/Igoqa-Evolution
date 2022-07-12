using System;
using System.Collections.Generic;
using System.Text;

namespace Evolution.Master.Domain.Models
{
    public class Equipment : BaseMasterModel
    {
        public string Code { get; set; }

        public string Commodity { get; set; }
    }
}
