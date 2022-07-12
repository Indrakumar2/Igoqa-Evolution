using AutoMapper;
using Evolution.Api.Controllers.Base;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Common.Models.Responses;
using Evolution.Draft.Domain.Interfaces.Draft;
using Evolution.Home.Domain.Interfaces.Homes;
using Evolution.Logging.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using DraftDomainModel = Evolution.Draft.Domain.Models;

namespace Evolution.Api.Controllers.Home
{
    [Route("api/Home")]
    [ApiController]
    public class HomeController : BaseController
    {
        private readonly IHomeService _homeService = null;
        private readonly IDraftService _draftService = null;
        private readonly IMyTaskService _myTaskService = null;
        private readonly IMapper _mapper = null;
        private readonly IAppLogger<HomeController> _logger = null;

        public HomeController(IHomeService homeService, IDraftService draftService, IMyTaskService myTaskService, IMapper mapper, IAppLogger<HomeController> logger)
        {
            _homeService = homeService;
            _draftService = draftService;
            _myTaskService = myTaskService;
            this._mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        [Route("contract/budgetinfo")]
        public Response GetContract([FromQuery]string companyCode, [FromQuery]string userName, [FromQuery]ContractStatus contractStatus = ContractStatus.Open, [FromQuery] bool isMyAssignmentOnly = true, [FromQuery] bool? IsOverBudgetValue = null, [FromQuery] bool? IsOverBudgetHour = null)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                return _homeService.GetBudget(companyCode, userName, contractStatus, isMyAssignmentOnly, IsOverBudgetValue, IsOverBudgetHour);
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                //Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), companyCode);
            }

            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }

        [HttpGet]
        [Route("{moduleCode}/mytasks")]
        public Response GetMyTask([FromQuery]string companyCode, [FromQuery]string userName)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                return _homeService.GetMyTask(companyCode, userName);
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                //Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), companyCode);
            }

            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }

        [HttpGet]
        [Route("{moduleCode}/myTasksAssignUsers")]
        public Response GetMyTaskAssignUsers([FromQuery]string companyCode)
        {

            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                return _homeService.GetMyTaskReAssignUsers(companyCode);
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                //Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), companyCode);
            }

            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }

        [HttpPut]
        [Route("{moduleCode}/mytasksReassign")]
        public Response ReassignMyTask([FromBody]IList<Evolution.Home.Domain.Models.Homes.MyTask> myTask, [FromRoute]string moduleCode)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                var taskType = myTask?.Where(x => !string.IsNullOrEmpty(x.TaskType))?.Select(x => x.TaskType).ToList();
                bool commitChanges = true;
                IList<DraftDomainModel.Draft> result = null;

                if (taskType?.Count > 0)
                {
                    List<string> draftTypes = new List<string> { DraftType.CreateProfile.ToString(),
                                                DraftType.TS_EditProfile.ToString(),
                                                DraftType.RCRM_EditProfile.ToString(),
                                                DraftType.TM_EditProfile.ToString(),
                                                DraftType.ProfileChangeHistory.ToString()};

                    var tasks = myTask?.Where(x => !draftTypes.Contains(x.TaskType)).ToList();
                    if (tasks?.Count > 0)
                    {
                        var responseResult = _myTaskService.Modify(tasks, commitChanges);
                        if (!string.IsNullOrEmpty(moduleCode) && moduleCode == "RSEARCH")
                        {
                            // call ModifyARSTask service from resource search service
                            _myTaskService.ModifyResourceSearchOnReassign(tasks, commitChanges);
                        }
                        return responseResult;
                    }

                    var drafts = myTask?.Where(x => draftTypes.Contains(x.TaskType)).ToList();
                    List<DraftDomainModel.Draft> lstdrafts = new List<DraftDomainModel.Draft>();
                    if (drafts?.Count > 0)
                    {
                        foreach (Evolution.Home.Domain.Models.Homes.MyTask mytaskData in drafts)
                        {
                            DraftDomainModel.Draft objDraft = new DraftDomainModel.Draft
                            {
                                Moduletype = mytaskData.Moduletype,
                                AssignedBy = mytaskData.AssignedBy,
                                AssignedTo = mytaskData.AssignedTo,
                                Id = mytaskData.Id.Value,
                                CreatedOn = mytaskData.CreatedOn,
                                LastModification = mytaskData.LastModification,
                                DraftId = mytaskData.TaskRefCode,
                                DraftType = mytaskData.TaskType,
                                Description = mytaskData.Description,
                                RecordStatus = mytaskData.RecordStatus
                            };
                            lstdrafts.Add(objDraft);
                        }
                        if (lstdrafts?.Count > 0)
                        {
                            return _draftService.ModifyListOfDrafts(lstdrafts, commitChanges);
                        }
                    }
                }

                return new Response().ToPopulate(ResponseType.Success, null, null, null, result, null, result?.Count);
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                //Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), myTask);
            }
            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }

        [HttpGet]
        [Route("mySearches")]
        public Response GetMYSearches([FromQuery] string companyCode, [FromQuery]string assignedTo, [FromQuery]bool isAllCoordinator)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                return _homeService.GetMyResourceSearch(companyCode, assignedTo, isAllCoordinator);
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                //Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), companyCode);
            }

            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }
    }
}