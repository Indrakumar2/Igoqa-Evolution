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
using CustomerContactRepo = Evolution.Customer.Domain.Interfaces.Data;
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

namespace Evolution.UnitTests.UnitTestCases.Projects.Services
{
    [TestClass]
    public class ProjectClientNotificationServiceUnitTestCase:BaseTestCase
    {
        IValidationService Validation = null;
        ServiceDomainData.IProjectClientNotificationService ProjectClientNotificationService = null;
        Mock<DomainData.IProjectClientNotificationRepository> mockProjectClientNotificationRepository = null;
        Mock<DomainData.IProjectRepository> mockProjectRepository = null;
        Mock<CustomerContactRepo.ICustomerContactRepository> mockCustomerContactRepo = null;
        IList<DomModel.ProjectClientNotification> projectClientNotificationeDomianModels = null;
        Mock<IAppLogger<Service.ProjectClientNotificationService>> mockLogger = null;
        Mock<IMapper> mockMapper = null;
        IQueryable<DbModel.Project> mockProjectDbData = null;
        IQueryable<DbModel.ProjectClientNotification> mockProjectClientNotificationDbData = null;
        IQueryable<DbModel.CustomerContact> mockCustomerContacts = null;
        ValdService.IProjectClientNotificationValidationService validationService = null;

        [TestInitialize]
        public void IntializeProjectClientNotification()
        {
            mockProjectClientNotificationRepository = new Mock<DomainData.IProjectClientNotificationRepository>();
            mockProjectRepository = new Mock<DomainData.IProjectRepository>();
            mockCustomerContactRepo = new Mock<CustomerContactRepo.ICustomerContactRepository>();
            projectClientNotificationeDomianModels = MockProjectDomainModel.GetProjectClientNotificationsMockeDomainData();
            mockLogger = new Mock<IAppLogger<Service.ProjectClientNotificationService>>();
            mockMapper = new Mock<IMapper>();
            mockProjectDbData = MockProject.GetprojectMockData();
            mockProjectClientNotificationDbData = MockProject.GetProjectClientNotificationsMockedData();
            mockCustomerContacts = MockProject.GetCustomerContactMockData();
        }

        [TestMethod]
        
        public void FetchAllProjectClientNotifiactionWithoutSearchValue()
        {
            mockProjectClientNotificationRepository.Setup(x => x.Search(It.IsAny<DomModel.ProjectClientNotification>())).Returns(projectClientNotificationeDomianModels);
            ProjectClientNotificationService = new Service.ProjectClientNotificationService(mockLogger.Object, mockProjectRepository.Object, mockProjectClientNotificationRepository.Object, mockCustomerContactRepo.Object, validationService);
            var response = ProjectClientNotificationService.GetProjectClientNotifications(new DomModel.ProjectClientNotification());
            Assert.AreEqual("1", response.Code);
            Assert.AreEqual(2, (response.Result as List<DomModel.ProjectClientNotification>).Count);
        }

        [TestMethod]
        public void FetchAllProjectClientNotificationWithSearchValue()
        {
            mockProjectClientNotificationRepository.Setup(x => x.Search(It.Is<DomModel.ProjectClientNotification>(c => c.ProjectNumber == 1))).Returns(new List<DomModel.ProjectClientNotification> { projectClientNotificationeDomianModels[0] });
            ProjectClientNotificationService = new Service.ProjectClientNotificationService(mockLogger.Object, mockProjectRepository.Object, mockProjectClientNotificationRepository.Object, mockCustomerContactRepo.Object, validationService);
            var response = ProjectClientNotificationService.GetProjectClientNotifications(new DomModel.ProjectClientNotification { ProjectNumber = 1 });


            Assert.AreEqual("1", response.Code);
            Assert.AreEqual(1, (response.Result as List<DomModel.ProjectClientNotification>).Count);
        }

