using Evolution.DbRepository.Services.Master;
using Evolution.Master.Infrastructure.Data; 
using Evolution.UnitTest.Mocks.Data.Masters.Db.Masters;
using Evolution.UnitTest.UnitTestCases;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DbData = Evolution.DbRepository.Interfaces.Master;
using DomainData = Evolution.Master.Domain.Interfaces.Data;
using DbModel = Evolution.DbRepository.Models;
using DomModel = Evolution.Master.Domain.Models;
using Evolution.UnitTests.Mocks.Data.Masters.Domain.Masters;
using AutoMapper;

namespace Evolution.UnitTests.UnitTestCases.Masters.Repositories
{
    [TestClass]
    public class MasterRepositoryUnitTestCase : BaseTestCase
    {
        DbData.ICountryRepository countryRepository = null;
        DbData.ICityRepository cityRepository = null;
        DbData.ICountyRepository countyRepository = null;
        DomainData.ITaxTypeRepository taxTypeRepository = null;
        DomainData.IEmailPlaceholderRepository emailPlaceholderRepository = null;
         DomainData.IModuleDocumentTypeRepository moduleDocumentTypeRespository = null;

        [TestInitialize]
        public void InitializeMasterRepository()
        {
            var countryMockDbSet = MockMaster.GetCountryMockDbSet(MockMaster.GetCountryMockData());
            var cityMockDbSet = MockMaster.GetCountryMockDbSet(MockMaster.GetCityMockData());
            var countyMockDbSet = MockMaster.GetCountyMockDbSet(MockMaster.GetCountyMockData());
            var taxMockDbSet = MockMaster.GetTaxMockDbSet(MockMaster.GetTaxMockData());
            var emailPlaceHolderMockDbSet = MockMaster.GetEmailPlaceholderMockDbSet(MockMaster.GetEmailPlaceholderMockData());
           
            mockContext.Setup(c => c.Set<DbModel.Country>()).Returns(countryMockDbSet.Object);
            mockContext.Setup(c => c.Set<DbModel.City>()).Returns(cityMockDbSet.Object);
            mockContext.Setup(c => c.Set<DbModel.County>()).Returns(countyMockDbSet.Object);
            mockContext.Setup(c => c.Set<DbModel.Tax>()).Returns(taxMockDbSet.Object);
            mockContext.Setup(c => c.Set<DbModel.EmailPlaceHolder>()).Returns(emailPlaceHolderMockDbSet.Object);
            mockContext.Setup(c => c.EmailPlaceHolder).Returns(emailPlaceHolderMockDbSet.Object);

            countryRepository = new CountryRepository(mockContext.Object);
            cityRepository = new CityRepository(mockContext.Object);
            countyRepository = new CountyRepository(mockContext.Object);
            taxTypeRepository = new TaxTypeRepository(mockContext.Object);
            emailPlaceholderRepository = new EmailPlaceholderRepository(Mapper.Instance, mockContext.Object);
        }

        //country
        [TestMethod]
        public void FetchAllCountry()
        {
            var country = countryRepository.GetAll().ToList();
            Assert.AreEqual(2, country.Count);

        }
        [TestMethod]
        public void FindCountryByName()
        {
            var country = countryRepository.FindBy(x => x.Name == "United Kingdom").ToList();
            Assert.AreEqual(1, country.Count);

        }
        [TestMethod]
        public void FindAllCountry()
        {
            var country = countryRepository.FindBy(null).ToList();
            Assert.AreEqual(2, country.Count);

        }

        [TestMethod]
        public void FindByCountryWithInvalidValue()
        {
            var country = countryRepository.FindBy(x => x.Name == "xyzabc").ToList();
            Assert.AreEqual(0, country.Count);

        }

        //city
        [TestMethod]
        public void FetchAllCities()
        {
            var cities = cityRepository.GetAll().ToList();
            Assert.AreEqual(2, cities.Count);

        }
        [TestMethod]
        public void FindCityByName()
        {
            var cities = cityRepository.FindBy(x => x.Name == "Brighton").ToList();
            Assert.AreEqual(1, cities.Count);

        }
        [TestMethod]
        public void FindAllCities()
        {
            var cities = cityRepository.FindBy(null).ToList();
            Assert.AreEqual(2, cities.Count);

        }
        [TestMethod]
        public void FindAllCitiesByCounty()
        {
            var cities = cityRepository.FindBy(x => x.County.Name == "Brighton and Hove").ToList();
            Assert.AreEqual(2, cities.Count);

        }
        [TestMethod]
        public void FindByCityWithInvalidValue()
        {
            var cities = cityRepository.FindBy(x => x.Name == "xyzabc").ToList();
            Assert.AreEqual(0, cities.Count);

        }

