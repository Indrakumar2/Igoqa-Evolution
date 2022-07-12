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
//   public  class ContractNoteRepositoryUnitTestCase :BaseTestCase
//   {
//        DomainData.IContractNoteRepository contractNoteRepository = null;
//        Mock<DbSet<DbModel.ContractNote>> mockDbSet = null;
//        IQueryable<DbModel.ContractNote> mockData = null;

//        [TestInitialize]
//        public void InitializeContractNoteRepository()
//        {
//            mockData = MockContract.GetContractNoteMockData();
//            mockDbSet = MockContract.GetContractNoteMockDbSet(mockData);

//            mockContext.Setup(c => c.Set<DbModel.ContractNote>()).Returns(mockDbSet.Object);
//            mockContext.Setup(c => c.ContractNote).Returns(mockDbSet.Object);
//            contractNoteRepository = new Evolution.Contract.Infrastructure.Data.ContractNoteRepository(mockContext.Object, Mapper.Instance);
//        }

//        [TestMethod]
//        public void GetAllContractNoteList()
//        {
//            var ContractNote = contractNoteRepository.GetAll().ToList();
//            Assert.AreEqual(2, ContractNote.Count);
//        }

//        [TestMethod]
//        public void FindByContractNoteListWithContractId()
//        {
//            var ContractNote = contractNoteRepository.FindBy(x => x.ContractId == 1).ToList();
//            Assert.AreEqual(1, ContractNote.Count);
//        }

//        [TestMethod]
//        public void FindByContractNoteListWithNull()
//        {
//            var ContractNote = contractNoteRepository.FindBy(null).ToList();
//            Assert.AreEqual(2, ContractNote.Count);
//        }

//        [TestMethod]
//        public void CheckContractNoteExistsWithNote()
//        {
//            var ContractNote = contractNoteRepository.Exists(x => x.Note == "Average TS hourly cost taken from StarBitz tool used to determine 2010 standard rates.").Result;
//            Assert.AreEqual(false, ContractNote);
//        }

//        [TestMethod]
//        public void FetchContractNoteListWithoutSearchValue()
//        {
//            var ContractNote = contractNoteRepository.Search(new Evolution.Contract.Domain.Models.Contracts.ContractNote());
//            Assert.AreEqual(4, ContractNote.Count);
//        }

//        //[TestMethod]
//        //public void FetchCompanyNoteListWithNullSearchModel()
//        //{
//        //    var companyNote = companyNoteRepository.Search(null);
//        //    Assert.AreEqual(4, companyNote.Count);
//        //}

//        [TestMethod]
//        public void FetchContractNoteListByContractNumber()
//        {
//            var ContractNote = contractNoteRepository.Search(new Evolution.Contract.Domain.Models.Contracts.ContractNote() { ContractNumber = "SU02412/0001" });
//            Assert.AreEqual(1, ContractNote.Count);
//        }

//        [TestMethod]
//        public void FetchContractNoteListByNote()
//        {
//            var ContractNote = contractNoteRepository.Search(new Evolution.Contract.Domain.Models.Contracts.ContractNote() { Notes = "This is a JV between Aker Solutions/ Santos/ O&G." });
//            Assert.AreEqual(1, ContractNote.Count);
//        }

//        [TestMethod]
//        public void FetchContractNoteListByNoteWildCardSerach()
//        {
//            var ContractNote = contractNoteRepository.Search(new Evolution.Contract.Domain.Models.Contracts.ContractNote() { Notes = "*This is a JV between Aker Solutions/ Santos/ O&G.*" });
//            Assert.AreEqual(1, ContractNote.Count);
//        }

//        [TestMethod]
//        public void FetchContractNoteListByNoteCreatedBy()
//        {
//            var ContractNote = contractNoteRepository.Search(new Evolution.Contract.Domain.Models.Contracts.ContractNote() { CreatedBy = "Jennn.Blyth" });
//            Assert.AreEqual(2, ContractNote.Count);
//        }

