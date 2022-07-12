using Evolution.Common.Enums;
using Evolution.Common.Models.Responses;
using Evolution.Email.Domain.Enums;
using Evolution.Email.Domain.Models;
using Evolution.Email.Models;
using Evolution.Master.Domain.Models;
using System.Collections.Generic;

namespace Evolution.Email.Domain.Interfaces.Email
{
    public interface IEmailQueueService
    {
        Response Get(EmailQueueMessage model);

        Response Get(IList<EmailSendStatus> statuses, int maxRetryValue = 3);

        Response Add(IList<EmailQueueMessage> emailMessages, bool commitChange = true, bool isDbValidationRequired = true);

        Response Modify(IList<EmailQueueMessage> emailMessages, bool commitChange = true, bool isDbValidationRequired = true);

        EmailQueueMessage PopulateEmailQueueMessage(ModuleType moduleType,
                                                    ModuleCodeType moduleCodeType,
                                                    string moduleRefCode,
                                                    EmailTemplate templateType,
                                                    EmailType emailType,
                                                    string emailSubject,
                                                    IList<EmailBodyPlaceHolder> emailContentPlaceholders,
                                                    List<EmailAddress> toAddresses,
                                                    List<EmailAddress> ccAddresses = null,
                                                    List<EmailAddress> bccAddresses = null,
                                                    bool IsMailSendAsGroup = false,
                                                    bool isEmailContentNeedToEncryp = false,
                                                    List<EmailAddress> fromAddresses = null);

        EmailQueueMessage PopulateEmailQueueMessage(ModuleType moduleType,
                                                    EmailTemplate emailTemplateType,
                                                    EmailType emailType,
                                                    ModuleCodeType moduleCodeType,
                                                    string moduleRefCode,
                                                    string emailSubject,
                                                    IList<KeyValuePair<string, string>> emailContentPlaceholders,
                                                    List<EmailAddress> toAddresses, 
                                                    List<EmailAddress> ccAddresses = null,
                                                    List<EmailAddress> bccAddresses = null,
                                                    List<EmailAddress> fromAddresses = null,
                                                    bool IsMailSendAsGroup = false);

        IList<SystemSetting> GetEmailTemplate(IList<string> emailTemplateKeyNames);
    }
}
