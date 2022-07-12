//using System;
//using System.Collections.Generic;

//namespace Evolution.Security.Domain.Models.SqlDatabaseContext
//{
//    public partial class Activity
//    {
//        public Activity()
//        {
//            ModuleActivity = new HashSet<ModuleActivity>();
//            RoleActivity = new HashSet<RoleActivity>();
//        }

//        public int Id { get; set; }
//        public int ApplicationId { get; set; }
//        public string Code { get; set; }
//        public string Name { get; set; }
//        public string Description { get; set; }
//        public DateTime? LastModification { get; set; }
//        public byte? UpdateCount { get; set; }
//        public string ModifiedBy { get; set; }

//        public virtual Application Application { get; set; }
//        public virtual ICollection<ModuleActivity> ModuleActivity { get; set; }
//        public virtual ICollection<RoleActivity> RoleActivity { get; set; }
//    }
//}
