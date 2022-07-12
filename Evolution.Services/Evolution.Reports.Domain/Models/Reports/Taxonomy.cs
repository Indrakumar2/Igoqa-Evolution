using Evolution.DbRepository.Models.SqlDatabaseContext;
using Evolution.Reports.Domain.Models.Reports;
using System;
using System.Collections.Generic;
using System.Text;

namespace Evolution.Reports.Domain.Models.Reports
{
    public class Taxonomy : TaxonomySearch
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string SubDivision { get; set; }
        public string HardStamp
        {
            get
            {
                if (!string.IsNullOrEmpty(HardStampCountryCode) && !string.IsNullOrEmpty(HardStampNumber))
                    return HardStampCountryCode + "-" + HardStampNumber;
                else if (!string.IsNullOrEmpty(HardStampCountryCode) && string.IsNullOrEmpty(HardStampNumber))
                    return HardStampCountryCode;
                else if (string.IsNullOrEmpty(HardStampCountryCode) && !string.IsNullOrEmpty(HardStampNumber))
                    return HardStampNumber;
                else
                    return string.Empty;
            }
        }
        public string SoftStamp
        {
            get
            {
                if (!string.IsNullOrEmpty(SoftStampCountryCode) && !string.IsNullOrEmpty(SoftStampNumber))
                    return SoftStampCountryCode + "-" + SoftStampNumber;
                else if (!string.IsNullOrEmpty(SoftStampCountryCode) && string.IsNullOrEmpty(SoftStampNumber))
                    return SoftStampCountryCode;
                else if (string.IsNullOrEmpty(SoftStampCountryCode) && !string.IsNullOrEmpty(SoftStampNumber))
                    return SoftStampNumber;
                else
                    return string.Empty;
            }
        }
        public string City { get; set; }
        public string County { get; set; }
        public string Country { get; set; }
        public string RAMP { get; set; }
        public string EReporting { get; set; }
        public string ResourceName
        {
            get
            {
                return string.Format("{0}, {1}", LastName, FirstName);
            }
        }
        public string SoftStampNumber { get; set; }
        public string HardStampNumber { get; set; }
        public string SoftStampCountryCode { get; set; }
        public string HardStampCountryCode { get; set; }
        public string CompanyName { get; set; }
        public int EPin { get; set; }
        public int CompanyId { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public string PostalCode { get; set; }
    }

    public class TaxonomySearch
    {
        public List<int> CompanyIds { get; set; }
        public List<int> ResourceEpins { get; set; }
        public List<int> EpinList { get; set; }
        public string CompanyCode { get; set; }
        public string ProfileStatus { get; set; }
        public string EmploymentStatus { get; set; }
        public string ApprovalStatus { get; set; }
        public string Category { get; set; }
        public string SubCategory { get; set; }
        public string Service { get; set; }

    }
}