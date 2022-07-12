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
    public class CompanyDivisionRepositoryUnitTestCase : BaseTestCase
    {
        DomainData.ICompanyDivisionRepository companyDivisionRepository = null;
        Mock<DbSet<DbModel.CompanyDivision>> mockDbSet = null;
        IQueryable<DbModel.CompanyDivision> mockData = null;

        [TestInitialize]
        public void InitializecompanyDivisionRepository()
        {
            mockData = MockCompany.GetCompanyDivisionMockData();
            mockDbSet = MockCompany.GetCompanyDivisionMockDbSet(mockData);

            mockContext.Setup(c => c.Set<DbModel.CompanyDivision>()).Returns(mockDbSet.Object);
            mockContext.Setup(c => c.CompanyDivision).Returns(mockDbSet.Object); 
            companyDivisionRepository = new Company.Infrastructure.Data.CompanyDivisionRepository(Mapper.Instance, mockContext.Object);
        }
         
        [TestMethod]
        public void GetAllCompanyDocumentList()
        {
            var companyDivision = companyDivisionRepository.GetAll().ToList();
            Assert.AreEqual(3, companyDivision.Count);
        }

        [TestMethod]
        public void FindByCompanyCostCenterList()
        {
            var companyDivision = companyDivisionRepository.FindBy(x => x.Division.Name == "Inspection").ToList();
            Assert.AreEqual(2, companyDivision.Count);
        }

        [TestMethod]
        public void FindByCompanyCostCenterListWithNull()
        {
            var companyDivision = companyDivisionRepository.FindBy(null).ToList();
            Assert.AreEqual(3, companyDivision.Count);
        }

        [TestMethod]
        public void CheckCompanyDivisionExistsWithCostCenterName()
        {
            var companyDivision = companyDivisionRepository.Exists(x => x.Division.Name == "Inspection").Result;
            Assert.AreEqual(true, companyDivision);
        }
         
        [TestMethod]
        public void FetchCompanyDivisionListWithoutSearchValue()
        {
            var companyDivision = companyDivisionRepository.Search(new Company.Domain.Models.Companies.CompanyDivision());
            Assert.AreEqual(3, companyDivision.Count);
        }

        //[TestMethod]
        //public void FetchCompanyDivisionListWithNullSearchModel()
        //{
        //    var companyDivision = companyDivisionRepository.Search(null);
        //    Assert.AreEqual(3, companyDivision.Count);
        //}

        [TestMethod]
        public void FetchCompanyDivisionListByCompanyCode()
        {
            var companyDivision = companyDivisionRepository.Search(new Company.Domain.Models.Companies.CompanyDivision() { CompanyCode = "UK050" });
            Assert.AreEqual(2, companyDivision.Count);
        }
          
        [TestMethod]
        public void FetchCompanyDivisionListByDivisionName()
        {
            var companyDivision = companyDivisionRepository.Search(new Company.Domain.Models.Companies.CompanyDivision() { DivisionName = "Inspection" });
            Assert.AreEqual(2, companyDivision.Count);
        }
          
        [TestMethod]
        public void FetchCompanyDivisionListByDivisionNameWildCardSearch()
        {
            var companyDivision = companyDivisionRepository.Search(new Company.Domain.Models.Companies.CompanyDivision() { DivisionName = "*SO" });
            Assert.AreEqual(1, companyDivision.Count);
        }

        [TestMethod]
        public void FetchCompanyDivisionListByCompanyCodeAndDivisionName()
        {
            var companyDivision = companyDivisionRepository.Search(new Company.Domain.Models.Companies.CompanyDivision() { CompanyCode = "UK051",  DivisionName = "PSO" });
            Assert.AreEqual(1, companyDivision.Count);
        }

        [TestMethod]
        public void FetchCompanyDivisionListByWrongCompanyCodeAndDivisionName()
        {
            var companyDivision = companyDivisionRepository.Search(new Company.Domain.Models.Companies.CompanyDivision() { CompanyCode = "UK0511", DivisionName = "PS1" });
            Assert.AreEqual(0, companyDivision.Count);
        }

        [TestMethod]
        public void FetchCompanyDivisionListWithCompanyCodeEmptystring()
        {
            var companyDivision = companyDivisionRepository.Search(new Company.Domain.Models.Companies.CompanyDivision() { CompanyCode = string.Empty });
            Assert.AreEqual(3, companyDivision.Count);
        }

        //[TestMethod]
        //public void FetchCompanyDivisionListWithSpaceInCompanyCodeAndDivisionName()
        //{
        //    var companyDivision = companyDivisionRepository.Search(new Company.Domain.Models.Companies.CompanyDivision() { CompanyCode = " ", DivisionName = " " });
        //    Assert.AreEqual(3, companyDivision.Count);
        //}

        [TestMethod]
        public void AddCompanyDivision()
        {
            var newCompanyDivisionData = mockData.First();
            var companyDivision = companyDivisionRepository.Add(newCompanyDivisionData);

            mockDbSet.Verify(m => m.Add(It.IsAny<DbModel.CompanyDivision>()), Times.Once);
            mockContext.Verify(m => m.SaveChanges(), Times.Once);
        }

        [TestMethod]
        public void AddListOfCompanyDivisions()
        {
            var newCompanyDivisionData = mockData.ToList();
            companyDivisionRepository.Add(newCompanyDivisionData);

            mockDbSet.Verify(m => m.AddRange(It.IsAny<IList<DbModel.CompanyDivision>>()), Times.Once);
            mockContext.Verify(m => m.SaveChanges(), Times.AtLeastOnce);
        }

        [TestMethod]
        public void CompanyDivisionUpdate()
        {
            var newCompanyDivisionData = mockData.First();
            companyDivisionRepository.Update(newCompanyDivisionData);

            mockDbSet.Verify(m => m.Update(It.IsAny<DbModel.CompanyDivision>()), Times.Once);
            mockContext.Verify(m => m.SaveChanges(), Times.Once);
        }

        [TestMethod]
        public void CompanyDivisionDelete()
        {
            var newCompanyDivisionData = mockData.First();
            companyDivisionRepository.Delete(newCompanyDivisionData);

            mockDbSet.Verify(m => m.Remove(It.IsAny<DbModel.CompanyDivision>()), Times.Once);
            mockContext.Verify(m => m.SaveChanges(), Times.Once);
        }

        [TestMethod]
        public void CompanyDivisionDeleteBasedOnExpression()
        {
            companyDivisionRepository.Delete(x => x.CompanyId == 116);

            mockContext.Verify(m => m.RemoveRange(It.IsAny<IQueryable<DbModel.CompanyDivision>>()), Times.AtLeastOnce);
            mockContext.Verify(m => m.SaveChanges(), Times.AtLeastOnce);
        }

        [TestMethod]
        public void ForceSaveDataShouldFailIfAutoSaveIsTrue()
        {
            companyDivisionRepository.AutoSave = true;
            companyDivisionRepository.ForceSave();
            companyDivisionRepository.AutoSave = false;
            mockContext.Verify(m => m.SaveChanges(), Times.Never);
        }

        [TestMethod]
        public void ForceSaveDataShouldPassIfAutoSaveIsFalse()
        {
            companyDivisionRepository.AutoSave = false;
            companyDivisionRepository.ForceSave();

            mockContext.Verify(m => m.SaveChanges(), Times.Once);
        }
    }
}