        [TestMethod]
        public void FetchProjectClientNotificationListWithNullSearchModel()
        {
            mockProjectClientNotificationRepository.Setup(x => x.Search(It.IsAny<DomModel.ProjectClientNotification>())).Returns(projectClientNotificationeDomianModels);

            ProjectClientNotificationService = new Service.ProjectClientNotificationService(mockLogger.Object, mockProjectRepository.Object, mockProjectClientNotificationRepository.Object, mockCustomerContactRepo.Object, validationService);

            var response = ProjectClientNotificationService.GetProjectClientNotifications(null);

            Assert.AreEqual("1", response.Code);
            Assert.AreEqual(2, (response.Result as List<DomModel.ProjectClientNotification>).Count);
        }
        [TestMethod]
        public void ThrowsExceptionWhileFetchingProjectClientNotification()
        {
            mockProjectClientNotificationRepository.Setup(x => x.Search(It.IsAny<DomModel.ProjectClientNotification>())).Throws(new Exception("Exception occured while performing some operation."));
            ProjectClientNotificationService = new Service.ProjectClientNotificationService(mockLogger.Object, mockProjectRepository.Object, mockProjectClientNotificationRepository.Object, mockCustomerContactRepo.Object, validationService);

            var response = ProjectClientNotificationService.GetProjectClientNotifications(new DomModel.ProjectClientNotification());

            Assert.AreEqual("11", response.Code);
            Assert.IsNotNull(response.Messages);
            Assert.AreEqual("31", response.Messages.FirstOrDefault().Code);
        }


