using Evolution.UnitTest.UnitTestCases;
using System;
using System.Collections.Generic;
using System.Text;
using DomainData = Evolution.Company.Domain.Interfaces.Data;
using DbModel = Evolution.DbRepository.Models;
using AutoMapper;
using Evolution.UnitTests.Mocks.Data.Companies.Db;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Linq;

namespace Evolution.UnitTests.UnitTestCases.Companies.Repositories
{
    [TestClass]
    public class CompanyTaxRepositoryUnitTestCase : BaseTestCase
    {
        DomainData.ICompanyTaxRepository companyTaxRepository = null;
        Mock<DbSet<DbModel.CompanyTax>> mockDbSet = null;
        IQueryable<DbModel.CompanyTax> mockData = null;

        [TestInitialize]
        public void InitializeCompanyTaxRepository()
        {
            mockData = MockCompany.GetCompanyTaxMockData();
            mockDbSet = MockCompany.GetCompanyTaxMockDbSet(mockData);

            mockContext.Setup(c => c.Set<DbModel.CompanyTax>()).Returns(mockDbSet.Object);
            mockContext.Setup(c => c.CompanyTax).Returns(mockDbSet.Object);
            companyTaxRepository = new Company.Infrastructure.Data.CompanyTaxRepository(mockContext.Object, Mapper.Instance);
        }

        [TestMethod]
        public void GetAllCompanyTaxList()
        {
            var companyTax = companyTaxRepository.GetAll().ToList();
            Assert.AreEqual(5, companyTax.Count);
        }

        [TestMethod]
        public void FindByCompanyListWithCompanyCode()
        {
            var companyTax = companyTaxRepository.FindBy(x => x.Company.Code == "UK050").ToList();
            Assert.AreEqual(2, companyTax.Count);
        }

        [TestMethod]
        public void FindByCompanyListWithNull()
        {
            var companyTax = companyTaxRepository.FindBy(null).ToList();
            Assert.AreEqual(5, companyTax.Count);
        }

        [TestMethod]
        public void CheckCompanyTaxExistsWithCompanyCode()
        {
            var companyTax = companyTaxRepository.Exists(x => x.Company.Code == "UK050").Result;
            Assert.AreEqual(true, companyTax);
        }

        [TestMethod]
        public void FetchCompanyTaxListWithoutSearchValue()
        {
            var companyTax = companyTaxRepository.Search(new Company.Domain.Models.Companies.CompanyTax());
            Assert.AreEqual(5, companyTax.Count);
        }

        //[TestMethod]
        //public void FetchCompanyTaxListWithNullSearchModel()
        //{
        //    var companyTax = companyTaxRepository.Search(null);
        //    Assert.AreEqual(6, companyTax.Count);
        //}

        [TestMethod]
        public void FetchCompanyTaxListByCompanyCode()
        {
            var companyTax = companyTaxRepository.Search(new Company.Domain.Models.Companies.CompanyTax() { CompanyCode = "UK050" });
            Assert.AreEqual(2, companyTax.Count);
        }

        [TestMethod]
        public void FetchCompanyTaxListByTaxNameWildCardSearch()
        {
            var companyTax = companyTaxRepository.Search(new Company.Domain.Models.Companies.CompanyTax() {  Tax = "VAT*" });
            Assert.AreEqual(4, companyTax.Count);
        }

        [TestMethod]
        public void FetchCompanyTaxListByCompanyCodeWildCardSearch()
        {
            var companyTax = companyTaxRepository.Search(new Company.Domain.Models.Companies.CompanyTax() { CompanyCode = "UK0*" });
            Assert.AreEqual(4, companyTax.Count);
        }

        [TestMethod]
        public void FetchCompanyTaxListByCompanyCodeAndTaxName()
        {
            var companyTax = companyTaxRepository.Search(new Company.Domain.Models.Companies.CompanyTax() { CompanyCode = "UK050", Tax = "VAT - Exempt" });
            Assert.AreEqual(1, companyTax.Count);
        }

        [TestMethod]
        public void FetchCompanyTaxListByOnlyTaxNameWildCardChar()
        {
            var companyTax = companyTaxRepository.Search(new Company.Domain.Models.Companies.CompanyTax() { Tax = "*" });
            Assert.AreEqual(5, companyTax.Count);
        }

        [TestMethod]
        public void FetchCompanTaxListWithCompanyCodeEmptystring()
        {
            var companyTax = companyTaxRepository.Search(new Company.Domain.Models.Companies.CompanyTax() { CompanyCode = string.Empty });
            Assert.AreEqual(5, companyTax.Count);
        }

        //[TestMethod]
        //public void FetchCompanyTaxListWithSpaceInCompanyCode()
        //{
        //    var companyTax = companyTaxRepository.Search(new Company.Domain.Models.Companies.CompanyTax() { Tax = " " });
        //    Assert.AreEqual(5, companyTax.Count);
        //}
         
        [TestMethod]
        public void AddCompanyTax()
        {
            var newCompanyTaxData = mockData.First();
            var companyTax = companyTaxRepository.Add(newCompanyTaxData);

            mockDbSet.Verify(m => m.Add(It.IsAny<DbModel.CompanyTax>()), Times.Once);
            mockContext.Verify(m => m.SaveChanges(), Times.Once);
        }

        [TestMethod]
        public void AddListOfCompanyTaxs()
        {
            var newCompanyTaxData = mockData.ToList();
            companyTaxRepository.Add(newCompanyTaxData);

            mockDbSet.Verify(m => m.AddRange(It.IsAny<IList<DbModel.CompanyTax>>()), Times.Once);
            mockContext.Verify(m => m.SaveChanges(), Times.AtLeastOnce);
        }

        [TestMethod]
        public void CompanyTaxUpdate()
        {
            var newCompanyTaxData = mockData.First();
            companyTaxRepository.Update(newCompanyTaxData);

            mockDbSet.Verify(m => m.Update(It.IsAny<DbModel.CompanyTax>()), Times.Once);
            mockContext.Verify(m => m.SaveChanges(), Times.Once);
        }

        [TestMethod]
        public void CompanyTaxDelete()
        {
            var newCompanyTaxData = mockData.First();
            companyTaxRepository.Delete(newCompanyTaxData);

            mockDbSet.Verify(m => m.Remove(It.IsAny<DbModel.CompanyTax>()), Times.Once);
            mockContext.Verify(m => m.SaveChanges(), Times.Once);
        }

        [TestMethod]
        public void CompanyTaxDeleteBasedOnExpression()
        {
            companyTaxRepository.Delete(x => x.Id == 1);

            mockContext.Verify(m => m.RemoveRange(It.IsAny<IQueryable<DbModel.CompanyTax>>()), Times.AtLeastOnce);
            mockContext.Verify(m => m.SaveChanges(), Times.AtLeastOnce);
        }

        [TestMethod]
        public void ForceSaveDataShouldFailIfAutoSaveIsTrue()
        {
            companyTaxRepository.AutoSave = true;
            companyTaxRepository.ForceSave();
            companyTaxRepository.AutoSave = false;
            mockContext.Verify(m => m.SaveChanges(), Times.Never);
        }

        [TestMethod]
        public void ForceSaveDataShouldPassIfAutoSaveIsFalse()
        {
            companyTaxRepository.AutoSave = false;
            companyTaxRepository.ForceSave();

            mockContext.Verify(m => m.SaveChanges(), Times.Once);
        }
    }
}
