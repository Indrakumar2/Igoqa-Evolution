using System;
using System.Collections.Generic;

namespace Evolution.DbRepository.Models.SqlDatabaseContext
{
    public partial class TechnicalSpecialistTrainingAndCompetencyType
    {
        public int Id { get; set; }
        public int TechnicalSpecialistTrainingAndCompetencyId { get; set; }
        public int TrainingOrCompetencyDataId { get; set; }

        public virtual TechnicalSpecialistTrainingAndCompetency TechnicalSpecialistTrainingAndCompetency { get; set; }
        public virtual Data TrainingOrCompetencyData { get; set; }
    }
}
