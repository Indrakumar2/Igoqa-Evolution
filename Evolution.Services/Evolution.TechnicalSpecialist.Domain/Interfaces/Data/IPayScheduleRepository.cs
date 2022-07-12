using Evolution.GenericDbRepository.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
namespace Evolution.TechnicalSpecialist.Domain.Interfaces.Data
{
    /// <summary>
    /// TODO : Replace string to Customer Document DB Model
    /// </summary>
    public interface IPayScheduleRepository : IGenericRepository<DbModel.TechnicalSpecialistPaySchedule>
    {
        ///TODO : Defined those function which is not covered in Generic Repository
    }
}
