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
    public class ContractNoteRepository : GenericRepository<DbModel.ContractNote>, IContractNoteRepository
    {
        private readonly DbModel.EvolutionSqlDbContext _dbContext = null;
        private readonly IMapper _mapper = null;
        private readonly IAppLogger<ContractNoteRepository> _logger = null;

        public ContractNoteRepository(EvolutionSqlDbContext dbContext,
            IMapper mapper,
            IAppLogger<ContractNoteRepository> logger) : base(dbContext)
        {
            this._dbContext = dbContext;
            this._mapper = mapper;
            _logger = logger;
        }

        public IList<DomainModel.ContractNote> Search(DomainModel.ContractNote searchModel)
        {
            var dbSearchModel = this._mapper.Map<DbModel.ContractNote>(searchModel);
            IQueryable<DbModel.ContractNote> whereClause = null;

            var contractNotes = _dbContext.ContractNote;

            //Wildcard Search for Contract Number
            if (searchModel.ContractNumber.HasEvoWildCardChar())
                whereClause = contractNotes.WhereLike(x => x.Contract.ContractNumber, searchModel.ContractNumber, '*');
            else
                whereClause = contractNotes.Where(x => string.IsNullOrEmpty(searchModel.ContractNumber) || x.Contract.ContractNumber == searchModel.ContractNumber);

            var expression = dbSearchModel.ToExpression();
            if (expression != null)
                whereClause = whereClause.Where(expression);
            return whereClause.GroupJoin(_dbContext.User,
                                          Note => Note.CreatedBy,
                                          User => User.SamaccountName,
                                          (Note, User) => new DomainModel.ContractNote()
                                          {
                                              Notes = Note.Note,
                                              CreatedBy = Note.CreatedBy,
                                              CreatedOn = Note.CreatedDate,
                                              ContractNoteId = Note.Id,
                                              UserDisplayName = User != null && User.Count() > 0 ? User.FirstOrDefault().Name : string.Empty,
                                              UpdateCount = Note.UpdateCount,
                                              LastModification = Note.LastModification,
                                              ModifiedBy = Note.ModifiedBy,
                                              ContractNumber= Note.Contract.ContractNumber
                                          })?.ToList();

        }

        public int DeleteNote(List<DomainModel.ContractNote> contractNotes)
        {
            var noteIds = contractNotes?.Where(x => x.ContractNoteId > 0)?.Select(x => x.ContractNoteId)?.Distinct().ToList();
            return DeleteNote(noteIds);
        }

        public int DeleteNote(List<DbModel.ContractNote> contractNotes)
        {
            var noteIds = contractNotes?.Select(x => x.Id)?.Distinct().ToList();
            return DeleteNote(noteIds);
        }

        public int DeleteNote(List<int> noteIds)
        {
            int count = 0;
            try
            {
                if (noteIds.Count > 0)
                {
                    var deleteStatement = string.Format(Utility.GetSqlQuery(SQLModuleType.Contract_Note, SQLModuleActionType.Delete), string.Join(",", noteIds.ToString<int>()));
                    count = _dbContext.Database.ExecuteSqlCommand(deleteStatement);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), "noteIds=" + noteIds.ToString<int>());
            }

            return count;
        }
    }
}
