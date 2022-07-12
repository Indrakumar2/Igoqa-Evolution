using System;
using System.Collections.Generic;

namespace Evolution.DbRepository.Models.SqlDatabaseContext
{
    public partial class HistoryItem
    {
        public int Id { get; set; }
        public string HistoryItem1 { get; set; }
        public byte? UpdateCount { get; set; }
        public byte[] LastModification { get; set; }
    }
}
