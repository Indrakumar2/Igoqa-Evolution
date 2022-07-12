using AutoMapper;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.ILearn.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.ILearn.Domain.Models;

namespace Evolution.ILearn.Core.Mappers
{
   public class DomainMapper : Profile
    {
        public DomainMapper()
        {
            #region Language
            CreateMap<LearnData, DbModel.IlearnData>()
                       .ForMember(dest => dest.CompletedDate, source => source.MapFrom(x => x.Transcript_Completed_Date))
                       .ForMember(dest => dest.TrainingObjectId, source => source.MapFrom(x => x.Training_Object_ID))
                       .ForMember(dest => dest.Score, source => source.MapFrom(x => x.Transcript_Score == null || x.Transcript_Score == "" ? (decimal?)null : Convert.ToDecimal(x.Transcript_Score))) //For Sarah Review Changes
                       .ForMember(dest => dest.TrainingHours, source => source.MapFrom(x => x.Training_Hours == null || x.Training_Hours =="" ? (decimal?)null : Convert.ToDecimal(x.Training_Hours))) //For Sarah Review Changes
                       .ForMember(dest => dest.TrainingTitle, source => source.MapFrom(x => x.Training_Title))
                       .ForMember(dest => dest.TechnicalSpecialistId, source => source.MapFrom(x => x.GRM_ePin_ID  == 0 ? null : x.GRM_ePin_ID)) //For Sarah Review Changes
                       .ForAllOtherMembers(x => x.Ignore());

            CreateMap<DbModel.IlearnData, LearnData>()
                       .ForMember(dest => dest.Transcript_Completed_Date, source => source.MapFrom(x => x.CompletedDate))
                       .ForMember(dest => dest.Training_Object_ID, source => source.MapFrom(x => x.TrainingObjectId))
                       .ForMember(dest => dest.Transcript_Score, source => source.MapFrom(x => x.Score))
                       .ForMember(dest => dest.Training_Hours, source => source.MapFrom(x => x.TrainingHours))
                       .ForMember(dest => dest.Training_Title, source => source.MapFrom(x => x.TrainingTitle))
                       .ForMember(dest => dest.GRM_ePin_ID, source => source.MapFrom(x => x.TechnicalSpecialistId))
                        .ForAllOtherMembers(x => x.Ignore());
            #endregion



        }
    }
}
