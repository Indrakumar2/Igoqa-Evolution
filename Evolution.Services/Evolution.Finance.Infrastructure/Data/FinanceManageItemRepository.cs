using AutoMapper;
using AutoMapper.QueryableExtensions;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Common.Helpers;
using Evolution.GenericDbRepository.Services;
using Evolution.Logging.Interfaces;
using Evolution.Finance.Domain.Interfaces.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using Evolution.Finance.Domain.Enums;
using System.Linq;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.Finance.Domain.Models.Finance;

namespace Evolution.Finance.Infrastructure.Data
{
    public class FinanceManageItemRepository : GenericRepository<DbModel.Invoice>, IFinanceManageItemRepository
    {
        private readonly DbModel.EvolutionSqlDbContext _dbContext = null;
        private readonly IMapper _mapper = null;
        private readonly IAppLogger<FinanceManageItemRepository> _logger = null;

        public FinanceManageItemRepository(DbModel.EvolutionSqlDbContext dbContext, 
                                 IMapper mapper, 
                                 IAppLogger<FinanceManageItemRepository> logger) : base(dbContext)
        {
            this._dbContext = dbContext;
            this._mapper = mapper;
            this._logger = logger;
        }

        public async Task<Response> LoadCustomer(string CompanyCode)
        {
            var visitAccountItemConsumables = await _dbContext.VisitTechnicalSpecialist.Join(_dbContext.VisitTechnicalSpecialistAccountItemConsumable,
                                                     dbTecSpec => new { TechUniqueId= dbTecSpec.Id},
                                                     dbVisitTechSpecConsumable => new { TechUniqueId =dbVisitTechSpecConsumable.VisitTechnicalSpecialistId },
                                                    (dbTecSpec, dbVisitTechSpecConsumable) =>new {dbTecSpec,dbVisitTechSpecConsumable})
                                                    .Where(x=>x.dbVisitTechSpecConsumable.InvoicingStatus == "N" 
                                                                && x.dbVisitTechSpecConsumable.ChargeTotalUnit!=0 
                                                                && x.dbVisitTechSpecConsumable.Chargerate!=0)
                                                                //&& x.dbVisitTechSpecConsumable.Visit.)
                                                                .ToList();

            var visitAccountItemTravel= await _dbContext.VisitTechnicalSpecialist.Join(_dbContext.VisitTechnicalSpecialistAccountItemTravel,
                                                dbTecSpec => new { TechUniqueId= dbTecSpec.Id},
                                                dbVisitTechSpecConsumable => new { TechUniqueId =dbVisitTechSpecConsumable.VisitTechnicalSpecialistId },
                                                (dbTecSpec, dbVisitTechSpecConsumable) =>new {dbTecSpec,dbVisitTechSpecConsumable})
                                                .Where(x=>x.dbVisitTechSpecConsumable.InvoicingStatus == "N" 
                                                      && x.dbVisitTechSpecConsumable.ChargeTotalUnit!=0 
                                                      && x.dbVisitTechSpecConsumable.Chargerate!=0)
                                                      .ToList();

        }
    }

}
