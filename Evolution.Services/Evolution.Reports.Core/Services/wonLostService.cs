using Evolution.Common.Models.Responses;
using Evolution.Logging.Interfaces;
using Evolution.Reports.Domain.Interfaces.Reports;
using DomainModel= Evolution.Reports.Domain.Models.Reports;
using System;
using System.Collections.Generic;
using System.Text;
using Evolution.Reports.Domain.Models.Reports;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Reports.Domain.Interfaces.Data;
using System.Transactions;

namespace Evolution.Reports.Core.Services
{
    public class WonLostService : IWonLostService
    {
        private IWonLostRepository _repository = null;
        private IAppLogger<CustomerApprovalService> _logger = null;
        public WonLostService(IWonLostRepository repository,
                                          IAppLogger<CustomerApprovalService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public Response Get(WonLost searchModel)
        {
            Exception exception = null;
            IList<DomainModel.WonLost> result = null;
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
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception, result?.Count);
        }
    }
}
