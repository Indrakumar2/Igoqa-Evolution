using Evolution.Email.Interfaces;

namespace Evolution.Email.Models
{
    ///TODO : Use below configuration in config otherwise set manually.
    //  "EmailConfiguration": {
    //    IncommingServer
    //    {
    //        "ServerName": "imps.myserver.com",
    //        "ServerPort": 465,
    //        "Username": "username",
    //        "Password": "password",
    //    },
    //    OutgoingServer
    //    {
    //        "ServerName": "imps.myserver.com",
    //        "ServerPort": 465,
    //        "Username": "username",
    //        "Password": "password",
    //}
    //     }

    public class EmailConfiguration : IEmailConfiguration
    {
        public IEmailServerInfo IncommingServer
        {
            get;
            set;
        }

        public IEmailServerInfo OutgoingServer
        {
            get;
            set;
        }
    }
}
