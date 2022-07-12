using System;
using Evolution.Common.Models.Documents;
using System.Collections.Generic;
using System.Text;
using Evolution.Common.Models.Base;
using Evolution.Document.Domain.Models.Document;

namespace Evolution.TechnicalSpecialist.Domain.Models.TechSpecialist
{
    public class TechnicalSpecialistCustomerApprovalInfo : BaseTechnicalSpecialistModel
    {
        [AuditNameAttribute("Technical Specialist Customer Approval Info Id")]
        public int Id { get; set; }
        [AuditNameAttribute("ePin")]
        public int Epin { get; set; }
        [AuditNameAttribute("Customer Code")]
        public string CustomerCode { get; set; }
        [AuditNameAttribute("Customer Name")]
        public string CustomerName { get; set; }
        [AuditNameAttribute("Customer SAP Id")]
        public string CustomerSap { get; set; }
        [AuditNameAttribute("Customer Codes")]
        public string CustCodes { get; set; }
        [AuditNameAttribute("From Date", "dd-MMM-yyyy", AuditNameformatDataType.DateTime)]
        public DateTime? FromDate { get; set; }
        [AuditNameAttribute("To Date", "dd-MMM-yyyy", AuditNameformatDataType.DateTime)]
        public DateTime? ToDate { get; set; }
        //[AuditNameAttribute("Display Order")]
        public int DispalyOrder { get; set; }
        [AuditNameAttribute("Comments")]
        public string Comments { get; set; }
        [AuditNameAttribute("Document Name")]
        public string DocumentName { get; set; }
        public IList<ModuleDocument> Documents { get; set; }

    }
}
