using System;
using Evolution.Common.Models.Base;

namespace Evolution.Timesheet.Domain.Models.Timesheets
{
    /// <summary>
    /// Contains the information of timesheet Documents
    /// </summary>
    public class TimesheetDocument : BaseModel
    {
        public int TimesheetDocumentId { get; set; }
        /// <summary>
        /// Document Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Format of teh Document
        /// </summary>
        public string DocumentType { get; set; }

        /// <summary>
        /// Shows whether the document is visible to Tech Spec
        /// </summary>
        public string VisibleToTS { get; set; }

        /// <summary>
        /// Shows whether the document is visible to customer 
        /// </summary>
        public string VisibleToCustomer { get; set; }

        /// <summary>
        /// Size of Document in KB
        /// </summary>
        public int DocumentSize { get; set; }
        
        /// <summary>
        /// Uploaded Date of Document
        /// </summary>
        public DateTime UploadedOn { get; set; }
    }
}
