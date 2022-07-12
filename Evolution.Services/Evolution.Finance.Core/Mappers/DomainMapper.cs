using AutoMapper;
using Evolution.Finance.Domain.Enums;
using Evolution.Finance.Domain.Models.Finance;
using System;
using System.Collections.Generic;
using System.Linq;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.Finance.Domain.Models.Finance;

namespace Evolution.Finance.Core.Mappers
{
    public class DomainMapper : Profile
    {
        public DomainMapper()
        {
            
        }
    }
}
