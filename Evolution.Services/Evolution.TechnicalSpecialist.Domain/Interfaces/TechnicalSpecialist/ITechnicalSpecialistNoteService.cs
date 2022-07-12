using Evolution.Common.Enums;
using Evolution.Common.Models.Messages;
using Evolution.Common.Models.Responses;
using Evolution.TechnicalSpecialist.Domain.Models.TechSpecialist;
using System.Collections.Generic;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;

namespace Evolution.TechnicalSpecialist.Domain.Interfaces
{
    public interface ITechnicalSpecialistNoteService
    {
        Response Get(TechnicalSpecialistNoteInfo searchModel);
        Response Get(IList<int> epin, IList<string> recordType);
        Response Get(IList<long> tsNoteIds);

        Response Get(NoteType noteType, IList<string> tsPins =null, IList<int> recordRefId = null);

        Response Get(NoteType noteType, bool fetchLatest, IList<string> tsPins = null, IList<int> recordRefId = null);

        Response GetByTSPin(IList<string> tsPins); 

        Response Add(IList<TechnicalSpecialistNoteInfo> tsNotes,
                       bool commitChange = true,
                       bool isDbValidationRequired = true);


        Response Add(IList<TechnicalSpecialistNoteInfo> tsNotes,
                        ref IList<DbModel.TechnicalSpecialistNote> dbTsNotes,
                        ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists,
                        bool commitChange = true,
                        bool isDbValidationRequired = true);
       //D661 issue 8 Start
        Response Update(IList<TechnicalSpecialistNoteInfo> tsNotes,
                  ref IList<DbModel.TechnicalSpecialistNote> dbTsNotes,
                  ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists,
                  bool commitChange = true,
                  bool isDbValidationRequired = true);
       //D661 issue 8 End
        Response IsRecordValidForProcess(IList<TechnicalSpecialistNoteInfo> tsNotes, ValidationType validationType);

        Response IsRecordValidForProcess(IList<TechnicalSpecialistNoteInfo> tsNotes,
                                        ValidationType validationType,
                                        ref IList<TechnicalSpecialistNoteInfo> filteredTSNotes,
                                        ref IList<DbModel.TechnicalSpecialistNote> dbTsNotes,
                                        ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists);

        Response IsRecordValidForProcess(IList<TechnicalSpecialistNoteInfo> tsNotes,
                                        ValidationType validationType,
                                        ref IList<DbModel.TechnicalSpecialistNote> dbTsNotes);

        Response IsRecordValidForProcess(IList<TechnicalSpecialistNoteInfo> tsNotes,
                                        ValidationType validationType,
                                        IList<DbModel.TechnicalSpecialistNote> dbTsNotes);

        Response IsRecordExistInDb(IList<long> tsNoteIds,
                                        ref IList<DbModel.TechnicalSpecialistNote> dbTsNotes,
                                        ref IList<ValidationMessage> validationMessages);

        Response IsRecordExistInDb(IList<long> tsNoteIds,
                                          ref IList<DbModel.TechnicalSpecialistNote> dbTsNotes,
                                          ref IList<long> tsNoteIdNotExists,
                                          ref IList<ValidationMessage> validationMessages);
    }
}
