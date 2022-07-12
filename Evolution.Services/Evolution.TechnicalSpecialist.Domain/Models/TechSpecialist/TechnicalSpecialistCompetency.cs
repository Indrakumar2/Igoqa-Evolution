using System;
using System.Collections.Generic;

namespace Evolution.TechnicalSpecialist.Domain.Models.TechSpecialist
{
    public class TechnicalSpecialistCompetency : TechnicalSpecialistInternalTrainingAndCompetency
    {
        [AuditNameAttribute("Duration ")]
        public string Duration { get; set; }
        [AuditNameAttribute("Effective Date", "dd-MMM-yyyy", AuditNameformatDataType.DateTime)]
        public DateTime? EffectiveDate { get; set; }
        [AuditNameAttribute("Titile ")]
        public string Titile { get; set; } // ILearn
        
        public int ILearnID { get; set; } // ILearn
        public IList<TechnicalSpecialistInternalTrainingAndCompetencyType> TechnicalSpecialistCompetencyTypes { get; set; }
    }
}
