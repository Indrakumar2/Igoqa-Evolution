using System;
using System.Collections.Generic;

namespace Evolution.DbRepository.Models.SqlDatabaseContext
{
    public partial class IlearnData
    {
        public int Id { get; set; }
        public string TrainingObjectId { get; set; }
        public string TrainingTitle { get; set; }
        public DateTime? CompletedDate { get; set; }
        public int? TechnicalSpecialistId { get; set; }
        public decimal? Score { get; set; }
        public decimal? TrainingHours { get; set; }
    }
}
