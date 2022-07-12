using Evolution.Common.Enums;
using Evolution.Common.Models.Messages;
using Evolution.Common.Models.Validations;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;

namespace Evolution.Common.Extensions
{
    public static class ValidationMessageExtension
    {
        public static void Add(this IList<ValidationMessage> source, JObject messages, object entity, MessageType type)
        {
            var errorCode = type.ToId();
            var messageDetail = new MessageDetail(errorCode, messages[errorCode].ToString());
            source.Add(new ValidationMessage(entity, new List<MessageDetail>() { messageDetail }));
        }

        public static void Add(this IList<ValidationMessage> source, JObject messages, object entity, MessageType type,params object[] param)
        {
            var errorCode = type.ToId();
            var messageDetail = new MessageDetail(errorCode, string.Format(messages[errorCode].ToString(), param));
            source.Add(new ValidationMessage(entity, new List<MessageDetail>() { messageDetail }));
        }

        public static void Add(this IList<ValidationMessage> source, JObject messages, ModuleType moduleType, IList<JsonPayloadValidationResult> validationResults)
        {
            validationResults?.ToList().ForEach(x =>
            {
                source?.Add(new ValidationMessage(x.Code, new List<MessageDetail>
                {
                    new MessageDetail(moduleType, x.Code, x.Message)
                }));
            });
        }
    }
}