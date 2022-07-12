using AutoMapper;
using Evolution.UnitTest.Mocks.Contexts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using DbModel = Evolution.DbRepository.Models;

namespace Evolution.UnitTest.UnitTestCases
{
    [TestClass]
    public class BaseTestCase
    {
        protected Mock<DbModel.Evolution2Context> mockContext = null;

        [TestInitialize]
        public void Initialize()
        {

            Mapper.Initialize(cfg =>
            {
                cfg.AddProfile(new Company.Core.Mappers.DomainMapper());
                cfg.AddProfile(new Customer.Core.Mappers.DomainMapper());
                cfg.AddProfile(new Master.Core.Mappers.DomainMapper());
                cfg.AddProfile(new Contract.Core.Mappers.DomainMapper());
                cfg.AddProfile(new Project.Core.Mappers.DomainMapper());
                cfg.AddProfile(new TechnicalSpecialist.Core.Mappers.DomainMapper());
                cfg.AddProfile(new Document.Core.Mappers.DomainMapper());
                cfg.AddProfile(new Supplier.Core.Mappers.DomainMapper());
                cfg.CreateMissingTypeMaps = true;
            });

            mockContext = MockedEvolutionContext.GetMockContext();
        }

        [TestCleanup]
        public void CleanUp()
        {
            Mapper.Reset();
        }
    }
}
