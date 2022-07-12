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
using AutoMapper;
using Microsoft.Extensions.Options;
using System.Linq.Expressions;
using Evolution.UnitTests.Mocks.Data.Companies.Db;

namespace Evolution.UnitTests.UnitTestCases.Companies.Services
{
    [TestClass]
    public class CompanyDocumentServiceUnitTestCase
    {
        ServiceDomainData.ICompanyDocumentService companyDocumentService = null;
        Mock<DomainData.ICompanyDocumentRepository> mockCompanyDocumentRepository = null;
        Mock<DomainData.ICompanyRepository> mockCompanyRepository = null;
        IList<DomModel.CompanyDocument> companyDocumentDomianModels = null;
        Mock<IAppLogger<Company.Core.Services.CompanyDocumentService>> mockLogger = null;
        Mock<IMapper> mockMapper = null;
         IOptions<DbRepository.MongoModels.Settings> mockMongoContext = null;
        IQueryable<DbModel.Company> mockComapanyDbData = null;
        IQueryable<DbModel.CompanyDocument> mockCompanyDocumentDbData = null;


        [TestInitialize]
        public void InitializeCompanyDocumentService()
        {
            mockCompanyDocumentRepository = new Mock<DomainData.ICompanyDocumentRepository>();
            mockCompanyRepository = new Mock<DomainData.ICompanyRepository>();
            companyDocumentDomianModels = MockCompanyDomainModel.GetCompanyDocumentMockedDomainModelData();
            mockLogger = new Mock<IAppLogger<Company.Core.Services.CompanyDocumentService>>();
            mockMapper = new Mock<IMapper>();
            mockMongoContext = Options.Create(new DbRepository.MongoModels.Settings() {    ConnectionString="mongodb://192.168.51.10:5300",Database="EvolutionDocuments"});
            mockComapanyDbData = MockCompany.GetCompanyMockData();
            mockCompanyDocumentDbData = MockCompany.GetCompanyDocumentMockData();
        }

        [TestMethod]
        public void FetchCompanyDocumentListWithoutSearchValue()
        {
            mockCompanyDocumentRepository.Setup(x => x.Search(It.IsAny<DomModel.CompanyDocument>())).Returns(companyDocumentDomianModels);

            companyDocumentService = new Company.Core.Services.CompanyDocumentService(mockMapper.Object, mockLogger.Object, mockCompanyDocumentRepository.Object,mockCompanyRepository.Object, mockMongoContext);

            var response = companyDocumentService.GetCompanyDocument(new DomModel.CompanyDocument());

            Assert.AreEqual("1", response.Code);
            Assert.AreEqual(4, (response.Result as List<DomModel.CompanyDocument>).Count);
        }

        [TestMethod]
        public void FetchCompanyDocumentListWithNullSearchModel()
        {
            mockCompanyDocumentRepository.Setup(x => x.Search(It.IsAny<DomModel.CompanyDocument>())).Returns(companyDocumentDomianModels);

            companyDocumentService = new Company.Core.Services.CompanyDocumentService(mockMapper.Object, mockLogger.Object, mockCompanyDocumentRepository.Object, mockCompanyRepository.Object, mockMongoContext);

            var response = companyDocumentService.GetCompanyDocument(null);

            Assert.AreEqual("1", response.Code);
            Assert.AreEqual(4, (response.Result as List<DomModel.CompanyDocument>).Count);
        }

        [TestMethod]
        public void FetchCompanyDocumentListByCompanyCode()
        {
            mockCompanyDocumentRepository.Setup(x => x.Search(It.Is<DomModel.CompanyDocument>(c => c.CompanyCode == "UK050"))).Returns(new List<DomModel.CompanyDocument> { companyDocumentDomianModels[0] });

            companyDocumentService = new Company.Core.Services.CompanyDocumentService(mockMapper.Object, mockLogger.Object, mockCompanyDocumentRepository.Object, mockCompanyRepository.Object, mockMongoContext);

            var response = companyDocumentService.GetCompanyDocument(new DomModel.CompanyDocument { CompanyCode = "UK050" });

            Assert.AreEqual("1", response.Code);
            Assert.AreEqual(1, (response.Result as List<DomModel.CompanyDocument>).Count);
        }

        [TestMethod]
        public void FetchCompanyDocumentListByWildCardSearchWithDocumentNameStartWith()
        {
            mockCompanyDocumentRepository.Setup(x => x.Search(It.Is<DomModel.CompanyDocument>(c => c.Name.StartsWith("A")))).Returns(new List<DomModel.CompanyDocument> { companyDocumentDomianModels[1] });

            companyDocumentService = new Company.Core.Services.CompanyDocumentService(mockMapper.Object, mockLogger.Object, mockCompanyDocumentRepository.Object, mockCompanyRepository.Object, mockMongoContext);

            var response = companyDocumentService.GetCompanyDocument(new DomModel.CompanyDocument { Name = "A*" });

            Assert.AreEqual("1", response.Code);
            Assert.AreEqual(1, (response.Result as List<DomModel.CompanyDocument>).Count);
        }

