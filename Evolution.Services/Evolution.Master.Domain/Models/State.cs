namespace Evolution.Master.Domain.Models
{
    public class State : BaseMasterModel
    {
        public string Country { set; get; }

        public int? CountryId { get; set; }//Added for D-1076
    }
}
