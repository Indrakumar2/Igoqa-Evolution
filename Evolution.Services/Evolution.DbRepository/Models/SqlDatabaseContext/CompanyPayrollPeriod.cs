using System;
using System.Collections.Generic;

namespace Evolution.DbRepository.Models.SqlDatabaseContext
{
    public partial class CompanyPayrollPeriod
    {
        public CompanyPayrollPeriod()
        {
            TimesheetTechnicalSpecialistAccountItemConsumable = new HashSet<TimesheetTechnicalSpecialistAccountItemConsumable>();
            TimesheetTechnicalSpecialistAccountItemExpense = new HashSet<TimesheetTechnicalSpecialistAccountItemExpense>();
            TimesheetTechnicalSpecialistAccountItemTime = new HashSet<TimesheetTechnicalSpecialistAccountItemTime>();
            TimesheetTechnicalSpecialistAccountItemTravel = new HashSet<TimesheetTechnicalSpecialistAccountItemTravel>();
            VisitTechnicalSpecialistAccountItemConsumable = new HashSet<VisitTechnicalSpecialistAccountItemConsumable>();
            VisitTechnicalSpecialistAccountItemExpense = new HashSet<VisitTechnicalSpecialistAccountItemExpense>();
            VisitTechnicalSpecialistAccountItemTime = new HashSet<VisitTechnicalSpecialistAccountItemTime>();
            VisitTechnicalSpecialistAccountItemTravel = new HashSet<VisitTechnicalSpecialistAccountItemTravel>();
        }

        public int Id { get; set; }
        public int CompanyPayrollId { get; set; }
        public string PeriodName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string PeriodStatus { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? LastModification { get; set; }
        public byte? UpdateCount { get; set; }
        public string ModifiedBy { get; set; }

        public virtual CompanyPayroll CompanyPayroll { get; set; }
        public virtual ICollection<TimesheetTechnicalSpecialistAccountItemConsumable> TimesheetTechnicalSpecialistAccountItemConsumable { get; set; }
        public virtual ICollection<TimesheetTechnicalSpecialistAccountItemExpense> TimesheetTechnicalSpecialistAccountItemExpense { get; set; }
        public virtual ICollection<TimesheetTechnicalSpecialistAccountItemTime> TimesheetTechnicalSpecialistAccountItemTime { get; set; }
        public virtual ICollection<TimesheetTechnicalSpecialistAccountItemTravel> TimesheetTechnicalSpecialistAccountItemTravel { get; set; }
        public virtual ICollection<VisitTechnicalSpecialistAccountItemConsumable> VisitTechnicalSpecialistAccountItemConsumable { get; set; }
        public virtual ICollection<VisitTechnicalSpecialistAccountItemExpense> VisitTechnicalSpecialistAccountItemExpense { get; set; }
        public virtual ICollection<VisitTechnicalSpecialistAccountItemTime> VisitTechnicalSpecialistAccountItemTime { get; set; }
        public virtual ICollection<VisitTechnicalSpecialistAccountItemTravel> VisitTechnicalSpecialistAccountItemTravel { get; set; }
    }
}
