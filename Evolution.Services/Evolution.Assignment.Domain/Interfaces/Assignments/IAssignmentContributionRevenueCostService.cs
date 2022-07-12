using Evolution.Common.Enums;
using Evolution.Common.Models.Responses;
using System.Collections.Generic;
using DomainModel = Evolution.Assignment.Domain.Models.Assignments;

namespace Evolution.Assignment.Domain.Interfaces.Assignments
{
    public interface IAssignmentContributionRevenueCostService
    {
        Response Get(DomainModel.AssignmentContributionRevenueCost searchModel);

        Response Add(IList<DomainModel.AssignmentContributionRevenueCost> contributionRevenueCosts, bool commitChange = true, bool isDbValidationRequired = true);

        Response Add(IList<DomainModel.AssignmentContributionRevenueCost> contributionRevenueCosts, ref IList<DbRepository.Models.SqlDatabaseContext.AssignmentContributionRevenueCost> dbAssignmentContributionRevenueCosts, bool commitChange = true, bool isDbValidationRequired = true);

        Response Modify(IList<DomainModel.AssignmentContributionRevenueCost> contributionRevenueCosts, bool commitChange = true, bool isValidationRequired = true);

        Response Modify(IList<DomainModel.AssignmentContributionRevenueCost> contributionRevenueCosts, ref IList<DbRepository.Models.SqlDatabaseContext.AssignmentContributionRevenueCost> dbAssignmentContributionRevenueCosts, bool commitChange = true, bool isDbValidationRequired = true);

        Response Delete(IList<DomainModel.AssignmentContributionRevenueCost> contributionRevenueCosts, bool commitChange = true, bool isDbValidationRequired = true);

        Response IsRecordValidForProcess(IList<DomainModel.AssignmentContributionRevenueCost> contributionRevenueCosts, ValidationType validationType);

        Response IsRecordValidForProcess(IList<DomainModel.AssignmentContributionRevenueCost> contributionRevenueCosts, ValidationType validationType, ref IList<DbRepository.Models.SqlDatabaseContext.AssignmentContributionRevenueCost> dbAssignmentContributionRevenueCosts);

        Response IsRecordValidForProcess(IList<DomainModel.AssignmentContributionRevenueCost> contributionRevenueCosts, ValidationType validationType, IList<DbRepository.Models.SqlDatabaseContext.AssignmentContributionRevenueCost> dbAssignmentContributionRevenueCosts);
    }
}
