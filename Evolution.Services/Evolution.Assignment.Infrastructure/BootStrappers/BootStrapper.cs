using Evolution.Assignment.Domain.Interfaces.Data;
using Evolution.Assignment.Domain.Interfaces.Validations;
using Evolution.Assignment.Infrastructe.Validations;
using Evolution.Assignment.Infrastructure.Data;
using Evolution.Assignment.Infrastructure.Validations;
using Microsoft.Extensions.DependencyInjection;

namespace Evolution.Assignment.Infrastructe.BootStrappers
{
    public class BootStrapper
    {
        public void Register(IServiceCollection services)
        {
            RegisterRepository(services);
            RegisterValidationService(services);
        }

        private void RegisterRepository(IServiceCollection services)
        {
            services.AddScoped<IAssignmentRepository, AssignmentRepository>();
            services.AddScoped<IAssignmentContractRateScheduleRepository, AssignmentContractRateScheduleRepository>();
            services.AddScoped<IAssignmentTechnicalSpecialistScheduleRepository, AssignmentTechnicalSpecialistScheduleRepository>();
            services.AddScoped<IAssignmentTechnicalSpecilaistRepository, AssignmentTechnicalSpecialistRepository>();
            services.AddScoped<IAssignmentAdditionalExpenseRepository, AssignmentAdditionalExpenseRepository>();
            services.AddScoped<IAssignmentReferenceTypeRepository, AssignmentReferenceRepository>();
            services.AddScoped<IAssignmentInterCompanyDiscountRepository, AssignmentInterCompanyDiscountRepository>();
            services.AddScoped<IAssignmentSubSupplerTSRepository, AssignmentSubSupplierTSRepository>();
            services.AddScoped<IAssignmentSubSupplerRepository, AssignmentSubSupplierRepository>();
            services.AddScoped<IAssignmentContributionCalculationRepository, AssignmentContributionCalculationRepository>();
            services.AddScoped<IAssignmentContributionRevenueCostRepository, AssignmentContributionRevenueCostRepository>();
            services.AddScoped<IAssignmentNoteRepository, AssignmentNoteRepository>();
            services.AddScoped<IAssignmentInstructionsRepository, AssignmentInstructionsRepository>();
            services.AddScoped<IAssignmentTaxonomyRepository, AssignmentTaxonomyRepository>();
        }

        private void RegisterValidationService(IServiceCollection services)
        {
            services.AddScoped<IAssignmentTechnicalSpecialistScheduleValidationService, AssignmentTechnicalSpecialistScheduleValidationService>();
            services.AddScoped<IAssignmentValidationService, AssignmentValidationService>();
            services.AddScoped<IAssignmentContractRateScheduleValidationService, AssignmentContractRateScheduleValidationService>();
            services.AddScoped<IAssignmentTechnicalSpecilaistValidationService, AssignmentTechnicalSpecialistValidationServcie>();
            services.AddScoped<IAssignmentContractRateScheduleValidationService, AssignmentContractRateScheduleValidationService>();
            services.AddScoped<IAssignmentAdditionalExpenseValidationService, AssignmentAdditionalExpenseValidationService>();
            services.AddScoped<IAssignmentReferenceTypeValidationService, AssignmentRefrenceValidationService>();
            services.AddScoped<IAssignmentInterCompanyDiscountValidationService, AssignmentInterCompanyDiscountValidationService>();
            services.AddScoped<IAssignmentContributionCalculationValidationService, AssignmentContributionCalculationValidationService>();
            //services.AddScoped<IAssignmentContributionRevenueCostValidationService, AssignmentContributionRevenueCostValidationService>();
            services.AddScoped<IAssignmentSubSupplierTSValidationService, AssignmentSubSupplierTSValidationService>();
            services.AddScoped<IAssignmentNoteValidationService, AssignmentNoteValidationService>();
            services.AddScoped<IAssignmentSubSupplierValidationService, AssignmentSubSupplierValidationService>();
            services.AddScoped<IAssignmentTaxonomyValidationService, AssignmentTaxonomyValidationService>();
        }
    }
}
