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
using MasterModel = Evolution.DbRepository.Interfaces.Master;
using ServiceDomainData = Evolution.Company.Domain.Interfaces.Companies;
using System.Linq.Expressions;
using Evolution.UnitTests.Mocks.Data.Companies.Db;

namespace Evolution.UnitTests.UnitTestCases.Companies.Services
{
    [TestClass]
    public class CompanyExpectedMarginServiceUnitTestCase
    {
        ServiceDomainData.ICompanyExpectedMarginService companyExpectedMarginService = null;
        Mock<DomainData.ICompanyExpectedMarginRepository> mockCompanyExpectedMarginRepository = null;
        IList<DomModel.CompanyExpectedMargin> companyExpectedMarginDomianModels = null;
        Mock<IAppLogger<Company.Core.Services.CompanyExpectedMarginService>> mockLogger = null;
        Mock<MasterModel.IDataRepository> mockdataRepository = null;
        Mock<DomainData.ICompanyRepository> mockcompanyRepository = null;
        IQueryable<DbModel.Company> mockComapanyDbData = null;
        IQueryable<DbModel.Data> mockExpectedMarginDbData = null;
        IQueryable<DbModel.CompanyExpectedMargin> mockCompanyExpectedMarginDbData = null;

        [TestInitialize]
        public void InitializeCompanyExpectedMarginService()
        {
            mockCompanyExpectedMarginRepository = new Mock<DomainData.ICompanyExpectedMarginRepository>();
            companyExpectedMarginDomianModels = MockCompanyDomainModel.GetCompanyExpectedMarginMockedDomainModelData();
            mockLogger = new Mock<IAppLogger<Company.Core.Services.CompanyExpectedMarginService>>();
            mockdataRepository = new Mock<MasterModel.IDataRepository>();
            mockcompanyRepository = new Mock<DomainData.ICompanyRepository>();
            mockComapanyDbData = MockCompany.GetCompanyMockData();
            mockExpectedMarginDbData= MockCompany.GetMarginTypeMockData();
            mockCompanyExpectedMarginDbData = MockCompany.GetCompanyExpectedMarginMockData();
        }

        //[TestMethod]
        //public void FetchCompanyExpectedMarginListWithoutCompanyCodeAndSearchValue()
        //{
        //    mockCompanyExpectedMarginRepository.Setup(x => x.Search(It.IsAny<DomModel.CompanyExpectedMargin>())).Returns(new List<DomModel.CompanyExpectedMargin> { companyExpectedMarginDomianModels[0], companyExpectedMarginDomianModels[1] });

        //    companyExpectedMarginService = new Company.Core.Services.CompanyExpectedMarginService(mockdataRepository.Object, mockcompanyRepository.Object,mockCompanyExpectedMarginRepository.Object, mockLogger.Object);

        //    var response = companyExpectedMarginService.GetCompanyExpectedMargin( new DomModel.CompanyExpectedMargin());

        //    Assert.AreEqual("11", response.Code);
        //    Assert.IsNull(response.Result);
        //    Assert.IsNotNull(response.Messages);
        //}

        [TestMethod]
        public void FetchCompanyExpectedMarginListWithoutSearchValue()
        {
            mockCompanyExpectedMarginRepository.Setup(x => x.Search(It.IsAny<DomModel.CompanyExpectedMargin>())).Returns(new List<DomModel.CompanyExpectedMargin> { companyExpectedMarginDomianModels[0], companyExpectedMarginDomianModels[1] });

            companyExpectedMarginService = new Company.Core.Services.CompanyExpectedMarginService(mockdataRepository.Object, mockcompanyRepository.Object, mockCompanyExpectedMarginRepository.Object, mockLogger.Object);

            var response = companyExpectedMarginService.GetCompanyExpectedMargin(new DomModel.CompanyExpectedMargin());

            Assert.AreEqual("1", response.Code);
            Assert.AreEqual(2, (response.Result as List<DomModel.CompanyExpectedMargin>).Count);
        }

        //[TestMethod]
        //public void FetchCompanyExpectedMarginListWithNullSearchModel()
        //{
        //    mockCompanyExpectedMarginRepository.Setup(x => x.Search(It.IsAny<DomModel.CompanyExpectedMargin>())).Returns(companyExpectedMarginDomianModels);

        //    companyExpectedMarginService = new Company.Core.Services.CompanyExpectedMarginService(mockdataRepository.Object, mockcompanyRepository.Object, mockCompanyExpectedMarginRepository.Object, mockLogger.Object);

