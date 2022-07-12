using Evolution.Project.Domain.Interfaces.Data;
using Evolution.UnitTest.UnitTestCases;
using DomainData = Evolution.Project.Domain.Interfaces.Data;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using ServiceDomainData = Evolution.Project.Domain.Interfaces.Projects;
using Evolution.Contract.Domain.Interfaces.Data;
using Evolution.Customer.Domain.Interfaces.Data;
using Evolution.Assignment.Domain.Interfaces.Data;
using ValdService = Evolution.Project.Domain.Interfaces.Validations;
using Evolution.Company.Domain.Interfaces.Data;
using Evolution.DbRepository.Interfaces.Master;
using Evolution.Master.Domain.Interfaces.Data;
using Evolution.Master.Domain.Interfaces.Services;
using Evolution.Logging.Interfaces;
using Evolution.Project.Core.Services;
using DomModel = Evolution.Project.Domain.Models.Projects;
using DbModel = Evolution.DbRepository.Models;
using System.Linq;
using Evolution.Contract.Domain.Interfaces.Contracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Evolution.UnitTests.Mocks.Data.Projects.Domain;
using Evolution.UnitTests.Mocks.Data.Projects.Db;
using Evolution.UnitTests.Mocks.Data.Contracts.Db;
using Evolution.UnitTest.Mocks.Data.Customers.Db;
using Evolution.UnitTests.Mocks.Data.Assignment.Db;
using Evolution.UnitTests.Mocks.Data.Companies.Db;
using Evolution.UnitTest.Mocks.Data.Masters.Db.Masters;
using AutoMapper;
using Evolution.Common.Models.Filters;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Evolution.ValidationService.Interfaces;
using Evolution.Common.Models.Responses;

namespace Evolution.UnitTests.UnitTestCases.Projects.Services
{
    [TestClass]
   public class ProjectServiceUnitTestCases :BaseTestCase
   {
        IProjectService projectService = null;
        IValidationService Validation = null;
       Mock<DomainData.IProjectRepository> mockProjectRepo=null;
        Mock<IContractRepository> mockContractRepo = null;
        Mock<ICustomerRepository> mockCustomerRepo = null;
        Mock<IAssignmentRepository> mockAssignmentRepo = null;
        Mock<ICompanyRepository> mockCompanyRepo = null;
        ValdService.IProjectValidationService validationService = null;
        Mock<IDataRepository> mockDataRepo = null;
         IContractExchangeRateService contractExchangeRateService = null;
        ICurrencyExchangeRateService currencyExchangeRateService = null;
        IMasterService masterService = null;
        Mock<IAppLogger<ProjectService>> mockLogger = null;
        IList<DomModel.Project> projectDomainModel = null;
        IQueryable<DbModel.Project> mockProjectDbData = null;
        IQueryable<DbModel.Contract> mockContractDbData = null;
        IQueryable<DbModel.Customer> mockCustomerDbData = null;
        IQueryable<DbModel.Assignment> mockAssignmentDbData = null;
        IQueryable<DbModel.Company> mockCompanyDbData = null;
        IQueryable<DbModel.Data> mockInvoicePaymentTerms = null;
        IQueryable<DbModel.Data> mockProjectType = null;
        IQueryable<DbModel.Data> mockIndustrySector = null;
        IQueryable<DbModel.Data> mockCurrency = null;
        IQueryable<DbModel.Data> mockLogo = null;
        
      

        [TestInitialize]
        public void IntializeProjectService()
        {
            mockProjectRepo = new Mock<IProjectRepository>();
            mockContractRepo = new Mock<IContractRepository>();
            mockCustomerRepo = new Mock<ICustomerRepository>();
            mockAssignmentRepo = new Mock<IAssignmentRepository>();
            mockCompanyRepo = new Mock<ICompanyRepository>();
            mockDataRepo = new Mock<IDataRepository>();
            projectDomainModel = MockProjectDomainModel.GetProjectsMockedDomainData();
            mockProjectDbData = MockProject.GetprojectMockData();
            mockContractDbData = MockContract.GetDbModelContract();
            mockCustomerDbData = MockCustomer.GetCustomerMockData();
            mockAssignmentDbData = MockAssignment.GetAssignmentMockData();
            mockCompanyDbData = MockCompany.GetCompanyMockData();
            mockInvoicePaymentTerms = MockMaster.GetInvoicePaymentTermsMockData();
            mockProjectType = MockMaster.GetProjectTypeMockData();
            mockIndustrySector = MockMaster.GetIndustrySectorMockData();
            mockCurrency = MockMaster.GetCurrencyMockData();
            mockLogo = MockMaster.GetLogoMockData();
            mockLogger = new Mock<IAppLogger<ProjectService>>();
         
           
        }

        [TestMethod]
        public void FetchAllProjectWithoutSearchValue()
        {
            mockProjectRepo.Setup(x => x.Search(It.IsAny<DomModel.ProjectSearch>())).Returns(projectDomainModel);
            projectService = new Evolution.Project.Core.Services.ProjectService( mockLogger.Object, mockProjectRepo.Object, validationService, Mapper.Instance, contractExchangeRateService, currencyExchangeRateService,masterService, mockDataRepo.Object,mockContractRepo.Object,mockAssignmentRepo.Object,mockCompanyRepo.Object,mockCustomerRepo.Object);
            var response = projectService.GetProjects(new DomModel.ProjectSearch(), new AdditionalFilter { IsRecordCountOnly = false });
            Assert.AreEqual("1", response.Code);
            Assert.AreEqual(2, (response.Result as List<DomModel.Project>).Count);
        }

