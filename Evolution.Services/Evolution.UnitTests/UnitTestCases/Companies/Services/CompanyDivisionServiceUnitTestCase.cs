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
using Evolution.DbRepository.Interfaces.Master;
using System.Linq.Expressions;
using Evolution.UnitTests.Mocks.Data.Companies.Db;
using MasterService = Evolution.Master.Domain.Interfaces.Services;

namespace Evolution.UnitTests.UnitTestCases.Companies.Services
{
    [TestClass]
    public class CompanyDivisionServiceUnitTestCase
    {
        ServiceDomainData.ICompanyDivisionService companyDivisionService = null;
        Mock<MasterService.IDivisionService> divisionService = null;
         Mock<DomainData.ICompanyDivisionRepository> mockCompanyDivisionRepository = null;
        Mock<DomainData.ICompanyRepository> mockCompanyRepository = null;
        Mock<IDataRepository> mockDataRepository = null;
        IQueryable<DbModel.CompanyDivision> mockCompanyDivisionDbData = null;
        IList<DomModel.CompanyDivision> companyDivisionDomianModels = null;
        Mock<IAppLogger<Company.Core.Services.CompanyDivisionService>> mockLogger = null;
        IQueryable<DbModel.Company> mockComapanyDbData = null;
        IQueryable<DbModel.Data> mockDivisionDbData = null;

        [TestInitialize]
        public void InitializeCompanyDivisionService()
        {
            divisionService = new Mock <MasterService.IDivisionService>();
              mockCompanyDivisionRepository = new Mock<DomainData.ICompanyDivisionRepository>();
            mockCompanyRepository = new Mock<DomainData.ICompanyRepository>();
            mockDataRepository = new Mock<IDataRepository>();
            companyDivisionDomianModels = MockCompanyDomainModel.GetCompanyDivisionMockedDomainModelData();
            mockLogger = new Mock<IAppLogger<Company.Core.Services.CompanyDivisionService>>();
            mockCompanyDivisionDbData = MockCompany.GetCompanyDivisionMockData();
            mockComapanyDbData = MockCompany.GetCompanyMockData();
            mockDivisionDbData = MockCompany.GetDivisionMockData();
        }

        [TestMethod]
        public void FetchCompanyDivisionListWithoutSearchValue()
        {
            mockCompanyDivisionRepository.Setup(x => x.Search(It.IsAny<DomModel.CompanyDivision>())).Returns(companyDivisionDomianModels);

            companyDivisionService = new Company.Core.Services.CompanyDivisionService(divisionService.Object,mockDataRepository.Object,mockCompanyRepository.Object,mockCompanyDivisionRepository.Object, mockLogger.Object);

            var response = companyDivisionService.GetCompanyDivision(new DomModel.CompanyDivision());

            Assert.AreEqual("1", response.Code);
            Assert.AreEqual(3, (response.Result as List<DomModel.CompanyDivision>).Count);
        }

        [TestMethod]
        public void FetchCompanyDivisionListWithNullSearchModel()
        {
            mockCompanyDivisionRepository.Setup(x => x.Search(It.IsAny<DomModel.CompanyDivision>())).Returns(companyDivisionDomianModels);

            companyDivisionService = new Company.Core.Services.CompanyDivisionService(divisionService.Object,mockDataRepository.Object, mockCompanyRepository.Object, mockCompanyDivisionRepository.Object, mockLogger.Object);

            var response = companyDivisionService.GetCompanyDivision(null);

            Assert.AreEqual("1", response.Code);
            Assert.AreEqual(3, (response.Result as List<DomModel.CompanyDivision>).Count);
        }

        [TestMethod]
        public void FetchCompanyDivisionListByCompanyCode()
        {
            mockCompanyDivisionRepository.Setup(x => x.Search(It.Is<DomModel.CompanyDivision>(c => c.CompanyCode == "UK050"))).Returns(new List<DomModel.CompanyDivision> { companyDivisionDomianModels[0] });

            companyDivisionService = new Company.Core.Services.CompanyDivisionService(divisionService.Object,mockDataRepository.Object, mockCompanyRepository.Object,mockCompanyDivisionRepository.Object, mockLogger.Object);

            var response = companyDivisionService.GetCompanyDivision(new DomModel.CompanyDivision{ CompanyCode = "UK050" });

            Assert.AreEqual("1", response.Code);
            Assert.AreEqual(1, (response.Result as List<DomModel.CompanyDivision>).Count);
        }

