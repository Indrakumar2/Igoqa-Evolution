using AutoMapper;
using AutoMapper.QueryableExtensions;
using Evolution.Common.Extensions;
using Evolution.TechnicalSpecialist.Domain.Interfaces.Data;
using Evolution.DbRepository.Models.SqlDatabaseContext;
using Evolution.GenericDbRepository.Services;
using System.Collections.Generic;
using System.Linq;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using  Evolution.TechnicalSpecialist.Domain.Models.TechSpecialist;
using Evolution.Common.Enums;
using Evolution.Document.Domain.Models.Document;
using Microsoft.EntityFrameworkCore;

namespace Evolution.TechnicalSpecialist.Infrastructure.Data
{
    public class TechnicalSpecialistCustomerApprovalRepository : GenericRepository<DbModel.TechnicalSpecialistCustomerApproval>, ITechnicalSpecialistCustomerApprovalRepository
    {

        private readonly DbModel.EvolutionSqlDbContext _dbContext = null;
        private readonly IMapper _mapper = null;

        public TechnicalSpecialistCustomerApprovalRepository(DbModel.EvolutionSqlDbContext dbContext, IMapper mapper) : base(dbContext)
        {
            this._dbContext = dbContext;
            this._mapper = mapper;
        }

        public IList<DbModel.TechnicalSpecialistCustomerApproval> Search(TechnicalSpecialistCustomerApprovalInfo model)
        {
            var tsAsQueryable = this.PopulateTsCustomerApprovalAsQuerable(model);
            return this.Get(tsAsQueryable, null, null, null);
        }

        public IList<DbModel.TechnicalSpecialistCustomerApproval> Get(IList<int> Ids)
        {
            return Get(null, Ids, null, null);
        }

        public IList<DbModel.TechnicalSpecialistCustomerApproval> GetByPinId(IList<string> pins)
        {
            return Get(null, null, pins, null);
        }

        public IList<DbModel.TechnicalSpecialistCustomerApproval> Get(IList<string> CustomerName)
        {
            return Get(null, null, null, CustomerName);
        }

        public IList<DbModel.TechnicalSpecialistCustomerApproval> Get(IList<KeyValuePair<string, string>> ePinAndCodeandStandard)
        {
            var pins = ePinAndCodeandStandard?.Select(x => x.Key).ToList();
            var CodeStandardNames = ePinAndCodeandStandard?.Select(x => x.Value).ToList();

            return Get(null, null, pins, CodeStandardNames);
        }

        private IList<DbModel.TechnicalSpecialistCustomerApproval> Get(IQueryable<DbModel.TechnicalSpecialistCustomerApproval> tsCustomerApprovalAsQuerable = null,
                                                            IList<int> Ids = null,
                                                            IList<string> pins = null,
                                                            IList<string> CustomerName = null)
        {
            if (tsCustomerApprovalAsQuerable == null)
                tsCustomerApprovalAsQuerable = _dbContext.TechnicalSpecialistCustomerApproval;

            if (Ids?.Count > 0)
                tsCustomerApprovalAsQuerable = tsCustomerApprovalAsQuerable.Where(x => Ids.Contains(x.Id));

            if (CustomerName?.Count > 0)
                tsCustomerApprovalAsQuerable = tsCustomerApprovalAsQuerable.Where(x => CustomerName.Contains(x.Customer.Name));

            if (pins?.Count > 0)
                tsCustomerApprovalAsQuerable = tsCustomerApprovalAsQuerable.Where(x => pins.Contains(x.TechnicalSpecialist.Pin.ToString()));

            return tsCustomerApprovalAsQuerable.Include(x => x.TechnicalSpecialist)
                                    .Include(x => x.Customer)
                                    .ToList();
        }

        private IQueryable<DbModel.TechnicalSpecialistCustomerApproval> PopulateTsCustomerApprovalAsQuerable(TechnicalSpecialistCustomerApprovalInfo model)
        {
            var dbSearchModel = this._mapper.Map<DbModel.TechnicalSpecialistCustomerApproval>(model);
            IQueryable<DbModel.TechnicalSpecialistCustomerApproval> tsCustomerApprovalAsQueryable = _dbContext.TechnicalSpecialistCustomerApproval;

            #region Wildcard Search for TS EPin
            if (model.Epin > 0)
                tsCustomerApprovalAsQueryable = tsCustomerApprovalAsQueryable.Where(x => x.TechnicalSpecialist.Pin == model.Epin);
            #endregion

            #region Wildcard Search for Customer Name
            if (model.CustomerName.HasEvoWildCardChar())
                tsCustomerApprovalAsQueryable = tsCustomerApprovalAsQueryable.WhereLike(x => x.Customer.Name.ToString(), model.CustomerName, '*');
            else if (!string.IsNullOrEmpty(model.CustomerName))
                tsCustomerApprovalAsQueryable = tsCustomerApprovalAsQueryable.Where(x => x.Customer.Name.ToString() == model.CustomerName);
            #endregion



            var defWhereExpr = dbSearchModel.ToExpression(new List<string> { nameof(dbSearchModel.CustomerCommodityCodesId) });
            if (defWhereExpr != null)
                tsCustomerApprovalAsQueryable = tsCustomerApprovalAsQueryable.Where(defWhereExpr);

            return tsCustomerApprovalAsQueryable;
        }
    }
}