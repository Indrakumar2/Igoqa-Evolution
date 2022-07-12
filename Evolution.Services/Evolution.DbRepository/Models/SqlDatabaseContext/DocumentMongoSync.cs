using System;
using System.Collections.Generic;

namespace Evolution.DbRepository.Models.SqlDatabaseContext
{
    public partial class DocumentMongoSync
    {
        public long Id { get; set; }
        public string DocumentUniqueName { get; set; }
        public bool IsCompleted { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string Reason { get; set; }
        public byte? FailCount { get; set; }
        public string Status { get; set; }
    }
}
