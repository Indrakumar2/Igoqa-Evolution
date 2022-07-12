using Evolution.Common.Models.Messages;
using Evolution.Common.Models.Responses;
using Evolution.Master.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Evolution.Master.Domain.Interfaces.Services
{
    public interface IDivisionService : IMasterService
    {
        Response SaveDivision(IList<Models.Division> model,bool returnResultSet=false);
        
        Response Search(Models.Division search);

        Response Search(IList<string> divisionNmes);

        bool IsValidDivision(IList<string> divisionNames, ref IList<string> divisionNotExist, ref IList<Division> divisions, ref List<MessageDetail> errorMessages);

        Response Delete(IList<int> divisionIds);
    }
}
