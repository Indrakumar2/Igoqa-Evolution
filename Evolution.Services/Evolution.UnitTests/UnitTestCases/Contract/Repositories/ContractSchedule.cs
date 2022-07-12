//using Evolution.UnitTest.UnitTestCases;
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
//using Microsoft.VisualStudio.TestTools.UnitTesting;

//namespace Evolution.UnitTests.UnitTestCases.Contract.Repositories
//{
//    [TestClass]
//    public class ContractSchedule : BaseTestCase
//    {
//        DomainData.IContractScheduleRepository contractScheduleRepository = null;
//        Mock<DbSet<DbModel.ContractSchedule>> mockDbSet = null;
//        IQueryable<DbModel.ContractSchedule> mockData = null;
//        [TestInitialize]
//        public void InitializeContractScheduleRepository()
//        {
//            mockData = MockContract.GetDbModelContractSchedule();
//            mockDbSet = MockContract.GetContractScheduleMockDbSet(mockData);

//            mockContext.Setup(c => c.Set<DbModel.ContractSchedule>()).Returns(mockDbSet.Object);
//            mockContext.Setup(c => c.ContractSchedule).Returns(mockDbSet.Object);
//            contractScheduleRepository = new Evolution.Contract.Infrastructure.Data.ContractScheduleRepository(mockContext.Object,Mapper.Instance );
//        }
//        [TestMethod]
//        public void GetAllContractScheduleList()
//        {
//            var ContractSchedule = contractScheduleRepository.GetAll().ToList();
//            Assert.AreEqual(2, ContractSchedule.Count);
//        }

//        [TestMethod]
//        public void FindByContractScheduleListWithContractId()
//        {
//            var ContractSchedule = contractScheduleRepository.FindBy(x => x.ContractId == 1).ToList();
//            Assert.AreEqual(1, ContractSchedule.Count);
//        }
//        [TestMethod]
//        public void FindByContractScheduleListWithNull()
//        {
//            var ContractSchedule = contractScheduleRepository.FindBy(null).ToList();
//            Assert.AreEqual(2, ContractSchedule.Count);
//        }
//        [TestMethod]
//        public void FetchContractScheduleListByContractNumber()
//        {
//            var ContractSchedule = contractScheduleRepository.Search(new Evolution.Contract.Domain.Models.Contracts.ContractSchedule() { ContractNumber = "SU02412 / 0001" });
//            Assert.AreEqual(1, ContractSchedule.Count);
//        }


//        [TestMethod]
//        public void FetchContractScheduleListByContractNumebrWildCardSerach()
//        {
//            var ContractSchedule = contractScheduleRepository.Search(new Evolution.Contract.Domain.Models.Contracts.ContractSchedule() { ContractNumber = "SU02412 / 0001" });
//            Assert.AreEqual(1, ContractSchedule.Count);
//        }
//        [TestMethod]
//        public void AddContractScheduele()
//        {
//            var newContractScheduleData = mockData.First();
//            var ContractSchedule = contractScheduleRepository.Add(newContractScheduleData);

//            mockDbSet.Verify(m => m.Add(It.IsAny<DbModel.ContractSchedule>()), Times.Once);
//            mockContext.Verify(m => m.SaveChanges(), Times.Once);
//        }

//        [TestMethod]
//        public void AddListOfContractInvoiceReference()
//        {
//            var newContractScheduleData = mockData.ToList();
//            contractScheduleRepository.Add(newContractScheduleData);

//            mockDbSet.Verify(m => m.AddRange(It.IsAny<IList<DbModel.ContractSchedule>>()), Times.Once);
//            mockContext.Verify(m => m.SaveChanges(), Times.AtLeastOnce);
//        }

//        [TestMethod]
//        public void ContractinvoiceReferenceUpdate()
//        {
//            var newContractScheduleData = mockData.First();
//            contractScheduleRepository.Update(newContractScheduleData);

//            mockDbSet.Verify(m => m.Update(It.IsAny<DbModel.ContractSchedule>()), Times.Once);
//            mockContext.Verify(m => m.SaveChanges(), Times.Once);
//        }

//        [TestMethod]
//        public void ContractInvoiceScheduleDelete()
//        {
//            var newContractScheduleData = mockData.First();
//            contractScheduleRepository.Delete(newContractScheduleData);

//            mockDbSet.Verify(m => m.Remove(It.IsAny<DbModel.ContractSchedule>()), Times.Once);
//            mockContext.Verify(m => m.SaveChanges(), Times.Once);
//        }

//        [TestMethod]
//        public void ContractInvoiceReferenceDeleteBasedOnExpression()
//        {
//            contractScheduleRepository.Delete(x => x.ContractId == 1);

//            mockContext.Verify(m => m.RemoveRange(It.IsAny<IQueryable<DbModel.ContractInvoiceReference>>()), Times.AtLeastOnce);
//            mockContext.Verify(m => m.SaveChanges(), Times.AtLeastOnce);
//        }

//        [TestMethod]
//        public void ForceSaveDataShouldFailIfAutoSaveIsTrue()
//        {
//            contractScheduleRepository.AutoSave = true;
//            contractScheduleRepository.ForceSave();
//            contractScheduleRepository.AutoSave = false;
//            mockContext.Verify(m => m.SaveChanges(), Times.Never);
//        }

//        [TestMethod]
//        public void ForceSaveDataShouldPassIfAutoSaveIsFalse()
//        {
//            contractScheduleRepository.AutoSave = false;
//            contractScheduleRepository.ForceSave();

//            mockContext.Verify(m => m.SaveChanges(), Times.Once);
//        }
//    }
//}
//}
