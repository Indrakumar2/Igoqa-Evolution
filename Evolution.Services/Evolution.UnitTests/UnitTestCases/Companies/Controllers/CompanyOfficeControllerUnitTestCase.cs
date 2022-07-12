using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using ServiceDomainData = Evolution.Company.Domain.Interfaces.Companies;
using DomModel = Evolution.Company.Domain.Models.Companies;
using Evolution.Api.Controllers.Company;
using Moq;
using Evolution.UnitTests.Mocks.Data.Companies;
using Evolution.Common.Models.Responses;
using Evolution.Common.Extensions;
using Evolution.Common.Enums;
using System.Linq;

namespace Evolution.UnitTests.UnitTestCases.Companies.Controllers
{
    [TestClass]
    public class CompanyOfficeControllerUnitTestCase
    {
        CompanyAddressController companyofficeController = null;
        Mock<ServiceDomainData.ICompanyOfficeService> mockcompanyofficeService = null;

        IList<DomModel.CompanyAddress> companyofficeDomianModels = null;

        [TestInitialize]
        public void InitializeCompanyExpectedmarginController()
        {
            mockcompanyofficeService = new Mock<ServiceDomainData.ICompanyOfficeService>();
            companyofficeDomianModels = MockCompanyDomainModel.GetCompanyOfficeMockedDomainModelData();
        }
        [TestMethod]
        public void FetchCompanyOfficeListWithoutSearchValue()
        {
            var result = new List<DomModel.CompanyAddress> { companyofficeDomianModels[0] };
            mockcompanyofficeService.Setup(x => x.GetCompanyAddress(It.IsAny<DomModel.CompanyAddress>())).Returns(new Response().ToPopulate(ResponseType.Success, null, null, null, result, null, result?.Count));

            companyofficeController = new CompanyAddressController(mockcompanyofficeService.Object);

            var response = companyofficeController.Get("UK050", new DomModel.CompanyAddress());

            Assert.AreEqual("1", response.Code);
            Assert.AreEqual(1, (response.Result as List<DomModel.CompanyAddress>).Count);
        }
        //[TestMethod]
        //public void FetchCompanyOfficeListWithNullSearchModel()
        //{
        //    var result = new List<DomModel.CompanyAddress> { companyofficeDomianModels[0] };
        //    mockcompanyofficeService.Setup(x => x.GetCompanyAddress(It.IsAny<DomModel.CompanyAddress>())).Returns(new Response().ToPopulate(ResponseType.Success, null, null, null, result, null, result?.Count));

        //    companyofficeController = new CompanyAddressController(mockcompanyofficeService.Object);

        //    var response = companyofficeController.Get("UK050", null);

        //    Assert.AreEqual("1", response.Code);
        //    Assert.AreEqual(1, (response.Result as List<DomModel.CompanyAddress>).Count);
        //}
        //[TestMethod]
        //public void FetchCompanyOfficeListWithNullSearchModelandNullSearchCode()
        //{
        //    var result = new List<DomModel.CompanyAddress> { companyofficeDomianModels[0], companyofficeDomianModels[1]};
        //    mockcompanyofficeService.Setup(x => x.GetCompanyAddress(It.IsAny<DomModel.CompanyAddress>())).Returns(new Response().ToPopulate(ResponseType.Success, null, null, null, result, null, result?.Count));

        //    companyofficeController = new CompanyAddressController(mockcompanyofficeService.Object);

        //    var response = companyofficeController.Get(null, null);

        //    Assert.AreEqual("11", response.Code);
        //    Assert.IsNull(response.Result);
        //}

