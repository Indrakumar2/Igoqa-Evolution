using Evolution.Common.Models.Responses;
using Evolution.Project.Domain.Models.Projects;

namespace Evolution.Project.Domain.Interfaces.Projects
{
    public interface IProjectDetailService
    {
        Response SaveProjectDetail(ProjectDetail projectDetail);

        Response UpdateProjectDetail(ProjectDetail projectDetail);

        Response DeleteProjectDetail(ProjectDetail projectDetail);
    }
}
