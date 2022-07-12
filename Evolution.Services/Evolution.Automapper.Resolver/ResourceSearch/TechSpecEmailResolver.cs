using AutoMapper;
using Evolution.Common.Enums;
using System.Collections.Generic;
using System.Linq;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;

namespace Evolution.Automapper.Resolver.ResourceSearch
{
   public class TechSpecEmailResolver : IMemberValueResolver<object, object, ICollection<DbModel.TechnicalSpecialistContact>, string>
    {
        public TechSpecEmailResolver()
        {
            
        }

        public string Resolve(object source, object destination, ICollection<DbModel.TechnicalSpecialistContact> sourceMember, string destMember, ResolutionContext context)
        {  
            if (sourceMember?.Count>0)
            {
                return sourceMember.FirstOrDefault(x => x.ContactType == ContactType.PrimaryEmail.ToString())?.EmailAddress; 
            }
            return string.Empty;
        }
    }
}
