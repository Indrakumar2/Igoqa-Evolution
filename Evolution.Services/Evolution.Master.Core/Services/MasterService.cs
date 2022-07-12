using AutoMapper;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Common.Models.Messages;
using Evolution.Common.Models.Responses;
using Evolution.Logging.Interfaces;
using Evolution.Master.Domain.Interfaces.Data;
using Evolution.Master.Domain.Interfaces.Services;
using Evolution.Master.Domain.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Transactions;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.Master.Domain.Models;

namespace Evolution.Master.Core
{
    public class MasterService : IMasterService
    {
        private readonly IAppLogger<MasterService> _logger = null;
        private readonly IMasterRepository _repository = null;
        private readonly IMapper _mapper = null;
        private readonly JObject _messages = null;

        public MasterService(IMapper mapper, IAppLogger<MasterService> logger, IMasterRepository repository, JObject messages)
        {
            this._logger = logger;
            this._repository = repository;
            this._mapper = mapper;
            this._messages = messages;
        }

        public IList<DbModel.Data> Get(IList<MasterType> types,
                                        IList<int> ids = null,
                                        IList<string> names = null,
                                        IList<string> codes = null,
                                        params Expression<Func<DbModel.Data, object>>[] includes)
        {
            IList<DbModel.Data> result = null;
            if (types?.Count > 0)
            {
                var masterIds = types.Select(x => Convert.ToInt32(x));

                var dataQueries = this._repository.FindBy(x => masterIds.Contains(x.MasterDataTypeId), includes);
                if (names?.Count > 0)
                    dataQueries = dataQueries.Where(x => names.Contains(x.Name));

                if (codes?.Count > 0)
                    dataQueries = dataQueries.Where(x => codes.Contains(x.Code));

                if (ids?.Count > 0)
                    dataQueries = dataQueries.Where(x => ids.Contains(x.Id));

                result = dataQueries.OrderBy(x => x.Name).ToList();
            }
            return result;
        }



        public IList<DbModel.Data> Get(IList<MasterType> types,
                                IList<int> ids = null,
                                IList<string> names = null,
                                IList<string> codes = null,
                                string[] includes = null)
        {
            IList<DbModel.Data> result = null;
            if (types?.Count > 0)
            {
                var masterIds = types.Select(x => Convert.ToInt32(x));

                var dataQueries = this._repository.FindBy(x => masterIds.Contains(x.MasterDataTypeId), includes);
                if (names?.Count > 0)
                    dataQueries = dataQueries.Where(x => names.Contains(x.Name));

                if (codes?.Count > 0)
                    dataQueries = dataQueries.Where(x => codes.Contains(x.Code));

                if (ids?.Count > 0)
                    dataQueries = dataQueries.Where(x => ids.Contains(x.Id));

                result = dataQueries.OrderBy(x=>x.Name).ToList();
            }
            return result;
        }

