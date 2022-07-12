using System;
using System.Collections.Generic;
using System.Text;

namespace Evolution.Reports.Domain.Models.Reports
{
    public class WonLost : WonLostSearch
    {

        public int Id { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? CreatedOn { get; set; }

        public string SearchParameter { get; set; }

        public string CustomerCode { get; set; }

        public string CategoryName { get; set; }

        public string SubCategoryName { get; set; }

        public string ServiceName { get; set; }

        public string Description { get; set; }


    }

    public class WonLostSearch
    {
        public DateTime? FromDate { get; set; }

        public DateTime? ToDate { get; set; }

        public string CompanyCode { get; set; }

        public string AssginedTo { get; set; }

        public string Action { get; set; }

        public string SearchType { get; set; }

        public string DispositionType { get; set; } //D769
    }
}