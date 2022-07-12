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
    public class TechnicalSpecialistCodeAndStandardUnitTestCase: BaseTestCase
    {
        IValidationService Validation = null;
        ServiceDomainData.ITechnicalSpecialistCodeAndStandardService TechnicalSpecialistCodeAndStandardService = null;
        Mock<DomainData.ITechnicalSpecialistCodeAndStandardRepository> mockTechnicalSpecialistCodeAndStandardRepository = null;
        Mock<DomainData.ITechnicalSpecialistRepository> mockTechnicalSpecialistRepository = null;
        IList<DomModel.TechnicalSpecialistCodeAndStandard> TechnicalSpecialistCodeAndStandardDomainModels = null;
        Mock<IAppLogger<Service.TechnicalSpecialistCodeAndStandardService>> mockLogger = null;
        Mock<IMapper> mockMapper = null;
        IQueryable<DbModel.TechnicalSpecialist> mockTechnicalSpecialistDbData = null;
        IQueryable<DbModel.TechnicalSpecialistCodeAndStandard> mockTechnicalSpecialistCodeAndStandardDbData = null;
        IQueryable<DbModel.Data> MockCodestandard = null;
        ValdService.ITechnicalSpecialistCodeAndStandardValidationService validationService = null;
        Mock<IDataRepository> dataRepository = null;
        [TestInitialize]
        public void IntializeTechnicalSpecialistCodeAndStandard()
        {
            mockTechnicalSpecialistCodeAndStandardRepository = new Mock<DomainData.ITechnicalSpecialistCodeAndStandardRepository>();
            mockTechnicalSpecialistRepository = new Mock<DomainData.ITechnicalSpecialistRepository>();
            TechnicalSpecialistCodeAndStandardDomainModels = MockTechnicalSpecialistModel.GetTechnicalSpecialistCodeAndStandardMockedDomainData();
            mockLogger = new Mock<IAppLogger<Service.TechnicalSpecialistCodeAndStandardService>>();
            mockMapper = new Mock<IMapper>();
            mockTechnicalSpecialistCodeAndStandardDbData = MockTechnicalSpecialist.GetTechnicalSpecialistCodeAndStandardMockData();
            dataRepository = new Mock<IDataRepository>();
            MockCodestandard = MockMaster.GetCodeStandardData();
            Validation = new Evolution.ValidationService.Services.ValidationService();
            mockTechnicalSpecialistDbData = MockTechnicalSpecialist.GetTechnicalSpecialistMockData();
           
        }
        [TestMethod]
        public void FetchAllCodeAndStandardSearchValue()
        {

            mockTechnicalSpecialistCodeAndStandardRepository.Setup(x => x.Search(It.Is<DomModel.TechnicalSpecialistCodeAndStandard>(c => c.Epin == 54))).Returns(TechnicalSpecialistCodeAndStandardDomainModels);
            TechnicalSpecialistCodeAndStandardService = new Service.TechnicalSpecialistCodeAndStandardService(mockTechnicalSpecialistCodeAndStandardRepository.Object, mockLogger.Object, mockTechnicalSpecialistRepository.Object,dataRepository.Object,mockMapper.Object, validationService); var response = TechnicalSpecialistCodeAndStandardService.GetTechnicalSpecialistCodeAndStandard(new DomModel.TechnicalSpecialistCodeAndStandard { Epin = 54 });
            Assert.AreEqual("1", response.Code);
            Assert.AreEqual(2, (response.Result as List<DomModel.TechnicalSpecialistCodeAndStandard>).Count);

        }
       

        [TestMethod]
        public void FetchAllCodeAndStandardWithNullSearchModel()
        {
            mockTechnicalSpecialistCodeAndStandardRepository.Setup(x => x.Search(It.IsAny<DomModel.TechnicalSpecialistCodeAndStandard>())).Returns(TechnicalSpecialistCodeAndStandardDomainModels);
            TechnicalSpecialistCodeAndStandardService = new Service.TechnicalSpecialistCodeAndStandardService(mockTechnicalSpecialistCodeAndStandardRepository.Object, mockLogger.Object, mockTechnicalSpecialistRepository.Object, dataRepository.Object, mockMapper.Object, validationService);
            var response = TechnicalSpecialistCodeAndStandardService.GetTechnicalSpecialistCodeAndStandard(new DomModel.TechnicalSpecialistCodeAndStandard { Epin = 54 });
            Assert.AreEqual("1", response.Code);
            Assert.AreEqual(2, (response.Result as List<DomModel.TechnicalSpecialistCodeAndStandard>).Count);
        }

        [TestMethod]
        public void ThrowsExceptionWhileFetchingCodeAndStandard()
        {
            mockTechnicalSpecialistCodeAndStandardRepository.Setup(x => x.Search(It.IsAny<DomModel.TechnicalSpecialistCodeAndStandard>())).Throws(new Exception("Exception occured while performing some operation."));
            TechnicalSpecialistCodeAndStandardService = new Service.TechnicalSpecialistCodeAndStandardService(mockTechnicalSpecialistCodeAndStandardRepository.Object, mockLogger.Object, mockTechnicalSpecialistRepository.Object, dataRepository.Object, mockMapper.Object, validationService);
            var response = TechnicalSpecialistCodeAndStandardService.GetTechnicalSpecialistCodeAndStandard(new DomModel.TechnicalSpecialistCodeAndStandard { Epin = 54 });

            Assert.AreEqual("11", response.Code);
            Assert.IsNotNull(response.Messages);
            Assert.AreEqual("31", response.Messages.FirstOrDefault().Code);
        }

        [TestMethod]
        public void SaveTechnicalSpecialistCodeAndStandard()
        {
            mockTechnicalSpecialistRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.TechnicalSpecialist, bool>>>())).Returns(mockTechnicalSpecialistDbData);
            dataRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Data, bool>>>())).Returns((Expression<Func<DbModel.Data, bool>> predicate) => MockCodestandard.Where(predicate));
            mockTechnicalSpecialistCodeAndStandardRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.TechnicalSpecialistCodeAndStandard, bool>>>())).Returns((Expression<Func<DbModel.TechnicalSpecialistCodeAndStandard, bool>> predicate) => mockTechnicalSpecialistCodeAndStandardDbData.Where(predicate));
            Validation = new Evolution.ValidationService.Services.ValidationService();
            validationService = new Evolution.TechnicalSpecialist.Infrastructure.Validations.TechnicalSpecialistCodeValidationService(Validation);
            TechnicalSpecialistCodeAndStandardService = new Service.TechnicalSpecialistCodeAndStandardService(mockTechnicalSpecialistCodeAndStandardRepository.Object, mockLogger.Object, mockTechnicalSpecialistRepository.Object, dataRepository.Object, mockMapper.Object, validationService);
            TechnicalSpecialistCodeAndStandardDomainModels.ToList().ForEach(x => x.RecordStatus = "N");
            var response = TechnicalSpecialistCodeAndStandardService.SaveTechnicalSpecialistCodeAndStandard((int)TechnicalSpecialistCodeAndStandardDomainModels[0].Epin, new List<DomModel.TechnicalSpecialistCodeAndStandard> { TechnicalSpecialistCodeAndStandardDomainModels[0] });
            mockTechnicalSpecialistCodeAndStandardRepository.Verify(m => m.Add(It.IsAny<IList<DbModel.TechnicalSpecialistCodeAndStandard>>()), Times.AtLeastOnce);
        }
        [TestMethod]
        public void ModifyTechnicalSpecialistCodeAndStandard()
        {
            mockTechnicalSpecialistRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.TechnicalSpecialist, bool>>>())).Returns(mockTechnicalSpecialistDbData);
            dataRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Data, bool>>>())).Returns((Expression<Func<DbModel.Data, bool>> predicate) => MockCodestandard.Where(predicate));
            mockTechnicalSpecialistCodeAndStandardRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.TechnicalSpecialistCodeAndStandard, bool>>>())).Returns((Expression<Func<DbModel.TechnicalSpecialistCodeAndStandard, bool>> predicate) => mockTechnicalSpecialistCodeAndStandardDbData.Where(predicate));
            Validation = new Evolution.ValidationService.Services.ValidationService();
            validationService = new Evolution.TechnicalSpecialist.Infrastructure.Validations.TechnicalSpecialistCodeValidationService(Validation);
            TechnicalSpecialistCodeAndStandardService = new Service.TechnicalSpecialistCodeAndStandardService(mockTechnicalSpecialistCodeAndStandardRepository.Object, mockLogger.Object, mockTechnicalSpecialistRepository.Object, dataRepository.Object, mockMapper.Object, validationService);
            TechnicalSpecialistCodeAndStandardDomainModels.ToList().ForEach(x => x.RecordStatus = "M");
            var response = TechnicalSpecialistCodeAndStandardService.ModifyTechnicalSpecialistCodeAndStandard((int)TechnicalSpecialistCodeAndStandardDomainModels[0].Epin, new List<DomModel.TechnicalSpecialistCodeAndStandard> { TechnicalSpecialistCodeAndStandardDomainModels[0] });
            //mockTechnicalSpecialistCodeAndStandardRepository.Verify(m => m.Update(It.IsAny<DbModel.TechnicalSpecialistCodeAndStandard>()), Times.AtLeastOnce);
            Assert.AreEqual("1", response.Code);
           mockTechnicalSpecialistCodeAndStandardRepository.Verify(m => m.ForceSave(), Times.Once);
        }
        [TestMethod]
        public void DeleteTechnicalSpecialistCodeAndStandard()
        {

            mockTechnicalSpecialistRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.TechnicalSpecialist, bool>>>())).Returns(mockTechnicalSpecialistDbData);
            dataRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Data, bool>>>())).Returns((Expression<Func<DbModel.Data, bool>> predicate) => MockCodestandard.Where(predicate));
            mockTechnicalSpecialistCodeAndStandardRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.TechnicalSpecialistCodeAndStandard, bool>>>())).Returns((Expression<Func<DbModel.TechnicalSpecialistCodeAndStandard, bool>> predicate) => mockTechnicalSpecialistCodeAndStandardDbData.Where(predicate));
            Validation = new Evolution.ValidationService.Services.ValidationService();
            validationService = new Evolution.TechnicalSpecialist.Infrastructure.Validations.TechnicalSpecialistCodeValidationService(Validation);
            TechnicalSpecialistCodeAndStandardService = new Service.TechnicalSpecialistCodeAndStandardService(mockTechnicalSpecialistCodeAndStandardRepository.Object, mockLogger.Object, mockTechnicalSpecialistRepository.Object, dataRepository.Object, mockMapper.Object, validationService);
            TechnicalSpecialistCodeAndStandardDomainModels.ToList().ForEach(x => x.RecordStatus = "D");
            var response = TechnicalSpecialistCodeAndStandardService.DeleteTechnicalSpecialistCodeAndStandard((int)TechnicalSpecialistCodeAndStandardDomainModels[0].Epin, new List<DomModel.TechnicalSpecialistCodeAndStandard> { TechnicalSpecialistCodeAndStandardDomainModels[0] });
            //mockTechnicalSpecialistCodeAndStandardRepository.Verify(m => m.Update(It.IsAny<DbModel.TechnicalSpecialistCodeAndStandard>()), Times.AtLeastOnce);
            Assert.AreEqual("1", response.Code);
            mockTechnicalSpecialistCodeAndStandardRepository.Verify(m => m.ForceSave(), Times.Once);
        }

    }
}
