using Evolution.Common.Enums;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Evolution.Common.Models.Messages
{
    public class BaseMessage
    {
        public BaseMessage(string code, string message)
        {
            Code = code;
            Message = message;
        }
        
        public string Code { get; private set; }
        
        public string Message { get; private set; }
    }
    
    public class MessageDetail : BaseMessage
    {
        public MessageDetail(string code, string message) : base(code, message)
        {
            
        }
        [JsonConstructor]
        public MessageDetail(ModuleType type, string code, string message) : base(code,message)
        {
            Type = type;
        }

        //TODO : NEed to Remove once ModuleType has been implemented across the projects
        public MessageDetail(MessageType type, string code, string message) : base(code, message)
        {
        
        }
        
        public ModuleType Type { get; private set; }
    }

    public class ValidationMessage
    {
        public ValidationMessage(object entityValue, IList<MessageDetail> messages)
        {
            this.EntityValue = entityValue;
            Messages = messages;
        }

        public ValidationMessage(object entityValue)
        {
            this.EntityValue = entityValue;
        }

        public object EntityValue { get; set; }

        public IList<MessageDetail> Messages { get; set; }
    }
}
