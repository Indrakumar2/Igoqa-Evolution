using AutoMapper;
using Evolution.UnitTest.UnitTestCases;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Linq;
using DbModel = Evolution.DbRepository.Models;
using DomainData = Evolution.Company.Domain.Interfaces.Data;
using Evolution.UnitTests.Mocks.Data.Companies.Db;

namespace Evolution.UnitTests.UnitTestCases.Companies.Repositories
{
    [TestClass]
    public class CompanyCostCenterRepositoryUnitTestCase : BaseTestCase
    {
        DomainData.ICompanyCostCenterRepository companyCostCenterRepository = null;
        Mock<DbSet<DbModel.CompanyDivisionCostCenter>> mockDbSet = null;
        IQueryable<DbModel.CompanyDivisionCostCenter> mockData = null;

        [TestInitialize]
        public void InitializecompanyCostCenterRepository()
        {
            mockData = MockCompany.GetCompanyDivisionCostCenterMockData();
            mockDbSet = MockCompany.GetCompanyDivisionCostCenterMockDbSet(mockData);

            mockContext.Setup(c => c.Set<DbModel.CompanyDivisionCostCenter>()).Returns(mockDbSet.Object);
            mockContext.Setup(c => c.CompanyDivisionCostCenter).Returns(mockDbSet.Object);
            companyCostCenterRepository = new Company.Infrastructure.Data.CompanyDivisionCOCRepository(Mapper.Instance, mockContext.Object);
        }
         
        [TestMethod]
        public void GetAllCompanyDocumentList()
        {
            var companyCostCenter = companyCostCenterRepository.GetAll().ToList();
            Assert.AreEqual(3, companyCostCenter.Count);
        }

        [TestMethod]
        public void FindByCompanyCostCenterList()
        {
            var companyCostCenter = companyCostCenterRepository.FindBy(x => x.Name == "Azerbaijan").ToList();
            Assert.AreEqual(2, companyCostCenter.Count);
        }

        [TestMethod]
        public void FindByCompanyCostCenterListWithNull()
        {
            var companyCostCenter = companyCostCenterRepository.FindBy(null).ToList();
            Assert.AreEqual(3, companyCostCenter.Count);
        }

        [TestMethod]
        public void CheckCompanyCostCenterExistsWithCostCenterName()
        {
            var companyCostCenter = companyCostCenterRepository.Exists(x => x.Name =="Azerbaijan").Result;
            Assert.AreEqual(true, companyCostCenter);
        }
         
        [TestMethod]
        public void FetchCompanyCostCenterListWithoutSearchValue()
        {
            var companyCostCenter = companyCostCenterRepository.Search(new Company.Domain.Models.Companies.CompanyDivisionCostCenter());
            Assert.AreEqual(3, companyCostCenter.Count);
        }

        //[TestMethod]
        //public void FetchCompanyCostCenterListWithNullSearchModel()
        //{
        //    var companyCostCenter = companyCostCenterRepository.Search(null);
        //    Assert.AreEqual(3, companyCostCenter.Count);
        //}

        [TestMethod]
        public void FetchCompanyCostCenterListByCompanyCode()
        {
            var companyCostCenter = companyCostCenterRepository.Search(new Company.Domain.Models.Companies.CompanyDivisionCostCenter() { CompanyCode = "UK050" });
            Assert.AreEqual(2, companyCostCenter.Count);
        }

        [TestMethod]
        public void FetchCompanyCostCenterListByCostCenterCode()
        {
            var companyCostCenter = companyCostCenterRepository.Search(new Company.Domain.Models.Companies.CompanyDivisionCostCenter() { CostCenterCode = "CC4" });
            Assert.AreEqual(1, companyCostCenter.Count);
        }

        [TestMethod]
        public void FetchCompanyCostCenterListByCostCenterName()
        {
            var companyCostCenter = companyCostCenterRepository.Search(new Company.Domain.Models.Companies.CompanyDivisionCostCenter() { CostCenterName = "Azerbaijan" });
            Assert.AreEqual(2, companyCostCenter.Count);
        }

        [TestMethod]
        public void FetchCompanyCostCenterListByDivisionName()
        {
            var companyCostCenter = companyCostCenterRepository.Search(new Company.Domain.Models.Companies.CompanyDivisionCostCenter() {  Division = "PSO" });
            Assert.AreEqual(2, companyCostCenter.Count);
        }

