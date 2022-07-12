using AutoMapper;
using AutoMapper.QueryableExtensions;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Common.Helpers;
using Evolution.Contract.Domain.Interfaces.Data;
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
    public class ContractInvoiceAttachmentRepository : GenericRepository<DbModel.ContractInvoiceAttachment>, IContractInvoiceAttachmentRepository
    {
        private IMapper _mapper = null;
        private EvolutionSqlDbContext _dbContext = null;
        private readonly IAppLogger<ContractInvoiceAttachmentRepository> _logger = null;

        public ContractInvoiceAttachmentRepository(IMapper mapper,
            EvolutionSqlDbContext dbContext,
            IAppLogger<ContractInvoiceAttachmentRepository> logger) : base(dbContext)
        {
            _mapper = mapper;
            _dbContext = dbContext;
            _logger = logger;
        }

        public IList<DomainModel.ContractInvoiceAttachment> Search(DomainModel.ContractInvoiceAttachment searchModel)
        {
            var dbSearchModel = _mapper.Map<DbModel.ContractInvoiceAttachment>(searchModel);
            var expression = dbSearchModel.ToExpression();

            IQueryable<DbModel.ContractInvoiceAttachment> whereClause = null;
            var contractInvoiceAttachment = _dbContext.ContractInvoiceAttachment;

            if (searchModel.ContractNumber.HasEvoWildCardChar())
                whereClause = contractInvoiceAttachment.WhereLike(x => x.Contract.ContractNumber, searchModel.ContractNumber, '*');
            else
                whereClause = contractInvoiceAttachment.Where(x => string.IsNullOrEmpty(searchModel.ContractNumber) || x.Contract.ContractNumber == searchModel.ContractNumber);

            if (searchModel.DocumentType.HasEvoWildCardChar())
                whereClause = whereClause.WhereLike(x => x.DocumentType.Name, searchModel.DocumentType, '*');
            else
                whereClause = whereClause.Where(x => string.IsNullOrEmpty(searchModel.DocumentType) || x.DocumentType.Name == searchModel.DocumentType);

            if (expression != null)
                return whereClause.Where(expression).ProjectTo<DomainModel.ContractInvoiceAttachment>().ToList();
            return whereClause.ProjectTo<DomainModel.ContractInvoiceAttachment>().ToList();
        }

        public int DeleteInvoiceAttachment(List<DomainModel.ContractInvoiceAttachment> contractInvoiceAttachments)
        {
            var attachmentIds = contractInvoiceAttachments?.Where(x => x.ContractInvoiceAttachmentId != null && x.ContractInvoiceAttachmentId > 0)?.Select(x => x.ContractInvoiceAttachmentId.Value)?.Distinct().ToList();
            return DeleteInvoiceAttachment(attachmentIds);
        }

        public int DeleteInvoiceAttachment(List<DbModel.ContractInvoiceAttachment> contractInvoiceAttachments)
        {
            var attachmentIds = contractInvoiceAttachments?.Select(x => x.Id)?.Distinct().ToList();
            return DeleteInvoiceAttachment(attachmentIds);
        }

        public int DeleteInvoiceAttachment(List<int> attachmentIds)
        {
            int count = 0;
            try
            {
                if (attachmentIds.Count > 0)
                {
                    var deleteStatement = string.Format(Utility.GetSqlQuery(SQLModuleType.Contract_InvoiceAttachment, SQLModuleActionType.Delete), string.Join(",", attachmentIds.ToString<int>()));
                    count = _dbContext.Database.ExecuteSqlCommand(deleteStatement);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), "AttachmentIds=" + attachmentIds.ToString<int>());
            }

            return count;
        }
    }
}
