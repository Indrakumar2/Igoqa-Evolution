using System;

namespace Evolution.Common.Models.Base
{
    public class BaseMessage
    {
        [AuditNameAttribute("Message Id")] public int? Id { get; set; }

        [AuditNameAttribute("Identifier")] public string MsgIdentifier { get; set; }

        [AuditNameAttribute("Msg Type")] public string MsgType { get; set; }

        [AuditNameAttribute("Text")] public string MsgText { get; set; }

        [AuditNameAttribute("Is DefaultMsg")] public bool IsDefaultMsg { get; set; }

        [AuditNameAttribute("Is Active")] public bool IsActive { get; set; }

        public string RecordStatus { get; set; }
        public byte? UpdateCount { get; set; }
        public DateTime? LastModification { get; set; }
        public string ModifiedBy { get; set; }
    }
}