using System;
using System.Collections.Generic;

namespace Evolution.DbRepository.Models.SqlDatabaseContext
{
    public partial class CustomerCommodity
    {
        public int Id { get; set; }
        public int CommodityId { get; set; }
        public int CustomerId { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? LastModification { get; set; }
        public byte? UpdateCount { get; set; }

        public virtual Data Commodity { get; set; }
        public virtual TechnicalSpecialistCustomers Customer { get; set; }
    }
}
