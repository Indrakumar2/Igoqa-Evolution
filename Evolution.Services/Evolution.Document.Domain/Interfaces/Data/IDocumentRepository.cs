using Evolution.GenericDbRepository.Interfaces;
using Evolution.Document.Domain.Models.Document;
using System.Collections.Generic;
using Evolution.Common.Enums;
using Evolution.DbRepository.Models.SqlDatabaseContext;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using System.Linq;
using System;

namespace Evolution.Document.Domain.Interfaces.Data
{
    public interface IDocumentRepository : IGenericRepository<Evolution.DbRepository.Models.SqlDatabaseContext.Document>
    {
        IList<ModuleDocument> Get(ModuleDocument model);

        IList<ModuleDocument> Get(ModuleCodeType moduleCodeType, IList<string> moduleRefCodes = null, IList<string> subModuleRefCodes = null);

        IList<ModuleDocument> Get(IList<string> documentUniqueNames);

        string[] GetCustomerName(string moduleCode, string moduleRefCode);

        DocumentUploadPath GetDocumentUploadPath();

        DocumentUploadPath UpdateDocumentUploadFull(int id);

        string GetDocumentPathByName(string documentUniqueName);

        IList<ModuleDocument> FilterDocumentsByCompany(ModuleDocument model);

        IList<ModuleDocument> GetDocumentsToBeProcessed(bool isFetchNewDoc);

        ModuleDocument GetDocumentsToBeProcessed(string documentTitle, string modifiedBy);

        IList<DbModel.Document> GetOrphandRecords();
    }
}