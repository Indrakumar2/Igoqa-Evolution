using AutoMapper;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.TechnicalSpecialist.Domain.Models.TechSpecialist;
using Newtonsoft.Json;
using System;

namespace Evolution.Automapper.Resolver.TechnicalSpecialist
{
    public class MyTaskResolver : IMemberValueResolver<object, object, Evolution.Draft.Domain.Models.Draft, string>
    {
        //D-670 Starts
        public string Resolve(object source, object destination, Evolution.Draft.Domain.Models.Draft sourceMember, string destMember, ResolutionContext context)
        {
            if (sourceMember != null && !string.IsNullOrEmpty(sourceMember.SerilizableObject))
            {
                var result = JsonConvert.DeserializeObject<TechnicalSpecialistDetail>(sourceMember.SerilizableObject);
                return string.Format("{0} for {1}, {2} ({3})", ((DraftType)Enum.Parse(typeof(DraftType), sourceMember.DraftType)).DisplayName(), result.TechnicalSpecialistInfo.LastName, result.TechnicalSpecialistInfo.FirstName, sourceMember.DraftId);
            }
            return string.Empty;
        }
        //D-670 End
    }
}
