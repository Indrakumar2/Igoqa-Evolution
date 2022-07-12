using Evolution.UnitTest.UnitTestCases;
using System;
using System.Collections.Generic;
using System.Text;
using DomainData = Evolution.Company.Domain.Interfaces.Data;
using DbModel = Evolution.DbRepository.Models;
using Moq;
using Microsoft.EntityFrameworkCore;
using Evolution.UnitTests.Mocks.Data.Companies.Db;
using AutoMapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace Evolution.UnitTests.UnitTestCases.Companies.Repositories
{
    [TestClass]
    public class CompanyInvoiceRepositoryUnitTestCase : BaseTestCase
    {
        DomainData.ICompanyInvoiceRepository companyInvoiceRepository = null;
        Mock<DbSet<DbModel.CompanyMessage>> invoiceMessagesMockDbSet = null;
        Mock<DbSet<DbModel.Company>> companyInvoiceMessagesMockDbSet = null;
        IQueryable<DbModel.CompanyMessage> mockData = null;

        [TestInitialize]
        public void InitializeCompanyInvoiceRepository()
        {
            mockData = MockCompany.GetCompanyInvoiceMockData();
            invoiceMessagesMockDbSet = MockCompany.GetCompanyInvoiceMockDbSet(mockData);
            companyInvoiceMessagesMockDbSet= MockCompany.GetCompanyInvoiceMessagesMockDbSet(MockCompany.GetCompanyInvoiceMessagessMockData());


            mockContext.Setup(c => c.Set<DbModel.CompanyMessage>()).Returns(invoiceMessagesMockDbSet.Object);
            mockContext.Setup(c => c.CompanyMessage).Returns(invoiceMessagesMockDbSet.Object);

            mockContext.Setup(c => c.Set<DbModel.Company>()).Returns(companyInvoiceMessagesMockDbSet.Object);
            mockContext.Setup(c => c.Company).Returns(companyInvoiceMessagesMockDbSet.Object);

            companyInvoiceRepository = new Company.Infrastructure.Data.CompanyInvoiceRepository(Mapper.Instance, mockContext.Object);
        }

        [TestMethod]
        public void GetAllCompanyInvoiceList()
        {
            var companyInvoice = companyInvoiceRepository.GetAll().ToList();
            Assert.AreEqual(4, companyInvoice.Count);
        }


        [TestMethod]
        public void FindByCompanyInvoiceList()
        {
            var companyInvoice = companyInvoiceRepository.FindBy(x => x.Company.Code == "UK050").ToList();
            Assert.AreEqual(4, companyInvoice.Count);
        }

        [TestMethod]
        public void FindByCompanyInvoiceListWithNull()
        {
            var companyInvoice = companyInvoiceRepository.FindBy(null).ToList();
            Assert.AreEqual(4, companyInvoice.Count);
        }

        [TestMethod]
        public void CheckcompanyInvoiceExistsWithInvoiceName()
        {
            var companyInvoice = companyInvoiceRepository.Exists(x => x.MessageType.Name == "InvoiceRemittanceText").Result;
            Assert.AreEqual(true, companyInvoice);
        }

        [TestMethod]
        public void FetchcompanyInvoiceListWithoutSearchValue()
        {
            var companyInvoice = companyInvoiceRepository.Search("");

            Assert.IsNull(companyInvoice);
        }

        [TestMethod]
        public void FetchcompanyInvoiceListWithNullSearchModel()
        {
            var companyInvoice = companyInvoiceRepository.Search(null);
            Assert.IsNull(companyInvoice);
        }

        [TestMethod]
        public void FetchcompanyInvoiceListByCompanyCode()
        {
            var companyInvoice = companyInvoiceRepository.Search("UK050");
            Assert.IsNotNull(companyInvoice);
            Assert.AreEqual("UK050", companyInvoice.CompanyCode);
        }

        //[TestMethod]
        //public void FetchcompanyInvoiceListWithCompanyCodeEmptystring()
        //{
        //    var companyInvoice = companyInvoiceRepository.Search("UK050");
        //    Assert.IsNull(companyInvoice);
        //}

        //[TestMethod]
        //public void FetchcompanyInvoiceListWithSpaceInCompanyCodeAndDivisionName()
        //{
        //    var companyInvoice = companyInvoiceRepository.Search("UK050");
        //    Assert.IsNull(companyInvoice);
        //}
         
        [TestMethod]
        public void AddCompanyInvoice()
        {
            var newCompanyInvoiceData = mockData.First();
            var companyInvoice = companyInvoiceRepository.Add(newCompanyInvoiceData);

            invoiceMessagesMockDbSet.Verify(m => m.Add(It.IsAny<DbModel.CompanyMessage>()), Times.Once);
            mockContext.Verify(m => m.SaveChanges(), Times.Once);
        }

        [TestMethod]
        public void AddListOfCompanyInvoices()
        {
            var newCompanyInvoiceData = mockData.ToList();
            companyInvoiceRepository.Add(newCompanyInvoiceData);

            invoiceMessagesMockDbSet.Verify(m => m.AddRange(It.IsAny<IList<DbModel.CompanyMessage>>()), Times.Once);
            mockContext.Verify(m => m.SaveChanges(), Times.AtLeastOnce);
        }

        [TestMethod]
        public void CompanyInvoiceUpdate()
        {
            var newCompanyInvoiceData = mockData.First();
            companyInvoiceRepository.Update(newCompanyInvoiceData);

            invoiceMessagesMockDbSet.Verify(m => m.Update(It.IsAny<DbModel.CompanyMessage>()), Times.Once);
            mockContext.Verify(m => m.SaveChanges(), Times.Once);
        }

        [TestMethod]
        public void CompanyInvoiceDelete()
        {
            var newCompanyInvoiceData = mockData.First();
            companyInvoiceRepository.Delete(newCompanyInvoiceData);

            invoiceMessagesMockDbSet.Verify(m => m.Remove(It.IsAny<DbModel.CompanyMessage>()), Times.Once);
            mockContext.Verify(m => m.SaveChanges(), Times.Once);
        }

        [TestMethod]
        public void CompanyInvoiceDeleteBasedOnExpression()
        {
            companyInvoiceRepository.Delete(x => x.CompanyId == 116);

            mockContext.Verify(m => m.RemoveRange(It.IsAny<IQueryable<DbModel.CompanyMessage>>()), Times.AtLeastOnce);
            mockContext.Verify(m => m.SaveChanges(), Times.AtLeastOnce);
        }

        [TestMethod]
        public void ForceSaveDataShouldFailIfAutoSaveIsTrue()
        {
            companyInvoiceRepository.AutoSave = true;
            companyInvoiceRepository.ForceSave();
            companyInvoiceRepository.AutoSave = false;
            mockContext.Verify(m => m.SaveChanges(), Times.Never);
        }

        [TestMethod]
        public void ForceSaveDataShouldPassIfAutoSaveIsFalse()
        {
            companyInvoiceRepository.AutoSave = false;
            companyInvoiceRepository.ForceSave();

            mockContext.Verify(m => m.SaveChanges(), Times.Once);
        }
    }
}
