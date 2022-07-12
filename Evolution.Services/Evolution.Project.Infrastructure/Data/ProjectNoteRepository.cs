using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Evolution.Common.Extensions;
using Evolution.DbRepository.Models.SqlDatabaseContext;
using Evolution.GenericDbRepository.Services;
using Evolution.Project.Domain.Interfaces.Data;
using Evolution.Project.Domain.Models.Projects;
using Microsoft.EntityFrameworkCore;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.Project.Domain.Models.Projects;

namespace Evolution.Project.Infrastructure.Data
{
    public class ProjectNoteRepository : GenericRepository<DbModel.ProjectNote>, IProjectNoteRepository
    {
        private readonly EvolutionSqlDbContext _dbContext = null;
        private readonly IMapper _mapper = null;

        public ProjectNoteRepository(EvolutionSqlDbContext dbContext, IMapper mapper) : base(dbContext)

        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public IList<DomainModel.ProjectNote> Search(DomainModel.ProjectNote searchModel)
        {
            var dbSearchModel = this._mapper.Map<DbModel.ProjectNote>(searchModel);
            IQueryable<DbModel.ProjectNote> whereClause = null;
            var projectNotes = _dbContext.ProjectNote;

            var expression = dbSearchModel.ToExpression();

            if (searchModel.ProjectNumber.HasValue)
                whereClause = projectNotes.Where(x => x.Project.ProjectNumber == searchModel.ProjectNumber);

            //Wildcard Search for Note
            if (searchModel.Notes.HasEvoWildCardChar())
                whereClause = whereClause.WhereLike(x => x.Note, searchModel.Notes, '*');
            else
                whereClause = whereClause.Where(x => string.IsNullOrEmpty(searchModel.Notes) || x.Note == searchModel.Notes);

            if (expression != null)
                whereClause = whereClause.Where(expression);

            return whereClause.GroupJoin(_dbContext.User,
            Note => Note.CreatedBy,
            User => User.SamaccountName,
            (Note, User) => new DomainModel.ProjectNote()
            {
                Notes = Note.Note,
                CreatedBy = Note.CreatedBy,
                CreatedOn = Note.CreatedDate,
                ProjectNoteId = Note.Id,
                UserDisplayName = User != null && User.Count() > 0 ? User.FirstOrDefault().Name : string.Empty,
                UpdateCount = Note.UpdateCount,
                LastModification = Note.LastModification,
                ModifiedBy = Note.ModifiedBy,
                ProjectNumber=Note.Project.ProjectNumber
            })?.ToList();
        }

    }


}
