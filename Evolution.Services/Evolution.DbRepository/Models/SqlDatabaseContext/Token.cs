using System;
using System.Collections.Generic;

namespace Evolution.DbRepository.Models.SqlDatabaseContext
{
    public partial class Token
    {
        public Token()
        {
            CostAccrual = new HashSet<CostAccrual>();
            RevenueAccrual = new HashSet<RevenueAccrual>();
            TokenMessage = new HashSet<TokenMessage>();
        }

        public int Id { get; set; }
        public string Token1 { get; set; }
        public string Logger { get; set; }
        public int? LoggerCompany { get; set; }
        public Guid ConversationHandle { get; set; }
        public string MessageBody { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public string TokenStatus { get; set; }
        public string TokenFor { get; set; }
        public byte? UpdateCount { get; set; }
        public DateTime? LastModification { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? CutOffDate { get; set; }
        public bool? IsStdChCompany { get; set; }
        public bool? IsPcChCompany { get; set; }
        public bool? IsOpcompany { get; set; }
        public int? LoggerCompanyId { get; set; }

        public virtual ICollection<CostAccrual> CostAccrual { get; set; }
        public virtual ICollection<RevenueAccrual> RevenueAccrual { get; set; }
        public virtual ICollection<TokenMessage> TokenMessage { get; set; }
    }
}
