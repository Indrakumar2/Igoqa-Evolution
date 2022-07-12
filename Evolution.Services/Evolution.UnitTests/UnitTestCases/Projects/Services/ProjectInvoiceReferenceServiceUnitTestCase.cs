using Evolution.UnitTest.UnitTestCases;
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
using System.Linq.Expressions;
using DbRepo = Evolution.DbRepository.Interfaces.Master;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Service = Evolution.Project.Core.Services;
using Evolution.UnitTests.Mocks.Data.Projects.Db;
using ValdService = Evolution.Project.Domain.Interfaces.Validations;
using Evolution.ValidationService.Interfaces;
using Evolution.DbRepository.Interfaces.Master;
using Evolution.UnitTest.Mocks.Data.Masters.Db.Masters;

namespace Evolution.UnitTests.UnitTestCases.Projects.Services
{
    [TestClass]
    public class ProjectInvoiceReferenceServiceUnitTestCase : BaseTestCase
    {

          IValidationService Validation = null;
         ServiceDomainData.IProjectInvoiceReferenceService ProjectInvoiceReferenceService = null;
            Mock<DomainData.IProjectInvoiceReferenceRepository> mockProjectinvoiceReferenceRepository = null;
            Mock<DomainData.IProjectRepository> mockProjectRepository = null;
            Mock<DbRepo.IDataRepository> mockDbRepo = null;
            IList<DomModel.ProjectInvoiceReference> projectInvoiceReferenceTypeDomianModels = null;
            Mock<IAppLogger<Service.ProjectInvoiceReferenceService>> mockLogger = null;
            Mock<IMapper> mockMapper = null;
            IQueryable<DbModel.Project> mockProjectDbData = null;
            IQueryable<DbModel.ProjectInvoiceAssignmentRefence> mockProjectInvoiceReferenceDbData = null;
            IQueryable<DbModel.Data> mockAssignmentType = null;
           ValdService.IInvoiceReferenceValidationService validationService = null;
          


        [TestInitialize]
            public void InitializeContractInvoiceReferencService()
            {
                mockProjectinvoiceReferenceRepository = new Mock<DomainData.IProjectInvoiceReferenceRepository>();
                mockProjectRepository = new Mock<DomainData.IProjectRepository>();
                projectInvoiceReferenceTypeDomianModels = MockProjectDomainModel.GetProjectInvoiceReferencesMockedDomainData();
                mockLogger = new Mock<IAppLogger<Service.ProjectInvoiceReferenceService>>();
                mockMapper = new Mock<IMapper>();
                mockProjectDbData = MockProject.GetprojectMockData();
                mockProjectInvoiceReferenceDbData = MockProject.GetprojectInvoiceAssignmentRefencesMockedData();
                mockDbRepo = new Mock<DbRepo.IDataRepository>();
                mockAssignmentType = MockContract.GetmoduleAssignmentTypeMockData();
              
            }
            [TestMethod]
            public void FetchAllProjectInvoiceReferencWithoutSearchValue()
            {
                mockProjectinvoiceReferenceRepository.Setup(x => x.Search(It.IsAny<DomModel.ProjectInvoiceReference>())).Returns(projectInvoiceReferenceTypeDomianModels);
                ProjectInvoiceReferenceService = new Service.ProjectInvoiceReferenceService( mockLogger.Object, mockProjectinvoiceReferenceRepository.Object, mockProjectRepository.Object, mockDbRepo.Object, validationService);
                var response = ProjectInvoiceReferenceService.GetProjectInvoiceReferences(new DomModel.ProjectInvoiceReference());
                Assert.AreEqual("1", response.Code);
                Assert.AreEqual(2, (response.Result as List<DomModel.ProjectInvoiceReference>).Count);
            }
            [TestMethod]
            public void FetchAllProjectInvoiceReferencWithSearchValue()
            {
                mockProjectinvoiceReferenceRepository.Setup(x => x.Search(It.Is<DomModel.ProjectInvoiceReference>(c => c.ProjectNumber == 1))).Returns(new List<DomModel.ProjectInvoiceReference> { projectInvoiceReferenceTypeDomianModels[0] });
                ProjectInvoiceReferenceService = new Service.ProjectInvoiceReferenceService(mockLogger.Object, mockProjectinvoiceReferenceRepository.Object, mockProjectRepository.Object, mockDbRepo.Object, validationService);
                var response = ProjectInvoiceReferenceService.GetProjectInvoiceReferences(new DomModel.ProjectInvoiceReference { ProjectNumber = 1});


                Assert.AreEqual("1", response.Code);
                Assert.AreEqual(1, (response.Result as List<DomModel.ProjectInvoiceReference>).Count);
            }

