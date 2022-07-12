using AutoMapper;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Common.Models.Base;
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
using System.Text;
using System.Transactions;

namespace Evolution.Master.Core.Services
{
    public class AssignmentTypeService : MasterService, IAssignmentTypeService
    {
        private readonly IAppLogger<MasterService> _logger = null;
        private readonly IMapper _mapper = null;
        private readonly IMasterRepository _repository = null;
        private readonly JObject _messages = null;
        private readonly IMemoryCache _memoryCache = null;
        private readonly AppEnvVariableBaseModel _environment = null;

        public AssignmentTypeService(IAppLogger<MasterService> logger, IMapper mapper, 
                                    IMasterRepository repository, JObject messages, 
                                    IMemoryCache memoryCache,
                                    IOptions<AppEnvVariableBaseModel> environment) : base(mapper, logger, repository, messages)
        {
            _logger = logger;
            _mapper = mapper;
            _repository = repository;
            _messages = messages;
            _memoryCache = memoryCache;
            _environment = environment.Value;
        }

        public Response Search(AssignmentType search)
        {
            IList<AssignmentType> result = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                if (search == null)
                    search = new AssignmentType();

                //var cacheKey = "AssignmentType";
                //if (!_memoryCache.TryGetValue(cacheKey, out result))
                //{
                    var masterData = _mapper.Map<AssignmentType, MasterData>(search);
                    using (var tranScope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                    {
                        //If condition added on 23-Sep2020 to find how complied queries perform
                        if (search.Name.HasEvoWildCardChar() || search.Description.HasEvoWildCardChar())
                            result = this._repository.FindBy(_mapper.Map<MasterData, Data>(masterData).ToExpression())
                                                    .Where(x => x.MasterDataTypeId == Convert.ToInt32(MasterType.AssignmentType) && (search.IsActive == null || x.IsActive == search.IsActive))
                                                    .Select(x => _mapper.Map<MasterData, AssignmentType>(_mapper.Map<Data, MasterData>(x))).OrderBy(x => x.Name).ToList();
                        else
                        {
                            masterData.MasterDataTypeId = Convert.ToInt32(MasterType.AssignmentType);
                            result = _repository.GetMasterData(masterData).Select(x => _mapper.Map<MasterData, AssignmentType>(x)).OrderBy(x => x.Name).ToList();
                        }
                        //_memoryCache.Set(cacheKey, result, ObjectExtension.CacheExpiry(_environment.AbsoluteExpiration, _environment.SlidingExpiration));
                        tranScope.Complete();
                    }
                //}
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
