using Evolution.Common.Models.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Evolution.TechnicalSpecialist.Domain.Models.TechSpecialist
{
    public class TechnicalSpecialistWorkHistoryInfo : BaseTechnicalSpecialistModel
    {
        [AuditNameAttribute("Technical Specialist Work History Info Id")]
        public int Id { get; set; }
        [AuditNameAttribute("ePin")]
        public int Epin { get; set; }
        [AuditNameAttribute("Client Name")]
        public string ClientName { get; set; }
        [AuditNameAttribute("Project Name")]
        public string ProjectName { get; set; }
        [AuditNameAttribute("Job Title")]
        public string JobTitle { get; set; }
        [AuditNameAttribute("Is Current Company")]
        public bool IsCurrentCompany { get; set; }
        [AuditNameAttribute("Responsibility")]
        public string Responsibility { get; set; }
        [AuditNameAttribute("Number of Years Experience")]
        public string NoofYearsExp { get; set; }
        [AuditNameAttribute("Description")]
        public string Description { get; set; }
        [AuditNameAttribute("From Date", "dd-MMM-yyyy", AuditNameformatDataType.DateTime)]
        public DateTime? FromDate { get; set; }
        [AuditNameAttribute("To Date ", "dd-MMM-yyyy", AuditNameformatDataType.DateTime)]
        public DateTime? ToDate { get; set; }
        [AuditNameAttribute("Display Order ")]
        public int? DisplayOrder { get; set; }
    }
}
