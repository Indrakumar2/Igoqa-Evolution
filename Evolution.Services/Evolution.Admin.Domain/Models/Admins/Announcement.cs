using Evolution.Common.Models.Base;
using System;

namespace Evolution.Admin.Domain.Models.Admins
{
    public class Announcement : BaseModel
    {
        //public int? Id { get; set; }
        public string Header { get; set; }
        public string Text { get; set; }
        public DateTime? DisplayTill { get; set; }
        public string TextColour { get; set; }
        public string BackgroundColour { get; set; }
        public bool? IsEvolutionLocked { get; set; }
        public string EvolutionLockMessage { get; set; }
    }
}
