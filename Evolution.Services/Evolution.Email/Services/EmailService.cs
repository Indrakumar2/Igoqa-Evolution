using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Common.Models.Base;
using Evolution.Document.Domain.Interfaces.Documents;
using Evolution.Document.Domain.Models.Document;
using Evolution.Email.Interfaces;
using Evolution.Email.Models;
using Evolution.Logging.Interfaces;
using MailKit.Net.Pop3;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Evolution.Email.Services
{
    public class EmailService : IEmailService
    {
        private readonly IEmailConfiguration _emailConfiguration;
        private readonly IDocumentService _documentService;
        private readonly IAppLogger<EmailService> _logger = null;
        private readonly AppEnvVariableBaseModel _environment = null;
        public readonly string _emailAttachmentDocumentEndpoint = "documents/EmailAttachmentDocuments";

        public EmailService(IEmailConfiguration emailConfiguration, IDocumentService documentService, IAppLogger<EmailService> logger, IOptions<AppEnvVariableBaseModel> environment)
        {
            _emailConfiguration = emailConfiguration;
            _documentService = documentService;
            _logger = logger;
            _environment = environment.Value;
        }

        public List<EmailMessage> ReceiveEmail(int maxCount = 10)
        {
            using (var emailClient = new Pop3Client())
            {
                emailClient.Connect(_emailConfiguration.IncommingServer.EmailServerName,
                                    _emailConfiguration.IncommingServer.EmailServerPort,
                                    _emailConfiguration.IncommingServer.IsEmailUseSslWrappedConnection);

                emailClient.AuthenticationMechanisms.Remove("XOAUTH2");

                emailClient.Authenticate(_emailConfiguration.IncommingServer.EmailUsername,
                                        _emailConfiguration.IncommingServer.EmailUserPassword);

                List<EmailMessage> emails = new List<EmailMessage>();
                for (int i = 0; i < emailClient.Count && i < maxCount; i++)
                {
                    var message = emailClient.GetMessage(i);
                    var emailMessage = new EmailMessage
                    {
                        Content = !string.IsNullOrEmpty(message.HtmlBody) ? message.HtmlBody : message.TextBody,
                        Subject = message.Subject
                    };
                    emailMessage.ToAddresses.AddRange(message.To.Select(x => (MailboxAddress)x).Select(x => new EmailAddress { Address = x.Address, DisplayName = x.Name }));
                    emailMessage.FromAddresses.AddRange(message.From.Select(x => (MailboxAddress)x).Select(x => new EmailAddress { Address = x.Address, DisplayName = x.Name }));
                    if (message.Cc?.Count > 0)
                        emailMessage.FromAddresses.AddRange(message.Cc.Select(x => (MailboxAddress)x).Select(x => new EmailAddress { Address = x.Address, DisplayName = x.Name }));
                    if (message.Bcc?.Count > 0)
                        emailMessage.FromAddresses.AddRange(message.Bcc.Select(x => (MailboxAddress)x).Select(x => new EmailAddress { Address = x.Address, DisplayName = x.Name }));
                }
                return emails;
            }
        }

        public void Send(EmailMessage emailMessage)
        {
            if (emailMessage == null)
                throw new ArgumentNullException("EmailMessage is null.");
            if (emailMessage.ToAddresses == null && emailMessage.ToAddresses.Count <= 0)
                throw new ArgumentNullException("ToAdress is either null or empty.");

            var message = new MimeMessage();
            if (emailMessage.FromAddresses != null && emailMessage.FromAddresses.Any())//DEf 978 fix
            {
                message.From.AddRange(emailMessage.FromAddresses.Select(x => new MailboxAddress(x.DisplayName, x.Address)));
            }
            else {
                message.From.Add(new MailboxAddress(_emailConfiguration.OutgoingServer.EmailUsername));
            } 

            if (!_emailConfiguration.OutgoingServer.IsSandBoxEnvironment)
            {
                message.To.AddRange(emailMessage.ToAddresses.Select(x => new MailboxAddress(x.DisplayName, x.Address)));
                if (emailMessage.BccAddresses?.Count > 0)
                    message.Bcc.AddRange(emailMessage.BccAddresses.Select(x => new MailboxAddress(x.DisplayName, x.Address)));
                if (emailMessage.CcAddresses?.Count > 0)
                    message.Cc.AddRange(emailMessage.CcAddresses.Select(x => new MailboxAddress(x.DisplayName, x.Address)));
            }
            else
            {
                message.To.AddRange(new List<MailboxAddress>() { new MailboxAddress(_emailConfiguration.OutgoingServer.SandBoxEnvirnonmentTOEmail) });
                if (!string.IsNullOrEmpty(_emailConfiguration.OutgoingServer.SandBoxEnvirnonmentCCEmail) && emailMessage.CcAddresses?.Count > 0)
                    message.Cc.AddRange(new List<MailboxAddress>() { new MailboxAddress(_emailConfiguration.OutgoingServer.SandBoxEnvirnonmentCCEmail) });
            }

            message.Subject = emailMessage.Subject;

            if (emailMessage.Attachments?.Count > 0)
            {
                var bodyBuilder = new BodyBuilder();
                if (emailMessage.IsUseHtmlFormat == true)
                    bodyBuilder.TextBody = emailMessage.Content;
                if (emailMessage.IsUseHtmlFormat == true)
                    bodyBuilder.HtmlBody = emailMessage.Content;

                var attachments = Task.Run(async () => await PostRequestToFeatchAttachmentDocuments(emailMessage.Attachments.Select(x => x.UniqueKey).ToList())).Result;//def 1289 fix
                if (attachments!=null && attachments.Any())
                {
                    for (int i = 0; i < emailMessage.Attachments?.Count; i++)
                    {
                        var documentData= attachments?.FirstOrDefault(x => x.DocumentUniqueName == emailMessage.Attachments[i].UniqueKey);
                        bodyBuilder.Attachments.Add(documentData?.DocumentName, documentData?.FileContent);
                        message.Body = bodyBuilder.ToMessageBody();
                    }
                }  
            }
            else
            {
                //We will say we are sending HTML. But there are options for plaintext etc. 
                message.Body = new TextPart(emailMessage.IsUseHtmlFormat ? TextFormat.Html : TextFormat.Plain)
                {
                    Text = emailMessage.Content
                };
            }

            //Be careful that the SmtpClient class is the one from Mailkit not the framework!
            using (var emailClient = new SmtpClient())
            {
                try
                {
                    emailClient.ServerCertificateValidationCallback = (s, c, h, e) => true; // Added to resolve certificate error in ITK server
                    //The last parameter here is to use SSL (Which you should!)
                    emailClient.Connect(_emailConfiguration.OutgoingServer.EmailServerName,
                                        _emailConfiguration.OutgoingServer.EmailServerPort,
                                        _emailConfiguration.OutgoingServer.IsEmailUseSslWrappedConnection
                                        );

                    //Removing OAuth functionality as we won't be using it. 
                    emailClient.AuthenticationMechanisms.Remove("XOAUTH2");

                    //Intertek uses O365 SMTP relay which does not require authentication, so this should be optiona
                    if (_emailConfiguration.OutgoingServer.IsAuthenticationRequired)
                    {
                        emailClient.Authenticate(_emailConfiguration.OutgoingServer.EmailUsername, _emailConfiguration.OutgoingServer.EmailUserPassword);
                    }
                    emailClient.Send(message);
                }
                catch (Exception cex)
                {
                    throw cex;
                }
                finally
                {
                    if (emailClient.IsConnected)
                        emailClient.Disconnect(true);
                }
            }
        }
        //def 1289
        private async Task<IList<DocumentDownloadResult>> PostRequestToFeatchAttachmentDocuments(IList<string> documentUniqueNames)
        {
            Exception exception = null;
            try
            {
                HttpClientHandler clientHandler = new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; }
                };

                // Pass the handler to httpclient(to call api)
                using (var httpClient = new HttpClient(clientHandler))
                {  
                    HttpContent content = new StringContent(JsonConvert.SerializeObject(documentUniqueNames), Encoding.UTF8, "application/json");
                    var response =await httpClient.PostAsync(new Uri(_environment.ApplicationGatewayURL + _emailAttachmentDocumentEndpoint), content);
                    if (!response.IsSuccessStatusCode)
                        _logger.LogError(ResponseType.Exception.ToId(), response?.ReasonPhrase, documentUniqueNames);
                    else if (response.IsSuccessStatusCode)
                        return (IList<DocumentDownloadResult>) JsonConvert.DeserializeObject(response.Content.ReadAsStringAsync().Result,typeof(IList<DocumentDownloadResult>));
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), documentUniqueNames);
            }
            return null;
        }
    }
}
