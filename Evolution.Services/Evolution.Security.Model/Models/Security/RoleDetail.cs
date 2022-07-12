using System;
using System.Collections.Generic;
using System.Text;

namespace Evolution.Security.Domain.Models.Security
{
    public class RoleDetail
    {
        public RoleInfo Role { get; set; }

        public IList<RoleModuleActivity> Modules { get; set; }
    }

    public class RoleModuleActivity
    {
        public ModuleInfo Module { get; set; }

        public IList<ActivityInfo> Activities { get; set; }
    }
}
