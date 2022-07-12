using Evolution.Api.Controllers.Base;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Common.Models.Responses;
using Evolution.Logging.Interfaces;
using Evolution.TechnicalSpecialist.Domain.Interfaces.TechnicalSpecialist;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DomainModel = Evolution.TechnicalSpecialist.Domain.Models.TechSpecialist;

namespace Evolution.Api.Controllers.TechnicalSpecialist
{
    [Route("/api/TechnicalSpecialists/TechnicalSpecialistCalendar")]
    public class TechnicalSpecialistCalendarController : BaseController
    {
        private readonly ITechnicalSpecialistCalendarService _technicalSpecialistCalendarService = null;
        private readonly IAppLogger<TechnicalSpecialistCalendarController> _logger = null;

        public TechnicalSpecialistCalendarController(ITechnicalSpecialistCalendarService technicalSpecialistCalendarService, IAppLogger<TechnicalSpecialistCalendarController> logger)
        {
            _technicalSpecialistCalendarService = technicalSpecialistCalendarService;
            _logger = logger;
        }

        [HttpGet]
        public Response Get([FromQuery]  DomainModel.TechnicalSpecialistCalendar searchModel, bool isSearch, bool isCalendarView = true)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                if (isSearch)
                    return this._technicalSpecialistCalendarService.SearchGet(searchModel, isCalendarView);
                else
                    return this._technicalSpecialistCalendarService.Get(searchModel, isCalendarView);

            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                //Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), new { searchModel, isSearch, isCalendarView });
            }
            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }

        [HttpPost]
        [Route("GetCalendarByTechnicalSpecialistId")]
        public Response GetCalendarByTechnicalSpecialistId([FromBody] DomainModel.TechnicalSpecialistCalendar searchModel)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                return this._technicalSpecialistCalendarService.GetCalendarByTechnicalSpecialistId(searchModel);
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                //Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), searchModel);
            }
            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }

        [HttpPost]
        public Response Post([FromBody]  IList<DomainModel.TechnicalSpecialistCalendar> searchModel)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                AssignValuesFromToken(ref searchModel);
                return this._technicalSpecialistCalendarService.Save(searchModel);
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                //Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), searchModel);
            }
            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }

        [HttpPut]
        public Response Put([FromBody]  IList<DomainModel.TechnicalSpecialistCalendar> searchModel)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                AssignValuesFromToken(ref searchModel);
                return this._technicalSpecialistCalendarService.Update(searchModel);
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                //Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), searchModel);
            }
            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }

        [HttpDelete]
        public Response Delete([FromBody]  IList<DomainModel.TechnicalSpecialistCalendar> searchModel)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                AssignValuesFromToken(ref searchModel);
                return this._technicalSpecialistCalendarService.Delete(searchModel);
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                //Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), searchModel);
            }
            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }

        private void AssignValuesFromToken(ref IList<DomainModel.TechnicalSpecialistCalendar> technicalSpecialistInfos)
        {
            technicalSpecialistInfos.ToList().ForEach(x =>
            {
                x.ModifiedBy = UserName;
                x.ActionByUser = UserName;
                x.CreatedBy = UserName;
                x.LastModification = DateTime.UtcNow;
            });
        }
    }
}
