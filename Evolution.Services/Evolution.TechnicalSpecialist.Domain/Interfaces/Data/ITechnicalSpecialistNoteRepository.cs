using Evolution.Common.Enums;
using Evolution.GenericDbRepository.Interfaces;
using System.Collections.Generic;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.TechnicalSpecialist.Domain.Models.TechSpecialist;
namespace Evolution.TechnicalSpecialist.Domain.Interfaces.Data
{ 
    public interface ITechnicalSpecialistNoteRepository : IGenericRepository<DbModel.TechnicalSpecialistNote>
    {
        IList<DbModel.TechnicalSpecialistNote> Search(DomainModel.TechnicalSpecialistNoteInfo searchModel);

        IList<DbModel.TechnicalSpecialistNote> Search(IList<string> recordType,IList<int> epin);

        IList<DbModel.TechnicalSpecialistNote> Get(IList<long> tsNoteIds);

        IList<DbModel.TechnicalSpecialistNote> GetByPinId(IList<string> pinIds);

        IList<DbModel.TechnicalSpecialistNote> Get(NoteType noteType, IList<string> tsPins = null, IList<int> recordRefId = null);

        IList<DbModel.TechnicalSpecialistNote> Get(NoteType noteType, bool fetchLatest = false, IList<string> tsPins = null, IList<int> recordRefId = null);


    }
}
