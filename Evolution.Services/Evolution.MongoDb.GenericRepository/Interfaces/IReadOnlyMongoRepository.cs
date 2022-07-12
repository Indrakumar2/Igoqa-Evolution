//using Evolution.MongoDb.GenericRepository.Models;
//using MongoDB.Driver;
//using System;
//using System.Collections.Generic;
//using System.Linq.Expressions;
//using System.Text;
//using System.Threading.Tasks;

//namespace Evolution.MongoDb.GenericRepository.Interfaces
//{
//    public interface IReadOnlyMongoRepository
//    {
//        string ConnectionString { get; set; }

//        string DatabaseName { get; set; }

//        #region Read

//        Task<TDocument> GetByIdAsync<TDocument>(Guid id, string partitionKey = null) 
//            where TDocument : IDocument;

//        TDocument GetById<TDocument>(Guid id, string partitionKey = null) 
//            where TDocument : IDocument;

//        Task<TDocument> GetOneAsync<TDocument>(Expression<Func<TDocument, bool>> filter, string partitionKey = null) 
//            where TDocument : IDocument;

//        TDocument GetOne<TDocument>(Expression<Func<TDocument, bool>> filter, string partitionKey = null) 
//            where TDocument : IDocument;

//        IFindFluent<TDocument, TDocument> GetCursor<TDocument>(Expression<Func<TDocument, bool>> filter, string partitionKey = null) 
//            where TDocument : IDocument;

//        Task<bool> AnyAsync<TDocument>(Expression<Func<TDocument, bool>> filter, string partitionKey = null) 
//            where TDocument : IDocument;

//        bool Any<TDocument>(Expression<Func<TDocument, bool>> filter, string partitionKey = null) 
//            where TDocument : IDocument;

//        Task<List<TDocument>> GetAllAsync<TDocument>(Expression<Func<TDocument, bool>> filter, string partitionKey = null) 
//            where TDocument : IDocument;

//        List<TDocument> GetAll<TDocument>(Expression<Func<TDocument, bool>> filter, string partitionKey = null) 
//            where TDocument : IDocument;

//        Task<long> CountAsync<TDocument>(Expression<Func<TDocument, bool>> filter, string partitionKey = null) 
//            where TDocument : IDocument;

//        long Count<TDocument>(Expression<Func<TDocument, bool>> filter,string partitionKey = null) 
//            where TDocument : IDocument;

//        #endregion

//        #region Read TKey

//        Task<TDocument> GetByIdAsync<TDocument, TKey>(TKey id, string partitionKey = null)
//            where TDocument : IDocument<TKey>
//            where TKey : IEquatable<TKey>;

//        TDocument GetById<TDocument, TKey>(TKey id, string partitionKey = null)
//            where TDocument : IDocument<TKey>
//            where TKey : IEquatable<TKey>;

//        Task<TDocument> GetOneAsync<TDocument, TKey>(Expression<Func<TDocument, bool>> filter, string partitionKey = null)
//            where TDocument : IDocument<TKey>
//            where TKey : IEquatable<TKey>;

//        TDocument GetOne<TDocument, TKey>(Expression<Func<TDocument, bool>> filter, string partitionKey = null)
//            where TDocument : IDocument<TKey>
//            where TKey : IEquatable<TKey>;

//        IFindFluent<TDocument, TDocument> GetCursor<TDocument, TKey>(Expression<Func<TDocument, bool>> filter, string partitionKey = null)
//            where TDocument : IDocument<TKey>
//            where TKey : IEquatable<TKey>;

//        Task<bool> AnyAsync<TDocument, TKey>(Expression<Func<TDocument, bool>> filter, string partitionKey = null)
//            where TDocument : IDocument<TKey>
//            where TKey : IEquatable<TKey>;

//        bool Any<TDocument, TKey>(Expression<Func<TDocument, bool>> filter, string partitionKey = null)
//            where TDocument : IDocument<TKey>
//            where TKey : IEquatable<TKey>;

//        Task<List<TDocument>> GetAllAsync<TDocument, TKey>(Expression<Func<TDocument, bool>> filter, string partitionKey = null)
//            where TDocument : IDocument<TKey>
//            where TKey : IEquatable<TKey>;

//        List<TDocument> GetAll<TDocument, TKey>(Expression<Func<TDocument, bool>> filter, string partitionKey = null)
//            where TDocument : IDocument<TKey>
//            where TKey : IEquatable<TKey>;

//        Task<long> CountAsync<TDocument, TKey>(Expression<Func<TDocument, bool>> filter, string partitionKey = null)
//            where TDocument : IDocument<TKey>
//            where TKey : IEquatable<TKey>;

