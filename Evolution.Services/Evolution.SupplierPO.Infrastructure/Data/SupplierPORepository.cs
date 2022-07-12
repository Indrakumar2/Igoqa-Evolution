using AutoMapper;
using AutoMapper.QueryableExtensions;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Common.Helpers;
using Evolution.GenericDbRepository.Services;
using Evolution.Logging.Interfaces;
using Evolution.SupplierPO.Domain.Interfaces.Data;
using Evolution.SupplierPO.Domain.Models.SupplierPO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModels = Evolution.SupplierPO.Domain.Models.SupplierPO;

namespace Evolution.SupplierPO.Infrastructure.Data
{
    public class SupplierPORepository : GenericRepository<DbModel.SupplierPurchaseOrder>, ISupplierPORepository
    {
        private readonly DbModel.EvolutionSqlDbContext _dbContext = null;
        private readonly IMapper _mapper = null;
        private readonly IAppLogger<SupplierPORepository> _logger = null;

        public SupplierPORepository(DbModel.EvolutionSqlDbContext dbcontext, IMapper mapper, IAppLogger<SupplierPORepository> logger) : base(dbcontext)
        {
            _dbContext = dbcontext;
            _mapper = mapper;
            _logger = logger;
        }

        public List<DomainModels.SupplierPO> Search(DomainModels.SupplierPOSearch searchModel)
        {
            var dbSearchModel = _mapper.Map<DbModel.SupplierPurchaseOrder>(searchModel);
            IQueryable<DbModel.SupplierPurchaseOrder> whereClause = Filter(searchModel);

            var expression = dbSearchModel.ToExpression();
            if (expression == null)
                return whereClause.ProjectTo<DomainModels.SupplierPO>().ToList();
            else
                return whereClause.Where(expression).ProjectTo<DomainModels.SupplierPO>().ToList();
        }

        public List<T> Search<T>(DomainModels.SupplierPOSearch searchModel)
        {
            var dbSearchModel = _mapper.Map<DbModel.SupplierPurchaseOrder>(searchModel);
            IQueryable<DbModel.SupplierPurchaseOrder> whereClause = Filter(searchModel);

            var expression = dbSearchModel.ToExpression();
            if (expression == null)
                return whereClause.ProjectTo<T>().ToList();
            else
                return whereClause.Where(expression).ProjectTo<T>().ToList();
        }

        /*Added for Assignment Dropdown Values clean up*/
        public List<DomainModels.SupplierPO> SearchSupplierPO(DomainModels.SupplierPOSearch searchModel)
        {
            var dbSearchModel = _mapper.Map<DbModel.SupplierPurchaseOrder>(searchModel);
            List<DomainModels.SupplierPO> whereClause = Filter(searchModel)
                            ?.Select(x => new DomainModels.SupplierPO
                            {
                                Id = x.Id,
                                SupplierPOId = x.Id,
                                SupplierPONumber = x.SupplierPonumber,
                                SupplierPOMainSupplierId = x.SupplierId,
                                SupplierPOMainSupplierName = x.Supplier.SupplierName,
                                ZipCode = x.Supplier.PostalCode,
                                Country = x.Supplier.Country.Name,
                                State = x.Supplier.County.Name,
                                City = x.Supplier.City.Name,
                                SupplierPOMaterialDescription = x.MaterialDescription,
                                LastModification = x.LastModification,
                                ModifiedBy = x.ModifiedBy,
                                UpdateCount = x.UpdateCount,
                            })?.ToList();


            return whereClause;
        }

