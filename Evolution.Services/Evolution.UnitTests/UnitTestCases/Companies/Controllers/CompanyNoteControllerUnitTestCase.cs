using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using ServiceDomainData = Evolution.Company.Domain.Interfaces.Companies;
using DomModel = Evolution.Company.Domain.Models.Companies;
using Evolution.Api.Controllers.Company;
using Evolution.UnitTests.Mocks.Data.Companies;
using Moq;
using Evolution.Common.Models.Responses;
using Evolution.Common.Extensions;
using Evolution.Common.Enums;
using System.Linq;

namespace Evolution.UnitTests.UnitTestCases.Companies.Controllers
{
    [TestClass]
    public class CompanyNoteControllerUnitTestCase
    {
        CompanyNoteController companyNoteController = null;
        Mock<ServiceDomainData.ICompanyNoteService> mockcompanyNoteService = null;
        IList<DomModel.CompanyNote> companyNoteDomianModels = null;

        [TestInitialize]
        public void InitializeCompanyNoteController()
        {
            mockcompanyNoteService = new Mock<ServiceDomainData.ICompanyNoteService>();
            companyNoteDomianModels =  MockCompanyDomainModel.GetCompanyNoteMockedDomainModelData();
        }
        [TestMethod]
        public void FetchCompanyNoteListWithoutSearchValue()
        {
            var result = new List<DomModel.CompanyNote> { companyNoteDomianModels[0] };
            mockcompanyNoteService.Setup(x => x.GetCompanyNote( It.IsAny<DomModel.CompanyNote>())).Returns(new Response().ToPopulate(ResponseType.Success, null, null, null, result, null, result?.Count));

            companyNoteController = new CompanyNoteController(mockcompanyNoteService.Object);

            var response = companyNoteController.Get("UK050", new DomModel.CompanyNote());

            Assert.AreEqual("1", response.Code);
            Assert.AreEqual(1, (response.Result as List<DomModel.CompanyNote>).Count);
        }
        //[TestMethod]
        //public void FetchCompanyExpectedMarginListWithNullSearchModel()
        //{
        //    var result = new List<DomModel.CompanyNote> { companyNoteDomianModels[0] };
        //    mockcompanyNoteService.Setup(x => x.GetCompanyNote( It.IsAny<DomModel.CompanyNote>())).Returns(new Response().ToPopulate(ResponseType.Success, null, null, null, result, null, result?.Count));

        //    companyNoteController = new CompanyNoteController(mockcompanyNoteService.Object);

        //    var response = companyNoteController.Get("UK050", null);

        //    Assert.AreEqual("1", response.Code);
        //    Assert.AreEqual(1, (response.Result as List<DomModel.CompanyNote>).Count);
        //}

        [TestMethod]
        public void FetchCompanyNoteListByCompanyCode()
        {
            var result = new List<DomModel.CompanyNote> { companyNoteDomianModels[0] };
            mockcompanyNoteService.Setup(x => x.GetCompanyNote( It.IsAny<DomModel.CompanyNote>())).Returns(new Response().ToPopulate(ResponseType.Success, null, null, null, result, null, result?.Count));

            companyNoteController = new CompanyNoteController(mockcompanyNoteService.Object);

            var response = companyNoteController.Get("UK050", new DomModel.CompanyNote { CompanyCode = "DZ" });

            Assert.AreEqual("1", response.Code);
            Assert.AreEqual(1, (response.Result as List<DomModel.CompanyNote>).Count);
        }
        [TestMethod]
        public void FetchCompanyNoteListByWildCardSearchWithNameStartWith()
        {
            var result = new List<DomModel.CompanyNote> { companyNoteDomianModels[0] };
            mockcompanyNoteService.Setup(x => x.GetCompanyNote( It.Is<DomModel.CompanyNote>(c => c.Notes.StartsWith("I")))).Returns(new Response().ToPopulate(ResponseType.Success, null, null, null, result, null, result?.Count));

            companyNoteController = new CompanyNoteController(mockcompanyNoteService.Object);

            var response = companyNoteController.Get("UK050", new DomModel.CompanyNote { Notes = "I*" });

            Assert.AreEqual("1", response.Code);
            Assert.AreEqual(1, (response.Result as List<DomModel.CompanyNote>).Count);
        }
        [TestMethod]
        public void FetchCompanyNoteByWildCardSearchWithNameEndWith()
        {
            var result = new List<DomModel.CompanyNote> {  companyNoteDomianModels[1] };
            mockcompanyNoteService.Setup(x => x.GetCompanyNote( It.Is<DomModel.CompanyNote>(c => c.Notes.EndsWith("File")))).Returns(new Response().ToPopulate(ResponseType.Success, null, null, null, result, null, result?.Count));

            companyNoteController = new CompanyNoteController(mockcompanyNoteService.Object);

            var response = companyNoteController.Get("UK051", new DomModel.CompanyNote { Notes = "*File" });

            Assert.AreEqual("1", response.Code);
            Assert.AreEqual(1, (response.Result as List<DomModel.CompanyNote>).Count);
        }
        [TestMethod]
        public void FetchCompanyNoteByWildCardSearchWithNameContains()
        {
            var result = new List<DomModel.CompanyNote> { companyNoteDomianModels[0] };
            mockcompanyNoteService.Setup(x => x.GetCompanyNote( It.Is<DomModel.CompanyNote>(c => c.Notes.Contains("Invoice")))).Returns(new Response().ToPopulate(ResponseType.Success, null, null, null, result, null, result?.Count));

            companyNoteController = new CompanyNoteController(mockcompanyNoteService.Object);
            var response = companyNoteController.Get("UK050", new DomModel.CompanyNote { Notes = "*Invoice*" });

            Assert.AreEqual("1", response.Code);
            Assert.AreEqual(1, (response.Result as List<DomModel.CompanyNote>).Count);
        }
        [TestMethod]
        public void ThrowsExceptionWhileFetchingCompanyList()
        {
            var exception = new Exception("Exception occured while performing some operation.");
            mockcompanyNoteService.Setup(x => x.GetCompanyNote( It.IsAny<DomModel.CompanyNote>())).Returns(new Response().ToPopulate(ResponseType.Error, null, null, null, null, exception, 0));

            companyNoteController = new CompanyNoteController(mockcompanyNoteService.Object);
            var response = companyNoteController.Get("UK050", new DomModel.CompanyNote { CompanyCode = "DZ" });

            Assert.AreEqual("11", response.Code);
            Assert.IsNotNull(response.Messages);
            Assert.AreEqual("31", response.Messages.FirstOrDefault().Code);
        }

