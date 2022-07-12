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
    public class CompanyQualificationControllerUnitTestCase
    {
        CompanyQualificationController companyQualificationController = null;
        Mock<ServiceDomainData.ICompanyQualificationService> mockcompanyQualificationService = null;
        IList<DomModel.CompanyQualification> companyQualificationDomianModels = null;

        [TestInitialize]
        public void InitializeCompanyExpectedmarginController()
        {
            mockcompanyQualificationService = new Mock<ServiceDomainData.ICompanyQualificationService>();
            companyQualificationDomianModels = MockCompanyDomainModel.GetCompanyQualificationMockedDomainModelData();
        }
        [TestMethod]
        public void FetchCompanyQualificationListWithoutSearchValue()
        {
            var result = new List<DomModel.CompanyQualification> { companyQualificationDomianModels[0], companyQualificationDomianModels[1], companyQualificationDomianModels[2], companyQualificationDomianModels[3] };
            mockcompanyQualificationService.Setup(x => x.GetCompanyQualification(It.IsAny<DomModel.CompanyQualification>())).Returns(new Response().ToPopulate(ResponseType.Success, null, null, null, result, null, result?.Count));

            companyQualificationController = new CompanyQualificationController(mockcompanyQualificationService.Object);

            var response = companyQualificationController.Get("UK050", new DomModel.CompanyQualification());

            Assert.AreEqual("1", response.Code);
            Assert.AreEqual(4, (response.Result as List<DomModel.CompanyQualification>).Count);
        }
        //[TestMethod]
        //public void FetchCompanyQualificationListWithNullSearchModel()
        //{
        //    var result = new List<DomModel.CompanyQualification> { companyQualificationDomianModels[0], companyQualificationDomianModels[1], companyQualificationDomianModels[2], companyQualificationDomianModels[3] };

        //    mockcompanyQualificationService.Setup(x => x.GetCompanyQualification(It.IsAny<DomModel.CompanyQualification>())).Returns(new Response().ToPopulate(ResponseType.Success, null, null, null, result, null, result?.Count));

        //    companyQualificationController = new CompanyQualificationController(mockcompanyQualificationService.Object);

        //    var response = companyQualificationController.Get("UK050", null);

        //    Assert.AreEqual("1", response.Code);
        //    Assert.AreEqual(4, (response.Result as List<DomModel.CompanyQualification>).Count);
        //}

        //[TestMethod]
        //public void FetchCompanyQualificationListWithNullParameters()
        //{
        //    var result = new List<DomModel.CompanyQualification> { companyQualificationDomianModels[0] };

        //    mockcompanyQualificationService.Setup(x => x.GetCompanyQualification(It.IsAny<DomModel.CompanyQualification>())).Returns(new Response().ToPopulate(ResponseType.Success, null, null, null, result, null, result?.Count));

        //    companyQualificationController = new CompanyQualificationController(mockcompanyQualificationService.Object);

        //    var response = companyQualificationController.Get(null, null);

        //    Assert.AreEqual("11", response.Code);
        //    Assert.IsNull(response.Result);
        //}

        [TestMethod]
        public void FetchCompanyQualificationListByCompanyCode()
        {
            var result = new List<DomModel.CompanyQualification> { companyQualificationDomianModels[0] };
            mockcompanyQualificationService.Setup(x => x.GetCompanyQualification(It.IsAny<DomModel.CompanyQualification>())).Returns(new Response().ToPopulate(ResponseType.Success, null, null, null, result, null, result?.Count));

            companyQualificationController = new CompanyQualificationController(mockcompanyQualificationService.Object);

            var response = companyQualificationController.Get("UK050", new DomModel.CompanyQualification { CompanyCode = "DZ" });

            Assert.AreEqual("1", response.Code);
            Assert.AreEqual(1, (response.Result as List<DomModel.CompanyQualification>).Count);
        }
        [TestMethod]
        public void FetchCompanyQualificationListByWildCardSearchWithNameStartWith()
        {
            var result = new List<DomModel.CompanyQualification> { companyQualificationDomianModels[0], companyQualificationDomianModels[1] };
            mockcompanyQualificationService.Setup(x => x.GetCompanyQualification(It.Is<DomModel.CompanyQualification>(c => c.Qualification.StartsWith("City")))).Returns(new Response().ToPopulate(ResponseType.Success, null, null, null, result, null, result?.Count));

            companyQualificationController = new CompanyQualificationController(mockcompanyQualificationService.Object);

            var response = companyQualificationController.Get("UK050", new DomModel.CompanyQualification { Qualification = "City*" });

            Assert.AreEqual("1", response.Code);
            Assert.AreEqual(2, (response.Result as List<DomModel.CompanyQualification>).Count);
        }
        [TestMethod]
        public void FetchCompanyExpectedMarginByWildCardSearchWithNameEndWith()
        {
            var result = new List<DomModel.CompanyQualification> { companyQualificationDomianModels[0], companyQualificationDomianModels[1], companyQualificationDomianModels[2], companyQualificationDomianModels[3] };
            mockcompanyQualificationService.Setup(x => x.GetCompanyQualification(It.Is<DomModel.CompanyQualification>(c => c.Qualification.EndsWith("Mechnical")))).Returns(new Response().ToPopulate(ResponseType.Success, null, null, null, result, null, result?.Count));

            companyQualificationController = new CompanyQualificationController(mockcompanyQualificationService.Object);

            var response = companyQualificationController.Get("UK050", new DomModel.CompanyQualification { Qualification = "*Mechnical" });

            Assert.AreEqual("1", response.Code);
            Assert.AreEqual(4, (response.Result as List<DomModel.CompanyQualification>).Count);
        }
        [TestMethod]
        public void FetchCompanyExpectedmarginByWildCardSearchWithNameContains()
        {
            var result = new List<DomModel.CompanyQualification> { companyQualificationDomianModels[0], companyQualificationDomianModels[1], companyQualificationDomianModels[2], companyQualificationDomianModels[3] };
            mockcompanyQualificationService.Setup(x => x.GetCompanyQualification(It.Is<DomModel.CompanyQualification>(c => c.Qualification.Contains("Mechnical")))).Returns(new Response().ToPopulate(ResponseType.Success, null, null, null, result, null, result?.Count));

            companyQualificationController = new CompanyQualificationController(mockcompanyQualificationService.Object);
            var response = companyQualificationController.Get("UK050", new DomModel.CompanyQualification { Qualification = "*Mechnical*" });

            Assert.AreEqual("1", response.Code);
            Assert.AreEqual(4, (response.Result as List<DomModel.CompanyQualification>).Count);
        }
        [TestMethod]
        public void ThrowsExceptionWhileFetchingCompanyList()
        {
            var exception = new Exception("Exception occured while performing some operation.");
            mockcompanyQualificationService.Setup(x => x.GetCompanyQualification(It.IsAny<DomModel.CompanyQualification>())).Returns(new Response().ToPopulate(ResponseType.Error, null, null, null, null, exception, 0));

            companyQualificationController = new CompanyQualificationController(mockcompanyQualificationService.Object);
            var response = companyQualificationController.Get("UK050", new DomModel.CompanyQualification { CompanyCode = "DZ" });

            Assert.AreEqual("11", response.Code);
            Assert.IsNotNull(response.Messages);
            Assert.AreEqual("31", response.Messages.FirstOrDefault().Code);
        }

    }
}
