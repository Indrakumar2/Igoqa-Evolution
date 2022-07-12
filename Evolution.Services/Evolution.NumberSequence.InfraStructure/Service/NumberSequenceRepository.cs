using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.GenericDbRepository.Services;
using Evolution.NumberSequence.InfraStructure.Interface;
using System.Collections.Generic;
using System.Linq;
using DbModels = Evolution.DbRepository.Models.SqlDatabaseContext;

namespace Evolution.NumberSequence.InfraStructure.Service
{
    public class NumberSequenceRepository : GenericRepository<DbModels.NumberSequence>, INumberSequenceRepository
    {
        private DbModels.EvolutionSqlDbContext _dbContext = null;
        public NumberSequenceRepository(DbModels.EvolutionSqlDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public int GetLastNumber(ModuleCodeType parentModule,
                                 ModuleCodeType relatedModule,
                                 ref int parentModuleId,
                                 ref int parentRefModuleId,
                                 int moduleData,
                                 ref DbModels.NumberSequence dbNumberSequence)
        {
            int? lastSequenceNumber = 1;
            DbModels.NumberSequence dbNumberSequences = null;
            var names = new List<string>() { EnumExtension.DisplayName(parentModule), EnumExtension.DisplayName(relatedModule) };
            var result = _dbContext.Module.Where(x => names.Contains(x.Name)).ToList();
            if (result?.ToList()?.Count > 0)
            {
                var parentId = parentModuleId = result.FirstOrDefault(x1 => x1.Name.ToLower() == EnumExtension.DisplayName(parentModule).ToLower()).Id;
                var parentRefId = parentRefModuleId = result.ToList().FirstOrDefault(x1 => x1.Name.ToLower() == EnumExtension.DisplayName(relatedModule).ToLower()).Id;
                dbNumberSequences = dbNumberSequence = _dbContext.NumberSequence.FirstOrDefault(x => x.ModuleId == parentId && x.ModuleRefId == parentRefId && x.ModuleData== moduleData);
                lastSequenceNumber = dbNumberSequence?.LastSequenceNumber + 1;
            }

            if (lastSequenceNumber == null)
                lastSequenceNumber = 1;

            return lastSequenceNumber.Value;
        }
    }
}
