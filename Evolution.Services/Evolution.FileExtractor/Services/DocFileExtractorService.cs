using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Common.Helpers;
using Evolution.FileExtractor.Exceptions;
using Evolution.FileExtractor.Interfaces;
using Evolution.FileExtractor.Validations;
using Newtonsoft.Json.Linq;
//using Syncfusion.DocIO;
//using Syncfusion.DocIO.DLS;
using System;
using System.IO;
using TextExtractor;

namespace Evolution.FileExtractor.Services
{
    public class DocFileExtractorService : IFileExtractorService
    {
        private readonly Validation _validation = null;

        public DocFileExtractorService(JObject messages)
        {
            _validation = new Validation(messages);
        }

        public bool CanExtractText(string filePath, out string errMessage)
        {
            errMessage = string.Empty; 
            try
            {
                if (!_validation.ValidateFile(filePath, FileExtensionType.DOC, ref errMessage)) return false;

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
                if (!_validation.ValidateFile(filePath, FileExtensionType.DOC, ref errMessage))
                    throw new FileProcessingException(filePath, errMessage);
                 
                result = ReadDocFile(filePath); 
            }
            catch (Exception ex)
            {
                throw new FileProcessingException(filePath, ex.Message, ex);
            }

            return result;
        }

        private string ReadDocFile(string filePath)
        {
            IDocumentExtractor documentExtractor = DocumentExtractor.Default();
            return documentExtractor.GetContent(new RawDocument(filePath, File.ReadAllBytes(filePath)))?.RemoveEmptyRowAndControlChar(); 
        }

        //public string ExtractText(string filePath)
        //{
        //    string errMessage = string.Empty;
        //    FileStream fileStream = null;
        //    WordDocument wordDocument = null;
        //    string result = string.Empty;
        //    try
        //    {
        //        if (!_validation.ValidateFile(filePath, FileExtensionType.DOCX, ref errMessage) &&
        //                !_validation.ValidateFile(filePath, FileExtensionType.DOC, ref errMessage))
        //            throw new FileProcessingException(filePath, errMessage);

        //        fileStream = new FileStream(filePath, FileMode.Open);
        //        wordDocument = new WordDocument(fileStream, FormatType.Docx);

        //        //Itearates into each section and get the text from the Word document.
        //        foreach (WSection sec in wordDocument.Sections)
        //        {
        //            IterateTextBody(sec.Body, ref result);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new FileProcessingException(filePath, ex.Message, ex);
        //    }
        //    finally
        //    {
        //        wordDocument?.Close();
        //        fileStream?.Close();
        //    }

        //    return result?.RemoveEmptyRow();
        //}

        //private static void IterateTextBody(WTextBody textBody, ref string wordContent)
        //{
        //    if (textBody != null)
        //    {
        //        foreach (IEntity entity in textBody.ChildEntities)
        //        {
        //            if (entity != null)
        //            {
        //                switch (entity.EntityType)
        //                {
        //                    case EntityType.Paragraph:
        //                        WParagraph paragraph = entity as WParagraph;
        //                        IterateParagraph(paragraph.Items, ref wordContent);
        //                        break;
        //                    case EntityType.Table:
        //                        IterateTable(entity as WTable, ref wordContent);
        //                        break;
        //                    case EntityType.BlockContentControl:
        //                        BlockContentControl blockContentControl = entity as BlockContentControl;
        //                        IterateTextBody(blockContentControl?.TextBody, ref wordContent);
        //                        break;
        //                }
        //            }
        //        }
        //    }
        //}

        //private static void IterateParagraph(ParagraphItemCollection paraItems, ref string wordContent)
        //{
        //    bool isFieldEnd = true;
        //    foreach (var item in paraItems)
        //    {
        //        if (item is WField)
        //        {
        //            wordContent += (item as WField).Text;
        //            isFieldEnd = false;
        //        }
        //        else if (item is WTextRange && isFieldEnd)
        //            wordContent += (item as WTextRange).Text;
        //        else if (item is WFieldMark && (item as WFieldMark).Type == FieldMarkType.FieldEnd)
        //            isFieldEnd = true;
        //        else if (item is WTextBox)
        //        {
        //            WTextBox textBox = item as WTextBox;
        //            IterateTextBody(textBox.TextBoxBody, ref wordContent);
        //        }
        //        else if (item is Shape)
        //        {
        //            Shape shape = item as Shape;
        //            IterateTextBody(shape.TextBody, ref wordContent);
        //        }
        //        else if (item is InlineContentControl)
        //        {
        //            InlineContentControl inlineContentControl = item as InlineContentControl;
        //            IterateParagraph(inlineContentControl.ParagraphItems, ref wordContent);
        //        }
        //    }
        //    wordContent += Environment.NewLine;
        //}

        //private static void IterateTable(WTable table, ref string wordContent)
        //{
        //    foreach (WTableRow row in table.Rows)
        //    {
        //        foreach (WTableCell cell in row.Cells)
        //        {
        //            IterateTextBody(cell, ref wordContent);
        //        }
        //    }
        //}
    }
}
