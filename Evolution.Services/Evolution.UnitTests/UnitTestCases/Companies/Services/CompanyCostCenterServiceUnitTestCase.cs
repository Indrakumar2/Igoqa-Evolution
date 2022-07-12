using Evolution.Logging.Interfaces;
using Evolution.UnitTests.Mocks.Data.Companies;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using DomainData = Evolution.Company.Domain.Interfaces.Data;
using DomModel = Evolution.Company.Domain.Models.Companies;
using DbModel = Evolution.DbRepository.Models;
using ServiceDomainData = Evolution.Company.Domain.Interfaces.Companies;
using AutoMapper;
using System.Linq.Expressions;
using Evolution.UnitTests.Mocks.Data.Companies.Db;

namespace Evolution.UnitTests.UnitTestCases.Companies.Services
{
    [TestClass]
    public class CompanyCostCenterServiceUnitTestCase
    {
        ServiceDomainData.ICompanyDivisionCostCenterService companyCostCenterService = null;
        Mock<DomainData.ICompanyCostCenterRepository> mockCompanyCostCenterRepository = null;
        IList<DomModel.CompanyDivisionCostCenter> companyCostCenterDomianModels = null;
        Mock<DomainData.ICompanyDivisionRepository> mockCompanydivisionRepository = null;
        Mock<IAppLogger<Company.Core.Services.CompanyCostCenterService>> mockLogger = null;
        Mock<IMapper> mockmapper = null;
        IQueryable<DbModel.CompanyDivision> mockCompanyDivisionDbData = null;
        IQueryable<DbModel.CompanyDivisionCostCenter> mockCompanyCostCenterDbData = null;
        IQueryable<DbModel.Company> mockComapanyDbData = null;
        Mock<DomainData.ICompanyRepository> mockcompanyRepository = null;
        
        [TestInitialize]
        public void InitializeCompanyCostCenterService()
        {
            mockCompanyCostCenterRepository = new Mock<DomainData.ICompanyCostCenterRepository>();
            companyCostCenterDomianModels = MockCompanyDomainModel.GetCompanyCostCenterMockedDomainModelData();
            mockLogger = new Mock<IAppLogger<Company.Core.Services.CompanyCostCenterService>>();
            mockCompanydivisionRepository = new Mock<DomainData.ICompanyDivisionRepository>();
            mockCompanyDivisionDbData = MockCompany.GetCompanyDivisionMockData();
            mockCompanyCostCenterDbData = MockCompany.GetCompanyDivisionCostCenterMockData();
            mockmapper = new Mock<IMapper>();
            mockComapanyDbData = MockCompany.GetCompanyMockData();
            mockcompanyRepository = new Mock<DomainData.ICompanyRepository>();
          
        }

        [TestMethod]
        public void FetchCompanyCostCenterListWithoutSearchValue()
        {
            mockCompanyCostCenterRepository.Setup(x => x.Search(It.IsAny<DomModel.CompanyDivisionCostCenter>())).Returns(companyCostCenterDomianModels);

            companyCostCenterService = new Company.Core.Services.CompanyCostCenterService(mockmapper.Object,mockCompanyCostCenterRepository.Object, mockCompanydivisionRepository.Object, mockLogger.Object);

            var response = companyCostCenterService.GetCompanyCostCenter(new DomModel.CompanyDivisionCostCenter());

            Assert.AreEqual("1", response.Code);
            Assert.AreEqual(3, (response.Result as List<DomModel.CompanyDivisionCostCenter>).Count);
        }

        [TestMethod]
        public void FetchCompanyCostCenterListWithNullSearchModel()
        {
            mockCompanyCostCenterRepository.Setup(x => x.Search(It.IsAny<DomModel.CompanyDivisionCostCenter>())).Returns(companyCostCenterDomianModels);

            companyCostCenterService = new Company.Core.Services.CompanyCostCenterService(mockmapper.Object,mockCompanyCostCenterRepository.Object, mockCompanydivisionRepository.Object,mockLogger.Object);

            var response = companyCostCenterService.GetCompanyCostCenter(null);

            Assert.AreEqual("1", response.Code);
            Assert.AreEqual(3, (response.Result as List<DomModel.CompanyDivisionCostCenter>).Count);
        }

