using AutoMapper;
using Evolution.DbRepository.Models.SqlDatabaseContext;
using Evolution.Logging.Interfaces;
using Evolution.Master.Domain.Interfaces.Data;
using Evolution.Master.Domain.Interfaces.Services;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using Evolution.Common.Models.Responses;
using Evolution.Master.Domain.Models;
using DomainModels = Evolution.Master.Domain.Models;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using System.Transactions;

namespace Evolution.Master.Core.Services
{
    public class TaxonomyBusinessUnitService :ITaxonomyBusinessUnitService
    {

        private readonly IAppLogger<TaxonomyBusinessUnitService> _logger = null;
        private readonly ITaxonomyBusinessUnitRepository _repository = null;
        private readonly IMapper _mapper = null;
        private readonly JObject _messages = null;

        public TaxonomyBusinessUnitService(IAppLogger<TaxonomyBusinessUnitService> logger, ITaxonomyBusinessUnitRepository repository, IMapper mapper, JObject messages)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
            _messages = messages;
        }

        public Response Search(Domain.Models.TaxonomyBusinessUnit search)
        {
            Exception exception = null;
            IList<DomainModels.TaxonomyBusinessUnit> result = null;
            try
            {
                using (var tranScope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))                
                {
                    result = _repository.Search(search);
                    tranScope.Complete();
                }
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
