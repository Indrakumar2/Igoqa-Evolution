using AutoMapper;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.GenericDbRepository.Services;
using Evolution.TechnicalSpecialist.Domain.Interfaces.Data;
using Evolution.TechnicalSpecialist.Domain.Models.TechSpecialist;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;

namespace Evolution.TechnicalSpecialist.Infrastructure.Data
{
    public class TechnicalSpecialistNoteRepository : GenericRepository<DbModel.TechnicalSpecialistNote>, ITechnicalSpecialistNoteRepository
    {
        private readonly DbModel.EvolutionSqlDbContext _dbContext = null;
        private readonly IMapper _mapper = null;

        public TechnicalSpecialistNoteRepository(DbModel.EvolutionSqlDbContext dbContext, IMapper mapper) : base(dbContext)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public IList<DbModel.TechnicalSpecialistNote> Get(IList<long> tsNoteIds)
        {
            return Get(noteIds: tsNoteIds);
        }

        public IList<DbModel.TechnicalSpecialistNote> Get(NoteType noteType, IList<string> tsPins = null, IList<int> recordRefId = null)
        {
            return Get(noteType, pins: tsPins,recordRefId: recordRefId);
        }
         
        public IList<DbModel.TechnicalSpecialistNote> Get(NoteType noteType, bool fetchLatest = false, IList<string> tsPins = null, IList<int> recordRefId = null)
        {
            return Get(noteType, fetchLatest, pins: tsPins, recordRefId: recordRefId);
        }

        public IList<DbModel.TechnicalSpecialistNote> GetByPinId(IList<string> pinIds)
        {
            return Get(pins: pinIds);
        }

        public IList<DbModel.TechnicalSpecialistNote> Search(TechnicalSpecialistNoteInfo searchModel)
        {
            var tsNoteAsQueryable = this.PopulateTsNoteAsQuerable(searchModel);
            return Get(tsNoteAsQueryable, null, null);
        }

        public IList<DbModel.TechnicalSpecialistNote> Search(IList<string> recordType,IList<int> epin)
        {

            IQueryable<DbModel.TechnicalSpecialistNote> tsNoteAsQueryable = _dbContext.TechnicalSpecialistNote;

            if (epin.Count > 0)
            {
                tsNoteAsQueryable = tsNoteAsQueryable.Where(x => epin.Contains(x.TechnicalSpecialist.Pin));
            }

            if (recordType.Count > 0)
            {
                tsNoteAsQueryable = tsNoteAsQueryable.Where(x => recordType.Contains(x.RecordType));
            }

            return tsNoteAsQueryable.ToList();

        }


        private IList<DbModel.TechnicalSpecialistNote> Get(IQueryable<DbModel.TechnicalSpecialistNote> tsNoteAsQuerable = null,
                                                            IList<long> noteIds = null,
                                                            IList<string> pins = null)
        {
            if (tsNoteAsQuerable == null)
                tsNoteAsQuerable = _dbContext.TechnicalSpecialistNote;

            if (noteIds?.Count > 0)
                tsNoteAsQuerable = tsNoteAsQuerable.Where(x => noteIds.Contains(x.Id));

            if (pins?.Count > 0)
                tsNoteAsQuerable = tsNoteAsQuerable.Where(x => pins.Contains(x.TechnicalSpecialist.Pin.ToString()));

            return tsNoteAsQuerable.Include(x => x.TechnicalSpecialist)
                                  .ToList();

        }

        private IList<DbModel.TechnicalSpecialistNote> Get(NoteType noteType, bool fetchLatest=false, IQueryable<DbModel.TechnicalSpecialistNote> tsNoteAsQuerable = null,
                                                           IList<long> noteIds = null,
                                                           IList<string> pins = null,
                                                           IList<int> recordRefId = null)
        {
            IList<DbModel.TechnicalSpecialistNote> result = null;

            if (tsNoteAsQuerable == null)
                tsNoteAsQuerable = _dbContext.TechnicalSpecialistNote.Where(x => x.RecordType == noteType.ToString());

            if (recordRefId?.Count > 0)
                tsNoteAsQuerable = _dbContext.TechnicalSpecialistNote.Where(x => recordRefId.Contains( x.RecordRefId.Value ));

            if (tsNoteAsQuerable == null)
                tsNoteAsQuerable = _dbContext.TechnicalSpecialistNote;

            if (noteIds?.Count > 0)
                tsNoteAsQuerable = tsNoteAsQuerable.Where(x => noteIds.Contains(x.Id));

            if (pins?.Count > 0)
                tsNoteAsQuerable = tsNoteAsQuerable.Where(x => pins.Contains(x.TechnicalSpecialist.Pin.ToString()));

            if (fetchLatest)
            { 
                var tsNotes = tsNoteAsQuerable.ToList(); 
                result= tsNotes.GroupJoin(tsNotes,
                                            ln => new { ln.TechnicalSpecialistId,ln.RecordRefId },
                                            rn => new { rn.TechnicalSpecialistId, rn.RecordRefId },
                                            (ln, rn) => new { ln, rn }).
                                            Where(x => x.ln.CreatedDate == x.rn.Max(x1 => x1.CreatedDate)).
                                            Select(x => x.ln).ToList(); 
            }
            else
            {
                result = tsNoteAsQuerable.Include(x => x.TechnicalSpecialist).ToList();
            } 
            return result;
        }
      
        private IQueryable<DbModel.TechnicalSpecialistNote> PopulateTsNoteAsQuerable(TechnicalSpecialistNoteInfo model)
        {
            var dbSearchModel = this._mapper.Map<DbModel.TechnicalSpecialistNote>(model);
            IQueryable<DbModel.TechnicalSpecialistNote> tsNoteAsQueryable = _dbContext.TechnicalSpecialistNote;
            if (model.Epin > 0)
            {
                tsNoteAsQueryable = tsNoteAsQueryable.Where(x => x.TechnicalSpecialist.Pin == model.Epin);
            }
            if (model.RecordType == "TSComment")
            {
                tsNoteAsQueryable = tsNoteAsQueryable.Where(x => x.RecordType == "TSComment" || x.RecordType == "General" || x.RecordType == "TSRejectComment"); // def 1306 
            }
            else
            {
                var defWhereExpr = dbSearchModel.ToExpression();
                if (defWhereExpr != null)
                {
                    tsNoteAsQueryable = tsNoteAsQueryable.Where(defWhereExpr);
                }
            }
            return tsNoteAsQueryable;
        }
    }
}
