//using System;
//using System.Collections.Generic;
//using System.Text;
//using DomainData = Evolution.Contract.Domain.Interfaces.Data;
//using DbModel = Evolution.DbRepository.Models;
//using Moq;
//using Microsoft.EntityFrameworkCore;
//using Evolution.UnitTests.Mocks.Data.Contracts.Db;
//using AutoMapper;
//using System.Linq;
//using Evolution.UnitTest.UnitTestCases;
//using Microsoft.VisualStudio.TestTools.UnitTesting;

//namespace Evolution.UnitTests.UnitTestCases.Contract.Repositories
//{
//    [TestClass]
//    public class ContractInvoiceAttachmentRepositoryUnitTestCase : BaseTestCase
//    {
//        DomainData.IContractInvoiceAttachmentRepository contractInvoiceAttachmentRepository = null;
//        Mock<DbSet<DbModel.ContractInvoiceAttachment>> mockDbSet = null;
//        IQueryable<DbModel.ContractInvoiceAttachment> mockData = null;

//        [TestInitialize]
//        public void InitializeContractInvoiceAttachmentRepository()
//        {
//            mockData = MockContract.GetDbModelInvoiceAttachments();
//            mockDbSet = MockContract.GetContractInvoiceAttachmentMockDbSet(mockData);

//            mockContext.Setup(c => c.Set<DbModel.ContractInvoiceAttachment>()).Returns(mockDbSet.Object);
//            mockContext.Setup(c => c.ContractInvoiceAttachment).Returns(mockDbSet.Object);
//            contractInvoiceAttachmentRepository = new Evolution.Contract.Infrastructure.Data.ContractInvoiceAttachmentRepository(Mapper.Instance, mockContext.Object);
//        }

//        [TestMethod]
//        public void GetAllContractInvoiceAttachmentList()
//        {
//            var ContractInvoiceAttachment = contractInvoiceAttachmentRepository.GetAll().ToList();
//            Assert.AreEqual(2, ContractInvoiceAttachment.Count);
//        }

//        [TestMethod]
//        public void FindByContractInvoiceAttachmentListWithContractId()
//        {
//            var ContractInvoiceAttachment = contractInvoiceAttachmentRepository.FindBy(x => x.ContractId == 1).ToList();
//            Assert.AreEqual(1, ContractInvoiceAttachment.Count);
//        }
//        [TestMethod]
//        public void FindByContractInvoiceAttachmentListWithNull()
//        {
//            var ContractInvoiceAttachment = contractInvoiceAttachmentRepository.FindBy(null).ToList();
//            Assert.AreEqual(2, ContractInvoiceAttachment.Count);
//        }
//        [TestMethod]
//        public void FetchContractInvoiceAttachmentListByContractNumber()
//        {
//            var ContractInvoiceAttachment = contractInvoiceAttachmentRepository.Search(new Evolution.Contract.Domain.Models.Contracts.ContractInvoiceAttachment() { ContractNumber = "SU02412 / 0001" });
//            Assert.AreEqual(1, ContractInvoiceAttachment.Count);
//        }


//        [TestMethod]
//        public void FetchContractinvoiceAttachmentListByExchangeRateWildCardSerach()
//        {
//            var ContractInvoiceAttachment = contractInvoiceAttachmentRepository.Search(new Evolution.Contract.Domain.Models.Contracts.ContractInvoiceAttachment() { ContractNumber = "SU02412 / 0001" });
//            Assert.AreEqual(1, ContractInvoiceAttachment.Count);
//        }
//        [TestMethod]
//        public void AddContractInvoiceAttachment()
//        {
//            var newContractInvoiceAttachmnetData = mockData.First();
//            var ContractInvoiceAttachment = contractInvoiceAttachmentRepository.Add(newContractInvoiceAttachmnetData);

//            mockDbSet.Verify(m => m.Add(It.IsAny<DbModel.ContractInvoiceAttachment>()), Times.Once);
//            mockContext.Verify(m => m.SaveChanges(), Times.Once);
//        }

//        [TestMethod]
//        public void AddListOfContractInvoiceAttachment()
//        {
//            var newContractInvoiceAttachmnetData = mockData.ToList();
//            contractInvoiceAttachmentRepository.Add(newContractInvoiceAttachmnetData);

//            mockDbSet.Verify(m => m.AddRange(It.IsAny<IList<DbModel.ContractInvoiceAttachment>>()), Times.Once);
//            mockContext.Verify(m => m.SaveChanges(), Times.AtLeastOnce);
//        }

//        [TestMethod]
//        public void ContractinvoiceAttachmentUpdate()
//        {
//            var newContractInvoiceAttachmnetData = mockData.First();
//            contractInvoiceAttachmentRepository.Update(newContractInvoiceAttachmnetData);

//            mockDbSet.Verify(m => m.Update(It.IsAny<DbModel.ContractInvoiceAttachment>()), Times.Once);
//            mockContext.Verify(m => m.SaveChanges(), Times.Once);
//        }

//        [TestMethod]
//        public void ContractInvoiceAttachmnetDelete()
//        {
//            var newContractInvoiceAttachmnetData = mockData.First();
//            contractInvoiceAttachmentRepository.Delete(newContractInvoiceAttachmnetData);

//            mockDbSet.Verify(m => m.Remove(It.IsAny<DbModel.ContractInvoiceAttachment>()), Times.Once);
//            mockContext.Verify(m => m.SaveChanges(), Times.Once);
//        }

//        [TestMethod]
//        public void ContractInvoiceAttachmentDeleteBasedOnExpression()
//        {
//            contractInvoiceAttachmentRepository.Delete(x => x.ContractId == 1);

//            mockContext.Verify(m => m.RemoveRange(It.IsAny<IQueryable<DbModel.ContractInvoiceAttachment>>()), Times.AtLeastOnce);
//            mockContext.Verify(m => m.SaveChanges(), Times.AtLeastOnce);
//        }

//        [TestMethod]
//        public void ForceSaveDataShouldFailIfAutoSaveIsTrue()
//        {
//            contractInvoiceAttachmentRepository.AutoSave = true;
//            contractInvoiceAttachmentRepository.ForceSave();
//            contractInvoiceAttachmentRepository.AutoSave = false;
//            mockContext.Verify(m => m.SaveChanges(), Times.Never);
//        }

//        [TestMethod]
//        public void ForceSaveDataShouldPassIfAutoSaveIsFalse()
//        {
//            contractInvoiceAttachmentRepository.AutoSave = false;
//            contractInvoiceAttachmentRepository.ForceSave();

//            mockContext.Verify(m => m.SaveChanges(), Times.Once);
//        }
//    }


//}

