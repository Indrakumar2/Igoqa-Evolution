using System.Collections.Generic;

namespace Evolution.Reports.Domain.Models.Reports
{
    public class ResourceDetails
    {
        public int ResourceId { get; set; }
        public string ResourceName { get; set; }
        public bool IsEmployee { get; set; }
        public bool IsContractor { get; set; }
        public string Company { get; set; }
        public int TaxonomyServiceId { get; set; }
        public string TaxonomyServiceName { get; set; }
        public string LastName { get; set; }
    }

    public class TaxonomyServices
    {
        public int TaxonomyServicesId { get; set; }
        public string TaxonomyServiceName { get; set; }
        public bool IsTaxonomyApplicable { get; set; }
    }

    public class ResourceTaxonomyServices : ResourceDetails
    {
        public ResourceTaxonomyServices()
        {
            ChildArray = new List<TaxonomyServices>();
        }
        public List<TaxonomyServices> ChildArray { get; set; }
        public int Count
        {
            get
            {
                return ChildArray.Count;
            }
        }
    }

    public class TaxonomyResourceServices : TaxonomyServices
    {
        public TaxonomyResourceServices()
        {
            ChildArray = new List<ResourceDetails>();
        }
        public List<ResourceDetails> ChildArray { get; set; }
        public int Count
        {
            get
            {
                return ChildArray.Count;
            }
        }
    }
}

