namespace Evolution.Visit.Domain.Models.Visits
{
    public class TechSpecIntertekWorkHistory
    {
        public int AssignmentId { get; set; }
        public int AssignmentNumber { get; set; }
        public int? ProjectNumber { get; set; }
        public string Client { get; set; }
        public string InspectedEquipment { get; set; }
        public string SupplierName { get; set; }
        public string SupplierCity { get; set; }
        public string SupplierCounty { get; set; }
        public string SupplierCountry { get; set; }
        public string SupplierPostalCode { get; set; }
        public string Category { get; set; }
        public string SubCategory { get; set; }
        public string Service { get; set; } 
    }
}
