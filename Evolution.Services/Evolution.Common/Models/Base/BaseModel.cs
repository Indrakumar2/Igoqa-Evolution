using System;

namespace Evolution.Common.Models.Base
{
    public class BaseModel
    {
        public int? Id { get; set; }
        public Byte? UpdateCount { get; set; }
        public string RecordStatus { get; set; }
        public DateTime? LastModification { get; set; }
        public string ModifiedBy { get; set; }
        public long? EventId { get; set; }
        public string ActionByUser { get; set; }
        public string UserCompanyCode { get; set; }
        public int OffSet { get; set; } = 0;
        public int FetchCount { get; set; }
        public int TotalCount { get; set; } 
        public bool IsExport { get; set; } 
        public string OrderBy { get; set; }
        public string ModuleName { get; set; }
    }
}
