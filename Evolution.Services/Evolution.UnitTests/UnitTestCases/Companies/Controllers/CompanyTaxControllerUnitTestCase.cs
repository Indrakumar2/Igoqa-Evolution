using Evolution.Api.Controllers.Company;
using System;
using System.Collections.Generic;
using System.Text;
using ServiceDomainData = Evolution.Company.Domain.Interfaces.Companies;
using DomModel = Evolution.Company.Domain.Models.Companies;
using Moq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Evolution.UnitTests.Mocks.Data.Companies;
using Evolution.Common.Models.Responses;
using Evolution.Common.Extensions;
using Evolution.Common.Enums;
using System.Linq;

namespace Evolution.UnitTests.UnitTestCases.Companies.Controllers
{
    [TestClass]
    public class CompanyTaxControllerUnitTestCase
    {
        CompanyTaxController companyTaxController = null;
        Mock<ServiceDomainData.ICompanyTaxService> mockcompanyTaxService = null;
        IList<DomModel.CompanyTax> companyTaxDomianModels = null;

        [TestInitialize]
        public void InitializeCompanyExpectedmarginController()
        {
            mockcompanyTaxService = new Mock<ServiceDomainData.ICompanyTaxService>();
            companyTaxDomianModels = MockCompanyDomainModel.GetCompanyTaxMockedDomainModelData();
        }
        [TestMethod]
        public void FetchCompanyQualificationListWithoutSearchValue()
        {
            var result = new List<DomModel.CompanyTax> { companyTaxDomianModels[0], companyTaxDomianModels[1], companyTaxDomianModels[2] };
            mockcompanyTaxService.Setup(x => x.GetCompanyTax( It.IsAny<DomModel.CompanyTax>())).Returns(new Response().ToPopulate(ResponseType.Success, null, null, null, result, null, result?.Count));

            companyTaxController = new CompanyTaxController(mockcompanyTaxService.Object);

            var response = companyTaxController.Get("UK050", new DomModel.CompanyTax());

            Assert.AreEqual("1", response.Code);
            Assert.AreEqual(3, (response.Result as List<DomModel.CompanyTax>).Count);
        }
        //[TestMethod]
        //public void FetchCompanyTaxListWithNullSearchModel()
        //{
        //    var result = new List<DomModel.CompanyTax> { companyTaxDomianModels[0], companyTaxDomianModels[1], companyTaxDomianModels[2] };

        //    mockcompanyTaxService.Setup(x => x.GetCompanyTax( It.IsAny<DomModel.CompanyTax>())).Returns(new Response().ToPopulate(ResponseType.Success, null, null, null, result, null, result?.Count));

        //    companyTaxController = new CompanyTaxController(mockcompanyTaxService.Object);

        //    var response = companyTaxController.Get("UK050", null);

        //    Assert.AreEqual("1", response.Code);
        //    Assert.AreEqual(3, (response.Result as List<DomModel.CompanyTax>).Count);
        //}

        //[TestMethod]
        //public void FetchCompanyExpectedMarginListWithNullParameters()
        //{
        //    var result = new List<DomModel.CompanyTax> { companyTaxDomianModels[0] };

        //    mockcompanyTaxService.Setup(x => x.GetCompanyTax( It.IsAny<DomModel.CompanyTax>())).Returns(new Response().ToPopulate(ResponseType.Success, null, null, null, result, null, result?.Count));

        //    companyTaxController = new CompanyTaxController(mockcompanyTaxService.Object);

        //    var response = companyTaxController.Get(null, null);

        //    Assert.AreEqual("11", response.Code);
        //    Assert.IsNull(response.Result);
        //}