        private List<HeaderList> GetHeaderList()
        {
            List<HeaderList> headerData = new List<HeaderList>
            {
                new HeaderList { Label = "Supplier PO", Key = "supplierPONumber" },
                new HeaderList { Label = "Customer", Key = "supplierPOCustomerName" },
                new HeaderList { Label = "Contract No", Key = "supplierPOContractNumber" },
                new HeaderList { Label = "Project No", Key = "supplierPOCustomerProjectNumber" },
                 new HeaderList { Label = "Csutomer Project Name", Key = "supplierPOCustomerProjectName" },
                new HeaderList { Label = "Main Supplier", Key = "supplierPOMainSupplierName" },
                new HeaderList { Label = "Sub Supplier", Key = "supplierPOSubSupplierName" },
                new HeaderList { Label = "Material Description", Key = "supplierPOMaterialDescription" },
                new HeaderList { Label = "Delivery Date", Key = "supplierPODeliveryDate" },
                new HeaderList { Label = "Status", Key = "supplierPOStatus" },
                new HeaderList { Label = "Completed Date", Key = "supplierPOCompletedDate" }
            };

            return headerData;
        }

        //Added extra to check Search functionality
        public Result SearchSupplierPO<T>(DomainModels.SupplierPOSearch searchModel)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            Result result = new Result();
            _dbContext.ChangeTracker.AutoDetectChangesEnabled = false;
            try
            {
                #region Supplier PO
                IQueryable<DbModel.SupplierPurchaseOrder> whereClauseSupplierPO = _dbContext.SupplierPurchaseOrder.AsNoTracking();
                if (searchModel.SupplierPOProjectNumber > 0)
                    whereClauseSupplierPO = whereClauseSupplierPO.Where(x => x.ProjectId == searchModel.SupplierPOProjectNumber);
                //if(searchModel.SupplierPOCompanyCode != null)
                //{
                //    whereClauseSupplierPO = whereClauseSupplierPO.Where(x => x.Project.Contract.ContractHolderCompany.Code == searchModel.SupplierPOCompanyCode);
                //}
                if (searchModel.SupplierPOCompanyId > 0)
                    whereClauseSupplierPO = whereClauseSupplierPO.Where(x => x.Project.Contract.ContractHolderCompanyId == searchModel.SupplierPOCompanyId);

                if (searchModel.ContractCompanyId > 0 && searchModel.SupplierPOCompanyId == 0)
                    whereClauseSupplierPO = whereClauseSupplierPO.Where(x => x.Project.Contract.ContractHolderCompanyId == searchModel.ContractCompanyId);

                if (searchModel.SupplierPOIds?.Count > 0)
                {
                    var supplierPOIDs = searchModel.SupplierPOIds.Select(x => x).ToList();
                    whereClauseSupplierPO = whereClauseSupplierPO.Where(x => supplierPOIDs.Contains(x.Id));
                }
                if (searchModel.SupplierPOMaterialDescription.HasEvoWildCardChar())
                    whereClauseSupplierPO = whereClauseSupplierPO.WhereLike(x => x.MaterialDescription, searchModel.SupplierPOMaterialDescription, '*');
                else if (!string.IsNullOrEmpty(searchModel.SupplierPOMaterialDescription))
                    whereClauseSupplierPO = whereClauseSupplierPO.Where(x => x.MaterialDescription.Equals(searchModel.SupplierPOMaterialDescription));

                if (searchModel.SupplierPONumber.HasEvoWildCardChar())
                    whereClauseSupplierPO = whereClauseSupplierPO.WhereLike(x => x.SupplierPonumber, searchModel.SupplierPONumber, '*');
                else if (!string.IsNullOrEmpty(searchModel.SupplierPONumber))
                    whereClauseSupplierPO = whereClauseSupplierPO.Where(x => x.SupplierPonumber.Equals(searchModel.SupplierPONumber));

                if (searchModel.SupplierPODeliveryDate != null)
                    whereClauseSupplierPO = whereClauseSupplierPO.Where(x => x.DeliveryDate == searchModel.SupplierPODeliveryDate);

                if (searchModel.SupplierPOCompletedDate != null)
                    whereClauseSupplierPO = whereClauseSupplierPO.Where(x => x.CompletionDate == searchModel.SupplierPOCompletedDate);

                if (searchModel.SupplierPOStatus != null)
                    whereClauseSupplierPO = whereClauseSupplierPO.Where(x => x.Status == searchModel.SupplierPOStatus);

                if (searchModel.SupplierPOMainSupplierId > 0)
                    whereClauseSupplierPO = whereClauseSupplierPO.Where(x => x.SupplierId == searchModel.SupplierPOMainSupplierId);

                if (searchModel.SupplierPOSubSupplierId > 0)
                    whereClauseSupplierPO = whereClauseSupplierPO.Where(x => x.SupplierPurchaseOrderSubSupplier.Any(ss => ss.SupplierId == searchModel.SupplierPOSubSupplierId));

                #endregion

                #region Project
                IQueryable<DbModel.Project> whereClauseProject = _dbContext.Project.AsNoTracking();
                if (searchModel.SupplierPOCustomerProjectName.HasEvoWildCardChar())
                    whereClauseProject = whereClauseProject.WhereLike(x => x.CustomerProjectName, searchModel.SupplierPOCustomerProjectName, '*');
                else if (!string.IsNullOrEmpty(searchModel.SupplierPOCustomerProjectName))
                    whereClauseProject = whereClauseProject.Where(x => x.CustomerProjectName == searchModel.SupplierPOCustomerProjectName);
                #endregion

                #region Contract
                IQueryable<DbModel.Contract> whereClauseContract = _dbContext.Contract.AsNoTracking();
                if (searchModel.SupplierPOContractNumber.HasEvoWildCardChar())
                    whereClauseContract = whereClauseContract.WhereLike(x => x.ContractNumber, searchModel.SupplierPOContractNumber, '*');
                else if (!string.IsNullOrEmpty(searchModel.SupplierPOContractNumber))
                    whereClauseContract = whereClauseContract.Where(x => x.ContractNumber == searchModel.SupplierPOContractNumber);

                if (searchModel.SupplierPOCustomerId > 0)
                    whereClauseContract = whereClauseContract.Where(x => x.CustomerId == searchModel.SupplierPOCustomerId);
                else
                {
                    if (searchModel.SupplierPOCustomerName.HasEvoWildCardChar())
                        whereClauseContract = whereClauseContract.WhereLike(x => x.Customer.Name, searchModel.SupplierPOCustomerName, '*');
                    else if (!string.IsNullOrEmpty(searchModel.SupplierPOCustomerName))
                        whereClauseContract = whereClauseContract.Where(x => x.Customer.Name == searchModel.SupplierPOCustomerName);
                }


                #endregion

                whereClauseProject = whereClauseProject.Where(x => whereClauseContract.Any(x1 => x1.Id == x.ContractId));
                whereClauseSupplierPO = whereClauseSupplierPO?.Where(x => whereClauseProject.Any(x1 => x1.Id == x.ProjectId));

                if (whereClauseSupplierPO != null && searchModel.TotalCount <= 0)
                    searchModel.TotalCount = whereClauseSupplierPO.AsNoTracking().Count();

                if (searchModel.TotalCount > 0)
                {
                    IList<BaseSupplierPO> supplierPOSearches = new List<BaseSupplierPO>();
                    if (searchModel.IsExport == true)
                    {
                        for (int i = 0; i <= searchModel.TotalCount; i += searchModel.FetchCount)
                        {
                            var dbData = whereClauseSupplierPO.AsNoTracking().OrderBy(x => searchModel.OrderBy).Skip(i).Take(searchModel.FetchCount);
                            var domData = MapData(dbData, searchModel.TotalCount);
                            supplierPOSearches.AddRange(domData);
                        }
                    }
                    else
                    {
                        if (searchModel.ModuleName != "SPO")
                            whereClauseSupplierPO = whereClauseSupplierPO.AsNoTracking()?.OrderBy(x => searchModel.OrderBy).Skip(searchModel.OffSet).Take(searchModel.FetchCount);
                        var domData = MapData(whereClauseSupplierPO, searchModel.TotalCount);
                        supplierPOSearches.AddRange(domData);
                    }

                    if (supplierPOSearches?.Any() == true)
                    {
                        result.Header = GetHeaderList();
                        result.BaseSupplierPO = supplierPOSearches;
                    }
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                _logger.LogError(responseType.ToId(), ex.ToFullString(), searchModel);
            }
            finally
            {
                _dbContext.ChangeTracker.AutoDetectChangesEnabled = true;
            }
            return result;
        }

