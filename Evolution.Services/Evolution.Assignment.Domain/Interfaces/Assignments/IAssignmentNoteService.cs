using Evolution.Assignment.Domain.Models.Assignments;
using Evolution.Common.Enums;
using Evolution.Common.Models.Responses;
using System.Collections.Generic;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;

namespace Evolution.Assignment.Domain.Interfaces.Assignments
{
    public interface IAssignmentNoteService 
    {
        Response Get(AssignmentNote searchModel);

        Response Add(IList<AssignmentNote> assignmentNotes, bool commitChange = true, bool isValidationRequire = true,int? assignmentIds=null);

        Response Add(IList<AssignmentNote> assignmentNotes, ref IList<DbModel.AssignmentNote> dbAssignmentNotes, ref IList<DbModel.Assignment> dbAssignments, bool commitChange = true, bool isValidationRequire = true, int? assignmentIds = null);

        Response Delete(IList<AssignmentNote> assignmentNotes,bool commitChange = true,bool isDbValidationRequired = true,int? assignmentIds = null);
        Response Delete(IList<AssignmentNote> assignmentNotes,ref IList<DbModel.AssignmentNote> dbAssignmentNotes,ref IList<DbModel.Assignment> dbAssignment, bool commitChange = true, bool isDbValidationRequired = true, int? assignmentIds = null);

        Response IsRecordValidForProcess(IList<AssignmentNote> assignmentNotes, ValidationType validationType);

        Response IsRecordValidForProcess(IList<AssignmentNote> assignmentNotes, ValidationType validationType, ref IList<DbModel.AssignmentNote> dbAssignmentNotes,ref IList<DbModel.Assignment> dbAssignments);

        Response IsRecordValidForProcess(IList<AssignmentNote> assignmentNotes, ValidationType validationType, IList<DbModel.AssignmentNote> dbAssignmentNotes, ref IList<DbModel.Assignment> dbAssignments);
    }
}
