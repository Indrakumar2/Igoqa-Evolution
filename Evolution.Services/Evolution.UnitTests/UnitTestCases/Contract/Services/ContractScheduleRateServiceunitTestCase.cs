using System;
using System.Collections.Generic;
using System.Text;
using ServiceDomainData = Evolution.Contract.Domain.Interfaces.Contracts;
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
using MasterService = Evolution.Master.Core;
using Evolution.Common.Enums;
using DbRepo = Evolution.DbRepository.Interfaces.Master;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Service = Evolution.Contract.Core.Services;
using Evolution.Master.Domain.Interfaces.Data;
using Evolution.UnitTest.Mocks.Data.Masters.Db.Masters;
using Evolution.UnitTest.UnitTestCases;
using ValidService = Evolution.Contract.Domain.Interfaces.Validations;

namespace Evolution.UnitTests.UnitTestCases.Contract.Services
{
    [TestClass]
   public  class ContractScheduleRateServiceunitTestCase : BaseTestCase
    {

        ServiceDomainData.IContractScheduleRateService ContractScheduleRateService = null;
        Mock<DomainData.IContractScheduleRepository> mockContractScheduleRepository = null;
        Mock<DomainData.IContractScheduleRateRepository> mockContractScheduleRateRepository = null;
        Mock<Masterdata.ICompanyInspectionTypeChargeRateRepository> mockinspectionTypeChargeRateRepository = null;
        Mock<DomainData.IContractRepository> mockContractRepository = null; 
        Mock<DbRepo.IDataRepository> mockDbRepo = null;
        IList<DomModel.ContractScheduleRate> contractScheduleRateDomianModels = null;
        IList<DomModel.ContractSchedule> contractScheduleDomianModels = null;
        Mock<IAppLogger<Service.ContractScheduleRateService>> mockLogger = null;
        Mock<IMapper> mockMapper = null;
        IQueryable<DbModel.Contract> mockContractDbData = null;
        IQueryable<DbModel.ContractRate> mockContractScheduleRateDbData = null;
        IQueryable<DbModel.ContractSchedule> mockContractScheduleDbData = null;
        IQueryable<DbModel.Data> mockContractExpenseType = null;
        IQueryable<DbModel.CompanyInspectionTypeChargeRate> mockCompanyInspectionTypeChargeRate = null;
        ValidService.IContractScheduleRateValidationService ScheduleRateValidService = null;


        [TestInitialize]
        public void InitializeContractScheduleRateService()
        {
            mockContractScheduleRepository = new Mock<DomainData.IContractScheduleRepository>();
            mockContractRepository = new Mock<DomainData.IContractRepository>();
            mockContractScheduleRateRepository = new Mock<DomainData.IContractScheduleRateRepository>();
            contractScheduleRateDomianModels = MockContractDomainModel.GetContractScheduleRateMockedDomainModelData();
            mockLogger = new Mock<IAppLogger<Service.ContractScheduleRateService>>();
            mockMapper = new Mock<IMapper>();
            mockContractDbData = MockContract.GetDbModelContract();
            mockContractScheduleRateDbData = MockContract.GetDbModelContractRate();
            mockContractScheduleDbData = MockContract.GetDbModelContractSchedule();
            contractScheduleDomianModels= MockContractDomainModel.GetContractScheduleMockedDomainModelData();
            mockDbRepo = new Mock<DbRepo.IDataRepository>();
            mockinspectionTypeChargeRateRepository = new Mock<Masterdata.ICompanyInspectionTypeChargeRateRepository>();
            mockContractExpenseType = MockContract.GetExpenseTypeMockData();
            mockCompanyInspectionTypeChargeRate = MockMaster.GetCompanyInspectionTypeChargeRateMockData();




        }

        [TestMethod]
        public void FetchAllContractScheduleRateWithoutSearchValue()
        {
            mockContractScheduleRateRepository.Setup(x => x.Search(It.IsAny<DomModel.ContractScheduleRate>())).Returns(contractScheduleRateDomianModels);
            ContractScheduleRateService = new Service.ContractScheduleRateService(mockMapper.Object, mockLogger.Object, mockContractScheduleRateRepository.Object, mockContractRepository.Object,  mockDbRepo.Object,  mockContractScheduleRepository.Object, mockinspectionTypeChargeRateRepository.Object, ScheduleRateValidService);
            var response = ContractScheduleRateService.GetContractScheduleRate(new DomModel.ContractScheduleRate());
            Assert.AreEqual("1", response.Code);
            Assert.AreEqual(2, (response.Result as List<DomModel.ContractScheduleRate>).Count);
        }
        [TestMethod]
        public void FetchAllContractScheduleRateWithSearchValue()
        {
            mockContractScheduleRateRepository.Setup(x => x.Search(It.Is<DomModel.ContractScheduleRate>(c => c.ContractNumber == "SU02412/0001"))).Returns(new List<DomModel.ContractScheduleRate> { contractScheduleRateDomianModels[0] });
            ContractScheduleRateService = new Service.ContractScheduleRateService(mockMapper.Object, mockLogger.Object, mockContractScheduleRateRepository.Object, mockContractRepository.Object, mockDbRepo.Object, mockContractScheduleRepository.Object, mockinspectionTypeChargeRateRepository.Object, ScheduleRateValidService);
            var response = ContractScheduleRateService.GetContractScheduleRate(new DomModel.ContractScheduleRate { ContractNumber = "SU02412/0001" });


            Assert.AreEqual("1", response.Code);
            Assert.AreEqual(1, (response.Result as List<DomModel.ContractScheduleRate>).Count);
        }

