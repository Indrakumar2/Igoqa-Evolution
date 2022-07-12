using Evolution.Common.Models.Responses;
using Evolution.Visit.Domain.Models.Visits;
using System.Collections.Generic;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using Evolution.Common.Enums;


namespace Evolution.Visit.Domain.Interfaces.Visits
{
    /// <summary>
    /// This will provide all the functionality related to Visit Note.
    /// </summary>
    public interface IVisitNoteService
    {
        Response Get(VisitNote searchModel);

        Response Add(IList<VisitNote> visitNotes,
                     bool commitChange = true,
                     bool isValidationRequire = true,
                     long? visitId = null);

        Response Add(IList<VisitNote> visitNotes,
                     ref IList<DbModel.VisitNote> dbVisitNotes,
                     ref IList<DbModel.Visit> dbVisits,
                     IList<DbModel.SqlauditModule> dbModule,
                     bool commitChange = true,
                     bool isValidationRequire = true,
                     long? visitId = null);
       //D661 issue 8 Start
        Response Update(IList<VisitNote> visitNotes,
                    ref IList<DbModel.VisitNote> dbVisitNotes,
                    ref IList<DbModel.Visit> dbVisits,
                    IList<DbModel.SqlauditModule> dbModule,
                    bool commitChange = true,
                    bool isValidationRequire = true,
                    long? visitId = null);
       //D661 issue 8 End
        Response Delete(IList<VisitNote> visitNotes,
                        bool commitChange = true,
                        bool isDbValidationRequired = true,
                        long? visitId = null);

        Response Delete(IList<VisitNote> visitNotes,
                        ref IList<DbModel.VisitNote> dbVisitNotes,
                        ref IList<DbModel.Visit> dbVisits,
                        bool commitChange = true,
                        bool isDbValidationRequired = true,
                        long? assignmentIds = null);

        Response IsRecordValidForProcess(IList<VisitNote> visitNotes,
                                         ValidationType validationType);

        Response IsRecordValidForProcess(IList<VisitNote> visitNotes,
                                         ValidationType validationType,
                                         ref IList<DbModel.VisitNote> dbVisitNotes,
                                         ref IList<DbModel.Visit> dbVisits);

        Response IsRecordValidForProcess(IList<VisitNote> visitNotes,
                                         ValidationType validationType,
                                         IList<DbModel.VisitNote> dbVisitNotes,
                                         ref IList<DbModel.Visit> dbVisits);
    }
}