//        [TestMethod]
//        public void FetchContractNoteListByContractNumberAndNote()
//        {
//            var ContractNote = contractNoteRepository.Search(new Evolution.Contract.Domain.Models.Contracts.ContractNote() { ContractNumber= "SU02412 / 0001",Notes= "This is a JV between Aker Solutions/ Santos/ O&G." });
//            Assert.AreEqual(1, ContractNote.Count);
//        }

//        [TestMethod]
//        public void FetchContractNoteListByWrongContractNumberAndNote()
//        {
//            var ContractNote = contractNoteRepository.Search(new Evolution.Contract.Domain.Models.Contracts.ContractNote() { ContractNumber = "SU02412 / 0021", Notes = "Read This is a JV between Aker Solutions/ Santos/ O&G." });
//            Assert.AreEqual(0, ContractNote.Count);
//        }
//        [TestMethod]
//        public void FetchContractNoteListWithContractNumberEmptystring()
//        {
//            var ContractNote = contractNoteRepository.Search(new Evolution.Contract.Domain.Models.Contracts.ContractNote() { ContractNumber = string.Empty });
//            Assert.AreEqual(2, ContractNote.Count);
//        }

//        //[TestMethod]
//        //public void FetchCompanyNoteListWithSpaceInCompanyCodeAndNote()
//        //{
//        //    var companyNote = companyNoteRepository.Search(new Company.Domain.Models.Companies.CompanyNote() { CompanyCode = " ", Notes = " " });
//        //    Assert.AreEqual(4, companyNote.Count);
//        //}

//        [TestMethod]
//        public void AddContractNote()
//        {
//            var newContractNoteData = mockData.First();
//            var ContractNote = contractNoteRepository.Add(newContractNoteData);

//            mockDbSet.Verify(m => m.Add(It.IsAny<DbModel.ContractNote>()), Times.Once);
//            mockContext.Verify(m => m.SaveChanges(), Times.Once);
//        }

//        [TestMethod]
//        public void AddListOfContractNotes()
//        {
//            var newContractNoteData = mockData.ToList();
//            contractNoteRepository.Add(newContractNoteData);

//            mockDbSet.Verify(m => m.AddRange(It.IsAny<IList<DbModel.ContractNote>>()), Times.Once);
//            mockContext.Verify(m => m.SaveChanges(), Times.AtLeastOnce);
//        }

//        [TestMethod]
//        public void ContractNoteUpdate()
//        {
//            var newContractNoteData = mockData.First();
//            contractNoteRepository.Update(newContractNoteData);

//            mockDbSet.Verify(m => m.Update(It.IsAny<DbModel.ContractNote>()), Times.Once);
//            mockContext.Verify(m => m.SaveChanges(), Times.Once);
//        }

//        [TestMethod]
//        public void ContractNoteDelete()
//        {
//            var newContractNoteData = mockData.First();
//            contractNoteRepository.Delete(newContractNoteData);

//            mockDbSet.Verify(m => m.Remove(It.IsAny<DbModel.ContractNote>()), Times.Once);
//            mockContext.Verify(m => m.SaveChanges(), Times.Once);
//        }

//        [TestMethod]
//        public void ContractNoteDeleteBasedOnExpression()
//        {
//            contractNoteRepository.Delete(x => x.ContractId == 1);

//            mockContext.Verify(m => m.RemoveRange(It.IsAny<IQueryable<DbModel.ContractNote>>()), Times.AtLeastOnce);
//            mockContext.Verify(m => m.SaveChanges(), Times.AtLeastOnce);
//        }

//        [TestMethod]
//        public void ForceSaveDataShouldFailIfAutoSaveIsTrue()
//        {
//            contractNoteRepository.AutoSave = true;
//            contractNoteRepository.ForceSave();
//            contractNoteRepository.AutoSave = false;
//            mockContext.Verify(m => m.SaveChanges(), Times.Never);
//        }

//        [TestMethod]
//        public void ForceSaveDataShouldPassIfAutoSaveIsFalse()
//        {
//            contractNoteRepository.AutoSave = false;
//            contractNoteRepository.ForceSave();

//            mockContext.Verify(m => m.SaveChanges(), Times.Once);
//        }
//    }

//} 



