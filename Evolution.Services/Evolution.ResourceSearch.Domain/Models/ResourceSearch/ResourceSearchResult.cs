using Evolution.Common.Models.Base;
using System;
using System.Collections.Generic;

namespace Evolution.ResourceSearch.Domain.Models.ResourceSearch
{
    public class ResourceSearchResult : BaseModel
    {  
        //public int Id { get; set; }
        public string SearchAction { get; set; }
        public string SearchType { get; set; }
        public string Description { get; set; }
        public string DispositionType { get; set; }
        public string AssignedBy { get; set; }
        public string AssignedTo { get; set; }
        public string AssignedToOmLognName { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string SearchParameter { get; set; } 
        public string CustomerCode { get; set; }
        public int? AssignmentId { get; set; }
        public string CompanyCode { get; set; }
        public string CustomerName { get; set; }
        public string CategoryName { get; set; }
        public string SubCategoryName { get; set; }
        public string ServiceName { get; set; } 
        public IList<OverridenPreferredResource> OverridenPreferredResources { get; set; }
    }
}
