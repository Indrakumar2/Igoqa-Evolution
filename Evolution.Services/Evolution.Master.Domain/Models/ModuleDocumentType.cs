using System;
using System.Collections.Generic;
using System.Text;

namespace Evolution.Master.Domain.Models
{
    public class ModuleDocumentType : BaseMasterModel
    {
        public string ModuleName { get; set; }

        public bool? IsTSVisible { get; set; }
    }
}
