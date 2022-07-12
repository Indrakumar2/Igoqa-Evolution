using AutoMapper;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Common.Models.Base;
using Evolution.Common.Models.Messages;
using Evolution.Common.Models.Responses;
using Evolution.DbRepository.Models.SqlDatabaseContext;
using Evolution.Logging.Interfaces;
using Evolution.Master.Domain.Interfaces.Data;
using Evolution.Master.Domain.Interfaces.Services;
using Evolution.Master.Domain.Models;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using dbModel=Evolution.DbRepository.Models.SqlDatabaseContext;

namespace Evolution.Master.Core
{
    public class AssignmentReferenceTypeService : MasterService, IAssignmentReferenceType
    {
        private readonly IAppLogger<MasterService> _logger = null;
        private readonly IMapper _mapper = null;
        private readonly IMasterRepository _repository = null;
        private readonly JObject _messages = null;
        private readonly IMemoryCache _memoryCache = null;
        private readonly AppEnvVariableBaseModel _environment = null;

        public AssignmentReferenceTypeService(IMapper mapper, IAppLogger<MasterService> logger, IMasterRepository repository,
                                               IMemoryCache memoryCache, IOptions<AppEnvVariableBaseModel> environment,JObject messages) : base(mapper, logger, repository,messages)
        {
            this._logger = logger;
            this._mapper = mapper;
            this._repository = repository;
            this._messages = messages;
            _memoryCache = memoryCache;
            _environment = environment.Value;
        }

        public Response Search(AssignmentReferenceType search)
        {
            IList<AssignmentReferenceType> result = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                var cacheKey = "AssignmentReferenceType";
                if (search == null)
                    search = new AssignmentReferenceType();
                var masterData = _mapper.Map<AssignmentReferenceType, MasterData>(search);

                if (search.IsFromRefresh)
                {
                    result = AssignmentReference(search, masterData);
                    if (_memoryCache.TryGetValue(cacheKey, out result))
                    {
                        _memoryCache.Remove(cacheKey);
                        _memoryCache.Set(cacheKey, result, ObjectExtension.CacheExpiry(_environment.AbsoluteExpiration, _environment.SlidingExpiration));
                    }
                }
                else
                {
                    if (!_memoryCache.TryGetValue(cacheKey, out result))
                    {
                        result = AssignmentReference(search, masterData);
                        _memoryCache.Set(cacheKey, result, ObjectExtension.CacheExpiry(_environment.AbsoluteExpiration, _environment.SlidingExpiration));
                    }
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
            return new Response().ToPopulate(responseType, result,result?.Count);
        }

        private IList<AssignmentReferenceType> AssignmentReference(AssignmentReferenceType search,MasterData masterData)
        {
            IList<AssignmentReferenceType> result = null;
            /* Modified to clean Assignment*/ //If condition added on 23-Sep2020 to find how complied queries perform
            if (search.Code.HasEvoWildCardChar() || search.Name.HasEvoWildCardChar())
                result = this._repository.FindBy(_mapper.Map<MasterData, Data>(masterData).ToExpression())
                                            .Where(x => x.MasterDataTypeId == Convert.ToInt32(MasterType.AssignmentReferenceType) && (search.IsActive == null || x.IsActive == search.IsActive))
                                            .Select(x => _mapper.Map<MasterData, AssignmentReferenceType>(_mapper.Map<Data, MasterData>(x))).OrderBy(x => x.Name).ToList();
            else
            {
                masterData.MasterDataTypeId = Convert.ToInt32(MasterType.AssignmentReferenceType);
                result = _repository.GetMasterData(masterData).Select(x => _mapper.Map<MasterData, AssignmentReferenceType>(x)).OrderBy(x => x.Name).ToList();
            }
            return result;
        }

        public bool IsValidRefType(IList<string> refTypes, ref IList<dbModel.Data> dbRefType, ref IList<ValidationMessage> messages)
        {
            IList<ValidationMessage> valdMessage = new List<ValidationMessage>();
            if (refTypes?.Count() > 0)
            {
                if (dbRefType == null || dbRefType?.Count <= 0)
                    dbRefType = _repository?.FindBy(x => refTypes.Contains(x.Name))?.ToList();

                var dbRef = dbRefType;
                var refNotExists = refTypes?.Where(x => !dbRef.Any(x2 => x2.Name == x))?.ToList();
                refNotExists?.ForEach(x =>
                {
                    string errorCode = MessageType.InvalidRefType.ToId();
                    valdMessage.Add(_messages, x, MessageType.InvalidRefType, x);
                });

                messages = valdMessage;
            }

            return messages?.Count <= 0;
        }
    }
}
