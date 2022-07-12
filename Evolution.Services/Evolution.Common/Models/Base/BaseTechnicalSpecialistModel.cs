using System;

namespace Evolution.Common.Models.Base
{
    public class BaseTechnicalSpecialistModel
    {
        public Byte? UpdateCount { get; set; }

        public string RecordStatus { get; set; }

        public DateTime? LastModification { get; set; }

        public string ModifiedBy { get; set; }

        public long? EventId { get; set; }

        public string ActionByUser { get; set; }

        //  UserCompanyCode will have logged in user selected home Company Info 
        public string UserCompanyCode { get; set; }
    }
}
