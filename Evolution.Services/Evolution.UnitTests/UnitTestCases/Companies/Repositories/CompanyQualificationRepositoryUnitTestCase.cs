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
    public class CompanyQualificationRepositoryUnitTestCase : BaseTestCase
    {
        DomainData.ICompanyQualificationRepository companyQualificationRepository = null;
        Mock<DbSet<DbModel.CompanyQualificationType>> mockDbSet = null;
        IQueryable<DbModel.CompanyQualificationType> mockData = null;

        [TestInitialize]
        public void InitializeCompanyQualificationRepository()
        {
            mockData = MockCompany.GetCompanyQualificationMockData();
            mockDbSet = MockCompany.GetCompanyQualificationMockDbSet(mockData);

            mockContext.Setup(c => c.Set<DbModel.CompanyQualificationType>()).Returns(mockDbSet.Object);
            mockContext.Setup(c => c.CompanyQualificationType).Returns(mockDbSet.Object);
            companyQualificationRepository = new Company.Infrastructure.Data.CompanyQualificationRepository(Mapper.Instance, mockContext.Object);
        }

        [TestMethod]
        public void GetAllCompanyQualificationList()
        {
            var companyQualification = companyQualificationRepository.GetAll().ToList();
            Assert.AreEqual(4, companyQualification.Count);
        }

        [TestMethod]
        public void FindByCompanyQualificationListWithCompanyQualificationTypeId()
        {
            var companyQualification = companyQualificationRepository.FindBy(x => x.QualificationTypeId == 325).ToList();
            Assert.AreEqual(1, companyQualification.Count);
        }

        [TestMethod]
        public void FindByCompanyQualificationListWithNull()
        {
            var companyQualification = companyQualificationRepository.FindBy(null).ToList();
            Assert.AreEqual(4, companyQualification.Count);
        }

        [TestMethod]
        public void CheckCompanyQualificationExistsWithQualificationTypeId()
        {
            var companyQualification = companyQualificationRepository.Exists(x => x.QualificationTypeId == 325).Result;
            Assert.AreEqual(true, companyQualification);
        }

        [TestMethod]
        public void FetchCompanyQualificationListWithoutSearchValue()
        {
            var companyQualification = companyQualificationRepository.Search(new Company.Domain.Models.Companies.CompanyQualification());
            Assert.AreEqual(4, companyQualification.Count);
        }

        //[TestMethod]
        //public void FetchCompanyQualificationListWithNullSearchModel()
        //{
        //    var companyQualification = companyQualificationRepository.Search(null);
        //    Assert.AreEqual(4, companyQualification.Count);
        //}

        [TestMethod]
        public void FetchCompanyQualificationListByCompanyCode()
        {
            var companyQualification = companyQualificationRepository.Search(new Company.Domain.Models.Companies.CompanyQualification() { CompanyCode = "UK050" });
            Assert.AreEqual(4, companyQualification.Count);
        }

        [TestMethod]
        public void FetchCompanyQualificationListByQualificationTypeName()
        {
            var companyQualification = companyQualificationRepository.Search(new Company.Domain.Models.Companies.CompanyQualification() {  Qualification = "City & Guild Craft - Mechnical" });
            Assert.AreEqual(1, companyQualification.Count);
        }

        [TestMethod]
        public void FetchCompanyQualificationListByQualificationWildCardSerach()
        {
            var companyQualification = companyQualificationRepository.Search(new Company.Domain.Models.Companies.CompanyQualification() { Qualification= "City & Guild *" });
            Assert.AreEqual(2, companyQualification.Count);
        }
         
        [TestMethod]
        public void FetchCompanyQualificationListByCompanyCodeAndQualification()
        {
            var companyQualification = companyQualificationRepository.Search(new Company.Domain.Models.Companies.CompanyQualification() { CompanyCode = "UK050", Qualification = "City & Guild Craft - Mechnical" });
            Assert.AreEqual(1, companyQualification.Count);
        }

        [TestMethod]
        public void FetchCompanyQualificationListByWrongCompanyCodeAndQualification()
        {
            var companyQualification = companyQualificationRepository.Search(new Company.Domain.Models.Companies.CompanyQualification() { CompanyCode = "UK058", Qualification = "City & Guild Craft - Mechnical" });
            Assert.AreEqual(0, companyQualification.Count);
        }

        [TestMethod]
        public void FetchCompanyQualificationListWithQualificationEmptystring()
        {
            var companyQualification = companyQualificationRepository.Search(new Company.Domain.Models.Companies.CompanyQualification() { CompanyCode=string.Empty,   Qualification = string.Empty });
            Assert.AreEqual(4, companyQualification.Count);
        }

        [TestMethod]
        public void FetchCompanyQualificationListWithCompanyCodeEmptystring()
        {
            var companyQualification = companyQualificationRepository.Search(new Company.Domain.Models.Companies.CompanyQualification() { CompanyCode = string.Empty });
            Assert.AreEqual(4, companyQualification.Count);
        }

        //[TestMethod]
        //public void FetchCompanyQualificationListWithSpaceInCompanyCodeAndQualification()
        //{
        //    var companyQualification = companyQualificationRepository.Search(new Company.Domain.Models.Companies.CompanyQualification() { CompanyCode = " ", Qualification = " "});
        //    Assert.AreEqual(4, companyQualification.Count);
        //}
         
        [TestMethod]
        public void AddCompanyQualification()
        {
            var newCompanyQualificationData = mockData.First();
            var companyQualification = companyQualificationRepository.Add(newCompanyQualificationData);

            mockDbSet.Verify(m => m.Add(It.IsAny<DbModel.CompanyQualificationType>()), Times.Once);
            mockContext.Verify(m => m.SaveChanges(), Times.Once);
        }

        [TestMethod]
        public void AddListOfCompanyQualifications()
        {
            var newCompanyQualificationData = mockData.ToList();
            companyQualificationRepository.Add(newCompanyQualificationData);

            mockDbSet.Verify(m => m.AddRange(It.IsAny<IList<DbModel.CompanyQualificationType>>()), Times.Once);
            mockContext.Verify(m => m.SaveChanges(), Times.AtLeastOnce);
        }

        [TestMethod]
        public void CompanyQualificationUpdate()
        {
            var newCompanyQualificationData = mockData.First();
            companyQualificationRepository.Update(newCompanyQualificationData);

            mockDbSet.Verify(m => m.Update(It.IsAny<DbModel.CompanyQualificationType>()), Times.Once);
            mockContext.Verify(m => m.SaveChanges(), Times.Once);
        }

        [TestMethod]
        public void CompanyQualificationDelete()
        {
            var newCompanyQualificationData = mockData.First();
            companyQualificationRepository.Delete(newCompanyQualificationData);

            mockDbSet.Verify(m => m.Remove(It.IsAny<DbModel.CompanyQualificationType>()), Times.Once);
            mockContext.Verify(m => m.SaveChanges(), Times.Once);
        }

        [TestMethod]
        public void CompanyQualificationDeleteBasedOnExpression()
        {
            companyQualificationRepository.Delete(x => x.Id == 1);

            mockContext.Verify(m => m.RemoveRange(It.IsAny<IQueryable<DbModel.CompanyQualificationType>>()), Times.AtLeastOnce);
            mockContext.Verify(m => m.SaveChanges(), Times.AtLeastOnce);
        }

        [TestMethod]
        public void ForceSaveDataShouldFailIfAutoSaveIsTrue()
        {
            companyQualificationRepository.AutoSave = true;
            companyQualificationRepository.ForceSave();
            companyQualificationRepository.AutoSave = false;
            mockContext.Verify(m => m.SaveChanges(), Times.Never);
        }

        [TestMethod]
        public void ForceSaveDataShouldPassIfAutoSaveIsFalse()
        {
            companyQualificationRepository.AutoSave = false;
            companyQualificationRepository.ForceSave();

            mockContext.Verify(m => m.SaveChanges(), Times.Once);
        }
    }
}
