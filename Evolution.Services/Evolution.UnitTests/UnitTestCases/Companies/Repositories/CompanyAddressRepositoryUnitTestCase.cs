using AutoMapper;
using Evolution.UnitTest.UnitTestCases;
using Evolution.UnitTests.Mocks.Data.Companies.Db;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Linq;
using DbModel = Evolution.DbRepository.Models;
using DomainData = Evolution.Company.Domain.Interfaces.Data;

namespace Evolution.UnitTests.UnitTestCases.Companies.Repositories
{
    [TestClass]
    public class CompanyAddressRepositoryUnitTestCase : BaseTestCase
    {
        DomainData.ICompanyAddressRepository companyAddressRepository = null;
        Mock<DbSet<DbModel.CompanyOffice>> mockDbSet = null;
        IQueryable<DbModel.CompanyOffice> mockData = null;

        [TestInitialize]
        public void InitializeCompanyAddressRepository()
        {
            mockData = MockCompany.GetCompanyAddressMockData();
            mockDbSet = MockCompany.GetCompanyAddressMockDbSet(mockData);

            mockContext.Setup(c => c.Set<DbModel.CompanyOffice>()).Returns(mockDbSet.Object);
            mockContext.Setup(c => c.CompanyOffice).Returns(mockDbSet.Object);
            companyAddressRepository = new Company.Infrastructure.Data.CompanyOfficeRepository(Mapper.Instance, mockContext.Object);
        }
 
        [TestMethod]
        public void GetAllCompanyAddressList()
        {
            var companyAddress = companyAddressRepository.GetAll().ToList();
            Assert.AreEqual(2, companyAddress.Count);
        }

        [TestMethod]
        public void FindByCompanyAddressList()
        {
            var companyAddress = companyAddressRepository.FindBy(x => x.Company.Code == "UK050").ToList();
            Assert.AreEqual(2, companyAddress.Count);
        }

        [TestMethod]
        public void FindByCompanyAddressListWithNull()
        {
            var companyAddress = companyAddressRepository.FindBy(null).ToList();
            Assert.AreEqual(2, companyAddress.Count);
        }

        [TestMethod]
        public void CheckCompanyAddressExistsWithDocumentName()
        {
            var companyAddress = companyAddressRepository.Exists(x => x.OfficeName == "Aberdeen").Result;
            Assert.AreEqual(true, companyAddress);
        }

        [TestMethod]
        public void FetchCompanyAddressListWithoutSearchValue()
        {
            var companyAddress = companyAddressRepository.Search(new Company.Domain.Models.Companies.CompanyAddress());
            Assert.AreEqual(2, companyAddress.Count);
        }

        //[TestMethod]
        //public void FetchCompanyAddressListWithNullSearchModel()
        //{
        //    var companyAddress = companyAddressRepository.Search(null);
        //    Assert.AreEqual(2, companyAddress.Count);
        //}

        [TestMethod]
        public void FetchCompanyAddressListByCompanyCode()
        {
            var companyAddress = companyAddressRepository.Search(new Company.Domain.Models.Companies.CompanyAddress() { CompanyCode = "UK050" });
            Assert.AreEqual(2, companyAddress.Count);
        }

        [TestMethod]
        public void FetchCompanyAddressListByOfficeNameWildCardSearch()
        {
            var companyAddress = companyAddressRepository.Search(new Company.Domain.Models.Companies.CompanyAddress() { OfficeName = "Haywards*" });
            Assert.AreEqual(1, companyAddress.Count);
        }

        [TestMethod]
        public void FetchCompanyAddressListByCompanyCodeWildCardSearch()
        {
            var companyAddress = companyAddressRepository.Search(new Company.Domain.Models.Companies.CompanyAddress() { CompanyCode = "UK0*" });
            Assert.AreEqual(2, companyAddress.Count);
        }

        [TestMethod]
        public void FetchCompanyAddressListByCompanyCodeAndOfficeName()
        {
            var companyAddress = companyAddressRepository.Search(new Company.Domain.Models.Companies.CompanyAddress() { CompanyCode = "UK050", OfficeName = "Aberdeen" });
            Assert.AreEqual(1, companyAddress.Count);
        }

        [TestMethod]
        public void FetchCompanyAddressListByOfficeCityName()
        {
            var companyAddress = companyAddressRepository.Search(new Company.Domain.Models.Companies.CompanyAddress() { City= "Aberdeen" });
            Assert.AreEqual(1, companyAddress.Count);
        }

        [TestMethod]
        public void FetchCompanyAddressListByOfficeCountyName()
        {
            var companyAddress = companyAddressRepository.Search(new Company.Domain.Models.Companies.CompanyAddress() { State = "Aberdeen" });
            Assert.AreEqual(1, companyAddress.Count);
        }

