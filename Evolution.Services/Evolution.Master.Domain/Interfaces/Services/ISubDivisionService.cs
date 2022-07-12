using Evolution.Common.Models.Messages;
using Evolution.Common.Models.Responses;
using System;
using System.Collections.Generic;
using System.Text;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;

namespace Evolution.Master.Domain.Interfaces.Services
{
  public  interface ISubDivisionService
    {
        Response Search(Models.SubDivision search);

        bool IsValidSubDivisionName(IList<string> names, ref IList<DbModel.Data> subDevisions, ref IList<ValidationMessage> valdMessages);
    }
}
