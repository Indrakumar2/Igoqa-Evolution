using Evolution.Common.Models.Responses;
using System.Collections.Generic;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;

namespace Evolution.Contract.Domain.Interfaces.Contracts
{
    public interface IContractNoteService
    { 
        Response SaveContractNote(string contractNumber, IList<Models.Contracts.ContractNote> contractNotes, bool commitchange=true, bool isResultSetRequired = false);

        Response SaveContractNote(string contractNumber, IList<Models.Contracts.ContractNote> contractNotes, IList<DbModel.Contract> dbContract, IList<DbModel.SqlauditModule> dbModule, bool commitChange = true, bool isResultSetRequired = false);
        Response GetContractNote(Models.Contracts.ContractNote searchModel);

        Response DeleteContractNote(string contractNumber, IList<Models.Contracts.ContractNote> contractNotes, bool commitChange = true, bool isResultSetRequired = false);

        Response ModifyContractNote(string contractNumber, IList<Models.Contracts.ContractNote> contractNotes, bool commitchange = true, bool isResultSetRequired = false);        //D661 issue 8 

    }
}