        [TestMethod]
        public void FetchCompanyAddressListByOfficeCountryName()
        {
            var companyAddress = companyAddressRepository.Search(new Company.Domain.Models.Companies.CompanyAddress() {  Country = "United Kingdom" });
            Assert.AreEqual(2, companyAddress.Count);
        }

        [TestMethod]
        public void FetchCompanyAddressListByOfficePostalCode()
        {
            var companyAddress = companyAddressRepository.Search(new Company.Domain.Models.Companies.CompanyAddress() {  PostalCode= "AB23 8HZ" });
            Assert.AreEqual(1, companyAddress.Count);
        }

        [TestMethod]
        public void FetchCompanyAddressListByOfficeMultipleSearchCondition()
        {
            var companyAddress = companyAddressRepository.Search(new Company.Domain.Models.Companies.CompanyAddress() { PostalCode = "AB23 8HZ", AccountRef = "Aber01" });
            Assert.AreEqual(1, companyAddress.Count);
        }

        [TestMethod]
        public void FetchCompanyAddressListByOfficeAddrress()
        {
            var companyAddress = companyAddressRepository.Search(new Company.Domain.Models.Companies.CompanyAddress() { FullAddress= "Intertek Inspection Services UK Limited (Formerly Moody International Limited)  The Forum,  277 London Road,  Burgess Hill,  West Sussex  RH15 9QU  UK" });
            Assert.AreEqual(1, companyAddress.Count);
        }

        [TestMethod]
        public void FetchCompanAddressListWithCompanyCodeEmptystring()
        {
            var companyAddress = companyAddressRepository.Search(new Company.Domain.Models.Companies.CompanyAddress() { CompanyCode = string.Empty });
            Assert.AreEqual(2, companyAddress.Count);
        }

        //[TestMethod]
        //public void FetchCompanyAddressListWithSpaceInCompanyCodeAndFullAddress()
        //{
        //    var companyAddress = companyAddressRepository.Search(new Company.Domain.Models.Companies.CompanyAddress() { CompanyCode = " ", FullAddress = " " });
        //    Assert.AreEqual(2, companyAddress.Count);
        //}
         
        [TestMethod]
        public void AddCompanyAddress()
        {
            var newCompanyAddressData = mockData.First();
            var companyAddress = companyAddressRepository.Add(newCompanyAddressData);

            mockDbSet.Verify(m => m.Add(It.IsAny<DbModel.CompanyOffice>()), Times.Once);
            mockContext.Verify(m => m.SaveChanges(), Times.Once);
        }

        [TestMethod]
        public void AddListOfCompanyAddresses()
        {
            var newCompanyAddressData = mockData.ToList();
            companyAddressRepository.Add(newCompanyAddressData);

            mockDbSet.Verify(m => m.AddRange(It.IsAny<IList<DbModel.CompanyOffice>>()), Times.Once);
            mockContext.Verify(m => m.SaveChanges(), Times.AtLeastOnce);
        }

        [TestMethod]
        public void CompanyAddressUpdate()
        {
            var newCompanyAddressData = mockData.First();
            companyAddressRepository.Update(newCompanyAddressData);

            mockDbSet.Verify(m => m.Update(It.IsAny<DbModel.CompanyOffice>()), Times.Once);
            mockContext.Verify(m => m.SaveChanges(), Times.Once);
        }

        [TestMethod]
        public void CompanyAddressDelete()
        {
            var newCompanyAddressData = mockData.First();
            companyAddressRepository.Delete(newCompanyAddressData);

            mockDbSet.Verify(m => m.Remove(It.IsAny<DbModel.CompanyOffice>()), Times.Once);
            mockContext.Verify(m => m.SaveChanges(), Times.Once);
        }

        [TestMethod]
        public void CompanyAddressDeleteBasedOnExpression()
        {
            companyAddressRepository.Delete(x => x.CompanyId == 116);

            mockContext.Verify(m => m.RemoveRange(It.IsAny<IQueryable<DbModel.CompanyOffice>>()), Times.AtLeastOnce);
            mockContext.Verify(m => m.SaveChanges(), Times.AtLeastOnce);
        }

        [TestMethod]
        public void ForceSaveDataShouldFailIfAutoSaveIsTrue()
        {
            companyAddressRepository.AutoSave = true;
            companyAddressRepository.ForceSave();
            companyAddressRepository.AutoSave = false;
            mockContext.Verify(m => m.SaveChanges(), Times.Never);
        }

        [TestMethod]
        public void ForceSaveDataShouldPassIfAutoSaveIsFalse()
        {
            companyAddressRepository.AutoSave = false;
            companyAddressRepository.ForceSave();

            mockContext.Verify(m => m.SaveChanges(), Times.Once);
        } 
    }
}
