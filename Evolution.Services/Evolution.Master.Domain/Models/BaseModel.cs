using System;
using System.Collections.Generic;
using System.Text;

namespace Evolution.Master.Domain.Models
{
    public class BaseMasterModel
    {
        public string Name { get; set; }

        public int? Id { get; set; }

        public DateTime? LastModification { get; set; }

        public byte? UpdateCount { get; set; }

        public string ModifiedBy { get; set; }

        public string RecordStatus { get; set; }

        public bool? IsActive { get; set; }   //Changes for Defect 112

        public bool IsFromRefresh { get; set; }
    }
}
