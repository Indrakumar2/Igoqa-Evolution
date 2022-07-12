using AutoMapper;
using Evolution.UnitTests.Mocks.Data.Companies.Db;
using Evolution.UnitTest.UnitTestCases;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using DomainData = Evolution.Company.Domain.Interfaces.Data;
using DbModel = Evolution.DbRepository.Models;
using Moq;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Evolution.UnitTests.UnitTestCases.Companies.Repositories
{
    [TestClass]
    public class CompanyRepositoryUnitTestCase : BaseTestCase
    {        
        DomainData.ICompanyRepository companyRepository = null;
        Mock<DbSet<DbModel.Company>> mockDbSet = null;
        IQueryable<DbModel.Company>  mockData=null;

        [TestInitialize]
        public void InitializeCompanyRepository()
        {   
            mockData = MockCompany.GetCompanyMockData();
            mockDbSet = MockCompany.GetCompanyMockDbSet(mockData);

            mockContext.Setup(c => c.Set<DbModel.Company>()).Returns(mockDbSet.Object);
            mockContext.Setup(c => c.Company).Returns(mockDbSet.Object);
            companyRepository = new Company.Infrastructure.Data.CompanyRepository(mockContext.Object, Mapper.Instance);
        }

        [TestMethod]
        public void FetchCompanyListWithoutSearchValue()
        {
            var company=companyRepository.Search(new Company.Domain.Models.Companies.Company());
            Assert.AreEqual(2, company.Count);
        }

        [TestMethod]
        public void FetchCompanyListWithNullSearchModel()
        {
            var company = companyRepository.Search(null);
            Assert.AreEqual(2, company.Count);
        }

        //[TestMethod]
        //public void FetchCompanyListByCompanyCode()
        //{
        //    var company = companyRepository.Search(new Company.Domain.Models.Companies.Company() { CompanyCode= "DZ002" });
        //    Assert.AreEqual(1, company.Count);
        //}

        [TestMethod]
        public void FetchCompanyListByNameWildCardSearch()
        {
            var company = companyRepository.Search(new Company.Domain.Models.Companies.Company() { CompanyName = "*Brasil*" });
            Assert.AreEqual(1, company.Count);
        }

        [TestMethod]
        public void FetchCompanListWithCompanyCodeEmptystring()
        {
            var company = companyRepository.Search(new Company.Domain.Models.Companies.Company() { CompanyCode = string.Empty });
            Assert.AreEqual(2, company.Count);
        }

        [TestMethod]
        public void FetchCompanyListWithSpaceInCompanyCode()
        {
            var company = companyRepository.Search(new Company.Domain.Models.Companies.Company() { CompanyCode = " " });
            Assert.AreEqual(2, company.Count);
        }

        [TestMethod]
        public void GetAllCompanyList()
        {
            var company = companyRepository.GetAll().ToList();
            Assert.AreEqual(2, company.Count);
        }

        //[TestMethod]
        //public void FindByCompanyListWithCompanyCode()
        //{
        //    var company = companyRepository.FindBy(x => x.Code == "DZ002").ToList();
        //    Assert.AreEqual(1, company.Count);
        //}

        [TestMethod]
        public void FindByCompanyListWithNull()
        {
            var company = companyRepository.FindBy(null).ToList();
            Assert.AreEqual(2, company.Count);
        }

        //[TestMethod]
        //public void CheckCompanyExistsWithCompanyCode()
        //{
        //    var company = companyRepository.Exists(x => x.Code == "DZ002").Result;
        //    Assert.AreEqual(true, company);
        //}
         
        [TestMethod]
        public void AddCompany()
        {
            var newCompanyData = mockData.First(); 
            var company = companyRepository.Add(newCompanyData);

            mockDbSet.Verify(m => m.Add(It.IsAny<DbModel.Company>()), Times.Once);
            mockContext.Verify(m => m.SaveChanges(), Times.Once);
        }

        [TestMethod]
        public void AddListOfCompanies()
        {
            var newCompaniesData = mockData.ToList();
             companyRepository.Add(newCompaniesData);

            mockDbSet.Verify(m => m.AddRange(It.IsAny<IList<DbModel.Company>>()), Times.Once);
            mockContext.Verify(m => m.SaveChanges(), Times.Once);
        }

        [TestMethod]
        public void CompanyUpdate()
        {
            var newCompanyData = mockData.First();
            companyRepository.Update(newCompanyData);

            mockDbSet.Verify(m => m.Update(It.IsAny<DbModel.Company>()), Times.Once);
            mockContext.Verify(m => m.SaveChanges(), Times.Once);
        }

        [TestMethod]
        public void CompanyDelete()
        {
            var newCompanyData = mockData.First();
            companyRepository.Delete(newCompanyData);

            mockDbSet.Verify(m => m.Remove(It.IsAny<DbModel.Company>()), Times.Once);
            mockContext.Verify(m => m.SaveChanges(), Times.Once);
        }

        //[TestMethod]
        //public void CompanyDeleteBasedOnExpression()
        //{  
        //    companyRepository.Delete(x=>x.Code== "DZ002");

        //    mockContext.Verify(m => m.RemoveRange(It.IsAny<IQueryable<DbModel.Company>>()), Times.AtLeastOnce);
        //    mockContext.Verify(m => m.SaveChanges(), Times.AtLeastOnce);
        //}

        [TestMethod]
        public void ForceSaveDataShouldFailIfAutoSaveIsTrue()
        {
            companyRepository.AutoSave = true;
            companyRepository.ForceSave();
            companyRepository.AutoSave = false;
            mockContext.Verify(m => m.SaveChanges(), Times.Never);
        }

        [TestMethod]
        public void ForceSaveDataShouldPassIfAutoSaveIsFalse()
        {
            companyRepository.AutoSave = false;
            companyRepository.ForceSave();

            mockContext.Verify(m => m.SaveChanges(), Times.Once);
        }

    }
}
