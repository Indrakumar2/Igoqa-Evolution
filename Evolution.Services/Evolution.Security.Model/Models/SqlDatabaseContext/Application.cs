//using System;
//using System.Collections.Generic;

//namespace Evolution.Security.Domain.Models.SqlDatabaseContext
//{
//    public partial class Application
//    {
//        public Application()
//        {
//            Activity = new HashSet<Activity>();
//            ApplicationMenu = new HashSet<ApplicationMenu>();
//            Module = new HashSet<Module>();
//            Role = new HashSet<Role>();
//            User = new HashSet<User>();
//        }

//        public int Id { get; set; }
//        public string Name { get; set; }
//        public string Description { get; set; }
//        public DateTime? LastModification { get; set; }
//        public byte? UpdateCount { get; set; }
//        public string ModifiedBy { get; set; }

//        public virtual ICollection<Activity> Activity { get; set; }
//        public virtual ICollection<ApplicationMenu> ApplicationMenu { get; set; }
//        public virtual ICollection<Module> Module { get; set; }
//        public virtual ICollection<Role> Role { get; set; }
//        public virtual ICollection<User> User { get; set; }
//    }
//}
