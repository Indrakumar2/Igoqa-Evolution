using System;
using System.Collections.Generic;

namespace Evolution.DbRepository.Models.SqlDatabaseContext
{
    public partial class DocumentLibrary
    {
        public int Id { get; set; }
        public long DocumentId { get; set; }
        public string ReviewStatus { get; set; }
        public DateTime? ReviewDate { get; set; }
        public DateTime? LastEmailSentOn { get; set; }
        public string ModifiedBy { get; set; }
        public byte? UpdateCount { get; set; }
        public DateTime? LastModification { get; set; }
        public string Owner { get; set; }

        public virtual Document Document { get; set; }
    }
}
