namespace Evolution.Web.Gateway.Models
{
    public class VisitAggregatorResponse
    {
        public object VisitInfo { get; set; }
        public object VisitTechnicalSpecialists { get; set; }
        public object VisitTechnicalSpecialistTimes { get; set; }
        public object VisitTechnicalSpecialistExpenses { get; set; }
        public object VisitTechnicalSpecialistTravels { get; set; }
        public object VisitTechnicalSpecialistConsumables { get; set; }
        public object VisitReferences { get; set; }
        public object VisitSupplierPerformances { get; set; }
        public object VisitInterCompanyDiscounts { get; set; }
        public object VisitDocuments { get; set; }
        public object VisitNotes { get; set; }
    }
}
