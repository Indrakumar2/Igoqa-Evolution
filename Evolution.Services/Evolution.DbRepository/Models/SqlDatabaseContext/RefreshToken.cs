using System;
using System.Collections.Generic;

namespace Evolution.DbRepository.Models.SqlDatabaseContext
{
    public partial class RefreshToken
    {
        public int Id { get; set; }
        public string Token { get; set; }
        public string Username { get; set; }
        public string RequestedIp { get; set; }
        public long TokenExpiretime { get; set; }
        public string Application { get; set; }
        public string AccessToken { get; set; }
    }
}
