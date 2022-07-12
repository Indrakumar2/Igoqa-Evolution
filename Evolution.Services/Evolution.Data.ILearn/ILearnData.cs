using Evolution.Common.Models.Base;
using System;


namespace Evolution.Data.ILearn
{
    class ILearnData : BaseModel
    {
       // public long? Id { get; set; }

        public string Training_Object_ID { get; set; }

        public string Training_Title { get; set; }

        public DateTime? Transcript_Completed_Date = null;

        public long? GRM_ePin_ID { get; set; } //For Sarah Review Changes

        public string Transcript_Score = null;

        public string Training_Hours = null;

    }
}
