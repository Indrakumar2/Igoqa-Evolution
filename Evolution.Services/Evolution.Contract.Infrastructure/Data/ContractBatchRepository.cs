using AutoMapper;
using EFCore.BulkExtensions;
using Evolution.AuditLog.Domain.Enums;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Contract.Domain.Interfaces.Data;
using Evolution.Contract.Domain.Models.Contracts;
using DomainModel = Evolution.Contract.Domain.Models.Contracts;
using Evolution.Logging.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Transactions;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using Evolution.AuditLog.Domain.Interfaces.Audit;
using Evolution.AuditLog.Domain.Functions;
using Evolution.AuditLog.Domain.Extensions;

namespace Evolution.Contract.Infrastructure.Data
{
    public class ContractBatchRepository : IContractBatchRepository, IDisposable
    {
        private readonly DbModel.EvolutionSqlDbContext _dbContext = null;
        private readonly IMapper _mapper = null;
        private readonly IAppLogger<ContractScheduleRateRepository> _logger = null;

        public ContractBatchRepository(DbModel.EvolutionSqlDbContext dbContext,
                                                IMapper mapper,
                                                IAppLogger<ContractScheduleRateRepository> logger)
        {
            this._dbContext = dbContext;
            this._mapper = mapper;
            this._logger = logger;
        }

