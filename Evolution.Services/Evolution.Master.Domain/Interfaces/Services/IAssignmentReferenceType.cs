using Evolution.Common.Models.Messages;
using Evolution.Common.Models.Responses;
using System;
using System.Collections.Generic;
using System.Text;
using DbModel=Evolution.DbRepository.Models.SqlDatabaseContext;

namespace Evolution.Master.Domain.Interfaces.Services
{
    public interface IAssignmentReferenceType:IMasterService
    {
        Response Search(Models.AssignmentReferenceType search);

       bool IsValidRefType(IList<string> refTypes, ref IList<DbModel.Data> dbRefType, ref IList<ValidationMessage> messages);
    }
}
