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
using Evolution.Logging.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Evolution.Visit.Infrastructure.Data
{
    public class VisitNoteRepository : GenericRepository<DbModel.VisitNote>, IVisitNotesRepository
    {
        private readonly DbModel.EvolutionSqlDbContext _dbContext = null;
        private readonly IMapper _mapper = null;
        private readonly IAppLogger<VisitNoteRepository> _logger = null;

        public VisitNoteRepository(DbModel.EvolutionSqlDbContext dbContext, IMapper mapper, IAppLogger<VisitNoteRepository> logger) : base(dbContext)
        {
            this._dbContext = dbContext;
            this._mapper = mapper;
            this._logger = logger;
        }

        public IList<DomainModel.VisitNote> Search(DomainModel.VisitNote searchModel)
        {
            var dbSearchModel = this._mapper.Map<DbModel.VisitNote>(searchModel);
            IQueryable<DbModel.VisitNote> whereClause = null;

            whereClause = _dbContext.VisitNote.Where(x => x.VisitId == searchModel.VisitId);

            var expression = dbSearchModel.ToExpression();
            if (expression != null)
                whereClause = whereClause.Where(expression);

            return whereClause.GroupJoin(_dbContext.User,
            Note => Note.CreatedBy,
            User => User.SamaccountName,
            (Note, User) => new DomainModel.VisitNote()
            {
                Note = Note.Note,
                CreatedBy = Note.CreatedBy,
                CreatedOn = Note.CreatedDate,
                VisitId = Note.VisitId,
                VisitNoteId = Note.Id,
                VisibleToCustomer = Note.IsCustomerVisible,
                VisibleToSpecialist = Note.IsSpecialistVisible,
                UserDisplayName = User != null && User.Count() > 0 ? User.FirstOrDefault().Name : string.Empty,
                UpdateCount = Note.UpdateCount,
                LastModification = Note.LastModification,
                ModifiedBy = Note.ModifiedBy
            })?.ToList();
        }

        //To be deleted later. Added only for sync purpose
        public long? GetMaxEvoId()
        {
            //var dbVisitNote = _dbContext.VisitNote.FromSql("SELECT TOP 1 Id, Evoid FROM visit.VisitNote with(nolock) ORDER By ID DESC")?.Select(x => new DbModel.VisitNote() { Id = x.Id, Evoid = x.Evoid });
            var dbVisitNote = _dbContext.VisitNote.OrderByDescending(x => x.Id)?.AsNoTracking()?.Select(x => new { Id = x.Id, Evoid = x.Evoid });
            long? visitNoteId = dbVisitNote.FirstOrDefault()?.Evoid ?? 0;
            //var dbTimesheetNote = _dbContext.TimesheetNote.FromSql("SELECT TOP 1 Id, Evoid FROM timesheet.TimesheetNote with(nolock) ORDER By ID DESC")?.Select(x => new DbModel.TimesheetNote() { Id = x.Id, Evoid = x.Evoid });
            var dbTimesheetNote = _dbContext.TimesheetNote.OrderByDescending(x => x.Id)?.AsNoTracking().Select(x => new { Id = x.Id, Evoid = x.Evoid });
            long? timesheetNoteId = dbTimesheetNote?.FirstOrDefault()?.Evoid ?? 0;

            if (visitNoteId == 0 && timesheetNoteId == 0)
                return dbTimesheetNote?.FirstOrDefault()?.Id;
            if (visitNoteId > timesheetNoteId)
                return visitNoteId;
            else
                return timesheetNoteId;
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }
    }
}
