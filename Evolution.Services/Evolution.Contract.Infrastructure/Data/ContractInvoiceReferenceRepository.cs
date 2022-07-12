using AutoMapper;
using AutoMapper.QueryableExtensions;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Common.Helpers;
using Evolution.Contract.Domain.Interfaces.Data;
using Evolution.Contract.Domain.Models.Contracts;
using Evolution.DbRepository.Models.SqlDatabaseContext;
using Evolution.GenericDbRepository.Services;
using Evolution.Logging.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.Contract.Domain.Models.Contracts;

namespace Evolution.Contract.Infrastructure.Data
{
    public class ContractInvoiceReferenceRepository: GenericRepository<DbModel.ContractInvoiceReference>, IContractInvoiceReferenceTypeRepository
    {

        private EvolutionSqlDbContext _dbContext = null;
        private IMapper _mapper = null;
        private readonly IAppLogger<ContractInvoiceReferenceRepository> _logger = null;

        public ContractInvoiceReferenceRepository(IMapper mapper,EvolutionSqlDbContext dbContext,
            IAppLogger<ContractInvoiceReferenceRepository> logger
            ) : base(dbContext)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
        }

        public IList<ContractInvoiceReferenceType> Search(ContractInvoiceReferenceType searchModel)
        {
            var dbSearchModel = _mapper.Map<DomainModel.ContractInvoiceReferenceType, DbModel.ContractInvoiceReference>(searchModel);
            var expression = dbSearchModel.ToExpression();
            var contractInvoiceReferences = _dbContext.ContractInvoiceReference;
            IQueryable<ContractInvoiceReference> whereClause = null;

            if (searchModel.ContractNumber.HasEvoWildCardChar())
                whereClause = contractInvoiceReferences.WhereLike(x => x.Contract.ContractNumber, searchModel.ContractNumber, '*');
            else
                whereClause = contractInvoiceReferences.Where(x => string.IsNullOrEmpty(searchModel.ContractNumber) || x.Contract.ContractNumber == searchModel.ContractNumber);

            if (searchModel.ReferenceType.HasEvoWildCardChar())
                whereClause = whereClause.WhereLike(x => x.AssignmentReferenceType.Name, searchModel.ReferenceType, '*');
            else
                whereClause = whereClause.Where(x => string.IsNullOrEmpty(searchModel.ReferenceType) || x.AssignmentReferenceType.Name == searchModel.ReferenceType);

            if (expression != null)
                return whereClause.Where(expression).ProjectTo<DomainModel.ContractInvoiceReferenceType>().ToList();
            return whereClause.ProjectTo<DomainModel.ContractInvoiceReferenceType>().ToList();

        }

        public int DeleteInvoiceReference(List<DomainModel.ContractInvoiceReferenceType>  contractInvoiceReferenceTypes)
        {
            var referenceTypeIds = contractInvoiceReferenceTypes?.Where(x => x.ContractInvoiceReferenceTypeId !=null && x.ContractInvoiceReferenceTypeId > 0)?.Select(x => x.ContractInvoiceReferenceTypeId.Value)?.Distinct().ToList();
            return DeleteInvoiceReference(referenceTypeIds);
        }

        public int DeleteInvoiceReference(List<DbModel.ContractInvoiceReference>  contractInvoiceReferences)
        {
            var referenceTypeIds = contractInvoiceReferences?.Select(x => x.Id)?.Distinct().ToList();
            return DeleteInvoiceReference(referenceTypeIds);
        }

        public int DeleteInvoiceReference(List<int> referenceTypeIds)
        {
            int count = 0;
            try
            {
                if (referenceTypeIds.Count > 0)
                {
                    var deleteStatement = string.Format(Utility.GetSqlQuery(SQLModuleType.Contract_InvoiceReference, SQLModuleActionType.Delete), string.Join(",", referenceTypeIds.ToString<int>()));
                    count = _dbContext.Database.ExecuteSqlCommand(deleteStatement);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), "ReferenceTypeIds=" + referenceTypeIds.ToString<int>());
            }

            return count;
        }
    }
}
