using AutoMapper;
using AutoMapper.QueryableExtensions;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.DbRepository.Models.SqlDatabaseContext;
using Evolution.GenericDbRepository.Services;
using Evolution.Master.Domain.Interfaces.Data;
using Evolution.Master.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using domModel = Evolution.Master.Domain.Models;

namespace Evolution.Master.Infrastructure.Data
{
    public class MasterRepository : GenericRepository<DbModel.Data>, IMasterRepository
    {
        private readonly DbModel.EvolutionSqlDbContext _dbContext = null;
        private readonly IMapper _mapper = null;

        public MasterRepository(DbModel.EvolutionSqlDbContext dbContext,IMapper mapper):base(dbContext)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public bool IsRecordValidByCode(MasterType masterType, 
                                        IList<string> codes, 
                                        ref IList<DbModel.Data> dbDatas,
                                        params Expression<Func<DbModel.Data, object>>[] includes)
        {
            var dbCtx = _dbContext.Data.AsQueryable();
            if (includes.Any())
                dbCtx = includes.Aggregate(dbCtx, (current, include) => current.Include(include));

            if (dbDatas == null && codes?.Count > 0)
                dbDatas = dbCtx.Where(x => x.MasterDataTypeId == System.Convert.ToInt32(masterType) && codes.Contains(x.Code)).ToList();

            return dbDatas?.Where(x => codes.Contains(x.Code))?.Count() > 0; //codes?.Distinct()?.Count() == dbDatas?.Count;
        }

        public bool IsRecordValidByName(MasterType masterType, 
                                        IList<string> names, 
                                        ref IList<DbModel.Data> dbDatas,
                                        params Expression<Func<DbModel.Data, object>>[] includes)
        {
            var dbCtx = _dbContext.Data.AsQueryable();

            if (includes.Any())
                dbCtx = includes.Aggregate(dbCtx, (current, include) => current.Include(include));

            if (dbDatas == null && names?.Count > 0)
                dbDatas = dbCtx.Where(x => x.MasterDataTypeId == System.Convert.ToInt32(masterType) && names.Contains(x.Name)).ToList();

            return dbDatas?.Where(x => names.Contains(x.Name))?.Count() > 0; //names?.Distinct()?.Count() == dbDatas?.Count;
        }

        public IList<domModel.MasterData> Search(domModel.MasterData searchModel)
        {
            var dbSearchModel = _mapper.Map<domModel.MasterData,DbModel.Data>(searchModel);
            var expression = dbSearchModel.ToExpression();
            var masterData = _dbContext.Data;
            IQueryable<DbModel.Data> whereClause = null;


            if (searchModel.MasterDataTypeId.HasValue)
                whereClause = masterData.Where(x => x.MasterDataTypeId == searchModel.MasterDataTypeId);

              if (searchModel.MasterType.HasEvoWildCardChar())
                whereClause = masterData.Where(x => x.MasterDataType.Name == searchModel.MasterType);
                else
                   whereClause = masterData.Where(x => string.IsNullOrEmpty(searchModel.Name) || x.Name == searchModel.Name);
              
            if (searchModel.Name.HasEvoWildCardChar())
                whereClause = masterData.WhereLike(x => x.Name, searchModel.Name, '*');
            else
                whereClause = masterData.Where(x => string.IsNullOrEmpty(searchModel.Name) || x.Name == searchModel.Name);

            if (searchModel.Code.HasEvoWildCardChar())
                whereClause = masterData.WhereLike(x => x.Code, searchModel.Code, '*');
            else
                whereClause = masterData.Where(x => string.IsNullOrEmpty(searchModel.Code) || x.Code == searchModel.Code);

            if (expression != null)
                return masterData.Where(expression).ProjectTo<domModel.MasterData>().ToList();
            return masterData.ProjectTo<domModel.MasterData>().ToList();
        }

        public IList<domModel.SystemSetting> GetCommonSystemSetting(IList<string> keys)
        {
            //Commented on 23 Sep 2020 and included precompiled
            //return _dbContext.SystemSetting.Where(x => keys.Contains(x.KeyName))?
            //                          .Select(x => new domModel.SystemSetting { KeyName = x.KeyName, KeyValue = x.KeyValue })?.ToList();

            return _GetSystemSetting(_dbContext, keys)?.ToList();
        }
          
        public IList<MasterData> GetMasterData(domModel.MasterData searchModel)
        {
          return _GetMasterDataCompiled(_dbContext, searchModel.MasterDataTypeId ?? 0, searchModel.IsActive ?? true)?.OrderBy(c => c.Name).ToList();
        }

        private static readonly Func<EvolutionSqlDbContext, int,bool, IEnumerable<MasterData>> _GetMasterDataCompiled =
           EF.CompileQuery((EvolutionSqlDbContext dbContext, int masterTypeId,bool isActive) =>
               dbContext.Data
                           .Where(c => c.MasterDataTypeId == masterTypeId && c.IsActive == isActive)
                           .Select(x => new MasterData
                           {
                               Id = x.Id,
                               IsActive = x.IsActive,
                               IsAlchiddenForNewFacility = x.IsAlchiddenForNewFacility,
                               Description=x.Description,
                               Name = x.Name,
                               Code=x.Code,
                               Hour=x.Hour,
                               MasterDataTypeId=x.MasterDataTypeId,
                               LastModification = x.LastModification,
                               ModifiedBy = x.ModifiedBy,
                               UpdateCount = x.UpdateCount
                           })
                           );

        private static readonly Func<EvolutionSqlDbContext, IList<string>, IEnumerable<domModel.SystemSetting>> _GetSystemSetting =
           EF.CompileQuery((EvolutionSqlDbContext dbContext, IList<string> keys) =>
               dbContext.SystemSetting
                        .Where(x => keys != null && keys.Contains(x.KeyName))
                        .Select(x => new domModel.SystemSetting
                        {
                            KeyName = x.KeyName,
                            KeyValue = x.KeyValue
                        }));

    }
}

