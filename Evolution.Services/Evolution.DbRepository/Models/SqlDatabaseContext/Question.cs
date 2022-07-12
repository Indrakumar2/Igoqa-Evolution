using System;
using System.Collections.Generic;

namespace Evolution.DbRepository.Models.SqlDatabaseContext
{
    public partial class Question
    {
        public int Id { get; set; }
        public int ApplicationId { get; set; }
        public string Question1 { get; set; }
        public bool IsActive { get; set; }
        public DateTime? LastModification { get; set; }
        public byte? UpdateCount { get; set; }
        public string ModifiedBy { get; set; }

        public virtual Application Application { get; set; }
    }
}
