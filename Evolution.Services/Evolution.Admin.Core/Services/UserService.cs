using Evolution.Admin.Domain.Interfaces.Admins;
using Evolution.Admin.Domain.Interfaces.Data;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Common.Models.Responses;
using Evolution.DbRepository.Models.SqlDatabaseContext;
using Evolution.Logging.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using DomainModels = Evolution.Admin.Domain.Models.Admins;

namespace Evolution.Admin.Core.Services
{
    public class UserService : IUserService
    {

        private readonly IUserRepository _repsoitory = null;
        private readonly IAppLogger<UserService> _logger = null;

        public UserService(IUserRepository repository, IAppLogger<UserService> logger)
        {
            _repsoitory = repository;
            _logger = logger;
        }

        public Response GeUser(string userName)
        {
            Exception exception = null;
            int result = 0;
            try
            {
                result= _repsoitory.FindBy(x => x.SamaccountName == userName).FirstOrDefault().Id;
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), userName);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception);
        }
        public Response GeUser(DomainModels.User searchModel)
        {
            Exception exception = null;
            IList<DomainModels.User> result = null;
            try
            {
                result = _repsoitory.Search(searchModel);
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), searchModel);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception, result?.Count);
        }

        public Response GetMICoordinators(IList<string> ContractHoldingCompanyCodes, IList<int> ContractHoldingCompanyIds=null)
        {
            IList<DomainModels.User> result = null;
            Exception exception = null;
            try
            {
                using (var tranScope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    if (ContractHoldingCompanyIds?.Count > 0)
                        result = _repsoitory.GetUsers(ContractHoldingCompanyIds);
                    else if(ContractHoldingCompanyCodes?.Count > 0)
                        result = _repsoitory.GetUsers(ContractHoldingCompanyCodes);

                    tranScope.Complete();
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), ContractHoldingCompanyCodes);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception, result?.Count);
        }

        public Response GetReportCoordniators(string loggedInCompany, bool isVisit, bool isOperating)
        {
            IList<DomainModels.User> result = null;
            Exception exception = null;
            try
            {
                using (var tranScope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    result = _repsoitory.GetUsers(loggedInCompany, isVisit, isOperating);
                    tranScope.Complete();
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), loggedInCompany);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception, result?.Count);
        }

        public Response GetByUserType(string companyCode, IList<string> userTypes, bool isFilterCompanyActiveCoordinators = false)
        {
            IList<DomainModels.User> result = null;
            Exception exception = null;
            try
            {
                using (var tranScope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    result = _repsoitory.Get(companyCode, userTypes, isFilterCompanyActiveCoordinators);
                    tranScope.Complete();
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), companyCode, userTypes, isFilterCompanyActiveCoordinators);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception, result?.Count);
        }
        public Response GetByUserType(IList<string> companyCode, IList<string> userTypes, bool isFilterCompanyActiveCoordinators = false)
        {
            IList<DomainModels.User> result = null;
            Exception exception = null;
            try
            {
                using (var tranScope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    result = _repsoitory.Get(companyCode, userTypes, isFilterCompanyActiveCoordinators);
                    tranScope.Complete();
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), companyCode, userTypes, isFilterCompanyActiveCoordinators);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception, result?.Count);
        }
    }
}
