using Evolution.Common.Models.Messages;
using Evolution.Common.Models.Responses;
using System.Collections.Generic;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;

namespace Evolution.Master.Domain.Interfaces.Services
{
    public interface ITrainingsService
    {
        Response Search(Models.Trainings search);
        bool IsValidTraining(IList<string> trainingNames,
                                        ref IList<DbModel.Data> dbTrainings,
                                        ref IList<ValidationMessage> messages);
    }
}