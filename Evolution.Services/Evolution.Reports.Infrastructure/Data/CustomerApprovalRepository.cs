using AutoMapper;
using AutoMapper.QueryableExtensions;
using Evolution.Common.Extensions;
using Evolution.GenericDbRepository.Services;
using Evolution.Reports.Domain.Interfaces.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.Reports.Domain.Models.Reports;

namespace Evolution.Reports.Infrastructure.Data
{
    public class CustomerApprovalRepository : GenericRepository<DbModel.TechnicalSpecialistCustomerApproval>, ICustomerApprovalRepository
    {

        private DbModel.EvolutionSqlDbContext _dbContext = null;
        private IMapper _mapper = null;

        public CustomerApprovalRepository(DbModel.EvolutionSqlDbContext dbContext, IMapper mapper) : base(dbContext)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public IList<DomainModel.CustomerApproval> Search(DomainModel.CustomerApprovalSearch searchModel)
        {
            var dbSearchModel = _mapper.Map<DbModel.TechnicalSpecialistCustomerApproval>(searchModel);
            var expression = dbSearchModel.ToExpression();
            var whereClause = FilterRecord(searchModel);

            if (expression == null)
                return whereClause.ProjectTo<DomainModel.CustomerApproval>().ToList();
            else
                return whereClause.Where(expression).ProjectTo<DomainModel.CustomerApproval>().ToList();

        }

        private IQueryable<DbModel.TechnicalSpecialistCustomerApproval> FilterRecord(DomainModel.CustomerApprovalSearch searchModel)
        {
            IQueryable<DbModel.TechnicalSpecialistCustomerApproval> whereClause = null; 
           //FirstName
             if (searchModel.FirstName.HasEvoWildCardChar())
                whereClause = _dbContext.TechnicalSpecialistCustomerApproval.WhereLike(x => x.TechnicalSpecialist.FirstName, searchModel.FirstName, '*');
            else
                whereClause = _dbContext.TechnicalSpecialistCustomerApproval.Where(x => string.IsNullOrEmpty(searchModel.FirstName) || x.TechnicalSpecialist.FirstName == searchModel.FirstName);


            return whereClause;
        }


    }
}
