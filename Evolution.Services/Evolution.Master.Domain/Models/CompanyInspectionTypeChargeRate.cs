using System;
using System.Collections.Generic;
using System.Text;

namespace Evolution.Master.Domain.Models
{
    public class CompanyInspectionTypeChargeRate : BaseMasterModel
    {
        public string CompanyCode { get; set; }

        public string CompanyChgSchInspGrpInspectionTypeName { get; set; }
        public string CompanyChgSchInspGroupName { get; set; }
        public string CompanyChargeScheduleName { get; set; }

        public string RateOffShoreOil { get; set; }

        public string RateOnShoreOil { get; set; }

        public string RateOnShoreNonOil { get; set; }

        public string IsRateOffShoreOil { get; set; }

        public string IsRateOnShoreOil { get; set; }

        public string IsRateOnShoreNonOil { get; set; }

        public string ItemSize { get; set; }

        public string ItemThickness { get; set; }

        public string FilmSize { get; set; }

        public string FilmType { get; set; }

        public string ExpenseType { get; set; }

    }
}
