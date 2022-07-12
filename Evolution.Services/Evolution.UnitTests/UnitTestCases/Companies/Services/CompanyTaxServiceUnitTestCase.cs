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
using Evolution.Master.Domain.Interfaces.Data;
using Evolution.UnitTests.Mocks.Data.Companies.Db;
using System.Linq.Expressions;

namespace Evolution.UnitTests.UnitTestCases.Companies.Services
{
    [TestClass]
    public class CompanyTaxServiceUnitTestCase
    { 
        ServiceDomainData.ICompanyTaxService companyTaxService = null;
        Mock<DomainData.ICompanyTaxRepository> mockCompanyTaxRepository = null;
        Mock<ITaxTypeRepository> mockTaxTypeRepository = null;
        Mock<DomainData.ICompanyRepository> mockCompanyRepository = null;
        IList<DomModel.CompanyTax> companyTaxDomianModels = null;
        Mock<IAppLogger<Company.Core.Services.CompanyTaxService>> mockLogger = null;
        IQueryable<DbModel.Company> mockComapanyDbData = null;
        IQueryable<DbModel.CompanyTax> mockCompanyTaxDbData = null;
        IQueryable<DbModel.Tax> mockTaxTypeDbData = null;

        [TestInitialize]
        public void InitializeCompanyTaxService()
        {
            mockCompanyTaxRepository = new Mock<DomainData.ICompanyTaxRepository>();
            companyTaxDomianModels = MockCompanyDomainModel.GetCompanyTaxMockedDomainModelData();
            mockLogger = new Mock<IAppLogger<Company.Core.Services.CompanyTaxService>>();
            mockTaxTypeRepository = new Mock<ITaxTypeRepository>();
            mockCompanyRepository = new Mock<DomainData.ICompanyRepository>();
            mockComapanyDbData= MockCompany.GetCompanyMockData();
            mockCompanyTaxDbData = MockCompany.GetCompanyTaxMockData();
            mockTaxTypeDbData = MockCompany.GetTaxTypeMockData();
        }

        [TestMethod]
        public void FetchCompanyTaxListWithoutSearchValue()
        {
            mockCompanyTaxRepository.Setup(x => x.Search(It.IsAny<DomModel.CompanyTax>())).Returns(companyTaxDomianModels);

            companyTaxService = new Company.Core.Services.CompanyTaxService(mockTaxTypeRepository.Object,mockCompanyTaxRepository.Object, mockLogger.Object,mockCompanyRepository.Object);

            var response = companyTaxService.GetCompanyTax(new DomModel.CompanyTax());

            Assert.AreEqual("1", response.Code);
            Assert.AreEqual(3, (response.Result as List<DomModel.CompanyTax>).Count);
        }

        [TestMethod]
        public void FetchCompanyTaxListWithNullSearchModel()
        {
            mockCompanyTaxRepository.Setup(x => x.Search(It.IsAny<DomModel.CompanyTax>())).Returns(companyTaxDomianModels);

            companyTaxService = new Company.Core.Services.CompanyTaxService(mockTaxTypeRepository.Object,mockCompanyTaxRepository.Object, mockLogger.Object,mockCompanyRepository.Object);

            var response = companyTaxService.GetCompanyTax(null);

            Assert.AreEqual("1", response.Code);
            Assert.AreEqual(3, (response.Result as List<DomModel.CompanyTax>).Count);
        }

        [TestMethod]
        public void FetchCompanyTaxListByCompanyCode()
        {
            mockCompanyTaxRepository.Setup(x => x.Search(It.Is<DomModel.CompanyTax>(c => c.CompanyCode == "UK050"))).Returns(companyTaxDomianModels);

            companyTaxService = new Company.Core.Services.CompanyTaxService(mockTaxTypeRepository.Object,mockCompanyTaxRepository.Object, mockLogger.Object,mockCompanyRepository.Object);

            var response = companyTaxService.GetCompanyTax(new DomModel.CompanyTax { CompanyCode = "UK050" });

            Assert.AreEqual("1", response.Code);
            Assert.AreEqual(3, (response.Result as List<DomModel.CompanyTax>).Count);
        }

        [TestMethod]
        public void FetchCompanyTaxListByWildCardSearchWithTaxTypeStartWith()
        {
            mockCompanyTaxRepository.Setup(x => x.Search(It.Is<DomModel.CompanyTax>(c => c.Tax.StartsWith("VAT")))).Returns(new List<DomModel.CompanyTax> { companyTaxDomianModels[0], companyTaxDomianModels[1] });

            companyTaxService = new Company.Core.Services.CompanyTaxService(mockTaxTypeRepository.Object,mockCompanyTaxRepository.Object, mockLogger.Object, mockCompanyRepository.Object);

            var response = companyTaxService.GetCompanyTax(new DomModel.CompanyTax { Tax = "VAT *" });

            Assert.AreEqual("1", response.Code);
            Assert.AreEqual(2, (response.Result as List<DomModel.CompanyTax>).Count);
        }

        [TestMethod]
        public void FetchCompanyTaxListByWildCardSearchWithTaxTypeEndWith()
        {
            mockCompanyTaxRepository.Setup(x => x.Search(It.Is<DomModel.CompanyTax>(c => c.Tax.EndsWith("Exempt")))).Returns(new List<DomModel.CompanyTax> { companyTaxDomianModels[1] });

            companyTaxService = new Company.Core.Services.CompanyTaxService(mockTaxTypeRepository.Object,mockCompanyTaxRepository.Object, mockLogger.Object, mockCompanyRepository.Object);

            var response = companyTaxService.GetCompanyTax(new DomModel.CompanyTax { Tax = "*Exempt" });

            Assert.AreEqual("1", response.Code);
            Assert.AreEqual(1, (response.Result as List<DomModel.CompanyTax>).Count);
        }

