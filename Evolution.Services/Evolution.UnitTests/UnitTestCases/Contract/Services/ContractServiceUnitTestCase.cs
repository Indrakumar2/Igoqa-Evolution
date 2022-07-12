using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using DomainData = Evolution.Contract.Domain.Interfaces.Data;
using DomModel = Evolution.Contract.Domain.Models.Contracts;
using DbModel = Evolution.DbRepository.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Evolution.Logging.Interfaces;
using AutoMapper;
using Evolution.Contract.Domain.Interfaces.Contracts;
using Evolution.Customer.Domain.Interfaces.Data;
using Evolution.Company.Domain.Interfaces.Data;
using Evolution.DbRepository.Interfaces.Master;
using Evolution.UnitTests.Mocks.Data.Contracts.Domain;
using Evolution.Common.Models.Filters;
using System.Linq;
using System.Linq.Expressions;
using Evolution.UnitTests.Mocks.Data.Contracts.Db;
using Evolution.UnitTests.Mocks.Data.Companies.Db;
using Evolution.UnitTest.Mocks.Data.Customers.Db;
using Evolution.UnitTest.Mocks.Data.Masters.Db.Masters;
using System.Threading.Tasks;
using Evolution.UnitTest.UnitTestCases;
using Evolution.Assignment.Domain.Interfaces.Data;
using Evolution.Master.Domain.Interfaces.Services;
using Evolution.Contract.Domain.Interfaces.Validations;

namespace Evolution.UnitTests.UnitTestCases.Contract.Services
{
    [TestClass]
    public class ContractServiceUnitTestCase : BaseTestCase
    {
        IContractService contractService = null;
        Mock<DomainData.IContractRepository> mockContractRepository = null;
        Mock<ICustomerRepository> mockCustomerRepository = null;
        Mock<ICompanyRepository> mockCompanyRepository = null;
        Mock<IDataRepository> mockDataRepository = null; 
        Mock<IAppLogger<Evolution.Contract.Core.Services.ContractService>> mockLogger = null;
        Mock<IMapper> mockMapper = null;
        Mock<IAssignmentRepository> mockAssignmentRepo = null;
        IContractExchangeRateService contractExchangeRateService = null;
        ICurrencyExchangeRateService currencyExchangeRateService = null;
        IMasterService masterService = null;
        IContractValidationService validationService = null;
        IList<DomModel.Contract> contractDomianModels = null;
        IQueryable<DbModel.Contract> mockContractDbData = null;
        IQueryable<DbModel.Company> mockCompanyDbData = null;
        IQueryable<DbModel.Customer> mockCustomerDbData = null;
        IQueryable<DbModel.Data> mockInvoicePaymentTermsDbData = null;
        IQueryable<DbModel.Data> mockCurrencyDbData = null;

        [TestInitialize]
        public void InitializeContractService()
        {
            mockContractRepository = new Mock<DomainData.IContractRepository>();
            mockCustomerRepository = new Mock<ICustomerRepository>();
            mockCompanyRepository = new Mock<ICompanyRepository>();
            mockDataRepository = new Mock<IDataRepository>();

            mockLogger = new Mock<IAppLogger<Evolution.Contract.Core.Services.ContractService>>();
            mockMapper = new Mock<IMapper>();

            contractDomianModels = MockContractDomainModel.GetDomainModelContract();
            mockContractDbData = MockContract.GetDbModelContract();
            mockCompanyDbData = MockCompany.GetCompanyMockData();
            mockCustomerDbData = MockCustomer.GetCustomerMockData();
            mockInvoicePaymentTermsDbData = MockMaster.GetInvoicePaymentTermsMockData();
            mockCurrencyDbData = MockMaster.GetCurrencyMockData();
        }
         
        [TestMethod]
        public void FetchAllContractWithoutSearchValue()
        {
            mockContractRepository.Setup(x => x.Search(It.IsAny<DomModel.ContractSearch>())).Returns(contractDomianModels);
            contractService = new Evolution.Contract.Core.Services.ContractService(mockMapper.Object, mockLogger.Object, mockContractRepository.Object, mockCustomerRepository.Object, mockCompanyRepository.Object, mockAssignmentRepo.Object ,mockDataRepository.Object,contractExchangeRateService,currencyExchangeRateService,masterService,validationService);
            var response = contractService.GetContract(new DomModel.ContractSearch(), new AdditionalFilter { IsRecordCountOnly = false });
            Assert.AreEqual("1", response.Code);
            Assert.AreEqual(3, (response.Result as List<DomModel.Contract>).Count);
        }

