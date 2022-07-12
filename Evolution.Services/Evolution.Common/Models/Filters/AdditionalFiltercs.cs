namespace Evolution.Common.Models.Filters
{
    public class AdditionalFilter
    {
        public bool IsRecordCountOnly { get; set; }

        public int SkipRecordCount { get; set; }

        public int NextRecordCount { get; set; }

        public bool IsInvoiceDetailRequired { get; set; }
    }
}
