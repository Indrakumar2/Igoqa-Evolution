using Evolution.Common.Enums;
using Evolution.Common.Models.Responses;
using System.Collections.Generic; 
using DomainModel = Evolution.Home.Domain.Models.Homes;

namespace Evolution.Home.Domain.Interfaces.Homes
{
    public interface IMyTaskService
    {
        Response Get(DomainModel.MyTask searchModel);

        Response Get(string taskType, IList<string> taskRefCode);

        Response Get(IList<string> moduleType, IList<string> assignedTo, IList<string> assignedBy = null, IList<string> taskRefCode=null);

        Response Get(IList<string> moduleType, IList<string> taskRefCode);

        Response Add(IList<DomainModel.MyTask>  myTasks, bool commitChange = true, bool isDbValidationRequired = true);

        Response Add(IList<DomainModel.MyTask> myTasks, ref IList<DbRepository.Models.SqlDatabaseContext.Task> dbMyTasks, ref long? eventId, bool commitChange = true, bool isDbValidationRequired = true);

        Response Modify(IList<DomainModel.MyTask> myTasks, bool commitChange = true, bool isDbValidationRequired = true);

        Response Modify(IList<DomainModel.MyTask> myTasks, ref IList<DbRepository.Models.SqlDatabaseContext.Task> dbMyTasks, bool commitChange = true, bool isDbValidationRequired = true);

        Response Delete(IList<DomainModel.MyTask> myTasks, bool commitChange = true, bool isDbValidationRequired = true);

        Response IsRecordValidForProcess(IList<DomainModel.MyTask> myTasks, ValidationType validationType);

        Response IsRecordValidForProcess(IList<DomainModel.MyTask> myTasks, ValidationType validationType, ref IList<DbRepository.Models.SqlDatabaseContext.Task> dbMyTasks);

        Response IsRecordValidForProcess(IList<DomainModel.MyTask> myTasks, ValidationType validationType, IList<DbRepository.Models.SqlDatabaseContext.Task> dbMyTasks);

        Response ModifyResourceSearchOnReassign(IList<Home.Domain.Models.Homes.MyTask> myTasks, bool commitChange = true);
    }
}
