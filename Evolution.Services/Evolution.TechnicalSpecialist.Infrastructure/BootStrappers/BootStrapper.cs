using Evolution.TechnicalSpecialist.Domain.Interfaces.Data;
using Evolution.TechnicalSpecialist.Domain.Interfaces.Validations;
using Evolution.TechnicalSpecialist.Infrastructe.Validations;
using Evolution.TechnicalSpecialist.Infrastructure.Data;
using Evolution.TechnicalSpecialist.Infrastructure.Validations;
using Microsoft.Extensions.DependencyInjection;

namespace Evolution.TechnicalSpecialist.Infrastructure.BootStrappers
{
    public class BootStrapper
    {
        public void Register(IServiceCollection services)
        {
            services.AddScoped<ITechnicalSpecialistCertificationAndTrainingRepository, TechnicalSpecialistCertificationAndTrainingRepository>();
            services.AddScoped<ITechnicalSpecialistCodeAndStandardRepository, TechnicalSpecialistCodeAndStandardRepository>();
            services.AddScoped<ITechnicalSpecialistCommodityEquipmentKnowledgeRepository, TechnicalSpecialistCommodityEquipmentKnowledgeRepository>();
            RegisterContactService(services);
            services.AddScoped<ITechnicalSpecialistCustomerApprovalRepository, TechnicalSpecialistCustomerApprovalRepository>();
            services.AddScoped<ITechnicalSpecialistEducationalQualificationRepository, TechnicalSpecialistEducationalQualificationRepository>();
            services.AddScoped<ITechnicalSpecialistLanguageCapabilityRepository, TechnicalSpecialistLanguageCapabilityRepository>();
            services.AddScoped<ITechnicalSpecialistPayRateRepository, TechnicalSpecialistPayRateRepository>();
            services.AddScoped<ITechnicalSpecialistPayScheduleRepository, TechnicalSpecialistPayScheduleRepository>();
            services.AddScoped<ITechnicalSpecialistCalendarRepository, TechnicalSpecialistCalendarRepository>();
            RegisterTsService(services);
            services.AddScoped<ITechnicalSpecialistStampInfoRepository, TechnicalSpecialistStampRepository>();
            services.AddScoped<ITechnicalSpecialistTaxonomyRepository, TechnicalSpecialistTaxonomyRepository>();
            services.AddScoped<ITechnicalSpecialistTaxonomyHistoryRepository, TechnicalSpecialistTaxonomyHistoryRepository>();
            services.AddScoped<ITechnicalSpecialistTrainingAndCompetencyRepository, TechnicalSpecialistTrainingAndCompetencyRepository>();
            services.AddScoped<ITechnicalSpecialistWorkHistoryRepository, TechnicalSpecialistWorkHistoryRepository>();
            services.AddScoped<ITechnicalSpecialistComputerElectronicKnowledgeServiceRepository, TechnicalSpecialistComputerElectronicKnowledgeRepository>();
            services.AddScoped<ITechnicalSpecialistTrainingAndCompetencyTypeRepository, TechnicalSpecialistTrainingAndCompetencyTypeRepository>();
            services.AddScoped<ITechnicalSpecialistUsersRepository, TechnicalSpecialistUserRepository>();
            services.AddScoped<ITechnicalSpecialistCertificationAndTrainingRepository, TechnicalSpecialistCertificationAndTrainingRepository>();
            services.AddScoped<ITechnicalSpecialistDraftRepository, TechnicalSpecialistDraftRepository>();
            services.AddScoped<ITechnicalSpecialistNoteRepository, TechnicalSpecialistNoteRepository>();

            services.AddScoped<ITechnicalSpecialistTrainingValidationService, TechnicalSpecilistTrainingValidationServices>();            
            services.AddScoped<ITechnicalSpecialistStampValidationService, TechnicalSpecialistStampValidationService>();
            services.AddScoped<ITechnicalSpecialistWorkHistoryValidationService, TechSpecialistWorkHistoryValidationService>();
            services.AddScoped<ITechnicalSpecialistPayScheduleValidationService, TechnicalSpecialistPayScheduleValidationService>();
            services.AddScoped<ITechnicalSpecialistComputerElectronicKnowledgeValidationService, TechSpecialistComputerElectronicKnowledgeValidationService>();
            services.AddScoped<ITechnicalSpecialistCustomerApprovalValidationService, TechnicalSpecialistCustomerApprovalValidationService>();
            services.AddScoped<ITechnicalSpecialistCustomerApprovalRepository, TechnicalSpecialistCustomerApprovalRepository>();
            services.AddScoped<ITechnicalSpecialistPayScheduleValidationService, TechnicalSpecialistPayScheduleValidationService>();
            services.AddScoped<ITechnicalSpecialistPayRateValidationService, TechnicalSpecialistPayRateValidationService>();
            services.AddScoped<ITechnicalSpecialistQualificationValidationService, TechnicalSpecialistQualificationValidationService>();
            services.AddScoped<ITechnicalSpecialistTrainingValidationService, TechnicalSpecialistTrainingValidationService>();
            services.AddScoped<ITechnicalSpecialistCompetencyValidationService, TechnicalSpecialistCompetencyValidationService>();
            services.AddScoped<ITechnicalSpecialistInternalTrainingValidationService, TechnicalSpecialistInternalTrainingValidationService>();
            services.AddScoped<ITechnicalSpecialistCodeAndStandardValidationService, TechnicalSpecialistCodeValidationService>();
            services.AddScoped<ITechnicalSpecialistCertificationValidationService, TechnicalSpecialistCertificationValidationService>();
            services.AddScoped<ITechnicalSpecialistDraftValidationService, TechnicalSpecialistDraftValidationService>();
            services.AddScoped<ITechnicalSpecialistCalendarValidationService, TechnicalSpecialistCalendarValidationService>();

            //services.AddScoped<ITechnicalSpecialistUsersRepository>();
            services.AddScoped<ITechnicalSpecialistNoteValidationService, TechnicalSpecialistNoteValidationService>();

            services.AddScoped<ITechnicalSpecialistCommodityEquipmentKnowledgeValidationService, TechnicalSpecialistCommodityEquipmentKnowledgeValidationService>();
            services.AddScoped<ITechnicalSpecialistTaxonomyValidationService, TechnicalSpecialistTaxonomyValidationService>();
            services.AddScoped<ITechnicalSpecialistLanguageCapabilityValidationService, TechnicalSpecialistLanguageCapabilityValidationService>();
            services.AddScoped<ITechnicalSpecialistTrainingAndCompetencyTypeValidationService, TechnicalSpecialistTrainingAndCompetencyTypeValidationService>();
            
        }

        public void RegisterContactService(IServiceCollection services)
        {
            services.AddScoped<ITechnicalSpecialistContactRepository, TechnicalSpecialistContactRepository>();
            services.AddScoped<ITechnicalSpecialistContactValidationService, TechnicalSpecialistContactValidationService>();
            services.AddScoped<ITechnicalSpecialistTimeOffRequestValidationService, TechnicalSpecialistTimeOffRequestValidationService>();
        }

        public void RegisterTsService(IServiceCollection services)
        {
            services.AddScoped<ITechnicalSpecialistRepository, TechnicalSpecialistInfoRepository>();
            services.AddScoped<ITechnicalSpecialistValidationService, TechnicalSpecialistValidationService>();
            services.AddScoped<ITechnicalSpecialistTimeOffRequestRepository, TechnicalSpecialistTimeOffRequestRepository>();
        }
    }
}
