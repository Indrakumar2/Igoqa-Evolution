using Evolution.Logging.Interfaces;
using Evolution.UnitTests.Mocks.Data.Companies;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using DomainData = Evolution.Company.Domain.Interfaces.Data;
using DomModel = Evolution.Company.Domain.Models.Companies;
using DbModel = Evolution.DbRepository.Models;
using ServiceDomainData = Evolution.Company.Domain.Interfaces.Companies;
using Evolution.UnitTests.Mocks.Data.Companies.Db;
using System.Linq.Expressions;

namespace Evolution.UnitTests.UnitTestCases.Companies.Services
{
    [TestClass]
    public class CompanyNoteServiceUnitTestCase
    {
        ServiceDomainData.ICompanyNoteService companyNoteService = null;
        Mock<DomainData.ICompanyNoteRepository> mockCompanyNoteRepository = null;
        Mock<DomainData.ICompanyRepository> mockCompanyRepository = null;
        IList<DomModel.CompanyNote> companyNoteDomianModels = null;
        Mock<IAppLogger<Company.Core.Services.CompanyNoteService>> mockLogger = null;
        IQueryable<DbModel.Company> mockComapanyDbData = null;
        IQueryable<DbModel.CompanyNote> mockComapanyNoteDbData = null;

        [TestInitialize]
        public void InitializeCompanyNoteService()
        {
            mockCompanyNoteRepository = new Mock<DomainData.ICompanyNoteRepository>();
            mockCompanyRepository = new Mock<DomainData.ICompanyRepository>();

            companyNoteDomianModels = MockCompanyDomainModel.GetCompanyNoteMockedDomainModelData();
            mockLogger = new Mock<IAppLogger<Company.Core.Services.CompanyNoteService>>();
            mockComapanyDbData = MockCompany.GetCompanyMockData();
            mockComapanyNoteDbData = MockCompany.GetCompanyNoteMockData();
        }

        [TestMethod]
        public void FetchCompanyNoteListWithoutSearchValue()
        {
            mockCompanyNoteRepository.Setup(x => x.Search(It.IsAny<DomModel.CompanyNote>())).Returns(companyNoteDomianModels);

            companyNoteService = new Company.Core.Services.CompanyNoteService(mockCompanyNoteRepository.Object, mockLogger.Object,mockCompanyRepository.Object);

            var response = companyNoteService.GetCompanyNote(new DomModel.CompanyNote());

            Assert.AreEqual("1", response.Code);
            Assert.AreEqual(4, (response.Result as List<DomModel.CompanyNote>).Count);
        }

        [TestMethod]
        public void FetchCompanyNoteListWithNullSearchModel()
        {
            mockCompanyNoteRepository.Setup(x => x.Search(It.IsAny<DomModel.CompanyNote>())).Returns(companyNoteDomianModels);

            companyNoteService = new Company.Core.Services.CompanyNoteService(mockCompanyNoteRepository.Object, mockLogger.Object,mockCompanyRepository.Object);

            var response = companyNoteService.GetCompanyNote(null);

            Assert.AreEqual("1", response.Code);
            Assert.AreEqual(4, (response.Result as List<DomModel.CompanyNote>).Count);
        }

        [TestMethod]
        public void FetchCompanyNoteListByCompanyCode()
        {
            mockCompanyNoteRepository.Setup(x => x.Search(It.Is<DomModel.CompanyNote>(c => c.CompanyCode == "UK050"))).Returns(new List<DomModel.CompanyNote> { companyNoteDomianModels[0] });

            companyNoteService = new Company.Core.Services.CompanyNoteService(mockCompanyNoteRepository.Object, mockLogger.Object,mockCompanyRepository.Object);

            var response = companyNoteService.GetCompanyNote(new DomModel.CompanyNote { CompanyCode = "UK050" });

            Assert.AreEqual("1", response.Code);
            Assert.AreEqual(1, (response.Result as List<DomModel.CompanyNote>).Count);
        }

        [TestMethod]
        public void FetchCompanyNoteListByWildCardSearchWithNotesStartWith()
        {
            mockCompanyNoteRepository.Setup(x => x.Search(It.Is<DomModel.CompanyNote>(c => c.Notes.StartsWith("Inv")))).Returns(new List<DomModel.CompanyNote> { companyNoteDomianModels[0] });

            companyNoteService = new Company.Core.Services.CompanyNoteService(mockCompanyNoteRepository.Object, mockLogger.Object,mockCompanyRepository.Object);

            var response = companyNoteService.GetCompanyNote(new DomModel.CompanyNote { Notes = "Inv*" });

            Assert.AreEqual("1", response.Code);
            Assert.AreEqual(1, (response.Result as List<DomModel.CompanyNote>).Count);
        }

