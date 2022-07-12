using System;
using System.Collections.Generic;
using System.Text;
using DomainModel = Evolution.Reports.Domain.Models.Reports;

namespace Evolution.Reports.Domain.Interfaces.Data
{
    public interface ICompanySpecificMatrixRepository
    {
        IList<DomainModel.ResourceTaxonomyServices> GetByResource(string[] companyID);
        IList<DomainModel.TaxonomyResourceServices> GetByTaxonomyService(string[] companyID);
        byte[] ExportReport(string[] companyCode);
    }
}

