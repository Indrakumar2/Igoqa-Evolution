using Evolution.Common.Models.Messages;
using Evolution.Common.Models.Responses;
using System.Collections.Generic;

namespace Evolution.Master.Domain.Interfaces.Services
{
    public interface IPayrollTypeService : IMasterService
    {   
        Response SavePayrollType(IList<Models.PayrollType> model, bool returnResultSet = false);

        Response UpdatePayrollType(IList<Models.PayrollType> model, bool returnResultSet = false);

        Response Search(Models.PayrollType search);

        Response Search(IList<string> payrollTypeNames);

        bool IsValidPayroll(IList<string> payrollTypeNames, ref IList<string> payrollTypeNotExists, ref IList<Models.PayrollType> payrollTypes, ref List<MessageDetail> errorMessages);
    }
}
