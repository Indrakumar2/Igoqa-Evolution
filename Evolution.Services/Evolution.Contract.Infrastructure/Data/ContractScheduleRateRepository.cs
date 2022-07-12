using AutoMapper;
using AutoMapper.QueryableExtensions;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Common.Helpers;
using Evolution.Contract.Domain.Interfaces.Data;
using Evolution.DbRepository.Models.SqlDatabaseContext;
using Evolution.GenericDbRepository.Services;
using Evolution.Logging.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.Contract.Domain.Models.Contracts;

namespace Evolution.Contract.Infrastructure.Data
{
    public class ContractScheduleRateRepository : GenericRepository<DbModel.ContractRate>, IContractScheduleRateRepository
    {
        private readonly EvolutionSqlDbContext _dbContext = null;
        private readonly IMapper _mapper = null;
        private readonly IAppLogger<ContractScheduleRateRepository> _logger = null;

        public ContractScheduleRateRepository(EvolutionSqlDbContext dbContext,
                                                IMapper mapper,
                                                IAppLogger<ContractScheduleRateRepository> logger) : base(dbContext)
        {
            this._dbContext = dbContext;
            this._mapper = mapper;
            _logger = logger;
        }

        public IList<DomainModel.ContractScheduleRate> Search(DomainModel.ContractScheduleRate searchModel)
        {
            var dbSearchModel = this._mapper.Map<DbModel.ContractRate>(searchModel);
            IQueryable<DbModel.ContractRate> whereClause = null;

            var contractRate = _dbContext.ContractRate;

            //Wildcard Search for Contract Number
            if (searchModel.ContractNumber.HasEvoWildCardChar())
                whereClause = contractRate.WhereLike(x => x.ContractSchedule.Contract.ContractNumber, searchModel.ContractNumber, '*');
            else
                whereClause = contractRate.Where(x => string.IsNullOrEmpty(searchModel.ContractNumber) || x.ContractSchedule.Contract.ContractNumber == searchModel.ContractNumber);

            //Wildcard Search for Schedule Name
            if (searchModel.ScheduleName.HasEvoWildCardChar())
                whereClause = whereClause.WhereLike(x => x.ContractSchedule.Name, searchModel.ScheduleName, '*');
            else
                whereClause = whereClause.Where(x => string.IsNullOrEmpty(searchModel.ScheduleName) || x.ContractSchedule.Name == searchModel.ScheduleName);

            //Wildcard Search for Charge Type
            if (searchModel.ChargeType.HasEvoWildCardChar())
                whereClause = whereClause.WhereLike(x => x.ExpenseType.Name, searchModel.ChargeType, '*');
            else
                whereClause = whereClause.Where(x => string.IsNullOrEmpty(searchModel.ChargeType) || x.ExpenseType.Name == searchModel.ChargeType);


            var expression = dbSearchModel.ToExpression();
            if (expression == null)
                return whereClause.ProjectTo<DomainModel.ContractScheduleRate>().ToList();
            else
                return whereClause.Where(expression).ProjectTo<DomainModel.ContractScheduleRate>().ToList();
        }

        public int DeleteContractRate(List<DomainModel.ContractScheduleRate> contractScheduleRates)
        {
            var rateIds = contractScheduleRates?.Where(x=>x.RateId>0).Select(x => x.RateId)?.Distinct().ToList();
            return DeleteContractRate(rateIds);
        }

        public int DeleteContractRate(List<DbModel.ContractRate> contractRates)
        {
            var rateIds = contractRates?.Select(x => x.Id)?.Distinct().ToList();
            return DeleteContractRate(rateIds);
        }

        public int DeleteContractRate(List<int> rateIds)
        {
            int count = 0;
            try
            {
                if (rateIds.Count > 0)
                {
                    var deleteStatement = string.Format(Utility.GetSqlQuery(SQLModuleType.Contract_ScheduleRate, SQLModuleActionType.Delete), string.Join(",", rateIds.ToString<int>()));
                    count = _dbContext.Database.ExecuteSqlCommand(deleteStatement);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), "RateIds=" + rateIds.ToString<int>());
            }

            return count;
        }
    }
}
