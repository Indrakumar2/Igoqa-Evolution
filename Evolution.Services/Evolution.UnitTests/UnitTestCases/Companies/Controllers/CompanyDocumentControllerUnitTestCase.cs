using Evolution.Api.Controllers.Company;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Common.Models.Responses;
using Evolution.UnitTests.Mocks.Data.Companies;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DomModel = Evolution.Company.Domain.Models.Companies;
using ServiceDomainData = Evolution.Company.Domain.Interfaces.Companies;

namespace Evolution.UnitTests.UnitTestCases.Companies.Controllers
{
    [TestClass]
    public  class CompanyDocumentControllerUnitTestCase
    {
        CompanyDocumentController companyDocumentController = null;
        Mock<ServiceDomainData.ICompanyDocumentService> mockCompanyDocumentService = null;
        IList<DomModel.CompanyDocument> companyDocumentDomianModels = null;

        [TestInitialize]
        public void InitializeCompanyController()
        {
            mockCompanyDocumentService = new Mock<ServiceDomainData.ICompanyDocumentService>();
            companyDocumentDomianModels = MockCompanyDomainModel.GetCompanyDocumentMockedDomainModelData();
        }

        [TestMethod]
        public void FetchCompanyDocumentListWithoutSearchValue()
        {
            var result = new List<DomModel.CompanyDocument> { companyDocumentDomianModels[0] };
            mockCompanyDocumentService.Setup(x => x.GetCompanyDocument(It.IsAny<DomModel.CompanyDocument>())).Returns(new Response().ToPopulate(ResponseType.Success, null, null, null, result, null, result?.Count));

            companyDocumentController = new CompanyDocumentController(mockCompanyDocumentService.Object);

            var response = companyDocumentController.Get("UK050",new DomModel.CompanyDocument());

            Assert.AreEqual("1", response.Code);
            Assert.AreEqual(1, (response.Result as List<DomModel.CompanyDocument>).Count);
        }

        //[TestMethod]
        //public void FetchCompanyDocumentListWithNullSearchModel()
        //{
        //    var result = new List<DomModel.CompanyDocument> { companyDocumentDomianModels[0] };
        //    mockCompanyDocumentService.Setup(x => x.GetCompanyDocument(It.IsAny<DomModel.CompanyDocument>())).Returns(new Response().ToPopulate(ResponseType.Success, null, null, null, result, null, result?.Count));

        //    companyDocumentController = new CompanyDocumentController(mockCompanyDocumentService.Object);

        //    var response = companyDocumentController.Get("UK050",null);

        //    Assert.AreEqual("1", response.Code);
        //    Assert.AreEqual(1, (response.Result as List<DomModel.CompanyDocument>).Count);
        //}

        //[TestMethod]
        //public void FetchCompanyDocumentListWithNullSearchAndCompanyCodeModel()
        //{
        //    var result = new List<DomModel.CompanyDocument> { companyDocumentDomianModels[0] };
        //    mockCompanyDocumentService.Setup(x => x.GetCompanyDocument(It.IsAny<DomModel.CompanyDocument>())).Returns(new Response().ToPopulate(ResponseType.Success, null, null, null, result, null, result?.Count));

        //    companyDocumentController = new CompanyDocumentController(mockCompanyDocumentService.Object);

        //    var response = companyDocumentController.Get(null, null);

        //    Assert.AreEqual("11", response.Code);
        //    Assert.IsNull(response.Result);
        //}

        [TestMethod]
        public void FetchCompanyDocumentListByCompanyCode()
        {
            var result = new List<DomModel.CompanyDocument> { companyDocumentDomianModels[0] };
            mockCompanyDocumentService.Setup(x => x.GetCompanyDocument(It.IsAny<DomModel.CompanyDocument>())).Returns(new Response().ToPopulate(ResponseType.Success, null, null, null, result, null, result?.Count));

            companyDocumentController = new CompanyDocumentController(mockCompanyDocumentService.Object);

            var response = companyDocumentController.Get("UK050",new DomModel.CompanyDocument { CompanyCode = "DZ" });

            Assert.AreEqual("1", response.Code);
            Assert.AreEqual(1, (response.Result as List<DomModel.CompanyDocument>).Count);
        }

