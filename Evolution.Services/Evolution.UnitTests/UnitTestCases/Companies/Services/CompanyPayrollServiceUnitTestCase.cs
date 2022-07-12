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
using Evolution.DbRepository.Interfaces.Master;
using System.Linq.Expressions;
using Evolution.UnitTests.Mocks.Data.Companies.Db;

namespace Evolution.UnitTests.UnitTestCases.Companies.Services
{
   [TestClass]
   public class CompanyPayrollServiceUnitTestCase
    {
        ServiceDomainData.ICompanyPayrollService companyPayrollService = null;
        Mock<DomainData.ICompanyPayrollRepository> mockCompanyPayrollRepository = null;
        Mock<DomainData.ICompanyRepository> mockCompanyRepository = null;
        Mock<IDataRepository> mockMasterDataRepository = null;
        IList<DomModel.CompanyPayroll> companyPayrollDomianModels = null;
        Mock<IAppLogger<Company.Core.Services.CompanyPayrollService>> mockLogger = null;
        IQueryable<DbModel.Company> mockComapanyDbData = null;
        IQueryable<DbModel.Data> mockPayrollTypeDbData = null;
        IQueryable<DbModel.Data> mockExportPrefixDbData = null;
        IQueryable<DbModel.CompanyPayroll> mockCompanyPayrollDbData = null;

        [TestInitialize]
        public void InitializeCompanyPayrollService()
        {
            mockCompanyRepository = new Mock<DomainData.ICompanyRepository>();
            mockCompanyPayrollRepository = new Mock<DomainData.ICompanyPayrollRepository>();
            mockMasterDataRepository = new Mock<IDataRepository>();
            mockComapanyDbData = MockCompany.GetCompanyMockData();
            mockPayrollTypeDbData=MockCompany.GetPayrollTypeMockData();
            mockExportPrefixDbData = MockCompany.GetExportPrefixMockData();
            mockCompanyPayrollDbData = MockCompany.GetCompanyPayrollMockData();
            companyPayrollDomianModels = MockCompanyDomainModel.GetCompanyPayrollMockedDomainModelData();
            mockLogger = new Mock<IAppLogger<Company.Core.Services.CompanyPayrollService>>();
        }

        [TestMethod]
        public void FetchCompanyPayrollListWithoutSearchValue()
        {
            mockCompanyPayrollRepository.Setup(x => x.Search(It.IsAny<DomModel.CompanyPayroll>())).Returns(companyPayrollDomianModels);

            companyPayrollService = new Company.Core.Services.CompanyPayrollService(mockCompanyPayrollRepository.Object, mockLogger.Object, mockMasterDataRepository.Object,mockCompanyRepository.Object);

            var response = companyPayrollService.GetCompanyPayroll(new DomModel.CompanyPayroll());

            Assert.AreEqual("1", response.Code);
            Assert.AreEqual(2, (response.Result as List<DomModel.CompanyPayroll>).Count);
        }

        [TestMethod]
        public void FetchCompanyPayrollListWithNullSearchModel()
        {
            mockCompanyPayrollRepository.Setup(x => x.Search(It.IsAny<DomModel.CompanyPayroll>())).Returns(companyPayrollDomianModels);

            companyPayrollService = new Company.Core.Services.CompanyPayrollService(mockCompanyPayrollRepository.Object, mockLogger.Object, mockMasterDataRepository.Object, mockCompanyRepository.Object);

            var response = companyPayrollService.GetCompanyPayroll(null);

            Assert.AreEqual("1", response.Code);
            Assert.AreEqual(2, (response.Result as List<DomModel.CompanyPayroll>).Count);
        }

        [TestMethod]
        public void FetchCompanyPayrollListByCompanyCode()
        {
            mockCompanyPayrollRepository.Setup(x => x.Search(It.Is<DomModel.CompanyPayroll>(c => c.CompanyCode == "UK050"))).Returns(new List<DomModel.CompanyPayroll> { companyPayrollDomianModels[0] });

            companyPayrollService = new Company.Core.Services.CompanyPayrollService(mockCompanyPayrollRepository.Object, mockLogger.Object, mockMasterDataRepository.Object, mockCompanyRepository.Object);

            var response = companyPayrollService.GetCompanyPayroll(new DomModel.CompanyPayroll { CompanyCode = "UK050" });

            Assert.AreEqual("1", response.Code);
            Assert.AreEqual(1, (response.Result as List<DomModel.CompanyPayroll>).Count);
        }

