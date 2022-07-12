using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using ServiceDomainData = Evolution.Company.Domain.Interfaces.Companies;
using DomModel = Evolution.Company.Domain.Models.Companies;
using Moq;
using Evolution.Api.Controllers.Company;
using Evolution.UnitTests.Mocks.Data.Companies;
using Evolution.Common.Models.Responses;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using System.Linq;

namespace Evolution.UnitTests.UnitTestCases.Companies.Controllers
{
    [TestClass]
    public class CompanyPayrollControllerUnitTestCase
    {
        CompanyPayrollController companyPayrollController = null;
        Mock<ServiceDomainData.ICompanyPayrollService> mockcompanyPayrollService = null;
        IList<DomModel.CompanyPayroll> companyPayrollDomianModels = null;

        [TestInitialize]
        public void InitializeCompanyExpectedmarginController()
        {
            mockcompanyPayrollService = new Mock<ServiceDomainData.ICompanyPayrollService>();
            companyPayrollDomianModels = MockCompanyDomainModel.GetCompanyPayrollMockedDomainModelData();
        }
        [TestMethod]
        public void FetchCompanyPayrollListWithoutSearchValue()
        {
            var result = new List<DomModel.CompanyPayroll> { companyPayrollDomianModels[0] };
            mockcompanyPayrollService.Setup(x => x.GetCompanyPayroll( It.IsAny<DomModel.CompanyPayroll>())).Returns(new Response().ToPopulate(ResponseType.Success, null, null, null, result, null, result?.Count));

            companyPayrollController = new CompanyPayrollController(mockcompanyPayrollService.Object);

            var response = companyPayrollController.Get("UK050", new DomModel.CompanyPayroll());

            Assert.AreEqual("1", response.Code);
            Assert.AreEqual(1, (response.Result as List<DomModel.CompanyPayroll>).Count);
        }
        //[TestMethod]
        //public void FetchCompanyPayrollListWithNullSearchModel()
        //{
        //    var result = new List<DomModel.CompanyPayroll> { companyPayrollDomianModels[0] };
        //    mockcompanyPayrollService.Setup(x => x.GetCompanyPayroll( It.IsAny<DomModel.CompanyPayroll>())).Returns(new Response().ToPopulate(ResponseType.Success, null, null, null, result, null, result?.Count));

        //    companyPayrollController = new CompanyPayrollController(mockcompanyPayrollService.Object);

        //    var response = companyPayrollController.Get("UK050", null);

        //    Assert.AreEqual("1", response.Code);
        //    Assert.AreEqual(1, (response.Result as List<DomModel.CompanyPayroll>).Count);
        ////}
        //[TestMethod]
        //public void FetchCompanyPayrollListWithNullSearchModelandNullSearchCode()
        //{
        //    var result = new List<DomModel.CompanyPayroll> { companyPayrollDomianModels[0] };
        //    mockcompanyPayrollService.Setup(x => x.GetCompanyPayroll(It.IsAny<DomModel.CompanyPayroll>())).Returns(new Response().ToPopulate(ResponseType.Success, null, null, null, result, null, result?.Count));

        //    companyPayrollController = new CompanyPayrollController(mockcompanyPayrollService.Object);

        //    var response = companyPayrollController.Get(null, null);

        //    Assert.AreEqual("11", response.Code);
        //    Assert.IsNull(response.Result);
        //}

        [TestMethod]
        public void FetchCompanyPayrollPeroidListByCompanyCode()
        {
            var result = new List<DomModel.CompanyPayroll> { companyPayrollDomianModels[0] };
            mockcompanyPayrollService.Setup(x => x.GetCompanyPayroll( It.IsAny<DomModel.CompanyPayroll>())).Returns(new Response().ToPopulate(ResponseType.Success, null, null, null, result, null, result?.Count));

            companyPayrollController = new CompanyPayrollController(mockcompanyPayrollService.Object);

            var response = companyPayrollController.Get("UK050", new DomModel.CompanyPayroll { CompanyCode = "DZ" });

            Assert.AreEqual("1", response.Code);
            Assert.AreEqual(1, (response.Result as List<DomModel.CompanyPayroll>).Count);
        }
        [TestMethod]
        public void FetchCompanyPayrollListByWildCardSearchWithNameStartWith()
        {
            var result = new List<DomModel.CompanyPayroll> { companyPayrollDomianModels[0] };
            mockcompanyPayrollService.Setup(x => x.GetCompanyPayroll(It.Is<DomModel.CompanyPayroll>(c => c.PayrollType.StartsWith("LTD")))).Returns(new Response().ToPopulate(ResponseType.Success, null, null, null, result, null, result?.Count));

            companyPayrollController = new CompanyPayrollController(mockcompanyPayrollService.Object);

            var response = companyPayrollController.Get("UK050", new DomModel.CompanyPayroll { PayrollType = "LTD*" });

            Assert.AreEqual("1", response.Code);
            Assert.AreEqual(1, (response.Result as List<DomModel.CompanyPayroll>).Count);
        }
        [TestMethod]
        public void FetchCompanyExpectedMarginByWildCardSearchWithNameEndWith()
        {
            var result = new List<DomModel.CompanyPayroll> { companyPayrollDomianModels[0] };
            mockcompanyPayrollService.Setup(x => x.GetCompanyPayroll( It.Is<DomModel.CompanyPayroll>(c => c.PayrollType.EndsWith("SE")))).Returns(new Response().ToPopulate(ResponseType.Success, null, null, null, result, null, result?.Count));

            companyPayrollController = new CompanyPayrollController(mockcompanyPayrollService.Object);

            var response = companyPayrollController.Get("UK050", new DomModel.CompanyPayroll { PayrollType = "*SE" });

            Assert.AreEqual("1", response.Code);
            Assert.AreEqual(1, (response.Result as List<DomModel.CompanyPayroll>).Count);
        }
        [TestMethod]
        public void FetchCompanyPayRollByWildCardSearchWithNameContains()
        {
            var result = new List<DomModel.CompanyPayroll> { companyPayrollDomianModels[0]};
            mockcompanyPayrollService.Setup(x => x.GetCompanyPayroll( It.Is<DomModel.CompanyPayroll>(c => c.PayrollType.Contains("Monthly")))).Returns(new Response().ToPopulate(ResponseType.Success, null, null, null, result, null, result?.Count));

            companyPayrollController = new CompanyPayrollController(mockcompanyPayrollService.Object);
            var response = companyPayrollController.Get("UK051", new DomModel.CompanyPayroll { PayrollType = "*Monthly*" });

            Assert.AreEqual("1", response.Code);
            Assert.AreEqual(1, (response.Result as List<DomModel.CompanyPayroll>).Count);
        }
        [TestMethod]
        public void ThrowsExceptionWhileFetchingCompanyList()
        {
            var exception = new Exception("Exception occured while performing some operation.");
            mockcompanyPayrollService.Setup(x => x.GetCompanyPayroll(It.IsAny<DomModel.CompanyPayroll>())).Returns(new Response().ToPopulate(ResponseType.Error, null, null, null, null, exception, 0));

            companyPayrollController = new CompanyPayrollController(mockcompanyPayrollService.Object);
            var response = companyPayrollController.Get("UK050", new DomModel.CompanyPayroll { CompanyCode = "DZ" });

            Assert.AreEqual("11", response.Code);
            Assert.IsNotNull(response.Messages);
            Assert.AreEqual("31", response.Messages.FirstOrDefault().Code);
        }

