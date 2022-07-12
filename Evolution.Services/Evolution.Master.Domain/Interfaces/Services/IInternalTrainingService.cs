using Evolution.Common.Models.Messages;
using Evolution.Common.Models.Responses;
using System.Collections.Generic;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;

namespace Evolution.Master.Domain.Interfaces.Services
{
    public  interface IInternalTrainingService
    {
        Response Search(Models.InternalTraining search);

        bool IsValidInternalTraining(IList<string> internalTrainingNames,
                                         ref IList<DbModel.Data> dbInternalTrainings,
                                         ref IList<ValidationMessage> messages);
    }
}
