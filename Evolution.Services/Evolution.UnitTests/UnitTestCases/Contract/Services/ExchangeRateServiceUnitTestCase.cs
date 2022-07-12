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
using MasterService = Evolution.Master.Domain.Interfaces;
using Evolution.Common.Enums;
using dbRepo = Evolution.DbRepository.Interfaces.Master;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Service = Evolution.Contract.Core.Services;
using Evolution.UnitTest.Mocks.Data.Masters.Db.Masters;
using Evolution.UnitTest.UnitTestCases;
using ValidService = Evolution.Contract.Domain.Interfaces.Validations;

namespace Evolution.UnitTests.UnitTestCases.Contract.Services
{
    [TestClass]
    public class ExchangeRateServiceUnitTestCase : BaseTestCase
    {
        ServiceDomainData.IContractExchangeRateService ContractExchangeRateService = null;
        Mock<DomainData.IContractExchangeRateRepository> mockContractExchangeRateRepository = null;
        Mock<DomainData.IContractRepository> mockContractRepository = null;
        Mock<dbRepo.IDataRepository> mockdbRepo = null;
        IList<DomModel.ContractExchangeRate> contractExchangeRateDomianModels = null;
        Mock<IAppLogger<Evolution.Contract.Core.Services.ContractExchangeRateService>> mockLogger = null;
        IQueryable<DbModel.Contract> mockContractDbData = null;
        IQueryable<DbModel.ContractExchangeRate> mockContractExchangeRateDbData = null;
        IQueryable<DbModel.Data> mockCurrency = null;
        ValidService.IContractExchangeRateValidationService ExchangeRateValidService = null;


        [TestInitialize]
        public void InitializeContractExchangeRateService()
        {
            mockContractExchangeRateRepository = new Mock<DomainData.IContractExchangeRateRepository>();
            mockContractRepository = new Mock<DomainData.IContractRepository>();
            contractExchangeRateDomianModels = MockContractDomainModel.GetDomainModelContractExchangeRates();
            mockLogger = new Mock<IAppLogger<Service.ContractExchangeRateService>>();
            mockContractDbData = MockContract.GetDbModelContract();
            mockContractExchangeRateDbData = MockContract.GetContratctExchangeRates();
            mockdbRepo = new Mock<dbRepo.IDataRepository>();
            mockCurrency = MockMaster.GetCurrencyMockData();


        }

        [TestMethod]
        public void FetchAllContractExchangeRateWithoutSearchValue()
        {
            mockContractExchangeRateRepository.Setup(x => x.Search(It.IsAny<DomModel.ContractExchangeRate>())).Returns(contractExchangeRateDomianModels);
            ContractExchangeRateService = new Service.ContractExchangeRateService(Mapper.Instance, mockLogger.Object, mockContractExchangeRateRepository.Object, mockdbRepo.Object, mockContractRepository.Object, ExchangeRateValidService);
            var response = ContractExchangeRateService.GetContractExchangeRate(new DomModel.ContractExchangeRate());
            Assert.AreEqual("1", response.Code);
            Assert.AreEqual(2, (response.Result as List<DomModel.ContractExchangeRate>).Count);
        }

        [TestMethod]
        public void FetchAllContractExchangeRateWithSearchValue()
        {
            mockContractExchangeRateRepository.Setup(x => x.Search(It.Is<DomModel.ContractExchangeRate>(c => c.ContractNumber == "SU0001/0001"))).Returns(new List<DomModel.ContractExchangeRate> { contractExchangeRateDomianModels[0] });
            ContractExchangeRateService = new Service.ContractExchangeRateService(Mapper.Instance, mockLogger.Object, mockContractExchangeRateRepository.Object, mockdbRepo.Object, mockContractRepository.Object, ExchangeRateValidService);
            var response = ContractExchangeRateService.GetContractExchangeRate(new DomModel.ContractExchangeRate { ContractNumber = "SU0001/0001" });


            Assert.AreEqual("1", response.Code);
            Assert.AreEqual(1, (response.Result as List<DomModel.ContractExchangeRate>).Count);
        }

