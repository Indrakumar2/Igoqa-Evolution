using Evolution.Reports.Domain.Interfaces.Data;
using Evolution.Reports.Infrastructure.Data;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Evolution.Reports.Infrastructure.BootStrapper
{
    public class BootStrapper
    {
        public void Register(IServiceCollection services)
        {
            this.RegisterReportsRepository(services);
       
        }

        public void RegisterReportsRepository(IServiceCollection services)
        {
            services.AddScoped<ICustomerApprovalRepository, CustomerApprovalRepository>();
            services.AddScoped<IWonLostRepository, WonLostRepository>();
            services.AddScoped<ICompanySpecificMatrixRepository, CompanySpecificMatrixRepository>();
            services.AddScoped<ICalendarScheduleDetailsRepository, CalendarScheduleDetailsRepository>();
            services.AddScoped<ITaxonomyRepository, TaxonomyRepository>();
        }
    }
}

