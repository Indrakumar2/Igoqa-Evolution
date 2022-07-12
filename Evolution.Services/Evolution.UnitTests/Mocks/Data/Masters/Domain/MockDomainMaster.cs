using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text; 
using DomModel = Evolution.Master.Domain.Models;

namespace Evolution.UnitTests.Mocks.Data.Masters.Domain.Masters
{
    public static partial class MockDomainMaster
    {
        public static IQueryable<DomModel.Currency> GetCurrencyMockData()
        {
            return new List<DomModel.Currency>
            {
                new DomModel.Currency { Code="GBP",Name="United Kingdom, Pounds",LastModification=DateTime.UtcNow,UpdateCount=0,ModifiedBy=null },
                new DomModel.Currency { Code="AED",Name="United Arab Emirates, Dirhams",LastModification=DateTime.UtcNow,UpdateCount=0,ModifiedBy=null},

            }.AsQueryable();

        }
        public static Mock<DbSet<DomModel.Currency>> GetCurrencyMockDbSet(IQueryable<DomModel.Currency> data)
        {
            var mockSet = new Mock<DbSet<DomModel.Currency>>();
            mockSet.As<IQueryable<DomModel.Currency>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<DomModel.Currency>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<DomModel.Currency>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<DomModel.Currency>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            return mockSet;
        }
        public static IQueryable<DomModel.AssignmentReferenceType> GetAssignmentRefMockData()
        {
            return new List<DomModel.AssignmentReferenceType>
            {
                new DomModel.AssignmentReferenceType { Code=null,Name="Activity Details",LastModification=DateTime.UtcNow,UpdateCount=0,ModifiedBy=null },
                new DomModel.AssignmentReferenceType { Code=null,Name="Assign. Date",LastModification=DateTime.UtcNow,UpdateCount=0,ModifiedBy=null},

            }.AsQueryable();

        }
        public static Mock<DbSet<DomModel.AssignmentReferenceType>> GetAssignmentRefMockDbSet(IQueryable<DomModel.AssignmentReferenceType> data)
        {
            var mockSet = new Mock<DbSet<DomModel.AssignmentReferenceType>>();
            mockSet.As<IQueryable<DomModel.AssignmentReferenceType>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<DomModel.AssignmentReferenceType>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<DomModel.AssignmentReferenceType>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<DomModel.AssignmentReferenceType>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            return mockSet;

        }
        public static IQueryable<DomModel.CostOfSale> GetCostOfSaleMockData()
        {
            return new List<DomModel.CostOfSale>
            {
                new DomModel.CostOfSale { Name="Hours",ChargeReference="100150",ChargeType="R",LastModification=DateTime.UtcNow,ModifiedBy=null,UpdateCount=0 },
                new DomModel.CostOfSale { Name="Days",ChargeReference="100150",ChargeType="E",LastModification=DateTime.UtcNow,ModifiedBy=null,UpdateCount=0},

            }.AsQueryable();

        }
        public static Mock<DbSet<DomModel.CostOfSale>> GetCostOfSaleMockDbSet(IQueryable<DomModel.CostOfSale> data)
        {
            var mockSet = new Mock<DbSet<DomModel.CostOfSale>>();
            mockSet.As<IQueryable<DomModel.CostOfSale>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<DomModel.CostOfSale>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<DomModel.CostOfSale>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<DomModel.CostOfSale>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            return mockSet;

        }

        public static IList<DomModel.City> GetCityMockedDomainData()
        {
            return new List<DomModel.City>
            {
                new DomModel.City { Name="Brighton",Country="UK",LastModification=DateTime.UtcNow,ModifiedBy=null,UpdateCount=0 },
                new DomModel.City { Name="Houston",Country="Andorra, Principality of",LastModification=DateTime.UtcNow,ModifiedBy=null,UpdateCount=0},

            };

        }
       


    }
}
