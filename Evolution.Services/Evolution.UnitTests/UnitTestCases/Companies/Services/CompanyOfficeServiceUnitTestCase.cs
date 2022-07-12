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
using AutoMapper;

namespace Evolution.UnitTests.UnitTestCases.Companies.Services
{
   [TestClass]
    public class CompanyOfficeServiceUnitTestCase
    {
        ServiceDomainData.ICompanyOfficeService companyOfficeService = null;
        Mock<DomainData.ICompanyAddressRepository> mockCompanyOfficeRepository = null;
        Mock<DomainData.ICompanyRepository> mockCompanyRepository = null;
        Mock<ICityRepository> mockCityRepository = null;
        IList<DomModel.CompanyAddress> companyOfficeDomianModels = null;
        Mock<IAppLogger<Company.Core.Services.CompanyOfficeService>> mockLogger = null;
        Mock<IMapper> mockMapper = null;
        Mock<ICountyRepository> mockCountyRepository = null;
        Mock<ICountryRepository> mockCountryRepository = null;

        [TestInitialize]
        public void InitializeCompanyOfficeService()
        {
            mockCompanyOfficeRepository = new Mock<DomainData.ICompanyAddressRepository>();
            mockCompanyRepository = new Mock<DomainData.ICompanyRepository>();
            mockCityRepository = new Mock<ICityRepository>();
            companyOfficeDomianModels = MockCompanyDomainModel.GetCompanyOfficeMockedDomainModelData();
            mockLogger = new Mock<IAppLogger<Company.Core.Services.CompanyOfficeService>>();
            mockMapper = new Mock<IMapper>();
            mockCountyRepository = new Mock<ICountyRepository>();
            mockCountryRepository = new Mock<ICountryRepository>();
        }
        [TestMethod]
        public void FetchCompanyOfficeListWithoutSearchValue()
        {
            mockCompanyOfficeRepository.Setup(x => x.Search(It.IsAny<DomModel.CompanyAddress>())).Returns(companyOfficeDomianModels);

            companyOfficeService = new Company.Core.Services.CompanyOfficeService(mockMapper.Object,mockCompanyOfficeRepository.Object, mockLogger.Object, mockCompanyRepository.Object,mockCityRepository.Object,mockCountyRepository.Object,mockCountryRepository.Object);

            var response = companyOfficeService.GetCompanyAddress(new DomModel.CompanyAddress());

            Assert.AreEqual("1", response.Code);
            Assert.AreEqual(2, (response.Result as List<DomModel.CompanyAddress>).Count);
        }

        [TestMethod]
        public void FetchCompanyOfficeListWithNullSearchModel()
        {
            mockCompanyOfficeRepository.Setup(x => x.Search(It.IsAny<DomModel.CompanyAddress>())).Returns(companyOfficeDomianModels);

            companyOfficeService = new Company.Core.Services.CompanyOfficeService(mockMapper.Object,mockCompanyOfficeRepository.Object, mockLogger.Object,mockCompanyRepository.Object,mockCityRepository.Object,mockCountyRepository.Object,mockCountryRepository.Object);

            var response = companyOfficeService.GetCompanyAddress(null);

            Assert.AreEqual("1", response.Code);
            Assert.AreEqual(2, (response.Result as List<DomModel.CompanyAddress>).Count);
        }

        [TestMethod]
        public void FetchCompanyOfficeListByCompanyCode()
        {
            mockCompanyOfficeRepository.Setup(x => x.Search(It.Is<DomModel.CompanyAddress>(c => c.CompanyCode == "UK050"))).Returns(new List<DomModel.CompanyAddress> { companyOfficeDomianModels[0] });

            companyOfficeService = new Company.Core.Services.CompanyOfficeService(mockMapper.Object,mockCompanyOfficeRepository.Object, mockLogger.Object, mockCompanyRepository.Object, mockCityRepository.Object,mockCountyRepository.Object,mockCountryRepository.Object);

            var response = companyOfficeService.GetCompanyAddress(new DomModel.CompanyAddress { CompanyCode = "UK050" });

            Assert.AreEqual("1", response.Code);
            Assert.AreEqual(1, (response.Result as List<DomModel.CompanyAddress>).Count);
        }

