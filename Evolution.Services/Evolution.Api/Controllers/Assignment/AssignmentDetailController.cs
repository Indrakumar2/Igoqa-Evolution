using Evolution.Api.Controllers.Base;
using Evolution.Assignment.Domain.Interfaces.Assignments;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Common.Models.Base;
using Evolution.Common.Models.Responses;
using Evolution.Logging.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using Model = Evolution.Assignment.Domain.Models.Assignments;

namespace Evolution.Api.Controllers.Assignment
{
    [Route("api/assignments/{assignmentId}/detail")]
    public class AssignmentDetailController : BaseController
    {
        private readonly IAssignmentDetailService _service = null;
        private readonly AppEnvVariableBaseModel _environment = null;
        private readonly IAppLogger<AssignmentDetailController> _logger = null;

        public AssignmentDetailController(IAssignmentDetailService service, IOptions<AppEnvVariableBaseModel> environment, IAppLogger<AssignmentDetailController> logger)
        {
            _service = service;
            _environment = environment.Value;
            _logger = logger;
        }

        //[AuthorisationFilter(SecurityModule.Assignment, SecurityPermission.N00001)]
        [HttpPost]
        public Response Post([FromBody]Model.AssignmentDetail model)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                AssignValues(model, ValidationType.Add);
                AssignValuesFromToken(ref model);

                return _service.Add(model, ApplicationAudienceCode != _environment.Evolution2_SPA_Client_Audience_code);
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                //Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), model);
            }

            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }

        [HttpPut]
        public Response Put([FromBody]Model.AssignmentDetail model)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                AssignValues(model, ValidationType.Update);
                AssignValuesFromToken(ref model);
                return _service.Modify(model, ApplicationAudienceCode != _environment.Evolution2_SPA_Client_Audience_code);
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                //Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), model);
            }

            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }

        [HttpDelete]
        public Response Delete([FromBody]Model.AssignmentDetail model)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                AssignValues(model, ValidationType.Delete);
                return _service.Delete(model, ApplicationAudienceCode != _environment.Evolution2_SPA_Client_Audience_code);
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                //Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), model);
            }
            return new Response().ToPopulate(responseType, null, null, null, null, exception);
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
            if (model != null)
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
}
