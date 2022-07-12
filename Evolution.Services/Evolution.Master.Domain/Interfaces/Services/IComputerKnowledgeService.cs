using Evolution.Common.Models.Messages;
using Evolution.Common.Models.Responses;
using System.Collections.Generic;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;

namespace Evolution.Master.Domain.Interfaces.Services
{
  public  interface IComputerKnowledgeService
    {
        Response Search(Models.ComputerKnowledge search);
        bool IsValidComputerKnowledgeName(IList<string> names, ref IList<DbModel.Data> dbcomputerKnowledge, ref IList<ValidationMessage> validMessages);
    }
}
