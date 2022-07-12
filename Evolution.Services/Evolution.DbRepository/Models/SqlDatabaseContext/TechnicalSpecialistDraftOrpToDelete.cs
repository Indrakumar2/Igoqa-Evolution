using System;
using System.Collections.Generic;

namespace Evolution.DbRepository.Models.SqlDatabaseContext
{
    public partial class TechnicalSpecialistDraftOrpToDelete
    {
        public int Id { get; set; }
        public string Moduletype { get; set; }
        public string Type { get; set; }
        public string SerilizableObject { get; set; }
        public string SerilizationType { get; set; }
        public string DraftId { get; set; }
    }
}
