using AutoMapper;
using Evolution.DbRepository.Interfaces.Master;
using Evolution.Logging.Interfaces;
using Evolution.Master.Domain.Interfaces.Services;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using Evolution.Common.Models.Responses;
using Evolution.Master.Domain.Models;
using Evolution.Common.Enums;
using System.Linq;
using System.Data.SqlClient;
using Evolution.Common.Extensions;
using System.Transactions;

namespace Evolution.Master.Core.Services
{
   public class TimeOffRequestCategoryService : ITimeOffRequestService
    {
        private readonly IAppLogger<TimeOffRequestCategoryService> _logger = null;
        private readonly ITimeOffRequestCategoryRepository _repository = null;
        private readonly IMapper _mapper = null;
        private readonly JObject _messages = null;

        public TimeOffRequestCategoryService(IMapper mapper, IAppLogger<TimeOffRequestCategoryService> logger, ITimeOffRequestCategoryRepository repository, JObject messages)
        {
            this._logger = logger;
            this._repository = repository;
            this._mapper = mapper;
            this._messages = messages;
        }

        public Response Search(TimeOffRequestCategory search)
        {
            IList<TimeOffRequestCategory> result = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                if (search == null)
                    search = new TimeOffRequestCategory();
                using (var tranScope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))                
                {
                    result = _repository.FindBy(null)
                                            .Where(x => (string.IsNullOrEmpty(search.EmploymentType) || x.EmploymentType.Name == search.EmploymentType)
                                                    && (string.IsNullOrEmpty(search.LeaveTypeCategory) || x.LeaveCategoryType.Name == search.LeaveTypeCategory)
                                                    && (search.IsActive == null || x.LeaveCategoryType.IsActive == search.IsActive))
                                            .Select(x => new TimeOffRequestCategory() { EmploymentType = x.EmploymentType.Name, LeaveTypeCategory = x.LeaveCategoryType.Name,Id=x.Id }).OrderBy(x => x.EmploymentType).ToList();
                    tranScope.Complete();
                }
            }
            catch (SqlException sqlE)
            {
                responseType = ResponseType.DbException;
                this._logger.LogError(responseType.ToId(), this._messages[responseType.ToId()].ToString(), sqlE);
            }
            catch (Exception ex)
            {
                responseType = ResponseType.Exception;
                this._logger.LogError(responseType.ToId(), this._messages[responseType.ToId()].ToString(), ex);
            }

            return new Response().ToPopulate(responseType, result, result?.Count);
        }
    }
}
