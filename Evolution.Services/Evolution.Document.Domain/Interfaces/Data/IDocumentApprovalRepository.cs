using Evolution.GenericDbRepository.Interfaces;
using System.Collections.Generic;
using DomainModel = Evolution.Document.Domain.Models.Document;

namespace Evolution.Document.Domain.Interfaces.Data
{
    public interface IDocumentApprovalRepository : IGenericRepository<Evolution.DbRepository.Models.SqlDatabaseContext.DocumentApproval>
    {
        IList<DomainModel.DocumentApproval> Search(DomainModel.DocumentApproval searchModel);
    }
}
