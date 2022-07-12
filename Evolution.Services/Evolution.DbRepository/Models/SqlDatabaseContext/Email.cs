using System;
using System.Collections.Generic;

namespace Evolution.DbRepository.Models.SqlDatabaseContext
{
    public partial class Email
    {
        public int Id { get; set; }
        public string ToEmail { get; set; }
        public string CcEmail { get; set; }
        public string BccEmail { get; set; }
        public string Subject { get; set; }
        public string BodyContent { get; set; }
        public string EmailStatus { get; set; }
        public string StatusReason { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? LastAttemptOn { get; set; }
        public int? RetryCount { get; set; }
        public string ModuleCode { get; set; }
        public string ModuleEmailRefCode { get; set; }
        public string EmailType { get; set; }
        public string BodyPlaceHolderAndValue { get; set; }
        public bool? IsMailSendAsGroup { get; set; }
        public bool IsMailContentEncrypted { get; set; }
        public string PrivateKey { get; set; }
        public string Attachment { get; set; }
        public string FromEmail { get; set; }
        public string Token { get; set; }
    }
}