        [TestMethod]
        public void FetchAllContractCountWithoutSearchValue()
        {
            mockContractRepository.Setup(x => x.GetCount(It.IsAny<DomModel.ContractSearch>())).Returns(contractDomianModels.Count);
            contractService = new Evolution.Contract.Core.Services.ContractService(mockMapper.Object, mockLogger.Object, mockContractRepository.Object, mockCustomerRepository.Object, mockCompanyRepository.Object, mockAssignmentRepo.Object, mockDataRepository.Object, contractExchangeRateService, currencyExchangeRateService, masterService, validationService);
            var response = contractService.GetContract(new DomModel.ContractSearch(), new AdditionalFilter { IsRecordCountOnly = true });
            Assert.AreEqual("1", response.Code);
            Assert.AreEqual(3, response.RecordCount);
        }

        [TestMethod]
        public void FetchAllContractWithSearchValue()
        {
            mockContractRepository.Setup(x => x.Search(It.IsAny<DomModel.ContractSearch>())).Returns(contractDomianModels);
            contractService = new Evolution.Contract.Core.Services.ContractService(mockMapper.Object, mockLogger.Object, mockContractRepository.Object, mockCustomerRepository.Object, mockCompanyRepository.Object, mockAssignmentRepo.Object, mockDataRepository.Object, contractExchangeRateService, currencyExchangeRateService, masterService, validationService);
            var response = contractService.GetContract(new DomModel.ContractSearch { ContractCustomerCode = "AB00007" }, new AdditionalFilter { IsRecordCountOnly = false });
            Assert.AreEqual("1", response.Code);
            Assert.AreEqual(3, (response.Result as List<DomModel.Contract>).Count);
        } 

        [TestMethod]
        public void FetchContractListWithNullSearchModel()
        {
            mockContractRepository.Setup(x => x.Search(It.IsAny<DomModel.ContractSearch>())).Returns(contractDomianModels);
            contractService = new Evolution.Contract.Core.Services.ContractService(mockMapper.Object, mockLogger.Object, mockContractRepository.Object, mockCustomerRepository.Object, mockCompanyRepository.Object, mockAssignmentRepo.Object, mockDataRepository.Object, contractExchangeRateService, currencyExchangeRateService, masterService, validationService);
            var response = contractService.GetContract(null, new AdditionalFilter { IsRecordCountOnly = false });
            Assert.AreEqual("1", response.Code);
            Assert.AreEqual(3, (response.Result as List<DomModel.Contract>).Count); 
        }

        [TestMethod]
        public void FetchContractListWithBothNullSearchParameters()
        {
            mockContractRepository.Setup(x => x.GetCount(It.IsAny<DomModel.ContractSearch>())).Returns(contractDomianModels.Count);
            contractService = new Evolution.Contract.Core.Services.ContractService(mockMapper.Object, mockLogger.Object, mockContractRepository.Object, mockCustomerRepository.Object, mockCompanyRepository.Object, mockAssignmentRepo.Object, mockDataRepository.Object, contractExchangeRateService, currencyExchangeRateService, masterService, validationService);
            var response = contractService.GetContract(null,null);
            Assert.AreEqual("1", response.Code);
            Assert.AreEqual(3, response.RecordCount);
        }

        [TestMethod]
        public void FetchContractListByWildCardSearchWithContractNumberStartWith()
        {
            mockContractRepository.Setup(x => x.Search(It.Is<DomModel.ContractSearch>(c => c.ContractNumber.StartsWith("SU02412")))).Returns( contractDomianModels );

            contractService = new Evolution.Contract.Core.Services.ContractService(mockMapper.Object, mockLogger.Object, mockContractRepository.Object, mockCustomerRepository.Object, mockCompanyRepository.Object, mockAssignmentRepo.Object, mockDataRepository.Object, contractExchangeRateService, currencyExchangeRateService, masterService, validationService);
            var response = contractService.GetContract(new DomModel.ContractSearch { ContractNumber = "SU02412*" }, new AdditionalFilter { IsRecordCountOnly = false });
            Assert.AreEqual("1", response.Code);
            Assert.AreEqual(3, (response.Result as List<DomModel.Contract>).Count);
        }

