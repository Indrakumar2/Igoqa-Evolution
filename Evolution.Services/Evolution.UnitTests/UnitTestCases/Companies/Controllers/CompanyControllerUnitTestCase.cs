using Evolution.Api.Controllers.Company;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Common.Models.Responses;
using Evolution.Logging.Interfaces;
using Evolution.UnitTests.Mocks.Data.Companies;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq; 
using DomModel = Evolution.Company.Domain.Models.Companies;
using ServiceDomainData = Evolution.Company.Domain.Interfaces;

namespace Evolution.UnitTests.UnitTestCases.Companies.Controllers
{
    [TestClass]
    public class CompanyControllerUnitTestCase
    {

        CompanyController companyController = null;
        Mock<ServiceDomainData.Companies.ICompanyService> mockCompanyService = null;
        Mock<ServiceDomainData.Data.ICompanyRepository> mockCompanyRepository= null;
        IList<DomModel.Company> companyDomianModels = null; 

        [TestInitialize]
        public void InitializeCompanyController()
        {
            mockCompanyService = new Mock<ServiceDomainData.Companies.ICompanyService>();
            companyDomianModels = MockCompanyDomainModel.GetCompanyMockedDomainModelData(); 
        }

        [TestMethod]
        public void FetchCompanyListWithoutSearchValue()
        {
            mockCompanyService.Setup(x => x.GetCompany(It.IsAny<DomModel.Company>())).Returns(new Response().ToPopulate(ResponseType.Success, null, null, null, companyDomianModels, null, companyDomianModels?.Count));

            companyController = new CompanyController(mockCompanyService.Object);

            var response = companyController.Get(new DomModel.Company());

            Assert.AreEqual("1", response.Code);
            Assert.AreEqual(3, (response.Result as List<DomModel.Company>).Count);
        }

        [TestMethod]
        public void FetchCompanyListWithNullSearchModel()
        {  
            mockCompanyService.Setup(x => x.GetCompany(It.IsAny<DomModel.Company>())).Returns(new Response().ToPopulate(ResponseType.Success, null, null, null, companyDomianModels, null, companyDomianModels?.Count));

            companyController = new CompanyController(mockCompanyService.Object);

            var response = companyController.Get(null);

            Assert.AreEqual("1", response.Code);
            Assert.AreEqual(3, (response.Result as List<DomModel.Company>).Count);
        }

        [TestMethod]
        public void FetchCompanyListByCompanyCode()
        { 
            var result = new List<DomModel.Company> { companyDomianModels[0] };
            mockCompanyService.Setup(x => x.GetCompany(It.IsAny<DomModel.Company>())).Returns(new Response().ToPopulate(ResponseType.Success, null, null, null, result, null, result?.Count));

            companyController = new CompanyController(mockCompanyService.Object);

            var response = companyController.Get(new DomModel.Company { CompanyCode = "DZ" });

            Assert.AreEqual("1", response.Code);
            Assert.AreEqual(1, (response.Result as List<DomModel.Company>).Count); 
        }

        [TestMethod]
        public void FetchCompanyListByWildCardSearchWithNameStartWith()
        { 
            var result = companyDomianModels;
            mockCompanyService.Setup(x => x.GetCompany(It.Is<DomModel.Company>(c => c.CompanyName.StartsWith("A")))).Returns(new Response().ToPopulate(ResponseType.Success, null, null, null, result, null, result?.Count));

            companyController = new CompanyController(mockCompanyService.Object);

            var response = companyController.Get(new DomModel.Company { CompanyName = "A*" });
             
            Assert.AreEqual("1", response.Code);
            Assert.AreEqual(3, (response.Result as List<DomModel.Company>).Count);
        }

        [TestMethod]
        public void FetchCompanyListByWildCardSearchWithNameEndWith()
        { 
            var result = new List<DomModel.Company> { companyDomianModels[0], companyDomianModels[1] };
            mockCompanyService.Setup(x => x.GetCompany(It.Is<DomModel.Company>(c => c.CompanyName.EndsWith("MI")))).Returns(new Response().ToPopulate(ResponseType.Success, null, null, null, result, null, result?.Count));

            companyController = new CompanyController(mockCompanyService.Object);

            var response = companyController.Get(new DomModel.Company { CompanyName = "*MI" });

            Assert.AreEqual("1", response.Code);
            Assert.AreEqual(2, (response.Result as List<DomModel.Company>).Count);
        }

