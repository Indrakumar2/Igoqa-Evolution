using Evolution.GenericDbRepository.Interfaces;
using System.Collections.Generic;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.ResourceSearch.Domain.Models.ResourceSearch;

namespace Evolution.ResourceSearch.Domain.Interfaces.Data
{
    public interface IResourceSearchRepository : IGenericRepository<DbModel.ResourceSearch>
    {
        IList<DomainModel.ResourceSearchResult> Search(DomainModel.BaseResourceSearch searchModel);
        IList<DomainModel.ResourceSearchResult> Search(IList<KeyValuePair<string, IList<string>>> mySearch, string assignedTo, string companyCode, bool isAllCoordinator);
        IList<DomainModel.ResourceSearchTechSpecInfo> SearchTechSpech(DomainModel.ResourceSearch searchModel);
        IList<DomainModel.ResourceSearchTechSpecInfo> GetExceptionTSList(DomainModel.ResourceSearch searchModel);
        List<DomainModel.AssignedResourceInfo> GetAssignmentTechSpec(int assignmentId, int supplierPoId);

        List<DbModel.Supplier> GetSupplier(List<int> supplierId); //693- Subsupplier Postal code is not getting passed because of MS-TS implementation
    }
}
