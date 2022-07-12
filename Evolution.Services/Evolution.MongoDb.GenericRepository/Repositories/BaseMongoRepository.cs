using Evolution.MongoDb.GenericRepository.DbContexts;
using Evolution.MongoDb.GenericRepository.Utils;
using Evolution.MongoDb.Model.Interfaces;
using Evolution.MongoDb.Model.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Evolution.MongoDb.GenericRepository.Repositories
{
    public abstract partial class BaseMongoRepository : ReadOnlyMongoRepository, IBaseMongoRepository
    {
        #region Constructor

        protected BaseMongoRepository(string connectionString, string databaseName) : base(connectionString, databaseName)
        {
            MongoDbContext = new MongoDbContext(connectionString, databaseName);
        }

        protected BaseMongoRepository(IMongoDbContext mongoDbContext) : base(mongoDbContext)
        {
            MongoDbContext = mongoDbContext;
        }

        protected BaseMongoRepository(IMongoDatabase mongoDatabase) : base(mongoDatabase)
        {
            MongoDbContext = new MongoDbContext(mongoDatabase);
        }

        #endregion

        #region Create
        
        public virtual async Task AddOneAsync<TDocument>(TDocument document) where TDocument : IDocument
        {
            FormatDocument(document);
            await HandlePartitioned(document).InsertOneAsync(document);
        }
        
        public virtual void AddOne<TDocument>(TDocument document) where TDocument : IDocument
        {
            FormatDocument(document);
            HandlePartitioned(document).InsertOne(document);
        }
        
        public virtual async Task AddManyAsync<TDocument>(IEnumerable<TDocument> documents) where TDocument : IDocument
        {
            if (!documents.Any())
            {
                return;
            }
            foreach (var document in documents)
            {
                FormatDocument(document);
            }
            // cannot use typeof(IPartitionedDocument).IsAssignableFrom(typeof(TDocument)), not available in netstandard 1.5
            if (documents.Any(e => e is IPartitionedDocument))
            {
                foreach (var group in documents.GroupBy(e => ((IPartitionedDocument)e).PartitionKey))
                {
                    await HandlePartitioned(group.FirstOrDefault()).InsertManyAsync(group.ToList());
                }
            }
            else
            {
                await GetCollection<TDocument>().InsertManyAsync(documents.ToList());
            }
        }
        
        public virtual void AddMany<TDocument>(IEnumerable<TDocument> documents) where TDocument : IDocument
        {
            if (!documents.Any())
            {
                return;
            }
            foreach (var document in documents)
            {
                FormatDocument(document);
            }
            // cannot use typeof(IPartitionedDocument).IsAssignableFrom(typeof(TDocument)), not available in netstandard 1.5
            if (documents.Any(e => e is IPartitionedDocument))
            {
                foreach (var group in documents.GroupBy(e => ((IPartitionedDocument)e).PartitionKey))
                {
                    HandlePartitioned(group.FirstOrDefault()).InsertMany(group.ToList());
                }
            }
            else
            {
                GetCollection<TDocument>().InsertMany(documents.ToList());
            }
        }

        #endregion Create

        #region Create TKey
        
        public virtual async Task AddOneAsync<TDocument, TKey>(TDocument document)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>
        {
            FormatDocument<TDocument, TKey>(document);
            await HandlePartitioned<TDocument, TKey>(document).InsertOneAsync(document);
        }
        
        public virtual void AddOne<TDocument, TKey>(TDocument document)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>
        {
            FormatDocument<TDocument, TKey>(document);
            HandlePartitioned<TDocument, TKey>(document).InsertOne(document);
        }
        
        public virtual async Task AddManyAsync<TDocument, TKey>(IEnumerable<TDocument> documents)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>
        {
            if (!documents.Any())
            {
                return;
            }
            foreach (var document in documents)
            {
                FormatDocument<TDocument, TKey>(document);
            }
            // cannot use typeof(IPartitionedDocument).IsAssignableFrom(typeof(TDocument)), not available in netstandard 1.5
            if (documents.Any(e => e is IPartitionedDocument))
            {
                foreach (var group in documents.GroupBy(e => ((IPartitionedDocument)e).PartitionKey))
                {
                    await HandlePartitioned<TDocument, TKey>(group.FirstOrDefault()).InsertManyAsync(group.ToList());
                }
            }
            else
            {
                await GetCollection<TDocument, TKey>().InsertManyAsync(documents.ToList());
            }
        }
        
        public virtual void AddMany<TDocument, TKey>(IEnumerable<TDocument> documents)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>
        {
            if (!documents.Any())
            {
                return;
            }
            foreach (var document in documents)
            {
                FormatDocument<TDocument, TKey>(document);
            }
            // cannot use typeof(IPartitionedDocument).IsAssignableFrom(typeof(TDocument)), not available in netstandard 1.5
            if (documents.Any(e => e is IPartitionedDocument))
            {
                foreach (var group in documents.GroupBy(e => ((IPartitionedDocument)e).PartitionKey))
                {
                    HandlePartitioned<TDocument, TKey>(group.FirstOrDefault()).InsertMany(group.ToList());
                }
            }
            else
            {
                GetCollection<TDocument, TKey>().InsertMany(documents.ToList());
            }
        }

        #endregion 

        #region Update

        public virtual async Task<bool> UpdateOneAsync<TDocument>(TDocument modifiedDocument) where TDocument : IDocument
        {
            var updateRes = await HandlePartitioned(modifiedDocument).ReplaceOneAsync(x => x.Id == modifiedDocument.Id, modifiedDocument);
            return updateRes.ModifiedCount == 1;
        }

        public virtual bool UpdateOne<TDocument>(TDocument modifiedDocument) where TDocument : IDocument
        {
            var updateRes = HandlePartitioned(modifiedDocument).ReplaceOne(x => x.Id == modifiedDocument.Id, modifiedDocument);
            return updateRes.ModifiedCount == 1;
        }

        public virtual async Task<bool> UpdateOneAsync<TDocument>(TDocument documentToModify, UpdateDefinition<TDocument> update)
            where TDocument : IDocument
        {
            var filter = Builders<TDocument>.Filter.Eq("Id", documentToModify.Id);
            var updateRes = await HandlePartitioned(documentToModify).UpdateOneAsync(filter, update);
            return updateRes.ModifiedCount == 1;
        }

        public virtual bool UpdateOne<TDocument, TField>(TDocument documentToModify, Expression<Func<TDocument, TField>> field, TField value)
            where TDocument : IDocument
        {
            var filter = Builders<TDocument>.Filter.Eq("Id", documentToModify.Id);
            var updateRes = HandlePartitioned(documentToModify).UpdateOne(filter, Builders<TDocument>.Update.Set(field, value));
            return updateRes.ModifiedCount == 1;
        }
        
        public virtual async Task<bool> UpdateOneAsync<TDocument, TField>(TDocument documentToModify, Expression<Func<TDocument, TField>> field, TField value)
            where TDocument : IDocument
        {
            var filter = Builders<TDocument>.Filter.Eq("Id", documentToModify.Id);
            var updateRes = await HandlePartitioned(documentToModify).UpdateOneAsync(filter, Builders<TDocument>.Update.Set(field, value));
            return updateRes.ModifiedCount == 1;
        }
        
        public virtual bool UpdateOne<TDocument, TField>(FilterDefinition<TDocument> filter, Expression<Func<TDocument, TField>> field, TField value, string partitionKey = null)
            where TDocument : IDocument
        {
            var collection = string.IsNullOrEmpty(partitionKey) ? GetCollection<TDocument>() : GetCollection<TDocument>(partitionKey);
            var updateRes = collection.UpdateOne(filter, Builders<TDocument>.Update.Set(field, value));
            return updateRes.ModifiedCount == 1;
        }
        
        public virtual bool UpdateOne<TDocument, TField>(Expression<Func<TDocument, bool>> filter, Expression<Func<TDocument, TField>> field, TField value, string partitionKey = null)
            where TDocument : IDocument
        {
            var collection = string.IsNullOrEmpty(partitionKey) ? GetCollection<TDocument>() : GetCollection<TDocument>(partitionKey);
            var updateRes = collection.UpdateOne(Builders<TDocument>.Filter.Where(filter), Builders<TDocument>.Update.Set(field, value));
            return updateRes.ModifiedCount == 1;
        }
        
        public virtual async Task<bool> UpdateOneAsync<TDocument, TField>(FilterDefinition<TDocument> filter, Expression<Func<TDocument, TField>> field, TField value, string partitionKey = null)
            where TDocument : IDocument
        {
            var collection = string.IsNullOrEmpty(partitionKey) ? GetCollection<TDocument>() : GetCollection<TDocument>(partitionKey);
            var updateRes = await collection.UpdateOneAsync(filter, Builders<TDocument>.Update.Set(field, value));
            return updateRes.ModifiedCount == 1;
        }
        
        public virtual async Task<bool> UpdateOneAsync<TDocument, TField>(Expression<Func<TDocument, bool>> filter, Expression<Func<TDocument, TField>> field, TField value, string partitionKey = null)
            where TDocument : IDocument
        {
            var collection = string.IsNullOrEmpty(partitionKey) ? GetCollection<TDocument>() : GetCollection<TDocument>(partitionKey);
            var updateRes = await collection.UpdateOneAsync(Builders<TDocument>.Filter.Where(filter), Builders<TDocument>.Update.Set(field, value));
            return updateRes.ModifiedCount == 1;
        }
        
        public virtual bool UpdateOne<TDocument>(TDocument documentToModify, UpdateDefinition<TDocument> update)
            where TDocument : IDocument
        {
            var filter = Builders<TDocument>.Filter.Eq("Id", documentToModify.Id);
            var updateRes = HandlePartitioned(documentToModify).UpdateOne(filter, update, new UpdateOptions { IsUpsert = true });
            return updateRes.ModifiedCount == 1;
        }

        #endregion Update

        #region Update TKey

        public virtual async Task<bool> UpdateOneAsync<TDocument, TKey>(TDocument modifiedDocument)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>
        {
            var filter = Builders<TDocument>.Filter.Eq("Id", modifiedDocument.Id);
            var updateRes = await HandlePartitioned<TDocument, TKey>(modifiedDocument).ReplaceOneAsync(filter, modifiedDocument);
            return updateRes.ModifiedCount == 1;
        }
        
        public virtual bool UpdateOne<TDocument, TKey>(TDocument modifiedDocument)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>
        {
            var filter = Builders<TDocument>.Filter.Eq("Id", modifiedDocument.Id);
            var updateRes = HandlePartitioned<TDocument, TKey>(modifiedDocument).ReplaceOne(filter, modifiedDocument);
            return updateRes.ModifiedCount == 1;
        }
        
        public virtual async Task<bool> UpdateOneAsync<TDocument, TKey>(TDocument documentToModify, UpdateDefinition<TDocument> update)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>
        {
            var filter = Builders<TDocument>.Filter.Eq("Id", documentToModify.Id);
            var updateRes = await HandlePartitioned<TDocument, TKey>(documentToModify).UpdateOneAsync(filter, update, new UpdateOptions { IsUpsert = true });
            return updateRes.ModifiedCount == 1;
        }
        
        public virtual bool UpdateOne<TDocument, TKey>(TDocument documentToModify, UpdateDefinition<TDocument> update)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>
        {
            var filter = Builders<TDocument>.Filter.Eq("Id", documentToModify.Id);
            var updateRes = HandlePartitioned<TDocument, TKey>(documentToModify).UpdateOne(filter, update, new UpdateOptions { IsUpsert = true });
            return updateRes.ModifiedCount == 1;
        }
        
        public virtual async Task<bool> UpdateOneAsync<TDocument, TKey, TField>(TDocument documentToModify, Expression<Func<TDocument, TField>> field, TField value)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>
        {
            var filter = Builders<TDocument>.Filter.Eq("Id", documentToModify.Id);
            var updateRes = await HandlePartitioned<TDocument, TKey>(documentToModify).UpdateOneAsync(filter, Builders<TDocument>.Update.Set(field, value));
            return updateRes.ModifiedCount == 1;
        }

        public virtual bool UpdateOne<TDocument, TKey, TField>(TDocument documentToModify, Expression<Func<TDocument, TField>> field, TField value)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>
        {
            var filter = Builders<TDocument>.Filter.Eq("Id", documentToModify.Id);
            var updateRes = HandlePartitioned<TDocument, TKey>(documentToModify).UpdateOne(filter, Builders<TDocument>.Update.Set(field, value));
            return updateRes.ModifiedCount == 1;
        }
        
        public virtual async Task<bool> UpdateOneAsync<TDocument, TKey, TField>(FilterDefinition<TDocument> filter, Expression<Func<TDocument, TField>> field, TField value, string partitionKey = null)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>
        {
            var collection = string.IsNullOrEmpty(partitionKey) ? GetCollection<TDocument, TKey>() : GetCollection<TDocument, TKey>(partitionKey);
            var updateRes = await collection.UpdateOneAsync(filter, Builders<TDocument>.Update.Set(field, value));
            return updateRes.ModifiedCount == 1;
        }
        
        public virtual async Task<bool> UpdateOneAsync<TDocument, TKey, TField>(Expression<Func<TDocument, bool>> filter, Expression<Func<TDocument, TField>> field, TField value, string partitionKey = null)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>
        {
            return await UpdateOneAsync<TDocument, TKey, TField>(Builders<TDocument>.Filter.Where(filter), field, value, partitionKey);
        }

        public virtual bool UpdateOne<TDocument, TKey, TField>(FilterDefinition<TDocument> filter, Expression<Func<TDocument, TField>> field, TField value, string partitionKey = null)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>
        {
            var collection = string.IsNullOrEmpty(partitionKey) ? GetCollection<TDocument, TKey>() : GetCollection<TDocument, TKey>(partitionKey);
            var updateRes = collection.UpdateOne(filter, Builders<TDocument>.Update.Set(field, value));
            return updateRes.ModifiedCount == 1;
        }

        public virtual bool UpdateOne<TDocument, TKey, TField>(Expression<Func<TDocument, bool>> filter, Expression<Func<TDocument, TField>> field, TField value, string partitionKey = null)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>
        {
            return UpdateOne<TDocument, TKey, TField>(Builders<TDocument>.Filter.Where(filter), field, value, partitionKey);
        }

        #endregion Update

        #region Delete
        
        public virtual async Task<long> DeleteOneAsync<TDocument>(TDocument document) where TDocument : IDocument
        {
            return await DeleteOneAsync<TDocument, Guid>(document);
        }

        public virtual long DeleteOne<TDocument>(TDocument document) where TDocument : IDocument
        {
            return DeleteOne<TDocument, Guid>(document);
        }
        
        public virtual long DeleteOne<TDocument>(Expression<Func<TDocument, bool>> filter, string partitionKey = null) where TDocument : IDocument
        {
            return DeleteOne<TDocument, Guid>(filter, partitionKey);
        }
        
        public virtual async Task<long> DeleteOneAsync<TDocument>(Expression<Func<TDocument, bool>> filter, string partitionKey = null) where TDocument : IDocument
        {
            return await DeleteOneAsync<TDocument, Guid>(filter, partitionKey);
        }
        
        public virtual async Task<long> DeleteManyAsync<TDocument>(Expression<Func<TDocument, bool>> filter, string partitionKey = null) where TDocument : IDocument
        {
            return (await HandlePartitioned<TDocument>(partitionKey).DeleteManyAsync(filter)).DeletedCount;
        }

        public virtual async Task<long> DeleteManyAsync<TDocument>(IEnumerable<TDocument> documents) where TDocument : IDocument
        {
            return await DeleteManyAsync<TDocument, Guid>(documents);
        }

        public virtual long DeleteMany<TDocument>(IEnumerable<TDocument> documents) where TDocument : IDocument
        {
            return DeleteMany<TDocument, Guid>(documents);
        }

        public virtual long DeleteMany<TDocument>(Expression<Func<TDocument, bool>> filter, string partitionKey = null) where TDocument : IDocument
        {
            return HandlePartitioned<TDocument>(partitionKey).DeleteMany(filter).DeletedCount;
        }

        #endregion Delete

        #region Delete TKey
        
        public virtual long DeleteOne<TDocument, TKey>(TDocument document)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>
        {
            var filter = Builders<TDocument>.Filter.Eq("Id", document.Id);
            return HandlePartitioned<TDocument, TKey>(document).DeleteOne(filter).DeletedCount;
        }

        public virtual async Task<long> DeleteOneAsync<TDocument, TKey>(TDocument document)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>
        {
            var filter = Builders<TDocument>.Filter.Eq("Id", document.Id);
            return (await HandlePartitioned<TDocument, TKey>(document).DeleteOneAsync(filter)).DeletedCount;
        }

        public virtual long DeleteOne<TDocument, TKey>(Expression<Func<TDocument, bool>> filter, string partitionKey = null)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>
        {
            return HandlePartitioned<TDocument, TKey>(partitionKey).DeleteOne(filter).DeletedCount;
        }

        public virtual async Task<long> DeleteOneAsync<TDocument, TKey>(Expression<Func<TDocument, bool>> filter, string partitionKey = null)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>
        {
            return (await HandlePartitioned<TDocument, TKey>(partitionKey).DeleteOneAsync(filter)).DeletedCount;
        }

        public virtual async Task<long> DeleteManyAsync<TDocument, TKey>(Expression<Func<TDocument, bool>> filter, string partitionKey = null)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>
        {
            return (await HandlePartitioned<TDocument, TKey>(partitionKey).DeleteManyAsync(filter)).DeletedCount;
        }

        public virtual async Task<long> DeleteManyAsync<TDocument, TKey>(IEnumerable<TDocument> documents)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>
        {
            if (!documents.Any())
            {
                return 0;
            }
            // cannot use typeof(IPartitionedDocument).IsAssignableFrom(typeof(TDocument)), not available in netstandard 1.5
            if (documents.Any(e => e is IPartitionedDocument))
            {
                long deleteCount = 0;
                foreach (var group in documents.GroupBy(e => ((IPartitionedDocument)e).PartitionKey))
                {
                    var groupIdsTodelete = group.Select(e => e.Id).ToArray();
                    deleteCount += (await HandlePartitioned<TDocument, TKey>(group.FirstOrDefault()).DeleteManyAsync(x => groupIdsTodelete.Contains(x.Id))).DeletedCount;
                }
                return deleteCount;
            }
            else
            {
                var idsTodelete = documents.Select(e => e.Id).ToArray();
                return (await HandlePartitioned<TDocument, TKey>(documents.FirstOrDefault()).DeleteManyAsync(x => idsTodelete.Contains(x.Id))).DeletedCount;
            }
        }
        
        public virtual long DeleteMany<TDocument, TKey>(IEnumerable<TDocument> documents)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>
        {
            if (!documents.Any())
            {
                return 0;
            }
            // cannot use typeof(IPartitionedDocument).IsAssignableFrom(typeof(TDocument)), not available in netstandard 1.5
            if (documents.Any(e => e is IPartitionedDocument))
            {
                long deleteCount = 0;
                foreach (var group in documents.GroupBy(e => ((IPartitionedDocument)e).PartitionKey))
                {
                    var groupIdsTodelete = group.Select(e => e.Id).ToArray();
                    deleteCount += (HandlePartitioned<TDocument, TKey>(group.FirstOrDefault()).DeleteMany(x => groupIdsTodelete.Contains(x.Id))).DeletedCount;
                }
                return deleteCount;
            }
            else
            {
                var idsTodelete = documents.Select(e => e.Id).ToArray();
                return (HandlePartitioned<TDocument, TKey>(documents.FirstOrDefault()).DeleteMany(x => idsTodelete.Contains(x.Id))).DeletedCount;
            }
        }

        public virtual long DeleteMany<TDocument, TKey>(Expression<Func<TDocument, bool>> filter, string partitionKey = null)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>
        {
            return HandlePartitioned<TDocument, TKey>(partitionKey).DeleteMany(filter).DeletedCount;
        }

        #endregion

        #region Project
        
        public virtual async Task<TProjection> ProjectOneAsync<TDocument, TProjection>(Expression<Func<TDocument, bool>> filter, Expression<Func<TDocument, TProjection>> projection, string partitionKey = null)
            where TDocument : IDocument
            where TProjection : class
        {
            return await HandlePartitioned<TDocument>(partitionKey).Find(filter)
                                                                   .Project(projection)
                                                                   .FirstOrDefaultAsync();
        }
        
        public virtual async Task<TProjection> ProjectOneAsync<TDocument, TProjection, TKey>(Expression<Func<TDocument, bool>> filter, Expression<Func<TDocument, TProjection>> projection, string partitionKey = null)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>
            where TProjection : class
        {
            return await HandlePartitioned<TDocument, TKey>(partitionKey).Find(filter)
                                                                   .Project(projection)
                                                                   .FirstOrDefaultAsync();
        }
        
        public virtual TProjection ProjectOne<TDocument, TProjection>(Expression<Func<TDocument, bool>> filter, Expression<Func<TDocument, TProjection>> projection, string partitionKey = null)
            where TDocument : IDocument
            where TProjection : class
        {
            return HandlePartitioned<TDocument>(partitionKey).Find(filter)
                                                             .Project(projection)
                                                             .FirstOrDefault();
        }
        
        public virtual TProjection ProjectOne<TDocument, TProjection, TKey>(Expression<Func<TDocument, bool>> filter, Expression<Func<TDocument, TProjection>> projection, string partitionKey = null)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>
            where TProjection : class
        {
            return HandlePartitioned<TDocument, TKey>(partitionKey).Find(filter)
                                                             .Project(projection)
                                                             .FirstOrDefault();
        }
        
        public virtual async Task<List<TProjection>> ProjectManyAsync<TDocument, TProjection>(Expression<Func<TDocument, bool>> filter, Expression<Func<TDocument, TProjection>> projection, string partitionKey = null)
            where TDocument : IDocument
            where TProjection : class
        {
            return await HandlePartitioned<TDocument>(partitionKey).Find(filter)
                                                                   .Project(projection)
                                                                   .ToListAsync();
        }
        
        public virtual async Task<List<TProjection>> ProjectManyAsync<TDocument, TProjection, TKey>(Expression<Func<TDocument, bool>> filter, Expression<Func<TDocument, TProjection>> projection, string partitionKey = null)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>
            where TProjection : class
        {
            return await HandlePartitioned<TDocument, TKey>(partitionKey).Find(filter)
                                                                   .Project(projection)
                                                                   .ToListAsync();
        }
        
        public virtual List<TProjection> ProjectMany<TDocument, TProjection>(Expression<Func<TDocument, bool>> filter, Expression<Func<TDocument, TProjection>> projection, string partitionKey = null)
            where TDocument : IDocument
            where TProjection : class
        {
            return HandlePartitioned<TDocument>(partitionKey).Find(filter)
                                                             .Project(projection)
                                                             .ToList();
        }
        
        public virtual List<TProjection> ProjectMany<TDocument, TProjection, TKey>(Expression<Func<TDocument, bool>> filter, Expression<Func<TDocument, TProjection>> projection, string partitionKey = null)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>
            where TProjection : class
        {
            return HandlePartitioned<TDocument, TKey>(partitionKey).Find(filter)
                                                             .Project(projection)
                                                             .ToList();
        }

        #endregion

        #region Grouping
        
        public virtual List<TProjection> GroupBy<TDocument, TGroupKey, TProjection>(
            Expression<Func<TDocument, TGroupKey>> groupingCriteria,
            Expression<Func<IGrouping<TGroupKey, TDocument>, TProjection>> groupProjection,
            string partitionKey = null)
            where TDocument : IDocument
            where TProjection : class, new()
        {
            var collection = string.IsNullOrEmpty(partitionKey) ? GetCollection<TDocument>() : GetCollection<TDocument>(partitionKey);
            return collection.Aggregate()
                             .Group(groupingCriteria, groupProjection)
                             .ToList();

        }
        
        public virtual List<TProjection> GroupBy<TDocument, TGroupKey, TProjection>(Expression<Func<TDocument, bool>> filter,
                                                       Expression<Func<TDocument, TGroupKey>> selector,
                                                       Expression<Func<IGrouping<TGroupKey, TDocument>, TProjection>> projection,
                                                       string partitionKey = null)
                                                       where TDocument : IDocument
                                                       where TProjection : class, new()
        {
            var collection = string.IsNullOrEmpty(partitionKey) ? GetCollection<TDocument>() : GetCollection<TDocument>(partitionKey);
            return collection.Aggregate()
                             .Match(Builders<TDocument>.Filter.Where(filter))
                             .Group(selector, projection)
                             .ToList();
        }


        #endregion

        #region Pagination

        public virtual async Task<List<TDocument>> GetPaginatedAsync<TDocument>(Expression<Func<TDocument, bool>> filter, int skipNumber = 0, int takeNumber = 50, string partitionKey = null)
            where TDocument : IDocument
        {
            return await HandlePartitioned<TDocument>(partitionKey).Find(filter).Skip(skipNumber).Limit(takeNumber).ToListAsync();
        }
        
        public virtual async Task<List<TDocument>> GetPaginatedAsync<TDocument, TKey>(Expression<Func<TDocument, bool>> filter, int skipNumber = 0, int takeNumber = 50, string partitionKey = null)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>
        {
            return await HandlePartitioned<TDocument, TKey>(partitionKey).Find(filter).Skip(skipNumber).Limit(takeNumber).ToListAsync();
        }

        #endregion

        #region Find And Update
        
        public virtual async Task<TDocument> GetAndUpdateOne<TDocument>(FilterDefinition<TDocument> filter, UpdateDefinition<TDocument> update, FindOneAndUpdateOptions<TDocument, TDocument> options) where TDocument : IDocument
        {
            return await GetCollection<TDocument>().FindOneAndUpdateAsync(filter, update, options);
        }
        
        public virtual async Task<TDocument> GetAndUpdateOne<TDocument, TKey>(FilterDefinition<TDocument> filter, UpdateDefinition<TDocument> update, FindOneAndUpdateOptions<TDocument, TDocument> options)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>
        {
            return await GetCollection<TDocument, TKey>().FindOneAndUpdateAsync(filter, update, options);
        }

        #endregion Find And Update

        #region Document Format

        protected void FormatDocument<TDocument, TKey>(TDocument document)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>
        {
            if (document == null)
            {
                throw new ArgumentNullException(nameof(document));
            }
            var defaultTKey = default(TKey);
            if (document.Id == null
                || (defaultTKey != null
                    && defaultTKey.Equals(document.Id)))
            {
                document.Id = IdGenerator.GetId<TKey>();
            }
        }
        
        protected void FormatDocument<TDocument>(TDocument document) where TDocument : IDocument
        {
            if (document == null)
            {
                throw new ArgumentNullException(nameof(document));
            }
            if (document.Id == default(Guid))
            {
                document.Id = Guid.NewGuid();
            }
        }

        #endregion

        #region Index Management

        /// <inheritdoc />
        public async Task<string> CreateTextIndexAsync<TDocument>(Expression<Func<TDocument, object>> field, IndexCreationOptions indexCreationOptions = null, string partitionKey = null)
            where TDocument : IDocument
        {
            return await CreateTextIndexAsync<TDocument, Guid>(field, indexCreationOptions, partitionKey);
        }

        /// <inheritdoc />
        public async Task<string> CreateTextIndexAsync<TDocument, TKey>(Expression<Func<TDocument, object>> field, IndexCreationOptions indexCreationOptions = null, string partitionKey = null)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>
        {
            return await HandlePartitioned<TDocument, TKey>(partitionKey).Indexes
                                                                   .CreateOneAsync(
                                                                     new CreateIndexModel<TDocument>(
                                                                     Builders<TDocument>.IndexKeys.Text(field),
                                                                     indexCreationOptions == null ? null : MapIndexOptions(indexCreationOptions)
                                                                   ));
        }

        /// <inheritdoc />
        public async Task<string> CreateAscendingIndexAsync<TDocument>(Expression<Func<TDocument, object>> field, IndexCreationOptions indexCreationOptions = null, string partitionKey = null) where TDocument : IDocument
        {
            return await CreateAscendingIndexAsync<TDocument, Guid>(field, indexCreationOptions, partitionKey);
        }

        /// <inheritdoc />
        public async Task<string> CreateAscendingIndexAsync<TDocument, TKey>(Expression<Func<TDocument, object>> field, IndexCreationOptions indexCreationOptions = null, string partitionKey = null)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>
        {
            var collection = HandlePartitioned<TDocument, TKey>(partitionKey);
            var createOptions = indexCreationOptions == null ? null : MapIndexOptions(indexCreationOptions);
            var indexKey = Builders<TDocument>.IndexKeys;
            return await collection.Indexes
                                   .CreateOneAsync(
                new CreateIndexModel<TDocument>(indexKey.Ascending(field), createOptions));
        }

        /// <inheritdoc />
        public async Task<string> CreateDescendingIndexAsync<TDocument>(Expression<Func<TDocument, object>> field, IndexCreationOptions indexCreationOptions = null, string partitionKey = null)
            where TDocument : IDocument
        {
            return await CreateDescendingIndexAsync<TDocument, Guid>(field, indexCreationOptions, partitionKey);
        }

        /// <inheritdoc />
        public async Task<string> CreateDescendingIndexAsync<TDocument, TKey>(Expression<Func<TDocument, object>> field, IndexCreationOptions indexCreationOptions = null, string partitionKey = null)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>
        {
            var collection = HandlePartitioned<TDocument, TKey>(partitionKey);
            var createOptions = indexCreationOptions == null ? null : MapIndexOptions(indexCreationOptions);
            var indexKey = Builders<TDocument>.IndexKeys;
            return await collection.Indexes
                                   .CreateOneAsync(
                new CreateIndexModel<TDocument>(indexKey.Descending(field), createOptions));
        }

        /// <inheritdoc />
        public async Task<string> CreateHashedIndexAsync<TDocument>(Expression<Func<TDocument, object>> field, IndexCreationOptions indexCreationOptions = null, string partitionKey = null)
            where TDocument : IDocument
        {
            return await CreateHashedIndexAsync<TDocument, Guid>(field, indexCreationOptions, partitionKey);
        }

        /// <inheritdoc />
        public async Task<string> CreateHashedIndexAsync<TDocument, TKey>(Expression<Func<TDocument, object>> field, IndexCreationOptions indexCreationOptions = null, string partitionKey = null)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>
        {
            var collection = HandlePartitioned<TDocument, TKey>(partitionKey);
            var createOptions = indexCreationOptions == null ? null : MapIndexOptions(indexCreationOptions);
            var indexKey = Builders<TDocument>.IndexKeys;
            return await collection.Indexes
                                   .CreateOneAsync(
                new CreateIndexModel<TDocument>(indexKey.Hashed(field), createOptions));
        }

        /// <inheritdoc />
        public async Task<string> CreateCombinedTextIndexAsync<TDocument>(IEnumerable<Expression<Func<TDocument, object>>> fields, IndexCreationOptions indexCreationOptions = null, string partitionKey = null) where TDocument : IDocument
        {
            return await CreateCombinedTextIndexAsync<TDocument, Guid>(fields, indexCreationOptions, partitionKey);
        }

        /// <inheritdoc />
        public async Task<string> CreateCombinedTextIndexAsync<TDocument, TKey>(IEnumerable<Expression<Func<TDocument, object>>> fields, IndexCreationOptions indexCreationOptions = null, string partitionKey = null)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>
        {
            var collection = HandlePartitioned<TDocument, TKey>(partitionKey);
            var createOptions = indexCreationOptions == null ? null : MapIndexOptions(indexCreationOptions);
            var listOfDefs = new List<IndexKeysDefinition<TDocument>>();
            foreach (var field in fields)
            {
                listOfDefs.Add(Builders<TDocument>.IndexKeys.Text(field));
            }
            return await collection.Indexes
                                   .CreateOneAsync(new CreateIndexModel<TDocument>(Builders<TDocument>.IndexKeys.Combine(listOfDefs), createOptions));
        }

        /// <inheritdoc />
        public async Task DropIndexAsync<TDocument>(string indexName, string partitionKey = null)
            where TDocument : IDocument
        {
            await DropIndexAsync<TDocument, Guid>(indexName, partitionKey);
        }

        /// <inheritdoc />
        public async Task DropIndexAsync<TDocument, TKey>(string indexName, string partitionKey = null)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>
        {
            await HandlePartitioned<TDocument, TKey>(partitionKey).Indexes.DropOneAsync(indexName);
        }

        /// <inheritdoc />
        public async Task<List<string>> GetIndexesNamesAsync<TDocument>(string partitionKey = null)
            where TDocument : IDocument
        {
            return await GetIndexesNamesAsync<TDocument, Guid>(partitionKey);
        }

        /// <inheritdoc />
        public async Task<List<string>> GetIndexesNamesAsync<TDocument, TKey>(string partitionKey = null)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>
        {
            var indexCursor = await HandlePartitioned<TDocument, TKey>(partitionKey).Indexes.ListAsync();
            var indexes = await indexCursor.ToListAsync();
            return indexes.Select(e => e["name"].ToString()).ToList();
        }


        #endregion Index Management

        #region Private Method

        private CreateIndexOptions MapIndexOptions(IndexCreationOptions indexCreationOptions)
        {
            return new CreateIndexOptions
            {
                Unique = indexCreationOptions.Unique,
                TextIndexVersion = indexCreationOptions.TextIndexVersion,
                SphereIndexVersion = indexCreationOptions.SphereIndexVersion,
                Sparse = indexCreationOptions.Sparse,
                Name = indexCreationOptions.Name,
                Min = indexCreationOptions.Min,
                Max = indexCreationOptions.Max,
                LanguageOverride = indexCreationOptions.LanguageOverride,
                ExpireAfter = indexCreationOptions.ExpireAfter,
                DefaultLanguage = indexCreationOptions.DefaultLanguage,
                BucketSize = indexCreationOptions.BucketSize,
                Bits = indexCreationOptions.Bits,
                Background = indexCreationOptions.Background,
                Version = indexCreationOptions.Version
            };
        }

        #endregion
    }
}