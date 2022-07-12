using Evolution.Common.Models.Responses;
using Evolution.Supplier.Domain.Interfaces.Suppliers;
using Evolution.Supplier.Domain.Models.Supplier;
using DomainModel = Evolution.Supplier.Domain.Models.Supplier;
using System;
using System.Collections.Generic;
using System.Text;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Supplier.Domain.Interfaces.Data;
using Evolution.Logging.Interfaces;
using System.Transactions;

namespace Evolution.Supplier.Core.Services
{
    public class SupplierPerformanceReportService : ISupplierPerformanceReportService
    {
        private ISupplierPerfomanceRepository _repository = null;
        private IAppLogger<SupplierPerformanceReportService> _logger = null;

        public SupplierPerformanceReportService(ISupplierPerfomanceRepository repository,
                                                IAppLogger<SupplierPerformanceReportService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public Response Get(SupplierPerformanceReportsearch searchModel)
        {
            Exception exception = null;
            IList<DomainModel.SupplierPerformanceReport> result = null;
            try
            {
                //using (var tranScope = new TransactionScope(TransactionScopeOption.RequiresNew,
                //                       new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                //{
                    result = _repository.Search(searchModel);
                //    tranScope.Complete();
                //}
            }
            catch(Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), searchModel);
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception, result?.Count);
        }
    }
}
