using AutoMapper.QueryableExtensions;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.GenericDbRepository.Services;
using Evolution.Visit.Domain.Enums;
using Evolution.Visit.Domain.Interfaces.Data;
using Evolution.Visit.Domain.Models.Visits;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.Visit.Domain.Models.Visits;
using AutoMapper;
using Evolution.Logging.Interfaces;

namespace Evolution.Visit.Infrastructure.Data
{
    public class VisitInterCompanyDiscountsRepository : GenericRepository<DbModel.VisitInterCompanyDiscount>, IVisitInterCompanyDiscountsRepository
    {
        private readonly DbModel.EvolutionSqlDbContext _dbContext = null;
        private readonly IMapper _mapper = null;
        private readonly IAppLogger<VisitInterCompanyDiscountsRepository> _logger = null;

        public VisitInterCompanyDiscountsRepository(DbModel.EvolutionSqlDbContext dbContext, IMapper mapper, IAppLogger<VisitInterCompanyDiscountsRepository> logger) : base(dbContext)
        {
            this._dbContext = dbContext;
            this._mapper = mapper;
            this._logger = logger;
        }

        public DomainModel.VisitInterCoDiscountInfo Search(long visitId)
        {
            DomainModel.VisitInterCoDiscountInfo result = null;
            if (visitId > 0)
            {
                var dbInterCompanyDiscounts = _dbContext.VisitInterCompanyDiscount.Where(x => x.VisitId == visitId).ToList();

                if (dbInterCompanyDiscounts != null && dbInterCompanyDiscounts.Count > 0)
                {
                    result = _mapper.Map<DomainModel.VisitInterCoDiscountInfo>(dbInterCompanyDiscounts);
                    result.VisitId = visitId;
                    result.AssignmentOperatingCompanyCode = dbInterCompanyDiscounts?.FirstOrDefault().Visit.Assignment.OperatingCompany?.Code;
                    result.AssignmentOperatingCompanyName = dbInterCompanyDiscounts?.FirstOrDefault().Visit.Assignment.OperatingCompany?.Name;
                    result.AssignmentOperatingCompanyDiscount = CalculateOperatingCompanyDiscount(result);
                }
            }

            return result;
        }

        private decimal? CalculateOperatingCompanyDiscount(DomainModel.VisitInterCoDiscountInfo result)
        {
            var intercompayDiscount = result;
            if (intercompayDiscount.AssignmentAdditionalIntercompany1_Discount == null)
                intercompayDiscount.AssignmentAdditionalIntercompany1_Discount = 0;

            if (intercompayDiscount.AssignmentAdditionalIntercompany2_Discount == null)
                intercompayDiscount.AssignmentAdditionalIntercompany2_Discount = 0;

            if (intercompayDiscount.AssignmentContractHoldingCompanyDiscount == null)
                intercompayDiscount.AssignmentContractHoldingCompanyDiscount = 0;

            if (intercompayDiscount.ParentContractHoldingCompanyDiscount == null)
                intercompayDiscount.ParentContractHoldingCompanyDiscount = 0;

            if (intercompayDiscount.AssignmentHostcompanyDiscount == null)
                intercompayDiscount.AssignmentHostcompanyDiscount = 0;

            return 100 - (intercompayDiscount.AssignmentAdditionalIntercompany1_Discount +
                         intercompayDiscount.AssignmentAdditionalIntercompany2_Discount +
                         intercompayDiscount.AssignmentContractHoldingCompanyDiscount +
                         intercompayDiscount.ParentContractHoldingCompanyDiscount +
                         intercompayDiscount.AssignmentHostcompanyDiscount
                );

        }

        public IList<DbModel.VisitInterCompanyDiscount> GetVisitInterCompanyDiscounts(long visitId,
                                                                        ValidationType validationType)
        {
            IList<DbModel.VisitInterCompanyDiscount> dbVisitInterCompanyDiscounts = null;
            if (validationType != ValidationType.Add)
            {
                dbVisitInterCompanyDiscounts = _dbContext.VisitInterCompanyDiscount.Where(x => x.VisitId == visitId).ToList();
            }
            return dbVisitInterCompanyDiscounts;
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }

        public DomainModel.VisitInterCoDiscountInfo UpdateVisitIntercompanyDiscount(DbModel.VisitInterCompanyDiscount visitintercompanydisco)
        {
            DomainModel.VisitInterCoDiscountInfo result = null;
            if (visitintercompanydisco.VisitId > 0)
            {
                DbModel.VisitInterCompanyDiscount dbInterCompanyDiscounts = _dbContext.VisitInterCompanyDiscount.FirstOrDefault(x => x.VisitId == visitintercompanydisco.VisitId);

                if (dbInterCompanyDiscounts != null)
                {
                    //  result = _mapper.Map<DomainModel.VisitInterCoDiscountInfo>(dbInterCompanyDiscounts);
                    dbInterCompanyDiscounts.Description = visitintercompanydisco.Description;
                    dbInterCompanyDiscounts.Percentage = visitintercompanydisco.Percentage;
                   /* result.AssignmentOperatingCompanyCode = dbInterCompanyDiscounts?.FirstOrDefault().Visit.Assignment.OperatingCompany?.Code;
                    result.AssignmentOperatingCompanyName = dbInterCompanyDiscounts?.FirstOrDefault().Visit.Assignment.OperatingCompany?.Name;
              */      int saved = _dbContext.SaveChanges();
                }
            }
         return result;
        }
    }
}
