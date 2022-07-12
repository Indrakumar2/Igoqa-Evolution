using System.Collections.Generic;
using Evolution.GenericDbRepository.Interfaces;
using dbModel=Evolution.DbRepository.Models.SqlDatabaseContext;

namespace Evolution.Master.Domain.Interfaces.Data
{
    public interface ILanguageReferenceTypeRepository : IGenericRepository<dbModel.LanguageReferenceType>
    {
        IList<Models.LanguageReferenceType> Search(Models.LanguageReferenceType search);
    }
}