using System;
using System.Collections.Generic;

namespace Evolution.DbRepository.Models.SqlDatabaseContext
{
    public partial class RevenueAccrual
    {
        public int Id { get; set; }
        public string ParentCompany { get; set; }
        public string ContractHoldingCompany { get; set; }
        public string OperatingCompany { get; set; }
        public string HostCompany { get; set; }
        public string AdditionalCompany1 { get; set; }
        public string AdditionalCompany2 { get; set; }
        public string Customer { get; set; }
        public string ContractHoldingCoordinator { get; set; }
        public string MicontractNumber { get; set; }
        public int? MiprojectNumber { get; set; }
        public int MiassignmentNo { get; set; }
        public string Suppliernumber { get; set; }
        public string Reportnumber { get; set; }
        public string Businessunit { get; set; }
        public string Visitstatus { get; set; }
        public string VisitDatePeriod { get; set; }
        public string OperatingCoordinator { get; set; }
        public string TechnicalSpecialist { get; set; }
        public DateTime? Date { get; set; }
        public string Type { get; set; }
        public decimal Units { get; set; }
        public decimal Rate { get; set; }
        public string Currency { get; set; }
        public string NativeCurrency { get; set; }
        public decimal? ExchangeRate { get; set; }
        public decimal? NetFeeTotal { get; set; }
        public decimal? NetExpenseTotal { get; set; }
        public string Office { get; set; }
        public string DivisionRef { get; set; }
        public string Division { get; set; }
        public string CostCentre { get; set; }
        public string ChargeReference { get; set; }
        public string CurrentInvoicingStatus { get; set; }
        public int? TokenId { get; set; }
        public string Ptype { get; set; }
        public int? ItemId { get; set; }
        public byte? UpdateCount { get; set; }
        public DateTime? LastModification { get; set; }
        public string ModifiedBy { get; set; }

        public virtual Token Token { get; set; }
    }
}
