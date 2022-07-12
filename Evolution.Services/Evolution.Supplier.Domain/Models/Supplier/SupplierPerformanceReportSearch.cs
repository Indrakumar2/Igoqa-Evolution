using Evolution.Supplier.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace Evolution.Supplier.Domain.Models.Supplier
{
    public class SupplierPerformanceReportsearch
    {
        public string CustomerName { get; set; }

        public string CustomerCode { get; set; }

        public string ContractNumber { get; set; }

        public string ContractHoldingCompanyCode { get; set; }

        public string ContractHoldingCompanyName { get; set; }

        public string OperatingCompanyCode { get; set; }

        public string OperatingCompanyName { get; set; }

        public string SupplierName { get; set; }

        public int? ProjectNumber { get; set; }

        public int? supplierId { get; set; }

        public string SupplierPoNumber { get; set; }

        public NcrStatus Ncr { get; set; }

        public int? AssignmentNumber { get; set; }
    }
}
