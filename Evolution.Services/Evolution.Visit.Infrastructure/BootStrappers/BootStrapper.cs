using Evolution.Visit.Domain.Interfaces.Data;
using Evolution.Visit.Domain.Interfaces.Validations;
using Evolution.Visit.Infrastructure.Data;
using Evolution.Visit.Infrastructure.Validations;
using Microsoft.Extensions.DependencyInjection;

namespace Evolution.Visit.Infrastructure.BootStrappers
{
    public class BootStrapper
    {
        public void Register(IServiceCollection services)
        {            
            services.AddScoped<IVisitRepository, VisitRepository>();
            services.AddScoped<IVisitDocumentRepository, VisitDocumentRepository>();
            services.AddScoped<IVisitInterCompanyDiscountsRepository, VisitInterCompanyDiscountsRepository>();
            services.AddScoped<IVisitNotesRepository, VisitNoteRepository>();
            services.AddScoped<IVisitReferencesRepository, VisitReferencesRepository>();
            services.AddScoped<IVisitSupplierPerformanceRepository, VisitSupplierPerformanceRepository>();
            services.AddScoped<IVisitTechnicalSpecialistsAccountRepository, VisitTechnicalSpecialistsAccountRepository>();
            services.AddScoped<IVisitTechnicalSpecialistTimeRespository, VisitTechnicalSpecialistTimeRepository>();
            services.AddScoped<IVisitTechnicalSpecialistTravelRepository, VisitTechnicalSpecialistTravelRepository>();
            services.AddScoped<IVisitTechnicalSpecialistExpenseRepository, VisitTechnicalSpecialistExpenseRepository>();
            services.AddScoped<IVisitTechnicalSpecialistConsumableRepository, VisitTechnicalSpecialistConsumablesRepository>();

            services.AddScoped<IVisitValidationService, VisitValidationService>();
            services.AddScoped<IVisitDocumentValidationService, VisitDocumentValidationService>();
            services.AddScoped<IVisitInterCompanyValidationService, VisitInterCompanyDiscountsValidationService>();
            services.AddScoped<IVisitNoteValidationService, VisitNoteValidationService>();
            services.AddScoped<IVisitReferenceValidationService, VisitReferencesValidationService>();
            services.AddScoped<IVisitSupplierPerformanceValidationService, VisitSupplierPerformanceValidationService>();
            services.AddScoped<IVisitTechnicalSpecialistAccountsValidationService, VisitTechnicalSpecialistsAccountsValidationService>();
            services.AddScoped<IVisitTechnicalSpecialistAccountItemTimeValidationService, VisitTechnicalSpecialistAccountItemTimeValidationService>();
            services.AddScoped<IVisitTechnicalSpecialistAccountItemTravelValidationService, VisitTechnicalSpecialistAccountItemTravelValidationService>();
            services.AddScoped<IVisitTechnicalSpecialistAccountItemExpenseValidationService, VisitTechnicalSpecialistAccountItemExpenseValidationService>();
            services.AddScoped<IVisitTechnicalSpecialistAccountItemConsumableValidationService, VisitTechnicalSpecialistAccountItemConsumableValidationService>();
        }
    }
}
