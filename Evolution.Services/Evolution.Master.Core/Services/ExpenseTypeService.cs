using AutoMapper;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Common.Models.Base;
using Evolution.Common.Models.Messages;
using Evolution.Common.Models.Responses;
using Evolution.DbRepository.Models.SqlDatabaseContext;
using Evolution.Logging.Interfaces;
using Evolution.Master.Domain.Interfaces.Data;
using Evolution.Master.Domain.Interfaces.Services;
using Evolution.Master.Domain.Models;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;

namespace Evolution.Master.Core.Services
{
    public class ExpenseTypeService : MasterService, IExpenseType
    {

        private readonly IAppLogger<MasterService> _logger = null;
        private readonly IMasterRepository _repository = null;
        private readonly IMapper _mapper = null;
        private readonly JObject _messages = null;
        private readonly IMemoryCache _memoryCache = null;
        private readonly AppEnvVariableBaseModel _environment = null;

        public ExpenseTypeService(IAppLogger<MasterService> logger, IMasterRepository repository, IMapper mapper, JObject messages, IMemoryCache memoryCache, IOptions<AppEnvVariableBaseModel> environment) : base(mapper, logger, repository, messages)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
            _messages = messages;
            _memoryCache = memoryCache;
            _environment = environment.Value;
        }
        public Response Search(ExpenseType search)
        {

            IList<ExpenseType> result = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                if (search == null)
                    search = new ExpenseType();

                var masterData = _mapper.Map<ExpenseType, MasterData>(search);
                var cacheKey = "ExpenseType";
                if (!_memoryCache.TryGetValue(cacheKey, out result))
                {
                    using (var tranScope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                    {
                        result = this._repository.FindBy(_mapper.Map<MasterData, Data>(masterData).ToExpression())
                                            .Where(x => x.MasterDataTypeId == Convert.ToInt32(MasterType.ExpenseType)
                                            && (search.IsActive == null || x.IsActive == search.IsActive))   //Changes for Defect 983
                                            .Select(x => _mapper.Map<MasterData, ExpenseType>(_mapper.Map<Data, MasterData>(x))).OrderBy(x => x.Name).ToList();
                        _memoryCache.Set(cacheKey, result, ObjectExtension.CacheExpiry(_environment.AbsoluteExpiration, _environment.SlidingExpiration));
                        tranScope.Complete();

                    }
                }
            }
            catch (Exception ex)
            {
                responseType = ResponseType.Exception;
                this._logger.LogError(responseType.ToId(), this._messages[responseType.ToId()].ToString(), ex);
            }
            return new Response().ToPopulate(responseType, result, result?.Count);
        }

        #region ValidationCheck
        public bool IsValidExpenseType(IList<string> expenseType, ref IList<DbModel.Data> dbExpenseTypes, ref IList<ValidationMessage> messages)
        {
            IList<ValidationMessage> message = new List<ValidationMessage>();
            dbExpenseTypes = dbExpenseTypes ?? new List<DbModel.Data>(); //Added for D702(#17 issue Ref by 06-03-2020 ALM Doc)
            if (expenseType?.Count() > 0)
            {

                var tsExpenseType = GetExpenseType(expenseType);
                //var dbExpenseType = _repository?.FindBy(x => expenseType.Contains(x.Name))?.ToList(); //Commented for D702(#17 issue Ref by 06-03-2020 ALM Doc)
                var expenseTypeNotExists = expenseType?.Where(x => !tsExpenseType.Any(x2 => x2.Name == x))?.ToList();
                expenseTypeNotExists?.ForEach(x =>
                {
                    string errorCode = MessageType.InvalidExpenseType.ToId();
                    message.Add(_messages, x, MessageType.InvalidExpenseType, x);
                });
                if (tsExpenseType != null && tsExpenseType.Any()) //Added for D702(#17 issue Ref by 06-03-2020 ALM Doc)
                {
                    dbExpenseTypes.AddRange(tsExpenseType);
                }
               // dbExpenseTypes = dbExpenseType;
                messages = message;
            }

            return messages?.Count <= 0;
         }
        #endregion

        #region Private Methods
        private IList<DbModel.Data> GetExpenseType(IList<string> expenseType)
        {
            IList<DbModel.Data> dbTsExpenseType = null;
            if (expenseType?.Count > 0)
                dbTsExpenseType = _repository?.FindBy(x => expenseType.Contains(x.Name))?.ToList();

            return dbTsExpenseType;
        }
        #endregion
    }
}

