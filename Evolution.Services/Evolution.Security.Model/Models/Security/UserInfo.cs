using Evolution.Common.Models.Base;
using System;
using System.Collections.Generic;

namespace Evolution.Security.Domain.Models.Security
{
    public class UserInfo : BaseModel
    {
        public int? UserId { get; set; }

        [AuditNameAttribute("Application Name")]
        public string ApplicationName { get; set; }

        [AuditNameAttribute("Name")]
        public string UserName { get; set; }

        [AuditNameAttribute("Logon Name")]
        public string LogonName { get; set; }

        [AuditNameAttribute("Email")]
        public string Email { get; set; }
        //public bool IsEmailConfirmed { get; set; }

        [AuditNameAttribute("Password")]
        public string Password { get; set; }

        [AuditNameAttribute("Phone Number")]
        public string PhoneNumber { get; set; }
        //public bool IsPhoneNumberConfirrmed { get; set; }

        [AuditNameAttribute("Account Locked")]
        public bool IsAccountLocked { get; set; }

        [AuditNameAttribute("Lockout End Date")]
        public DateTime? LockoutEndDateUtc { get; set; }

        [AuditNameAttribute("Company Code")]
        public string CompanyCode { get; set; }

        [AuditNameAttribute("Company Name")]
        public string CompanyName { get; set; }

        [AuditNameAttribute("Company Office")]
        public string CompanyOfficeName { get; set; }

        [AuditNameAttribute("Application Mode")]
        public string AuthenticationMode { get; set; }

        [AuditNameAttribute("Active")]
        public bool IsActive { get; set; } = true;

        [AuditNameAttribute("Password Need To Be Change")]
        public bool IsPasswordNeedToBeChange { get; set; }

        [AuditNameAttribute("Culture")]
        public string Culture { get; set; }

        [AuditNameAttribute("User Type")]
        public string UserType { get; set; }

        [AuditNameAttribute("Security Question")]
        public string SecurityQuestion1 { get; set; }

        [AuditNameAttribute("Answer")]
        public string SecurityQuestion1Answer { get; set; }

        [AuditNameAttribute("Show New Visit")]
        public bool IsShowNewVisit { get; set; }
         
        [AuditNameAttribute("Extranet Access Level")]
        public string ExtranetAccessLevel { get; set; }

        [AuditNameAttribute("Comments")]
        public string Comments { get; set; }

        [AuditNameAttribute("Created Date")]
        public DateTime CreatedDate { get; set; }

        [AuditNameAttribute("Last Login Date")]
        public DateTime? LastLoginDate { get; set; }

        public IList<string> DefaultCompanyUserType { get; set; }
        public IList<CustomerUserProject> CustomerUserProjectNumbers { get; set; }

    }

    public class CustomerUserProject: BaseModel
    {
        //public int Id { get; set; }
        public int? UserId { get; set; }
        public int ProjectNumber { get; set; }
    }

    public class UserInfos : BaseModel
    {
        public int? UserId { get; set; }

        public string ApplicationName { get; set; }
        public string UserName { get; set; }
        public string LogonName { get; set; }
        public string Email { get; set; }
        //public bool IsEmailConfirmed { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        //public bool IsPhoneNumberConfirrmed { get; set; }
        public bool IsAccountLocked { get; set; }
        public DateTime? LockoutEndDateUtc { get; set; }
        public string CompanyCode { get; set; }
        public string CompanyName { get; set; }
        public string CompanyOfficeName { get; set; }
        public string AuthenticationMode { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsPasswordNeedToBeChange { get; set; }
        public string Culture { get; set; }
        public IList<string> UserType { get; set; }
        public string SecurityQuestion1 { get; set; }
        public string SecurityQuestion1Answer { get; set; }
        public bool iSUserTypeRequired = true;
        public DateTime CreatedDate { get; set; }
        public DateTime? LastLoginDate { get; set; }
    }

    public class ExtranetUserInfo : UserInfo
    {
        public int CustomerAddressId { get; set; }
        public int ContactId { get; set; }
    }
}