using System;
using System.Collections.Generic;

namespace Evolution.DbRepository.Models.SqlDatabaseContext
{
    public partial class Announcement
    {
        public int Id { get; set; }
        public string Header { get; set; }
        public string Text { get; set; }
        public DateTime? DisplayTill { get; set; }
        public string TextColour { get; set; }
        public string BackgroundColour { get; set; }
        public bool? IsEvolutionLocked { get; set; }
        public string EvolutionLockMessage { get; set; }
        public DateTime? LastModification { get; set; }
        public DateTime? DisplayFrom { get; set; }
    }
}
