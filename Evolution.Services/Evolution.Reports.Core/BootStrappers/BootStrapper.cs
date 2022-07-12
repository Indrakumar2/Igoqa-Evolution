using Evolution.Reports.Core.Services;
using Evolution.Reports.Domain.Interfaces.Reports;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Evolution.Reports.Core.BootStrapper
{
    public class BootStrapper
    {
        public void Register(IServiceCollection services)
        {
            this.RegisterReportServices(services);
           

        }
        public void RegisterReportServices(IServiceCollection services)
        {
            services.AddScoped<ICustomerApprovalService, CustomerApprovalService>();
            services.AddScoped<IWonLostService, WonLostService>();
            services.AddScoped<ICompanySpecificMatrixService, CompanySpecificMatrixService>();
            services.AddScoped<ICalendarScheduleDetailsService, CalendarScheduleDetailsService>();
            services.AddScoped<ITaxonomyService, TaxonomyService>();
        }
    }
}

