namespace Evolution.Web.Gateway.Models
{
    public class TimesheetAggregatorResponse
    {
        public object TimesheetInfo { get; set; }

        public object TimesheetTechnicalSpecialists { get; set; }

        public object TimesheetTechnicalSpecialistConsumables { get; set; }

        public object TimesheetTechnicalSpecialistExpenses { get; set; }

        public object TimesheetTechnicalSpecialistTimes { get; set; }

        public object TimesheetTechnicalSpecialistTravels { get; set; }

        public object TimesheetReferences { get; set; }

        public object TimesheetDocuments { get; set; }

        public object TimesheetNotes { get; set; }

       // public object TimesheetInvoices { get; set; }
    }
}
