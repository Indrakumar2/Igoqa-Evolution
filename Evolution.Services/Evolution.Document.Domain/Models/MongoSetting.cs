using System;
using System.Collections.Generic;
using System.Text;

namespace Evolution.Document.Domain.Models
{
    public class MongoSetting
    {
        public string ConnectionString { get; set; }

        public string DatabaseName { get; set; }

        /// <summary>
        /// Deleimeted Value
        /// </summary>
        public string DocumentTypes { get; set; }
    }
}
