using Evolution.Common.Enums;
using Evolution.Common.Models.Messages;
using Evolution.Common.Models.Responses;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Evolution.Common.Extensions
{
    public static class ResponseExtension
    {
        private readonly static JObject _messages = null;

        static ResponseExtension()
        {
            _messages = JObject.Parse(File.ReadAllText(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"Message_EN.json")));
        }

        public static Response ToPopulate(this Response source,ResponseType responseType, IList<KeyValuePair<MessageType, ModuleType>> messageCodes,List<MessageDetail> messages,List<ValidationMessage> valdMessages,object result, Exception ex, Int32? recordCount =null)
        {
            if (messages == null)
                messages = new List<MessageDetail>();

            if (valdMessages == null)
                valdMessages = new List<ValidationMessage>();

            if (ex != null)
            {
                responseType = ResponseType.Error;
                if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
                { 
                    messages.Add(new MessageDetail(ResponseType.Exception.ToId(), ex.ToFullString()));
                }
                else
                {
                    messages.Add(new MessageDetail(ResponseType.Exception.ToId(), _messages[MessageType.Exception.ToId()].ToString()));
                }

            }

            return ResponseExtension.PopulateResponse(responseType, messageCodes, messages, valdMessages, result,recordCount);
        }

        public static Response ToPopulate(this Response source, ResponseType type, Int32? recordCount = 0)
        {
            return ResponseExtension.PopulateResponse(type, null, null, null, null, recordCount);
        }

        public static Response ToPopulate(this Response source, ResponseType type, object result, Int32? recordCount = 0)
        {
            return ResponseExtension.PopulateResponse(type, null, null, null, result, recordCount);
        }

        public static Response ToPopulate(this Response source, ResponseType type, IList<MessageDetail> messages, object result, Int32? recordCount = null)
        {
            return ResponseExtension.PopulateResponse(type, null, messages, null, result, recordCount);
        }

        public static Response ToPopulate(this Response source, ResponseType type, IList<ValidationMessage> validationMessages, object result, Int32? recordCount = null)
        {
            return ResponseExtension.PopulateResponse(type, null, null, validationMessages, result, recordCount);
        }

        public static Response ToPopulate(this Response source, ResponseType type, IList<ValidationMessage> validationMessage, IList<MessageDetail> messages, object result, Int32? recordCount = null)
        {
            return ResponseExtension.PopulateResponse(type, null, messages, validationMessage, result, recordCount);
        }

        public static Response ToPopulate(this Response source, ResponseType type, IList<KeyValuePair<MessageType, ModuleType>> messageCodes, object result, Int32? recordCount = null)
        {
            return ResponseExtension.PopulateResponse(type, messageCodes, null, null, result, recordCount);
        }

        public static Response ToPopulate(this Response source, ResponseType type, IList<KeyValuePair<MessageType, ModuleType>> messageCodes, IList<MessageDetail> messages, object result, Int32? recordCount = null)
        {
            return ResponseExtension.PopulateResponse(type, messageCodes, messages, null, result, recordCount);
        }

        private static Response PopulateResponse(ResponseType type, IList<KeyValuePair<MessageType, ModuleType>> messageKeys, IList<MessageDetail> messages, IList<ValidationMessage> entityMessages, object result, Int32? recordCount = 0)
        {
            var messagesUnion = new List<MessageDetail>();

            if (messageKeys?.Count>0)
                messagesUnion.AddRange(messageKeys.Select(x => new MessageDetail(x.Value, x.Key.ToId(), _messages[x.Key.ToId()].ToString())).ToList());

            if (messages != null && messages.Count > 0)
                messagesUnion.AddRange(messages);

            if(messagesUnion.Count>0)
                type = ResponseType.Error;

            if(entityMessages?.Count>0)
                type = ResponseType.Validation;

            return new Response(type.ToId(), _messages[type.ToId()].ToString(), messagesUnion, entityMessages, result, recordCount);
        }
    }
}
