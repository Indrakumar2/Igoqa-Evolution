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
    public class CompanyNoteRepositoryUnitTestCase : BaseTestCase
    {
        DomainData.ICompanyNoteRepository companyNoteRepository = null;
        Mock<DbSet<DbModel.CompanyNote>> mockDbSet = null;
        IQueryable<DbModel.CompanyNote> mockData = null;

        [TestInitialize]
        public void InitializeCompanyNoteRepository()
        {
            mockData = MockCompany.GetCompanyNoteMockData();
            mockDbSet = MockCompany.GetCompanyNoteMockDbSet(mockData);

            mockContext.Setup(c => c.Set<DbModel.CompanyNote>()).Returns(mockDbSet.Object);
            mockContext.Setup(c => c.CompanyNote).Returns(mockDbSet.Object);
            companyNoteRepository = new Company.Infrastructure.Data.CompanyNoteRepository(Mapper.Instance, mockContext.Object);
        }
         
        [TestMethod]
        public void GetAllCompanyNoteList()
        {
            var companyNote = companyNoteRepository.GetAll().ToList();
            Assert.AreEqual(4, companyNote.Count);
        }

        [TestMethod]
        public void FindByCompanyNoteListWithCompanyId()
        {
            var companyNote = companyNoteRepository.FindBy(x => x.CompanyId == 116).ToList();
            Assert.AreEqual(1, companyNote.Count);
        }

        [TestMethod]
        public void FindByCompanyNoteListWithNull()
        {
            var companyNote = companyNoteRepository.FindBy(null).ToList();
            Assert.AreEqual(4, companyNote.Count);
        }

        [TestMethod]
        public void CheckCompanyNoteExistsWithNote()
        {
            var companyNote = companyNoteRepository.Exists(x => x.Note == "Average TS hourly cost taken from StarBitz tool used to determine 2010 standard rates.").Result;
            Assert.AreEqual(true, companyNote);
        }

        [TestMethod]
        public void FetchCompanyNoteListWithoutSearchValue()
        {
            var companyNote = companyNoteRepository.Search(new Company.Domain.Models.Companies.CompanyNote());
            Assert.AreEqual(4, companyNote.Count);
        }

        //[TestMethod]
        //public void FetchCompanyNoteListWithNullSearchModel()
        //{
        //    var companyNote = companyNoteRepository.Search(null);
        //    Assert.AreEqual(4, companyNote.Count);
        //}

        [TestMethod]
        public void FetchCompanyNoteListByCompanyCode()
        {
            var companyNote = companyNoteRepository.Search(new Company.Domain.Models.Companies.CompanyNote() { CompanyCode = "UK050" });
            Assert.AreEqual(1, companyNote.Count);
        }

        [TestMethod]
        public void FetchCompanyNoteListByNote()
        {
            var companyNote = companyNoteRepository.Search(new Company.Domain.Models.Companies.CompanyNote() {  Notes = "Welcome to MI Extranet!  Please note MI Company Inspection forms and company notices may be obtained under Company Documents portion of this Extranet.  IMPORTANT NOTICE:  US Nuclear Regulatory Commission 10CFR 21: Notification of Defects  If you suspect that any item of equipment that is destined for a nuclear facility in the USA is defective, contact your Project Coordinator immediately!  This applies to you whether or not you are assigned to work on that specific item of equipment.  “Defective” means deviates from the technical requirements." });
            Assert.AreEqual(1, companyNote.Count);
        }

        [TestMethod]
        public void FetchCompanyNoteListByNoteWildCardSerach()
        {
            var companyNote = companyNoteRepository.Search(new Company.Domain.Models.Companies.CompanyNote() { Notes = "*Welcome to MI Extranet!  Please note MI Company Inspection*" });
            Assert.AreEqual(1, companyNote.Count);
        }

        [TestMethod]
        public void FetchCompanyNoteListByNoteCreatedBy()
        {
            var companyNote = companyNoteRepository.Search(new Company.Domain.Models.Companies.CompanyNote() {  CreatedBy = "Jennn.Blyth" });
            Assert.AreEqual(2, companyNote.Count);
        }
         
        [TestMethod]
        public void FetchCompanyNoteListByCompanyCodeAndNote()
        {
            var companyNote = companyNoteRepository.Search(new Company.Domain.Models.Companies.CompanyNote() { CompanyCode = "UK056", Notes = "Name Change to PIpe payroll due to incorrect naming of payroll schedules.  There had been an additional payroll added into the system 01/2011b which was incorrect.  01/2011b became 02/2011 and the additional pay periods changed accordingly." });
            Assert.AreEqual(1, companyNote.Count);
        }

        [TestMethod]
        public void FetchCompanyNoteListByWrongCompanyCodeAndNote()
        {
            var companyNote = companyNoteRepository.Search(new Company.Domain.Models.Companies.CompanyNote() { CompanyCode = "UK058", Notes = "Name Change to PIpe payroll due to incorrect naming of payroll schedules.  There had been an additional payroll added into the system 01/2011b which was incorrect.  01/2011b became 02/2011 and the additional pay periods changed accordingly." });
            Assert.AreEqual(0, companyNote.Count);
        }
          
        [TestMethod]
        public void FetchCompanyNoteListWithCompanyCodeEmptystring()
        {
            var companyNote = companyNoteRepository.Search(new Company.Domain.Models.Companies.CompanyNote() { CompanyCode = string.Empty });
            Assert.AreEqual(4, companyNote.Count);
        }

        //[TestMethod]
        //public void FetchCompanyNoteListWithSpaceInCompanyCodeAndNote()
        //{
        //    var companyNote = companyNoteRepository.Search(new Company.Domain.Models.Companies.CompanyNote() { CompanyCode = " ", Notes = " " });
        //    Assert.AreEqual(4, companyNote.Count);
        //}
         
        [TestMethod]
        public void AddCompanyNote()
        {
            var newCompanyNoteData = mockData.First();
            var companyNote = companyNoteRepository.Add(newCompanyNoteData);

            mockDbSet.Verify(m => m.Add(It.IsAny<DbModel.CompanyNote>()), Times.Once);
            mockContext.Verify(m => m.SaveChanges(), Times.Once);
        }

        [TestMethod]
        public void AddListOfCompanyNotes()
        {
            var newCompanyNoteData = mockData.ToList();
            companyNoteRepository.Add(newCompanyNoteData);

            mockDbSet.Verify(m => m.AddRange(It.IsAny<IList<DbModel.CompanyNote>>()), Times.Once);
            mockContext.Verify(m => m.SaveChanges(), Times.AtLeastOnce);
        }

        [TestMethod]
        public void CompanyNoteUpdate()
        {
            var newCompanyNoteData = mockData.First();
            companyNoteRepository.Update(newCompanyNoteData);

            mockDbSet.Verify(m => m.Update(It.IsAny<DbModel.CompanyNote>()), Times.Once);
            mockContext.Verify(m => m.SaveChanges(), Times.Once);
        }

        [TestMethod]
        public void CompanyNoteDelete()
        {
            var newCompanyNoteData = mockData.First();
            companyNoteRepository.Delete(newCompanyNoteData);

            mockDbSet.Verify(m => m.Remove(It.IsAny<DbModel.CompanyNote>()), Times.Once);
            mockContext.Verify(m => m.SaveChanges(), Times.Once);
        }

        [TestMethod]
        public void CompanyNoteDeleteBasedOnExpression()
        {
            companyNoteRepository.Delete(x => x.CompanyId == 116);

            mockContext.Verify(m => m.RemoveRange(It.IsAny<IQueryable<DbModel.CompanyNote>>()), Times.AtLeastOnce);
            mockContext.Verify(m => m.SaveChanges(), Times.AtLeastOnce);
        }

        [TestMethod]
        public void ForceSaveDataShouldFailIfAutoSaveIsTrue()
        {
            companyNoteRepository.AutoSave = true;
            companyNoteRepository.ForceSave();
            companyNoteRepository.AutoSave = false;
            mockContext.Verify(m => m.SaveChanges(), Times.Never);
        }

        [TestMethod]
        public void ForceSaveDataShouldPassIfAutoSaveIsFalse()
        {
            companyNoteRepository.AutoSave = false;
            companyNoteRepository.ForceSave();

            mockContext.Verify(m => m.SaveChanges(), Times.Once);
        }
    }
}
