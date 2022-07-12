using Evolution.Common.Models.Responses;
using System.Collections.Generic;
using DomainModels = Evolution.Company.Domain.Models.Companies;
using DbModels=Evolution.DbRepository.Models.SqlDatabaseContext;

namespace Evolution.Company.Domain.Interfaces.Companies
{
    public interface ICompanyEmailTemplateService
    {
        Response GetCompanyEmailTemplate(string companyCode);

        Response AddCompanyEmailTemplate(string companyCode, DomainModels.CompanyEmailTemplate companyEmailTemplate,ref IList<DbModels.CompanyMessage> msgToBeInsert,ref IList<DbModels.CompanyMessage> msgToBeUpdate,ref IList<DbModels.CompanyMessage>msgToBeDelete, bool commitChange = true);

        Response ModifyCompanyEmailTemplate(string companyCode, DomainModels.CompanyEmailTemplate companyEmailTemplate,ref IList<DbModels.CompanyMessage> msgToBeInsert,ref IList<DbModels.CompanyMessage> msgToBeUpdate,ref IList<DbModels.CompanyMessage>msgToBeDelete, bool commitChange = true);

        //Response DeleteCompanyTemplate(string companyCode, IList<DomainModels.CompanyEmailTemplate> companyEmailTemplates, bool commitChange = true);
    }
}
