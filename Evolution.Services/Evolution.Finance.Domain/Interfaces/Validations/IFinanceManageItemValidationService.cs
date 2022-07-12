using Evolution.Common.Enums;
using Evolution.Finance.Domain.Models.Validations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Evolution.Finance.Domain.Interfaces.Validations
{
    public interface IFinanceManageItemValidationService
    {
        IList<FinanceManageItemValidationResult> Validate(string jsonModel, ValidationType type);    
    } 
}
