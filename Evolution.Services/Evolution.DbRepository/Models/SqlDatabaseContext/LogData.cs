using System;
using System.Collections.Generic;

namespace Evolution.DbRepository.Models.SqlDatabaseContext
{
    public partial class LogData
    {
        public int Id { get; set; }
        public string Module { get; set; }
        public string LogReason { get; set; }
        public string ObjectType { get; set; }
        public string Object { get; set; }
        public DateTime? LoggedOn { get; set; }
        public byte? RetryCount { get; set; }
    }
}
