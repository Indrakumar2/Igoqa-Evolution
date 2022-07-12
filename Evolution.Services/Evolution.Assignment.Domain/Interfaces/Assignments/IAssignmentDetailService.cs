using System.Threading.Tasks;
using Evolution.Assignment.Domain.Models.Assignments;
using Evolution.Common.Models.Responses;

namespace Evolution.Assignment.Domain.Interfaces.Assignments
{
    public interface IAssignmentDetailService
    {
        Response Add(AssignmentDetail assignmentDetails, bool IsAPIValidationRequired=false);

        Response Modify(AssignmentDetail assignmentDetails, bool IsAPIValidationRequired = false);

        Response Delete(AssignmentDetail assignmentDetails,bool IsAPIValidationRequired= false);

    }
}
