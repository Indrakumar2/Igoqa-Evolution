using Evolution.GenericDbRepository.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
namespace Evolution.Customer.Domain.Interfaces.Data
{
    /// <summary>
    /// TODO : Replace string to Customer Assignment Reference DB Model
    /// </summary>
    public interface IPayRateRepository : IGenericRepository<DbModel.TechnicalSpecialistPayRate>
    {
        ///TODO : Defined those function which is not covered in Generic Repository
    }
}
