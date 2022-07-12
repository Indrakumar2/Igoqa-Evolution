using Evolution.Common.Enums;
using Evolution.Common.Models.Messages;
using Evolution.Common.Models.Responses;
using System.Collections.Generic;

namespace Evolution.Master.Domain.Interfaces.Services
{
    public interface IModuleDocumentTypeService :IMasterService
    {
        Response Search(Models.ModuleDocumentType search);
        bool IsValidDocumentType(DocumentModuleType documentModuleType, IList<string> documentTypeNames, ModuleType moduleType, ref List<MessageDetail> errorMessages);
    }
}
