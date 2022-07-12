using System;
using System.Collections.Generic;
using System.Text;

namespace Evolution.Master.Domain.Models
{
   public  class CompanyChargeSchedule :BaseMasterModel
   {
       
         public string CompanyCode { get; set; }

         public string Currency { get; set; }
   }
}
