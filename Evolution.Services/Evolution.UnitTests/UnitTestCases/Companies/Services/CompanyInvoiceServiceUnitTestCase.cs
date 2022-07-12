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
    class CompanyInvoiceServiceUnitTestCase
    {
        ServiceDomainData.ICompanyInvoiceService companyInvoiceService = null;
        Mock<DomainData.ICompanyInvoiceRepository> mockCompanyInvoiceRepository = null;
        IList<DomModel.CompanyInvoice> companyInvoiceDomianModels = null;
        Mock<IAppLogger<Company.Core.Services.CompanyInvoiceService>> mockLogger = null;
        Mock<IMapper> mockMapper = null;
        Mock<DomainData.ICompanyRepository> mockCompanyRepository = null;


        [TestInitialize]
        public void InitializeCompanyInvoiceService()
        {
            mockCompanyInvoiceRepository = new Mock<DomainData.ICompanyInvoiceRepository>();
            companyInvoiceDomianModels = MockCompanyDomainModel.GetCompanyInvoiceMockedDomainModelData();
            mockLogger = new Mock<IAppLogger<Company.Core.Services.CompanyInvoiceService>>();
            mockMapper = new Mock<IMapper>();
            mockCompanyRepository = new Mock<DomainData.ICompanyRepository>();
        }

        [TestMethod]
        public void FetchCompanyInvoiceListWithoutSearchValue()
        {
            mockCompanyInvoiceRepository.Setup(x => x.Search("UK050")).Returns(new DomModel.CompanyInvoice());

            companyInvoiceService = new Company.Core.Services.CompanyInvoiceService(mockMapper.Object,mockCompanyInvoiceRepository.Object,mockCompanyRepository.Object, mockLogger.Object);

            var response = companyInvoiceService.GetCompanyInvoice("UK050");

            Assert.AreEqual("11", response.Code);
            Assert.IsNull(response.Result);
            Assert.IsNotNull(response.Messages);
        }

        [TestMethod]
        public void FetchCompanyInvoiceListWithNullSearchModel()
        {
            mockCompanyInvoiceRepository.Setup(x => x.Search("UK050")).Returns(new DomModel.CompanyInvoice());

            companyInvoiceService = new Company.Core.Services.CompanyInvoiceService(mockMapper.Object,mockCompanyInvoiceRepository.Object, mockCompanyRepository.Object, mockLogger.Object);

            var response = companyInvoiceService.GetCompanyInvoice(null);

            Assert.AreEqual("11", response.Code);
            Assert.IsNull(response.Result);
            Assert.IsNotNull(response.Messages);
        }

        [TestMethod]
        public void FetchCompanyInvoiceListByCompanyCode()
        {
            mockCompanyInvoiceRepository.Setup(x => x.Search("UK050")).Returns(companyInvoiceDomianModels[0]);

            companyInvoiceService = new Company.Core.Services.CompanyInvoiceService(mockMapper.Object,mockCompanyInvoiceRepository.Object, mockCompanyRepository.Object, mockLogger.Object);

            var response = companyInvoiceService.GetCompanyInvoice("UK050");

            Assert.AreEqual("1", response.Code);
            Assert.IsNotNull(response.Result);
            Assert.AreEqual("UK050", (response.Result as DomModel.CompanyInvoice).CompanyCode);
        }

        //[TestMethod]
        //public void FetchCompanyInvoiceListByWildCardSearchWithInvoiceCompanyNameStartWith()
        //{
        //    mockCompanyInvoiceRepository.Setup(x => x.Search(It.Is<DomModel.CompanyInvoice>(c => c.InvoiceDraftText.StartsWith("UK - Intertek")))).Returns(companyInvoiceDomianModels[0]);

        //    companyInvoiceService = new Company.Core.Services.CompanyInvoiceService(mockCompanyInvoiceRepository.Object, mockLogger.Object);

        //    var response = companyInvoiceService.GetCompanyInvoice(new DomModel.CompanyInvoice { InvoiceCompanyName = "UK - Intertek*" });

        //    Assert.AreEqual("1", response.Code);
        //    Assert.IsNotNull(response.Result);
        //}

        //[TestMethod]
        //public void FetchCompanyInvoiceListByWildCardSearchWithInvoiceCompanyNameEndWith()
        //{
        //    mockCompanyInvoiceRepository.Setup(x => x.Search(It.Is<DomModel.CompanyInvoice>(c => c.InvoiceCompanyName.EndsWith(" UK Ltd")))).Returns(companyInvoiceDomianModels[0]);

        //    companyInvoiceService = new Company.Core.Services.CompanyInvoiceService(mockCompanyInvoiceRepository.Object, mockLogger.Object);

        //    var response = companyInvoiceService.GetCompanyInvoice(new DomModel.CompanyInvoice { InvoiceCompanyName = "* UK Ltd" });

        //    Assert.AreEqual("1", response.Code);
        //    Assert.IsNotNull(response.Result);
        //}

        //[TestMethod]
        //public void FetchCompanyInvoiceListByWildCardSearchWithInvoiceCompanyNameContains()
        //{
        //    mockCompanyInvoiceRepository.Setup(x => x.Search(It.Is<DomModel.CompanyInvoice>(c => c.InvoiceCompanyName.Contains("Inspection Services ")))).Returns(companyInvoiceDomianModels[0]);

        //    companyInvoiceService = new Company.Core.Services.CompanyInvoiceService(mockCompanyInvoiceRepository.Object, mockLogger.Object);

        //    var response = companyInvoiceService.GetCompanyInvoice(new DomModel.CompanyInvoice { InvoiceCompanyName = "*Inspection Services *" });

        //    Assert.AreEqual("1", response.Code);
        //    Assert.IsNotNull(response.Result);
        //}

        [TestMethod]
        public void ThrowsExceptionWhileFetchingCompanyInvoiceList()
        {
            mockCompanyInvoiceRepository.Setup(x => x.Search("UK050")).Throws(new Exception("Exception occured while performing some operation."));
            companyInvoiceService = new Company.Core.Services.CompanyInvoiceService(mockMapper.Object,mockCompanyInvoiceRepository.Object, mockCompanyRepository.Object,mockLogger.Object);

            var response = companyInvoiceService.GetCompanyInvoice("UK050");

            Assert.AreEqual("11", response.Code);
            Assert.IsNotNull(response.Messages);
            Assert.AreEqual("31", response.Messages.FirstOrDefault().Code);
        }
    }
}
