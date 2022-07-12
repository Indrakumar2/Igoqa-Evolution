using System;
using System.Collections.Generic;

namespace Evolution.DbRepository.Models.SqlDatabaseContext
{
    public partial class NumberSequence
    {
        public int Id { get; set; }
        public int? ModuleId { get; set; }
        public long ModuleData { get; set; }
        public int? ModuleRefId { get; set; }
        public int LastSequenceNumber { get; set; }

        public virtual Module Module { get; set; }
       // public virtual Module ModuleRef { get; set; }
    }
}
