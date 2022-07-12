using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Evolution.TechnicalSpecialist.Domain.Models.TechSpecialist;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;

namespace Evolution.Automapper.Resolver.TechnicalSpecialist
{
    public class TsCalendarEventsResolver : IMemberValueResolver<object, object, IList<TechnicalSpecialistCalendar>, IList<TechnicalSpecialistCalendarEventView>>
    {
        public TsCalendarEventsResolver()
        {

        }

        public IList<TechnicalSpecialistCalendarEventView> Resolve(object source, object destination, IList<TechnicalSpecialistCalendar> sourceMember, IList<TechnicalSpecialistCalendarEventView> destMember, ResolutionContext context)
        {
            var tsTimeOffRequestInfo = new List<DbModel.TechnicalSpecialistTimeOffRequest>();
            var dbCompanies = new List<DbModel.Company>();
            var tsInfo = new List<BaseTechnicalSpecialistInfo>();
            if (context.Options.Items.ContainsKey("tsInfo"))
                tsInfo = ((List<BaseTechnicalSpecialistInfo>)context.Options.Items["tsInfo"]);
            if (context.Options.Items.ContainsKey("tsTimeOffRequestInfo"))
                tsTimeOffRequestInfo = ((List<DbModel.TechnicalSpecialistTimeOffRequest>)context.Options.Items["tsTimeOffRequestInfo"]);
            if (context.Options.Items.ContainsKey("dbCompanies"))
                dbCompanies = ((List<DbModel.Company>)context.Options.Items["dbCompanies"]);

            if (sourceMember?.Count > 0)
            {
                return sourceMember.Select(x => new TechnicalSpecialistCalendarEventView()
                {
                    Id = x.Id,
                    Start = x.StartDateTime,
                    End = x.EndDateTime,
                    ResourceId = x.TechnicalSpecialistId,
                    Title = x.CalendarType == "PTO" ? tsTimeOffRequestInfo?.FirstOrDefault(x1 => x1.Id == x.CalendarRefCode)?.LeaveType?.Name : x.CalendarType == "PRE" ? "Pre-Assignment" : x.JobReferenceNumber == null ? string.Empty: x.JobReferenceNumber,
                    BgColor = x.CalendarType == "PRE" ? "#474E54" : (x.CalendarStatus == "Confirmed" && x.CalendarType != "PRE") ? "#C00000" : (x.CalendarStatus == "Tentative" && x.CalendarType != "PRE") ? "#FFC700" : (x.CalendarStatus == "TBA" && x.CalendarType != "PRE") ? "#A3A6A9" : "#21B6D7",
                    Resizable = false,
                    CalendarRefCode = x.CalendarRefCode,
                    CalendarType = x.CalendarType,
                    CompanyId = x.CompanyId,
                    CompanyCode = dbCompanies.FirstOrDefault(Com => Com.Id == x.CompanyId).Code,
                    CalendarStatus = x.CalendarStatus,
                    CreatedBy = x.CreatedBy,
                    IsActive = x.IsActive,
                    UpdateCount = x.UpdateCount,
                    RecordStatus = x.RecordStatus,
                    LogInName = tsInfo?.FirstOrDefault(s => s.Id == x.TechnicalSpecialistId)?.LogonName,
                    Description = x.Description
                }).ToList();
            }
            return null;
        }
    }
}