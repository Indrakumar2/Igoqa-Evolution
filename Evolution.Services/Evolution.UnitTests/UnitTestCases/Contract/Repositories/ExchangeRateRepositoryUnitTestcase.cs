//using Microsoft.VisualStudio.TestTools.UnitTesting;
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

//namespace Evolution.UnitTests.UnitTestCases.Contract.Repositories
//{
//    [TestClass]
//    public class ExchangeRateRepositoryUnitTestcase :BaseTestCase
//    {
//        DomainData.IContractExchangeRateRepository contractExchangeRateRepository = null;
//        Mock<DbSet<DbModel.ContractExchangeRate>> mockDbSet = null;
//        IQueryable<DbModel.ContractExchangeRate> mockData = null;

//        [TestInitialize]
//        public void InitializeContractNoteRepository()
//        {
//            mockData = MockContract.GetContratctExchangeRates();
//            mockDbSet = MockContract.GetContractExchangeRateMockDbSet(mockData);

//            mockContext.Setup(c => c.Set<DbModel.ContractExchangeRate>()).Returns(mockDbSet.Object);
//            mockContext.Setup(c => c.ContractExchangeRate).Returns(mockDbSet.Object);
//            contractExchangeRateRepository = new Evolution.Contract.Infrastructure.Data.ContractExchangeRateRepository(Mapper.Instance,mockContext.Object);
//        }

//        [TestMethod]
//        public void GetAllContractExchangeRateList()
//        {
//            var ContractExchangeRate = contractExchangeRateRepository.GetAll().ToList();
//            Assert.AreEqual(2, ContractExchangeRate.Count);
//        }


//        [TestMethod]
//        public void FindByContractExchangeRateListWithContractId()
//        {
//            var ContractExchangeRate = contractExchangeRateRepository.FindBy(x => x.ContractId == 1).ToList();
//            Assert.AreEqual(1, ContractExchangeRate.Count);
//        }

//        [TestMethod]
//        public void FindByContractNoteListWithNull()
//        {
//            var ContractExchangeRate = contractExchangeRateRepository.FindBy(null).ToList();
//            Assert.AreEqual(2, ContractExchangeRate.Count);
//        }
//        [TestMethod]
//        public void FetchContractExchangeRateListByContractNumber()
//        {
//            var ContractExchangeRate = contractExchangeRateRepository.Search(new Evolution.Contract.Domain.Models.Contracts.ContractExchangeRate() { ContractNumber = "SU02412 / 0001" });
//            Assert.AreEqual(1, ContractExchangeRate.Count);
//        }

//        [TestMethod]
//        public void FetchContractExchangeRateListByExchangeRateWildCardSerach()
//        {
//            var ContractExchangeRate = contractExchangeRateRepository.Search(new Evolution.Contract.Domain.Models.Contracts.ContractExchangeRate() { ContractNumber = "*SU02412 / 0001*" });
//            Assert.AreEqual(1, ContractExchangeRate.Count);
//        }
//        [TestMethod]
//        public void AddContractExchangeRate()
//        {
//            var newContractExchangeRateData = mockData.First();
//            var ContractNote = contractExchangeRateRepository.Add(newContractExchangeRateData);

//            mockDbSet.Verify(m => m.Add(It.IsAny<DbModel.ContractExchangeRate>()), Times.Once);
//            mockContext.Verify(m => m.SaveChanges(), Times.Once);
//        }

//        [TestMethod]
//        public void AddListOfContractExchangeRate()
//        {
//            var newContractExchangeRateData = mockData.ToList();
//            contractExchangeRateRepository.Add(newContractExchangeRateData);

//            mockDbSet.Verify(m => m.AddRange(It.IsAny<IList<DbModel.ContractExchangeRate>>()), Times.Once);
//            mockContext.Verify(m => m.SaveChanges(), Times.AtLeastOnce);
//        }

//        [TestMethod]
//        public void ContractExchangeRateUpdate()
//        {
//            var newContractExchangeRateData = mockData.First();
//            contractExchangeRateRepository.Update(newContractExchangeRateData);

//            mockDbSet.Verify(m => m.Update(It.IsAny<DbModel.ContractExchangeRate>()), Times.Once);
//            mockContext.Verify(m => m.SaveChanges(), Times.Once);
//        }

//        [TestMethod]
//        public void ContractExchangeRateDelete()
//        {
//            var newContractExchangeRateData = mockData.First();
//            contractExchangeRateRepository.Delete(newContractExchangeRateData);

//            mockDbSet.Verify(m => m.Remove(It.IsAny<DbModel.ContractExchangeRate>()), Times.Once);
//            mockContext.Verify(m => m.SaveChanges(), Times.Once);
//        }

//        [TestMethod]
//        public void ContractExchangeRateDeleteBasedOnExpression()
//        {
//            contractExchangeRateRepository.Delete(x => x.ContractId == 1);

//            mockContext.Verify(m => m.RemoveRange(It.IsAny<IQueryable<DbModel.ContractExchangeRate>>()), Times.AtLeastOnce);
//            mockContext.Verify(m => m.SaveChanges(), Times.AtLeastOnce);
//        }

//        [TestMethod]
//        public void ForceSaveDataShouldFailIfAutoSaveIsTrue()
//        {
//            contractExchangeRateRepository.AutoSave = true;
//            contractExchangeRateRepository.ForceSave();
//            contractExchangeRateRepository.AutoSave = false;
//            mockContext.Verify(m => m.SaveChanges(), Times.Never);
//        }

//        [TestMethod]
//        public void ForceSaveDataShouldPassIfAutoSaveIsFalse()
//        {
//            contractExchangeRateRepository.AutoSave = false;
//            contractExchangeRateRepository.ForceSave();

//            mockContext.Verify(m => m.SaveChanges(), Times.Once);
//        }
//    }
//}
