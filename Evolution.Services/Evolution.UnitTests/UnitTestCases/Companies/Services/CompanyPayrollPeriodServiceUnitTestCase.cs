using Evolution.Logging.Interfaces;
using Evolution.UnitTests.Mocks.Data.Companies;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using DomainData = Evolution.Company.Domain.Interfaces.Data;
using DomModel = Evolution.Company.Domain.Models.Companies;
using DbModel = Evolution.DbRepository.Models;
using ServiceDomainData = Evolution.Company.Domain.Interfaces.Companies;
using Evolution.UnitTests.Mocks.Data.Companies.Db;
using System.Linq.Expressions;

namespace Evolution.UnitTests.UnitTestCases.Companies.Services
{
    [TestClass]
    public class CompanyPayrollPeriodServiceUnitTestCase
    {
        ServiceDomainData.ICompanyPayrollPeriodService companyPayrollPeriodService = null;
        Mock<DomainData.ICompanyPayrollPeriodRepository> mockCompanyPayrollPeriodRepository = null;
        Mock<DomainData.ICompanyRepository> mockCompanyRepository = null;
        Mock<DomainData.ICompanyPayrollRepository> mockCompanyPayrollRepository = null;
        IList<DomModel.CompanyPayrollPeriod> companyPayrollPeriodDomianModels = null;
        Mock<IAppLogger<Company.Core.Services.CompanyPayrollPeriodService>> mockLogger = null;
        IQueryable<DbModel.Company> mockComapanyDbData = null;
        IQueryable<DbModel.Data> mockPayrollTypeDbData = null;
        IQueryable<DbModel.CompanyPayroll> mockCompanyPayrollDbData = null;
        IQueryable<DbModel.CompanyPayrollPeriod> mockCompanyPayrollPeroidDbData = null;

        [TestInitialize]
        public void InitializeCompanyPayrollPeriodService()
        {
            mockCompanyRepository = new Mock<DomainData.ICompanyRepository>();
            mockCompanyPayrollPeriodRepository = new Mock<DomainData.ICompanyPayrollPeriodRepository>();
            companyPayrollPeriodDomianModels = MockCompanyDomainModel.GetCompanyPayrollPeriodMockedDomainModelData(); 
            mockLogger = new Mock<IAppLogger<Company.Core.Services.CompanyPayrollPeriodService>>();
            mockCompanyPayrollRepository = new Mock<DomainData.ICompanyPayrollRepository>();
            mockComapanyDbData = MockCompany.GetCompanyMockData();
            mockPayrollTypeDbData = MockCompany.GetPayrollTypeMockData();
            mockCompanyPayrollDbData = MockCompany.GetCompanyPayrollMockData();
            mockCompanyPayrollPeroidDbData = MockCompany.GetCompanyPayrollPeriodMockData();

        }

        [TestMethod]
        public void FetchCompanyPayrollPeriodListWithoutSearchValue()
        {
            mockCompanyPayrollPeriodRepository.Setup(x => x.Search(It.IsAny<DomModel.CompanyPayrollPeriod>())).Returns(companyPayrollPeriodDomianModels);

            companyPayrollPeriodService = new Company.Core.Services.CompanyPayrollPeriodService(mockCompanyPayrollPeriodRepository.Object, mockLogger.Object,mockCompanyRepository.Object, mockCompanyPayrollRepository.Object);

            var response = companyPayrollPeriodService.GetCompanyPayrollPeriod(new DomModel.CompanyPayrollPeriod());

            Assert.AreEqual("1", response.Code);
            Assert.AreEqual(2, (response.Result as List<DomModel.CompanyPayrollPeriod>).Count);
        }

        [TestMethod]
        public void FetchCompanyPayrollPeriodListWithNullSearchModel()
        {
            mockCompanyPayrollPeriodRepository.Setup(x => x.Search(It.IsAny<DomModel.CompanyPayrollPeriod>())).Returns(companyPayrollPeriodDomianModels);

            companyPayrollPeriodService = new Company.Core.Services.CompanyPayrollPeriodService(mockCompanyPayrollPeriodRepository.Object, mockLogger.Object,mockCompanyRepository.Object, mockCompanyPayrollRepository.Object);

            var response = companyPayrollPeriodService.GetCompanyPayrollPeriod(null);

            Assert.AreEqual("1", response.Code);
            Assert.AreEqual(2, (response.Result as List<DomModel.CompanyPayrollPeriod>).Count);
        }

