using Evolution.Common.Models.Responses;
using Evolution.Logging.Interfaces;
using Evolution.Reports.Domain.Interfaces.Data;
using Evolution.Reports.Domain.Interfaces.Reports;
using Evolution.Reports.Domain.Models.Reports;
using System;
using System.Collections.Generic;
using DomainModel = Evolution.Reports.Domain.Models.Reports;
using System.Text;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using System.Transactions;

namespace Evolution.Reports.Core.Services
{
    public class CustomerApprovalService : ICustomerApprovalService
    {
        private ICustomerApprovalRepository _repository = null;
        private IAppLogger<CustomerApprovalService> _logger = null;
        public CustomerApprovalService(ICustomerApprovalRepository repository,
                                          IAppLogger<CustomerApprovalService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public Response Get(CustomerApproval searchModel)
        {
            Exception exception = null;
            IList<DomainModel.CustomerApproval> result = null;
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
