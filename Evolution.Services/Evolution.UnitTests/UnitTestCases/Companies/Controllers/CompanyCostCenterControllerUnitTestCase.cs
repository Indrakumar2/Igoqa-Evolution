using Evolution.Api.Controllers.Company;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using ServiceDomainData = Evolution.Company.Domain.Interfaces.Companies;
using DomModel = Evolution.Company.Domain.Models.Companies;
using Evolution.UnitTests.Mocks.Data.Companies;
using Evolution.Common.Models.Responses;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using System.Linq;

namespace Evolution.UnitTests.UnitTestCases.Companies.Controllers
{
    [TestClass]
    public  class CompanyCostCenterControllerUnitTestCase
    {
        CompanyDivisionCostCenterController companyCostCenterController = null;
        Mock<ServiceDomainData.ICompanyDivisionCostCenterService> mockCompanyDivisonCostCenterService = null;
        IList<DomModel.CompanyDivisionCostCenter> companyDivisonCostCenterDomianModels = null;


        [TestInitialize]
        public void InitializeCompanyController()
        {
            mockCompanyDivisonCostCenterService = new Mock<ServiceDomainData.ICompanyDivisionCostCenterService>();
            companyDivisonCostCenterDomianModels = MockCompanyDomainModel.GetCompanyCostCenterMockedDomainModelData();
        }
        [TestMethod]
        public void FetchCompanyCostCenterListWithoutSearchValue()
        {
            mockCompanyDivisonCostCenterService.Setup(x => x.GetCompanyCostCenter(It.IsAny<DomModel.CompanyDivisionCostCenter>())).Returns(new Response().ToPopulate(ResponseType.Success, null, null, null, companyDivisonCostCenterDomianModels, null, companyDivisonCostCenterDomianModels?.Count));

            companyCostCenterController = new CompanyDivisionCostCenterController(mockCompanyDivisonCostCenterService.Object);

            var response = companyCostCenterController.Get("UK050", "Inspection", new DomModel.CompanyDivisionCostCenter());

            Assert.AreEqual("1", response.Code);
            Assert.AreEqual(3, (response.Result as List<DomModel.CompanyDivisionCostCenter>).Count);
        }
        //[TestMethod]
        //public void FetchCompanyCostCenterListWithNullSearchModel()
        //{
        //    mockCompanyDivisonCostCenterService.Setup(x => x.GetCompanyCostCenter(It.IsAny<DomModel.CompanyDivisionCostCenter>())).Returns(new Response().ToPopulate(ResponseType.Success, null, null, null, companyDivisonCostCenterDomianModels, null, companyDivisonCostCenterDomianModels?.Count));

        //    companyCostCenterController = new CompanyDivisionCostCenterController(mockCompanyDivisonCostCenterService.Object);

        //    var response = companyCostCenterController.Get("UK050", "Inspection",null);

        //    Assert.AreEqual("1", response.Code);
        //    Assert.AreEqual(3, (response.Result as List<DomModel.CompanyDivisionCostCenter>).Count);
        //}

        //[TestMethod]
        //public void FetchCompanyCostCenterListWithNullParameters()
        //{
        //    mockCompanyDivisonCostCenterService.Setup(x => x.GetCompanyCostCenter(It.IsAny<DomModel.CompanyDivisionCostCenter>())).Returns(new Response().ToPopulate(ResponseType.Success, null, null, null, companyDivisonCostCenterDomianModels, null, companyDivisonCostCenterDomianModels?.Count));

        //    companyCostCenterController = new CompanyDivisionCostCenterController(mockCompanyDivisonCostCenterService.Object);

        //    var response = companyCostCenterController.Get(null, null, null);

        //    Assert.AreEqual("11", response.Code);
        //    Assert.IsNull(response.Result);
        //}

        //[TestMethod]
        //public void FetchCompanyCostCenterListWithNullCompanyCodeAndDivisionNameParameter()
        //{
        //    mockCompanyDivisonCostCenterService.Setup(x => x.GetCompanyCostCenter(It.IsAny<DomModel.CompanyDivisionCostCenter>())).Returns(new Response().ToPopulate(ResponseType.Success, null, null, null, companyDivisonCostCenterDomianModels, null, companyDivisonCostCenterDomianModels?.Count));

        //    companyCostCenterController = new CompanyDivisionCostCenterController(mockCompanyDivisonCostCenterService.Object);

        //    var response = companyCostCenterController.Get(null, null, new DomModel.CompanyDivisionCostCenter());

        //    Assert.AreEqual("11", response.Code);
        //    Assert.IsNull(response.Result);
        //}

