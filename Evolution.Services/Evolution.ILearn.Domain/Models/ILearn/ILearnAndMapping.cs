using Evolution.Common.Models.Base;
using System;

namespace Evolution.ILearn.Domain.Models.ILearn
{
    public class ILearnAndMapping : BaseModel
    {
        public string Training_Object_ID { get; set; }

        public string Training_Title { get; set; }

        public DateTime? Transcript_Completed_Date = null;

        public long GRM_ePin_ID { get; set; }

        public string Transcript_Score = null;

        public string Training_Hours = null;

        public string TrainingType = null;

        public string Title = null;

        public string IlearnId = null;
    }
}
