using Evolution.Assignment.Domain.Interfaces.Assignments;
using Evolution.Common.Models.Responses;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using DomainModel = Evolution.Assignment.Domain.Models.Assignments;

namespace Evolution.Api.Controllers.Assignment
{
    [Route("api/assignments/{assignmentid}/contributionRevenueCosts")]
    [ApiController]
    public class AssignmentContributionRevenueCostController : ControllerBase
    {
        private readonly IAssignmentContributionRevenueCostService _assignmentContributionRevenueCostService = null;

        public AssignmentContributionRevenueCostController(IAssignmentContributionRevenueCostService assignmentContributionRevenueCostService)
        {
            _assignmentContributionRevenueCostService = assignmentContributionRevenueCostService;
        }

        [HttpGet]
        public Response Get([FromRoute]int assignmentId, [FromQuery]DomainModel.AssignmentContributionRevenueCost searchModel)
        {
            //searchModel.AssignmentId = assignmentId;
            return _assignmentContributionRevenueCostService.Get(searchModel);
        }

        [HttpPost]
        public Response Post([FromBody]IList<DomainModel.AssignmentContributionRevenueCost> assignmentContributionRevenueCosts)
        {
            return _assignmentContributionRevenueCostService.Add(assignmentContributionRevenueCosts);
        }

        [HttpPut]
        public Response Put([FromBody]IList<DomainModel.AssignmentContributionRevenueCost> assignmentContributionRevenueCosts)
        {
            return _assignmentContributionRevenueCostService.Modify(assignmentContributionRevenueCosts);
        }

        [HttpDelete]
        public Response Delete([FromBody]IList<DomainModel.AssignmentContributionRevenueCost> assignmentContributionRevenueCosts)
        {
            return _assignmentContributionRevenueCostService.Delete(assignmentContributionRevenueCosts);
        }
    }
}