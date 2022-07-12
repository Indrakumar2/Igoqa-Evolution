using Evolution.Common.Models.Responses;
using System.Collections.Generic;
using DomainModel = Evolution.ResourceSearch.Domain.Models.ResourceSearch;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using Evolution.Common.Models.Messages;

namespace Evolution.ResourceSearch.Domain.Interfaces.ResourceSearch
{
    public interface IResourceSearchService
    {
        Response Get(DomainModel.BaseResourceSearch searchModel);
        Response Get(IList<KeyValuePair<string, IList<string>>> mySearch, string assignedToUser, string companyCode, bool isAllCoordinator);
        Response GetARSSearchAssignmentDetail(int assignmentId);
        Response Save(DomainModel.ResourceSearch resource, bool commitChange = true);

        Response Save(DomainModel.ResourceSearch resource, ref IList<DbModel.ResourceSearch> dbResourceSearch, bool commitChange = true);

        Response Save(DomainModel.ResourceSearch resource,
           ref IList<DbModel.ResourceSearch> dbResourceSearch,
           ref IList<DbModel.Customer> dbCustomers,
           ref IList<DbModel.Company> dbCompanies,
           ref IList<DbModel.User> dbCoordinators,
           ref IList<DbModel.Data> dbCategory,
           ref IList<DbModel.TaxonomySubCategory> dbSubCategory,
           ref IList<DbModel.TaxonomyService> dbService,
           ref IList<DbModel.OverrideResource> dbOverrideResources,
           ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialist,
           ref DbModel.Assignment dbAssignment,
           bool isDBValidationRequire = true,
           bool commitChange = true,
           IList<DbModel.SqlauditModule> dbModule = null);

        Response Modify(DomainModel.ResourceSearch resource, bool commitChange = true);

        Response Modify(DomainModel.ResourceSearch resource, ref IList<DbModel.ResourceSearch> dbResourceSearch,bool commitChange = true);

        Response Modify(DomainModel.ResourceSearch resource,
           ref IList<DbModel.ResourceSearch> dbResourceSearch,
           ref IList<DbModel.Customer> dbCustomers,
           ref IList<DbModel.Company> dbCompanies,
           ref IList<DbModel.User> dbCoordinators,
           ref IList<DbModel.Data> dbCategory,
           ref IList<DbModel.TaxonomySubCategory> dbSubCategory,
           ref IList<DbModel.TaxonomyService> dbService,
           ref IList<DbModel.OverrideResource> dbOverrideResources,
           ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialist,
           ref DbModel.Assignment dbAssignment,
           bool isDBValidationRequire = true,
           bool commitChange = true,
           IList<DbModel.SqlauditModule> dbModule = null);

        Response Delete(int resourceId, bool commitChange = true);

        Response ChangeStatusOfPreAssignment(IList<int?> preAssignmentIds, int? assignmentId, string ModifiedByUser, ref IList<ValidationMessage> validationMessages, bool commitChange = true);

        Response IsRecordValidForProcess(DomainModel.ResourceSearch resourceSearch,
                                    ref IList<DbModel.ResourceSearch> dbResourceSearches,
                                    ref IList<DbModel.User> dbCoordinators,
                                    ref IList<DbModel.Company> dbCompanies,
                                    ref IList<DbModel.Customer> dbCustomers,
                                    ref IList<DbModel.Data> dbCategory,
                                    ref IList<DbModel.TaxonomySubCategory> dbSubCategory,
                                    ref IList<DbModel.TaxonomyService> dbService,
                                    ref IList<DbModel.OverrideResource> dbOverrideResources,
                                    ref DbModel.Assignment dbAssignment);
    }
}
