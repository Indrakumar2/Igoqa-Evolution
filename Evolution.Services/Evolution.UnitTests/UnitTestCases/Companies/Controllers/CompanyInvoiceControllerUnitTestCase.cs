using System;
using System.Collections.Generic;
using System.Text;
using ServiceDomainData = Evolution.Company.Domain.Interfaces.Companies;
using DomModel = Evolution.Company.Domain.Models.Companies;
using Evolution.Api.Controllers.Company;
using Moq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Evolution.UnitTests.Mocks.Data.Companies;
using Evolution.Common.Models.Responses;
using Evolution.Common.Extensions;
using Evolution.Common.Enums;
using System.Linq;

namespace Evolution.UnitTests.UnitTestCases.Companies.Controllers
{
    [TestClass]
    public class CompanyInvoiceControllerUnitTestCase
    {
        CompanyInvoiceController companyInvoiceController = null;
        Mock<ServiceDomainData.ICompanyInvoiceService> mockcompanyInvoiceService = null;
        IList<DomModel.CompanyInvoice> companyInvoiceDomianModels = null;

        [TestInitialize]
        public void InitializeCompanyExpectedmarginController()
        {
            mockcompanyInvoiceService = new Mock<ServiceDomainData.ICompanyInvoiceService>();
            companyInvoiceDomianModels = MockCompanyDomainModel.GetCompanyInvoiceMockedDomainModelData();
        }
        [TestMethod]
        public void FetchCompanyInvoiceListWithoutSearchValue()
        {
            var result = new List<DomModel.CompanyInvoice> { companyInvoiceDomianModels[0] };
            mockcompanyInvoiceService.Setup(x => x.GetCompanyInvoice("UK050")).Returns(new Response().ToPopulate(ResponseType.Success, null, null, null, result, null, result?.Count));

            companyInvoiceController = new CompanyInvoiceController(mockcompanyInvoiceService.Object);

            var response = companyInvoiceController.Get("UK050");

            Assert.AreEqual("1", response.Code);
            Assert.AreEqual(1, (response.Result as List<DomModel.CompanyInvoice>).Count);
        }
        [TestMethod]
        public void FetchCompanyInvoiceListWithNullSearchModel()
        {
            var result = new List<DomModel.CompanyInvoice> { companyInvoiceDomianModels[0] };
            mockcompanyInvoiceService.Setup(x => x.GetCompanyInvoice("UK050")).Returns(new Response().ToPopulate(ResponseType.Success, null, null, null, result, null, result?.Count));

            companyInvoiceController = new CompanyInvoiceController(mockcompanyInvoiceService.Object);

            var response = companyInvoiceController.Get("UK050");

            Assert.AreEqual("1", response.Code);
            Assert.AreEqual(1, (response.Result as List<DomModel.CompanyInvoice>).Count);
        }
        
        //[TestMethod]
        //public void FetchCompanyInvoiceListWithNullParameters()
        //{
        //    var result = new List<DomModel.CompanyInvoice> { companyInvoiceDomianModels[0] };
        //    mockcompanyInvoiceService.Setup(x => x.GetCompanyInvoice("UK050")).Returns(new Response().ToPopulate(ResponseType.Success, null, null, null, result, null, result?.Count));

        //    companyInvoiceController = new CompanyInvoiceController(mockcompanyInvoiceService.Object);

        //    var response = companyInvoiceController.Get(null);

        //    Assert.AreEqual("11", response.Code);
        //    Assert.IsNull(response.Result);
        //}

