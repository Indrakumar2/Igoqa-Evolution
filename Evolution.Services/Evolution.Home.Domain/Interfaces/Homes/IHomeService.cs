using Evolution.Common.Enums;
using Evolution.Common.Models.Responses;
using System.Collections.Generic;

namespace Evolution.Home.Domain.Interfaces.Homes
{
    public interface IHomeService
    {
        Response GetBudget(string companyCode = null, 
                            string userName = null, 
                            ContractStatus contractStatus = ContractStatus.All, 
                            bool isMyAssignmentOnly = true,
                            bool? IsOverBudgetValue = null, 
                            bool? IsOverBudgetHour = null);

        Response GetMyTask(string companyCode, string userName, ModuleCodeType moduleCodeType);

        Response GetMyTask(string companyCode, string userName);

        Response GetMyResourceSearch(string companyCode, string assignedToUser,bool isAllCoordinator);

        Response GetMyTaskAssignUsers(string companyCode);

        Response GetMyTaskReAssignUsers(string companyCode); //Added for ITK Defect 908(Ref ALM Document 14-05-2020)
    }
}
