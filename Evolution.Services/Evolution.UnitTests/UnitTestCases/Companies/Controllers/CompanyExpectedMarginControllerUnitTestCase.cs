using Evolution.Api.Controllers.Company;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using ServiceDomainData = Evolution.Company.Domain.Interfaces.Companies;
using DomModel = Evolution.Company.Domain.Models.Companies;
using Moq;
using Evolution.UnitTests.Mocks.Data.Companies;
using Evolution.Common.Models.Responses;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using System.Linq;

namespace Evolution.UnitTests.UnitTestCases.Companies.Controllers
{
    [TestClass]
    public class CompanyExpectedMarginControllerUnitTestCase
    {
        CompanyExpectedMarginController companyExpectedMarginController = null;
        Mock<ServiceDomainData.ICompanyExpectedMarginService> mockcompanyExpectedMarginService = null;
        IList<DomModel.CompanyExpectedMargin> companyExpectedMarginDomianModels = null;

        [TestInitialize]
        public void InitializeCompanyExpectedmarginController()
        {
            mockcompanyExpectedMarginService = new Mock<ServiceDomainData.ICompanyExpectedMarginService>();
            companyExpectedMarginDomianModels = MockCompanyDomainModel.GetCompanyExpectedMarginMockedDomainModelData();
        }
        [TestMethod]
        public void FetchCompanyExpectedMarginListWithoutSearchValue()
        {
            var result = new List<DomModel.CompanyExpectedMargin> { companyExpectedMarginDomianModels[0] };
            mockcompanyExpectedMarginService.Setup(x => x.GetCompanyExpectedMargin( It.IsAny<DomModel.CompanyExpectedMargin>())).Returns(new Response().ToPopulate(ResponseType.Success, null, null, null, result, null, result?.Count));

            companyExpectedMarginController = new CompanyExpectedMarginController(mockcompanyExpectedMarginService.Object);

            var response = companyExpectedMarginController.Get("UK050", new DomModel.CompanyExpectedMargin());

            Assert.AreEqual("1", response.Code);
            Assert.AreEqual(1, (response.Result as List<DomModel.CompanyExpectedMargin>).Count);
        }
        //[TestMethod]
        //public void FetchCompanyExpectedMarginListWithNullSearchModel()
        //{
        //    var result = new List<DomModel.CompanyExpectedMargin> { companyExpectedMarginDomianModels[0] };

        //    mockcompanyExpectedMarginService.Setup(x => x.GetCompanyExpectedMargin( It.IsAny<DomModel.CompanyExpectedMargin>())).Returns(new Response().ToPopulate(ResponseType.Success, null, null, null, result, null, result?.Count));

        //    companyExpectedMarginController = new CompanyExpectedMarginController(mockcompanyExpectedMarginService.Object);

        //    var response = companyExpectedMarginController.Get("UK050", null);

        //    Assert.AreEqual("1", response.Code);
        //    Assert.AreEqual(1, (response.Result as List<DomModel.CompanyExpectedMargin>).Count);
        //}

        //[TestMethod]
        //public void FetchCompanyExpectedMarginListWithNullParameters()
        //{
        //    var result = new List<DomModel.CompanyExpectedMargin> { companyExpectedMarginDomianModels[0] };

        //    mockcompanyExpectedMarginService.Setup(x => x.GetCompanyExpectedMargin( It.IsAny<DomModel.CompanyExpectedMargin>())).Returns(new Response().ToPopulate(ResponseType.Success, null, null, null, result, null, result?.Count));

        //    companyExpectedMarginController = new CompanyExpectedMarginController(mockcompanyExpectedMarginService.Object);

        //    var response = companyExpectedMarginController.Get(null, null);

        //    Assert.AreEqual("11", response.Code);
        //    Assert.IsNull( response.Result);
        //}

