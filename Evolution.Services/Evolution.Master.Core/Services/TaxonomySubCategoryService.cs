using AutoMapper;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Common.Models.Messages;
using Evolution.Common.Models.Responses;
using Evolution.DbRepository.Interfaces.Master;
using Evolution.Logging.Interfaces;
using Evolution.Master.Domain.Interfaces.Services;
using Evolution.Master.Domain.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Transactions;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;

namespace Evolution.Master.Core.Services
{
    public class TaxonomySubCategoryService : ITaxonomySubCategoryService
    {
        private readonly IAppLogger<TaxonomySubCategoryService> _logger = null;
        private readonly ITaxonomySubCategoryRepository _repository = null;
        private readonly IMapper _mapper = null;
        private readonly JObject _messages = null;

        public TaxonomySubCategoryService(IMapper mapper, IAppLogger<TaxonomySubCategoryService> logger, ITaxonomySubCategoryRepository repository)
        {
            this._logger = logger;
            this._repository = repository;
            this._mapper = mapper;
            this._messages = JObject.Parse(File.ReadAllText(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"Message_EN.json")));
        }

        public bool IsValidSubCategoryName(IList<KeyValuePair<string, string>> catAndSubCategory, ref IList<DbModel.TaxonomySubCategory> dbSubCategories, ref IList<ValidationMessage> valdMessages)
        {
            var messages = new List<ValidationMessage>();
            if (catAndSubCategory?.Count() > 0)
            {
                if (dbSubCategories == null)
                {
                    var cats = catAndSubCategory.Select(x => x.Key).ToList();
                    var subCats = catAndSubCategory.Select(x => x.Value).ToList();
                    dbSubCategories = _repository.FindBy(x => cats.Contains(x.TaxonomyCategory.Name) && subCats.Contains(x.TaxonomySubCategoryName)).ToList();
                }

                IList <DbModel.TaxonomySubCategory> dbDatas = dbSubCategories;
                var subCategoriesNotExists = catAndSubCategory?.Where(x => !dbDatas.Any(x2 => x2.TaxonomyCategory.Name == x.Key && 
                                                                                            x2.TaxonomySubCategoryName==x.Value))?.ToList();
                subCategoriesNotExists?.ForEach(x =>
                {
                    messages.Add(_messages, x, MessageType.InvalidSubCategory, x);
                });
                valdMessages = messages;
            }
            return messages?.Count<=0;
        }

        public Response Search(TaxonomySubCategory search)
        {
            IList<TaxonomySubCategory> result = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                if (search == null)
                    search = new TaxonomySubCategory();

                using (var tranScope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))                
                {
                    result = _repository.FindBy(x =>
                                                    (search.TaxonomyCategoryId == 0 || x.TaxonomyCategoryId == search.TaxonomyCategoryId)
                                                    && (string.IsNullOrEmpty(search.Name) || x.TaxonomySubCategoryName == search.Name)
                                                    && (string.IsNullOrEmpty(search.TaxonomyCategory) || x.TaxonomyCategory.Name == search.TaxonomyCategory))
                                            .Select(x => new TaxonomySubCategory() {Id=x.Id, TaxonomySubCategoryName = x.TaxonomySubCategoryName, TaxonomyCategory = x.TaxonomyCategory.Name }).OrderBy(x => x.TaxonomySubCategoryName).ToList();
                    tranScope.Complete();
                }
            }
            catch (SqlException sqlE)
            {
                responseType = ResponseType.DbException;
                this._logger.LogError(responseType.ToId(), this._messages[responseType.ToId()].ToString(), sqlE);
            }
            catch (Exception ex)
            {
                responseType = ResponseType.Exception;
                this._logger.LogError(responseType.ToId(), this._messages[responseType.ToId()].ToString(), ex);
            }

            return new Response().ToPopulate(responseType, result, result?.Count);
        }
    }

}