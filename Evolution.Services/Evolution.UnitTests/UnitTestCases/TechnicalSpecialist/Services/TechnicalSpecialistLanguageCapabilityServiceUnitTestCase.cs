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
    public class TechnicalSpecialistLanguageCapabilityServiceUnitTestCase
    {
        IValidationService Validation = null;
        ServiceDomainData.ITechnicalSpecialistLanguageCapabilityService TechnicalSpecialistLanguageCapabilityService = null;
        Mock<DomainData.ITechnicalSpecialistLanguageCapabilityRepository> mockTechnicalSpecialistLanguageCapabilityRepository = null;
        Mock<DomainData.ITechnicalSpecialistRepository> mockTechnicalSpecialistRepository = null;       
        IList<DomModel.TechnicalSpecialistLanguageCapability> TechnicalSpecialistLanguageCapabilityDomainModels = null;
        Mock<IAppLogger<Service.TechnicalSpecialistLanguageCapabilityService>> mockLogger = null;
        Mock<IMapper> mockMapper = null;
        IQueryable<DbModel.TechnicalSpecialist> mockTechnicalSpecialistDbData = null;
        IQueryable<DbModel.TechnicalSpecialistLanguageCapability> mockTechnicalSpecialistLanguageCapabilityDbData = null;
        IQueryable<DbModel.Data> MockLanguage = null;
        ValdService.ITechnicalSpecialistLanguageCapabilityValidationService validationService = null;
        Mock<IDataRepository> dataRepository = null;


        [TestInitialize]
        public void IntializeTechnicalSpecialistLanguageCapability()
        {
            mockTechnicalSpecialistLanguageCapabilityRepository = new Mock<DomainData.ITechnicalSpecialistLanguageCapabilityRepository>();
            mockTechnicalSpecialistRepository = new Mock<DomainData.ITechnicalSpecialistRepository>();
            TechnicalSpecialistLanguageCapabilityDomainModels = MockTechnicalSpecialistModel.GetLanguageCapabilityMockedDomainData();
            mockLogger = new Mock<IAppLogger<Service.TechnicalSpecialistLanguageCapabilityService>>();
            mockMapper = new Mock<IMapper>();
            mockTechnicalSpecialistLanguageCapabilityDbData = MockTechnicalSpecialist.GetLanguageCapabilityMockData();
            dataRepository = new Mock<IDataRepository>();
            MockLanguage = MockMaster.GetLanguageData();           
            mockTechnicalSpecialistDbData = MockTechnicalSpecialist.GetTechnicalSpecialistMockData();
        }

        [TestMethod]
        public void SaveTechnicalSpecialistLanguageCapability()
        {

            mockTechnicalSpecialistRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.TechnicalSpecialist, bool>>>())).Returns(mockTechnicalSpecialistDbData);
           // mockTechnicalSpecialistRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.TechnicalSpecialist, bool>>>())).Returns(mockTechnicalSpecialistDbData);
            dataRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Data, bool>>>())).Returns((Expression<Func<DbModel.Data, bool>> predicate) => MockLanguage.Where(predicate));           
            mockTechnicalSpecialistLanguageCapabilityRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.TechnicalSpecialistLanguageCapability, bool>>>())).Returns((Expression<Func<DbModel.TechnicalSpecialistLanguageCapability, bool>> predicate) => mockTechnicalSpecialistLanguageCapabilityDbData.Where(predicate));           
            Validation = new Evolution.ValidationService.Services.ValidationService();
            validationService = new Evolution.TechnicalSpecialist.Infrastructure.Validations.TechnicalSpecialistLanguageCapabilityValidationService(Validation);
            TechnicalSpecialistLanguageCapabilityService = new Service.TechnicalSpecialistLanguageCapabilityService(mockTechnicalSpecialistLanguageCapabilityRepository.Object, mockLogger.Object, mockTechnicalSpecialistRepository.Object, dataRepository.Object, validationService);
            TechnicalSpecialistLanguageCapabilityDomainModels.ToList().ForEach(x => x.RecordStatus = "N");
            var response = TechnicalSpecialistLanguageCapabilityService.SaveLanguageCapabilities((int)TechnicalSpecialistLanguageCapabilityDomainModels[0].Epin, new List<DomModel.TechnicalSpecialistLanguageCapability> { TechnicalSpecialistLanguageCapabilityDomainModels[0] });
            mockTechnicalSpecialistLanguageCapabilityRepository.Verify(m => m.Add(It.IsAny<IList<DbModel.TechnicalSpecialistLanguageCapability>>()), Times.AtLeastOnce);
        }

        [TestMethod]
        public void ModifyTechnicalSpecialistLanguageCapability()
        {

            mockTechnicalSpecialistRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.TechnicalSpecialist, bool>>>())).Returns(mockTechnicalSpecialistDbData);
            dataRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Data, bool>>>())).Returns((Expression<Func<DbModel.Data, bool>> predicate) => MockLanguage.Where(predicate));
            mockTechnicalSpecialistLanguageCapabilityRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.TechnicalSpecialistLanguageCapability, bool>>>())).Returns((Expression<Func<DbModel.TechnicalSpecialistLanguageCapability, bool>> predicate) => mockTechnicalSpecialistLanguageCapabilityDbData.Where(predicate));
            Validation = new Evolution.ValidationService.Services.ValidationService();
            validationService = new Evolution.TechnicalSpecialist.Infrastructure.Validations.TechnicalSpecialistLanguageCapabilityValidationService(Validation);
            TechnicalSpecialistLanguageCapabilityService = new Service.TechnicalSpecialistLanguageCapabilityService(mockTechnicalSpecialistLanguageCapabilityRepository.Object, mockLogger.Object, mockTechnicalSpecialistRepository.Object, dataRepository.Object, validationService);
            TechnicalSpecialistLanguageCapabilityDomainModels.ToList().ForEach(x => x.RecordStatus = "M");
            var response = TechnicalSpecialistLanguageCapabilityService.ModifyLanguageCapabilities((int)TechnicalSpecialistLanguageCapabilityDomainModels[0].Epin, new List<DomModel.TechnicalSpecialistLanguageCapability> { TechnicalSpecialistLanguageCapabilityDomainModels[0] });
            Assert.AreEqual("1", response.Code);
            mockTechnicalSpecialistLanguageCapabilityRepository.Verify(m => m.ForceSave(), Times.Once);
        }

        [TestMethod]
        public void DeleteTechnicalSpecialistLanguageCapability()
        {

            mockTechnicalSpecialistRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.TechnicalSpecialist, bool>>>())).Returns(mockTechnicalSpecialistDbData);            
            dataRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Data, bool>>>())).Returns((Expression<Func<DbModel.Data, bool>> predicate) => MockLanguage.Where(predicate));
            mockTechnicalSpecialistLanguageCapabilityRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.TechnicalSpecialistLanguageCapability, bool>>>())).Returns((Expression<Func<DbModel.TechnicalSpecialistLanguageCapability, bool>> predicate) => mockTechnicalSpecialistLanguageCapabilityDbData.Where(predicate));
            Validation = new Evolution.ValidationService.Services.ValidationService();
            validationService = new Evolution.TechnicalSpecialist.Infrastructure.Validations.TechnicalSpecialistLanguageCapabilityValidationService(Validation);
            TechnicalSpecialistLanguageCapabilityService = new Service.TechnicalSpecialistLanguageCapabilityService(mockTechnicalSpecialistLanguageCapabilityRepository.Object, mockLogger.Object, mockTechnicalSpecialistRepository.Object, dataRepository.Object, validationService);
            TechnicalSpecialistLanguageCapabilityDomainModels.ToList().ForEach(x => x.RecordStatus = "D");
            var response = TechnicalSpecialistLanguageCapabilityService.DeleteLanguageCapabilities((int)TechnicalSpecialistLanguageCapabilityDomainModels[0].Epin, new List<DomModel.TechnicalSpecialistLanguageCapability> { TechnicalSpecialistLanguageCapabilityDomainModels[0] });
            Assert.AreEqual("1", response.Code);
            mockTechnicalSpecialistLanguageCapabilityRepository.Verify(m => m.ForceSave(), Times.Once);
        }

        [TestMethod]
        public void FetchAllLanguageCapabilityWithSearchValue()
        {
            mockTechnicalSpecialistLanguageCapabilityRepository.Setup(x => x.Search(It.Is<DomModel.TechnicalSpecialistLanguageCapability>(c => c.Epin == 54))).Returns(TechnicalSpecialistLanguageCapabilityDomainModels);
            TechnicalSpecialistLanguageCapabilityService = new Service.TechnicalSpecialistLanguageCapabilityService(mockTechnicalSpecialistLanguageCapabilityRepository.Object, mockLogger.Object, mockTechnicalSpecialistRepository.Object, dataRepository.Object, validationService);
            var response = TechnicalSpecialistLanguageCapabilityService.GetLanguageCapabilities(new DomModel.TechnicalSpecialistLanguageCapability { Epin = 54 });


            Assert.AreEqual("1", response.Code);
            Assert.AreEqual(2, (response.Result as List<DomModel.TechnicalSpecialistLanguageCapability>).Count);
        }

        [TestMethod]
        public void FetchAllLanguageCapabilityWithoutSearchValue()
        {
            mockTechnicalSpecialistLanguageCapabilityRepository.Setup(x => x.Search(It.IsAny<DomModel.TechnicalSpecialistLanguageCapability>())).Returns(TechnicalSpecialistLanguageCapabilityDomainModels);
            TechnicalSpecialistLanguageCapabilityService = new Service.TechnicalSpecialistLanguageCapabilityService(mockTechnicalSpecialistLanguageCapabilityRepository.Object, mockLogger.Object, mockTechnicalSpecialistRepository.Object, dataRepository.Object, validationService);
            var response = TechnicalSpecialistLanguageCapabilityService.GetLanguageCapabilities(new DomModel.TechnicalSpecialistLanguageCapability());

            Assert.AreEqual("1", response.Code);
            Assert.AreEqual(2, (response.Result as List<DomModel.TechnicalSpecialistLanguageCapability>).Count);
        }

        [TestMethod]
        public void FetchAllLanguageCapabilityWithNullSearchModel()
        {
            mockTechnicalSpecialistLanguageCapabilityRepository.Setup(x => x.Search(It.IsAny<DomModel.TechnicalSpecialistLanguageCapability>())).Returns(TechnicalSpecialistLanguageCapabilityDomainModels);
            TechnicalSpecialistLanguageCapabilityService = new Service.TechnicalSpecialistLanguageCapabilityService(mockTechnicalSpecialistLanguageCapabilityRepository.Object, mockLogger.Object, mockTechnicalSpecialistRepository.Object, dataRepository.Object, validationService);
            var response = TechnicalSpecialistLanguageCapabilityService.GetLanguageCapabilities(null);


            Assert.AreEqual("1", response.Code);
            Assert.AreEqual(2, (response.Result as List<DomModel.TechnicalSpecialistLanguageCapability>).Count);
        }

        [TestMethod]
        public void ThrowsExceptionWhileFetchingLanguageCapability()
        {
            mockTechnicalSpecialistLanguageCapabilityRepository.Setup(x => x.Search(It.IsAny<DomModel.TechnicalSpecialistLanguageCapability>())).Throws(new Exception("Exception occured while performing some operation."));
            TechnicalSpecialistLanguageCapabilityService = new Service.TechnicalSpecialistLanguageCapabilityService(mockTechnicalSpecialistLanguageCapabilityRepository.Object, mockLogger.Object, mockTechnicalSpecialistRepository.Object, dataRepository.Object, validationService);

            var response = TechnicalSpecialistLanguageCapabilityService.GetLanguageCapabilities(new DomModel.TechnicalSpecialistLanguageCapability());

            Assert.AreEqual("11", response.Code);
            Assert.IsNotNull(response.Messages);
            Assert.AreEqual("31", response.Messages.FirstOrDefault().Code);
        }
    }
}
