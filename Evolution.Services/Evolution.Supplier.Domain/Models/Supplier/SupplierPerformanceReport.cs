using Evolution.TechnicalSpecialist.Domain.Models.TechSpecialist;
using Evolution.Visit.Domain.Models.Visits;
using System;
using System.Collections.Generic;
using System.Text;

namespace Evolution.Supplier.Domain.Models.Supplier
{
    public class SupplierPerformanceReport: SupplierPerformanceReportsearch
    {
        public string Country { get; set; }

        public string State { get; set; }

        public string City { get; set; }

        public string SupplierPerformanceType { get; set; }

        public DateTime VisitDate { get; set; }

        public string NCRReference { get; set; }
        
        public string TechnicalSpecialistsName { get; set; }

        public int VisitNumber { get; set; }

        public string VisitReportNumber { get; set; }

        public DateTime? NcrClosureOutDate { get; set; }

    }
}
