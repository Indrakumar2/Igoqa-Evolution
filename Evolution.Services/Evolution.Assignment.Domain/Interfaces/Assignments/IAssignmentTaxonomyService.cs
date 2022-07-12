using Evolution.Common.Enums;
using Evolution.Common.Models.Responses;
using System.Collections.Generic;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.Assignment.Domain.Models.Assignments;

namespace Evolution.Assignment.Domain.Interfaces.Assignments
{
    public interface IAssignmentTaxonomyService 
    {
        Response Get(DomainModel.AssignmentTaxonomy searchModel);

        Response Add(IList<DomainModel.AssignmentTaxonomy> assignmentTaxonomies, bool commitChange = true, bool isValidationRequire = true, int? assignmentId = null);

        Response Add(IList<DomainModel.AssignmentTaxonomy> assignmentTaxonomies, ref IList<DbModel.AssignmentTaxonomy> dbAssignmentTaxonomies, ref IList<DbModel.Assignment> dbAssignment, ref IList<DbModel.TaxonomyService> dbTaxonomyService, bool commitChange = true, bool isValidationRequire = true, int? assignmnetid = null);

        Response Update(IList<DomainModel.AssignmentTaxonomy> assignmentTaxonomies, bool commitChange = true, bool isValidationRequire = true, int? assignmentId = null);

        Response Update(IList<DomainModel.AssignmentTaxonomy> assignmentTaxonomies, ref IList<DbModel.AssignmentTaxonomy> dbAssignmentTaxonomies, ref IList<DbModel.Assignment> assignment, ref IList<DbModel.TaxonomyService> dbTaxonomyService, bool commitChange = true, bool isValidationRequire = true, int? assignmentId = null);

        Response Delete(IList<DomainModel.AssignmentTaxonomy> assignmentTaxonomies, bool commitChange = true, bool isValidationRequire = true, int? assignmentId = null);

        Response Delete(IList<DomainModel.AssignmentTaxonomy> assignmentTaxonomies, ref IList<DbModel.AssignmentTaxonomy> dbAssignmentTaxonomies, ref IList<DbModel.Assignment> dbAssignment, ref IList<DbModel.TaxonomyService> dbTaxonomyService, bool commitChange = true, bool isValidationRequire = true, int? assignmentId = null);

        Response IsRecordValidForProcess(IList<DomainModel.AssignmentTaxonomy> assignmentTaxonomies,
                                               ValidationType validationType);

        Response IsRecordValidForProcess(IList<DomainModel.AssignmentTaxonomy> assignmentTaxonomies,
                                                 ValidationType validationType,
                                                 ref IList<DbModel.AssignmentTaxonomy> dbAssignmentTaxonomies,
                                                 ref IList<DbModel.TaxonomyService> dbTaxonomyService,
                                                 ref IList<DbModel.Assignment> dbAssignments);

        Response IsRecordValidForProcess(IList<DomainModel.AssignmentTaxonomy> assignmentTaxonomies,
                                                 ValidationType validationType,
                                                 IList<DbModel.AssignmentTaxonomy> dbAssignmentTaxonomies,
                                                 ref IList<DbModel.TaxonomyService> dbTaxonomyService,
                                                 ref IList<DbModel.Assignment> dbAssignments);




    }
}
