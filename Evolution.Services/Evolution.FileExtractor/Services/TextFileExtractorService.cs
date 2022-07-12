using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.FileExtractor.Exceptions;
using Evolution.FileExtractor.Interfaces;
using Evolution.FileExtractor.Validations;
using Newtonsoft.Json.Linq;
using System;
using System.IO;

namespace Evolution.FileExtractor.Services
{
    public class TextFileExtractorService : IFileExtractorService
    {
        private readonly Validation _validation = null;

        public TextFileExtractorService(JObject messages)
        {
            _validation = new Validation(messages);
        }

        public bool CanExtractText(string filePath, out string errMessage)
        {
            errMessage = string.Empty;
            bool result = false;
            try
            {
                result = _validation.ValidateFile(filePath, FileExtensionType.TXT, ref errMessage);
            }
            catch (Exception ex)
            {
                result = false;
                throw ex;
            }

            return result;
        }

        public string ExtractText(string filePath)
        {
            string errMessage = string.Empty;
            try
            {
                if (!_validation.ValidateFile(filePath, FileExtensionType.TXT, ref errMessage))
                    throw new FileProcessingException(filePath, errMessage);

                return File.ReadAllText(filePath)?.RemoveEmptyRowAndControlChar();
            }
            catch (Exception ex)
            {
                throw new FileProcessingException(filePath, ex.Message, ex);
            }
        }
    }
}
