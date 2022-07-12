using System;
using System.Collections.Generic;

namespace Evolution.DbRepository.Models.SqlDatabaseContext
{
    public partial class ServiceTypeProjectType
    {
        public int Id { get; set; }
        public int ServiceTypeId { get; set; }
        public int ProjectTypeId { get; set; }
        public byte? UpdateCount { get; set; }
        public string ModifiedBy { get; set; }

        public virtual Data ProjectType { get; set; }
        public virtual Data ServiceType { get; set; }
    }
}