        //    var response = companyExpectedMarginService.GetCompanyExpectedMargin( null);

        //    Assert.AreEqual("11", response.Code);
        //    Assert.IsNull(response.Result);
        //    Assert.IsNotNull(response.Messages);
        //}

        [TestMethod]
        public void FetchCompanyExpectedMarginListByCompanyCode()
        {
            mockCompanyExpectedMarginRepository.Setup(x => x.Search(It.Is<DomModel.CompanyExpectedMargin>(c => c.CompanyCode == "UK050"))).Returns(new List<DomModel.CompanyExpectedMargin> { companyExpectedMarginDomianModels[0], companyExpectedMarginDomianModels[1] });

            companyExpectedMarginService = new Company.Core.Services.CompanyExpectedMarginService(mockdataRepository.Object, mockcompanyRepository.Object, mockCompanyExpectedMarginRepository.Object, mockLogger.Object);

            var response = companyExpectedMarginService.GetCompanyExpectedMargin(new DomModel.CompanyExpectedMargin { CompanyCode = "UK050" });

            Assert.AreEqual("1", response.Code);
            Assert.AreEqual(2, (response.Result as List<DomModel.CompanyExpectedMargin>).Count);
        }

        [TestMethod]
        public void FetchCompanyExpectedMarginListByWildCardSearchWithExpectedMarginTypeStartWith()
        {
            mockCompanyExpectedMarginRepository.Setup(x => x.Search(It.Is<DomModel.CompanyExpectedMargin>(c => c.MarginType.StartsWith("TI")))).Returns(new List<DomModel.CompanyExpectedMargin> { companyExpectedMarginDomianModels[0] });

            companyExpectedMarginService = new Company.Core.Services.CompanyExpectedMarginService(mockdataRepository.Object, mockcompanyRepository.Object, mockCompanyExpectedMarginRepository.Object, mockLogger.Object);

            var response = companyExpectedMarginService.GetCompanyExpectedMargin(new DomModel.CompanyExpectedMargin { MarginType = "TI*" });

            Assert.AreEqual("1", response.Code);
            Assert.AreEqual(1, (response.Result as List<DomModel.CompanyExpectedMargin>).Count);
        }

        [TestMethod]
        public void FetchCompanyExpectedMarginListByWildCardSearchWithExpectedMarginTypeEndWith()
        {
            mockCompanyExpectedMarginRepository.Setup(x => x.Search(It.Is<DomModel.CompanyExpectedMargin>(c => c.MarginType.EndsWith("Services)")))).Returns(new List<DomModel.CompanyExpectedMargin> { companyExpectedMarginDomianModels[0], companyExpectedMarginDomianModels[1] });

            companyExpectedMarginService = new Company.Core.Services.CompanyExpectedMarginService(mockdataRepository.Object, mockcompanyRepository.Object, mockCompanyExpectedMarginRepository.Object, mockLogger.Object);

            var response = companyExpectedMarginService.GetCompanyExpectedMargin(new DomModel.CompanyExpectedMargin { MarginType = "*Services)" });

            Assert.AreEqual("1", response.Code);
            Assert.AreEqual(2, (response.Result as List<DomModel.CompanyExpectedMargin>).Count);
        }

        [TestMethod]
        public void FetchCompanyExpectedMarginListByWildCardSearchWithExpectedMarginTypeNameContains()
        {
            mockCompanyExpectedMarginRepository.Setup(x => x.Search(It.Is<DomModel.CompanyExpectedMargin>(c => c.MarginType.Contains("Services")))).Returns(new List<DomModel.CompanyExpectedMargin> { companyExpectedMarginDomianModels[0], companyExpectedMarginDomianModels[1] });

            companyExpectedMarginService = new Company.Core.Services.CompanyExpectedMarginService(mockdataRepository.Object, mockcompanyRepository.Object, mockCompanyExpectedMarginRepository.Object, mockLogger.Object);

            var response = companyExpectedMarginService.GetCompanyExpectedMargin(new DomModel.CompanyExpectedMargin { MarginType = "*Services*" });

            Assert.AreEqual("1", response.Code);
            Assert.AreEqual(2, (response.Result as List<DomModel.CompanyExpectedMargin>).Count);
        }

