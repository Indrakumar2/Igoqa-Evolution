using Evolution.Common.Enums;
using Evolution.Common.Models.Messages;
using Evolution.Common.Models.Responses;
using System.Collections.Generic;
using DomainModel = Evolution.ResourceSearch.Domain.Models.ResourceSearch;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;

namespace Evolution.ResourceSearch.Domain.Interfaces.ResourceSearch
{
    public interface IOverrideResourceService
    {
        Response Get(IList<int> resourceSearchIds);
        Response Save(IList<DomainModel.OverridenPreferredResource> resources, bool commitChange = true);
        Response Save(IList<DomainModel.OverridenPreferredResource> resources,
                            out IList<DbModel.OverrideResource> dbOverrideResources,
                            out IList<DbModel.TechnicalSpecialist> dbTsInfos, bool commitChange = true);
        Response Modify(IList<DomainModel.OverridenPreferredResource> resources, bool commitChange = true);
        Response Modify(IList<DomainModel.OverridenPreferredResource> resources, ref IList<DbModel.OverrideResource> dbOverrideResources, out IList<DbModel.TechnicalSpecialist> dbTsInfos, bool commitChange = true);
        bool IsRecordExistInDb(IList<int> overrideResourceId, ref IList<DbModel.OverrideResource> dbOverrideResources, ref IList<ValidationMessage> validationMessages);
    }
}
