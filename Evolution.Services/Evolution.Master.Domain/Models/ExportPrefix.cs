using Evolution.Common.Models.Base;

namespace Evolution.Master.Domain.Models
{
    public class ExportPrefix : BaseModel
    {
        public string Name { get; set; }

        public bool? IsActive { get; set; }
    }
}
