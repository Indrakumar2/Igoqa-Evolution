using Evolution.Common.Models.Messages;
using Evolution.Common.Models.Responses;
using System;
using System.Collections.Generic;
using System.Text;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;

namespace Evolution.Master.Domain.Interfaces.Services
{
    public interface IExpenseType
    {
        Response Search(Models.ExpenseType search);

        bool IsValidExpenseType(IList<string> expenseTypes, ref IList<DbModel.Data> dbExpenseTypes, ref IList<ValidationMessage> messages);
    }
}