        private List<DomainModels.BaseSupplierPO> MapData(IQueryable<DbModel.SupplierPurchaseOrder> dbData, int totalCount)
        {
            string delimiter = ",";
            var domData = dbData?.Select(x => new DomainModels.BaseSupplierPO
            {
                SupplierPOId = x.Id,
                SupplierPONumber = x.SupplierPonumber,
                SupplierPOCustomerName = x.Project.Contract.Customer.Name,
                SupplierPOContractNumber = x.Project.Contract.ContractNumber,
                SupplierPOProjectNumber = (int)x.Project.ProjectNumber,
                SupplierPOCustomerProjectNumber = x.Project.CustomerProjectNumber,
                SupplierPOCustomerProjectName = x.Project.CustomerProjectName,
                SupplierPOMainSupplierName = x.Supplier.SupplierName,
                //SupplierPOSubSupplierName = x.SupplierPurchaseOrderSubSupplier.HasItems()
                //                            ? string.Join(delimiter, x.SupplierPurchaseOrderSubSupplier.Select(x1 => x1.Supplier.SupplierName).ToList())
                //                            : string.Empty,
                SupplierPOSubSupplierName = string.Join(delimiter, x.SupplierPurchaseOrderSubSupplier.Select(x1 => x1.Supplier.SupplierName).ToList()),
                SupplierPOMaterialDescription = x.MaterialDescription,
                SupplierPODeliveryDate = x.DeliveryDate,
                SupplierPOStatus = x.Status,
                SupplierPOCompletedDate = x.CompletionDate,
                SupplierPOCompanyCode = x.Project.Contract.ContractHolderCompany.Code,
                SupplierPOCompanyName = x.Project.Contract.ContractHolderCompany.Name,
                SupplierPOCompanyId = x.Project.Contract.ContractHolderCompanyId,
                TotalCount = totalCount
            })?.ToList();

            return domData;
        }

