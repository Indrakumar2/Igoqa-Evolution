//using System;
//using System.Collections.Generic;

//namespace Evolution.Security.Domain.Models.SqlDatabaseContext
//{
//    public partial class User
//    {
//        public User()
//        {
//            UserRole = new HashSet<UserRole>();
//        }

//        public int Id { get; set; }
//        public int ApplicationId { get; set; }
//        public string Name { get; set; }
//        public string SamaccountName { get; set; }
//        public string Email { get; set; }
//        public int? CompanyId { get; set; }
//        public int? CompanyOfficeId { get; set; }
//        public bool? IsActive { get; set; }
//        public string Culture { get; set; }
//        public DateTime? LastModification { get; set; }
//        public byte? UpdateCount { get; set; }
//        public string ModifiedBy { get; set; }

//        public virtual Application Application { get; set; }
//        public virtual ICollection<UserRole> UserRole { get; set; }
//    }
//}
