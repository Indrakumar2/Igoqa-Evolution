using AutoMapper;
using Evolution.Common.Models.Base;
using System.Collections.Generic;
using System.Linq;

namespace Evolution.Common.Mappers.Resolvers
{
    public class MessageResolver<S, D> : IValueResolver<IList<S>, D, string>
    {
        private readonly string messageType;
        public MessageResolver(string type)
        {
            messageType = type;
        }

        public string Resolve(IList<S> source, D destination, string destMember, ResolutionContext context)
        { 
            return Mapper.Map<IList<BaseMessage>>(source)?.FirstOrDefault(x => GetMessageType( x) == messageType)?.MsgText;
        }

        public string GetMessageType(object o)
        {
            return o?.GetType()?.GetProperty("MsgType")?.GetValue(o, null)?.ToString();
        }
    }
}
