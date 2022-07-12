using Evolution.Common.Models.Responses;
using Evolution.Reports.Domain.Interfaces.Reports;
using Evolution.Reports.Domain.Models.Reports;
using DomainModel = Evolution.Reports.Domain.Models.Reports;
using System;
using System.Collections.Generic;
using System.Text;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Reports.Domain.Interfaces.Data;
using Evolution.Logging.Interfaces;
using System.Transactions;

namespace Evolution.Reports.Core.Services
{
    public class TaxonomyService : ITaxonomyService
    {
        private ITaxonomyRepository _repository = null;
        private IAppLogger<TaxonomyService> _logger = null;

        public TaxonomyService(ITaxonomyRepository repository,IAppLogger<TaxonomyService> logger)
        {
            _repository = repository;
            _logger = logger;
        }
        public Response Get(Taxonomy searchModel)
        {
            Exception exception = null;
            IList<DomainModel.Taxonomy> result = null;
            try
            {
                //using (var tranScope = new TransactionScope(TransactionScopeOption.RequiresNew,
                //                      new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                //{
                    result = _repository.Search(searchModel);
                //    tranScope.Complete();
                //}
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), searchModel);
                result = new List<Taxonomy>();
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception, result?.Count);
        }
    }
}