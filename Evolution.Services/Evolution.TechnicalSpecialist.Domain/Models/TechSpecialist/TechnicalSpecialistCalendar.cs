using Evolution.Common.Models.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Evolution.TechnicalSpecialist.Domain.Models.TechSpecialist
{
    public class TechnicalSpecialistCalendar : BaseModel
    {
        public long? Id { get; set; }
        public int TechnicalSpecialistId { get; set; }
        public string TechnicalSpecialistName { get; set; }
        public int? CompanyId { get; set; }
        public string CompanyCode { get; set; }
        public string CalendarType { get; set; }
        public long CalendarRefCode { get; set; }
        public string CalendarStatus { get; set; }
        public DateTime? StartDateTime { get; set; }
        public DateTime? EndDateTime { get; set; }
        public string CreatedBy { get; set; }
        public bool IsActive { get; set; }
        public string Description { get; set; }
        public string ResourceName { get; set; }
        public string CustomerName { get; set; }
        public string SupplierName { get; set; }
        public string SupplierLocation { get; set; }
        public string JobReferenceNumber { get; set; }

        public IList<int> TechnicalSpecialistIds { get; set; }
    }

    public class TechnicalSpecialistCalendarView
    {
        public IList<TechnicalSpecialistCalendarResourceView> Resources { get; set; }
        public IList<TechnicalSpecialistCalendarEventView> Events { get; set; }
    }

    public class TechnicalSpecialistCalendarEventView
    {
        public long? Id { get; set; }
        public DateTime? Start { get; set; }
        public DateTime? End { get; set; }
        public int ResourceId { get; set; }
        public string Title { get; set; }
        public string BgColor { get; set; }
        public bool Resizable { get; set; }
        public long CalendarRefCode { get; set; }
        public string CalendarType { get; set; }
        public int? CompanyId { get; set; }
        public string CompanyCode { get; set; }
        public string CalendarStatus { get; set; }
        public string CreatedBy { get; set; }
        public string LogInName { get; set; }
        public bool IsActive { get; set; }
        public Byte? UpdateCount { get; set; }
        public string RecordStatus { get; set; }
        public string Description { get; set; }
    }

    public class TechnicalSpecialistCalendarResourceView
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
