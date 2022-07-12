using Evolution.Common.Models.Base;
using System;

namespace Evolution.Home.Domain.Models.Homes
{
    public class MyTask:BaseModel
    {
        public int? MyTaskId { get; set; }
        public string Moduletype { get; set; }
        public string TaskType { get; set; }
        public string TaskRefCode { get; set; } 
        public string AssignedBy { get; set; } 
        public string AssignedTo { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string Description { get; set; }

        public string CompanyCode { get; set; } //D661 issue1 myTask CR
        public string CompanyName { get; set; } // D363 CR Change
    }
}