        [TestMethod]
        public void FetchCompanyPayrollPeriodListByCompanyCode()
        {
            mockCompanyPayrollPeriodRepository.Setup(x => x.Search(It.Is<DomModel.CompanyPayrollPeriod>(c => c.CompanyCode == "UK050"))).Returns(new List<DomModel.CompanyPayrollPeriod> { companyPayrollPeriodDomianModels[0] });

            companyPayrollPeriodService = new Company.Core.Services.CompanyPayrollPeriodService(mockCompanyPayrollPeriodRepository.Object, mockLogger.Object, mockCompanyRepository.Object, mockCompanyPayrollRepository.Object);

            var response = companyPayrollPeriodService.GetCompanyPayrollPeriod(new DomModel.CompanyPayrollPeriod { CompanyCode = "UK050" });

            Assert.AreEqual("1", response.Code);
            Assert.AreEqual(1, (response.Result as List<DomModel.CompanyPayrollPeriod>).Count);
        }

        [TestMethod]
        public void FetchCompanyPayrollPeriodListByWildCardSearchWithPayrollPeriodNameStartWith()
        {
            mockCompanyPayrollPeriodRepository.Setup(x => x.Search(It.Is<DomModel.CompanyPayrollPeriod>(c => c.PeriodName.StartsWith("Inv")))).Returns(new List<DomModel.CompanyPayrollPeriod> { companyPayrollPeriodDomianModels[0] });

            companyPayrollPeriodService = new Company.Core.Services.CompanyPayrollPeriodService(mockCompanyPayrollPeriodRepository.Object, mockLogger.Object, mockCompanyRepository.Object, mockCompanyPayrollRepository.Object);

            var response = companyPayrollPeriodService.GetCompanyPayrollPeriod(new DomModel.CompanyPayrollPeriod { PeriodName = "Inv*" });

            Assert.AreEqual("1", response.Code);
            Assert.AreEqual(1, (response.Result as List<DomModel.CompanyPayrollPeriod>).Count);
        }

        [TestMethod]
        public void FetchCompanyPayrollPeriodListByWildCardSearchWithPayrollPeriodNameEndWith()
        {
            mockCompanyPayrollPeriodRepository.Setup(x => x.Search(It.Is<DomModel.CompanyPayrollPeriod>(c => c.PeriodName.EndsWith("12")))).Returns(new List<DomModel.CompanyPayrollPeriod> { companyPayrollPeriodDomianModels[1] });

            companyPayrollPeriodService = new Company.Core.Services.CompanyPayrollPeriodService(mockCompanyPayrollPeriodRepository.Object, mockLogger.Object, mockCompanyRepository.Object, mockCompanyPayrollRepository.Object);

            var response = companyPayrollPeriodService.GetCompanyPayrollPeriod(new DomModel.CompanyPayrollPeriod { PeriodName = "*12" });

            Assert.AreEqual("1", response.Code);
            Assert.AreEqual(1, (response.Result as List<DomModel.CompanyPayrollPeriod>).Count);
        }

        [TestMethod]
        public void FetchCompanyPayrollPeriodListByWildCardSearchWithPayrollPeriodNameContains()
        {
            mockCompanyPayrollPeriodRepository.Setup(x => x.Search(It.Is<DomModel.CompanyPayrollPeriod>(c => c.PeriodName.Contains("Month")))).Returns(new List<DomModel.CompanyPayrollPeriod> { companyPayrollPeriodDomianModels[1] });

            companyPayrollPeriodService = new Company.Core.Services.CompanyPayrollPeriodService(mockCompanyPayrollPeriodRepository.Object, mockLogger.Object, mockCompanyRepository.Object, mockCompanyPayrollRepository.Object);

            var response = companyPayrollPeriodService.GetCompanyPayrollPeriod(new DomModel.CompanyPayrollPeriod { PeriodName = "*Month*" });

            Assert.AreEqual("1", response.Code);
            Assert.AreEqual(1, (response.Result as List<DomModel.CompanyPayrollPeriod>).Count);
        }

        [TestMethod]
        public void ThrowsExceptionWhileFetchingCompanyPayrollPeriodList()
        {
            mockCompanyPayrollPeriodRepository.Setup(x => x.Search(It.IsAny<DomModel.CompanyPayrollPeriod>())).Throws(new Exception("Exception occured while performing some operation."));
            companyPayrollPeriodService = new Company.Core.Services.CompanyPayrollPeriodService(mockCompanyPayrollPeriodRepository.Object, mockLogger.Object, mockCompanyRepository.Object, mockCompanyPayrollRepository.Object);

            var response = companyPayrollPeriodService.GetCompanyPayrollPeriod(new DomModel.CompanyPayrollPeriod());

            Assert.AreEqual("11", response.Code);
            Assert.IsNotNull(response.Messages);
            Assert.AreEqual("31", response.Messages.FirstOrDefault().Code);
        }

