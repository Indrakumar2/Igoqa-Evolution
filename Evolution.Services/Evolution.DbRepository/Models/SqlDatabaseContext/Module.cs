using System;
using System.Collections.Generic;

namespace Evolution.DbRepository.Models.SqlDatabaseContext
{
    public partial class Module
    {
        public Module()
        {
            ApplicationMenu = new HashSet<ApplicationMenu>();
            ModuleActivity = new HashSet<ModuleActivity>();
            NumberSequenceModule = new HashSet<NumberSequence>();
            //NumberSequenceModuleRef = new HashSet<NumberSequence>();
            RoleActivity = new HashSet<RoleActivity>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime? LastModification { get; set; }
        public byte? UpdateCount { get; set; }
        public string ModifiedBy { get; set; }
        public int ApplicationId { get; set; }

        public virtual Application Application { get; set; }
        public virtual ICollection<ApplicationMenu> ApplicationMenu { get; set; }
        public virtual ICollection<ModuleActivity> ModuleActivity { get; set; }
        public virtual ICollection<NumberSequence> NumberSequenceModule { get; set; }
        //public virtual ICollection<NumberSequence> NumberSequenceModuleRef { get; set; }
        public virtual ICollection<RoleActivity> RoleActivity { get; set; }
    }
}
