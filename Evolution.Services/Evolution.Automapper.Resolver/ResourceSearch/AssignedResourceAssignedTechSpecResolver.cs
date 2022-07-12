using AutoMapper;
using Evolution.ResourceSearch.Domain.Models.ResourceSearch;
using System;
using System.Collections.Generic;
using System.Linq;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;

namespace Evolution.Automapper.Resolver.ResourceSearch
{
    public class AssignedResourceAssignedTechSpecResolver : IMemberValueResolver<object, object, ICollection<DbModel.AssignmentSubSupplierTechnicalSpecialist>, IList<BaseResourceSearchTechSpecInfo>>
    {
        public AssignedResourceAssignedTechSpecResolver()
        {

        }

        public IList<BaseResourceSearchTechSpecInfo> Resolve(object source, object destination, ICollection<DbModel.AssignmentSubSupplierTechnicalSpecialist> sourceMember, IList<BaseResourceSearchTechSpecInfo> destMember, ResolutionContext context)
        {
            if (sourceMember?.Count > 0)
            {
                return sourceMember.Select(x => {
                    return new BaseResourceSearchTechSpecInfo
                    {
                        Epin =Convert.ToInt32(x?.TechnicalSpecialist?.TechnicalSpecialist.Pin),
                        FirstName= x?.TechnicalSpecialist ?.TechnicalSpecialist.FirstName,
                        LastName = x?.TechnicalSpecialist?.TechnicalSpecialist.LastName,
                        ProfileStatus = x?.TechnicalSpecialist?.TechnicalSpecialist?.ProfileStatus.Name, 
                    };
                 }
                ).ToList();
                 
            }
            return null;
        }
    }
}