        [TestMethod]
        public void FetchContractListByWildCardSearchWithContractNumberEndWith()
        {
            mockContractRepository.Setup(x => x.Search(It.Is<DomModel.ContractSearch>(c => c.ContractNumber.EndsWith("01")))).Returns(new List<DomModel.Contract> { contractDomianModels[0] });

            contractService = new Evolution.Contract.Core.Services.ContractService(mockMapper.Object, mockLogger.Object, mockContractRepository.Object, mockCustomerRepository.Object, mockCompanyRepository.Object, mockAssignmentRepo.Object, mockDataRepository.Object, contractExchangeRateService, currencyExchangeRateService, masterService, validationService);
            var response = contractService.GetContract(new DomModel.ContractSearch { ContractNumber = "*01" }, new AdditionalFilter { IsRecordCountOnly = false });
            Assert.AreEqual("1", response.Code);
            Assert.AreEqual(1, (response.Result as List<DomModel.Contract>).Count);
        }

        [TestMethod]
        public void FetchContractListByWildCardSearchWithContractHoldingCompanyCodeContains()
        {
            mockContractRepository.Setup(x => x.Search(It.Is<DomModel.ContractSearch>(c => c.ContractHoldingCompanyCode.Contains("05")))).Returns(contractDomianModels);

            contractService = new Evolution.Contract.Core.Services.ContractService(mockMapper.Object, mockLogger.Object, mockContractRepository.Object, mockCustomerRepository.Object, mockCompanyRepository.Object, mockAssignmentRepo.Object, mockDataRepository.Object, contractExchangeRateService, currencyExchangeRateService, masterService, validationService);
            var response = contractService.GetContract(new DomModel.ContractSearch { ContractHoldingCompanyCode = "*05*" }, new AdditionalFilter { IsRecordCountOnly = false });
            Assert.AreEqual("1", response.Code);
            Assert.AreEqual(3, (response.Result as List<DomModel.Contract>).Count);
        }

        [TestMethod]
        public void ThrowsExceptionWhileFetchingContractList()
        {
            mockContractRepository.Setup(x => x.Search(It.IsAny<DomModel.ContractSearch>())).Throws(new Exception("Exception occured while performing some operation."));
            contractService = new Evolution.Contract.Core.Services.ContractService(mockMapper.Object, mockLogger.Object, mockContractRepository.Object, mockCustomerRepository.Object, mockCompanyRepository.Object, mockAssignmentRepo.Object, mockDataRepository.Object, contractExchangeRateService, currencyExchangeRateService, masterService, validationService);
            var response = contractService.GetContract(new DomModel.ContractSearch { ContractHoldingCompanyCode = "*05*" }, new AdditionalFilter { IsRecordCountOnly = false });
            Assert.AreEqual("11", response.Code);
            Assert.IsNotNull(response.Messages);
            Assert.AreEqual("31", response.Messages.FirstOrDefault().Code);
        }

