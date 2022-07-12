namespace Evolution.Master.Domain.Models
{
    public class TaxonomySubCategory : BaseMasterModel
    {
        public string TaxonomyCategory { set; get; }
        public string TaxonomySubCategoryName { get; set; }
        public int TaxonomyCategoryId { set; get; }
    }

}
