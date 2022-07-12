using Evolution.Common.Enums;
using Evolution.Common.Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;

namespace Evolution.Master.Domain.Interfaces.Services
{
    public interface IMasterService
    {
        //Response Save(IList<Models.MasterData> datas);
        
        Response MasterSave (IList<Models.MasterData> datas);

        Response Search(Models.MasterData search, MasterType type);

        //Response Modify(IList<Models.MasterData> datas);

       // Response Delete(IList<Models.MasterData> datas);

        //TODO: extra filter to be included in the Search function 

        Response SearchMasterData(Models.MasterData search);
        
        IList<DbModel.Data> Get(IList<MasterType> types, 
                                IList<int> ids = null, 
                                IList<string> names = null, 
                                IList<string> codes = null,
                                params Expression<Func<DbModel.Data, object>>[] includes);

        IList<DbModel.Data> Get(IList<MasterType> types,
                                IList<int> ids = null,
                                IList<string> names = null,
                                IList<string> codes = null,
                                string[] includes = null);

        Response GetMasterData(Models.MasterData search);

        Response GetCommonSystemSetting(IList<string> keyValues);
    }
}
