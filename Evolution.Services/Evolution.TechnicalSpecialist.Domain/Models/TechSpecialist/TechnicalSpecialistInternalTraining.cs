using System;
using System.Collections.Generic;

namespace Evolution.TechnicalSpecialist.Domain.Models.TechSpecialist
{
    public class TechnicalSpecialistInternalTraining : TechnicalSpecialistInternalTrainingAndCompetency
    {
        [AuditNameAttribute("Training Date","DD-MMM-YYYY")]
        public DateTime? TrainingDate { get; set; }
       
        public IList<TechnicalSpecialistInternalTrainingAndCompetencyType> TechnicalSpecialistInternalTrainingTypes { get; set; }
        [AuditNameAttribute("Title")]
        public string Title { get; set; } // I-Learn

        public int ILearnID { get; set; } // ILearn
    }
}
