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
    public class TechnicalSpecialistCommodityEquipmentKnowledgeServiceUnitTestCase : BaseTestCase
    {
        IValidationService Validation = null;
        ServiceDomainData.ITechnicalSpecialistComputerElectronicKnowledgeService TechnicalSpecialistComputerElectronicKnowledgeService = null;
        Mock<DomainData.ITechnicalSpecialistComputerElectronicKnowledgeServiceRepository> mockTechnicalSpecialistComputerElectronicKnowledgeRepository = null;
        Mock<DomainData.ITechnicalSpecialistRepository> mockTechnicalSpecialistRepository = null;
        IList<DomModel.TechnicalSpecialistComputerElectronicKnowledge> TechnicalSpecialistComputerElectronicKnowledgeDomainModels = null;
        Mock<IAppLogger<Service.TechnicalSpecialistComputerElectronicKnowledgeService>> mockLogger = null;
        Mock<IMapper> mockMapper = null;
        IQueryable<DbModel.TechnicalSpecialist> mockTechnicalSpecialistDbData = null;
        IQueryable<DbModel.TechnicalSpecialistComputerElectronicKnowledge> mockTechnicalSpecialistComputerElectronicKnowledgeDbData = null;
        IQueryable<DbModel.Data> MockComputerElectronic = null;
        ValdService.ITechnicalSpecialistComputerElectronicKnowledgeValidationService validationService = null;
        Mock<IDataRepository> dataRepository = null;

        [TestInitialize]
        public void IntializeTechnicalSpecialistComputerElectronicKnowledge()
        {
            mockTechnicalSpecialistComputerElectronicKnowledgeRepository = new Mock<DomainData.ITechnicalSpecialistComputerElectronicKnowledgeServiceRepository>();
            mockTechnicalSpecialistRepository = new Mock<DomainData.ITechnicalSpecialistRepository>();
            TechnicalSpecialistComputerElectronicKnowledgeDomainModels = MockTechnicalSpecialistModel.GetComputerElectronicKnowledgeMockedDomainData();
            mockLogger = new Mock<IAppLogger<Service.TechnicalSpecialistComputerElectronicKnowledgeService>>();
            mockMapper = new Mock<IMapper>();
            mockTechnicalSpecialistComputerElectronicKnowledgeDbData = MockTechnicalSpecialist.GetComputerElectronicKnowledgeMockData();
            dataRepository = new Mock<IDataRepository>();
            MockComputerElectronic = MockMaster.GetComputerElectronic();
            mockTechnicalSpecialistDbData = MockTechnicalSpecialist.GetTechnicalSpecialistMockData();
        }

        [TestMethod]
        public void SaveTechnicalSpecialistComputerElectronicKnowledge()
        {
            mockTechnicalSpecialistRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.TechnicalSpecialist, bool>>>())).Returns(mockTechnicalSpecialistDbData);
            dataRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Data, bool>>>())).Returns((Expression<Func<DbModel.Data, bool>> predicate) => MockComputerElectronic.Where(predicate));
            mockTechnicalSpecialistComputerElectronicKnowledgeRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.TechnicalSpecialistComputerElectronicKnowledge, bool>>>())).Returns((Expression<Func<DbModel.TechnicalSpecialistComputerElectronicKnowledge, bool>> predicate) => mockTechnicalSpecialistComputerElectronicKnowledgeDbData.Where(predicate));
            Validation = new Evolution.ValidationService.Services.ValidationService();
            validationService = new Evolution.TechnicalSpecialist.Infrastructure.Validations.TechSpecialistComputerElectronicKnowledgeValidationService(Validation);
            TechnicalSpecialistComputerElectronicKnowledgeService = new Service.TechnicalSpecialistComputerElectronicKnowledgeService(mockTechnicalSpecialistComputerElectronicKnowledgeRepository.Object, mockMapper.Object, mockLogger.Object, validationService, mockTechnicalSpecialistRepository.Object, dataRepository.Object);
            TechnicalSpecialistComputerElectronicKnowledgeDomainModels.ToList().ForEach(x => x.RecordStatus = "N");
            var response = TechnicalSpecialistComputerElectronicKnowledgeService.SaveTechnicalSpecialistComputerElectronicKnowledge((int)TechnicalSpecialistComputerElectronicKnowledgeDomainModels[0].Epin, new List<DomModel.TechnicalSpecialistComputerElectronicKnowledge> { TechnicalSpecialistComputerElectronicKnowledgeDomainModels[0] });
            mockTechnicalSpecialistComputerElectronicKnowledgeRepository.Verify(m => m.Add(It.IsAny<IList<DbModel.TechnicalSpecialistComputerElectronicKnowledge>>()), Times.AtLeastOnce);
        }

        [TestMethod]
        public void ModifyTechnicalSpecialistComputerElectronicKnowledge()
        {
            mockTechnicalSpecialistRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.TechnicalSpecialist, bool>>>())).Returns(mockTechnicalSpecialistDbData);
            dataRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Data, bool>>>())).Returns((Expression<Func<DbModel.Data, bool>> predicate) => MockComputerElectronic.Where(predicate));
            mockTechnicalSpecialistComputerElectronicKnowledgeRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.TechnicalSpecialistComputerElectronicKnowledge, bool>>>())).Returns((Expression<Func<DbModel.TechnicalSpecialistComputerElectronicKnowledge, bool>> predicate) => mockTechnicalSpecialistComputerElectronicKnowledgeDbData.Where(predicate));
            Validation = new Evolution.ValidationService.Services.ValidationService();
            validationService = new Evolution.TechnicalSpecialist.Infrastructure.Validations.TechSpecialistComputerElectronicKnowledgeValidationService(Validation);
            TechnicalSpecialistComputerElectronicKnowledgeService = new Service.TechnicalSpecialistComputerElectronicKnowledgeService(mockTechnicalSpecialistComputerElectronicKnowledgeRepository.Object, mockMapper.Object, mockLogger.Object, validationService, mockTechnicalSpecialistRepository.Object, dataRepository.Object);
            TechnicalSpecialistComputerElectronicKnowledgeDomainModels.ToList().ForEach(x => x.RecordStatus = "M");
            var response = TechnicalSpecialistComputerElectronicKnowledgeService.ModifyTechnicalSpecialistComputerElectronicKnowledge((int)TechnicalSpecialistComputerElectronicKnowledgeDomainModels[0].Epin, new List<DomModel.TechnicalSpecialistComputerElectronicKnowledge> { TechnicalSpecialistComputerElectronicKnowledgeDomainModels[0] });
            Assert.AreEqual("1", response.Code);
            mockTechnicalSpecialistComputerElectronicKnowledgeRepository.Verify(m => m.ForceSave(), Times.Once);
        }

        [TestMethod]
        public void DeleteTechnicalSpecialistComputerElectronicKnowledge()
        {
            mockTechnicalSpecialistRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.TechnicalSpecialist, bool>>>())).Returns(mockTechnicalSpecialistDbData);
            dataRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Data, bool>>>())).Returns((Expression<Func<DbModel.Data, bool>> predicate) => MockComputerElectronic.Where(predicate));
            mockTechnicalSpecialistComputerElectronicKnowledgeRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.TechnicalSpecialistComputerElectronicKnowledge, bool>>>())).Returns((Expression<Func<DbModel.TechnicalSpecialistComputerElectronicKnowledge, bool>> predicate) => mockTechnicalSpecialistComputerElectronicKnowledgeDbData.Where(predicate));
            Validation = new Evolution.ValidationService.Services.ValidationService();
            validationService = new Evolution.TechnicalSpecialist.Infrastructure.Validations.TechSpecialistComputerElectronicKnowledgeValidationService(Validation);
            TechnicalSpecialistComputerElectronicKnowledgeService = new Service.TechnicalSpecialistComputerElectronicKnowledgeService(mockTechnicalSpecialistComputerElectronicKnowledgeRepository.Object, mockMapper.Object, mockLogger.Object, validationService, mockTechnicalSpecialistRepository.Object, dataRepository.Object);
            TechnicalSpecialistComputerElectronicKnowledgeDomainModels.ToList().ForEach(x => x.RecordStatus = "D");
            var response = TechnicalSpecialistComputerElectronicKnowledgeService.DeleteTechnicalSpecialistComputerElectronicKnowledge((int)TechnicalSpecialistComputerElectronicKnowledgeDomainModels[0].Epin, new List<DomModel.TechnicalSpecialistComputerElectronicKnowledge> { TechnicalSpecialistComputerElectronicKnowledgeDomainModels[0] });
            Assert.AreEqual("1", response.Code);
            mockTechnicalSpecialistComputerElectronicKnowledgeRepository.Verify(m => m.ForceSave(), Times.Once);
        }

        [TestMethod]
        public void FetchAllComputerElectronicKnowledgeWithSearchValue()
        {
            mockTechnicalSpecialistComputerElectronicKnowledgeRepository.Setup(x => x.Search(It.Is<DomModel.TechnicalSpecialistComputerElectronicKnowledge>(c => c.Epin == 54))).Returns(TechnicalSpecialistComputerElectronicKnowledgeDomainModels);
            TechnicalSpecialistComputerElectronicKnowledgeService = new Service.TechnicalSpecialistComputerElectronicKnowledgeService(mockTechnicalSpecialistComputerElectronicKnowledgeRepository.Object, mockMapper.Object, mockLogger.Object, validationService, mockTechnicalSpecialistRepository.Object, dataRepository.Object);
            var response = TechnicalSpecialistComputerElectronicKnowledgeService.GetTechnicalSpecialistComputerElectronicKnowledge(new DomModel.TechnicalSpecialistComputerElectronicKnowledge { Epin = 54 });

            Assert.AreEqual("1", response.Code);
            Assert.AreEqual(2, (response.Result as List<DomModel.TechnicalSpecialistComputerElectronicKnowledge>).Count);
        }

        [TestMethod]
        public void FetchAllComputerElectronicKnowledgeWithoutSearchValue()
        {
            mockTechnicalSpecialistComputerElectronicKnowledgeRepository.Setup(x => x.Search(It.IsAny<DomModel.TechnicalSpecialistComputerElectronicKnowledge>())).Returns(TechnicalSpecialistComputerElectronicKnowledgeDomainModels);
            TechnicalSpecialistComputerElectronicKnowledgeService = new Service.TechnicalSpecialistComputerElectronicKnowledgeService(mockTechnicalSpecialistComputerElectronicKnowledgeRepository.Object, mockMapper.Object, mockLogger.Object,validationService, mockTechnicalSpecialistRepository.Object, dataRepository.Object);
            var response = TechnicalSpecialistComputerElectronicKnowledgeService.GetTechnicalSpecialistComputerElectronicKnowledge(new DomModel.TechnicalSpecialistComputerElectronicKnowledge());

            Assert.AreEqual("1", response.Code);
            Assert.AreEqual(2, (response.Result as List<DomModel.TechnicalSpecialistComputerElectronicKnowledge>).Count);
        }

        [TestMethod]
        public void FetchAllComputerElectronicKnowledgeWithNullSearchModel()
        {
            mockTechnicalSpecialistComputerElectronicKnowledgeRepository.Setup(x => x.Search(It.IsAny<DomModel.TechnicalSpecialistComputerElectronicKnowledge>())).Returns(TechnicalSpecialistComputerElectronicKnowledgeDomainModels);
            TechnicalSpecialistComputerElectronicKnowledgeService = new Service.TechnicalSpecialistComputerElectronicKnowledgeService(mockTechnicalSpecialistComputerElectronicKnowledgeRepository.Object, mockMapper.Object, mockLogger.Object, validationService, mockTechnicalSpecialistRepository.Object, dataRepository.Object);
            var response = TechnicalSpecialistComputerElectronicKnowledgeService.GetTechnicalSpecialistComputerElectronicKnowledge(null);


            Assert.AreEqual("1", response.Code);
            Assert.AreEqual(2, (response.Result as List<DomModel.TechnicalSpecialistComputerElectronicKnowledge>).Count);
        }

        [TestMethod]
        public void ThrowsExceptionWhileFetchingComputerElectronicKnowledge()
        {
            mockTechnicalSpecialistComputerElectronicKnowledgeRepository.Setup(x => x.Search(It.IsAny<DomModel.TechnicalSpecialistComputerElectronicKnowledge>())).Throws(new Exception("Exception occured while performing some operation."));
            TechnicalSpecialistComputerElectronicKnowledgeService = new Service.TechnicalSpecialistComputerElectronicKnowledgeService(mockTechnicalSpecialistComputerElectronicKnowledgeRepository.Object, mockMapper.Object, mockLogger.Object, validationService, mockTechnicalSpecialistRepository.Object, dataRepository.Object);

            var response = TechnicalSpecialistComputerElectronicKnowledgeService.GetTechnicalSpecialistComputerElectronicKnowledge(new DomModel.TechnicalSpecialistComputerElectronicKnowledge());

            Assert.AreEqual("11", response.Code);
            Assert.IsNotNull(response.Messages);
            Assert.AreEqual("31", response.Messages.FirstOrDefault().Code);
        }

    }
}
