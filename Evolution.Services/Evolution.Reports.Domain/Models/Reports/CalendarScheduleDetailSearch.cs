using Evolution.DbRepository.Models.SqlDatabaseContext;
using System;
using System.Collections.Generic;

namespace Evolution.Reports.Domain.Models.Reports
{
    public class CalendarScheduleDetailSearch
    {
        public string CompanyID { get; set; }

        public int Year { get; set; }

        public int Month { get; set; }

        public string CustomerName { get; set; }

        public string SupplierName { get; set; }

        public string ProjectNo { get; set; }

        public string AssignmentNo { get; set; }

        public List<string> CHCoordinator { get; set; }

        public List<string> OCCoordinator { get; set; }

        public string ResourceName { get; set; }

        public List<int> ResourceEpins { get; set; }

        public List<int> EPins { get; set; }
    }

    public class CalendarScheduleDetail
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string VisitTimesheetStatus { get; set; }

        public string ResourceName
        {
            get
            {
                return string.Format("{0}, {1}", LastName, FirstName);
            }
        }

        public string Company { get; set; }

        public string CHCoordinator { get; set; }

        public string OCCoordinator { get; set; }

        public int? ProjectNo { get; set; }

        public int? AssignmentNo { get; set; }

        public string EVONo { get; set; }

        //public string EVONo {
        //    get {

        //        return string.Format("{0}-{1}", ProjectNo, AssignmentNo);
        //    }
        //}

        public DateTime? FirstVisitTimesheetStartDate { get; set; }

        public DateTime? FromDate { get; set; }

        public DateTime? ToDate { get; set; }

        public DateTime? StartDateTime { get; set; }

        public DateTime? EndDateTime { get; set; }

        public DateTime? ActualDate { get; set; }

        public DateTime? AssignmentCreationDate { get; set; }

        //public string AssignmentCreationDate
        //{
        //    get
        //    {
        //        if (FirstVisitTimesheetStartDate.HasValue)
        //           return string.Format("{0:dd-MMM-yyyy}", FirstVisitTimesheetStartDate.Value);
        //        else
        //           return string.Empty;
        //    }
        //}

        public string VisitTimesheetStartDate
        {
            get
            {
                return string.Format("{0:dd-MMM-yyyy}", FromDate);
            }
        }

        public string VisitTimesheetEndData
        {
            get
            {
                if (ToDate.HasValue)
                    return string.Format("{0:dd-MMM-yyyy}", ToDate.Value);
                else
                    return string.Empty;
            }
        }

        public string ResourceVisitDate
        {
            get
            {
                if (StartDateTime.HasValue)
                    return string.Format("{0:dd-MMM-yyyy}", StartDateTime.Value);
                else
                    return string.Empty;
            }
        }

        public string AllocatedTime
        {
            get
            {
                if (StartDateTime.HasValue && EndDateTime.HasValue)
                    return DateTime.Parse(string.Format("{0:t}", StartDateTime)).ToString("hh:mm tt") + " - " + DateTime.Parse(string.Format("{0:t}", EndDateTime)).ToString("hh:mm tt"); //D-1385
                else
                    return "";
            }
        }

        public string EmploymentStatus { get; set; }

        public string CustomerName { get; set; }

        public string SupplierName { get; set; }

        public int? SupplierID { get; set; }

        public string SCity { get; set; }

        public string CountryName { get; set; }

        public string PostalCode { get; set; }

        public string SupplierLocation { get; set; }

        //public string SupplierLocation
        //{
        //    get
        //    {
        //        string location = string.Empty;
        //        if (!string.IsNullOrEmpty(SCity))
        //           location = SCity + ", ";
        //        if (!string.IsNullOrEmpty(CountryName))
        //            location += CountryName + ", ";
        //        if (!string.IsNullOrEmpty(PostalCode))
        //            location += PostalCode;
        //        else
        //            location = location.Trim().TrimEnd(',');
        //        return location;
        //    }

        //}

        public int EPIN { get; set; }

        public string SubDivision { get; set; }

        public Int64 Id { get; set; }

        public int TechSpecID { get; set; }

        public string Notes { get; set; }

        public string EmploymentType { get; set; }
    }
}