using System;
using System.Collections.Generic;
using System.Text;

namespace Evolution.Master.Domain.Models
{
  public class UnusedReason:BaseMasterModel
    {
        public string Code { set; get; }
        public int Precedences { set; get; }
    }
}
