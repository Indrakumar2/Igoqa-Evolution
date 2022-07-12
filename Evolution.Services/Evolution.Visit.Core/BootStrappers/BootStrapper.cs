using Evolution.Visit.Core.Services;
using Evolution.Visit.Domain.Interfaces.Visits;
using Microsoft.Extensions.DependencyInjection;

namespace Evolution.Visit.Core.BootStrappers
{
    public class BootStrapper
    {
        public void Register(IServiceCollection services)
        {
            this.RegisterVisitService(services);
        }

        public void RegisterVisitService(IServiceCollection services)
        {
            services.AddScoped<IVisitService, VisitService>();
            services.AddScoped<IVisitDetailService, VisitDetailService>();
            services.AddScoped<IVisitDocumentService, VisitDocumentService>();
            services.AddScoped<IVisitInterCompanyService, VisitInterCompanyDiscountsService>();
            services.AddScoped<IVisitNoteService, VisitNotesService>();
            services.AddScoped<IVisitReferenceService, VisitReferencesService>();
            services.AddScoped<IVisitSupplierPerformanceService, VisitSupplierPerformanceService>();
            services.AddScoped<IVisitTechnicalSpecilaistAccountsService, VisitTechnicalSpecialistsAccountsService>();
            services.AddScoped<IVisitTechnicalSpecialistTimeService, VisitTechnicalSpecialistTimeService>();
            services.AddScoped<IVisitTechnicalSpecialistTravelService, VisitTechnicalSpecialistTravelService>();
            services.AddScoped<IVisitTechnicalSpecialistExpenseService, VisitTechnicalSpecialistExpenseService>();
            services.AddScoped<IVisitTechnicalSpecialistConsumableService, VisitTechnicalSpecialistConsumableService>();
        }
    }
}
