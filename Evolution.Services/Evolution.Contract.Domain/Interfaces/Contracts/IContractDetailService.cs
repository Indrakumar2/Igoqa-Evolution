using Evolution.Common.Models.Responses;
using System;
using System.Collections.Generic;
using System.Text;

namespace Evolution.Contract.Domain.Interfaces.Contracts
{
    public interface IContractDetailService
    {
        Response SaveContractDetail(Models.Contracts.ContractDetail contractDetail);

        Response UpdateContractDetail(Models.Contracts.ContractDetail contractDetail);

        Response DeleteContractDetail(Models.Contracts.ContractDetail contractDetail);
    }
}
