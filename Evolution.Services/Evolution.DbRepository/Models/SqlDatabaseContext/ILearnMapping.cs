using System;
using System.Collections.Generic;

namespace Evolution.DbRepository.Models.SqlDatabaseContext
{
    public partial class ILearnMapping
    {
        public int Id { get; set; }
        public string TrainingType { get; set; }
        public string IlearnObjectId { get; set; }
        public string Title { get; set; }
        public string IlearnId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
