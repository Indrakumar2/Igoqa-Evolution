using AutoMapper;
using AutoMapper.QueryableExtensions;
using Evolution.Assignment.Domain.Interfaces.Data;
using Evolution.Common.Extensions;
using Evolution.DbRepository.Models.SqlDatabaseContext;
using Evolution.GenericDbRepository.Services;
using System.Collections.Generic;
using System.Linq;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.Assignment.Domain.Models.Assignments;

namespace Evolution.Assignment.Infrastructure.Data
{
    public class AssignmentNoteRepository : GenericRepository<DbModel.AssignmentNote>, IAssignmentNoteRepository
    {
        private EvolutionSqlDbContext _dbContext = null;
        private IMapper _mapper = null;

        public AssignmentNoteRepository(EvolutionSqlDbContext dbContext, IMapper mapper) : base(dbContext)
        {
            this._dbContext = dbContext;
            this._mapper = mapper;
        }

        public IList<DomainModel.AssignmentNote> Search(DomainModel.AssignmentNote model)
        {
            var expression = this._mapper.Map<DbModel.AssignmentNote>(model).ToExpression();
            var whereClause = _dbContext.AssignmentNote.AsQueryable();
            if (expression != null)
            {
                whereClause = whereClause.Where(expression);
            }
            return whereClause.GroupJoin(_dbContext.User,
            Note => Note.CreatedBy,
            User => User.SamaccountName,
            (Note, User) => new { Note, User })
            .Select(x=> new DomainModel.AssignmentNote()
            { 
                Note = x.Note.Note,
                CreatedBy = x.Note.CreatedBy,
                CreatedOn = x.Note.CreatedDate,
                AssignmentId = x.Note.AssignmentId,
                AssignmnetNoteId = x.Note.Id,
                UserDisplayName = x.User != null && x.User.Count() > 0 ? x.User.FirstOrDefault().Name : string.Empty,
                UpdateCount = x.Note.UpdateCount,
                LastModification = x.Note.LastModification,
                ModifiedBy = x.Note.ModifiedBy
            })?.ToList();
        }

        public void Dispose()
        {
            _dbContext.Dispose();
            _mapper = null;
        }
    }
}