using System;

namespace Evolution.Timesheet.Domain
{

    /// <summary>
    /// Contains the information of Base Timesheet Model    
    /// </summary>
    public class BaseTimesheetModel
    {
        /// <summary>
        /// unique Identifier
        /// </summary>
        public long TimesheetId { get; set; }

        /// <summary>
        /// Refers the Assignment Number to which the Timesheet Belong
        /// </summary>
        public int? AssignmentId { get; set; }

        /// <summary>
        /// last Modified Date
        /// </summary>
        public DateTime? LastModifiedOn { get; set; }

        /// <summary>
        /// Last Modified User Name
        /// </summary>
        public string ModifiedBy { get; set; }

        /// <summary>
        /// Updated Count
        /// </summary>
        public int? UpdateCount { get; set; }



       
    }
}
