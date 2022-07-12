using System;
using System.Collections.Generic;

namespace Evolution.DbRepository.Models.SqlDatabaseContext
{
    public partial class TechnicalSpecialistComputerElectronicKnowledge
    {
        public int Id { get; set; }
        public int TechnicalSpecialistId { get; set; }
        public int ComputerKnowledgeId { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? LastModification { get; set; }
        public byte? UpdateCount { get; set; }

        public virtual Data ComputerKnowledge { get; set; }
        public virtual TechnicalSpecialist TechnicalSpecialist { get; set; }
    }
}