        [TestMethod]
        public void FetchCompanyOfficeListByWildCardSearchWithAddressStartWith()
        {
            mockCompanyOfficeRepository.Setup(x => x.Search(It.Is<DomModel.CompanyAddress>(c => c.FullAddress.StartsWith("Intertek")))).Returns(new List<DomModel.CompanyAddress> { companyOfficeDomianModels[0] });

            companyOfficeService = new Company.Core.Services.CompanyOfficeService(mockMapper.Object,mockCompanyOfficeRepository.Object, mockLogger.Object,mockCompanyRepository.Object,mockCityRepository.Object,mockCountyRepository.Object,mockCountryRepository.Object);

            var response = companyOfficeService.GetCompanyAddress(new DomModel.CompanyAddress { FullAddress = "Intertek*" });

            Assert.AreEqual("1", response.Code);
            Assert.AreEqual(1, (response.Result as List<DomModel.CompanyAddress>).Count);
        }

        [TestMethod]
        public void FetchCompanyOfficeListByWildCardSearchWithAddressEndWith()
        {
            mockCompanyOfficeRepository.Setup(x => x.Search(It.Is<DomModel.CompanyAddress>(c => c.FullAddress.EndsWith("West Sussex  RH15 9QU  UK")))).Returns(new List<DomModel.CompanyAddress> { companyOfficeDomianModels[0] });

            companyOfficeService = new Company.Core.Services.CompanyOfficeService(mockMapper.Object,mockCompanyOfficeRepository.Object, mockLogger.Object,mockCompanyRepository.Object,mockCityRepository.Object,mockCountyRepository.Object,mockCountryRepository.Object);

            var response = companyOfficeService.GetCompanyAddress(new DomModel.CompanyAddress { FullAddress = "*West Sussex  RH15 9QU  UK" });

            Assert.AreEqual("1", response.Code);
            Assert.AreEqual(1, (response.Result as List<DomModel.CompanyAddress>).Count);
        }

        [TestMethod]
        public void FetchCompanyOfficeListByWildCardSearchWithAddressContains()
        {
            mockCompanyOfficeRepository.Setup(x => x.Search(It.Is<DomModel.CompanyAddress>(c => c.FullAddress.Contains("Formerly Moody International Limited")))).Returns(new List<DomModel.CompanyAddress> { companyOfficeDomianModels[0] });

            companyOfficeService = new Company.Core.Services.CompanyOfficeService(mockMapper.Object,mockCompanyOfficeRepository.Object, mockLogger.Object,mockCompanyRepository.Object,mockCityRepository.Object,mockCountyRepository.Object,mockCountryRepository.Object);

            var response = companyOfficeService.GetCompanyAddress(new DomModel.CompanyAddress { FullAddress = "*Formerly Moody International Limited*" });

            Assert.AreEqual("1", response.Code);
            Assert.AreEqual(1, (response.Result as List<DomModel.CompanyAddress>).Count);
        }

        [TestMethod]
        public void ThrowsExceptionWhileFetchingCompanyOfficeList()
        {
            mockCompanyOfficeRepository.Setup(x => x.Search(It.IsAny<DomModel.CompanyAddress>())).Throws(new Exception("Exception occured while performing some operation."));
            companyOfficeService = new Company.Core.Services.CompanyOfficeService(mockMapper.Object,mockCompanyOfficeRepository.Object, mockLogger.Object,mockCompanyRepository.Object,mockCityRepository.Object,mockCountyRepository.Object,mockCountryRepository.Object);

            var response = companyOfficeService.GetCompanyAddress(new DomModel.CompanyAddress());

            Assert.AreEqual("11", response.Code);
            Assert.IsNotNull(response.Messages);
            Assert.AreEqual("31", response.Messages.FirstOrDefault().Code);
        }
         
    }
}