        [TestMethod]
        public void SaveCompanyNote()
        {
            var companyNoteToSave = new List<DomModel.CompanyNote> { new DomModel.CompanyNote { CompanyCode = "UK050", Notes = "InvoiceTest", UpdateCount = null, RecordStatus = null, LastModification = DateTime.UtcNow, ModifiedBy = null } };
            var result = new List<DomModel.CompanyNote> { companyNoteDomianModels[0] };
            mockcompanyNoteService.Setup(x => x.SaveCompanyNote( "UK050" ,It.IsAny<IList<DomModel.CompanyNote>>(),true)).Returns(new Response().ToPopulate(ResponseType.Success, null, null, null, result, null));

            companyNoteController = new CompanyNoteController(mockcompanyNoteService.Object);
            var response = companyNoteController.Post("UK050", companyNoteToSave,true);

            Assert.AreEqual("1", response.Code);
            Assert.IsNotNull(response.Result);
            Assert.AreEqual(1, (response.Result as List<DomModel.CompanyNote>).Count);
        }

        [TestMethod]
        public void ModifyCompanies()
        {
            var companyNoteToModify = new List<DomModel.CompanyNote> { new DomModel.CompanyNote { CompanyCode = "UK050", Notes = "InvoiceTest", UpdateCount = null, RecordStatus = null, LastModification = DateTime.UtcNow, ModifiedBy = null } };

            companyNoteDomianModels[0].UpdateCount = Convert.ToByte(Convert.ToInt32(companyNoteDomianModels[0].UpdateCount) + 1);
            companyNoteDomianModels[0].ModifiedBy = "test";
            var result = new List<DomModel.CompanyNote> { companyNoteDomianModels[0] };
            mockcompanyNoteService.Setup(x => x.ModifyCompanyNote("UK050", It.IsAny<IList<DomModel.CompanyNote>>(),true)).Returns(new Response().ToPopulate(ResponseType.Success, null, null, null, result, null));

            companyNoteController = new CompanyNoteController(mockcompanyNoteService.Object);
            var response = companyNoteController.Put("UK050", companyNoteToModify,true);

            Assert.AreEqual("1", response.Code);
            Assert.IsNotNull(response.Result);
            Assert.AreEqual(1, (response.Result as List<DomModel.CompanyNote>).Count);
            Assert.AreEqual("test", (response.Result as List<DomModel.CompanyNote>)[0].ModifiedBy);
            Assert.AreEqual(1, Convert.ToInt32((response.Result as List<DomModel.CompanyNote>)[0].UpdateCount));
        }
        [TestMethod]
        public void DeleteCompanyNote()
        {
            var companyNoteToDelete = new List<DomModel.CompanyNote> { new DomModel.CompanyNote { CompanyCode = "UK050", Notes = "InvoiceTest", UpdateCount = null, RecordStatus = null, LastModification = DateTime.UtcNow, ModifiedBy = null } };
            var result = new List<DomModel.CompanyNote> { companyNoteDomianModels[0] };
            mockcompanyNoteService.Setup(x => x.DeleteCompanyNote("UK050", It.IsAny<IList<DomModel.CompanyNote>>(),true)).Returns(new Response().ToPopulate(ResponseType.Success, null, null, null, result, null));

            companyNoteController = new CompanyNoteController(mockcompanyNoteService.Object);
            var response = companyNoteController.Delete("UK050", companyNoteToDelete,true);

            Assert.AreEqual("1", response.Code);
          
        }



    }
}

