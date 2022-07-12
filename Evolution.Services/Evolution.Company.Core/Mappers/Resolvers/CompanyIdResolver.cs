using AutoMapper;
using Evolution.Common.Extensions;
using Evolution.Company.Domain.Interfaces.Data;
using System.Linq;

namespace Evolution.Company.Core.Mappers.Resolvers
{
    public class CompanyIdResolver : IValueResolver<object, object, int>
    {
        private readonly ICompanyRepository _repository = null;

        public CompanyIdResolver(ICompanyRepository repository)
        {
            this._repository = repository;
        }

        public int Resolve(object source, object destination, int destinationMember, ResolutionContext context)
        {
            var companyName = (string)source.GetPropertyValue("CompanyCode");
            if (string.IsNullOrEmpty(companyName))
            {
                companyName = (string)source.GetPropertyValue("Code");
                if (string.IsNullOrEmpty(companyName))
                {
                    companyName = (string)source.GetPropertyValue("CompanyName");
                    if (string.IsNullOrEmpty(companyName))
                    {
                        companyName = (string)source.GetPropertyValue("Name");
                    }
                }
            }
            if (!string.IsNullOrEmpty(companyName))
            {
                var dbApplication = _repository.FindBy(x => x.Name == companyName).FirstOrDefault();
                if (dbApplication != null)
                    return dbApplication.Id;
            }
            return 0;
        }
    }
}
