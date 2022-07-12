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
    public class CompanyDocumentRepositoryUnitTestCase : BaseTestCase
    {
        DomainData.ICompanyDocumentRepository companyDocumentRepository = null;
        Mock<DbSet<DbModel.CompanyDocument>> mockDbSet = null;
        IQueryable<DbModel.CompanyDocument> mockData = null;

        [TestInitialize]
        public void InitializecompanyDocumentRepository()
        {
            mockData = MockCompany.GetCompanyDocumentMockData();
            mockDbSet = MockCompany.GetCompanyDocumentMockDbSet(mockData);

            mockContext.Setup(c => c.Set<DbModel.CompanyDocument>()).Returns(mockDbSet.Object);
            mockContext.Setup(c => c.CompanyDocument).Returns(mockDbSet.Object);
            companyDocumentRepository = new Company.Infrastructure.Data.CompanyDocumentRepository(Mapper.Instance, mockContext.Object);
        }

        [TestMethod]
        public void GetAllCompanyDocumentList()
        {
            var companyDocument = companyDocumentRepository.GetAll().ToList();
            Assert.AreEqual(4, companyDocument.Count);
        }

        [TestMethod]
        public void FindByCompanyDocumentList()
        {
            var companyDocument = companyDocumentRepository.FindBy(x=>x.Name== "Invoice").ToList();
            Assert.AreEqual(1, companyDocument.Count);
        }

        [TestMethod]
        public void FindByCompanyDocumentListWithNull()
        {
            var companyDocument = companyDocumentRepository.FindBy(null).ToList();
            Assert.AreEqual(4, companyDocument.Count);
        }

        [TestMethod]
        public void CheckCompanyDocumentExistsWithDocumentName()
        {
            var companyDocument = companyDocumentRepository.Exists(x => x.Name == "Invoice").Result;
            Assert.AreEqual(true, companyDocument);
        }

        [TestMethod]
        public void FetchCompanyDocumentListWithoutSearchValue()
        {
            var companyDocument = companyDocumentRepository.Search(new Company.Domain.Models.Companies.CompanyDocument());
            Assert.AreEqual(4, companyDocument.Count);
        }

        //[TestMethod]
        //public void FetchCompanyDocumentListWithNullSearchModel()
        //{
        //    var companyDocument = companyDocumentRepository.Search(null);
        //    Assert.AreEqual(4, companyDocument.Count);
        //}

        [TestMethod]
        public void FetchCompanyDocumentListByCompanyCode()
        {
            var companyDocument = companyDocumentRepository.Search(new Company.Domain.Models.Companies.CompanyDocument() { CompanyCode = "UK050" });
            Assert.AreEqual(1, companyDocument.Count);
        }

        [TestMethod]
        public void FetchCompanyDocumentListByDocumentName()
        {
            var companyDocument = companyDocumentRepository.Search(new Company.Domain.Models.Companies.CompanyDocument() {  Name = "Invoice" });
            Assert.AreEqual(1, companyDocument.Count);
        }
        [TestMethod]
        public void FetchCompanyDocumentListByDocumentNameWildCardSerach()
        {
            var companyDocument = companyDocumentRepository.Search(new Company.Domain.Models.Companies.CompanyDocument() { Name = "Contract*" });
            Assert.AreEqual(1, companyDocument.Count);
        }
          
        [TestMethod]
        public void FetchCompanyDocumentListByDocumentType()
        {
            var companyDocument = companyDocumentRepository.Search(new Company.Domain.Models.Companies.CompanyDocument() { DocumentType= "Email" });
            Assert.AreEqual(2, companyDocument.Count);
        }

        [TestMethod]
        public void FetchCompanyDocumentListByIsVisibleToCustomer()
        {
            var companyDocument = companyDocumentRepository.Search(new Company.Domain.Models.Companies.CompanyDocument() {  IsVisibleToCustomer =true });
            Assert.AreEqual(3, companyDocument.Count);
        }

        [TestMethod]
        public void FetchCompanyDocumentListByCompanyCodeAndDocumentName()
        {
            var companyDocument = companyDocumentRepository.Search(new Company.Domain.Models.Companies.CompanyDocument() { CompanyCode = "UK051", Name = "Assignment File" });
            Assert.AreEqual(1, companyDocument.Count);
        }

        [TestMethod]
        public void FetchCompanyDocumentListByWrongCompanyCodeAndDocumentName()
        {
            var companyDocument = companyDocumentRepository.Search(new Company.Domain.Models.Companies.CompanyDocument() { CompanyCode = "UK0511", Name = "PS1" });
            Assert.AreEqual(0, companyDocument.Count);
        }

        [TestMethod]
        public void FetchCompanyDocumentListWithCompanyCodeEmptystring()
        {
            var companyDocument = companyDocumentRepository.Search(new Company.Domain.Models.Companies.CompanyDocument() { CompanyCode = string.Empty });
            Assert.AreEqual(4, companyDocument.Count);
        }

        //[TestMethod]
        //public void FetchCompanyDocumentListWithSpaceInCompanyCodeAndDocumentName()
        //{
        //    var companyDocument = companyDocumentRepository.Search(new Company.Domain.Models.Companies.CompanyDocument() { CompanyCode = " ", Name = " " });
        //    Assert.AreEqual(4, companyDocument.Count);
        //}

        [TestMethod]
        public void AddCompanyDocument()
        {
            var newCompanyDocumentData = mockData.First();
            var companyDocument = companyDocumentRepository.Add(newCompanyDocumentData);

            mockDbSet.Verify(m => m.Add(It.IsAny<DbModel.CompanyDocument>()), Times.Once);
            mockContext.Verify(m => m.SaveChanges(), Times.Once);
        }

        [TestMethod]
        public void AddListOfCompanyDocuments()
        {
            var newCompanyDocumentData = mockData.ToList();
            companyDocumentRepository.Add(newCompanyDocumentData);

            mockDbSet.Verify(m => m.AddRange(It.IsAny<IList<DbModel.CompanyDocument>>()), Times.Once);
            mockContext.Verify(m => m.SaveChanges(), Times.AtLeastOnce);
        }

        [TestMethod]
        public void CompanyDocumentUpdate()
        {
            var newCompanyDocumentData = mockData.First();
            companyDocumentRepository.Update(newCompanyDocumentData);

            mockDbSet.Verify(m => m.Update(It.IsAny<DbModel.CompanyDocument>()), Times.Once);
            mockContext.Verify(m => m.SaveChanges(), Times.Once);
        }

        [TestMethod]
        public void CompanyDocumentDelete()
        {
            var newCompanyDocumentData = mockData.First();
            companyDocumentRepository.Delete(newCompanyDocumentData);

            mockDbSet.Verify(m => m.Remove(It.IsAny<DbModel.CompanyDocument>()), Times.Once);
            mockContext.Verify(m => m.SaveChanges(), Times.Once);
        }

        [TestMethod]
        public void CompanyDocumentDeleteBasedOnExpression()
        {
            companyDocumentRepository.Delete(x => x.CompanyId == 116);

            mockContext.Verify(m => m.RemoveRange(It.IsAny<IQueryable<DbModel.CompanyDocument>>()), Times.AtLeastOnce);
            mockContext.Verify(m => m.SaveChanges(), Times.AtLeastOnce);
        }

        [TestMethod]
        public void ForceSaveDataShouldFailIfAutoSaveIsTrue()
        {
            companyDocumentRepository.AutoSave = true;
            companyDocumentRepository.ForceSave();
            companyDocumentRepository.AutoSave = false;
            mockContext.Verify(m => m.SaveChanges(), Times.Never);
        }

        [TestMethod]
        public void ForceSaveDataShouldPassIfAutoSaveIsFalse()
        {
            companyDocumentRepository.AutoSave = false;
            companyDocumentRepository.ForceSave();

            mockContext.Verify(m => m.SaveChanges(), Times.Once);
        }
    }
}
