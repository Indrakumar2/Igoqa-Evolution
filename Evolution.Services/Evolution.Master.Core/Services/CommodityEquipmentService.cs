using AutoMapper;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Common.Models.Responses;
using Evolution.Logging.Interfaces;
using Evolution.Master.Domain.Interfaces.Data;
using Evolution.Master.Domain.Interfaces.Services;
using Evolution.Master.Domain.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace Evolution.Master.Core
{
    public class CommodityEquipmentService : ICommodityEquipmentService
    {
        private readonly IAppLogger<CommodityEquipmentService> _logger = null;
        private readonly IMasterRepository _masterRepository = null;
        private readonly ICommodityEquipmentRepository _repository = null;
        private readonly IMapper _mapper = null;
        private readonly JObject _messages = null;

        public CommodityEquipmentService(IAppLogger<CommodityEquipmentService> logger,
                                         IMasterRepository masterRepository,
                                         ICommodityEquipmentRepository repository,
                                         IMapper mapper,
                                         JObject messages)
        {
            _logger = logger;
            _masterRepository = masterRepository;
            _repository = repository;
            _mapper = mapper;
            _messages = messages;
        }


        public Response Search(CommodityEquipment search)
        {
            IList<CommodityEquipment> result = null;
            Exception exception = null;
            try
            {
                result = _repository.Search(search);
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), search);
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception, result?.Count);
        }
    }
}