namespace Evolution.Master.Domain.Models
{
    public  class TaxonomyService:BaseMasterModel
    {
        public string TaxonomyCategory { get; set; }
        public string TaxonomySubCategory { get; set; }
        public string TaxonomyServiceName { get; set; }
        public int TaxonomySubCategoryId { get; set; }
        public int TaxonomyCategoryId { get; set; }
    }
}
