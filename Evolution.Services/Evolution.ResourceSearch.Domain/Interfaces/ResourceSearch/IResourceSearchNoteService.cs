using Evolution.Common.Models.Responses;
using System.Collections.Generic;
using DomainModel = Evolution.ResourceSearch.Domain.Models.ResourceSearch;

namespace Evolution.ResourceSearch.Domain.Interfaces.ResourceSearch
{
    public interface IResourceSearchNoteService
    {
        Response Save(IList<DomainModel.ResourceSearchNote>  resourceSearchNotes, bool commitchange = true);

        Response Get(DomainModel.ResourceSearchNote searchModel);

        Response Get(IList<int> ResourceSearchIds, bool fetchLatest = false);

    }
}
