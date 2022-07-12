using System;
using System.Collections.Generic;

namespace Evolution.DbRepository.Models.SqlDatabaseContext
{
    public partial class DocumentUploadPath
    {
        public int Id { get; set; }
        public string ServerName { get; set; }
        public string FolderPath { get; set; }
        public bool IsActive { get; set; }
        public bool IsFull { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? LastModification { get; set; }
        public string ModifiedBy { get; set; }
    }
}
