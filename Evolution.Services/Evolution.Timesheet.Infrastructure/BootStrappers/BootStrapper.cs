using Evolution.Timesheet.Domain.Interfaces.Data;
using Evolution.Timesheet.Domain.Interfaces.Validations;
using Evolution.Timesheet.Infrastructure.Data;
using Evolution.Timesheet.Infrastructure.Validations;
using Microsoft.Extensions.DependencyInjection;

namespace Evolution.Timesheet.Infrastructure.BootStrappers
{
    public class BootStrapper
    {
        public void Register(IServiceCollection services)
        {
            services.AddScoped<ITimesheetRepository, TimesheetRepository>();
            services.AddScoped<ITimesheetReferenceRepository, TimesheetReferenceRepository>();
            services.AddScoped<ITimesheetInterCompanyDiscountsRepository, TimesheetInterCompanyDiscountsRepository>();
            services.AddScoped<ITimesheetNoteRepository, TimesheetNoteRepository>();
            services.AddScoped<ITimesheetTechnicalSpecialistRepository, TimesheetTechnicalSpecialistRepository>();
            services.AddScoped<ITechSpecAccountItemTimeRepository, TimesheetTechnicalSpecialistTimeRepository>();
            services.AddScoped<ITechSpecAccountItemTravelRepository, TimesheetTechnicalSpecialistTravelRepository>();
            services.AddScoped<ITechSpecAccountItemExpenseRepository, TimesheetTechnicalSpecialistExpenseRepository>();
            services.AddScoped<ITechSpecAccountItemConsumableRepository, TimesheetTechnicalSpecialistConsumablesRepository>();

            services.AddScoped<ITimesheetValidationService, TimesheetValidationService>();
            services.AddScoped<ITimesheetReferenceValidationService, TimesheetReferenceValidationService>();
            services.AddScoped<ITimesheetNoteValidationService, TimesheetNoteValidationService>();
            services.AddScoped<ITimesheetTechSpecValidationService, TimesheetTechSpecValidationService>();
            services.AddScoped<ITechSpecItemTimeValidationService, TechSpecItemTimeValidationService>();
            services.AddScoped<ITechSpecItemTravelValidationService, TechSpecItemTravelValidationService>();
            services.AddScoped<ITechSpecItemExpenseValidationService, TechSpecItemExpenseValidationService>();
            services.AddScoped<ITechSpecItemConsumableValidationService, TechSpecItemConsumableValidationService>();

        }
    }
}
