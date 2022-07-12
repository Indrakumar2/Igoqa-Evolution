using AutoMapper;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Common.Models.Responses;
using Evolution.DbRepository.Models.SqlDatabaseContext;
using Evolution.Logging.Interfaces;
using Evolution.Master.Domain.Interfaces.Data;
using Evolution.Master.Domain.Interfaces.Services;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using Evolution.Master.Domain.Models;

namespace Evolution.Master.Core
{
    public class CompanyInspectionTypeChargeRateService : MasterService, ICompanyInspectionTypeChargeRate
    {
        private readonly IMasterRepository _masterRepository = null;
        private readonly IAppLogger<MasterService> _logger = null;
        private readonly ICompanyInspectionTypeChargeRateRepository _repository = null;
        private readonly IMapper _mapper = null;
        private readonly JObject _messages = null;
        public CompanyInspectionTypeChargeRateService(IMapper mapper,
                                                       IAppLogger<MasterService> logger,
                                                        IMasterRepository masterRepository,
                                                       ICompanyInspectionTypeChargeRateRepository repository,JObject messages) : base(mapper, logger, masterRepository,messages)
        {
            this._logger = logger;
            this._repository = repository;
            this._mapper = mapper;
            this._masterRepository = masterRepository;
            this._messages = messages;
        }

        public Response Search(Domain.Models.CompanyInspectionTypeChargeRate search)
        {
            IList<Domain.Models.CompanyInspectionTypeChargeRate> result = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                if (search == null)
                    search = new Domain.Models.CompanyInspectionTypeChargeRate();

                result = this._repository.Search(search);


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
