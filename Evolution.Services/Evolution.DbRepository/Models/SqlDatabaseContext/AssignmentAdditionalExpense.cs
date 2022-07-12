using System;
using System.Collections.Generic;

namespace Evolution.DbRepository.Models.SqlDatabaseContext
{
    public partial class AssignmentAdditionalExpense
    {
        public int Id { get; set; }
        public int AssignmentId { get; set; }
        public int CompanyId { get; set; }
        public int ExpenseTypeId { get; set; }
        public string Description { get; set; }
        public string Currency { get; set; }
        public decimal Rate { get; set; }
        public decimal TotalUnit { get; set; }
        public bool? IsAlreadyLinked { get; set; }
        public DateTime? LastModification { get; set; }
        public byte? UpdateCount { get; set; }
        public string ModifiedBy { get; set; }

        public virtual Assignment Assignment { get; set; }
        public virtual Company Company { get; set; }
        public virtual Data ExpenseType { get; set; }
    }
}
