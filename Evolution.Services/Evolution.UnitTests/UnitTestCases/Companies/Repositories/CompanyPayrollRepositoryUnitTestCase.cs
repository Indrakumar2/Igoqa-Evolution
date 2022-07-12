using Evolution.UnitTest.UnitTestCases;
using System;
using System.Collections.Generic;
using System.Text;
using DomainData = Evolution.Company.Domain.Interfaces.Data;
using DbModel = Evolution.DbRepository.Models;
using Moq;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Evolution.UnitTests.Mocks.Data.Companies.Db;
using AutoMapper;
using System.Linq;

namespace Evolution.UnitTests.UnitTestCases.Companies.Repositories
{
    [TestClass]
   public class CompanyPayrollRepositoryUnitTestCase : BaseTestCase
    {
        DomainData.ICompanyPayrollRepository companyPayrollRepository = null;
        Mock<DbSet<DbModel.CompanyPayroll>> mockDbSet = null;
        IQueryable<DbModel.CompanyPayroll> mockData = null;

        [TestInitialize]
        public void InitializeCompanyPayrollRepository()
        {
            mockData = MockCompany.GetCompanyPayrollMockData();
            mockDbSet = MockCompany.GetCompanyPayrollMockDbSet(mockData);

            mockContext.Setup(c => c.Set<DbModel.CompanyPayroll>()).Returns(mockDbSet.Object);
            mockContext.Setup(c => c.CompanyPayroll).Returns(mockDbSet.Object);
            companyPayrollRepository = new Company.Infrastructure.Data.CompanyPayrollRepository(Mapper.Instance, mockContext.Object);
        }
         
        [TestMethod]
        public void GetAllCompanyPayrollList()
        {
            var companyPayroll = companyPayrollRepository.GetAll().ToList();
            Assert.AreEqual(3, companyPayroll.Count);
        }

        [TestMethod]
        public void FindByCompanyPayrollListWithCompanyPayrollTypeId()
        {
            var companyPayroll = companyPayrollRepository.FindBy(x => x.PayrollTypeId == 2898).ToList();
            Assert.AreEqual(2, companyPayroll.Count);
        }

        [TestMethod]
        public void FindByCompanyPayrollListWithNull()
        {
            var companyPayroll = companyPayrollRepository.FindBy(null).ToList();
            Assert.AreEqual(3, companyPayroll.Count);
        }

        [TestMethod]
        public void CheckCompanyPayrollExistsWithPayrollTypeId()
        {
            var companyPayroll = companyPayrollRepository.Exists(x => x.PayrollTypeId == 2898).Result;
            Assert.AreEqual(true, companyPayroll);
        }

        [TestMethod]
        public void FetchCompanyPayrollListWithoutSearchValue()
        {
            var companyPayroll = companyPayrollRepository.Search(new Company.Domain.Models.Companies.CompanyPayroll());
            Assert.AreEqual(3, companyPayroll.Count);
        }

        //[TestMethod]
        //public void FetchCompanyPayrollListWithNullSearchModel()
        //{
        //    var companyPayroll = companyPayrollRepository.Search(null);
        //    Assert.AreEqual(3, companyPayroll.Count);
        //}

        [TestMethod]
        public void FetchCompanyPayrollListByCompanyCode()
        {
            var companyPayroll = companyPayrollRepository.Search(new Company.Domain.Models.Companies.CompanyPayroll() { CompanyCode = "UK050" });
            Assert.AreEqual(2, companyPayroll.Count);
        }

        [TestMethod]
        public void FetchCompanyPayrollListByPayrollTypeName()
        {
            var companyPayroll = companyPayrollRepository.Search(new Company.Domain.Models.Companies.CompanyPayroll() { PayrollType= "LTD/VAT/SE" });
            Assert.AreEqual(2, companyPayroll.Count);
        }

        [TestMethod]
        public void FetchCompanyPayrollListByPayrollWildCardSerach()
        {
            var companyPayroll = companyPayrollRepository.Search(new Company.Domain.Models.Companies.CompanyPayroll() { PayrollType = "PAYE*" });
            Assert.AreEqual(1, companyPayroll.Count);
        }


        [TestMethod]
        public void FetchCompanyPayrollListByCompanyCodeAndPayroll()
        {
            var companyPayroll = companyPayrollRepository.Search(new Company.Domain.Models.Companies.CompanyPayroll() { CompanyCode = "DZ", PayrollType = "LTD/VAT/SE" });
            Assert.AreEqual(1, companyPayroll.Count);
        }

