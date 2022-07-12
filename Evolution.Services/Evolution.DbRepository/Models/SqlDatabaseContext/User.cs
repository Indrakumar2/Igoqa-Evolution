using System;
using System.Collections.Generic;

namespace Evolution.DbRepository.Models.SqlDatabaseContext
{
    public partial class User
    {
        public User()
        {
            AssignmentContractCompanyCoordinator = new HashSet<Assignment>();
            AssignmentOperatingCompanyCoordinator = new HashSet<Assignment>();
            CustomerUserProjectAccess = new HashSet<CustomerUserProjectAccess>();
            Document = new HashSet<Document>();
            ProjectCoordinator = new HashSet<Project>();
            ProjectManagedServicesCoordinator = new HashSet<Project>();
            TaskAssignedBy = new HashSet<Task>();
            TaskAssignedTo = new HashSet<Task>();
            TechnicalSpecialistCertificationAndTraining = new HashSet<TechnicalSpecialistCertificationAndTraining>();
            TechnicalSpecialistPendingWith = new HashSet<TechnicalSpecialist>();
            TechnicalSpecialistUser = new HashSet<TechnicalSpecialist>();
            UserRole = new HashSet<UserRole>();
            UserType = new HashSet<UserType>();
        }

        public int Id { get; set; }
        public int ApplicationId { get; set; }
        public string Name { get; set; }
        public string SamaccountName { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public string PasswordHash { get; set; }
        public string SecurityStamp { get; set; }
        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public bool LockoutEnabled { get; set; }
        public DateTime? LockoutEndDateUtc { get; set; }
        public int? AccessFailedCount { get; set; }
        public int? CompanyId { get; set; }
        public int? CompanyOfficeId { get; set; }
        public bool? IsActive { get; set; }
        public string Culture { get; set; }
        public DateTime? LastModification { get; set; }
        public byte? UpdateCount { get; set; }
        public string ModifiedBy { get; set; }
        public string AuthenticationMode { get; set; }
        public bool IsPasswordNeedToBeChange { get; set; }
        public string SecurityQuestion1 { get; set; }
        public string SecurityQuestion1Answer { get; set; }
        public string Comments { get; set; }
        public bool? IsErepTrained { get; set; }
        public string ExtranetAccessLevel { get; set; }
        public bool? IsShowNewVisit { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? LastLoginDate { get; set; }

        public virtual Application Application { get; set; }
        public virtual Company Company { get; set; }
        public virtual CompanyOffice CompanyOffice { get; set; }
        public virtual ICollection<Assignment> AssignmentContractCompanyCoordinator { get; set; }
        public virtual ICollection<Assignment> AssignmentOperatingCompanyCoordinator { get; set; }
        public virtual ICollection<CustomerUserProjectAccess> CustomerUserProjectAccess { get; set; }
        public virtual ICollection<Document> Document { get; set; }
        public virtual ICollection<Project> ProjectCoordinator { get; set; }
        public virtual ICollection<Project> ProjectManagedServicesCoordinator { get; set; }
        public virtual ICollection<Task> TaskAssignedBy { get; set; }
        public virtual ICollection<Task> TaskAssignedTo { get; set; }
        public virtual ICollection<TechnicalSpecialistCertificationAndTraining> TechnicalSpecialistCertificationAndTraining { get; set; }
        public virtual ICollection<TechnicalSpecialist> TechnicalSpecialistPendingWith { get; set; }
        public virtual ICollection<TechnicalSpecialist> TechnicalSpecialistUser { get; set; }
        public virtual ICollection<UserRole> UserRole { get; set; }
        public virtual ICollection<UserType> UserType { get; set; }
    }
}
