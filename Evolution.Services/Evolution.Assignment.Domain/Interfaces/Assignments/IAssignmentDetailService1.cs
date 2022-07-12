using Evolution.Assignment.Domain.Models.Assignments;
using Evolution.Common.Models.Responses;

namespace Evolution.Assignment.Domain.Interfaces.Assignments
{
    public interface IAssignmentDetailService1
    {
        Response Get(AssignmentDetail searchModel);

        Response Add(AssignmentDetail assignmentDetails);

        Response Modify(AssignmentDetail assignmentDetails);

        Response Delete(AssignmentDetail assignmentDetails);

        Response Add(AssignmentDetail assignmentDetails, bool isAdd);
    }
}
