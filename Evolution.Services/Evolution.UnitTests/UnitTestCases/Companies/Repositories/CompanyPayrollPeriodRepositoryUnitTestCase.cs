using Evolution.UnitTest.UnitTestCases;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using DomainData = Evolution.Company.Domain.Interfaces.Data;
using DbModel = Evolution.DbRepository.Models;
using Moq;
using Microsoft.EntityFrameworkCore;
using Evolution.UnitTests.Mocks.Data.Companies.Db;
using AutoMapper;
using System.Linq;

namespace Evolution.UnitTests.UnitTestCases.Companies.Repositories
{
    [TestClass]
    public class CompanyPayrollPeriodRepositoryUnitTestCase : BaseTestCase
    {
        DomainData.ICompanyPayrollPeriodRepository companyPayrollPeriodRepository = null;
        Mock<DbSet<DbModel.CompanyPayrollPeriod>> mockDbSet = null;
        IQueryable<DbModel.CompanyPayrollPeriod> mockData = null;

        [TestInitialize]
        public void InitializeCompanyPayrollPeriodRepository()
        {
            mockData = MockCompany.GetCompanyPayrollPeriodMockData();
            mockDbSet = MockCompany.GetCompanyPayrollPeriodMockDbSet(mockData);

            mockContext.Setup(c => c.Set<DbModel.CompanyPayrollPeriod>()).Returns(mockDbSet.Object);
            mockContext.Setup(c => c.CompanyPayrollPeriod).Returns(mockDbSet.Object);
            companyPayrollPeriodRepository = new Company.Infrastructure.Data.CompanyPayrollPeriodRepository(Mapper.Instance, mockContext.Object);
        }

        [TestMethod]
        public void GetAllCompanyPayrollPeriodList()
        {
            var companyPayrollPeriod = companyPayrollPeriodRepository.GetAll().ToList();
            Assert.AreEqual(3, companyPayrollPeriod.Count);
        }

        [TestMethod]
        public void FindByCompanyPayrollPeriodListWithCompanyPayrollId()
        {
            var companyPayrollPeriod = companyPayrollPeriodRepository.FindBy(x => x.CompanyPayrollId == 66).ToList();
            Assert.AreEqual(1, companyPayrollPeriod.Count);
        }

        [TestMethod]
        public void FindByCompanyPayrollPeriodListWithNull()
        {
            var companyPayrollPeriod = companyPayrollPeriodRepository.FindBy(null).ToList();
            Assert.AreEqual(3, companyPayrollPeriod.Count);
        }

        [TestMethod]
        public void CheckCompanyPayrollPeriodExistsWithCompanyPayrollId()
        {
            var companyPayrollPeriod = companyPayrollPeriodRepository.Exists(x => x.CompanyPayrollId==66).Result;
            Assert.AreEqual(true, companyPayrollPeriod);
        }

        [TestMethod]
        public void FetchCompanyPayrollPeriodListWithoutSearchValue()
        {
            var companyPayrollPeriod = companyPayrollPeriodRepository.Search(new Company.Domain.Models.Companies.CompanyPayrollPeriod());
            Assert.AreEqual(3, companyPayrollPeriod.Count);
        }

        //[TestMethod]
        //public void FetchCompanyPayrollPeriodListWithNullSearchModel()
        //{
        //    var companyPayrollPeriod = companyPayrollPeriodRepository.Search(null);
        //    Assert.AreEqual(3, companyPayrollPeriod.Count);
        //}

        [TestMethod]
        public void FetchCompanyPayrollPeriodListByCompanyCode()
        {
            var companyPayrollPeriod = companyPayrollPeriodRepository.Search(new Company.Domain.Models.Companies.CompanyPayrollPeriod() { CompanyCode = "UK050" });
            Assert.AreEqual(3, companyPayrollPeriod.Count);
        }

        [TestMethod]
        public void FetchCompanyPayrollPeriodListByStartDate()
        {
            var companyPayrollPeriod = companyPayrollPeriodRepository.Search(new Company.Domain.Models.Companies.CompanyPayrollPeriod() { StartDate= Convert.ToDateTime("2007-09-29 00:00:00.000") });
            Assert.AreEqual(1, companyPayrollPeriod.Count);
        }

        [TestMethod]
        public void FetchCompanyPayrollPeriodListByPayrollPeriodWildCardSerach()
        {
            var companyPayrollPeriod = companyPayrollPeriodRepository.Search(new Company.Domain.Models.Companies.CompanyPayrollPeriod() { PeriodName ="*12"});
            Assert.AreEqual(2, companyPayrollPeriod.Count);
        }

        [TestMethod]
        public void FetchCompanyPayrollPeriodListByPeriodStatus()
        {
            var companyPayrollPeriod = companyPayrollPeriodRepository.Search(new Company.Domain.Models.Companies.CompanyPayrollPeriod() { PeriodStatus = "N" });
            Assert.AreEqual(2, companyPayrollPeriod.Count);
        }

