using Evolution.UnitTest.UnitTestCases;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using ServiceDomainData = Evolution.Project.Domain.Interfaces.Projects;
using DomainData = Evolution.Project.Domain.Interfaces.Data;
using Moq;
using DomModel = Evolution.Project.Domain.Models.Projects;
using Evolution.Logging.Interfaces;
using AutoMapper;
using DbModel = Evolution.DbRepository.Models;
using System.Linq;
using Evolution.UnitTests.Mocks.Data.Projects.Domain;
using Evolution.UnitTests.Mocks.Data.Contracts.Db;
using Masterdata = Evolution.Master.Domain.Interfaces.Data;
using System.Linq.Expressions;
using Microsoft.Extensions.Options;
using MasterService = Evolution.Master.Domain.Interfaces;
using Evolution.Common.Enums;
using dbRepo = Evolution.DbRepository.Interfaces.Master;
using Service = Evolution.Project.Core.Services;
using Evolution.UnitTests.Mocks.Data.Projects.Db;
using ValdService = Evolution.Project.Domain.Interfaces.Validations;
using Evolution.ValidationService.Interfaces;

namespace Evolution.UnitTests.UnitTestCases.Projects.Services
{
    [TestClass]
   public class ProjectInvoiceAttachmentServiceUnitTestCases :BaseTestCase
    {
        IValidationService Validation = null;
        ValdService.IInvoiceAttachmentValidationService validationService = null;
        ServiceDomainData.IProjectInvoiceAttachmentService ProjectInvoiceAttachmentService = null;
        Mock<DomainData.IProjectInvoiceAttachmentRepository> mockProjectinvoiceAttachmentRepository = null;
        Mock<DomainData.IProjectRepository> mockProjectRepository = null;
        Mock<Masterdata.IModuleDocumentTypeRepository> mockDocumenttype = null;
        IList<DomModel.ProjectInvoiceAttachment> projectInvoiceAttachmentDomianModels = null;
        Mock<IAppLogger<Service.ProjectInvoiceAttachmentService>> mockLogger = null;
        Mock<IMapper> mockMapper = null;
        IQueryable<DbModel.Project> mockProjectDbData = null;
        IQueryable<DbModel.ProjectInvoiceAttachment> mockProjectInvoiceAttachmentDbData = null;
        IQueryable<DbModel.ModuleDocumentType> mockModuleDocumentType = null;

        [TestInitialize]
        public void InitializeProjectInvoiceAttachmentService()
        {
            mockProjectinvoiceAttachmentRepository = new Mock<DomainData.IProjectInvoiceAttachmentRepository>();
            mockProjectRepository = new Mock<DomainData.IProjectRepository>();
            mockDocumenttype = new Mock<Masterdata.IModuleDocumentTypeRepository>();
            projectInvoiceAttachmentDomianModels = MockProjectDomainModel.GetProjectInvoiceAttachmentsMockedDomaindata();
            mockLogger = new Mock<IAppLogger<Service.ProjectInvoiceAttachmentService>>();
            mockMapper = new Mock<IMapper>();
            mockProjectDbData = MockProject.GetprojectMockData();
            mockProjectInvoiceAttachmentDbData = MockProject.GetProjectInvoiceAttachmentsMockedData();
            mockModuleDocumentType = MockProject.GetmoduleDocumentTypeMockData();

        }

        [TestMethod]
        public void FetchAllProjectInvoiceAttachmentWithoutSearchValue()
        {
            mockProjectinvoiceAttachmentRepository.Setup(x => x.Search(It.IsAny<DomModel.ProjectInvoiceAttachment>())).Returns(projectInvoiceAttachmentDomianModels);
            ProjectInvoiceAttachmentService = new Service.ProjectInvoiceAttachmentService( mockLogger.Object, mockProjectinvoiceAttachmentRepository.Object, mockProjectRepository.Object, mockDocumenttype.Object, validationService);
            var response = ProjectInvoiceAttachmentService.GetProjectInvoiceAttachments(new DomModel.ProjectInvoiceAttachment());
            Assert.AreEqual("1", response.Code);
            Assert.AreEqual(2, (response.Result as List<DomModel.ProjectInvoiceAttachment>).Count);
        }
        [TestMethod]
        public void FetchAllProjectInvoiceAttachmentWithSearchValue()
        {
            mockProjectinvoiceAttachmentRepository.Setup(x => x.Search(It.Is<DomModel.ProjectInvoiceAttachment>(c => c.ProjectNumber == 1))).Returns(new List<DomModel.ProjectInvoiceAttachment> { projectInvoiceAttachmentDomianModels[0] });
            ProjectInvoiceAttachmentService = new Service.ProjectInvoiceAttachmentService(mockLogger.Object, mockProjectinvoiceAttachmentRepository.Object, mockProjectRepository.Object, mockDocumenttype.Object, validationService);
            var response = ProjectInvoiceAttachmentService.GetProjectInvoiceAttachments(new DomModel.ProjectInvoiceAttachment { ProjectNumber = 1 });


            Assert.AreEqual("1", response.Code);
            Assert.AreEqual(1, (response.Result as List<DomModel.ProjectInvoiceAttachment>).Count);
        }