        [TestMethod]
        public void FetchCompanyCostCenterListByCompanyCode()
        {
            mockCompanyCostCenterRepository.Setup(x => x.Search(It.Is<DomModel.CompanyDivisionCostCenter>(c => c.CompanyCode == "UK050"))).Returns(new List<DomModel.CompanyDivisionCostCenter> { companyCostCenterDomianModels[0] });

            companyCostCenterService = new Company.Core.Services.CompanyCostCenterService(mockmapper.Object,mockCompanyCostCenterRepository.Object, mockCompanydivisionRepository.Object,mockLogger.Object);

            var response = companyCostCenterService.GetCompanyCostCenter(new DomModel.CompanyDivisionCostCenter { CompanyCode = "UK050" });

            Assert.AreEqual("1", response.Code);
            Assert.AreEqual(1, (response.Result as List<DomModel.CompanyDivisionCostCenter>).Count);
        }

        [TestMethod]
        public void FetchCompanyCostCenterListByWildCardSearchWithCostCenterNameStartWith()
        {
            mockCompanyCostCenterRepository.Setup(x => x.Search(It.Is<DomModel.CompanyDivisionCostCenter>(c => c.CostCenterName.StartsWith("A")))).Returns(new List<DomModel.CompanyDivisionCostCenter> { companyCostCenterDomianModels[1], companyCostCenterDomianModels[2] });

            companyCostCenterService = new Company.Core.Services.CompanyCostCenterService(mockmapper.Object,mockCompanyCostCenterRepository.Object, mockCompanydivisionRepository.Object, mockLogger.Object);

            var response = companyCostCenterService.GetCompanyCostCenter(new DomModel.CompanyDivisionCostCenter { CostCenterName = "A*" });

            Assert.AreEqual("1", response.Code);
            Assert.AreEqual(2, (response.Result as List<DomModel.CompanyDivisionCostCenter>).Count);
        }

        [TestMethod]
        public void FetchCompanyCostCenterListByWildCardSearchWithCostCenterNameEndWith()
        {
            mockCompanyCostCenterRepository.Setup(x => x.Search(It.Is<DomModel.CompanyDivisionCostCenter>(c => c.CostCenterName.EndsWith("an")))).Returns(new List<DomModel.CompanyDivisionCostCenter> { companyCostCenterDomianModels[1], companyCostCenterDomianModels[2] });

            companyCostCenterService = new Company.Core.Services.CompanyCostCenterService(mockmapper.Object,mockCompanyCostCenterRepository.Object, mockCompanydivisionRepository.Object, mockLogger.Object);

            var response = companyCostCenterService.GetCompanyCostCenter(new DomModel.CompanyDivisionCostCenter { CostCenterName = "*an" });

            Assert.AreEqual("1", response.Code);
            Assert.AreEqual(2, (response.Result as List<DomModel.CompanyDivisionCostCenter>).Count);
        }

        [TestMethod]
        public void FetchCompanyCostCenterListByWildCardSearchWithCostCenterNameContains()
        {
            mockCompanyCostCenterRepository.Setup(x => x.Search(It.Is<DomModel.CompanyDivisionCostCenter>(c => c.CostCenterName.Contains("baij")))).Returns(new List<DomModel.CompanyDivisionCostCenter> { companyCostCenterDomianModels[1], companyCostCenterDomianModels[2] });

            companyCostCenterService = new Company.Core.Services.CompanyCostCenterService(mockmapper.Object, mockCompanyCostCenterRepository.Object,  mockCompanydivisionRepository.Object, mockLogger.Object);

            var response = companyCostCenterService.GetCompanyCostCenter(new DomModel.CompanyDivisionCostCenter { CostCenterName = "*baij*" });

            Assert.AreEqual("1", response.Code);
            Assert.AreEqual(2, (response.Result as List<DomModel.CompanyDivisionCostCenter>).Count);
        }

