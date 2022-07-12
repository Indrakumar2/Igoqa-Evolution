using System.Collections.Generic;

namespace Evolution.Email.Models
{
    public class EmailMessage
    {
        public EmailMessage()
        {
            ToAddresses = new List<EmailAddress>();
            FromAddresses = new List<EmailAddress>();
            BccAddresses = new List<EmailAddress>();
            CcAddresses = new List<EmailAddress>();

        }

        public List<EmailAddress> ToAddresses { get; set; }

        public List<EmailAddress> FromAddresses { get; set; }

        public List<EmailAddress> BccAddresses { get; set; }

        public List<EmailAddress> CcAddresses { get; set; }

        public string Subject { get; set; }

        public string Content { get; set; }

        public bool IsUseHtmlFormat { get; set; }

        public string BodyPlaceHolderAndValue { get; set; }

        public List<Attachment> Attachments { get; set; }
    }

    public class Attachment
    {
        public string UniqueKey { get; set; }
     
        public string AttachmentFileLimit { get; set; }

        public string AttachmentFileName { get; set; }

        //public string AttachmentDocumentPath { get; set; }
    }

}
