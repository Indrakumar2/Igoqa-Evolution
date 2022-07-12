using System;
using System.Collections.Generic;
using System.Text;

namespace Evolution.Master.Domain.Models
{
    public class CustomerCommodity : BaseMasterModel
    {
        public string CustomerName { get; set; }
        public string CustomerCode { get; set; }
        public string CommodityName { set; get; }


    }
}
