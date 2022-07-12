using System;
using System.Collections.Generic;

namespace Evolution.DbRepository.Models.SqlDatabaseContext
{
    public partial class TechnicalSpecialistLanguageCapability
    {
        public int Id { get; set; }
        public int TechnicalSpecialistId { get; set; }
        public int LanguageId { get; set; }
        public string SpeakingCapabilityLevel { get; set; }
        public string WritingCapabilityLevel { get; set; }
        public string ComprehensionCapabilityLevel { get; set; }
        public int? DispalyOrder { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? LastModification { get; set; }
        public byte? UpdateCount { get; set; }

        public virtual Data Language { get; set; }
        public virtual TechnicalSpecialist TechnicalSpecialist { get; set; }
    }
}
