//using Evolution.GenericDbRepository.Services;
//using Evolution.Logging.Interfaces;
//using Newtonsoft.Json;
//using Newtonsoft.Json.Linq;
//using System;
//using System.Linq;
//using DbModels = Evolution.DbRepository.Models.SqlDatabaseContext;

//namespace Evolution.Logging
//{
//    public class DataLogger<T> : GenericRepository<T>, IDataLogger<T> where T: class
//    {
//        private readonly DbModels.EvolutionSqlDbContext _dbContext = null;

//        public DataLogger(DbModels.EvolutionSqlDbContext dbContext) : base(dbContext)
//        {
//            _dbContext = dbContext;
//        }

//        public void LogToDb(string errorCode, string message, string objectType, object args)
//        {
//            var data = JsonConvert.SerializeObject(args);
//            var companyData = (JObject)JsonConvert.DeserializeObject(data);
//            companyData.Property("UpdateCount").Remove();
//            companyData.Property("LastModification").Remove();
//            companyData.Property("ModifiedBy").Remove();

//            var log = _dbContext.Log.Where(x => x.Logged.ToString("yyyy-MM-dd") == DateTime.Now.ToString("yyyy-MM-dd") && x.Object.Equals(companyData.ToString())).FirstOrDefault();
//            if (log != null)
//            {
//                log.RetryCount = Convert.ToByte(log.RetryCount + 1);
//                _dbContext.Log.Update(log);
//            }
//            else
//            {
//                _dbContext.Log.Add(new DbModels.LogData()
//                {
//                    LoggedOn = DateTime.Now,
//                    LogReason = message,
//                    Module = typeof(T).Name,
//                    Object = companyData.ToString(),
//                    ObjectType = objectType,
//                    RetryCount = 1
//                });
//            }
//            _dbContext.SaveChanges();
//        }

//    }
//}
