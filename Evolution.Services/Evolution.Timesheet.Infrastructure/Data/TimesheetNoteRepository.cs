using AutoMapper;
using AutoMapper.QueryableExtensions;
using Evolution.Common.Extensions;
using Evolution.DbRepository.Models.SqlDatabaseContext;
using Evolution.GenericDbRepository.Services;
using Evolution.Timesheet.Domain.Interfaces.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.Timesheet.Domain.Models.Timesheets;

namespace Evolution.Timesheet.Infrastructure.Data
{
    public class TimesheetNoteRepository : GenericRepository<DbModel.TimesheetNote>, ITimesheetNoteRepository
    {
        private readonly EvolutionSqlDbContext _dbContext = null;
        private readonly IMapper _mapper = null;

        public TimesheetNoteRepository(EvolutionSqlDbContext dbContext, IMapper mapper) : base(dbContext)
        {
            this._dbContext = dbContext;
            this._mapper = mapper;
        }

        public IList<DomainModel.TimesheetNote> Search(DomainModel.TimesheetNote model)
        {
            var dbSearchModel = this._mapper.Map<DbModel.TimesheetNote>(model);
            var expression = dbSearchModel.ToExpression((new List<string> { nameof(dbSearchModel.IsCustomerVisible), nameof(dbSearchModel.IsSpecialistVisible) }));
            var whereClause = this._dbContext.TimesheetNote.AsQueryable();
            if (expression != null)
                whereClause = whereClause.Where(expression);

            return whereClause.GroupJoin(_dbContext.User,
         Note => Note.CreatedBy,
         User => User.SamaccountName,
         (Note, User) => new DomainModel.TimesheetNote()
         {
             Note = Note.Note,
             CreatedBy = Note.CreatedBy,
             CreatedOn = Note.CreatedDate,
             TimesheetId = Note.TimesheetId,
             TimesheetNoteId = Note.Id,
             IsCustomerVisible = Note.IsCustomerVisible,
             IsSpecialistVisible = Note.IsSpecialistVisible,
             UserDisplayName = User != null && User.Count() > 0 ? User.FirstOrDefault().Name : string.Empty,
             UpdateCount = Note.UpdateCount,
             LastModification = Note.LastModification,
             ModifiedBy = Note.ModifiedBy
         })?.ToList();
        }

        //To be deleted later. Added only for sync purpose
        public long? GetMaxEvoId()
        {
            var dbVisitNote = _dbContext.VisitNote.FromSql("SELECT TOP 1 Id,EvoId FROM visit.VisitNote with(nolock) ORDER By ID DESC")?.AsNoTracking()?.Select(x => new DbModel.VisitNote() { Id = x.Id, Evoid = x.Evoid });
            long? visitNoteId = dbVisitNote?.FirstOrDefault()?.Evoid ?? 0;
            var dbTimesheetNote = _dbContext.TimesheetNote.FromSql("SELECT TOP 1 Id,EvoId FROM timesheet.TimesheetNote with(nolock) ORDER By ID DESC")?.AsNoTracking()?.Select(x => new DbModel.TimesheetNote() { Id = x.Id, Evoid = x.Evoid })?.FirstOrDefault();
            long? timesheetNoteId = dbTimesheetNote?.Evoid ?? 0;

            if (visitNoteId == 0 && timesheetNoteId == 0)
                return dbTimesheetNote?.Id;
            if (visitNoteId > timesheetNoteId)
                return visitNoteId;
            else
                return timesheetNoteId;
        }
        
        public void Dispose()
        {
            this._dbContext.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