        [TestMethod]
        public void ThrowsExceptionWhileFetchingCompanyList()
        {
            mockCompanyCostCenterRepository.Setup(x => x.Search(It.IsAny<DomModel.CompanyDivisionCostCenter>())).Throws(new Exception("Exception occured while performing some operation."));
            companyCostCenterService = new Company.Core.Services.CompanyCostCenterService(mockmapper.Object, mockCompanyCostCenterRepository.Object, mockCompanydivisionRepository.Object, mockLogger.Object);

            var response = companyCostCenterService.GetCompanyCostCenter(new DomModel.CompanyDivisionCostCenter());

            Assert.AreEqual("11", response.Code);
            Assert.IsNotNull(response.Messages);
            Assert.AreEqual("31", response.Messages.FirstOrDefault().Code);
        }

        [TestMethod]
        public void SaveCompanyCostCenters()
        {
            mockCompanyCostCenterRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.CompanyDivisionCostCenter, bool>>>())).Returns((Expression<Func<DbModel.CompanyDivisionCostCenter, bool>> predicate) => mockCompanyCostCenterDbData.Where(predicate));
            mockCompanydivisionRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.CompanyDivision, bool>>>())).Returns((Expression<Func<DbModel.CompanyDivision, bool>> predicate) => mockCompanyDivisionDbData.Where(predicate));
             
            companyCostCenterService = new Company.Core.Services.CompanyCostCenterService(mockmapper.Object, mockCompanyCostCenterRepository.Object, mockCompanydivisionRepository.Object, mockLogger.Object);

            companyCostCenterDomianModels.ToList().ForEach(x => x.RecordStatus = "N");

            companyCostCenterDomianModels[0].CostCenterName = "Test Cost Center";

            var response = companyCostCenterService.SaveCompanyCostCenter(companyCostCenterDomianModels[0].CompanyCode, companyCostCenterDomianModels[0].Division, new List<DomModel.CompanyDivisionCostCenter> { companyCostCenterDomianModels[0] });

            mockCompanyCostCenterRepository.Verify(m => m.Add(It.IsAny<IList<DbModel.CompanyDivisionCostCenter>>()), Times.AtLeastOnce);
        }

        [TestMethod]
        public void ModifyCompanyCostCenter()
        {
            mockCompanyCostCenterRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.CompanyDivisionCostCenter, bool>>>())).Returns((Expression<Func<DbModel.CompanyDivisionCostCenter, bool>> predicate) => mockCompanyCostCenterDbData.Where(predicate));
            mockCompanydivisionRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.CompanyDivision, bool>>>())).Returns((Expression<Func<DbModel.CompanyDivision, bool>> predicate) => mockCompanyDivisionDbData.Where(predicate));
              
            companyCostCenterService = new Company.Core.Services.CompanyCostCenterService(mockmapper.Object, mockCompanyCostCenterRepository.Object, mockCompanydivisionRepository.Object, mockLogger.Object);
            companyCostCenterDomianModels.ToList().ForEach(x => x.RecordStatus = "M");
            var response = companyCostCenterService.ModifyCompanyCostCenter(companyCostCenterDomianModels[0].CompanyCode, companyCostCenterDomianModels[0].Division, new List<DomModel.CompanyDivisionCostCenter> { companyCostCenterDomianModels[0] });

            mockCompanyCostCenterRepository.Verify(m => m.Update(It.IsAny<DbModel.CompanyDivisionCostCenter>()), Times.AtLeast(1));
        }

        [TestMethod]
        public void DeleteCompanyCostCenter()
        {
            mockCompanyCostCenterRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.CompanyDivisionCostCenter, bool>>>())).Returns((Expression<Func<DbModel.CompanyDivisionCostCenter, bool>> predicate) => mockCompanyCostCenterDbData.Where(predicate));
            mockCompanydivisionRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.CompanyDivision, bool>>>())).Returns((Expression<Func<DbModel.CompanyDivision, bool>> predicate) => mockCompanyDivisionDbData.Where(predicate));

            companyCostCenterService = new Company.Core.Services.CompanyCostCenterService(mockmapper.Object, mockCompanyCostCenterRepository.Object, mockCompanydivisionRepository.Object, mockLogger.Object);
            companyCostCenterDomianModels.ToList().ForEach(x => x.RecordStatus = "D");
            var response = companyCostCenterService.DeleteCompanyCostCenter(companyCostCenterDomianModels[1].CompanyCode, companyCostCenterDomianModels[0].Division, new List<DomModel.CompanyDivisionCostCenter> { companyCostCenterDomianModels[1] });

            mockCompanyCostCenterRepository.Verify(m => m.Delete(It.IsAny<DbModel.CompanyDivisionCostCenter>()), Times.AtLeast(1));
        }
        [TestMethod]
        public void ModifyCompanyCostCenterUpdateCountMissMatch()
        {
            mockCompanyCostCenterRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.CompanyDivisionCostCenter, bool>>>())).Returns((Expression<Func<DbModel.CompanyDivisionCostCenter, bool>> predicate) => mockCompanyCostCenterDbData.Where(predicate));
            mockCompanydivisionRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.CompanyDivision, bool>>>())).Returns((Expression<Func<DbModel.CompanyDivision, bool>> predicate) => mockCompanyDivisionDbData.Where(predicate));

            companyCostCenterService = new Company.Core.Services.CompanyCostCenterService(mockmapper.Object, mockCompanyCostCenterRepository.Object, mockCompanydivisionRepository.Object, mockLogger.Object);
            companyCostCenterDomianModels.ToList().ForEach(x => x.RecordStatus = "M");
            companyCostCenterDomianModels[0].UpdateCount = 1;

            var response = companyCostCenterService.ModifyCompanyCostCenter(companyCostCenterDomianModels[0].CompanyCode, companyCostCenterDomianModels[0].Division, new List<DomModel.CompanyDivisionCostCenter> { companyCostCenterDomianModels[0] });

         
            Assert.AreEqual("11", response.Code);
        }
        [TestMethod]
        public void SaveCompanyCostCentersWithDuplicateCostCenter()
        {
            mockCompanyCostCenterRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.CompanyDivisionCostCenter, bool>>>())).Returns((Expression<Func<DbModel.CompanyDivisionCostCenter, bool>> predicate) => mockCompanyCostCenterDbData.Where(predicate));
            mockCompanydivisionRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.CompanyDivision, bool>>>())).Returns((Expression<Func<DbModel.CompanyDivision, bool>> predicate) => mockCompanyDivisionDbData.Where(predicate));

            companyCostCenterService = new Company.Core.Services.CompanyCostCenterService(mockmapper.Object, mockCompanyCostCenterRepository.Object, mockCompanydivisionRepository.Object, mockLogger.Object);

            companyCostCenterDomianModels.ToList().ForEach(x => x.RecordStatus = "N");


            var response = companyCostCenterService.SaveCompanyCostCenter(companyCostCenterDomianModels[0].CompanyCode, companyCostCenterDomianModels[0].Division, new List<DomModel.CompanyDivisionCostCenter> { companyCostCenterDomianModels[0] });

            Assert.AreEqual("11", response.Code);

        }
        [TestMethod]
        
        public void ModifyCompanyCostCenterwithNULLDivisonName()
        {
            mockCompanyCostCenterRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.CompanyDivisionCostCenter, bool>>>())).Returns((Expression<Func<DbModel.CompanyDivisionCostCenter, bool>> predicate) => mockCompanyCostCenterDbData.Where(predicate));
            mockCompanydivisionRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.CompanyDivision, bool>>>())).Returns((Expression<Func<DbModel.CompanyDivision, bool>> predicate) => mockCompanyDivisionDbData.Where(predicate));

            companyCostCenterService = new Company.Core.Services.CompanyCostCenterService(mockmapper.Object, mockCompanyCostCenterRepository.Object, mockCompanydivisionRepository.Object, mockLogger.Object);
            companyCostCenterDomianModels.ToList().ForEach(x => x.RecordStatus = "M");

            var response = companyCostCenterService.ModifyCompanyCostCenter(companyCostCenterDomianModels[0].CompanyCode, null, new List<DomModel.CompanyDivisionCostCenter> { companyCostCenterDomianModels[0] });
           
            Assert.AreEqual("11", response.Code);

        }
        [TestMethod]
        public void ModifyCompanyCostCenterwithInvalidDivision()
        {
            mockCompanyCostCenterRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.CompanyDivisionCostCenter, bool>>>())).Returns((Expression<Func<DbModel.CompanyDivisionCostCenter, bool>> predicate) => mockCompanyCostCenterDbData.Where(predicate));
            mockCompanydivisionRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.CompanyDivision, bool>>>())).Returns((Expression<Func<DbModel.CompanyDivision, bool>> predicate) => mockCompanyDivisionDbData.Where(predicate));

            companyCostCenterService = new Company.Core.Services.CompanyCostCenterService(mockmapper.Object, mockCompanyCostCenterRepository.Object, mockCompanydivisionRepository.Object, mockLogger.Object);
            companyCostCenterDomianModels.ToList().ForEach(x => x.RecordStatus = "M");
            companyCostCenterDomianModels[0].Division = "Invalid Divison";
            var response = companyCostCenterService.ModifyCompanyCostCenter(companyCostCenterDomianModels[0].CompanyCode, companyCostCenterDomianModels[0].Division, new List<DomModel.CompanyDivisionCostCenter> { companyCostCenterDomianModels[0] });
            Assert.AreEqual("11", response.Code);

            // mockCompanyCostCenterRepository.Verify(m => m.Update(It.IsAny<DbModel.CompanyDivisionCostCenter>()), Times.AtLeast(1));
        }
        [TestMethod]
        public void ModifyCompanyCostCenterWithRecordStatusNull()
        {
            mockCompanyCostCenterRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.CompanyDivisionCostCenter, bool>>>())).Returns((Expression<Func<DbModel.CompanyDivisionCostCenter, bool>> predicate) => mockCompanyCostCenterDbData.Where(predicate));
            mockCompanydivisionRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.CompanyDivision, bool>>>())).Returns((Expression<Func<DbModel.CompanyDivision, bool>> predicate) => mockCompanyDivisionDbData.Where(predicate));

            companyCostCenterService = new Company.Core.Services.CompanyCostCenterService(mockmapper.Object, mockCompanyCostCenterRepository.Object, mockCompanydivisionRepository.Object, mockLogger.Object);
           // companyCostCenterDomianModels.ToList().ForEach(x => x.RecordStatus = null);
            companyCostCenterDomianModels[0].RecordStatus = null;
            var response = companyCostCenterService.ModifyCompanyCostCenter(companyCostCenterDomianModels[0].CompanyCode, companyCostCenterDomianModels[0].Division,null);
            Assert.AreEqual("1", response.Code);

            // mockCompanyCostCenterRepository.Verify(m => m.Update(It.IsAny<DbModel.CompanyDivisionCostCenter>()), Times.AtLeast(1));
        }
        [TestMethod]
        public void SaveCompanyCostCentersWithCompanyCodeAndDivisonNameNUll()
        {
            mockCompanyCostCenterRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.CompanyDivisionCostCenter, bool>>>())).Returns((Expression<Func<DbModel.CompanyDivisionCostCenter, bool>> predicate) => mockCompanyCostCenterDbData.Where(predicate));
            mockCompanydivisionRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.CompanyDivision, bool>>>())).Returns((Expression<Func<DbModel.CompanyDivision, bool>> predicate) => mockCompanyDivisionDbData.Where(predicate));

            companyCostCenterService = new Company.Core.Services.CompanyCostCenterService(mockmapper.Object, mockCompanyCostCenterRepository.Object, mockCompanydivisionRepository.Object, mockLogger.Object);

            companyCostCenterDomianModels.ToList().ForEach(x => x.RecordStatus = "N");
            companyCostCenterDomianModels[0].CompanyCode = null;
            companyCostCenterDomianModels[0].Division = null;

            var response = companyCostCenterService.SaveCompanyCostCenter(companyCostCenterDomianModels[0].CompanyCode, companyCostCenterDomianModels[0].Division, new List<DomModel.CompanyDivisionCostCenter> { companyCostCenterDomianModels[0] });

            Assert.AreEqual("11", response.Code);

        }
        /// <summary>
        /// Expected Exception
        /// </summary>
        [TestMethod]
        public void UpdateCompanyCostCentersWithCompanyCodeAndDivisonNameNUll()
        {
            mockCompanyCostCenterRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.CompanyDivisionCostCenter, bool>>>())).Returns((Expression<Func<DbModel.CompanyDivisionCostCenter, bool>> predicate) => mockCompanyCostCenterDbData.Where(predicate));
            mockCompanydivisionRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.CompanyDivision, bool>>>())).Returns((Expression<Func<DbModel.CompanyDivision, bool>> predicate) => mockCompanyDivisionDbData.Where(predicate));

            companyCostCenterService = new Company.Core.Services.CompanyCostCenterService(mockmapper.Object, mockCompanyCostCenterRepository.Object, mockCompanydivisionRepository.Object, mockLogger.Object);

            companyCostCenterDomianModels.ToList().ForEach(x => x.RecordStatus = "M");
            companyCostCenterDomianModels[0].CompanyCode = null;
            companyCostCenterDomianModels[0].Division = null;

            var response = companyCostCenterService.ModifyCompanyCostCenter(companyCostCenterDomianModels[0].CompanyCode, companyCostCenterDomianModels[0].Division, new List<DomModel.CompanyDivisionCostCenter> { companyCostCenterDomianModels[0] });

            Assert.AreEqual("11", response.Code);

        }

        [TestMethod]
        public void DeleteCompanyCostCentersWithChildData()
        {
            mockcompanyRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Company, bool>>>())).Returns(mockComapanyDbData);
            mockCompanyCostCenterRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.CompanyDivisionCostCenter, bool>>>())).Returns((Expression<Func<DbModel.CompanyDivisionCostCenter, bool>> predicate) => mockCompanyCostCenterDbData.Where(predicate));
            mockCompanydivisionRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.CompanyDivision, bool>>>())).Returns((Expression<Func<DbModel.CompanyDivision, bool>> predicate) => mockCompanyDivisionDbData.Where(predicate));

            companyCostCenterService = new Company.Core.Services.CompanyCostCenterService(mockmapper.Object, mockCompanyCostCenterRepository.Object, mockCompanydivisionRepository.Object, mockLogger.Object);
            companyCostCenterDomianModels.ToList().ForEach(x => x.RecordStatus = "D");

            var response = companyCostCenterService.DeleteCompanyCostCenter(companyCostCenterDomianModels[0].CompanyCode, companyCostCenterDomianModels[0].Division, new List<DomModel.CompanyDivisionCostCenter> { companyCostCenterDomianModels[0] });

            Assert.AreEqual("11", response.Code);
        }





    }
}