        [TestMethod]
        public void FetchCompanyCostCenterListByCompanyCode()
        {
            var result = new List<DomModel.CompanyDivisionCostCenter> { companyDivisonCostCenterDomianModels[0] };
            mockCompanyDivisonCostCenterService.Setup(x => x.GetCompanyCostCenter(It.IsAny<DomModel.CompanyDivisionCostCenter>())).Returns(new Response().ToPopulate(ResponseType.Success, null, null, null, result, null, result?.Count));

            companyCostCenterController = new CompanyDivisionCostCenterController(mockCompanyDivisonCostCenterService.Object);

            var response = companyCostCenterController.Get("UK050", "Inspection",new DomModel.CompanyDivisionCostCenter { CompanyCode = "UK050" });

            Assert.AreEqual("1", response.Code);
            Assert.AreEqual(1, (response.Result as List<DomModel.CompanyDivisionCostCenter>).Count);
        }
        [TestMethod]
        public void FetchCompanyListByWildCardSearchWithNameStartWith()
        {
            var result = new List<DomModel.CompanyDivisionCostCenter> { companyDivisonCostCenterDomianModels[0] };
            mockCompanyDivisonCostCenterService.Setup(x => x.GetCompanyCostCenter(It.Is<DomModel.CompanyDivisionCostCenter>(c => c.CostCenterName.StartsWith("I")))).Returns(new Response().ToPopulate(ResponseType.Success, null, null, null, result, null, result?.Count));

            companyCostCenterController = new CompanyDivisionCostCenterController(mockCompanyDivisonCostCenterService.Object);

            var response = companyCostCenterController.Get("UK050", "Inspection",new DomModel.CompanyDivisionCostCenter { CostCenterName = "I*" });

            Assert.AreEqual("1", response.Code);
            Assert.AreEqual(1, (response.Result as List<DomModel.CompanyDivisionCostCenter>).Count);
        }
        [TestMethod]
        public void FetchCompanyCostCenterListByWildCardSearchWithNameEndWith()
        {
            var result = new List<DomModel.CompanyDivisionCostCenter> { companyDivisonCostCenterDomianModels[0], companyDivisonCostCenterDomianModels[1] };
            mockCompanyDivisonCostCenterService.Setup(x => x.GetCompanyCostCenter(It.Is<DomModel.CompanyDivisionCostCenter>(c => c.CostCenterName.EndsWith("on")))).Returns(new Response().ToPopulate(ResponseType.Success, null, null, null, result, null, result?.Count));

            companyCostCenterController = new CompanyDivisionCostCenterController(mockCompanyDivisonCostCenterService.Object);

            var response = companyCostCenterController.Get("UK050", "Inspection",new DomModel.CompanyDivisionCostCenter { CostCenterName = "*on" });

            Assert.AreEqual("1", response.Code);
            Assert.AreEqual(2, (response.Result as List<DomModel.CompanyDivisionCostCenter>).Count);
        }
        [TestMethod]
        public void FetchCompanyCostCenterListByWildCardSearchWithNameContains()
        {
            var result = new List<DomModel.CompanyDivisionCostCenter> { companyDivisonCostCenterDomianModels[0], companyDivisonCostCenterDomianModels[1] };
            mockCompanyDivisonCostCenterService.Setup(x => x.GetCompanyCostCenter(It.Is<DomModel.CompanyDivisionCostCenter>(c => c.CostCenterName.Contains("In")))).Returns(new Response().ToPopulate(ResponseType.Success, null, null, null, result, null, result?.Count));

            companyCostCenterController = new CompanyDivisionCostCenterController(mockCompanyDivisonCostCenterService.Object);
            var response = companyCostCenterController.Get("UK050", "Inspection",new DomModel.CompanyDivisionCostCenter { CostCenterName = "*In*" });

            Assert.AreEqual("1", response.Code);
            Assert.AreEqual(2, (response.Result as List<DomModel.CompanyDivisionCostCenter>).Count);
        }
        [TestMethod]
        public void ThrowsExceptionWhileFetchingCompanycostcenterList()
        {
            var exception = new Exception("Exception occured while performing some operation.");
            mockCompanyDivisonCostCenterService.Setup(x => x.GetCompanyCostCenter(It.IsAny<DomModel.CompanyDivisionCostCenter>())).Returns(new Response().ToPopulate(ResponseType.Error, null, null, null, null, exception, 0));

            companyCostCenterController = new CompanyDivisionCostCenterController(mockCompanyDivisonCostCenterService.Object);
            var response = companyCostCenterController.Get("UK050", "Inspection" ,new DomModel.CompanyDivisionCostCenter { CompanyCode = "DZ" });

            Assert.AreEqual("11", response.Code);
            Assert.IsNotNull(response.Messages);
            Assert.AreEqual("31", response.Messages.FirstOrDefault().Code);
        }
        [TestMethod]
        public void SaveCompanyCostCenter()
        {
            var companyCostCenterToSave = new List<DomModel.CompanyDivisionCostCenter> { new DomModel.CompanyDivisionCostCenter {CompanyCode = "UK050", Division = "Inspection", CostCenterCode = "1", CostCenterName = "Burgess Hill", UpdateCount = null, RecordStatus = null, LastModification = DateTime.UtcNow, ModifiedBy = null } };
            var result = new List<DomModel.CompanyDivisionCostCenter> { companyDivisonCostCenterDomianModels[0] };
            mockCompanyDivisonCostCenterService.Setup(x => x.SaveCompanyCostCenter("UK050",  "Inspection",It.IsAny<IList<DomModel.CompanyDivisionCostCenter>>(),true)).Returns(new Response().ToPopulate(ResponseType.Success, null, null, null, result, null));

            companyCostCenterController = new CompanyDivisionCostCenterController(mockCompanyDivisonCostCenterService.Object);
            var response = companyCostCenterController.Post("UK050", "Inspection",companyCostCenterToSave);

            Assert.AreEqual("1", response.Code);
            Assert.IsNotNull(response.Result);
            Assert.AreEqual(1, (response.Result as List<DomModel.CompanyDivisionCostCenter>).Count);
        }
        [TestMethod]
        public void ModifyCompanyCostCenter()
        {
            var companyCostCenterToSave = new List<DomModel.CompanyDivisionCostCenter> { new DomModel.CompanyDivisionCostCenter { CompanyCode = "UK050", Division = "Inspection", CostCenterCode = "1", CostCenterName = "Burgess Hill", UpdateCount = null, RecordStatus = null, LastModification = DateTime.UtcNow, ModifiedBy = null } };

            companyDivisonCostCenterDomianModels[0].UpdateCount = Convert.ToByte(Convert.ToInt32(companyCostCenterToSave[0].UpdateCount) + 1);
            companyDivisonCostCenterDomianModels[0].ModifiedBy = "J.Blyth";
            var result = new List<DomModel.CompanyDivisionCostCenter> { companyDivisonCostCenterDomianModels[0] };
            mockCompanyDivisonCostCenterService.Setup(x => x.ModifyCompanyCostCenter("UK050", "Inspection",It.IsAny<IList<DomModel.CompanyDivisionCostCenter>>(),true)).Returns(new Response().ToPopulate(ResponseType.Success, null, null, null, result, null));

            companyCostCenterController = new CompanyDivisionCostCenterController(mockCompanyDivisonCostCenterService.Object);
            var response = companyCostCenterController.Put("UK050", "Inspection",companyCostCenterToSave);

            Assert.AreEqual("1", response.Code);
            Assert.IsNotNull(response.Result);
            Assert.AreEqual(1, (response.Result as List<DomModel.CompanyDivisionCostCenter>).Count);
            Assert.AreEqual("J.Blyth", (response.Result as List<DomModel.CompanyDivisionCostCenter>)[0].ModifiedBy);
            Assert.AreEqual(1, Convert.ToInt32((response.Result as List<DomModel.CompanyDivisionCostCenter>)[0].UpdateCount));
        }
        [TestMethod]
        public void DeleteCompanyCostCenter()
        {
            var companyCostCenterToDelete = new List<DomModel.CompanyDivisionCostCenter> { new DomModel.CompanyDivisionCostCenter { CompanyCode = "UK050", Division = "Inspection", CostCenterCode = "1", CostCenterName = "Burgess Hill", UpdateCount = null, RecordStatus = null, LastModification = DateTime.UtcNow, ModifiedBy = null } };
            var result = new List<DomModel.CompanyDivisionCostCenter> { companyDivisonCostCenterDomianModels[0] };
            mockCompanyDivisonCostCenterService.Setup(x => x.DeleteCompanyCostCenter("UK050", "Inspection", It.IsAny<IList<DomModel.CompanyDivisionCostCenter>>(),true)).Returns(new Response().ToPopulate(ResponseType.Success, null, null, null, result, null));

            companyCostCenterController = new CompanyDivisionCostCenterController(mockCompanyDivisonCostCenterService.Object);
            var response = companyCostCenterController.Delete("UK050", "Inspection", companyCostCenterToDelete);

            Assert.AreEqual("1", response.Code);
           
        }

    }

}
