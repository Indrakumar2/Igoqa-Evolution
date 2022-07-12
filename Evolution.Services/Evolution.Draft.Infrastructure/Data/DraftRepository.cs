using AutoMapper;
using AutoMapper.QueryableExtensions;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.DbRepository.Models.SqlDatabaseContext;
using Evolution.Draft.Domain.Interfaces.Data;
using Evolution.GenericDbRepository.Services;
using Evolution.TechnicalSpecialist.Domain.Interfaces.Data;
using Evolution.Logging.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.Draft.Domain.Models;

namespace Evolution.TechnicalSpecialist.Infrastructure.Data
{
    public class DraftRepository : GenericRepository<DbModel.Draft>, IDraftRepository
    {
        private readonly DbModel.EvolutionSqlDbContext _dbContext = null;
        private readonly IMapper _mapper = null;
        private readonly IAppLogger<DraftRepository> _logger = null;
        private readonly ITechnicalSpecialistRepository _technicalSpecialistRepository = null;

        public DraftRepository(EvolutionSqlDbContext dbContext, IMapper mapper, ITechnicalSpecialistRepository technicalSpecialistRepository, IAppLogger<DraftRepository> logger) : base(dbContext)
        {
            this._dbContext = dbContext;
            this._mapper = mapper;
            this._technicalSpecialistRepository = technicalSpecialistRepository;
            this._logger = logger;
        }

        public IList<DomainModel.Draft> Search(DomainModel.Draft searchModel)
        {
            var dbSearchModel = this._mapper.Map<DbModel.Draft>(searchModel);
           
            IQueryable<DbModel.Draft> whereClause = _dbContext.Draft;

            if (!string.IsNullOrEmpty(searchModel.DraftId))
                whereClause = whereClause.Where(x => x.DraftId == searchModel.DraftId);

            if (searchModel.Moduletype.HasEvoWildCardChar())
                whereClause = whereClause.WhereLike(x => x.Moduletype, searchModel.Moduletype, '*');
            else
                whereClause = whereClause.Where(x => string.IsNullOrEmpty(searchModel.Moduletype) || x.Moduletype == searchModel.Moduletype);

            if (searchModel.AssignedBy.HasEvoWildCardChar())
                whereClause = whereClause.WhereLike(x => x.AssignedBy, searchModel.AssignedBy, '*');
            else
                whereClause = whereClause.Where(x => string.IsNullOrEmpty(searchModel.AssignedBy) || x.AssignedBy == searchModel.AssignedBy);

            if (searchModel.AssignedTo.HasEvoWildCardChar())
                whereClause = whereClause.WhereLike(x => x.AssignedTo, searchModel.AssignedTo, '*');
            else
                whereClause = whereClause.Where(x => string.IsNullOrEmpty(searchModel.AssignedTo) || x.AssignedTo == searchModel.AssignedTo);

            if (!string.IsNullOrEmpty(searchModel.DraftType))
                whereClause = whereClause.Where(x => x.Description==searchModel.DraftType); 

            var expression = dbSearchModel.ToExpression();

            if (expression == null)
                return whereClause.ProjectTo<DomainModel.Draft>().ToList();
            else
                return whereClause.Where(expression).ProjectTo<DomainModel.Draft>().ToList();
        }

        public IList<DomainModel.Draft> GetDraftMyTask(DomainModel.Draft searchModel)
        {
            var dbSearchModel = this._mapper.Map<DbModel.Draft>(searchModel);
           
            IQueryable<DbModel.Draft> whereClause = _dbContext.Draft;

            if (!string.IsNullOrEmpty(searchModel.DraftId))
                whereClause = whereClause.Where(x => x.DraftId == searchModel.DraftId);

            if (searchModel.Moduletype.HasEvoWildCardChar())
                whereClause = whereClause.WhereLike(x => x.Moduletype, searchModel.Moduletype, '*');
            else
                whereClause = whereClause.Where(x => string.IsNullOrEmpty(searchModel.Moduletype) || x.Moduletype == searchModel.Moduletype);

            if (searchModel.AssignedBy.HasEvoWildCardChar())
                whereClause = whereClause.WhereLike(x => x.AssignedBy, searchModel.AssignedBy, '*');
            else
                whereClause = whereClause.Where(x => string.IsNullOrEmpty(searchModel.AssignedBy) || x.AssignedBy == searchModel.AssignedBy);

            if (searchModel.AssignedToUsers?.Count > 0)
                whereClause = whereClause.Where(x => searchModel.AssignedToUsers.Contains(x.AssignedTo));            

            if (!string.IsNullOrEmpty(searchModel.DraftType))
                whereClause = whereClause.Where(x => x.Description==searchModel.DraftType);

            if (!string.IsNullOrEmpty(searchModel.CompanyCode))//D661 issue1 myTask CR //Commented for D363 CR Change
                whereClause = whereClause.Where(x => x.Company.Code == searchModel.CompanyCode);  //D363 CR Change //D702 #18issue (Ref ALM Doc 11-06-2020)

            var expression = dbSearchModel.ToExpression();

            if (expression == null)
                return whereClause.ProjectTo<DomainModel.Draft>().ToList();
            else
                return whereClause.Where(expression).ProjectTo<DomainModel.Draft>().ToList();
        }

        public void UpdateTechSpec(DomainModel.Draft myDraft, int pendingWithId)
        {
            Exception exception = null;
            try
            {
                int techSpecId =Convert.ToInt32(myDraft.DraftId);
                List<KeyValuePair<string, object>> updateValueProps = new List<KeyValuePair<string, object>>(); 
                List<KeyValuePair<DbModel.TechnicalSpecialist, List<KeyValuePair<string, object>>>> technicalSpecialistUpdate = new List<KeyValuePair<DbModel.TechnicalSpecialist, List<KeyValuePair<string, object>>>>();

                var techSpec = _dbContext.TechnicalSpecialist?.Where(x => x.Pin == techSpecId).Select(x => new DbModel.TechnicalSpecialist { Id = x.Id, Pin = x.Pin , PendingWithId = x.PendingWithId}).FirstOrDefault();
                    if (techSpec != null)
                    {
                        updateValueProps.AddRange(new List<KeyValuePair<string, object>> {
                                            new KeyValuePair<string, object>("PendingWithId", pendingWithId),
                                           });
                        technicalSpecialistUpdate.Add(new KeyValuePair<DbModel.TechnicalSpecialist, List<KeyValuePair<string, object>>>(techSpec, updateValueProps));
                    }

                _technicalSpecialistRepository.Update(technicalSpecialistUpdate,  c => c.PendingWithId);
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), myDraft);
            }
        }
    }
}
