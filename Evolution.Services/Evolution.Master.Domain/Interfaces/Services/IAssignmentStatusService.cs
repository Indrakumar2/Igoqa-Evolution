using Evolution.Common.Models.Responses;
using System;
using System.Collections.Generic;
using System.Text;

namespace Evolution.Master.Domain.Interfaces.Services
{
    public interface IAssignmentStatusService:IMasterService
    {
        Response Search(Models.AssignmentStatus search);
    }
}