        [TestMethod]
        public void SaveContrcats()
        {
            mockCompanyRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Company,bool>>>())).Returns((Expression<Func<DbModel.Company, bool>> predicate) => mockCompanyDbData.Where(predicate));
            mockCustomerRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Customer, bool>>>())).Returns((Expression<Func<DbModel.Customer, bool>> predicate) => mockCustomerDbData.Where(predicate));
            mockDataRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Data, bool>>>())).Returns((Expression<Func<DbModel.Data, bool>> predicate) => mockInvoicePaymentTermsDbData.Union(mockCurrencyDbData).Where(predicate));
            mockContractRepository.Setup(moq => moq.ForceSave()).Returns(Task<int>.Factory.StartNew(() => 1));

            contractService = new Evolution.Contract.Core.Services.ContractService(mockMapper.Object, mockLogger.Object, mockContractRepository.Object, mockCustomerRepository.Object, mockCompanyRepository.Object, mockAssignmentRepo.Object, mockDataRepository.Object, contractExchangeRateService, currencyExchangeRateService, masterService, validationService);
            contractDomianModels.ToList().ForEach(x => x.RecordStatus = "N");
            var response = contractService.SaveContract(new List<DomModel.Contract> { contractDomianModels[0] });
            Assert.AreEqual("1", response.Code);
            mockContractRepository.Verify(m => m.ForceSave(), Times.Once);
        }

        [TestMethod]
        public void SaveContrcatIsValidCompany()
        {
            mockCompanyRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Company, bool>>>())).Returns((Expression<Func<DbModel.Company, bool>> predicate) => mockCompanyDbData.Where(predicate));
            mockCustomerRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Customer, bool>>>())).Returns((Expression<Func<DbModel.Customer, bool>> predicate) => mockCustomerDbData.Where(predicate));
            mockDataRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Data, bool>>>())).Returns((Expression<Func<DbModel.Data, bool>> predicate) => mockInvoicePaymentTermsDbData.Union(mockCurrencyDbData).Where(predicate));
            mockContractRepository.Setup(moq => moq.ForceSave()).Returns(Task<int>.Factory.StartNew(() => 1));

            contractService = new Evolution.Contract.Core.Services.ContractService(mockMapper.Object, mockLogger.Object, mockContractRepository.Object, mockCustomerRepository.Object, mockCompanyRepository.Object, mockAssignmentRepo.Object, mockDataRepository.Object, contractExchangeRateService, currencyExchangeRateService, masterService, validationService);
            contractDomianModels.ToList().ForEach(x => x.RecordStatus = "N");
            contractDomianModels[0].ContractHoldingCompanyCode = "UK000"; 
            var response = contractService.SaveContract(new List<DomModel.Contract> { contractDomianModels[0] });
            Assert.AreEqual("11", response.Code);
            Assert.IsNotNull(response.Messages);
            Assert.AreEqual("1002", response.Messages.FirstOrDefault().Code);
        }

        [TestMethod]
        public void SaveContrcatIsValidCompanyOffice()
        {
            mockCompanyRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Company, bool>>>())).Returns((Expression<Func<DbModel.Company, bool>> predicate) => mockCompanyDbData.Where(predicate));
            mockCustomerRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Customer, bool>>>())).Returns((Expression<Func<DbModel.Customer, bool>> predicate) => mockCustomerDbData.Where(predicate));
            mockDataRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Data, bool>>>())).Returns((Expression<Func<DbModel.Data, bool>> predicate) => mockInvoicePaymentTermsDbData.Union(mockCurrencyDbData).Where(predicate));
            mockContractRepository.Setup(moq => moq.ForceSave()).Returns(Task<int>.Factory.StartNew(() => 1));

            contractService = new Evolution.Contract.Core.Services.ContractService(mockMapper.Object, mockLogger.Object, mockContractRepository.Object, mockCustomerRepository.Object, mockCompanyRepository.Object, mockAssignmentRepo.Object, mockDataRepository.Object, contractExchangeRateService, currencyExchangeRateService, masterService, validationService);
            contractDomianModels.ToList().ForEach(x => x.RecordStatus = "N");
            contractDomianModels[0].ParentCompanyOffice = "Test Office ";
            var response = contractService.SaveContract(new List<DomModel.Contract> { contractDomianModels[0] });
            Assert.AreEqual("11", response.Code);
            Assert.IsNotNull(response.Messages);
            Assert.AreEqual("1119", response.Messages.FirstOrDefault().Code);
        }

        [TestMethod]
        public void SaveContrcatIsValidCustomer()
        {
            mockCompanyRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Company, bool>>>())).Returns((Expression<Func<DbModel.Company, bool>> predicate) => mockCompanyDbData.Where(predicate));
            mockCustomerRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Customer, bool>>>())).Returns((Expression<Func<DbModel.Customer, bool>> predicate) => mockCustomerDbData.Where(predicate));
            mockDataRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Data, bool>>>())).Returns((Expression<Func<DbModel.Data, bool>> predicate) => mockInvoicePaymentTermsDbData.Union(mockCurrencyDbData).Where(predicate));
            mockContractRepository.Setup(moq => moq.ForceSave()).Returns(Task<int>.Factory.StartNew(() => 1));

            contractService = new Evolution.Contract.Core.Services.ContractService(mockMapper.Object, mockLogger.Object, mockContractRepository.Object, mockCustomerRepository.Object, mockCompanyRepository.Object, mockAssignmentRepo.Object, mockDataRepository.Object, contractExchangeRateService, currencyExchangeRateService, masterService, validationService);
            contractDomianModels.ToList().ForEach(x => x.RecordStatus = "N");
            contractDomianModels[0].ContractCustomerCode = "CodeTest";
            var response = contractService.SaveContract(new List<DomModel.Contract> { contractDomianModels[0] });
            Assert.AreEqual("11", response.Code);
            Assert.IsNotNull(response.Messages);
            Assert.AreEqual("2265", response.Messages.FirstOrDefault().Code);
        }

        [TestMethod]
        public void SaveContrcatIsValidCompanyAddress()
        {
            mockCompanyRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Company, bool>>>())).Returns((Expression<Func<DbModel.Company, bool>> predicate) => mockCompanyDbData.Where(predicate));
            mockCustomerRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Customer, bool>>>())).Returns((Expression<Func<DbModel.Customer, bool>> predicate) => mockCustomerDbData.Where(predicate));
            mockDataRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Data, bool>>>())).Returns((Expression<Func<DbModel.Data, bool>> predicate) => mockInvoicePaymentTermsDbData.Union(mockCurrencyDbData).Where(predicate));
            mockContractRepository.Setup(moq => moq.ForceSave()).Returns(Task<int>.Factory.StartNew(() => 1));

            contractService = new Evolution.Contract.Core.Services.ContractService(mockMapper.Object, mockLogger.Object, mockContractRepository.Object, mockCustomerRepository.Object, mockCompanyRepository.Object, mockAssignmentRepo.Object, mockDataRepository.Object, contractExchangeRateService, currencyExchangeRateService, masterService, validationService);
            contractDomianModels.ToList().ForEach(x => x.RecordStatus = "N");
            contractDomianModels[0].ContractCustomerContactAddress = "Test address ";
            var response = contractService.SaveContract(new List<DomModel.Contract> { contractDomianModels[0] });
            Assert.AreEqual("11", response.Code);
            Assert.IsNotNull(response.Messages);
            Assert.AreEqual("2264", response.Messages.FirstOrDefault().Code);
        }

        [TestMethod]
        public void SaveContrcatIsValidCustomerContact()
        {
            mockCompanyRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Company, bool>>>())).Returns((Expression<Func<DbModel.Company, bool>> predicate) => mockCompanyDbData.Where(predicate));
            mockCustomerRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Customer, bool>>>())).Returns((Expression<Func<DbModel.Customer, bool>> predicate) => mockCustomerDbData.Where(predicate));
            mockDataRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Data, bool>>>())).Returns((Expression<Func<DbModel.Data, bool>> predicate) => mockInvoicePaymentTermsDbData.Union(mockCurrencyDbData).Where(predicate));
            mockContractRepository.Setup(moq => moq.ForceSave()).Returns(Task<int>.Factory.StartNew(() => 1));

            contractService = new Evolution.Contract.Core.Services.ContractService(mockMapper.Object, mockLogger.Object, mockContractRepository.Object, mockCustomerRepository.Object, mockCompanyRepository.Object, mockAssignmentRepo.Object, mockDataRepository.Object, contractExchangeRateService, currencyExchangeRateService, masterService, validationService);
            contractDomianModels.ToList().ForEach(x => x.RecordStatus = "N");
            contractDomianModels[0].ContractCustomerContact = "Test address contact ";
            var response = contractService.SaveContract(new List<DomModel.Contract> { contractDomianModels[0] });
            Assert.AreEqual("11", response.Code);
            Assert.IsNotNull(response.Messages);
            Assert.AreEqual("2163", response.Messages.FirstOrDefault().Code);
        }

        [TestMethod]
        public void SaveContrcatIsValidContract_ParentContractNumber()
        {
            mockCompanyRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Company, bool>>>())).Returns((Expression<Func<DbModel.Company, bool>> predicate) => mockCompanyDbData.Where(predicate));
            mockCustomerRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Customer, bool>>>())).Returns((Expression<Func<DbModel.Customer, bool>> predicate) => mockCustomerDbData.Where(predicate));
            mockDataRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Data, bool>>>())).Returns((Expression<Func<DbModel.Data, bool>> predicate) => mockInvoicePaymentTermsDbData.Union(mockCurrencyDbData).Where(predicate));
            mockContractRepository.Setup(moq => moq.ForceSave()).Returns(Task<int>.Factory.StartNew(() => 1));

            contractService = new Evolution.Contract.Core.Services.ContractService(mockMapper.Object, mockLogger.Object, mockContractRepository.Object, mockCustomerRepository.Object, mockCompanyRepository.Object, mockAssignmentRepo.Object, mockDataRepository.Object, contractExchangeRateService, currencyExchangeRateService, masterService, validationService);
            contractDomianModels.ToList().ForEach(x => x.RecordStatus = "N");
            contractDomianModels[0].ParentContractNumber = "Test Contract number ";
            var response = contractService.SaveContract(new List<DomModel.Contract> { contractDomianModels[0] });
            Assert.AreEqual("11", response.Code);
            Assert.IsNotNull(response.Messages);
            Assert.AreEqual("10001", response.Messages.FirstOrDefault().Code);
        }
         
        [TestMethod]
        public void SaveContrcatIsValidInvoicePaymentTerm()
        {
            mockCompanyRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Company, bool>>>())).Returns((Expression<Func<DbModel.Company, bool>> predicate) => mockCompanyDbData.Where(predicate));
            mockCustomerRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Customer, bool>>>())).Returns((Expression<Func<DbModel.Customer, bool>> predicate) => mockCustomerDbData.Where(predicate));
            mockDataRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Data, bool>>>())).Returns((Expression<Func<DbModel.Data, bool>> predicate) => mockInvoicePaymentTermsDbData.Union(mockCurrencyDbData).Where(predicate));
            mockContractRepository.Setup(moq => moq.ForceSave()).Returns(Task<int>.Factory.StartNew(() => 1));

            contractService = new Evolution.Contract.Core.Services.ContractService(mockMapper.Object, mockLogger.Object, mockContractRepository.Object, mockCustomerRepository.Object, mockCompanyRepository.Object, mockAssignmentRepo.Object, mockDataRepository.Object, contractExchangeRateService, currencyExchangeRateService, masterService, validationService);
            contractDomianModels.ToList().ForEach(x => x.RecordStatus = "N");
            contractDomianModels[0].ContractInvoicePaymentTerms = "Test Contract Invoice Payment Terms ";
            var response = contractService.SaveContract(new List<DomModel.Contract> { contractDomianModels[0] });
            Assert.AreEqual("11", response.Code);
            Assert.IsNotNull(response.Messages);
            Assert.AreEqual("10002", response.Messages.FirstOrDefault().Code);
        }

        [TestMethod]
        public void SaveContrcatIsValidIdentifiers()
        {
            mockCompanyRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Company, bool>>>())).Returns((Expression<Func<DbModel.Company, bool>> predicate) => mockCompanyDbData.Where(predicate));
            mockCustomerRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Customer, bool>>>())).Returns((Expression<Func<DbModel.Customer, bool>> predicate) => mockCustomerDbData.Where(predicate));
            mockDataRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Data, bool>>>())).Returns((Expression<Func<DbModel.Data, bool>> predicate) => mockInvoicePaymentTermsDbData.Union(mockCurrencyDbData).Where(predicate));
            mockContractRepository.Setup(moq => moq.ForceSave()).Returns(Task<int>.Factory.StartNew(() => 1));

            contractService = new Evolution.Contract.Core.Services.ContractService(mockMapper.Object, mockLogger.Object, mockContractRepository.Object, mockCustomerRepository.Object, mockCompanyRepository.Object, mockAssignmentRepo.Object, mockDataRepository.Object, contractExchangeRateService, currencyExchangeRateService, masterService, validationService);
            contractDomianModels.ToList().ForEach(x => x.RecordStatus = "N");
            contractDomianModels[0].ContractInvoiceRemittanceIdentifier = "Test Contract Invoice Remittance Identifier ";
            var response = contractService.SaveContract(new List<DomModel.Contract> { contractDomianModels[0] });
            Assert.AreEqual("11", response.Code);
            Assert.IsNotNull(response.Messages);
            Assert.AreEqual("10003", response.Messages.FirstOrDefault().Code);
        }

        [TestMethod]
        public void SaveContrcatIsValidTax()
        {
            mockCompanyRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Company, bool>>>())).Returns((Expression<Func<DbModel.Company, bool>> predicate) => mockCompanyDbData.Where(predicate));
            mockCustomerRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Customer, bool>>>())).Returns((Expression<Func<DbModel.Customer, bool>> predicate) => mockCustomerDbData.Where(predicate));
            mockDataRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Data, bool>>>())).Returns((Expression<Func<DbModel.Data, bool>> predicate) => mockInvoicePaymentTermsDbData.Union(mockCurrencyDbData).Where(predicate));
            mockContractRepository.Setup(moq => moq.ForceSave()).Returns(Task<int>.Factory.StartNew(() => 1));

            contractService = new Evolution.Contract.Core.Services.ContractService(mockMapper.Object, mockLogger.Object, mockContractRepository.Object, mockCustomerRepository.Object, mockCompanyRepository.Object, mockAssignmentRepo.Object, mockDataRepository.Object, contractExchangeRateService, currencyExchangeRateService, masterService, validationService);
            contractDomianModels.ToList().ForEach(x => x.RecordStatus = "N");
            contractDomianModels[0].ContractSalesTax = "Test Contract Sales Tax ";
            var response = contractService.SaveContract(new List<DomModel.Contract> { contractDomianModels[0] });
            Assert.AreEqual("11", response.Code);
            Assert.IsNotNull(response.Messages);
            Assert.AreEqual("1915", response.Messages.FirstOrDefault().Code);
        }

        [TestMethod]
        public void SaveContrcatIsValidCurrency()
        {
            mockCompanyRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Company, bool>>>())).Returns((Expression<Func<DbModel.Company, bool>> predicate) => mockCompanyDbData.Where(predicate));
            mockCustomerRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Customer, bool>>>())).Returns((Expression<Func<DbModel.Customer, bool>> predicate) => mockCustomerDbData.Where(predicate));
            mockDataRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Data, bool>>>())).Returns((Expression<Func<DbModel.Data, bool>> predicate) => mockInvoicePaymentTermsDbData.Union(mockCurrencyDbData).Where(predicate));
            mockContractRepository.Setup(moq => moq.ForceSave()).Returns(Task<int>.Factory.StartNew(() => 1));

            contractService = new Evolution.Contract.Core.Services.ContractService(mockMapper.Object, mockLogger.Object, mockContractRepository.Object, mockCustomerRepository.Object, mockCompanyRepository.Object, mockAssignmentRepo.Object, mockDataRepository.Object, contractExchangeRateService, currencyExchangeRateService, masterService, validationService);
            contractDomianModels.ToList().ForEach(x => x.RecordStatus = "N");
            contractDomianModels[0].ContractInvoicingCurrency = "123";
            var response = contractService.SaveContract(new List<DomModel.Contract> { contractDomianModels[0] });
            Assert.AreEqual("11", response.Code);
            Assert.IsNotNull(response.Messages);
            Assert.AreEqual("10005", response.Messages.FirstOrDefault().Code);
        }

        [TestMethod]
        public void ModifyContract()
        {
            mockCompanyRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Company, bool>>>())).Returns((Expression<Func<DbModel.Company, bool>> predicate) => mockCompanyDbData.Where(predicate));
            mockCustomerRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Customer, bool>>>())).Returns((Expression<Func<DbModel.Customer, bool>> predicate) => mockCustomerDbData.Where(predicate));
            mockDataRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Data, bool>>>())).Returns((Expression<Func<DbModel.Data, bool>> predicate) => mockInvoicePaymentTermsDbData.Union(mockCurrencyDbData).Where(predicate));
            mockContractRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Contract, bool>>>())).Returns((Expression<Func<DbModel.Contract, bool>> predicate) => mockContractDbData.Where(predicate));
            mockContractRepository.Setup(moq => moq.ForceSave()).Returns(Task<int>.Factory.StartNew(() => 1));

            contractService = new Evolution.Contract.Core.Services.ContractService(mockMapper.Object, mockLogger.Object, mockContractRepository.Object, mockCustomerRepository.Object, mockCompanyRepository.Object, mockAssignmentRepo.Object, mockDataRepository.Object, contractExchangeRateService, currencyExchangeRateService, masterService, validationService);
            contractDomianModels.ToList().ForEach(x => x.RecordStatus = "M");
            var response = contractService.ModifyContract(new List<DomModel.Contract> { contractDomianModels[0] });
            Assert.AreEqual("1", response.Code);
            mockContractRepository.Verify(m => m.ForceSave(), Times.Once);
        }
         
        [TestMethod]
        public void DeleteContract()
        {
            mockCompanyRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Company, bool>>>())).Returns((Expression<Func<DbModel.Company, bool>> predicate) => mockCompanyDbData.Where(predicate));
            mockCustomerRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Customer, bool>>>())).Returns((Expression<Func<DbModel.Customer, bool>> predicate) => mockCustomerDbData.Where(predicate));
            mockDataRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Data, bool>>>())).Returns((Expression<Func<DbModel.Data, bool>> predicate) => mockInvoicePaymentTermsDbData.Union(mockCurrencyDbData).Where(predicate));
            mockContractRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Contract, bool>>>())).Returns((Expression<Func<DbModel.Contract, bool>> predicate) => mockContractDbData.Where(predicate));
            mockContractRepository.Setup(moq => moq.ForceSave()).Returns(Task<int>.Factory.StartNew(() => 1));

            contractService = new Evolution.Contract.Core.Services.ContractService(mockMapper.Object, mockLogger.Object, mockContractRepository.Object, mockCustomerRepository.Object, mockCompanyRepository.Object, mockAssignmentRepo.Object, mockDataRepository.Object, contractExchangeRateService, currencyExchangeRateService, masterService, validationService);
            contractDomianModels.ToList().ForEach(x => x.RecordStatus = "D");
            var response = contractService.DeleteContract(new List<DomModel.Contract> { contractDomianModels[2] });
            Assert.AreEqual("1", response.Code);
            mockContractRepository.Verify(m => m.ForceSave(), Times.Once);
        }

        [TestMethod]
        public void DeleteContractWithReferenceInProjectShouldNotBeDeleted()
        {
            mockCompanyRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Company, bool>>>())).Returns((Expression<Func<DbModel.Company, bool>> predicate) => mockCompanyDbData.Where(predicate));
            mockCustomerRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Customer, bool>>>())).Returns((Expression<Func<DbModel.Customer, bool>> predicate) => mockCustomerDbData.Where(predicate));
            mockDataRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Data, bool>>>())).Returns((Expression<Func<DbModel.Data, bool>> predicate) => mockInvoicePaymentTermsDbData.Union(mockCurrencyDbData).Where(predicate));
            mockContractRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Contract, bool>>>())).Returns((Expression<Func<DbModel.Contract, bool>> predicate) => mockContractDbData.Where(predicate));
            mockContractRepository.Setup(moq => moq.ForceSave()).Returns(Task<int>.Factory.StartNew(() => 1));

            contractService = new Evolution.Contract.Core.Services.ContractService(mockMapper.Object, mockLogger.Object, mockContractRepository.Object, mockCustomerRepository.Object, mockCompanyRepository.Object, mockAssignmentRepo.Object, mockDataRepository.Object, contractExchangeRateService, currencyExchangeRateService, masterService, validationService);
            contractDomianModels.ToList().ForEach(x => x.RecordStatus = "D");
            var response = contractService.DeleteContract(new List<DomModel.Contract> { contractDomianModels[0] });
            Assert.AreEqual("11", response.Code);
            Assert.AreEqual("10007", response.Messages.First().Code);
        }

        [TestMethod]
        public void ClosedContractShouldnotBeDeleted()
        {
            mockCompanyRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Company, bool>>>())).Returns((Expression<Func<DbModel.Company, bool>> predicate) => mockCompanyDbData.Where(predicate));
            mockCustomerRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Customer, bool>>>())).Returns((Expression<Func<DbModel.Customer, bool>> predicate) => mockCustomerDbData.Where(predicate));
            mockDataRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Data, bool>>>())).Returns((Expression<Func<DbModel.Data, bool>> predicate) => mockInvoicePaymentTermsDbData.Union(mockCurrencyDbData).Where(predicate));
            mockContractRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Contract, bool>>>())).Returns((Expression<Func<DbModel.Contract, bool>> predicate) => mockContractDbData.Where(predicate));
            mockContractRepository.Setup(moq => moq.ForceSave()).Returns(Task<int>.Factory.StartNew(() => 1));

            contractService = new Evolution.Contract.Core.Services.ContractService(mockMapper.Object, mockLogger.Object, mockContractRepository.Object, mockCustomerRepository.Object, mockCompanyRepository.Object, mockAssignmentRepo.Object, mockDataRepository.Object, contractExchangeRateService, currencyExchangeRateService, masterService, validationService);
            contractDomianModels.ToList().ForEach(x => x.RecordStatus = "D");
            var response = contractService.DeleteContract(new List<DomModel.Contract> { contractDomianModels[1] });
            Assert.AreEqual("11", response.Code);
            Assert.IsNotNull(response.Messages);
            Assert.AreEqual("10010", response.Messages.FirstOrDefault().Code);
        }

    }
}
