using Evolution.Common.Models.Messages;
using Evolution.Common.Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;

namespace Evolution.Master.Domain.Interfaces.Services
{
    public interface ITaxonomyServices
    {
        Response Search(Models.TaxonomyService search);
        bool IsValidServiceName(IList<KeyValuePair<string, string>> tsSubCategoryAndService, ref IList<DbModel.TaxonomyService> dbServices, ref IList<ValidationMessage> validMessages);
        bool IsValidTaxonomyService(IList<string> taxonomyService, ref IList<DbModel.TaxonomyService> dbServices, ref IList<ValidationMessage> validationMessage, params Expression<Func<DbModel.TaxonomyService, object>>[] includes);
       //l bool IsValidSubCategoryService(IList<KeyValuePair<string, string>> taxnomyServicenmaes, ref IList<DbModel.TaxonomyService> dbServices, ref IList<ValidationMessage> validationMessage, params Expression<Func<DbModel.TaxonomyService, object>>[] includes);
    }
}
