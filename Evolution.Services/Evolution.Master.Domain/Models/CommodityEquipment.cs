using System;
using System.Collections.Generic;
using System.Text;

namespace Evolution.Master.Domain.Models
{
    public class CommodityEquipment : BaseMasterModel
    {
        public string CommodityCode { get; set; }
        public string Commodity { get; set; }
        public string Equipment { get; set; }
        public string EquipmentCode { get; set; }
    }
}
