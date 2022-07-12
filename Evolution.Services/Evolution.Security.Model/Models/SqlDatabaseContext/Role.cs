//using System;
//using System.Collections.Generic;

//namespace Evolution.Security.Domain.Models.SqlDatabaseContext
//{
//    public partial class Role
//    {
//        public Role()
//        {
//            RoleActivity = new HashSet<RoleActivity>();
//            UserRole = new HashSet<UserRole>();
//        }

//        public int Id { get; set; }
//        public int ApplicationId { get; set; }
//        public string Name { get; set; }
//        public string Description { get; set; }
//        public DateTime? LastModification { get; set; }
//        public byte? UpdateCount { get; set; }
//        public string ModifiedBy { get; set; }

//        public virtual Application Application { get; set; }
//        public virtual ICollection<RoleActivity> RoleActivity { get; set; }
//        public virtual ICollection<UserRole> UserRole { get; set; }
//    }
//}
