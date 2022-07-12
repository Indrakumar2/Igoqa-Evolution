using AutoMapper;
using AutoMapper.QueryableExtensions;
using Evolution.Assignment.Domain.Interfaces.Data;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.GenericDbRepository.Services;
using Evolution.Logging.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.Assignment.Domain.Models.Assignments;

namespace Evolution.Assignment.Infrastructure.Data
{
    public class AssignmentTechnicalSpecialistScheduleRepository : GenericRepository<DbModel.AssignmentTechnicalSpecialistSchedule>, IAssignmentTechnicalSpecialistScheduleRepository
    {
        private DbModel.EvolutionSqlDbContext _dbContext = null;
        private IMapper _mapper = null;
        private IAppLogger<AssignmentTechnicalSpecialistScheduleRepository> _logger = null;

        public AssignmentTechnicalSpecialistScheduleRepository(DbModel.EvolutionSqlDbContext dbContext, IMapper mapper, IAppLogger<AssignmentTechnicalSpecialistScheduleRepository> logger) : base(dbContext)
        {
            this._dbContext = dbContext;
            this._mapper = mapper;
            this._logger = logger;
        }

        public IList<DomainModel.AssignmentTechnicalSpecialistSchedule> Search(DomainModel.AssignmentTechnicalSpecialistSchedule model)
        {
            var dbSearchModel = _mapper.Map<DbModel.AssignmentTechnicalSpecialistSchedule>(model);
            IQueryable<DbModel.AssignmentTechnicalSpecialistSchedule> whereClause = null;

            if (model.ContractScheduleName.HasEvoWildCardChar())
                whereClause = _dbContext.AssignmentTechnicalSpecialistSchedule
                                        .WhereLike(x => x.ContractChargeSchedule.Name, model.ContractScheduleName, '*');
            else
                whereClause = _dbContext.AssignmentTechnicalSpecialistSchedule
                                        .Where(x => string.IsNullOrEmpty(model.ContractScheduleName) ||
                                                    x.ContractChargeSchedule.Name == model.ContractScheduleName);

            if (model.TechnicalSpecialistPayScheduleName.HasEvoWildCardChar())
                whereClause = whereClause.WhereLike(x => x.TechnicalSpecialistPaySchedule.PayScheduleName, model.TechnicalSpecialistPayScheduleName, '*');
            else
                whereClause = whereClause.Where(x => string.IsNullOrEmpty(model.TechnicalSpecialistPayScheduleName) || x.TechnicalSpecialistPaySchedule.PayScheduleName == model.TechnicalSpecialistPayScheduleName);


            var expression = dbSearchModel.ToExpression();

            if (expression == null)
                return whereClause.ProjectTo<DomainModel.AssignmentTechnicalSpecialistSchedule>().ToList();
            else
                return whereClause.Where(expression).ProjectTo<DomainModel.AssignmentTechnicalSpecialistSchedule>().ToList();
        }

