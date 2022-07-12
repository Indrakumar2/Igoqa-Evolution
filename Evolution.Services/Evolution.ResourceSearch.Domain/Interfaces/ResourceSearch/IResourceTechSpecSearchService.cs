using Evolution.Common.Models.Responses;
using Evolution.ResourceSearch.Domain.Models.ResourceSearch;
using System.Collections.Generic;
using DomainModel = Evolution.ResourceSearch.Domain.Models.ResourceSearch;

namespace Evolution.ResourceSearch.Domain.Interfaces.ResourceSearch
{
    public interface IResourceTechSpecSearchService
    {
        Response Get(DomainModel.ResourceSearch searchModel,bool IncludeGeoLocation=false); 
        Response GetGeoLocationInfo(IList<ResourceTechSpecSearchResult> resourceResultInfo);
    }
}
