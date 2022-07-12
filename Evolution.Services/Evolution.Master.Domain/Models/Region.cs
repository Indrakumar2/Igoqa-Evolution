using System;
using System.Collections.Generic;
using System.Text;

namespace Evolution.Master.Domain.Models
{
   public  class Region :BaseMasterModel
   {
        public bool? IsActive { get; set; } = true;
   }
}
