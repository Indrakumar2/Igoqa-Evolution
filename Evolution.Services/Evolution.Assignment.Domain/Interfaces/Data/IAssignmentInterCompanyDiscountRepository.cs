using Evolution.GenericDbRepository.Interfaces;
using System;
using System.Collections.Generic;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.Assignment.Domain.Models.Assignments;

namespace Evolution.Assignment.Domain.Interfaces.Data
{
    public interface IAssignmentInterCompanyDiscountRepository: IGenericRepository<DbModel.AssignmentInterCompanyDiscount>, IDisposable
    {
       DomainModel.AssignmentInterCoDiscountInfo Search(int assignmentId);

       DomainModel.AssignmentInterCoDiscountInfo SearchWithCompany(int assignmentId);
    }
}