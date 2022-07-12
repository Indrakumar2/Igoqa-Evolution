using Evolution.Google.Model.Models;
using Evolution.MongoDb.Model.Interfaces;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace Evolution.Google.Model.Interfaces
{
    public interface IMongoGeoCoordinateRepository : IBaseMongoRepository
    {
        IMongoCollection<MongoGeoCoordinateInfo> DocumentCollection { get; }
    }
}