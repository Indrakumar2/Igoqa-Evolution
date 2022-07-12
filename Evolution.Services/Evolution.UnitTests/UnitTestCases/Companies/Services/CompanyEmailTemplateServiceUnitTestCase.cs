using AutoMapper;
using Evolution.Logging.Interfaces;
using Evolution.UnitTests.Mocks.Data.Companies;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using DomainData = Evolution.Company.Domain.Interfaces.Data;
using DomModel = Evolution.Company.Domain.Models.Companies;
using ServiceDomainData = Evolution.Company.Domain.Interfaces.Companies;

namespace Evolution.UnitTests.UnitTestCases.Companies.Services
{
    [TestClass]
    public class CompanyEmailTemplateServiceUnitTestCase
    {
        ServiceDomainData.ICompanyEmailTemplateService companyEmailTemplateService = null;
        Mock<DomainData.ICompanyEmailTemplateRepository> mockCompanyEmailTemplateRepository = null;
        IList<DomModel.CompanyEmailTemplate> companyEmailTemplateDomianModels = null;
        Mock<IAppLogger<Company.Core.Services.CompanyEmailTemplateService>> mockLogger = null;
        Mock<DomainData.ICompanyRepository> mockCompanyRepository = null;
        Mock<IMapper> mockMapper = null;

        [TestInitialize]
        public void InitializeCompanyEmailTemplateService()
        {
            mockCompanyEmailTemplateRepository = new Mock<DomainData.ICompanyEmailTemplateRepository>();
            companyEmailTemplateDomianModels = MockCompanyDomainModel.GetCompanyEmailTemplateMockedDomainModelData();
            mockLogger = new Mock<IAppLogger<Company.Core.Services.CompanyEmailTemplateService>>();
            mockCompanyRepository = new Mock<DomainData.ICompanyRepository>();
            mockMapper = new Mock<IMapper>();
        }

        //[TestMethod]
        //public void FetchCompanyEmailTemplateListWithoutSearchValue()
        //{
        //    mockCompanyEmailTemplateRepository.Setup(x => x.Search("UK050")).Returns(companyEmailTemplateDomianModels[0]);

        //    companyEmailTemplateService = new Company.Core.Services.CompanyEmailTemplateService(mockMapper.Object,mockCompanyEmailTemplateRepository.Object, mockCompanyRepository.Object, mockLogger.Object);

        //    var response = companyEmailTemplateService.GetCompanyEmailTemplate("");

        //    Assert.AreEqual("11", response.Code);
        //    Assert.IsNull(response.Result);
        //    Assert.IsNotNull(response.Messages);
        //}

        //[TestMethod]
        //public void FetchCompanyEmailTemplateListWithNullSearchModel()
        //{
        //    mockCompanyEmailTemplateRepository.Setup(x => x.Search("UK050")).Returns(companyEmailTemplateDomianModels[0]);

        //    companyEmailTemplateService = new Company.Core.Services.CompanyEmailTemplateService(mockMapper.Object,mockCompanyEmailTemplateRepository.Object, mockCompanyRepository.Object, mockLogger.Object);

        //    var response = companyEmailTemplateService.GetCompanyEmailTemplate(null);

        //    Assert.AreEqual("11", response.Code);
        //    Assert.IsNull(response.Result);
        //    Assert.IsNotNull(response.Messages);
        //}

        [TestMethod]
        public void FetchCompanyEmailTemplateListByCompanyCode()
        {
            mockCompanyEmailTemplateRepository.Setup(x => x.Search("UK050")).Returns(companyEmailTemplateDomianModels[0]);

            companyEmailTemplateService = new Company.Core.Services.CompanyEmailTemplateService(mockMapper.Object,mockCompanyEmailTemplateRepository.Object, mockCompanyRepository.Object, mockLogger.Object);

            var response = companyEmailTemplateService.GetCompanyEmailTemplate("UK050" );

            Assert.AreEqual("1", response.Code);
            Assert.IsNotNull(response.Result);
            Assert.AreEqual("UK050", (response.Result as DomModel.CompanyEmailTemplate).CompanyCode);
        }

        //[TestMethod]
        //public void FetchCompanyEmailTemplateListByWildCardSearchWithEmailTemplateNameStartWith()
        //{
        //    mockCompanyEmailTemplateRepository.Setup(x => x.Search("UK050");

        //    companyEmailTemplateService = new Company.Core.Services.CompanyEmailTemplateService(mockMapper.Object,mockCompanyEmailTemplateRepository.Object, mockLogger.Object);

        //    var response = companyEmailTemplateService.GetCompanyEmailTemplate(new DomModel.CompanyEmailTemplate { CustomerReportingNotificationEmailText = "Customer :*" });

        //    Assert.AreEqual("1", response.Code);
        //    Assert.IsNotNull(response.Result); 
        //}

        //[TestMethod]
        //public void FetchCompanyEmailTemplateListByWildCardSearchWithEmailTemplateNameEndWith()
        //{
        //    mockCompanyEmailTemplateRepository.Setup(x => x.Search(It.Is<DomModel.CompanyEmailTemplate>(c => c.CustomerReportingNotificationEmailText.EndsWith("@CoordinatorName")))).Returns(companyEmailTemplateDomianModels[1]);

        //    companyEmailTemplateService = new Company.Core.Services.CompanyEmailTemplateService(mockCompanyEmailTemplateRepository.Object, mockLogger.Object);

        //    var response = companyEmailTemplateService.GetCompanyEmailTemplate(new DomModel.CompanyEmailTemplate { CustomerReportingNotificationEmailText = "*@CoordinatorName" });

        //    Assert.AreEqual("1", response.Code);
        //    Assert.IsNotNull(response.Result);
        //}

        //[TestMethod]
        //public void FetchCompanyEmailTemplateListByWildCardSearchWithEmailTemplateNameContains()
        //{
        //    mockCompanyEmailTemplateRepository.Setup(x => x.Search(It.Is<DomModel.CompanyEmailTemplate>(c => c.CustomerReportingNotificationEmailText.Contains("@CustomerName@")))).Returns(companyEmailTemplateDomianModels[0] );

        //    companyEmailTemplateService = new Company.Core.Services.CompanyEmailTemplateService(mockCompanyEmailTemplateRepository.Object, mockLogger.Object);

        //    var response = companyEmailTemplateService.GetCompanyEmailTemplate(new DomModel.CompanyEmailTemplate { CustomerReportingNotificationEmailText = "*@CustomerName@*" });

        //    Assert.AreEqual("1", response.Code);
        //    Assert.IsNotNull(response.Result);
        //}

        [TestMethod]
        public void ThrowsExceptionWhileFetchingCompanyEmailTemplateList()
        {
            mockCompanyEmailTemplateRepository.Setup(x => x.Search("UK050")).Throws(new Exception("Exception occured while performing some operation."));
            companyEmailTemplateService = new Company.Core.Services.CompanyEmailTemplateService(mockMapper.Object,mockCompanyEmailTemplateRepository.Object, mockCompanyRepository.Object, mockLogger.Object);

            var response = companyEmailTemplateService.GetCompanyEmailTemplate("UK050");

            Assert.AreEqual("11", response.Code);
            Assert.IsNotNull(response.Messages);
            Assert.AreEqual("31", response.Messages.FirstOrDefault().Code);
        }
         
    }
}
