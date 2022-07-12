using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Evolution.Admin.Domain.Models.Admins
{
    public class Batches
    {
        public int Id { get; set; }
        public int BatchID { get; set; }
        public int ParamID { get; set; }
        public int ProcessStatus { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string ErrorMessage { get; set; }
        public string BatchStatus
        {
            get
            {
                if (BatchID == 1 && (ProcessStatus == 0 || ProcessStatus == 1))
                    return "Framework Contract Schedule is getting copied to Related Framework Contracts";
                else if (BatchID == 1 && ProcessStatus == 2)
                    return "Framework Contract Schedules were successfully copied to Related Framework Contracts on " + UpdatedDate?.ToString("dd-MMM-yyyy");
                else if (BatchID == 1 && ProcessStatus == 3)
                {
                    string message = ErrorMessage;
                    return message + " Failed on " + UpdatedDate?.ToString("dd-MMM-yyyy");
                }
                return string.Empty;
            }
        }
        public string FileStatus
        {
            get
            {
                if (ProcessStatus == 0)
                    return "Background process Initiated";
                if (ProcessStatus == 1)
                    return "Processing";
                if (ProcessStatus == 2)
                    return "Completed";
                if (ProcessStatus == 3)
                    return "Failed";
                return string.Empty;
            }
        }
        public bool IsDisabled
        {
            get
            {
                if (ProcessStatus == 0 || ProcessStatus == 1 || ProcessStatus == 3)
                    return true;
                else
                    return false;
            }
        }
        public string FullPath
        {
            get
            {
                if (ProcessStatus == 0 || ProcessStatus == 1 || ProcessStatus == 3)
                    return string.Empty;
                else
                    return ReportFilePath + ReportFileName;
            }
        }
        public int? ReportType { get; set; }
        public string DisplayFileName
        {
            get
            {
                if (!string.IsNullOrEmpty(ReportFileName))
                    return ReportFileName.Split('-')?.FirstOrDefault() + FileExtension;
                else
                    return string.Empty;
            }
        }
        public string ReportFileName { get; set; }
        public string ReportFilePath { get; set; }
        public string ReportParam { get; set; }
        public string FileExtension { get; set; }
        public bool? IsDeleted { get; set; }
        public int FailCount { get; set; }
    }
}