        [TestMethod]
        public void FetchCompanyCostCenterListByCostCenterNameWildCardSearch()
        {
            var companyCostCenter = companyCostCenterRepository.Search(new Company.Domain.Models.Companies.CompanyDivisionCostCenter() {  CostCenterName = "Azerbai*" });
            Assert.AreEqual(2, companyCostCenter.Count);
        }

        [TestMethod]
        public void FetchCompanyCostCenterListByCompanyCodeAndDivisionName()
        {
            var companyCostCenter = companyCostCenterRepository.Search(new Company.Domain.Models.Companies.CompanyDivisionCostCenter() { CompanyCode = "UK050",  Division = "PSO" });
            Assert.AreEqual(1, companyCostCenter.Count);
        }

        [TestMethod]
        public void FetchCompanCostCenterListWithCompanyCodeEmptystring()
        {
            var companyCostCenter = companyCostCenterRepository.Search(new Company.Domain.Models.Companies.CompanyDivisionCostCenter() { CompanyCode = string.Empty });
            Assert.AreEqual(3, companyCostCenter.Count);
        }

        //[TestMethod]
        //public void FetchCompanyCostCenterListWithSpaceInCompanyCodeAndDivision()
        //{
        //    var companyCostCenter = companyCostCenterRepository.Search(new Company.Domain.Models.Companies.CompanyDivisionCostCenter() { CompanyCode = " ", Division = " " });
        //    Assert.AreEqual(3, companyCostCenter.Count);
        //}


        [TestMethod]
        public void AddCompanyCostCenter()
        {
            var newCompanyCostCenterData = mockData.First();
            var companyCostCenter = companyCostCenterRepository.Add(newCompanyCostCenterData);

            mockDbSet.Verify(m => m.Add(It.IsAny<DbModel.CompanyDivisionCostCenter>()), Times.Once);
            mockContext.Verify(m => m.SaveChanges(), Times.Once);
        }

        [TestMethod]
        public void AddListOfCompanyCostCenteres()
        {
            var newCompanyCostCenterData = mockData.ToList();
            companyCostCenterRepository.Add(newCompanyCostCenterData);

            mockDbSet.Verify(m => m.AddRange(It.IsAny<IList<DbModel.CompanyDivisionCostCenter>>()), Times.Once);
            mockContext.Verify(m => m.SaveChanges(), Times.AtLeastOnce);
        }

        [TestMethod]
        public void CompanyCostCenterUpdate()
        {
            var newCompanyCostCenterData = mockData.First();
            companyCostCenterRepository.Update(newCompanyCostCenterData);

            mockDbSet.Verify(m => m.Update(It.IsAny<DbModel.CompanyDivisionCostCenter>()), Times.Once);
            mockContext.Verify(m => m.SaveChanges(), Times.Once);
        }

        [TestMethod]
        public void CompanyCostCenterDelete()
        {
            var newCompanyCostCenterData = mockData.First();
            companyCostCenterRepository.Delete(newCompanyCostCenterData);

            mockDbSet.Verify(m => m.Remove(It.IsAny<DbModel.CompanyDivisionCostCenter>()), Times.Once);
            mockContext.Verify(m => m.SaveChanges(), Times.Once);
        }

        [TestMethod]
        public void CompanyCostCenterDeleteBasedOnExpression()
        {
            companyCostCenterRepository.Delete(x => x.Code == "1");

            mockContext.Verify(m => m.RemoveRange(It.IsAny<IQueryable<DbModel.CompanyDivisionCostCenter>>()), Times.AtLeastOnce);
            mockContext.Verify(m => m.SaveChanges(), Times.AtLeastOnce);
        }

        [TestMethod]
        public void ForceSaveDataShouldFailIfAutoSaveIsTrue()
        {
            companyCostCenterRepository.AutoSave = true;
            companyCostCenterRepository.ForceSave();
            companyCostCenterRepository.AutoSave = false;
            mockContext.Verify(m => m.SaveChanges(), Times.Never);
        }

        [TestMethod]
        public void ForceSaveDataShouldPassIfAutoSaveIsFalse()
        {
            companyCostCenterRepository.AutoSave = false;
            companyCostCenterRepository.ForceSave();

            mockContext.Verify(m => m.SaveChanges(), Times.Once);
        }
    }
}
