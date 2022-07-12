using AutoMapper;
using AutoMapper.QueryableExtensions;
using Evolution.Common.Extensions;
using Evolution.DbRepository.Models.SqlDatabaseContext;
using Evolution.GenericDbRepository.Services;
using Evolution.Master.Domain.Interfaces.Data;
using Evolution.Master.Domain.Models;
using System.Collections.Generic;
using System.Linq;

namespace Evolution.Master.Infrastructure.Data
{
    public class EmailPlaceholderRepository : GenericRepository<EmailPlaceHolder>, IEmailPlaceholderRepository
    {
        private readonly EvolutionSqlDbContext _dbContext = null;
        private readonly IMapper _mapper = null;

        public EmailPlaceholderRepository(IMapper mapper,EvolutionSqlDbContext dbContext) : base(dbContext)
        {
            this._dbContext = dbContext;
            this._mapper = mapper;
        }

        public IList<EmailPlaceholder> Search(EmailPlaceholder searchModel)
        {
            IQueryable<EmailPlaceHolder> whereClause = _dbContext.EmailPlaceHolder;
            var expression = this._mapper.Map<EmailPlaceHolder>(searchModel).ToExpression();

            if (expression == null)
                return whereClause.ProjectTo<EmailPlaceholder>().ToList();
            else
                return whereClause.Where(expression).ProjectTo<EmailPlaceholder>().ToList();

        }
    }
}