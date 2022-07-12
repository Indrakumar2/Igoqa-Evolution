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
using System.IO;
using System.Reflection;
using System.Text;

namespace Evolution.Master.Core
{
    public class CompanyChgSchInspGrpInspectionTypeService : MasterService, ICompanyChgSchInspGrpInspectionType
    {

        private readonly IMasterRepository _masterRepository = null;
        private readonly IAppLogger<MasterService> _logger = null;
        private readonly ICompanyChgSchInspGrpInspectionTypeRepository _repository = null;
        private readonly IMapper _mapper = null;
        private readonly JObject _messages = null;
        public CompanyChgSchInspGrpInspectionTypeService(IMapper mapper, 
            IAppLogger<MasterService> logger, 
            IMasterRepository masterRepository,
            ICompanyChgSchInspGrpInspectionTypeRepository repository,JObject messages) : base(mapper, logger, masterRepository,messages)
        {

            this._logger = logger;
            this._repository = repository;
            this._mapper = mapper;
            this._masterRepository = masterRepository;
            this._messages = JObject.Parse(File.ReadAllText(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"Message_EN.json")));
        }

        public Response Search(CompanyChgSchInspGrpInspectionType search)
        {

            IList<CompanyChgSchInspGrpInspectionType> result = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                if (search == null)
                    search = new CompanyChgSchInspGrpInspectionType();

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
