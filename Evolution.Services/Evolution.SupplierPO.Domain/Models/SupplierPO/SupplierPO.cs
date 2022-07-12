using System;

namespace Evolution.SupplierPO.Domain.Models.SupplierPO
{
    public class SupplierPO : BaseSupplierPO
    {
        private decimal _supplierPORemainingBudgetValue = 0;
        private decimal _supplierPORemainingBudgetHours = 0;
        private decimal _supplierPOBudgetValueWarningPercentage = 0;
        private decimal _supplierPOBudgetHoursWarningPercentage = 0;
        [AuditNameAttribute("Budget Monetary Value")]
        public decimal SupplierPOBudget { get; set; }

        [AuditNameAttribute("Budget Monetary Warning")]
        public int? SupplierPOBudgetWarning { get; set; }

        [AuditNameAttribute("Budget Hours")]
        public decimal SupplierPOBudgetHours { get; set; }

        [AuditNameAttribute("Budget Hours Warning ")]
        public int? SupplierPOBudgetHoursWarning { get; set; }


        
        public decimal SupplierPOBudgetInvoicedToDate { get; set; }
        
        public decimal SupplierPOBudgetUninvoicedToDate { get; set; }

   
        public decimal SupplierPOBudgetHoursInvoicedToDate { get; set; }

        
        public decimal SupplierPOBudgetHoursUnInvoicedToDate { get; set; }

        [AuditNameAttribute("Currency")]
        public string SupplierPOCurrency { get; set; }

        [AuditNameAttribute("Sub-Supplier Id")]
        public int? SupplierPOSubSupplierId { get; set; }

        [AuditNameAttribute("Main Supplier Address")]
        public string SupplierPOMainSupplierAddress { get; set; }
        
   
        public decimal? SupplierPORemainingBudgetValue
        {
            get
            {
                return _supplierPORemainingBudgetValue = (SupplierPOBudget - Math.Round(SupplierPOBudgetInvoicedToDate,2) - Math.Round(SupplierPOBudgetUninvoicedToDate,2));
            }
        }
        
        public decimal? SupplierPORemainingBudgetHours
        {
            get
            {
                return _supplierPORemainingBudgetHours = (SupplierPOBudgetHours - Math.Round(SupplierPOBudgetHoursInvoicedToDate,2) - Math.Round(SupplierPOBudgetHoursUnInvoicedToDate,2));
            }
        }
        public decimal SupplierPOBudgetValueWarningPercentage
        {
            get
            {
                var value = SupplierPOBudget > 0 ? ((Math.Round(SupplierPOBudgetInvoicedToDate, 2) + Math.Round(SupplierPOBudgetUninvoicedToDate, 2)) / SupplierPOBudget) * 100 : 0;
                return _supplierPOBudgetValueWarningPercentage = value > 0 && value >= (SupplierPOBudgetWarning ?? 0) ? Math.Round(value, 2) : 0;
            }
        }
        
        public decimal SupplierPOBudgetHourWarningPercentage
        {
            get
            {
                var value = SupplierPOBudgetHours > 0 ? ((Math.Round(SupplierPOBudgetHoursInvoicedToDate, 2) + Math.Round(SupplierPOBudgetHoursUnInvoicedToDate, 2)) / SupplierPOBudgetHours) * 100 : 0;
                return _supplierPOBudgetHoursWarningPercentage = value > 0 && value >= (SupplierPOBudgetHoursWarning ?? 0) ? Math.Round(value, 2) : 0;
            }
        }


        public string City { get; set; }

        public string State { get; set; }

        public string Country { get; set; }

        public string ZipCode { get; set; }

    }
}
