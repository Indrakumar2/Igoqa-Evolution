using System;
using System.Collections.Generic;
using System.Text;
using Evolution.Common.Models.Responses;
using DomainModel = Evolution.Reports.Domain.Models.Reports;

namespace Evolution.Reports.Domain.Interfaces.Reports
{
    public interface ITaxonomyService
    {
        Response Get(DomainModel.Taxonomy searchModel);
    }
}


