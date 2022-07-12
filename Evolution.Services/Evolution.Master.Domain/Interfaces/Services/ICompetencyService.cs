using Evolution.Common.Models.Messages;
using Evolution.Common.Models.Responses;
using System.Collections.Generic;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;

namespace Evolution.Master.Domain.Interfaces.Services
{
    public interface ICompetencyService
    {
        Response Search(Models.Competency search);

        bool IsValidCompetency(IList<string> competencyNames,
                                       ref IList<DbModel.Data> dbCompetencys,
                                       ref IList<ValidationMessage> messages);
    }
}
