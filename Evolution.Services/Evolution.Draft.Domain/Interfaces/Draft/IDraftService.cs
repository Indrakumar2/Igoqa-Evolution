using Evolution.Common.Enums;
using Evolution.Common.Models.Responses;
using System.Collections.Generic;
using DomainModel = Evolution.Draft.Domain.Models;

namespace Evolution.Draft.Domain.Interfaces.Draft
{
    public interface IDraftService
    { 
        Response SaveDraft(DomainModel.Draft draft, bool commitChange = true);

        Response ModifyDraft(string jsontext, string drafId,bool commitChange = true);

        Response ModifyListOfDrafts(IList<DomainModel.Draft> drafts, bool commitChange = true);

        Response DeleteDraft(string drafId, bool commitChange = true);

        Response DeleteDraft(IList<string> DraftIds, bool commitChange = true);

        Response DeleteDraft(string draftId, DraftType draftType, bool commitChange = true);

        Response GetDraft(DomainModel.Draft searchModel);
        Response GetDraftMyTask(DomainModel.Draft searchModel);
       
    }
}
