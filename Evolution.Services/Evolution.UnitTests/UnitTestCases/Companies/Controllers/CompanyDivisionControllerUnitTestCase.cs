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
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using System.Linq;

namespace Evolution.UnitTests.UnitTestCases.Companies.Controllers
{
    [TestClass]
    public class CompanyDivisionControllerUnitTestCase
    {
        CompanyDivisionController companyDivisonController = null;
        Mock<ServiceDomainData.ICompanyDivisionService> mockCompanyDivisonService = null;
        IList<DomModel.CompanyDivision> companyDivisonDomianModels = null;




        [TestInitialize]
        public void InitializeCompanyController()
        {
            mockCompanyDivisonService = new Mock<ServiceDomainData.ICompanyDivisionService>();
            companyDivisonDomianModels = MockCompanyDomainModel.GetCompanyDivisionMockedDomainModelData();
        }
        [TestMethod]
        public void FetchCompanyDivisonListWithoutSearchValue()
        {
            mockCompanyDivisonService.Setup(x => x.GetCompanyDivision(It.IsAny<DomModel.CompanyDivision>())).Returns(new Response().ToPopulate(ResponseType.Success, null, null, null, companyDivisonDomianModels, null, companyDivisonDomianModels?.Count));

            companyDivisonController = new CompanyDivisionController(mockCompanyDivisonService.Object);

            var response = companyDivisonController.Get("UK050",  new DomModel.CompanyDivision());

            Assert.AreEqual("1", response.Code);
            Assert.AreEqual(3, (response.Result as List<DomModel.CompanyDivision>).Count);
        }
        //[TestMethod]
        //public void FetchCompanyDivisonListWithNullSearchModel()
        //{
        //    var result = new List<DomModel.CompanyDivision> { companyDivisonDomianModels[0] };
        //    mockCompanyDivisonService.Setup(x => x.GetCompanyDivision(It.IsAny<DomModel.CompanyDivision>())).Returns(new Response().ToPopulate(ResponseType.Success, null, null, null, result, null, result?.Count));

        //    companyDivisonController = new CompanyDivisionController(mockCompanyDivisonService.Object);

        //    var response = companyDivisonController.Get("UK050",  null);

        //    Assert.AreEqual("1", response.Code);
        //    Assert.AreEqual(1, (response.Result as List<DomModel.CompanyDivision>).Count);
        //}

        //[TestMethod]
        //public void FetchCompanyDivisonListWithNullParameters()
        //{
        //    var result = new List<DomModel.CompanyDivision> { companyDivisonDomianModels[0] };
        //    mockCompanyDivisonService.Setup(x => x.GetCompanyDivision(It.IsAny<DomModel.CompanyDivision>())).Returns(new Response().ToPopulate(ResponseType.Success, null, null, null, result, null, result?.Count));

        //    companyDivisonController = new CompanyDivisionController(mockCompanyDivisonService.Object);

        //    var response = companyDivisonController.Get(null, null);

        //    Assert.AreEqual("11", response.Code);
        //    Assert.IsNull(response.Result);
        //}

        //[TestMethod]
        //public void FetchCompanyDivisionListByCompanyCode()
        //{
        //    var result = new List<DomModel.CompanyDivision> { companyDivisonDomianModels[0] };
        //    mockCompanyDivisonService.Setup(x => x.GetCompanyDivision(It.IsAny<DomModel.CompanyDivision>())).Returns(new Response().ToPopulate(ResponseType.Success, null, null, null, result, null, result?.Count));

        //    companyDivisonController = new CompanyDivisionController(mockCompanyDivisonService.Object);

        //    var response = companyDivisonController.Get("UK050", new DomModel.CompanyDivision { CompanyCode = "UK050" });

