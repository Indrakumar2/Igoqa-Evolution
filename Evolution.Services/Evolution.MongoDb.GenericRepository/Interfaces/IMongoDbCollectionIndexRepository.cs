//using Evolution.MongoDb.GenericRepository.Models;
//using System;
//using System.Collections.Generic;
//using System.Linq.Expressions;
//using System.Text;
//using System.Threading.Tasks;

//namespace Evolution.MongoDb.GenericRepository.Interfaces
//{
//    public interface IMongoDbCollectionIndexRepository
//    {
//        Task<string> CreateTextIndexAsync<TDocument>(Expression<Func<TDocument, object>> field,
//                                                    IndexCreationOptions indexCreationOptions = null,
//                                                    string partitionKey = null)
//            where TDocument : IDocument;

//        Task<string> CreateTextIndexAsync<TDocument, TKey>(Expression<Func<TDocument, object>> field,
//                                                            IndexCreationOptions indexCreationOptions = null,
//                                                            string partitionKey = null)
//            where TDocument : IDocument<TKey>
//            where TKey : IEquatable<TKey>;

//        Task<string> CreateAscendingIndexAsync<TDocument>(Expression<Func<TDocument, object>> field,
//                                                            IndexCreationOptions indexCreationOptions = null,
//                                                            string partitionKey = null)
//            where TDocument : IDocument;


//        Task<string> CreateAscendingIndexAsync<TDocument, TKey>(Expression<Func<TDocument, object>> field,
//                                                                IndexCreationOptions indexCreationOptions = null,
//                                                                string partitionKey = null)
//            where TDocument : IDocument<TKey>
//            where TKey : IEquatable<TKey>;

//        Task<string> CreateDescendingIndexAsync<TDocument>(Expression<Func<TDocument, object>> field,
//                                                            IndexCreationOptions indexCreationOptions = null,
//                                                            string partitionKey = null)
//            where TDocument : IDocument;

//        Task<string> CreateDescendingIndexAsync<TDocument, TKey>(Expression<Func<TDocument, object>> field,
//                                                                IndexCreationOptions indexCreationOptions = null,
//                                                                string partitionKey = null)
//            where TDocument : IDocument<TKey>
//            where TKey : IEquatable<TKey>;

//        Task<string> CreateHashedIndexAsync<TDocument>(Expression<Func<TDocument, object>> field,
//                                                        IndexCreationOptions indexCreationOptions = null,
//                                                        string partitionKey = null)
//            where TDocument : IDocument;

//        Task<string> CreateHashedIndexAsync<TDocument, TKey>(Expression<Func<TDocument, object>> field,
//                                                                IndexCreationOptions indexCreationOptions = null,
//                                                                string partitionKey = null)
//            where TDocument : IDocument<TKey>
//            where TKey : IEquatable<TKey>;

//        Task<string> CreateCombinedTextIndexAsync<TDocument>(IEnumerable<Expression<Func<TDocument, object>>> fields,
//                                                                IndexCreationOptions indexCreationOptions = null,
//                                                                string partitionKey = null)
//            where TDocument : IDocument;

//        Task<string> CreateCombinedTextIndexAsync<TDocument, TKey>(IEnumerable<Expression<Func<TDocument, object>>> fields,
//                                                                    IndexCreationOptions indexCreationOptions = null,
//                                                                    string partitionKey = null)
//            where TDocument : IDocument<TKey>
//            where TKey : IEquatable<TKey>;

//        Task DropIndexAsync<TDocument>(string indexName, string partitionKey = null) where TDocument : IDocument;

//        Task DropIndexAsync<TDocument, TKey>(string indexName, string partitionKey = null)
//            where TDocument : IDocument<TKey>
//            where TKey : IEquatable<TKey>;

//        Task<List<string>> GetIndexesNamesAsync<TDocument>(string partitionKey = null) where TDocument : IDocument;

//        Task<List<string>> GetIndexesNamesAsync<TDocument, TKey>(string partitionKey = null)
//            where TDocument : IDocument<TKey>
//            where TKey : IEquatable<TKey>;
//    }
//}