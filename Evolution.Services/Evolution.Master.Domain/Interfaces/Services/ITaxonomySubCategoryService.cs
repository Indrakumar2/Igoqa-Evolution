using Evolution.Common.Models.Messages;
using Evolution.Common.Models.Responses;
using System.Collections.Generic;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;

namespace Evolution.Master.Domain.Interfaces.Services
{
    public interface ITaxonomySubCategoryService
    {
        Response Search(Models.TaxonomySubCategory search);

        bool IsValidSubCategoryName(IList<KeyValuePair<string, string>> tsCategoryAndSubCategory, ref IList<DbModel.TaxonomySubCategory> dbSubCategories, ref IList<ValidationMessage> validMessages);
    }
}
