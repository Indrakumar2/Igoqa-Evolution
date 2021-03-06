using Evolution.Common.Models.Responses;
using System;
using System.Collections.Generic;
using System.Text;
using DomainModel = Evolution.Reports.Domain.Models.Reports;

namespace Evolution.Reports.Domain.Interfaces.Reports
{
    public interface IWonLostService
    {
        Response Get(DomainModel.WonLost searchModel);
    }
}
