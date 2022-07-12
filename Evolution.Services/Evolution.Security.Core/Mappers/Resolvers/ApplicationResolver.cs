//using AutoMapper;
//using Evolution.Common.Extensions;
//using Evolution.Security.Domain.Interfaces.Data;
//using System.Linq;

//namespace Evolution.Security.Core.Mappers.Resolvers
//{
//    public class ApplicationIdResolver : IValueResolver<object, object, int>
//    {
//        private readonly IApplicationRepository _applicationRepository = null;

//        public ApplicationIdResolver(IApplicationRepository applicationRepository)
//        {
//            this._applicationRepository = applicationRepository;
//        }

//        public int Resolve(object source, object destination, int destinationMember, ResolutionContext context)
//        {
//            var appName = (string)source.GetPropertyValue("ApplicationName");
//            if (!string.IsNullOrEmpty(appName))
//            {
//                var dbApplication = _applicationRepository.FindBy(x => x.Name == appName).FirstOrDefault();
//                if (dbApplication != null)
//                    return dbApplication.Id;
//            }
//            return 0;
//        }
//    }
//}



