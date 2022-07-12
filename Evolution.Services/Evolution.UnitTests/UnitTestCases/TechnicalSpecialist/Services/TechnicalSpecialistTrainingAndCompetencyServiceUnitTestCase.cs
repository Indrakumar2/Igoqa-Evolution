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
using Evolution.TechnicalSpecialist.Domain.Interfaces.Validations;

namespace Evolution.UnitTests.UnitTestCases.TechnicalSpecialist.Services
{
   [TestClass]
   public class TechnicalSpecialistTrainingAndCompetencyServiceUnitTestCase: BaseTestCase
    {
        IValidationService Validation = null;
        ServiceDomainData.ITechnicalSpecialistTrainingAndCompetencyService TechnicalSpecialistTrainingAndCompetencyService = null;
        Mock<DomainData.ITechnicalSpecialistTrainingAndCompetencyRepository> mockTechnicalSpecialistTrainingAndCompetencyRepository = null;
        Mock<DomainData.ITechnicalSpecialistRepository> mockTechnicalSpecialistRepository = null;
        IList<DomModel.TechnicalSpecialistCompetency> TechnicalSpecialistCompetencyDomainModels = null;
        Mock<IAppLogger<Service.TechnicalSpecialistTrainingAndCompetencyService>> mockLogger = null;
        Mock<IMapper> mockMapper = null;
        IQueryable<DbModel.TechnicalSpecialist> mockTechnicalSpecialistDbData = null;
        IQueryable<DbModel.TechnicalSpecialistTrainingAndCompetency> mockTechnicalSpecialistTrainingAndCompetencyDbData = null;
        IQueryable<DbModel.Data> MockLanguage = null;
        ValdService.ITechnicalSpecialistTrainingAndCompetencyValidationService validationService = null;
        Mock<IDataRepository> dataRepository = null;
        Mock<ITechnicalSpecialistCompetencyValidationService> mockTechnicalSpecialistCompetencyvalidationService = null;
        string recordType = "";


        [TestInitialize]
        public void IntializeTechnicalSpecialistTrainingAndCompetency()
        {
            mockTechnicalSpecialistTrainingAndCompetencyRepository = new Mock<DomainData.ITechnicalSpecialistTrainingAndCompetencyRepository>();
            mockTechnicalSpecialistRepository = new Mock<DomainData.ITechnicalSpecialistRepository>();
           // TechnicalSpecialistCompetencyDomainModels = MockTechnicalSpecialistModel.GetTrainingAndCompetencyMockedDomainData();
            mockLogger = new Mock<IAppLogger<Service.TechnicalSpecialistTrainingAndCompetencyService>>();
            mockMapper = new Mock<IMapper>();
           // mockTechnicalSpecialistTrainingAndCompetencyDbData = MockTechnicalSpecialist.GetTrainingAndCompetencyMockData();
            //dataRepository = new Mock<IDataRepository>();
            //MockLanguage = MockMaster.GetLanguageData();
            mockTechnicalSpecialistDbData = MockTechnicalSpecialist.GetTechnicalSpecialistMockData();
        }
        [TestMethod]
        public void FetchAllTrainingAndCompetencyWithSearchValue()
        {
            mockTechnicalSpecialistTrainingAndCompetencyRepository.Setup(x => x.SearchCompetency(It.Is<DomModel.TechnicalSpecialistCompetency>(c => c.Epin == 54), recordType)).Returns(TechnicalSpecialistCompetencyDomainModels);
            TechnicalSpecialistTrainingAndCompetencyService = new Service.TechnicalSpecialistTrainingAndCompetencyService(mockTechnicalSpecialistTrainingAndCompetencyRepository.Object, validationService, mockLogger.Object, dataRepository.Object, mockTechnicalSpecialistRepository.Object, mockTechnicalSpecialistCompetencyvalidationService.Object);
            var response = TechnicalSpecialistTrainingAndCompetencyService.GetTechnicalSpecialistCompetency(new DomModel.TechnicalSpecialistCompetency{ Epin = 54 },recordType);

            Assert.AreEqual("1", response.Code);
            Assert.AreEqual(2, (response.Result as List<DomModel.TechnicalSpecialistCompetency>).Count);
        }

    }
}
