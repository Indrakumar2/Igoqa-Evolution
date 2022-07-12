using System;

namespace Evolution.Email.Domain.Models
{
    public class EmailQueueMessage : Evolution.Email.Models.EmailMessage
    {
        public int Id { get; set; }
        public string EmailStatus { get; set; }
        public string StatusReason { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? LastAttemptOn { get; set; }
        public int? RetryCount { get; set; }
        public string ModuleCode { get; set; }
        public string ModuleEmailRefCode { get; set; }
        public string EmailType { get; set; }
        public bool IsMailSendAsGroup { get; set; }
        public bool IsContentEncrypt { get; set; }
        public string PrivateKey { get; set; }
        public string Token { get; set; }
    }

    public class EmailBodyPlaceHolder
    {
        public EmailBodyPlaceHolder(string placeHolderName,string placeHolderValue,string placeHolderForEmailId)
        {
            PlaceHolderName = placeHolderName;
            PlaceHolderValue = placeHolderValue;
            PlaceHolderForEmailId = placeHolderForEmailId;
        }

        public string PlaceHolderName { get; set; }

        public string PlaceHolderValue { get; set; }

        public string PlaceHolderForEmailId { get; set; }
    }
}
