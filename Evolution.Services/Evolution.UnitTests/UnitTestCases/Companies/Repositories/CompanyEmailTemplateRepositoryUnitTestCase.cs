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
    public class CompanyEmailTemplateRepositoryUnitTestCase : BaseTestCase
    {
        DomainData.ICompanyEmailTemplateRepository companyEmailTemplateRepository = null;
        Mock<DbSet<DbModel.CompanyMessage>> emailTemplatesMockDbSet = null; 
        Mock<DbSet<DbModel.Company>> companyEmailTemplatesMockDbSet = null;
        IQueryable<DbModel.CompanyMessage> mockData = null;

        [TestInitialize]
        public void InitializeCompanyEmailTemplateRepository()
        {
            mockData = MockCompany.GetEmailTemplateMockData();
            emailTemplatesMockDbSet = MockCompany.GetEmailTemplateMockDbSet(mockData);
            companyEmailTemplatesMockDbSet = MockCompany.GetCompanyEmailTemplatesMockDbSet(MockCompany.GetCompanyEmailTemplatesMockData());

            mockContext.Setup(c => c.Set<DbModel.CompanyMessage>()).Returns(emailTemplatesMockDbSet.Object);
            mockContext.Setup(c => c.CompanyMessage).Returns(emailTemplatesMockDbSet.Object);

            mockContext.Setup(c => c.Set<DbModel.Company>()).Returns(companyEmailTemplatesMockDbSet.Object);
            mockContext.Setup(c => c.Company).Returns(companyEmailTemplatesMockDbSet.Object);
            companyEmailTemplateRepository = new Company.Infrastructure.Data.CompanyEmailTemplateRepository(Mapper.Instance, mockContext.Object);
        }

        [TestMethod]
        public void GetAllCompanyEmailTemplateList()
        {
            var companyEmailTemplate = companyEmailTemplateRepository.GetAll().ToList();
            Assert.AreEqual(4, companyEmailTemplate.Count);
        }

        [TestMethod]
        public void FindByCompanyEmailTemplateList()
        {
            var companyEmailTemplate = companyEmailTemplateRepository.FindBy(x => x.Company.Code == "UK050").ToList();
            Assert.AreEqual(4, companyEmailTemplate.Count);
        }

        [TestMethod]
        public void FindByCompanyEmailTemplateListWithNull()
        {
            var companyEmailTemplate = companyEmailTemplateRepository.FindBy(null).ToList();
            Assert.AreEqual(4, companyEmailTemplate.Count);
        }

        [TestMethod]
        public void CheckcompanyEmailTemplateExistsWithEmailTemplateName()
        {
            var companyEmailTemplate = companyEmailTemplateRepository.Exists(x => x.MessageType.Name == "EmailCustomerReportingNotification").Result;
            Assert.AreEqual(true, companyEmailTemplate);
        }

        //[TestMethod]
        //public void FetchcompanyEmailTemplateListWithoutSearchValue()
        //{
        //    var companyEmailTemplate = companyEmailTemplateRepository.Search("UK050");
        //    Assert.IsNull(companyEmailTemplate);
        //}

        //[TestMethod]
        //public void FetchcompanyEmailTemplateListWithNullSearchModel()
        //{
        //    var companyEmailTemplate = companyEmailTemplateRepository.Search(null);
        //    Assert.IsNull(companyEmailTemplate);
        //}

        [TestMethod]
        public void FetchcompanyEmailTemplateListByCompanyCode()
        {
            var companyEmailTemplate = companyEmailTemplateRepository.Search("UK050");
            Assert.IsNotNull(companyEmailTemplate); 
            Assert.AreEqual("UK050",companyEmailTemplate.CompanyCode);
        }

        //[TestMethod]
        //public void FetchcompanyEmailTemplateListWithCompanyCodeEmptystring()
        //{
        //    var companyEmailTemplate = companyEmailTemplateRepository.Search("UK050");
        //    Assert.IsNull(companyEmailTemplate);
        //}

        //[TestMethod]
        //public void FetchcompanyEmailTemplateListWithSpaceInCompanyCodeAndDivisionName()
        //{
        //    var companyEmailTemplate = companyEmailTemplateRepository.Search("UK050");
        //    Assert.IsNull(companyEmailTemplate);
        //}
         
        [TestMethod]
        public void AddCompanyEmailTemplate()
        {
            var newCompanyEmailTemplateData = mockData.First();
            var companyEmailTemplate = companyEmailTemplateRepository.Add(newCompanyEmailTemplateData);

            emailTemplatesMockDbSet.Verify(m => m.Add(It.IsAny<DbModel.CompanyMessage>()), Times.Once);
            mockContext.Verify(m => m.SaveChanges(), Times.Once);
        }

        [TestMethod]
        public void AddListOfCompanyEmailTemplates()
        {
            var newCompanyEmailTemplateData = mockData.ToList();
            companyEmailTemplateRepository.Add(newCompanyEmailTemplateData);

            emailTemplatesMockDbSet.Verify(m => m.AddRange(It.IsAny<IList<DbModel.CompanyMessage>>()), Times.Once);
            mockContext.Verify(m => m.SaveChanges(), Times.AtLeastOnce);
        }

        [TestMethod]
        public void CompanyEmailTemplateUpdate()
        {
            var newCompanyEmailTemplateData = mockData.First();
            companyEmailTemplateRepository.Update(newCompanyEmailTemplateData);

            emailTemplatesMockDbSet.Verify(m => m.Update(It.IsAny<DbModel.CompanyMessage>()), Times.Once);
            mockContext.Verify(m => m.SaveChanges(), Times.Once);
        }

        [TestMethod]
        public void CompanyEmailTemplateDelete()
        {
            var newCompanyEmailTemplateData = mockData.First();
            companyEmailTemplateRepository.Delete(newCompanyEmailTemplateData);

            emailTemplatesMockDbSet.Verify(m => m.Remove(It.IsAny<DbModel.CompanyMessage>()), Times.Once);
            mockContext.Verify(m => m.SaveChanges(), Times.Once);
        }

        [TestMethod]
        public void CompanyEmailTemplateDeleteBasedOnExpression()
        {
            companyEmailTemplateRepository.Delete(x => x.CompanyId == 116);

            mockContext.Verify(m => m.RemoveRange(It.IsAny<IQueryable<DbModel.CompanyMessage>>()), Times.AtLeastOnce);
            mockContext.Verify(m => m.SaveChanges(), Times.AtLeastOnce);
        }

        [TestMethod]
        public void ForceSaveDataShouldFailIfAutoSaveIsTrue()
        {
            companyEmailTemplateRepository.AutoSave = true;
            companyEmailTemplateRepository.ForceSave();
            companyEmailTemplateRepository.AutoSave = false;
            mockContext.Verify(m => m.SaveChanges(), Times.Never);
        }

        [TestMethod]
        public void ForceSaveDataShouldPassIfAutoSaveIsFalse()
        {
            companyEmailTemplateRepository.AutoSave = false;
            companyEmailTemplateRepository.ForceSave();

            mockContext.Verify(m => m.SaveChanges(), Times.Once);
        }
    }
}
