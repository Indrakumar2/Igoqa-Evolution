namespace Evolution.Visit.Domain.Models.Visits
{
    public class VisitDocuments : BaseVisit
    {
        /// <summary>
        /// 
        /// </summary>
        public int VisitDocumentId { get; set; }

        /// <summary>
        ///Defines the Pay TotalUnit
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///Defines the Type of Document
        /// </summary>
        public string DocumentType { get; set; }

        /// <summary>
        ///Defines whether document visible to Technical Specialist
        /// </summary>
        public string VisibleToTS { get; set; }

        /// <summary>
        ///Defines whether the document will be visible to Customer
        /// </summary>
        public string VisibleToCustomer { get; set; }

        /// <summary>
        ///Defines the Size of Document
        /// </summary>
        public int DocumentSize { get; set; }

        /// <summary>
        ///Defines the Date of document Upload
        /// </summary>
        public string UploadedOn { get; set; }

        public string ModuleRefCode { get; set; }
    }
}
