using Evolution.UnitTest.UnitTestCases;
using System;
using System.Collections.Generic;
using System.Text;
using ServiceDomainData = Evolution.Document.Core.Services;
using DomainData = Evolution.Document.Domain.Interfaces.Data;
using Moq;
using DomainModels = Evolution.Document.Domain.Models.Document;
using Evolution.Logging.Interfaces;
using AutoMapper;
using DbModel = Evolution.DbRepository.Models;
using System.Linq;
using Masterdata = Evolution.Master.Domain.Interfaces.Data;
using System.Linq.Expressions;

using MasterService = Evolution.Master.Domain.Interfaces;
using Evolution.Common.Enums;
using Evolution.Common.Models.Messages;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Evolution.UnitTests.Mocks.Data.Projects.Domain;
using Evolution.UnitTests.Mocks.Data.Projects.Db;
using Service = Evolution.Document.Core.Services;
using ValdService = Evolution.Document.Domain.Validations;
using Evolution.ValidationService.Interfaces;
using Evolution.Master.Domain.Interfaces.Services;
using Evolution.Common.Models.Documents;
using Evolution.Document.Domain.Interfaces.Documents;
using Evolution.Document.Domain.Models.Document;
using Evolution.UnitTests.Mocks.Data.Document.Domain;
using Microsoft.Extensions.Options;

namespace Evolution.UnitTests.UnitTestCases.Projects.Services
{
    [TestClass]
    public class ProjectDocumentServicesUnittestCases : BaseTestCase
    {
        IValidationService Validation = null;
        ValdService.IDocumentValidationService validationService = null;
        ServiceDomainData.DocumentService DocumentService = null;
        Mock<DomainData.IDocumentRepository> mockProjectDocumentRepository = null;
        //  Mock<DomainData.IProjectRepository> mockProjectRepository = null;
        Mock<Masterdata.IModuleDocumentTypeRepository> mockDocumenttype = null;
        IList<DomainModels.ModuleDocument> DocumentDomainModel = null;
        Mock<IAppLogger<Service.DocumentService>> mockLogger = null;
        Mock<IMapper> mockMapper = null;
       
        IOptions<DocumentUploadSetting>docUploadSetting = null;
        DocumentUploadSetting documentUploadSetting = null;
        IQueryable<DbModel.Document> mockProjectDbData = null;
        IQueryable<DbModel.ModuleDocumentType> mockModuleDocumentType = null;
        
        IList<DocumentUniqueNameDetail> documentUniqueNames = null;
        [TestInitialize]
        public void InitializeProjectDocumentService()
        {
            mockProjectDocumentRepository = new Mock<DomainData.IDocumentRepository>();
            mockDocumenttype = new Mock<Masterdata.IModuleDocumentTypeRepository>();
            DocumentDomainModel = MockProjectDomainModel.GetProjectDocumentsMockedDomainData();
            mockLogger = new Mock<IAppLogger<ServiceDomainData.DocumentService>>();
            mockProjectDbData = MockProject.GetProjectDocumentsMockData();
            mockModuleDocumentType = MockProject.GetmoduleDocumentTypeMockData();
                docUploadSetting = Options.Create(new DocumentUploadSetting() { FolderName="Evolution2Docs",
                TempFileRefCode= "TEMP",
                FileDefaultExtension= "evo",
                DocumentTypes= ".pdf,.doc,.txt,.msg,.xls,.xlsx"  });
            documentUniqueNames = MockDocumentDomainModel.GetUniqueNameMockedDomainData();
        }
        [TestMethod]
        public void FetchAllProjectDocumentWithoutSearchValue()
        {
            mockProjectDocumentRepository.Setup(x => x.Get(It.IsAny<DomainModels.ModuleDocument>())).Returns(DocumentDomainModel);
            DocumentService = new Service.DocumentService(mockProjectDocumentRepository.Object, validationService, docUploadSetting, mockLogger.Object, Mapper.Instance);
            var response = DocumentService.Get(new DomainModels.ModuleDocument());
            Assert.AreEqual("1", response.Code);
            Assert.AreEqual(2, (response.Result as List<DomainModels.ModuleDocument>).Count);
        }
        
        [TestMethod]
        public void FetchAllProjectDocumentWithSearchValue()
        {
            mockProjectDocumentRepository.Setup(x => x.Get(It.IsAny<DomainModels.ModuleDocument>())).Returns(DocumentDomainModel);
            DocumentService = new Service.DocumentService(mockProjectDocumentRepository.Object, validationService, docUploadSetting, mockLogger.Object, Mapper.Instance);
            var response = DocumentService.Get(new DomainModels.ModuleDocument() { ModuleRefCode = "1" });
            Assert.AreEqual("1", response.Code);
            Assert.AreEqual(2, (response.Result as List<DomainModels.ModuleDocument>).Count);
        }
        [TestMethod]
        public void GetOrphandDocuments()
        {
            mockProjectDocumentRepository.Setup(x => x.Get(It.IsAny<DomainModels.ModuleDocument>())).Returns(DocumentDomainModel);
            DocumentService = new Service.DocumentService(mockProjectDocumentRepository.Object, validationService, docUploadSetting, mockLogger.Object, Mapper.Instance);
            var response = DocumentService.GetOrphandDocuments();
            Assert.AreEqual("1", response.Code);
            Assert.AreEqual(0, (response.Result as List<DomainModels.ModuleDocument>).Count);
        }

