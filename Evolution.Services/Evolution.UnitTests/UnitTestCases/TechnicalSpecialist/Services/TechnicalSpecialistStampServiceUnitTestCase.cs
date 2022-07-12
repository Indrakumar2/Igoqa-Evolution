using Evolution.UnitTest.UnitTestCases;
using System;
using System.Collections.Generic;
using System.Text;
using ServiceDomainData = Evolution.TechnicalSpecialist.Domain.Interfaces.TechnicalSpecialist;
using DomainData = Evolution.TechnicalSpecialist.Domain.Interfaces.Data;
using Moq;
using DomModel = Evolution.TechnicalSpecialist.Domain.Models.TechSpecialist;
using Evolution.Logging.Interfaces;
using AutoMapper;
using DbModel = Evolution.DbRepository.Models;
using System.Linq;
using Evolution.UnitTests.Mocks.Data.TechnicalSpecialist.Domain;
using Evolution.UnitTests.Mocks.Data.Contracts.Db;
using System.Linq.Expressions;
using DbRepo = Evolution.DbRepository.Interfaces.Master;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Service = Evolution.TechnicalSpecialist.Core.Services;
using Evolution.UnitTests.Mocks.Data.Projects.Db;
using ValdService = Evolution.TechnicalSpecialist.Domain.Interfaces.Validations;
using Evolution.ValidationService.Interfaces;
using Evolution.UnitTests.Mocks.Data.TechnicalSpecialist.Db;
using Evolution.DbRepository.Interfaces.Master;

using Evolution.UnitTest.Mocks.Data.Masters.Db.Masters;

namespace Evolution.UnitTests.UnitTestCases.TechnicalSpecialist.Services
{
    [TestClass]
    public class TechnicalSpecialistStampServiceUnitTestCase
    {
        IValidationService Validation = null;
        ServiceDomainData.ITechnicalSpecialistStampService TechnicalSpecialistStampService = null;
        Mock<DomainData.ITechnicalSpecialistStampRepository> mockTechnicalSpecialistStampRepository = null;
        Mock<DomainData.ITechnicalSpecialistRepository> mockTechnicalSpecialistRepository = null;
        Mock<ICountryRepository> mockCountryRepository = null;
        IList<DomModel.TechnicalSpecialistStamp> TechnicalSpecialistStampDomainModels = null;
        Mock<IAppLogger<Service.TechnicalSpecialistStampService>> mockLogger = null;
        Mock<IMapper> mockMapper = null;
        IQueryable<DbModel.TechnicalSpecialist> mockTechnicalSpecialistDbData = null;
        IQueryable<DbModel.TechnicalSpecialistStamp> mockTechnicalSpecialistStampDbData = null;
        Mock<ICountryRepository> mockCountry = null;
        IQueryable<DbModel.Country> mockCountryDbData = null;
        ValdService.ITechnicalSpecialistStampValidationService validationService = null;
        Mock<IDataRepository> dataRepository = null;

        [TestInitialize]
        public void IntializeTechnicalSpecialistStamp()
        {
            mockTechnicalSpecialistStampRepository = new Mock<DomainData.ITechnicalSpecialistStampRepository>();
            mockTechnicalSpecialistRepository = new Mock<DomainData.ITechnicalSpecialistRepository>();
            TechnicalSpecialistStampDomainModels = MockTechnicalSpecialistModel.GetTechnicalSpecialistStampMockedDomainData();
            mockLogger = new Mock<IAppLogger<Service.TechnicalSpecialistStampService>>();
            mockMapper = new Mock<IMapper>();
            mockTechnicalSpecialistStampDbData = MockTechnicalSpecialist.GetTechnicalSpecialistStampMockData();
            dataRepository = new Mock<IDataRepository>();
            mockCountryDbData = MockMaster.GetCountryMockData();
            mockCountry = new Mock<ICountryRepository>();
            mockTechnicalSpecialistDbData = MockTechnicalSpecialist.GetTechnicalSpecialistMockData();
            mockCountryRepository = new Mock<ICountryRepository>();
        }

        [TestMethod]

        public void FetchAllStampsWithoutSearchValue()
        {
            mockTechnicalSpecialistStampRepository.Setup(x => x.Search(It.IsAny<DomModel.TechnicalSpecialistStamp>())).Returns(TechnicalSpecialistStampDomainModels);
            TechnicalSpecialistStampService = new Service.TechnicalSpecialistStampService(mockTechnicalSpecialistStampRepository.Object, mockLogger.Object, mockTechnicalSpecialistRepository.Object, mockCountry.Object, validationService);
            var response = TechnicalSpecialistStampService.GetStampTypeDetails(new DomModel.TechnicalSpecialistStamp());

            Assert.AreEqual("1", response.Code);
            Assert.AreEqual(2, (response.Result as List<DomModel.TechnicalSpecialistStamp>).Count);
        }

