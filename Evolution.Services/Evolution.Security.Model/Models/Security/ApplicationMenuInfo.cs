using Evolution.Common.Models.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Evolution.Security.Domain.Models.Security
{
    public class ApplicationMenuInfo
    {
        public string ApplicationName { get; set; }

        public string ModuleName { get; set; }

        public string MenuName { get; set; }

        public string ActivityCodes { get; set; }

        public bool? IsActive { get; set; }

        //public string ParentMenu { get; set; }
    }

    public class ApplicationInfo : BaseModel
    {
        public string ApplicationName { get; set; }

        public string Description { get; set; }
    }
}