        [TestMethod]
        public void FetchProjectInvoiceAttachmentListWithNullSearchModel()
        {
            mockProjectinvoiceAttachmentRepository.Setup(x => x.Search(It.IsAny<DomModel.ProjectInvoiceAttachment>())).Returns(projectInvoiceAttachmentDomianModels);

            ProjectInvoiceAttachmentService = new Service.ProjectInvoiceAttachmentService(mockLogger.Object, mockProjectinvoiceAttachmentRepository.Object, mockProjectRepository.Object, mockDocumenttype.Object, validationService);
        
            var response = ProjectInvoiceAttachmentService.GetProjectInvoiceAttachments(null);

            Assert.AreEqual("1", response.Code);
            Assert.AreEqual(2, (response.Result as List<DomModel.ProjectInvoiceAttachment>).Count);
        }
        [TestMethod]
        public void ThrowsExceptionWhileFetchingInvoiceAttachment()
        {
            mockProjectinvoiceAttachmentRepository.Setup(x => x.Search(It.IsAny<DomModel.ProjectInvoiceAttachment>())).Throws(new Exception("Exception occured while performing some operation."));
            ProjectInvoiceAttachmentService = new Service.ProjectInvoiceAttachmentService(mockLogger.Object, mockProjectinvoiceAttachmentRepository.Object, mockProjectRepository.Object, mockDocumenttype.Object, validationService);
            

            var response = ProjectInvoiceAttachmentService.GetProjectInvoiceAttachments(new DomModel.ProjectInvoiceAttachment());

            Assert.AreEqual("11", response.Code);
            Assert.IsNotNull(response.Messages);
            Assert.AreEqual("31", response.Messages.FirstOrDefault().Code);
        }

