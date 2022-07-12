using AutoMapper;
using AutoMapper.QueryableExtensions;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Common.Models.Responses;
using Evolution.Logging.Interfaces;
using Evolution.Master.Domain.Interfaces.Data;
using Evolution.Master.Domain.Interfaces.Services;
using Evolution.Master.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.Master.Domain.Models;

namespace Evolution.Master.Core
{
    public class LogoService : ILogoService
    {
        private readonly IAppLogger<LogoService> _logger = null;
        private readonly IMasterRepository _repository = null;
        private readonly IMapper _mapper = null;

        public LogoService(IAppLogger<LogoService> logger,IMasterRepository repository, IMapper mapper)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
        }

        public Response Search(MasterData search, MasterType type)
        {
            IList<DomainModel.Logo> result = null;
            Exception exception = null;
            try
            {
                result = _repository.FindBy(x => x.MasterDataTypeId == Convert.ToInt32(type) && (search.IsActive == null || x.IsActive == search.IsActive) &&
                            (string.IsNullOrEmpty(search.Code)|| (x.Code == null || x.Code == search.Code))).ProjectTo<DomainModel.Logo>().OrderBy(x => x.Name).ToList();
            }
            catch(Exception ex)
            {
                exception = ex;
            }
            return new Response().ToPopulate(ResponseType.Success, result, result?.Count);
        }


    }
}
