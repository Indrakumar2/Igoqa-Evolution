using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Common.Models.Responses;
using Evolution.Logging.Interfaces;
using Evolution.Reports.Domain.Interfaces.Data;
using Evolution.Reports.Domain.Interfaces.Reports;
using System;
using System.Collections.Generic;
using System.Transactions;
using DomainModel = Evolution.Reports.Domain.Models.Reports;

namespace Evolution.Reports.Core.Services
{
    public class CompanySpecificMatrixService : ICompanySpecificMatrixService
    {
        private ICompanySpecificMatrixRepository _repository = null;
        private IAppLogger<CompanySpecificMatrixService> _logger = null;
        public CompanySpecificMatrixService(ICompanySpecificMatrixRepository repository, IAppLogger<CompanySpecificMatrixService> logger)
        {
            _repository = repository;
            _logger = logger;
        }
        public Response GetByResource(string[] companyID)
        {
            Exception exception = null;
            IList<DomainModel.ResourceTaxonomyServices> result = null;
            try
            {
                //using (var tranScope = new TransactionScope(TransactionScopeOption.RequiresNew,
                //                       new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                //{
                    result = _repository.GetByResource(companyID);
                //    tranScope.Complete();
                //}
            }
            catch (Exception ex)
            {
                exception = ex;
            }
            //int count = result != null ? (result as ).Count : 0;
            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception, result?.Count);
        }

        public Response GetByTaxonomyService(string[] companyID)
        {
            Exception exception = null;
            IList<DomainModel.TaxonomyResourceServices> result = null;
            try
            {
                //using (var tranScope = new TransactionScope(TransactionScopeOption.RequiresNew,
                //                       new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                //{
                    result = _repository.GetByTaxonomyService(companyID);
                //    tranScope.Complete();
                //}
            }
            catch (Exception ex)
            {
                exception = ex;
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception, result?.Count);
        }

        public object ExportReport(string[] companyCode)
        {
            Exception exception = null;
            int code = 200;
            string message = string.Empty;
            byte[] result = null;
            try
            {
                result = _repository.ExportReport(companyCode);
            }
            catch (Exception ex)
            {
                code = 500;
                exception = ex;
                message = exception.ToFullString();
            }
            
            if (exception == null)
                message = result == null ? "No data found." : string.Empty;

            return new { data = result, message = message, code = code };

        }
    }
}