        [TestMethod]
        public void SaveCompanyPayrollPeriods()
        {
            mockCompanyRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Company, bool>>>())).Returns(mockComapanyDbData);
            mockCompanyPayrollRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.CompanyPayroll, bool>>>())).Returns((Expression<Func<DbModel.CompanyPayroll, bool>> predicate) => mockCompanyPayrollDbData.Where(predicate));
            mockCompanyPayrollPeriodRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.CompanyPayrollPeriod, bool>>>())).Returns((Expression<Func<DbModel.CompanyPayrollPeriod, bool>> predicate) => mockCompanyPayrollPeroidDbData.Where(predicate));

            companyPayrollPeriodService = new Company.Core.Services.CompanyPayrollPeriodService(mockCompanyPayrollPeriodRepository.Object, mockLogger.Object, mockCompanyRepository.Object, mockCompanyPayrollRepository.Object);

            companyPayrollPeriodDomianModels.ToList().ForEach(x => x.RecordStatus = "N");
            companyPayrollPeriodDomianModels[0].PeriodName = "Test Period";
           var response = companyPayrollPeriodService.SaveCompanyPayrollPeriod(companyPayrollPeriodDomianModels[0].CompanyCode, companyPayrollPeriodDomianModels[0].PayrollType, new List<DomModel.CompanyPayrollPeriod> { companyPayrollPeriodDomianModels[0] });

            mockCompanyPayrollPeriodRepository.Verify(m => m.Add(It.IsAny<IList<DbModel.CompanyPayrollPeriod>>()), Times.AtLeastOnce);
        }

        [TestMethod]
        public void ModifyCompanyPayrollPeriod()
        {
            mockCompanyRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Company, bool>>>())).Returns(mockComapanyDbData);
            mockCompanyPayrollRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.CompanyPayroll, bool>>>())).Returns((Expression<Func<DbModel.CompanyPayroll, bool>> predicate) => mockCompanyPayrollDbData.Where(predicate));
            mockCompanyPayrollPeriodRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.CompanyPayrollPeriod, bool>>>())).Returns((Expression<Func<DbModel.CompanyPayrollPeriod, bool>> predicate) => mockCompanyPayrollPeroidDbData.Where(predicate));

            companyPayrollPeriodService = new Company.Core.Services.CompanyPayrollPeriodService(mockCompanyPayrollPeriodRepository.Object, mockLogger.Object, mockCompanyRepository.Object, mockCompanyPayrollRepository.Object);
            companyPayrollPeriodDomianModels.ToList().ForEach(x => x.RecordStatus = "M");
            var response = companyPayrollPeriodService.ModifyCompanyPayrollPeriod(companyPayrollPeriodDomianModels[0].CompanyCode, companyPayrollPeriodDomianModels[0].PayrollType, new List<DomModel.CompanyPayrollPeriod> { companyPayrollPeriodDomianModels[0] });

            mockCompanyPayrollPeriodRepository.Verify(m => m.Update(It.IsAny<DbModel.CompanyPayrollPeriod>()), Times.AtLeast(1));
        }

        [TestMethod]
        public void DeleteCompanyPayrollPeriod()
        {
            mockCompanyRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Company, bool>>>())).Returns(mockComapanyDbData);
            mockCompanyPayrollRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.CompanyPayroll, bool>>>())).Returns((Expression<Func<DbModel.CompanyPayroll, bool>> predicate) => mockCompanyPayrollDbData.Where(predicate));
            mockCompanyPayrollPeriodRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.CompanyPayrollPeriod, bool>>>())).Returns((Expression<Func<DbModel.CompanyPayrollPeriod, bool>> predicate) => mockCompanyPayrollPeroidDbData.Where(predicate));
             
            companyPayrollPeriodService = new Company.Core.Services.CompanyPayrollPeriodService(mockCompanyPayrollPeriodRepository.Object, mockLogger.Object, mockCompanyRepository.Object, mockCompanyPayrollRepository.Object);
            companyPayrollPeriodDomianModels.ToList().ForEach(x => x.RecordStatus = "D");
            var response = companyPayrollPeriodService.DeleteCompanyPayrollPeriod(companyPayrollPeriodDomianModels[0].CompanyCode, companyPayrollPeriodDomianModels[0].PayrollType, new List<DomModel.CompanyPayrollPeriod> { companyPayrollPeriodDomianModels[0] });

            mockCompanyPayrollPeriodRepository.Verify(m => m.Delete(It.IsAny<DbModel.CompanyPayrollPeriod>()), Times.AtLeast(1));
        }
    }
}
