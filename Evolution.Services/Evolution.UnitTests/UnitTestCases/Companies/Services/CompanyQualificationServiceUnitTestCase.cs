using Evolution.Logging.Interfaces;
using Evolution.UnitTests.Mocks.Data.Companies;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using DomainData = Evolution.Company.Domain.Interfaces.Data;
using DomModel = Evolution.Company.Domain.Models.Companies;
using ServiceDomainData = Evolution.Company.Domain.Interfaces.Companies;

namespace Evolution.UnitTests.UnitTestCases.Companies.Services
{
    [TestClass]
   public class CompanyQualificationServiceUnitTestCase
    {
        ServiceDomainData.ICompanyQualificationService companyQualificationService = null;
        Mock<DomainData.ICompanyQualificationRepository> mockCompanyQualificationRepository = null;
        IList<DomModel.CompanyQualification> companyQualificationDomianModels = null;
        Mock<IAppLogger<Company.Core.Services.CompanyQualificationService>> mockLogger = null;

        [TestInitialize]
        public void InitializeCompanyQualificationService()
        {
            mockCompanyQualificationRepository = new Mock<DomainData.ICompanyQualificationRepository>();
            companyQualificationDomianModels = MockCompanyDomainModel.GetCompanyQualificationMockedDomainModelData();
            mockLogger = new Mock<IAppLogger<Company.Core.Services.CompanyQualificationService>>();
        }

        [TestMethod]
        public void FetchCompanyQualificationListWithoutSearchValue()
        {
            mockCompanyQualificationRepository.Setup(x => x.Search(It.IsAny<DomModel.CompanyQualification>())).Returns(companyQualificationDomianModels);

            companyQualificationService = new Company.Core.Services.CompanyQualificationService(mockCompanyQualificationRepository.Object, mockLogger.Object);

            var response = companyQualificationService.GetCompanyQualification(new DomModel.CompanyQualification());

            Assert.AreEqual("1", response.Code);
            Assert.AreEqual(4, (response.Result as List<DomModel.CompanyQualification>).Count);
        }

        [TestMethod]
        public void FetchCompanyQualificationListWithNullSearchModel()
        {
            mockCompanyQualificationRepository.Setup(x => x.Search(It.IsAny<DomModel.CompanyQualification>())).Returns(companyQualificationDomianModels);

            companyQualificationService = new Company.Core.Services.CompanyQualificationService(mockCompanyQualificationRepository.Object, mockLogger.Object);

            var response = companyQualificationService.GetCompanyQualification(null);

            Assert.AreEqual("1", response.Code);
            Assert.AreEqual(4, (response.Result as List<DomModel.CompanyQualification>).Count);
        }

        [TestMethod]
        public void FetchCompanyQualificationListByCompanyCode()
        {
            mockCompanyQualificationRepository.Setup(x => x.Search(It.Is<DomModel.CompanyQualification>(c => c.CompanyCode == "UK050"))).Returns(companyQualificationDomianModels);

            companyQualificationService = new Company.Core.Services.CompanyQualificationService(mockCompanyQualificationRepository.Object, mockLogger.Object);

            var response = companyQualificationService.GetCompanyQualification(new DomModel.CompanyQualification { CompanyCode = "UK050" });

            Assert.AreEqual("1", response.Code);
            Assert.AreEqual(4, (response.Result as List<DomModel.CompanyQualification>).Count);
        }

        [TestMethod]
        public void FetchCompanyQualificationListByWildCardSearchWithQualificationTypeStartWith()
        {
            mockCompanyQualificationRepository.Setup(x => x.Search(It.Is<DomModel.CompanyQualification>(c => c.Qualification.StartsWith("City & Guild ")))).Returns(new List<DomModel.CompanyQualification> { companyQualificationDomianModels[0] , companyQualificationDomianModels[1] });

            companyQualificationService = new Company.Core.Services.CompanyQualificationService(mockCompanyQualificationRepository.Object, mockLogger.Object);

            var response = companyQualificationService.GetCompanyQualification(new DomModel.CompanyQualification { Qualification = "City & Guild *" });

            Assert.AreEqual("1", response.Code);
            Assert.AreEqual(2, (response.Result as List<DomModel.CompanyQualification>).Count);
        }

        [TestMethod]
        public void FetchCompanyQualificationListByWildCardSearchWithQualificationTypeEndWith()
        {
            mockCompanyQualificationRepository.Setup(x => x.Search(It.Is<DomModel.CompanyQualification>(c => c.Qualification.EndsWith("- Mechanical")))).Returns(companyQualificationDomianModels);

            companyQualificationService = new Company.Core.Services.CompanyQualificationService(mockCompanyQualificationRepository.Object, mockLogger.Object);

            var response = companyQualificationService.GetCompanyQualification(new DomModel.CompanyQualification { Qualification = "*- Mechanical" });

            Assert.AreEqual("1", response.Code);
            Assert.AreEqual(4, (response.Result as List<DomModel.CompanyQualification>).Count);
        }

        [TestMethod]
        public void FetchCompanyQualificationListByWildCardSearchWithQualificationTypeContains()
        {
            mockCompanyQualificationRepository.Setup(x => x.Search(It.Is<DomModel.CompanyQualification>(c => c.Qualification.Contains("Guild Craft")))).Returns(new List<DomModel.CompanyQualification> { companyQualificationDomianModels[0] });

            companyQualificationService = new Company.Core.Services.CompanyQualificationService(mockCompanyQualificationRepository.Object, mockLogger.Object);

            var response = companyQualificationService.GetCompanyQualification(new DomModel.CompanyQualification { Qualification = "*Guild Craft*" });

            Assert.AreEqual("1", response.Code);
            Assert.AreEqual(1, (response.Result as List<DomModel.CompanyQualification>).Count);
        }

        [TestMethod]
        public void ThrowsExceptionWhileFetchingCompanyQualificationList()
        {
            mockCompanyQualificationRepository.Setup(x => x.Search(It.IsAny<DomModel.CompanyQualification>())).Throws(new Exception("Exception occured while performing some operation."));
            companyQualificationService = new Company.Core.Services.CompanyQualificationService(mockCompanyQualificationRepository.Object, mockLogger.Object);

            var response = companyQualificationService.GetCompanyQualification(new DomModel.CompanyQualification());

            Assert.AreEqual("11", response.Code);
            Assert.IsNotNull(response.Messages);
            Assert.AreEqual("31", response.Messages.FirstOrDefault().Code);
        }
         
    }
}
