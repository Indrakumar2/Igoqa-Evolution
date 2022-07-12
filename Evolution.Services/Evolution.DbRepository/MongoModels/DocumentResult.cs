using System;
using System.Collections.Generic;
using System.Text;

namespace Evolution.DbRepository.MongoModels
{
    public class DocumentResult
    {
        public string DocumentName { get; set; }
        public byte[] DocumentData { get; set; }
    }
}
