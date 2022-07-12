using Evolution.Common.Models.Messages;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Evolution.Common.Models.Responses
{
    public class Response
    {
        public Response() { }

        public Response(string code,string description)
        {   
            Code = code;
            Description=description;
        }

        public Response(string code, string description, IList<MessageDetail> messages):this(code,description)
        {
            Messages = messages;
        }

        public Response(string code, string description, object result) : this(code, description)
        {
            Result = result;
        }

        [JsonConstructor]
        public Response(string code, string description, IList<MessageDetail> messages, object result) : this(code, description,messages)
        {
            Result = result;
        }
        
        public Response(string code, string description, IList<ValidationMessage> entityMessages, object result) : this(code, description)
        {
            Result = result;
            ValidationMessages = entityMessages;
        }

        public Response(string code, string description, IList<MessageDetail> messages, IList<ValidationMessage> entityMessages, object result, Int32? recordCount = null) : this(code, description, entityMessages,result)
        {
            Messages = messages;
            RecordCount = recordCount;
        }
        
        /// <summary>
        /// Response Code
        /// </summary>
        public string Code { get; private set; }

        /// <summary>
        /// Response Description
        /// </summary>
        public string Description { get; private set; }

        /// <summary>
        /// Response Messages
        /// </summary>
        public IList<MessageDetail> Messages { get; private set; }

        public IList<ValidationMessage> ValidationMessages { get; set; }

        /// <summary>
        /// Response Data
        /// </summary>
        public object Result { get; private set; }

        public Int32? RecordCount { get; set; }

        public void Returns(Response response)
        {
            throw new NotImplementedException();
        }
    }
}
