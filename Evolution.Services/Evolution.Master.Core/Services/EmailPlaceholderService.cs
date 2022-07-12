using AutoMapper;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Common.Models.Responses;
using Evolution.Logging.Interfaces;
using Evolution.Master.Domain.Interfaces.Data;
using Evolution.Master.Domain.Interfaces.Services;
using Evolution.Master.Domain.Models;
using System;
using System.Collections.Generic;

namespace Evolution.Master.Core
{
    public class EmailPlaceholderService : IEmailPlaceholderService
    {
        private readonly IAppLogger<EmailPlaceholderService> _logger = null;
        private readonly IEmailPlaceholderRepository _repository = null;

        public EmailPlaceholderService(IAppLogger<EmailPlaceholderService> logger,IEmailPlaceholderRepository repository) 
        {
            this._logger = logger;
            this._repository = repository;
        }

        public Response Search(EmailPlaceholder searchModel)
        {
            IList<EmailPlaceholder> result = null;
            Exception exception = null;
            try
            {
                result = this._repository.Search(searchModel);
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), searchModel);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception, result?.Count);
        }
    }
}