        [TestMethod]
        public void FetchCompanyOfficeListByCompanyCode()
        {
            var result = new List<DomModel.CompanyAddress> { companyofficeDomianModels[0] };
            mockcompanyofficeService.Setup(x => x.GetCompanyAddress(It.IsAny<DomModel.CompanyAddress>())).Returns(new Response().ToPopulate(ResponseType.Success, null, null, null, result, null, result?.Count));

            companyofficeController = new CompanyAddressController(mockcompanyofficeService.Object);

            var response = companyofficeController.Get("UK050", new DomModel.CompanyAddress { CompanyCode = "DZ" });

            Assert.AreEqual("1", response.Code);
            Assert.AreEqual(1, (response.Result as List<DomModel.CompanyAddress>).Count);
        }
        [TestMethod]
        public void FetchCompanyAddressListByWildCardSearchWithNameStartWith()
        {
            var result = new List<DomModel.CompanyAddress> { companyofficeDomianModels[0] };
            mockcompanyofficeService.Setup(x => x.GetCompanyAddress(It.Is<DomModel.CompanyAddress>(c => c.OfficeName.StartsWith("Hay")))).Returns(new Response().ToPopulate(ResponseType.Success, null, null, null, result, null, result?.Count));

            companyofficeController = new CompanyAddressController(mockcompanyofficeService.Object);

            var response = companyofficeController.Get("UK050", new DomModel.CompanyAddress { OfficeName = "Hay*" });

            Assert.AreEqual("1", response.Code);
            Assert.AreEqual(1, (response.Result as List<DomModel.CompanyAddress>).Count);
        }
        [TestMethod]
        public void FetchCompanyOfiiceByWildCardSearchWithNameEndWith()
        {
            var result = new List<DomModel.CompanyAddress> { companyofficeDomianModels[0] };
            mockcompanyofficeService.Setup(x => x.GetCompanyAddress(It.Is<DomModel.CompanyAddress>(c => c.OfficeName.EndsWith("Heath")))).Returns(new Response().ToPopulate(ResponseType.Success, null, null, null, result, null, result?.Count));

            companyofficeController = new CompanyAddressController(mockcompanyofficeService.Object);

            var response = companyofficeController.Get("UK050", new DomModel.CompanyAddress { OfficeName = "*Heath" });

            Assert.AreEqual("1", response.Code);
            Assert.AreEqual(1, (response.Result as List<DomModel.CompanyAddress>).Count);
        }
        [TestMethod]
        public void FetchCompanyOfficeByWildCardSearchWithNameContains()
        {
            var result = new List<DomModel.CompanyAddress> { companyofficeDomianModels[0] };
            mockcompanyofficeService.Setup(x => x.GetCompanyAddress(It.Is<DomModel.CompanyAddress>(c => c.OfficeName.Contains("Heath")))).Returns(new Response().ToPopulate(ResponseType.Success, null, null, null, result, null, result?.Count));

            companyofficeController = new CompanyAddressController(mockcompanyofficeService.Object);
            var response = companyofficeController.Get("UK050", new DomModel.CompanyAddress { OfficeName = "*Heath*" });

            Assert.AreEqual("1", response.Code);
            Assert.AreEqual(1, (response.Result as List<DomModel.CompanyAddress>).Count);
        }
        [TestMethod]
        public void ThrowsExceptionWhileFetchingCompanyList()
        {
            var exception = new Exception("Exception occured while performing some operation.");
            mockcompanyofficeService.Setup(x => x.GetCompanyAddress(It.IsAny<DomModel.CompanyAddress>())).Returns(new Response().ToPopulate(ResponseType.Error, null, null, null, null, exception, 0));

            companyofficeController = new CompanyAddressController(mockcompanyofficeService.Object);
            var response = companyofficeController.Get("UK050", new DomModel.CompanyAddress { CompanyCode = "DZ" });

            Assert.AreEqual("11", response.Code);
            Assert.IsNotNull(response.Messages);
            Assert.AreEqual("31", response.Messages.FirstOrDefault().Code);
        }
        [TestMethod]
   