        [TestMethod]
        public void FetchCompanyDocumentListByWildCardSearchWithNameStartWith()
        {
            var result = new List<DomModel.CompanyDocument> { companyDocumentDomianModels[0] };
            mockCompanyDocumentService.Setup(x => x.GetCompanyDocument(It.Is<DomModel.CompanyDocument>(c =>c.CompanyCode== "UK050" && c.Name.StartsWith("I")))).Returns(new Response().ToPopulate(ResponseType.Success, null, null, null, result, null, result?.Count));

            companyDocumentController = new CompanyDocumentController(mockCompanyDocumentService.Object);

            var response = companyDocumentController.Get("UK050",new DomModel.CompanyDocument { Name = "I*" });

            Assert.AreEqual("1", response.Code);
            Assert.AreEqual(1, (response.Result as List<DomModel.CompanyDocument>).Count);
        }

        [TestMethod]
        public void FetchCompanyListByWildCardSearchWithNameEndWith()
        {
            var result = new List<DomModel.CompanyDocument> { companyDocumentDomianModels[0], companyDocumentDomianModels[1] };
            mockCompanyDocumentService.Setup(x => x.GetCompanyDocument(It.Is<DomModel.CompanyDocument>(c => c.Name.EndsWith("File")))).Returns(new Response().ToPopulate(ResponseType.Success, null, null, null, result, null, result?.Count));

            companyDocumentController = new CompanyDocumentController(mockCompanyDocumentService.Object);

            var response = companyDocumentController.Get("UK051",new DomModel.CompanyDocument { Name = "*File" });

            Assert.AreEqual("1", response.Code);
            Assert.AreEqual(2, (response.Result as List<DomModel.CompanyDocument>).Count);
        }

        [TestMethod]
        public void FetchCompanyListByWildCardSearchWithNameContains()
        {
            var result = new List<DomModel.CompanyDocument> { companyDocumentDomianModels[2] };
            mockCompanyDocumentService.Setup(x => x.GetCompanyDocument(It.Is<DomModel.CompanyDocument>(c => c.CompanyCode == "UK052" && c.Name.Contains("Copy")))).Returns(new Response().ToPopulate(ResponseType.Success, null, null, null, result, null, result?.Count));

            companyDocumentController = new CompanyDocumentController(mockCompanyDocumentService.Object);
            var response = companyDocumentController.Get("UK052",new DomModel.CompanyDocument { Name = "*Copy*" });

            Assert.AreEqual("1", response.Code);
            Assert.AreEqual(1, (response.Result as List<DomModel.CompanyDocument>).Count);
        }

        [TestMethod]
        public void ThrowsExceptionWhileFetchingCompanyList()
        {
            var exception = new Exception("Exception occured while performing some operation.");
            mockCompanyDocumentService.Setup(x => x.GetCompanyDocument(It.IsAny<DomModel.CompanyDocument>())).Returns(new Response().ToPopulate(ResponseType.Error, null, null, null, null, exception, 0));

            companyDocumentController = new CompanyDocumentController(mockCompanyDocumentService.Object);
            var response = companyDocumentController.Get("UK050",new DomModel.CompanyDocument { CompanyCode = "DZ" });

            Assert.AreEqual("11", response.Code);
            Assert.IsNotNull(response.Messages);
            Assert.AreEqual("31", response.Messages.FirstOrDefault().Code);
        }

