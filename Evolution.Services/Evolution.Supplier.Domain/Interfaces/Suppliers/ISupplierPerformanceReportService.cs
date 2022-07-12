using Evolution.Common.Models.Responses;
using System;
using System.Collections.Generic;
using System.Text;
using DomainModel = Evolution.Supplier.Domain.Models.Supplier;

namespace Evolution.Supplier.Domain.Interfaces.Suppliers
{
    public interface ISupplierPerformanceReportService
    {
        Response Get(DomainModel.SupplierPerformanceReportsearch searchModel);
    }
}
