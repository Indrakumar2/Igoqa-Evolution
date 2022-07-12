using System;
using System.Threading.Tasks;
using Evolution.Api.Controllers.Base;
using Evolution.Assignment.Domain.Interfaces.Assignments;
using Evolution.Common.Extensions;
using Evolution.Common.Models.Responses;
using Microsoft.AspNetCore.Mvc;
using Model = Evolution.Assignment.Domain.Models.Assignments;
using Evolution.Common.Enums;

namespace Evolution.Api.Controllers.Assignment
{
    [Route("api/assignmentchec/{assignmentId}/detail")]
    public class AssignmentCheckController : BaseController
    {
        private readonly IAssignmentDetailService _service = null;

        public AssignmentCheckController(IAssignmentDetailService service)
        {
            this._service = service;
        }

        [HttpPost]
        public Response Post([FromBody]Model.AssignmentDetail model)
        {
            AssignValues(model, ValidationType.Add);
            AssignValuesFromToken(ref model);
            return _service.Add(model);
        }

        [HttpPut]
        public Response Put([FromBody]Model.AssignmentDetail model)
        {
            AssignValues(model, ValidationType.Update);
            AssignValuesFromToken(ref model);
            return _service.Modify(model);
        }

        [HttpDelete]
        public Response Delete([FromBody]Model.AssignmentDetail model)
        {
            AssignValues(model, ValidationType.Delete);
            return _service.Delete(model);
        }

        [HttpGet]
        public Response Get([FromRoute]int assignmentId)
        {
            return Task.Run<Response>(async () => await this._service.Get(assignmentId)).Result;
        }

        private void AssignValuesFromToken(ref Model.AssignmentDetail model)
        {
            if (model?.ResourceSearch?.SearchParameter != null)
            {
                model.ResourceSearch.ActionByUser = UserName;
                model.ResourceSearch.UserTypes = UserType?.Split(',');
                model.ResourceSearch.CreatedBy = UserName;
                model.ResourceSearch.AssignedBy = UserName;
                model.ResourceSearch.CreatedOn = DateTime.UtcNow;
                model.ResourceSearch.ModifiedBy = UserName;
                model.ResourceSearch.LastModification = DateTime.UtcNow;
            }
        }

        private void AssignValues(Model.AssignmentDetail model, ValidationType validationType)
        {
            model.AssignmentInfo.SetPropertyValue("ActionByUser", UserName);
            model.AssignmentAdditionalExpenses.SetPropertyValue("ActionByUser", UserName);
            model.AssignmentContractSchedules.SetPropertyValue("ActionByUser", UserName);
            model.AssignmentContributionCalculators.SetPropertyValue("ActionByUser", UserName);
            model.AssignmentDocuments.SetPropertyValue("ActionByUser", UserName);
            model.AssignmentInstructions.SetPropertyValue("ActionByUser", UserName);
            model.AssignmentInterCompanyDiscounts.SetPropertyValue("ActionByUser", UserName);
            model.AssignmentNotes.SetPropertyValue("ActionByUser", UserName);
            model.AssignmentReferences.SetPropertyValue("ActionByUser", UserName);
            model.AssignmentSubSuppliers.SetPropertyValue("ActionByUser", UserName);
            model.AssignmentTaxonomy.SetPropertyValue("ActionByUser", UserName);
            model.AssignmentTechnicalSpecialists.SetPropertyValue("ActionByUser", UserName);
            if (validationType != ValidationType.Add)
                model.AssignmentInfo.SetPropertyValue("ModifiedBy", UserName);
            if (validationType != ValidationType.Add)
                model.AssignmentAdditionalExpenses.SetPropertyValue("ModifiedBy", UserName);
            if (validationType != ValidationType.Add)
                model.AssignmentContractSchedules.SetPropertyValue("ModifiedBy", UserName);
            if (validationType != ValidationType.Add)
                model.AssignmentContributionCalculators.SetPropertyValue("ModifiedBy", UserName);
            if (validationType != ValidationType.Add)
                model.AssignmentDocuments.SetPropertyValue("ModifiedBy", UserName);
            if (validationType != ValidationType.Add)
                model.AssignmentInstructions.SetPropertyValue("ModifiedBy", UserName);
            if (validationType != ValidationType.Add)
                model.AssignmentInterCompanyDiscounts.SetPropertyValue("ModifiedBy", UserName);
            if (validationType != ValidationType.Add)
                model.AssignmentNotes.SetPropertyValue("ModifiedBy", UserName);
            if (validationType != ValidationType.Add)
                model.AssignmentReferences.SetPropertyValue("ModifiedBy", UserName);
            if (validationType != ValidationType.Add)
                model.AssignmentSubSuppliers.SetPropertyValue("ModifiedBy", UserName);
            if (validationType != ValidationType.Add)
                model.AssignmentTaxonomy.SetPropertyValue("ModifiedBy", UserName);
            if (validationType != ValidationType.Add)
                model.AssignmentTechnicalSpecialists.SetPropertyValue("ModifiedBy", UserName);
        }
    }
}
