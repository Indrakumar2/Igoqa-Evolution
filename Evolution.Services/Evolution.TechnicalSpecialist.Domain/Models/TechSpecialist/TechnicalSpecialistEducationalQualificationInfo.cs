using Evolution.Common.Models.Base;
using Evolution.Document.Domain.Models.Document;
using System;
using System.Collections.Generic;

namespace Evolution.TechnicalSpecialist.Domain.Models.TechSpecialist
{
    public class TechnicalSpecialistEducationalQualificationInfo : BaseTechnicalSpecialistModel
    {
        [AuditNameAttribute("Technical Specialist Educational Qualification Info Id")]
        public int Id { get; set; }
        [AuditNameAttribute("ePin")]
        public int Epin { get; set; }
        [AuditNameAttribute("Qualification")]
        public string Qualification { get; set; }
        [AuditNameAttribute("Institution")]
        public string Institution { get; set; }
        [AuditNameAttribute("Address")]
        public string Address { get; set; }
        [AuditNameAttribute("Country")]
        public string CountryName { get; set; }
        [AuditNameAttribute("County")]
        public string CountyName { get; set; }
        [AuditNameAttribute("City")]
        public string CityName { get; set; }
        [AuditNameAttribute("From Date", "dd-MMM-yyyy", AuditNameformatDataType.DateTime)]
        public DateTime? FromDate { get; set; }
        [AuditNameAttribute("To Date", "dd-MMM-yyyy", AuditNameformatDataType.DateTime)]
        public DateTime? ToDate { get; set; }
       // [AuditNameAttribute("Percentage")]
        public decimal Percentage { get; set; }
        [AuditNameAttribute("Place")]
        public string Place { get; set; }
        //[AuditNameAttribute("Display Order ")]
        public int? DisplayOrder { get; set; }

        public IList<ModuleDocument> Documents { get; set; }
    }
}
