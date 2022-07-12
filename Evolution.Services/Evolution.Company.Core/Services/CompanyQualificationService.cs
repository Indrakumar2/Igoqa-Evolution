using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Common.Models.Responses;
using Evolution.Company.Domain.Interfaces.Companies;
using Evolution.Company.Domain.Interfaces.Data;
using Evolution.Company.Domain.Models.Companies;
using Evolution.Logging.Interfaces;
using System;
using System.Collections.Generic;
using System.Transactions;
using DomainModel = Evolution.Company.Domain.Models.Companies;

namespace Evolution.Company.Core.Services
{
    public class CompanyQualificationService : ICompanyQualificationService
    {
        private readonly IAppLogger<CompanyQualificationService> _logger = null;

        public CompanyQualificationService(IAppLogger<CompanyQualificationService> logger)
        {
            this._logger = logger;
        }

        public Response GetCompanyQualification(CompanyQualification searchModel)
        {
            IList<DomainModel.CompanyQualification> result = null;
            Exception exception = null;
            try
            {
                //using (var tranScope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                //{
                  //  result = this._repository.Search(searchModel);
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
