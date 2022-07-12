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
    public class TechnicalSpecialistPayScheduleUnitTestCase : BaseTestCase
    {
        IValidationService Validation = null;
        ServiceDomainData.ITechnicalSpecialistPayScheduleService TechnicalSpecialistPayScheduleService = null;
        Mock<DomainData.ITechnicalSpecialistPayScheduleRepository> mockTechnicalSpecialistPayScheduleRepository = null;
        Mock<DomainData.ITechnicalSpecialistRepository> mockTechnicalSpecialistRepository = null;
        IList<DomModel.TechnicalSpecialistPaySchedule> TechnicalSpecialistPayScheduleDomainModels = null;
        Mock<IAppLogger<Service.TechnicalSpecialistPayScheduleService>> mockLogger = null;
        Mock<IMapper> mockMapper = null;
        IQueryable<DbModel.TechnicalSpecialist> mockTechnicalSpecialistDbData = null;
        IQueryable<DbModel.TechnicalSpecialistPaySchedule> mockTechnicalSpecialistPayScheduleDbData = null;
        IQueryable<DbModel.Data> MockCodestandard = null;
        ValdService.ITechnicalSpecialistPayScheduleValidationService validationService = null;
        Mock<IDataRepository> dataRepository = null;

        [TestInitialize]
        public void IntializeTechnicalSpecialistPaySchedule()
        {
            mockTechnicalSpecialistPayScheduleRepository = new Mock<DomainData.ITechnicalSpecialistPayScheduleRepository>();
            mockTechnicalSpecialistRepository = new Mock<DomainData.ITechnicalSpecialistRepository>();
            TechnicalSpecialistPayScheduleDomainModels = MockTechnicalSpecialistModel.GetTechnicalSpecialistPaySchduleMockedDomainData();
            mockLogger = new Mock<IAppLogger<Service.TechnicalSpecialistPayScheduleService>>();
            mockMapper = new Mock<IMapper>();
            mockTechnicalSpecialistPayScheduleDbData = MockTechnicalSpecialist.GetTechnicalSpecialistPayScheduleMockData();
            dataRepository = new Mock<IDataRepository>();
            //MockCodestandard = MockMaster.GetCodeStandardData();
            Validation = new Evolution.ValidationService.Services.ValidationService();
            mockTechnicalSpecialistDbData = MockTechnicalSpecialist.GetTechnicalSpecialistMockData();

        }
        [TestMethod]
        public void FetchAllPayScheduleSearchValue()
        {
            mockTechnicalSpecialistPayScheduleRepository.Setup(x => x.Search(It.Is<DomModel.TechnicalSpecialistPaySchedule>(c => c.Epin == 54))).Returns(TechnicalSpecialistPayScheduleDomainModels);
            TechnicalSpecialistPayScheduleService = new Service.TechnicalSpecialistPayScheduleService(mockTechnicalSpecialistPayScheduleRepository.Object,dataRepository.Object,mockLogger.Object,mockTechnicalSpecialistRepository.Object,validationService,mockMapper.Object);
            var response = TechnicalSpecialistPayScheduleService.GetTechnicalSpecialistPaySchdule(new DomModel.TechnicalSpecialistPaySchedule { Epin = 54 });
            Assert.AreEqual("1", response.Code);
            Assert.AreEqual(2, (response.Result as List<DomModel.TechnicalSpecialistPaySchedule>).Count);

        }
          [TestMethod]
        public void ThrowsExceptionWhileFetchingCodeAndStandard()
        {
            mockTechnicalSpecialistPayScheduleRepository.Setup(x => x.Search(It.IsAny<DomModel.TechnicalSpecialistPaySchedule>())).Throws(new Exception("Exception occured while performing some operation."));
            TechnicalSpecialistPayScheduleService = new Service.TechnicalSpecialistPayScheduleService(mockTechnicalSpecialistPayScheduleRepository.Object, dataRepository.Object, mockLogger.Object, mockTechnicalSpecialistRepository.Object, validationService, mockMapper.Object);
            var response = TechnicalSpecialistPayScheduleService.GetTechnicalSpecialistPaySchdule(new DomModel.TechnicalSpecialistPaySchedule { Epin = 54 });

            Assert.AreEqual("11", response.Code);
            Assert.IsNotNull(response.Messages);
            Assert.AreEqual("31", response.Messages.FirstOrDefault().Code);
        }
        [TestMethod]
        public void SaveTechnicalSpecialistCodeAndStandard()
        {
            mockTechnicalSpecialistRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.TechnicalSpecialist, bool>>>())).Returns(mockTechnicalSpecialistDbData);
            dataRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Data, bool>>>())).Returns((Expression<Func<DbModel.Data, bool>> predicate) => MockCodestandard.Where(predicate));
            mockTechnicalSpecialistPayScheduleRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.TechnicalSpecialistPaySchedule, bool>>>())).Returns((Expression<Func<DbModel.TechnicalSpecialistPaySchedule, bool>> predicate) => mockTechnicalSpecialistPayScheduleDbData.Where(predicate));
            Validation = new Evolution.ValidationService.Services.ValidationService();
            validationService = new Evolution.TechnicalSpecialist.Infrastructure.Validations.TechnicalSpecialistPayScheduleValidationService(Validation);
            TechnicalSpecialistPayScheduleService = new Service.TechnicalSpecialistPayScheduleService(mockTechnicalSpecialistPayScheduleRepository.Object, dataRepository.Object, mockLogger.Object, mockTechnicalSpecialistRepository.Object, validationService, mockMapper.Object);
            TechnicalSpecialistPayScheduleDomainModels.ToList().ForEach(x => x.RecordStatus = "N");
            var response = TechnicalSpecialistPayScheduleService.SaveTechnicalSpecialistPaySchdule((int)TechnicalSpecialistPayScheduleDomainModels[0].Epin, new List<DomModel.TechnicalSpecialistPaySchedule> { TechnicalSpecialistPayScheduleDomainModels[0] });
            mockTechnicalSpecialistPayScheduleRepository.Verify(m => m.Add(It.IsAny<IList<DbModel.TechnicalSpecialistPaySchedule>>()), Times.AtLeastOnce);
        }
        [TestMethod]
        public void UpdateTechnicalSpecialistCodeAndStandard()
        {
            mockTechnicalSpecialistRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.TechnicalSpecialist, bool>>>())).Returns(mockTechnicalSpecialistDbData);
            dataRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Data, bool>>>())).Returns((Expression<Func<DbModel.Data, bool>> predicate) => MockCodestandard.Where(predicate));
            mockTechnicalSpecialistPayScheduleRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.TechnicalSpecialistPaySchedule, bool>>>())).Returns((Expression<Func<DbModel.TechnicalSpecialistPaySchedule, bool>> predicate) => mockTechnicalSpecialistPayScheduleDbData.Where(predicate));
            Validation = new Evolution.ValidationService.Services.ValidationService();
            validationService = new Evolution.TechnicalSpecialist.Infrastructure.Validations.TechnicalSpecialistPayScheduleValidationService(Validation);
            TechnicalSpecialistPayScheduleService = new Service.TechnicalSpecialistPayScheduleService(mockTechnicalSpecialistPayScheduleRepository.Object, dataRepository.Object, mockLogger.Object, mockTechnicalSpecialistRepository.Object, validationService, mockMapper.Object);
            TechnicalSpecialistPayScheduleDomainModels.ToList().ForEach(x => x.RecordStatus = "M");
            var response = TechnicalSpecialistPayScheduleService.ModifyTechnicalSpecialistPaySchdule((int)TechnicalSpecialistPayScheduleDomainModels[0].Epin, new List<DomModel.TechnicalSpecialistPaySchedule> { TechnicalSpecialistPayScheduleDomainModels[0] });
            // mockTechnicalSpecialistPayScheduleRepository.Verify(m => m.Add(It.IsAny<IList<DbModel.TechnicalSpecialistPaySchedule>>()), Times.AtLeastOnce);
            Assert.AreEqual("1", response.Code);
            mockTechnicalSpecialistPayScheduleRepository.Verify(m => m.ForceSave(), Times.Once);
        }
        [TestMethod]
        public void DeleteTechnicalSpecialistCodeAndStandard()
        {
            mockTechnicalSpecialistRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.TechnicalSpecialist, bool>>>())).Returns(mockTechnicalSpecialistDbData);
            dataRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Data, bool>>>())).Returns((Expression<Func<DbModel.Data, bool>> predicate) => MockCodestandard.Where(predicate));
            mockTechnicalSpecialistPayScheduleRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.TechnicalSpecialistPaySchedule, bool>>>())).Returns((Expression<Func<DbModel.TechnicalSpecialistPaySchedule, bool>> predicate) => mockTechnicalSpecialistPayScheduleDbData.Where(predicate));
            Validation = new Evolution.ValidationService.Services.ValidationService();
            validationService = new Evolution.TechnicalSpecialist.Infrastructure.Validations.TechnicalSpecialistPayScheduleValidationService(Validation);
            TechnicalSpecialistPayScheduleService = new Service.TechnicalSpecialistPayScheduleService(mockTechnicalSpecialistPayScheduleRepository.Object, dataRepository.Object, mockLogger.Object, mockTechnicalSpecialistRepository.Object, validationService, mockMapper.Object);
            TechnicalSpecialistPayScheduleDomainModels.ToList().ForEach(x => x.RecordStatus = "D");
            var response = TechnicalSpecialistPayScheduleService.DeleteTechnicalSpecialistPaySchdule((int)TechnicalSpecialistPayScheduleDomainModels[0].Epin, new List<DomModel.TechnicalSpecialistPaySchedule> { TechnicalSpecialistPayScheduleDomainModels[0] });
            //mockTechnicalSpecialistPayScheduleRepository.Verify(m => m.Add(It.IsAny<IList<DbModel.TechnicalSpecialistPaySchedule>>()), Times.AtLeastOnce);
            Assert.AreEqual("1", response.Code);
            mockTechnicalSpecialistPayScheduleRepository.Verify(m => m.ForceSave(), Times.Once);
        }
    }
}