        [TestMethod]
        public void FetchAllStampInfoWithSearchValue()
        {
            mockTechnicalSpecialistStampRepository.Setup(x => x.Search(It.Is<DomModel.TechnicalSpecialistStamp>(c => c.Epin == 54))).Returns(TechnicalSpecialistStampDomainModels);
            TechnicalSpecialistStampService = new Service.TechnicalSpecialistStampService(mockTechnicalSpecialistStampRepository.Object, mockLogger.Object, mockTechnicalSpecialistRepository.Object, mockCountry.Object, validationService);
            var response = TechnicalSpecialistStampService.GetStampTypeDetails(new DomModel.TechnicalSpecialistStamp { Epin = 54 });


            Assert.AreEqual("1", response.Code);
            Assert.AreEqual(2, (response.Result as List<DomModel.TechnicalSpecialistStamp>).Count);
        }

        [TestMethod]
        public void FetchAllStampInfoWithNullSearchModel()
        {
            mockTechnicalSpecialistStampRepository.Setup(x => x.Search(It.IsAny<DomModel.TechnicalSpecialistStamp>())).Returns(TechnicalSpecialistStampDomainModels);
            TechnicalSpecialistStampService = new Service.TechnicalSpecialistStampService(mockTechnicalSpecialistStampRepository.Object, mockLogger.Object, mockTechnicalSpecialistRepository.Object, mockCountry.Object, validationService);
            var response = TechnicalSpecialistStampService.GetStampTypeDetails(null);


            Assert.AreEqual("1", response.Code);
            Assert.AreEqual(2, (response.Result as List<DomModel.TechnicalSpecialistStamp>).Count);
        }

        [TestMethod]
        public void ThrowsExceptionWhileFetchingStampInfo()
        {
            mockTechnicalSpecialistStampRepository.Setup(x => x.Search(It.IsAny<DomModel.TechnicalSpecialistStamp>())).Throws(new Exception("Exception occured while performing some operation."));
            TechnicalSpecialistStampService = new Service.TechnicalSpecialistStampService(mockTechnicalSpecialistStampRepository.Object, mockLogger.Object, mockTechnicalSpecialistRepository.Object, mockCountry.Object, validationService);

            var response = TechnicalSpecialistStampService.GetStampTypeDetails(new DomModel.TechnicalSpecialistStamp());

            Assert.AreEqual("11", response.Code);
            Assert.IsNotNull(response.Messages);
            Assert.AreEqual("31", response.Messages.FirstOrDefault().Code);
        }

        [TestMethod]
        public void SaveTechnicalSpecialistStamp()
        {
            mockTechnicalSpecialistRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.TechnicalSpecialist, bool>>>())).Returns(mockTechnicalSpecialistDbData);
            mockCountryRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Country, bool>>>())).Returns(mockCountryDbData);
            mockTechnicalSpecialistStampRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.TechnicalSpecialistStamp, bool>>>())).Returns((Expression<Func<DbModel.TechnicalSpecialistStamp, bool>> predicate) => mockTechnicalSpecialistStampDbData.Where(predicate));           
            Validation = new Evolution.ValidationService.Services.ValidationService();
            validationService = new Evolution.TechnicalSpecialist.Infrastructure.Validations.TechnicalSpecialistStampValidationService(Validation);
            TechnicalSpecialistStampService = new Service.TechnicalSpecialistStampService(mockTechnicalSpecialistStampRepository.Object, mockLogger.Object, mockTechnicalSpecialistRepository.Object, mockCountry.Object, validationService);
            TechnicalSpecialistStampDomainModels.ToList().ForEach(x => x.RecordStatus = "N");
            var response = TechnicalSpecialistStampService.SaveStampTypeDetails((int)TechnicalSpecialistStampDomainModels[0].Epin, new List<DomModel.TechnicalSpecialistStamp> { TechnicalSpecialistStampDomainModels[0] });
            mockTechnicalSpecialistStampRepository.Verify(m => m.Add(It.IsAny<IList<DbModel.TechnicalSpecialistStamp>>()), Times.AtLeastOnce);
        }
    }
}
