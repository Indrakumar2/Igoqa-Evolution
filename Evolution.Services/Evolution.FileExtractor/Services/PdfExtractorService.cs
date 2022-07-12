using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.FileExtractor.Exceptions;
using Evolution.FileExtractor.Interfaces;
using Evolution.FileExtractor.Validations;
using Newtonsoft.Json.Linq;
//using Syncfusion.Pdf;
//using Syncfusion.Pdf.Parsing;
using System;
using System.Linq;

namespace Evolution.FileExtractor.Services
{
    public class PdfExtractorService : IFileExtractorService
    {
        private readonly Validation _validation = null;

        public PdfExtractorService(JObject messages)
        {
            _validation = new Validation(messages);
        }

        public bool CanExtractText(string filePath, out string errMessage)
        {
            errMessage = string.Empty;
            try
            {
                if (!_validation.ValidateFile(filePath, FileExtensionType.PDF, ref errMessage)) return false;

                if (IsPasswordProtected(filePath))
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
            try
            {
                if (!_validation.ValidateFile(filePath, FileExtensionType.PDF, ref errMessage))
                    throw new FileProcessingException(filePath, errMessage);

                return ReadPdfFile(filePath);
            }
            catch (Exception ex)
            {
                throw new FileProcessingException(filePath, ex.Message, ex);
            }
        }

        private string ReadPdfFile(string filePath)
        {
            // Reference URL : https://github.com/UglyToad/PdfPig 
            using (UglyToad.PdfPig.PdfDocument document = UglyToad.PdfPig.PdfDocument.Open(filePath))
            {
                return string.Join(" ", document.GetPages()?.Select(x => x.Text))?.RemoveEmptyRowAndControlChar();
            } 
        }

        private bool IsPasswordProtected(string filePath)
        {
            try
            {
                using (UglyToad.PdfPig.PdfDocument document = UglyToad.PdfPig.PdfDocument.Open(filePath)) { } ;
            }
            catch (UglyToad.PdfPig.Exceptions.PdfDocumentEncryptedException)
            {
                return true;
            }
            return false;
        }

        //private string ReadPdfFile(string filePath)
        //{
        //    StringBuilder resultBuilder = null;
        //    FileStream docStream = null;
        //    PdfLoadedDocument loadedDocument = null;

        //    try
        //    {
        //        resultBuilder = new StringBuilder();
        //        docStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);

        //        //Loading the PDF document from stream.
        //        loadedDocument = new PdfLoadedDocument(docStream);

        //        // Loading page collections
        //        PdfLoadedPageCollection loadedPages = loadedDocument.Pages;

        //        // Extracting text from existing PDF document pages
        //        foreach (PdfLoadedPage loadedPage in loadedPages)
        //        {
        //            resultBuilder.Append(loadedPage.ExtractText(true));
        //        }
        //    }
        //    finally
        //    {
        //        //Closing the document instance.
        //        if (loadedDocument != null)
        //            loadedDocument.Close(true);

        //        if (docStream != null)
        //            docStream.Close();
        //    }

        //    return resultBuilder.ToString().RemoveEmptyRow();
        //}
    }
}