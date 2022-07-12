using AutoMapper;
using AutoMapper.QueryableExtensions;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Common.Models.Messages;
using Evolution.Common.Models.Responses;
using Evolution.DbRepository.Models.SqlDatabaseContext;
using Evolution.Logging.Interfaces;
using Evolution.Master.Domain.Interfaces.Data;
using Evolution.Master.Domain.Interfaces.Services;
using Evolution.Master.Domain.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;

namespace Evolution.Master.Core
{
    public class PayrollTypeService : MasterService, IPayrollTypeService
    {
        private readonly IAppLogger<MasterService> _logger = null;
        private readonly IMasterRepository _repository = null;
        private readonly IMapper _mapper = null;
        private readonly JObject _messages = null;

        public PayrollTypeService(IMapper mapper, IAppLogger<MasterService> logger, IMasterRepository repository ,JObject messages) : base(mapper, logger, repository, messages)
        {
            this._mapper = mapper;
            this._repository = repository;
            this._logger = logger;
            this._messages = messages;
        }

        public bool IsValidPayroll(IList<string> payrollTypeNames, ref IList<string> payrollTypeNotExists, ref IList<PayrollType> payrollTypes, ref List<MessageDetail> errorMessages)
        {
            var messages = new List<MessageDetail>();
            if (errorMessages == null)
                errorMessages = new List<MessageDetail>();

            if (payrollTypeNames?.Count > 0)
            {
                var searchResult = this.Search(payrollTypeNames).Result?.Populate<IList<PayrollType>>();
                var payrollNotExist = payrollTypeNames.Where(x => !searchResult.Any(x1 => x1.Name.ToLower() == x.ToLower())).ToList();
                payrollNotExist.ForEach(x =>
                {
                    string errorCode = MessageType.PayrollNotExists.ToId();
                    messages.Add(new MessageDetail(ModuleType.Master, errorCode, string.Format(_messages[errorCode].ToString(), x)));
                });

                payrollTypeNotExists = payrollNotExist;
                payrollTypes = searchResult;

                if (messages.Count > 0)
                    errorMessages.AddRange(messages);
            }
            return errorMessages?.Count <= 0;
        }

        private bool IsPayrollAlreadyAssociated(IList<string> payrollTypeNames, ref IList<PayrollType> payrollTypes, ref List<MessageDetail> errorMessages)
        {
            var messages = new List<MessageDetail>();
            if (errorMessages == null)
                errorMessages = new List<MessageDetail>();

            if (payrollTypeNames?.Count > 0)
            {
                payrollTypes = this.Search(payrollTypeNames).Result?.Populate<IList<PayrollType>>();

            }
            return payrollTypes?.Count > 0;
        }
        private bool IsPayrollAlreadyExists(IList<string> payrollTypeNames, ref IList<PayrollType> payrollTypes, ref List<MessageDetail> errorMessages, IList<int?> payrollTypeIds)
        {
            var messages = new List<MessageDetail>();
            if (errorMessages == null)
                errorMessages = new List<MessageDetail>();
            bool results =false;
            if (payrollTypeNames?.Count > 0)
            {
                payrollTypes = this.Search(payrollTypeNames).Result?.Populate<IList<PayrollType>>();
                if(payrollTypes?.Count > 0)
                {
                    results = payrollTypes.Where(x => payrollTypeIds.Contains(x.Id)).ToList()?.Count > 0;
                }
            }
            return results;
        }
        public Response UpdatePayrollType(IList<PayrollType> model, bool returnResultSet = false)
        {
            Exception exception = null;
            List<MessageDetail> errorMessages = null;
            IList<PayrollType> payrollTypes = null;
            try
            {
                if (model != null)
                {
                    bool payrollAlreadyExists= this.IsPayrollAlreadyExists(model?.Where(x => x.RecordStatus.IsRecordStatusModified())?.Select(x => x.Name)?.ToList(), ref payrollTypes, ref errorMessages, model?.Where(x => x.RecordStatus.IsRecordStatusModified())?.Select(x => x.Id)?.ToList());
                    if (payrollAlreadyExists)
                    {
                        IList<DbRepository.Models.SqlDatabaseContext.Data> dbPayrollTypes = model.AsQueryable().ProjectTo<Data>().ToList();
                        dbPayrollTypes.ToList().ForEach(x => x.MasterDataTypeId = Convert.ToInt32(MasterType.PayrollType));
                        this._repository.Update(dbPayrollTypes);
                        if (returnResultSet)
                            return this.Search(model.Select(x => x.Name).ToList());
                    }
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), model);
            }
            return new Response().ToPopulate(ResponseType.Success, null, errorMessages, null, payrollTypes, exception);
        }
        public Response SavePayrollType(IList<PayrollType> model, bool returnResultSet = false)
        {
            Exception exception = null;
            List<MessageDetail> errorMessages = null;
            IList<PayrollType> payrollTypes = null;
            try
            {
                if (model != null)
                {

                    if (!this.IsPayrollAlreadyAssociated(model?.Where(x => x.RecordStatus.IsRecordStatusNew())?.Select(x => x.Name)?.ToList(), ref payrollTypes, ref errorMessages))
                    {
                        IList<DbRepository.Models.SqlDatabaseContext.Data> dbPayrollTypes = model.AsQueryable().ProjectTo<Data>().ToList();
                        dbPayrollTypes.ToList().ForEach(x => x.MasterDataTypeId = Convert.ToInt32(MasterType.PayrollType));
                        this._repository.Add(dbPayrollTypes);
                        if (returnResultSet)
                            return this.Search(model.Select(x => x.Name).ToList());
                    }

                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), model);
            }
            return new Response().ToPopulate(ResponseType.Success, null, errorMessages, null, payrollTypes, exception);
        }

        public Response Search(PayrollType search)
        {
            IList<PayrollType> result = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                if (search == null)
                    search = new PayrollType();

                var masterData = _mapper.Map<PayrollType, MasterData>(search);
                using (var tranScope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))                
                {
                    result = this._repository.FindBy(_mapper.Map<MasterData, Data>(masterData).ToExpression())
                                                .Where(x => x.MasterDataTypeId == Convert.ToInt32(MasterType.PayrollType))
                                                .Select(x => _mapper.Map<MasterData, PayrollType>(_mapper.Map<Data, MasterData>(x))).OrderBy(x => x.Name).ToList();
                    tranScope.Complete();
                }
            }
            catch (Exception ex)
            {
                responseType = ResponseType.Exception;
                this._logger.LogError(responseType.ToId(), this._messages[responseType.ToId()].ToString(), ex);
            }
            return new Response().ToPopulate(responseType, result, result?.Count);
        }

        public Response Search(IList<string> payrollTypeNames)
        {
            IList<PayrollType> result = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                if (payrollTypeNames != null)
                using (var tranScope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))                
                {
                    result = this._repository.FindBy(x => x.MasterDataTypeId == Convert.ToInt32(MasterType.PayrollType) && payrollTypeNames.Contains(x.Name))
                                            .Select(x => _mapper.Map<MasterData, PayrollType>(_mapper.Map<Data, MasterData>(x))).OrderBy(x => x.Name).ToList();
                    tranScope.Complete();
                }            
            }
            catch (Exception ex)
            {
                responseType = ResponseType.Exception;
                this._logger.LogError(responseType.ToId(), this._messages[responseType.ToId()].ToString(), ex);
            }
            return new Response().ToPopulate(responseType, result, result?.Count);
        }
    }
}