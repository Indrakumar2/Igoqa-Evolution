using Evolution.Timesheet.Core.Services;
using Evolution.Timesheet.Domain.Interfaces.Timesheets;
using Microsoft.Extensions.DependencyInjection;

namespace Evolution.Timesheet.Core.BootStrappers
{
    public class BootStrapper
    {
        public void Register(IServiceCollection services)
        {
            services.AddScoped<ITimesheetService, TimesheetService>();
            services.AddScoped<ITechSpecAccountItemConsumableService, TechSpecAccountItemConsumableService>();
            services.AddScoped<ITechSpecAccountItemTimeService, TechSpecAccountItemTimeService>();
            services.AddScoped<ITechSpecAccountItemExpenseService, TechSpecAccountItemExpenseService>();
            services.AddScoped<ITechSpecAccountItemTravelService, TechSpecAccountItemTravelService>();
            services.AddScoped<ITimesheetTechSpecService, TimesheetTechSpecService>();
            services.AddScoped<ITimesheetReferenceService, TimesheetReferenceService>();
            services.AddScoped<ITimesheetNoteService, TimesheetNoteService>();
            services.AddScoped<ITimesheetDetailService, TimesheetDetailService>();
            services.AddScoped<ITimesheetDocumentService, TimesheetDocumentService>();
        }
    }
}