            [TestMethod]
            public void FetchProjectInvoiceReferencListWithNullSearchModel()
            {
                mockProjectinvoiceReferenceRepository.Setup(x => x.Search(It.IsAny<DomModel.ProjectInvoiceReference>())).Returns(projectInvoiceReferenceTypeDomianModels);

                ProjectInvoiceReferenceService = new Service.ProjectInvoiceReferenceService(mockLogger.Object, mockProjectinvoiceReferenceRepository.Object, mockProjectRepository.Object, mockDbRepo.Object, validationService);

                var response = ProjectInvoiceReferenceService.GetProjectInvoiceReferences(null);

                Assert.AreEqual("1", response.Code);
                Assert.AreEqual(2, (response.Result as List<DomModel.ProjectInvoiceReference>).Count);
            }
            [TestMethod]
            public void ThrowsExceptionWhileFetchingInvoiceReferenc()
            {
                mockProjectinvoiceReferenceRepository.Setup(x => x.Search(It.IsAny<DomModel.ProjectInvoiceReference>())).Throws(new Exception("Exception occured while performing some operation."));
                ProjectInvoiceReferenceService = new Service.ProjectInvoiceReferenceService(mockLogger.Object, mockProjectinvoiceReferenceRepository.Object, mockProjectRepository.Object, mockDbRepo.Object, validationService);

                var response = ProjectInvoiceReferenceService.GetProjectInvoiceReferences(new DomModel.ProjectInvoiceReference());

                Assert.AreEqual("11", response.Code);
                Assert.IsNotNull(response.Messages);
                Assert.AreEqual("31", response.Messages.FirstOrDefault().Code);
            }
            [TestMethod]
            public void FetchProjectInvoiceReferencListByWildCardSearchWithReftypeStartWith()
            {
                mockProjectinvoiceReferenceRepository.Setup(x => x.Search(It.Is<DomModel.ProjectInvoiceReference>(c => c.ReferenceType.StartsWith("Call")))).Returns(new List<DomModel.ProjectInvoiceReference> { projectInvoiceReferenceTypeDomianModels[0] });

                ProjectInvoiceReferenceService = new Service.ProjectInvoiceReferenceService(mockLogger.Object, mockProjectinvoiceReferenceRepository.Object, mockProjectRepository.Object, mockDbRepo.Object, validationService);

                var response = ProjectInvoiceReferenceService.GetProjectInvoiceReferences(new DomModel.ProjectInvoiceReference { ReferenceType = "Call*" });

                Assert.AreEqual("1", response.Code);
                Assert.AreEqual(1, (response.Result as List<DomModel.ProjectInvoiceReference>).Count);
            }
            [TestMethod]
            public void FetchProjectInvoiceRefListByWildCardSearchWithReftypeEndWith()
            {
                mockProjectinvoiceReferenceRepository.Setup(x => x.Search(It.Is<DomModel.ProjectInvoiceReference>(c => c.ReferenceType.EndsWith("Center")))).Returns(new List<DomModel.ProjectInvoiceReference> { projectInvoiceReferenceTypeDomianModels[1] });

                ProjectInvoiceReferenceService = new Service.ProjectInvoiceReferenceService(mockLogger.Object, mockProjectinvoiceReferenceRepository.Object, mockProjectRepository.Object, mockDbRepo.Object, validationService);

                var response = ProjectInvoiceReferenceService.GetProjectInvoiceReferences(new DomModel.ProjectInvoiceReference { ReferenceType = "*Center" });

                Assert.AreEqual("1", response.Code);
                Assert.AreEqual(1, (response.Result as List<DomModel.ProjectInvoiceReference>).Count);
            }

