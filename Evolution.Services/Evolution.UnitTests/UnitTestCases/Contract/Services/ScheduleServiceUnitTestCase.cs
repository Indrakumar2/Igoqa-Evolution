using System;
using System.Collections.Generic;
using System.Text;
using ServiceDomainData = Evolution.Contract.Domain.Interfaces.Contracts;
using ValidService = Evolution.Contract.Domain.Interfaces.Validations;
using DomainData = Evolution.Contract.Domain.Interfaces.Data;
using Moq;
using DomModel = Evolution.Contract.Domain.Models.Contracts;
using Evolution.Logging.Interfaces;
using AutoMapper;
using DbModel = Evolution.DbRepository.Models;
using System.Linq;
using Evolution.UnitTests.Mocks.Data.Contracts.Domain;
using Evolution.UnitTests.Mocks.Data.Contracts.Db;
using Masterdata = Evolution.Master.Domain.Interfaces.Data;
using System.Linq.Expressions;
using Microsoft.Extensions.Options;
using MasterService = Evolution.Master.Domain.Interfaces;
using Evolution.Common.Enums;
using DbRepo = Evolution.DbRepository.Interfaces.Master;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Service = Evolution.Contract.Core.Services;
using Evolution.UnitTest.Mocks.Data.Masters.Db.Masters;
using Evolution.UnitTest.UnitTestCases;


namespace Evolution.UnitTests.UnitTestCases.Contract.Services
{
    [TestClass]
    public class ScheduleServiceUnitTestCase : BaseTestCase
    {
        ServiceDomainData.IContractScheduleService ContractScheduleService = null;
        ValidService.IContractScheduleValidationService ScheduleValidService = null;
        Mock<DomainData.IContractScheduleRepository> mockContractScheduleRepository = null;
        Mock<DomainData.IContractRepository> mockContractRepository = null;
        Mock<DbRepo.IDataRepository> mockDbRepo = null;
        IList<DomModel.ContractSchedule> contractScheduleDomianModels = null;
        IList<DomModel.ContractScheduleRate> contractScheduleRateDomianModels = null;
        Mock<IAppLogger<Service.ContractScheduleService>> mockLogger = null;
        Mock<IMapper> mockMapper = null;
        IQueryable<DbModel.Contract> mockContractDbData = null;
        IQueryable<DbModel.ContractSchedule> mockContractScheduleDbData = null;
        IQueryable<DbModel.ContractRate> mockContractScheduleRateDbData = null;
        IQueryable<DbModel.Data> mockCurrency = null;
       Mock< ServiceDomainData.IContractScheduleRateService> ContractScheduleRateService = null;

        [TestInitialize]
        public void InitializeContractScheduleService()
        {
            mockContractScheduleRepository = new Mock<DomainData.IContractScheduleRepository>();
            mockContractRepository = new Mock<DomainData.IContractRepository>();
            contractScheduleDomianModels = MockContractDomainModel.GetContractScheduleMockedDomainModelData();
            mockLogger = new Mock<IAppLogger<Service.ContractScheduleService>>();
            mockMapper = new Mock<IMapper>();
            mockContractDbData = MockContract.GetDbModelContract();
            mockContractScheduleDbData = MockContract.GetDbModelContractSchedule();
            mockDbRepo = new Mock<DbRepo.IDataRepository>();
            mockCurrency = MockMaster.GetCurrencyMockData();
            ContractScheduleRateService = new Mock<ServiceDomainData.IContractScheduleRateService>();
            contractScheduleRateDomianModels = MockContractDomainModel.GetContractScheduleRateMockedDomainModelData();
            mockContractScheduleRateDbData = MockContract.GetDbModelContractRate();
           

        }


