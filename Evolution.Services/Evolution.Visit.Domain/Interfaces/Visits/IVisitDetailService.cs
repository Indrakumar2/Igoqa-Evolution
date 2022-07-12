using Evolution.Common.Models.Responses;
using Evolution.Visit.Domain.Models.Visits;
using System.Threading.Tasks;

namespace Evolution.Visit.Domain.Interfaces.Visits
{
    public interface IVisitDetailService
    {
        Response Get(VisitDetail searchModel);

        Task<Response> GetTechnicalSpecialistWithGrossMargin(VisitTechnicalSpecialist technicalSpecialist);

        Response Add(VisitDetail visitDetails, bool IsAPIValidationRequired=false);

        Response Modify(VisitDetail visitDetails, bool IsAPIValidationRequired=false);

        Response Delete(VisitDetail visitDetails, bool IsAPIValidationRequired=false, bool isFinalVisit=false);

        Response ApproveVisit(VisitEmailData visitDetails);
        Response RejectVisit(VisitEmailData visitDetails);
        Response ApprovalCustomerReportNotification(CustomerReportingNotification clientReportingNotification);
    }
}
