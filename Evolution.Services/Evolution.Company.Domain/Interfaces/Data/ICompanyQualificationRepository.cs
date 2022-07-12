using Evolution.GenericDbRepository.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.Company.Domain.Models;

namespace Evolution.Company.Domain.Interfaces.Data
{
    /// <summary>
    /// TODO : Replace string to Company DB Model
    /// </summary>
    public interface ICompanyQualificationRepository : IGenericRepository<Evolution.DbRepository.Models.SqlDatabaseContext.CompanyQualificationType>
    {
        IList<DomainModel.Companies.CompanyQualification> Search(DomainModel.Companies.CompanyQualification model);
    }
}