        [TestMethod]
        public void FetchCompanyExpectedmarginListByCompanyCode()
        {
            var result = new List<DomModel.CompanyExpectedMargin> { companyExpectedMarginDomianModels[0] };
            mockcompanyExpectedMarginService.Setup(x => x.GetCompanyExpectedMargin( It.IsAny<DomModel.CompanyExpectedMargin>())).Returns(new Response().ToPopulate(ResponseType.Success, null, null, null, result, null, result?.Count));

            companyExpectedMarginController = new CompanyExpectedMarginController(mockcompanyExpectedMarginService.Object);

            var response = companyExpectedMarginController.Get("UK050", new DomModel.CompanyExpectedMargin { CompanyCode = "DZ" });

            Assert.AreEqual("1", response.Code);
            Assert.AreEqual(1, (response.Result as List<DomModel.CompanyExpectedMargin>).Count);
        }
        [TestMethod]
        public void FetchCompanyExpectedMarginListByWildCardSearchWithNameStartWith()
        {
            var result = new List<DomModel.CompanyExpectedMargin> { companyExpectedMarginDomianModels[0] };
            mockcompanyExpectedMarginService.Setup(x => x.GetCompanyExpectedMargin( It.Is<DomModel.CompanyExpectedMargin>(c => c.MarginType.StartsWith("TIS")))).Returns(new Response().ToPopulate(ResponseType.Success, null, null, null, result, null, result?.Count));

            companyExpectedMarginController = new CompanyExpectedMarginController(mockcompanyExpectedMarginService.Object);

            var response = companyExpectedMarginController.Get("UK050", new DomModel.CompanyExpectedMargin { MarginType = "TIS*" });

            Assert.AreEqual("1", response.Code);
            Assert.AreEqual(1, (response.Result as List<DomModel.CompanyExpectedMargin>).Count);
        }
        [TestMethod]
        public void FetchCompanyExpectedMarginByWildCardSearchWithNameEndWith()
        {
            var result = new List<DomModel.CompanyExpectedMargin> { companyExpectedMarginDomianModels[0], companyExpectedMarginDomianModels[1] };
            mockcompanyExpectedMarginService.Setup(x => x.GetCompanyExpectedMargin(It.Is<DomModel.CompanyExpectedMargin>(c => c.MarginType.EndsWith("Services")))).Returns(new Response().ToPopulate(ResponseType.Success, null, null, null, result, null, result?.Count));

            companyExpectedMarginController = new CompanyExpectedMarginController(mockcompanyExpectedMarginService.Object);

            var response = companyExpectedMarginController.Get("UK050", new DomModel.CompanyExpectedMargin { MarginType = "*Services" });

            Assert.AreEqual("1", response.Code);
            Assert.AreEqual(2, (response.Result as List<DomModel.CompanyExpectedMargin>).Count);
        }
        [TestMethod]
        public void FetchCompanyExpectedmarginByWildCardSearchWithNameContains()
        {
            var result = new List<DomModel.CompanyExpectedMargin> { companyExpectedMarginDomianModels[0], companyExpectedMarginDomianModels[1] };
            mockcompanyExpectedMarginService.Setup(x => x.GetCompanyExpectedMargin(It.Is<DomModel.CompanyExpectedMargin>(c => c.MarginType.Contains("Services")))).Returns(new Response().ToPopulate(ResponseType.Success, null, null, null, result, null, result?.Count));

            companyExpectedMarginController = new CompanyExpectedMarginController(mockcompanyExpectedMarginService.Object);
            var response = companyExpectedMarginController.Get("UK050", new DomModel.CompanyExpectedMargin{ MarginType = "*Services*" });

            Assert.AreEqual("1", response.Code);
            Assert.AreEqual(2, (response.Result as List<DomModel.CompanyExpectedMargin>).Count);
        }
        [TestMethod]
        public void ThrowsExceptionWhileFetchingCompanyList()
        {
            var exception = new Exception("Exception occured while performing some operation.");
            mockcompanyExpectedMarginService.Setup(x => x.GetCompanyExpectedMargin(It.IsAny<DomModel.CompanyExpectedMargin>())).Returns(new Response().ToPopulate(ResponseType.Error, null, null, null, null, exception, 0));

            companyExpectedMarginController = new CompanyExpectedMarginController(mockcompanyExpectedMarginService.Object);
            var response = companyExpectedMarginController.Get("UK050", new DomModel.CompanyExpectedMargin { CompanyCode = "DZ" });

            Assert.AreEqual("11", response.Code);
            Assert.IsNotNull(response.Messages);
            Assert.AreEqual("31", response.Messages.FirstOrDefault().Code);
        }

