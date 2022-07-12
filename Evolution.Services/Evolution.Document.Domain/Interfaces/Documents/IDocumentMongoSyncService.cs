using Evolution.Common.Enums;
using Evolution.Common.Models.Responses;
using Evolution.Document.Domain.Models.Document;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using System;
using System.Collections.Generic;

namespace Evolution.Document.Domain.Interfaces.Documents
{
    public interface IDocumentMongoSyncService
    {
        Response DeleteOrphandDocuments(IList<string> uniqueNames);

        ModuleDocument FetchTop1DocumentToBeProcessed(bool isFetchNewDoc);

        ModuleDocument FetchTop1DocumentToBeProcessed(string documentTitle, string modifiedBy);

        bool IsDocumentValidForSync(string documentUniqueName, out string message);

        void ProcessDocument(ModuleDocument document, string fileText);

        void ProcessDocumentMigratedDocuments(ModuleDocument document, string fileText);

        void SaveSyncDocumentAuditData(string documentUniqueName, MongoSyncStatusType status, bool isCompleted = true, string reason = null);

        void SaveSyncDocumentAuditDataUpdateDocumentStatus(string documentUniqueName, MongoSyncStatusType status, bool isCompleted = true, string reason = null);

        string ExtractOnlyTextFromFile(string filePath);
    }
}