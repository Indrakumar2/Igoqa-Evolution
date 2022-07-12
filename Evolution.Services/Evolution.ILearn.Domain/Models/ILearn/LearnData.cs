using Evolution.Common.Models.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Evolution.ILearn.Domain.Models
{
   public class LearnData : BaseModel
    {
        public string Training_Object_ID { get; set; }

        public string Training_Title { get; set; }

        public DateTime? Transcript_Completed_Date = null;

        public long? GRM_ePin_ID { get; set; } //For Sarah Review Changes

        public string Transcript_Score = null;

        public string Training_Hours = null;

        public bool IsILearn { get; set; }
    }
}
