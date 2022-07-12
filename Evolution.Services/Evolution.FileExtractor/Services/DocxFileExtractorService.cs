using DocumentFormat.OpenXml.Packaging;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Common.Helpers;
using Evolution.FileExtractor.Exceptions;
using Evolution.FileExtractor.Interfaces;
using Evolution.FileExtractor.Validations;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Text;
using System.Xml;

namespace Evolution.FileExtractor.Services
{
    public class DocxFileExtractorService : IFileExtractorService
    {
        private readonly Validation _validation = null;

        public DocxFileExtractorService(JObject messages)
        {
            _validation = new Validation(messages);
        }

        public bool CanExtractText(string filePath, out string errMessage)
        {
            errMessage = string.Empty;
            try
            {
                if (!_validation.ValidateFile(filePath, FileExtensionType.DOCX, ref errMessage)) return false;

                if (MsOfficeHelper.IsPasswordProtected(filePath))
                { 
                    throw new NotSupportedException("Failed to process file, As file is password protected");
                }
            }
            catch (Exception)
            {
                throw;
            }
            return true;
        }

        public string ExtractText(string filePath)
        {
            string errMessage = string.Empty;
            string result = string.Empty;
            try
            {
                if (!_validation.ValidateFile(filePath, FileExtensionType.DOCX, ref errMessage))
                    throw new FileProcessingException(filePath, errMessage);

                result = ReadTextFromDocx(filePath);//def 899 fix: for “.docx”  file type it is not returning “next line character” or “Return carriage” character  properly , so it is considering last word from first line and first word from second line as single word. 

            }
            catch (Exception ex)
            {
                throw new FileProcessingException(filePath, ex.Message, ex);
            }

            return result?.RemoveEmptyRowAndControlChar();
        }

        public static string ReadTextFromDocx(string filePath)
        {
            StringBuilder stringBuilder;
            try
            {
                using (WordprocessingDocument wordprocessingDocument = WordprocessingDocument.Open(filePath, false))
                {
                    NameTable nameTable = new NameTable();
                    XmlNamespaceManager xmlNamespaceManager = new XmlNamespaceManager(nameTable);
                    xmlNamespaceManager.AddNamespace("w", "http://schemas.openxmlformats.org/wordprocessingml/2006/main");

                    string wordprocessingDocumentText;
                    using (StreamReader streamReader = new StreamReader(wordprocessingDocument.MainDocumentPart.GetStream()))
                    {
                        wordprocessingDocumentText = streamReader.ReadToEnd();
                    } 
                    stringBuilder = new StringBuilder(wordprocessingDocumentText.Length);

                    XmlDocument xmlDocument = new XmlDocument(nameTable);
                    xmlDocument.LoadXml(wordprocessingDocumentText);

                    XmlNodeList paragraphNodes = xmlDocument.SelectNodes("//w:p", xmlNamespaceManager);
                    foreach (XmlNode paragraphNode in paragraphNodes)
                    {
                        XmlNodeList textNodes = paragraphNode.SelectNodes(".//w:t | .//w:tab | .//w:br", xmlNamespaceManager);
                        foreach (XmlNode textNode in textNodes)
                        {
                            switch (textNode.Name)
                            {
                                case "w:t":
                                    stringBuilder.Append(textNode.InnerText);
                                    break;

                                case "w:tab":
                                    stringBuilder.Append("\t");
                                    break;

                                case "w:br":
                                    stringBuilder.Append("\v");
                                    break;
                            }
                        } 
                        stringBuilder.Append(Environment.NewLine);
                    }
                } 
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return stringBuilder.ToString();
        }
    }
}
