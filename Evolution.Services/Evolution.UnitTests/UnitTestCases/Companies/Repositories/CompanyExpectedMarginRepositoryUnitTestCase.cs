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
   public class CompanyExpectedMarginRepositoryUnitTestCase : BaseTestCase
    {
        DomainData.ICompanyExpectedMarginRepository companyExpectedMarginRepository = null;
        Mock<DbSet<DbModel.CompanyExpectedMargin>> mockDbSet = null;
        IQueryable<DbModel.CompanyExpectedMargin> mockData = null;

        [TestInitialize]
        public void InitializeCompanyExpectedMarginRepository()
        {
            mockData = MockCompany.GetCompanyExpectedMarginMockData();
            mockDbSet = MockCompany.GetCompanyExpectedMarginMockDbSet(mockData);

            mockContext.Setup(c => c.Set<DbModel.CompanyExpectedMargin>()).Returns(mockDbSet.Object);
            mockContext.Setup(c => c.CompanyExpectedMargin).Returns(mockDbSet.Object);
            companyExpectedMarginRepository = new Company.Infrastructure.Data.CompanyExpectedMarginRepository(Mapper.Instance, mockContext.Object);
        }
         
        [TestMethod]
        public void GetAllCompanyExpectedMarginList()
        {
            var companyExpectedMargin = companyExpectedMarginRepository.GetAll().ToList();
            Assert.AreEqual(5, companyExpectedMargin.Count);
        }

        [TestMethod]
        public void FindByCompanyExpectedMarginListWithMinimumMargin()
        {
            var companyExpectedMargin = companyExpectedMarginRepository.FindBy(x => x.MinimumMargin>10 && x.MinimumMargin < 25).ToList();
            Assert.AreEqual(3, companyExpectedMargin.Count);
        }

        [TestMethod]
        public void FindByCompanyExpectedMarginListWithNull()
        {
            var companyExpectedMargin = companyExpectedMarginRepository.FindBy(null).ToList();
            Assert.AreEqual(5, companyExpectedMargin.Count);
        }

        [TestMethod]
        public void CheckCompanyExpectedMarginExistsWithExpectedMarginName()
        {
            var companyExpectedMargin = companyExpectedMarginRepository.Exists(x => x.MarginType.Name == "TIS (Technical Inspection Services)").Result;
            Assert.AreEqual(true, companyExpectedMargin);
        }

        [TestMethod]
        public void FetchCompanyExpectedMarginListWithoutSearchValue()
        {
            var companyExpectedMargin = companyExpectedMarginRepository.Search(new Company.Domain.Models.Companies.CompanyExpectedMargin());
            Assert.AreEqual(5, companyExpectedMargin.Count);
        }

        //[TestMethod]
        //public void FetchCompanyExpectedMarginListWithNullSearchModel()
        //{
        //    var companyExpectedMargin = companyExpectedMarginRepository.Search(null);
        //    Assert.AreEqual(5, companyExpectedMargin.Count);
        //}

        [TestMethod]
        public void FetchCompanyExpectedMarginListByCompanyCode()
        {
            var companyExpectedMargin = companyExpectedMarginRepository.Search(new Company.Domain.Models.Companies.CompanyExpectedMargin() { CompanyCode = "UK050" });
            Assert.AreEqual(2, companyExpectedMargin.Count);
        }

        [TestMethod]
        public void FetchCompanyExpectedMarginListByExpectedMarginName()
        {
            var companyExpectedMargin = companyExpectedMarginRepository.Search(new Company.Domain.Models.Companies.CompanyExpectedMargin() { MarginType= "TIS (Technical Inspection Services)" });
            Assert.AreEqual(2, companyExpectedMargin.Count);
        }

        [TestMethod]
        public void FetchCompanyExpectedMarginListByExpectedMarginNameWildCardSerach()
        {
            var companyExpectedMargin = companyExpectedMarginRepository.Search(new Company.Domain.Models.Companies.CompanyExpectedMargin() { MarginType = "*Technical*" });
            Assert.AreEqual(4, companyExpectedMargin.Count);
        }

        [TestMethod]
        public void FetchCompanyExpectedMarginListByMarginTypeAndCompanyCode()
        {
            var companyExpectedMargin = companyExpectedMarginRepository.Search(new Company.Domain.Models.Companies.CompanyExpectedMargin() { MarginType = "AIM (Asset Integrity Management)", CompanyCode= "MX167" });
            Assert.AreEqual(1, companyExpectedMargin.Count);
        }

        [TestMethod]
        public void FetchCompanyExpectedMarginListByMinimumMarginEqualTo15()
        {
            var companyExpectedMargin = companyExpectedMarginRepository.Search(new Company.Domain.Models.Companies.CompanyExpectedMargin() {  MinimumMargin = 15 });
            Assert.AreEqual(1, companyExpectedMargin.Count);
        }

        [TestMethod]
        public void FetchCompanyExpectedMarginListByWrongSearchInput()
        {
            var companyExpectedMargin = companyExpectedMarginRepository.Search(new Company.Domain.Models.Companies.CompanyExpectedMargin() { CompanyCode = "UK050", MarginType = "AMM(Asset Management)" });
            Assert.AreEqual(0, companyExpectedMargin.Count);
        }

        [TestMethod]
        public void FetchCompanyExpectedMarginListWithCompanyCodeEmptystring()
        {
            var companyExpectedMargin = companyExpectedMarginRepository.Search(new Company.Domain.Models.Companies.CompanyExpectedMargin() { CompanyCode = string.Empty });
            Assert.AreEqual(5, companyExpectedMargin.Count);
        }

        //[TestMethod]
        //public void FetchCompanyExpectedMarginListWithSpaceInCompanyCodeAndMarginType()
        //{
        //    var companyExpectedMargin = companyExpectedMarginRepository.Search(new Company.Domain.Models.Companies.CompanyExpectedMargin() { CompanyCode = " ", MarginType = " " });
        //    Assert.AreEqual(5, companyExpectedMargin.Count);
        //}
         
        [TestMethod]
        public void AddCompanyExpectedMargin()
        {
            var newCompanyExpectedMarginData = mockData.First();
            var companyExpectedMargin = companyExpectedMarginRepository.Add(newCompanyExpectedMarginData);

            mockDbSet.Verify(m => m.Add(It.IsAny<DbModel.CompanyExpectedMargin>()), Times.Once);
            mockContext.Verify(m => m.SaveChanges(), Times.Once);
        }

        [TestMethod]
        public void AddListOfCompanyExpectedMargins()
        {
            var newCompanyExpectedMarginData = mockData.ToList();
            companyExpectedMarginRepository.Add(newCompanyExpectedMarginData);

            mockDbSet.Verify(m => m.AddRange(It.IsAny<IList<DbModel.CompanyExpectedMargin>>()), Times.Once);
            mockContext.Verify(m => m.SaveChanges(), Times.AtLeastOnce);
        }

        [TestMethod]
        public void CompanyExpectedMarginUpdate()
        {
            var newCompanyExpectedMarginData = mockData.First();
            companyExpectedMarginRepository.Update(newCompanyExpectedMarginData);

            mockDbSet.Verify(m => m.Update(It.IsAny<DbModel.CompanyExpectedMargin>()), Times.Once);
            mockContext.Verify(m => m.SaveChanges(), Times.Once);
        }

        [TestMethod]
        public void CompanyExpectedMarginDelete()
        {
            var newCompanyExpectedMarginData = mockData.First();
            companyExpectedMarginRepository.Delete(newCompanyExpectedMarginData);

            mockDbSet.Verify(m => m.Remove(It.IsAny<DbModel.CompanyExpectedMargin>()), Times.Once);
            mockContext.Verify(m => m.SaveChanges(), Times.Once);
        }

        [TestMethod]
        public void CompanyExpectedMarginDeleteBasedOnExpression()
        {
            companyExpectedMarginRepository.Delete(x => x.CompanyId == 116);

            mockContext.Verify(m => m.RemoveRange(It.IsAny<IQueryable<DbModel.CompanyExpectedMargin>>()), Times.AtLeastOnce);
            mockContext.Verify(m => m.SaveChanges(), Times.AtLeastOnce);
        }

        [TestMethod]
        public void ForceSaveDataShouldFailIfAutoSaveIsTrue()
        {
            companyExpectedMarginRepository.AutoSave = true;
            companyExpectedMarginRepository.ForceSave();
            companyExpectedMarginRepository.AutoSave = false;
            mockContext.Verify(m => m.SaveChanges(), Times.Never);
        }

        [TestMethod]
        public void ForceSaveDataShouldPassIfAutoSaveIsFalse()
        {
            companyExpectedMarginRepository.AutoSave = false;
            companyExpectedMarginRepository.ForceSave();

            mockContext.Verify(m => m.SaveChanges(), Times.Once);
        }
    }
}