        [TestMethod]
        public void FetchCompanyPayrollPeriodListByCompanyCodeAndPayrollPeriod()
        {
            var companyPayrollPeriod = companyPayrollPeriodRepository.Search(new Company.Domain.Models.Companies.CompanyPayrollPeriod() { CompanyCode = "UK050", PeriodName = "October" });
            Assert.AreEqual(1, companyPayrollPeriod.Count);
        }

        [TestMethod]
        public void FetchCompanyPayrollPeriodListByWrongCompanyCodeAndPayrollPeriod()
        {
            var companyPayrollPeriod = companyPayrollPeriodRepository.Search(new Company.Domain.Models.Companies.CompanyPayrollPeriod() { CompanyCode = "UK058", PeriodName = "October112" });
            Assert.AreEqual(0, companyPayrollPeriod.Count);
        }

        [TestMethod]
        public void FetchCompanyPayrollPeriodListWithCompanyCodeAndPeriodNameEmptystring()
        {
            var companyPayrollPeriod = companyPayrollPeriodRepository.Search(new Company.Domain.Models.Companies.CompanyPayrollPeriod() { CompanyCode = string.Empty, PeriodName = string.Empty });
            Assert.AreEqual(3, companyPayrollPeriod.Count);
        }

        [TestMethod]
        public void FetchCompanyPayrollPeriodListWithCompanyCodeEmptystring()
        {
            var companyPayrollPeriod = companyPayrollPeriodRepository.Search(new Company.Domain.Models.Companies.CompanyPayrollPeriod() { CompanyCode = string.Empty });
            Assert.AreEqual(3, companyPayrollPeriod.Count);
        }

        //[TestMethod]
        //public void FetchCompanyPayrollPeriodListWithSpaceInCompanyCodeAndPeriodName()
        //{
        //    var companyPayrollPeriod = companyPayrollPeriodRepository.Search(new Company.Domain.Models.Companies.CompanyPayrollPeriod() { CompanyCode = " ", PeriodName = " " });
        //    Assert.AreEqual(3, companyPayrollPeriod.Count);
        //}
         
        [TestMethod]
        public void AddCompanyPayrollPeriod()
        {
            var newCompanyPayrollPeriodData = mockData.First();
            var companyPayrollPeriod = companyPayrollPeriodRepository.Add(newCompanyPayrollPeriodData);

            mockDbSet.Verify(m => m.Add(It.IsAny<DbModel.CompanyPayrollPeriod>()), Times.Once);
            mockContext.Verify(m => m.SaveChanges(), Times.Once);
        }

        [TestMethod]
        public void AddListOfCompanyPayrollPeriods()
        {
            var newCompanyPayrollPeriodData = mockData.ToList();
            companyPayrollPeriodRepository.Add(newCompanyPayrollPeriodData);

            mockDbSet.Verify(m => m.AddRange(It.IsAny<IList<DbModel.CompanyPayrollPeriod>>()), Times.Once);
            mockContext.Verify(m => m.SaveChanges(), Times.AtLeastOnce);
        }

        [TestMethod]
        public void CompanyPayrollPeriodUpdate()
        {
            var newCompanyPayrollPeriodData = mockData.First();
            companyPayrollPeriodRepository.Update(newCompanyPayrollPeriodData);

            mockDbSet.Verify(m => m.Update(It.IsAny<DbModel.CompanyPayrollPeriod>()), Times.Once);
            mockContext.Verify(m => m.SaveChanges(), Times.Once);
        }

        [TestMethod]
        public void CompanyPayrollPeriodDelete()
        {
            var newCompanyPayrollPeriodData = mockData.First();
            companyPayrollPeriodRepository.Delete(newCompanyPayrollPeriodData);

            mockDbSet.Verify(m => m.Remove(It.IsAny<DbModel.CompanyPayrollPeriod>()), Times.Once);
            mockContext.Verify(m => m.SaveChanges(), Times.Once);
        }

        [TestMethod]
        public void CompanyPayrollPeriodDeleteBasedOnExpression()
        {
            companyPayrollPeriodRepository.Delete(x => x.Id == 1);

            mockContext.Verify(m => m.RemoveRange(It.IsAny<IQueryable<DbModel.CompanyPayrollPeriod>>()), Times.AtLeastOnce);
            mockContext.Verify(m => m.SaveChanges(), Times.AtLeastOnce);
        }

        [TestMethod]
        public void ForceSaveDataShouldFailIfAutoSaveIsTrue()
        {
            companyPayrollPeriodRepository.AutoSave = true;
            companyPayrollPeriodRepository.ForceSave();
            companyPayrollPeriodRepository.AutoSave = false;
            mockContext.Verify(m => m.SaveChanges(), Times.Never);
        }

        [TestMethod]
        public void ForceSaveDataShouldPassIfAutoSaveIsFalse()
        {
            companyPayrollPeriodRepository.AutoSave = false;
            companyPayrollPeriodRepository.ForceSave();

            mockContext.Verify(m => m.SaveChanges(), Times.Once);
        }
    }
}
