﻿using Evolution.GenericDbRepository.Interfaces;
using System.Collections.Generic;
using DomainModel = Evolution.Company.Domain.Models.Companies;

namespace Evolution.Company.Domain.Interfaces.Data
{
    public interface ICompanyInvoiceRepository : IGenericRepository<Evolution.DbRepository.Models.SqlDatabaseContext.CompanyMessage>
    {
        DomainModel.CompanyInvoice Search(string companyCode);
    }
}
