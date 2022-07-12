using Evolution.Common.Enums;
using Evolution.GenericDbRepository.Interfaces;
using DbModels = Evolution.DbRepository.Models.SqlDatabaseContext;

namespace Evolution.NumberSequence.InfraStructure.Interface
{
    public interface INumberSequenceRepository : IGenericRepository<DbModels.NumberSequence>
	{
        int GetLastNumber(ModuleCodeType parentModule,
                                 ModuleCodeType relatedModule,
                                 ref int parentModuleId,
                                 ref int parentRefModuleId,
                                 int moduleData,
                                 ref DbModels.NumberSequence dbNumberSequence);
    }
}
