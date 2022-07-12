using AutoMapper;
using AutoMapper.QueryableExtensions;
using Evolution.Common.Extensions;
using Evolution.GenericDbRepository.Services;
using Evolution.Reports.Domain.Interfaces.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.Reports.Domain.Models.Reports;
namespace Evolution.Reports.Infrastructure.Data
{
    public class WonLostRepository : GenericRepository<DbModel.ResourceSearch>, IWonLostRepository
    {
        private DbModel.EvolutionSqlDbContext _dbContext = null;
        private IMapper _mapper = null;

        public WonLostRepository(DbModel.EvolutionSqlDbContext dbContext, IMapper mapper) : base(dbContext)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public IList<DomainModel.WonLost> Search(DomainModel.WonLostSearch searchModel)
        {
            var dbSearchModel = _mapper.Map<DbModel.ResourceSearch>(searchModel);

            List<DomainModel.WonLost> result = null;
            var expression = dbSearchModel.ToExpression();
            var whereClause = FilterRecord(searchModel);

            if (expression == null)
                result = whereClause.ProjectTo<DomainModel.WonLost>().ToList();
            else
                result = whereClause.ProjectTo<DomainModel.WonLost>().ToList();

            var distinctId = result.Select(x1 => x1.Id).Distinct().ToList();

            var dbResourceSearchNotes = _dbContext.ResourceSearchNote.OrderByDescending(x1 => x1.Id).Where(x2 => distinctId.Contains(x2.ResourceSearchId)).ToList();

            var response = result.GroupJoin(dbResourceSearchNotes,
                            RS => new { Id = RS.Id },
                            RSN => new { Id = RSN.ResourceSearchId },
                            (RS, RSN) => new { RS, RSN })
                            .Select(x =>
                            {
                                x.RS.Description = x.RSN?.FirstOrDefault()?.Note;
                                return x.RS;
                            }).ToList();

            return response;


        }

        private IQueryable<DbModel.ResourceSearch> FilterRecord(DomainModel.WonLostSearch searchModel)
        {
            IQueryable<DbModel.ResourceSearch> whereClause = _dbContext.ResourceSearch;

            //ActionStatus
            if (searchModel.Action != null)
            {

                whereClause = whereClause.Where(x => string.IsNullOrEmpty(searchModel.Action) || x.ActionStatus == searchModel.Action);
            }

            //Disposition Type //769
            if (searchModel.DispositionType != null)
            {

                whereClause = whereClause.Where(x => string.IsNullOrEmpty(searchModel.DispositionType) || x.DispositionType == searchModel.DispositionType);
            }

            //SearchType
            if (searchModel.SearchType != null)
            {
                if (searchModel.SearchType.HasEvoWildCardChar())
                    whereClause = whereClause.WhereLike(x => x.SearchType, searchModel.SearchType, '*');
                else
                    whereClause = whereClause.Where(x => string.IsNullOrEmpty(searchModel.SearchType) || x.SearchType != searchModel.SearchType);
            }

            //CompanyCode

            if (!string.IsNullOrEmpty(searchModel.CompanyCode))
            {
                var chCompanyCode = string.Format(@"""chCompanyCode"":""{0}"",", searchModel.CompanyCode);
                var opCompanyCode = string.Format(@"""opCompanyCode"":""{0}"",", searchModel.CompanyCode);
                whereClause = whereClause.Where(x => (x.SerilizableObject.Contains(chCompanyCode) || x.SerilizableObject.Contains(opCompanyCode)));
            }
            //Co_Ordinator
            if (!string.IsNullOrEmpty(searchModel.AssginedTo))
            {
                var chCordinator = string.Format(@"""chCoordinatorLogOnName"":""{0}"",", searchModel.AssginedTo);
                var opCordinator = string.Format(@"""opCoordinatorLogOnName"":""{0}"",", searchModel.AssginedTo);
                whereClause = whereClause.Where(x => (x.SerilizableObject.Contains(chCordinator) || x.SerilizableObject.Contains(opCordinator)));
            }
            //Created_From_Date
            if (searchModel.FromDate != null && searchModel.ToDate == null)
                whereClause = whereClause.Where(x => x.CreatedOn.Date >= searchModel.FromDate.Value.Date); // Modified IGO 914
            else if (searchModel.ToDate != null && searchModel.FromDate == null)
                whereClause = whereClause.Where(x => x.CreatedOn.Date == searchModel.ToDate.Value.Date);
            else if (searchModel.ToDate != null && searchModel.FromDate != null)
                whereClause = whereClause.Where(x => x.CreatedOn.Date >= searchModel.FromDate.Value.Date && x.CreatedOn.Date <= searchModel.ToDate.Value.Date);

            return whereClause;
        }

    }
}