        [TestMethod]
        public void SaveProjectDocument()
        {

            mockProjectDocumentRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Document, bool>>>())).Returns(mockProjectDbData.Where(x => x.Id == 1));

            Validation = new Evolution.ValidationService.Services.ValidationService();
            validationService = new Evolution.Document.Infrastructure.Validations.DocumentValidationService(Validation);
            DocumentService = new Service.DocumentService(mockProjectDocumentRepository.Object, validationService, docUploadSetting, mockLogger.Object, Mapper.Instance);
            DocumentDomainModel.ToList().ForEach(x => x.RecordStatus = "N");
            var response = DocumentService.Save(new List<DomainModels.ModuleDocument> { DocumentDomainModel[0] });
            mockProjectDocumentRepository.Verify(m => m.Update(It.IsAny<DbModel.Document>()), Times.AtLeastOnce);
        }

        [TestMethod]
        public void SaveWithNewUniqueNameProjectDocument()
        {

            mockProjectDocumentRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Document, bool>>>())).Returns(mockProjectDbData.Where(x => x.Id == 1));

           
            Validation = new Evolution.ValidationService.Services.ValidationService();
            validationService = new Evolution.Document.Infrastructure.Validations.DocumentValidationService(Validation);
            DocumentService = new Service.DocumentService(mockProjectDocumentRepository.Object, validationService, docUploadSetting, mockLogger.Object, Mapper.Instance);
            DocumentDomainModel.ToList().ForEach(x => x.RecordStatus = "N");
            DocumentDomainModel[0].DocumentUniqueName = "UniqueNameForDocument";
            var response = DocumentService.Save(new List<DomainModels.ModuleDocument> { DocumentDomainModel[0] });
            mockProjectDocumentRepository.Verify(m => m.Add(It.IsAny<DbModel.Document>()), Times.AtLeastOnce);
        }
        [TestMethod]
        public void GenrateUniqueName()
        {

            mockProjectDocumentRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Document, bool>>>())).Returns(mockProjectDbData.Where(x => x.Id == 1));
            Validation = new Evolution.ValidationService.Services.ValidationService();
            validationService = new Evolution.Document.Infrastructure.Validations.DocumentValidationService(Validation);
             
            DocumentService = new Service.DocumentService(mockProjectDocumentRepository.Object, validationService, docUploadSetting, mockLogger.Object, Mapper.Instance);
            var response = DocumentService.GenerateDocumentUniqueName(new List<DomainModels.DocumentUniqueNameDetail> { documentUniqueNames[0] });
            mockProjectDocumentRepository.Verify(m => m.Add(It.IsAny<DbModel.Document>()), Times.AtLeastOnce);

        }
        [TestMethod]
        public void ModifyProjectDocument()
        {
            mockProjectDocumentRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Document, bool>>>())).Returns(mockProjectDbData.Where(x => x.Id == 1));

            Validation = new Evolution.ValidationService.Services.ValidationService();
            validationService = new Evolution.Document.Infrastructure.Validations.DocumentValidationService(Validation);
            DocumentService = new Service.DocumentService(mockProjectDocumentRepository.Object, validationService, docUploadSetting, mockLogger.Object, Mapper.Instance);
            DocumentDomainModel.ToList().ForEach(x => x.RecordStatus = "M");
            var response = DocumentService.Modify(new List<DomainModels.ModuleDocument> { DocumentDomainModel[0] });
            mockProjectDocumentRepository.Verify(m => m.Update(It.IsAny<DbModel.Document>()), Times.AtLeastOnce);
        }

        [TestMethod]
        public void ChangeDocumentStatus()
        {
            mockProjectDocumentRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Document, bool>>>())).Returns(mockProjectDbData.Where(x => x.Id == 1));
            Validation = new Evolution.ValidationService.Services.ValidationService();
            validationService = new Evolution.Document.Infrastructure.Validations.DocumentValidationService(Validation);
            DocumentService = new Service.DocumentService(mockProjectDocumentRepository.Object, validationService, docUploadSetting, mockLogger.Object, Mapper.Instance);
            DocumentDomainModel.ToList().ForEach(x => x.RecordStatus = "M");
            var response = DocumentService.ChangeDocumentStatus(new List<DomainModels.ModuleDocument> { DocumentDomainModel[0] });
            mockProjectDocumentRepository.Verify(m => m.Add(It.IsAny<DbModel.Document>()), Times.AtLeastOnce);
        }

    }

}
