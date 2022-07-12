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
using Evolution.UnitTests.Mocks.Data.Projects.Db;
using System.Linq.Expressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Service = Evolution.Project.Core.Services;
using ValdService = Evolution.Project.Domain.Interfaces.Validations;
using Evolution.ValidationService.Interfaces;

namespace Evolution.UnitTests.UnitTestCases.Projects.Services
{
    [TestClass]
   public class ProjectNoteServicesUnitTestCases:BaseTestCase
   {
        IValidationService Validation = null;
        ServiceDomainData.IProjectNotesService ProjectNoteService = null;
        Mock<DomainData.IProjectNoteRepository> mockProjectNoteRepository = null;
        Mock<DomainData.IProjectRepository> mockProjectRepository = null;
        IList<DomModel.ProjectNote> ProjectNoteDomianModels = null;
        Mock<IAppLogger<Service.ProjectNotesService>> mockLogger = null;
        IQueryable<DbModel.Project> mockProjectDbData = null;
        IQueryable<DbModel.ProjectNote> mockProjectNoteDbData = null;
        ValdService.IProjectNoteValidationService validationService = null;


        [TestInitialize]
        public void InitializeProjectNoteService()
        {
            mockProjectNoteRepository = new Mock<DomainData.IProjectNoteRepository>();
            mockProjectRepository = new Mock<DomainData.IProjectRepository>();
            ProjectNoteDomianModels = MockProjectDomainModel.GetProjectNotesMockedDomainData();
            mockLogger = new Mock<IAppLogger<Service.ProjectNotesService>>();
            mockProjectDbData =MockProject.GetprojectMockData();
            mockProjectNoteDbData = MockProject.GetProjectNotesMockedData();


        }

        [TestMethod]
        public void FetchProjectNoteListWithoutSearchValue()
        {
            mockProjectNoteRepository.Setup(x => x.Search(It.IsAny<DomModel.ProjectNote>())).Returns(ProjectNoteDomianModels);

            ProjectNoteService = new Service.ProjectNotesService(Mapper.Instance, mockLogger.Object, mockProjectNoteRepository.Object, mockProjectRepository.Object, validationService);

            var response = ProjectNoteService.GetProjectNotes(new DomModel.ProjectNote());

            Assert.AreEqual("1", response.Code);
            Assert.AreEqual(2, (response.Result as List<DomModel.ProjectNote>).Count);
        }
        [TestMethod]
        public void FetchProjectNoteListWithNullSearchModel()
        {
            mockProjectNoteRepository.Setup(x => x.Search(It.IsAny<DomModel.ProjectNote>())).Returns(ProjectNoteDomianModels);

            ProjectNoteService = new Service.ProjectNotesService(Mapper.Instance, mockLogger.Object, mockProjectNoteRepository.Object, mockProjectRepository.Object, validationService);

            var response = ProjectNoteService.GetProjectNotes(null);

            Assert.AreEqual("1", response.Code);
            Assert.AreEqual(2, (response.Result as List<DomModel.ProjectNote>).Count);
        }
        [TestMethod]
        public void FetchProjectNoteListByProjectNumber()
        {
            mockProjectNoteRepository.Setup(x => x.Search(It.Is<DomModel.ProjectNote>(c => c.ProjectNumber == 1))).Returns(new List<DomModel.ProjectNote> { ProjectNoteDomianModels[0] });

            ProjectNoteService = new Service.ProjectNotesService(Mapper.Instance, mockLogger.Object, mockProjectNoteRepository.Object, mockProjectRepository.Object, validationService);

            var response = ProjectNoteService.GetProjectNotes(new DomModel.ProjectNote { ProjectNumber = 1 });

            Assert.AreEqual("1", response.Code);
            Assert.AreEqual(1, (response.Result as List<DomModel.ProjectNote>).Count);
        }
        [TestMethod]
        public void FetchProjectNoteListByWildCardSearchWithNotesStartWith()
        {
            mockProjectNoteRepository.Setup(x => x.Search(It.Is<DomModel.ProjectNote>(c => c.Notes.StartsWith("Re")))).Returns(new List<DomModel.ProjectNote> { ProjectNoteDomianModels[0] });

            ProjectNoteService = new Service.ProjectNotesService(Mapper.Instance, mockLogger.Object, mockProjectNoteRepository.Object, mockProjectRepository.Object, validationService);

            var response = ProjectNoteService.GetProjectNotes(new DomModel.ProjectNote { Notes = "Re*" });

            Assert.AreEqual("1", response.Code);
            Assert.AreEqual(1, (response.Result as List<DomModel.ProjectNote>).Count);
        }
        [TestMethod]
        public void FetchProjectNoteListByWildCardSearchWithNotesEndWith()
        {
            mockProjectNoteRepository.Setup(x => x.Search(It.Is<DomModel.ProjectNote>(c => c.Notes.EndsWith("EVO")))).Returns(new List<DomModel.ProjectNote> { ProjectNoteDomianModels[0] });

            ProjectNoteService = new Service.ProjectNotesService(Mapper.Instance, mockLogger.Object, mockProjectNoteRepository.Object, mockProjectRepository.Object, validationService);

            var response = ProjectNoteService.GetProjectNotes(new DomModel.ProjectNote { Notes = "*EVO" });

            Assert.AreEqual("1", response.Code);
            Assert.AreEqual(1, (response.Result as List<DomModel.ProjectNote>).Count);
        }

