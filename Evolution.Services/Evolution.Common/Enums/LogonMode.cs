using System.ComponentModel;

namespace Evolution.Common.Enums
{
    public enum LogonMode
    {
        [DisplayName("Active Directory")]
        AD,
        [DisplayName("User Password")]
        UP
    }
}