        [TestMethod]
        public void FetchAllprojectCountWithoutSearchValue()
        {

            mockProjectRepo.Setup(x => x.GetCount(It.IsAny<DomModel.ProjectSearch>())).Returns(projectDomainModel.Count);
            projectService = new Evolution.Project.Core.Services.ProjectService(mockLogger.Object, mockProjectRepo.Object, validationService, Mapper.Instance, contractExchangeRateService, currencyExchangeRateService, masterService, mockDataRepo.Object, mockContractRepo.Object, mockAssignmentRepo.Object, mockCompanyRepo.Object, mockCustomerRepo.Object);
            var response = projectService.GetProjects(new DomModel.ProjectSearch(), new AdditionalFilter { IsRecordCountOnly = true });
            Assert.AreEqual("1", response.Code);
            Assert.AreEqual(2, response.RecordCount);
        }
        [TestMethod]
        public void FetchAllProjectWithSearchValue()
        {
            mockProjectRepo.Setup(x => x.Search(It.IsAny<DomModel.ProjectSearch>())).Returns(projectDomainModel);
            projectService = new Evolution.Project.Core.Services.ProjectService(mockLogger.Object, mockProjectRepo.Object, validationService, Mapper.Instance, contractExchangeRateService, currencyExchangeRateService, masterService, mockDataRepo.Object, mockContractRepo.Object, mockAssignmentRepo.Object, mockCompanyRepo.Object, mockCustomerRepo.Object);
            var response = projectService.GetProjects(new DomModel.ProjectSearch {ContractNumber= "SU02412/0001" }, new AdditionalFilter { IsRecordCountOnly = false });
            Assert.AreEqual("1", response.Code);
            Assert.AreEqual(2, (response.Result as List<DomModel.Project>).Count);
        }

        [TestMethod]
        public void FetchProjectListWithNullSearchModel()
        {
            mockProjectRepo.Setup(x => x.Search(It.IsAny<DomModel.ProjectSearch>())).Returns(projectDomainModel);
            projectService = new Evolution.Project.Core.Services.ProjectService(mockLogger.Object, mockProjectRepo.Object, validationService, Mapper.Instance, contractExchangeRateService, currencyExchangeRateService, masterService, mockDataRepo.Object, mockContractRepo.Object, mockAssignmentRepo.Object, mockCompanyRepo.Object, mockCustomerRepo.Object);
            var response = projectService.GetProjects(null, new AdditionalFilter { IsRecordCountOnly = false });
            Assert.AreEqual("1", response.Code);
            Assert.AreEqual(2, (response.Result as List<DomModel.Project>).Count);
        }
        [TestMethod]
        public void FetchProjectListWithBothNullSearchParameters()
        {
            mockProjectRepo.Setup(x => x.GetCount(It.IsAny<DomModel.ProjectSearch>())).Returns(projectDomainModel.Count);
            projectService = new Evolution.Project.Core.Services.ProjectService(mockLogger.Object, mockProjectRepo.Object, validationService, Mapper.Instance, contractExchangeRateService, currencyExchangeRateService, masterService, mockDataRepo.Object, mockContractRepo.Object, mockAssignmentRepo.Object, mockCompanyRepo.Object, mockCustomerRepo.Object);
            var response = projectService.GetProjects(null, null);
            Assert.AreEqual("1", response.Code);
            Assert.AreEqual(2, response.RecordCount);
        }
        [TestMethod]
        public void ThrowsExceptionWhileFetchingContractList()
        {
            mockProjectRepo.Setup(x => x.Search(It.IsAny<DomModel.ProjectSearch>())).Throws(new Exception("Exception occured while performing some operation."));
            projectService = new Evolution.Project.Core.Services.ProjectService(mockLogger.Object, mockProjectRepo.Object, validationService, Mapper.Instance, contractExchangeRateService, currencyExchangeRateService, masterService, mockDataRepo.Object, mockContractRepo.Object, mockAssignmentRepo.Object, mockCompanyRepo.Object, mockCustomerRepo.Object);
            var response = projectService.GetProjects(new DomModel.ProjectSearch { ContractHoldingCompanyCode = "*05*" }, new AdditionalFilter { IsRecordCountOnly = false });
            Assert.AreEqual("11", response.Code);
            Assert.IsNotNull(response.Messages);
            Assert.AreEqual("31", response.Messages.FirstOrDefault().Code);
        }


        [TestMethod]
        public void SaveProjects()
        {
            mockProjectRepo.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Project, bool>>>())).Returns(mockProjectDbData);
         
