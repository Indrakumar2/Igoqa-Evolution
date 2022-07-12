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
    public class TechnicalSpecialistCompetencyUnitTestCase : BaseTestCase
    {
        IValidationService Validation = null;
        ServiceDomainData.ITechnicalSpecialistTrainingAndCompetencyService TechnicalSpecialistCompetencyService = null;
        Mock<DomainData.ITechnicalSpecialistTrainingAndCompetencyRepository> mockTechnicalSpecialistCompetencyRepository = null;
        Mock<DomainData.ITechnicalSpecialistRepository> mockTechnicalSpecialistRepository = null;
        IList<DomModel.TechnicalSpecialistCompetency> TechnicalSpecialistCompetencyDomainModels = null;
        Mock<IAppLogger<Service.TechnicalSpecialistTrainingAndCompetencyService>> mockLogger = null;
        Mock<IMapper> mockMapper = null;
        IQueryable<DbModel.TechnicalSpecialist> mockTechnicalSpecialistDbData = null;
        IQueryable<DbModel.TechnicalSpecialistTrainingAndCompetency> mockTechnicalSpecialistCompetencyDbData = null;
        IQueryable<DbModel.Data> MockCodestandard = null;
        ValdService.ITechnicalSpecialistCompetencyValidationService validationService = null;
        ValdService.ITechnicalSpecialistTrainingValidationService trainingValidationService = null;
        ValdService.ITechnicalSpecialistTrainingAndCompetencyValidationService TechnicalSpecialistTrainingAndCompetencyValidationService = null;
        Mock<IDataRepository> dataRepository = null;

        [TestInitialize]
        public void IntializeTechnicalSpecialistCodeAndStandard()
        {
            mockTechnicalSpecialistCompetencyRepository = new Mock<DomainData.ITechnicalSpecialistTrainingAndCompetencyRepository>();
            mockTechnicalSpecialistRepository = new Mock<DomainData.ITechnicalSpecialistRepository>();
            TechnicalSpecialistCompetencyDomainModels = MockTechnicalSpecialistModel.GetCompetencyMockedDomainData();
            mockLogger = new Mock<IAppLogger<Service.TechnicalSpecialistTrainingAndCompetencyService>>();
            mockMapper = new Mock<IMapper>();
            mockTechnicalSpecialistCompetencyDbData = MockTechnicalSpecialist.GetTechnicalSpecialistCompetencyMockData();
            dataRepository = new Mock<IDataRepository>();
            Validation = new Evolution.ValidationService.Services.ValidationService();

            mockTechnicalSpecialistDbData = MockTechnicalSpecialist.GetTechnicalSpecialistMockData();
        }
        [TestMethod]
        public void FetchAllCompetencySearchValue()
        {
            string recordType = "Competency";
            mockTechnicalSpecialistCompetencyRepository.Setup(x => x.SearchCompetency(It.Is<DomModel.TechnicalSpecialistCompetency>(c => c.Epin == 54), recordType)).Returns(TechnicalSpecialistCompetencyDomainModels);
            TechnicalSpecialistCompetencyService = new Service.TechnicalSpecialistTrainingAndCompetencyService(
            mockTechnicalSpecialistCompetencyRepository.Object, TechnicalSpecialistTrainingAndCompetencyValidationService, mockLogger.Object, dataRepository.Object, mockTechnicalSpecialistRepository.Object, validationService);
            var response = TechnicalSpecialistCompetencyService.GetTechnicalSpecialistCompetency(new DomModel.TechnicalSpecialistCompetency { Epin = 54 }, recordType);
            Assert.AreEqual("1", response.Code);
            Assert.AreEqual(2, (response.Result as List<DomModel.TechnicalSpecialistCompetency>).Count);

        }
        [TestMethod]
        public void ThrowsExceptionWhileFetchingCompetency()
        {
            string recordType = "Competency";
            mockTechnicalSpecialistCompetencyRepository.Setup(x => x.SearchCompetency(It.Is<DomModel.TechnicalSpecialistCompetency>(c => c.Epin == 54), recordType)).Throws(new Exception("Exception occured while performing some operation."));
            TechnicalSpecialistCompetencyService = new Service.TechnicalSpecialistTrainingAndCompetencyService(
            mockTechnicalSpecialistCompetencyRepository.Object, TechnicalSpecialistTrainingAndCompetencyValidationService, mockLogger.Object, dataRepository.Object, mockTechnicalSpecialistRepository.Object, validationService);
            var response = TechnicalSpecialistCompetencyService.GetTechnicalSpecialistCompetency(new DomModel.TechnicalSpecialistCompetency { Epin = 54 }, recordType);
            Assert.AreEqual("11", response.Code);
            Assert.IsNotNull(response.Messages);
            Assert.AreEqual("31", response.Messages.FirstOrDefault().Code);

        }
        [TestMethod]
        public void SaveTechnicalSpecialistCompetency()
        {
            string recordType = "Competency";
            mockTechnicalSpecialistRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.TechnicalSpecialist, bool>>>())).Returns(mockTechnicalSpecialistDbData);
            dataRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Data, bool>>>())).Returns((Expression<Func<DbModel.Data, bool>> predicate) => MockCodestandard.Where(predicate));
            mockTechnicalSpecialistCompetencyRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.TechnicalSpecialistTrainingAndCompetency, bool>>>())).Returns((Expression<Func<DbModel.TechnicalSpecialistTrainingAndCompetency, bool>> predicate) => mockTechnicalSpecialistCompetencyDbData.Where(predicate));
            Validation = new Evolution.ValidationService.Services.ValidationService();
            validationService = new Evolution.TechnicalSpecialist.Infrastructure.Validations.TechnicalSpecialistCompetencyValidationService(Validation);
            TechnicalSpecialistCompetencyService = new Service.TechnicalSpecialistTrainingAndCompetencyService(mockTechnicalSpecialistCompetencyRepository.Object, TechnicalSpecialistTrainingAndCompetencyValidationService, mockLogger.Object,dataRepository.Object,mockTechnicalSpecialistRepository.Object,validationService);
            TechnicalSpecialistCompetencyDomainModels.ToList().ForEach(x => x.RecordStatus = "N");
            var response = TechnicalSpecialistCompetencyService.SaveTechnicalSpecialistCompetency((int)TechnicalSpecialistCompetencyDomainModels[0].Epin, new List<DomModel.TechnicalSpecialistCompetency> { TechnicalSpecialistCompetencyDomainModels[0] },recordType);
            mockTechnicalSpecialistCompetencyRepository.Verify(m => m.Add(It.IsAny<IList<DbModel.TechnicalSpecialistTrainingAndCompetency>>()), Times.AtLeastOnce);
        }
        [TestMethod]
        public void UpdateTechnicalSpecialistCompetency()
        {
            string recordType = "Competency";
            mockTechnicalSpecialistRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.TechnicalSpecialist, bool>>>())).Returns(mockTechnicalSpecialistDbData);
            dataRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Data, bool>>>())).Returns((Expression<Func<DbModel.Data, bool>> predicate) => MockCodestandard.Where(predicate));
            mockTechnicalSpecialistCompetencyRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.TechnicalSpecialistTrainingAndCompetency, bool>>>())).Returns((Expression<Func<DbModel.TechnicalSpecialistTrainingAndCompetency, bool>> predicate) => mockTechnicalSpecialistCompetencyDbData.Where(predicate));
            Validation = new Evolution.ValidationService.Services.ValidationService();
            validationService = new Evolution.TechnicalSpecialist.Infrastructure.Validations.TechnicalSpecialistCompetencyValidationService(Validation);
            TechnicalSpecialistCompetencyService = new Service.TechnicalSpecialistTrainingAndCompetencyService(mockTechnicalSpecialistCompetencyRepository.Object, TechnicalSpecialistTrainingAndCompetencyValidationService, mockLogger.Object, dataRepository.Object, mockTechnicalSpecialistRepository.Object, validationService);
            TechnicalSpecialistCompetencyDomainModels.ToList().ForEach(x => x.RecordStatus = "M");
            var response = TechnicalSpecialistCompetencyService.ModifyTechnicalSpecialistICompetency((int)TechnicalSpecialistCompetencyDomainModels[0].Epin, new List<DomModel.TechnicalSpecialistCompetency> { TechnicalSpecialistCompetencyDomainModels[0] }, recordType);
            Assert.AreEqual("1", response.Code);
            mockTechnicalSpecialistCompetencyRepository.Verify(m => m.ForceSave(), Times.Once);
        }
        [TestMethod]
        public void DeleteTechnicalSpecialistCompetency()
        {
            string recordType = "Competency";
            mockTechnicalSpecialistRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.TechnicalSpecialist, bool>>>())).Returns(mockTechnicalSpecialistDbData);
            dataRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Data, bool>>>())).Returns((Expression<Func<DbModel.Data, bool>> predicate) => MockCodestandard.Where(predicate));
            mockTechnicalSpecialistCompetencyRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.TechnicalSpecialistTrainingAndCompetency, bool>>>())).Returns((Expression<Func<DbModel.TechnicalSpecialistTrainingAndCompetency, bool>> predicate) => mockTechnicalSpecialistCompetencyDbData.Where(predicate));
            Validation = new Evolution.ValidationService.Services.ValidationService();
            validationService = new Evolution.TechnicalSpecialist.Infrastructure.Validations.TechnicalSpecialistCompetencyValidationService(Validation);
            TechnicalSpecialistCompetencyService = new Service.TechnicalSpecialistTrainingAndCompetencyService(mockTechnicalSpecialistCompetencyRepository.Object, TechnicalSpecialistTrainingAndCompetencyValidationService, mockLogger.Object, dataRepository.Object, mockTechnicalSpecialistRepository.Object, validationService);
            TechnicalSpecialistCompetencyDomainModels.ToList().ForEach(x => x.RecordStatus = "D");
            var response = TechnicalSpecialistCompetencyService.DeleteTechnicalSpecialistICompetency((int)TechnicalSpecialistCompetencyDomainModels[0].Epin, new List<DomModel.TechnicalSpecialistCompetency> { TechnicalSpecialistCompetencyDomainModels[0] }, recordType);
            Assert.AreEqual("1", response.Code);
            mockTechnicalSpecialistCompetencyRepository.Verify(m => m.ForceSave(), Times.Once);
        }

    }
}