        [TestMethod]
        public void FetchCompanyTaxListByWildCardSearchWithTaxTypeContains()
        {
            mockCompanyTaxRepository.Setup(x => x.Search(It.Is<DomModel.CompanyTax>(c => c.Tax.Contains("GST")))).Returns(new List<DomModel.CompanyTax> { companyTaxDomianModels[2] });

            companyTaxService = new Company.Core.Services.CompanyTaxService(mockTaxTypeRepository.Object,mockCompanyTaxRepository.Object, mockLogger.Object, mockCompanyRepository.Object);

            var response = companyTaxService.GetCompanyTax(new DomModel.CompanyTax { Tax = "*GST*" });

            Assert.AreEqual("1", response.Code);
            Assert.AreEqual(1, (response.Result as List<DomModel.CompanyTax>).Count);
        }

        [TestMethod]
        public void ThrowsExceptionWhileFetchingCompanyTaxList()
        {
            mockCompanyTaxRepository.Setup(x => x.Search(It.IsAny<DomModel.CompanyTax>())).Throws(new Exception("Exception occured while performing some operation."));
            companyTaxService = new Company.Core.Services.CompanyTaxService(mockTaxTypeRepository.Object,mockCompanyTaxRepository.Object, mockLogger.Object, mockCompanyRepository.Object);

            var response = companyTaxService.GetCompanyTax(new DomModel.CompanyTax());

            Assert.AreEqual("11", response.Code);
            Assert.IsNotNull(response.Messages);
            Assert.AreEqual("31", response.Messages.FirstOrDefault().Code);
        }

        [TestMethod]
        public void SaveCompanyTax()
        {
            mockCompanyRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Company, bool>>>())).Returns(mockComapanyDbData);
            mockCompanyTaxRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.CompanyTax, bool>>>())).Returns((Expression<Func<DbModel.CompanyTax, bool>> predicate) => mockCompanyTaxDbData.Where(predicate));
            mockTaxTypeRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Tax, bool>>>())).Returns((Expression<Func<DbModel.Tax, bool>> predicate) => mockTaxTypeDbData.Where(predicate));

            companyTaxService = new Company.Core.Services.CompanyTaxService(mockTaxTypeRepository.Object,mockCompanyTaxRepository.Object, mockLogger.Object, mockCompanyRepository.Object);

            companyTaxDomianModels.ToList().ForEach(x => x.RecordStatus = "N");
            companyTaxDomianModels[0].Tax= "VAT - 12.5%";
            var response = companyTaxService.SaveCompanyTax(companyTaxDomianModels[0].CompanyCode, new List<DomModel.CompanyTax> { companyTaxDomianModels[0] });

            mockCompanyTaxRepository.Verify(m => m.Add(It.IsAny<IList<DbModel.CompanyTax>>()), Times.AtLeastOnce);
        }

        [TestMethod]
        public void ModifyCompanyTax()
        {
            mockCompanyRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Company, bool>>>())).Returns(mockComapanyDbData);
            mockCompanyTaxRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.CompanyTax, bool>>>())).Returns((Expression<Func<DbModel.CompanyTax, bool>> predicate) => mockCompanyTaxDbData.Where(predicate));
            mockTaxTypeRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Tax, bool>>>())).Returns((Expression<Func<DbModel.Tax, bool>> predicate) => mockTaxTypeDbData.Where(predicate));


            companyTaxService = new Company.Core.Services.CompanyTaxService(mockTaxTypeRepository.Object,mockCompanyTaxRepository.Object, mockLogger.Object, mockCompanyRepository.Object);
            companyTaxDomianModels.ToList().ForEach(x => x.RecordStatus = "M");
            var response = companyTaxService.ModifyCompanyTax(companyTaxDomianModels[0].CompanyCode, new List<DomModel.CompanyTax> { companyTaxDomianModels[0] });

            mockCompanyTaxRepository.Verify(m => m.Update(It.IsAny<DbModel.CompanyTax>()), Times.AtLeast(1));
        }

        [TestMethod]
        public void DeleteCompanyTax()
        {
            mockCompanyRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Company, bool>>>())).Returns(mockComapanyDbData);
            mockCompanyTaxRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.CompanyTax, bool>>>())).Returns((Expression<Func<DbModel.CompanyTax, bool>> predicate) => mockCompanyTaxDbData.Where(predicate));
            mockTaxTypeRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Tax, bool>>>())).Returns((Expression<Func<DbModel.Tax, bool>> predicate) => mockTaxTypeDbData.Where(predicate));

            companyTaxService = new Company.Core.Services.CompanyTaxService(mockTaxTypeRepository.Object,mockCompanyTaxRepository.Object, mockLogger.Object, mockCompanyRepository.Object);
            companyTaxDomianModels.ToList().ForEach(x => x.RecordStatus = "D");
            var response = companyTaxService.DeleteCompanyTax(companyTaxDomianModels[0].CompanyCode, new List<DomModel.CompanyTax> { companyTaxDomianModels[0] });

            mockCompanyTaxRepository.Verify(m => m.Delete(It.IsAny<DbModel.CompanyTax>()), Times.AtLeast(1));
        }
    }
}
