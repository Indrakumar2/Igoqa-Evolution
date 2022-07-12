using Evolution.Common.Models.Responses;
using System;
using System.Collections.Generic;
using System.Text;

namespace Evolution.Reports.Domain.Interfaces.Reports
{
    public interface ICompanySpecificMatrixService
    {
        Response GetByResource(string[] companyID);
        Response GetByTaxonomyService(string[] companyID);
        object ExportReport(string[] companyCode);
    }
}

