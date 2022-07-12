using System;
using System.Collections.Generic;
using System.Text;

namespace Evolution.Assignment.Domain.Models.Assignments
{
    /// <summary>
    /// Contains the information of assignment technical specialist name and unique number
    /// </summary>
    public class TechnicalSpecialist
    {
        [AuditNameAttribute("Assignment Id")]
        public int? AssignmentId { get; set; }

        [AuditNameAttribute("Full Name")]
        public string FullName
        {
            get
            {
                return string.Format("{0}, {1} ({2})", LastName, FirstName, Pin);
            }
            private set { }
        }

        [AuditNameAttribute("First Name")]
        public string FirstName { get; set; }

        [AuditNameAttribute("Last Name")]
        public string LastName { get; set; }

        [AuditNameAttribute("PIN")]
        public int? Pin { get; set; }

        [AuditNameAttribute("ProfileStatus")]
        public string ProfileStatus { get; set; }
    }
}
