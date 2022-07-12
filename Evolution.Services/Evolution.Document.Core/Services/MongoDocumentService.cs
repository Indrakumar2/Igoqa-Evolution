using AutoMapper;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Document.Domain.Interfaces.Data;
using Evolution.Document.Domain.Interfaces.Documents;
using Evolution.Document.Domain.Models.Document;
using Evolution.Logging.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Evolution.Document.Core.Services
{
    public class MongoDocumentService : IMongoDocumentService
    {
        private readonly IMongoDocumentRepository _mongoDocumentRepository = null;
        private readonly IMapper _mapper = null;
        private readonly IAppLogger<MongoDocumentService> _logger = null;

        public MongoDocumentService(IMapper mapper,
                                    IAppLogger<MongoDocumentService> logger,
                                    IMongoDocumentRepository mongoDocumentRepository)
        {
            _mapper = mapper;
            _logger = logger;
            _mongoDocumentRepository = mongoDocumentRepository;
        }

        public async Task<EvolutionMongoDocument> AddAsync(EvolutionMongoDocument model)
        {
            EvolutionMongoDocument result = null;
            try
            {
                await _mongoDocumentRepository.AddOneAsync<EvolutionMongoDocument>(this._mapper.Map<EvolutionMongoDocument>(model));

                var addedDocument = await _mongoDocumentRepository.GetOneAsync<EvolutionMongoDocument>(f => f.UniqueName == model.UniqueName);

                result = this._mapper.Map<EvolutionMongoDocument>(addedDocument);
            }
            catch (Exception ex)
            {
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), model);
                throw;
            }

            return result;
        }

        public EvolutionMongoDocument Add(EvolutionMongoDocument model)
        {
            EvolutionMongoDocument result = null;
            try
            {
                _mongoDocumentRepository.AddOne<EvolutionMongoDocument>(this._mapper.Map<EvolutionMongoDocument>(model));

                var addedDocument = _mongoDocumentRepository.GetOne<EvolutionMongoDocument>(f => f.UniqueName == model.UniqueName);

                result = this._mapper.Map<EvolutionMongoDocument>(addedDocument);
            }
            catch (Exception ex)
            {
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), model);
                throw;
            }

            return result;
        }

        public async Task<EvolutionMongoDocument> ModifyAsync(EvolutionMongoDocument model)
        {
            EvolutionMongoDocument result = null;
            try
            {
                await _mongoDocumentRepository.UpdateOneAsync<EvolutionMongoDocument>(this._mapper.Map<EvolutionMongoDocument>(model));

                var updatedDocument = await _mongoDocumentRepository.GetOneAsync<EvolutionMongoDocument>(f => f.Id == model.Id);

                result = this._mapper.Map<EvolutionMongoDocument>(updatedDocument);
            }
            catch (Exception ex)
            {
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), model);
                throw;
            }

            return result;
        }

        public EvolutionMongoDocument Modify(EvolutionMongoDocument model)
        {
            EvolutionMongoDocument result = null;
            try
            {
                _mongoDocumentRepository.UpdateOne<EvolutionMongoDocument>(this._mapper.Map<EvolutionMongoDocument>(model));
                var updatedDocument = _mongoDocumentRepository.GetOne<EvolutionMongoDocument>(f => f.Id == model.Id);
                result = this._mapper.Map<EvolutionMongoDocument>(updatedDocument);
            }
            catch (Exception ex)
            {
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), model);
                throw;
            }

            return result;
        }

        public async Task<bool> RemoveAsync(EvolutionMongoDocument model)
        {
            bool result = false;
            try
            {
                await _mongoDocumentRepository.DeleteOneAsync<EvolutionMongoDocument>(this._mapper.Map<EvolutionMongoDocument>(model));
                result = true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), model);
                throw;
            }

            return result;
        }

        public bool Remove(EvolutionMongoDocument model)
        {
            bool result = false;
            try
            {
                _mongoDocumentRepository.DeleteOne<EvolutionMongoDocument>(this._mapper.Map<EvolutionMongoDocument>(model));
                result = true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), model);
                throw;
            }

            return result;
        }

        public bool RemoveByUniqueName(List<string> uniqueNames)
        {
            bool result = false;
            try
            {
                if (uniqueNames?.Count > 0)
                    _mongoDocumentRepository.DeleteManyAsync<EvolutionMongoDocument>(x => uniqueNames.Contains(x.UniqueName));
                result = true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), uniqueNames);
                throw;
            }

            return result;
        }

        public async Task<bool> RemoveByUniqueNameAsync(List<string> uniqueNames)
        {
            bool result = false;
            try
            {
                if (uniqueNames?.Count > 0)
                {
                    result = await _mongoDocumentRepository.DeleteManyAsync<EvolutionMongoDocument>(x => uniqueNames.Contains(x.UniqueName)) > 0;
                }  
            }
            catch (Exception ex)
            {
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), uniqueNames);
                throw;
            }

            return result;
        }

        public async Task<IList<EvolutionMongoDocument>> SearchAsync(EvolutionMongoDocumentSearch search)
        {
            IList<EvolutionMongoDocument> result = null;
            try
            {
                var searchModel = this._mapper.Map<EvolutionMongoDocument>(search);
                var excludeExprProps = new List<string>()
                {
                    nameof(searchModel.Id),
                    nameof(searchModel.AddedAtUtc),
                    nameof(searchModel.Version)
                };
                var searchExpr = searchModel.ToMongoFilterDefinition<EvolutionMongoDocument>(excludeExprProps, "Text",textSearchLanguage:"EN");// Here "Text" is property with search index in MONGO DB
                var searchResponse = await _mongoDocumentRepository.GetAllAsync<EvolutionMongoDocument>(searchExpr);
                result = _mapper.Map<IList<EvolutionMongoDocument>>(searchResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), search);
            }

            return result;
        }

        public async Task<IList<string>> SearchDistinctFieldAsync(EvolutionMongoDocumentSearch search, string mongoFieldName = "ReferenceCode")
        {
            IList<string> result = null;
            try
            {
                var searchModel = this._mapper.Map<EvolutionMongoDocument>(search);
                var excludeExprProps = new List<string>()
                {
                    nameof(searchModel.Id),
                    nameof(searchModel.AddedAtUtc),
                    nameof(searchModel.Version)
                };
                var searchExpr = searchModel.ToMongoFilterDefinition<EvolutionMongoDocument>(excludeExprProps, "Text", textSearchLanguage: "EN");// Here "Text" is property with search index in MONGO DB;
                MongoDB.Driver.FieldDefinition<EvolutionMongoDocument, string> field = mongoFieldName;
                result = await _mongoDocumentRepository.GetDistinctAsync<EvolutionMongoDocument, string>(field, searchExpr);
            }
            catch (Exception ex)
            {
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), search);
            }
            return result?.Where(i => i != null)?.ToList(); 
        }

        public IList<EvolutionMongoDocument> Search(EvolutionMongoDocumentSearch search)
        {
            IList<EvolutionMongoDocument> result = null;
            try
            {
                var searchModel = this._mapper.Map<EvolutionMongoDocument>(search);
                var excludeProps = new List<string>() { nameof(search.Id), nameof(search.AddedAtUtc), nameof(search.Version) };
                var searchResponse = _mongoDocumentRepository.GetAll<EvolutionMongoDocument>(searchModel.ToExpression<EvolutionMongoDocument>(excludeProps));

                result = _mapper.Map<IList<EvolutionMongoDocument>>(searchResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), search);
            }

            return result;
        }

        public bool Any(string documentUniquename)
        {
            bool result = false;
            try
            {
                result = _mongoDocumentRepository.Any<EvolutionMongoDocument>(doc => doc.UniqueName == documentUniquename);

            }
            catch (MongoDB.Driver.MongoConnectionException ex )
            {
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), documentUniquename);
                throw new Exception("Problem in connecting to MongoDB instance.");
            } 
            catch(TimeoutException ex)
            {
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), documentUniquename);
                throw new Exception("Problem in connecting to MongoDB instance.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), documentUniquename);
                throw;
            }

            return result;
        }


    }
}