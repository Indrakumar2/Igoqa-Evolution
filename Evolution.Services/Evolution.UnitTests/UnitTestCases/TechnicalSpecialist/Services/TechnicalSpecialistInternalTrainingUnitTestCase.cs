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
    public class TechnicalSpecialistInternalTrainingUnitTestCase: BaseTestCase
    {
        IValidationService Validation = null;
        ServiceDomainData.ITechnicalSpecialistTrainingAndCompetencyService TechnicalSpecialistCompetencyService = null;
        Mock<DomainData.ITechnicalSpecialistTrainingAndCompetencyRepository> mockTechnicalSpecialistCompetencyRepository = null;
        Mock<DomainData.ITechnicalSpecialistRepository> mockTechnicalSpecialistRepository = null;
        IList<DomModel.TechnicalSpecialistInternalTraining> TechnicalSpecialistInternalTrainingDomainModels = null;
        Mock<IAppLogger<Service.TechnicalSpecialistTrainingAndCompetencyService>> mockLogger = null;
        Mock<IMapper> mockMapper = null;
        IQueryable<DbModel.TechnicalSpecialist> mockTechnicalSpecialistDbData = null;
        IQueryable<DbModel.TechnicalSpecialistTrainingAndCompetency> mockTechnicalSpecialistintenalTraningDbData = null;
        IQueryable<DbModel.Data> MockCodestandard = null;
       ValdService.ITechnicalSpecialistTrainingAndCompetencyValidationService validationService = null;
        //ValdService.ITechnicalSpecialistTrainingValidationService trainingValidationService = null;
        ValdService.ITechnicalSpecialistCompetencyValidationService competencyvalidationService = null;
         Mock<IDataRepository> dataRepository = null;


       // private readonly ITechnicalSpecialistTrainingAndCompetencyValidationService _validationService = null;
       // private readonly ITechnicalSpecialistCompetencyValidationService _competencyvalidationService = null;

        [TestInitialize]
        public void IntializeTechnicalSpecialistCodeAndStandard()
        {
            mockTechnicalSpecialistCompetencyRepository = new Mock<DomainData.ITechnicalSpecialistTrainingAndCompetencyRepository>();
            mockTechnicalSpecialistRepository = new Mock<DomainData.ITechnicalSpecialistRepository>();
            TechnicalSpecialistInternalTrainingDomainModels = MockTechnicalSpecialistModel.GetInternalTrainingMockedDomainData();
            mockLogger = new Mock<IAppLogger<Service.TechnicalSpecialistTrainingAndCompetencyService>>();
            mockMapper = new Mock<IMapper>();
            mockTechnicalSpecialistintenalTraningDbData = MockTechnicalSpecialist.GetTechnicalSpecialistInternalTrainingMockData();
            dataRepository = new Mock<IDataRepository>();
            Validation = new Evolution.ValidationService.Services.ValidationService();
            mockTechnicalSpecialistDbData = MockTechnicalSpecialist.GetTechnicalSpecialistMockData();
        }
        [TestMethod]
        public void FetchAllInternalTrainingSearchValue()
        {
            string recordType = "InternalTraining";
            mockTechnicalSpecialistCompetencyRepository.Setup(x => x.SearchInternalTraining(It.Is<DomModel.TechnicalSpecialistInternalTraining>(c => c.Epin == 54), recordType)).Returns(TechnicalSpecialistInternalTrainingDomainModels);
            TechnicalSpecialistCompetencyService = new Service.TechnicalSpecialistTrainingAndCompetencyService(mockTechnicalSpecialistCompetencyRepository.Object, validationService, mockLogger.Object,dataRepository.Object,mockTechnicalSpecialistRepository.Object, competencyvalidationService);
            var response = TechnicalSpecialistCompetencyService.GetTechnicalSpecialistInternalTraining(new DomModel.TechnicalSpecialistInternalTraining { Epin = 54 }, recordType);
            Assert.AreEqual("1", response.Code);
            Assert.AreEqual(2, (response.Result as List<DomModel.TechnicalSpecialistInternalTraining>).Count);

        }
        [TestMethod]
        public void ThrowsExceptionWhileFetchingInternalTraining()
        {
            string recordType = "InternalTraining";
            mockTechnicalSpecialistCompetencyRepository.Setup(x => x.SearchInternalTraining(It.Is<DomModel.TechnicalSpecialistInternalTraining>(c => c.Epin == 54), recordType)).Throws(new Exception("Exception occured while performing some operation."));
            TechnicalSpecialistCompetencyService = new Service.TechnicalSpecialistTrainingAndCompetencyService(
            mockTechnicalSpecialistCompetencyRepository.Object,validationService, mockLogger.Object, dataRepository.Object, mockTechnicalSpecialistRepository.Object, competencyvalidationService);
            var response = TechnicalSpecialistCompetencyService.GetTechnicalSpecialistInternalTraining(new DomModel.TechnicalSpecialistInternalTraining { Epin = 54 }, recordType);
            Assert.AreEqual("11", response.Code);
            Assert.IsNotNull(response.Messages);
            Assert.AreEqual("31", response.Messages.FirstOrDefault().Code);

        }
        //[TestMethod]
        //public void SaveTechnicalSpecialistCompetency()
        //{
        //    string recordType = "InternalTraining";

        //    mockTechnicalSpecialistRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.TechnicalSpecialist, bool>>>())).Returns(mockTechnicalSpecialistDbData);
        //    dataRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Data, bool>>>())).Returns((Expression<Func<DbModel.Data, bool>> predicate) => MockCodestandard.Where(predicate));
        //    mockTechnicalSpecialistCompetencyRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.TechnicalSpecialistTrainingAndCompetency, bool>>>())).Returns((Expression<Func<DbModel.TechnicalSpecialistTrainingAndCompetency, bool>> predicate) => mockTechnicalSpecialistCompetencyDbData.Where(predicate));
        //    Validation = new Evolution.ValidationService.Services.ValidationService();
        //    validationService = new Evolution.TechnicalSpecialist.Infrastructure.Validations.(Validation);
        //    TechnicalSpecialistCompetencyService = new Service.TechnicalSpecialistTrainingAndCompetencyService(mockTechnicalSpecialistCompetencyRepository.Object, TechnicalSpecialistTrainingAndCompetencyValidationService, mockLogger.Object, dataRepository.Object, mockTechnicalSpecialistRepository.Object, validationService);
        //    TechnicalSpecialistCompetencyDomainModels.ToList().ForEach(x => x.RecordStatus = "N");
        //    var response = TechnicalSpecialistCompetencyService.SaveTechnicalSpecialistCompetency((int)TechnicalSpecialistCompetencyDomainModels[0].Epin, new List<DomModel.TechnicalSpecialistCompetency> { TechnicalSpecialistCompetencyDomainModels[0] }, recordType);
        //    mockTechnicalSpecialistCompetencyRepository.Verify(m => m.Add(It.IsAny<IList<DbModel.TechnicalSpecialistTrainingAndCompetency>>()), Times.AtLeastOnce);
        //}
    }
}
