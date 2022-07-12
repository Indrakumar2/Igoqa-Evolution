using AutoMapper;
using System.Collections.Generic;
using System.Linq;

namespace Evolution.Common.Mappers.Resolvers
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="S">Source</typeparam>
    /// <typeparam name="D">Destination Model Type</typeparam>
    public class MessagesResolver<S, D,R> : IValueResolver<IList<S>, D, IList<R>>
    {
        private readonly string messageType;

        public MessagesResolver(string type)
        {
            messageType = type;
        }

        public IList<R> Resolve(IList<S> source, D destination, IList<R> destMember, ResolutionContext context)
        {
            return Mapper.Map<IList<R>>(source)?.Where(x => GetMessageType(x) == messageType).ToList();
        }

        public string GetMessageType(object o)
        {
            return o?.GetType()?.GetProperty("MsgType")?.GetValue(o, null)?.ToString();
        }
    }
}