using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Evolution.MongoDb.Model.Interfaces
{
    public interface IBaseMongoRepository : IReadOnlyMongoRepository, IMongoDbCollectionIndexRepository
    {
        #region Create

        Task AddOneAsync<TDocument>(TDocument document) where TDocument : IDocument;

        void AddOne<TDocument>(TDocument document) where TDocument : IDocument;

        Task AddManyAsync<TDocument>(IEnumerable<TDocument> documents) where TDocument : IDocument;

        void AddMany<TDocument>(IEnumerable<TDocument> documents) where TDocument : IDocument;

        #endregion

        #region Create TKey

        Task AddOneAsync<TDocument, TKey>(TDocument document)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>;

        void AddOne<TDocument, TKey>(TDocument document)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>;

        Task AddManyAsync<TDocument, TKey>(IEnumerable<TDocument> documents)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>;

        void AddMany<TDocument, TKey>(IEnumerable<TDocument> documents)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>;

        #endregion

        #region Update

        Task<bool> UpdateOneAsync<TDocument>(TDocument modifiedDocument) where TDocument : IDocument;

        bool UpdateOne<TDocument>(TDocument modifiedDocument) where TDocument : IDocument;

        bool UpdateOne<TDocument, TField>(FilterDefinition<TDocument> filter, Expression<Func<TDocument, TField>> field, TField value, string partitionKey = null)
            where TDocument : IDocument;

        Task<bool> UpdateOneAsync<TDocument, TField>(FilterDefinition<TDocument> filter, Expression<Func<TDocument, TField>> field, TField value, string partitionKey = null)
            where TDocument : IDocument;

        bool UpdateOne<TDocument, TField>(TDocument documentToModify, Expression<Func<TDocument, TField>> field, TField value)
            where TDocument : IDocument;

        Task<bool> UpdateOneAsync<TDocument, TField>(TDocument documentToModify, Expression<Func<TDocument, TField>> field, TField value)
            where TDocument : IDocument;

        bool UpdateOne<TDocument, TField>(Expression<Func<TDocument, bool>> filter, Expression<Func<TDocument, TField>> field, TField value, string partitionKey = null)
            where TDocument : IDocument;

        Task<bool> UpdateOneAsync<TDocument>(TDocument documentToModify, UpdateDefinition<TDocument> update)
            where TDocument : IDocument;

        Task<bool> UpdateOneAsync<TDocument, TField>(Expression<Func<TDocument, bool>> filter, Expression<Func<TDocument, TField>> field, TField value, string partitionKey = null)
            where TDocument : IDocument;

        bool UpdateOne<TDocument>(TDocument documentToModify, UpdateDefinition<TDocument> update)
            where TDocument : IDocument;

        #endregion

        #region Update TKey

        Task<bool> UpdateOneAsync<TDocument, TKey>(TDocument modifiedDocument)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>;

        bool UpdateOne<TDocument, TKey>(TDocument modifiedDocument)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>;

        Task<bool> UpdateOneAsync<TDocument, TKey>(TDocument documentToModify, UpdateDefinition<TDocument> update)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>;

        bool UpdateOne<TDocument, TKey>(TDocument documentToModify, UpdateDefinition<TDocument> update)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>;

        bool UpdateOne<TDocument, TKey, TField>(Expression<Func<TDocument, bool>> filter, Expression<Func<TDocument, TField>> field, TField value, string partitionKey = null)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>;

        Task<bool> UpdateOneAsync<TDocument, TKey, TField>(TDocument documentToModify, Expression<Func<TDocument, TField>> field, TField value)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>;

        Task<bool> UpdateOneAsync<TDocument, TKey, TField>(Expression<Func<TDocument, bool>> filter, Expression<Func<TDocument, TField>> field, TField value, string partitionKey = null)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>;

        bool UpdateOne<TDocument, TKey, TField>(TDocument documentToModify, Expression<Func<TDocument, TField>> field, TField value)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>;

        Task<bool> UpdateOneAsync<TDocument, TKey, TField>(FilterDefinition<TDocument> filter, Expression<Func<TDocument, TField>> field, TField value, string partitionKey = null)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>;

        bool UpdateOne<TDocument, TKey, TField>(FilterDefinition<TDocument> filter, Expression<Func<TDocument, TField>> field, TField value, string partitionKey = null)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>;

        #endregion

        #region Delete

        Task<long> DeleteOneAsync<TDocument>(TDocument document) where TDocument : IDocument;

        Task<long> DeleteOneAsync<TDocument>(Expression<Func<TDocument, bool>> filter, string partitionKey = null) where TDocument : IDocument;

        long DeleteOne<TDocument>(TDocument document) where TDocument : IDocument;

        long DeleteOne<TDocument>(Expression<Func<TDocument, bool>> filter, string partitionKey = null) where TDocument : IDocument;

        Task<long> DeleteManyAsync<TDocument>(Expression<Func<TDocument, bool>> filter, string partitionKey = null) where TDocument : IDocument;

        Task<long> DeleteManyAsync<TDocument>(IEnumerable<TDocument> documents) where TDocument : IDocument;

        long DeleteMany<TDocument>(IEnumerable<TDocument> documents) where TDocument : IDocument;

        long DeleteMany<TDocument>(Expression<Func<TDocument, bool>> filter, string partitionKey = null) where TDocument : IDocument;

        #endregion

        #region Delete TKey

        long DeleteOne<TDocument, TKey>(TDocument document)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>;

        Task<long> DeleteOneAsync<TDocument, TKey>(TDocument document)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>;

        long DeleteOne<TDocument, TKey>(Expression<Func<TDocument, bool>> filter, string partitionKey = null)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>;

        Task<long> DeleteOneAsync<TDocument, TKey>(Expression<Func<TDocument, bool>> filter, string partitionKey = null)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>;

        Task<long> DeleteManyAsync<TDocument, TKey>(Expression<Func<TDocument, bool>> filter, string partitionKey = null)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>;

        Task<long> DeleteManyAsync<TDocument, TKey>(IEnumerable<TDocument> documents)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>;

        long DeleteMany<TDocument, TKey>(IEnumerable<TDocument> documents)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>;

        long DeleteMany<TDocument, TKey>(Expression<Func<TDocument, bool>> filter, string partitionKey = null)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>;

        #endregion

        #region Project

        Task<TProjection> ProjectOneAsync<TDocument, TProjection>(Expression<Func<TDocument, bool>> filter, Expression<Func<TDocument, TProjection>> projection, string partitionKey = null)
                                        where TDocument : IDocument
                                        where TProjection : class;

        Task<TProjection> ProjectOneAsync<TDocument, TProjection, TKey>(Expression<Func<TDocument, bool>> filter, Expression<Func<TDocument, TProjection>> projection, string partitionKey = null)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>
            where TProjection : class;

        TProjection ProjectOne<TDocument, TProjection>(Expression<Func<TDocument, bool>> filter, Expression<Func<TDocument, TProjection>> projection, string partitionKey = null)
                                        where TDocument : IDocument
                                        where TProjection : class;

        TProjection ProjectOne<TDocument, TProjection, TKey>(Expression<Func<TDocument, bool>> filter, Expression<Func<TDocument, TProjection>> projection, string partitionKey = null)
                                        where TDocument : IDocument<TKey>
                                        where TKey : IEquatable<TKey>
                                        where TProjection : class;

        Task<List<TProjection>> ProjectManyAsync<TDocument, TProjection>(Expression<Func<TDocument, bool>> filter, Expression<Func<TDocument, TProjection>> projection, string partitionKey = null)
                                        where TDocument : IDocument
                                        where TProjection : class;

        Task<List<TProjection>> ProjectManyAsync<TDocument, TProjection, TKey>(Expression<Func<TDocument, bool>> filter, Expression<Func<TDocument, TProjection>> projection, string partitionKey = null)
                                        where TDocument : IDocument<TKey>
                                        where TKey : IEquatable<TKey>
                                        where TProjection : class;

        List<TProjection> ProjectMany<TDocument, TProjection>(Expression<Func<TDocument, bool>> filter, Expression<Func<TDocument, TProjection>> projection, string partitionKey = null)
                                        where TDocument : IDocument
                                        where TProjection : class;

        List<TProjection> ProjectMany<TDocument, TProjection, TKey>(Expression<Func<TDocument, bool>> filter, Expression<Func<TDocument, TProjection>> projection, string partitionKey = null)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>
            where TProjection : class;

        #endregion

        #region Pagination

        Task<List<TDocument>> GetPaginatedAsync<TDocument>(Expression<Func<TDocument, bool>> filter, int skipNumber = 0, int takeNumber = 50, string partitionKey = null)
            where TDocument : IDocument;

        Task<List<TDocument>> GetPaginatedAsync<TDocument, TKey>(Expression<Func<TDocument, bool>> filter, int skipNumber = 0, int takeNumber = 50, string partitionKey = null)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>;

        #endregion

        #region Get And Update

        Task<TDocument> GetAndUpdateOne<TDocument>(FilterDefinition<TDocument> filter, UpdateDefinition<TDocument> update, FindOneAndUpdateOptions<TDocument, TDocument> options)
            where TDocument : IDocument;

        Task<TDocument> GetAndUpdateOne<TDocument, TKey>(FilterDefinition<TDocument> filter, UpdateDefinition<TDocument> update, FindOneAndUpdateOptions<TDocument, TDocument> options)
                                        where TDocument : IDocument<TKey>
                                        where TKey : IEquatable<TKey>;

        #endregion

        #region Grouping

        List<TProjection> GroupBy<TDocument, TGroupKey, TProjection>(
            Expression<Func<TDocument, TGroupKey>> groupingCriteria,
            Expression<Func<IGrouping<TGroupKey, TDocument>, TProjection>> groupProjection,
            string partitionKey = null)
            where TDocument : IDocument
            where TProjection : class, new();

        List<TProjection> GroupBy<TDocument, TGroupKey, TProjection>(Expression<Func<TDocument, bool>> filter,
                                                       Expression<Func<TDocument, TGroupKey>> selector,
                                                       Expression<Func<IGrouping<TGroupKey, TDocument>, TProjection>> projection,
                                                       string partitionKey = null)
                                                       where TDocument : IDocument
                                                       where TProjection : class, new();

        #endregion
    }
}
