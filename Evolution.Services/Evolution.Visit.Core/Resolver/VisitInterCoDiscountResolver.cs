using AutoMapper;
using Evolution.Visit.Domain.Models.Visits;
using Evolution.Common.Extensions;
using System.Collections.Generic;
using System.Linq;
using Enum = Evolution.Assignment.Domain.Enums;

namespace Evolution.Visit.Core.Resolver
{
    public class VisitInterCoDiscountResolver<Source, Destination> : IValueResolver<IList<Source>, Destination, string>
    {
        private readonly string fieldName = null;
        private readonly string discountType = null;

        public VisitInterCoDiscountResolver(string fieldName, string discountType)
        {
            this.discountType = discountType;
            this.fieldName = fieldName;
        }

        public string Resolve(IList<Source> source, Destination destination, string destMember, ResolutionContext context)
        {
            var result = Mapper.Map<IList<VisitInterCompanyDiscounts>>(source)?.FirstOrDefault(x => x.DiscountType == discountType);
            return fieldName == EnumExtension.DisplayName(Enum.AssignmentInterCo.CompanyCode) ? result?.CompanyCode : fieldName == EnumExtension.DisplayName(Enum.AssignmentInterCo.CompanyName) ? result?.CompanyName : fieldName == EnumExtension.DisplayName(Enum.AssignmentInterCo.Description) ? result?.Description : null;
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
            return Mapper.Map<IList<VisitInterCompanyDiscounts>>(source)?.FirstOrDefault(x => x.DiscountType == discountType)?.DiscountPercentage;

        }
    }
}
