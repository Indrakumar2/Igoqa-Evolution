using AutoMapper;
using Evolution.Logging.Interfaces;
using Evolution.Master.Domain.Interfaces.Data;
using Evolution.Master.Domain.Interfaces.Services;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using Evolution.Common.Models.Responses;
using Evolution.Master.Domain.Models;
using Evolution.Common.Enums;
using Evolution.DbRepository.Models.SqlDatabaseContext;
using Evolution.Common.Extensions;
using System.Linq;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using Evolution.Common.Models.Messages;
using System.Transactions;

namespace Evolution.Master.Core.Services
{
   public class RegionService : MasterService,IRegionService
    {

        private readonly IAppLogger<MasterService> _logger = null;
        private readonly IMasterRepository _repository = null;
        private readonly IMapper _mapper = null;
        private readonly JObject _messages = null;

        public RegionService(IAppLogger<MasterService> logger, IMasterRepository repository, IMapper mapper, JObject messages):base(mapper,logger,repository,messages)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
            _messages = messages;
        }

        public Response Search(Region search)
        {
            IList<Region> result = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                if (search == null)
                    search = new Region();

                var masterData = _mapper.Map<Region, MasterData>(search);
                using (var tranScope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))                
                {
                    result = this._repository.FindBy(_mapper.Map<MasterData, Data>(masterData).ToExpression())
                                                .Where(x => x.MasterDataTypeId == Convert.ToInt32(MasterType.Region))
                                                .Select(x => _mapper.Map<MasterData, Region>(_mapper.Map<Data, MasterData>(x))).OrderBy(x => x.Name).ToList();
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


        public bool IsValidRegion(IList<string> names, ref IList<DbModel.Data> dbRegions, ref IList<ValidationMessage> valdMessages)
        {
            bool? result = false;
            var messages = new List<ValidationMessage>();
            if (names?.Count() > 0)
            {
                result = _repository?.IsRecordValidByName(MasterType.Region, names, ref dbRegions);
                IList<DbModel.Data> dbDatas = dbRegions;
                var regionNotExists = names?.Where(x => !dbDatas.Any(x2 => x2.Name == x))?.ToList();
                regionNotExists?.ForEach(x =>
                {
                    messages.Add(_messages, x, MessageType.InvalidRegion, x);
                });
                valdMessages = messages;
            }
            return (bool)result;
        }

    }
}
