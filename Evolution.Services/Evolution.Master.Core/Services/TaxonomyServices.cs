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
using System.Linq.Expressions;
using System.Reflection;
using System.Transactions;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;

namespace Evolution.Master.Core.Services
{
    public class TaxonomyServices : ITaxonomyServices
    {
        private readonly IAppLogger<TaxonomyServices> _logger = null;
        private readonly ITaxonomyServiceRepository _repository = null;
        private readonly IMapper _mapper = null;
        private readonly JObject _messages = null;

        public TaxonomyServices(IMapper mapper, IAppLogger<TaxonomyServices> logger, ITaxonomyServiceRepository repository)
        {
            this._logger = logger;
            this._repository = repository;
            this._mapper = mapper;
            this._messages = JObject.Parse(File.ReadAllText(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"Message_EN.json")));
        }

        public bool IsValidServiceName(IList<KeyValuePair<string, string>> SubCategoryAndService, ref IList<DbModel.TaxonomyService> dbServices, ref IList<ValidationMessage> validMessages)
        {
            var messages = new List<ValidationMessage>();
            if (SubCategoryAndService?.Count() > 0)
            {
                if (dbServices == null)
                {
                    var subCats = SubCategoryAndService.Select(x => x.Key).ToList();
                    var Serv = SubCategoryAndService.Select(x => x.Value).ToList();
                    dbServices = _repository.FindBy(x => subCats.Contains(x.TaxonomySubCategory.TaxonomySubCategoryName) && Serv.Contains(x.TaxonomyServiceName)).ToList();
                }

                IList<DbModel.TaxonomyService> dbDatas = dbServices;
                var serviceNotExists = SubCategoryAndService?.Where(x => !dbDatas.Any(x2 => x2.TaxonomySubCategory.TaxonomySubCategoryName == x.Key &&
                                                                                            x2.TaxonomyServiceName == x.Value))?.ToList();
                serviceNotExists?.ForEach(x =>
                {
                    messages.Add(_messages, x, MessageType.InvalidSubCategory, x);
                });
                validMessages = messages;
            }
            return messages?.Count <= 0;
        }

        public Response Search(TaxonomyService search)
        {
            IList<TaxonomyService> result = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                if (search == null)
                    search = new TaxonomyService();

                using (var tranScope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))                
                {
                    result= _repository.FindBy(x =>
                                                (search.TaxonomyCategoryId == 0 || x.TaxonomySubCategory.TaxonomyCategoryId == search.TaxonomyCategoryId)
                                                && (search.TaxonomySubCategoryId == 0 || x.TaxonomySubCategoryId == search.TaxonomySubCategoryId)
                                                && (string.IsNullOrEmpty(search.Name) || x.TaxonomyServiceName == search.Name)
                                                && (string.IsNullOrEmpty(search.TaxonomySubCategory) || x.TaxonomySubCategory.TaxonomySubCategoryName == search.TaxonomySubCategory)
                                                && (string.IsNullOrEmpty(search.TaxonomyCategory) || x.TaxonomySubCategory.TaxonomyCategory.Name == search.TaxonomyCategory ))
                                        .Select(x => new TaxonomyService() { Id = x.Id, TaxonomySubCategory = x.TaxonomySubCategory.TaxonomySubCategoryName, TaxonomyServiceName = x.TaxonomyServiceName }).OrderBy(x => x.TaxonomyServiceName).ToList();
                    tranScope.Complete();                
                }
                 //= _repository.FindBy(null)
                 //                       .Where(x => (string.IsNullOrEmpty(search.Name) || x.TaxonomyServiceName == search.Name)
                 //                              && (string.IsNullOrEmpty(search.TaxonomySubCategory) || x.TaxonomySubCategory.TaxonomySubCategoryName == search.TaxonomySubCategory))
                 //                       .Select(x => new TaxonomyService() { TaxonomySubCategory = x.TaxonomySubCategory.TaxonomySubCategoryName, TaxonomyServiceName = x.TaxonomyServiceName }).OrderBy(x => x.TaxonomyServiceName).ToList();

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



        public bool IsValidTaxonomyService(IList<string> taxonomyServiceNames, 
                                          ref IList<DbModel.TaxonomyService> dbTaxonomyServiceNames,
                                          ref IList<ValidationMessage> valdMessages,
                                          params Expression<Func<DbModel.TaxonomyService, object>>[] includes)
        {
            var messages = new List<ValidationMessage>();
            if (taxonomyServiceNames?.Count() > 0 && dbTaxonomyServiceNames == null)
            {
                if (dbTaxonomyServiceNames == null && taxonomyServiceNames.Count > 0)
                    dbTaxonomyServiceNames = _repository.FindBy(x => taxonomyServiceNames.Contains(x.TaxonomyServiceName), includes).ToList();

                IList<DbModel.TaxonomyService> dbDatas = dbTaxonomyServiceNames;
                var countryNotExists = taxonomyServiceNames.Where(x => !dbDatas.Any(x2 => x2.TaxonomyServiceName == x))?.ToList();
                countryNotExists?.ForEach(x =>
                {
                    messages.Add(_messages, x, MessageType.InvalidTaxonomyService, x);
                });
                valdMessages = messages;
            }
            return dbTaxonomyServiceNames != null ? true : false;
        }
    }

}


    