        [TestMethod]
        public void FetchProjectInvoiceAttachmentListByWildCardSearchWithNameStartWith()
        {
            mockProjectinvoiceAttachmentRepository.Setup(x => x.Search(It.Is<DomModel.ProjectInvoiceAttachment>(c => c.DocumentType.StartsWith("Con")))).Returns(new List<DomModel.ProjectInvoiceAttachment> { projectInvoiceAttachmentDomianModels[0] });

            ProjectInvoiceAttachmentService = new Service.ProjectInvoiceAttachmentService( mockLogger.Object, mockProjectinvoiceAttachmentRepository.Object, mockProjectRepository.Object, mockDocumenttype.Object, validationService);

            var response = ProjectInvoiceAttachmentService.GetProjectInvoiceAttachments(new DomModel.ProjectInvoiceAttachment { DocumentType = "Con*" });

            Assert.AreEqual("1", response.Code);
            Assert.AreEqual(1, (response.Result as List<DomModel.ProjectInvoiceAttachment>).Count);
        }
        [TestMethod]
        public void FetchProjectInvoiceAttachmentListByWildCardSearchWithNameEndsWith()
        {
            mockProjectinvoiceAttachmentRepository.Setup(x => x.Search(It.Is<DomModel.ProjectInvoiceAttachment>(c => c.DocumentType.EndsWith("Report")))).Returns(new List<DomModel.ProjectInvoiceAttachment> { projectInvoiceAttachmentDomianModels[1] });

            ProjectInvoiceAttachmentService = new Service.ProjectInvoiceAttachmentService(mockLogger.Object, mockProjectinvoiceAttachmentRepository.Object, mockProjectRepository.Object, mockDocumenttype.Object, validationService);

            var response = ProjectInvoiceAttachmentService.GetProjectInvoiceAttachments(new DomModel.ProjectInvoiceAttachment { DocumentType = "*Report" });

            Assert.AreEqual("1", response.Code);
            Assert.AreEqual(1, (response.Result as List<DomModel.ProjectInvoiceAttachment>).Count);
        }
        [TestMethod]
        public void FetchInvoiceAttachmentListByWildCardSearchWithDocumentNameContains()
        {
            mockProjectinvoiceAttachmentRepository.Setup(x => x.Search(It.Is<DomModel.ProjectInvoiceAttachment>(c => c.DocumentType.Contains("Report")))).Returns(new List<DomModel.ProjectInvoiceAttachment> { projectInvoiceAttachmentDomianModels[1] });

            ProjectInvoiceAttachmentService = new Service.ProjectInvoiceAttachmentService( mockLogger.Object, mockProjectinvoiceAttachmentRepository.Object, mockProjectRepository.Object, mockDocumenttype.Object, validationService);

            var response = ProjectInvoiceAttachmentService.GetProjectInvoiceAttachments(new DomModel.ProjectInvoiceAttachment { DocumentType = "*Report*" });

            Assert.AreEqual("1", response.Code);
            Assert.AreEqual(1, (response.Result as List<DomModel.ProjectInvoiceAttachment>).Count);
        }
        [TestMethod]
        public void SaveProjectInvoiceAttachment()
        {

            mockProjectRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Project, bool>>>())).Returns(mockProjectDbData);
            mockProjectinvoiceAttachmentRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.ProjectInvoiceAttachment, bool>>>())).Returns((Expression<Func<DbModel.ProjectInvoiceAttachment, bool>> predicate) => mockProjectInvoiceAttachmentDbData.Where(predicate));
            mockDocumenttype.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.ModuleDocumentType, bool>>>())).Returns((Expression<Func<DbModel.ModuleDocumentType, bool>> predicate) => mockModuleDocumentType.Where(predicate));
            Validation = new Evolution.ValidationService.Services.ValidationService();
            validationService = new Evolution.Project.Infrastructure.Validations.InvoiceAttachmentValidationService(Validation);
            ProjectInvoiceAttachmentService = new Service.ProjectInvoiceAttachmentService(mockLogger.Object, mockProjectinvoiceAttachmentRepository.Object, mockProjectRepository.Object, mockDocumenttype.Object, validationService);
            projectInvoiceAttachmentDomianModels.ToList().ForEach(x => x.RecordStatus = "N");
            var response = ProjectInvoiceAttachmentService.SaveProjectInvoiceAttachments((int)projectInvoiceAttachmentDomianModels[0].ProjectNumber, new List<DomModel.ProjectInvoiceAttachment> { projectInvoiceAttachmentDomianModels[0] });
            mockProjectinvoiceAttachmentRepository.Verify(m => m.Add(It.IsAny<IList<DbModel.ProjectInvoiceAttachment>>()), Times.AtLeastOnce);
        }
        [TestMethod]
        public void ModifyProjectInvoiceAttachment()
        {

            mockProjectRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Project, bool>>>())).Returns(mockProjectDbData);
            mockProjectinvoiceAttachmentRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.ProjectInvoiceAttachment, bool>>>())).Returns((Expression<Func<DbModel.ProjectInvoiceAttachment, bool>> predicate) => mockProjectInvoiceAttachmentDbData.Where(predicate));
            mockDocumenttype.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.ModuleDocumentType, bool>>>())).Returns((Expression<Func<DbModel.ModuleDocumentType, bool>> predicate) => mockModuleDocumentType.Where(predicate));
            Validation = new Evolution.ValidationService.Services.ValidationService();
            validationService = new Evolution.Project.Infrastructure.Validations.InvoiceAttachmentValidationService(Validation);
            ProjectInvoiceAttachmentService = new Service.ProjectInvoiceAttachmentService(mockLogger.Object, mockProjectinvoiceAttachmentRepository.Object, mockProjectRepository.Object, mockDocumenttype.Object, validationService);
            projectInvoiceAttachmentDomianModels.ToList().ForEach(x => x.RecordStatus = "M");
            var response = ProjectInvoiceAttachmentService.ModifyProjectInvoiceAttachments((int)projectInvoiceAttachmentDomianModels[0].ProjectNumber, new List<DomModel.ProjectInvoiceAttachment> { projectInvoiceAttachmentDomianModels[0] });
            mockProjectinvoiceAttachmentRepository.Verify(m => m.Update(It.IsAny<DbModel.ProjectInvoiceAttachment>()), Times.AtLeastOnce);
        }

        [TestMethod]
        public void DeleteProjectInvoiceAttachment()
        {

            mockProjectRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Project, bool>>>())).Returns(mockProjectDbData);
            mockProjectinvoiceAttachmentRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.ProjectInvoiceAttachment, bool>>>())).Returns((Expression<Func<DbModel.ProjectInvoiceAttachment, bool>> predicate) => mockProjectInvoiceAttachmentDbData.Where(predicate));
          
            Validation = new Evolution.ValidationService.Services.ValidationService();
            validationService = new Evolution.Project.Infrastructure.Validations.InvoiceAttachmentValidationService(Validation);
            ProjectInvoiceAttachmentService = new Service.ProjectInvoiceAttachmentService(mockLogger.Object, mockProjectinvoiceAttachmentRepository.Object, mockProjectRepository.Object, mockDocumenttype.Object, validationService);
            projectInvoiceAttachmentDomianModels.ToList().ForEach(x => x.RecordStatus = "D");
            var response = ProjectInvoiceAttachmentService.DeleteProjectInvoiceAttachments((int)projectInvoiceAttachmentDomianModels[0].ProjectNumber, new List<DomModel.ProjectInvoiceAttachment> { projectInvoiceAttachmentDomianModels[0] });
            mockProjectinvoiceAttachmentRepository.Verify(m => m.Delete(It.IsAny<DbModel.ProjectInvoiceAttachment>()), Times.AtLeastOnce);
        }
        [TestMethod]
        public void ThrowingExceptionWhileSavingProjectInvoiceAttachment()
        {

            mockProjectRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Project, bool>>>())).Throws(new Exception("Exception occured while performing some operation."));
          
            Validation = new Evolution.ValidationService.Services.ValidationService();
            validationService = new Evolution.Project.Infrastructure.Validations.InvoiceAttachmentValidationService(Validation);
            ProjectInvoiceAttachmentService = new Service.ProjectInvoiceAttachmentService(mockLogger.Object, mockProjectinvoiceAttachmentRepository.Object, mockProjectRepository.Object, mockDocumenttype.Object, validationService);
            projectInvoiceAttachmentDomianModels.ToList().ForEach(x => x.RecordStatus = "N");
            var response = ProjectInvoiceAttachmentService.SaveProjectInvoiceAttachments((int)projectInvoiceAttachmentDomianModels[0].ProjectNumber, new List<DomModel.ProjectInvoiceAttachment> { projectInvoiceAttachmentDomianModels[0] });
            Assert.AreEqual("11", response.Code);
            Assert.IsNotNull(response.Messages);
            Assert.AreEqual("31", response.Messages.FirstOrDefault().Code);
        }

        [TestMethod]
        public void ThrowingExceptionWhileProjectUpdateInvoiceAttachment()
        {

            mockProjectRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Project, bool>>>())).Throws(new Exception("Exception occured while performing some operation."));
          
            Validation = new Evolution.ValidationService.Services.ValidationService();
            validationService = new Evolution.Project.Infrastructure.Validations.InvoiceAttachmentValidationService(Validation);
            ProjectInvoiceAttachmentService = new Service.ProjectInvoiceAttachmentService(mockLogger.Object, mockProjectinvoiceAttachmentRepository.Object, mockProjectRepository.Object, mockDocumenttype.Object, validationService);
            projectInvoiceAttachmentDomianModels.ToList().ForEach(x => x.RecordStatus = "M");
            var response = ProjectInvoiceAttachmentService.ModifyProjectInvoiceAttachments((int)projectInvoiceAttachmentDomianModels[0].ProjectNumber, new List<DomModel.ProjectInvoiceAttachment> { projectInvoiceAttachmentDomianModels[0] });
            Assert.AreEqual("11", response.Code);
            Assert.IsNotNull(response.Messages);
            Assert.AreEqual("31", response.Messages.FirstOrDefault().Code);
        }
        [TestMethod]
        public void ThrowingExceptionWhileDeleteProjectInvoiceAttachment()
        {

            mockProjectRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Project, bool>>>())).Throws(new Exception("Exception occured while performing some operation."));
         
            Validation = new Evolution.ValidationService.Services.ValidationService();
            validationService = new Evolution.Project.Infrastructure.Validations.InvoiceAttachmentValidationService(Validation);
            ProjectInvoiceAttachmentService = new Service.ProjectInvoiceAttachmentService (mockLogger.Object, mockProjectinvoiceAttachmentRepository.Object, mockProjectRepository.Object, mockDocumenttype.Object, validationService);
            projectInvoiceAttachmentDomianModels.ToList().ForEach(x => x.RecordStatus = "D");
            var response = ProjectInvoiceAttachmentService.DeleteProjectInvoiceAttachments((int)projectInvoiceAttachmentDomianModels[0].ProjectNumber, new List<DomModel.ProjectInvoiceAttachment> { projectInvoiceAttachmentDomianModels[0] });
            Assert.AreEqual("11", response.Code);
            Assert.IsNotNull(response.Messages);
            Assert.AreEqual("31", response.Messages.FirstOrDefault().Code);
        }
        [TestMethod]
        public void GetAllDataAfterSavingProjectInvoiceAttachment()
        {

            mockProjectRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Project, bool>>>())).Returns(mockProjectDbData);
            mockProjectinvoiceAttachmentRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.ProjectInvoiceAttachment, bool>>>())).Returns((Expression<Func<DbModel.ProjectInvoiceAttachment, bool>> predicate) => mockProjectInvoiceAttachmentDbData.Where(predicate));
            mockDocumenttype.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.ModuleDocumentType, bool>>>())).Returns((Expression<Func<DbModel.ModuleDocumentType, bool>> predicate) => mockModuleDocumentType.Where(predicate));
            Validation = new Evolution.ValidationService.Services.ValidationService();
            validationService = new Evolution.Project.Infrastructure.Validations.InvoiceAttachmentValidationService(Validation);
            ProjectInvoiceAttachmentService = new Service.ProjectInvoiceAttachmentService(mockLogger.Object, mockProjectinvoiceAttachmentRepository.Object, mockProjectRepository.Object, mockDocumenttype.Object, validationService);
            projectInvoiceAttachmentDomianModels.ToList().ForEach(x => x.RecordStatus = "N");
            var response = ProjectInvoiceAttachmentService.SaveProjectInvoiceAttachments((int)projectInvoiceAttachmentDomianModels[0].ProjectNumber, new List<DomModel.ProjectInvoiceAttachment> { projectInvoiceAttachmentDomianModels[0] }, true, true);
            mockProjectinvoiceAttachmentRepository.Verify(m => m.Add(It.IsAny<IList<DbModel.ProjectInvoiceAttachment>>()), Times.AtLeastOnce);
        }
        [TestMethod]
        public void GetAllDataAftereModifingProjectInvoiceAttachment()
        {

            mockProjectRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Project, bool>>>())).Returns(mockProjectDbData);
            mockProjectinvoiceAttachmentRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.ProjectInvoiceAttachment, bool>>>())).Returns((Expression<Func<DbModel.ProjectInvoiceAttachment, bool>> predicate) => mockProjectInvoiceAttachmentDbData.Where(predicate));
            mockDocumenttype.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.ModuleDocumentType, bool>>>())).Returns((Expression<Func<DbModel.ModuleDocumentType, bool>> predicate) => mockModuleDocumentType.Where(predicate));
            Validation = new Evolution.ValidationService.Services.ValidationService();
            validationService = new Evolution.Project.Infrastructure.Validations.InvoiceAttachmentValidationService(Validation);
            ProjectInvoiceAttachmentService = new Service.ProjectInvoiceAttachmentService(mockLogger.Object, mockProjectinvoiceAttachmentRepository.Object, mockProjectRepository.Object, mockDocumenttype.Object, validationService);
            projectInvoiceAttachmentDomianModels.ToList().ForEach(x => x.RecordStatus = "M");
            var response = ProjectInvoiceAttachmentService.ModifyProjectInvoiceAttachments((int)projectInvoiceAttachmentDomianModels[0].ProjectNumber, new List<DomModel.ProjectInvoiceAttachment> { projectInvoiceAttachmentDomianModels[0] }, true, true);
            mockProjectinvoiceAttachmentRepository.Verify(m => m.Update(It.IsAny<DbModel.ProjectInvoiceAttachment>()), Times.AtLeastOnce);
        }

        [TestMethod]
        public void GetAllDataAfterDeletingProjectInvoiceAttachment()
        {

            mockProjectRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Project, bool>>>())).Returns(mockProjectDbData);
            mockProjectinvoiceAttachmentRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.ProjectInvoiceAttachment, bool>>>())).Returns((Expression<Func<DbModel.ProjectInvoiceAttachment, bool>> predicate) => mockProjectInvoiceAttachmentDbData.Where(predicate));
        
            Validation = new Evolution.ValidationService.Services.ValidationService();
            validationService = new Evolution.Project.Infrastructure.Validations.InvoiceAttachmentValidationService(Validation);
            ProjectInvoiceAttachmentService = new Service.ProjectInvoiceAttachmentService(mockLogger.Object, mockProjectinvoiceAttachmentRepository.Object, mockProjectRepository.Object, mockDocumenttype.Object, validationService);
            projectInvoiceAttachmentDomianModels.ToList().ForEach(x => x.RecordStatus = "D");
            var response = ProjectInvoiceAttachmentService.DeleteProjectInvoiceAttachments((int)projectInvoiceAttachmentDomianModels[0].ProjectNumber, new List<DomModel.ProjectInvoiceAttachment> { projectInvoiceAttachmentDomianModels[0] }, true, true);
            mockProjectinvoiceAttachmentRepository.Verify(m => m.Delete(It.IsAny<DbModel.ProjectInvoiceAttachment>()), Times.AtLeastOnce);
        }

        [TestMethod]
        public void ShouldNotSaveProjectInvoiceAttachmentWhenProjectNumberIsNull()
        {

            mockProjectRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Project, bool>>>())).Returns(mockProjectDbData);
         
            Validation = new Evolution.ValidationService.Services.ValidationService();
            validationService = new Evolution.Project.Infrastructure.Validations.InvoiceAttachmentValidationService(Validation);
            ProjectInvoiceAttachmentService = new Service.ProjectInvoiceAttachmentService(mockLogger.Object, mockProjectinvoiceAttachmentRepository.Object, mockProjectRepository.Object, mockDocumenttype.Object, validationService);
            projectInvoiceAttachmentDomianModels.ToList().ForEach(x => x.RecordStatus = "N");
            var response = ProjectInvoiceAttachmentService.SaveProjectInvoiceAttachments(0, new List<DomModel.ProjectInvoiceAttachment> { projectInvoiceAttachmentDomianModels[0] });
            Assert.AreEqual("11", response.Code);

        }

        [TestMethod]
        public void ShouldNotUpdateProjectInvoiceAttachmentWhenProjectNumberIsNull()
        {

            mockProjectRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Project, bool>>>())).Returns(mockProjectDbData);
        
            Validation = new Evolution.ValidationService.Services.ValidationService();
            validationService = new Evolution.Project.Infrastructure.Validations.InvoiceAttachmentValidationService(Validation);
            ProjectInvoiceAttachmentService = new Service.ProjectInvoiceAttachmentService(mockLogger.Object, mockProjectinvoiceAttachmentRepository.Object, mockProjectRepository.Object, mockDocumenttype.Object, validationService);
            projectInvoiceAttachmentDomianModels.ToList().ForEach(x => x.RecordStatus = "M");
            var response = ProjectInvoiceAttachmentService.ModifyProjectInvoiceAttachments(0, new List<DomModel.ProjectInvoiceAttachment> { projectInvoiceAttachmentDomianModels[0] });
            Assert.AreEqual("11", response.Code);

        }
        [TestMethod]
        public void ShouldNotSaveProjectInvoiceAttachmentWithInvalidProjectNumber()
        {

            
            Validation = new Evolution.ValidationService.Services.ValidationService();
            validationService = new Evolution.Project.Infrastructure.Validations.InvoiceAttachmentValidationService(Validation);
            ProjectInvoiceAttachmentService = new Service.ProjectInvoiceAttachmentService(mockLogger.Object, mockProjectinvoiceAttachmentRepository.Object, mockProjectRepository.Object, mockDocumenttype.Object, validationService);
            projectInvoiceAttachmentDomianModels.ToList().ForEach(x => x.RecordStatus = "N");
            var response = ProjectInvoiceAttachmentService.SaveProjectInvoiceAttachments((int)projectInvoiceAttachmentDomianModels[0].ProjectNumber, new List<DomModel.ProjectInvoiceAttachment> { projectInvoiceAttachmentDomianModels[0] });
            Assert.AreEqual("11", response.Code);

        }

        [TestMethod]
        public void ShouldNotUpdateProjectInvoiceAttachmentWithInvalidProject()
        {

           
            Validation = new Evolution.ValidationService.Services.ValidationService();
            validationService = new Evolution.Project.Infrastructure.Validations.InvoiceAttachmentValidationService(Validation);
            ProjectInvoiceAttachmentService = new Service.ProjectInvoiceAttachmentService(mockLogger.Object, mockProjectinvoiceAttachmentRepository.Object, mockProjectRepository.Object, mockDocumenttype.Object, validationService);
            projectInvoiceAttachmentDomianModels.ToList().ForEach(x => x.RecordStatus = "M");
            var response = ProjectInvoiceAttachmentService.ModifyProjectInvoiceAttachments((int)projectInvoiceAttachmentDomianModels[0].ProjectNumber, new List<DomModel.ProjectInvoiceAttachment> { projectInvoiceAttachmentDomianModels[0] });
            Assert.AreEqual("11", response.Code);

        }
        [TestMethod]
        public void ValidationOfDocumentTypeWhileSave()
        {

            mockProjectRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Project, bool>>>())).Returns(mockProjectDbData);
       
            mockDocumenttype.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.ModuleDocumentType, bool>>>())).Returns((Expression<Func<DbModel.ModuleDocumentType, bool>> predicate) => mockModuleDocumentType.Where(predicate));
            Validation = new Evolution.ValidationService.Services.ValidationService();
            validationService = new Evolution.Project.Infrastructure.Validations.InvoiceAttachmentValidationService(Validation);
            ProjectInvoiceAttachmentService = new Service.ProjectInvoiceAttachmentService(mockLogger.Object, mockProjectinvoiceAttachmentRepository.Object, mockProjectRepository.Object, mockDocumenttype.Object, validationService);
            projectInvoiceAttachmentDomianModels.ToList().ForEach(x => x.RecordStatus = "N");
            projectInvoiceAttachmentDomianModels[0].DocumentType = "Contract1";
            var response = ProjectInvoiceAttachmentService.SaveProjectInvoiceAttachments((int)projectInvoiceAttachmentDomianModels[0].ProjectNumber, new List<DomModel.ProjectInvoiceAttachment> { projectInvoiceAttachmentDomianModels[0] });
            Assert.AreEqual("11", response.Code);

        }
        [TestMethod]
        public void ValidationOfDocumentTypeWhileUpdate()
        {

            mockProjectRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Project, bool>>>())).Returns(mockProjectDbData);
        
            mockDocumenttype.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.ModuleDocumentType, bool>>>())).Returns((Expression<Func<DbModel.ModuleDocumentType, bool>> predicate) => mockModuleDocumentType.Where(predicate));
            Validation = new Evolution.ValidationService.Services.ValidationService();
            validationService = new Evolution.Project.Infrastructure.Validations.InvoiceAttachmentValidationService(Validation);
            ProjectInvoiceAttachmentService = new Service.ProjectInvoiceAttachmentService(mockLogger.Object, mockProjectinvoiceAttachmentRepository.Object, mockProjectRepository.Object, mockDocumenttype.Object, validationService);
            projectInvoiceAttachmentDomianModels.ToList().ForEach(x => x.RecordStatus = "M");
            projectInvoiceAttachmentDomianModels[0].DocumentType = "Contract1";
            var response = ProjectInvoiceAttachmentService.ModifyProjectInvoiceAttachments((int)projectInvoiceAttachmentDomianModels[0].ProjectNumber, new List<DomModel.ProjectInvoiceAttachment> { projectInvoiceAttachmentDomianModels[0] });
            Assert.AreEqual("11", response.Code);

        }
        [TestMethod]
        public void ValidationOfDocumentTypeWhileDelete()
        {

            mockProjectRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Project, bool>>>())).Returns(mockProjectDbData);
         
            mockDocumenttype.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.ModuleDocumentType, bool>>>())).Returns((Expression<Func<DbModel.ModuleDocumentType, bool>> predicate) => mockModuleDocumentType.Where(predicate));
            Validation = new Evolution.ValidationService.Services.ValidationService();
            validationService = new Evolution.Project.Infrastructure.Validations.InvoiceAttachmentValidationService(Validation);
            ProjectInvoiceAttachmentService = new Service.ProjectInvoiceAttachmentService(mockLogger.Object, mockProjectinvoiceAttachmentRepository.Object,mockProjectRepository.Object, mockDocumenttype.Object, validationService);
            projectInvoiceAttachmentDomianModels.ToList().ForEach(x => x.RecordStatus = "M");
            projectInvoiceAttachmentDomianModels[0].DocumentType = "Contract1";
            var response = ProjectInvoiceAttachmentService.ModifyProjectInvoiceAttachments((int)projectInvoiceAttachmentDomianModels[0].ProjectNumber, new List<DomModel.ProjectInvoiceAttachment> { projectInvoiceAttachmentDomianModels[0] });
            Assert.AreEqual("11", response.Code);

        }

        [TestMethod]
        public void ValidationOfUpdatingRecordId()
        {

            mockProjectRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Project, bool>>>())).Returns(mockProjectDbData);
            mockProjectinvoiceAttachmentRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.ProjectInvoiceAttachment, bool>>>())).Returns((Expression<Func<DbModel.ProjectInvoiceAttachment, bool>> predicate) => mockProjectInvoiceAttachmentDbData.Where(predicate));
            mockDocumenttype.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.ModuleDocumentType, bool>>>())).Returns((Expression<Func<DbModel.ModuleDocumentType, bool>> predicate) => mockModuleDocumentType.Where(predicate));
            Validation = new Evolution.ValidationService.Services.ValidationService();
            validationService = new Evolution.Project.Infrastructure.Validations.InvoiceAttachmentValidationService(Validation);
            ProjectInvoiceAttachmentService = new Service.ProjectInvoiceAttachmentService(mockLogger.Object, mockProjectinvoiceAttachmentRepository.Object,mockProjectRepository.Object, mockDocumenttype.Object, validationService);
            projectInvoiceAttachmentDomianModels.ToList().ForEach(x => x.RecordStatus = "M");
            projectInvoiceAttachmentDomianModels[0].ProjectInvoiceAttachmentId = 5;
            var response = ProjectInvoiceAttachmentService.ModifyProjectInvoiceAttachments((int)projectInvoiceAttachmentDomianModels[0].ProjectNumber, new List<DomModel.ProjectInvoiceAttachment> { projectInvoiceAttachmentDomianModels[0] });
            Assert.AreEqual("11", response.Code);

        }
        [TestMethod]
        public void ValidationOfDeletingRecordId()
        {

            mockProjectRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Project, bool>>>())).Returns(mockProjectDbData);
            mockProjectinvoiceAttachmentRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.ProjectInvoiceAttachment, bool>>>())).Returns((Expression<Func<DbModel.ProjectInvoiceAttachment, bool>> predicate) => mockProjectInvoiceAttachmentDbData.Where(predicate));
       
            Validation = new Evolution.ValidationService.Services.ValidationService();
            validationService = new Evolution.Project.Infrastructure.Validations.InvoiceAttachmentValidationService(Validation);
            ProjectInvoiceAttachmentService = new Service.ProjectInvoiceAttachmentService(mockLogger.Object, mockProjectinvoiceAttachmentRepository.Object, mockProjectRepository.Object, mockDocumenttype.Object, validationService);
            projectInvoiceAttachmentDomianModels.ToList().ForEach(x => x.RecordStatus = "D");
            projectInvoiceAttachmentDomianModels[0].ProjectInvoiceAttachmentId = 5;
            var response = ProjectInvoiceAttachmentService.DeleteProjectInvoiceAttachments((int)projectInvoiceAttachmentDomianModels[0].ProjectNumber, new List<DomModel.ProjectInvoiceAttachment> { projectInvoiceAttachmentDomianModels[0] });
            Assert.AreEqual("11", response.Code);

        }
        [TestMethod]
        public void ValidationOfupdateCountMissMatch()
        {

            mockProjectRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Project, bool>>>())).Returns(mockProjectDbData);
            mockProjectinvoiceAttachmentRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.ProjectInvoiceAttachment, bool>>>())).Returns((Expression<Func<DbModel.ProjectInvoiceAttachment, bool>> predicate) => mockProjectInvoiceAttachmentDbData.Where(predicate));
            mockDocumenttype.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.ModuleDocumentType, bool>>>())).Returns((Expression<Func<DbModel.ModuleDocumentType, bool>> predicate) => mockModuleDocumentType.Where(predicate));
            Validation = new Evolution.ValidationService.Services.ValidationService();
            validationService = new Evolution.Project.Infrastructure.Validations.InvoiceAttachmentValidationService(Validation);
            ProjectInvoiceAttachmentService = new Service.ProjectInvoiceAttachmentService(mockLogger.Object, mockProjectinvoiceAttachmentRepository.Object,mockProjectRepository.Object, mockDocumenttype.Object, validationService);
            projectInvoiceAttachmentDomianModels.ToList().ForEach(x => x.RecordStatus = "M");
            projectInvoiceAttachmentDomianModels[0].UpdateCount = 5;
            var response = ProjectInvoiceAttachmentService.ModifyProjectInvoiceAttachments((int)projectInvoiceAttachmentDomianModels[0].ProjectNumber, new List<DomModel.ProjectInvoiceAttachment> { projectInvoiceAttachmentDomianModels[0] });
            Assert.AreEqual("11", response.Code);

        }

    }
}
