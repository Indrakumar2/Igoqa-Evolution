using Evolution.Common.Models.Responses;
using DomainModels = Evolution.Company.Domain.Models.Companies;
using DbModels=Evolution.DbRepository.Models.SqlDatabaseContext;
using System.Collections.Generic;

namespace Evolution.Company.Domain.Interfaces.Companies
{
    public interface ICompanyInvoiceService
    {
        Response GetCompanyInvoice(string companyCode);

        Response AddCompanyInvoice(string companyCode, DomainModels.CompanyInvoice companyInvoice,ref List<DbModels.CompanyMessage> msgToBeInsert ,ref List<DbModels.CompanyMessage> msgToBeUpdate,
                                                   ref List<DbModels.CompanyMessage> msgToBeDelete, bool commitChange = true);

        Response ModifyCompanyInvoice(string companyCode, DomainModels.CompanyInvoice companyInvoice,ref List<DbModels.CompanyMessage> msgToBeInsert ,ref List<DbModels.CompanyMessage> msgToBeUpdate,
                                                   ref List<DbModels.CompanyMessage> msgToBeDelete, bool commitChange = true);
    }
}
