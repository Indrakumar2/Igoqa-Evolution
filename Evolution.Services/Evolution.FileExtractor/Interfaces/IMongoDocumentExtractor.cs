namespace Evolution.FileExtractor.Interfaces
{
    public interface IMongoDocumentExtractor
    {  
        /// <summary>
        ///     Getting text from a file
        /// </summary>
        /// <param name="filePath">Path to the file from which text to be extracted</param>
        /// <returns></returns>
        string GetContent(string filePath);

        /// <summary>
        ///  To Validate supported file extention for text extraction.
        /// </summary>
        /// <param name="filePath">Path to the file which is validated for supported file type</param>
        /// <param name="errMessage"> Validation message if file is not supported for text extraction</param>
        /// <returns> Boolean </returns>
        bool CanExtractText(string filePath, out string errMessage); 
    }
}
