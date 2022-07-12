using Evolution.TechnicalSpecialist.Core.Services;
using Evolution.TechnicalSpecialist.Domain.Interfaces;
using Evolution.TechnicalSpecialist.Domain.Interfaces.TechnicalSpecialist;
using Microsoft.Extensions.DependencyInjection;

namespace Evolution.TechnicalSpecialist.Core.BootStrappers
{
    public class BootStrapper
    {
        public void Register(IServiceCollection services)
        {
            RegisterTsService(services);
            services.AddScoped<ITechnicalSpecialistCertificationService, TechnicalSpecialistCertificationService>();
            services.AddScoped<ITechnicalSpecialistTrainingService, TechnicalSpecialistTrainingService>();
            services.AddScoped<ITechnicalSpecialistCodeAndStandardService, TechnicalSpecialistCodeAndStandardService>();
            services.AddScoped<ITechnicalSpecialCommodityEquipmentKnowledgeService, TechnicalSpecialistCommodityEquipmentKnowledgeService>();
            services.AddScoped<ITechnicalSpecialistComputerElectronicKnowledgeService, TechnicalSpecialistComputerElectronicKnowledgeService>();
            services.AddScoped<ITechnicalSpecialistCalendarService, TechnicalSpecialistCalendarService>();
            //services.AddScoped<ITechnicalSpecialistContactService, TechnicalSpecialistContactInfoService>();
            RegisterContactService(services);
            services.AddScoped<ITechnicalSpecialistCustomerApprovalService, TechnicalSpecialistCustomerApprovalService>();
            services.AddScoped<ITechnicalSpecialistEducationalQualificationService, TechnicalSpecialistEducationalQualificationService>();
            services.AddScoped<ITechnicalSpecialistLanguageCapabilityService, TechnicalSpecialistLanguageCapabilityService>();
            services.AddScoped<ITechnicalSpecialistPayRateService, TechnicalSpecialistPayRateInfoService>();
            services.AddScoped<ITechnicalSpecialistPayScheduleService, TechnicalSpecialistPayScheduleInfoService>();
            services.AddScoped<ITechnicalSpecialistStampInfoService, TechnicalSpecialistStampInfoService>();
            services.AddScoped<ITechnicalSpecialistTaxonomyService, TechnicalSpecialistTaxonomyInfoService>();
            services.AddScoped<ITechnicalSpecialistCompetencyService, TechnicalSpecialistCompetencyService>();
            services.AddScoped<ITechnicalSpecialistInternalTrainingService, TechnicalSpecialistInternalTrainingService>();
            services.AddScoped<ITechnicalSpecialistWorkHistoryService, TechnicalSpecialistWorkHistoryService>();
            services.AddScoped<ITechnicalSpecialistTrainingAndCompetancyTypeService, TechnicalSpecialistTrainingAndCompetencyTypeService>();
            services.AddScoped<ITechnicalSpecialistDetailService, TechnicalSpecialistDetailService>();
            //services.AddScoped<ITechnicalSpecialistDraftService, TechnicalSpecialistDraftService>();
            services.AddScoped<ITechnicalSpecialistNoteService, TechnicalSpecialistNoteService>();
            services.AddScoped<ITechnicalSpecialistTimeOffRequestService, TechnicalSpecialistTimeOffRequestService>();
        }

        public void RegisterContactService(IServiceCollection services)
        {
            services.AddScoped<ITechnicalSpecialistContactService, TechnicalSpecialistContactInfoService>();
        }

        public void RegisterTsService(IServiceCollection services)
        {
            services.AddScoped<ITechnicalSpecialistService, TechnicalSpecialistService>();
        }
    }
}