        [TestMethod]
        public void FetchCompanyPayrollListByWrongCompanyCodeAndPayroll()
        {
            var companyPayroll = companyPayrollRepository.Search(new Company.Domain.Models.Companies.CompanyPayroll() { CompanyCode = "UK058", PayrollType = "PAYE Weekly" });
            Assert.AreEqual(0, companyPayroll.Count);
        }

        [TestMethod]
        public void FetchCompanyPayrollListWithCompanyCodeAndPayrollTypeAsEmptystring()
        {
            var companyPayroll = companyPayrollRepository.Search(new Company.Domain.Models.Companies.CompanyPayroll() { CompanyCode = string.Empty, PayrollType = string.Empty });
            Assert.AreEqual(3, companyPayroll.Count);
        }

        [TestMethod]
        public void FetchCompanyPayrollListWithCompanyCodeEmptystring()
        {
            var companyPayroll = companyPayrollRepository.Search(new Company.Domain.Models.Companies.CompanyPayroll() { CompanyCode = string.Empty });
            Assert.AreEqual(3, companyPayroll.Count);
        }

        //[TestMethod]
        //public void FetchCompanyPayrollListWithSpaceInCompanyCodeAndPayrollType()
        //{
        //    var companyPayroll = companyPayrollRepository.Search(new Company.Domain.Models.Companies.CompanyPayroll() { CompanyCode = " ", PayrollType = " " });
        //    Assert.AreEqual(3, companyPayroll.Count);
        //}
         
        [TestMethod]
        public void AddCompanyPayroll()
        {
            var newCompanyPayrollData = mockData.First();
            var companyPayroll = companyPayrollRepository.Add(newCompanyPayrollData);

            mockDbSet.Verify(m => m.Add(It.IsAny<DbModel.CompanyPayroll>()), Times.Once);
            mockContext.Verify(m => m.SaveChanges(), Times.Once);
        }

        [TestMethod]
        public void AddListOfCompanyPayrolls()
        {
            var newCompanyPayrollData = mockData.ToList();
            companyPayrollRepository.Add(newCompanyPayrollData);

            mockDbSet.Verify(m => m.AddRange(It.IsAny<IList<DbModel.CompanyPayroll>>()), Times.Once);
            mockContext.Verify(m => m.SaveChanges(), Times.AtLeastOnce);
        }

        [TestMethod]
        public void CompanyPayrollUpdate()
        {
            var newCompanyPayrollData = mockData.First();
            companyPayrollRepository.Update(newCompanyPayrollData);

            mockDbSet.Verify(m => m.Update(It.IsAny<DbModel.CompanyPayroll>()), Times.Once);
            mockContext.Verify(m => m.SaveChanges(), Times.Once);
        }

        [TestMethod]
        public void CompanyPayrollDelete()
        {
            var newCompanyPayrollData = mockData.First();
            companyPayrollRepository.Delete(newCompanyPayrollData);

            mockDbSet.Verify(m => m.Remove(It.IsAny<DbModel.CompanyPayroll>()), Times.Once);
            mockContext.Verify(m => m.SaveChanges(), Times.Once);
        }

        [TestMethod]
        public void CompanyPayrollDeleteBasedOnExpression()
        {
            companyPayrollRepository.Delete(x => x.Id == 66);

            mockContext.Verify(m => m.RemoveRange(It.IsAny<IQueryable<DbModel.CompanyPayroll>>()), Times.AtLeastOnce);
            mockContext.Verify(m => m.SaveChanges(), Times.AtLeastOnce);
        }

        [TestMethod]
        public void ForceSaveDataShouldFailIfAutoSaveIsTrue()
        {
            companyPayrollRepository.AutoSave = true;
            companyPayrollRepository.ForceSave();
            companyPayrollRepository.AutoSave = false;
            mockContext.Verify(m => m.SaveChanges(), Times.Never);
        }

        [TestMethod]
        public void ForceSaveDataShouldPassIfAutoSaveIsFalse()
        {
            companyPayrollRepository.AutoSave = false;
            companyPayrollRepository.ForceSave();

            mockContext.Verify(m => m.SaveChanges(), Times.Once);
        }
    }
}