        //Called from Edit Supplier PO- scrapped- Also getting called from Assignment
        private IQueryable<DbModel.SupplierPurchaseOrder> Filter(DomainModels.SupplierPOSearch searchModel)
        {
            IQueryable<DbModel.SupplierPurchaseOrder> whereClause = _dbContext.SupplierPurchaseOrder; ;

            if (searchModel.SupplierPOCustomerCode.HasEvoWildCardChar())
                whereClause = whereClause.WhereLike(x => x.Project.Contract.Customer.Code, searchModel.SupplierPOCustomerCode, '*');
            else
                whereClause = whereClause.Where(x => string.IsNullOrEmpty(searchModel.SupplierPOCustomerCode) || x.Project.Contract.Customer.Code.Equals(searchModel.SupplierPOCustomerCode));

            if (searchModel.SupplierPOIds?.Count > 0)
                whereClause = whereClause.Where(x => searchModel.SupplierPOIds.Contains(x.Id));

            // Supplier CustomerName Wild Card Char check
            if (searchModel.SupplierPOCustomerName.HasEvoWildCardChar())
                whereClause = whereClause.WhereLike(x => x.Project.Contract.Customer.Name, searchModel.SupplierPOCustomerName, '*');
            else
                whereClause = whereClause.Where(x => string.IsNullOrEmpty(searchModel.SupplierPOCustomerName) || x.Project.Contract.Customer.Name.Equals(searchModel.SupplierPOCustomerName));

            // Supplier PO Contract Number Wild Card Char Check
            if (searchModel.SupplierPOContractNumber.HasEvoWildCardChar())
                whereClause = whereClause.WhereLike(x => x.Project.Contract.ContractNumber, searchModel.SupplierPOContractNumber, '*');
            else
                whereClause = whereClause.Where(x => string.IsNullOrEmpty(searchModel.SupplierPOContractNumber) || x.Project.Contract.ContractNumber.Equals(searchModel.SupplierPOContractNumber));

            // Supplier PO Main Supplier Wild Card Char Check 
            if (searchModel.SupplierPOMainSupplierName.HasEvoWildCardChar())
                whereClause = whereClause.WhereLike(x => x.Supplier.SupplierName, searchModel.SupplierPOMainSupplierName, '*');
            else
                whereClause = whereClause.Where(x => string.IsNullOrEmpty(searchModel.SupplierPOMainSupplierName) || x.Supplier.SupplierName.Equals(searchModel.SupplierPOMainSupplierName));

            // SupplIer PO Project name Wild Card Char check
            if (searchModel.SupplierPOCustomerProjectName.HasEvoWildCardChar())
                whereClause = whereClause.WhereLike(x => x.Project.CustomerProjectName, searchModel.SupplierPOCustomerProjectName, '*');
            else
                whereClause = whereClause.Where(x => string.IsNullOrEmpty(searchModel.SupplierPOCustomerProjectName) || x.Project.CustomerProjectName.Equals(searchModel.SupplierPOCustomerProjectName));

            // Supplier PO Customer Project Number Wild Card Char Check
            if (!string.IsNullOrEmpty(searchModel.SupplierPOCustomerProjectNumber))
                whereClause = whereClause.Where(x => x.Project.CustomerProjectNumber == searchModel.SupplierPOCustomerProjectNumber);

            // Supplier PO Number Wild Card Char Check
            if (searchModel.SupplierPONumber.HasEvoWildCardChar())
                whereClause = whereClause.WhereLike(x => x.SupplierPonumber, searchModel.SupplierPONumber, '*');
            else
                whereClause = whereClause.Where(x => string.IsNullOrEmpty(searchModel.SupplierPONumber) || x.SupplierPonumber.Equals(searchModel.SupplierPONumber));

            if (searchModel.SupplierPOCompanyCode.HasEvoWildCardChar())
                whereClause = whereClause.WhereLike(x => x.Project.Contract.ContractHolderCompany.Code, searchModel.SupplierPOCompanyCode, '*');
            else
                whereClause = whereClause.Where(x => string.IsNullOrEmpty(searchModel.SupplierPOCompanyCode) || x.Project.Contract.ContractHolderCompany.Code.Equals(searchModel.SupplierPOCompanyCode));

            if (searchModel.SupplierPOCompanyName.HasEvoWildCardChar())
                whereClause = whereClause.WhereLike(x => x.Project.Contract.ContractHolderCompany.Name, searchModel.SupplierPOCompanyName, '*');
            else
                whereClause = whereClause.Where(x => string.IsNullOrEmpty(searchModel.SupplierPOCompanyName) || x.Project.Contract.ContractHolderCompany.Name.Equals(searchModel.SupplierPOCompanyName));

            if (!string.IsNullOrEmpty(searchModel.SupplierPOSubSupplierName))
                whereClause = whereClause.Where(x => x.SupplierPurchaseOrderSubSupplier.Any(x1 => x1.Supplier.SupplierName == searchModel.SupplierPOSubSupplierName));

            if (searchModel.SupplierPOProjectNumber > 0)
                whereClause = whereClause.Where(x => x.ProjectId == searchModel.SupplierPOProjectNumber);

            if (searchModel.SupplierPODeliveryDate != null)
                whereClause = whereClause.Where(x => x.DeliveryDate == searchModel.SupplierPODeliveryDate);

            if (searchModel.SupplierPOCompletedDate != null)
                whereClause = whereClause.Where(x => x.CompletionDate == searchModel.SupplierPOCompletedDate);

            return whereClause;
        }

        public int DeleteSupplierPO(int supplierPOId)
        {
            int count = -1;
            try
            {
                var deleteStatement = Utility.GetSqlQuery(SQLModuleType.SupplierPO_Detail, SQLModuleActionType.Delete);
                count = _dbContext.Database.ExecuteSqlCommand(deleteStatement, supplierPOId);

            }
            catch (Exception ex)
            {
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), "SupplierPoId=" + supplierPOId);
            }

            return count;
        }
    }
}
