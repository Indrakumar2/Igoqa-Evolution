using AutoMapper;
using AutoMapper.QueryableExtensions;
using Evolution.Admin.Domain.Interfaces.Data;
using Evolution.Common.Extensions;
using Evolution.GenericDbRepository.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using DbModels = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.Admin.Domain.Models.Admins;

namespace Evolution.Admin.Infrastructure.Data
{
    public class AnnouncementRepository : GenericRepository<Evolution.DbRepository.Models.SqlDatabaseContext.Announcement>, IAnnouncementRepository
    {
        private readonly DbModels.EvolutionSqlDbContext _dbContext = null;
        private readonly IMapper _mapper = null;

        public AnnouncementRepository(IMapper mapper, DbModels.EvolutionSqlDbContext dbContext) : base(dbContext)
        {
            this._dbContext = dbContext;
            this._mapper = mapper;
        }

        public IList<DomainModel.Announcement> Search(DomainModel.Announcement searchModel)
        {
            var dbSearchModel = _mapper.Map<DbModels.Announcement>(searchModel);
            var expression = dbSearchModel.ToExpression();
            //IQueryable<DbModels.Announcement> whereClause = _dbContext.Announcement.Where(x => x.DisplayTill >= DateTime.Now);
            var whereClause =_GetAnnouncement(_dbContext).AsQueryable();

            if (expression == null)
                return whereClause.ProjectTo<DomainModel.Announcement>().ToList();
            else
                return whereClause.Where(expression).ProjectTo<DomainModel.Announcement>().ToList();
        }

        private static readonly Func<DbModels.EvolutionSqlDbContext, IEnumerable<DbModels.Announcement>> _GetAnnouncement =
          EF.CompileQuery((DbModels.EvolutionSqlDbContext dbContext) =>
              dbContext.Announcement.Where(x => x.DisplayTill >= DateTime.Now && x.DisplayFrom <= DateTime.Now));
    }
}
