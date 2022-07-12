using Evolution.Common.Models.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Evolution.Security.Domain.Models.Security
{
    public class ModuleDetail
    {
        public ModuleInfo Module { get; set; }

        public IList<ActivityInfo> Activities { get; set; }
    }
}