        public DomainModel.AssignmentTechSpecSchedules GetAssignmentTechSpecRateSchedules(int assignmentId, int epin, params string[] includes)
        {
            DomainModel.AssignmentTechSpecSchedules result = null;
            try
            {
                var whereClause = this._dbContext.AssignmentTechnicalSpecialistSchedule.Where(x => x.AssignmentTechnicalSpecialist.AssignmentId == assignmentId);
                if (epin > 0)
                    whereClause = whereClause.Where(x => x.AssignmentTechnicalSpecialist.TechnicalSpecialist.Pin == epin);
                if (includes.Any())
                    whereClause = includes.Aggregate(whereClause, (current, include) => current.Include(include));

                var dbExpenseType = _dbContext.Data.Where(x => x.MasterDataTypeId == (int)MasterType.ExpenseType).Select(x => new
                {
                    x.Name,
                    x.Id,
                    x.Type,
                }).ToList();

                var dbAssignmentSchedules = whereClause.Select(x => new
                {
                    x.Id,
                    x.AssignmentTechnicalSpecialist.TechnicalSpecialistId,
                    x.AssignmentTechnicalSpecialist.TechnicalSpecialist.Pin,
                    x.AssignmentTechnicalSpecialistId,
                    x.ContractChargeScheduleId,
                    x.TechnicalSpecialistPayScheduleId,
                    x.ContractChargeSchedule,
                    x.ContractChargeSchedule.ContractRate,
                    x.TechnicalSpecialistPaySchedule,
                    x.TechnicalSpecialistPaySchedule.TechnicalSpecialistPayRate,
                    x.AssignmentTechnicalSpecialist.TechnicalSpecialist.Company.Code
                }).ToList();

                if (dbAssignmentSchedules?.Count != 0)
                {
                  var  dbAssignmentChargeSchedules = dbAssignmentSchedules.GroupBy(c => new
                    {
                        c.AssignmentTechnicalSpecialistId,
                        c.ContractChargeScheduleId,
                    }).Select(g => g.FirstOrDefault()).ToList();

                    var dbAssignmentPaySchedules = dbAssignmentSchedules.GroupBy(c => new
                    {
                        c.AssignmentTechnicalSpecialistId,
                        c.TechnicalSpecialistPayScheduleId,
                    }).Select(g => g.FirstOrDefault()).ToList();

                    var contractSchedules = dbAssignmentChargeSchedules?.Select(x => new DomainModel.TechnicalSpecialistChargeSchedule
                    {
                        AssignmentTechnicalSpecialistScheduleId = x.Id,
                        ContractScheduleId = x.ContractChargeSchedule?.Id,
                        ContractScheduleName = x.ContractChargeSchedule?.Name,
                        ChargeScheduleCurrency = x.ContractChargeSchedule?.Currency,
                        TechnicalSpecialistId = x.TechnicalSpecialistId,
                        Epin = x.Pin,
                        ContractCompanyCode = x.ContractChargeSchedule.Contract.ContractHolderCompany.Code,
                        ChargeScheduleRates = x.ContractRate?.Select(r => new DomainModel.ChargeScheduleRates
                        {
                            AssignmentTechnicalSpecialistScheduleId = x.Id,
                            RateId = r.Id,
                            Currency = x.ContractChargeSchedule?.Currency,
                            ChargeTypeId = r.ExpenseTypeId,
                            //ChargeType = r.ExpenseType.Name,
                            //Type = r.ExpenseType.Type,
                            ChargeType = dbExpenseType.FirstOrDefault(x1 => x1.Id == r.ExpenseTypeId)?.Name,
                            Type = dbExpenseType.FirstOrDefault(x1 => x1.Id == r.ExpenseTypeId)?.Type,
                            ChargeRate = r.Rate.ToString(),
                            Description = r.Description,
                            EffectiveFrom = r.FromDate,
                            EffectiveTo = r.ToDate,
                            IsActive = r.IsActive,
                            IsPrintDescriptionOnInvoice = r.IsPrintDescriptionOnInvoice
                        }).ToList()
                    }).ToList();

                    var paySchedules = dbAssignmentPaySchedules?.Select(x => new DomainModel.TechnicalSpecialistPaySchedule
                    {
                        AssignmentTechnicalSpecialistScheduleId = x.Id,
                        TechnicalSpecialistPayScheduleId = x.TechnicalSpecialistPaySchedule?.Id,
                        TechnicalSpecialistPayScheduleName = x.TechnicalSpecialistPaySchedule?.PayScheduleName,
                        PayScheduleCurrency = x.TechnicalSpecialistPaySchedule?.PayCurrency,
                        TechnicalSpecialistId = x.TechnicalSpecialistId,
                        Epin = x.Pin,
                        TechnicalSpecialistCompanyCode = x.Code,
                        PayScheduleRates = x.TechnicalSpecialistPayRate?.Select(r => new DomainModel.PayScheduleRates
                        {
                            AssignmentTechnicalSpecialistScheduleId = x.Id,
                            RateId = r.Id,
                            Currency = x.TechnicalSpecialistPaySchedule?.PayCurrency,
                            ExpenseTypeId = r.ExpenseTypeId,
                            //ExpenseType = r.ExpenseType.Name,
                            //Type = r.ExpenseType.Type,
                            ExpenseType = dbExpenseType.FirstOrDefault(x1 => x1.Id == r.ExpenseTypeId)?.Name,
                            Type = dbExpenseType.FirstOrDefault(x1 => x1.Id == r.ExpenseTypeId)?.Type,
                            PayRate = r.Rate,
                            SPayRate = Convert.ToString(r.Rate),
                            Description = r.Description,
                            EffectiveFrom = r.FromDate,
                            EffectiveTo = r.ToDate,
                            IsActive = r.IsActive,
                        }).ToList()
                    }).ToList();

                    result = new DomainModel.AssignmentTechSpecSchedules
                    {
                        ChargeSchedules = contractSchedules,
                        PaySchedules = paySchedules
                    };
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), assignmentId, epin);
            }

