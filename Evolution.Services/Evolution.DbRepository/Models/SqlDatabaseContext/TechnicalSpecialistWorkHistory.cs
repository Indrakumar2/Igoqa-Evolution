using System;
using System.Collections.Generic;

namespace Evolution.DbRepository.Models.SqlDatabaseContext
{
    public partial class TechnicalSpecialistWorkHistory
    {
        public int Id { get; set; }
        public int TechnicalSpecialistId { get; set; }
        public string ClientName { get; set; }
        public string ProjectName { get; set; }
        public string JobTitle { get; set; }
        public bool IsCurrentCompany { get; set; }
        public string JobResponsibility { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public string JobDescription { get; set; }
        public int? DisplayOrder { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? LastModification { get; set; }
        public byte? UpdateCount { get; set; }

        public virtual TechnicalSpecialist TechnicalSpecialist { get; set; }
    }
}
