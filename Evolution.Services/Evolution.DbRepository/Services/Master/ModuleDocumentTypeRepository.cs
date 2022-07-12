using Evolution.DbRepository.Interfaces.Master;
using Evolution.DbRepository.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Evolution.DbRepository.Services.Master
{
   public class ModuleDocumentTypeRepository : GenericRepository<ModuleDocumentType>, IModuleDocumentTypeRepository
    {
        public ModuleDocumentTypeRepository(Evolution2Context dbContext) : base(dbContext)
        {

        }
    }
}
