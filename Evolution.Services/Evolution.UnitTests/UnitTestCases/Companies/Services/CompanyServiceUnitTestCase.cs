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
using AutoMapper;
using System.Linq.Expressions;
using Evolution.UnitTests.Mocks.Data.Companies.Db;

namespace Evolution.UnitTests.UnitTestCases.Companies.Services
{
    [TestClass]
    public class CompanyServiceUnitTestCase
    {
        ServiceDomainData.ICompanyService companyService = null;
        Mock<DomainData.ICompanyRepository> mockCompanyRepository = null;
        IList<DomModel.Company> companyDomianModels = null;
        Mock<IAppLogger<Company.Core.Services.CompanyService>> mockLogger = null;
        Mock<IMapper> mockMapper = null;
        IQueryable<DbModel.Company> mockComapanyDbData = null;

        [TestInitialize]
        public void InitializeCompanyService()
        {
            mockCompanyRepository = new Mock<DomainData.ICompanyRepository>();
            companyDomianModels = MockCompanyDomainModel.GetCompanyMockedDomainModelData();
            mockLogger = new Mock<IAppLogger<Company.Core.Services.CompanyService>>();
            mockMapper = new Mock<IMapper>();
            mockComapanyDbData = MockCompany.GetCompanyMockData();

        }

        [TestMethod]
        public void FetchCompanyListWithoutSearchValue()
        {
            mockCompanyRepository.Setup(x => x.Search(It.IsAny<DomModel.Company>())).Returns(companyDomianModels);

            companyService = new Company.Core.Services.CompanyService(mockMapper.Object,mockCompanyRepository.Object, mockLogger.Object);

            var response = companyService.GetCompany(new DomModel.Company());

            Assert.AreEqual("1", response.Code);
            Assert.AreEqual(3, (response.Result as List<DomModel.Company>).Count);
        }

        [TestMethod]
        public void FetchCompanyListWithNullSearchModel()
        {
            mockCompanyRepository.Setup(x => x.Search(It.IsAny<DomModel.Company>())).Returns(companyDomianModels);

            companyService = new Company.Core.Services.CompanyService(mockMapper.Object,mockCompanyRepository.Object, mockLogger.Object);

            var response = companyService.GetCompany(null);

            Assert.AreEqual("1", response.Code);
            Assert.AreEqual(3, (response.Result as List<DomModel.Company>).Count);
        }

        [TestMethod]
        public void FetchCompanyListByCompanyCode()
        {
            mockCompanyRepository.Setup(x => x.Search(It.Is<DomModel.Company>(c => c.CompanyCode == "DZ"))).Returns(new List<DomModel.Company> { companyDomianModels[0] });

            companyService = new Company.Core.Services.CompanyService(mockMapper.Object,mockCompanyRepository.Object, mockLogger.Object);

            var response = companyService.GetCompany(new DomModel.Company { CompanyCode = "DZ" });

            Assert.AreEqual("1", response.Code);
            Assert.AreEqual(1, (response.Result as List<DomModel.Company>).Count);
        }

        [TestMethod]
        public void FetchCompanyListByWildCardSearchWithNameStartWith()
        {
            mockCompanyRepository.Setup(x => x.Search(It.Is<DomModel.Company>(c => c.CompanyName.StartsWith("A")))).Returns(companyDomianModels);

            companyService = new Company.Core.Services.CompanyService(mockMapper.Object,mockCompanyRepository.Object, mockLogger.Object);

            var response = companyService.GetCompany(new DomModel.Company { CompanyName = "A*" });

            Assert.AreEqual("1", response.Code);
            Assert.AreEqual(3, (response.Result as List<DomModel.Company>).Count);
        }

        [TestMethod]
        public void FetchCompanyListByWildCardSearchWithNameEndWith()
        {
            mockCompanyRepository.Setup(x => x.Search(It.Is<DomModel.Company>(c => c.CompanyName.EndsWith("MI")))).Returns(new List<DomModel.Company> { companyDomianModels[0], companyDomianModels[1] });

            companyService = new Company.Core.Services.CompanyService(mockMapper.Object,mockCompanyRepository.Object, mockLogger.Object);

            var response = companyService.GetCompany(new DomModel.Company { CompanyName = "*MI" });

            Assert.AreEqual("1", response.Code);
            Assert.AreEqual(2, (response.Result as List<DomModel.Company>).Count);
        }

        [TestMethod]
        public void FetchCompanyListByWildCardSearchWithNameContains()
        {
            mockCompanyRepository.Setup(x => x.Search(It.Is<DomModel.Company>(c => c.CompanyName.Contains("ge")))).Returns(new List<DomModel.Company> { companyDomianModels[0], companyDomianModels[1] });

            companyService = new Company.Core.Services.CompanyService(mockMapper.Object,mockCompanyRepository.Object, mockLogger.Object);

            var response = companyService.GetCompany(new DomModel.Company { CompanyName = "*ge*" });

            Assert.AreEqual("1", response.Code);
            Assert.AreEqual(2, (response.Result as List<DomModel.Company>).Count);
        }

        [TestMethod]
        public void ThrowsExceptionWhileFetchingCompanyList()
        {
            mockCompanyRepository.Setup(x => x.Search(It.IsAny<DomModel.Company>())).Throws(new Exception("Exception occured while performing some operation."));
            companyService = new Company.Core.Services.CompanyService(mockMapper.Object,mockCompanyRepository.Object, mockLogger.Object);

            var response = companyService.GetCompany(new DomModel.Company());

            Assert.AreEqual("11", response.Code);
            Assert.IsNotNull(response.Messages);
            Assert.AreEqual("31", response.Messages.FirstOrDefault().Code);
        }
         
        [TestMethod]
        public void SaveCompanies()
        { 
            companyService = new Company.Core.Services.CompanyService(mockMapper.Object,mockCompanyRepository.Object, mockLogger.Object);
            companyDomianModels.ToList().ForEach(x => x.RecordStatus = "N");

            var response = companyService.SaveCompany(companyDomianModels);
             
            mockCompanyRepository.Verify(m => m.Add(It.IsAny<IList<DbModel.Company>>()), Times.AtLeastOnce);  
        }

        [TestMethod]
        public void ModifyCompanies()
        {
            mockCompanyRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Company, bool>>>())).Returns((Expression<Func<DbModel.Company, bool>> predicate) => mockComapanyDbData.Where(predicate)); 
            companyService = new Company.Core.Services.CompanyService(mockMapper.Object,mockCompanyRepository.Object, mockLogger.Object);
            companyDomianModels.ToList().ForEach(x => x.RecordStatus = "M");
           
            var response = companyService.ModifyCompany(new List<DomModel.Company> { companyDomianModels[0] });

            mockCompanyRepository.Verify(m => m.Update(It.IsAny<DbModel.Company>()), Times.AtLeast(1));
        }
    }
}
