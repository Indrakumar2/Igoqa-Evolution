using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Cryptography;
using Evolution.Email.Domain.Enums;
using Evolution.Email.Domain.Interfaces.Email;
using Evolution.Email.Domain.Models;
using Evolution.Email.Interfaces;
using Evolution.Logging.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Evolution.Email.Notification
{
    public class EmailSenderService
    {
        private readonly IEmailQueueService _emailQueueService = null;
        private readonly IAppLogger<EmailSenderService> _logger = null;
        private readonly IEmailService _emailService = null;

        private bool _isMailSendInProgress = false;

        public EmailSenderService(IEmailQueueService emailQueueService,
                                  IEmailService emailService,
                                  IAppLogger<EmailSenderService> logger)
        {
            _emailService = emailService;
            _emailQueueService = emailQueueService;
            _logger = logger;
        }

        public void PerformEmailSending()
        {
            try
            {
                if (!this._isMailSendInProgress)
                {
                    //Console.Clear();
                    //Console.WriteLine("Don't Press Enter Key Otherwise System Will Shutdown.");
                    this.Start();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString());
                //Console.WriteLine(ex.ToFullString());
            }
        }

        private void Start()
        {
            try
            {
                this._isMailSendInProgress = true;
                PrintMessage("Email Sending Started", null);

                var statuses = new List<EmailSendStatus>() { EmailSendStatus.ERR, EmailSendStatus.NEW };
                IList<EmailQueueMessage> emailQueueMessages = _emailQueueService.Get(statuses)
                                                                                .Result?
                                                                                .Populate<IList<EmailQueueMessage>>();

                PrintMessage(string.Format("Total no. of email found ({0}) to be send.", (emailQueueMessages == null ? 0 : emailQueueMessages.Count)), null);
                _logger.LogError("Total no. of email found ({0}) to be send ", Convert.ToString(emailQueueMessages == null ? 0 : emailQueueMessages.Count));

                var emailToBeSends = new List<KeyValuePair<int, Models.EmailMessage>>();
                if (emailQueueMessages?.Count > 0)
                {
                    foreach (var email in emailQueueMessages)
                    {
                        if (email.IsMailSendAsGroup)
                            emailToBeSends.Add(new KeyValuePair<int, Models.EmailMessage>(email.Id, this.PopulateEmailMessage(email.ToAddresses, email)));
                        else
                        {
                            email.ToAddresses.ForEach(x =>
                            {
                                emailToBeSends.Add(new KeyValuePair<int, Models.EmailMessage>(email.Id, this.PopulateEmailMessage(x.Address, email)));
                            }); 
                        }
                    }

                    var recordStatusToBeUpdate = new List<EmailQueueMessage>();
                    emailToBeSends.ForEach(x =>
                    {
                        var emailQueue = emailQueueMessages.FirstOrDefault(x1 => x1.Id == x.Key);
                        try
                        {
                            this._emailService.Send(x.Value);                            
                            if (emailQueue != null)
                            {
                                emailQueue.LastAttemptOn = DateTime.UtcNow;
                                emailQueue.EmailStatus = EmailSendStatus.SNT.ToString();
                                recordStatusToBeUpdate.Add(emailQueue);
                            }
                        }
                        catch (Exception ex)
                        {
                            if (emailQueue != null)
                            {
                                emailQueue.LastAttemptOn = DateTime.UtcNow;
                                emailQueue.EmailStatus = EmailSendStatus.ERR.ToString();
                                emailQueue.StatusReason = ex.Message.ToString();
                                recordStatusToBeUpdate.Add(emailQueue);
                            }

                            _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), x.Value);
                            //Console.WriteLine(ex.ToFullString());
                        }
                    });

                    this.UpdateEmailStatus(recordStatusToBeUpdate);
                }

                PrintMessage("Email Send Process is Completed.", null);
            }
            catch (Exception ex)
            {
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString());
                //Console.WriteLine(ex.ToFullString());
            }
            finally
            {
                this._isMailSendInProgress = false;
            }
        }

        private void PrintMessage(string message, object document)
        {
            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine(string.Format("{0}  DateTime :- {1}", message, DateTime.Now.ToString()));
            if (document != null)
            {
                Console.WriteLine("Document Detail :-");
                Console.WriteLine(document?.ToText());
            }
            _logger.LogInformation("", string.Format("{0} $ DateTime :- {1}", message, DateTime.Now.ToString()), document?.ToText());
        }

        private Models.EmailMessage PopulateEmailMessage(string emailId, EmailQueueMessage emailQueueMessage)
        {
            return this.PopulateEmailMessage(new List<Models.EmailAddress>() { new Models.EmailAddress() { Address = emailId } }, emailQueueMessage);
        }

        private Models.EmailMessage PopulateEmailMessage(List<Models.EmailAddress> emails, EmailQueueMessage emailQueueMessage)
        {
            string placeHolderEmailId = string.Empty;

            if (!emailQueueMessage.IsMailSendAsGroup && emails?.Count == 1)
                placeHolderEmailId = emails.FirstOrDefault().Address;

            return new Models.EmailMessage()
            {
                ToAddresses = emails,
                FromAddresses = emailQueueMessage.FromAddresses,
                BccAddresses = emailQueueMessage.BccAddresses,
                CcAddresses = emailQueueMessage.CcAddresses,
                Content = this.ParseMailBody(placeHolderEmailId, emailQueueMessage.Content, emailQueueMessage.BodyPlaceHolderAndValue, emailQueueMessage.IsContentEncrypt, emailQueueMessage.PrivateKey),
                IsUseHtmlFormat = true,
                Subject = emailQueueMessage.Subject,
                Attachments= emailQueueMessage.Attachments
            };
        }

        private string ParseMailBody(string emailId, string content, string contentPlaceHolder, bool isMailBodyEncrypted = false, string privateKey = null)
        {
            string result = content;

            if (isMailBodyEncrypted)
            {
                result = new SymmetricCryptography().Decrypt(content, privateKey);
                contentPlaceHolder = new SymmetricCryptography().Decrypt(contentPlaceHolder, privateKey); ;
            }

            if (!string.IsNullOrEmpty(contentPlaceHolder) && !string.IsNullOrEmpty(emailId))
            {
                var splitted = contentPlaceHolder.Split("||");
                if (splitted?.Length > 0)
                {
                    foreach (var placeHolder in splitted)
                    {
                        var splitted1 = placeHolder.Split('$');
                        if (splitted1?.Length == 3 && splitted1[2].Trim() == emailId.Trim())
                        {
                            result = result.Replace(splitted1[0], splitted1[1]);
                        }
                    }
                }
            }
            return result;
        }

        private void UpdateEmailStatus(IList<EmailQueueMessage> emailQueueMessage)
        {
            try
            {
                _emailQueueService.Modify(emailQueueMessage);
            }
            catch (Exception ex)
            {
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString());
                //Console.WriteLine(ex.ToFullString());
            }
        }
    }
}
