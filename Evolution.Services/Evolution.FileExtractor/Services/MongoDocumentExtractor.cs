using Evolution.FileExtractor.Interfaces;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;

namespace Evolution.FileExtractor.Services
{
    public class MongoDocumentExtractor : IMongoDocumentExtractor
    {
        private readonly Dictionary<string, IFileExtractorService> _fileExtensions;
        private readonly JObject _messages = null; 

        public MongoDocumentExtractor(JObject messages) 
        { 
            _messages = messages;  
            _fileExtensions = new Dictionary<string, IFileExtractorService>
                                  {
                                      {".pdf", new PdfExtractorService(messages)},
                                      {".doc", new DocFileExtractorService(messages)},
                                      {".docx",new DocxFileExtractorService(messages)},
                                      {".txt", new TextFileExtractorService(messages)},
                                      {".msg", new EmailExtractorService(messages)},
                                      {".xls", new XlsFileExtractorService(messages)},
                                      {".xlsx", new XlsxFileExtractorService(messages)},
                                  };
        }
         
        public bool CanExtractText(string filePath, out string errMessage)
        {
            try
            {
                var extension = GetExtension(filePath);

                if (_fileExtensions.ContainsKey(extension) == false)
                    throw new NotSupportedException($"Unsupported extension :[{extension}]");

                return _fileExtensions[extension].CanExtractText(filePath, out errMessage);
            }
            catch 
            {
                throw ;
            }
        }

        public string GetContent(string filePath)
        {
            try
            {
                var extension = GetExtension(filePath);

                if (_fileExtensions.ContainsKey(extension) == false)
                    throw new NotSupportedException(extension);

                return _fileExtensions[extension].ExtractText(filePath);

            }
            catch 
            {
                throw ;
            }
        }

        private string GetExtension(string fileName)
        {
            try
            {
                return Path.GetExtension(fileName)?.ToLowerInvariant();
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"Can't define extension for file {fileName}", ex);
            }
        }

    }
}