        public Response GetMasterData(MasterData searchModel)
        {
            IList<MasterData> result = null;
            Exception exception = null;
            try
            {
                result = _repository.Search(searchModel);
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), searchModel);
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception, result?.Count);

        }
        public Response GetCommonSystemSetting(IList<string> keyValues)
        {
            IList<DomainModel.SystemSetting> result = null;
            Exception exception = null;
            try
            {
                if (keyValues?.Any()== true)
                {
                    using (var tranScope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                    {
                        result = _repository.GetCommonSystemSetting(keyValues);
                        tranScope.Complete();
                    }
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), keyValues);
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception, result?.Count);

        }

        public Response MasterSave(IList<MasterData> datas)
        {
            Exception exception = null;
            List<ValidationMessage> validationMessages = new List<ValidationMessage>();

            Response response = new Response();
            var newRecords = datas?.Where(x => x.RecordStatus.IsRecordStatusNew())?.ToList();
            var UpdatedRecords = datas?.Where(x => x.RecordStatus.IsRecordStatusModified())?.ToList();
            var deletedRecords = datas?.Where(x => x.RecordStatus.IsRecordStatusDeleted())?.ToList();
            if (newRecords?.Count > 0)
            {
                response = this.Save(datas.ToList());
            }
            if (UpdatedRecords?.Count > 0 && (response==null||response?.Code==ResponseType.Success.ToId()))
            {
                response = this.Modify(datas.ToList());
            }
            
            if (deletedRecords?.Count > 0 && (response==null||response?.Code==ResponseType.Success.ToId()) )
            {
                response = this.Delete(datas.ToList());
            }
            
            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), null, exception);
        }


        private Response Modify(IList<MasterData> datas)
        {
            Exception exception = null;
            IList<DbModel.Data> dbData = null;
            try
            {
                if (datas != null)
                {
                    var recordToBeUpdate = FilterRecord(datas, ValidationType.Update);

                    var validateData = IsValidMasterData(recordToBeUpdate, ValidationType.Update, ref dbData, recordToBeUpdate);


                    if (Convert.ToBoolean(validateData.Result))
                    {
                        var dataId = recordToBeUpdate.Where(x => x.Id != null && x.Id > 0).Select(x => x.Id).ToList();
                        var dbMasterData = dbData?.Where(x => dataId.Contains(x.Id)).ToList();

                        dbMasterData?.ToList()?.ForEach(masterData =>
                        {
                            _repository.AutoSave = false;
                            var masterDataToBeModify = recordToBeUpdate.FirstOrDefault(x => x.Id == masterData.Id);
                            _mapper.Map(masterDataToBeModify, masterData, opt =>
                            {
                                opt.Items["isMasterId"] = true;
                                opt.Items["MasterType"] = true;
                            });
                            masterData.LastModification = DateTime.UtcNow;
                            masterData.UpdateCount = masterDataToBeModify.UpdateCount.CalculateUpdateCount();
                            masterData.ModifiedBy = masterDataToBeModify.ModifiedBy;
                        });
                        _repository.Update(dbMasterData);
                        _repository.ForceSave();
                    }
                    else
                        return validateData;
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), datas);
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, null, null, exception);
        }

        private Response Save(List<MasterData> datas)
        {
            Exception exception = null;
            List<ValidationMessage> validationMessages = null;
            try
            {
                if (datas != null)
                {
                    IList<DbModel.Data> dbData = null;
                    var recordToBeAdd = FilterRecord(datas, ValidationType.Add);
                    var validateData = IsValidMasterData(recordToBeAdd, ValidationType.Add, ref dbData, recordToBeAdd);
                    if (Convert.ToBoolean(validateData.Result) && dbData != null)
                    {
                        _repository.AutoSave = false;
                        dbData = _mapper.Map<IList<DbModel.Data>>(recordToBeAdd, opt =>
                        {
                            opt.Items["isMasterId"] = false;
                           // opt.Items["MasterType"] = true;
                        });
                           
                        _repository.Add(dbData);
                        _repository.ForceSave();
                    }
                    else
                        return validateData;
                }

            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), datas);
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), null, exception);
        }

        private Response Delete(List<MasterData> datas)
        {
            Exception exception = null;
            List<ValidationMessage> validationMessages = null;
            try
            {
                if (datas != null)
                {

                    IList<DbModel.Data> dbData = null;
                    // validationMessages = new List<ValidationMessage>();
                    var recordToBeDelete = FilterRecord(datas, ValidationType.Delete);

                    if (recordToBeDelete.Count > 0)
                    {
                        var validateData = IsValidMasterData(recordToBeDelete, ValidationType.Delete, ref dbData, recordToBeDelete);

                        if (Convert.ToBoolean(validateData.Result) && dbData != null)
                        {
                            var dataId = recordToBeDelete.Select(x => x.Id).ToList();
                            var dbMasterData = dbData?.Where(x => dataId.Contains(x.Id)).ToList();

                            _repository.AutoSave = false;
                            dbMasterData?.ToList().ForEach(masterData =>
                           {
                               masterData.IsActive = false;
                               masterData.UpdateCount = recordToBeDelete?.FirstOrDefault(x => x.Id == masterData.Id)?.UpdateCount.CalculateUpdateCount();
                               _repository.Update(dbMasterData);
                           });
                            _repository.ForceSave();
                        }
                        else
                            return validateData;
                    }

                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), datas);
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), null, exception);
        }

        public Response Search(MasterData search, MasterType type)
        {
            IList<DomainModel.MasterData> result = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                if (search == null)
                    search = new MasterData();

                DbModel.Data dbCustomer = _mapper.Map<DomainModel.MasterData, DbModel.Data>(search);
                result = _mapper.Map<IList<DbModel.Data>, IList<DomainModel.MasterData>>(this._repository.FindBy(dbCustomer.ToExpression()).Where(x => x.MasterDataTypeId == Convert.ToInt32(type)).OrderBy(x => x.Name).ToList());
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

        public Response SearchMasterData(MasterData search)
        {
            IList<DomainModel.MasterData> result = null;
            ResponseType responseType = ResponseType.Success;
            try
            {

                search = search ?? new MasterData();

                DbModel.Data dbData = _mapper.Map<DomainModel.MasterData, DbModel.Data>(search);
                var data = this._repository.FindBy(dbData.ToExpression());
                if (!string.IsNullOrEmpty(search.MasterType) && data != null)
                {
                    var masterDataTypes = search.MasterType.Split(",");
                    result = _mapper.Map<IList<DbModel.Data>, IList<DomainModel.MasterData>>
                        (data?.Where(x => masterDataTypes.Contains(Convert.ToString(x.MasterDataType.Name) ) && x.IsActive==true).OrderBy(x => x.Name).ToList());
                }
            }
            catch (Exception ex)
            {
                responseType = ResponseType.Exception;
                this._logger.LogError(responseType.ToId(), this._messages[responseType.ToId()].ToString(), ex);
            }
            return new Response().ToPopulate(responseType, result, result?.Count);
        }



        private Response IsValidMasterData(IList<MasterData> datas, ValidationType validationType, ref IList<DbModel.Data> dbData, IList<MasterData> filterData)
        {

            Exception exception = null;
            bool result = true;
            IList<ValidationMessage> validationMessages = null;
              IList<int> moduleNotExists = null;
            try
            {


                if (datas != null && datas.Count > 0)
                { 
                    if (filterData == null || filterData.Count <= 0)
                        filterData = FilterRecord(datas, validationType);

                    if (filterData != null && filterData?.Count > 0)
                    { 
                        var MasterDataId = filterData.Where(x => x.Id.HasValue).Select(x => x.Id.Value).Distinct().ToList();
 
                        if ((dbData == null || dbData.Count <= 0))
                            dbData = GetMasterData(filterData);

                        if (validationType == ValidationType.Delete || validationType == ValidationType.Update)
                        {
                            result = IsMasterDataExistInDb(MasterDataId,
                                                                    dbData,
                                                                    ref moduleNotExists,
                                                                    ref validationMessages);   
                        }
                         if (result && validationType != ValidationType.Delete)
                        {
                            result = IsNameDuplicate(filterData, validationType, dbData, ref validationMessages);
                        }
                    }
                    else
                        result = false;

                }


            }
            catch (Exception ex)
            {
                result = false;
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), datas);
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), result, exception);


        }


        private IList<DomainModel.MasterData> FilterRecord(IList<DomainModel.MasterData> datas,
                                                                     ValidationType filterType)
        {
            IList<DomainModel.MasterData> filteredMasterData = null;

            if (filterType == ValidationType.Add)
                filteredMasterData = datas?.Where(x => x.RecordStatus.IsRecordStatusNew()).ToList();
            else if (filterType == ValidationType.Update)
                filteredMasterData = datas?.Where(x => x.RecordStatus.IsRecordStatusModified()).ToList();
            else if (filterType == ValidationType.Delete)
                filteredMasterData = datas?.Where(x => x.RecordStatus.IsRecordStatusDeleted()).ToList();

            return filteredMasterData;
        }

        private IList<DbModel.Data> GetMasterData(IList<DomainModel.MasterData> masterData)
        {
            IList<DbModel.Data> dbData = null;
            if (masterData?.Count > 0)
            {
                var MasterTypes = masterData.Where (x=>!string.IsNullOrEmpty(x.MasterType)).Select(x => x.MasterType).ToList();
                dbData = _repository.FindBy(x => MasterTypes.Contains(x.MasterDataType.Name) && x.IsActive == true).ToList();
            }

            return dbData;
        }

        private bool IsMasterDataExistInDb(IList<int> masterDataIds,
                                                  IList<DbModel.Data> dbData,
                                                  ref IList<int> ModuleNotExists,
                                                  ref IList<ValidationMessage> validationMessages)
        {
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            if (dbData == null)
                dbData = new List<DbModel.Data>();

            var validMessages = validationMessages;

            if (masterDataIds?.Count > 0)
            {
                ModuleNotExists = masterDataIds.Where(x => !dbData.Select(x1 => x1.Id).ToList().Contains(x)).ToList();
                ModuleNotExists?.ToList().ForEach(x =>
                {
                    validMessages.Add(_messages, x, MessageType.InvalidMasterDataId, x);
                });
            }



            if (validMessages.Count > 0)
                validationMessages = validMessages;

            return validationMessages.Count <= 0;
        }


        private bool IsNameDuplicate(IList<MasterData> datas, ValidationType validationType, IList<DbModel.Data> dbData, ref IList<ValidationMessage> errorMessages)
        {
            IList<DbModel.Data> masterDataAlreadyExsists = null;
            if (errorMessages == null)
                errorMessages = new List<ValidationMessage>();

            var validMessages = errorMessages;

           var names=datas?.Where(x=>!string.IsNullOrEmpty( x.Name)).Select(x=>x.Name).ToList();

            if (validationType == ValidationType.Add)
            {
                masterDataAlreadyExsists = dbData?.Where(x=>names.Contains(x.Name)).ToList();
            }
            if (validationType == ValidationType.Update)
            {
                masterDataAlreadyExsists = dbData?.Where(x=>names.Contains(x.Name)).ToList();
            }
            masterDataAlreadyExsists.ToList().ForEach(x =>
            {
                validMessages.Add(_messages, x.Name, MessageType.InvalidMasterData, x.Name);
            });


            if (validMessages.Count > 0)
                errorMessages = validMessages;



            return errorMessages.Count <= 0;
        }

    }
}
