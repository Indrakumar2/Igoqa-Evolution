using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Newtonsoft.Json.Linq;
using System.IO;

namespace Evolution.FileExtractor.Validations
{
    public class Validation
    {
        private readonly JObject _messages = null;

        public Validation(JObject messages)
        {
            _messages = messages;
        }

        public bool ValidateFile(string filePath, FileExtensionType type, ref string errMessage)
        {
            errMessage = string.Empty;

            if (File.Exists(filePath))
            {
                if (!Path.GetExtension(filePath).ToLower().Contains("." + type.ToString().ToLower()))
                    errMessage = string.Format(_messages[MessageType.InvalidFileForProcess.ToId()].ToString(), filePath);
            }
            else
                errMessage = string.Format(_messages[MessageType.FileDoesNotExist.ToId()].ToString(), filePath);

            return string.IsNullOrEmpty(errMessage);
        }
    }
}
