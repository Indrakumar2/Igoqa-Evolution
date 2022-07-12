using AutoMapper;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.ResourceSearch.Domain.Models.ResourceSearch;

namespace Evolution.Automapper.Resolver.ResourceSearch
{
    public class SerilizableObjectResolver : IMemberValueResolver<object, object, ResourceSearchParameter, string>
    {
        public SerilizableObjectResolver()
        {

        }

        public string Resolve(object source, object destination, ResourceSearchParameter sourceMember, string destMember, ResolutionContext context)
        {
            string serilizationType = string.Empty;
            if (context.Options.Items.ContainsKey("serializationType")) 
            {
                serilizationType = (context.Options.Items["serializationType"]).ToString();
                if (sourceMember != null)
                { 
                    return sourceMember.Serialize(serilizationType.ToEnum<SerializationType>());
                }
            }
           
            return string.Empty;
        }
    }
}
