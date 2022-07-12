using Evolution.Common.Enums;
using Evolution.Common.Models.Responses;
using Evolution.GenericDbRepository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using domModel = Evolution.Master.Domain.Models;

namespace Evolution.Master.Domain.Interfaces.Data
{
    public interface IMasterRepository : IGenericRepository<DbModel.Data>
    {
        bool IsRecordValidByName(MasterType masterType, IList<string> names, ref IList<DbModel.Data> dbDatas, params Expression<Func<DbModel.Data, object>>[] includes);

        bool IsRecordValidByCode(MasterType masterType, IList<string> codes, ref IList<DbModel.Data> dbDatas, params Expression<Func<DbModel.Data, object>>[] includes);

        IList<domModel.MasterData> Search(domModel.MasterData search);

        IList<domModel.SystemSetting> GetCommonSystemSetting(IList<string> keys);

        IList<domModel.MasterData> GetMasterData(domModel.MasterData searchModel);
    }
}

