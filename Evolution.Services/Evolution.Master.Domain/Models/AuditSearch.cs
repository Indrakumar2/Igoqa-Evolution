using System;
using System.Collections.Generic;
using System.Text;

namespace Evolution.Master.Domain.Models
{
    public class AuditSearch : BaseMasterModel
    {
        public string ModuleName { get; set; }

        public string SearchParameterName { get; set; }
    }
}
