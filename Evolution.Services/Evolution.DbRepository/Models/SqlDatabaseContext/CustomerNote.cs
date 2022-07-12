﻿using System;
using System.Collections.Generic;

namespace Evolution.DbRepository.Models.SqlDatabaseContext
{
    public partial class CustomerNote
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string Note { get; set; }
        public DateTime? LastModification { get; set; }
        public byte? UpdateCount { get; set; }
        public string ModifiedBy { get; set; }

        public virtual Customer Customer { get; set; }
    }
}
