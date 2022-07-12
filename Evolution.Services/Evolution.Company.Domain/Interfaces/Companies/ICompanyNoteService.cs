using Evolution.Common.Models.Responses;
using System;
using System.Collections.Generic;
using System.Text;

namespace Evolution.Company.Domain.Interfaces.Companies
{
    /// <summary>
    /// This will provide all the functionality related to Company Note.
    /// </summary>
    public interface ICompanyNoteService
    {
        /// <summary>
        /// Save Company Notes
        /// </summary>
        /// <param name="companyNotes">List of Company Note</param>
        /// <param name="companyCode">List of Company Note</param>
        /// <param name="commitchange">List of Company Note</param>
        /// <returns>All the Saved Company Note Details</returns>
        Response SaveCompanyNote(string companyCode,IList<Models.Companies.CompanyNote> companyNotes,bool commitchange );

        /// <summary>
        /// Modify list of Company Notes
        /// </summary>
        /// <param name="companyNotes">List of Company which need to update.</param>
        Response ModifyCompanyNote(string companyCode,IList<Models.Companies.CompanyNote> companyNotes, bool commitchange);

        /// <summary>
        /// Delete list of Company Notes
        /// </summary>
        /// <param name="companyNotes">List of Company which need to delete.</param>
        Response DeleteCompanyNote(string companyCode, IList<Models.Companies.CompanyNote> companyNotes, bool commitchange);

        /// <summary>
        /// Return all the match search Company Notes.
        /// </summary>
        /// <param name="searchModel">search model</param>
        /// <returns>List of Matched Search Result</returns>
        Response GetCompanyNote(Models.Companies.CompanyNote searchModel);
    }
}
