using AutoMapper;
using AutoMapper.QueryableExtensions;
using Evolution.Common.Extensions;
using Evolution.GenericDbRepository.Services;
using Evolution.Project.Domain.Interfaces.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.Project.Domain.Models.Projects;

namespace Evolution.Project.Infrastructure.Data
{
    public class ProjectClientNotificationRepository : GenericRepository<DbModel.ProjectClientNotification>, IProjectClientNotificationRepository
    {
        private IMapper _mapper = null;
        private DbModel.EvolutionSqlDbContext _dbContext = null;

        public ProjectClientNotificationRepository(IMapper mapper, DbModel.EvolutionSqlDbContext dbContext) : base(dbContext)
        {
            _mapper = mapper;
            _dbContext = dbContext;
        }

        public IList<DomainModel.ProjectClientNotification> Search(DomainModel.ProjectClientNotification searchModel)
        {
            var dbSearchModel = _mapper.Map<DbModel.ProjectClientNotification>(searchModel);
          
            IQueryable<DbModel.ProjectClientNotification> whereClause = null;
            var projectClientNotification = _dbContext.ProjectClientNotification;

            if (searchModel.ProjectNumber.HasValue)
                whereClause = projectClientNotification.Where(x => x.Project.ProjectNumber == searchModel.ProjectNumber);

            //Wildcard Search for ContactName
            if (searchModel.CustomerContact.HasEvoWildCardChar())
                whereClause = whereClause.WhereLike(x => x.CustomerContact.ContactName, searchModel.CustomerContact, '*');
            else
                whereClause = whereClause.Where(x => string.IsNullOrEmpty(searchModel.CustomerContact) || x.CustomerContact.ContactName == searchModel.CustomerContact);
                        
            return whereClause.ProjectTo<DomainModel.ProjectClientNotification>().ToList();
        }
    }
}