            [TestMethod]
            public void FetchProjectInvoiceReferencListByWildCardSearchWithReftypeContains()
            {
                mockProjectinvoiceReferenceRepository.Setup(x => x.Search(It.Is<DomModel.ProjectInvoiceReference>(c => c.ReferenceType.Contains("Cost")))).Returns(new List<DomModel.ProjectInvoiceReference> { projectInvoiceReferenceTypeDomianModels[1] });

                ProjectInvoiceReferenceService = new Service.ProjectInvoiceReferenceService(mockLogger.Object, mockProjectinvoiceReferenceRepository.Object, mockProjectRepository.Object, mockDbRepo.Object, validationService);

                var response = ProjectInvoiceReferenceService.GetProjectInvoiceReferences(new DomModel.ProjectInvoiceReference { ReferenceType = "*Cost*" });

                Assert.AreEqual("1", response.Code);
                Assert.AreEqual(1, (response.Result as List<DomModel.ProjectInvoiceReference>).Count);
            }

            [TestMethod]
            public void SaveProjectInvoiceRefType()
            {

                mockProjectRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Project, bool>>>())).Returns(mockProjectDbData);
                mockProjectinvoiceReferenceRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.ProjectInvoiceAssignmentRefence, bool>>>())).Returns((Expression<Func<DbModel.ProjectInvoiceAssignmentRefence, bool>> predicate) => mockProjectInvoiceReferenceDbData.Where(predicate));
                mockDbRepo.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Data, bool>>>())).Returns((Expression<Func<DbModel.Data, bool>> predicate) => mockAssignmentType.Where(predicate));
                 Validation = new Evolution.ValidationService.Services.ValidationService();
                 validationService = new Evolution.Project.Infrastructure.Validations.InvoiceReferenceValidationService(Validation);
                 ProjectInvoiceReferenceService = new Service.ProjectInvoiceReferenceService(mockLogger.Object, mockProjectinvoiceReferenceRepository.Object, mockProjectRepository.Object, mockDbRepo.Object, validationService);
                projectInvoiceReferenceTypeDomianModels.ToList().ForEach(x => x.RecordStatus = "N");
                var response = ProjectInvoiceReferenceService.SaveProjectInvoiceReferences((int)projectInvoiceReferenceTypeDomianModels[0].ProjectNumber, new List<DomModel.ProjectInvoiceReference> { projectInvoiceReferenceTypeDomianModels[0] });
                mockProjectinvoiceReferenceRepository.Verify(m => m.Add(It.IsAny<IList<DbModel.ProjectInvoiceAssignmentRefence>>()), Times.AtLeastOnce);
            }
            [TestMethod]
            public void ModifyProjectInvoiceRefType()
            {

                mockProjectRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Project, bool>>>())).Returns(mockProjectDbData);
                mockProjectinvoiceReferenceRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.ProjectInvoiceAssignmentRefence, bool>>>())).Returns((Expression<Func<DbModel.ProjectInvoiceAssignmentRefence, bool>> predicate) => mockProjectInvoiceReferenceDbData.Where(predicate));
                mockDbRepo.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Data, bool>>>())).Returns((Expression<Func<DbModel.Data, bool>> predicate) => mockAssignmentType.Where(predicate));
               Validation = new Evolution.ValidationService.Services.ValidationService();
               validationService = new Evolution.Project.Infrastructure.Validations.InvoiceReferenceValidationService(Validation);
                 ProjectInvoiceReferenceService = new Service.ProjectInvoiceReferenceService(mockLogger.Object, mockProjectinvoiceReferenceRepository.Object, mockProjectRepository.Object, mockDbRepo.Object, validationService);
                projectInvoiceReferenceTypeDomianModels.ToList().ForEach(x => x.RecordStatus = "M");
                var response = ProjectInvoiceReferenceService.ModifyProjectInvoiceReferences((int)projectInvoiceReferenceTypeDomianModels[0].ProjectNumber, new List<DomModel.ProjectInvoiceReference> { projectInvoiceReferenceTypeDomianModels[0] });
                mockProjectinvoiceReferenceRepository.Verify(m => m.Update(It.IsAny<DbModel.ProjectInvoiceAssignmentRefence>()), Times.AtLeastOnce);
            }

            [TestMethod]
            public void DeleteProjectInvoiceRefType()
            {

                mockProjectRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Project, bool>>>())).Returns(mockProjectDbData);
                mockProjectinvoiceReferenceRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.ProjectInvoiceAssignmentRefence, bool>>>())).Returns((Expression<Func<DbModel.ProjectInvoiceAssignmentRefence, bool>> predicate) => mockProjectInvoiceReferenceDbData.Where(predicate));
               
            Validation = new Evolution.ValidationService.Services.ValidationService();
            validationService = new Evolution.Project.Infrastructure.Validations.InvoiceReferenceValidationService(Validation);

            ProjectInvoiceReferenceService = new Service.ProjectInvoiceReferenceService(mockLogger.Object, mockProjectinvoiceReferenceRepository.Object, mockProjectRepository.Object, mockDbRepo.Object, validationService);
                projectInvoiceReferenceTypeDomianModels.ToList().ForEach(x => x.RecordStatus = "D");
                var response = ProjectInvoiceReferenceService.DeleteProjectInvoiceReferences((int)projectInvoiceReferenceTypeDomianModels[0].ProjectNumber, new List<DomModel.ProjectInvoiceReference> { projectInvoiceReferenceTypeDomianModels[0] });
                mockProjectinvoiceReferenceRepository.Verify(m => m.Delete(It.IsAny<DbModel.ProjectInvoiceAssignmentRefence>()), Times.AtLeastOnce);
            }

            [TestMethod]
            public void ThrowExceptionWhileSavingProjectInvoiceRefType()
            {

                mockProjectRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Project, bool>>>())).Throws(new Exception("Exception occured while performing some operation."));
               
                Validation = new Evolution.ValidationService.Services.ValidationService();
                validationService = new Evolution.Project.Infrastructure.Validations.InvoiceReferenceValidationService(Validation);
                  ProjectInvoiceReferenceService = new Service.ProjectInvoiceReferenceService(mockLogger.Object, mockProjectinvoiceReferenceRepository.Object, mockProjectRepository.Object, mockDbRepo.Object, validationService);
                projectInvoiceReferenceTypeDomianModels.ToList().ForEach(x => x.RecordStatus = "N");
                var response = ProjectInvoiceReferenceService.SaveProjectInvoiceReferences((int)projectInvoiceReferenceTypeDomianModels[0].ProjectNumber, new List<DomModel.ProjectInvoiceReference> { projectInvoiceReferenceTypeDomianModels[0] });
                Assert.AreEqual("11", response.Code);
                Assert.IsNotNull(response.Messages);
                Assert.AreEqual("31", response.Messages.FirstOrDefault().Code);
            }
            [TestMethod]
            public void ThrowExceptionWhileUpdatingProjectInvoiceRefType()
            {

                mockProjectRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Project, bool>>>())).Throws(new Exception("Exception occured while performing some operation."));
               
               Validation = new Evolution.ValidationService.Services.ValidationService();
               validationService = new Evolution.Project.Infrastructure.Validations.InvoiceReferenceValidationService(Validation);
               ProjectInvoiceReferenceService = new Service.ProjectInvoiceReferenceService(mockLogger.Object, mockProjectinvoiceReferenceRepository.Object, mockProjectRepository.Object, mockDbRepo.Object, validationService);
                projectInvoiceReferenceTypeDomianModels.ToList().ForEach(x => x.RecordStatus = "M");
                var response = ProjectInvoiceReferenceService.ModifyProjectInvoiceReferences((int)projectInvoiceReferenceTypeDomianModels[0].ProjectNumber, new List<DomModel.ProjectInvoiceReference> { projectInvoiceReferenceTypeDomianModels[0] });
                Assert.AreEqual("11", response.Code);
                Assert.IsNotNull(response.Messages);
                Assert.AreEqual("31", response.Messages.FirstOrDefault().Code);
            }

            [TestMethod]
            public void ThrowExceptionWhileDeletingProjectInvoiceRefType()
            {

                mockProjectRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Project, bool>>>())).Throws(new Exception("Exception occured while performing some operation."));
              
                Validation = new Evolution.ValidationService.Services.ValidationService();
               validationService = new Evolution.Project.Infrastructure.Validations.InvoiceReferenceValidationService(Validation);
               ProjectInvoiceReferenceService = new Service.ProjectInvoiceReferenceService(mockLogger.Object, mockProjectinvoiceReferenceRepository.Object, mockProjectRepository.Object, mockDbRepo.Object, validationService);
                projectInvoiceReferenceTypeDomianModels.ToList().ForEach(x => x.RecordStatus = "D");
                var response = ProjectInvoiceReferenceService.DeleteProjectInvoiceReferences((int)projectInvoiceReferenceTypeDomianModels[0].ProjectNumber, new List<DomModel.ProjectInvoiceReference> { projectInvoiceReferenceTypeDomianModels[0] });
                Assert.AreEqual("11", response.Code);
                Assert.IsNotNull(response.Messages);
                Assert.AreEqual("31", response.Messages.FirstOrDefault().Code);
            }
            [TestMethod]
            public void GetAllDataAfterSavingInvoiceRef()
            {

                mockProjectRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Project, bool>>>())).Returns(mockProjectDbData);
                mockProjectinvoiceReferenceRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.ProjectInvoiceAssignmentRefence, bool>>>())).Returns((Expression<Func<DbModel.ProjectInvoiceAssignmentRefence, bool>> predicate) => mockProjectInvoiceReferenceDbData.Where(predicate));
                mockDbRepo.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Data, bool>>>())).Returns((Expression<Func<DbModel.Data, bool>> predicate) => mockAssignmentType.Where(predicate));
                Validation = new Evolution.ValidationService.Services.ValidationService();
              validationService = new Evolution.Project.Infrastructure.Validations.InvoiceReferenceValidationService(Validation);
              ProjectInvoiceReferenceService = new Service.ProjectInvoiceReferenceService(mockLogger.Object, mockProjectinvoiceReferenceRepository.Object, mockProjectRepository.Object, mockDbRepo.Object, validationService);
                projectInvoiceReferenceTypeDomianModels.ToList().ForEach(x => x.RecordStatus = "N");
                var response = ProjectInvoiceReferenceService.SaveProjectInvoiceReferences((int)projectInvoiceReferenceTypeDomianModels[0].ProjectNumber, new List<DomModel.ProjectInvoiceReference> { projectInvoiceReferenceTypeDomianModels[0] });
                mockProjectinvoiceReferenceRepository.Verify(m => m.Add(It.IsAny<IList<DbModel.ProjectInvoiceAssignmentRefence>>()), Times.AtLeastOnce);
            }
            [TestMethod]
            public void GetAllDataAfterUpdatingInvoiceRef()
            {

                mockProjectRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Project, bool>>>())).Returns(mockProjectDbData);
                mockProjectinvoiceReferenceRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.ProjectInvoiceAssignmentRefence, bool>>>())).Returns((Expression<Func<DbModel.ProjectInvoiceAssignmentRefence, bool>> predicate) => mockProjectInvoiceReferenceDbData.Where(predicate));
                mockDbRepo.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Data, bool>>>())).Returns((Expression<Func<DbModel.Data, bool>> predicate) => mockAssignmentType.Where(predicate));
              Validation = new Evolution.ValidationService.Services.ValidationService();
               validationService = new Evolution.Project.Infrastructure.Validations.InvoiceReferenceValidationService(Validation);
               ProjectInvoiceReferenceService = new Service.ProjectInvoiceReferenceService(mockLogger.Object, mockProjectinvoiceReferenceRepository.Object, mockProjectRepository.Object, mockDbRepo.Object, validationService);
                projectInvoiceReferenceTypeDomianModels.ToList().ForEach(x => x.RecordStatus = "M");
                var response = ProjectInvoiceReferenceService.ModifyProjectInvoiceReferences((int)projectInvoiceReferenceTypeDomianModels[0].ProjectNumber, new List<DomModel.ProjectInvoiceReference> { projectInvoiceReferenceTypeDomianModels[0] }, true, true);
                mockProjectinvoiceReferenceRepository.Verify(m => m.Update(It.IsAny<DbModel.ProjectInvoiceAssignmentRefence>()), Times.AtLeastOnce);
            }

            [TestMethod]
            public void GetAllDataAfterDeletingInvoiceRef()
            {

                mockProjectRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Project, bool>>>())).Returns(mockProjectDbData);
                mockProjectinvoiceReferenceRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.ProjectInvoiceAssignmentRefence, bool>>>())).Returns((Expression<Func<DbModel.ProjectInvoiceAssignmentRefence, bool>> predicate) => mockProjectInvoiceReferenceDbData.Where(predicate));
              
               Validation = new Evolution.ValidationService.Services.ValidationService();
               validationService = new Evolution.Project.Infrastructure.Validations.InvoiceReferenceValidationService(Validation);
               ProjectInvoiceReferenceService = new Service.ProjectInvoiceReferenceService(mockLogger.Object, mockProjectinvoiceReferenceRepository.Object, mockProjectRepository.Object, mockDbRepo.Object, validationService);
                projectInvoiceReferenceTypeDomianModels.ToList().ForEach(x => x.RecordStatus = "D");
                var response = ProjectInvoiceReferenceService.DeleteProjectInvoiceReferences((int)projectInvoiceReferenceTypeDomianModels[0].ProjectNumber, new List<DomModel.ProjectInvoiceReference> { projectInvoiceReferenceTypeDomianModels[0] }, true, true);
                mockProjectinvoiceReferenceRepository.Verify(m => m.Delete(It.IsAny<DbModel.ProjectInvoiceAssignmentRefence>()), Times.AtLeastOnce);
            }
            [TestMethod]
            public void ShouldNotSaveDataWhenProjectNumberIsNull()
            {

                mockProjectRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Project, bool>>>())).Returns(mockProjectDbData);
               
               Validation = new Evolution.ValidationService.Services.ValidationService();
               validationService = new Evolution.Project.Infrastructure.Validations.InvoiceReferenceValidationService(Validation);
                ProjectInvoiceReferenceService = new Service.ProjectInvoiceReferenceService(mockLogger.Object, mockProjectinvoiceReferenceRepository.Object, mockProjectRepository.Object, mockDbRepo.Object, validationService);
                projectInvoiceReferenceTypeDomianModels.ToList().ForEach(x => x.RecordStatus = "N");
                var response = ProjectInvoiceReferenceService.SaveProjectInvoiceReferences(0, new List<DomModel.ProjectInvoiceReference> { projectInvoiceReferenceTypeDomianModels[0] });
                Assert.AreEqual("11", response.Code);
            }
            [TestMethod]
            public void ShouldNotModifyDataWhenProjectNumberIsNull()
            {

                mockProjectRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Project, bool>>>())).Returns(mockProjectDbData);
              
                Validation = new Evolution.ValidationService.Services.ValidationService();
                validationService = new Evolution.Project.Infrastructure.Validations.InvoiceReferenceValidationService(Validation);
                ProjectInvoiceReferenceService = new Service.ProjectInvoiceReferenceService(mockLogger.Object, mockProjectinvoiceReferenceRepository.Object, mockProjectRepository.Object, mockDbRepo.Object, validationService);
                projectInvoiceReferenceTypeDomianModels.ToList().ForEach(x => x.RecordStatus = "M");
                var response = ProjectInvoiceReferenceService.ModifyProjectInvoiceReferences(0, new List<DomModel.ProjectInvoiceReference> { projectInvoiceReferenceTypeDomianModels[0] });
                Assert.AreEqual("11", response.Code);
            }
            [TestMethod]
            public void ShouldNotSaveDataWithProjectNumberIsInvalid()
            {

            Validation = new Evolution.ValidationService.Services.ValidationService();
            validationService = new Evolution.Project.Infrastructure.Validations.InvoiceReferenceValidationService(Validation);
            ProjectInvoiceReferenceService = new Service.ProjectInvoiceReferenceService(mockLogger.Object, mockProjectinvoiceReferenceRepository.Object, mockProjectRepository.Object, mockDbRepo.Object, validationService);
                projectInvoiceReferenceTypeDomianModels.ToList().ForEach(x => x.RecordStatus = "N");
                var response = ProjectInvoiceReferenceService.SaveProjectInvoiceReferences(0, new List<DomModel.ProjectInvoiceReference> { projectInvoiceReferenceTypeDomianModels[0] });
                Assert.AreEqual("11", response.Code);
            }

            [TestMethod]
            public void ShouldNotModifyDataWhenProjectNumberIsInvalid()
            {

            Validation = new Evolution.ValidationService.Services.ValidationService();
            validationService = new Evolution.Project.Infrastructure.Validations.InvoiceReferenceValidationService(Validation);
            ProjectInvoiceReferenceService = new Service.ProjectInvoiceReferenceService(mockLogger.Object, mockProjectinvoiceReferenceRepository.Object, mockProjectRepository.Object, mockDbRepo.Object, validationService);
                projectInvoiceReferenceTypeDomianModels.ToList().ForEach(x => x.RecordStatus = "M");
                var response = ProjectInvoiceReferenceService.ModifyProjectInvoiceReferences((int)projectInvoiceReferenceTypeDomianModels[0].ProjectNumber, new List<DomModel.ProjectInvoiceReference> { projectInvoiceReferenceTypeDomianModels[0] });
                Assert.AreEqual("11", response.Code);
            }
            [TestMethod]
            public void ValidationOfAssignmentRefWhileSave()
            {

                mockProjectRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Project, bool>>>())).Returns(mockProjectDbData);
             
                mockDbRepo.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Data, bool>>>())).Returns((Expression<Func<DbModel.Data, bool>> predicate) => mockAssignmentType.Where(predicate));
            Validation = new Evolution.ValidationService.Services.ValidationService();
            validationService = new Evolution.Project.Infrastructure.Validations.InvoiceReferenceValidationService(Validation);
            ProjectInvoiceReferenceService = new Service.ProjectInvoiceReferenceService(mockLogger.Object, mockProjectinvoiceReferenceRepository.Object, mockProjectRepository.Object, mockDbRepo.Object, validationService);
                projectInvoiceReferenceTypeDomianModels.ToList().ForEach(x => x.RecordStatus = "N");
                projectInvoiceReferenceTypeDomianModels[0].ReferenceType = "Invalid";
                var response = ProjectInvoiceReferenceService.SaveProjectInvoiceReferences((int)projectInvoiceReferenceTypeDomianModels[0].ProjectNumber, new List<DomModel.ProjectInvoiceReference> { projectInvoiceReferenceTypeDomianModels[0] });
                Assert.AreEqual("11", response.Code);
            }

            [TestMethod]
            public void ValidationOfAssignmentRefWhileUpdate()
            {

                mockProjectRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Project, bool>>>())).Returns(mockProjectDbData);
          
                mockDbRepo.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Data, bool>>>())).Returns((Expression<Func<DbModel.Data, bool>> predicate) => mockAssignmentType.Where(predicate));
            Validation = new Evolution.ValidationService.Services.ValidationService();
            validationService = new Evolution.Project.Infrastructure.Validations.InvoiceReferenceValidationService(Validation);
            ProjectInvoiceReferenceService = new Service.ProjectInvoiceReferenceService(mockLogger.Object, mockProjectinvoiceReferenceRepository.Object, mockProjectRepository.Object, mockDbRepo.Object, validationService);
                projectInvoiceReferenceTypeDomianModels.ToList().ForEach(x => x.RecordStatus = "M");
                projectInvoiceReferenceTypeDomianModels[0].ReferenceType = "invalid";
                var response = ProjectInvoiceReferenceService.ModifyProjectInvoiceReferences((int)projectInvoiceReferenceTypeDomianModels[0].ProjectNumber, new List<DomModel.ProjectInvoiceReference> { projectInvoiceReferenceTypeDomianModels[0] });
                Assert.AreEqual("11", response.Code);
            }
            [TestMethod]
            public void ValidationOfUpdateCount()
            {

                mockProjectRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Project, bool>>>())).Returns(mockProjectDbData);
                mockProjectinvoiceReferenceRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.ProjectInvoiceAssignmentRefence, bool>>>())).Returns((Expression<Func<DbModel.ProjectInvoiceAssignmentRefence, bool>> predicate) => mockProjectInvoiceReferenceDbData.Where(predicate));
                mockDbRepo.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Data, bool>>>())).Returns((Expression<Func<DbModel.Data, bool>> predicate) => mockAssignmentType.Where(predicate));
                 Validation = new Evolution.ValidationService.Services.ValidationService();
                validationService = new Evolution.Project.Infrastructure.Validations.InvoiceReferenceValidationService(Validation);
                 ProjectInvoiceReferenceService = new Service.ProjectInvoiceReferenceService(mockLogger.Object, mockProjectinvoiceReferenceRepository.Object, mockProjectRepository.Object, mockDbRepo.Object, validationService);
                projectInvoiceReferenceTypeDomianModels.ToList().ForEach(x => x.RecordStatus = "M");
                projectInvoiceReferenceTypeDomianModels[0].UpdateCount = 5;
                var response = ProjectInvoiceReferenceService.ModifyProjectInvoiceReferences((int)projectInvoiceReferenceTypeDomianModels[0].ProjectNumber, new List<DomModel.ProjectInvoiceReference> { projectInvoiceReferenceTypeDomianModels[0] });
                Assert.AreEqual("11", response.Code);
            }
            [TestMethod]
            public void ValidationOfExsistanceOfInvoiceRef()
            {

                mockProjectRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Project, bool>>>())).Returns(mockProjectDbData);
              
                mockDbRepo.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Data, bool>>>())).Returns((Expression<Func<DbModel.Data, bool>> predicate) => mockAssignmentType.Where(predicate));
                Validation = new Evolution.ValidationService.Services.ValidationService();
                validationService = new Evolution.Project.Infrastructure.Validations.InvoiceReferenceValidationService(Validation);  
                 ProjectInvoiceReferenceService = new Service.ProjectInvoiceReferenceService(mockLogger.Object, mockProjectinvoiceReferenceRepository.Object, mockProjectRepository.Object, mockDbRepo.Object, validationService);
                projectInvoiceReferenceTypeDomianModels.ToList().ForEach(x => x.RecordStatus = "M");
                projectInvoiceReferenceTypeDomianModels[0].ReferenceType = "ABC";
                var response = ProjectInvoiceReferenceService.ModifyProjectInvoiceReferences((int)projectInvoiceReferenceTypeDomianModels[0].ProjectNumber, new List<DomModel.ProjectInvoiceReference> { projectInvoiceReferenceTypeDomianModels[0] });
                Assert.AreEqual("11", response.Code);
            }

            [TestMethod]
            public void ISRecordIdIsValidForUpdate()
            {

                mockProjectRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Project, bool>>>())).Returns(mockProjectDbData);
                mockProjectinvoiceReferenceRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.ProjectInvoiceAssignmentRefence, bool>>>())).Returns((Expression<Func<DbModel.ProjectInvoiceAssignmentRefence, bool>> predicate) => mockProjectInvoiceReferenceDbData.Where(predicate));
                mockDbRepo.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Data, bool>>>())).Returns((Expression<Func<DbModel.Data, bool>> predicate) => mockAssignmentType.Where(predicate));
                Validation = new Evolution.ValidationService.Services.ValidationService();
               validationService = new Evolution.Project.Infrastructure.Validations.InvoiceReferenceValidationService(Validation);
              ProjectInvoiceReferenceService = new Service.ProjectInvoiceReferenceService(mockLogger.Object, mockProjectinvoiceReferenceRepository.Object, mockProjectRepository.Object, mockDbRepo.Object, validationService);
                projectInvoiceReferenceTypeDomianModels.ToList().ForEach(x => x.RecordStatus = "M");
                projectInvoiceReferenceTypeDomianModels[0].ProjectInvoiceReferenceTypeId = 5;
                var response = ProjectInvoiceReferenceService.ModifyProjectInvoiceReferences((int)projectInvoiceReferenceTypeDomianModels[0].ProjectNumber, new List<DomModel.ProjectInvoiceReference> { projectInvoiceReferenceTypeDomianModels[0] });
                Assert.AreEqual("11", response.Code);




            }
            [TestMethod]
            public void ISRecordIdIsValidForDelete()
            {

                mockProjectRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Project, bool>>>())).Returns(mockProjectDbData);
                mockProjectinvoiceReferenceRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.ProjectInvoiceAssignmentRefence, bool>>>())).Returns((Expression<Func<DbModel.ProjectInvoiceAssignmentRefence, bool>> predicate) => mockProjectInvoiceReferenceDbData.Where(predicate));
             
                Validation = new Evolution.ValidationService.Services.ValidationService();
               validationService = new Evolution.Project.Infrastructure.Validations.InvoiceReferenceValidationService(Validation);
               ProjectInvoiceReferenceService = new Service.ProjectInvoiceReferenceService(mockLogger.Object, mockProjectinvoiceReferenceRepository.Object, mockProjectRepository.Object, mockDbRepo.Object, validationService);
                projectInvoiceReferenceTypeDomianModels.ToList().ForEach(x => x.RecordStatus = "D");
                projectInvoiceReferenceTypeDomianModels[0].ProjectInvoiceReferenceTypeId = 5;
                var response = ProjectInvoiceReferenceService.DeleteProjectInvoiceReferences((int)projectInvoiceReferenceTypeDomianModels[0].ProjectNumber, new List<DomModel.ProjectInvoiceReference> { projectInvoiceReferenceTypeDomianModels[0] });
                Assert.AreEqual("11", response.Code);




            }

        
    }
}