        [TestMethod]
        public void FetchCompanyPayrollListByWildCardSearchWithPayrollTypeStartWith()
        {
            mockCompanyPayrollRepository.Setup(x => x.Search(It.Is<DomModel.CompanyPayroll>(c => c.PayrollType.StartsWith("LTD")))).Returns(new List<DomModel.CompanyPayroll> { companyPayrollDomianModels[0] });

            companyPayrollService = new Company.Core.Services.CompanyPayrollService(mockCompanyPayrollRepository.Object, mockLogger.Object, mockMasterDataRepository.Object, mockCompanyRepository.Object);

            var response = companyPayrollService.GetCompanyPayroll(new DomModel.CompanyPayroll { PayrollType = "LTD*" });

            Assert.AreEqual("1", response.Code);
            Assert.AreEqual(1, (response.Result as List<DomModel.CompanyPayroll>).Count);
        }

        [TestMethod]
        public void FetchCompanyPayrollListByWildCardSearchWithPayrollTypeEndWith()
        {
            mockCompanyPayrollRepository.Setup(x => x.Search(It.Is<DomModel.CompanyPayroll>(c => c.PayrollType.EndsWith("/SE")))).Returns(new List<DomModel.CompanyPayroll> { companyPayrollDomianModels[0] });

            companyPayrollService = new Company.Core.Services.CompanyPayrollService(mockCompanyPayrollRepository.Object, mockLogger.Object, mockMasterDataRepository.Object, mockCompanyRepository.Object);

            var response = companyPayrollService.GetCompanyPayroll(new DomModel.CompanyPayroll { PayrollType = "*/SE" });

            Assert.AreEqual("1", response.Code);
            Assert.AreEqual(1, (response.Result as List<DomModel.CompanyPayroll>).Count);
        }

        [TestMethod]
        public void FetchCompanyPayrollListByWildCardSearchWithPayrollTypeContains()
        {
            mockCompanyPayrollRepository.Setup(x => x.Search(It.Is<DomModel.CompanyPayroll>(c => c.PayrollType.Contains("Month")))).Returns(new List<DomModel.CompanyPayroll> { companyPayrollDomianModels[1] });

            companyPayrollService = new Company.Core.Services.CompanyPayrollService(mockCompanyPayrollRepository.Object, mockLogger.Object, mockMasterDataRepository.Object, mockCompanyRepository.Object);

            var response = companyPayrollService.GetCompanyPayroll(new DomModel.CompanyPayroll { PayrollType = "*Month*" });

            Assert.AreEqual("1", response.Code);
            Assert.AreEqual(1, (response.Result as List<DomModel.CompanyPayroll>).Count);
        }

        [TestMethod]
        public void ThrowsExceptionWhileFetchingCompanyPayrollList()
        {
            mockCompanyPayrollRepository.Setup(x => x.Search(It.IsAny<DomModel.CompanyPayroll>())).Throws(new Exception("Exception occured while performing some operation."));
            companyPayrollService = new Company.Core.Services.CompanyPayrollService(mockCompanyPayrollRepository.Object, mockLogger.Object, mockMasterDataRepository.Object, mockCompanyRepository.Object);

            var response = companyPayrollService.GetCompanyPayroll(new DomModel.CompanyPayroll());

            Assert.AreEqual("11", response.Code);
            Assert.IsNotNull(response.Messages);
            Assert.AreEqual("31", response.Messages.FirstOrDefault().Code);
        }

