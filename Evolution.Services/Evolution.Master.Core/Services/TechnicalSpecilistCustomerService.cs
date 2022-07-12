using Evolution.Master.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Text;
using Evolution.Master.Domain.Models;
using Evolution.Common.Models.Responses;
using Evolution.Logging.Interfaces;
using Evolution.DbRepository.Interfaces.Master;
using AutoMapper;
using System.Linq;
using System.Data.SqlClient;
using Evolution.Common.Extensions;
using Evolution.Common.Enums;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Reflection;
using Evolution.Common.Models.Messages;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using System.Transactions;

namespace Evolution.Master.Core.Services
{
    public class TechnicalSpecilistCustomerService : ITechnicalSpecialistCustomerService
    {
        private readonly IAppLogger<TechnicalSpecilistCustomerService> _logger = null;
        private readonly ITechnicalSpecialistCustomerRepository _repository = null;
        private readonly IMapper _mapper = null;
        private readonly JObject _messages = null;

        public TechnicalSpecilistCustomerService(IMapper mapper, IAppLogger<TechnicalSpecilistCustomerService> logger, ITechnicalSpecialistCustomerRepository repository)
        {
            this._logger = logger;
            this._repository = repository;
            this._mapper = mapper;
            this._messages = JObject.Parse(File.ReadAllText(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"Message_EN.json")));
        }       

        public Response Search(TechnicalSpecialistCustomers search)
        {
            IList<TechnicalSpecialistCustomers> result = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                if (search == null)
                    search = new TechnicalSpecialistCustomers();

                using (var tranScope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))                
                {
                    result = _repository.FindBy(x => (string.IsNullOrEmpty(search.Name) || x.Name == search.Name))                                              
                                        .Select(x => new TechnicalSpecialistCustomers() { Name = x.Name,Code=x.Code }).OrderBy(x => x.Name).ToList();
                    tranScope.Complete();
                }                

            }
            catch (SqlException sqlE)
            {
                responseType = ResponseType.DbException;
                this._logger.LogError(responseType.ToId(), this._messages[responseType.ToId()].ToString(), sqlE);
            }
            catch (Exception ex)
            {
                responseType = ResponseType.Exception;
                this._logger.LogError(responseType.ToId(), this._messages[responseType.ToId()].ToString(), ex);
            }

            return new Response().ToPopulate(responseType, result, result?.Count);
        }
        public bool IsValidCustomer(IList<string> customeCode, ref IList<DbModel.TechnicalSpecialistCustomers> dbCustomers, ref IList<ValidationMessage> messages)
        {
            IList<ValidationMessage> valdMessage = new List<ValidationMessage>();
            if (customeCode?.Count() > 0)
            {
                if (dbCustomers == null || dbCustomers?.Count <= 0)
                    dbCustomers = _repository?.FindBy(x => customeCode.Contains(x.Code))?.ToList();

                var dbCustomerCode = dbCustomers;
                var companyCodeNotExists = customeCode?.Where(x => !dbCustomerCode.Any(x2 => x2.Code == x))?.ToList();
                companyCodeNotExists?.ForEach(x =>
                {
                    string errorCode = MessageType.InvalidCustomerCode.ToId();
                    valdMessage.Add(_messages, x, MessageType.InvalidCustomerCode, x);
                });

                messages = valdMessage;
            }

            return messages?.Count <= 0;
        }

    }
}