        //County
        [TestMethod]
        public void FetchAllCounties()
        {
            var Counties = countyRepository.GetAll().ToList();
            Assert.AreEqual(2, Counties.Count);

        }
        [TestMethod]
        public void FindCountiesByName()
        {
            var Counties = countyRepository.FindBy(x => x.Name == "Andorra la Vella").ToList();
            Assert.AreEqual(1, Counties.Count);

        }
        [TestMethod]
        public void FindAllCounties()
        {
            var Counties = countyRepository.FindBy(null).ToList();
            Assert.AreEqual(2, Counties.Count);

        }
        [TestMethod]
        public void FindAllCountiesByCounty()
        {
            var Counties = countyRepository.FindBy(x => x.Country.Name == "Andorra, Principality of").ToList();
            Assert.AreEqual(2, Counties.Count);

        }
        [TestMethod]
        public void FindByCountiesWithInvalidValue()
        {
            var Counties = countyRepository.FindBy(x => x.Name == "xyzabc").ToList();
            Assert.AreEqual(0, Counties.Count);

        }

        //Taxes
        [TestMethod]
        public void FetchAllTaxList()
        {
            var Tax = taxTypeRepository.GetAll().ToList();
            Assert.AreEqual(2, Tax.Count);

        }
        [TestMethod]
        public void TaxSearchByName()
        {
            var TaxSearch = new DbModel.Tax() { Name = "GST-0%" };
            var Tax = taxTypeRepository.Search(TaxSearch);
            Assert.AreEqual(1, Tax.Count);

        }
        [TestMethod]
        public void SearchListofTax()
        {
            var Tax = taxTypeRepository.Search(null).ToList();
            Assert.AreEqual(2, Tax.Count);

        }

        [TestMethod]
        public void FindByTaxWithInvalidValue()
        {
            var Tax = taxTypeRepository.FindBy(x => x.Name == "Gst").ToList();
            Assert.AreEqual(0, Tax.Count);

        }

        [TestMethod]
        public void SearchByTaxWithTaxType()
        {
            var TaxSearch = new DbModel.Tax() { TaxType = "S" };
            var Tax = taxTypeRepository.Search(TaxSearch).ToList();
            Assert.AreEqual(1, Tax.Count);

        }

        //emailplaceholder
        [TestMethod]
        public void FetchAllEmailPlaceHolderList()
        {
            var emailPlaceHolders = emailPlaceholderRepository.GetAll().ToList();
            Assert.AreEqual(3, emailPlaceHolders.Count);

        }
        [TestMethod]
        public void EmailPlaceHolderSearchByName()
        {
            var emailPlaceHolderSearch = new DomModel.EmailPlaceholder() { Name = "ProjectNumber" };
            var emailPlaceHolders = emailPlaceholderRepository.Search(emailPlaceHolderSearch);
            Assert.AreEqual(1, emailPlaceHolders.Count);

        }
        [TestMethod]
        public void SearchListofEamilPlaceHolder()
        {
            var emailPlaceHolders = emailPlaceholderRepository.Search(null).ToList();
            Assert.AreEqual(3, emailPlaceHolders.Count);

        }
        [TestMethod]
        public void FindByEmailPlaceHolderWithInvalidValue()
        {
            var emailPlaceHolders = emailPlaceholderRepository.FindBy(x => x.Name == "Gst").ToList();
            Assert.AreEqual(0, emailPlaceHolders.Count);

        }
        [TestMethod]
        public void SearchByEmailPlaceHolderWithModule()
        {
            var EmailPlaceHolderSearch = new DomModel.EmailPlaceholder() { ModuleName = "Visit" };
            var emailPlaceholders = emailPlaceholderRepository.Search(EmailPlaceHolderSearch).ToList();
            Assert.AreEqual(1, emailPlaceholders.Count);

        }

        





    }
}
