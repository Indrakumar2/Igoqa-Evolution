using AutoMapper;
using AutoMapper.QueryableExtensions;
using Evolution.Common.Extensions;
using Evolution.GenericDbRepository.Services;
using Evolution.Supplier.Domain.Enum;
using Evolution.Supplier.Domain.Interfaces.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.Supplier.Domain.Models.Supplier;

namespace Evolution.Supplier.Infrastructure.Data
{
    public class SupplierPerformanceRepository : GenericRepository<DbModel.VisitSupplierPerformance>, ISupplierPerfomanceRepository
    {
        private DbModel.EvolutionSqlDbContext _dbContext = null;
        private IMapper _mapper = null;

        public SupplierPerformanceRepository(DbModel.EvolutionSqlDbContext dbContext, IMapper mapper) : base(dbContext)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public IList<DomainModel.SupplierPerformanceReport> Search(DomainModel.SupplierPerformanceReportsearch searchModel)
        {
          var dbSearchModel = _mapper.Map<DbModel.VisitSupplierPerformance>(searchModel);
            var expression = dbSearchModel.ToExpression();
            var whereClause = FilterRecord(searchModel);

            if (expression == null)
                return whereClause.ProjectTo<DomainModel.SupplierPerformanceReport>().ToList();
            else
                return whereClause.Where(expression).ProjectTo<DomainModel.SupplierPerformanceReport>().ToList();


        }

        private IQueryable<DbModel.VisitSupplierPerformance> FilterRecord(DomainModel.SupplierPerformanceReportsearch searchModel)
        {
            IQueryable<DbModel.VisitSupplierPerformance> whereClause = null;


            // CHC Code
            if (searchModel.ContractHoldingCompanyCode.HasEvoWildCardChar())
                whereClause = _dbContext.VisitSupplierPerformance.WhereLike(x => x.Visit.Assignment.ContractCompany.Code, searchModel.ContractHoldingCompanyCode, '*');
            else
                whereClause = _dbContext.VisitSupplierPerformance.Where(x => string.IsNullOrEmpty(searchModel.ContractHoldingCompanyCode) || x.Visit.Assignment.ContractCompany.Code == searchModel.ContractHoldingCompanyCode);

            // CHC Name
            if (searchModel.ContractHoldingCompanyName.HasEvoWildCardChar())
                whereClause = whereClause.WhereLike(x => x.Visit.Assignment.ContractCompany.Name, searchModel.ContractHoldingCompanyName, '*');
            else
                whereClause = whereClause.Where(x => string.IsNullOrEmpty(searchModel.ContractHoldingCompanyName) || x.Visit.Assignment.ContractCompany.Name == searchModel.ContractHoldingCompanyName);

            // OC Code
            if (searchModel.OperatingCompanyCode.HasEvoWildCardChar())
                whereClause = whereClause.WhereLike(x => x.Visit.Assignment.OperatingCompany.Code, searchModel.OperatingCompanyCode, '*');
            else
                whereClause = whereClause.Where(x => string.IsNullOrEmpty(searchModel.OperatingCompanyCode) || x.Visit.Assignment.OperatingCompany.Code == searchModel.OperatingCompanyCode);

            //OC NAme
            if (searchModel.OperatingCompanyName.HasEvoWildCardChar())
                whereClause = whereClause.WhereLike(x => x.Visit.Assignment.OperatingCompany.Name, searchModel.OperatingCompanyCode, '*');
            else
                whereClause = whereClause.Where(x => string.IsNullOrEmpty(searchModel.OperatingCompanyName) || x.Visit.Assignment.OperatingCompany.Name == searchModel.OperatingCompanyName);
           
            // supplier Name
            if (searchModel.SupplierName.HasEvoWildCardChar())
                whereClause = whereClause.WhereLike(x => x.Visit.Supplier.SupplierName, searchModel.SupplierName, '*');
            else
                whereClause = whereClause.Where(x => string.IsNullOrEmpty(searchModel.SupplierName) || x.Visit.Supplier.SupplierName == searchModel.SupplierName);

            // Contract Number
            if (searchModel.ContractNumber.HasEvoWildCardChar())
                whereClause = whereClause.WhereLike(x => x.Visit.Assignment.Project.Contract.ContractNumber, searchModel.ContractNumber, '*');
            else
                whereClause = whereClause.Where(x => string.IsNullOrEmpty(searchModel.ContractNumber) || x.Visit.Assignment.Project.Contract.ContractNumber == searchModel.ContractNumber);
            
            // customer code
            if (searchModel.CustomerCode.HasEvoWildCardChar())
                whereClause = whereClause.WhereLike(x => x.Visit.Assignment.Project.Contract.Customer.Code, searchModel.CustomerCode, '*');
            else
                whereClause = whereClause.Where(x => string.IsNullOrEmpty(searchModel.CustomerCode) || x.Visit.Assignment.Project.Contract.Customer.Code == searchModel.CustomerCode);
          
            // Customer Name
            if (searchModel.CustomerName.HasEvoWildCardChar())
                whereClause = whereClause.WhereLike(x => x.Visit.Assignment.Project.Contract.Customer.Name, searchModel.CustomerName, '*');
            else
                whereClause = whereClause.Where(x => string.IsNullOrEmpty(searchModel.CustomerName) || x.Visit.Assignment.Project.Contract.Customer.Name == searchModel.CustomerName);
          
            // Supplier Po Number
            if (searchModel.SupplierPoNumber.HasEvoWildCardChar())
                whereClause = whereClause.Where(x => x.Visit.Supplier.SupplierPurchaseOrder.Any(x1 => x1.SupplierPonumber.Contains(searchModel.SupplierPoNumber)));
            else
                whereClause = whereClause.Where(x => string.IsNullOrEmpty(searchModel.SupplierPoNumber)|| x.Visit.Supplier.SupplierPurchaseOrder.Any(x1 => x1.SupplierPonumber == searchModel.SupplierPoNumber));

            // NCR status
            if (searchModel.Ncr == NcrStatus.Open)
                whereClause = whereClause.Where(x => x.NcrcloseOutDate == null);
            if (searchModel.Ncr == NcrStatus.Closed)
                whereClause = whereClause.Where(x => x.NcrcloseOutDate != null);

            // Project number
            whereClause = whereClause.Where(x => searchModel.ProjectNumber == null || x.Visit.Assignment.Project.ProjectNumber == searchModel.ProjectNumber);

            // Supplier Id 
            whereClause = whereClause.Where(x => searchModel.supplierId == null || x.Visit.SupplierId == searchModel.supplierId);

            // Assignment number
            whereClause = whereClause.Where(x => searchModel.AssignmentNumber == null || x.Visit.Assignment.AssignmentNumber == searchModel.AssignmentNumber);

            return whereClause;
        }
    }
}