        [TestMethod]
        public void FetchCompanyNoteListByWildCardSearchWithNotesEndWith()
        {
            mockCompanyNoteRepository.Setup(x => x.Search(It.Is<DomModel.CompanyNote>(c => c.Notes.EndsWith("April 2008")))).Returns(new List<DomModel.CompanyNote> { companyNoteDomianModels[1], companyNoteDomianModels[3] });

            companyNoteService = new Company.Core.Services.CompanyNoteService(mockCompanyNoteRepository.Object, mockLogger.Object,mockCompanyRepository.Object);

            var response = companyNoteService.GetCompanyNote(new DomModel.CompanyNote { Notes = "*File" });

            Assert.AreEqual("1", response.Code);
            Assert.AreEqual(2, (response.Result as List<DomModel.CompanyNote>).Count);
        }

        [TestMethod]
        public void FetchCompanyNoteListByWildCardSearchWithNotesContains()
        {
            mockCompanyNoteRepository.Setup(x => x.Search(It.Is<DomModel.CompanyNote>(c => c.Notes.Contains("ignment")))).Returns(new List<DomModel.CompanyNote> { companyNoteDomianModels[1] });

            companyNoteService = new Company.Core.Services.CompanyNoteService(mockCompanyNoteRepository.Object, mockLogger.Object,mockCompanyRepository.Object);

            var response = companyNoteService.GetCompanyNote(new DomModel.CompanyNote { Notes = "*ignment*" });

            Assert.AreEqual("1", response.Code);
            Assert.AreEqual(1, (response.Result as List<DomModel.CompanyNote>).Count);
        }

        [TestMethod]
        public void ThrowsExceptionWhileFetchingCompanyNoteList()
        {
            mockCompanyNoteRepository.Setup(x => x.Search(It.IsAny<DomModel.CompanyNote>())).Throws(new Exception("Exception occured while performing some operation."));
            companyNoteService = new Company.Core.Services.CompanyNoteService(mockCompanyNoteRepository.Object, mockLogger.Object,mockCompanyRepository.Object);

            var response = companyNoteService.GetCompanyNote(new DomModel.CompanyNote());

            Assert.AreEqual("11", response.Code);
            Assert.IsNotNull(response.Messages);
            Assert.AreEqual("31", response.Messages.FirstOrDefault().Code);
        }

        [TestMethod]
        public void SaveCompanyNotes()
        {
            mockCompanyRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Company, bool>>>())).Returns(mockComapanyDbData);
            mockCompanyNoteRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.CompanyNote, bool>>>())).Returns((Expression<Func<DbModel.CompanyNote, bool>> predicate) => mockComapanyNoteDbData.Where(predicate));
            companyNoteService = new Company.Core.Services.CompanyNoteService(mockCompanyNoteRepository.Object, mockLogger.Object,mockCompanyRepository.Object);

            companyNoteDomianModels.ToList().ForEach(x => x.RecordStatus = "N");

            var response = companyNoteService.SaveCompanyNote(companyNoteDomianModels[0].CompanyCode, new List<DomModel.CompanyNote> { companyNoteDomianModels[0] },true);

            mockCompanyNoteRepository.Verify(m => m.Add(It.IsAny<IList<DbModel.CompanyNote>>()), Times.AtLeastOnce);
        }

        [TestMethod]
        public void ModifyCompanyNote()
        {
            mockCompanyRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Company, bool>>>())).Returns(mockComapanyDbData);
            mockCompanyNoteRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.CompanyNote, bool>>>())).Returns((Expression<Func<DbModel.CompanyNote, bool>> predicate) => mockComapanyNoteDbData.Where(predicate));
            companyNoteService = new Company.Core.Services.CompanyNoteService(mockCompanyNoteRepository.Object, mockLogger.Object,mockCompanyRepository.Object);
            companyNoteDomianModels.ToList().ForEach(x => x.RecordStatus = "M");
            var response = companyNoteService.ModifyCompanyNote(companyNoteDomianModels[0].CompanyCode, new List<DomModel.CompanyNote> { companyNoteDomianModels[0] },true);

            mockCompanyNoteRepository.Verify(m => m.Update(It.IsAny<DbModel.CompanyNote>()), Times.AtLeast(1));
        }

        [TestMethod]
        public void DeleteCompanyNote()
        {
            mockCompanyRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Company, bool>>>())).Returns(mockComapanyDbData);
            mockCompanyNoteRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.CompanyNote, bool>>>())).Returns((Expression<Func<DbModel.CompanyNote, bool>> predicate) => mockComapanyNoteDbData.Where(predicate));
            companyNoteService = new Company.Core.Services.CompanyNoteService(mockCompanyNoteRepository.Object, mockLogger.Object,mockCompanyRepository.Object);
            companyNoteDomianModels.ToList().ForEach(x => x.RecordStatus = "D");
            var response = companyNoteService.DeleteCompanyNote(companyNoteDomianModels[0].CompanyCode, new List<DomModel.CompanyNote> { companyNoteDomianModels[0] },true);

            mockCompanyNoteRepository.Verify(m => m.Delete(It.IsAny<DbModel.CompanyNote>()), Times.AtLeast(1));
        }
    }
}
