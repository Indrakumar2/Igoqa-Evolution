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
     public class CompanyPayrollPeroidControllerUnitTestCase
     {
        CompanyPayrollPeriodController companyPayrollPeroidController = null;
        Mock<ServiceDomainData.ICompanyPayrollPeriodService> mockcompanyPayrollPeroidService = null;
        IList<DomModel.CompanyPayrollPeriod> companyPayrollPeroidDomianModels = null;

        [TestInitialize]
        public void InitializeCompanyExpectedmarginController()
        {
            mockcompanyPayrollPeroidService = new Mock<ServiceDomainData.ICompanyPayrollPeriodService>();
            companyPayrollPeroidDomianModels = MockCompanyDomainModel.GetCompanyPayrollPeriodMockedDomainModelData();
        }
        [TestMethod]
        public void FetchCompanypayrollPeroidListWithoutSearchValue()
        {
            var result = new List<DomModel.CompanyPayrollPeriod> { companyPayrollPeroidDomianModels[0] };
            mockcompanyPayrollPeroidService.Setup(x => x.GetCompanyPayrollPeriod( It.IsAny<DomModel.CompanyPayrollPeriod>())).Returns(new Response().ToPopulate(ResponseType.Success, null, null, null, result, null, result?.Count));

            companyPayrollPeroidController = new CompanyPayrollPeriodController(mockcompanyPayrollPeroidService.Object);

            var response = companyPayrollPeroidController.Get("UK050", "LTD/VAT/SE" ,new DomModel.CompanyPayrollPeriod());

            Assert.AreEqual("1", response.Code);
            Assert.AreEqual(1, (response.Result as List<DomModel.CompanyPayrollPeriod>).Count);
        }
        //[TestMethod]
        //public void FetchCompanyPayrollPeroidListWithNullSearchModel()
        //{
        //    var result = new List<DomModel.CompanyPayrollPeriod> { companyPayrollPeroidDomianModels[0] };

        //    mockcompanyPayrollPeroidService.Setup(x => x.GetCompanyPayrollPeriod( It.IsAny<DomModel.CompanyPayrollPeriod>())).Returns(new Response().ToPopulate(ResponseType.Success, null, null, null, result, null, result?.Count));

        //    companyPayrollPeroidController = new CompanyPayrollPeriodController(mockcompanyPayrollPeroidService.Object);

        //    var response = companyPayrollPeroidController.Get("UK050", "LTD/VAT/SE", null);

        //    Assert.AreEqual("1", response.Code);
        //    Assert.AreEqual(1, (response.Result as List<DomModel.CompanyPayrollPeriod>).Count);
        //}

        //[TestMethod]
        //public void FetchCompanyPayrollPeroidListWithNullParameters()
        //{
        //    var result = new List<DomModel.CompanyPayrollPeriod> { companyPayrollPeroidDomianModels[0] };

        //    mockcompanyPayrollPeroidService.Setup(x => x.GetCompanyPayrollPeriod(It.IsAny<DomModel.CompanyPayrollPeriod>())).Returns(new Response().ToPopulate(ResponseType.Success, null, null, null, result, null, result?.Count));

        //    companyPayrollPeroidController = new CompanyPayrollPeriodController(mockcompanyPayrollPeroidService.Object);

        //    var response = companyPayrollPeroidController.Get(null,null, null);

        //    Assert.AreEqual("11", response.Code);
        //    Assert.IsNull(response.Result);
        //}

        [TestMethod]
        public void FetchCompanyPayrollPeroidListByCompanyCode()
        {
            var result = new List<DomModel.CompanyPayrollPeriod> { companyPayrollPeroidDomianModels[0] };
            mockcompanyPayrollPeroidService.Setup(x => x.GetCompanyPayrollPeriod( It.IsAny<DomModel.CompanyPayrollPeriod>())).Returns(new Response().ToPopulate(ResponseType.Success, null, null, null, result, null, result?.Count));

            companyPayrollPeroidController = new CompanyPayrollPeriodController(mockcompanyPayrollPeroidService.Object);

            var response = companyPayrollPeroidController.Get("UK050", "LTD/VAT/SE", new DomModel.CompanyPayrollPeriod { CompanyCode = "DZ" });

            Assert.AreEqual("1", response.Code);
            Assert.AreEqual(1, (response.Result as List<DomModel.CompanyPayrollPeriod>).Count);
        }
        [TestMethod]
        public void FetchCompanyPayrollPeroidListByWildCardSearchWithNameStartWith()
        {
            var result = new List<DomModel.CompanyPayrollPeriod> { companyPayrollPeroidDomianModels[0] };
            mockcompanyPayrollPeroidService.Setup(x => x.GetCompanyPayrollPeriod(It.Is<DomModel.CompanyPayrollPeriod>(c => c.PayrollType.StartsWith("LTD")))).Returns(new Response().ToPopulate(ResponseType.Success, null, null, null, result, null, result?.Count));

            companyPayrollPeroidController = new CompanyPayrollPeriodController(mockcompanyPayrollPeroidService.Object);

            var response = companyPayrollPeroidController.Get("UK050", "LTD/VAT/SE", new DomModel.CompanyPayrollPeriod {PayrollType  = "LTD*" });

            Assert.AreEqual("1", response.Code);
            Assert.AreEqual(1, (response.Result as List<DomModel.CompanyPayrollPeriod>).Count);
        }
        [TestMethod]
        public void FetchCompanyPayrollPeroidByWildCardSearchWithNameEndWith()
        {
            var result = new List<DomModel.CompanyPayrollPeriod> { companyPayrollPeroidDomianModels[1]};
            mockcompanyPayrollPeroidService.Setup(x => x.GetCompanyPayrollPeriod( It.Is<DomModel.CompanyPayrollPeriod>(c => c.PayrollType.EndsWith("ly")))).Returns(new Response().ToPopulate(ResponseType.Success, null, null, null, result, null, result?.Count));

            companyPayrollPeroidController = new CompanyPayrollPeriodController(mockcompanyPayrollPeroidService.Object);

            var response = companyPayrollPeroidController.Get("UK051", "PAYE Monthly", new DomModel.CompanyPayrollPeriod { PayrollType = "*ly" });
            Assert.AreEqual("1", response.Code);
            Assert.AreEqual(1, (response.Result as List<DomModel.CompanyPayrollPeriod>).Count);
        }
        [TestMethod]
        public void FetchCompanyPayrollPeroidByWildCardSearchWithNameContains()
        {
            var result = new List<DomModel.CompanyPayrollPeriod> { companyPayrollPeroidDomianModels[0] };
            mockcompanyPayrollPeroidService.Setup(x => x.GetCompanyPayrollPeriod( It.Is<DomModel.CompanyPayrollPeriod>(c => c.PayrollType.Contains("VAT")))).Returns(new Response().ToPopulate(ResponseType.Success, null, null, null, result, null, result?.Count));

            companyPayrollPeroidController = new CompanyPayrollPeriodController(mockcompanyPayrollPeroidService.Object);
            var response = companyPayrollPeroidController.Get("UK050", "LTD/VAT/SE", new DomModel.CompanyPayrollPeriod { PayrollType = "*VAT*" });

            Assert.AreEqual("1", response.Code);
            Assert.AreEqual(1, (response.Result as List<DomModel.CompanyPayrollPeriod>).Count);
        }
        [TestMethod]
        public void ThrowsExceptionWhileFetchingCompanyList()
        {
            var exception = new Exception("Exception occured while performing some operation.");
            mockcompanyPayrollPeroidService.Setup(x => x.GetCompanyPayrollPeriod( It.IsAny<DomModel.CompanyPayrollPeriod>())).Returns(new Response().ToPopulate(ResponseType.Error, null, null, null, null, exception, 0));

            companyPayrollPeroidController = new CompanyPayrollPeriodController(mockcompanyPayrollPeroidService.Object);
            var response = companyPayrollPeroidController.Get("UK050", "LTD/VAT/SE",new DomModel.CompanyPayrollPeriod { CompanyCode = "DZ" });

            Assert.AreEqual("11", response.Code);
            Assert.IsNotNull(response.Messages);
            Assert.AreEqual("31", response.Messages.FirstOrDefault().Code);
        }

        [TestMethod]
        public void SaveCompanyPayrollPeroid()
        {
            var companyPayrollPeroisToSave = new List<DomModel.CompanyPayrollPeriod> { new DomModel.CompanyPayrollPeriod { CompanyCode = "UK050", PayrollType = "LTD/VAT/SE", PeriodName = "October", StartDate = Convert.ToDateTime("2007-09-29 00:00:00.000"), EndDate = Convert.ToDateTime("2007-10-26 00:00:00.000"), PeriodStatus = "N", IsActive = true, LastModification = null, UpdateCount = null, ModifiedBy = null } };
            var result = new List<DomModel.CompanyPayrollPeriod> { companyPayrollPeroidDomianModels[0] };
            mockcompanyPayrollPeroidService.Setup(x => x.SaveCompanyPayrollPeriod("UK050", "LTD/VAT/SE", It.IsAny<IList<DomModel.CompanyPayrollPeriod>>(),true)).Returns(new Response().ToPopulate(ResponseType.Success, null, null, null, result, null));

            companyPayrollPeroidController = new CompanyPayrollPeriodController(mockcompanyPayrollPeroidService.Object);
            var response = companyPayrollPeroidController.Post("UK050", "LTD/VAT/SE", companyPayrollPeroisToSave);

            Assert.AreEqual("1", response.Code);
            Assert.IsNotNull(response.Result);
            Assert.AreEqual(1, (response.Result as List<DomModel.CompanyPayrollPeriod>).Count);
        }

        [TestMethod]
        public void ModifyCompanyPayrollPeroid()
        {
            var companyPayrollPeroidToModify = new List<DomModel.CompanyPayrollPeriod> { new DomModel.CompanyPayrollPeriod { CompanyCode = "UK050", PayrollType = "LTD/VAT/SE", PeriodName = "October", StartDate = Convert.ToDateTime("2007-09-29 00:00:00.000"), EndDate = Convert.ToDateTime("2007-10-26 00:00:00.000"), PeriodStatus = "N", IsActive = true, LastModification = null, UpdateCount = null, ModifiedBy = null } };

            companyPayrollPeroidDomianModels[0].UpdateCount = Convert.ToByte(Convert.ToInt32(companyPayrollPeroidToModify[0].UpdateCount) + 1);
            companyPayrollPeroidDomianModels[0].ModifiedBy = "test";
            var result = new List<DomModel.CompanyPayrollPeriod> { companyPayrollPeroidDomianModels[0] };
            mockcompanyPayrollPeroidService.Setup(x => x.ModifyCompanyPayrollPeriod("UK050", "LTD/VAT/SE", It.IsAny<IList<DomModel.CompanyPayrollPeriod>>(),true)).Returns(new Response().ToPopulate(ResponseType.Success, null, null, null, result, null));

            companyPayrollPeroidController = new CompanyPayrollPeriodController(mockcompanyPayrollPeroidService.Object);
            var response = companyPayrollPeroidController.Put("UK050", "LTD/VAT/SE",companyPayrollPeroidToModify);

            Assert.AreEqual("1", response.Code);
            Assert.IsNotNull(response.Result);
            Assert.AreEqual(1, (response.Result as List<DomModel.CompanyPayrollPeriod>).Count);
            Assert.AreEqual("test", (response.Result as List<DomModel.CompanyPayrollPeriod>)[0].ModifiedBy);
            Assert.AreEqual(1, Convert.ToInt32((response.Result as List<DomModel.CompanyPayrollPeriod>)[0].UpdateCount));
        }
        [TestMethod]
        public void DeleteCompanyPayrollPeroid()
        {
            var companyPayrollPeroisToDelete = new List<DomModel.CompanyPayrollPeriod> { new DomModel.CompanyPayrollPeriod { CompanyCode = "UK050", PayrollType = "LTD/VAT/SE", PeriodName = "October", StartDate = Convert.ToDateTime("2007-09-29 00:00:00.000"), EndDate = Convert.ToDateTime("2007-10-26 00:00:00.000"), PeriodStatus = "N", IsActive = true, LastModification = null, UpdateCount = null, ModifiedBy = null } };
            var result = new List<DomModel.CompanyPayrollPeriod> { companyPayrollPeroidDomianModels[0] };
            mockcompanyPayrollPeroidService.Setup(x => x.DeleteCompanyPayrollPeriod("UK050", "LTD/VAT/SE", It.IsAny<IList<DomModel.CompanyPayrollPeriod>>(),true)).Returns(new Response().ToPopulate(ResponseType.Success, null, null, null, result, null));

            companyPayrollPeroidController = new CompanyPayrollPeriodController(mockcompanyPayrollPeroidService.Object);
            var response = companyPayrollPeroidController.Delete("UK050", "LTD/VAT/SE", companyPayrollPeroisToDelete);

            Assert.AreEqual("1", response.Code);
          
        }
    }
}
