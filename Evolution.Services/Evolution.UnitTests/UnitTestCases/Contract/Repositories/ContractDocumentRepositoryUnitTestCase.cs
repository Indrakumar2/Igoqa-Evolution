//using AutoMapper;
//using Evolution.UnitTest.UnitTestCases;
//using Evolution.UnitTests.Mocks.Data.Contracts.Db;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using Moq;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using DbModel = Evolution.DbRepository.Models;
//using DomainData = Evolution.Contract.Domain.Interfaces.Data;

//namespace Evolution.UnitTests.UnitTestCases.Contract.Repositories
//{
//    [TestClass]
//    public  class ContractDocumentRepositoryUnitTestCase : BaseTestCase
//    {
//        DomainData.IContractDocumentRepository ContractDocumentRepository = null;
//        Mock<DbSet<DbModel.ContractDocument>> mockDbSet = null;
//        IQueryable<DbModel.ContractDocument> mockData = null;
        

//        [TestInitialize]
//        public void InitializecontractDocumentRepository()
//        {
//            mockData = MockContract.GetDbModelContractDocuments();
//            mockDbSet = MockContract.GetContractDocumentsMockDbSet(mockData);
//           mockContext.Setup(c => c.Set<DbModel.ContractDocument>()).Returns(mockDbSet.Object);
//            mockContext.Setup(c => c.ContractDocument).Returns(mockDbSet.Object);
//            ContractDocumentRepository = new Evolution.Contract.Infrastructure.Data.ContractDocumentRepository(Mapper.Instance, mockContext.Object);
//        }

//        [TestMethod]
//        public void GetAllContractDocumentList()
//        {
//            var contractDocuments = ContractDocumentRepository.GetAll().ToList();
//            Assert.AreEqual(2, contractDocuments.Count);
//        }
//        [TestMethod]
//        public void FindByContractDocumentList()
//        {
//            var contractDocuments = ContractDocumentRepository.FindBy(x => x.Name == "Contract").ToList();
//            Assert.AreEqual(1, contractDocuments.Count);
//        }
//        [TestMethod]
//        public void FindByContractDocumentListWithNull()
//        {
//            var contractDocuments = ContractDocumentRepository.FindBy(null).ToList();
//            Assert.AreEqual(2, contractDocuments.Count);
//        }
//        [TestMethod]
//        public void CheckContractDocumentExistsWithDocumentName()
//        {
//            var contractDocuments = ContractDocumentRepository.Exists(x => x.Name == "Contract").Result;
//            Assert.AreEqual(true, contractDocuments);
//        }
//        [TestMethod]
//        public void FetchContractDocumentListWithoutSearchValue()
//        {
//            var contractDocuments = ContractDocumentRepository.Search(new Evolution.Contract.Domain.Models.Contracts.ContractDocument());
//            Assert.AreEqual(2, contractDocuments.Count);
//        }
//        [TestMethod]
//        public void FetchContractDocumentListByContractNumber()
//        {
//            var contractDocuments = ContractDocumentRepository.Search(new Evolution.Contract.Domain.Models.Contracts.ContractDocument() { ContractNumber = "SU02412 / 0001" });
//            Assert.AreEqual(1, contractDocuments.Count);
//        }

//        [TestMethod]
//        public void FetchContractDocumentListByDocumentName()
//        {
//            var contractDocuments = ContractDocumentRepository.Search(new Evolution.Contract.Domain.Models.Contracts.ContractDocument() { Name = "Contract" });
//            Assert.AreEqual(1, contractDocuments.Count);
//        }
//        [TestMethod]
//        public void FetchContractDocumentListByDocumentNameWildCardSerach()
//        {
//            var contractDocuments = ContractDocumentRepository.Search(new Evolution.Contract.Domain.Models.Contracts.ContractDocument() { Name = "Contract*" });
//            Assert.AreEqual(1, contractDocuments.Count);
//        }

//        [TestMethod]
//        public void FetchContractDocumentListByDocumentType()
//        {
//            var contractDocuments = ContractDocumentRepository.Search(new Evolution.Contract.Domain.Models.Contracts.ContractDocument() { DocumentType = "Certificate" });
//            Assert.AreEqual(2, contractDocuments.Count);
//        }

//        [TestMethod]
//        public void FetchContractDocumentListByIsVisibleToCustomer()
//        {
//            var contractDocuments = ContractDocumentRepository.Search(new Evolution.Contract.Domain.Models.Contracts.ContractDocument() { IsVisibleToCustomer = true });
//            Assert.AreEqual(3, contractDocuments.Count);
//        }

//        [TestMethod]
//        public void FetchContractDocumentListByContractNumberAndDocumentName()
//        {
//            var contractDocuments = ContractDocumentRepository.Search(new Evolution.Contract.Domain.Models.Contracts.ContractDocument() { ContractNumber = "SU02412/0001", Name = "Contract" });
//            Assert.AreEqual(1, contractDocuments.Count);
//        }

       
//        [TestMethod]
//        public void FetchCompanyDocumentListWithContractEmptystring()
//        {
//            var contractDocuments = ContractDocumentRepository.Search(new Evolution.Contract.Domain.Models.Contracts.ContractDocument() { ContractNumber = string.Empty });
//            Assert.AreEqual(2, contractDocuments.Count);
//        }
//    }
//}
