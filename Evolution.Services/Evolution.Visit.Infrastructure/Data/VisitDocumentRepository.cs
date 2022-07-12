using AutoMapper.QueryableExtensions;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.GenericDbRepository.Services;
using Evolution.Visit.Domain.Enums;
using Evolution.Visit.Domain.Interfaces.Data;
using Evolution.Visit.Domain.Models.Visits;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.Visit.Domain.Models.Visits;
using AutoMapper;

namespace Evolution.Visit.Infrastructure.Data
{
    public class VisitDocumentRepository : GenericRepository<DbModel.VisitDocument>, IVisitDocumentRepository
    {
        private readonly DbModel.EvolutionSqlDbContext _dbContext = null;
        private readonly IMapper _mapper = null;

        public VisitDocumentRepository(DbModel.EvolutionSqlDbContext dbContext, IMapper mapper) : base(dbContext)
        {
            this._dbContext = dbContext;
            this._mapper = mapper;
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }
    }
}
