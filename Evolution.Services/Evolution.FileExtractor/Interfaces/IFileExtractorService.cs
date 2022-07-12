namespace Evolution.FileExtractor.Interfaces
{
    public interface IFileExtractorService
    {
        string ExtractText(string filePath);

        bool CanExtractText(string filePath, out string errMessage);
    }
}
