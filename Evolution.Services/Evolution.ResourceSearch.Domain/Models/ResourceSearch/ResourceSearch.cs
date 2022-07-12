using Evolution.Common.Models.Base;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Evolution.ResourceSearch.Domain.Models.ResourceSearch
{
    public class BaseResourceSearch : BaseModel
    {
        //public int Id { get; set; }
        public string SearchAction { get; set; }
        public string SearchType { get; set; }
        public string Description { get; set; }
        public string DispositionType { get; set; }
        public string AssignedBy { get; set; }
        public string AssignedTo { get; set; }
        public string CreatedBy { get; set; }
        public string CustomerCode { get; set; }
        public int? AssignmentId { get; set; }
        public string CompanyCode { get; set; }

        public string CategoryName { get; set; }
        public string SubCategoryName { get; set; }
        public string ServiceName { get; set; }

        public string AssignedToOmLognName { get; set; }

        public string MainSupplierName { get; set; }

        public string TaskType { get; set; }

        public DateTime? CreatedOn { get; set; }
    }

    public class ResourceSearch : BaseResourceSearch
    {
        // UserTypes will have value from Auth Token 
        public IList<string> UserTypes { get; set; }
        public int MyTaskId { get; set; }
        public bool? IsTechSpecFromAssignmentTaxonomy { get; set; }
        public IList<OverridenPreferredResource> OverridenPreferredResources { get; set; }
        public ResourceSearchParameter SearchParameter { get; set; }
    }

    public class ResourceSearchParameter
    {
        public string CustomerCode { get; set; }
        public string CustomerName { get; set; }
        public string ContractNumber { get; set; }
        public string ProjectName { get; set; }
        public string ProjectNumber { get; set; }
        public string AssignmentNumber { get; set; }
        public string PreAssignmentId { get; set; }

        public string CHCompanyCode { get; set; }
        public string CHCompanyName { get; set; }
        public string CHCoordinatorLogOnName { get; set; }

        public string OPCompanyCode { get; set; }
        public string OPCompanyName { get; set; }
        public string OPCoordinatorLogOnName { get; set; }

        public string CustomerContactPerson { get; set; }
        public string CustomerPhoneNumber1 { get; set; }
        public string CustomerPhoneNumber2 { get; set; }
        public string CustomerMobileNumber { get; set; }
        public string CustomerContactEmail { get; set; }

        public string MaterialDescription { get; set; }

        public string Supplier { get; set; }
        public int SupplierId { get; set; }
        // public string SupplierLocation { get; set; }
        private string _supplierLocation;
        public string SupplierLocation
        {
            get
            {  
                return _supplierLocation;
            }
            set
            {
                _supplierLocation =  Regex.Replace( value ?? string.Empty, "\n|\r", " ");
            }
        }
        private string _supplierFullAddress;
        public string SupplierFullAddress
        {
            get
            {
                return _supplierFullAddress;
            }
            set
            {
                _supplierFullAddress = Regex.Replace(value ?? string.Empty, "\n|\r", " ");
            }
        }
        public string SupplierPurchaseOrder { get; set; }

        public string AssignmentType { get; set; }
        public string AssignmentStatus { get; set; }
        public string WorkFlowType { get; set; }
        public DateTime? AssignmentCreatedDate { get; set; }

        public DateTime? FirstVisitFromDate { get; set; }
        public DateTime? FirstVisitToDate { get; set; }
        public string FirstVisitLocation { get; set; }
        public string FirstVisitStatus { get; set; }
        public int? FirstVisitSupplierId { get; set; }

        public string CategoryName { get; set; }
        public string SubCategoryName { get; set; }
        public string ServiceName { get; set; }

        public int ServiceId { get; set; }

        public IList<BaseResourceSearchTechSpecInfo> SelectedTechSpecInfo { get; set; }
        public ResourceSearchOptionalParameter OptionalSearch { get; set; }
        public IList<ResourceSearchSubSupplierInfo> SubSupplierInfos { get; set; }
        public IList<AssignedResourceInfo> AssignedResourceInfos { get; set; }
        public TaxonomyInfo PLOTaxonomyInfo { get; set; }
        public TaxonomyInfo OverrideTaxonomyInfo { get; set; }
    }

    public class OverridenPreferredResource : BaseModel
    {
        //public int Id { get; set; }
        public BaseResourceSearchTechSpecInfo TechSpecialist { get; set; }
        public bool? IsApproved { get; set; }
        public int ResourceSearchId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
    }

    public class SelectedTechSpec
    {
        public string Location { get; set; }
        public IList<BaseResourceSearchTechSpecInfo> TechSpecialist { get; set; }
    }

    public class ResourceSearchSubSupplierInfo
    {
        public int SubSupplierId { get; set; }
        public string SubSupplier { get; set; }
        
        private string _subSupplierLocation;
        public string SubSupplierLocation
        {
            get
            {  
                return _subSupplierLocation;
            }
            set
            {
                _subSupplierLocation =  Regex.Replace( value ?? string.Empty, "\n|\r", " ");
            }
        }
        private string _subSupplierFullAddress;
        public string SubSupplierFullAddress
        {
            get
            {
                return _subSupplierFullAddress;
            }
            set
            {
                _subSupplierFullAddress = Regex.Replace(value ?? string.Empty, "\n|\r", " ");
            }
        }
        public IList<BaseResourceSearchTechSpecInfo> SelectedSubSupplierTS { get; set; }
    }

    public class ResourceSearchOptionalParameter
    {
        //  public string EquipmentMaterialDescription { get; set; }
        public IList<string> EquipmentMaterialDescription { get; set; }
        public IList<string> CustomerApproval { get; set; } // Changes for D1465 
        public IList<string> Certification { get; set; }
        public DateTime? CertificationExpiryFrom { get; set; }
        public DateTime? CertificationExpiryTo { get; set; }
        public IList<string> LanguageSpeaking { get; set; }
        public IList<string> LanguageWriting { get; set; }
        public IList<string> LanguageComprehension { get; set; }
        public decimal? Radius { get; set; }
        public string SearchInProfile { get; set; }
    }

    public class AssignedResourceInfo
    {
        public IList<BaseResourceSearchTechSpecInfo> AssignedTechSpec { get; set; }
        public string SupplierName { get; set; }
        public int SupplierId { get; set; }
        public string TaxonomyServiceName { get; set; }
    }

    public class TaxonomyInfo
    {
        public string CategoryName { get; set; }
        public string SubCategoryName { get; set; }
        public string ServiceName { get; set; }
    }
}