        [TestMethod]
        public void FetchAllContractScheduleWithoutSearchValue()
        {
            mockContractScheduleRepository.Setup(x => x.Search(It.IsAny<DomModel.ContractSchedule>())).Returns(contractScheduleDomianModels);
            ContractScheduleService = new Service.ContractScheduleService(mockMapper.Object, mockLogger.Object, mockContractScheduleRepository.Object, mockDbRepo.Object, mockContractRepository.Object, ContractScheduleRateService.Object, ScheduleValidService);
            var response = ContractScheduleService.GetContractSchedule(new DomModel.ContractSchedule());
            Assert.AreEqual("1", response.Code);
            Assert.AreEqual(2, (response.Result as List<DomModel.ContractSchedule>).Count);
        }
        [TestMethod]
        public void FetchAllContractScheduleWithSearchValue()
        {
            mockContractScheduleRepository.Setup(x => x.Search(It.Is<DomModel.ContractSchedule>(c => c.ContractNumber == "SU02412/0001"))).Returns(new List<DomModel.ContractSchedule> { contractScheduleDomianModels[0] });
            ContractScheduleService = new Service.ContractScheduleService(mockMapper.Object, mockLogger.Object, mockContractScheduleRepository.Object, mockDbRepo.Object, mockContractRepository.Object, ContractScheduleRateService.Object, ScheduleValidService);
            var response = ContractScheduleService.GetContractSchedule(new DomModel.ContractSchedule { ContractNumber = "SU02412/0001" });


            Assert.AreEqual("1", response.Code);
            Assert.AreEqual(1, (response.Result as List<DomModel.ContractSchedule>).Count);
        }

