using Evolution.NumberSequence.InfraStructure.Interface;
using Evolution.NumberSequence.InfraStructure.Service;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Evolution.NumberSequence.InfraStructure.BootStrapper
{
    public class BootStrapper
    {
		public void Register(IServiceCollection services)
		{
			this.RegisterNumberSequeceService(services);
		}

		private void RegisterNumberSequeceService(IServiceCollection services)
		{
			services.AddScoped<INumberSequenceRepository, NumberSequenceRepository>();
		}
	}
}
