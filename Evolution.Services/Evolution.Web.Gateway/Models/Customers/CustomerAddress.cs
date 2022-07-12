using System;
using System.Collections.Generic;

namespace Evolution.Web.Gateway.Models.Customers
{
    public class Base
    {
        public byte? UpdateCount { get; set; }
        
        public string ModifiedBy { get; set; }
        
        public DateTime? LastModifiedOn { get; set; }
        
        public string RecordStatus { get; set; }
    }

    public class CustomerDetail : Base
    {   
        public string CustomerCode { get; set; }
        
        public string CustomerName { get; set; }
        
        public string ParentCompanyName { get; set; }
                
        public int? MIIWAId { get; set; }

        public int? MIIWAParentId { get; set; }
        
        public string OperatingCountry { get; set; }
        
        public string Active { get; set; }
    }

    public class AddressDetail : Base
    {
        public string Address { get; set; }

        public string Country { get; set; }

        public string County { get; set; }

        public string City { get; set; }

        public string PostalCode { get; set; }

        public string EUVatPrefix { get; set; }

        public string VatTaxRegNumber { get; set; }

        public int AddressId { get; set; }

        public string CustomerCode { get; set; }

        public int? CountryId { get; set; } //Changes for D1536

        public int? StateId { get; set; } //Changes for D1536

        public int? CityId { get; set; } //Changes for D1536

        public IList<Contact> Contacts { get; set; }
    }

    public class Contact : Base
    {        
        public int ContactId { get; set; }
        
        public int CustomerAddressId { get; set; }
        
        public string Salutation { get; set; }
        
        public string Position { get; set; }
        
        public string ContactPersonName { get; set; }
        
        public string Landline { get; set; }
        
        public string Fax { get; set; }
        
        public string Mobile { get; set; }

        public string Email { get; set; }
        
        public string OtherDetail { get; set; } 

        public string LogonName { get; set; }

        public bool IsPortalUser { get; set; }

        public ExtranetUserInfo UserInfo { get; set; }
    }

    public class AssignmentReference : Base
    {
        public string CustomerCode { get; set; }

        public int CustomerAssignmentReferenceId { get; set; }

        public string AssignmentRefType { get; set; }
    }

    public class CompanyAccountReference : Base
    {
        public string CustomerCode { get; set; }

        public int CustomerCompanyAccountReferenceId { get; set; }
        
        public string AccountReferenceValue { get; set; }

        public string CompanyCode { get; set; }
    }

    public class CustomerNote : Base
    {
        public string CustomerCode { get; set; }

        public string Note { get; set; }
        
        public string CreatedBy { get; set; }
        
        public DateTime? CreatedOn { get; set; }
        
        public int CustomerNoteId { get; set; }
    }

    public class Document : Base
    {
        public string CustomerCode { get; set; }

        public int DocumentId { get; set; }

        public string Name { get; set; }

        public string DocumentType { get; set; }

        public string VisibleToTS { get; set; }

        public string VisibleToCustomer { get; set; }

        public long? DocumentSize { get; set; }

        public DateTime? UploadedOn { get; set; }
    }

    public class ExtranetUserInfo  
    {
        public int CustomerAddressId { get; set; }
        public int ContactId { get; set; }
        public int? UserId { get; set; }
         
        public string ApplicationName { get; set; }
         
        public string UserName { get; set; }
         
        public string LogonName { get; set; }
         
        public string Email { get; set; } 
        
        public string Password { get; set; }
         
        public string PhoneNumber { get; set; } 
         
        public bool IsAccountLocked { get; set; }
         
        public DateTime? LockoutEndDateUtc { get; set; }
         
        public string CompanyCode { get; set; }
         
        public string CompanyName { get; set; }
         
        public string CompanyOfficeName { get; set; }
         
        public string AuthenticationMode { get; set; }
         
        public bool IsActive { get; set; } = true;
          
        public bool IsPasswordNeedToBeChange { get; set; }
         
        public string Culture { get; set; }
         
        public string UserType { get; set; }
         
        public string SecurityQuestion1 { get; set; }
         
        public string SecurityQuestion1Answer { get; set; }
         
        public bool IsShowNewVisit { get; set; }
         
        public string ExtranetAccessLevel { get; set; }
         
        public string Comments { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? LastLoginDate { get; set; }

        public Byte? UpdateCount { get; set; }

        public IList<string> DefaultCompanyUserType { get; set; }
        public IList<CustomerUserProject> CustomerUserProjectNumbers { get; set; }

    }

    public class CustomerUserProject  
    {
        public int Id { get; set; }
        public int? UserId { get; set; }
        public int ProjectNumber { get; set; }
    }
}