        [TestMethod]
        public void FetchContractExchangeRateListWithNullSearchModel()
        {
            mockContractExchangeRateRepository.Setup(x => x.Search(It.IsAny<DomModel.ContractExchangeRate>())).Returns(contractExchangeRateDomianModels);

            ContractExchangeRateService = new Service.ContractExchangeRateService(Mapper.Instance, mockLogger.Object, mockContractExchangeRateRepository.Object, mockdbRepo.Object, mockContractRepository.Object, ExchangeRateValidService);

            var response = ContractExchangeRateService.GetContractExchangeRate(null);

            Assert.AreEqual("1", response.Code);
            Assert.AreEqual(2, (response.Result as List<DomModel.ContractExchangeRate>).Count);
        }

        [TestMethod]
        public void ThrowsExceptionWhileFetchingExchangeRate()
        {
            mockContractExchangeRateRepository.Setup(x => x.Search(It.IsAny<DomModel.ContractExchangeRate>())).Throws(new Exception("Exception occured while performing some operation."));
            ContractExchangeRateService = new Service.ContractExchangeRateService(Mapper.Instance, mockLogger.Object, mockContractExchangeRateRepository.Object, mockdbRepo.Object, mockContractRepository.Object, ExchangeRateValidService);

            var response = ContractExchangeRateService.GetContractExchangeRate(new DomModel.ContractExchangeRate());

            Assert.AreEqual("11", response.Code);
            Assert.IsNotNull(response.Messages);
            Assert.AreEqual("31", response.Messages.FirstOrDefault().Code);
        }

