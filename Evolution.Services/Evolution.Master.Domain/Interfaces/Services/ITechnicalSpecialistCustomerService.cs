using System;
using System.Collections.Generic;
using System.Text;
using Evolution.Common.Models.Responses;
using Evolution.Common.Models.Messages;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;

namespace Evolution.Master.Domain.Interfaces.Services
{
    public interface ITechnicalSpecialistCustomerService
    {
        Response Search(Models.TechnicalSpecialistCustomers search);
        bool IsValidCustomer(IList<string> customerName, ref IList<DbModel.TechnicalSpecialistCustomers> dbCustomers, ref IList<ValidationMessage> messages);


    }
}