        [TestMethod]
        public void SaveProjectClientNotification()
        {

            mockProjectRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Project, bool>>>())).Returns(mockProjectDbData);
            mockProjectClientNotificationRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.ProjectClientNotification, bool>>>())).Returns((Expression<Func<DbModel.ProjectClientNotification, bool>> predicate) => mockProjectClientNotificationDbData.Where(predicate));
            mockCustomerContactRepo.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.CustomerContact, bool>>>())).Returns((Expression<Func<DbModel.CustomerContact, bool>> predicate) => mockCustomerContacts.Where(predicate));
            Validation = new Evolution.ValidationService.Services.ValidationService();
            validationService = new Evolution.Project.Infrastructure.Validations.ProjectClientNotificationValidationService(Validation);
            ProjectClientNotificationService = new Service.ProjectClientNotificationService(mockLogger.Object, mockProjectRepository.Object, mockProjectClientNotificationRepository.Object, mockCustomerContactRepo.Object, validationService);
            projectClientNotificationeDomianModels.ToList().ForEach(x => x.RecordStatus = "N");
            var response = ProjectClientNotificationService.SaveProjectClientNotifications((int)projectClientNotificationeDomianModels[0].ProjectNumber, new List<DomModel.ProjectClientNotification> { projectClientNotificationeDomianModels[0] });
            mockProjectClientNotificationRepository.Verify(m => m.Add(It.IsAny<IList<DbModel.ProjectClientNotification>>()), Times.AtLeastOnce);
        }
        [TestMethod]
        public void ModifyProjectClientNotification()
        {

            mockProjectRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Project, bool>>>())).Returns(mockProjectDbData);
            mockProjectClientNotificationRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.ProjectClientNotification, bool>>>())).Returns((Expression<Func<DbModel.ProjectClientNotification, bool>> predicate) => mockProjectClientNotificationDbData.Where(predicate));
            mockCustomerContactRepo.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.CustomerContact, bool>>>())).Returns((Expression<Func<DbModel.CustomerContact, bool>> predicate) => mockCustomerContacts.Where(predicate));
            Validation = new Evolution.ValidationService.Services.ValidationService();
            validationService = new Evolution.Project.Infrastructure.Validations.ProjectClientNotificationValidationService(Validation);
            ProjectClientNotificationService = new Service.ProjectClientNotificationService(mockLogger.Object, mockProjectRepository.Object, mockProjectClientNotificationRepository.Object, mockCustomerContactRepo.Object, validationService);
            projectClientNotificationeDomianModels.ToList().ForEach(x => x.RecordStatus = "M");
            var response = ProjectClientNotificationService.ModifyProjectClientNotifications((int)projectClientNotificationeDomianModels[0].ProjectNumber, new List<DomModel.ProjectClientNotification> { projectClientNotificationeDomianModels[0] });
            mockProjectClientNotificationRepository.Verify(m => m.Update(It.IsAny<DbModel.ProjectClientNotification>()), Times.AtLeastOnce);
        }

        [TestMethod]
        public void DeleteProjectClientNotification()
        {

            mockProjectRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Project, bool>>>())).Returns(mockProjectDbData);
            mockProjectClientNotificationRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.ProjectClientNotification, bool>>>())).Returns((Expression<Func<DbModel.ProjectClientNotification, bool>> predicate) => mockProjectClientNotificationDbData.Where(predicate));
           // mockCustomerContactRepo.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.CustomerContact, bool>>>())).Returns((Expression<Func<DbModel.CustomerContact, bool>> predicate) => mockCustomerContacts.Where(predicate));
            Validation = new Evolution.ValidationService.Services.ValidationService();
            validationService = new Evolution.Project.Infrastructure.Validations.ProjectClientNotificationValidationService(Validation);
            ProjectClientNotificationService = new Service.ProjectClientNotificationService(mockLogger.Object, mockProjectRepository.Object, mockProjectClientNotificationRepository.Object, mockCustomerContactRepo.Object, validationService);
            projectClientNotificationeDomianModels.ToList().ForEach(x => x.RecordStatus = "D");
            var response = ProjectClientNotificationService.DeleteProjectClientNotifications((int)projectClientNotificationeDomianModels[0].ProjectNumber, new List<DomModel.ProjectClientNotification> { projectClientNotificationeDomianModels[0] });
            mockProjectClientNotificationRepository.Verify(m => m.Delete(It.IsAny<DbModel.ProjectClientNotification>()), Times.AtLeastOnce);
        }

       
        [TestMethod]
        public void GetAllDataAfterSavingProjectClientNotification()
        {

            mockProjectRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Project, bool>>>())).Returns(mockProjectDbData);
            mockProjectClientNotificationRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.ProjectClientNotification, bool>>>())).Returns((Expression<Func<DbModel.ProjectClientNotification, bool>> predicate) => mockProjectClientNotificationDbData.Where(predicate));
            mockCustomerContactRepo.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.CustomerContact, bool>>>())).Returns((Expression<Func<DbModel.CustomerContact, bool>> predicate) => mockCustomerContacts.Where(predicate));
            Validation = new Evolution.ValidationService.Services.ValidationService();
            validationService = new Evolution.Project.Infrastructure.Validations.ProjectClientNotificationValidationService(Validation);
            ProjectClientNotificationService = new Service.ProjectClientNotificationService(mockLogger.Object, mockProjectRepository.Object, mockProjectClientNotificationRepository.Object, mockCustomerContactRepo.Object, validationService);
            projectClientNotificationeDomianModels.ToList().ForEach(x => x.RecordStatus = "N");
            var response = ProjectClientNotificationService.SaveProjectClientNotifications((int)projectClientNotificationeDomianModels[0].ProjectNumber, new List<DomModel.ProjectClientNotification> { projectClientNotificationeDomianModels[0] },true,null,true);
            mockProjectClientNotificationRepository.Verify(m => m.Add(It.IsAny<IList<DbModel.ProjectClientNotification>>()), Times.AtLeastOnce);
        }
        [TestMethod]
        public void GetAllDataAfterUpdatingProjectClientNotification()
        {

            mockProjectRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Project, bool>>>())).Returns(mockProjectDbData);
            mockProjectClientNotificationRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.ProjectClientNotification, bool>>>())).Returns((Expression<Func<DbModel.ProjectClientNotification, bool>> predicate) => mockProjectClientNotificationDbData.Where(predicate));
            mockCustomerContactRepo.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.CustomerContact, bool>>>())).Returns((Expression<Func<DbModel.CustomerContact, bool>> predicate) => mockCustomerContacts.Where(predicate));
            Validation = new Evolution.ValidationService.Services.ValidationService();
            validationService = new Evolution.Project.Infrastructure.Validations.ProjectClientNotificationValidationService(Validation);
            ProjectClientNotificationService = new Service.ProjectClientNotificationService(mockLogger.Object, mockProjectRepository.Object, mockProjectClientNotificationRepository.Object, mockCustomerContactRepo.Object, validationService);
            projectClientNotificationeDomianModels.ToList().ForEach(x => x.RecordStatus = "M");
            var response = ProjectClientNotificationService.ModifyProjectClientNotifications((int)projectClientNotificationeDomianModels[0].ProjectNumber, new List<DomModel.ProjectClientNotification> { projectClientNotificationeDomianModels[0] }, true, null, true);
            mockProjectClientNotificationRepository.Verify(m => m.Update(It.IsAny<DbModel.ProjectClientNotification>()), Times.AtLeastOnce);
        }

        [TestMethod]
        public void GetAllDataAfterDeletingProjectClientNotification()
        {

            mockProjectRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Project, bool>>>())).Returns(mockProjectDbData);
            mockProjectClientNotificationRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.ProjectClientNotification, bool>>>())).Returns((Expression<Func<DbModel.ProjectClientNotification, bool>> predicate) => mockProjectClientNotificationDbData.Where(predicate));
           // mockCustomerContactRepo.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.CustomerContact, bool>>>())).Returns((Expression<Func<DbModel.CustomerContact, bool>> predicate) => mockCustomerContacts.Where(predicate));
            Validation = new Evolution.ValidationService.Services.ValidationService();
            validationService = new Evolution.Project.Infrastructure.Validations.ProjectClientNotificationValidationService(Validation);
            ProjectClientNotificationService = new Service.ProjectClientNotificationService(mockLogger.Object, mockProjectRepository.Object, mockProjectClientNotificationRepository.Object, mockCustomerContactRepo.Object, validationService);
            projectClientNotificationeDomianModels.ToList().ForEach(x => x.RecordStatus = "D");
            var response = ProjectClientNotificationService.DeleteProjectClientNotifications((int)projectClientNotificationeDomianModels[0].ProjectNumber, new List<DomModel.ProjectClientNotification> { projectClientNotificationeDomianModels[0] }, true, null, true);
            mockProjectClientNotificationRepository.Verify(m => m.Delete(It.IsAny<DbModel.ProjectClientNotification>()), Times.AtLeastOnce);
        }
        [TestMethod]
        public void ShouldNotSaveDataWhenProjectNumberIsNull()
        {

            mockProjectRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Project, bool>>>())).Returns(mockProjectDbData);
           // mockProjectClientNotificationRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.ProjectClientNotification, bool>>>())).Returns((Expression<Func<DbModel.ProjectClientNotification, bool>> predicate) => mockProjectClientNotificationDbData.Where(predicate));
           // mockCustomerContactRepo.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.CustomerContact, bool>>>())).Returns((Expression<Func<DbModel.CustomerContact, bool>> predicate) => mockCustomerContacts.Where(predicate));
            Validation = new Evolution.ValidationService.Services.ValidationService();
            validationService = new Evolution.Project.Infrastructure.Validations.ProjectClientNotificationValidationService(Validation);
            ProjectClientNotificationService = new Service.ProjectClientNotificationService(mockLogger.Object, mockProjectRepository.Object, mockProjectClientNotificationRepository.Object, mockCustomerContactRepo.Object, validationService);
            projectClientNotificationeDomianModels.ToList().ForEach(x => x.RecordStatus = "N");
            var response = ProjectClientNotificationService.SaveProjectClientNotifications(0, new List<DomModel.ProjectClientNotification> { projectClientNotificationeDomianModels[0] });
            Assert.AreEqual("11", response.Code);
        }
        [TestMethod]
        public void ShouldNotModifyDataWhenProjectNumberIsNull()
        {

            mockProjectRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Project, bool>>>())).Returns(mockProjectDbData);
          //  mockProjectClientNotificationRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.ProjectClientNotification, bool>>>())).Returns((Expression<Func<DbModel.ProjectClientNotification, bool>> predicate) => mockProjectClientNotificationDbData.Where(predicate));
          //  mockCustomerContactRepo.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.CustomerContact, bool>>>())).Returns((Expression<Func<DbModel.CustomerContact, bool>> predicate) => mockCustomerContacts.Where(predicate));
            Validation = new Evolution.ValidationService.Services.ValidationService();
            validationService = new Evolution.Project.Infrastructure.Validations.ProjectClientNotificationValidationService(Validation);
            ProjectClientNotificationService = new Service.ProjectClientNotificationService(mockLogger.Object, mockProjectRepository.Object, mockProjectClientNotificationRepository.Object, mockCustomerContactRepo.Object, validationService);
            projectClientNotificationeDomianModels.ToList().ForEach(x => x.RecordStatus = "M");
            var response = ProjectClientNotificationService.ModifyProjectClientNotifications(0, new List<DomModel.ProjectClientNotification> { projectClientNotificationeDomianModels[0] });
            Assert.AreEqual("11", response.Code);
        }
        [TestMethod]
        public void ShouldNotSaveDataWithProjectNumberIsInvalid()
        {

            //mockProjectRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Project, bool>>>())).Returns(mockProjectDbData);
          //  mockProjectClientNotificationRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.ProjectClientNotification, bool>>>())).Returns((Expression<Func<DbModel.ProjectClientNotification, bool>> predicate) => mockProjectClientNotificationDbData.Where(predicate));
           // mockCustomerContactRepo.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.CustomerContact, bool>>>())).Returns((Expression<Func<DbModel.CustomerContact, bool>> predicate) => mockCustomerContacts.Where(predicate));
            Validation = new Evolution.ValidationService.Services.ValidationService();
            validationService = new Evolution.Project.Infrastructure.Validations.ProjectClientNotificationValidationService(Validation);
            ProjectClientNotificationService = new Service.ProjectClientNotificationService(mockLogger.Object, mockProjectRepository.Object, mockProjectClientNotificationRepository.Object, mockCustomerContactRepo.Object, validationService);
            projectClientNotificationeDomianModels.ToList().ForEach(x => x.RecordStatus = "N");
       
            var response = ProjectClientNotificationService.SaveProjectClientNotifications(5, new List<DomModel.ProjectClientNotification> { projectClientNotificationeDomianModels[0] });
            Assert.AreEqual("11", response.Code);
        }

        [TestMethod]
        public void ShouldNotModifyDataWhenProjectNumberIsInvalid()
        {

            // mockProjectRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Project, bool>>>())).Returns(mockProjectDbData);
         //   mockProjectClientNotificationRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.ProjectClientNotification, bool>>>())).Returns((Expression<Func<DbModel.ProjectClientNotification, bool>> predicate) => mockProjectClientNotificationDbData.Where(predicate));
         //   mockCustomerContactRepo.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.CustomerContact, bool>>>())).Returns((Expression<Func<DbModel.CustomerContact, bool>> predicate) => mockCustomerContacts.Where(predicate));
            Validation = new Evolution.ValidationService.Services.ValidationService();
            validationService = new Evolution.Project.Infrastructure.Validations.ProjectClientNotificationValidationService(Validation);
            ProjectClientNotificationService = new Service.ProjectClientNotificationService(mockLogger.Object, mockProjectRepository.Object, mockProjectClientNotificationRepository.Object, mockCustomerContactRepo.Object, validationService);
            projectClientNotificationeDomianModels.ToList().ForEach(x => x.RecordStatus = "M");
            var response = ProjectClientNotificationService.ModifyProjectClientNotifications(5,new List<DomModel.ProjectClientNotification> { projectClientNotificationeDomianModels[0] });
            Assert.AreEqual("11", response.Code);
        }
        [TestMethod]
        public void ValidationOfCustomerContactWhileSave()
        {

            mockProjectRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Project, bool>>>())).Returns(mockProjectDbData);
         //   mockProjectClientNotificationRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.ProjectClientNotification, bool>>>())).Returns((Expression<Func<DbModel.ProjectClientNotification, bool>> predicate) => mockProjectClientNotificationDbData.Where(predicate));
            mockCustomerContactRepo.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.CustomerContact, bool>>>())).Returns((Expression<Func<DbModel.CustomerContact, bool>> predicate) => mockCustomerContacts.Where(predicate));
            Validation = new Evolution.ValidationService.Services.ValidationService();
            validationService = new Evolution.Project.Infrastructure.Validations.ProjectClientNotificationValidationService(Validation);
            ProjectClientNotificationService = new Service.ProjectClientNotificationService(mockLogger.Object, mockProjectRepository.Object, mockProjectClientNotificationRepository.Object, mockCustomerContactRepo.Object, validationService);
            projectClientNotificationeDomianModels.ToList().ForEach(x => x.RecordStatus = "N");
            projectClientNotificationeDomianModels[0].CustomerContact = "Invalid";
            var response = ProjectClientNotificationService.SaveProjectClientNotifications((int)projectClientNotificationeDomianModels[0].ProjectNumber, new List<DomModel.ProjectClientNotification> { projectClientNotificationeDomianModels[0] });
            Assert.AreEqual("11", response.Code);
        }

        [TestMethod]
        public void ValidationOfCustomerContactWhileUpdate()
        {

            mockProjectRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Project, bool>>>())).Returns(mockProjectDbData);
            mockProjectClientNotificationRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.ProjectClientNotification, bool>>>())).Returns((Expression<Func<DbModel.ProjectClientNotification, bool>> predicate) => mockProjectClientNotificationDbData.Where(predicate));
            mockCustomerContactRepo.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.CustomerContact, bool>>>())).Returns((Expression<Func<DbModel.CustomerContact, bool>> predicate) => mockCustomerContacts.Where(predicate));
            Validation = new Evolution.ValidationService.Services.ValidationService();
            validationService = new Evolution.Project.Infrastructure.Validations.ProjectClientNotificationValidationService(Validation);
            ProjectClientNotificationService = new Service.ProjectClientNotificationService(mockLogger.Object, mockProjectRepository.Object, mockProjectClientNotificationRepository.Object, mockCustomerContactRepo.Object, validationService);
            projectClientNotificationeDomianModels.ToList().ForEach(x => x.RecordStatus = "M");
            projectClientNotificationeDomianModels[0].CustomerContact = "ABC";
            var response = ProjectClientNotificationService.ModifyProjectClientNotifications((int)projectClientNotificationeDomianModels[0].ProjectNumber, new List<DomModel.ProjectClientNotification> { projectClientNotificationeDomianModels[0] });
            Assert.AreEqual("11", response.Code);
        }
        [TestMethod]
        public void ValidationOfUpdateCount()
        {

            mockProjectRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Project, bool>>>())).Returns(mockProjectDbData);
           // mockProjectClientNotificationRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.ProjectClientNotification, bool>>>())).Returns((Expression<Func<DbModel.ProjectClientNotification, bool>> predicate) => mockProjectClientNotificationDbData.Where(predicate));
           // mockCustomerContactRepo.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.CustomerContact, bool>>>())).Returns((Expression<Func<DbModel.CustomerContact, bool>> predicate) => mockCustomerContacts.Where(predicate));
            ProjectClientNotificationService = new Service.ProjectClientNotificationService(mockLogger.Object, mockProjectRepository.Object, mockProjectClientNotificationRepository.Object, mockCustomerContactRepo.Object, validationService);
            projectClientNotificationeDomianModels.ToList().ForEach(x => x.RecordStatus = "M");
            projectClientNotificationeDomianModels[0].UpdateCount = 5;
            var response = ProjectClientNotificationService.ModifyProjectClientNotifications((int)projectClientNotificationeDomianModels[0].ProjectNumber, new List<DomModel.ProjectClientNotification> { projectClientNotificationeDomianModels[0] });
            Assert.AreEqual("11", response.Code);
        }
        [TestMethod]
        public void IsClientNotificationAlreadyAssociatedForContact()
        {

            mockProjectRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Project, bool>>>())).Returns(mockProjectDbData);
           // mockProjectClientNotificationRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.ProjectClientNotification, bool>>>())).Returns((Expression<Func<DbModel.ProjectClientNotification, bool>> predicate) => mockProjectClientNotificationDbData.Where(predicate));
          //  mockCustomerContactRepo.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.CustomerContact, bool>>>())).Returns((Expression<Func<DbModel.CustomerContact, bool>> predicate) => mockCustomerContacts.Where(predicate));
            ProjectClientNotificationService = new Service.ProjectClientNotificationService(mockLogger.Object, mockProjectRepository.Object, mockProjectClientNotificationRepository.Object, mockCustomerContactRepo.Object, validationService);
            projectClientNotificationeDomianModels.ToList().ForEach(x => x.RecordStatus = "M");
            projectClientNotificationeDomianModels[0].CustomerContact= "Layla Gill";
            var response = ProjectClientNotificationService.ModifyProjectClientNotifications((int)projectClientNotificationeDomianModels[0].ProjectNumber, new List<DomModel.ProjectClientNotification> { projectClientNotificationeDomianModels[0] });
            Assert.AreEqual("11", response.Code);
        }

        [TestMethod]
        public void ISRecordIdIsValidForUpdate()
        {

            mockProjectRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Project, bool>>>())).Returns(mockProjectDbData);
          //  mockProjectClientNotificationRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.ProjectClientNotification, bool>>>())).Returns((Expression<Func<DbModel.ProjectClientNotification, bool>> predicate) => mockProjectClientNotificationDbData.Where(predicate));
           // mockCustomerContactRepo.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.CustomerContact, bool>>>())).Returns((Expression<Func<DbModel.CustomerContact, bool>> predicate) => mockCustomerContacts.Where(predicate));
            ProjectClientNotificationService = new Service.ProjectClientNotificationService(mockLogger.Object, mockProjectRepository.Object, mockProjectClientNotificationRepository.Object, mockCustomerContactRepo.Object, validationService);
            projectClientNotificationeDomianModels.ToList().ForEach(x => x.RecordStatus = "M");
            projectClientNotificationeDomianModels[0].ProjectClientNotificationId = 5;
            var response = ProjectClientNotificationService.ModifyProjectClientNotifications((int)projectClientNotificationeDomianModels[0].ProjectNumber, new List<DomModel.ProjectClientNotification> { projectClientNotificationeDomianModels[0] });
            Assert.AreEqual("11", response.Code);
        }
        [TestMethod]
        public void ISRecordIdIsValidForDelete()
        {

            mockProjectRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Project, bool>>>())).Returns(mockProjectDbData);
           // mockProjectClientNotificationRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.ProjectClientNotification, bool>>>())).Returns((Expression<Func<DbModel.ProjectClientNotification, bool>> predicate) => mockProjectClientNotificationDbData.Where(predicate));
           // mockCustomerContactRepo.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.CustomerContact, bool>>>())).Returns((Expression<Func<DbModel.CustomerContact, bool>> predicate) => mockCustomerContacts.Where(predicate));
            ProjectClientNotificationService = new Service.ProjectClientNotificationService(mockLogger.Object, mockProjectRepository.Object, mockProjectClientNotificationRepository.Object, mockCustomerContactRepo.Object, validationService);
            projectClientNotificationeDomianModels.ToList().ForEach(x => x.RecordStatus = "D");
            projectClientNotificationeDomianModels[0].ProjectClientNotificationId = 5;
            var response = ProjectClientNotificationService.DeleteProjectClientNotifications((int)projectClientNotificationeDomianModels[0].ProjectNumber, new List<DomModel.ProjectClientNotification> { projectClientNotificationeDomianModels[0] });
            Assert.AreEqual("11", response.Code);
        }


    }


}
