using AutoMapper;
using Evolution.AuditLog.Domain.Interfaces.Audit;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Common.Models.Messages;
using Evolution.Common.Models.Responses;
using Evolution.Logging.Interfaces;
using Evolution.Visit.Domain.Interfaces.Data;
using Evolution.Visit.Domain.Interfaces.Validations;
using Evolution.Visit.Domain.Interfaces.Visits;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using DomainModel = Evolution.Visit.Domain.Models.Visits;

namespace Evolution.Visit.Core.Services
{
    public class VisitInterCompanyDiscountsService : IVisitInterCompanyService
    {
        private readonly IMapper _mapper = null;
        private IAppLogger<VisitInterCompanyDiscountsService> _logger = null;
        private IVisitInterCompanyDiscountsRepository _repository = null;
        private readonly JObject _messageDescriptions = null;
        private readonly IAuditLogger _auditLogger = null;

        public VisitInterCompanyDiscountsService(IMapper mapper, IAppLogger<VisitInterCompanyDiscountsService> logger, 
                                                 IVisitInterCompanyDiscountsRepository repository, JObject messages,IAuditLogger auditLogger)
        {
            this._mapper = mapper;
            this._logger = logger;
            this._repository = repository;
            this._messageDescriptions = messages;
            this._auditLogger = auditLogger;
        }

        #region Public Methods

        public Response GetVisitInterCompany(long visitId)
        {
            DomainModel.VisitInterCoDiscountInfo result = null;
            Exception exception = null;
            try
            {
                result = this._repository.Search(visitId);
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString());
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception, null);
        }

        public Response SaveVisitInterCompany(IList<DomainModel.VisitInterCompanyDiscounts> visitInterCompanyDiscounts, bool commitChanges = true)
        {
            Exception exception = null;
            List<MessageDetail> errorMessages = null;
            List<ValidationMessage> validationMessages = null;
            try
            {
                _repository.AutoSave = false;
                errorMessages = new List<MessageDetail>();

            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString());
            }

            return new Response().ToPopulate(ResponseType.Success, null, errorMessages, validationMessages, null, exception);
        }

        public Response ModifyVisitInterCompany(IList<DomainModel.VisitInterCompanyDiscounts> visitInterCompanyDiscounts, bool commitChanges = true)
        {
            Exception exception = null;
            Response response = null;
            try
            {

            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString());
            }

            return response;
        }

        public Response DeleteVisitInterCompany(IList<DomainModel.VisitInterCompanyDiscounts> visitInterCompanyDiscounts, bool commitChanges = true)
        {
            Exception exception = null;
            Response response = null;
            try
            {

            }
            catch(Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString());
            }

            return response;
        }        

        #endregion
    }
}