        [TestMethod]
        public void FetchCompanyDivisionListByWildCardSearchWithDivisionNameStartWith()
        {
            mockCompanyDivisionRepository.Setup(x => x.Search(It.Is<DomModel.CompanyDivision>(c => c.DivisionName.StartsWith("P")))).Returns(new List<DomModel.CompanyDivision> { companyDivisionDomianModels[1]});

            companyDivisionService = new Company.Core.Services.CompanyDivisionService(divisionService.Object,mockDataRepository.Object, mockCompanyRepository.Object, mockCompanyDivisionRepository.Object, mockLogger.Object);

            var response = companyDivisionService.GetCompanyDivision(new DomModel.CompanyDivision { DivisionName = "P*" });

            Assert.AreEqual("1", response.Code);
            Assert.AreEqual(1, (response.Result as List<DomModel.CompanyDivision>).Count);
        }

        [TestMethod]
        public void FetchCompanyDivisionListByWildCardSearchWithDivisionNameEndWith()
        {
            mockCompanyDivisionRepository.Setup(x => x.Search(It.Is<DomModel.CompanyDivision>(c => c.DivisionName.EndsWith("on")))).Returns(new List<DomModel.CompanyDivision> { companyDivisionDomianModels[0], companyDivisionDomianModels[2] });

            companyDivisionService = new Company.Core.Services.CompanyDivisionService(divisionService.Object,mockDataRepository.Object, mockCompanyRepository.Object, mockCompanyDivisionRepository.Object, mockLogger.Object);

            var response = companyDivisionService.GetCompanyDivision(new DomModel.CompanyDivision { DivisionName = "*on" });

            Assert.AreEqual("1", response.Code);
            Assert.AreEqual(2, (response.Result as List<DomModel.CompanyDivision>).Count);
        }

        [TestMethod]
        public void FetchCompanyDivisionListByWildCardSearchWithDivisionNameContains()
        {
            mockCompanyDivisionRepository.Setup(x => x.Search(It.Is<DomModel.CompanyDivision>(c => c.DivisionName.Contains("spec")))).Returns(new List<DomModel.CompanyDivision> { companyDivisionDomianModels[0], companyDivisionDomianModels[2] });

            companyDivisionService = new Company.Core.Services.CompanyDivisionService(divisionService.Object,mockDataRepository.Object, mockCompanyRepository.Object, mockCompanyDivisionRepository.Object, mockLogger.Object);

            var response = companyDivisionService.GetCompanyDivision(new DomModel.CompanyDivision { DivisionName = "*spec*" });

            Assert.AreEqual("1", response.Code);
            Assert.AreEqual(2, (response.Result as List<DomModel.CompanyDivision>).Count);
        }

        [TestMethod]
        public void ThrowsExceptionWhileFetchingCompanyDivisionList()
        {
            mockCompanyDivisionRepository.Setup(x => x.Search(It.IsAny<DomModel.CompanyDivision>())).Throws(new Exception("Exception occured while performing some operation."));
            companyDivisionService = new Company.Core.Services.CompanyDivisionService(divisionService.Object,mockDataRepository.Object, mockCompanyRepository.Object, mockCompanyDivisionRepository.Object, mockLogger.Object);

            var response = companyDivisionService.GetCompanyDivision(new DomModel.CompanyDivision());

            Assert.AreEqual("11", response.Code);
            Assert.IsNotNull(response.Messages);
            Assert.AreEqual("31", response.Messages.FirstOrDefault().Code);
        }

