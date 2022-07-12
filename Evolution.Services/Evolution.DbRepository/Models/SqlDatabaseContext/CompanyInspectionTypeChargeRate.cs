using System;
using System.Collections.Generic;

namespace Evolution.DbRepository.Models.SqlDatabaseContext
{
    public partial class CompanyInspectionTypeChargeRate
    {
        public CompanyInspectionTypeChargeRate()
        {
            ContractRate = new HashSet<ContractRate>();
        }

        public int Id { get; set; }
        public int CompanyChgSchInspGrpInspectionTypeId { get; set; }
        public string ItemDescription { get; set; }
        public decimal RateOffShoreOil { get; set; }
        public decimal RateOnShoreOil { get; set; }
        public decimal RateOnShoreNonOil { get; set; }
        public bool? IsRateOffShoreOil { get; set; }
        public bool? IsRateOnShoreOil { get; set; }
        public bool? IsRateOnShoreNonOil { get; set; }
        public int? ItemSizeId { get; set; }
        public int? ItemThicknessId { get; set; }
        public int? FilmSizeId { get; set; }
        public int? FilmTypeId { get; set; }
        public int ExpenseTypeId { get; set; }
        public DateTime? LastModification { get; set; }
        public byte? UpdateCount { get; set; }
        public string ModifiedBy { get; set; }

        public virtual CompanyChgSchInspGrpInspectionType CompanyChgSchInspGrpInspectionType { get; set; }
        public virtual Data ExpenseType { get; set; }
        public virtual Data FilmSize { get; set; }
        public virtual Data FilmType { get; set; }
        public virtual Data ItemSize { get; set; }
        public virtual Data ItemThickness { get; set; }
        public virtual ICollection<ContractRate> ContractRate { get; set; }
    }
}
