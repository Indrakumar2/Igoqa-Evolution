using Evolution.Common.Constants;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Email.Domain.Interfaces.Email;
using Evolution.Email.Domain.Models;
using Evolution.Email.Models;
using Evolution.Logging.Interfaces;
using Evolution.Security.Domain.Interfaces.Data;
using Evolution.TechnicalSpecialist.Domain.Interfaces.Data;
using Evolution.TechnicalSpecialist.Domain.Models.TechSpecialist;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Evolution.Email.ExpiryNotification
{
    public class ExpiryNotificationService
    {
        private readonly IEmailQueueService _emailQueueService = null;
        private readonly IAppLogger<ExpiryNotificationService> _logger = null; 
        private readonly ITechnicalSpecialistRepository _technicalSpecialistRepository= null;
        private readonly IUserRepository _userRepository = null;

        private bool _isMailSendInProgress = false;

        public ExpiryNotificationService(IEmailQueueService emailQueueService,
                                    ITechnicalSpecialistRepository technicalSpecialistRepository,
                                    IUserRepository userRepository,
                                    IAppLogger<ExpiryNotificationService> logger)
        { 
            _emailQueueService = emailQueueService;
            _technicalSpecialistRepository = technicalSpecialistRepository;
            _userRepository = userRepository;
            _logger = logger;
        }

        public void FindingExpiredRecords()
        {
            try
            {
                if (!this._isMailSendInProgress)
                { 
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
            IList<EmailQueueMessage> recordToBeAdded = new List<EmailQueueMessage>();
            try
            { 
                this._isMailSendInProgress = true;
                PrintMessage("Finding Expired Records Process Started", null); 
                var expiredRecords= _technicalSpecialistRepository.GetExpiredRecords(); 
                PrintMessage(string.Format("Total no of expired records found ({0}).", (expiredRecords == null ? 0 : expiredRecords.Count)), null);
                if (expiredRecords != null && expiredRecords.Any())
                {
                    recordToBeAdded= ProcessEmailNotifications(expiredRecords);
                    this.AddNotificationEmails(recordToBeAdded);
                } 
                PrintMessage("Finding Expired Records Process Completed.", null);
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

        private void AddNotificationEmails(IList<EmailQueueMessage> emailQueueMessage)
        {
            try
            {
                _emailQueueService.Add(emailQueueMessage);
            }
            catch (Exception ex)
            {
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString());
                //Console.WriteLine(ex.ToFullString());
            }
        }

        private IList<EmailQueueMessage> ProcessEmailNotifications(IList<TechnicalSpecialistExpiredRecord> tsExpiredRecords)
        {
            IList<EmailQueueMessage> emailMessages = new List<EmailQueueMessage>();
            string emailSubject = string.Empty;   
            List<EmailAddress> toAddresses = null;
            List<EmailAddress> ccAddresses = null;
            IList<KeyValuePair<string, string>> emailContentPlaceholders = null;
            if (tsExpiredRecords != null && tsExpiredRecords.Any())
            {
                var companyIds = tsExpiredRecords.Select(x => x.CompanyId).Distinct().ToList();
                var rcUsers = _userRepository.GetUserByType(companyIds, new List<string> { "ResourceCoordinator" });
                emailSubject = string.Format(TechnicalSpecialistConstants.Email_Notification_RecordsNearExpiry_Subject);

                tsExpiredRecords.ToList().ForEach(e => {
                    toAddresses = new List<EmailAddress> { new EmailAddress() { Address = e.Email } };
                    ccAddresses = rcUsers?.Where(x => x.CompanyId == e.CompanyId)?.Select(x => new EmailAddress() { DisplayName = x.User?.Name, Address = x.User?.Email }).ToList();
                    emailContentPlaceholders = new List<KeyValuePair<string, string>> {
                                    new KeyValuePair<string, string>(TechnicalSpecialistConstants.Email_Content_Document_Type, e.DocumentType),
                                    new KeyValuePair<string, string>(TechnicalSpecialistConstants.Email_Content_Expiry_Date,e.ExpiryDate.Value.ToString("dd-MMM-yyyy")),
                                };
                   
                    var emailMessage = _emailQueueService.PopulateEmailQueueMessage(ModuleType.TechnicalSpecialist, EmailTemplate.EmailRecordsNearExpiry, EmailType.DCE, ModuleCodeType.TS, e.TechnicalSpecialistId.ToString(), emailSubject, emailContentPlaceholders, toAddresses, ccAddresses);

                    emailMessages.Add(emailMessage);
                });

            }

            return emailMessages;
        }
    }
}
