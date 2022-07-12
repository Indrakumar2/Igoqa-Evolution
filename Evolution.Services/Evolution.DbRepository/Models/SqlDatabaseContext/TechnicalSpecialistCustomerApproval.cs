using System;
using System.Collections.Generic;

namespace Evolution.DbRepository.Models.SqlDatabaseContext
{
    public partial class TechnicalSpecialistCustomerApproval
    {
        public int Id { get; set; }
        public int TechnicalSpecialistId { get; set; }
        public int? CustomerId { get; set; }
        public string CustomerSapId { get; set; }
        public int? CustomerCommodityCodesId { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public string Comments { get; set; }
        public int? DisplayOrder { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? LastModification { get; set; }
        public byte? UpdateCount { get; set; }

        public virtual TechnicalSpecialistCustomers Customer { get; set; }
        public virtual Data CustomerCommodityCodes { get; set; }
        public virtual TechnicalSpecialist TechnicalSpecialist { get; set; }
    }
}
