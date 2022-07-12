using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Evolution.Visit.Domain.Enums
{
    public enum VisitStatus
    {
        [Display(Name = "Approved By Contract Holder")]
        A,
        [Display(Name = "Awaiting Approval")]
        C,
        [Display(Name = "Rejected By Operator")]
        J,
        [Display(Name = "Not Submitted")]
        N,
        [Display(Name = "Approved By Operator")]
        O,
        [Display(Name = "Confirmed - Awaiting Visit")]
        Q,
        [Display(Name = "Rejected By Contract Holder")]
        R,
        [Display(Name = "Tentative - Pending Approval")]
        T,
        [Display(Name = "TBA - Date Unknown")]
        U,
        [Display(Name = "Unused Visit")]
        D,
    }
}
