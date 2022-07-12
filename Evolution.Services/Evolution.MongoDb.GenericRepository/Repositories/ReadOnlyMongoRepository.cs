using Evolution.MongoDb.GenericRepository.DbContexts;
using Evolution.MongoDb.Model.Interfaces;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Evolution.MongoDb.GenericRepository.Repositories
{
    public class ReadOnlyMongoRepository : IReadOnlyMongoRepository
    {
        public string ConnectionString { get; set; }

        public string DatabaseName { get; set; }

        protected IMongoDbContext MongoDbContext = null;

        #region Constructor
        protected ReadOnlyMongoRepository(string connectionString, string databaseName)
        {
            MongoDbContext = new MongoDbContext(connectionString, databaseName);
        }

        protected ReadOnlyMongoRepository(IMongoDbContext mongoDbContext)
        {
            MongoDbContext = mongoDbContext;
        }

        protected ReadOnlyMongoRepository(IMongoDatabase mongoDatabase)
        {
            MongoDbContext = new MongoDbContext(mongoDatabase);
        }
        #endregion

        #region Read
                
        public async Task<TDocument> GetByIdAsync<TDocument>(Guid id, string partitionKey = null) where TDocument : IDocument
        {
            var filter = Builders<TDocument>.Filter.Eq("Id", id);
            return await HandlePartitioned<TDocument>(partitionKey).Find(filter).FirstOrDefaultAsync();
        }
        
        public TDocument GetById<TDocument>(Guid id, string partitionKey = null) where TDocument : IDocument
        {
            var filter = Builders<TDocument>.Filter.Eq("Id", id);
            return HandlePartitioned<TDocument>(partitionKey).Find(filter).FirstOrDefault();
        }
                
        public async Task<TDocument> GetOneAsync<TDocument>(Expression<Func<TDocument, bool>> filter, string partitionKey = null) where TDocument : IDocument
        {
            return await HandlePartitioned<TDocument>(partitionKey).Find(filter).FirstOrDefaultAsync();
        }
        
        public TDocument GetOne<TDocument>(Expression<Func<TDocument, bool>> filter, string partitionKey = null) where TDocument : IDocument
        {
            return HandlePartitioned<TDocument>(partitionKey).Find(filter).FirstOrDefault();
        }

        /// <summary>
        /// Returns a collection cursor.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <param name="filter">A LINQ expression filter.</param>
        /// <param name="partitionKey">An optional partition key.</param>
        public IFindFluent<TDocument, TDocument> GetCursor<TDocument>(Expression<Func<TDocument, bool>> filter, string partitionKey = null) where TDocument : IDocument
        {
            return HandlePartitioned<TDocument>(partitionKey).Find(filter);
        }
                
        public async Task<bool> AnyAsync<TDocument>(Expression<Func<TDocument, bool>> filter, string partitionKey = null) where TDocument : IDocument
        {
            var count = await HandlePartitioned<TDocument>(partitionKey).CountDocumentsAsync(filter);
            return (count > 0);
        }
        
        public bool Any<TDocument>(Expression<Func<TDocument, bool>> filter, string partitionKey = null) where TDocument : IDocument
        {
            var count = HandlePartitioned<TDocument>(partitionKey).CountDocuments(filter);
            return (count > 0);
        }
        
        public async Task<List<TDocument>> GetAllAsync<TDocument>(Expression<Func<TDocument, bool>> filter, string partitionKey = null) where TDocument : IDocument
        {
           return await HandlePartitioned<TDocument>(partitionKey).Find(filter).ToListAsync();
        }

        public async Task<List<TDocument>> GetAllAsync<TDocument>(FilterDefinition<TDocument> filter, string partitionKey = null) where TDocument : IDocument
        {
            return await HandlePartitioned<TDocument>(partitionKey).Find(filter).ToListAsync();
        }

        public async Task<List<TField>> GetDistinctAsync<TDocument,TField>(FieldDefinition<TDocument, TField> field, FilterDefinition<TDocument> filter, string partitionKey = null) 
            where TDocument : IDocument
            where TField : class
        {
            return await HandlePartitioned<TDocument>(partitionKey).DistinctAsync(field, filter).Result.ToListAsync();
        }

        public List<TDocument> GetAll<TDocument>(Expression<Func<TDocument, bool>> filter, string partitionKey = null) where TDocument : IDocument
        {
            return HandlePartitioned<TDocument>(partitionKey).Find(filter).ToList();
        }
        
        public async Task<long> CountAsync<TDocument>(Expression<Func<TDocument, bool>> filter, string partitionKey = null) where TDocument : IDocument
        {
            return await HandlePartitioned<TDocument>(partitionKey).CountDocumentsAsync(filter);
        }
        
        public long Count<TDocument>(Expression<Func<TDocument, bool>> filter, string partitionKey = null) where TDocument : IDocument
        {
            return HandlePartitioned<TDocument>(partitionKey).Find(filter).CountDocuments();
        }
        
        public async Task<TDocument> GetByMaxAsync<TDocument>(Expression<Func<TDocument, bool>> filter, Expression<Func<TDocument, object>> maxValueSelector, string partitionKey = null)
            where TDocument : IDocument
        {
            return await GetByMaxAsync<TDocument, Guid>(filter, maxValueSelector, partitionKey);
        }
        
        public TDocument GetByMax<TDocument>(Expression<Func<TDocument, bool>> filter, Expression<Func<TDocument, object>> maxValueSelector, string partitionKey = null)
            where TDocument : IDocument
        {
            return GetByMax<TDocument, Guid>(filter, maxValueSelector, partitionKey);
        }
        
        public async Task<TDocument> GetByMinAsync<TDocument>(Expression<Func<TDocument, bool>> filter, Expression<Func<TDocument, object>> minValueSelector, string partitionKey = null)
            where TDocument : IDocument
        {
            return await GetByMinAsync<TDocument, Guid>(filter, minValueSelector, partitionKey);
        }
        
        public TDocument GetByMin<TDocument>(Expression<Func<TDocument, bool>> filter, Expression<Func<TDocument, object>> orderByAscending, string partitionKey = null)
            where TDocument : IDocument
        {
            return GetByMin<TDocument, Guid>(filter, orderByAscending, partitionKey);
        }

        #endregion

        #region Read TKey
        
        public async Task<TDocument> GetByIdAsync<TDocument, TKey>(TKey id, string partitionKey = null)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>
        {
            var filter = Builders<TDocument>.Filter.Eq("Id", id);
            return await HandlePartitioned<TDocument, TKey>(partitionKey).Find(filter).FirstOrDefaultAsync();
        }
        
        public TDocument GetById<TDocument, TKey>(TKey id, string partitionKey = null)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>
        {
            var filter = Builders<TDocument>.Filter.Eq("Id", id);
            return HandlePartitioned<TDocument, TKey>(partitionKey).Find(filter).FirstOrDefault();
        }
        
        public async Task<TDocument> GetOneAsync<TDocument, TKey>(Expression<Func<TDocument, bool>> filter, string partitionKey = null)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>
        {
            return await HandlePartitioned<TDocument, TKey>(partitionKey).Find(filter).FirstOrDefaultAsync();
        }
        
        public TDocument GetOne<TDocument, TKey>(Expression<Func<TDocument, bool>> filter, string partitionKey = null)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>
        {
            return HandlePartitioned<TDocument, TKey>(partitionKey).Find(filter).FirstOrDefault();
        }

        /// <summary>
        /// Returns a collection cursor.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <typeparam name="TKey">The type of the primary key for a Document.</typeparam>
        /// <param name="filter">A LINQ expression filter.</param>
        /// <param name="partitionKey">An optional partition key.</param>
        public IFindFluent<TDocument, TDocument> GetCursor<TDocument, TKey>(Expression<Func<TDocument, bool>> filter, string partitionKey = null)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>
        {
            return HandlePartitioned<TDocument, TKey>(partitionKey).Find(filter);
        }
        
        public async Task<bool> AnyAsync<TDocument, TKey>(Expression<Func<TDocument, bool>> filter, string partitionKey = null)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>
        {
            var count = await HandlePartitioned<TDocument, TKey>(partitionKey).CountDocumentsAsync(filter);
            return (count > 0);
        }
        
        public bool Any<TDocument, TKey>(Expression<Func<TDocument, bool>> filter, string partitionKey = null)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>
        {
            var count = HandlePartitioned<TDocument, TKey>(partitionKey).CountDocuments(filter);
            return (count > 0);
        }
        
        public async Task<List<TDocument>> GetAllAsync<TDocument, TKey>(Expression<Func<TDocument, bool>> filter, string partitionKey = null)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>
        {
            return await HandlePartitioned<TDocument, TKey>(partitionKey).Find(filter).ToListAsync();
        }
        
        public List<TDocument> GetAll<TDocument, TKey>(Expression<Func<TDocument, bool>> filter, string partitionKey = null)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>
        {
            return HandlePartitioned<TDocument, TKey>(partitionKey).Find(filter).ToList();
        }
        
        public async Task<long> CountAsync<TDocument, TKey>(Expression<Func<TDocument, bool>> filter, string partitionKey = null)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>
        {
            return await HandlePartitioned<TDocument, TKey>(partitionKey).CountDocumentsAsync(filter);
        }
        
        public long Count<TDocument, TKey>(Expression<Func<TDocument, bool>> filter, string partitionKey = null)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>
        {
            return HandlePartitioned<TDocument, TKey>(partitionKey).Find(filter).CountDocuments();
        }
        
        public async Task<TDocument> GetByMaxAsync<TDocument, TKey>(Expression<Func<TDocument, bool>> filter, Expression<Func<TDocument, object>> maxValueSelector, string partitionKey = null)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>
        {
            return await GetCollection<TDocument, TKey>(partitionKey).Find(Builders<TDocument>.Filter.Where(filter))
                                                                     .SortByDescending(maxValueSelector)
                                                                     .Limit(1)
                                                                     .FirstOrDefaultAsync();
        }
        
        public TDocument GetByMax<TDocument, TKey>(Expression<Func<TDocument, bool>> filter, Expression<Func<TDocument, object>> maxValueSelector, string partitionKey = null)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>
        {
            return GetCollection<TDocument, TKey>(partitionKey).Find(Builders<TDocument>.Filter.Where(filter))
                                                               .SortByDescending(maxValueSelector)
                                                               .Limit(1)
                                                               .FirstOrDefault();
        }
        
        public async Task<TDocument> GetByMinAsync<TDocument, TKey>(Expression<Func<TDocument, bool>> filter, Expression<Func<TDocument, object>> minValueSelector, string partitionKey = null)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>
        {
            return await GetCollection<TDocument, TKey>(partitionKey).Find(Builders<TDocument>.Filter.Where(filter))
                                                   .SortBy(minValueSelector)
                                                   .Limit(1)
                                                   .FirstOrDefaultAsync();
        }
        
        public TDocument GetByMin<TDocument, TKey>(Expression<Func<TDocument, bool>> filter, Expression<Func<TDocument, object>> minValueSelector, string partitionKey = null)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>
        {
            return GetCollection<TDocument, TKey>(partitionKey).Find(Builders<TDocument>.Filter.Where(filter))
                                                               .SortBy(minValueSelector)
                                                               .Limit(1)
                                                               .FirstOrDefault();
        }
        
        private IFindFluent<TDocument, TDocument> GetMinMongoQuery<TDocument, TKey, TValue>(Expression<Func<TDocument, bool>> filter, Expression<Func<TDocument, TValue>> minValueSelector, string partitionKey = null)
                    where TDocument : IDocument<TKey>
                    where TKey : IEquatable<TKey>
        {
            return GetCollection<TDocument, TKey>(partitionKey).Find(Builders<TDocument>.Filter.Where(filter))
                                                               .SortBy(ConvertExpression(minValueSelector))
                                                               .Limit(1);
        }
        
        private IFindFluent<TDocument, TDocument> GetMaxMongoQuery<TDocument, TKey, TValue>(Expression<Func<TDocument, bool>> filter, Expression<Func<TDocument, TValue>> maxValueSelector, string partitionKey = null)
                    where TDocument : IDocument<TKey>
                    where TKey : IEquatable<TKey>
        {
            return GetCollection<TDocument, TKey>(partitionKey).Find(Builders<TDocument>.Filter.Where(filter))
                                                               .SortByDescending(ConvertExpression(maxValueSelector))
                                                               .Limit(1);
        }
        
        public async Task<TValue> GetMaxValueAsync<TDocument, TValue>(Expression<Func<TDocument, bool>> filter, Expression<Func<TDocument, TValue>> maxValueSelector, string partitionKey = null)
            where TDocument : IDocument
        {
            return await GetMaxValueAsync<TDocument, Guid, TValue>(filter, maxValueSelector, partitionKey);
        }
        
        public async Task<TValue> GetMaxValueAsync<TDocument, TKey, TValue>(Expression<Func<TDocument, bool>> filter, Expression<Func<TDocument, TValue>> maxValueSelector, string partitionKey = null)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>
        {
            return await GetMaxMongoQuery<TDocument, TKey, TValue>(filter, maxValueSelector, partitionKey)
                .Project(maxValueSelector)
                .FirstOrDefaultAsync();
        }
        
        public TValue GetMaxValue<TDocument, TValue>(Expression<Func<TDocument, bool>> filter, Expression<Func<TDocument, TValue>> maxValueSelector, string partitionKey = null)
            where TDocument : IDocument
        {
            return GetMaxValue<TDocument, Guid, TValue>(filter, maxValueSelector, partitionKey);
        }
        
        public TValue GetMaxValue<TDocument, TKey, TValue>(Expression<Func<TDocument, bool>> filter, Expression<Func<TDocument, TValue>> maxValueSelector, string partitionKey = null)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>
        {

            return GetMaxMongoQuery<TDocument, TKey, TValue>(filter, maxValueSelector, partitionKey)
                      .Project(maxValueSelector)
                      .FirstOrDefault();
        }
        
        public async Task<TValue> GetMinValueAsync<TDocument, TValue>(Expression<Func<TDocument, bool>> filter, Expression<Func<TDocument, TValue>> minValueSelector, string partitionKey = null)
            where TDocument : IDocument
        {
            return await GetMinValueAsync<TDocument, Guid, TValue>(filter, minValueSelector, partitionKey);
        }
        
        public async Task<TValue> GetMinValueAsync<TDocument, TKey, TValue>(Expression<Func<TDocument, bool>> filter, Expression<Func<TDocument, TValue>> minValueSelector, string partitionKey = null)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>
        {
            return await GetMinMongoQuery<TDocument, TKey, TValue>(filter, minValueSelector, partitionKey).Project(minValueSelector).FirstOrDefaultAsync();
        }
        
        public TValue GetMinValue<TDocument, TValue>(Expression<Func<TDocument, bool>> filter, Expression<Func<TDocument, TValue>> minValueSelector, string partitionKey = null)
            where TDocument : IDocument
        {
            return GetMinValue<TDocument, Guid, TValue>(filter, minValueSelector, partitionKey);
        }
        
        public TValue GetMinValue<TDocument, TKey, TValue>(Expression<Func<TDocument, bool>> filter, Expression<Func<TDocument, TValue>> minValueSelector, string partitionKey = null)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>
        {
            return GetMinMongoQuery<TDocument, TKey, TValue>(filter, minValueSelector, partitionKey).Project(minValueSelector).FirstOrDefault();
        }

        #endregion

        #region Utility Methods

        /// <summary>
        /// Gets a collections for the type TDocument with the matching partition key (if any).
        /// </summary>
        /// <typeparam name="TDocument">The document type.</typeparam>
        /// <param name="partitionKey">An optional partition key.</param>
        /// <returns>An <see cref="IMongoCollection{TDocument}"/></returns>
        protected IMongoCollection<TDocument> GetCollection<TDocument>(string partitionKey = null) where TDocument : IDocument
        {
            return MongoDbContext.GetCollection<TDocument>(partitionKey);
        }

        /// <summary>
        /// Gets a collections for the type TDocument
        /// </summary>
        /// <typeparam name="TDocument">The document type.</typeparam>
        /// <param name="document">The document.</param>
        /// <returns></returns>
        protected IMongoCollection<TDocument> HandlePartitioned<TDocument>(TDocument document) where TDocument : IDocument
        {
            if (document is IPartitionedDocument)
            {
                return GetCollection<TDocument>(((IPartitionedDocument)document).PartitionKey);
            }
            return GetCollection<TDocument>();
        }

        /// <summary>
        /// Gets a collections for a potentially partitioned document type.
        /// </summary>
        /// <typeparam name="TDocument">The document type.</typeparam>
        /// <typeparam name="TKey">The type of the primary key.</typeparam>
        /// <param name="document">The document.</param>
        /// <returns></returns>
        protected IMongoCollection<TDocument> HandlePartitioned<TDocument, TKey>(TDocument document)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>
        {
            if (document is IPartitionedDocument)
            {
                return GetCollection<TDocument, TKey>(((IPartitionedDocument)document).PartitionKey);
            }
            return GetCollection<TDocument, TKey>();
        }

        /// <summary>
        /// Gets a collections for a potentially partitioned document type.
        /// </summary>
        /// <typeparam name="TDocument">The document type.</typeparam>
        /// <param name="partitionKey">The collection partition key.</param>
        /// <returns></returns>
        protected IMongoCollection<TDocument> HandlePartitioned<TDocument>(string partitionKey) where TDocument : IDocument
        {
            if (!string.IsNullOrEmpty(partitionKey))
            {
                return GetCollection<TDocument>(partitionKey);
            }
            return GetCollection<TDocument>();
        }

        /// <summary>
        /// Gets a collections for the type TDocument with a partition key.
        /// </summary>
        /// <typeparam name="TDocument">The document type.</typeparam>
        /// <typeparam name="TKey">The type of the primary key.</typeparam>
        /// <param name="partitionKey">The collection partition key.</param>
        /// <returns></returns>
        protected IMongoCollection<TDocument> GetCollection<TDocument, TKey>(string partitionKey = null)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>
        {
            return MongoDbContext.GetCollection<TDocument>(partitionKey);
        }

        /// <summary>
        /// Gets a collections for a potentially partitioned document type.
        /// </summary>
        /// <typeparam name="TDocument">The document type.</typeparam>
        /// <typeparam name="TKey">The type of the primary key.</typeparam>
        /// <param name="partitionKey">The collection partition key.</param>
        /// <returns></returns>
        protected IMongoCollection<TDocument> HandlePartitioned<TDocument, TKey>(string partitionKey)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>
        {
            if (!string.IsNullOrEmpty(partitionKey))
            {
                return GetCollection<TDocument, TKey>(partitionKey);
            }
            return GetCollection<TDocument, TKey>();
        }

        /// <summary>
        /// Converts a LINQ expression of TDocument, TValue to a LINQ expression of TDocument, object
        /// </summary>
        /// <typeparam name="TDocument">The document type.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="expression">The expression to convert</param>
        protected static Expression<Func<TDocument, object>> ConvertExpression<TDocument, TValue>(Expression<Func<TDocument, TValue>> expression)
        {
            var param = expression.Parameters[0];
            Expression body = expression.Body;
            var convert = Expression.Convert(body, typeof(object));
            return Expression.Lambda<Func<TDocument, object>>(convert, param);
        }

        #endregion
    }
}