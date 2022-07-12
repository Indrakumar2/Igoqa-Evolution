using System;
using System.Collections.Generic;
using System.Text;

namespace Evolution.TechnicalSpecialist.Domain.Models.TechSpecialist
{
   public class TechnicalSpecialistDocument:BaseTechnicalSpecialistInfo
    {
        /// <summary>
        ///  Name of the document
        /// </summary>

        public string Name { get; set; }
        /// <summary>
        /// Type of document
        /// </summary>

        public string DocumentType { get; set; }
        /// <summary>
        ///  Size of the document
        /// </summary>

        public int? DocumentSize { get; set; }
        /// <summary>
        /// who uploaded the document(name of user)
        /// </summary>

        public DateTime? UploadOn { get; set; }
        /// <summary>
        /// Is it visible to customer or not
        /// </summary>

        public string VisibleToCustomer { get; set; }
        /// <summary>
        /// Is it visible to technical specialist or not
        /// </summary>

        public string VisibleToTechSpecialist { get; set; }
       
    }
}
