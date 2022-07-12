using AutoMapper;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Common.Models.Responses;
using Evolution.DbRepository.Models.SqlDatabaseContext;
using Evolution.Logging.Interfaces;
using Evolution.Master.Domain.Interfaces.Data;
using Evolution.Master.Domain.Interfaces.Services;
using Evolution.Master.Domain.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using DomModel = Evolution.Master.Domain.Models;

namespace Evolution.Master.Core.Services
{
    public class ModuleTypeService : MasterService, IModuleTypeService
    {
        private readonly IAppLogger<MasterService> _logger = null;
        private readonly IMapper _mapper = null;
        private readonly IMasterRepository _repository = null;
        private readonly JObject _messages = null;

        public ModuleTypeService(IAppLogger<MasterService> logger, IMapper mapper, IMasterRepository repository, JObject messages) : base(mapper, logger, repository, messages)
        {
            _logger = logger;
            _mapper = mapper;
            _repository = repository;
            _messages = messages;
        }

        public Response Search(DomModel.MasterModuleTypes search)
        {
            IList<DomModel.MasterModuleTypes> result = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                if (search == null)
                    search = new DomModel.MasterModuleTypes();

                var masterData = _mapper.Map<DomModel.MasterModuleTypes, MasterData>(search);
                using (var tranScope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))                
                {
                    result = this._repository.FindBy(_mapper.Map<MasterData, Data>(masterData).ToExpression())
                                                .Where(x => x.MasterDataTypeId == Convert.ToInt32(MasterType.ModuleType))
                                                .Select(x => _mapper.Map<MasterData, DomModel.MasterModuleTypes>(_mapper.Map<Data, MasterData>(x))).OrderBy(x => x.Name).ToList();
                    tranScope.Complete();
                }
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
