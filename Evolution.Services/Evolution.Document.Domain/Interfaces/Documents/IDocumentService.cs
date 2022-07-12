using Evolution.Common.Enums;
using Evolution.Common.Models.Responses;
using Evolution.Document.Domain.Models.Document;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using DBModel = Evolution.DbRepository.Models.SqlDatabaseContext;

namespace Evolution.Document.Domain.Interfaces.Documents
{
    public interface IDocumentService
    {
        Response Save(IList<ModuleDocument> documents, bool commitChange = true, bool isResultSetRequired = false);

        Response Modify(IList<ModuleDocument> documents, bool commitChange = true, bool isResultSetRequired = false);

        //Audit
        Response Save(IList<ModuleDocument> documents, ref List<DBModel.Document> dbDocuments, bool commitChange = true, bool isResultSetRequired = false);

        Response Modify(IList<ModuleDocument> documents, ref List<DBModel.Document> dbDocuments, bool commitChange = true, bool isResultSetRequired = false);

        Response GetOrphandDocuments(int returnRowCount = 0);

        Response Get(IList<string> documentUniqueNames);

        Response Get(ModuleDocument model);

        Response Get(ModuleCodeType moduleCodeType, IList<string> moduleRefCodes=null, IList<string> subModuleRefCodes=null);

        Response Get(IList<ModuleDocument> model);

        DocumentDownloadResult Get(string documentUniqueName);

        IList<DocumentDownloadResult> GetEmailAttachmentDocuments(IList<string> documentUniqueNames);

        Response Delete(IList<ModuleDocument> documents, bool commitChange = true, bool isResultSetRequired = false);

        Response DeletOrphandDocuments();

        void DeletOrphandDocumentFiles(List<string> documentUniqueNames);

        void DeletOrphandDocuments(List<string> documentUniqueNames);

        Response ChangeDocumentStatus(IList<ModuleDocument> documents);

        Response ChangeDocumentStatus(ModuleDocument document);

        Response GenerateDocumentUniqueName(IList<DocumentUniqueNameDetail> model);

        Response Upload(IFormFileCollection files, string code);
        
        Response UploadByStream(HttpRequest httpRequest, ModuleCodeType moduleCodeType, string referenceCode, string documentUniqueName);

        Task<Response> UploadByStreamAsync(HttpRequest httpRequest, ModuleCodeType moduleCodeType, string referenceCode, string documentUniqueName);        

        DocumentDownloadResult Download(ModuleCodeType moduleCodeType, string documentUniqueName);

        //Response Copy(ModuleCodeType moduleCodeType, string referenceCode, string documentUniqueName);

        Response Paste(ModuleCodeType moduleCodeType, string code, string copyDocumentUniqueName,string logInUsername = "");

        Response Get(string moduleCode, string moduleRefCode);

        Response UploadEmailDocument(DocumentUniqueNameDetail model, string emailSubject, bool IsVisibleToCustomer, bool IsVisibleToTS);
        
        Response UploadEmailDocuments(EmailDocument emailDocument);

        Response UploadEmailDocuments(EmailDocumentUpload emailDocument);
        string CheckAndUpdateDocumentUploadPath();
    }
}