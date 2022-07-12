using System;

namespace Evolution.FileExtractor.Exceptions
{
    public class FileProcessingException : Exception
    {
        public string ExceptionMessage
        {
            get; set;
        }

        public string File
        {
            get; set;
        }
                
        public FileProcessingException(string file) : base()
        {
            File = file;
        }

        public FileProcessingException(string file, string exceptionMessage) : base(exceptionMessage)
        {
            File = file;
            ExceptionMessage = exceptionMessage;
        }

        public FileProcessingException(string file, string exceptionMessage, string message) : base(message)
        {
            File = file;
            ExceptionMessage = exceptionMessage;
        }

        public FileProcessingException(string file, string message, Exception innerException)
            : base(message, innerException)
        {
            File = file;
            ExceptionMessage = message;
        }

        public FileProcessingException(string file, string exceptionMessage, string message, Exception innerException)
            : base(message, innerException)
        {
            File = file;
            ExceptionMessage = exceptionMessage;
        }
    }
}
