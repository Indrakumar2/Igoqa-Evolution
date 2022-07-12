using System;
using System.Collections.Generic;
using System.Text;

namespace Evolution.Security.Domain.Models.Security
{
    public class UserMenuPermissionInfo
    {
        public string Module { get; set; }

        public string MenuName { get; set; }

        public bool IsVisible { get; set; }
    }
}