        [TestMethod]
        public void SaveCompanyDocument()
        {
            var companiesDocumentToModify = new List<DomModel.CompanyDocument> { new DomModel.CompanyDocument { CompanyCode = "UK050", Name = "Invoice", DocumentType = "Email", IsVisibleToCustomer = true, IsVisibleToTS = true, DocumentSize = 1000, UploadedOn = DateTime.UtcNow, UpdateCount = null, RecordStatus = null, LastModification = DateTime.UtcNow, ModifiedBy = null } };
            var result = new List<DomModel.CompanyDocument> { companyDocumentDomianModels[0] };
            mockCompanyDocumentService.Setup(x => x.SaveCompanyDocument("UK050",It.IsAny<IList<DomModel.CompanyDocument>>(),true)).Returns(new Response().ToPopulate(ResponseType.Success, null, null, null, result, null));

            companyDocumentController = new CompanyDocumentController(mockCompanyDocumentService.Object);
            var response = companyDocumentController.Post("UK050", companiesDocumentToModify);

            Assert.AreEqual("1", response.Code);
            Assert.IsNotNull(response.Result);
            Assert.AreEqual(1, (response.Result as List<DomModel.CompanyDocument>).Count);
        }

        [TestMethod]
        public void ModifyCompanyDocument()
        {
            var companiesToSave = new List<DomModel.CompanyDocument> { new DomModel.CompanyDocument { CompanyCode = "UK050", Name = "Invoice", DocumentType = "Email", IsVisibleToCustomer = true, IsVisibleToTS = true, DocumentSize = 1000, UploadedOn = DateTime.UtcNow, UpdateCount = null, RecordStatus = null, LastModification = DateTime.UtcNow, ModifiedBy = "Test" } };

            companyDocumentDomianModels[0].UpdateCount = Convert.ToByte(Convert.ToInt32(companiesToSave[0].UpdateCount) + 1);
            companyDocumentDomianModels[0].ModifiedBy = "Test";
            var result = new List<DomModel.CompanyDocument> { companyDocumentDomianModels[0] };
            mockCompanyDocumentService.Setup(x => x.ModifyCompanyDocument("UK050",It.IsAny<IList<DomModel.CompanyDocument>>(),true)).Returns(new Response().ToPopulate(ResponseType.Success, null, null, null, result, null));

            companyDocumentController = new CompanyDocumentController(mockCompanyDocumentService.Object);
            var response = companyDocumentController.Put("UK050",companiesToSave);

            Assert.AreEqual("1", response.Code);
            Assert.IsNotNull(response.Result);
            Assert.AreEqual(1, (response.Result as List<DomModel.CompanyDocument>).Count);
            Assert.AreEqual("Test", (response.Result as List<DomModel.CompanyDocument>)[0].ModifiedBy);
            Assert.AreEqual(1, Convert.ToInt32((response.Result as List<DomModel.CompanyDocument>)[0].UpdateCount));
        }
        [TestMethod]
        public void DeleteCompanyDocument()
        {
            var companiesDocumentToDelete = new List<DomModel.CompanyDocument> { new DomModel.CompanyDocument { CompanyCode = "UK050", Name = "Invoice", DocumentType = "Email", IsVisibleToCustomer = true, IsVisibleToTS = true, DocumentSize = 1000, UploadedOn = DateTime.UtcNow, UpdateCount = null, RecordStatus = null, LastModification = DateTime.UtcNow, ModifiedBy = null } };
            var result = new List<DomModel.CompanyDocument> { companyDocumentDomianModels[0] };
            mockCompanyDocumentService.Setup(x => x.DeleteCompanyDocument("UK050", It.IsAny<IList<DomModel.CompanyDocument>>(),true)).Returns(new Response().ToPopulate(ResponseType.Success, null, null, null, result, null));

            companyDocumentController = new CompanyDocumentController(mockCompanyDocumentService.Object);
            var response = companyDocumentController.Delete("UK050", companiesDocumentToDelete);

            Assert.AreEqual("1", response.Code);
            
        }
    }
}
