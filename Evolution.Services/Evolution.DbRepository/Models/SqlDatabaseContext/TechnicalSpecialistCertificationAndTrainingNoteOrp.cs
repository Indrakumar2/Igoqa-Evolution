using System;
using System.Collections.Generic;

namespace Evolution.DbRepository.Models.SqlDatabaseContext
{
    public partial class TechnicalSpecialistCertificationAndTrainingNoteOrp
    {
        public int Id { get; set; }
        public int TechnicalSpecCertificationAndTrainingId { get; set; }
        public string Notes { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? LastModification { get; set; }
        public byte? UpdateCount { get; set; }
    }
}