            return result;
        }

        public DomainModel.AssignmentTechSpecSchedules GetAssignmentTechSpecRateSchedules(int assignmentId,
                                                                                          IList<DbModel.Data> dbExpenseType,
                                                                                          params string[] includes)
        {
            DomainModel.AssignmentTechSpecSchedules result = null;
            try
            {
                if (dbExpenseType == null)
                    dbExpenseType = _dbContext.Data.Where(x => x.MasterDataTypeId == (int)MasterType.ExpenseType).ToList();
                var whereClause = this._dbContext.AssignmentTechnicalSpecialistSchedule
                                                 .Where(x => x.AssignmentTechnicalSpecialist.AssignmentId == assignmentId);
                if (includes.Any())
                    whereClause = includes.Aggregate(whereClause, (current, include) => current.Include(include));
                var dbData = whereClause.Select(x => new
                {
                    x.Id,
                    x.AssignmentTechnicalSpecialist.TechnicalSpecialistId,
                    x.AssignmentTechnicalSpecialist.TechnicalSpecialist.Pin,
                    x.AssignmentTechnicalSpecialistId,
                    x.ContractChargeScheduleId,
                    x.TechnicalSpecialistPayScheduleId,
                    x.ContractChargeSchedule,
                    x.ContractChargeSchedule.ContractRate,
                    x.TechnicalSpecialistPaySchedule,
                    x.TechnicalSpecialistPaySchedule.TechnicalSpecialistPayRate
                }).ToList();
                if (dbData?.Count > 0)
                {
                    dbData = dbData.GroupBy(c => new
                        {
                            c.AssignmentTechnicalSpecialistId,
                            c.ContractChargeScheduleId,
                            c.TechnicalSpecialistPayScheduleId,
                        })?
                        .Select(g => g.FirstOrDefault()).ToList();

                    var contractSchedules = dbData?.Select(x => new DomainModel.TechnicalSpecialistChargeSchedule
                    {
                        AssignmentTechnicalSpecialistScheduleId = x.Id,
                        ContractScheduleId = x.ContractChargeScheduleId,
                        ContractScheduleName = x.ContractChargeSchedule?.Name,
                        ChargeScheduleCurrency = x.ContractChargeSchedule?.Currency,
                        Epin = x.TechnicalSpecialistId,
                        TechnicalSpecialistId = x.TechnicalSpecialistId,
                        ChargeScheduleRates = x.ContractRate?.Select(r =>
                            new DomainModel.ChargeScheduleRates
                            {
                                AssignmentTechnicalSpecialistScheduleId = x.Id,
                                RateId = r.Id,
                                Currency = x.ContractChargeSchedule.Currency,
                                ChargeType = dbExpenseType.FirstOrDefault(x1 => x1.Id == r.ExpenseTypeId)?.Name,
                                ChargeTypeId = dbExpenseType.FirstOrDefault(x1 => x1.Id == r.ExpenseTypeId)?.Id,
                                Type = dbExpenseType.FirstOrDefault(x1 => x1.Id == r.ExpenseTypeId)?.Type,
                                ChargeRate = r.Rate.ToString(),
                                Description = r.Description,
                                EffectiveFrom = r.FromDate,
                                EffectiveTo = r.ToDate,
                                IsActive = r.IsActive,
                                IsPrintDescriptionOnInvoice = r.IsPrintDescriptionOnInvoice
                            })?.ToList()
                    })?.ToList();

                    var paySchedules = dbData?.Select(x => new DomainModel.TechnicalSpecialistPaySchedule
                    {
                        AssignmentTechnicalSpecialistScheduleId = x.Id,
                        TechnicalSpecialistPayScheduleId = x.TechnicalSpecialistPayScheduleId,
                        TechnicalSpecialistPayScheduleName = x.TechnicalSpecialistPaySchedule?.PayScheduleName,
                        PayScheduleCurrency = x.TechnicalSpecialistPaySchedule?.PayCurrency,
                        Epin = x.TechnicalSpecialistId,
                        TechnicalSpecialistId = x.TechnicalSpecialistId,
                        PayScheduleRates = x.TechnicalSpecialistPayRate?.Select(r => new DomainModel.PayScheduleRates
                            {
                                AssignmentTechnicalSpecialistScheduleId = x.Id,
                                RateId = r.Id,
                                Currency = x.TechnicalSpecialistPaySchedule?.PayCurrency,
                                ExpenseType = dbExpenseType.FirstOrDefault(x1 => x1.Id == r.ExpenseTypeId)?.Name,
                                ExpenseTypeId = dbExpenseType.FirstOrDefault(x1 => x1.Id == r.ExpenseTypeId)?.Id,
                                Type = dbExpenseType.FirstOrDefault(x1 => x1.Id == r.ExpenseTypeId)?.Type,
                                PayRate = r.Rate,
                                SPayRate = Convert.ToString(r.Rate),
                                Description = r.Description,
                                EffectiveFrom = r.FromDate,
                                EffectiveTo = r.ToDate,
                                IsActive = r.IsActive,
                            })?.ToList()
                    })?.ToList();

                    result = new DomainModel.AssignmentTechSpecSchedules
                    {
                        ChargeSchedules = contractSchedules,
                        PaySchedules = paySchedules
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), assignmentId);
            }

            return result;
        }

