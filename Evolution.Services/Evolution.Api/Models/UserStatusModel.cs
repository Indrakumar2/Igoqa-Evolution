using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Evolution.Api.Models
{
    public class UserStatusModel
    {
        public string UserSamaAccountName { set; get; }

        public bool NewStatus { set; get; }
    }
}
