using Evolution.Common.Models.Responses;
using System;
using System.Collections.Generic;
using System.Text;

namespace Evolution.Master.Domain.Interfaces.Services
{
    public interface ICompanyChargeSchedule : IMasterService
    {
        Response Search(Models.CompanyChargeSchedule search);
    }
}