        [TestMethod]
        public void ThrowsExceptionWhileFetchingCompanyExpectedMarginList()
        {
            mockCompanyExpectedMarginRepository.Setup(x => x.Search(It.IsAny<DomModel.CompanyExpectedMargin>())).Throws(new Exception("Exception occured while performing some operation."));
            companyExpectedMarginService = new Company.Core.Services.CompanyExpectedMarginService(mockdataRepository.Object, mockcompanyRepository.Object, mockCompanyExpectedMarginRepository.Object, mockLogger.Object);

            var response = companyExpectedMarginService.GetCompanyExpectedMargin(new DomModel.CompanyExpectedMargin());

            Assert.AreEqual("11", response.Code);
            Assert.IsNotNull(response.Messages);
            Assert.AreEqual("31", response.Messages.FirstOrDefault().Code);
        }

        [TestMethod]
        public void SaveCompanyExpectedMargins()
        {
            mockcompanyRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Company, bool>>>())).Returns(mockComapanyDbData);
            mockdataRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Data, bool>>>())).Returns((Expression<Func<DbModel.Data, bool>> predicate) => mockExpectedMarginDbData.Where(predicate));
            mockCompanyExpectedMarginRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.CompanyExpectedMargin, bool>>>())).Returns((Expression<Func<DbModel.CompanyExpectedMargin, bool>> predicate) => mockCompanyExpectedMarginDbData.Where(predicate));


            companyExpectedMarginService = new Company.Core.Services.CompanyExpectedMarginService(mockdataRepository.Object, mockcompanyRepository.Object, mockCompanyExpectedMarginRepository.Object, mockLogger.Object);

            companyExpectedMarginDomianModels.ToList().ForEach(x => x.RecordStatus = "N");

            companyExpectedMarginDomianModels[0].MarginType = "NDT (Non Destructive Testing)";

            var response = companyExpectedMarginService.SaveCompanyExpectedMargin(companyExpectedMarginDomianModels[0].CompanyCode, new List<DomModel.CompanyExpectedMargin> { companyExpectedMarginDomianModels[0] });

            mockCompanyExpectedMarginRepository.Verify(m => m.Add(It.IsAny<IList<DbModel.CompanyExpectedMargin>>()), Times.AtLeastOnce);
        }

        [TestMethod]
        public void ModifyCompanyExpectedMargin()
        {
            mockcompanyRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Company, bool>>>())).Returns(mockComapanyDbData);
            mockdataRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Data, bool>>>())).Returns((Expression<Func<DbModel.Data, bool>> predicate) => mockExpectedMarginDbData.Where(predicate));
            mockCompanyExpectedMarginRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.CompanyExpectedMargin, bool>>>())).Returns((Expression<Func<DbModel.CompanyExpectedMargin, bool>> predicate) => mockCompanyExpectedMarginDbData.Where(predicate));


            companyExpectedMarginService = new Company.Core.Services.CompanyExpectedMarginService(mockdataRepository.Object, mockcompanyRepository.Object, mockCompanyExpectedMarginRepository.Object, mockLogger.Object);
            companyExpectedMarginDomianModels.ToList().ForEach(x => x.RecordStatus = "M");
            var response = companyExpectedMarginService.ModifyCompanyExpectedMargin(companyExpectedMarginDomianModels[0].CompanyCode, new List<DomModel.CompanyExpectedMargin> { companyExpectedMarginDomianModels[0] });

            mockCompanyExpectedMarginRepository.Verify(m => m.Update(It.IsAny<DbModel.CompanyExpectedMargin>()), Times.AtLeast(1));
        }

        [TestMethod]
        public void DeleteCompanyExpectedMargin()
        {
            mockcompanyRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Company, bool>>>())).Returns(mockComapanyDbData);
            mockdataRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Data, bool>>>())).Returns((Expression<Func<DbModel.Data, bool>> predicate) => mockExpectedMarginDbData.Where(predicate));
            mockCompanyExpectedMarginRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.CompanyExpectedMargin, bool>>>())).Returns((Expression<Func<DbModel.CompanyExpectedMargin, bool>> predicate) => mockCompanyExpectedMarginDbData.Where(predicate));
             
            companyExpectedMarginService = new Company.Core.Services.CompanyExpectedMarginService(mockdataRepository.Object, mockcompanyRepository.Object, mockCompanyExpectedMarginRepository.Object, mockLogger.Object);
            companyExpectedMarginDomianModels.ToList().ForEach(x => x.RecordStatus = "D");
            var response = companyExpectedMarginService.DeleteCompanyExpectedMargin(companyExpectedMarginDomianModels[0].CompanyCode, new List<DomModel.CompanyExpectedMargin> { companyExpectedMarginDomianModels[0] });

            mockCompanyExpectedMarginRepository.Verify(m => m.Delete(It.IsAny<DbModel.CompanyExpectedMargin>()), Times.AtLeast(1));
        }
    }
}