        [TestMethod]
        public void FetchProjectNoteListByWildCardSearchWithNotesContains()
        {
            mockProjectNoteRepository.Setup(x => x.Search(It.Is<DomModel.ProjectNote>(c => c.Notes.Contains("assignment")))).Returns(new List<DomModel.ProjectNote> { ProjectNoteDomianModels[0] });

            ProjectNoteService = new Service.ProjectNotesService(Mapper.Instance, mockLogger.Object, mockProjectNoteRepository.Object, mockProjectRepository.Object, validationService);
            var response = ProjectNoteService.GetProjectNotes(new DomModel.ProjectNote { Notes = "*assignment*" });

            Assert.AreEqual("1", response.Code);
            Assert.AreEqual(1, (response.Result as List<DomModel.ProjectNote>).Count);
        }

        [TestMethod]
        public void ThrowsExceptionWhileFetchingNote()
        {
            mockProjectNoteRepository.Setup(x => x.Search(It.IsAny<DomModel.ProjectNote>())).Throws(new Exception("Exception occured while performing some operation."));
            ProjectNoteService = new Service.ProjectNotesService(Mapper.Instance, mockLogger.Object, mockProjectNoteRepository.Object, mockProjectRepository.Object, validationService);

            var response = ProjectNoteService.GetProjectNotes(new DomModel.ProjectNote());

            Assert.AreEqual("11", response.Code);
            Assert.IsNotNull(response.Messages);
            Assert.AreEqual("31", response.Messages.FirstOrDefault().Code);
        }

        [TestMethod]
        public void SaveProjectNote()
        {
            mockProjectRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Project, bool>>>())).Returns(mockProjectDbData);
        
            Validation = new Evolution.ValidationService.Services.ValidationService();
            validationService = new Evolution.Project.Infrastructure.Validations.ProjectNoteValidationService(Validation);
            ProjectNoteService = new Service.ProjectNotesService(Mapper.Instance, mockLogger.Object, mockProjectNoteRepository.Object, mockProjectRepository.Object, validationService);

            ProjectNoteDomianModels.ToList().ForEach(x => x.RecordStatus = "N");

            var response = ProjectNoteService.SaveProjectNotes((int)ProjectNoteDomianModels[0].ProjectNumber, new List<DomModel.ProjectNote> { ProjectNoteDomianModels[0] }, true);

