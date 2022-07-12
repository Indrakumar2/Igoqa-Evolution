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
   public class TechnicalSpecialistContactServiceUnitTestCase:BaseTestCase
    {
        IValidationService Validation = null;
        ServiceDomainData.ITechnicalSpecialistContactService TechnicalSpecialistContactService = null;
        Mock<DomainData.ITechnicalSpecialistContactRepository> mockTechnicalSpecialistContactRepository = null;
        Mock<DomainData.ITechnicalSpecialistRepository> mockTechnicalSpecialistRepository = null;
        IList<DomModel.TechnicalSpecialistContact> TechnicalSpecialistContactDomainModels = null;
        Mock<IAppLogger<Service.TechnicalSpecialistContactService>> mockLogger = null;
        Mock<IMapper> mockMapper = null;
        IQueryable<DbModel.TechnicalSpecialist> mockTechnicalSpecialistDbData = null;
        IQueryable<DbModel.TechnicalSpecialistContact> mockTechnicalSpecialistContactDbData = null;
        IQueryable<DbModel.Country> MockCountry = null;
        IQueryable<DbModel.County> MockCounty = null;
        IQueryable<DbModel.City> MockCity = null;
        ValdService.ITechnicalSpecialistContactValidationService validationService = null;
        Mock<ICountryRepository> countryRepository = null;
        Mock<ICountyRepository> countyRepository = null;
        Mock<ICityRepository> cityRepository = null;

        [TestInitialize]
        public void IntializeTechnicalSpecialistContact()
        {
            mockTechnicalSpecialistContactRepository = new Mock<DomainData.ITechnicalSpecialistContactRepository>();
            mockTechnicalSpecialistRepository = new Mock<DomainData.ITechnicalSpecialistRepository>();
            TechnicalSpecialistContactDomainModels = MockTechnicalSpecialistModel.GetContactMockedDomainData();
            mockLogger = new Mock<IAppLogger<Service.TechnicalSpecialistContactService>>();
            mockMapper = new Mock<IMapper>();
            mockTechnicalSpecialistContactDbData = MockTechnicalSpecialist.GetContactMockData();
            countryRepository = new Mock<ICountryRepository>();
            MockCountry = MockMaster.GetCountryMockData();
            countyRepository = new Mock<ICountyRepository>();
            MockCounty = MockMaster.GetCountyMockData();
            cityRepository = new Mock<ICityRepository>();
            MockCity = MockMaster.GetCityMockData();
            mockTechnicalSpecialistDbData = MockTechnicalSpecialist.GetTechnicalSpecialistMockData();
        }

        [TestMethod]
        public void FetchAllContactWithSearchValue()
        {
            mockTechnicalSpecialistContactRepository.Setup(x => x.Search(It.Is<DomModel.TechnicalSpecialistContact>(c => c.Epin == 54))).Returns(TechnicalSpecialistContactDomainModels);
            TechnicalSpecialistContactService = new Service.TechnicalSpecialistContactService(mockTechnicalSpecialistContactRepository.Object, mockLogger.Object, mockTechnicalSpecialistRepository.Object, validationService, cityRepository.Object, countyRepository.Object, countryRepository.Object);
            var response = TechnicalSpecialistContactService.GetContactInfo(new DomModel.TechnicalSpecialistContact { Epin = 54 });
            Assert.AreEqual("1", response.Code);
            Assert.AreEqual(2, (response.Result as List<DomModel.TechnicalSpecialistContact>).Count);
        }

        [TestMethod]
        public void FetchAllContactWithoutSearchValue()
        {
            mockTechnicalSpecialistContactRepository.Setup(x => x.Search(It.IsAny<DomModel.TechnicalSpecialistContact>())).Returns(TechnicalSpecialistContactDomainModels);
            TechnicalSpecialistContactService = new Service.TechnicalSpecialistContactService(mockTechnicalSpecialistContactRepository.Object, mockLogger.Object, mockTechnicalSpecialistRepository.Object, validationService, cityRepository.Object, countyRepository.Object, countryRepository.Object);
            var response = TechnicalSpecialistContactService.GetContactInfo(new DomModel.TechnicalSpecialistContact());
            Assert.AreEqual("1", response.Code);
            Assert.AreEqual(2, (response.Result as List<DomModel.TechnicalSpecialistContact>).Count);
        }

        [TestMethod]
        public void FetchAllContactWithNullSearchModel()
        {
            mockTechnicalSpecialistContactRepository.Setup(x => x.Search(It.IsAny<DomModel.TechnicalSpecialistContact>())).Returns(TechnicalSpecialistContactDomainModels);
            TechnicalSpecialistContactService = new Service.TechnicalSpecialistContactService(mockTechnicalSpecialistContactRepository.Object, mockLogger.Object, mockTechnicalSpecialistRepository.Object, validationService, cityRepository.Object, countyRepository.Object, countryRepository.Object);
            var response = TechnicalSpecialistContactService.GetContactInfo(null);
            Assert.AreEqual("1", response.Code);
            Assert.AreEqual(2, (response.Result as List<DomModel.TechnicalSpecialistContact>).Count);
        }

        [TestMethod]
        public void ThrowsExceptionWhileFetchingContact()
        {
            mockTechnicalSpecialistContactRepository.Setup(x => x.Search(It.IsAny<DomModel.TechnicalSpecialistContact>())).Throws(new Exception("Exception occured while performing some operation."));
            TechnicalSpecialistContactService = new Service.TechnicalSpecialistContactService(mockTechnicalSpecialistContactRepository.Object, mockLogger.Object, mockTechnicalSpecialistRepository.Object, validationService, cityRepository.Object, countyRepository.Object, countryRepository.Object);
            var response = TechnicalSpecialistContactService.GetContactInfo(new DomModel.TechnicalSpecialistContact());
            Assert.AreEqual("11", response.Code);
            Assert.IsNotNull(response.Messages);
            Assert.AreEqual("31", response.Messages.FirstOrDefault().Code);
        }

        [TestMethod]
        public void SaveTechnicalSpecialistContact()
        {

            mockTechnicalSpecialistRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.TechnicalSpecialist, bool>>>())).Returns(mockTechnicalSpecialistDbData);
            countryRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Country, bool>>>())).Returns((Expression<Func<DbModel.Country, bool>> predicate) => MockCountry.Where(predicate));
            countyRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.County, bool>>>())).Returns((Expression<Func<DbModel.County, bool>> predicate) => MockCounty.Where(predicate));
            cityRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.City, bool>>>())).Returns((Expression<Func<DbModel.City, bool>> predicate) => MockCity.Where(predicate));
            mockTechnicalSpecialistContactRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.TechnicalSpecialistContact, bool>>>())).Returns((Expression<Func<DbModel.TechnicalSpecialistContact, bool>> predicate) => mockTechnicalSpecialistContactDbData.Where(predicate));
            Validation = new Evolution.ValidationService.Services.ValidationService();
            validationService = new Evolution.TechnicalSpecialist.Infrastructure.Validations.TechnicalSpecialistContactValidationService(Validation);
            TechnicalSpecialistContactService = new Service.TechnicalSpecialistContactService(mockTechnicalSpecialistContactRepository.Object, mockLogger.Object, mockTechnicalSpecialistRepository.Object, validationService, cityRepository.Object, countyRepository.Object, countryRepository.Object);
            TechnicalSpecialistContactDomainModels.ToList().ForEach(x => x.RecordStatus = "N");
            var response = TechnicalSpecialistContactService.SaveContactInfo((int)TechnicalSpecialistContactDomainModels[0].Epin, new List<DomModel.TechnicalSpecialistContact> { TechnicalSpecialistContactDomainModels[0] });
            mockTechnicalSpecialistContactRepository.Verify(m => m.Add(It.IsAny<IList<DbModel.TechnicalSpecialistContact>>()), Times.AtLeastOnce);
        }

        [TestMethod]
        public void ModifyTechnicalSpecialistContact()
        {

            mockTechnicalSpecialistRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.TechnicalSpecialist, bool>>>())).Returns(mockTechnicalSpecialistDbData);
            countryRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Country, bool>>>())).Returns((Expression<Func<DbModel.Country, bool>> predicate) => MockCountry.Where(predicate));
            countyRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.County, bool>>>())).Returns((Expression<Func<DbModel.County, bool>> predicate) => MockCounty.Where(predicate));
            cityRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.City, bool>>>())).Returns((Expression<Func<DbModel.City, bool>> predicate) => MockCity.Where(predicate));
            mockTechnicalSpecialistContactRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.TechnicalSpecialistContact, bool>>>())).Returns((Expression<Func<DbModel.TechnicalSpecialistContact, bool>> predicate) => mockTechnicalSpecialistContactDbData.Where(predicate));
            Validation = new Evolution.ValidationService.Services.ValidationService();
            validationService = new Evolution.TechnicalSpecialist.Infrastructure.Validations.TechnicalSpecialistContactValidationService(Validation);
            TechnicalSpecialistContactService = new Service.TechnicalSpecialistContactService(mockTechnicalSpecialistContactRepository.Object, mockLogger.Object, mockTechnicalSpecialistRepository.Object, validationService, cityRepository.Object, countyRepository.Object, countryRepository.Object);
            TechnicalSpecialistContactDomainModels.ToList().ForEach(x => x.RecordStatus = "M");
            var response = TechnicalSpecialistContactService.ModifyContactInfo((int)TechnicalSpecialistContactDomainModels[0].Epin, new List<DomModel.TechnicalSpecialistContact> { TechnicalSpecialistContactDomainModels[0] });
            Assert.AreEqual("1", response.Code);
            mockTechnicalSpecialistContactRepository.Verify(m => m.ForceSave(), Times.Once);
        }

        [TestMethod]
        public void DeleteTechnicalSpecialistContact()
        {

            mockTechnicalSpecialistRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.TechnicalSpecialist, bool>>>())).Returns(mockTechnicalSpecialistDbData);
            countryRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Country, bool>>>())).Returns((Expression<Func<DbModel.Country, bool>> predicate) => MockCountry.Where(predicate));
            countyRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.County, bool>>>())).Returns((Expression<Func<DbModel.County, bool>> predicate) => MockCounty.Where(predicate));
            cityRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.City, bool>>>())).Returns((Expression<Func<DbModel.City, bool>> predicate) => MockCity.Where(predicate));
            mockTechnicalSpecialistContactRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.TechnicalSpecialistContact, bool>>>())).Returns((Expression<Func<DbModel.TechnicalSpecialistContact, bool>> predicate) => mockTechnicalSpecialistContactDbData.Where(predicate));
            Validation = new Evolution.ValidationService.Services.ValidationService();
            validationService = new Evolution.TechnicalSpecialist.Infrastructure.Validations.TechnicalSpecialistContactValidationService(Validation);
            TechnicalSpecialistContactService = new Service.TechnicalSpecialistContactService(mockTechnicalSpecialistContactRepository.Object, mockLogger.Object, mockTechnicalSpecialistRepository.Object, validationService, cityRepository.Object, countyRepository.Object, countryRepository.Object);
            TechnicalSpecialistContactDomainModels.ToList().ForEach(x => x.RecordStatus = "D");
            var response = TechnicalSpecialistContactService.DeleteContactInfo((int)TechnicalSpecialistContactDomainModels[0].Epin, new List<DomModel.TechnicalSpecialistContact> { TechnicalSpecialistContactDomainModels[0] });
            Assert.AreEqual("1", response.Code);
            mockTechnicalSpecialistContactRepository.Verify(m => m.ForceSave(), Times.Once);
        }
    }
}