        [TestMethod]
        public void SaveCompanyDivisions()
        {
            mockCompanyDivisionRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.CompanyDivision, bool>>>())).Returns((Expression<Func<DbModel.CompanyDivision, bool>> predicate) => mockCompanyDivisionDbData.Where(predicate));
            mockCompanyRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Company, bool>>>())).Returns(mockComapanyDbData);
            mockDataRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Data, bool>>>())).Returns((Expression<Func<DbModel.Data, bool>> predicate) => mockDivisionDbData.Where(predicate));

            companyDivisionService = new Company.Core.Services.CompanyDivisionService(divisionService.Object,mockDataRepository.Object, mockCompanyRepository.Object, mockCompanyDivisionRepository.Object, mockLogger.Object);
            companyDivisionDomianModels.ToList().ForEach(x => x.RecordStatus = "N");
            companyDivisionDomianModels[0].DivisionName = "Technical Training";
            var response = companyDivisionService.SaveCompanyDivision(companyDivisionDomianModels[0].CompanyCode, new List<DomModel.CompanyDivision> { companyDivisionDomianModels[0] });

            mockCompanyDivisionRepository.Verify(m => m.Add(It.IsAny<IList<DbModel.CompanyDivision>>()), Times.AtLeastOnce);
        }

        [TestMethod]
        public void ModifyCompanyDivision()
        {
             
            mockCompanyDivisionRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.CompanyDivision, bool>>>())).Returns((Expression<Func<DbModel.CompanyDivision, bool>> predicate) => mockCompanyDivisionDbData.Where(predicate));
            mockCompanyRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Company, bool>>>())).Returns(mockComapanyDbData);
            mockDataRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Data, bool>>>())).Returns((Expression<Func<DbModel.Data, bool>> predicate) => mockDivisionDbData.Where(predicate));
             
            companyDivisionService = new Company.Core.Services.CompanyDivisionService(divisionService.Object,mockDataRepository.Object, mockCompanyRepository.Object, mockCompanyDivisionRepository.Object, mockLogger.Object);
            companyDivisionDomianModels.ToList().ForEach(x => x.RecordStatus = "M"); 
            var response = companyDivisionService.ModifyCompanyDivision(companyDivisionDomianModels[0].CompanyCode,  new List<DomModel.CompanyDivision> { companyDivisionDomianModels[0] });

            mockCompanyDivisionRepository.Verify(m => m.Update(It.IsAny<DbModel.CompanyDivision>()), Times.AtLeast(1));
        }

        //[TestMethod]
        //public void ModifyCompanyDivisionUpdatingDivisionToNewDivision()
        //{

        //    mockCompanyDivisionRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.CompanyDivision, bool>>>())).Returns((Expression<Func<DbModel.CompanyDivision, bool>> predicate) => mockCompanyDivisionDbData.Where(predicate));
        //    mockCompanyRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Company, bool>>>())).Returns(mockComapanyDbData);
        //    mockDataRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Data, bool>>>())).Returns((Expression<Func<DbModel.Data, bool>> predicate) => mockDivisionDbData.Where(predicate));

        //    companyDivisionService = new Company.Core.Services.CompanyDivisionService(mockDataRepository.Object, mockCompanyRepository.Object, mockCompanyDivisionRepository.Object, mockLogger.Object);
        //    companyDivisionDomianModels.ToList().ForEach(x => x.RecordStatus = "M");
        //    companyDivisionDomianModels[0].DivisionCode = "115";
        //    companyDivisionDomianModels[0].DivisionName = "Technical Training";
        //    var response = companyDivisionService.ModifyCompanyDivision(companyDivisionDomianModels[0].CompanyCode, new List<DomModel.CompanyDivision> { companyDivisionDomianModels[0] });

        //    mockCompanyDivisionRepository.Verify(m => m.Update(It.IsAny<DbModel.CompanyDivision>()), Times.AtLeast(1));
        //}

        //[TestMethod]
        //public void ModifyCompanyDivisionAlreadyExistingDivision()
        //{ 
        //    mockCompanyDivisionRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.CompanyDivision, bool>>>())).Returns((Expression<Func<DbModel.CompanyDivision, bool>> predicate) => mockCompanyDivisionDbData.Where(predicate));
        //    mockCompanyRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Company, bool>>>())).Returns(mockComapanyDbData);
        //    mockDataRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Data, bool>>>())).Returns((Expression<Func<DbModel.Data, bool>> predicate) => mockDivisionDbData.Where(predicate));

        //    companyDivisionService = new Company.Core.Services.CompanyDivisionService(mockDataRepository.Object, mockCompanyRepository.Object, mockCompanyDivisionRepository.Object, mockLogger.Object);
        //    companyDivisionDomianModels.ToList().ForEach(x => x.RecordStatus = "M"); 
        //    var response = companyDivisionService.ModifyCompanyDivision(companyDivisionDomianModels[0].CompanyCode, new List<DomModel.CompanyDivision> { companyDivisionDomianModels[0] });

        //    mockCompanyDivisionRepository.Verify(m => m.Update(It.IsAny<DbModel.CompanyDivision>()), Times.Never);
        //    Assert.AreEqual("1215", response.Messages.FirstOrDefault().Code);
        //}
        [TestMethod]
        public void DeleteCompanyDivision()
        {

            mockCompanyDivisionRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.CompanyDivision, bool>>>())).Returns((Expression<Func<DbModel.CompanyDivision, bool>> predicate) => mockCompanyDivisionDbData.Where(predicate));
            mockCompanyRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Company, bool>>>())).Returns(mockComapanyDbData);
            mockDataRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Data, bool>>>())).Returns((Expression<Func<DbModel.Data, bool>> predicate) => mockDivisionDbData.Where(predicate));

            companyDivisionService = new Company.Core.Services.CompanyDivisionService(divisionService.Object,mockDataRepository.Object, mockCompanyRepository.Object, mockCompanyDivisionRepository.Object, mockLogger.Object);
            companyDivisionDomianModels.ToList().ForEach(x => x.RecordStatus = "D");
            var response = companyDivisionService.DeleteCompanyDivision(companyDivisionDomianModels[0].CompanyCode, new List<DomModel.CompanyDivision> { companyDivisionDomianModels[0] });

            mockCompanyDivisionRepository.Verify(m => m.Delete(It.IsAny<DbModel.CompanyDivision>()), Times.AtLeast(1));
        }
        [TestMethod]
        public void ModifyCompanyCostCenterUpdateCountMissMatch()
        {
            mockCompanyDivisionRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.CompanyDivision, bool>>>())).Returns((Expression<Func<DbModel.CompanyDivision, bool>> predicate) => mockCompanyDivisionDbData.Where(predicate));
            mockCompanyRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Company, bool>>>())).Returns(mockComapanyDbData);
            mockDataRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Data, bool>>>())).Returns((Expression<Func<DbModel.Data, bool>> predicate) => mockDivisionDbData.Where(predicate));

            companyDivisionService = new Company.Core.Services.CompanyDivisionService(divisionService.Object,mockDataRepository.Object, mockCompanyRepository.Object, mockCompanyDivisionRepository.Object, mockLogger.Object);
            companyDivisionDomianModels.ToList().ForEach(x => x.RecordStatus = "M");
            companyDivisionDomianModels[0].UpdateCount = 2;

            var response = companyDivisionService.ModifyCompanyDivision(companyDivisionDomianModels[0].CompanyCode, new List<DomModel.CompanyDivision> { companyDivisionDomianModels[0] });

           
            Assert.AreEqual("11", response.Code);
        }
        [TestMethod]
        public void SaveCompanyCostCentersWithDuplicateCostCenter()
        {
            mockCompanyDivisionRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.CompanyDivision, bool>>>())).Returns((Expression<Func<DbModel.CompanyDivision, bool>> predicate) => mockCompanyDivisionDbData.Where(predicate));
            mockCompanyRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Company, bool>>>())).Returns(mockComapanyDbData);
            mockDataRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Data, bool>>>())).Returns((Expression<Func<DbModel.Data, bool>> predicate) => mockDivisionDbData.Where(predicate));

            companyDivisionService = new Company.Core.Services.CompanyDivisionService(divisionService.Object,mockDataRepository.Object, mockCompanyRepository.Object, mockCompanyDivisionRepository.Object, mockLogger.Object);
            companyDivisionDomianModels.ToList().ForEach(x => x.RecordStatus = "N");
            
            var response = companyDivisionService.SaveCompanyDivision(companyDivisionDomianModels[0].CompanyCode, new List<DomModel.CompanyDivision> { companyDivisionDomianModels[0] });

            
            Assert.AreEqual("11", response.Code);

        }
        [TestMethod]

        public void ModifyCompanyDivisonwithNULLDivisonName()
        {
            mockCompanyDivisionRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.CompanyDivision, bool>>>())).Returns((Expression<Func<DbModel.CompanyDivision, bool>> predicate) => mockCompanyDivisionDbData.Where(predicate));
            mockCompanyRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Company, bool>>>())).Returns(mockComapanyDbData);
            mockDataRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Data, bool>>>())).Returns((Expression<Func<DbModel.Data, bool>> predicate) => mockDivisionDbData.Where(predicate));

            companyDivisionService = new Company.Core.Services.CompanyDivisionService(divisionService.Object,mockDataRepository.Object, mockCompanyRepository.Object, mockCompanyDivisionRepository.Object, mockLogger.Object);
            companyDivisionDomianModels.ToList().ForEach(x => x.RecordStatus = "M");
            var response = companyDivisionService.ModifyCompanyDivision(companyDivisionDomianModels[0].CompanyCode, null);
            Assert.AreEqual("1", response.Code);

        }
        [TestMethod]
        public void ModifyCompanyDivisonwithInvalidDivision()
        {
            mockCompanyDivisionRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.CompanyDivision, bool>>>())).Returns((Expression<Func<DbModel.CompanyDivision, bool>> predicate) => mockCompanyDivisionDbData.Where(predicate));
            mockCompanyRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Company, bool>>>())).Returns(mockComapanyDbData);
            mockDataRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Data, bool>>>())).Returns((Expression<Func<DbModel.Data, bool>> predicate) => mockDivisionDbData.Where(predicate));

            companyDivisionService = new Company.Core.Services.CompanyDivisionService(divisionService.Object,mockDataRepository.Object, mockCompanyRepository.Object, mockCompanyDivisionRepository.Object, mockLogger.Object);
            companyDivisionDomianModels.ToList().ForEach(x => x.RecordStatus = "M");
            companyDivisionDomianModels[0].DivisionName = "InvalidDivison";
            var response = companyDivisionService.ModifyCompanyDivision(companyDivisionDomianModels[0].CompanyCode, new List<DomModel.CompanyDivision> { companyDivisionDomianModels[0] });
            Assert.AreEqual("11", response.Code);


        }
        [TestMethod]
        public void ModifyCompanyCostCenterWithRecordStatusNull()
        {

            mockCompanyDivisionRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.CompanyDivision, bool>>>())).Returns((Expression<Func<DbModel.CompanyDivision, bool>> predicate) => mockCompanyDivisionDbData.Where(predicate));
            mockCompanyRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Company, bool>>>())).Returns(mockComapanyDbData);
            mockDataRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Data, bool>>>())).Returns((Expression<Func<DbModel.Data, bool>> predicate) => mockDivisionDbData.Where(predicate));

            companyDivisionService = new Company.Core.Services.CompanyDivisionService(divisionService.Object,mockDataRepository.Object, mockCompanyRepository.Object, mockCompanyDivisionRepository.Object, mockLogger.Object);
            companyDivisionDomianModels[0].RecordStatus = null;
            var response = companyDivisionService.ModifyCompanyDivision(companyDivisionDomianModels[0].CompanyCode, null);

           
            Assert.AreEqual("1", response.Code);

            // mockCompanyCostCenterRepository.Verify(m => m.Update(It.IsAny<DbModel.CompanyDivisionCostCenter>()), Times.AtLeast(1));
        }
        [TestMethod]
        public void SaveCompanyDivisonsWithCompanyCodeAndDivisonNameNUll()
        {

            mockCompanyDivisionRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.CompanyDivision, bool>>>())).Returns((Expression<Func<DbModel.CompanyDivision, bool>> predicate) => mockCompanyDivisionDbData.Where(predicate));
            mockCompanyRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Company, bool>>>())).Returns(mockComapanyDbData);
            mockDataRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Data, bool>>>())).Returns((Expression<Func<DbModel.Data, bool>> predicate) => mockDivisionDbData.Where(predicate));

            companyDivisionService = new Company.Core.Services.CompanyDivisionService(divisionService.Object,mockDataRepository.Object, mockCompanyRepository.Object, mockCompanyDivisionRepository.Object, mockLogger.Object);
            companyDivisionDomianModels.ToList().ForEach(x => x.RecordStatus = "N");
            
            var response = companyDivisionService.SaveCompanyDivision(null, null);

          

            Assert.AreEqual("11", response.Code);

        }

    }
}
