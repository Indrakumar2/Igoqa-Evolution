using Evolution.Common.Models.Notes;

namespace Evolution.Project.Domain.Models.Projects
{
    public class ProjectNote : Note
    {
        [AuditNameAttribute("Project Note Id")]
        public int? ProjectNoteId { get; set; }
        
        [AuditNameAttribute("Project Number")]
        public int? ProjectNumber { get; set; }
    }
}