        [TestMethod]
        public void FetchCompanyListByWildCardSearchWithNameContains()
        { 
            var result = new List<DomModel.Company> { companyDomianModels[0], companyDomianModels[1] };
            mockCompanyService.Setup(x => x.GetCompany(It.Is<DomModel.Company>(c => c.CompanyName.Contains("ge")))).Returns(new Response().ToPopulate(ResponseType.Success, null, null, null, result, null, result?.Count));

            companyController = new CompanyController(mockCompanyService.Object); 
            var response = companyController.Get(new DomModel.Company { CompanyName = "*ge*" }); 

            Assert.AreEqual("1", response.Code);
            Assert.AreEqual(2, (response.Result as List<DomModel.Company>).Count);
        }

        [TestMethod]
        public void ThrowsExceptionWhileFetchingCompanyList()
        { 
            var exception = new Exception("Exception occured while performing some operation.");
            mockCompanyService.Setup(x => x.GetCompany(It.IsAny<DomModel.Company>())).Returns(new Response().ToPopulate(ResponseType.Error, null, null, null, null, exception, 0));
             
            companyController = new CompanyController(mockCompanyService.Object);
            var response = companyController.Get(new DomModel.Company { CompanyCode = "DZ" });
             
            Assert.AreEqual("11", response.Code);
            Assert.IsNotNull(response.Messages);
            Assert.AreEqual("31", response.Messages.FirstOrDefault().Code);
        }

        [TestMethod]
        public void SaveCompanies()
        {
            var companiesToSave = new List<DomModel.Company> { new DomModel.Company{ CompanyCode= "DZ", CompanyName= "Algeria MI", InvoiceName= "Algeria MI", IsActive= true, Currency= "DZD", SalesTaxDescription= null, WithholdingTaxDescription= null, InterCompanyExpenseAccRef= null, InterCompanyRateAccRef= null, InterCompanyRoyaltyAccRef= null, CompanyMiiwaid= 2, OperatingCountry= "Algeria", CompanyMiiwaref= 2, IsUseIctms=false, IsFullUse= true, GfsCoa= "ITKD-404", GfsBu= "E04", Region= "5. Africa", IsCOSEmailOverrideAllow= false,  AvgTSHourlyCost= null, VatTaxRegNo= "000230049009354", EUVatPrefix= "      ", IARegion= "4", CognosNumber= "404", UpdateCount= null, RecordStatus= "N", LastModification=DateTime.UtcNow, ModifiedBy= null }};
            var result = new List<DomModel.Company> { companyDomianModels[0]};
            mockCompanyService.Setup(x => x.SaveCompany(It.IsAny<IList<DomModel.Company>>(),true)).Returns(new Response().ToPopulate(ResponseType.Success, null, null, null, result, null));

            companyController = new CompanyController(mockCompanyService.Object);
            var response = companyController.Post(companiesToSave);

            Assert.AreEqual("1", response.Code);
            Assert.IsNotNull(response.Result);
            Assert.AreEqual(1, (response.Result as List<DomModel.Company>).Count);
        }

        [TestMethod]
        public void ModifyCompanies()
        {
            var companiesToSave = new List<DomModel.Company> { new DomModel.Company { CompanyCode = "DZ", CompanyName = "Algeria MI", InvoiceName = "Algeria MI", IsActive = true, Currency = "DZD", SalesTaxDescription = null, WithholdingTaxDescription = null, InterCompanyExpenseAccRef = null, InterCompanyRateAccRef = null, InterCompanyRoyaltyAccRef = null, CompanyMiiwaid = 2, OperatingCountry = "Algeria", CompanyMiiwaref = 2, IsUseIctms = false, IsFullUse = true, GfsCoa = "ITKD-404", GfsBu = "E04", Region = "5. Africa", IsCOSEmailOverrideAllow = false, AvgTSHourlyCost = null, VatTaxRegNo = "000230049009354", EUVatPrefix = "      ", IARegion = "4", CognosNumber = "404", UpdateCount = null, RecordStatus = "M", LastModification = DateTime.UtcNow, ModifiedBy = "J.Blyth" } };

            companyDomianModels[0].UpdateCount = Convert.ToByte(Convert.ToInt32(companiesToSave[0].UpdateCount) + 1);
            companyDomianModels[0].ModifiedBy = "J.Blyth";
            var result = new List<DomModel.Company> { companyDomianModels[0]};
            mockCompanyService.Setup(x => x.ModifyCompany(It.IsAny<IList<DomModel.Company>>(),true)).Returns(new Response().ToPopulate(ResponseType.Success, null, null, null, result, null));

            companyController = new CompanyController(mockCompanyService.Object);
            var response = companyController.Put(companiesToSave);

            Assert.AreEqual("1", response.Code);
            Assert.IsNotNull(response.Result);
            Assert.AreEqual(1, (response.Result as List<DomModel.Company>).Count);
            Assert.AreEqual("J.Blyth", (response.Result as List<DomModel.Company>)[0].ModifiedBy);
            Assert.AreEqual(1, Convert.ToInt32((response.Result as List<DomModel.Company>)[0].UpdateCount));
        }

    }
}
