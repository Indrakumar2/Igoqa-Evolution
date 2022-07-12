using Evolution.Common.Models.Base;
using System;
using System.Collections.Generic;

namespace Evolution.Timesheet.Domain.Models.Timesheets
{
    public class TimesheetSearch : BaseModel
    {
        private string _timesheetJobReferenceNumber { get; set; }
        [AuditNameAttribute("Timesheet Id")]
        public long? TimesheetId { get; set; }

        public IList<Int64> TimesheetIds { get; set; }

        [AuditNameAttribute("Timesheet Number")]
        public int? TimesheetNumber { get; set; }
        [AuditNameAttribute("Assignment Id")]
        public int? TimesheetAssignmentId { get; set; }
        [AuditNameAttribute("Customer Code")]
        public string TimesheetCustomerCode { get; set; }
        [AuditNameAttribute("Customer Name")]
        public string TimesheetCustomerName { get; set; }
        [AuditNameAttribute("EndDate", "dd-MMM-yyyy", AuditNameformatDataType.DateTime)]
        public DateTime? TimesheetEndDate { get; set; }
        [AuditNameAttribute("StartDate", "dd-MMM-yyyy", AuditNameformatDataType.DateTime)]
        public DateTime? TimesheetStartDate { get; set; }
        [AuditNameAttribute("Timesheet Description")]
        public string TimesheetDescription { get; set; }
        [AuditNameAttribute("Timesheet Status")]

        public string TimesheetStatus { get; set; }

        /// <summary>
        /// Used for serching TS based on the searched input
        /// </summary>
        [AuditNameAttribute("TechnicalSpecialist Name")]
        /// 
        public string TechnicalSpecialistName { get; set; }

        /// <summary>
        /// To send fetched TS as list
        /// </summary>
        public IList<TechnicalSpecialist> TechSpecialists { get; set; }
        [AuditNameAttribute("Contract Number")]

        public string TimesheetContractNumber { get; set; }
        [AuditNameAttribute("Project Number")]

        public int? TimesheetProjectNumber { get; set; }
        [AuditNameAttribute("Assignment Number")]

        public int? TimesheetAssignmentNumber { get; set; }
        [AuditNameAttribute("Contract Holder Company Code")]

        public string TimesheetContractHolderCompanyCode { get; set; }
        [AuditNameAttribute("Contract Holder Company")]

        public string TimesheetContractHolderCompany { get; set; }
        [AuditNameAttribute("Contract Holder Coordinator")]

        public string TimesheetContractHolderCoordinator { get; set; }
        [AuditNameAttribute("Contract Holder Coordinator Code")]

        public string TimesheetContractHolderCoordinatorCode { get; set; }
        [AuditNameAttribute("Operating Company Code")]

        public string TimesheetOperatingCompanyCode { get; set; }
        [AuditNameAttribute("Operating Company")]

        public string TimesheetOperatingCompany { get; set; }
        [AuditNameAttribute("Operating Company Coordinator")]

        public string TimesheetOperatingCompanyCoordinator { get; set; }
        [AuditNameAttribute("Operating Company Coordinator Code")]

        public string TimesheetOperatingCompanyCoordinatorCode { get; set; }
        [AuditNameAttribute("Customer Project Name")]

        public string CustomerProjectName { get; set; }
        [AuditNameAttribute("Invoice Instruction Notes")]

        public string ProjectInvoiceInstructionNotes { get; set; }

        public string SearchDocumentType { get; set; }

        public string DocumentSearchText { get; set; }

        [AuditNameAttribute("Job Reference")]
        public string TimesheetJobReference
        {
            get
            {
                return _timesheetJobReferenceNumber = TimesheetProjectNumber + "-" + TimesheetAssignmentNumber + "-" + TimesheetNumber;
            }
        }

        public bool IsOnlyViewTimesheet { get; set; }

        public string LoggedInCompanyCode { get; set; }

        public int LoggedInCompanyId { get; set; }
        public int CustomerId { get; set; }
        public int ContractCompanyId { get; set; }
        public int OperatingCompanyId { get; set; }
        public int ContractCoordinatorId { get; set; }
        public int OperatingCoordinatorId { get; set; }
        public int CategoryId { get; set; }
        public int SubCategoryId { get; set; }
        public int ServiceId { get; set; }
    }

    public class HeaderList
    {
        public string Label { get; set; }
        public string Key { get; set; }
    }

    public class Result
    {
        public IList<HeaderList> Header { get; set; }
        public IList<TimesheetSearch> TimesheetSearch { get; set; }
    }
}
