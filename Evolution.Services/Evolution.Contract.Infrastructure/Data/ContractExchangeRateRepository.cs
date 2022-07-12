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
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.Contract.Domain.Models.Contracts;

namespace Evolution.Contract.Infrastructure.Data
{
    public class ContractExchangeRateRepository : GenericRepository<DbModel.ContractExchangeRate>, IContractExchangeRateRepository
    {

        private EvolutionSqlDbContext _dbContext = null;
        private IMapper _mapper = null;
        private readonly IAppLogger<ContractExchangeRateRepository> _logger = null;

        public ContractExchangeRateRepository(IMapper mapper,
            EvolutionSqlDbContext dbContext,
            IAppLogger<ContractExchangeRateRepository> logger) : base(dbContext)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
        }
        

        public IList<DomainModel.ContractExchangeRate> Search(DomainModel.ContractExchangeRate searchModel)
        {
            var dbSearchModel = _mapper.Map<DbModel.ContractExchangeRate>(searchModel);
            var contractExchangeRates = _dbContext.ContractExchangeRate;
            var expression = dbSearchModel.ToExpression();
            IQueryable<DbModel.ContractExchangeRate> whereClause = null;

            if (searchModel.ContractNumber.HasEvoWildCardChar())
                whereClause = contractExchangeRates.WhereLike(x => x.Contract.ContractNumber, searchModel.ContractNumber, '*');
            else
                whereClause = contractExchangeRates.Where(x => string.IsNullOrEmpty(searchModel.ContractNumber) || x.Contract.ContractNumber == searchModel.ContractNumber);

            if (expression != null)
                return whereClause.Where(expression).ProjectTo<DomainModel.ContractExchangeRate>().ToList();
            else
                return whereClause.ProjectTo<DomainModel.ContractExchangeRate>().ToList();
        }

        public IList<Common.Models.ExchangeRate.ContractExchangeRate> GetContractExchangeRates(IList<int> contractIds)
        { 
            var groupedContract = _dbContext.ContractExchangeRate
                            .Where(x => contractIds!=null && contractIds.Contains(x.ContractId))
                            .GroupBy(x => new { x.ContractId, x.CurrencyFrom, x.CurrencyTo })
                            .Select(x => new
                            {
                                x.Key.ContractId,
                                x.Key.CurrencyFrom,
                                x.Key.CurrencyTo,
                                EffectiveFrom = x.Max(x1 => x1.EffectiveFrom)
                            });

            return _dbContext.ContractExchangeRate
                     .Join(groupedContract,
                     outer => new { outer.ContractId, outer.CurrencyFrom, outer.CurrencyTo, outer.EffectiveFrom },
                     inner => new { inner.ContractId, inner.CurrencyFrom, inner.CurrencyTo, inner.EffectiveFrom },
                     (outer, inner) => new { outer, inner })
                     .Where(x => contractIds.Contains(x.outer.ContractId))
                     .Select(x => new Common.Models.ExchangeRate.ContractExchangeRate
                     {
                         ContractId = x.inner.ContractId,
                         CurrencyFrom = x.outer.CurrencyFrom,
                         CurrencyTo = x.outer.CurrencyTo,
                         Rate = x.outer.ExchangeRate
                     }).ToList();
        }

        public int DeleteExchangeRate(List<DomainModel.ContractExchangeRate>  contractExchangeRates)
        {
            var exchangeRateIds = contractExchangeRates?.Where(x =>   x.ExchangeRateId > 0)?.Select(x => x.ExchangeRateId)?.Distinct().ToList();
            return DeleteExchangeRate(exchangeRateIds);
        }

        public int DeleteExchangeRate(List<DbModel.ContractExchangeRate>  contractExchangeRates)
        {
            var exchangeRateIds = contractExchangeRates?.Select(x => x.Id)?.Distinct().ToList();
            return DeleteExchangeRate(exchangeRateIds);
        }

        public int DeleteExchangeRate(List<int> exchangeRateIds)
        {
            int count = 0;
            try
            {
                if (exchangeRateIds.Count > 0)
                {
                    var deleteStatement =string.Format(Utility.GetSqlQuery(SQLModuleType.Contract_ExchangeRate, SQLModuleActionType.Delete), string.Join(",", exchangeRateIds.ToString<int>()));
                    count = _dbContext.Database.ExecuteSqlCommand(deleteStatement);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), "ExchangeRateIds=" + exchangeRateIds.ToString<int>());
            }

            return count;
        }

    }

}
