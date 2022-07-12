using System;
using System.Collections.Generic;

namespace Evolution.DbRepository.Models.SqlDatabaseContext
{
    public partial class CommodityEquipment
    {
        public int Id { get; set; }
        public int CommodityId { get; set; }
        public int EquipmentId { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? LastModification { get; set; }
        public byte? UpdateCount { get; set; }

        public virtual Data Commodity { get; set; }
        public virtual Data Equipment { get; set; }
    }
}