            mockProjectNoteRepository.Verify(m => m.Add(It.IsAny<IList<DbModel.ProjectNote>>()), Times.AtLeastOnce);
        }
        [TestMethod]
        public void DeleteProjectNote()
        {
            mockProjectRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Project, bool>>>())).Returns(mockProjectDbData);
            mockProjectNoteRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.ProjectNote, bool>>>())).Returns((Expression<Func<DbModel.ProjectNote, bool>> predicate) => mockProjectNoteDbData.Where(predicate));
            Validation = new Evolution.ValidationService.Services.ValidationService();
            validationService = new Evolution.Project.Infrastructure.Validations.ProjectNoteValidationService(Validation);
            ProjectNoteService = new Service.ProjectNotesService(Mapper.Instance, mockLogger.Object, mockProjectNoteRepository.Object, mockProjectRepository.Object, validationService);
            ProjectNoteDomianModels.ToList().ForEach(x => x.RecordStatus = "D");
            var response = ProjectNoteService.DeleteProjectNotes((int)ProjectNoteDomianModels[0].ProjectNumber, new List<DomModel.ProjectNote> { ProjectNoteDomianModels[0] }, true);

            mockProjectNoteRepository.Verify(m => m.Delete(It.IsAny<DbModel.ProjectNote>()), Times.AtLeast(1));
        }
        [TestMethod]
        public void ThrowsExceptionWhileSavingNote()
        {
            mockProjectRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Project, bool>>>())).Throws(new Exception("Exception occured while performing some operation."));
        
            Validation = new Evolution.ValidationService.Services.ValidationService();
            validationService = new Evolution.Project.Infrastructure.Validations.ProjectNoteValidationService(Validation);
            ProjectNoteService = new Service.ProjectNotesService(Mapper.Instance, mockLogger.Object, mockProjectNoteRepository.Object, mockProjectRepository.Object, validationService);

            ProjectNoteDomianModels.ToList().ForEach(x => x.RecordStatus = "N");

            var response = ProjectNoteService.SaveProjectNotes((int)ProjectNoteDomianModels[0].ProjectNumber, new List<DomModel.ProjectNote> { ProjectNoteDomianModels[0] }, true);


            Assert.AreEqual("11", response.Code);
            Assert.IsNotNull(response.Messages);
            Assert.AreEqual("31", response.Messages.FirstOrDefault().Code);
        }
        [TestMethod]
        public void ThrowsExceptionWhileDeletingNote()
        {
            mockProjectRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Project, bool>>>())).Throws(new Exception("Exception occured while performing some operation."));
         
            Validation = new Evolution.ValidationService.Services.ValidationService();
            validationService = new Evolution.Project.Infrastructure.Validations.ProjectNoteValidationService(Validation);
            ProjectNoteService = new Service.ProjectNotesService(Mapper.Instance, mockLogger.Object, mockProjectNoteRepository.Object, mockProjectRepository.Object, validationService);
            ProjectNoteDomianModels.ToList().ForEach(x => x.RecordStatus = "D");
            var response = ProjectNoteService.DeleteProjectNotes((int)ProjectNoteDomianModels[0].ProjectNumber, new List<DomModel.ProjectNote> { ProjectNoteDomianModels[0] }, true);
            Assert.AreEqual("11", response.Code);
            Assert.IsNotNull(response.Messages);
            Assert.AreEqual("31", response.Messages.FirstOrDefault().Code);

        }

     

        [TestMethod]
        public void ShouldNotSaveDataWhenProjectIsInvalid()
        {
          
            Validation = new Evolution.ValidationService.Services.ValidationService();
            validationService = new Evolution.Project.Infrastructure.Validations.ProjectNoteValidationService(Validation);
            ProjectNoteService = new Service.ProjectNotesService(Mapper.Instance, mockLogger.Object, mockProjectNoteRepository.Object, mockProjectRepository.Object, validationService);

            ProjectNoteDomianModels.ToList().ForEach(x => x.RecordStatus = "N");

            var response = ProjectNoteService.SaveProjectNotes((int)ProjectNoteDomianModels[0].ProjectNumber, new List<DomModel.ProjectNote> { ProjectNoteDomianModels[0] }, true);
            Assert.AreEqual("11", response.Code);

        }

        [TestMethod]
        public void GetAllSavedNotes()
        {
            mockProjectRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Project, bool>>>())).Returns(mockProjectDbData);
           
            Validation = new Evolution.ValidationService.Services.ValidationService();
            validationService = new Evolution.Project.Infrastructure.Validations.ProjectNoteValidationService(Validation);
            ProjectNoteService = new Service.ProjectNotesService(Mapper.Instance, mockLogger.Object, mockProjectNoteRepository.Object, mockProjectRepository.Object, validationService);

            ProjectNoteDomianModels.ToList().ForEach(x => x.RecordStatus = "N");

            var response = ProjectNoteService.SaveProjectNotes((int)ProjectNoteDomianModels[0].ProjectNumber, new List<DomModel.ProjectNote> { ProjectNoteDomianModels[0] }, true, true);

            mockProjectNoteRepository.Verify(m => m.Add(It.IsAny<IList<DbModel.ProjectNote>>()), Times.AtLeastOnce);
        }



        [TestMethod]
        public void GetAllNotesAfterDelete()
        {
            mockProjectRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Project, bool>>>())).Returns(mockProjectDbData);
            mockProjectNoteRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.ProjectNote, bool>>>())).Returns((Expression<Func<DbModel.ProjectNote, bool>> predicate) => mockProjectNoteDbData.Where(predicate));
            Validation = new Evolution.ValidationService.Services.ValidationService();
            validationService = new Evolution.Project.Infrastructure.Validations.ProjectNoteValidationService(Validation);
            ProjectNoteService = new Service.ProjectNotesService(Mapper.Instance, mockLogger.Object, mockProjectNoteRepository.Object, mockProjectRepository.Object, validationService);
            ProjectNoteDomianModels.ToList().ForEach(x => x.RecordStatus = "D");
            var response = ProjectNoteService.DeleteProjectNotes((int)ProjectNoteDomianModels[0].ProjectNumber, new List<DomModel.ProjectNote> { ProjectNoteDomianModels[0] }, true, true);

            mockProjectNoteRepository.Verify(m => m.Delete(It.IsAny<DbModel.ProjectNote>()), Times.AtLeast(1));
        }




    }
}
