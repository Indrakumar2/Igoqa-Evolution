using Evolution.Google.Model.Models;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Evolution.Google.Model.Interfaces
{
    public interface IMongoGeoCoordinateService
    {
        Task<IList<MongoGeoCoordinateInfo>> SearchAsync(MongoGeoCoordinateInfo search);

        IList<MongoGeoCoordinateInfo> Search(MongoGeoCoordinateInfo search);

        IList<MongoGeoCoordinateInfo> Search(IList<MongoGeoCoordinateInfo> search);

        Task<MongoGeoCoordinateInfo> AddAsync(MongoGeoCoordinateInfo model);

        MongoGeoCoordinateInfo Add(MongoGeoCoordinateInfo model);

        Task<bool> RemoveAsync(MongoGeoCoordinateInfo model);

        bool Remove(MongoGeoCoordinateInfo model);

        Task<MongoGeoCoordinateInfo> ModifyAsync(MongoGeoCoordinateInfo model);

        MongoGeoCoordinateInfo Modify(MongoGeoCoordinateInfo model);

        IList<MongoGeoCoordinateInfo> SearchAndSyncToMongo(IList<MongoGeoCoordinateInfo> searchModels);

        bool InsureIndex();
    }
}
