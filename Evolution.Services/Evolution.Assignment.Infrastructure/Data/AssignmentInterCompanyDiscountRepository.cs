using AutoMapper;
using Evolution.Assignment.Domain.Interfaces.Data;
using Evolution.DbRepository.Models.SqlDatabaseContext;
using Evolution.GenericDbRepository.Services;
using System.Linq;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.Assignment.Domain.Models.Assignments;

namespace Evolution.Assignment.Infrastructure.Data
{
    public class AssignmentInterCompanyDiscountRepository : GenericRepository<DbModel.AssignmentInterCompanyDiscount>, IAssignmentInterCompanyDiscountRepository
    {
        private  EvolutionSqlDbContext _dbContext = null;
        private  IMapper _mapper = null;

        public AssignmentInterCompanyDiscountRepository(EvolutionSqlDbContext dbContext, IMapper mapper) : base(dbContext)
        {
            this._dbContext = dbContext;
            this._mapper = mapper;
        }

        public DomainModel.AssignmentInterCoDiscountInfo Search(int assignmentId)
        {
            DomainModel.AssignmentInterCoDiscountInfo result = null;
            if (assignmentId > 0)
            {
                var dbInterCompanyDiscounts = _dbContext.AssignmentInterCompanyDiscount.Where(x => x.AssignmentId == assignmentId).ToList();
                if (dbInterCompanyDiscounts != null && dbInterCompanyDiscounts.Count>0)
                {
                    result = _mapper.Map<DomainModel.AssignmentInterCoDiscountInfo>(dbInterCompanyDiscounts);
                    result.AssignmentId = assignmentId;
                    result.AssignmentOperatingCompanyCode = dbInterCompanyDiscounts?.FirstOrDefault().Assignment.OperatingCompany?.Code;
                    result.AssignmentOperatingCompanyName = dbInterCompanyDiscounts?.FirstOrDefault().Assignment.OperatingCompany?.Name;
                    result.AssignmentOperatingCompanyDiscount = CalculateOperatingCompanyDiscount(result);
                }
                else
                {
                    result = _mapper.Map<DomainModel.AssignmentInterCoDiscountInfo>(dbInterCompanyDiscounts);
                    result.AssignmentId = assignmentId;
                    result.AssignmentOperatingCompanyDiscount = 100;
                }
            }
          
            return result;
        }

        public DomainModel.AssignmentInterCoDiscountInfo SearchWithCompany(int assignmentId)
        {
            DomainModel.AssignmentInterCoDiscountInfo result = null;
            if (assignmentId > 0)
            {
                //var dbInterCompanyDiscounts = _dbContext.AssignmentInterCompanyDiscount?.Join(_dbContext.Company,
                //                                     dbAssigmtCon => new { dbAssigmtCon.CompanyId},
                //                                     dbCom => new { CompanyId = dbCom.Id },
                //                                    (dbAssigmtCon, dbCom) => new { dbAssigmtCon, dbCom })?
                //                                    .Where(x => x.dbAssigmtCon.AssignmentId == assignmentId)?
                //                                    .Select(x=> new DbModel.AssignmentInterCompanyDiscount
                //                                    {
                //                                        Id=x.dbAssigmtCon.Id,
                //                                        AssignmentId=x.dbAssigmtCon.AssignmentId,
                //                                        CompanyId=x.dbAssigmtCon.CompanyId,
                //                                        Description=x.dbAssigmtCon.Description,
                //                                        DiscountType=x.dbAssigmtCon.DiscountType,
                //                                        LastModification=x.dbAssigmtCon.LastModification,
                //                                        ModifiedBy=x.dbAssigmtCon.ModifiedBy,
                //                                        UpdateCount=x.dbAssigmtCon.UpdateCount,
                //                                        Percentage=x.dbAssigmtCon.Percentage,
                //                                        Company= new DbModel.Company { Id= x.dbCom.Id ,Code =x.dbCom.Code, Name=x.dbCom.Name},
                //                                        Assignment= new DbModel.Assignment { OperatingCompany = new DbModel.Company { Id = x.dbCom.Id, Code = x.dbCom.Code, Name = x.dbCom.Name } }
                //                                    })?.ToList();
                var dbInterCompanyDiscounts = _dbContext.AssignmentInterCompanyDiscount?.Where(x => x.AssignmentId == assignmentId)?.ToList(); //fix for Bug-933
                if (dbInterCompanyDiscounts != null && dbInterCompanyDiscounts.Count > 0)
                {
                    result = _mapper.Map<DomainModel.AssignmentInterCoDiscountInfo>(dbInterCompanyDiscounts);
                    result.AssignmentId = assignmentId;
                    result.AssignmentOperatingCompanyCode = dbInterCompanyDiscounts?.FirstOrDefault().Assignment.OperatingCompany?.Code;
                    result.AssignmentOperatingCompanyName = dbInterCompanyDiscounts?.FirstOrDefault().Assignment.OperatingCompany?.Name;
                    result.AssignmentOperatingCompanyDiscount = CalculateOperatingCompanyDiscount(result);
                }
            }

            return result;
        }

        private decimal? CalculateOperatingCompanyDiscount(DomainModel.AssignmentInterCoDiscountInfo result)
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

        public void Dispose()
        {
            _dbContext.Dispose();
            _mapper = null;
        }
    }
}