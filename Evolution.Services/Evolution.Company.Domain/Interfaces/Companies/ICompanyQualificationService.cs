using Evolution.Common.Models.Responses;

namespace Evolution.Company.Domain.Interfaces.Companies
{
    public interface ICompanyQualificationService
    {
        /// <summary>
        /// Return all the match search Company Qualification
        /// </summary>
        /// <param name="searchModel">search model</param>
        /// <returns>List of Matched Search Result</returns>
        Response GetCompanyQualification(Models.Companies.CompanyQualification searchModel);
    }
}