        [TestMethod]
        public void SaveCompanies()
        {
            var companyPayrollToSave = new List<DomModel.CompanyPayroll> { new DomModel.CompanyPayroll { CompanyCode = "UK050", PayrollType = "Payroll", Currency = "USD", LastModification = null, UpdateCount = null, ModifiedBy = null } };
            var result = new List<DomModel.CompanyPayroll> { companyPayrollDomianModels[0] };
            mockcompanyPayrollService.Setup(x => x.SaveCompanyPayroll("UK050", It.IsAny<IList<DomModel.CompanyPayroll>>(),true)).Returns(new Response().ToPopulate(ResponseType.Success, null, null, null, result, null));

            companyPayrollController = new CompanyPayrollController(mockcompanyPayrollService.Object);
            var response = companyPayrollController.Post("UK050", companyPayrollToSave);

            Assert.AreEqual("1", response.Code);
            Assert.IsNotNull(response.Result);
            Assert.AreEqual(1, (response.Result as List<DomModel.CompanyPayroll>).Count);
        }

        [TestMethod]
        public void ModifyPayroll()
        {
            var companyPayrollToModify = new List<DomModel.CompanyPayroll> { new DomModel.CompanyPayroll { CompanyCode = "UK050", PayrollType = "Payroll", Currency = "USD", LastModification = null, UpdateCount = null, ModifiedBy = "Test" } };

            companyPayrollDomianModels[0].UpdateCount = Convert.ToByte(Convert.ToInt32(companyPayrollToModify[0].UpdateCount) + 1);
            companyPayrollDomianModels[0].ModifiedBy = "Test";
            var result = new List<DomModel.CompanyPayroll> { companyPayrollDomianModels[0] };
            mockcompanyPayrollService.Setup(x => x.ModifyCompanyPayroll("UK050", It.IsAny<IList<DomModel.CompanyPayroll>>(),true)).Returns(new Response().ToPopulate(ResponseType.Success, null, null, null, result, null));

            companyPayrollController = new CompanyPayrollController(mockcompanyPayrollService.Object);
            var response = companyPayrollController.Put("UK050", companyPayrollToModify);

            Assert.AreEqual("1", response.Code);
            Assert.IsNotNull(response.Result);
            Assert.AreEqual(1, (response.Result as List<DomModel.CompanyPayroll>).Count);
            Assert.AreEqual("Test", (response.Result as List<DomModel.CompanyPayroll>)[0].ModifiedBy);
            Assert.AreEqual(1, Convert.ToInt32((response.Result as List<DomModel.CompanyPayroll>)[0].UpdateCount));
        }
        [TestMethod]
        public void DeletePayrollCompanies()
        {
            var companyPayrollToDelete = new List<DomModel.CompanyPayroll> { new DomModel.CompanyPayroll { CompanyCode = "UK050", PayrollType = "Payroll", Currency = "USD", LastModification = null, UpdateCount = null, ModifiedBy = null } };
            var result = new List<DomModel.CompanyPayroll> { companyPayrollDomianModels[0] };
            mockcompanyPayrollService.Setup(x => x.DeleteCompanyPayroll("UK050", It.IsAny<IList<DomModel.CompanyPayroll>>(),true)).Returns(new Response().ToPopulate(ResponseType.Success, null, null, null, result, null));

            companyPayrollController = new CompanyPayrollController(mockcompanyPayrollService.Object);
            var response = companyPayrollController.Delete("UK050", companyPayrollToDelete);

            Assert.AreEqual("1", response.Code);
           
        }

    }
}
