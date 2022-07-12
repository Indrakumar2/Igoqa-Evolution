using Evolution.Email.Models;
using System.Collections.Generic;

namespace Evolution.Email.Interfaces
{
    public interface IEmailService
    {
        void Send(EmailMessage emailMessage);

        List<EmailMessage> ReceiveEmail(int maxCount = 10);
    }
}
