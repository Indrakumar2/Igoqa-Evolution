using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Evolution.ResourceSearch.Domain.Enums
{
    public enum EmploymentType
    {
        [Display(Name="FT Employee")]
        Ft=1,

        [Display(Name = "PT Employee")]
        Pt=2,

        [Display(Name = "Independent Contractor")]
        Ic=3,

        [Display(Name = "Third Party Contractor")]
        Tpc =4,

        [Display(Name = "Temp Employee")]
        Temp = 5,

        [Display(Name = "Office Staff")]
        OfficeStaff =6,
          
        [Display(Name = "Former")]
        Former =7,
        
        [Display(Name ="Prospect")]
        Prospect =8,

        [Display(Name ="Offline Technical Specialist")]
        OTS =9,

        [Display(Name ="None")]
        None =100
    }
}
