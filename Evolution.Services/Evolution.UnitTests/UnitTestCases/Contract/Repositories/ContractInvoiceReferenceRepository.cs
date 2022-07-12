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
//   public  class ContractInvoiceReferenceRepository:BaseTestCase
//   {
//        DomainData.IContractInvoiceReferenceTypeRepository contractInvoiceReferenceRepository = null;
//        Mock<DbSet<DbModel.ContractInvoiceReference>> mockDbSet = null;
//        IQueryable<DbModel.ContractInvoiceReference> mockData = null;
//        [TestInitialize]
//        public void InitializeContractInvoiceReferenceRepository()
//        {
//            mockData = MockContract.GetDbModelContractInvoiceReferences();
//            mockDbSet = MockContract.GetContractInvoiceReferencesMockDbSet(mockData);

//            mockContext.Setup(c => c.Set<DbModel.ContractInvoiceReference>()).Returns(mockDbSet.Object);
//            mockContext.Setup(c => c.ContractInvoiceReference).Returns(mockDbSet.Object);
//            contractInvoiceReferenceRepository = new Evolution.Contract.Infrastructure.Data.ContractInvoiceReferenceRepository(Mapper.Instance, mockContext.Object);
//        }
//        [TestMethod]
//        public void GetAllContractInvoiceReferenceList()
//        {
//            var ContractInvoiceReference= contractInvoiceReferenceRepository.GetAll().ToList();
//            Assert.AreEqual(2, ContractInvoiceReference.Count);
//        }

//        [TestMethod]
//        public void FindByContractInvoiceReferenceListWithContractId()
//        {
//            var ContractInvoiceReference = contractInvoiceReferenceRepository.FindBy(x => x.ContractId == 1).ToList();
//            Assert.AreEqual(1, ContractInvoiceReference.Count);
//        }
//        [TestMethod]
//        public void FindByContractInvoiceReferenceListWithNull()
//        {
//            var ContractInvoiceReference = contractInvoiceReferenceRepository.FindBy(null).ToList();
//            Assert.AreEqual(2, ContractInvoiceReference.Count);
//        }
//        [TestMethod]
//        public void FetchContractInvoiceReferenceListByContractNumber()
//        {
//            var ContractInvoiceReference = contractInvoiceReferenceRepository.Search(new Evolution.Contract.Domain.Models.Contracts.ContractInvoiceReferenceType() { ContractNumber = "SU02412 / 0001" });
//            Assert.AreEqual(1, ContractInvoiceReference.Count);
//        }


//        [TestMethod]
//        public void FetchContractInvoiceReferenceListByContractNumebrWildCardSerach()
//        {
//            var ContractInvoiceReference = contractInvoiceReferenceRepository.Search(new Evolution.Contract.Domain.Models.Contracts.ContractInvoiceReferenceType() { ContractNumber = "SU02412 / 0001" });
//            Assert.AreEqual(1, ContractInvoiceReference.Count);
//        }
//        [TestMethod]
//        public void AddContractInvoiceAttachment()
//        {
//            var newContractInvoiceReferencetData = mockData.First();
//            var ContractInvoiceReference = contractInvoiceReferenceRepository.Add(newContractInvoiceReferencetData);

//            mockDbSet.Verify(m => m.Add(It.IsAny<DbModel.ContractInvoiceReference>()), Times.Once);
//            mockContext.Verify(m => m.SaveChanges(), Times.Once);
//        }

//        [TestMethod]
//        public void AddListOfContractInvoiceReference()
//        {
//            var newContractInvoiceReferencetData = mockData.ToList();
//            contractInvoiceReferenceRepository.Add(newContractInvoiceReferencetData);

//            mockDbSet.Verify(m => m.AddRange(It.IsAny<IList<DbModel.ContractInvoiceReference>>()), Times.Once);
//            mockContext.Verify(m => m.SaveChanges(), Times.AtLeastOnce);
//        }

//        [TestMethod]
//        public void ContractinvoiceReferenceUpdate()
//        {
//            var newContractInvoiceReferencetData = mockData.First();
//            contractInvoiceReferenceRepository.Update(newContractInvoiceReferencetData);

//            mockDbSet.Verify(m => m.Update(It.IsAny<DbModel.ContractInvoiceReference>()), Times.Once);
//            mockContext.Verify(m => m.SaveChanges(), Times.Once);
//        }

//        [TestMethod]
//        public void ContractInvoiceReferenceDelete()
//        {
//            var newContractInvoiceReferencetData = mockData.First();
//            contractInvoiceReferenceRepository.Delete(newContractInvoiceReferencetData);

//            mockDbSet.Verify(m => m.Remove(It.IsAny<DbModel.ContractInvoiceReference>()), Times.Once);
//            mockContext.Verify(m => m.SaveChanges(), Times.Once);
//        }

//        [TestMethod]
//        public void ContractInvoiceReferenceDeleteBasedOnExpression()
//        {
//            contractInvoiceReferenceRepository.Delete(x => x.ContractId == 1);

//            mockContext.Verify(m => m.RemoveRange(It.IsAny<IQueryable<DbModel.ContractInvoiceReference>>()), Times.AtLeastOnce);
//            mockContext.Verify(m => m.SaveChanges(), Times.AtLeastOnce);
//        }

//        [TestMethod]
//        public void ForceSaveDataShouldFailIfAutoSaveIsTrue()
//        {
//            contractInvoiceReferenceRepository.AutoSave = true;
//            contractInvoiceReferenceRepository.ForceSave();
//            contractInvoiceReferenceRepository.AutoSave = false;
//            mockContext.Verify(m => m.SaveChanges(), Times.Never);
//        }

//        [TestMethod]
//        public void ForceSaveDataShouldPassIfAutoSaveIsFalse()
//        {
//            contractInvoiceReferenceRepository.AutoSave = false;
//            contractInvoiceReferenceRepository.ForceSave();

//            mockContext.Verify(m => m.SaveChanges(), Times.Once);
//        }
//    }

//}

