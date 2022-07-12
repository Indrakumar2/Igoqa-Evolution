using Microsoft.AspNetCore.Mvc;

namespace Evolution.Document.Domain.Models.Document
{
    public class DocumentDownloadResult
    {
        public string DocumentUniqueName { get; set; }

        public string DocumentName { get; set; }

        public byte[] FileContent { get; set; }

    }
}
