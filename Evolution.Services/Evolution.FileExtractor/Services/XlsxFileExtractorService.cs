using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Common.Helpers;
using Evolution.FileExtractor.Exceptions;
using Evolution.FileExtractor.Interfaces;
using Evolution.FileExtractor.Validations;
using Newtonsoft.Json.Linq;
//using Syncfusion.XlsIO;
//using Syncfusion.XlsIO.Implementation.Shapes;
using System;
using System.IO;
using TextExtractor;

namespace Evolution.FileExtractor.Services
{
    public class XlsxFileExtractorService : IFileExtractorService
    {
        private readonly Validation _validation = null;

        public XlsxFileExtractorService(JObject messages)
        {
            _validation = new Validation(messages);
        }

        public bool CanExtractText(string filePath, out string errMessage)
        { 
            errMessage = string.Empty;
            try
            {
                if (!_validation.ValidateFile(filePath, FileExtensionType.XLSX, ref errMessage)) return false;

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
            string result = string.Empty;
            string errMessage = string.Empty;
            try
            {
                if (!_validation.ValidateFile(filePath, FileExtensionType.XLSX, ref errMessage))
                    throw new FileProcessingException(filePath, errMessage);

                var documentExtractor = DocumentExtractor.Default();
                result = documentExtractor.GetContent(new RawDocument(filePath, File.ReadAllBytes(filePath)));
            }
            catch (Exception ex)
            {
                throw new FileProcessingException(filePath, ex.Message, ex);
            }

            return result?.RemoveEmptyRowAndControlChar(); 
        }

        //public string ExtractText(string filePath)
        //{
        //    string result = string.Empty;
        //    string errMessage = string.Empty;
        //    try
        //    {
        //        if (!_validation.ValidateFile(filePath, FileExtensionType.XLS, ref errMessage) &&
        //            !_validation.ValidateFile(filePath, FileExtensionType.XLSX, ref errMessage))
        //            throw new FileProcessingException(filePath, errMessage);

        //        result = ExtractExcelContentAsText(filePath);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new FileProcessingException(filePath, ex.Message, ex);
        //    }

        //    return result;
        //}

        //private string ExtractExcelContentAsText(string filePath)
        //{
        //    ExcelEngine excelEngine = null;
        //    IApplication application = null;
        //    IWorkbook workbook = null;
        //    FileStream fileStream = null;
        //    StringBuilder textCollection = null;
        //    try
        //    {
        //        excelEngine = new ExcelEngine();
        //        application = excelEngine.Excel;

        //        textCollection = new StringBuilder();
        //        //application.DefaultVersion = ExcelVersion.Excel2013;

        //        fileStream = new FileStream(filePath, FileMode.Open);
        //        workbook = application.Workbooks.Open(fileStream);
        //        foreach (IWorksheet worksheet in workbook.Worksheets)
        //        {
        //            ExtractTextFormWorkSheet(worksheet, ref textCollection);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {
        //        workbook?.Close();
        //        excelEngine?.Dispose();
        //        fileStream?.Close();
        //    }
        //    return textCollection?.ToString()?.RemoveEmptyRow();
        //}

        //private void ExtractTextFormWorkSheet(IWorksheet worksheet, ref StringBuilder textCollection)
        //{
        //    for (int i = 1; i <= worksheet?.UsedRange.LastRow; i++)
        //    {
        //        for (int j = 1; j <= worksheet.UsedRange.LastColumn; j++)
        //        {
        //            IRange range = worksheet.Range[i, j];
        //            if (!string.IsNullOrEmpty(range.DisplayText))
        //            {
        //                textCollection.Append(range.DisplayText + "\t");
        //            }
        //        }
        //        textCollection.AppendLine("");
        //    }

        //    foreach (IShape shape in worksheet?.Shapes)
        //    {
        //        if (shape is AutoShapeImpl)
        //            textCollection.AppendLine((shape as ShapeImpl).TextFrame.TextRange.Text);
        //    }

        //    foreach (IComment comment in worksheet?.Comments)
        //    {
        //        if (comment != null)
        //            textCollection.AppendLine(comment.Text);
        //    }
        //}
    }
}