        [TestMethod]
        public void FetchContractScheduleListWithNullSearchModel()
        {
            mockContractScheduleRepository.Setup(x => x.Search(It.IsAny<DomModel.ContractSchedule>())).Returns(contractScheduleDomianModels);

            ContractScheduleService = new Service.ContractScheduleService(mockMapper.Object, mockLogger.Object, mockContractScheduleRepository.Object, mockDbRepo.Object, mockContractRepository.Object, ContractScheduleRateService.Object, ScheduleValidService);

            var response = ContractScheduleService.GetContractSchedule(null);

            Assert.AreEqual("1", response.Code);
            Assert.AreEqual(2, (response.Result as List<DomModel.ContractSchedule>).Count);
        }
        [TestMethod]
        public void ThrowsExceptionWhileFetchingContractSchedule()
        {
            mockContractScheduleRepository.Setup(x => x.Search(It.IsAny<DomModel.ContractSchedule>())).Throws(new Exception("Exception occured while performing some operation."));
            ContractScheduleService = new Service.ContractScheduleService(mockMapper.Object, mockLogger.Object, mockContractScheduleRepository.Object, mockDbRepo.Object, mockContractRepository.Object, ContractScheduleRateService.Object, ScheduleValidService);

            var response = ContractScheduleService.GetContractSchedule(new DomModel.ContractSchedule());

            Assert.AreEqual("11", response.Code);
            Assert.IsNotNull(response.Messages);
            Assert.AreEqual("31", response.Messages.FirstOrDefault().Code);
        }

     
        [TestMethod]
        public void SaveContrcatSchedule()
        {
            mockContractDbData = mockContractDbData.Where(z => z.Id == 1);
            mockContractRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Contract, bool>>>())).Returns(mockContractDbData);
            mockContractScheduleRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.ContractSchedule, bool>>>())).Returns((Expression<Func<DbModel.ContractSchedule, bool>> predicate) => mockContractScheduleDbData.Where(predicate));
            mockDbRepo.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Data, bool>>>())).Returns((Expression<Func<DbModel.Data, bool>> predicate) => mockCurrency.Where(predicate));
            ContractScheduleService = new Service.ContractScheduleService(Mapper.Instance, mockLogger.Object, mockContractScheduleRepository.Object, mockDbRepo.Object, mockContractRepository.Object, ContractScheduleRateService.Object, ScheduleValidService);
            
            contractScheduleDomianModels.ToList().ForEach(x => x.RecordStatus = "N");
            var response = ContractScheduleService.SaveContractSchedule(contractScheduleDomianModels[0].ContractNumber, new List<DomModel.ContractSchedule> { contractScheduleDomianModels[0] });
            Assert.AreEqual("1", response.Code);
            mockContractScheduleRepository.Verify(m => m.ForceSave(), Times.AtLeastOnce);
        }
        [TestMethod]
        public void ModifyContractSchedule()
        {
            mockContractDbData = mockContractDbData.Where(z => z.Id == 1);
            mockContractRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Contract, bool>>>())).Returns(mockContractDbData);
            mockContractScheduleRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.ContractSchedule, bool>>>())).Returns((Expression<Func<DbModel.ContractSchedule, bool>> predicate) => mockContractScheduleDbData.Where(predicate));
            mockDbRepo.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Data, bool>>>())).Returns((Expression<Func<DbModel.Data, bool>> predicate) => mockCurrency.Where(predicate));

            ContractScheduleService = new Service.ContractScheduleService(mockMapper.Object, mockLogger.Object, mockContractScheduleRepository.Object, mockDbRepo.Object, mockContractRepository.Object, ContractScheduleRateService.Object, ScheduleValidService);
            contractScheduleDomianModels.ToList().ForEach(x => x.RecordStatus = "M");
            var response = ContractScheduleService.ModifyContractSchedule(contractScheduleDomianModels[0].ContractNumber, new List<DomModel.ContractSchedule> { contractScheduleDomianModels[0] });
            mockContractScheduleRepository.Verify(m => m.Update(It.IsAny<DbModel.ContractSchedule>()), Times.AtLeastOnce);
        }

        [TestMethod]
        public void DeleteContractSchedule()
        {
            mockContractDbData = mockContractDbData.Where(z => z.Id == 1);
            mockContractRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Contract, bool>>>())).Returns(mockContractDbData);
            mockContractScheduleRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.ContractSchedule, bool>>>())).Returns((Expression<Func<DbModel.ContractSchedule, bool>> predicate) => mockContractScheduleDbData.Where(predicate));
            mockDbRepo.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Data, bool>>>())).Returns((Expression<Func<DbModel.Data, bool>> predicate) => mockCurrency.Where(predicate));

            ContractScheduleService = new Service.ContractScheduleService(mockMapper.Object, mockLogger.Object, mockContractScheduleRepository.Object, mockDbRepo.Object, mockContractRepository.Object, ContractScheduleRateService.Object, ScheduleValidService);
            contractScheduleDomianModels.ToList().ForEach(x => x.RecordStatus = "D");
            var response = ContractScheduleService.DeleteContractSchedule(contractScheduleDomianModels[0].ContractNumber, new List<DomModel.ContractSchedule> { contractScheduleDomianModels[0] });
            mockContractScheduleRepository.Verify(m => m.ForceSave(), Times.AtLeastOnce);
        }

        [TestMethod]
        public void SaveContrcatScheduleAndScheduleRate()
        {
            mockContractDbData = mockContractDbData.Where(z => z.Id == 1);
            mockContractRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Contract, bool>>>())).Returns(mockContractDbData);
            mockContractScheduleRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.ContractSchedule, bool>>>())).Returns((Expression<Func<DbModel.ContractSchedule, bool>> predicate) => mockContractScheduleDbData.Where(predicate));
            mockDbRepo.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Data, bool>>>())).Returns((Expression<Func<DbModel.Data, bool>> predicate) => mockCurrency.Where(predicate));
            ContractScheduleService = new Service.ContractScheduleService(Mapper.Instance, mockLogger.Object, mockContractScheduleRepository.Object, mockDbRepo.Object, mockContractRepository.Object, ContractScheduleRateService.Object, ScheduleValidService);

            contractScheduleDomianModels.ToList().ForEach(x => x.RecordStatus = "N");
            var response = ContractScheduleService.SaveContractScheduleAndScheduleRate(contractScheduleDomianModels[0].ContractNumber,  new List<DomModel.ContractSchedule> { contractScheduleDomianModels[0] }, new List<DomModel.ContractScheduleRate> { contractScheduleRateDomianModels[0] });
            Assert.AreEqual("1", response.Code);
            mockContractScheduleRepository.Verify(m => m.ForceSave(), Times.AtLeastOnce);
        }

        [TestMethod]
        public void ThrowsExceptionWhileSavingContractSchedule()
        {
            mockContractDbData = mockContractDbData.Where(z => z.Id == 1);
            mockContractRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Contract, bool>>>())).Throws(new Exception("Exception occured while performing some operation."));
            mockContractScheduleRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.ContractSchedule, bool>>>())).Returns((Expression<Func<DbModel.ContractSchedule, bool>> predicate) => mockContractScheduleDbData.Where(predicate));
            mockDbRepo.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Data, bool>>>())).Returns((Expression<Func<DbModel.Data, bool>> predicate) => mockCurrency.Where(predicate));
            ContractScheduleService = new Service.ContractScheduleService(Mapper.Instance, mockLogger.Object, mockContractScheduleRepository.Object, mockDbRepo.Object, mockContractRepository.Object, ContractScheduleRateService.Object, ScheduleValidService);

            contractScheduleDomianModels.ToList().ForEach(x => x.RecordStatus = "N");
            var response = ContractScheduleService.SaveContractSchedule(contractScheduleDomianModels[0].ContractNumber, new List<DomModel.ContractSchedule> { contractScheduleDomianModels[0] });

        

            Assert.AreEqual("11", response.Code);
            Assert.IsNotNull(response.Messages);
            Assert.AreEqual("31", response.Messages.FirstOrDefault().Code);
        }

        [TestMethod]
        public void ThrowsExceptionWhileUpdatingContractSchedule()
        {
            mockContractDbData = mockContractDbData.Where(z => z.Id == 1);
            mockContractRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Contract, bool>>>())).Throws(new Exception("Exception occured while performing some operation."));
            mockContractScheduleRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.ContractSchedule, bool>>>())).Returns((Expression<Func<DbModel.ContractSchedule, bool>> predicate) => mockContractScheduleDbData.Where(predicate));
            mockDbRepo.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Data, bool>>>())).Returns((Expression<Func<DbModel.Data, bool>> predicate) => mockCurrency.Where(predicate));
            ContractScheduleService = new Service.ContractScheduleService(Mapper.Instance, mockLogger.Object, mockContractScheduleRepository.Object, mockDbRepo.Object, mockContractRepository.Object, ContractScheduleRateService.Object, ScheduleValidService);

            contractScheduleDomianModels.ToList().ForEach(x => x.RecordStatus = "M");
            var response = ContractScheduleService.ModifyContractSchedule(contractScheduleDomianModels[0].ContractNumber, new List<DomModel.ContractSchedule> { contractScheduleDomianModels[0] });



            Assert.AreEqual("11", response.Code);
            Assert.IsNotNull(response.Messages);
            Assert.AreEqual("31", response.Messages.FirstOrDefault().Code);
        }


        [TestMethod]
        public void ThrowsExceptionWhileDeletingContractSchedule()
        {
            mockContractDbData = mockContractDbData.Where(z => z.Id == 1);
            mockContractRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Contract, bool>>>())).Throws(new Exception("Exception occured while performing some operation."));
            mockContractScheduleRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.ContractSchedule, bool>>>())).Returns((Expression<Func<DbModel.ContractSchedule, bool>> predicate) => mockContractScheduleDbData.Where(predicate));
            mockDbRepo.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Data, bool>>>())).Returns((Expression<Func<DbModel.Data, bool>> predicate) => mockCurrency.Where(predicate));
            ContractScheduleService = new Service.ContractScheduleService(Mapper.Instance, mockLogger.Object, mockContractScheduleRepository.Object, mockDbRepo.Object, mockContractRepository.Object, ContractScheduleRateService.Object, ScheduleValidService);

            contractScheduleDomianModels.ToList().ForEach(x => x.RecordStatus = "D");
            var response = ContractScheduleService.DeleteContractSchedule(contractScheduleDomianModels[0].ContractNumber, new List<DomModel.ContractSchedule> { contractScheduleDomianModels[0] });



            Assert.AreEqual("11", response.Code);
            Assert.IsNotNull(response.Messages);
            Assert.AreEqual("31", response.Messages.FirstOrDefault().Code);
        }

        [TestMethod]
        public void ShouldNotSaveContractScheduleWithNullContractNumber()
        {
            mockContractDbData = mockContractDbData.Where(z => z.Id == 1);
          //  mockContractRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Contract, bool>>>())).Returns(mockContractDbData);
            mockContractScheduleRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.ContractSchedule, bool>>>())).Returns((Expression<Func<DbModel.ContractSchedule, bool>> predicate) => mockContractScheduleDbData.Where(predicate));
            mockDbRepo.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Data, bool>>>())).Returns((Expression<Func<DbModel.Data, bool>> predicate) => mockCurrency.Where(predicate));
            ContractScheduleService = new Service.ContractScheduleService(Mapper.Instance, mockLogger.Object, mockContractScheduleRepository.Object, mockDbRepo.Object, mockContractRepository.Object, ContractScheduleRateService.Object, ScheduleValidService);

            contractScheduleDomianModels.ToList().ForEach(x => x.RecordStatus = "N");
            var response = ContractScheduleService.SaveContractSchedule(contractScheduleDomianModels[0].ContractNumber, new List<DomModel.ContractSchedule> { contractScheduleDomianModels[0] });
            Assert.AreEqual("11", response.Code);
           
        }

        [TestMethod]
        public void ShouldNotUpdateContractScheduleWithNullContractNumber()
        {
             mockContractDbData = mockContractDbData.Where(z => z.Id == 1);
           // mockContractRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Contract, bool>>>())).Returns(mockContractDbData);
            mockContractScheduleRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.ContractSchedule, bool>>>())).Returns((Expression<Func<DbModel.ContractSchedule, bool>> predicate) => mockContractScheduleDbData.Where(predicate));
            mockDbRepo.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Data, bool>>>())).Returns((Expression<Func<DbModel.Data, bool>> predicate) => mockCurrency.Where(predicate));
            ContractScheduleService = new Service.ContractScheduleService(Mapper.Instance, mockLogger.Object, mockContractScheduleRepository.Object, mockDbRepo.Object, mockContractRepository.Object, ContractScheduleRateService.Object, ScheduleValidService);

            contractScheduleDomianModels.ToList().ForEach(x => x.RecordStatus = "M");
            var response = ContractScheduleService.ModifyContractSchedule(contractScheduleDomianModels[0].ContractNumber, new List<DomModel.ContractSchedule> { contractScheduleDomianModels[0] });
            Assert.AreEqual("11", response.Code);

        }

        [TestMethod]
        public void ShouldNotUpdateWithInvalidcurrency()
        {
            mockContractDbData = mockContractDbData.Where(z => z.Id == 1);
            mockContractRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Contract, bool>>>())).Returns(mockContractDbData);
            mockContractScheduleRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.ContractSchedule, bool>>>())).Returns((Expression<Func<DbModel.ContractSchedule, bool>> predicate) => mockContractScheduleDbData.Where(predicate));
            mockDbRepo.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Data, bool>>>())).Returns((Expression<Func<DbModel.Data, bool>> predicate) => mockCurrency.Where(predicate));
            ContractScheduleService = new Service.ContractScheduleService(Mapper.Instance, mockLogger.Object, mockContractScheduleRepository.Object, mockDbRepo.Object, mockContractRepository.Object, ContractScheduleRateService.Object, ScheduleValidService);

            contractScheduleDomianModels.ToList().ForEach(x => x.RecordStatus = "M");
            contractScheduleDomianModels[0].ChargeCurrency = "ABC";
           var response = ContractScheduleService.ModifyContractSchedule(contractScheduleDomianModels[0].ContractNumber, new List<DomModel.ContractSchedule> { contractScheduleDomianModels[0] });
            Assert.AreEqual("11", response.Code);

        }

        [TestMethod]
        public void ShouldNotSaveWithInvalidcurrency()
        {
            mockContractDbData = mockContractDbData.Where(z => z.Id == 1);
            mockContractRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Contract, bool>>>())).Returns(mockContractDbData);
            mockContractScheduleRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.ContractSchedule, bool>>>())).Returns((Expression<Func<DbModel.ContractSchedule, bool>> predicate) => mockContractScheduleDbData.Where(predicate));
            mockDbRepo.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Data, bool>>>())).Returns((Expression<Func<DbModel.Data, bool>> predicate) => mockCurrency.Where(predicate));
            ContractScheduleService = new Service.ContractScheduleService(Mapper.Instance, mockLogger.Object, mockContractScheduleRepository.Object, mockDbRepo.Object, mockContractRepository.Object, ContractScheduleRateService.Object, ScheduleValidService);

            contractScheduleDomianModels.ToList().ForEach(x => x.RecordStatus = "N");
            contractScheduleDomianModels[0].ChargeCurrency = "ABC";
            var response = ContractScheduleService.SaveContractSchedule(contractScheduleDomianModels[0].ContractNumber, new List<DomModel.ContractSchedule> { contractScheduleDomianModels[0] });
            Assert.AreEqual("11", response.Code);

        }

        [TestMethod]
        public void ShouldNotSaveWithExsistanceScheduleName()
        {
            mockContractDbData = mockContractDbData.Where(z => z.Id == 1);
            mockContractRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Contract, bool>>>())).Returns(mockContractDbData);
            mockContractScheduleRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.ContractSchedule, bool>>>())).Returns((Expression<Func<DbModel.ContractSchedule, bool>> predicate) => mockContractScheduleDbData.Where(predicate));
            mockDbRepo.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Data, bool>>>())).Returns((Expression<Func<DbModel.Data, bool>> predicate) => mockCurrency.Where(predicate));
            ContractScheduleService = new Service.ContractScheduleService(Mapper.Instance, mockLogger.Object, mockContractScheduleRepository.Object, mockDbRepo.Object, mockContractRepository.Object, ContractScheduleRateService.Object,ScheduleValidService);

            contractScheduleDomianModels.ToList().ForEach(x => x.RecordStatus = "N");
            contractScheduleDomianModels[0].ScheduleName = "Rocks, Gerry";
            contractScheduleDomianModels[0].ChargeCurrency = "AED";
            var response = ContractScheduleService.SaveContractSchedule(contractScheduleDomianModels[0].ContractNumber, new List<DomModel.ContractSchedule> { contractScheduleDomianModels[0] });
            Assert.AreEqual("11", response.Code);

        }

        [TestMethod]
        public void ShouldNotUpdateWithExsistanceScheduleName()
        {
            mockContractDbData = mockContractDbData.Where(z => z.Id == 1);
            mockContractRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Contract, bool>>>())).Returns(mockContractDbData);
            mockContractScheduleRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.ContractSchedule, bool>>>())).Returns((Expression<Func<DbModel.ContractSchedule, bool>> predicate) => mockContractScheduleDbData.Where(predicate));
            mockDbRepo.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Data, bool>>>())).Returns((Expression<Func<DbModel.Data, bool>> predicate) => mockCurrency.Where(predicate));
            ContractScheduleService = new Service.ContractScheduleService(Mapper.Instance, mockLogger.Object, mockContractScheduleRepository.Object, mockDbRepo.Object, mockContractRepository.Object, ContractScheduleRateService.Object, ScheduleValidService);

            contractScheduleDomianModels.ToList().ForEach(x => x.RecordStatus = "M");
            contractScheduleDomianModels[0].ScheduleId = 3;
         
            var response = ContractScheduleService.ModifyContractSchedule(contractScheduleDomianModels[0].ContractNumber, new List<DomModel.ContractSchedule> { contractScheduleDomianModels[0] });
            Assert.AreEqual("11", response.Code);

        }

        [TestMethod]
        public void ShouldNotDeleteWithExsistanceScheduleName()
        {
            mockContractDbData = mockContractDbData.Where(z => z.Id == 1);
            mockContractRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Contract, bool>>>())).Returns(mockContractDbData);
            mockContractScheduleRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.ContractSchedule, bool>>>())).Returns((Expression<Func<DbModel.ContractSchedule, bool>> predicate) => mockContractScheduleDbData.Where(predicate));
            mockDbRepo.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Data, bool>>>())).Returns((Expression<Func<DbModel.Data, bool>> predicate) => mockCurrency.Where(predicate));
            ContractScheduleService = new Service.ContractScheduleService(Mapper.Instance, mockLogger.Object, mockContractScheduleRepository.Object, mockDbRepo.Object, mockContractRepository.Object, ContractScheduleRateService.Object, ScheduleValidService);

            contractScheduleDomianModels.ToList().ForEach(x => x.RecordStatus = "D");
            contractScheduleDomianModels[0].ScheduleId = 3;

            var response = ContractScheduleService.DeleteContractSchedule(contractScheduleDomianModels[0].ContractNumber, new List<DomModel.ContractSchedule> { contractScheduleDomianModels[0] });
            Assert.AreEqual("11", response.Code);

        }

        [TestMethod]
        public void ShouldNotDeleteScheduleWhenItisAlreadyAssociatedWithAnotherModule()
        {
            mockContractDbData = mockContractDbData.Where(z => z.Id == 1);
            mockContractRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Contract, bool>>>())).Returns(mockContractDbData);
            mockContractScheduleRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.ContractSchedule, bool>>>())).Returns((Expression<Func<DbModel.ContractSchedule, bool>> predicate) => mockContractScheduleDbData.Where(predicate));
            mockDbRepo.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Data, bool>>>())).Returns((Expression<Func<DbModel.Data, bool>> predicate) => mockCurrency.Where(predicate));
            ContractScheduleService = new Service.ContractScheduleService(Mapper.Instance, mockLogger.Object, mockContractScheduleRepository.Object, mockDbRepo.Object, mockContractRepository.Object, ContractScheduleRateService.Object, ScheduleValidService);

            contractScheduleDomianModels.ToList().ForEach(x => x.RecordStatus = "D");
            

            var response = ContractScheduleService.DeleteContractSchedule(contractScheduleDomianModels[1].ContractNumber, new List<DomModel.ContractSchedule> { contractScheduleDomianModels[1] });
            Assert.AreEqual("11", response.Code);

        }
    }


}