            mockDataRepo.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Data, bool>>>())).Returns((Expression<Func<DbModel.Data, bool>> predicate) => mockInvoicePaymentTerms.Union(mockCurrency).Union(mockIndustrySector).Union(mockProjectType).Union(mockLogo).Where(predicate));
            mockContractRepo.Setup(moq=>moq.FindBy(It.IsAny<Expression<Func<DbModel.Contract, bool>>>())).Returns((Expression<Func<DbModel.Contract, bool>> Predicate) => mockContractDbData.Where(Predicate));
           
            Validation = new Evolution.ValidationService.Services.ValidationService();
            validationService = new Evolution.Project.Infrastructure.Validations.ProjectValidationService(Validation);
            projectService = new Evolution.Project.Core.Services.ProjectService(mockLogger.Object, mockProjectRepo.Object, validationService, Mapper.Instance, contractExchangeRateService, currencyExchangeRateService, masterService, mockDataRepo.Object, mockContractRepo.Object, mockAssignmentRepo.Object, mockCompanyRepo.Object, mockCustomerRepo.Object);
            projectDomainModel.ToList().ForEach(x => x.RecordStatus = "N");
            
            var response = projectService.SaveProjects(new List<DomModel.Project> { projectDomainModel[0] });
            Assert.AreEqual("1", response.Code);
            mockProjectRepo.Verify(m => m.ForceSave(), Times.Once);
        }

        [TestMethod]
        public void ModifyProjects()
        {
            mockProjectRepo.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Project, bool>>>())).Returns(mockProjectDbData);
          
            mockDataRepo.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Data, bool>>>())).Returns((Expression<Func<DbModel.Data, bool>> predicate) => mockInvoicePaymentTerms.Union(mockCurrency).Union(mockIndustrySector).Union(mockProjectType).Union(mockLogo).Where(predicate));
            mockContractRepo.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Contract, bool>>>())).Returns((Expression<Func<DbModel.Contract, bool>> Predicate) => mockContractDbData.Where(Predicate));
          //  mockProjectRepo.Setup(moq => moq.ForceSave()).Returns(.Factory.StartNew(() => 1));
            Validation = new Evolution.ValidationService.Services.ValidationService();
            validationService = new Evolution.Project.Infrastructure.Validations.ProjectValidationService(Validation);
            projectService = new Evolution.Project.Core.Services.ProjectService(mockLogger.Object, mockProjectRepo.Object, validationService, Mapper.Instance, contractExchangeRateService, currencyExchangeRateService, masterService, mockDataRepo.Object, mockContractRepo.Object, mockAssignmentRepo.Object, mockCompanyRepo.Object, mockCustomerRepo.Object);
            projectDomainModel.ToList().ForEach(x => x.RecordStatus = "M");

            var response = projectService.ModifyProjects(new List<DomModel.Project> { projectDomainModel[0] });
            Assert.AreEqual("1", response.Code);
            mockProjectRepo.Verify(m => m.ForceSave(), Times.Once);
        }

        [TestMethod]
        public void DeleteProjects()
        {
            mockProjectRepo.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Project, bool>>>())).Returns(mockProjectDbData);
            
            Validation = new Evolution.ValidationService.Services.ValidationService();
            validationService = new Evolution.Project.Infrastructure.Validations.ProjectValidationService(Validation);
            projectService = new Evolution.Project.Core.Services.ProjectService(mockLogger.Object, mockProjectRepo.Object, validationService, Mapper.Instance, contractExchangeRateService, currencyExchangeRateService, masterService, mockDataRepo.Object, mockContractRepo.Object, mockAssignmentRepo.Object, mockCompanyRepo.Object, mockCustomerRepo.Object);
            projectDomainModel.ToList().ForEach(x => x.RecordStatus = "D");

            var response = projectService.DeleteProjects(new List<DomModel.Project> { projectDomainModel[0] });
            Assert.AreEqual("1", response.Code);
            mockProjectRepo.Verify(m => m.ForceSave(), Times.Once);
        }
        [TestMethod]
        public void IsValidContractHolidingCompanyToSaveProject()
        {
            mockProjectRepo.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Project, bool>>>())).Returns(mockProjectDbData);
           
            mockContractRepo.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Contract, bool>>>())).Returns((Expression<Func<DbModel.Contract, bool>> Predicate) => mockContractDbData.Where(Predicate));
           
            Validation = new Evolution.ValidationService.Services.ValidationService();
            validationService = new Evolution.Project.Infrastructure.Validations.ProjectValidationService(Validation);
            projectService = new Evolution.Project.Core.Services.ProjectService(mockLogger.Object, mockProjectRepo.Object, validationService, Mapper.Instance, contractExchangeRateService, currencyExchangeRateService, masterService, mockDataRepo.Object, mockContractRepo.Object, mockAssignmentRepo.Object, mockCompanyRepo.Object, mockCustomerRepo.Object);
            projectDomainModel.ToList().ForEach(x => x.RecordStatus = "N");

            projectDomainModel[0].ContractHoldingCompanyCode = "UK050";
            var response = projectService.SaveProjects(new List<DomModel.Project> { projectDomainModel[0] });
            Assert.AreEqual("11",response.Code);
            Assert.IsNotNull(response.Messages);
            Assert.AreEqual("3004", response.Messages.FirstOrDefault().Code);
        }

        [TestMethod]
        public void IsValidCompanyOfficeToSaveProject()
        {
            mockProjectRepo.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Project, bool>>>())).Returns(mockProjectDbData);
            
            mockContractRepo.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Contract, bool>>>())).Returns((Expression<Func<DbModel.Contract, bool>> Predicate) => mockContractDbData.Where(Predicate));
           
            Validation = new Evolution.ValidationService.Services.ValidationService();
            validationService = new Evolution.Project.Infrastructure.Validations.ProjectValidationService(Validation);
            projectService = new Evolution.Project.Core.Services.ProjectService(mockLogger.Object, mockProjectRepo.Object, validationService, Mapper.Instance, contractExchangeRateService, currencyExchangeRateService, masterService, mockDataRepo.Object, mockContractRepo.Object, mockAssignmentRepo.Object, mockCompanyRepo.Object, mockCustomerRepo.Object);
            projectDomainModel.ToList().ForEach(x => x.RecordStatus = "N");

           
            projectDomainModel[0].CompanyOffice = "India";
            var response = projectService.SaveProjects(new List<DomModel.Project> { projectDomainModel[0] });
            Assert.AreEqual("11", response.Code);
            Assert.IsNotNull(response.Messages);
            Assert.AreEqual("3004", response.Messages.FirstOrDefault().Code);
        }

        [TestMethod]
        public void IsValidContractToSaveProject()
        {
            mockProjectRepo.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Project, bool>>>())).Returns(mockProjectDbData);
            
            mockContractRepo.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Contract, bool>>>())).Returns((Expression<Func<DbModel.Contract, bool>> Predicate) => mockContractDbData.Where(Predicate));
            
            Validation = new Evolution.ValidationService.Services.ValidationService();
            validationService = new Evolution.Project.Infrastructure.Validations.ProjectValidationService(Validation);
            projectService = new Evolution.Project.Core.Services.ProjectService(mockLogger.Object, mockProjectRepo.Object, validationService, Mapper.Instance, contractExchangeRateService, currencyExchangeRateService, masterService, mockDataRepo.Object, mockContractRepo.Object, mockAssignmentRepo.Object, mockCompanyRepo.Object, mockCustomerRepo.Object);
            projectDomainModel.ToList().ForEach(x => x.RecordStatus = "N");

            
            projectDomainModel[0].ContractNumber = "SU024";
            var response = projectService.SaveProjects(new List<DomModel.Project> { projectDomainModel[0] });
            Assert.AreEqual("11", response.Code);
            Assert.IsNotNull(response.Messages);
            Assert.AreEqual("3001", response.Messages.FirstOrDefault().Code);
        }

        [TestMethod]
        public void IsValidUserToSaveProject()
        {
            mockProjectRepo.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Project, bool>>>())).Returns(mockProjectDbData);
            
            mockDataRepo.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Data, bool>>>())).Returns((Expression<Func<DbModel.Data, bool>> predicate) => mockInvoicePaymentTerms.Union(mockCurrency).Union(mockIndustrySector).Union(mockProjectType).Union(mockLogo).Where(predicate));
            mockContractRepo.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Contract, bool>>>())).Returns((Expression<Func<DbModel.Contract, bool>> Predicate) => mockContractDbData.Where(Predicate));
           
            Validation = new Evolution.ValidationService.Services.ValidationService();
            validationService = new Evolution.Project.Infrastructure.Validations.ProjectValidationService(Validation);
            projectService = new Evolution.Project.Core.Services.ProjectService(mockLogger.Object, mockProjectRepo.Object, validationService, Mapper.Instance, contractExchangeRateService, currencyExchangeRateService, masterService, mockDataRepo.Object, mockContractRepo.Object, mockAssignmentRepo.Object, mockCompanyRepo.Object, mockCustomerRepo.Object);
            projectDomainModel.ToList().ForEach(x => x.RecordStatus = "N");
            projectDomainModel[0].ManagedServiceCoordinatorName = "M.Peacock";
            var response = projectService.SaveProjects(new List<DomModel.Project> { projectDomainModel[0] });
            Assert.AreEqual("11", response.Code);
            Assert.IsNotNull(response.Messages);
            Assert.AreEqual("3009", response.Messages.FirstOrDefault().Code);
        }

        [TestMethod]
        public void IsValidCurrencyToSaveProject()
        {
            mockProjectRepo.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Project, bool>>>())).Returns(mockProjectDbData);
            
            mockDataRepo.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Data, bool>>>())).Returns((Expression<Func<DbModel.Data, bool>> predicate) => mockInvoicePaymentTerms.Union(mockCurrency).Union(mockIndustrySector).Union(mockProjectType).Union(mockLogo).Where(predicate));
            mockContractRepo.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Contract, bool>>>())).Returns((Expression<Func<DbModel.Contract, bool>> Predicate) => mockContractDbData.Where(Predicate));
            
            Validation = new Evolution.ValidationService.Services.ValidationService();
            validationService = new Evolution.Project.Infrastructure.Validations.ProjectValidationService(Validation);
            projectService = new Evolution.Project.Core.Services.ProjectService(mockLogger.Object, mockProjectRepo.Object, validationService, Mapper.Instance, contractExchangeRateService, currencyExchangeRateService, masterService, mockDataRepo.Object, mockContractRepo.Object, mockAssignmentRepo.Object, mockCompanyRepo.Object, mockCustomerRepo.Object);
            projectDomainModel.ToList().ForEach(x => x.RecordStatus = "N");
            projectDomainModel[0].ProjectInvoicingCurrency = "USD";
            var response = projectService.SaveProjects(new List<DomModel.Project> { projectDomainModel[0] });
            Assert.AreEqual("11", response.Code);
            Assert.IsNotNull(response.Messages);
            Assert.AreEqual("3017", response.Messages.FirstOrDefault().Code);
        }

        [TestMethod]
        public void IsValidLogoToSaveProject()
        {
            mockProjectRepo.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Project, bool>>>())).Returns(mockProjectDbData);
           
            mockDataRepo.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Data, bool>>>())).Returns((Expression<Func<DbModel.Data, bool>> predicate) => mockInvoicePaymentTerms.Union(mockCurrency).Union(mockIndustrySector).Union(mockProjectType).Union(mockLogo).Where(predicate));
            mockContractRepo.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Contract, bool>>>())).Returns((Expression<Func<DbModel.Contract, bool>> Predicate) => mockContractDbData.Where(Predicate));
           
            Validation = new Evolution.ValidationService.Services.ValidationService();
            validationService = new Evolution.Project.Infrastructure.Validations.ProjectValidationService(Validation);
            projectService = new Evolution.Project.Core.Services.ProjectService(mockLogger.Object, mockProjectRepo.Object, validationService, Mapper.Instance, contractExchangeRateService, currencyExchangeRateService, masterService, mockDataRepo.Object, mockContractRepo.Object, mockAssignmentRepo.Object, mockCompanyRepo.Object, mockCustomerRepo.Object);
            projectDomainModel.ToList().ForEach(x => x.RecordStatus = "N");
            projectDomainModel[0].LogoText = "Netserv";
            var response = projectService.SaveProjects(new List<DomModel.Project> { projectDomainModel[0] });
            Assert.AreEqual("11", response.Code);
            Assert.IsNotNull(response.Messages);
            Assert.AreEqual("3018", response.Messages.FirstOrDefault().Code);
        }

        [TestMethod]
        public void IsValidCustomerAddressToSaveProject()
        {
            mockProjectRepo.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Project, bool>>>())).Returns(mockProjectDbData);
            
            mockDataRepo.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Data, bool>>>())).Returns((Expression<Func<DbModel.Data, bool>> predicate) => mockInvoicePaymentTerms.Union(mockCurrency).Union(mockIndustrySector).Union(mockProjectType).Union(mockLogo).Where(predicate));
            mockContractRepo.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Contract, bool>>>())).Returns((Expression<Func<DbModel.Contract, bool>> Predicate) => mockContractDbData.Where(Predicate));
           
            Validation = new Evolution.ValidationService.Services.ValidationService();
            validationService = new Evolution.Project.Infrastructure.Validations.ProjectValidationService(Validation);
            projectService = new Evolution.Project.Core.Services.ProjectService(mockLogger.Object, mockProjectRepo.Object, validationService, Mapper.Instance, contractExchangeRateService, currencyExchangeRateService, masterService, mockDataRepo.Object, mockContractRepo.Object, mockAssignmentRepo.Object, mockCompanyRepo.Object, mockCustomerRepo.Object);
            projectDomainModel.ToList().ForEach(x => x.RecordStatus = "N");
            projectDomainModel[0].ProjectCustomerContactAddress = "Banglore";
            var response = projectService.SaveProjects(new List<DomModel.Project> { projectDomainModel[0] });
            Assert.AreEqual("11", response.Code);
            Assert.IsNotNull(response.Messages);
            Assert.AreEqual("2264", response.Messages.FirstOrDefault().Code);
        }


        [TestMethod]
        public void IsValidCustomerInvoiceAddressToSaveProject()
        {
            mockProjectRepo.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Project, bool>>>())).Returns(mockProjectDbData);
           
            mockDataRepo.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Data, bool>>>())).Returns((Expression<Func<DbModel.Data, bool>> predicate) => mockInvoicePaymentTerms.Union(mockCurrency).Union(mockIndustrySector).Union(mockProjectType).Union(mockLogo).Where(predicate));
            mockContractRepo.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Contract, bool>>>())).Returns((Expression<Func<DbModel.Contract, bool>> Predicate) => mockContractDbData.Where(Predicate));
          
            Validation = new Evolution.ValidationService.Services.ValidationService();
            validationService = new Evolution.Project.Infrastructure.Validations.ProjectValidationService(Validation);
            projectService = new Evolution.Project.Core.Services.ProjectService(mockLogger.Object, mockProjectRepo.Object, validationService, Mapper.Instance, contractExchangeRateService, currencyExchangeRateService, masterService, mockDataRepo.Object, mockContractRepo.Object, mockAssignmentRepo.Object, mockCompanyRepo.Object, mockCustomerRepo.Object);
            projectDomainModel.ToList().ForEach(x => x.RecordStatus = "N");
            projectDomainModel[0].ProjectCustomerContactAddress = "Banglore";
            var response = projectService.SaveProjects(new List<DomModel.Project> { projectDomainModel[0] });
            Assert.AreEqual("11", response.Code);
            Assert.IsNotNull(response.Messages);
            Assert.AreEqual("2264", response.Messages.FirstOrDefault().Code);
        }

        [TestMethod]
        public void ShouldNotDeleteProjectWhenProjectStatusIsClosed()
        {
            mockProjectDbData.ToList().ForEach(x=>x.Status="C");
            mockProjectRepo.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Project, bool>>>())).Returns(mockProjectDbData.Where(x=>x.Id==1));
           
            Validation = new Evolution.ValidationService.Services.ValidationService();
            validationService = new Evolution.Project.Infrastructure.Validations.ProjectValidationService(Validation);
            projectService = new Evolution.Project.Core.Services.ProjectService(mockLogger.Object, mockProjectRepo.Object, validationService, Mapper.Instance, contractExchangeRateService, currencyExchangeRateService, masterService, mockDataRepo.Object, mockContractRepo.Object, mockAssignmentRepo.Object, mockCompanyRepo.Object, mockCustomerRepo.Object);
            projectDomainModel.ToList().ForEach(x => x.RecordStatus = "D");
            projectDomainModel[0].ProjectStatus = "C";
            var response = projectService.DeleteProjects(new List<DomModel.Project> { projectDomainModel[0] });
            Assert.AreEqual("11", response.Code);
            Assert.IsNotNull(response.Messages);
            Assert.AreEqual("3021", response.Messages.FirstOrDefault().Code);
        }

        [TestMethod]
        public void UpdateCountMissMatchTest()
        {
            mockProjectRepo.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Project, bool>>>())).Returns(mockProjectDbData);

            mockDataRepo.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Data, bool>>>())).Returns((Expression<Func<DbModel.Data, bool>> predicate) => mockInvoicePaymentTerms.Union(mockCurrency).Union(mockIndustrySector).Union(mockProjectType).Union(mockLogo).Where(predicate));
            mockContractRepo.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Contract, bool>>>())).Returns((Expression<Func<DbModel.Contract, bool>> Predicate) => mockContractDbData.Where(Predicate));
           // mockProjectRepo.Setup(moq => moq.ForceSave()).Returns(Task<int>.Factory.StartNew(() => 1));
            Validation = new Evolution.ValidationService.Services.ValidationService();
            validationService = new Evolution.Project.Infrastructure.Validations.ProjectValidationService(Validation);
            projectService = new Evolution.Project.Core.Services.ProjectService(mockLogger.Object, mockProjectRepo.Object, validationService, Mapper.Instance, contractExchangeRateService, currencyExchangeRateService, masterService, mockDataRepo.Object, mockContractRepo.Object, mockAssignmentRepo.Object, mockCompanyRepo.Object, mockCustomerRepo.Object);
            projectDomainModel.ToList().ForEach(x => x.RecordStatus = "M");
            projectDomainModel[0].UpdateCount = 3;
            var response = projectService.ModifyProjects(new List<DomModel.Project> { projectDomainModel[0] });
            Assert.AreEqual("11", response.Code);
            Assert.IsNotNull(response.Messages);
            Assert.AreEqual("3020", response.Messages.FirstOrDefault().Code);
        }

        [TestMethod]
        public void IsValidProjectTypeToSaveProject()
        {
            mockProjectRepo.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Project, bool>>>())).Returns(mockProjectDbData);

            mockDataRepo.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Data, bool>>>())).Returns((Expression<Func<DbModel.Data, bool>> predicate) => mockInvoicePaymentTerms.Union(mockCurrency).Union(mockIndustrySector).Union(mockProjectType).Union(mockLogo).Where(predicate));
            mockContractRepo.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Contract, bool>>>())).Returns((Expression<Func<DbModel.Contract, bool>> Predicate) => mockContractDbData.Where(Predicate));

            Validation = new Evolution.ValidationService.Services.ValidationService();
            validationService = new Evolution.Project.Infrastructure.Validations.ProjectValidationService(Validation);
            projectService = new Evolution.Project.Core.Services.ProjectService(mockLogger.Object, mockProjectRepo.Object, validationService, Mapper.Instance, contractExchangeRateService, currencyExchangeRateService, masterService, mockDataRepo.Object, mockContractRepo.Object, mockAssignmentRepo.Object, mockCompanyRepo.Object, mockCustomerRepo.Object);
            projectDomainModel.ToList().ForEach(x => x.RecordStatus = "N");
            projectDomainModel[0].ProjectType = "ABCD";
            var response = projectService.SaveProjects(new List<DomModel.Project> { projectDomainModel[0] });
            Assert.AreEqual("11", response.Code);
            Assert.IsNotNull(response.Messages);
            Assert.AreEqual("3005", response.Messages.FirstOrDefault().Code);
        }

        [TestMethod]
        public void IsValidIndustrySectorToSaveProject()
        {
            mockProjectRepo.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Project, bool>>>())).Returns(mockProjectDbData);

            mockDataRepo.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Data, bool>>>())).Returns((Expression<Func<DbModel.Data, bool>> predicate) => mockInvoicePaymentTerms.Union(mockCurrency).Union(mockIndustrySector).Union(mockProjectType).Union(mockLogo).Where(predicate));
            mockContractRepo.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Contract, bool>>>())).Returns((Expression<Func<DbModel.Contract, bool>> Predicate) => mockContractDbData.Where(Predicate));

            Validation = new Evolution.ValidationService.Services.ValidationService();
            validationService = new Evolution.Project.Infrastructure.Validations.ProjectValidationService(Validation);
            projectService = new Evolution.Project.Core.Services.ProjectService(mockLogger.Object, mockProjectRepo.Object, validationService, Mapper.Instance, contractExchangeRateService, currencyExchangeRateService, masterService, mockDataRepo.Object, mockContractRepo.Object, mockAssignmentRepo.Object, mockCompanyRepo.Object, mockCustomerRepo.Object);
            projectDomainModel.ToList().ForEach(x => x.RecordStatus = "N");
            projectDomainModel[0].IndustrySector = "AIM";
            var response = projectService.SaveProjects(new List<DomModel.Project> { projectDomainModel[0] });
            Assert.AreEqual("11", response.Code);
            Assert.IsNotNull(response.Messages);
            Assert.AreEqual("3006", response.Messages.FirstOrDefault().Code);
        }


        [TestMethod]
        public void IsValidCustomerContactToSaveProject()
        {
            mockProjectRepo.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Project, bool>>>())).Returns(mockProjectDbData);

            mockDataRepo.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Data, bool>>>())).Returns((Expression<Func<DbModel.Data, bool>> predicate) => mockInvoicePaymentTerms.Union(mockCurrency).Union(mockIndustrySector).Union(mockProjectType).Union(mockLogo).Where(predicate));
            mockContractRepo.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Contract, bool>>>())).Returns((Expression<Func<DbModel.Contract, bool>> Predicate) => mockContractDbData.Where(Predicate));

            Validation = new Evolution.ValidationService.Services.ValidationService();
            validationService = new Evolution.Project.Infrastructure.Validations.ProjectValidationService(Validation);
            projectService = new Evolution.Project.Core.Services.ProjectService(mockLogger.Object, mockProjectRepo.Object, validationService, Mapper.Instance, contractExchangeRateService, currencyExchangeRateService, masterService, mockDataRepo.Object, mockContractRepo.Object, mockAssignmentRepo.Object, mockCompanyRepo.Object, mockCustomerRepo.Object);
            projectDomainModel.ToList().ForEach(x => x.RecordStatus = "N");
            projectDomainModel[0].ProjectCustomerContact = "Mr.RAW";
            var response = projectService.SaveProjects(new List<DomModel.Project> { projectDomainModel[0] });
            Assert.AreEqual("11", response.Code);
            Assert.IsNotNull(response.Messages);
            Assert.AreEqual("2163", response.Messages.FirstOrDefault().Code);
        }

        [TestMethod]
        public void IsValidCustomerInvoiceContactToSaveProject()
        {
            mockProjectRepo.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Project, bool>>>())).Returns(mockProjectDbData);

            mockDataRepo.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Data, bool>>>())).Returns((Expression<Func<DbModel.Data, bool>> predicate) => mockInvoicePaymentTerms.Union(mockCurrency).Union(mockIndustrySector).Union(mockProjectType).Union(mockLogo).Where(predicate));
            mockContractRepo.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Contract, bool>>>())).Returns((Expression<Func<DbModel.Contract, bool>> Predicate) => mockContractDbData.Where(Predicate));

            Validation = new Evolution.ValidationService.Services.ValidationService();
            validationService = new Evolution.Project.Infrastructure.Validations.ProjectValidationService(Validation);
            projectService = new Evolution.Project.Core.Services.ProjectService(mockLogger.Object, mockProjectRepo.Object, validationService, Mapper.Instance, contractExchangeRateService, currencyExchangeRateService, masterService, mockDataRepo.Object, mockContractRepo.Object, mockAssignmentRepo.Object, mockCompanyRepo.Object, mockCustomerRepo.Object);
            projectDomainModel.ToList().ForEach(x => x.RecordStatus = "N");
            projectDomainModel[0].ProjectCustomerInvoiceContact = "Mr.RAW";
            var response = projectService.SaveProjects(new List<DomModel.Project> { projectDomainModel[0] });
            Assert.AreEqual("11", response.Code);
            Assert.IsNotNull(response.Messages);
            Assert.AreEqual("2164", response.Messages.FirstOrDefault().Code);
        }

        [TestMethod]
        public void IsValidInvoicePaymentTermsToSaveProject()
        {
            mockProjectRepo.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Project, bool>>>())).Returns(mockProjectDbData);

            mockDataRepo.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Data, bool>>>())).Returns((Expression<Func<DbModel.Data, bool>> predicate) => mockInvoicePaymentTerms.Union(mockCurrency).Union(mockIndustrySector).Union(mockProjectType).Union(mockLogo).Where(predicate));
            mockContractRepo.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Contract, bool>>>())).Returns((Expression<Func<DbModel.Contract, bool>> Predicate) => mockContractDbData.Where(Predicate));

            Validation = new Evolution.ValidationService.Services.ValidationService();
            validationService = new Evolution.Project.Infrastructure.Validations.ProjectValidationService(Validation);
            projectService = new Evolution.Project.Core.Services.ProjectService(mockLogger.Object, mockProjectRepo.Object, validationService, Mapper.Instance, contractExchangeRateService, currencyExchangeRateService, masterService, mockDataRepo.Object, mockContractRepo.Object, mockAssignmentRepo.Object, mockCompanyRepo.Object, mockCustomerRepo.Object);
            projectDomainModel.ToList().ForEach(x => x.RecordStatus = "N");
            projectDomainModel[0].ProjectInvoicePaymentTerms = "GST";
            var response = projectService.SaveProjects(new List<DomModel.Project> { projectDomainModel[0] });
            Assert.AreEqual("11", response.Code);
            Assert.IsNotNull(response.Messages);
            Assert.AreEqual("10002", response.Messages.FirstOrDefault().Code);
        }

        [TestMethod]
        public void IsValidContractRemiitenceTextToSaveProject()
        {
            mockProjectRepo.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Project, bool>>>())).Returns(mockProjectDbData);

            mockDataRepo.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Data, bool>>>())).Returns((Expression<Func<DbModel.Data, bool>> predicate) => mockInvoicePaymentTerms.Union(mockCurrency).Union(mockIndustrySector).Union(mockProjectType).Union(mockLogo).Where(predicate));
            mockContractRepo.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Contract, bool>>>())).Returns((Expression<Func<DbModel.Contract, bool>> Predicate) => mockContractDbData.Where(Predicate));

            Validation = new Evolution.ValidationService.Services.ValidationService();
            validationService = new Evolution.Project.Infrastructure.Validations.ProjectValidationService(Validation);
            projectService = new Evolution.Project.Core.Services.ProjectService(mockLogger.Object, mockProjectRepo.Object, validationService, Mapper.Instance, contractExchangeRateService, currencyExchangeRateService, masterService, mockDataRepo.Object, mockContractRepo.Object, mockAssignmentRepo.Object, mockCompanyRepo.Object, mockCustomerRepo.Object);
            projectDomainModel.ToList().ForEach(x => x.RecordStatus = "N");
            projectDomainModel[0].ProjectInvoiceRemittanceIdentifier = "TEXT";
            var response = projectService.SaveProjects(new List<DomModel.Project> { projectDomainModel[0] });
            Assert.AreEqual("11", response.Code);
            Assert.IsNotNull(response.Messages);
            Assert.AreEqual("10003", response.Messages.FirstOrDefault().Code);
        }

        [TestMethod]
        public void IsValidContractFooterTextToSaveProject()
        {
            mockProjectRepo.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Project, bool>>>())).Returns(mockProjectDbData);

            mockDataRepo.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Data, bool>>>())).Returns((Expression<Func<DbModel.Data, bool>> predicate) => mockInvoicePaymentTerms.Union(mockCurrency).Union(mockIndustrySector).Union(mockProjectType).Union(mockLogo).Where(predicate));
            mockContractRepo.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Contract, bool>>>())).Returns((Expression<Func<DbModel.Contract, bool>> Predicate) => mockContractDbData.Where(Predicate));

            Validation = new Evolution.ValidationService.Services.ValidationService();
            validationService = new Evolution.Project.Infrastructure.Validations.ProjectValidationService(Validation);
            projectService = new Evolution.Project.Core.Services.ProjectService(mockLogger.Object, mockProjectRepo.Object, validationService, Mapper.Instance, contractExchangeRateService, currencyExchangeRateService, masterService, mockDataRepo.Object, mockContractRepo.Object, mockAssignmentRepo.Object, mockCompanyRepo.Object, mockCustomerRepo.Object);
            projectDomainModel.ToList().ForEach(x => x.RecordStatus = "N");
            projectDomainModel[0].ProjectInvoiceFooterIdentifier = "TEXT";
            var response = projectService.SaveProjects(new List<DomModel.Project> { projectDomainModel[0] });
            Assert.AreEqual("11", response.Code);
            Assert.IsNotNull(response.Messages);
            Assert.AreEqual("10004", response.Messages.FirstOrDefault().Code);
        }

        [TestMethod]
        public void IsValidWithHoldingTaxSaveProject()
        {
            mockProjectRepo.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Project, bool>>>())).Returns(mockProjectDbData);

            mockDataRepo.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Data, bool>>>())).Returns((Expression<Func<DbModel.Data, bool>> predicate) => mockInvoicePaymentTerms.Union(mockCurrency).Union(mockIndustrySector).Union(mockProjectType).Union(mockLogo).Where(predicate));
            mockContractRepo.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Contract, bool>>>())).Returns((Expression<Func<DbModel.Contract, bool>> Predicate) => mockContractDbData.Where(Predicate));

            Validation = new Evolution.ValidationService.Services.ValidationService();
            validationService = new Evolution.Project.Infrastructure.Validations.ProjectValidationService(Validation);
            projectService = new Evolution.Project.Core.Services.ProjectService(mockLogger.Object, mockProjectRepo.Object, validationService, Mapper.Instance, contractExchangeRateService, currencyExchangeRateService, masterService, mockDataRepo.Object, mockContractRepo.Object, mockAssignmentRepo.Object, mockCompanyRepo.Object, mockCustomerRepo.Object);
            projectDomainModel.ToList().ForEach(x => x.RecordStatus = "N");
            projectDomainModel[0].ProjectWithHoldingTax = "TAX";
            var response = projectService.SaveProjects(new List<DomModel.Project> { projectDomainModel[0] });
            Assert.AreEqual("11", response.Code);
            Assert.IsNotNull(response.Messages);
            Assert.AreEqual("1914", response.Messages.FirstOrDefault().Code);
        }

        [TestMethod]
        public void IsValidSalesTaxToSaveProject()
        {
            mockProjectRepo.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Project, bool>>>())).Returns(mockProjectDbData);

            mockDataRepo.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Data, bool>>>())).Returns((Expression<Func<DbModel.Data, bool>> predicate) => mockInvoicePaymentTerms.Union(mockCurrency).Union(mockIndustrySector).Union(mockProjectType).Union(mockLogo).Where(predicate));
            mockContractRepo.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Contract, bool>>>())).Returns((Expression<Func<DbModel.Contract, bool>> Predicate) => mockContractDbData.Where(Predicate));

            Validation = new Evolution.ValidationService.Services.ValidationService();
            validationService = new Evolution.Project.Infrastructure.Validations.ProjectValidationService(Validation);
            projectService = new Evolution.Project.Core.Services.ProjectService(mockLogger.Object, mockProjectRepo.Object, validationService, Mapper.Instance, contractExchangeRateService, currencyExchangeRateService, masterService, mockDataRepo.Object, mockContractRepo.Object, mockAssignmentRepo.Object, mockCompanyRepo.Object, mockCustomerRepo.Object);
            projectDomainModel.ToList().ForEach(x => x.RecordStatus = "N");
            projectDomainModel[0].ProjectSalesTax = "TAX";
            var response = projectService.SaveProjects(new List<DomModel.Project> { projectDomainModel[0] });
            Assert.AreEqual("11", response.Code);
            Assert.IsNotNull(response.Messages);
            Assert.AreEqual("1915", response.Messages.FirstOrDefault().Code);
        }
    }
}
