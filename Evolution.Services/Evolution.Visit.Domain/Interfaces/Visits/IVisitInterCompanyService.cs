using Evolution.Common.Models.Responses;
using System.Collections.Generic;
using Evolution.Visit.Domain.Models.Visits;

namespace Evolution.Visit.Domain.Interfaces.Visits
{
    /// <summary>
    /// This will provide all the functionality related to Visit Inter Company Service.
    /// </summary>
    public interface IVisitInterCompanyService
    {
        /// <summary>
        /// Save Visit Inter Company details
        /// </summary>
        /// <param name="visitInterCompanyDiscounts">List of Visit Inter Company details</param>
        /// <returns>All the Saved Visit Inter company detail</returns>
        Response SaveVisitInterCompany(IList<VisitInterCompanyDiscounts> visitInterCompanyDiscounts, bool commitChanges);

        /// <summary>
        /// Modify list of Visit Inter Company details
        /// </summary>
        /// <param name="visitInterCompanyDiscounts">List of Visit Inter Company details which need to update.</param>
        Response ModifyVisitInterCompany(IList<VisitInterCompanyDiscounts> visitInterCompanyDiscounts, bool commitChanges);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="visitInterCompanyDiscounts"></param>
        /// <param name="commitChanges"></param>
        /// <returns></returns>
        Response DeleteVisitInterCompany(IList<VisitInterCompanyDiscounts> visitInterCompanyDiscounts, bool commitChanges);

        /// <summary>
        /// Return all the match search Visit Inter Company details
        /// </summary>
        /// <param name="searchModel">search model</param>
        /// <returns>List of Matched Search Result</returns>
        Response GetVisitInterCompany(long visitId);
    }
}
