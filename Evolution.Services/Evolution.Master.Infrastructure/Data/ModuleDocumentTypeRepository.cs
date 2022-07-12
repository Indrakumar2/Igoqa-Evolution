using AutoMapper;
using AutoMapper.QueryableExtensions;
using Evolution.DbRepository.Models.SqlDatabaseContext;
using Evolution.GenericDbRepository.Services;
using Evolution.Master.Domain.Interfaces.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Evolution.Master.Infrastructure.Data
{
    public class ModuleDocumentTypeRepository : GenericRepository<DbRepository.Models.SqlDatabaseContext.ModuleDocumentType>, IModuleDocumentTypeRepository
    {
        private readonly EvolutionSqlDbContext _dbContext = null;
        private IMapper _mapper = null;

        public ModuleDocumentTypeRepository(EvolutionSqlDbContext dbContext, IMapper mapper) : base(dbContext)
        {
            this._dbContext = dbContext;
            _mapper = mapper;
        }

        public IList<Domain.Models.ModuleDocumentType> Search(Domain.Models.ModuleDocumentType search)
        {
                      return this.FindBy(x => (String.IsNullOrEmpty(search.ModuleName) || x.Module.Name == search.ModuleName)
                                          && (string.IsNullOrEmpty(search.Name) || x.DocumentType.Name == search.Name)
                                          && ((search.IsTSVisible == null) || x.Tsvisible == search.IsTSVisible)
                                          && (search.IsActive == null || x.DocumentType.IsActive == search.IsActive))
                                          .ProjectTo<Domain.Models.ModuleDocumentType>().OrderBy(x1 => x1.Name).ToList();
        }
    }
}