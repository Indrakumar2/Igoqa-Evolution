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
    public class TechnicalSpecialistEducationalQualificationServiceUnitTestCase
    {

        IValidationService Validation = null;
        ServiceDomainData.ITechnicalSpecialistEducationalQualificationService TechnicalSpecialistEducationalQualificationService = null;
        Mock<DomainData.ITechnicalSpecialistEducationalQualificationRepository> mockTechnicalSpecialistEducationalQualificationRepository = null;
        Mock<DomainData.ITechnicalSpecialistRepository> mockTechnicalSpecialistRepository = null;
        IList<DomModel.TechnicalSpecialistEducationalQualification> TechnicalSpecialistEducationalQualificationDomainModels = null;
        Mock<IAppLogger<Service.TechnicalSpecialistEducationalQualificationService>> mockLogger = null;
        Mock<IMapper> mockMapper = null;
        IQueryable<DbModel.TechnicalSpecialist> mockTechnicalSpecialistDbData = null;
        IQueryable<DbModel.TechnicalSpecialistEducationalQualification> mockTechnicalSpecialistEducationalQualificationDbData = null;
        IQueryable<DbModel.Country> MockCountry = null;
        IQueryable<DbModel.County> MockCounty = null;
        IQueryable<DbModel.City> MockCity = null;
        ValdService.ITechnicalSpecialistQualificationValidationService validationService = null;
        Mock<IDataRepository> dataRepository = null;
        Mock<ICountryRepository> countryRepository = null;
        Mock<ICountyRepository> countyRepository = null;
        Mock<ICityRepository> cityRepository = null;


        [TestInitialize]
        public void IntializeTechnicalSpecialistEducationalQualification()
        {
            mockTechnicalSpecialistEducationalQualificationRepository = new Mock<DomainData.ITechnicalSpecialistEducationalQualificationRepository>();
            mockTechnicalSpecialistRepository = new Mock<DomainData.ITechnicalSpecialistRepository>();
            TechnicalSpecialistEducationalQualificationDomainModels = MockTechnicalSpecialistModel.GetEducationalQualificationMockedDomainData();
            mockLogger = new Mock<IAppLogger<Service.TechnicalSpecialistEducationalQualificationService>>();
            mockMapper = new Mock<IMapper>();
            mockTechnicalSpecialistEducationalQualificationDbData = MockTechnicalSpecialist.GetEducationalQualificationMockData();
            countryRepository = new Mock<ICountryRepository>();
            MockCountry = MockMaster.GetCountryMockData();
            countyRepository = new Mock<ICountyRepository>();
            MockCounty = MockMaster.GetCountyMockData();
            cityRepository = new Mock<ICityRepository>();
            MockCity = MockMaster.GetCityMockData();
            mockTechnicalSpecialistDbData = MockTechnicalSpecialist.GetTechnicalSpecialistMockData();
        }
        [TestMethod]
        public void SaveTechnicalSpecialistEducationalQualification()
        {

            mockTechnicalSpecialistRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.TechnicalSpecialist, bool>>>())).Returns(mockTechnicalSpecialistDbData);
            countryRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Country, bool>>>())).Returns((Expression<Func<DbModel.Country, bool>> predicate) => MockCountry.Where(predicate));
            countyRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.County, bool>>>())).Returns((Expression<Func<DbModel.County, bool>> predicate) => MockCounty.Where(predicate));
            cityRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.City, bool>>>())).Returns((Expression<Func<DbModel.City, bool>> predicate) => MockCity.Where(predicate));
            mockTechnicalSpecialistEducationalQualificationRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.TechnicalSpecialistEducationalQualification, bool>>>())).Returns((Expression<Func<DbModel.TechnicalSpecialistEducationalQualification, bool>> predicate) => mockTechnicalSpecialistEducationalQualificationDbData.Where(predicate));
            Validation = new Evolution.ValidationService.Services.ValidationService();
            validationService = new Evolution.TechnicalSpecialist.Infrastructure.Validations.TechnicalSpecialistQualificationValidationService(Validation);
            TechnicalSpecialistEducationalQualificationService = new Service.TechnicalSpecialistEducationalQualificationService(mockTechnicalSpecialistEducationalQualificationRepository.Object, mockLogger.Object, mockTechnicalSpecialistRepository.Object, cityRepository.Object, countyRepository.Object, countryRepository.Object, mockMapper.Object, validationService);
            TechnicalSpecialistEducationalQualificationDomainModels.ToList().ForEach(x => x.RecordStatus = "N");
            var response = TechnicalSpecialistEducationalQualificationService.SaveEducationalQualification((int)TechnicalSpecialistEducationalQualificationDomainModels[0].Epin, new List<DomModel.TechnicalSpecialistEducationalQualification> { TechnicalSpecialistEducationalQualificationDomainModels[0] });
            mockTechnicalSpecialistEducationalQualificationRepository.Verify(m => m.Add(It.IsAny<IList<DbModel.TechnicalSpecialistEducationalQualification>>()), Times.AtLeastOnce);
        }

        [TestMethod]
        public void ModifyTechnicalSpecialistEducationalQualification()
        {

            mockTechnicalSpecialistRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.TechnicalSpecialist, bool>>>())).Returns(mockTechnicalSpecialistDbData);
            countryRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Country, bool>>>())).Returns((Expression<Func<DbModel.Country, bool>> predicate) => MockCountry.Where(predicate));
            countyRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.County, bool>>>())).Returns((Expression<Func<DbModel.County, bool>> predicate) => MockCounty.Where(predicate));
            cityRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.City, bool>>>())).Returns((Expression<Func<DbModel.City, bool>> predicate) => MockCity.Where(predicate));
            mockTechnicalSpecialistEducationalQualificationRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.TechnicalSpecialistEducationalQualification, bool>>>())).Returns((Expression<Func<DbModel.TechnicalSpecialistEducationalQualification, bool>> predicate) => mockTechnicalSpecialistEducationalQualificationDbData.Where(predicate));
            Validation = new Evolution.ValidationService.Services.ValidationService();
            validationService = new Evolution.TechnicalSpecialist.Infrastructure.Validations.TechnicalSpecialistQualificationValidationService(Validation);
            TechnicalSpecialistEducationalQualificationService = new Service.TechnicalSpecialistEducationalQualificationService(mockTechnicalSpecialistEducationalQualificationRepository.Object, mockLogger.Object, mockTechnicalSpecialistRepository.Object, cityRepository.Object, countyRepository.Object, countryRepository.Object, mockMapper.Object, validationService);
            TechnicalSpecialistEducationalQualificationDomainModels.ToList().ForEach(x => x.RecordStatus = "M");
            var response = TechnicalSpecialistEducationalQualificationService.ModifyEducationalQualification((int)TechnicalSpecialistEducationalQualificationDomainModels[0].Epin, new List<DomModel.TechnicalSpecialistEducationalQualification> { TechnicalSpecialistEducationalQualificationDomainModels[0] });
            Assert.AreEqual("1", response.Code);
            mockTechnicalSpecialistEducationalQualificationRepository.Verify(m => m.ForceSave(), Times.Once);
        }

        [TestMethod]
        public void DeleteTechnicalSpecialistLanguageCapability()
        {
            mockTechnicalSpecialistRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.TechnicalSpecialist, bool>>>())).Returns(mockTechnicalSpecialistDbData);
            countryRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Country, bool>>>())).Returns((Expression<Func<DbModel.Country, bool>> predicate) => MockCountry.Where(predicate));
            countyRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.County, bool>>>())).Returns((Expression<Func<DbModel.County, bool>> predicate) => MockCounty.Where(predicate));
            cityRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.City, bool>>>())).Returns((Expression<Func<DbModel.City, bool>> predicate) => MockCity.Where(predicate));
            mockTechnicalSpecialistEducationalQualificationRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.TechnicalSpecialistEducationalQualification, bool>>>())).Returns((Expression<Func<DbModel.TechnicalSpecialistEducationalQualification, bool>> predicate) => mockTechnicalSpecialistEducationalQualificationDbData.Where(predicate));
            Validation = new Evolution.ValidationService.Services.ValidationService();
            validationService = new Evolution.TechnicalSpecialist.Infrastructure.Validations.TechnicalSpecialistQualificationValidationService(Validation);
            TechnicalSpecialistEducationalQualificationService = new Service.TechnicalSpecialistEducationalQualificationService(mockTechnicalSpecialistEducationalQualificationRepository.Object, mockLogger.Object, mockTechnicalSpecialistRepository.Object, cityRepository.Object, countyRepository.Object, countryRepository.Object, mockMapper.Object, validationService);
            TechnicalSpecialistEducationalQualificationDomainModels.ToList().ForEach(x => x.RecordStatus = "D");
            var response = TechnicalSpecialistEducationalQualificationService.DeleteEducationalQualification((int)TechnicalSpecialistEducationalQualificationDomainModels[0].Epin, new List<DomModel.TechnicalSpecialistEducationalQualification> { TechnicalSpecialistEducationalQualificationDomainModels[0] });
            Assert.AreEqual("1", response.Code);
            mockTechnicalSpecialistEducationalQualificationRepository.Verify(m => m.ForceSave(), Times.Once);
        }

        [TestMethod]
        public void FetchAllEducationalQualificationWithSearchValue()
        {
            mockTechnicalSpecialistEducationalQualificationRepository.Setup(x => x.Search(It.Is<DomModel.TechnicalSpecialistEducationalQualification>(c => c.Epin == 54))).Returns(TechnicalSpecialistEducationalQualificationDomainModels);
            TechnicalSpecialistEducationalQualificationService = new Service.TechnicalSpecialistEducationalQualificationService(mockTechnicalSpecialistEducationalQualificationRepository.Object, mockLogger.Object, mockTechnicalSpecialistRepository.Object, cityRepository.Object,countyRepository.Object,countryRepository.Object, mockMapper.Object, validationService);
            var response = TechnicalSpecialistEducationalQualificationService.GetEducationalQualification(new DomModel.TechnicalSpecialistEducationalQualification { Epin = 54 });
            Assert.AreEqual("1", response.Code);
            Assert.AreEqual(2, (response.Result as List<DomModel.TechnicalSpecialistEducationalQualification>).Count);
        }

        [TestMethod]
        public void FetchAllEducationalQualificationWithoutSearchValue()
        {
            mockTechnicalSpecialistEducationalQualificationRepository.Setup(x => x.Search(It.IsAny<DomModel.TechnicalSpecialistEducationalQualification>())).Returns(TechnicalSpecialistEducationalQualificationDomainModels);
            TechnicalSpecialistEducationalQualificationService = new Service.TechnicalSpecialistEducationalQualificationService(mockTechnicalSpecialistEducationalQualificationRepository.Object, mockLogger.Object, mockTechnicalSpecialistRepository.Object, cityRepository.Object, countyRepository.Object, countryRepository.Object, mockMapper.Object, validationService);
            var response = TechnicalSpecialistEducationalQualificationService.GetEducationalQualification(new DomModel.TechnicalSpecialistEducationalQualification());
            Assert.AreEqual("1", response.Code);
            Assert.AreEqual(2, (response.Result as List<DomModel.TechnicalSpecialistEducationalQualification>).Count);
        }

        [TestMethod]
        public void FetchAllEducationalQualificationWithNullSearchModel()
        {
            mockTechnicalSpecialistEducationalQualificationRepository.Setup(x => x.Search(It.IsAny<DomModel.TechnicalSpecialistEducationalQualification>())).Returns(TechnicalSpecialistEducationalQualificationDomainModels);
            TechnicalSpecialistEducationalQualificationService = new Service.TechnicalSpecialistEducationalQualificationService(mockTechnicalSpecialistEducationalQualificationRepository.Object, mockLogger.Object, mockTechnicalSpecialistRepository.Object, cityRepository.Object, countyRepository.Object, countryRepository.Object, mockMapper.Object, validationService);
            var response = TechnicalSpecialistEducationalQualificationService.GetEducationalQualification(null);
            Assert.AreEqual("1", response.Code);
            Assert.AreEqual(2, (response.Result as List<DomModel.TechnicalSpecialistEducationalQualification>).Count);
        }

        [TestMethod]
        public void ThrowsExceptionWhileFetchingEducationalQualification()
        {
            mockTechnicalSpecialistEducationalQualificationRepository.Setup(x => x.Search(It.IsAny<DomModel.TechnicalSpecialistEducationalQualification>())).Throws(new Exception("Exception occured while performing some operation."));
            TechnicalSpecialistEducationalQualificationService = new Service.TechnicalSpecialistEducationalQualificationService(mockTechnicalSpecialistEducationalQualificationRepository.Object, mockLogger.Object, mockTechnicalSpecialistRepository.Object, cityRepository.Object, countyRepository.Object, countryRepository.Object, mockMapper.Object, validationService);
            var response = TechnicalSpecialistEducationalQualificationService.GetEducationalQualification(new DomModel.TechnicalSpecialistEducationalQualification());
            Assert.AreEqual("11", response.Code);
            Assert.IsNotNull(response.Messages);
            Assert.AreEqual("31", response.Messages.FirstOrDefault().Code);
        }

    }
}
