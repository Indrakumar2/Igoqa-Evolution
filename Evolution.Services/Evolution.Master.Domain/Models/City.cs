using System;
using System.Collections.Generic;
using System.Text;

namespace Evolution.Master.Domain.Models
{
    public class City : BaseMasterModel
    {
        public string Country { set; get; }

        public string State { set; get; }

        public int? StateId { get; set; }  //Added for D-1076
    }
}
