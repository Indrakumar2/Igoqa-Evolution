using Evolution.Common.Enums;
using System;

namespace Evolution.Common.Models.Budget
{
    public class Budget
    {
        private decimal _remainingBudgetValue = 0;
        private decimal _remainingBudgetHours = 0;
        private decimal _totalInvoicedBudgetPrecentage = 0;
        private decimal _totalInvoicedBudgetHourPrecentage = 0;
        private bool? _isOverBudgetHour = null;
        private bool? _isOverBudgetValue = null;

        public int ContractId { get; set; }

        public int ProjectId { get; set; }

        public int AssignmentId { get; set; }

        public string ContractNumber { get; set; }

        public int ProjectNumber { get; set; }

        public int AssignmentNumber { get; set; }

        public string ContractCustomerCode { get; set; }

        public string ContractCustomerName { get; set; }

        public string CustomerContractNumber { get; set; }

        public decimal BudgetValue { get; set; }

        public string BudgetCurrency { get; set; }

        public int? BudgetWarning { get; set; }

        public decimal BudgetHours { get; set; }

        public int? BudgetHoursWarning { get; set; }

        public decimal InvoicedToDate { get; set; }

        public decimal UninvoicedToDate { get; set; }

        public decimal HoursInvoicedToDate { get; set; }

        public decimal HoursUninvoicedToDate { get; set; }

        public string ContractHoldingCompanyCode { get; set; } //D-1351 Fix

        public decimal RemainingBudgetValue
        {
            get
            {
                return this._remainingBudgetValue = (this.BudgetValue - this.InvoicedToDate - this.UninvoicedToDate);
            }
        }

        public decimal RemainingBudgetHours
        {
            get
            {
                return this._remainingBudgetHours = (this.BudgetHours - this.HoursInvoicedToDate - this.HoursUninvoicedToDate);
            }
        }

        public decimal TotalInvoicedBudgetPrecentage
        {
            get
            {
                if (this.BudgetValue > 0)
                    this._totalInvoicedBudgetPrecentage = (((this.InvoicedToDate + this.UninvoicedToDate) / this.BudgetValue) * 100);

                return this._totalInvoicedBudgetPrecentage;
            }
        }

        public decimal TotalInvoicedBudgetHourPrecentage
        {
            get
            {
                if (this.BudgetHours > 0)
                    this._totalInvoicedBudgetHourPrecentage = (((this.HoursInvoicedToDate + this.HoursUninvoicedToDate) / this.BudgetHours) * 100);

                return this._totalInvoicedBudgetHourPrecentage;
            }

        }

        public bool? IsOverBudgetHour
        {
            get
            {
                if (this.BudgetHoursWarning != null && this.BudgetHoursWarning > 0)
                    this._isOverBudgetHour = (this.TotalInvoicedBudgetHourPrecentage >= Convert.ToDecimal(this.BudgetHoursWarning));

                return this._isOverBudgetHour;
            }
            set
            {
                this._isOverBudgetHour = value;
            }
        }

        public bool? IsOverBudgetValue
        {
            get
            {
                if (this.BudgetWarning != null && this.BudgetWarning > 0)
                    this._isOverBudgetValue = (this.TotalInvoicedBudgetPrecentage >= this.BudgetWarning);

                return this._isOverBudgetValue;
            }
            set
            {
                this._isOverBudgetValue = value;
            }
        }

        public string BudgetInformationType
        {
            get
            {
                return (this.ContractId > 0 && this.ProjectId > 0 && this.AssignmentId > 0) ? BudgetInfoType.Assignment.ToString() : ((this.ContractId > 0 && this.ProjectId > 0) ? BudgetInfoType.Project.ToString() : BudgetInfoType.Contract.ToString());
            }
        }

    }
}
