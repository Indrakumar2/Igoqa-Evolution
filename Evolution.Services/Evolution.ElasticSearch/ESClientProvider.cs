using Nest;
using System;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Evolution.ElasticSearch
{
    public class ElasticSearchClientProvider<T> where T : class
    {
        public ElasticSearchClientProvider(IOptions<ElasticSearchOption> options)
        {
            var settings = new ConnectionSettings(new Uri(options.Value.Uri))
                .DefaultIndex(options.Value.DefaultIndex);

            if (!String.IsNullOrEmpty(options.Value.UserName) && !String.IsNullOrEmpty(options.Value.Password))
            {
                settings.BasicAuthentication(options.Value.UserName, options.Value.Password);
            }

            this.Client = new ElasticClient(settings);
            this.DefaultIndex = options.Value.DefaultIndex;
        }

        public ElasticClient Client { get; private set; }

        public string DefaultIndex { get; private set; }

        /// <summary>
        /// This will ensure whether default index name has created or not
        /// </summary>
        /// <param name="indexName"></param>
        /// <param name="customMapping"></param>
        /// <returns></returns>
        public bool EnsureIndexWithMapping(string indexName = null, Func<PutMappingDescriptor<T>, PutMappingDescriptor<T>> customMapping = null)
        {
            try
            {
                if (String.IsNullOrEmpty(indexName))
                    indexName = this.DefaultIndex;

                // Map type T to that index
                this.Client.ConnectionSettings.DefaultIndices.Add(typeof(T), indexName);

                // Does the index exists?
                var indexExistsResponse = this.Client.IndexExists(new IndexExistsRequest(indexName));
                if (!indexExistsResponse.IsValid)
                    throw new InvalidOperationException(indexExistsResponse.DebugInformation);

                // If exists, return
                if (indexExistsResponse.Exists)
                    return false;

                // Otherwise create the index and the type mapping
                var createIndexRes = this.Client.CreateIndex(indexName);
                if (!createIndexRes.IsValid)
                    throw new InvalidOperationException(createIndexRes.DebugInformation);

                var res = this.Client.Map<T>(m =>
                {
                    m.AutoMap().Index(indexName);
                    if (customMapping != null)
                        m = customMapping(m);
                    return m;
                });

                if (!res.IsValid)
                    throw new InvalidOperationException(res.DebugInformation);
            }
            catch(Exception ex)
            {
                //Log Error
                return false;
                throw ex;
            }

            return true;
        }

        /// <summary>
        /// This will create Search Document Index
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<IIndexResponse> CreateDocumentIndex(T model)
        {
            try
            {
                //IIndexResponse res = await this.Client.IndexAsync<T>(model);
                //if (!res.IsValid)
                //    throw new InvalidOperationException(res.DebugInformation);

                //return res;

                return null;
            }
            catch (Exception ex)
            {
                //Log Error
                return null;
                //throw ex;
            }
        }

        /// <summary>
        /// Search the string
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        public async Task<IList<T>> Find(string search)
        {
            try
            {
                var res = await this.Client.SearchAsync<T>(x => x
                    .Query(q => q.
                       SimpleQueryString(qs => qs.Query(search))));
                if (!res.IsValid)
                {
                    throw new InvalidOperationException(res.DebugInformation);
                }
            }
            catch(Exception ex)
            {
                //Log the error
                throw ex;
            }
            return null;
        }
    }
}
