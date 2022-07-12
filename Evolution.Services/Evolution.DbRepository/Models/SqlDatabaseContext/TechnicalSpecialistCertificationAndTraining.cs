using System;
using System.Collections.Generic;

namespace Evolution.DbRepository.Models.SqlDatabaseContext
{
    public partial class TechnicalSpecialistCertificationAndTraining
    {
        public int Id { get; set; }
        public int TechnicalSpecialistId { get; set; }
        public string Duration { get; set; }
        public DateTime? EffeciveDate { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public string VerificationStatus { get; set; }
        public DateTime? VerificationDate { get; set; }
        public int? VerifiedById { get; set; }
        public string VerificationType { get; set; }
        public string RecordType { get; set; }
        public int DispalyOrder { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? LastModification { get; set; }
        public byte? UpdateCount { get; set; }
        public int CertificationAndTrainingId { get; set; }
        public bool? IsExternal { get; set; }
        public string CertificationAndTrainingRefId { get; set; }
        public string Description { get; set; }
        public bool? IsIlearn { get; set; }

        public virtual Data CertificationAndTraining { get; set; }
        public virtual TechnicalSpecialist TechnicalSpecialist { get; set; }
        public virtual User VerifiedBy { get; set; }
    }
}