        public IList<DbModel.AssignmentTechnicalSpecialistSchedule> IsUniqueTSSchedule(IList<DomainModel.AssignmentTechnicalSpecialistSchedule> assignmentTSSchedules,
                                                                                   IList<DbModel.AssignmentTechnicalSpecialistSchedule> dbAssignmentTSSchedule)
        {
            IList<DbModel.AssignmentTechnicalSpecialistSchedule> dbSpecificAssgmtTSSchedule = null;
            IList<DbModel.AssignmentTechnicalSpecialistSchedule> dbAssgmtTSSch = null;
            var assignmentTSSch = assignmentTSSchedules?.Where(x => x.TechnicalSpecialistPayScheduleId > 0 && x.ContractScheduleId > 0)?
                                                                  .Select(x => x.AssignmentTechnicalSpecilaistId)?
                                                                  .ToList();

            if (dbAssignmentTSSchedule == null && dbAssignmentTSSchedule?.Count == 0)
            {
                dbSpecificAssgmtTSSchedule = _dbContext.AssignmentTechnicalSpecialistSchedule?.Where(x => assignmentTSSch.Contains(x.AssignmentTechnicalSpecialistId)).ToList();
            }
            else
                dbSpecificAssgmtTSSchedule = dbAssignmentTSSchedule;

            if (dbSpecificAssgmtTSSchedule?.Count > 0)
                dbAssgmtTSSch = dbSpecificAssgmtTSSchedule.Join(assignmentTSSchedules.ToList(),
                                                 dbAssigmtTSSch => new { AssignmentTSID = dbAssigmtTSSch.AssignmentTechnicalSpecialistId, ContractSch = dbAssigmtTSSch.ContractChargeScheduleId, Rate = dbAssigmtTSSch.TechnicalSpecialistPayScheduleId },
                                                 domAssigmtTSSch => new { AssignmentTSID = (int)domAssigmtTSSch.AssignmentTechnicalSpecilaistId, ContractSch = (int)domAssigmtTSSch.ContractScheduleId, Rate = (int)domAssigmtTSSch.TechnicalSpecialistPayScheduleId },
                                                (dbAssigmtTSSch, domAssigmtTSSch) => new { dbAssigmtTSSch, domAssigmtTSSch })
                                                .Where(x => x.dbAssigmtTSSch.Id != x.domAssigmtTSSch.AssignmentTechnicalSpecialistScheduleId)
                                                .Select(x => x.dbAssigmtTSSch)
                                                .ToList();

            return dbAssgmtTSSch;
        }

        public void Dispose()
        {
            _dbContext.Dispose();
            _mapper = null;
            _logger = null;
        }

    }
}
