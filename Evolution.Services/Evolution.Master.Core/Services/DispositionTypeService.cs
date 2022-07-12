using AutoMapper;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Common.Models.Responses; 
using Evolution.Logging.Interfaces;
using Evolution.Master.Domain.Interfaces.Data;
using Evolution.Master.Domain.Interfaces.Services;
using Evolution.Master.Domain.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.SqlClient; 
using Evolution.DbRepository.Models.SqlDatabaseContext; 
using System.Linq; 

namespace Evolution.Master.Core.Services
{
    public class DispositionTypeService : MasterService, IDispositionTypeService
    {
        private readonly IAppLogger<MasterService> _logger = null;
        private readonly IMasterRepository _repository = null;
        private readonly IMapper _mapper = null;
        private readonly JObject _messages = null;

        public DispositionTypeService(IMapper mapper, IAppLogger<MasterService> logger, IMasterRepository repository, JObject messages) : base(mapper, logger, repository, messages)
        {
            this._mapper = mapper;
            this._repository = repository;
            this._logger = logger;
            this._messages = messages;
        }

        public Response Search(DispositionType search)
        {
            IList<DispositionType> result = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                if (search == null)
                    search = new DispositionType();

                var masterData = _mapper.Map<DispositionType, MasterData>(search); 
                result = this._repository.FindBy(_mapper.Map<MasterData,Data>(masterData).ToExpression())
                            .Where(x => x.MasterDataTypeId == Convert.ToInt32(MasterType.DispositionType) && (search.IsActive == null || x.IsActive == search.IsActive))
                            .Select(x => _mapper.Map<MasterData, DispositionType>(_mapper.Map<Data, MasterData>(x))).OrderBy(x => x.Name).ToList();
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
    }
}
