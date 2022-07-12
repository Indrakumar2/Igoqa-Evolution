using System;
using System.Collections.Generic;

namespace Evolution.DbRepository.Models.SqlDatabaseContext
{
    public partial class TokenMessage
    {
        public int Id { get; set; }
        public int TokenId { get; set; }
        public DateTime? Period { get; set; }
        public string Message { get; set; }
        public byte? UpdateCount { get; set; }
        public DateTime? LastModification { get; set; }
        public string ModifiedBy { get; set; }

        public virtual Token Token { get; set; }
    }
}
