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
    public class TechnicalSpecialistWorkHistoryServiceUnitTestCase : BaseTestCase
    {
        IValidationService Validation = null;
        ServiceDomainData.ITechnicalSpecialistWorkHistoryService TechnicalSpecialistWorkHistoryService = null;
        Mock<DomainData.ITechnicalSpecialistWorkHistoryRepository> mockTechnicalSpecialistWorkHistoryRepository = null;
        Mock<DomainData.ITechnicalSpecialistRepository> mockTechnicalSpecialistRepository = null;
        IList<DomModel.TechnicalSpecialistWorkHistory> TechnicalSpecialistWorkHistoryDomainModels = null;
        Mock<IAppLogger<Service.TechnicalSpecialistWorkHistoryService>> mockLogger = null;
        Mock<IMapper> mockMapper = null;
        IQueryable<DbModel.TechnicalSpecialist> mockTechnicalSpecialistDbData = null;
        IQueryable<DbModel.TechnicalSpecialistWorkHistory> mockTechnicalSpecialistWorkHistoryDbData = null;
        IQueryable<DbModel.Data> MockLanguage = null;
        ValdService.ITechnicalSpecialistWorkHistoryValidationService validationService = null;
        Mock<IDataRepository> dataRepository = null;
        // IQueryable<DbModel.TechnicalSpecialist> mockTechnicalSpecialistDbData = null;


        [TestInitialize]
        public void IntializeTechnicalSpecialistWorkHistory()
        {
            mockTechnicalSpecialistWorkHistoryRepository = new Mock<DomainData.ITechnicalSpecialistWorkHistoryRepository>();
            mockTechnicalSpecialistRepository = new Mock<DomainData.ITechnicalSpecialistRepository>();
            TechnicalSpecialistWorkHistoryDomainModels = MockTechnicalSpecialistModel.GetWorkHistoryMockedDomainData();
            mockLogger = new Mock<IAppLogger<Service.TechnicalSpecialistWorkHistoryService>>();
            mockMapper = new Mock<IMapper>();
            mockTechnicalSpecialistWorkHistoryDbData = MockTechnicalSpecialist.GetWorkHistoryMockData();
            dataRepository = new Mock<IDataRepository>();
            mockTechnicalSpecialistDbData = MockTechnicalSpecialist.GetTechnicalSpecialistMockData();
            //  mockTechnicalSpecialistDbData = MockTechnicalSpecialist.GetWorkHistoryMockData();
        }
        [TestMethod]
        public void FetchAllWorkHistoryWithSearchValue()
        {
            mockTechnicalSpecialistWorkHistoryRepository.Setup(x => x.Search(It.Is<DomModel.TechnicalSpecialistWorkHistory>(c => c.Epin == 54))).Returns(TechnicalSpecialistWorkHistoryDomainModels);
            TechnicalSpecialistWorkHistoryService = new Service.TechnicalSpecialistWorkHistoryService(mockTechnicalSpecialistWorkHistoryRepository.Object, mockMapper.Object, mockLogger.Object, mockTechnicalSpecialistRepository.Object, validationService); var response = TechnicalSpecialistWorkHistoryService.GetTechnicalSpecialistWorkHistory(new DomModel.TechnicalSpecialistWorkHistory { Epin = 54 });
            Assert.AreEqual("1", response.Code);
            Assert.AreEqual(2, (response.Result as List<DomModel.TechnicalSpecialistWorkHistory>).Count);
        }

        [TestMethod]
        public void FetchAllWorkHistoryWithNullSearchModel()
        {
            mockTechnicalSpecialistWorkHistoryRepository.Setup(x => x.Search(It.IsAny<DomModel.TechnicalSpecialistWorkHistory>())).Returns(TechnicalSpecialistWorkHistoryDomainModels);
            TechnicalSpecialistWorkHistoryService = new Service.TechnicalSpecialistWorkHistoryService(mockTechnicalSpecialistWorkHistoryRepository.Object, mockMapper.Object, mockLogger.Object, mockTechnicalSpecialistRepository.Object, validationService); var response = TechnicalSpecialistWorkHistoryService.GetTechnicalSpecialistWorkHistory(new DomModel.TechnicalSpecialistWorkHistory { Epin = 54 });
            Assert.AreEqual("1", response.Code);
            Assert.AreEqual(2, (response.Result as List<DomModel.TechnicalSpecialistWorkHistory>).Count);
        }

        [TestMethod]
        public void ThrowsExceptionWhileFetchingWorkHistory()
        {
            mockTechnicalSpecialistWorkHistoryRepository.Setup(x => x.Search(It.IsAny<DomModel.TechnicalSpecialistWorkHistory>())).Throws(new Exception("Exception occured while performing some operation."));
            TechnicalSpecialistWorkHistoryService = new Service.TechnicalSpecialistWorkHistoryService(mockTechnicalSpecialistWorkHistoryRepository.Object, mockMapper.Object, mockLogger.Object, mockTechnicalSpecialistRepository.Object, validationService); var response = TechnicalSpecialistWorkHistoryService.GetTechnicalSpecialistWorkHistory(new DomModel.TechnicalSpecialistWorkHistory { Epin = 54 });

            Assert.AreEqual("11", response.Code);
            Assert.IsNotNull(response.Messages);
            Assert.AreEqual("31", response.Messages.FirstOrDefault().Code);
        }
        [TestMethod]
        public void SaveTechnicalSpecialistWorkHistory()
        {
            mockTechnicalSpecialistRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.TechnicalSpecialist, bool>>>())).Returns(mockTechnicalSpecialistDbData);
            dataRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Data, bool>>>())).Returns((Expression<Func<DbModel.Data, bool>> predicate) => MockLanguage.Where(predicate));
            mockTechnicalSpecialistWorkHistoryRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.TechnicalSpecialistWorkHistory, bool>>>())).Returns((Expression<Func<DbModel.TechnicalSpecialistWorkHistory, bool>> predicate) => mockTechnicalSpecialistWorkHistoryDbData.Where(predicate));
            Validation = new Evolution.ValidationService.Services.ValidationService();
            validationService = new Evolution.TechnicalSpecialist.Infrastructure.Validations.TechSpecialistWorkHistoryValidationService(Validation);
            TechnicalSpecialistWorkHistoryService = new Service.TechnicalSpecialistWorkHistoryService(mockTechnicalSpecialistWorkHistoryRepository.Object, mockMapper.Object, mockLogger.Object, mockTechnicalSpecialistRepository.Object, validationService);
            TechnicalSpecialistWorkHistoryDomainModels.ToList().ForEach(x => x.RecordStatus = "N");
            var response = TechnicalSpecialistWorkHistoryService.SaveTechnicalSpecialistWorkHistory((int)TechnicalSpecialistWorkHistoryDomainModels[0].Epin, new List<DomModel.TechnicalSpecialistWorkHistory> { TechnicalSpecialistWorkHistoryDomainModels[0] });
            mockTechnicalSpecialistWorkHistoryRepository.Verify(m => m.Add(It.IsAny<IList<DbModel.TechnicalSpecialistWorkHistory>>()), Times.AtLeastOnce);
        }
        [TestMethod]
        public void ModifyTechnicalSpecialistWorkHistory()
        {
            mockTechnicalSpecialistRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.TechnicalSpecialist, bool>>>())).Returns(mockTechnicalSpecialistDbData);
            dataRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Data, bool>>>())).Returns((Expression<Func<DbModel.Data, bool>> predicate) => MockLanguage.Where(predicate));
            mockTechnicalSpecialistWorkHistoryRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.TechnicalSpecialistWorkHistory, bool>>>())).Returns((Expression<Func<DbModel.TechnicalSpecialistWorkHistory, bool>> predicate) => mockTechnicalSpecialistWorkHistoryDbData.Where(predicate));
            Validation = new Evolution.ValidationService.Services.ValidationService();
            validationService = new Evolution.TechnicalSpecialist.Infrastructure.Validations.TechSpecialistWorkHistoryValidationService(Validation);
            TechnicalSpecialistWorkHistoryService = new Service.TechnicalSpecialistWorkHistoryService(mockTechnicalSpecialistWorkHistoryRepository.Object, mockMapper.Object, mockLogger.Object, mockTechnicalSpecialistRepository.Object, validationService);
            TechnicalSpecialistWorkHistoryDomainModels.ToList().ForEach(x => x.RecordStatus = "M");
            var response = TechnicalSpecialistWorkHistoryService.ModifyWorkHistoryInfo((int)TechnicalSpecialistWorkHistoryDomainModels[0].Epin, new List<DomModel.TechnicalSpecialistWorkHistory> { TechnicalSpecialistWorkHistoryDomainModels[0] });
            //mockTechnicalSpecialistWorkHistoryRepository.Verify(m => m.Update(It.IsAny<DbModel.TechnicalSpecialistWorkHistory>()), Times.AtLeastOnce);
            Assert.AreEqual("1", response.Code);
            mockTechnicalSpecialistWorkHistoryRepository.Verify(m => m.ForceSave(), Times.Once);
        }
        [TestMethod]
        public void DeleteTechnicalSpecialistWorkHistory()
        {

            mockTechnicalSpecialistRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.TechnicalSpecialist, bool>>>())).Returns(mockTechnicalSpecialistDbData);
            dataRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Data, bool>>>())).Returns((Expression<Func<DbModel.Data, bool>> predicate) => MockLanguage.Where(predicate));
            mockTechnicalSpecialistWorkHistoryRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.TechnicalSpecialistWorkHistory, bool>>>())).Returns((Expression<Func<DbModel.TechnicalSpecialistWorkHistory, bool>> predicate) => mockTechnicalSpecialistWorkHistoryDbData.Where(predicate));
            Validation = new Evolution.ValidationService.Services.ValidationService();
            validationService = new Evolution.TechnicalSpecialist.Infrastructure.Validations.TechSpecialistWorkHistoryValidationService(Validation);
            TechnicalSpecialistWorkHistoryService = new Service.TechnicalSpecialistWorkHistoryService(mockTechnicalSpecialistWorkHistoryRepository.Object, mockMapper.Object, mockLogger.Object, mockTechnicalSpecialistRepository.Object, validationService);
            TechnicalSpecialistWorkHistoryDomainModels.ToList().ForEach(x => x.RecordStatus = "D");
            var response = TechnicalSpecialistWorkHistoryService.DeleteWorkHistoryInfo((int)TechnicalSpecialistWorkHistoryDomainModels[0].Epin, new List<DomModel.TechnicalSpecialistWorkHistory> { TechnicalSpecialistWorkHistoryDomainModels[0] });
           // mockTechnicalSpecialistWorkHistoryRepository.Verify(m => m.Delete(It.IsAny<DbModel.TechnicalSpecialistWorkHistory>()), Times.AtLeastOnce);
            Assert.AreEqual("1", response.Code);
            mockTechnicalSpecialistWorkHistoryRepository.Verify(m => m.ForceSave(), Times.Once);
        }

    }

}
