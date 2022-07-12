using System;
using System.Collections.Generic;

namespace Evolution.DbRepository.Models.SqlDatabaseContext
{
    public partial class BatchProcess
    {
        public int Id { get; set; }
        public int BatchId { get; set; }
        public int ParamId { get; set; }
        public int ProcessStatus { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string ErrorMessage { get; set; }
        public int? ReportType { get; set; }
        public string ReportFileName { get; set; }
        public string ReportFilePath { get; set; }
        public string ReportParam { get; set; }
        public string FileExtension { get; set; }
        public bool? IsDeleted { get; set; }
        public int? FailCount { get; set; }
    }
}