        //    Assert.AreEqual("1", response.Code);
        //    Assert.AreEqual(1, (response.Result as List<DomModel.CompanyDivision>).Count);
        //}
        [TestMethod]
        public void FetchCompanyDivisionListByWildCardSearchWithNameStartWith()
        {
            var result = new List<DomModel.CompanyDivision> { companyDivisonDomianModels[0], companyDivisonDomianModels[2] };
            mockCompanyDivisonService.Setup(x => x.GetCompanyDivision(It.Is<DomModel.CompanyDivision>(c => c.DivisionName.StartsWith("I")))).Returns(new Response().ToPopulate(ResponseType.Success, null, null, null, result, null, result?.Count));

            companyDivisonController = new CompanyDivisionController(mockCompanyDivisonService.Object);

            var response = companyDivisonController.Get("UK050", new DomModel.CompanyDivision { DivisionName = "I*" });

            Assert.AreEqual("1", response.Code);
            Assert.AreEqual(2, (response.Result as List<DomModel.CompanyDivision>).Count);
        }
        [TestMethod]
        public void FetchCompanyDivisionListByWildCardSearchWithNameEndWith()
        {
            var result = new List<DomModel.CompanyDivision> { companyDivisonDomianModels[0], companyDivisonDomianModels[2] };
            mockCompanyDivisonService.Setup(x => x.GetCompanyDivision(It.Is<DomModel.CompanyDivision>(c => c.DivisionName.EndsWith("tion")))).Returns(new Response().ToPopulate(ResponseType.Success, null, null, null, result, null, result?.Count));

            companyDivisonController = new CompanyDivisionController(mockCompanyDivisonService.Object);

            var response = companyDivisonController.Get("UK050",  new DomModel.CompanyDivision { DivisionName = "*tion" });

            Assert.AreEqual("1", response.Code);
            Assert.AreEqual(2, (response.Result as List<DomModel.CompanyDivision>).Count);
        }
        [TestMethod]
        public void FetchCompanyDivisionListByWildCardSearchWithNameContains()
        {
            var result = new List<DomModel.CompanyDivision> { companyDivisonDomianModels[0], companyDivisonDomianModels[2] };
            mockCompanyDivisonService.Setup(x => x.GetCompanyDivision(It.Is<DomModel.CompanyDivision>(c => c.DivisionName.Contains("In")))).Returns(new Response().ToPopulate(ResponseType.Success, null, null, null, result, null, result?.Count));

            companyDivisonController = new CompanyDivisionController(mockCompanyDivisonService.Object);
            var response = companyDivisonController.Get("UK050", new DomModel.CompanyDivision { DivisionName = "*In*" });

            Assert.AreEqual("1", response.Code);
            Assert.AreEqual(2, (response.Result as List<DomModel.CompanyDivision>).Count);
        }
        [TestMethod]
        public void ThrowsExceptionWhileFetchingCompanyDivisionList()
        {
            var exception = new Exception("Exception occured while performing some operation.");
            mockCompanyDivisonService.Setup(x => x.GetCompanyDivision(It.IsAny<DomModel.CompanyDivision>())).Returns(new Response().ToPopulate(ResponseType.Error, null, null, null, null, exception, 0));

            companyDivisonController = new CompanyDivisionController(mockCompanyDivisonService.Object);
            var response = companyDivisonController.Get("UK050",  new DomModel.CompanyDivision { CompanyCode = "DZ" });

            Assert.AreEqual("11", response.Code);
            Assert.IsNotNull(response.Messages);
            Assert.AreEqual("31", response.Messages.FirstOrDefault().Code);
        }
        [TestMethod]
        public void SaveCompanyCostCenter()
        {
            var companyDivisionToSave = new List<DomModel.CompanyDivision> { new DomModel.CompanyDivision { CompanyCode = "UK050", DivisionName = "Inspection", DivisionAcReference = "1", UpdateCount = null, RecordStatus = null, LastModification = DateTime.UtcNow, ModifiedBy = null } };
            var result = new List<DomModel.CompanyDivision> { companyDivisonDomianModels[0] };
            mockCompanyDivisonService.Setup(x => x.SaveCompanyDivision("UK050",  It.IsAny<IList<DomModel.CompanyDivision>>(),true)).Returns(new Response().ToPopulate(ResponseType.Success, null, null, null, result, null));

            companyDivisonController = new CompanyDivisionController(mockCompanyDivisonService.Object);
            var response = companyDivisonController.Post("UK050", companyDivisionToSave);

            Assert.AreEqual("1", response.Code);
            Assert.IsNotNull(response.Result);
            Assert.AreEqual(1, (response.Result as List<DomModel.CompanyDivision>).Count);
        }
        [TestMethod]
        public void ModifyCompanyDivision()
        {
            var companyDivisionToModify = new List<DomModel.CompanyDivision> { new DomModel.CompanyDivision { CompanyCode = "UK050", DivisionName = "Inspection", DivisionAcReference = "1", UpdateCount = null, RecordStatus = null, LastModification = DateTime.UtcNow, ModifiedBy = null } };

            companyDivisonDomianModels[0].UpdateCount = Convert.ToByte(Convert.ToInt32(companyDivisionToModify[0].UpdateCount) + 1);
            companyDivisonDomianModels[0].ModifiedBy = "J.Blyth";
            var result = new List<DomModel.CompanyDivision> { companyDivisonDomianModels[0] };
            mockCompanyDivisonService.Setup(x => x.ModifyCompanyDivision("UK050",  It.IsAny<IList<DomModel.CompanyDivision>>(),true)).Returns(new Response().ToPopulate(ResponseType.Success, null, null, null, result, null));

            companyDivisonController = new CompanyDivisionController(mockCompanyDivisonService.Object);
            var response = companyDivisonController.Put("UK050", companyDivisionToModify);

            Assert.AreEqual("1", response.Code);
            Assert.IsNotNull(response.Result);
            Assert.AreEqual(1, (response.Result as List<DomModel.CompanyDivision>).Count);
            Assert.AreEqual("J.Blyth", (response.Result as List<DomModel.CompanyDivision>)[0].ModifiedBy);
            Assert.AreEqual(1, Convert.ToInt32((response.Result as List<DomModel.CompanyDivision>)[0].UpdateCount));
        }
        [TestMethod]
        public void DeleteCompanyDivison()
        {
            var companyDivisionToDelete = new List<DomModel.CompanyDivision> { new DomModel.CompanyDivision { CompanyCode = "UK050", DivisionName = "Inspection", DivisionAcReference = "1", UpdateCount = null, RecordStatus = null, LastModification = DateTime.UtcNow, ModifiedBy = null } };
            var result = new List<DomModel.CompanyDivision> { companyDivisonDomianModels[0] };
            mockCompanyDivisonService.Setup(x => x.DeleteCompanyDivision("UK050", It.IsAny<IList<DomModel.CompanyDivision>>(),true)).Returns(new Response().ToPopulate(ResponseType.Success, null, null, null, result, null));

            companyDivisonController = new CompanyDivisionController(mockCompanyDivisonService.Object);
            var response = companyDivisonController.Delete("UK050", companyDivisionToDelete);

            Assert.AreEqual("1", response.Code);
            
        }
    }
    }
