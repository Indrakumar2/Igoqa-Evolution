using Evolution.Common.Models.Responses;
using Evolution.Master.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Evolution.Master.Domain.Interfaces.Services
{
    public interface ICustomerCommodityService
    {
        Response Search(Models.CustomerCommodity search);
    }
}
