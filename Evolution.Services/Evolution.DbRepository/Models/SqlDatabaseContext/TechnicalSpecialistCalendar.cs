using System;
using System.Collections.Generic;

namespace Evolution.DbRepository.Models.SqlDatabaseContext
{
    public partial class TechnicalSpecialistCalendar
    {
        public long Id { get; set; }
        public int TechnicalSpecialistId { get; set; }
        public int? CompanyId { get; set; }
        public string CalendarType { get; set; }
        public long? CalendarRefCode { get; set; }
        public string CalendarStatus { get; set; }
        public DateTime? StartDateTime { get; set; }
        public DateTime? EndDateTime { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? LastModification { get; set; }
        public byte UpdateCount { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Description { get; set; }

        public virtual TechnicalSpecialist TechnicalSpecialist { get; set; }
    }
}
