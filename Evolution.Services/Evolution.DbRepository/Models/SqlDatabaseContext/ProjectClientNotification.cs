using System;
using System.Collections.Generic;

namespace Evolution.DbRepository.Models.SqlDatabaseContext
{
    public partial class ProjectClientNotification
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public int CustomerContactId { get; set; }
        public bool SendInspectionReleaseNotesNotification { get; set; }
        public bool SendFlashReportingNotification { get; set; }
        public bool SendNcrreportingNotification { get; set; }
        public bool SendCustomerReportingNotification { get; set; }
        public bool SendCustomerDirectReportingNotification { get; set; }
        public byte? UpdateCount { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? LastModification { get; set; }

        public virtual CustomerContact CustomerContact { get; set; }
        public virtual Project Project { get; set; }
    }
}
