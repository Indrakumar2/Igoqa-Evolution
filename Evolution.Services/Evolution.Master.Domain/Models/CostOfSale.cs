namespace Evolution.Master.Domain.Models
{
    public class CostOfSale : BaseMasterModel
    {
        public string ChargeType { get; set; }

        public string ChargeReference { get; set; }

        public string Description { get; set; }

        public string PayReference { get; set; } //Added for D1089 (Ref ALM 22-05-2020 Doc)
    }
}
