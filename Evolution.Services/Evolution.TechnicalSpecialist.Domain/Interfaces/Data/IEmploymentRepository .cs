using Evolution.GenericDbRepository.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
namespace Evolution.TechnicalSpecialist.Domain.Interfaces.Data
{
    /// <summary>
    /// TODO : Replace string to TechnicalSpecialist Address DB Model
    /// </summary>
    public interface IEmploymentRepository : IGenericRepository<DbModel.TechnicalSpecialist>
    {
        ///TODO : Defined those function which is not covered in Generic Repository
    }
}
