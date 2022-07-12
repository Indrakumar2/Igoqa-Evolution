using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Evolution.Email.Domain.Enums
{
    public enum EmailSendStatus
    {
        [DisplayName("Error")]
        ERR,
        [DisplayName("Sent")]
        SNT,
        [DisplayName("New")]
        NEW
    }
}