//        long Count<TDocument, TKey>(Expression<Func<TDocument, bool>> filter, string partitionKey = null)
//            where TDocument : IDocument<TKey>
//            where TKey : IEquatable<TKey>;

//        #endregion

//        #region Min / Max

//        Task<TDocument> GetByMaxAsync<TDocument>(Expression<Func<TDocument, bool>> filter, Expression<Func<TDocument, object>> orderByDescending, string partitionKey = null)
//            where TDocument : IDocument;

//        TDocument GetByMax<TDocument>(Expression<Func<TDocument, bool>> filter, Expression<Func<TDocument, object>> orderByDescending, string partitionKey = null)
//            where TDocument : IDocument;

//        Task<TDocument> GetByMinAsync<TDocument>(Expression<Func<TDocument, bool>> filter, Expression<Func<TDocument, object>> orderByAscending, string partitionKey = null)
//            where TDocument : IDocument;

//        TDocument GetByMin<TDocument>(Expression<Func<TDocument, bool>> filter, Expression<Func<TDocument, object>> orderByAscending, string partitionKey = null)
//            where TDocument : IDocument;

//        Task<TDocument> GetByMaxAsync<TDocument, TKey>(Expression<Func<TDocument, bool>> filter, Expression<Func<TDocument, object>> orderByDescending, string partitionKey = null)
//            where TDocument : IDocument<TKey>
//            where TKey : IEquatable<TKey>;

//        TDocument GetByMax<TDocument, TKey>(Expression<Func<TDocument, bool>> filter, Expression<Func<TDocument, object>> orderByDescending, string partitionKey = null)
//            where TDocument : IDocument<TKey>
//            where TKey : IEquatable<TKey>;

//        Task<TDocument> GetByMinAsync<TDocument, TKey>(Expression<Func<TDocument, bool>> filter, Expression<Func<TDocument, object>> orderByAscending, string partitionKey = null)
//            where TDocument : IDocument<TKey>
//            where TKey : IEquatable<TKey>;

//        TDocument GetByMin<TDocument, TKey>(Expression<Func<TDocument, bool>> filter, Expression<Func<TDocument, object>> orderByAscending, string partitionKey = null)
//            where TDocument : IDocument<TKey>
//            where TKey : IEquatable<TKey>;

//        Task<TValue> GetMaxValueAsync<TDocument, TValue>(Expression<Func<TDocument, bool>> filter, Expression<Func<TDocument, TValue>> maxValueSelector, string partitionKey = null)
//            where TDocument : IDocument;

//        Task<TValue> GetMaxValueAsync<TDocument, TKey, TValue>(Expression<Func<TDocument, bool>> filter, Expression<Func<TDocument, TValue>> maxValueSelector, string partitionKey = null)
//            where TDocument : IDocument<TKey>
//            where TKey : IEquatable<TKey>;

//        TValue GetMaxValue<TDocument, TValue>(Expression<Func<TDocument, bool>> filter, Expression<Func<TDocument, TValue>> maxValueSelector, string partitionKey = null)
//            where TDocument : IDocument;

//        TValue GetMaxValue<TDocument, TKey, TValue>(Expression<Func<TDocument, bool>> filter, Expression<Func<TDocument, TValue>> orderByDescending, string partitionKey = null)
//            where TDocument : IDocument<TKey>
//            where TKey : IEquatable<TKey>;

//        Task<TValue> GetMinValueAsync<TDocument, TValue>(Expression<Func<TDocument, bool>> filter, Expression<Func<TDocument, TValue>> minValueSelector, string partitionKey = null)
//            where TDocument : IDocument;

//        Task<TValue> GetMinValueAsync<TDocument, TKey, TValue>(Expression<Func<TDocument, bool>> filter, Expression<Func<TDocument, TValue>> minValueSelector, string partitionKey = null)
//            where TDocument : IDocument<TKey>
//            where TKey : IEquatable<TKey>;

//        TValue GetMinValue<TDocument, TValue>(Expression<Func<TDocument, bool>> filter, Expression<Func<TDocument, TValue>> minValueSelector, string partitionKey = null)
//            where TDocument : IDocument;

//        TValue GetMinValue<TDocument, TKey, TValue>(Expression<Func<TDocument, bool>> filter, Expression<Func<TDocument, TValue>> minValueSelector, string partitionKey = null)
//            where TDocument : IDocument<TKey>
//            where TKey : IEquatable<TKey>;

//        #endregion
//    }
//}
