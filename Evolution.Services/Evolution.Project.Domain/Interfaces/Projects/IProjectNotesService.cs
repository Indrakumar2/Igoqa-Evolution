using Evolution.Common.Models.Responses;
using Evolution.Project.Domain.Models.Projects;
using System.Collections.Generic;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;

namespace Evolution.Project.Domain.Interfaces.Projects
{
    public interface IProjectNotesService 
    { 
        Response GetProjectNotes(ProjectNote searchModel);
         
        Response SaveProjectNotes(int projectNumebr,IList<ProjectNote> notes, bool commitChange = true, bool isResultSetRequired = false);

        Response SaveProjectNotes(int projectNumebr, IList<ProjectNote> notes, IList<DbModel.Project> dbProjects, IList<DbModel.SqlauditModule> dbModule, bool commitChange = true, bool isResultSetRequired = false);

        Response DeleteProjectNotes(int projectNumebr, IList<ProjectNote> notes, bool commitChange = true, bool isResultSetRequired = false);

        Response ModifyProjectNotes(int projectNumber, IList<ProjectNote> notes, bool commitChanges = true, bool isResultSetRequired = false);        //D661 issue 8 

    }
}
