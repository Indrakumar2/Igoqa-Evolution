using AutoMapper;
using AutoMapper.QueryableExtensions;
using Evolution.Common.Extensions;
using Evolution.Company.Domain.Interfaces.Data;
using Evolution.DbRepository.Models.SqlDatabaseContext;
using Evolution.GenericDbRepository.Services;
using System.Collections.Generic;
using System.Linq;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.Company.Domain.Models.Companies;

namespace Evolution.Company.Infrastructure.Data
{
    public class CompanyNoteRepository : GenericRepository<Evolution.DbRepository.Models.SqlDatabaseContext.CompanyNote>, ICompanyNoteRepository
    {
        private readonly EvolutionSqlDbContext _dbContext = null;
        private readonly IMapper _mapper = null;

        public CompanyNoteRepository(IMapper mapper, EvolutionSqlDbContext dbContext) : base(dbContext)
        {
            this._dbContext = dbContext;
            this._mapper = mapper;
        }

        public IList<DomainModel.CompanyNote> Search(Domain.Models.Companies.CompanyNote searchModel)
        {
            var dbSearchModel = this._mapper.Map<DbModel.CompanyNote>(searchModel);
            IQueryable<DbModel.CompanyNote> whereClause = null;

            var compNotes = _dbContext.CompanyNote;

            //Wildcard Search for Company Code
            if (searchModel.CompanyCode.HasEvoWildCardChar())
                whereClause = compNotes.WhereLike(x => x.Company.Code, searchModel.CompanyCode, '*');
            else
                whereClause = compNotes.Where(x => string.IsNullOrEmpty(searchModel.CompanyCode) || x.Company.Code == searchModel.CompanyCode);

            //Wildcard Search for Note
            if (searchModel.Notes.HasEvoWildCardChar())
                whereClause = whereClause.WhereLike(x => x.Note, searchModel.Notes, '*');
            else
                whereClause = whereClause.Where(x => string.IsNullOrEmpty(searchModel.Notes) || x.Note == searchModel.Notes);

            var expression = dbSearchModel.ToExpression();
            if (expression != null)
                whereClause = whereClause.Where(expression);

            return whereClause.GroupJoin(_dbContext.User,
      Note => Note.CreatedBy,
      User => User.SamaccountName,
      (Note, User) => new DomainModel.CompanyNote()
      {
          Notes = Note.Note,
          CreatedBy = Note.CreatedBy,
          CreatedOn = Note.CreatedDate,
          CompanyNoteId = Note.Id,
          UserDisplayName = User != null && User.Count() > 0 ? User.FirstOrDefault().Name : string.Empty,
          UpdateCount = Note.UpdateCount,
          LastModification = Note.LastModification,
          ModifiedBy = Note.ModifiedBy,
          CompanyCode = Note.Company.Code
      })?.ToList();
        }
    }
}
