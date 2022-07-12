using Evolution.Api.Controllers.Company;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using ServiceDomainData = Evolution.Company.Domain.Interfaces.Companies;
using DomModel = Evolution.Company.Domain.Models.Companies;
using Evolution.UnitTests.Mocks.Data.Companies;
using Evolution.Common.Models.Responses;
using Evolution.Common.Extensions;
using Evolution.Common.Enums;
using System.Linq;

namespace Evolution.UnitTests.UnitTestCases.Companies.Controllers
{
    [TestClass]public class CompanyEmailTemplateControllerUnitTestCase
    {
        CompanyEmailTemplateController companyEmailTemplateController = null;
        Mock<ServiceDomainData.ICompanyEmailTemplateService> mockEmailTemplateService = null;
        IList<DomModel.CompanyEmailTemplate> companyEmailTemplateDomianModels = null;
        [TestInitialize]
        public void InitializeCompanyEmailTemplateController()
        {
            mockEmailTemplateService = new Mock<ServiceDomainData.ICompanyEmailTemplateService>();
            companyEmailTemplateDomianModels = MockCompanyDomainModel.GetCompanyEmailTemplateMockedDomainModelData();
        }
        //[TestMethod]
        //public void FetchCompanyEmailTemplateListWithoutSearchValue()
        //{
        //    var result = new List<DomModel.CompanyEmailTemplate> { companyEmailTemplateDomianModels[0] };
        //    mockEmailTemplateService.Setup(x => x.GetCompanyEmailTemplate("")).Returns(new Response().ToPopulate(ResponseType.Success, null, null, null, result, null, result?.Count));

        //    companyEmailTemplateController = new CompanyEmailTemplateController(mockEmailTemplateService.Object);

        //    var response = companyEmailTemplateController.Get("UK050");

        //    Assert.AreEqual("1", response.Code);
        //    Assert.AreEqual(1, (response.Result as List<DomModel.CompanyEmailTemplate>).Count);
        //}
        [TestMethod]
        public void FetchCompanyEmailTemplateListWithNullSearchModel()
        {
            var result = new List<DomModel.CompanyEmailTemplate> { companyEmailTemplateDomianModels[0] };
            mockEmailTemplateService.Setup(x => x.GetCompanyEmailTemplate("UK050")).Returns(new Response().ToPopulate(ResponseType.Success, null, null, null, result, null, result?.Count));

            companyEmailTemplateController = new CompanyEmailTemplateController(mockEmailTemplateService.Object);

            var response = companyEmailTemplateController.Get("UK050");

            Assert.AreEqual("1", response.Code);
            Assert.AreEqual(1, (response.Result as List<DomModel.CompanyEmailTemplate>).Count);
        }
        //[TestMethod]
        //public void FetchCompanyEmailTemplateListWithNullParameters()
        //{
        //    var result = new List<DomModel.CompanyEmailTemplate> { companyEmailTemplateDomianModels[0] };
        //    mockEmailTemplateService.Setup(x => x.GetCompanyEmailTemplate("UK050")).Returns(new Response().ToPopulate(ResponseType.Success, null, null, null, result, null, result?.Count));

        //    companyEmailTemplateController = new CompanyEmailTemplateController(mockEmailTemplateService.Object);

        //    var response = companyEmailTemplateController.Get(null);

        //    Assert.AreEqual("11", response.Code);
        //    Assert.IsNull(response.Result);
        //}
        [TestMethod]
        public void FetchCompanyEmailTemplateListByCompanyCode()
        {
            var result = new List<DomModel.CompanyEmailTemplate> { companyEmailTemplateDomianModels[0] };
            mockEmailTemplateService.Setup(x => x.GetCompanyEmailTemplate("UK050")).Returns(new Response().ToPopulate(ResponseType.Success, null, null, null, result, null, result?.Count));

            companyEmailTemplateController = new CompanyEmailTemplateController(mockEmailTemplateService.Object);

            var response = companyEmailTemplateController.Get("UK050");

            Assert.AreEqual("1", response.Code);
            Assert.AreEqual(1, (response.Result as List<DomModel.CompanyEmailTemplate>).Count);
        }
        [TestMethod]
        public void FetchCompanyEmailTemplateListByWildCardSearchWithNameStartWith()
        {
            var result = new List<DomModel.CompanyEmailTemplate> { companyEmailTemplateDomianModels[0] };
            mockEmailTemplateService.Setup(x => x.GetCompanyEmailTemplate("UK050")).Returns(new Response().ToPopulate(ResponseType.Success, null, null, null, result, null, result?.Count));

            companyEmailTemplateController = new CompanyEmailTemplateController(mockEmailTemplateService.Object);

            var response = companyEmailTemplateController.Get("UK050");

            Assert.AreEqual("1", response.Code);
            Assert.AreEqual(1, (response.Result as List<DomModel.CompanyEmailTemplate>).Count);
        }
        //[TestMethod]
        //public void FetchCompanyEmailTemplateByWildCardSearchWithNameEndWith()
        //{
        //    var result = new List<DomModel.CompanyEmailTemplate> { companyEmailTemplateDomianModels[0]};
        //    mockEmailTemplateService.Setup(x => x.GetCompanyEmailTemplate(It.Is<DomModel.CompanyEmailTemplate>(c => c.CustomerReportingNotificationEmailText.EndsWith("@Company@")))).Returns(new Response().ToPopulate(ResponseType.Success, null, null, null, result, null, result?.Count));

        //    companyEmailTemplateController = new CompanyEmailTemplateController(mockEmailTemplateService.Object);

        //    var response = companyEmailTemplateController.Get("UK050", new DomModel.CompanyEmailTemplate { CustomerReportingNotificationEmailText = "*@Company@" });

        //    Assert.AreEqual("1", response.Code);
        //    Assert.AreEqual(1, (response.Result as List<DomModel.CompanyEmailTemplate>).Count);
        //}
        //[TestMethod]
        //public void FetchCompanyEmailTemplateByWildCardSearchWithNameContains()
        //{
        //    var result = new List<DomModel.CompanyEmailTemplate> { companyEmailTemplateDomianModels[0]};
        //    mockEmailTemplateService.Setup(x => x.GetCompanyEmailTemplate(It.Is<DomModel.CompanyEmailTemplate>(c => c.CustomerReportingNotificationEmailText.Contains("@CustomerName@")))).Returns(new Response().ToPopulate(ResponseType.Success, null, null, null, result, null, result?.Count));

        //    companyEmailTemplateController = new CompanyEmailTemplateController(mockEmailTemplateService.Object);
        //    var response = companyEmailTemplateController.Get("UK050", new DomModel.CompanyEmailTemplate { CustomerReportingNotificationEmailText = "*@CustomerName@*" });

        //    Assert.AreEqual("1", response.Code);
        //    Assert.AreEqual(1, (response.Result as List<DomModel.CompanyEmailTemplate>).Count);
        //}
        //[TestMethod]
        //public void ThrowsExceptionWhileFetchingCompanyEmailTemplate()
        //{
        //    var exception = new Exception("Exception occured while performing some operation.");
        //    mockEmailTemplateService.Setup(x => x.GetCompanyEmailTemplate(It.IsAny<DomModel.CompanyEmailTemplate>())).Returns(new Response().ToPopulate(ResponseType.Error, null, null, null, null, exception, 0));

        //    companyEmailTemplateController = new CompanyEmailTemplateController(mockEmailTemplateService.Object);
        //    var response = companyEmailTemplateController.Get("UK050", new DomModel.CompanyEmailTemplate { CompanyCode = "DZ" });

        //    Assert.AreEqual("11", response.Code);
        //    Assert.IsNotNull(response.Messages);
        //    Assert.AreEqual("31", response.Messages.FirstOrDefault().Code);
        //}
       



    }
}
