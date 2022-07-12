using System;
using System.Collections.Generic;
using System.Text;

namespace Evolution.Master.Domain.Models
{
  public  class TimeOffRequestCategory :BaseMasterModel
    {
        public string EmploymentType { get; set; }

        public string LeaveTypeCategory { get; set; }
    }
}