        [TestMethod]
        public void FetchContractScheduleRateListWithNullSearchModel()
        {
            mockContractScheduleRateRepository.Setup(x => x.Search(It.IsAny<DomModel.ContractScheduleRate>())).Returns(contractScheduleRateDomianModels);

            ContractScheduleRateService = new Service.ContractScheduleRateService(mockMapper.Object, mockLogger.Object, mockContractScheduleRateRepository.Object, mockContractRepository.Object, mockDbRepo.Object, mockContractScheduleRepository.Object, mockinspectionTypeChargeRateRepository.Object, ScheduleRateValidService);

            var response = ContractScheduleRateService.GetContractScheduleRate(null);

            Assert.AreEqual("1", response.Code);
            Assert.AreEqual(2, (response.Result as List<DomModel.ContractScheduleRate>).Count);
        }
        [TestMethod]
        public void ThrowsExceptionWhileFetchingContractScheduleRate()
        {
            mockContractScheduleRateRepository.Setup(x => x.Search(It.IsAny<DomModel.ContractScheduleRate>())).Throws(new Exception("Exception occured while performing some operation."));
            ContractScheduleRateService = new Service.ContractScheduleRateService(mockMapper.Object, mockLogger.Object, mockContractScheduleRateRepository.Object, mockContractRepository.Object, mockDbRepo.Object, mockContractScheduleRepository.Object, mockinspectionTypeChargeRateRepository.Object, ScheduleRateValidService);

            var response = ContractScheduleRateService.GetContractScheduleRate(new DomModel.ContractScheduleRate());

            Assert.AreEqual("11", response.Code);
            Assert.IsNotNull(response.Messages);
            Assert.AreEqual("31", response.Messages.FirstOrDefault().Code);
        }
        [TestMethod]
        public void SaveContrcatScheduleRate()
        {
            mockContractScheduleRateRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.ContractRate, bool>>>())).Returns((Expression<Func<DbModel.ContractRate, bool>> predicate) => mockContractScheduleRateDbData.Where(predicate));
            mockContractRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Contract, bool>>>())).Returns(mockContractDbData);
            mockContractScheduleRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.ContractSchedule, bool>>>())).Returns((Expression<Func<DbModel.ContractSchedule, bool>> predicate) => mockContractScheduleDbData.Where(predicate));
            mockDbRepo.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Data, bool>>>())).Returns((Expression<Func<DbModel.Data, bool>> predicate) => mockContractExpenseType.Where(predicate));
            mockContractScheduleRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.ContractSchedule, bool>>>())).Returns((Expression<Func<DbModel.ContractSchedule, bool>> predicate) => mockContractScheduleDbData.Where(predicate));
            mockinspectionTypeChargeRateRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.CompanyInspectionTypeChargeRate, bool>>>())).Returns((Expression<Func<DbModel.CompanyInspectionTypeChargeRate, bool>> predicate) => mockCompanyInspectionTypeChargeRate.Where(predicate));

            ContractScheduleRateService = new Service.ContractScheduleRateService(mockMapper.Object, mockLogger.Object, mockContractScheduleRateRepository.Object, mockContractRepository.Object, mockDbRepo.Object, mockContractScheduleRepository.Object, mockinspectionTypeChargeRateRepository.Object, ScheduleRateValidService);
            contractScheduleRateDomianModels.ToList().ForEach(x => x.RecordStatus = "N");
            contractScheduleRateDomianModels[0].ScheduleName = "Norway";
            var response = ContractScheduleRateService.SaveContractScheduleRate(contractScheduleRateDomianModels[0].ContractNumber, contractScheduleRateDomianModels[0].ScheduleName, new List<DomModel.ContractScheduleRate> { contractScheduleRateDomianModels[0] });
            mockContractScheduleRateRepository.Verify(m => m.ForceSave(), Times.Once);
        }
        [TestMethod]
        public void ModifyContractScheduleRate()
        {
            mockContractScheduleRateRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.ContractRate, bool>>>())).Returns((Expression<Func<DbModel.ContractRate, bool>> predicate) => mockContractScheduleRateDbData.Where(predicate));
            mockContractRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Contract, bool>>>())).Returns(mockContractDbData);
            mockContractScheduleRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.ContractSchedule, bool>>>())).Returns((Expression<Func<DbModel.ContractSchedule, bool>> predicate) => mockContractScheduleDbData.Where(predicate));
            mockDbRepo.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Data, bool>>>())).Returns((Expression<Func<DbModel.Data, bool>> predicate) => mockContractExpenseType.Where(predicate));
            mockinspectionTypeChargeRateRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.CompanyInspectionTypeChargeRate, bool>>>())).Returns((Expression<Func<DbModel.CompanyInspectionTypeChargeRate, bool>> predicate) => mockCompanyInspectionTypeChargeRate.Where(predicate));
             
            ContractScheduleRateService = new Service.ContractScheduleRateService(mockMapper.Object, mockLogger.Object, mockContractScheduleRateRepository.Object, mockContractRepository.Object, mockDbRepo.Object, mockContractScheduleRepository.Object, mockinspectionTypeChargeRateRepository.Object, ScheduleRateValidService);
            contractScheduleRateDomianModels.ToList().ForEach(x => x.RecordStatus = "M");
           
            var response = ContractScheduleRateService.ModifyContractScheduleRate(contractScheduleRateDomianModels[0].ContractNumber, contractScheduleRateDomianModels[0].ScheduleName, new List<DomModel.ContractScheduleRate> { contractScheduleRateDomianModels[0] });
            mockContractScheduleRateRepository.Verify(m => m.Update(It.IsAny<DbModel.ContractRate>()), Times.AtLeastOnce);
        }

        [TestMethod]
        public void DeleteContractScheduleRate()
        {
            mockContractScheduleRateRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.ContractRate, bool>>>())).Returns((Expression<Func<DbModel.ContractRate, bool>> predicate) => mockContractScheduleRateDbData.Where(predicate));
            mockContractRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Contract, bool>>>())).Returns(mockContractDbData);
            mockContractScheduleRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.ContractSchedule, bool>>>())).Returns((Expression<Func<DbModel.ContractSchedule, bool>> predicate) => mockContractScheduleDbData.Where(predicate));
            mockDbRepo.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Data, bool>>>())).Returns((Expression<Func<DbModel.Data, bool>> predicate) => mockContractExpenseType.Where(predicate));
            mockContractScheduleRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.ContractSchedule, bool>>>())).Returns((Expression<Func<DbModel.ContractSchedule, bool>> predicate) => mockContractScheduleDbData.Where(predicate));
            ContractScheduleRateService = new Service.ContractScheduleRateService(mockMapper.Object, mockLogger.Object, mockContractScheduleRateRepository.Object, mockContractRepository.Object, mockDbRepo.Object, mockContractScheduleRepository.Object, mockinspectionTypeChargeRateRepository.Object, ScheduleRateValidService);
            contractScheduleRateDomianModels.ToList().ForEach(x => x.RecordStatus = "D");
          
            var response = ContractScheduleRateService.DeleteContractScheduleRate(contractScheduleRateDomianModels[0].ContractNumber, contractScheduleRateDomianModels[0].ScheduleName, new List<DomModel.ContractScheduleRate> { contractScheduleRateDomianModels[0] });
            mockContractScheduleRateRepository.Verify(m => m.Delete(It.IsAny<DbModel.ContractRate>()), Times.AtLeastOnce);
        }

        //[TestMethod]
        //public void ThrowsExceptionWhileSavingContrcatScheduleRate()
        //{
        //    mockContractScheduleRateRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.ContractRate, bool>>>())).Throws(new Exception("Exception occured while performing some operation."));
        //    mockContractRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Contract, bool>>>())).Returns(mockContractDbData);
        //    mockContractScheduleRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.ContractSchedule, bool>>>())).Returns((Expression<Func<DbModel.ContractSchedule, bool>> predicate) => mockContractScheduleDbData.Where(predicate));
        //    mockDbRepo.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Data, bool>>>())).Returns((Expression<Func<DbModel.Data, bool>> predicate) => mockContractExpenseType.Where(predicate));
        //    mockContractScheduleRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.ContractSchedule, bool>>>())).Returns((Expression<Func<DbModel.ContractSchedule, bool>> predicate) => mockContractScheduleDbData.Where(predicate));
        //    mockinspectionTypeChargeRateRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.CompanyInspectionTypeChargeRate, bool>>>())).Returns((Expression<Func<DbModel.CompanyInspectionTypeChargeRate, bool>> predicate) => mockCompanyInspectionTypeChargeRate.Where(predicate));

        //    ContractScheduleRateService = new Service.ContractScheduleRateService(mockMapper.Object, mockLogger.Object, mockContractScheduleRateRepository.Object, mockContractRepository.Object, mockDbRepo.Object, mockContractScheduleRepository.Object, mockinspectionTypeChargeRateRepository.Object);
        //    contractScheduleRateDomianModels.ToList().ForEach(x => x.RecordStatus = "N");
           
        //    var response = ContractScheduleRateService.SaveContractScheduleRate(contractScheduleRateDomianModels[0].ContractNumber, contractScheduleRateDomianModels[0].ScheduleName, new List<DomModel.ContractScheduleRate> { contractScheduleRateDomianModels[0] });
        //    Assert.AreEqual("11", response.Code);
        //    Assert.IsNotNull(response.Messages);
        //    Assert.AreEqual("31", response.Messages.FirstOrDefault().Code);
        //}
        [TestMethod]
        public void ThrowsExceptionWhileModifingContrcatScheduleRate()
        {
            mockContractScheduleRateRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.ContractRate, bool>>>())).Throws(new Exception("Exception occured while performing some operation."));
            mockContractRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Contract, bool>>>())).Returns(mockContractDbData);
            mockContractScheduleRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.ContractSchedule, bool>>>())).Returns((Expression<Func<DbModel.ContractSchedule, bool>> predicate) => mockContractScheduleDbData.Where(predicate));
            mockDbRepo.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Data, bool>>>())).Returns((Expression<Func<DbModel.Data, bool>> predicate) => mockContractExpenseType.Where(predicate));
            mockinspectionTypeChargeRateRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.CompanyInspectionTypeChargeRate, bool>>>())).Returns((Expression<Func<DbModel.CompanyInspectionTypeChargeRate, bool>> predicate) => mockCompanyInspectionTypeChargeRate.Where(predicate));

            ContractScheduleRateService = new Service.ContractScheduleRateService(mockMapper.Object, mockLogger.Object, mockContractScheduleRateRepository.Object, mockContractRepository.Object, mockDbRepo.Object, mockContractScheduleRepository.Object, mockinspectionTypeChargeRateRepository.Object, ScheduleRateValidService);
            contractScheduleRateDomianModels.ToList().ForEach(x => x.RecordStatus = "M");

            var response = ContractScheduleRateService.ModifyContractScheduleRate(contractScheduleRateDomianModels[0].ContractNumber, contractScheduleRateDomianModels[0].ScheduleName, new List<DomModel.ContractScheduleRate> { contractScheduleRateDomianModels[0] });
            Assert.AreEqual("11", response.Code);
            Assert.IsNotNull(response.Messages);
            Assert.AreEqual("31", response.Messages.FirstOrDefault().Code);
        }

        [TestMethod]
        public void ThrowsExceptionWhileDeletingContractScheduleRate()
        {
            mockContractScheduleRateRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.ContractRate, bool>>>())).Throws(new Exception("Exception occured while performing some operation."));
            mockContractRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Contract, bool>>>())).Returns(mockContractDbData);
            mockContractScheduleRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.ContractSchedule, bool>>>())).Returns((Expression<Func<DbModel.ContractSchedule, bool>> predicate) => mockContractScheduleDbData.Where(predicate));
            mockDbRepo.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Data, bool>>>())).Returns((Expression<Func<DbModel.Data, bool>> predicate) => mockContractExpenseType.Where(predicate));
            mockContractScheduleRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.ContractSchedule, bool>>>())).Returns((Expression<Func<DbModel.ContractSchedule, bool>> predicate) => mockContractScheduleDbData.Where(predicate));
            ContractScheduleRateService = new Service.ContractScheduleRateService(mockMapper.Object, mockLogger.Object, mockContractScheduleRateRepository.Object, mockContractRepository.Object, mockDbRepo.Object, mockContractScheduleRepository.Object, mockinspectionTypeChargeRateRepository.Object, ScheduleRateValidService);
            contractScheduleRateDomianModels.ToList().ForEach(x => x.RecordStatus = "D");

            var response = ContractScheduleRateService.DeleteContractScheduleRate(contractScheduleRateDomianModels[0].ContractNumber, contractScheduleRateDomianModels[0].ScheduleName, new List<DomModel.ContractScheduleRate> { contractScheduleRateDomianModels[0] });
            Assert.AreEqual("11", response.Code);
            Assert.IsNotNull(response.Messages);
            Assert.AreEqual("31", response.Messages.FirstOrDefault().Code);
        }

        [TestMethod]
        public void GetAllDataAfterSavingContrcatScheduleRate()
        {
            mockContractScheduleRateRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.ContractRate, bool>>>())).Returns((Expression<Func<DbModel.ContractRate, bool>> predicate) => mockContractScheduleRateDbData.Where(predicate));
            mockContractRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Contract, bool>>>())).Returns(mockContractDbData);
            mockContractScheduleRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.ContractSchedule, bool>>>())).Returns((Expression<Func<DbModel.ContractSchedule, bool>> predicate) => mockContractScheduleDbData.Where(predicate));
            mockDbRepo.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Data, bool>>>())).Returns((Expression<Func<DbModel.Data, bool>> predicate) => mockContractExpenseType.Where(predicate));
            mockContractScheduleRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.ContractSchedule, bool>>>())).Returns((Expression<Func<DbModel.ContractSchedule, bool>> predicate) => mockContractScheduleDbData.Where(predicate));
            mockinspectionTypeChargeRateRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.CompanyInspectionTypeChargeRate, bool>>>())).Returns((Expression<Func<DbModel.CompanyInspectionTypeChargeRate, bool>> predicate) => mockCompanyInspectionTypeChargeRate.Where(predicate));

            ContractScheduleRateService = new Service.ContractScheduleRateService(mockMapper.Object, mockLogger.Object, mockContractScheduleRateRepository.Object, mockContractRepository.Object, mockDbRepo.Object, mockContractScheduleRepository.Object, mockinspectionTypeChargeRateRepository.Object, ScheduleRateValidService);
            contractScheduleRateDomianModels.ToList().ForEach(x => x.RecordStatus = "N");
            contractScheduleRateDomianModels[0].ScheduleName = "Norway";
            var response = ContractScheduleRateService.SaveContractScheduleRate(contractScheduleRateDomianModels[0].ContractNumber, contractScheduleRateDomianModels[0].ScheduleName, new List<DomModel.ContractScheduleRate> { contractScheduleRateDomianModels[0] },true,true);
            mockContractScheduleRateRepository.Verify(m => m.ForceSave(), Times.Once);
        }
        [TestMethod]
        public void GetAllDataAfterModifyingContrcatScheduleRate()
        {
            mockContractScheduleRateRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.ContractRate, bool>>>())).Returns((Expression<Func<DbModel.ContractRate, bool>> predicate) => mockContractScheduleRateDbData.Where(predicate));
            mockContractRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Contract, bool>>>())).Returns(mockContractDbData);
            mockContractScheduleRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.ContractSchedule, bool>>>())).Returns((Expression<Func<DbModel.ContractSchedule, bool>> predicate) => mockContractScheduleDbData.Where(predicate));
            mockDbRepo.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Data, bool>>>())).Returns((Expression<Func<DbModel.Data, bool>> predicate) => mockContractExpenseType.Where(predicate));
            mockinspectionTypeChargeRateRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.CompanyInspectionTypeChargeRate, bool>>>())).Returns((Expression<Func<DbModel.CompanyInspectionTypeChargeRate, bool>> predicate) => mockCompanyInspectionTypeChargeRate.Where(predicate));

            ContractScheduleRateService = new Service.ContractScheduleRateService(mockMapper.Object, mockLogger.Object, mockContractScheduleRateRepository.Object, mockContractRepository.Object, mockDbRepo.Object, mockContractScheduleRepository.Object, mockinspectionTypeChargeRateRepository.Object, ScheduleRateValidService);
            contractScheduleRateDomianModels.ToList().ForEach(x => x.RecordStatus = "M");

            var response = ContractScheduleRateService.ModifyContractScheduleRate(contractScheduleRateDomianModels[0].ContractNumber, contractScheduleRateDomianModels[0].ScheduleName, new List<DomModel.ContractScheduleRate> { contractScheduleRateDomianModels[0] },true,true);
            mockContractScheduleRateRepository.Verify(m => m.Update(It.IsAny<DbModel.ContractRate>()), Times.AtLeastOnce);
        }

        [TestMethod]
        public void GetAllDataAfterDeletingContrcatScheduleRate()
        {
            mockContractScheduleRateRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.ContractRate, bool>>>())).Returns((Expression<Func<DbModel.ContractRate, bool>> predicate) => mockContractScheduleRateDbData.Where(predicate));
            mockContractRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Contract, bool>>>())).Returns(mockContractDbData);
            mockContractScheduleRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.ContractSchedule, bool>>>())).Returns((Expression<Func<DbModel.ContractSchedule, bool>> predicate) => mockContractScheduleDbData.Where(predicate));
            mockDbRepo.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Data, bool>>>())).Returns((Expression<Func<DbModel.Data, bool>> predicate) => mockContractExpenseType.Where(predicate));
            mockContractScheduleRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.ContractSchedule, bool>>>())).Returns((Expression<Func<DbModel.ContractSchedule, bool>> predicate) => mockContractScheduleDbData.Where(predicate));
            ContractScheduleRateService = new Service.ContractScheduleRateService(mockMapper.Object, mockLogger.Object, mockContractScheduleRateRepository.Object, mockContractRepository.Object, mockDbRepo.Object, mockContractScheduleRepository.Object, mockinspectionTypeChargeRateRepository.Object, ScheduleRateValidService);
            contractScheduleRateDomianModels.ToList().ForEach(x => x.RecordStatus = "D");

            var response = ContractScheduleRateService.DeleteContractScheduleRate(contractScheduleRateDomianModels[0].ContractNumber, contractScheduleRateDomianModels[0].ScheduleName, new List<DomModel.ContractScheduleRate> { contractScheduleRateDomianModels[0] },true,true);
            mockContractScheduleRateRepository.Verify(m => m.Delete(It.IsAny<DbModel.ContractRate>()), Times.AtLeastOnce);
        }
        [TestMethod]
        public void ShouldNotSaveDataWhenContractNumberIsNull()
        {
            mockContractScheduleRateRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.ContractRate, bool>>>())).Returns((Expression<Func<DbModel.ContractRate, bool>> predicate) => mockContractScheduleRateDbData.Where(predicate));
            mockContractRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Contract, bool>>>())).Returns(mockContractDbData);
            mockContractScheduleRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.ContractSchedule, bool>>>())).Returns((Expression<Func<DbModel.ContractSchedule, bool>> predicate) => mockContractScheduleDbData.Where(predicate));
            mockDbRepo.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Data, bool>>>())).Returns((Expression<Func<DbModel.Data, bool>> predicate) => mockContractExpenseType.Where(predicate));
            mockContractScheduleRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.ContractSchedule, bool>>>())).Returns((Expression<Func<DbModel.ContractSchedule, bool>> predicate) => mockContractScheduleDbData.Where(predicate));
            mockinspectionTypeChargeRateRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.CompanyInspectionTypeChargeRate, bool>>>())).Returns((Expression<Func<DbModel.CompanyInspectionTypeChargeRate, bool>> predicate) => mockCompanyInspectionTypeChargeRate.Where(predicate));

            ContractScheduleRateService = new Service.ContractScheduleRateService(mockMapper.Object, mockLogger.Object, mockContractScheduleRateRepository.Object, mockContractRepository.Object, mockDbRepo.Object, mockContractScheduleRepository.Object, mockinspectionTypeChargeRateRepository.Object, ScheduleRateValidService);
            contractScheduleRateDomianModels.ToList().ForEach(x => x.RecordStatus = "N");
            contractScheduleRateDomianModels[0].ScheduleName = "Norway";
            var response = ContractScheduleRateService.SaveContractScheduleRate(null, contractScheduleRateDomianModels[0].ScheduleName, new List<DomModel.ContractScheduleRate> { contractScheduleRateDomianModels[0] }, true, true);
            Assert.AreEqual("11", response.Code);
        }
        [TestMethod]
        public void ShouldNotUpdatedataWhenContractNumberIsNull()
        {
            mockContractScheduleRateRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.ContractRate, bool>>>())).Returns((Expression<Func<DbModel.ContractRate, bool>> predicate) => mockContractScheduleRateDbData.Where(predicate));
            mockContractRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Contract, bool>>>())).Returns(mockContractDbData);
            mockContractScheduleRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.ContractSchedule, bool>>>())).Returns((Expression<Func<DbModel.ContractSchedule, bool>> predicate) => mockContractScheduleDbData.Where(predicate));
            mockDbRepo.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Data, bool>>>())).Returns((Expression<Func<DbModel.Data, bool>> predicate) => mockContractExpenseType.Where(predicate));
            mockinspectionTypeChargeRateRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.CompanyInspectionTypeChargeRate, bool>>>())).Returns((Expression<Func<DbModel.CompanyInspectionTypeChargeRate, bool>> predicate) => mockCompanyInspectionTypeChargeRate.Where(predicate));

            ContractScheduleRateService = new Service.ContractScheduleRateService(mockMapper.Object, mockLogger.Object, mockContractScheduleRateRepository.Object, mockContractRepository.Object, mockDbRepo.Object, mockContractScheduleRepository.Object, mockinspectionTypeChargeRateRepository.Object, ScheduleRateValidService);
            contractScheduleRateDomianModels.ToList().ForEach(x => x.RecordStatus = "M");

            var response = ContractScheduleRateService.ModifyContractScheduleRate(null, contractScheduleRateDomianModels[0].ScheduleName, new List<DomModel.ContractScheduleRate> { contractScheduleRateDomianModels[0] }, true, true);
            Assert.AreEqual("11", response.Code);
        }

        [TestMethod]
        public void ShouldNotSaveDataWhenContractIsInvalid()
        {
            mockContractScheduleRateRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.ContractRate, bool>>>())).Returns((Expression<Func<DbModel.ContractRate, bool>> predicate) => mockContractScheduleRateDbData.Where(predicate));
           // mockContractRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Contract, bool>>>())).Returns(mockContractDbData);
            mockContractScheduleRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.ContractSchedule, bool>>>())).Returns((Expression<Func<DbModel.ContractSchedule, bool>> predicate) => mockContractScheduleDbData.Where(predicate));
            mockDbRepo.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Data, bool>>>())).Returns((Expression<Func<DbModel.Data, bool>> predicate) => mockContractExpenseType.Where(predicate));
            mockContractScheduleRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.ContractSchedule, bool>>>())).Returns((Expression<Func<DbModel.ContractSchedule, bool>> predicate) => mockContractScheduleDbData.Where(predicate));
            mockinspectionTypeChargeRateRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.CompanyInspectionTypeChargeRate, bool>>>())).Returns((Expression<Func<DbModel.CompanyInspectionTypeChargeRate, bool>> predicate) => mockCompanyInspectionTypeChargeRate.Where(predicate));

            ContractScheduleRateService = new Service.ContractScheduleRateService(mockMapper.Object, mockLogger.Object, mockContractScheduleRateRepository.Object, mockContractRepository.Object, mockDbRepo.Object, mockContractScheduleRepository.Object, mockinspectionTypeChargeRateRepository.Object, ScheduleRateValidService);
            contractScheduleRateDomianModels.ToList().ForEach(x => x.RecordStatus = "N");
            contractScheduleRateDomianModels[0].ScheduleName = "Norway";
            var response = ContractScheduleRateService.SaveContractScheduleRate(contractScheduleRateDomianModels[0].ContractNumber, contractScheduleRateDomianModels[0].ScheduleName, new List<DomModel.ContractScheduleRate> { contractScheduleRateDomianModels[0] }, true, true);
            Assert.AreEqual("11", response.Code);
        }
        [TestMethod]
        public void ShouldNotUpdateDataWhenContractIsInvalid()
        {
            mockContractScheduleRateRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.ContractRate, bool>>>())).Returns((Expression<Func<DbModel.ContractRate, bool>> predicate) => mockContractScheduleRateDbData.Where(predicate));
          // mockContractRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Contract, bool>>>())).Returns(mockContractDbData);
            mockContractScheduleRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.ContractSchedule, bool>>>())).Returns((Expression<Func<DbModel.ContractSchedule, bool>> predicate) => mockContractScheduleDbData.Where(predicate));
            mockDbRepo.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Data, bool>>>())).Returns((Expression<Func<DbModel.Data, bool>> predicate) => mockContractExpenseType.Where(predicate));
            mockinspectionTypeChargeRateRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.CompanyInspectionTypeChargeRate, bool>>>())).Returns((Expression<Func<DbModel.CompanyInspectionTypeChargeRate, bool>> predicate) => mockCompanyInspectionTypeChargeRate.Where(predicate));

            ContractScheduleRateService = new Service.ContractScheduleRateService(mockMapper.Object, mockLogger.Object, mockContractScheduleRateRepository.Object, mockContractRepository.Object, mockDbRepo.Object, mockContractScheduleRepository.Object, mockinspectionTypeChargeRateRepository.Object, ScheduleRateValidService);
            contractScheduleRateDomianModels.ToList().ForEach(x => x.RecordStatus = "M");

            var response = ContractScheduleRateService.ModifyContractScheduleRate(contractScheduleRateDomianModels[0].ContractNumber, contractScheduleRateDomianModels[0].ScheduleName, new List<DomModel.ContractScheduleRate> { contractScheduleRateDomianModels[0] }, true, true);
            Assert.AreEqual("11", response.Code);
        }
        [TestMethod]
        public void ShouldNotSaveDataWithExsitanceSchedule()
        {
            mockContractScheduleRateRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.ContractRate, bool>>>())).Returns((Expression<Func<DbModel.ContractRate, bool>> predicate) => mockContractScheduleRateDbData.Where(predicate));
             mockContractRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Contract, bool>>>())).Returns(mockContractDbData);
            mockContractScheduleRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.ContractSchedule, bool>>>())).Returns((Expression<Func<DbModel.ContractSchedule, bool>> predicate) => mockContractScheduleDbData.Where(predicate));
            mockDbRepo.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Data, bool>>>())).Returns((Expression<Func<DbModel.Data, bool>> predicate) => mockContractExpenseType.Where(predicate));
            mockContractScheduleRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.ContractSchedule, bool>>>())).Returns((Expression<Func<DbModel.ContractSchedule, bool>> predicate) => mockContractScheduleDbData.Where(predicate));
            mockinspectionTypeChargeRateRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.CompanyInspectionTypeChargeRate, bool>>>())).Returns((Expression<Func<DbModel.CompanyInspectionTypeChargeRate, bool>> predicate) => mockCompanyInspectionTypeChargeRate.Where(predicate));

            ContractScheduleRateService = new Service.ContractScheduleRateService(mockMapper.Object, mockLogger.Object, mockContractScheduleRateRepository.Object, mockContractRepository.Object, mockDbRepo.Object, mockContractScheduleRepository.Object, mockinspectionTypeChargeRateRepository.Object, ScheduleRateValidService);
            contractScheduleRateDomianModels.ToList().ForEach(x => x.RecordStatus = "N");
            contractScheduleRateDomianModels[0].ScheduleName = "ABC";
            var response = ContractScheduleRateService.SaveContractScheduleRate(contractScheduleRateDomianModels[0].ContractNumber, contractScheduleRateDomianModels[0].ScheduleName, new List<DomModel.ContractScheduleRate> { contractScheduleRateDomianModels[0] }, true, true);
            Assert.AreEqual("11", response.Code);
        }
        [TestMethod]
        public void ValidationOfChargeTypeWhileSave()
        {
            mockContractScheduleRateRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.ContractRate, bool>>>())).Returns((Expression<Func<DbModel.ContractRate, bool>> predicate) => mockContractScheduleRateDbData.Where(predicate));
            mockContractRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Contract, bool>>>())).Returns(mockContractDbData);
            mockContractScheduleRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.ContractSchedule, bool>>>())).Returns((Expression<Func<DbModel.ContractSchedule, bool>> predicate) => mockContractScheduleDbData.Where(predicate));
            mockDbRepo.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Data, bool>>>())).Returns((Expression<Func<DbModel.Data, bool>> predicate) => mockContractExpenseType.Where(predicate));
            mockContractScheduleRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.ContractSchedule, bool>>>())).Returns((Expression<Func<DbModel.ContractSchedule, bool>> predicate) => mockContractScheduleDbData.Where(predicate));
            mockinspectionTypeChargeRateRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.CompanyInspectionTypeChargeRate, bool>>>())).Returns((Expression<Func<DbModel.CompanyInspectionTypeChargeRate, bool>> predicate) => mockCompanyInspectionTypeChargeRate.Where(predicate));

            ContractScheduleRateService = new Service.ContractScheduleRateService(mockMapper.Object, mockLogger.Object, mockContractScheduleRateRepository.Object, mockContractRepository.Object, mockDbRepo.Object, mockContractScheduleRepository.Object, mockinspectionTypeChargeRateRepository.Object, ScheduleRateValidService);
            contractScheduleRateDomianModels.ToList().ForEach(x => x.RecordStatus = "N");
            contractScheduleRateDomianModels[0].ScheduleName = "Norway";
            contractScheduleRateDomianModels[0].ChargeType = "Norway";


            var response = ContractScheduleRateService.SaveContractScheduleRate(contractScheduleRateDomianModels[0].ContractNumber, contractScheduleRateDomianModels[0].ScheduleName, new List<DomModel.ContractScheduleRate> { contractScheduleRateDomianModels[0] }, true, true);
            Assert.AreEqual("11", response.Code);
        }
        [TestMethod]
        public void ValidationOfChargeTypeWhileUpdate ()
        {
            mockContractScheduleRateRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.ContractRate, bool>>>())).Returns((Expression<Func<DbModel.ContractRate, bool>> predicate) => mockContractScheduleRateDbData.Where(predicate));
            mockContractRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Contract, bool>>>())).Returns(mockContractDbData);
            mockContractScheduleRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.ContractSchedule, bool>>>())).Returns((Expression<Func<DbModel.ContractSchedule, bool>> predicate) => mockContractScheduleDbData.Where(predicate));
            mockDbRepo.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Data, bool>>>())).Returns((Expression<Func<DbModel.Data, bool>> predicate) => mockContractExpenseType.Where(predicate));
            mockContractScheduleRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.ContractSchedule, bool>>>())).Returns((Expression<Func<DbModel.ContractSchedule, bool>> predicate) => mockContractScheduleDbData.Where(predicate));
            mockinspectionTypeChargeRateRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.CompanyInspectionTypeChargeRate, bool>>>())).Returns((Expression<Func<DbModel.CompanyInspectionTypeChargeRate, bool>> predicate) => mockCompanyInspectionTypeChargeRate.Where(predicate));

            ContractScheduleRateService = new Service.ContractScheduleRateService(mockMapper.Object, mockLogger.Object, mockContractScheduleRateRepository.Object, mockContractRepository.Object, mockDbRepo.Object, mockContractScheduleRepository.Object, mockinspectionTypeChargeRateRepository.Object, ScheduleRateValidService);
            contractScheduleRateDomianModels.ToList().ForEach(x => x.RecordStatus = "M");
           
            contractScheduleRateDomianModels[0].ChargeType = "Norway";


            var response = ContractScheduleRateService.ModifyContractScheduleRate(contractScheduleRateDomianModels[0].ContractNumber, contractScheduleRateDomianModels[0].ScheduleName, new List<DomModel.ContractScheduleRate> { contractScheduleRateDomianModels[0] }, true, true);
            Assert.AreEqual("11", response.Code);
        }

        [TestMethod]
        public void ValidationOfRecordIdWhileUpdating()
        {
            mockContractScheduleRateRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.ContractRate, bool>>>())).Returns((Expression<Func<DbModel.ContractRate, bool>> predicate) => mockContractScheduleRateDbData.Where(predicate));
            mockContractRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Contract, bool>>>())).Returns(mockContractDbData);
            mockContractScheduleRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.ContractSchedule, bool>>>())).Returns((Expression<Func<DbModel.ContractSchedule, bool>> predicate) => mockContractScheduleDbData.Where(predicate));
            mockDbRepo.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Data, bool>>>())).Returns((Expression<Func<DbModel.Data, bool>> predicate) => mockContractExpenseType.Where(predicate));
            mockContractScheduleRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.ContractSchedule, bool>>>())).Returns((Expression<Func<DbModel.ContractSchedule, bool>> predicate) => mockContractScheduleDbData.Where(predicate));
            mockinspectionTypeChargeRateRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.CompanyInspectionTypeChargeRate, bool>>>())).Returns((Expression<Func<DbModel.CompanyInspectionTypeChargeRate, bool>> predicate) => mockCompanyInspectionTypeChargeRate.Where(predicate));

            ContractScheduleRateService = new Service.ContractScheduleRateService(mockMapper.Object, mockLogger.Object, mockContractScheduleRateRepository.Object, mockContractRepository.Object, mockDbRepo.Object, mockContractScheduleRepository.Object, mockinspectionTypeChargeRateRepository.Object, ScheduleRateValidService);
            contractScheduleRateDomianModels.ToList().ForEach(x => x.RecordStatus = "M");
            contractScheduleRateDomianModels[0].ScheduleName = "Norway";
            contractScheduleRateDomianModels[0].RateId = 5;
            var response = ContractScheduleRateService.ModifyContractScheduleRate(contractScheduleRateDomianModels[0].ContractNumber, contractScheduleRateDomianModels[0].ScheduleName, new List<DomModel.ContractScheduleRate> { contractScheduleRateDomianModels[0] });
            Assert.AreEqual("11", response.Code);
        }
        [TestMethod]
        public void ValidationOfRecordIdWhileDeleting()
        {
            mockContractScheduleRateRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.ContractRate, bool>>>())).Returns((Expression<Func<DbModel.ContractRate, bool>> predicate) => mockContractScheduleRateDbData.Where(predicate));
            mockContractRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Contract, bool>>>())).Returns(mockContractDbData);
            mockContractScheduleRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.ContractSchedule, bool>>>())).Returns((Expression<Func<DbModel.ContractSchedule, bool>> predicate) => mockContractScheduleDbData.Where(predicate));
            mockDbRepo.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Data, bool>>>())).Returns((Expression<Func<DbModel.Data, bool>> predicate) => mockContractExpenseType.Where(predicate));
            mockinspectionTypeChargeRateRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.CompanyInspectionTypeChargeRate, bool>>>())).Returns((Expression<Func<DbModel.CompanyInspectionTypeChargeRate, bool>> predicate) => mockCompanyInspectionTypeChargeRate.Where(predicate));

            ContractScheduleRateService = new Service.ContractScheduleRateService(mockMapper.Object, mockLogger.Object, mockContractScheduleRateRepository.Object, mockContractRepository.Object, mockDbRepo.Object, mockContractScheduleRepository.Object, mockinspectionTypeChargeRateRepository.Object, ScheduleRateValidService);
            contractScheduleRateDomianModels.ToList().ForEach(x => x.RecordStatus = "D");
                 contractScheduleRateDomianModels[0].RateId = 5;
            var response = ContractScheduleRateService.DeleteContractScheduleRate(contractScheduleRateDomianModels[0].ContractNumber, contractScheduleRateDomianModels[0].ScheduleName, new List<DomModel.ContractScheduleRate> { contractScheduleRateDomianModels[0] });
            Assert.AreEqual("11", response.Code);
        }
        [TestMethod]
        public void ValidationOfUpdateCount()
        {
            mockContractScheduleRateRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.ContractRate, bool>>>())).Returns((Expression<Func<DbModel.ContractRate, bool>> predicate) => mockContractScheduleRateDbData.Where(predicate));
            mockContractRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Contract, bool>>>())).Returns(mockContractDbData);
            mockContractScheduleRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.ContractSchedule, bool>>>())).Returns((Expression<Func<DbModel.ContractSchedule, bool>> predicate) => mockContractScheduleDbData.Where(predicate));
            mockDbRepo.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Data, bool>>>())).Returns((Expression<Func<DbModel.Data, bool>> predicate) => mockContractExpenseType.Where(predicate));
            mockContractScheduleRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.ContractSchedule, bool>>>())).Returns((Expression<Func<DbModel.ContractSchedule, bool>> predicate) => mockContractScheduleDbData.Where(predicate));
            mockinspectionTypeChargeRateRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.CompanyInspectionTypeChargeRate, bool>>>())).Returns((Expression<Func<DbModel.CompanyInspectionTypeChargeRate, bool>> predicate) => mockCompanyInspectionTypeChargeRate.Where(predicate));

            ContractScheduleRateService = new Service.ContractScheduleRateService(mockMapper.Object, mockLogger.Object, mockContractScheduleRateRepository.Object, mockContractRepository.Object, mockDbRepo.Object, mockContractScheduleRepository.Object, mockinspectionTypeChargeRateRepository.Object, ScheduleRateValidService);
            contractScheduleRateDomianModels.ToList().ForEach(x => x.RecordStatus = "M");
            contractScheduleRateDomianModels[0].ScheduleName = "Norway";
            contractScheduleRateDomianModels[0].UpdateCount = 5;
            var response = ContractScheduleRateService.ModifyContractScheduleRate(contractScheduleRateDomianModels[0].ContractNumber, contractScheduleRateDomianModels[0].ScheduleName, new List<DomModel.ContractScheduleRate> { contractScheduleRateDomianModels[0] });
            Assert.AreEqual("11", response.Code);
        }
    }
}

