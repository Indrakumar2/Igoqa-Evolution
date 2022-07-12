using System;
using System.Collections.Generic;

namespace Evolution.DbRepository.Models.SqlDatabaseContext
{
    public partial class UserTemp
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string SamaccountName { get; set; }
        public string Email { get; set; }
        public int? CompanyId { get; set; }
        public int? CompanyOfficeId { get; set; }
        public bool? IsActive { get; set; }
        public string Culture { get; set; }
        public DateTime? LastModification { get; set; }
        public byte? UpdateCount { get; set; }
        public string ModifiedBy { get; set; }
        public int? ApplicationId { get; set; }
        public bool? EmailConfirmed { get; set; }
        public string PasswordHash { get; set; }
        public string SecurityStamp { get; set; }
        public string PhoneNumber { get; set; }
        public long? LockoutEnabled { get; set; }
        public bool? PhoneNumberConfirmed { get; set; }
        public DateTime? LockoutEndDateUtc { get; set; }
        public int? AccessFailedCount { get; set; }
        public string AuthenticationMode { get; set; }
        public string UserType { get; set; }

        public virtual Application Application { get; set; }
        public virtual Company Company { get; set; }
        public virtual CompanyOffice CompanyOffice { get; set; }
    }
}
