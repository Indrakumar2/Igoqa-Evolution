using Evolution.GenericDbRepository.Interfaces;
using Evolution.Common.Enums;
using System;
using System.Collections.Generic;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.Visit.Domain.Models.Visits;



namespace Evolution.Visit.Domain.Interfaces.Data
{
    public interface IVisitInterCompanyDiscountsRepository : IGenericRepository<DbModel.VisitInterCompanyDiscount>, IDisposable
    {
        DomainModel.VisitInterCoDiscountInfo Search(long visitId);

        IList<DbModel.VisitInterCompanyDiscount> GetVisitInterCompanyDiscounts(long visitId, ValidationType validationType);

        DomainModel.VisitInterCoDiscountInfo UpdateVisitIntercompanyDiscount(DbModel.VisitInterCompanyDiscount visitintercompanydisco);
    }
}
