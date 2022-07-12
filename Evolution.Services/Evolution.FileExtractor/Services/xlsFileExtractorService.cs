using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Common.Helpers;
using Evolution.FileExtractor.Exceptions;
using Evolution.FileExtractor.Interfaces;
using Evolution.FileExtractor.Validations;
using Newtonsoft.Json.Linq;
using NPOI.HSSF.Extractor;
using NPOI.HSSF.UserModel;
using System;
using System.IO;

namespace Evolution.FileExtractor.Services
{
    public class XlsFileExtractorService : IFileExtractorService
    {
        private readonly Validation _validation = null;

        public XlsFileExtractorService(JObject messages)
        {
            _validation = new Validation(messages);
        }

        public bool CanExtractText(string filePath, out string errMessage)
        {
            errMessage = string.Empty;
            try
            {
                if (!_validation.ValidateFile(filePath, FileExtensionType.XLS, ref errMessage)) return false;

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
            HSSFWorkbook workbook = null;
            FileStream fileStream = null;
            try
            {
                if (!_validation.ValidateFile(filePath, FileExtensionType.XLS, ref errMessage))
                    throw new FileProcessingException(filePath, errMessage);

                using (fileStream = new FileStream(filePath, FileMode.Open))
                {
                    workbook = new HSSFWorkbook(new FileStream(filePath, FileMode.Open));
                }
                ExcelExtractor extractor = new ExcelExtractor(workbook);
                result = extractor.Text;
            }
            catch (Exception ex)
            {
                throw new FileProcessingException(filePath, ex.Message, ex);
            }
            finally
            {
                if (workbook != null)
                {
                    workbook.Close();
                    workbook = null;
                }

                if (fileStream != null)
                {
                    fileStream.Close();
                    fileStream.Dispose();
                    fileStream = null;
                }
            }

            return result?.RemoveEmptyRowAndControlChar();
        }

    }
}