        public void SaveAddress()
        {
            var companyQualificatoionToSave = new List<DomModel.CompanyAddress> { new DomModel.CompanyAddress { CompanyCode = "UK050", OfficeName = "Haywards Heath", AccountRef = "HH01", FullAddress = "Intertek Inspection Services UK Limited (Formerly Moody International Limited)  The Forum,  277 London Road,  Burgess Hill,  West Sussex  RH15 9QU  UK", City = "Haywards Heath", PostalCode = "RH16 3BW", UpdateCount = null, RecordStatus = null, LastModification = DateTime.UtcNow, ModifiedBy = null } };
            var result = new List<DomModel.CompanyAddress> { companyofficeDomianModels[0] };
            mockcompanyofficeService.Setup(x => x.SaveCompanyAddress("UK050", It.IsAny<IList<DomModel.CompanyAddress>>(), true,true)).Returns(new Response().ToPopulate(ResponseType.Success, null, null, null, result, null));

            companyofficeController = new CompanyAddressController(mockcompanyofficeService.Object);
            var response = companyofficeController.Post("UK050", companyQualificatoionToSave);

            Assert.AreEqual("1", response.Code);
            Assert.IsNotNull(response.Result);
            Assert.AreEqual(1, (response.Result as List<DomModel.CompanyAddress>).Count);
        }
        [TestMethod]
        public void ModifyCompanyAddress()
        {
            var companyAddressToModify = new List<DomModel.CompanyAddress> { new DomModel.CompanyAddress { CompanyCode = "UK050", OfficeName = "Haywards Heath", AccountRef = "HH01", FullAddress = "Intertek Inspection Services UK Limited (Formerly Moody International Limited)  The Forum,  277 London Road,  Burgess Hill,  West Sussex  RH15 9QU  UK", City = "Haywards Heath", PostalCode = "RH16 3BW", UpdateCount = null, RecordStatus = null, LastModification = DateTime.UtcNow, ModifiedBy = null } };

            companyofficeDomianModels[0].UpdateCount = Convert.ToByte(Convert.ToInt32(companyAddressToModify[0].UpdateCount) + 1);
            companyofficeDomianModels[0].ModifiedBy = "Test";
            var result = new List<DomModel.CompanyAddress> { companyofficeDomianModels[0] };
            mockcompanyofficeService.Setup(x => x.ModifyCompanyAddress("UK050", It.IsAny<IList<DomModel.CompanyAddress>>(), true,true)).Returns(new Response().ToPopulate(ResponseType.Success, null, null, null, result, null));

            companyofficeController = new CompanyAddressController(mockcompanyofficeService.Object);
            var response = companyofficeController.Put("UK050", companyAddressToModify);

            Assert.AreEqual("1", response.Code);
            Assert.IsNotNull(response.Result);
            Assert.AreEqual(1, (response.Result as List<DomModel.CompanyAddress>).Count);
            Assert.AreEqual("Test", (response.Result as List<DomModel.CompanyAddress>)[0].ModifiedBy);
            Assert.AreEqual(1, Convert.ToInt32((response.Result as List<DomModel.CompanyAddress>)[0].UpdateCount));
        }
        [TestMethod]
        public void DeleteCompanyTax()
        {
            var companyAddressToDelete = new List<DomModel.CompanyAddress> { new DomModel.CompanyAddress { CompanyCode = "UK050", OfficeName = "Haywards Heath", AccountRef = "HH01", FullAddress = "Intertek Inspection Services UK Limited (Formerly Moody International Limited)  The Forum,  277 London Road,  Burgess Hill,  West Sussex  RH15 9QU  UK", City = "Haywards Heath", PostalCode = "RH16 3BW", UpdateCount = null, RecordStatus = null, LastModification = DateTime.UtcNow, ModifiedBy = null } };
            var result = new List<DomModel.CompanyAddress> { companyofficeDomianModels[0] };
            mockcompanyofficeService.Setup(x => x.DeleteCompanyAddress("UK050", It.IsAny<IList<DomModel.CompanyAddress>>(), true,true)).Returns(new Response().ToPopulate(ResponseType.Success, null, null, null, result, null));

            companyofficeController = new CompanyAddressController(mockcompanyofficeService.Object);
            var response = companyofficeController.Delete("UK050", companyAddressToDelete);

            Assert.AreEqual("1", response.Code);

        }

    } 
}
