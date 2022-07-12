using System;
using System.Collections.Generic;

namespace Evolution.DbRepository.Models.SqlDatabaseContext
{
    public partial class SystemSetting
    {
        public int Id { get; set; }
        public string KeyName { get; set; }
        public string KeyValue { get; set; }
        public string ModifiedBy { get; set; }
        public byte? UpdateCount { get; set; }
        public DateTime? LastModification { get; set; }
    }
}
