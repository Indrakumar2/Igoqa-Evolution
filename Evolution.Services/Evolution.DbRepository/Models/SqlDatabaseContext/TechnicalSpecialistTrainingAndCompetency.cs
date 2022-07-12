using System;
using System.Collections.Generic;

namespace Evolution.DbRepository.Models.SqlDatabaseContext
{
    public partial class TechnicalSpecialistTrainingAndCompetency
    {
        public TechnicalSpecialistTrainingAndCompetency()
        {
            TechnicalSpecialistTrainingAndCompetencyType = new HashSet<TechnicalSpecialistTrainingAndCompetencyType>();
        }

        public int Id { get; set; }
        public int TechnicalSpecialistId { get; set; }
        public DateTime? TrainingDate { get; set; }
        public DateTime? Expiry { get; set; }
        public string Score { get; set; }
        public string Competency { get; set; }
        public string RecordType { get; set; }
        public string Duration { get; set; }
        public int? DisplayOrder { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? LastModification { get; set; }
        public byte? UpdateCount { get; set; }
        public bool? IsIlearn { get; set; }

        public virtual TechnicalSpecialist TechnicalSpecialist { get; set; }
        public virtual ICollection<TechnicalSpecialistTrainingAndCompetencyType> TechnicalSpecialistTrainingAndCompetencyType { get; set; }
    }
}
