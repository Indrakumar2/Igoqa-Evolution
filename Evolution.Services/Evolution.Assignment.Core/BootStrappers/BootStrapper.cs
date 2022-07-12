using Evolution.Assignment.Core.Services;
using Evolution.Assignment.Domain.Interfaces.Assignments;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Evolution.Assignment.Core.BootStrappers
{
    public class BootStrapper //: IDisposable
    {
       // private bool _disposed = false;
        public void Register(IServiceCollection services)
        {
            services.AddScoped<IAssignmentService, AssignmentService>();
            services.AddScoped<IAssignmentContractRateScheduleService, AssignmentContractRateScheduleService>();
            services.AddScoped<IAssignmentTechnicalSpecialistScheduleService, AssignmentTechnicalSpecialistScheduleService>();
            services.AddScoped<IAssignmentTechnicalSpecilaistService, AssignmentTechnicalSpecialistService>();
            services.AddScoped<IAssignmentAdditionalExpenseService, AssignmentAdditionalExpenseService>();
            services.AddScoped<IAssignmentInstructionsService, AssignmentInstructionsService>();
            services.AddScoped<IAssignmentReferenceService, AssignmentRefrenceService>();
            services.AddScoped<IAssignmentInterCompanyDiscountService, AssignmentInterCompanyDiscountService>();
            // services.AddScoped<IAssignmentSubSupplierTSService, AssignmentSubSupplierTSService>();
            services.AddScoped<IAssignmentContributionCalculationService, AssignmentContributionCalculationService>();
            //services.AddScoped<IAssignmentContributionRevenueCostService, AssignmentContributionRevenueCostService>(); 
            services.AddScoped<IAssignmentNoteService, AssignmentNoteService>();
            services.AddScoped<IAssignmentSubSupplierService, AssignmentSubSupplierService>();
            services.AddScoped<IAssignmentDetailService, AssignmentDetailService>();
            services.AddScoped<IAssignmentTaxonomyService, AssignmentTaxonomyService>();

            //services.AddScoped<IAssignmentDetailService1, AssignmentDetailService1>();
        }

        //public void Dispose()
        //{
        //    Dispose(true);
        //    GC.SuppressFinalize(this);
        //}

        //protected virtual void Dispose(bool disposing)
        //{
        //    if (_disposed)
        //        return;

        //    if (disposing)
        //    {
        //        //IServiceCollection services =null;
        //       //services = null;
        //        // Free any other managed objects here.
        //    }

        //    // Free any unmanaged objects here.
        //    //Marshal.FreeHGlobal(_bufferPtr);
        //    _disposed = true;
        //}

        //~BootStrapper()
        //{
        //    Dispose(false);
        //}
    }
}
