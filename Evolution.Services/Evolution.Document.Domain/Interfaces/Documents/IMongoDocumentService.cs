using Evolution.Document.Domain.Models.Document;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Evolution.Document.Domain.Interfaces.Documents
{
    public interface IMongoDocumentService
    {
        Task<IList<EvolutionMongoDocument>> SearchAsync(EvolutionMongoDocumentSearch search);

        Task<IList<string>> SearchDistinctFieldAsync(EvolutionMongoDocumentSearch search, string mongoFieldName = "ReferenceCode");

        IList<EvolutionMongoDocument> Search(EvolutionMongoDocumentSearch search);

        Task<EvolutionMongoDocument> AddAsync(EvolutionMongoDocument model);

        EvolutionMongoDocument Add(EvolutionMongoDocument model);

        Task<bool> RemoveAsync(EvolutionMongoDocument model);

        bool Remove(EvolutionMongoDocument model);

        bool RemoveByUniqueName(List<string> uniqueNames);

        Task<bool> RemoveByUniqueNameAsync(List<string> uniqueNames);

        Task<EvolutionMongoDocument> ModifyAsync(EvolutionMongoDocument model);

        EvolutionMongoDocument Modify(EvolutionMongoDocument model);

        bool Any(string documentUniquename);
    }
}