        [TestMethod]
        public void SaveCompanyExpectedMargin()
        {
            var companyExpectedMarginToSave = new List<DomModel.CompanyExpectedMargin> { new DomModel.CompanyExpectedMargin { CompanyCode = "UK050", MarginType = "TIS -Technical Inspection Services Test", MinimumMargin = 15.000000M, UpdateCount = null, RecordStatus = "N", LastModification = DateTime.UtcNow, ModifiedBy = null } };
            var result = new List<DomModel.CompanyExpectedMargin> { companyExpectedMarginDomianModels[0] };
            mockcompanyExpectedMarginService.Setup(x => x.SaveCompanyExpectedMargin("UK050",It.IsAny<IList<DomModel.CompanyExpectedMargin>>(),true)).Returns(new Response().ToPopulate(ResponseType.Success, null, null, null, result, null));

            companyExpectedMarginController = new CompanyExpectedMarginController(mockcompanyExpectedMarginService.Object);
            var response = companyExpectedMarginController.Post("UK050", companyExpectedMarginToSave);

            Assert.AreEqual("1", response.Code);
            Assert.IsNotNull(response.Result);
            Assert.AreEqual(1, (response.Result as List<DomModel.CompanyExpectedMargin>).Count);
        }

        [TestMethod]
        public void ModifyCompanies()
        {
            var companyExpectedMarginToModify= new List<DomModel.CompanyExpectedMargin> { new DomModel.CompanyExpectedMargin { CompanyCode = "UK050", MarginType = "TIS -Technical Inspection Services Test", MinimumMargin = 15.000000M, UpdateCount = null, RecordStatus = null, LastModification = DateTime.UtcNow, ModifiedBy = "test" } };

            companyExpectedMarginDomianModels[0].UpdateCount = Convert.ToByte(Convert.ToInt32(companyExpectedMarginToModify[0].UpdateCount) + 1);
            companyExpectedMarginDomianModels[0].ModifiedBy = "test";
            var result = new List<DomModel.CompanyExpectedMargin> { companyExpectedMarginDomianModels[0] };
            mockcompanyExpectedMarginService.Setup(x => x.ModifyCompanyExpectedMargin("UK050", It.IsAny<IList<DomModel.CompanyExpectedMargin>>(),true)).Returns(new Response().ToPopulate(ResponseType.Success, null, null, null, result, null));

            companyExpectedMarginController = new CompanyExpectedMarginController(mockcompanyExpectedMarginService.Object);
            var response = companyExpectedMarginController.Put("UK050", companyExpectedMarginToModify);

            Assert.AreEqual("1", response.Code);
            Assert.IsNotNull(response.Result);
            Assert.AreEqual(1, (response.Result as List<DomModel.CompanyExpectedMargin>).Count);
            Assert.AreEqual("test", (response.Result as List<DomModel.CompanyExpectedMargin>)[0].ModifiedBy);
            Assert.AreEqual(1, Convert.ToInt32((response.Result as List<DomModel.CompanyExpectedMargin>)[0].UpdateCount));
        }
        [TestMethod]
        public void DeleteCompanyExpectedMargin()
        {
            var companyExpectedMarginToDelete= new List<DomModel.CompanyExpectedMargin> { new DomModel.CompanyExpectedMargin { CompanyCode = "UK050", MarginType = "TIS -Technical Inspection Services Test", MinimumMargin = 15.000000M, UpdateCount = null, RecordStatus = "N", LastModification = DateTime.UtcNow, ModifiedBy = null } };
            var result = new List<DomModel.CompanyExpectedMargin> { companyExpectedMarginDomianModels[0] };
            mockcompanyExpectedMarginService.Setup(x => x.SaveCompanyExpectedMargin("UK050", It.IsAny<IList<DomModel.CompanyExpectedMargin>>(),true)).Returns(new Response().ToPopulate(ResponseType.Success, null, null, null, result, null));

            companyExpectedMarginController = new CompanyExpectedMarginController(mockcompanyExpectedMarginService.Object);
            var response = companyExpectedMarginController.Post("UK050", companyExpectedMarginToDelete);

            Assert.AreEqual("1", response.Code);
            Assert.IsNotNull(response.Result);
            Assert.AreEqual(1, (response.Result as List<DomModel.CompanyExpectedMargin>).Count);
        }

    }
}