        [TestMethod]
        public void FetchCompanyDocumentListByWildCardSearchWithDocumentNameEndWith()
        {
            mockCompanyDocumentRepository.Setup(x => x.Search(It.Is<DomModel.CompanyDocument>(c => c.Name.EndsWith("File")))).Returns(new List<DomModel.CompanyDocument> { companyDocumentDomianModels[1], companyDocumentDomianModels[3] });

            companyDocumentService = new Company.Core.Services.CompanyDocumentService(mockMapper.Object, mockLogger.Object, mockCompanyDocumentRepository.Object, mockCompanyRepository.Object, mockMongoContext);

            var response = companyDocumentService.GetCompanyDocument(new DomModel.CompanyDocument { Name = "*File" });

            Assert.AreEqual("1", response.Code);
            Assert.AreEqual(2, (response.Result as List<DomModel.CompanyDocument>).Count);
        }

        [TestMethod]
        public void FetchCompanyDocumentListByWildCardSearchWithDocumentNameContains()
        {
            mockCompanyDocumentRepository.Setup(x => x.Search(It.Is<DomModel.CompanyDocument>(c => c.Name.Contains("sign")))).Returns(new List<DomModel.CompanyDocument> { companyDocumentDomianModels[1]});

            companyDocumentService = new Company.Core.Services.CompanyDocumentService(mockMapper.Object, mockLogger.Object, mockCompanyDocumentRepository.Object, mockCompanyRepository.Object, mockMongoContext);

            var response = companyDocumentService.GetCompanyDocument(new DomModel.CompanyDocument { Name = "*sign*" });

            Assert.AreEqual("1", response.Code);
            Assert.AreEqual(1, (response.Result as List<DomModel.CompanyDocument>).Count);
        }

        [TestMethod]
        public void ThrowsExceptionWhileFetchingCompanyDocumentList()
        {
            mockCompanyDocumentRepository.Setup(x => x.Search(It.IsAny<DomModel.CompanyDocument>())).Throws(new Exception("Exception occured while performing some operation."));
            companyDocumentService = new Company.Core.Services.CompanyDocumentService(mockMapper.Object, mockLogger.Object, mockCompanyDocumentRepository.Object, mockCompanyRepository.Object, mockMongoContext);

            var response = companyDocumentService.GetCompanyDocument(new DomModel.CompanyDocument());

            Assert.AreEqual("11", response.Code);
            Assert.IsNotNull(response.Messages);
            Assert.AreEqual("31", response.Messages.FirstOrDefault().Code);
        }

        [TestMethod]
        public void SaveCompanyDocuments()
        {

            mockCompanyRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Company, bool>>>())).Returns(mockComapanyDbData);
            mockCompanyDocumentRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.CompanyDocument, bool>>>())).Returns((Expression<Func<DbModel.CompanyDocument, bool>> predicate) => mockCompanyDocumentDbData.Where(predicate));
            


            companyDocumentService = new Company.Core.Services.CompanyDocumentService(mockMapper.Object, mockLogger.Object, mockCompanyDocumentRepository.Object, mockCompanyRepository.Object, mockMongoContext);

            companyDocumentDomianModels.ToList().ForEach(x => x.RecordStatus = "N");

            var response = companyDocumentService.SaveCompanyDocument(companyDocumentDomianModels[0].CompanyCode, new List<DomModel.CompanyDocument> { companyDocumentDomianModels[0] });

            mockCompanyDocumentRepository.Verify(m => m.Add(It.IsAny<IList<DbModel.CompanyDocument>>()), Times.AtLeastOnce);
        }

        [TestMethod]
        public void ModifyCompanyDocument()
        {
            mockCompanyDocumentRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.CompanyDocument, bool>>>())).Returns((Expression<Func<DbModel.CompanyDocument, bool>> predicate) => mockCompanyDocumentDbData.Where(predicate));
            mockCompanyRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Company, bool>>>())).Returns(mockComapanyDbData);

            companyDocumentService = new Company.Core.Services.CompanyDocumentService(mockMapper.Object, mockLogger.Object, mockCompanyDocumentRepository.Object, mockCompanyRepository.Object, mockMongoContext);
            companyDocumentDomianModels.ToList().ForEach(x => x.RecordStatus = "M");
            var response = companyDocumentService.ModifyCompanyDocument(companyDocumentDomianModels[0].CompanyCode, new List<DomModel.CompanyDocument> { companyDocumentDomianModels[0] });

            mockCompanyDocumentRepository.Verify(m => m.Update(It.IsAny<DbModel.CompanyDocument>()), Times.AtLeast(1));
        }

        [TestMethod]
        public void DeleteCompanyDocument()
        {
            mockCompanyDocumentRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.CompanyDocument, bool>>>())).Returns((Expression<Func<DbModel.CompanyDocument, bool>> predicate) => mockCompanyDocumentDbData.Where(predicate));
            mockCompanyRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Company, bool>>>())).Returns(mockComapanyDbData);
             
            companyDocumentService = new Company.Core.Services.CompanyDocumentService(mockMapper.Object, mockLogger.Object, mockCompanyDocumentRepository.Object, mockCompanyRepository.Object, mockMongoContext);
            companyDocumentDomianModels.ToList().ForEach(x => x.RecordStatus = "D");
            var response = companyDocumentService.DeleteCompanyDocument(companyDocumentDomianModels[0].CompanyCode, new List<DomModel.CompanyDocument> { companyDocumentDomianModels[0] });

            mockCompanyDocumentRepository.Verify(m => m.Delete(It.IsAny<DbModel.CompanyDocument>()), Times.AtLeast(1));
        }
    }
}
