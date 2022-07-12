using System;
using System.Collections.Generic;
using System.Text;

namespace Evolution.Master.Domain.Models
{
    public class VisitStatus:BaseMasterModel
    {
        public string Code { get; set; }

        public int Precedence { get; set; }
    }
}