        [TestMethod]
        public void SaveCompanyPayrolls()
        {
            mockCompanyRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Company, bool>>>())).Returns(mockComapanyDbData);
            mockMasterDataRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Data, bool>>>())).Returns((Expression<Func<DbModel.Data, bool>> predicate) => mockPayrollTypeDbData.Union(mockExportPrefixDbData).Where(predicate));
            mockCompanyPayrollRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.CompanyPayroll, bool>>>())).Returns((Expression<Func<DbModel.CompanyPayroll, bool>> predicate) => mockCompanyPayrollDbData.Where(predicate));
             
            companyPayrollService = new Company.Core.Services.CompanyPayrollService(mockCompanyPayrollRepository.Object, mockLogger.Object, mockMasterDataRepository.Object, mockCompanyRepository.Object);

            companyPayrollDomianModels.ToList().ForEach(x => x.RecordStatus = "N");
            companyPayrollDomianModels[0].ExportPrefix = "Amelia";
            companyPayrollDomianModels[0].PayrollType = "Ltd Monthly TS";
            var response = companyPayrollService.SaveCompanyPayroll(companyPayrollDomianModels[0].CompanyCode, new List<DomModel.CompanyPayroll> { companyPayrollDomianModels[0] });

            mockCompanyPayrollRepository.Verify(m => m.Add(It.IsAny<IList<DbModel.CompanyPayroll>>()), Times.AtLeastOnce);
        }

        [TestMethod]
        public void SaveCompanyPayrollAlreadyExisitngPayrollType()
        {
            mockCompanyRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Company, bool>>>())).Returns(mockComapanyDbData);
            mockMasterDataRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Data, bool>>>())).Returns((Expression<Func<DbModel.Data, bool>> predicate) => mockPayrollTypeDbData.Union(mockExportPrefixDbData).Where(predicate));
            mockCompanyPayrollRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.CompanyPayroll, bool>>>())).Returns((Expression<Func<DbModel.CompanyPayroll, bool>> predicate) => mockCompanyPayrollDbData.Where(predicate));

            companyPayrollService = new Company.Core.Services.CompanyPayrollService(mockCompanyPayrollRepository.Object, mockLogger.Object, mockMasterDataRepository.Object, mockCompanyRepository.Object);

            companyPayrollDomianModels.ToList().ForEach(x => x.RecordStatus = "N"); ;
            var response = companyPayrollService.SaveCompanyPayroll(companyPayrollDomianModels[0].CompanyCode, new List<DomModel.CompanyPayroll> { companyPayrollDomianModels[0] });

            mockCompanyPayrollRepository.Verify(m => m.Add(It.IsAny<IList<DbModel.CompanyPayroll>>()), Times.Never);
            Assert.AreEqual("1164", response.Messages.FirstOrDefault().Code);
        }

        [TestMethod]
        public void ModifyCompanyPayroll()
        {
            mockCompanyRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Company, bool>>>())).Returns(mockComapanyDbData);
            mockMasterDataRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Data, bool>>>())).Returns((Expression<Func<DbModel.Data, bool>> predicate) => mockPayrollTypeDbData.Union(mockExportPrefixDbData).Where(predicate));
            mockCompanyPayrollRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.CompanyPayroll, bool>>>())).Returns((Expression<Func<DbModel.CompanyPayroll, bool>> predicate) => mockCompanyPayrollDbData.Where(predicate));
 
            companyPayrollService = new Company.Core.Services.CompanyPayrollService(mockCompanyPayrollRepository.Object, mockLogger.Object, mockMasterDataRepository.Object, mockCompanyRepository.Object);
            companyPayrollDomianModels.ToList().ForEach(x => x.RecordStatus = "M");
            var response = companyPayrollService.ModifyCompanyPayroll(companyPayrollDomianModels[0].CompanyCode, new List<DomModel.CompanyPayroll> { companyPayrollDomianModels[0] });

            mockCompanyPayrollRepository.Verify(m => m.Update(It.IsAny<DbModel.CompanyPayroll>()), Times.AtLeast(1));
        }


        [TestMethod]
        public void DeleteCompanyPayroll()
        {
            mockCompanyRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Company, bool>>>())).Returns(mockComapanyDbData);
            mockMasterDataRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Data, bool>>>())).Returns((Expression<Func<DbModel.Data, bool>> predicate) => mockPayrollTypeDbData.Union(mockExportPrefixDbData).Where(predicate));
            mockCompanyPayrollRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.CompanyPayroll, bool>>>())).Returns((Expression<Func<DbModel.CompanyPayroll, bool>> predicate) => mockCompanyPayrollDbData.Where(predicate));

            companyPayrollService = new Company.Core.Services.CompanyPayrollService(mockCompanyPayrollRepository.Object, mockLogger.Object, mockMasterDataRepository.Object, mockCompanyRepository.Object);
             
            companyPayrollDomianModels.ToList().ForEach(x => x.RecordStatus = "D");
            var response = companyPayrollService.DeleteCompanyPayroll(companyPayrollDomianModels[0].CompanyCode, new List<DomModel.CompanyPayroll> { companyPayrollDomianModels[0] });

            mockCompanyPayrollRepository.Verify(m => m.Delete(It.IsAny<DbModel.CompanyPayroll>()), Times.AtLeast(1));
        }
    }
}
