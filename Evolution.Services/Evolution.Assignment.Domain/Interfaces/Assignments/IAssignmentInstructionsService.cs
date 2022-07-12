using Evolution.Common.Models.Responses;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.Assignment.Domain.Models.Assignments;

namespace Evolution.Assignment.Domain.Interfaces.Assignments
{
    public interface IAssignmentInstructionsService 
    {
        Response GetAssignmentInstructions(int assignmentId);

        Response Add(int assignmentId,
                     DomainModel.AssignmentInstructions assignmentInstructions,
                     DbModel.Assignment dbAssignment = null);

        Response Modify(int assignmentId,
                        DomainModel.AssignmentInstructions assignmentInstructions,
                        DbModel.Assignment dbAssignment = null);
    }
}
