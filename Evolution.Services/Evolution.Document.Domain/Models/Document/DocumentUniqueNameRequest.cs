using Evolution.Common.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Evolution.Document.Domain.Models.Document
{
    public class DocumentUniqueNameDetail
    {
        public string ModuleCode { get; set; }

        public string ModuleCodeReference { get; set; }

        public string SubModuleCodeReference { get; set; }

        public string DocumentName { get; set; }

        public string UniqueName { get; set; }

        public string RequestedBy { get; set; }

        public string Status { get; set; }

        public string DocumentType { get; set; }

        public long? DocumentSize { get; set; }
    }
}
