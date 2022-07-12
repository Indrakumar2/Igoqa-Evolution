using AutoMapper;
using Evolution.Assignment.Domain.Models.Assignments;
using Evolution.Common.Extensions;
using System.Collections.Generic;
using System.Linq;
using Enum=Evolution.Assignment.Domain.Enums;

namespace Evolution.Assignment.Core.Resolver
{
    public class AssignmentInterCoDiscountResolver<Source, Destination> : IValueResolver<IList<Source>, Destination, string>
    {
        private readonly string fieldName = null;
        private readonly string discountType = null;

        public AssignmentInterCoDiscountResolver(string fieldName, string discountType)
        {
            this.discountType = discountType;
            this.fieldName = fieldName;
        }

        public string Resolve(IList<Source> source, Destination destination, string destMember, ResolutionContext context)
        {
            var result = Mapper.Map<IList<AssignmentInterCompanyDiscount>>(source)?.FirstOrDefault(x => x.DiscountType == discountType);
            return fieldName == EnumExtension.DisplayName(Enum.AssignmentInterCo.CompanyCode) ? result?.CompanyCode : fieldName == EnumExtension.DisplayName(Enum.AssignmentInterCo.CompanyName) ? result?.CompanyName : fieldName == EnumExtension.DisplayName(Enum.AssignmentInterCo.Description) ? result?.Description : fieldName == EnumExtension.DisplayName(Enum.AssignmentInterCo.AmendmentReason) ? result?.AmendmentReason : null;
        }
    }

    public class DiscountResolver<Source, Destination> : IValueResolver<IList<Source>, Destination, decimal?>
    {
        private readonly string discountType;
        public DiscountResolver(string discountType)
        {
            this.discountType = discountType;
        }

        public decimal? Resolve(IList<Source> source, Destination destination, decimal? destMember, ResolutionContext context)
        {
           return Mapper.Map<IList<AssignmentInterCompanyDiscount>>(source)?.FirstOrDefault(x => x.DiscountType == discountType)?.DiscountPercentage;
            
        }
    }



}
