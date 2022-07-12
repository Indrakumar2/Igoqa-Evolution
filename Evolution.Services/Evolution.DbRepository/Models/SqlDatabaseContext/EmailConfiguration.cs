using System;
using System.Collections.Generic;

namespace Evolution.DbRepository.Models.SqlDatabaseContext
{
    public partial class EmailConfiguration
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Server { get; set; }
        public int Port { get; set; }
        public string ServerUser { get; set; }
        public string ServerUserPassword { get; set; }
        public bool UseSslConnection { get; set; }
        public string ConfigType { get; set; }
        public bool IsDefault { get; set; }
    }
}
