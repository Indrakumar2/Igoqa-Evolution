using System;
using System.Collections.Generic;

namespace Evolution.DbRepository.Models.SqlDatabaseContext
{
    public partial class IlearnLogData
    {
        public int Id { get; set; }
        public string TrainingType { get; set; }
        public string IlearnObjectId { get; set; }
        public string Title { get; set; }
        public string IlearnId { get; set; }
        public DateTime? CompletedDate { get; set; }
        public string ErrorMessage { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