        [TestMethod]
        public void SaveContractExchangeRate()
        {

            mockContractRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Contract, bool>>>())).Returns(mockContractDbData);
            mockContractExchangeRateRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.ContractExchangeRate, bool>>>())).Returns((Expression<Func<DbModel.ContractExchangeRate, bool>> predicate) => mockContractExchangeRateDbData.Where(predicate));
            mockdbRepo.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Data, bool>>>())).Returns((Expression<Func<DbModel.Data, bool>> predicate) => mockCurrency.Where(predicate));

            ContractExchangeRateService = new Service.ContractExchangeRateService(Mapper.Instance, mockLogger.Object, mockContractExchangeRateRepository.Object, mockdbRepo.Object, mockContractRepository.Object, ExchangeRateValidService);
            contractExchangeRateDomianModels.ToList().ForEach(x => x.RecordStatus = "N");
            contractExchangeRateDomianModels[0].FromCurrency = "ALL";

            var response = ContractExchangeRateService.SaveContractExchangeRate(contractExchangeRateDomianModels[0].ContractNumber, new List<DomModel.ContractExchangeRate> { contractExchangeRateDomianModels[0] });
            Assert.AreEqual("1", response.Code);
            mockContractExchangeRateRepository.Verify(m => m.ForceSave(), Times.Once);
        }

        [TestMethod]
        public void ModifyContractExchangeRate()
        {

            mockContractRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Contract, bool>>>())).Returns(mockContractDbData);
            mockContractExchangeRateRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.ContractExchangeRate, bool>>>())).Returns((Expression<Func<DbModel.ContractExchangeRate, bool>> predicate) => mockContractExchangeRateDbData.Where(predicate));
            mockdbRepo.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Data, bool>>>())).Returns((Expression<Func<DbModel.Data, bool>> predicate) => mockCurrency.Where(predicate));

            ContractExchangeRateService = new Service.ContractExchangeRateService(Mapper.Instance, mockLogger.Object, mockContractExchangeRateRepository.Object, mockdbRepo.Object, mockContractRepository.Object, ExchangeRateValidService);
            contractExchangeRateDomianModels.ToList().ForEach(x => x.RecordStatus = "M");
            var response = ContractExchangeRateService.ModifyContractExchangeRate(contractExchangeRateDomianModels[0].ContractNumber, new List<DomModel.ContractExchangeRate> { contractExchangeRateDomianModels[0] });
            mockContractExchangeRateRepository.Verify(m => m.Update(It.IsAny<DbModel.ContractExchangeRate>()), Times.AtLeastOnce);
        }

        [TestMethod]
        public void DeleteContractExchangeRate()
        {

            mockContractRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Contract, bool>>>())).Returns(mockContractDbData);
            mockContractExchangeRateRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.ContractExchangeRate, bool>>>())).Returns((Expression<Func<DbModel.ContractExchangeRate, bool>> predicate) => mockContractExchangeRateDbData.Where(predicate));
            mockdbRepo.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Data, bool>>>())).Returns((Expression<Func<DbModel.Data, bool>> predicate) => mockCurrency.Where(predicate));

            ContractExchangeRateService = new Service.ContractExchangeRateService(Mapper.Instance, mockLogger.Object, mockContractExchangeRateRepository.Object, mockdbRepo.Object, mockContractRepository.Object, ExchangeRateValidService);
            contractExchangeRateDomianModels.ToList().ForEach(x => x.RecordStatus = "D");
            var response = ContractExchangeRateService.DeleteContractExchangeRate(contractExchangeRateDomianModels[0].ContractNumber, new List<DomModel.ContractExchangeRate> { contractExchangeRateDomianModels[0] });
            mockContractExchangeRateRepository.Verify(m => m.Delete(It.IsAny<DbModel.ContractExchangeRate>()), Times.AtLeastOnce);
        }


        [TestMethod]
        public void SaveContractExchangeRateWithInvalidCurrency()
        {

            mockContractRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Contract, bool>>>())).Returns(mockContractDbData);
            mockContractExchangeRateRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.ContractExchangeRate, bool>>>())).Returns((Expression<Func<DbModel.ContractExchangeRate, bool>> predicate) => mockContractExchangeRateDbData.Where(predicate));
            mockdbRepo.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Data, bool>>>())).Returns((Expression<Func<DbModel.Data, bool>> predicate) => mockCurrency.Where(predicate));

            ContractExchangeRateService = new Service.ContractExchangeRateService(Mapper.Instance, mockLogger.Object, mockContractExchangeRateRepository.Object, mockdbRepo.Object, mockContractRepository.Object, ExchangeRateValidService);
            contractExchangeRateDomianModels.ToList().ForEach(x => x.RecordStatus = "N");
            contractExchangeRateDomianModels[0].FromCurrency = "ABC";
            contractExchangeRateDomianModels[0].ToCurrency = "DEF";
            var response = ContractExchangeRateService.SaveContractExchangeRate(contractExchangeRateDomianModels[0].ContractNumber, new List<DomModel.ContractExchangeRate> { contractExchangeRateDomianModels[0] });
             Assert.AreEqual("11", response.Code);
        }
        [TestMethod]
        public void ModifyContractExchangeRateWithInvalidCurrency()
        {

            mockContractRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Contract, bool>>>())).Returns(mockContractDbData);
            mockContractExchangeRateRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.ContractExchangeRate, bool>>>())).Returns((Expression<Func<DbModel.ContractExchangeRate, bool>> predicate) => mockContractExchangeRateDbData.Where(predicate));
            mockdbRepo.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Data, bool>>>())).Returns((Expression<Func<DbModel.Data, bool>> predicate) => mockCurrency.Where(predicate));

            ContractExchangeRateService = new Service.ContractExchangeRateService(Mapper.Instance, mockLogger.Object, mockContractExchangeRateRepository.Object, mockdbRepo.Object, mockContractRepository.Object, ExchangeRateValidService);
            contractExchangeRateDomianModels.ToList().ForEach(x => x.RecordStatus = "M");
            contractExchangeRateDomianModels[0].FromCurrency = "ABC";
            contractExchangeRateDomianModels[0].ToCurrency = "DEF";
            var response = ContractExchangeRateService.ModifyContractExchangeRate(contractExchangeRateDomianModels[0].ContractNumber, new List<DomModel.ContractExchangeRate> { contractExchangeRateDomianModels[0] });
            Assert.AreEqual("11", response.Code);
        }

        [TestMethod]
        public void  ShouldNotSaveContractExchangeRateWithNullContractNumber()
        {

           // mockContractRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Contract, bool>>>())).Returns(mockContractDbData);
            mockContractExchangeRateRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.ContractExchangeRate, bool>>>())).Returns((Expression<Func<DbModel.ContractExchangeRate, bool>> predicate) => mockContractExchangeRateDbData.Where(predicate));
            mockdbRepo.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Data, bool>>>())).Returns((Expression<Func<DbModel.Data, bool>> predicate) => mockCurrency.Where(predicate));

            ContractExchangeRateService = new Service.ContractExchangeRateService(Mapper.Instance, mockLogger.Object, mockContractExchangeRateRepository.Object, mockdbRepo.Object, mockContractRepository.Object, ExchangeRateValidService);
            contractExchangeRateDomianModels.ToList().ForEach(x => x.RecordStatus = "N");
          
            var response = ContractExchangeRateService.SaveContractExchangeRate(contractExchangeRateDomianModels[0].ContractNumber, new List<DomModel.ContractExchangeRate> { contractExchangeRateDomianModels[0] });
            Assert.AreEqual("11", response.Code);
        }


        [TestMethod]
        public void ShouldNotUpdateContractExchangeRateWithNullContractNumber()
        {

            // mockContractRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Contract, bool>>>())).Returns(mockContractDbData);
            mockContractExchangeRateRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.ContractExchangeRate, bool>>>())).Returns((Expression<Func<DbModel.ContractExchangeRate, bool>> predicate) => mockContractExchangeRateDbData.Where(predicate));
            mockdbRepo.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Data, bool>>>())).Returns((Expression<Func<DbModel.Data, bool>> predicate) => mockCurrency.Where(predicate));

            ContractExchangeRateService = new Service.ContractExchangeRateService(Mapper.Instance, mockLogger.Object, mockContractExchangeRateRepository.Object, mockdbRepo.Object, mockContractRepository.Object, ExchangeRateValidService);
            contractExchangeRateDomianModels.ToList().ForEach(x => x.RecordStatus = "M");
           var response = ContractExchangeRateService.ModifyContractExchangeRate(contractExchangeRateDomianModels[0].ContractNumber, new List<DomModel.ContractExchangeRate> { contractExchangeRateDomianModels[0] });
            Assert.AreEqual("11", response.Code);
        }

        [TestMethod]
        public void DeleteContractExchangeRateWith()
        {

            mockContractRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Contract, bool>>>())).Returns(mockContractDbData);
            //mockContractExchangeRateRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.ContractExchangeRate, bool>>>())).Returns((Expression<Func<DbModel.ContractExchangeRate, bool>> predicate) => mockContractExchangeRateDbData.Where(predicate));
            mockdbRepo.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Data, bool>>>())).Returns((Expression<Func<DbModel.Data, bool>> predicate) => mockCurrency.Where(predicate));

            ContractExchangeRateService = new Service.ContractExchangeRateService(Mapper.Instance, mockLogger.Object, mockContractExchangeRateRepository.Object, mockdbRepo.Object, mockContractRepository.Object, ExchangeRateValidService);
            contractExchangeRateDomianModels.ToList().ForEach(x => x.RecordStatus = "D");
            var response = ContractExchangeRateService.DeleteContractExchangeRate(contractExchangeRateDomianModels[0].ContractNumber, new List<DomModel.ContractExchangeRate> { contractExchangeRateDomianModels[0] });
            Assert.AreEqual("11", response.Code);
        }

        [TestMethod]
        public void ThrowsExceptionWhileSavingExchangeRate()
        {
            mockContractRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Contract, bool>>>())).Throws(new Exception("Exception occured while performing some operation."));
            mockContractExchangeRateRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.ContractExchangeRate, bool>>>())).Returns((Expression<Func<DbModel.ContractExchangeRate, bool>> predicate) => mockContractExchangeRateDbData.Where(predicate));
            mockdbRepo.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Data, bool>>>())).Returns((Expression<Func<DbModel.Data, bool>> predicate) => mockCurrency.Where(predicate));
            ContractExchangeRateService = new Service.ContractExchangeRateService(Mapper.Instance, mockLogger.Object, mockContractExchangeRateRepository.Object, mockdbRepo.Object, mockContractRepository.Object, ExchangeRateValidService);
            contractExchangeRateDomianModels.ToList().ForEach(x => x.RecordStatus = "N");
            var response = ContractExchangeRateService.SaveContractExchangeRate(contractExchangeRateDomianModels[0].ContractNumber, new List<DomModel.ContractExchangeRate> { contractExchangeRateDomianModels[0] });
           

            Assert.AreEqual("11", response.Code);
            Assert.IsNotNull(response.Messages);
            Assert.AreEqual("31", response.Messages.FirstOrDefault().Code);
        }

        [TestMethod]
        public void ThrowsExceptionWhileUpdatingExchangeRate()
        {
            mockContractRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Contract, bool>>>())).Throws(new Exception("Exception occured while performing some operation."));
            mockContractExchangeRateRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.ContractExchangeRate, bool>>>())).Returns((Expression<Func<DbModel.ContractExchangeRate, bool>> predicate) => mockContractExchangeRateDbData.Where(predicate));
            mockdbRepo.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Data, bool>>>())).Returns((Expression<Func<DbModel.Data, bool>> predicate) => mockCurrency.Where(predicate));

            ContractExchangeRateService = new Service.ContractExchangeRateService(Mapper.Instance, mockLogger.Object, mockContractExchangeRateRepository.Object, mockdbRepo.Object, mockContractRepository.Object, ExchangeRateValidService);
            contractExchangeRateDomianModels.ToList().ForEach(x => x.RecordStatus = "M");
            var response = ContractExchangeRateService.ModifyContractExchangeRate(contractExchangeRateDomianModels[0].ContractNumber, new List<DomModel.ContractExchangeRate> { contractExchangeRateDomianModels[0] });

            Assert.AreEqual("11", response.Code);
            Assert.IsNotNull(response.Messages);
            Assert.AreEqual("31", response.Messages.FirstOrDefault().Code);
        }

        [TestMethod]
        public void ThrowsExceptionWhileDeletingExchangeRate()
        {
            mockContractRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Contract, bool>>>())).Throws(new Exception("Exception occured while performing some operation."));
            mockContractExchangeRateRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.ContractExchangeRate, bool>>>())).Returns((Expression<Func<DbModel.ContractExchangeRate, bool>> predicate) => mockContractExchangeRateDbData.Where(predicate));
            mockdbRepo.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Data, bool>>>())).Returns((Expression<Func<DbModel.Data, bool>> predicate) => mockCurrency.Where(predicate));

            ContractExchangeRateService = new Service.ContractExchangeRateService(Mapper.Instance, mockLogger.Object, mockContractExchangeRateRepository.Object, mockdbRepo.Object, mockContractRepository.Object, ExchangeRateValidService);
            contractExchangeRateDomianModels.ToList().ForEach(x => x.RecordStatus = "D");
            var response = ContractExchangeRateService.DeleteContractExchangeRate(contractExchangeRateDomianModels[0].ContractNumber, new List<DomModel.ContractExchangeRate> { contractExchangeRateDomianModels[0] });

            Assert.AreEqual("11", response.Code);
            Assert.IsNotNull(response.Messages);
            Assert.AreEqual("31", response.Messages.FirstOrDefault().Code);
        }

        [TestMethod]
        public void GetAllExchangeRateAfterSave()
        {

            mockContractRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Contract, bool>>>())).Returns(mockContractDbData);
            mockContractExchangeRateRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.ContractExchangeRate, bool>>>())).Returns((Expression<Func<DbModel.ContractExchangeRate, bool>> predicate) => mockContractExchangeRateDbData.Where(predicate));
            mockdbRepo.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Data, bool>>>())).Returns((Expression<Func<DbModel.Data, bool>> predicate) => mockCurrency.Where(predicate));

            ContractExchangeRateService = new Service.ContractExchangeRateService(Mapper.Instance, mockLogger.Object, mockContractExchangeRateRepository.Object, mockdbRepo.Object, mockContractRepository.Object, ExchangeRateValidService);
            contractExchangeRateDomianModels.ToList().ForEach(x => x.RecordStatus = "N");
            contractExchangeRateDomianModels[0].FromCurrency = "ALL";

            var response = ContractExchangeRateService.SaveContractExchangeRate(contractExchangeRateDomianModels[0].ContractNumber, new List<DomModel.ContractExchangeRate> { contractExchangeRateDomianModels[0] },true,true);
            Assert.AreEqual("1", response.Code);
            mockContractExchangeRateRepository.Verify(m => m.ForceSave(), Times.Once);
        }

        [TestMethod]
        public void GetAllExchangeRateAfterModify()
        {

            mockContractRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Contract, bool>>>())).Returns(mockContractDbData);
            mockContractExchangeRateRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.ContractExchangeRate, bool>>>())).Returns((Expression<Func<DbModel.ContractExchangeRate, bool>> predicate) => mockContractExchangeRateDbData.Where(predicate));
            mockdbRepo.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Data, bool>>>())).Returns((Expression<Func<DbModel.Data, bool>> predicate) => mockCurrency.Where(predicate));

            ContractExchangeRateService = new Service.ContractExchangeRateService(Mapper.Instance, mockLogger.Object, mockContractExchangeRateRepository.Object, mockdbRepo.Object, mockContractRepository.Object, ExchangeRateValidService);
            contractExchangeRateDomianModels.ToList().ForEach(x => x.RecordStatus = "M");
            var response = ContractExchangeRateService.ModifyContractExchangeRate(contractExchangeRateDomianModels[0].ContractNumber, new List<DomModel.ContractExchangeRate> { contractExchangeRateDomianModels[0] },true,true);
            mockContractExchangeRateRepository.Verify(m => m.Update(It.IsAny<DbModel.ContractExchangeRate>()), Times.AtLeastOnce);
        }

        [TestMethod]
        public void GetAllExchangeRateAfterDelete()
        {

            mockContractRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Contract, bool>>>())).Returns(mockContractDbData);
            mockContractExchangeRateRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.ContractExchangeRate, bool>>>())).Returns((Expression<Func<DbModel.ContractExchangeRate, bool>> predicate) => mockContractExchangeRateDbData.Where(predicate));
            mockdbRepo.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Data, bool>>>())).Returns((Expression<Func<DbModel.Data, bool>> predicate) => mockCurrency.Where(predicate));

            ContractExchangeRateService = new Service.ContractExchangeRateService(Mapper.Instance, mockLogger.Object, mockContractExchangeRateRepository.Object, mockdbRepo.Object, mockContractRepository.Object, ExchangeRateValidService);
            contractExchangeRateDomianModels.ToList().ForEach(x => x.RecordStatus = "D");
            var response = ContractExchangeRateService.DeleteContractExchangeRate(contractExchangeRateDomianModels[0].ContractNumber, new List<DomModel.ContractExchangeRate> { contractExchangeRateDomianModels[0] },true,true);
            mockContractExchangeRateRepository.Verify(m => m.Delete(It.IsAny<DbModel.ContractExchangeRate>()), Times.AtLeastOnce);
        }


    }

}

