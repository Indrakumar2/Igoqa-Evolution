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
    public  class ContractScheduleRepository: GenericRepository<DbModel.ContractSchedule>, IContractScheduleRepository
    {
        private readonly DbModel.EvolutionSqlDbContext _dbContext = null;
        private readonly IMapper _mapper = null;
        private readonly IAppLogger<ContractScheduleRepository> _logger = null;

        public ContractScheduleRepository(EvolutionSqlDbContext dbContext,
                                        IMapper mapper,
                                        IAppLogger<ContractScheduleRepository> _logger) : base(dbContext)
        {
            this._dbContext = dbContext;
            this._mapper = mapper;
        }

        public IList<DomainModel.ContractSchedule> Search(DomainModel.ContractSchedule searchModel)
        {
            var dbSearchModel = this._mapper.Map<DbModel.ContractSchedule>(searchModel);
            IQueryable<DbModel.ContractSchedule> whereClause = null;

            var contractSchedule = _dbContext.ContractSchedule;

            //Wildcard Search for Contract Number
            if (searchModel.ContractNumber.HasEvoWildCardChar())
                whereClause = contractSchedule.WhereLike(x => x.Contract.ContractNumber, searchModel.ContractNumber, '*');
            else
                whereClause = contractSchedule.Where(x => string.IsNullOrEmpty(searchModel.ContractNumber) || x.Contract.ContractNumber == searchModel.ContractNumber);

            var expression = dbSearchModel.ToExpression();
            if (expression == null)
                return whereClause.ProjectTo<DomainModel.ContractSchedule>().ToList();
            else
                return whereClause.Where(expression).ProjectTo<DomainModel.ContractSchedule>().ToList();
        }

        public int DeleteSchedule(List<DomainModel.ContractSchedule> contractSchedules)
        {
            var scheduleIds = contractSchedules?.Where(x=>x.ScheduleId>0)?.Select(x => x.ScheduleId)?.Distinct().ToList();
            return DeleteSchedule(scheduleIds);
        }

        public int DeleteSchedule(List<DbModel.ContractSchedule> contractSchedules)
        {
            var scheduleIds = contractSchedules?.Select(x => x.Id)?.Distinct().ToList();
            return DeleteSchedule(scheduleIds);
        }

        public int DeleteSchedule(List<int> scheduleIds)
        {
            int count = 0;
            try
            {
                if (scheduleIds.Count > 0)
                {
                    var deleteStatement =string.Format(Utility.GetSqlQuery(SQLModuleType.Contract_Schedule, SQLModuleActionType.Delete), string.Join(",", scheduleIds.ToString<int>()));
                    count = _dbContext.Database.ExecuteSqlCommand(deleteStatement);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), "scheduleIds=" + scheduleIds.ToString<int>());
            }

            return count;
        }
    }
}
