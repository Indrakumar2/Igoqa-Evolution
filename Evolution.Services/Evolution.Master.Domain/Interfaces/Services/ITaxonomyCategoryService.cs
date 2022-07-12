using Evolution.Common.Models.Messages;
using Evolution.Common.Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;

namespace Evolution.Master.Domain.Interfaces.Services
{
    public interface ITaxonomyCategoryService
    {
        Response Search(Models.TaxonomyCategory search);
        bool IsValidCategoryName(IList<string> names, ref IList<DbModel.Data> dbCategories, ref IList<ValidationMessage> validMessagesparams);
    }
}
