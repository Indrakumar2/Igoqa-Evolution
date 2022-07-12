using System;
using Evolution.TechnicalSpecialist.Domain.Models.TechSpecialist;
using System.Collections.Generic;
using Evolution.Common.Extensions;
using Evolution.DbRepository.Models;
using Evolution.ResourceSearch.Domain.Enums;
using Evolution.Google.Model.Models;

namespace Evolution.ResourceSearch.Domain.Models.ResourceSearch
{
    public class ResourceTechSpecSearchResult
    {
        public string Location { get; set; }
        public string Address { get; set; } // D - 912
        public bool IsFirstVisit { get; set; }
        public string SupplierLocationId { get; set; }
        public GeoCoordinateInfo SupplierGeoLocation { get; set; }
        public IList<ResourceSearchTechSpecInfo> ResourceSearchTechspecInfos { get; set; }
        public IList<ResourceSupplierInfo> SupplierInfo { get; set; }
        public IList<ResourceSearchTechSpecInfo> SearchExceptionResourceInfos { get; set; }
    }

    public class ResourceSupplierInfo
    {
        public string SupplierType { get; set; }
        public string SupplierName { get; set; }

    }

    public class BaseResourceSearchTechSpecInfo
    {
        public int Epin { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string ProfileStatus { get; set; }
        public bool? IsTechSpecFromAssignmentTaxonomy { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string Zip { get; set; }
        public GeoCoordinateInfo TechSpecGeoLocation { get; set; }
    }

    public class ResourceSearchTechSpecInfo : BaseResourceSearchTechSpecInfo
    {
        public int? EmploymentTypePrecedence
        {
            get
            {
                int? result = null;
                try
                {
                    var empType = !string.IsNullOrEmpty(EmploymentType) ? EmploymentType : Evolution.ResourceSearch.Domain.Enums.EmploymentType.None.ToString();
                    result = EnumExtension.ToIdByDisplayName<EmploymentType>(empType);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return result;
            }
        }

        public string EmploymentType { get; set; }
        public int? ScheduleStatusPrecedence
        {
            get
            {
                int? result = null;
                try
                {
                    result = EnumExtension.ToIdByDisplayName<ScheduleStatus>(ScheduleStatus);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return result;
            }
        }

        public string ScheduleStatus { get; set; }
        public string DistanceFromVenderInKm { get; set; }
        public double DistanceFromVenderInMile { get; set; }
        public bool IsSupplier { get; set; }
        public string MobileNumber { get; set; }
        public string Email { get; set; }
        public string SubDivision { get; set; }
        public bool IsSelected { get; set; }
        public string ExceptionComment { get; set; }

        public string GoogleAddress => PopulateAddress();

        private string PopulateAddress()
        {
            if (!string.IsNullOrEmpty(Country))
            {
                var requestQuery = Country;
                if (!string.IsNullOrEmpty(Zip))
                    requestQuery = Zip + ", " + requestQuery;
                // DEf 902 Fix
                if (!string.IsNullOrEmpty(State))
                    requestQuery = (State?.Split("-")?.Length == 2 && State?.Split("-")[0]?.Trim()?.Length == 2 ? State?.Split("-")[0] : State) + ", " + requestQuery; //IGO QC def 892 : taking only code from county when code and name is present in county value (ex NJ -New jersey => take only "NJ").

                if (!string.IsNullOrEmpty(City))
                    requestQuery = City + ", " + requestQuery;
                if (!string.IsNullOrEmpty(State) && !string.IsNullOrEmpty(City) && !string.IsNullOrEmpty(Zip))
                {
                    string removeZip = ", " + Zip;
                    requestQuery = requestQuery.Replace(removeZip, "")?.Trim();
                }
                return requestQuery;
            }
            else
            {
                return string.Empty;
            }
        }
    }
}
