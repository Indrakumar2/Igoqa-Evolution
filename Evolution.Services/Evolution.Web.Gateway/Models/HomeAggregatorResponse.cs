using System;

namespace Evolution.Web.Gateway.Models
{
    public class HomeAggregatorResponse
    {
        public Int32? AssignmentCount { get; set; }

        public Int32? InactiveAssignmentCount { get; set; }

        public Int32? VisitCount { get; set; }

        public Int32? TimesheetCount { get; set; }

        public Int32? ContractCount { get; set; }

        public Int32? DocumentApprovalCount { get; set; }
        public Int32? MyTaskCount { get; set; }
        public Int32? MySearchCount { get; set; }
    }
}