        public string RelatedFrameworkContractBatch(int frameworkContractId,
            string createdBy, List<DbModel.SqlauditModule> dbModule
            , IAuditLogger _auditLogger)
        {
            string message = string.Empty;
            _dbContext.Database.SetCommandTimeout(3600);
            IList<DbModel.ContractSchedule> frameworkSchedules = new List<DbModel.ContractSchedule>();
            IList<DbModel.ContractSchedule> relatedFrameworkSchedules = new List<DbModel.ContractSchedule>();
            IList<DbModel.ContractSchedule> schedulesToUpdate = new List<DbModel.ContractSchedule>();
            IList<DbModel.ContractSchedule> schedulesToInsert = new List<DbModel.ContractSchedule>();
            IList<DbModel.ContractRate> ratesToInsert = new List<DbModel.ContractRate>();
            IList<DbModel.ContractRate> ratesToUpdate = new List<DbModel.ContractRate>();
            IList<DbModel.ContractRate> ratesToDelete = new List<DbModel.ContractRate>();
            List<int> frameworkContractRateIdsToInsert = new List<int>();
            frameworkSchedules = _dbContext.ContractSchedule.Include(include => include.Contract).
                Include(include => include.ContractRate).Where(item => item.ContractId == frameworkContractId)?.ToList();
            var relatedFrameworkContract = _dbContext.Contract.Include(include => include.ContractSchedule)
                .Where(x => x.FrameworkContractId == frameworkContractId)?.ToList();
            var relatedFrameworkContracts = relatedFrameworkContract?.Select(x1 => x1.Id)?.Distinct().ToList();
            relatedFrameworkSchedules = _dbContext.ContractSchedule.Include(include => include.ContractRate)
            .Include("ContractRate.VisitTechnicalSpecialistAccountItemConsumable")
                    .Include("ContractRate.VisitTechnicalSpecialistAccountItemExpense")
                    .Include("ContractRate.VisitTechnicalSpecialistAccountItemTime")
                    .Include("ContractRate.VisitTechnicalSpecialistAccountItemTravel")
                    .Include("ContractRate.TimesheetTechnicalSpecialistAccountItemTime")
                    .Include("ContractRate.TimesheetTechnicalSpecialistAccountItemConsumable")
                    .Include("ContractRate.TimesheetTechnicalSpecialistAccountItemExpense")
                    .Include("ContractRate.TimesheetTechnicalSpecialistAccountItemTravel")?
                    .Include("ContractRate.ExpenseType")?.Where(item => relatedFrameworkContracts.Contains(item.ContractId))?.ToList();
            List<DbModel.ContractSchedule> contractSchedules = relatedFrameworkSchedules?.Select(a => new DbModel.ContractSchedule()
            {
                Id = a.Id,
                Name = a.Name,
                Contract = a.Contract,
                AssignmentContractSchedule = a.AssignmentContractSchedule,
                AssignmentTechnicalSpecialistSchedule = a.AssignmentTechnicalSpecialistSchedule,
                BaseScheduleId = a.BaseScheduleId,
                ContractId = a.ContractId,
                ContractRate = a.ContractRate,
                Currency = a.Currency,
                LastModification = a.LastModification,
                ModifiedBy = a.ModifiedBy,
                ScheduleNoteForInvoice = a.ScheduleNoteForInvoice,
                UpdateCount = a.UpdateCount
            }).ToList();
            var isRFScheduleRates = relatedFrameworkSchedules?.SelectMany(x1 => x1.ContractRate).ToList();
            var Rates = isRFScheduleRates?.Select(a => new DbModel.ContractRate()
            {
                BaseRateId = a.BaseRateId,
                ContractSchedule = a.ContractSchedule,
                BaseScheduleId = a.BaseScheduleId,
                ContractScheduleId = a.ContractScheduleId,
                Description = a.Description,
                DiscountApplied = a.DiscountApplied,
                ExpenseType = a.ExpenseType,
                ExpenseTypeId = a.ExpenseTypeId,
                FromDate = a.FromDate,
                Id = a.Id,
                IsActive = a.IsActive,
                IsPrintDescriptionOnInvoice = a.IsPrintDescriptionOnInvoice,
                LastModification = a.LastModification,
                ModifiedBy = a.ModifiedBy,
                Percentage = a.Percentage,
                Rate = a.Rate,
                StandardInspectionTypeChargeRate = a.StandardInspectionTypeChargeRate,
                StandardInspectionTypeChargeRateId = a.StandardInspectionTypeChargeRateId,
                StandardValue = a.StandardValue,
                TimesheetTechnicalSpecialistAccountItemConsumable = a.TimesheetTechnicalSpecialistAccountItemConsumable,
                TimesheetTechnicalSpecialistAccountItemExpense = a.TimesheetTechnicalSpecialistAccountItemExpense,
                TimesheetTechnicalSpecialistAccountItemTime = a.TimesheetTechnicalSpecialistAccountItemTime,
                TimesheetTechnicalSpecialistAccountItemTravel = a.TimesheetTechnicalSpecialistAccountItemTravel,
                ToDate = a.ToDate,
                UpdateCount = a.UpdateCount,
                VisitTechnicalSpecialistAccountItemConsumable = a.VisitTechnicalSpecialistAccountItemConsumable,
                VisitTechnicalSpecialistAccountItemExpense = a.VisitTechnicalSpecialistAccountItemExpense,
                VisitTechnicalSpecialistAccountItemTime = a.VisitTechnicalSpecialistAccountItemTime,
                VisitTechnicalSpecialistAccountItemTravel = a.VisitTechnicalSpecialistAccountItemTravel
            }).ToList();
            var isFrameScheduleRates = frameworkSchedules?.SelectMany(x1 => x1.ContractRate).ToList();

            frameworkSchedules?.ToList().ForEach(x =>
            {
                var relatedSchedules = relatedFrameworkSchedules?.Where(x1 => x1.BaseScheduleId == x.Id)?.Select(x2 => x2)?.ToList();
                if (relatedSchedules?.Count > 0)
                {
                    var frameworkRatesIds = x.ContractRate?.Select(x1 => x1.Id)?.ToList();
                    if (frameworkRatesIds?.Count > 0)
                    {
                        var relatedFrameworkBaseRateIds = relatedSchedules?.SelectMany(x1 => x1.ContractRate)?.Where(x2 => x2.BaseRateId.HasValue)?.Select(x3 => x3.BaseRateId.Value)?.ToList();
                        if (relatedFrameworkBaseRateIds?.Count > 0)
                        {
                            var frameworkRateIdsToInsert = frameworkRatesIds.Except(relatedFrameworkBaseRateIds)?.ToList();
                            if (frameworkRateIdsToInsert?.Count > 0)
                                frameworkContractRateIdsToInsert.AddRange(frameworkRateIdsToInsert);
                        }
                        else
                        {
                            var Ids = x.ContractRate?.Where(item => item.ContractScheduleId == x.Id).Select(item => item.Id).Distinct()?.ToList();
                            frameworkContractRateIdsToInsert.AddRange(Ids);
                        }
                    }
                }
            });

            IList<DbModel.ContractRate> relatedRatesToUpdate = new List<DbModel.ContractRate>();
            relatedRatesToUpdate = isRFScheduleRates?.Join(isFrameScheduleRates,
                                        relatedRate => new { Id = relatedRate.BaseRateId },
                                        frameworkRate => new { Id = frameworkRate.Id as int? },
                                        (relatedRate, frameworkRate) => new { relatedRate, frameworkRate })
                                        .Where(x2 => x2.relatedRate.ExpenseTypeId != x2.frameworkRate.ExpenseTypeId || x2.relatedRate.Rate != x2.frameworkRate.Rate
                                                    || x2.relatedRate.Description != x2.frameworkRate.Description || x2.relatedRate.IsPrintDescriptionOnInvoice != x2.frameworkRate.IsPrintDescriptionOnInvoice
                                                    || x2.relatedRate.FromDate != x2.frameworkRate.FromDate || x2.relatedRate.ToDate != x2.frameworkRate.ToDate
                                                    || x2.relatedRate.IsActive != x2.frameworkRate.IsActive)
                                        .Select(y =>
                                        {
                                            y.relatedRate.ExpenseTypeId = y.frameworkRate.ExpenseTypeId;
                                            y.relatedRate.Rate = y.frameworkRate.Rate;
                                            y.relatedRate.Description = y.frameworkRate.Description;
                                            y.relatedRate.IsPrintDescriptionOnInvoice = y.frameworkRate.IsPrintDescriptionOnInvoice;
                                            y.relatedRate.FromDate = y.frameworkRate.FromDate;
                                            y.relatedRate.ToDate = y.frameworkRate.ToDate;
                                            y.relatedRate.IsActive = y.frameworkRate.IsActive;
                                            y.relatedRate.LastModification = DateTime.UtcNow;
                                            y.relatedRate.UpdateCount = y.relatedRate.UpdateCount.CalculateUpdateCount();
                                            y.relatedRate.ModifiedBy = y.frameworkRate.ModifiedBy;
                                            return y.relatedRate;
                                        })?.ToList();

            if (relatedRatesToUpdate?.Count > 0)
                ratesToUpdate.AddRange(relatedRatesToUpdate);

            IList<DbModel.ContractRate> relatedRatesToDelete = new List<DbModel.ContractRate>();
            if (isFrameScheduleRates != null && frameworkSchedules != null)
            {
                relatedRatesToDelete = isRFScheduleRates?.Where(x => x.BaseRateId.HasValue && x.BaseScheduleId.HasValue &&
                 !isFrameScheduleRates.Any(x1 => x1.Id == x.BaseRateId.Value)
                 && frameworkSchedules.Any(x2 => x2.Id == x.BaseScheduleId.Value)
                 && x.VisitTechnicalSpecialistAccountItemConsumable.Count() == 0
                     && x.VisitTechnicalSpecialistAccountItemExpense.Count() == 0
                     && x.VisitTechnicalSpecialistAccountItemTime.Count() == 0
                     && x.VisitTechnicalSpecialistAccountItemTravel.Count() == 0
                     && x.TimesheetTechnicalSpecialistAccountItemConsumable.Count() == 0
                     && x.TimesheetTechnicalSpecialistAccountItemExpense.Count() == 0
                     && x.TimesheetTechnicalSpecialistAccountItemTime.Count() == 0
                     && x.TimesheetTechnicalSpecialistAccountItemTravel.Count() == 0
                 ).ToList();
            }
            if (relatedRatesToDelete?.Count > 0)
                ratesToDelete.AddRange(relatedRatesToDelete);

            relatedFrameworkContracts?.ForEach(relatedContractId =>
            {
                IList<DbModel.ContractSchedule> relatedSchedules = new List<DbModel.ContractSchedule>();
                IList<DbModel.ContractSchedule> relatedSchedulesToUpdate = new List<DbModel.ContractSchedule>();

                relatedSchedules = relatedFrameworkSchedules?.Where(x => x.ContractId == relatedContractId)?.ToList(); // filtered related contract schedules

                relatedSchedulesToUpdate = relatedSchedules?.Join(frameworkSchedules,
                    isRFSchedule => new { scheduleId = isRFSchedule.BaseScheduleId },
                                                    newschedules => new { scheduleId = newschedules.Id as int? },
                                                    (isRFSchedule, newschedules) => new { isRFSchedule, newschedules })
                                                    .Where(x3 => x3.isRFSchedule.BaseScheduleId != null
                                                        && (x3.isRFSchedule.Name != x3.newschedules.Name
                                                        || x3.isRFSchedule.Currency != x3.newschedules.Currency
                                                        || x3.isRFSchedule.ScheduleNoteForInvoice != x3.newschedules.ScheduleNoteForInvoice)
                                                    )
                                                    .Select(x2 =>
                                                    {
                                                        x2.isRFSchedule.Name = x2.newschedules.Name;
                                                        x2.isRFSchedule.Currency = x2.newschedules.Currency;
                                                        x2.isRFSchedule.ScheduleNoteForInvoice = x2.newschedules.ScheduleNoteForInvoice;
                                                        x2.isRFSchedule.LastModification = DateTime.UtcNow;
                                                        x2.isRFSchedule.ModifiedBy = x2.newschedules.ModifiedBy;
                                                        x2.isRFSchedule.UpdateCount = x2.isRFSchedule.UpdateCount.CalculateUpdateCount();
                                                        return x2.isRFSchedule;
                                                    }).ToList();

                IList<DbModel.ContractRate> frameworkRatesToInsert = new List<DbModel.ContractRate>();
                IList<DbModel.ContractRate> ratesToInsert2 = new List<DbModel.ContractRate>();
                var data = frameworkSchedules?.SelectMany(x1 => x1.ContractRate)?.ToList();
                frameworkRatesToInsert = data?.Where(x2 => frameworkContractRateIdsToInsert.Contains(x2.Id))?.Select(x3 => x3).ToList();

                ratesToInsert2 = frameworkRatesToInsert?.Join(relatedSchedules,
                                    frameRate => new { id = frameRate.ContractScheduleId },
                                    relatedSchedule => new { id = relatedSchedule.BaseScheduleId },
                                    (frameRate, relatedSchedule) => new { frameRate, relatedSchedule })
                                    .Select(x1 => new DbModel.ContractRate
                                    {
                                        ContractScheduleId = x1.relatedSchedule.Id,
                                        ExpenseTypeId = x1.frameRate.ExpenseTypeId,
                                        Rate = x1.frameRate.Rate,
                                        Description = x1.frameRate.Description,
                                        IsPrintDescriptionOnInvoice = x1.frameRate.IsPrintDescriptionOnInvoice,
                                        FromDate = x1.frameRate.FromDate,
                                        ToDate = x1.frameRate.ToDate,
                                        IsActive = x1.frameRate.IsActive,
                                        //LastModification = DateTime.UtcNow,   //Commented for D-789 Batch Process Sync
                                        StandardValue = x1.frameRate.StandardValue,
                                        DiscountApplied = x1.frameRate.DiscountApplied,
                                        Percentage = x1.frameRate.Percentage,
                                        StandardInspectionTypeChargeRateId = x1.frameRate.StandardInspectionTypeChargeRateId,
                                        BaseRateId = x1.frameRate.Id,
                                        BaseScheduleId = x1.frameRate.ContractScheduleId,
                                        UpdateCount = 0,
                                        ModifiedBy = x1.relatedSchedule.ModifiedBy
                                    }).ToList();

                ratesToInsert.AddRange(ratesToInsert2);

                if (relatedSchedulesToUpdate?.Count > 0)
                {
                    schedulesToUpdate.AddRange(relatedSchedulesToUpdate);
                    frameworkSchedules?.ToList().ForEach(x =>
                    {
                        var isRFSchedule = relatedSchedules?.Where(x1 => x1.BaseScheduleId == x.Id)?.Select(x2 => x2)?.ToList();

                        if (isRFSchedule?.Count == 0 && !relatedSchedules.Any(x1 => x1.Name?.ToLower().Trim() == x.Name?.ToLower().Trim())) //Added toLower() to avoid duplicate schedule insert 
                        {
                            var isRFSchedulesToInsert = new DbModel.ContractSchedule
                            {
                                ContractId = relatedContractId,
                                Name = x.Name,
                                ScheduleNoteForInvoice = x.ScheduleNoteForInvoice,
                                Currency = x.Currency,
                                BaseScheduleId = x.Id,
                                ModifiedBy = x.ModifiedBy,
                                //LastModification = DateTime.UtcNow, //Commented for D-789 Batch Process Sync
                                UpdateCount = 0,
                                ContractRate = x.ContractRate?.Select(y => new DbModel.ContractRate
                                {
                                    ExpenseTypeId = y.ExpenseTypeId,
                                    Rate = y.Rate,
                                    Description = y.Description,
                                    IsPrintDescriptionOnInvoice = y.IsPrintDescriptionOnInvoice,
                                    FromDate = y.FromDate,
                                    ToDate = y.ToDate,
                                    IsActive = y.IsActive,
                                    //LastModification = DateTime.UtcNow, //Commented for D-789 Batch Process Sync
                                    StandardValue = y.StandardValue,
                                    DiscountApplied = y.DiscountApplied,
                                    Percentage = y.Percentage,
                                    StandardInspectionTypeChargeRateId = y.StandardInspectionTypeChargeRateId,
                                    BaseRateId = y.Id,
                                    BaseScheduleId = y.ContractScheduleId,
                                    UpdateCount = 0,
                                    ModifiedBy = x.ModifiedBy
                                }).ToList()
                            };
                            schedulesToInsert.Add(isRFSchedulesToInsert);
                        }
                    });
                }
                else
                {
                    frameworkSchedules?.ToList().ForEach(x =>
                    {
                        var isInsertRateSchedule = relatedSchedules?.Count == 0 || !relatedSchedules.Any(x1 => x1.Name?.ToLower().Trim() == x.Name?.ToLower().Trim());
                        if (isInsertRateSchedule)
                        {
                            var isRFSchedulesToInsert = new DbModel.ContractSchedule
                            {
                                ContractId = relatedContractId,
                                Name = x.Name,
                                ScheduleNoteForInvoice = x.ScheduleNoteForInvoice,
                                Currency = x.Currency,
                                BaseScheduleId = x.Id,
                                ModifiedBy = x.ModifiedBy,
                                //LastModification = DateTime.UtcNow, //Commented for D-789 Batch Process Sync
                                UpdateCount = 0,
                                ContractRate = x.ContractRate?.Select(y => new DbModel.ContractRate
                                {
                                    ExpenseTypeId = y.ExpenseTypeId,
                                    Rate = y.Rate,
                                    Description = y.Description,
                                    IsPrintDescriptionOnInvoice = y.IsPrintDescriptionOnInvoice,
                                    FromDate = y.FromDate,
                                    ToDate = y.ToDate,
                                    IsActive = y.IsActive,
                                    //LastModification = DateTime.UtcNow, //Commented for D-789 Batch Process Sync
                                    StandardValue = y.StandardValue,
                                    DiscountApplied = y.DiscountApplied,
                                    Percentage = y.Percentage,
                                    StandardInspectionTypeChargeRateId = y.StandardInspectionTypeChargeRateId,
                                    BaseRateId = y.Id,
                                    BaseScheduleId = y.ContractScheduleId,
                                    UpdateCount = 0,
                                    ModifiedBy = x.ModifiedBy
                                }).ToList()
                            };
                            schedulesToInsert.Add(isRFSchedulesToInsert);
                        }
                    });
                }
            });
            using (var tranScope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.Snapshot }))
            {
                frameworkSchedules = null;
                relatedFrameworkSchedules = null;
                message = this.BulkRelatedFrameworkCopy(schedulesToUpdate, schedulesToInsert, ratesToInsert, ratesToUpdate, ratesToDelete);
                if (message.ToLower().Contains("completed"))
                {
                    List<DbModel.SqlauditLogDetail> totalAudit = new List<DbModel.SqlauditLogDetail>();
                    var rates = schedulesToInsert.Select(a => a.ContractRate.ToList()).ToList();
                    List<int> rateIDs = ratesToInsert?.Select(a => a.Id)?.ToList();
                    List<DbModel.ContractRate> ratesData = rates?.SelectMany(a => a)?.ToList();
                    List<int> ratesID = ratesData?.Select(a => a.Id)?.ToList();
                    List<int> duplicates = rateIDs?.Intersect(ratesID)?.ToList();
                    ratesData.ForEach(item =>
                    {
                        if (!duplicates.Contains(item.Id))
                            ratesToInsert.Add(item);
                    });

                    IList<DbModel.Data> expenseType = new List<DbModel.Data>();
                    if ((ratesToInsert != null && ratesToInsert.Count > 0) || (ratesToUpdate != null && ratesToUpdate.Count > 0) || (ratesToDelete != null && ratesToDelete.Count > 0))
                    {
                        List<int> insertRates = ratesToInsert?.Select(a => a.ExpenseTypeId)?.Distinct()?.ToList();
                        List<int> deleteRates = ratesToDelete?.Select(a => a.ExpenseTypeId)?.Distinct()?.ToList();
                        List<int> updateRates = ratesToUpdate?.Select(a => a.ExpenseTypeId)?.Distinct()?.ToList();
                        List<int> masterRates = insertRates?.Union(deleteRates)?.Union(updateRates)?.Distinct()?.ToList();
                        expenseType = _dbContext.Data.Where(a => masterRates.Contains(a.Id) && a.MasterDataTypeId == 7).ToList();
                    }
                    totalAudit = ProcessAudit(createdBy, dbModule.ToList(),
                        relatedFrameworkContracts,
                        relatedFrameworkContract,
                        contractSchedules,
                        Rates,
                        schedulesToInsert,
                        schedulesToUpdate,
                        ratesToUpdate,
                        ratesToInsert,
                        ratesToDelete,
                        expenseType, _auditLogger);

                    message = this.BulkRelatedFrameworkCopyAudit(totalAudit);
                    totalAudit = null;
                    if (message.ToLower().Contains("completed"))
                        tranScope.Complete();
                }
            }
            return message;
        }

        private List<DbModel.SqlauditLogDetail> ProcessAudit(string createdBy, List<DbModel.SqlauditModule> dbModule,
            List<int> relatedFrameworkContracts, List<DbModel.Contract> relatedFrameworkContract,
            IList<DbModel.ContractSchedule> relatedFrameworkSchedules,
            List<DbModel.ContractRate> isRFScheduleRates,
            IList<DbModel.ContractSchedule> schedulesToInsert,
            IList<DbModel.ContractSchedule> schedulesToUpdate,
            IList<DbModel.ContractRate> ratesToUpdate,
            IList<DbModel.ContractRate> ratesToInsert,
            IList<DbModel.ContractRate> ratesToDelete,
            IList<DbModel.Data> expenseType,
            IAuditLogger _auditLogger)
        {
            List<DbModel.SqlauditLogDetail> totalAudit = new List<DbModel.SqlauditLogDetail>();
            relatedFrameworkContracts.ForEach(id =>
            {
                long? eventId = 0;
                DbModel.Contract item = relatedFrameworkContract?.FirstOrDefault(a => a.Id == id);
                List<DomainModel.ContractSchedule> schedulesToInserts = schedulesToInsert?.Where(a => a.ContractId == id)?.
                    Select(a => new DomainModel.ContractSchedule()
                    {
                        ScheduleId = a.Id,
                        ContractNumber = item.ContractNumber,
                        ScheduleName = a.Name,
                        ScheduleNameForInvoicePrint = a.ScheduleNoteForInvoice,
                        ChargeCurrency = a.Currency
                    })?.ToList();
                List<DomainModel.ContractSchedule> schedulesToUpdates = schedulesToUpdate?.Where(a => a.ContractId == id)?
                    .Select(a => new DomainModel.ContractSchedule()
                    {
                        ScheduleId = a.Id,
                        ContractNumber = item.ContractNumber,
                        ScheduleName = a.Name,
                        ScheduleNameForInvoicePrint = a.ScheduleNoteForInvoice,
                        ChargeCurrency = a.Currency
                    })?.ToList();
                List<DomainModel.ContractScheduleRate> ratesToUpdates = ratesToUpdate?.Where(a => a.ContractSchedule?.ContractId == id)?.
                    Select(a => new DomainModel.ContractScheduleRate()
                    {
                        RateId = a.Id,
                        ContractNumber = item.ContractNumber,
                        ScheduleName = a.ContractSchedule?.Name,  //a.ContractSchedule.Name,
                        Currency = a.ContractSchedule?.Currency,
                        StandardValue = a.StandardValue,
                        ChargeValue = Convert.ToString(a.Rate),
                        ChargeType = expenseType != null ? expenseType.FirstOrDefault(b => b.Id == a.ExpenseTypeId)?.Name : string.Empty,
                        Description = a.Description,
                        DiscountApplied = Convert.ToString(a.DiscountApplied),
                        Percentage = a.Percentage,
                        IsDescriptionPrintedOnInvoice = a.IsPrintDescriptionOnInvoice,
                        EffectiveFrom = a.FromDate,
                        EffectiveTo = a.ToDate,
                        IsActive = a.IsActive

                    })?.ToList();

                List<DomainModel.ContractScheduleRate> ratesToInserts = new List<ContractScheduleRate>();
                ratesToInserts = ratesToInsert?.Where(a => a.ContractSchedule?.ContractId == id)?.
                        Select(a => new DomainModel.ContractScheduleRate()
                        {
                            RateId = a.Id,
                            ContractNumber = item.ContractNumber,
                            ScheduleName = a.ContractSchedule?.Name,
                            Currency = a.ContractSchedule?.Currency,
                            StandardValue = a.StandardValue,
                            ChargeValue = Convert.ToString(a.Rate),
                            ChargeType = expenseType != null ? expenseType.FirstOrDefault(b => b.Id == a.ExpenseTypeId)?.Name : string.Empty,
                            Description = a.Description,
                            DiscountApplied = Convert.ToString(a.DiscountApplied),
                            Percentage = a.Percentage,
                            IsDescriptionPrintedOnInvoice = a.IsPrintDescriptionOnInvoice,
                            EffectiveFrom = a.FromDate,
                            EffectiveTo = a.ToDate,
                            StandardInspectionTypeChargeRateId = a.StandardInspectionTypeChargeRateId,
                            IsActive = a.IsActive
                        })?.ToList();

                List<DomainModel.ContractScheduleRate> ratesToDeletes = ratesToDelete?.Where(a => a.ContractSchedule?.ContractId == id)?.
                    Select(a => new DomainModel.ContractScheduleRate()
                    {
                        RateId = a.Id,
                        ContractNumber = item.ContractNumber,
                        ScheduleName = a.ContractSchedule?.Name,
                        Currency = a.ContractSchedule?.Currency,
                        StandardValue = a.StandardValue,
                        ChargeType = expenseType != null ? expenseType.FirstOrDefault(b => b.Id == a.ExpenseTypeId)?.Name : string.Empty,
                        ChargeValue = Convert.ToString(a.Rate),
                        Description = a.Description,
                        DiscountApplied = Convert.ToString(a.DiscountApplied),
                        Percentage = a.Percentage,
                        IsDescriptionPrintedOnInvoice = a.IsPrintDescriptionOnInvoice,
                        EffectiveFrom = a.FromDate,
                        EffectiveTo = a.ToDate,
                        IsActive = a.IsActive
                    })?.ToList();

                List<DomainModel.ContractSchedule> oldContractSchedule = relatedFrameworkSchedules?.
                    Where(a => a.ContractId == id)?.Select(
                            a => new DomainModel.ContractSchedule()
                            {
                                ScheduleId = a.Id,
                                ContractNumber = item.ContractNumber,
                                ScheduleName = a.Name,
                                ScheduleNameForInvoicePrint = a.ScheduleNoteForInvoice,
                                ChargeCurrency = a.Currency
                            }
                        )?.ToList();
                List<DomainModel.ContractScheduleRate> oldRates = isRFScheduleRates?.Where(a => a.ContractSchedule?.ContractId == id)?.
                    Select(a => new DomainModel.ContractScheduleRate()
                    {
                        RateId = a.Id,
                        ContractNumber = item.ContractNumber,
                        ScheduleName = a.ContractSchedule?.Name,
                        Currency = a.ContractSchedule?.Currency,
                        StandardValue = a.StandardValue,
                        ChargeType = expenseType != null ? expenseType.FirstOrDefault(b => b.Id == a.ExpenseTypeId)?.Name : string.Empty,
                        ChargeValue = Convert.ToString(a.Rate),
                        Description = a.Description,
                        DiscountApplied = Convert.ToString(a.DiscountApplied),
                        Percentage = a.Percentage,
                        IsDescriptionPrintedOnInvoice = a.IsPrintDescriptionOnInvoice,
                        EffectiveFrom = a.FromDate,
                        EffectiveTo = a.ToDate,
                        IsActive = a.IsActive
                    })?.ToList();

                LogEventGeneration logEvent = new LogEventGeneration(_auditLogger);
                eventId = logEvent.GetEventLogId(eventId,
                                     ValidationType.Update.ToAuditActionType(),
                                     createdBy,
                                      "{" + AuditSelectType.Id + ":" + item.Id + "}" +
                                      "${" + AuditSelectType.ContractNumber + ":" + item.ContractNumber?.Trim() + "}" +
                                      "${" + AuditSelectType.CustomerContractNumber + ":" + item.CustomerContractNumber?.Trim() + "}"
                                      , SqlAuditModuleType.Contract.ToString());
                List<DbModel.SqlauditLogDetail> auditSchedules = FormAuditLogSchedules(eventId, dbModule?.ToList(), schedulesToUpdates, schedulesToInserts, oldContractSchedule);
                List<DbModel.SqlauditLogDetail> auditRates = FormAuditLogRates(eventId, dbModule?.ToList(), ratesToUpdates, ratesToInserts, ratesToDeletes, oldRates);
                List<DbModel.SqlauditLogDetail> temp = auditSchedules.Union(auditRates).ToList();
                if (auditSchedules.Count > 0 || auditRates.Count > 0)
                {
                    DomainModel.Contract contract = new DomainModel.Contract()
                    {
                        Id = item.Id,
                        ContractNumber = item.ContractNumber,
                        CustomerContractNumber = item.CustomerContractNumber
                    };
                    temp.Add(new DbModel.SqlauditLogDetail()
                    {
                        NewValue = contract.AuditSerializeAttribute<Object, AuditNameAttribute>("AuditName"),
                        OldValue = contract.AuditSerializeAttribute<Object, AuditNameAttribute>("AuditName"),
                        SqlAuditSubModuleId = dbModule.FirstOrDefault(x1 => x1.ModuleName == SqlAuditModuleType.Contract.ToString()).Id,
                        SqlAuditLogId = (long)eventId,
                    });
                }
                totalAudit.AddRange(temp);
            });
            return totalAudit;
        }

        private List<DbModel.SqlauditLogDetail> FormAuditLogSchedules(long? eventId,
            List<DbModel.SqlauditModule> dbModule,
            List<DomainModel.ContractSchedule> schedulesToUpdate,
            List<DomainModel.ContractSchedule> schedulesToInsert,
            List<DomainModel.ContractSchedule> oldRecords)
        {
            List<DbModel.SqlauditLogDetail> sqlAuditLogDetail = new List<DbModel.SqlauditLogDetail>();
            int moduleTypeId = dbModule.FirstOrDefault(x1 => x1.ModuleName == SqlAuditModuleType.ContractSchedule.ToString()).Id;
            schedulesToUpdate.ForEach(item =>
            {
                var oldValue = oldRecords?.FirstOrDefault(a => a.ScheduleId == item.ScheduleId);
                sqlAuditLogDetail.Add(new DbModel.SqlauditLogDetail()
                {
                    NewValue = item.AuditSerializeAttribute<Object, AuditNameAttribute>("AuditName"),
                    OldValue = oldValue?.AuditSerializeAttribute<Object, AuditNameAttribute>("AuditName"),
                    SqlAuditSubModuleId = moduleTypeId,
                    SqlAuditLogId = (long)eventId,
                });
            });

            schedulesToInsert.ForEach(item =>
            {
                sqlAuditLogDetail.Add(new DbModel.SqlauditLogDetail()
                {
                    NewValue = item.AuditSerializeAttribute<Object, AuditNameAttribute>("AuditName"),
                    OldValue = null,
                    SqlAuditSubModuleId = moduleTypeId,
                    SqlAuditLogId = (long)eventId,
                });
            });
            return sqlAuditLogDetail;
        }

        private List<DbModel.SqlauditLogDetail> FormAuditLogRates(long? eventId,
            List<DbModel.SqlauditModule> dbModule,
            List<ContractScheduleRate> ratesToUpdate,
            List<ContractScheduleRate> ratesToInsert,
            List<ContractScheduleRate> ratesToDelete,
            List<ContractScheduleRate> oldRates)
        {
            List<DbModel.SqlauditLogDetail> sqlAuditLogDetail = new List<DbModel.SqlauditLogDetail>();
            int moduleTypeId = dbModule.FirstOrDefault(x1 => x1.ModuleName == SqlAuditModuleType.ContractRate.ToString()).Id;
            ratesToUpdate.ForEach(item =>
            {
                var oldValue = oldRates?.FirstOrDefault(a => a.RateId == item.RateId);
                sqlAuditLogDetail.Add(new DbModel.SqlauditLogDetail()
                {
                    NewValue = item.AuditSerializeAttribute<Object, AuditNameAttribute>("AuditName"),
                    OldValue = oldValue?.AuditSerializeAttribute<Object, AuditNameAttribute>("AuditName"),
                    SqlAuditSubModuleId = moduleTypeId,
                    SqlAuditLogId = (long)eventId,
                });
            });
            ratesToInsert.ForEach(item =>
            {
                sqlAuditLogDetail.Add(new DbModel.SqlauditLogDetail()
                {
                    NewValue = item.AuditSerializeAttribute<Object, AuditNameAttribute>("AuditName"),
                    OldValue = null,
                    SqlAuditSubModuleId = moduleTypeId,
                    SqlAuditLogId = (long)eventId,
                });
            });
            ratesToDelete.ForEach(item =>
            {
                sqlAuditLogDetail.Add(new DbModel.SqlauditLogDetail()
                {
                    NewValue = null,
                    OldValue = item.AuditSerializeAttribute<Object, AuditNameAttribute>("AuditName"),
                    SqlAuditSubModuleId = moduleTypeId,
                    SqlAuditLogId = (long)eventId,
                });
            });
            return sqlAuditLogDetail;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="alstSchedulesToUpdate"></param>
        /// <param name="alstSchedulesToInsert"></param>
        /// <param name="alstRatesToInsert"></param>
        /// <param name="alstRatesToDelete"></param>
        /// <returns></returns>
        public string BulkRelatedFrameworkCopy(IList<DbModel.ContractSchedule> alstSchedulesToUpdate, IList<DbModel.ContractSchedule> alstSchedulesToInsert, IList<DbModel.ContractRate> alstRatesToInsert, IList<DbModel.ContractRate> alstRatesToUpdate, IList<DbModel.ContractRate> ratesToDelete)
        {
            try
            {
                //_dbContext.BulkUpdate(alstSchedulesToUpdate);
                _dbContext.UpdateRange(alstSchedulesToUpdate);
                _dbContext.UpdateRange(alstRatesToUpdate);
                //_dbContext.BulkUpdate(alstRatesToUpdate);
                _dbContext.ContractRate.AddRange(alstRatesToInsert);
                _dbContext.RemoveRange(ratesToDelete);
                //_dbContext.BulkDelete(ratesToDelete);
                _dbContext.ContractSchedule.AddRange(alstSchedulesToInsert);
                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), null);
                return "Internal error Please try again.";
            }
            return "Bulk Operation Completed.";
        }

        public string BulkRelatedFrameworkCopyAudit(IList<DbModel.SqlauditLogDetail> sqlauditLogDetails)
        {
            try
            {
                _dbContext.AddRange(sqlauditLogDetails);
                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), null);
                return "Internal error Please try again.";
            }
            return "Bulk Operation Completed.";
        }

        public void BulkInsertSchedule(int contractId, List<DbModel.ContractSchedule> dbContractSchedules, IList<ContractScheduleRate> contractScheduleRates,
                                       IList<DbModel.CompanyInspectionTypeChargeRate> companyInspectionTypeCharges, IList<DbModel.Data> dbExpense,
                                       ref IList<DbModel.ContractRate> dbContractRate, ref IList<DbModel.ContractSchedule> dbSchedules)
        {
            try
            {
                if (dbContractSchedules?.Any() == true)
                    _dbContext.BulkInsert(dbContractSchedules);

                dbSchedules = _dbContext.ContractSchedule.AsNoTracking().Where(x => x.ContractId == contractId)?.ToList();
                if (dbSchedules?.Any() == true)
                {
                    //Stopwatch stopWatch = new Stopwatch();
                    //stopWatch.Start();

                    dbContractRate = dbSchedules?.Join(contractScheduleRates,
                           schedule => schedule.Name,
                           rate => rate.ScheduleName,
                           (schedule, rate) => new { schedule, rate })
                       ?.Select(x => new DbModel.ContractRate
                       {
                           ContractScheduleId = x.schedule.Id,
                           ExpenseTypeId = dbExpense.FirstOrDefault(x1 => x1.Name == x.rate.ChargeType).Id,
                           Rate = Convert.ToDecimal(x.rate.ChargeValue),
                           Description = x.rate.Description,
                           IsPrintDescriptionOnInvoice = x.rate.IsDescriptionPrintedOnInvoice,
                           FromDate = x.rate.EffectiveFrom,
                           ToDate = x.rate.EffectiveTo,
                           IsActive = x.rate.IsActive,
                           //LastModification = x.rate.LastModification,
                           StandardValue = x.rate.StandardValue,
                           DiscountApplied = Convert.ToDecimal(x.rate.DiscountApplied),
                           Percentage = x.rate.Percentage,
                           StandardInspectionTypeChargeRateId = companyInspectionTypeCharges?.FirstOrDefault(x2 => x2.Id == x.rate.StandardInspectionTypeChargeRateId)?.Id, //To be chceked with Ragavi
                           BaseRateId = x.rate.BaseRateId,
                           BaseScheduleId = x.rate.BaseScheduleId,
                           ModifiedBy = x.rate.ModifiedBy,
                           UpdateCount = x.rate.UpdateCount
                       }).ToList();

                    //var count = dbContractRate?.Count();
                    //stopWatch.Stop();
                    //TimeSpan ts = stopWatch.Elapsed;
                    //Console.WriteLine("Process Time ContractSchedule Bulk Insert and Forming Contract Rate Object " + String.Format("{0:00}:{1:00}:{2:00}.{3:00}.{4:00}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10, count));

                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), null);
            }
        }

        public void BulkInsertRate(IList<DbModel.ContractRate> dbcontractRate)
        {
            try
            {
                //Stopwatch stopWatch = new Stopwatch();
                //stopWatch.Start();
                if (dbcontractRate?.Any() == true)
                {
                    _dbContext.ChangeTracker.AutoDetectChangesEnabled = false;
                    _dbContext.BulkInsert(dbcontractRate, new BulkConfig() { BatchSize = 2000 });
                }

                //stopWatch.Stop();
                //TimeSpan ts = stopWatch.Elapsed;
                //Console.WriteLine("Process Time Contract Rate Bulk Insert" + String.Format("{0:00}:{1:00}:{2:00}.{3:00}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10));
            }
            catch (Exception ex)
            {
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), null);
            }
        }

        public void BulkAuditInsertSchedule(long? eventId, int contractId, string contractNumber, List<DbModel.ContractSchedule> dbContractSchedule, IList<DbModel.SqlauditModule> dbModule,
                                            IList<DbModel.Data> dbExpense, bool IsBulk = false)
        {
            try
            {
                //Stopwatch stopWatch = new Stopwatch();
                //stopWatch.Start();
                IList<DbModel.SqlauditLogDetail> sqlContractScheduleLogDetails = null;
                IList<DbModel.SqlauditLogDetail> sqlContractRateLogDetails = null;
                if (dbContractSchedule?.Any() == true && dbModule?.Any() == true && eventId > 0)
                {
                    var schedules = dbContractSchedule.ToList().Select(x => new ContractSchedule
                    {
                        ScheduleId = x.Id,
                        ScheduleName = x.Name,
                        ContractNumber = contractNumber,
                        ScheduleNameForInvoicePrint = x.ScheduleNoteForInvoice,
                        ChargeCurrency = x.Currency,
                        BaseScheduleId = x.BaseScheduleId,
                        //LastModification = x.LastModification,
                        ModifiedBy = x.ModifiedBy
                    });

                    schedules.ToList().ForEach(x =>
                    {
                        sqlContractScheduleLogDetails = sqlContractScheduleLogDetails ?? new List<DbModel.SqlauditLogDetail>();
                        sqlContractScheduleLogDetails.Add(new DbModel.SqlauditLogDetail()
                        {
                            SqlAuditLogId = (long)eventId,
                            SqlAuditSubModuleId = dbModule.FirstOrDefault(x1 => x1.ModuleName == SqlAuditModuleType.ContractSchedule.ToString()).Id,
                            NewValue = x.AuditSerializeAttribute<Object, AuditNameAttribute>("AuditName"),
                            OldValue = null
                        });
                    });


                    //stopWatch.Stop();
                    //TimeSpan ts = stopWatch.Elapsed;
                    //Console.WriteLine("Process Time  for forming Audit Schedule Data" + String.Format("{0:00}:{1:00}:{2:00}.{3:00}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10));

                    //Stopwatch stopWatch1 = new Stopwatch();
                    //stopWatch1.Start();
                    if (dbExpense?.Any() == true)
                    {
                        var rates = _dbContext.ContractSchedule?.AsNoTracking()?.Join(_dbContext.ContractRate,
                             schedule => schedule.Id,
                             rate => rate.ContractScheduleId,
                             (schedule, rate) => new { schedule, rate })
                             ?.Where(x => x.schedule.ContractId == contractId)
                             ?.Select(x => new ContractScheduleRate
                             {
                                 RateId = x.rate.Id,
                                 ContractNumber = contractNumber,
                                 ScheduleName = x.schedule.Name,
                                 ScheduleId = x.schedule.Id,
                                 ExpenseTypeId = x.rate.ExpenseTypeId,
                                 ChargeType = dbExpense.FirstOrDefault(x1 => x1.Id == x.rate.ExpenseTypeId).Name,
                                 ChargeValue = Convert.ToString(x.rate.Rate),
                                 Description = x.rate.Description,
                                 IsDescriptionPrintedOnInvoice = x.rate.IsPrintDescriptionOnInvoice,
                                 StandardValue = x.rate.StandardValue,
                                 DiscountApplied = Convert.ToString(x.rate.DiscountApplied),
                                 Percentage = x.rate.Percentage,
                                 EffectiveFrom = x.rate.FromDate,
                                 EffectiveTo = x.rate.ToDate,
                                 IsActive = x.rate.IsActive,
                                 //LastModification = x.rate.LastModification,
                                 ModifiedBy = x.rate.ModifiedBy,
                                 Currency = x.schedule.Currency,
                                 BaseScheduleId = x.schedule.BaseScheduleId,
                                 BaseRateId = x.rate.BaseRateId,
                                 ChargeRateType = dbExpense.FirstOrDefault(x1 => x1.Id == x.rate.ExpenseTypeId).Type,
                                 StandardChargeSchedule = x.rate.StandardInspectionTypeChargeRate != null ? x.rate.StandardInspectionTypeChargeRate.CompanyChgSchInspGrpInspectionType.CompanyChgSchInspGroup.CompanyChargeSchedule.StandardChargeSchedule.Name : string.Empty,
                                 StandardChargeScheduleInspectionGroup = x.rate.StandardInspectionTypeChargeRate != null ? x.rate.StandardInspectionTypeChargeRate.CompanyChgSchInspGrpInspectionType.CompanyChgSchInspGroup.StandardInspectionGroup.Name : string.Empty,
                                 StandardChargeScheduleInspectionType = x.rate.StandardInspectionTypeChargeRate != null ? x.rate.StandardInspectionTypeChargeRate.CompanyChgSchInspGrpInspectionType.StandardInspectionType.Name : string.Empty,
                                 StandardInspectionTypeChargeRateId = x.rate.StandardInspectionTypeChargeRate != null ? x.rate.StandardInspectionTypeChargeRate.Id : 0,
                             })?.ToList();
                        //stopWatch1.Stop();
                        //TimeSpan ts1 = stopWatch1.Elapsed;
                        //Console.WriteLine("Process Time for forming Audit Rate Data" + String.Format("{0:00}:{1:00}:{2:00}.{3:00}", ts1.Hours, ts1.Minutes, ts1.Seconds, ts1.Milliseconds / 10));

                        rates?.ToList().ForEach(x =>
                        {
                            sqlContractRateLogDetails = sqlContractRateLogDetails ?? new List<DbModel.SqlauditLogDetail>();
                            sqlContractRateLogDetails.Add(new DbModel.SqlauditLogDetail()
                            {
                                SqlAuditLogId = (long)eventId,
                                SqlAuditSubModuleId = dbModule.FirstOrDefault(x1 => x1.ModuleName == SqlAuditModuleType.ContractRate.ToString()).Id,
                                NewValue = x.AuditSerializeAttribute<Object, AuditNameAttribute>("AuditName"),
                                OldValue = null
                            });
                        });
                    }

                }

                //Stopwatch stopWatch2 = new Stopwatch();
                //stopWatch2.Start();

                if (sqlContractScheduleLogDetails?.Any() == true && IsBulk == true)
                {
                    _dbContext.ChangeTracker.AutoDetectChangesEnabled = false;
                    _dbContext.BulkInsert(sqlContractScheduleLogDetails);
                }

                //stopWatch2.Stop();
                //TimeSpan ts2 = stopWatch2.Elapsed;
                //Console.WriteLine("Process Time Audit Schedule Bulk Insert" + String.Format("{0:00}:{1:00}:{2:00}.{3:00}", ts2.Hours, ts2.Minutes, ts2.Seconds, ts2.Milliseconds / 10));

                //Stopwatch stopWatch3 = new Stopwatch();
                //stopWatch3.Start();

                if (sqlContractRateLogDetails?.Any() == true && IsBulk == true)
                {
                    _dbContext.ChangeTracker.AutoDetectChangesEnabled = false;
                    _dbContext.BulkInsert(sqlContractRateLogDetails);
                }


                if (sqlContractScheduleLogDetails?.Any() == true && IsBulk == false)
                {
                    _dbContext.ChangeTracker.AutoDetectChangesEnabled = false;
                    _dbContext.AddRange(sqlContractScheduleLogDetails);
                    _dbContext.SaveChanges();
                }

                if (sqlContractRateLogDetails?.Any() == true && IsBulk == false)
                {
                    _dbContext.ChangeTracker.AutoDetectChangesEnabled = false;
                    _dbContext.AddRange(sqlContractRateLogDetails);
                    _dbContext.SaveChanges();
                }

                //stopWatch3.Stop();
                //TimeSpan ts3 = stopWatch3.Elapsed;
                //Console.WriteLine("Process Time Audit Rate Bulk Insert" + String.Format("{0:00}:{1:00}:{2:00}.{3:00}", ts3.Hours, ts3.Minutes, ts3.Seconds, ts3.Milliseconds / 10));
            }
            catch (Exception ex)
            {
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), null);
            }
        }

        public void Dispose()
        {
            GC.Collect();
        }
    }
}