        [TestMethod]
        public void FetchCompanyExpectedmarginListByCompanyCode()
        {
            var result = new List<DomModel.CompanyTax> { companyTaxDomianModels[0] };
            mockcompanyTaxService.Setup(x => x.GetCompanyTax( It.IsAny<DomModel.CompanyTax>())).Returns(new Response().ToPopulate(ResponseType.Success, null, null, null, result, null, result?.Count));

            companyTaxController = new CompanyTaxController(mockcompanyTaxService.Object);

            var response = companyTaxController.Get("UK050", new DomModel.CompanyTax { CompanyCode = "DZ" });

            Assert.AreEqual("1", response.Code);
            Assert.AreEqual(1, (response.Result as List<DomModel.CompanyTax>).Count);
        }
        [TestMethod]
        public void FetchCompanyQualificationListByWildCardSearchWithNameStartWith()
        {
            var result = new List<DomModel.CompanyTax> { companyTaxDomianModels[2] };
            mockcompanyTaxService.Setup(x => x.GetCompanyTax( It.Is<DomModel.CompanyTax>(c => c.Tax.StartsWith("GST")))).Returns(new Response().ToPopulate(ResponseType.Success, null, null, null, result, null, result?.Count));

            companyTaxController = new CompanyTaxController(mockcompanyTaxService.Object);

            var response = companyTaxController.Get("UK050", new DomModel.CompanyTax { Tax = "GST*" });

            Assert.AreEqual("1", response.Code);
            Assert.AreEqual(1, (response.Result as List<DomModel.CompanyTax>).Count);
        }
        [TestMethod]
        public void FetchCompanyExpectedMarginByWildCardSearchWithNameEndWith()
        {
            var result = new List<DomModel.CompanyTax> { companyTaxDomianModels[1] };
            mockcompanyTaxService.Setup(x => x.GetCompanyTax( It.Is<DomModel.CompanyTax>(c => c.Tax.EndsWith("Exempt")))).Returns(new Response().ToPopulate(ResponseType.Success, null, null, null, result, null, result?.Count));

            companyTaxController = new CompanyTaxController(mockcompanyTaxService.Object);

            var response = companyTaxController.Get("UK050", new DomModel.CompanyTax { Tax = "*Exempt" });

            Assert.AreEqual("1", response.Code);
            Assert.AreEqual(1, (response.Result as List<DomModel.CompanyTax>).Count);
        }
        [TestMethod]
        public void FetchCompanyTaxByWildCardSearchWithNameContains()
        {
            var result = new List<DomModel.CompanyTax> { companyTaxDomianModels[0] };
            mockcompanyTaxService.Setup(x => x.GetCompanyTax( It.Is<DomModel.CompanyTax>(c => c.Tax.Contains(".5%")))).Returns(new Response().ToPopulate(ResponseType.Success, null, null, null, result, null, result?.Count));

            companyTaxController = new CompanyTaxController(mockcompanyTaxService.Object);
            var response = companyTaxController.Get("UK050", new DomModel.CompanyTax { Tax = "*.5%*" });

            Assert.AreEqual("1", response.Code);
            Assert.AreEqual(1, (response.Result as List<DomModel.CompanyTax>).Count);
        }
        [TestMethod]
        public void ThrowsExceptionWhileFetchingCompanyList()
        {
            var exception = new Exception("Exception occured while performing some operation.");
            mockcompanyTaxService.Setup(x => x.GetCompanyTax( It.IsAny<DomModel.CompanyTax>())).Returns(new Response().ToPopulate(ResponseType.Error, null, null, null, null, exception, 0));

            companyTaxController = new CompanyTaxController(mockcompanyTaxService.Object);
            var response = companyTaxController.Get("UK050", new DomModel.CompanyTax{ CompanyCode = "DZ" });

            Assert.AreEqual("11", response.Code);
            Assert.IsNotNull(response.Messages);
            Assert.AreEqual("31", response.Messages.FirstOrDefault().Code);
        }

        [TestMethod]
        public void SaveCompanies()
        {
            var companyQualificatoionToSave = new List<DomModel.CompanyTax> { new DomModel.CompanyTax { CompanyCode = "UK050", Tax = "GST-0", LastModification = null, UpdateCount = null, ModifiedBy = null } };
            var result = new List<DomModel.CompanyTax> { companyTaxDomianModels[0] };
            mockcompanyTaxService.Setup(x => x.SaveCompanyTax("UK050", It.IsAny<IList<DomModel.CompanyTax>>(),true)).Returns(new Response().ToPopulate(ResponseType.Success, null, null, null, result, null));

            companyTaxController = new CompanyTaxController(mockcompanyTaxService.Object);
            var response = companyTaxController.Post("UK050", companyQualificatoionToSave);

            Assert.AreEqual("1", response.Code);
            Assert.IsNotNull(response.Result);
            Assert.AreEqual(1, (response.Result as List<DomModel.CompanyTax>).Count);
        }

        [TestMethod]
        public void ModifyCompanies()
        {
            var companyTaxToModify = new List<DomModel.CompanyTax> { new DomModel.CompanyTax { CompanyCode = "UK050", Tax = "GST-0", LastModification = null, UpdateCount = null, ModifiedBy = "Test" } };

            companyTaxDomianModels[0].UpdateCount = Convert.ToByte(Convert.ToInt32(companyTaxToModify[0].UpdateCount) + 1);
            companyTaxDomianModels[0].ModifiedBy = "Test";
            var result = new List<DomModel.CompanyTax> { companyTaxDomianModels[0] };
            mockcompanyTaxService.Setup(x => x.ModifyCompanyTax("UK050", It.IsAny<IList<DomModel.CompanyTax>>(),true)).Returns(new Response().ToPopulate(ResponseType.Success, null, null, null, result, null));

            companyTaxController = new CompanyTaxController(mockcompanyTaxService.Object);
            var response = companyTaxController.Put("UK050", companyTaxToModify);

            Assert.AreEqual("1", response.Code);
            Assert.IsNotNull(response.Result);
            Assert.AreEqual(1, (response.Result as List<DomModel.CompanyTax>).Count);
            Assert.AreEqual("Test", (response.Result as List<DomModel.CompanyTax>)[0].ModifiedBy);
            Assert.AreEqual(1, Convert.ToInt32((response.Result as List<DomModel.CompanyTax>)[0].UpdateCount));
        }
        [TestMethod]
        public void DeleteCompanyTax()
        {
            var companyQualificatoionToDelete = new List<DomModel.CompanyTax> { new DomModel.CompanyTax { CompanyCode = "UK050", Tax = "GST-0", LastModification = null, UpdateCount = null, ModifiedBy = null } };
            var result = new List<DomModel.CompanyTax> { companyTaxDomianModels[0] };
            mockcompanyTaxService.Setup(x => x.DeleteCompanyTax("UK050", It.IsAny<IList<DomModel.CompanyTax>>(),true)).Returns(new Response().ToPopulate(ResponseType.Success, null, null, null, result, null));

            companyTaxController = new CompanyTaxController(mockcompanyTaxService.Object);
            var response = companyTaxController.Delete("UK050", companyQualificatoionToDelete);

            Assert.AreEqual("1", response.Code);
           
        }


    }
}