        [TestMethod]
        public void FetchCompanyInvoiceListByCompanyCode()
        {
            var result = new List<DomModel.CompanyInvoice> { companyInvoiceDomianModels[0] };
            mockcompanyInvoiceService.Setup(x => x.GetCompanyInvoice("UK050")).Returns(new Response().ToPopulate(ResponseType.Success, null, null, null, result, null, result?.Count));

            companyInvoiceController = new CompanyInvoiceController(mockcompanyInvoiceService.Object);

            var response = companyInvoiceController.Get("UK050");

            Assert.AreEqual("1", response.Code);
            Assert.AreEqual(1, (response.Result as List<DomModel.CompanyInvoice>).Count);
        }
        //[TestMethod]
        //public void FetchCompanyInvoiceListByWildCardSearchWithNameStartWith()
        //{
        //    var result = new List<DomModel.CompanyInvoice> { companyInvoiceDomianModels[0] };
        //    mockcompanyInvoiceService.Setup(x => x.GetCompanyInvoice(It.Is<DomModel.CompanyInvoice>(c => c.InvoiceDraftText.StartsWith("DRA")))).Returns(new Response().ToPopulate(ResponseType.Success, null, null, null, result, null, result?.Count));

        //    companyInvoiceController = new CompanyInvoiceController(mockcompanyInvoiceService.Object);

        //    var response = companyInvoiceController.Get("UK050", new DomModel.CompanyInvoice { InvoiceCompanyName = "UK*" });

        //    Assert.AreEqual("1", response.Code);
        //    Assert.AreEqual(1, (response.Result as List<DomModel.CompanyInvoice>).Count);
        //}
        //[TestMethod]
        //public void FetchCompanyInvoiceByWildCardSearchWithNameEndWith()
        //{
        //    var result = new List<DomModel.CompanyInvoice> { companyInvoiceDomianModels[0]};
        //    mockcompanyInvoiceService.Setup(x => x.GetCompanyInvoice(It.Is<DomModel.CompanyInvoice>(c =>c.CompanyCode== "UK050" && c.InvoiceCompanyName.EndsWith("UK Ltd")))).Returns(new Response().ToPopulate(ResponseType.Success, null, null, null, result, null, result?.Count));

        //    companyInvoiceController = new CompanyInvoiceController(mockcompanyInvoiceService.Object);

        //    var response = companyInvoiceController.Get("UK050", new DomModel.CompanyInvoice { InvoiceCompanyName = "*UK Ltd" });

        //    Assert.AreEqual("1", response.Code);
        //    Assert.AreEqual(1, (response.Result as List<DomModel.CompanyInvoice>).Count);
        //}
        //[TestMethod]
        //public void FetchCompanyInvoiceByWildCardSearchWithNameContains()
        //{
        //    var result = new List<DomModel.CompanyInvoice> { companyInvoiceDomianModels[0] };
        //    mockcompanyInvoiceService.Setup(x => x.GetCompanyInvoice(It.Is<DomModel.CompanyInvoice>(c => c.InvoiceCompanyName.Contains("Inspection")))).Returns(new Response().ToPopulate(ResponseType.Success, null, null, null, result, null, result?.Count));

        //    companyInvoiceController = new CompanyInvoiceController(mockcompanyInvoiceService.Object);
        //    var response = companyInvoiceController.Get("UK050", new DomModel.CompanyInvoice { InvoiceCompanyName = "*Inspection*" });

        //    Assert.AreEqual("1", response.Code);
        //    Assert.AreEqual(1, (response.Result as List<DomModel.CompanyInvoice>).Count);
        //}
        [TestMethod]
        public void ThrowsExceptionWhileFetchingCompanyList()
        {
            var exception = new Exception("Exception occured while performing some operation.");
            mockcompanyInvoiceService.Setup(x => x.GetCompanyInvoice("UK050")).Returns(new Response().ToPopulate(ResponseType.Error, null, null, null, null, exception, 0));

            companyInvoiceController = new CompanyInvoiceController(mockcompanyInvoiceService.Object);
            var response = companyInvoiceController.Get("UK050");

            Assert.AreEqual("11", response.Code);
            Assert.IsNotNull(response.Messages);
            Assert.AreEqual("31", response.Messages.FirstOrDefault().Code);
        }

    }
}
