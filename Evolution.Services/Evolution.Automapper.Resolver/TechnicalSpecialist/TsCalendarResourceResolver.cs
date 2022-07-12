using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Evolution.TechnicalSpecialist.Domain.Models.TechSpecialist;
using System;

namespace Evolution.Automapper.Resolver.TechnicalSpecialist
{
    public class TsCalendarResourceResolver : IMemberValueResolver<object, object, IList<TechnicalSpecialistCalendar>, IList<TechnicalSpecialistCalendarResourceView>>
    {
        public TsCalendarResourceResolver()
        {

        }

        public IList<TechnicalSpecialistCalendarResourceView> Resolve(object source, object destination, IList<TechnicalSpecialistCalendar> sourceMember, IList<TechnicalSpecialistCalendarResourceView> destMember, ResolutionContext context)
        {
            var tsInfo = new List<BaseTechnicalSpecialistInfo>();
            if (context.Options.Items.ContainsKey("tsInfo"))
                tsInfo = ((List<BaseTechnicalSpecialistInfo>)context.Options.Items["tsInfo"]);
            if (tsInfo?.Count > 0)
            {
                return tsInfo
                .Select(x => new TechnicalSpecialistCalendarResourceView()
                {
                    Id = Convert.ToInt32(x.Id),
                    Name = string.Format("{0} {1}", x.LastName, x.FirstName).Trim()
                }).ToList();
            }
            return null;
        }
    }
}