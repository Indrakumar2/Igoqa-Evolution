using Evolution.GenericDbRepository.Interfaces;
using System.Collections.Generic;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.Contract.Domain.Models.Contracts;

namespace Evolution.Contract.Domain.Interfaces.Data
{
    public interface IContractNoteRepository: IGenericRepository<DbModel.ContractNote>
    {
        IList<DomainModel.ContractNote> Search(DomainModel.ContractNote searchModel);

        int DeleteNote(List<int> noteIds);

        int DeleteNote(List<DbModel.ContractNote> contractNotes);

        int DeleteNote(List<DomainModel.ContractNote> contractNotes);
    }
}
