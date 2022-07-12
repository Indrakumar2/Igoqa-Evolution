using System;
using System.Collections.Generic;
using System.Text;

using ServiceDomainData = Evolution.Contract.Domain.Interfaces.Contracts;
using DomainData = Evolution.Contract.Domain.Interfaces.Data;
using Moq;
using DomModel = Evolution.Contract.Domain.Models.Contracts;
using Evolution.Logging.Interfaces;
using AutoMapper;
using DbModel = Evolution.DbRepository.Models;
using System.Linq;
using Evolution.UnitTests.Mocks.Data.Contracts.Domain;
using Evolution.UnitTests.Mocks.Data.Contracts.Db;
using Masterdata = Evolution.Master.Domain.Interfaces.Data;
using System.Linq.Expressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Service = Evolution.Contract.Core.Services;
using Evolution.UnitTest.UnitTestCases;
using ValidService = Evolution.Contract.Domain.Interfaces.Validations;

namespace Evolution.UnitTests.UnitTestCases.Contract.Services
{
    [TestClass]
   public class ContractNotesServiceUnitTestCase : BaseTestCase
    {
       ServiceDomainData.IContractNoteService ContractNoteService = null;
        Mock<DomainData.IContractNoteRepository> mockContractNoteRepository = null;
        Mock<DomainData.IContractRepository> mockContractRepository = null;
        IList<DomModel.ContractNote> contractNoteDomianModels = null;
        Mock<IAppLogger<Service.ContractNoteService>> mockLogger = null;
        Mock<IMapper> mockMapper = null;
        IQueryable<DbModel.Contract> mockContractDbData = null;
        IQueryable<DbModel.ContractNote> mockContractNoteDbData = null;
        ValidService.IContractNoteValidationService NoteValidService = null;

        [TestInitialize]
        public void InitializeContractNoteService()
        {
            mockContractNoteRepository = new Mock<DomainData.IContractNoteRepository>();
            mockContractRepository = new Mock<DomainData.IContractRepository>();
            contractNoteDomianModels = MockContractDomainModel.GetContractNoteMockedDomainModelData();
            mockLogger = new Mock<IAppLogger<Service.ContractNoteService>>();
            mockMapper = new Mock<IMapper>();
            mockContractDbData = MockContract.GetDbModelContract();
          
            mockContractNoteDbData = MockContract.GetContractNoteMockData();
           
           
        }
        [TestMethod]
        public void FetchContractNoteListWithoutSearchValue()
        {
            mockContractNoteRepository.Setup(x => x.Search(It.IsAny<DomModel.ContractNote>())).Returns(contractNoteDomianModels);

            ContractNoteService = new Service.ContractNoteService(mockMapper.Object,mockLogger.Object,mockContractNoteRepository.Object,mockContractRepository.Object, NoteValidService);

            var response = ContractNoteService.GetContractNote(new DomModel.ContractNote());

            Assert.AreEqual("1", response.Code);
            Assert.AreEqual(2, (response.Result as List<DomModel.ContractNote>).Count);
        }
        [TestMethod]
        public void FetchContractNoteListWithNullSearchModel()
        {
            mockContractNoteRepository.Setup(x => x.Search(It.IsAny<DomModel.ContractNote>())).Returns(contractNoteDomianModels);

            ContractNoteService = new Service.ContractNoteService(mockMapper.Object, mockLogger.Object, mockContractNoteRepository.Object,mockContractRepository.Object, NoteValidService);

            var response = ContractNoteService.GetContractNote(null);

            Assert.AreEqual("1", response.Code);
            Assert.AreEqual(2, (response.Result as List<DomModel.ContractNote>).Count);
        }

        [TestMethod]
        public void FetchContractNoteListByContractNumber()
        {
            mockContractNoteRepository.Setup(x => x.Search(It.Is<DomModel.ContractNote>(c => c.ContractNumber == "SU02412/0001"))).Returns(new List<DomModel.ContractNote> { contractNoteDomianModels[0] });

            ContractNoteService = new Service.ContractNoteService(mockMapper.Object, mockLogger.Object, mockContractNoteRepository.Object, mockContractRepository.Object, NoteValidService);

            var response = ContractNoteService.GetContractNote(new DomModel.ContractNote { ContractNumber = "SU02412/0001" });

            Assert.AreEqual("1", response.Code);
            Assert.AreEqual(1, (response.Result as List<DomModel.ContractNote>).Count);
        }
        [TestMethod]
        public void FetchContractNoteListByWildCardSearchWithNotesStartWith()
        {
            mockContractNoteRepository.Setup(x => x.Search(It.Is<DomModel.ContractNote>(c => c.Notes.StartsWith("Current")))).Returns(new List<DomModel.ContractNote> { contractNoteDomianModels[1] });

            ContractNoteService = new Service.ContractNoteService(mockMapper.Object, mockLogger.Object, mockContractNoteRepository.Object, mockContractRepository.Object, NoteValidService);

            var response = ContractNoteService.GetContractNote(new DomModel.ContractNote { Notes = "Current*" });

            Assert.AreEqual("1", response.Code);
            Assert.AreEqual(1, (response.Result as List<DomModel.ContractNote>).Count);
        }
        [TestMethod]
        public void FetchContractNoteListByWildCardSearchWithNotesEndWith()
        {
            mockContractNoteRepository.Setup(x => x.Search(It.Is<DomModel.ContractNote>(c => c.Notes.EndsWith("April 2008")))).Returns(new List<DomModel.ContractNote> { contractNoteDomianModels[1]});

            ContractNoteService = new Service.ContractNoteService(mockMapper.Object, mockLogger.Object, mockContractNoteRepository.Object, mockContractRepository.Object, NoteValidService);

            var response = ContractNoteService.GetContractNote(new DomModel.ContractNote { Notes = "*April 2008" });

            Assert.AreEqual("1", response.Code);
            Assert.AreEqual(1, (response.Result as List<DomModel.ContractNote>).Count);
        }
       
        [TestMethod]
        public void FetchcontractNoteListByWildCardSearchWithNotesContains()
        {
            mockContractNoteRepository.Setup(x => x.Search(It.Is<DomModel.ContractNote>(c => c.Notes.Contains("Aker Solutions")))).Returns(new List<DomModel.ContractNote> { contractNoteDomianModels[1] });

            ContractNoteService = new Service.ContractNoteService(mockMapper.Object, mockLogger.Object, mockContractNoteRepository.Object, mockContractRepository.Object, NoteValidService);
            var response = ContractNoteService.GetContractNote(new DomModel.ContractNote { Notes = "*Aker Solutions*" });

            Assert.AreEqual("1", response.Code);
            Assert.AreEqual(1, (response.Result as List<DomModel.ContractNote>).Count);
        }

        [TestMethod]
        public void ThrowsExceptionWhileFetchingNote()
        {
            mockContractNoteRepository.Setup(x => x.Search(It.IsAny<DomModel.ContractNote>())).Throws(new Exception("Exception occured while performing some operation."));
            ContractNoteService = new Service.ContractNoteService(mockMapper.Object, mockLogger.Object, mockContractNoteRepository.Object, mockContractRepository.Object, NoteValidService);

            var response = ContractNoteService.GetContractNote(new DomModel.ContractNote());

            Assert.AreEqual("11", response.Code);
            Assert.IsNotNull(response.Messages);
            Assert.AreEqual("31", response.Messages.FirstOrDefault().Code);
        }

        [TestMethod]
        public void SaveContractNotes()
        {
            mockContractRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Contract, bool>>>())).Returns(mockContractDbData);
            mockContractNoteRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.ContractNote, bool>>>())).Returns((Expression<Func<DbModel.ContractNote, bool>> predicate) => mockContractNoteDbData.Where(predicate));
            ContractNoteService = new Service.ContractNoteService(mockMapper.Object, mockLogger.Object, mockContractNoteRepository.Object, mockContractRepository.Object, NoteValidService);

            contractNoteDomianModels.ToList().ForEach(x => x.RecordStatus = "N");

            var response = ContractNoteService.SaveContractNote(contractNoteDomianModels[0].ContractNumber, new List<DomModel.ContractNote> { contractNoteDomianModels[0] }, true);

            mockContractNoteRepository.Verify(m => m.Add(It.IsAny<IList<DbModel.ContractNote>>()), Times.AtLeastOnce);
        }

       

        [TestMethod]
        public void DeletecontractNote()
        {
            mockContractRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Contract, bool>>>())).Returns(mockContractDbData);
            mockContractNoteRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.ContractNote, bool>>>())).Returns((Expression<Func<DbModel.ContractNote, bool>> predicate) => mockContractNoteDbData.Where(predicate));
            ContractNoteService = new Service.ContractNoteService(mockMapper.Object, mockLogger.Object, mockContractNoteRepository.Object, mockContractRepository.Object, NoteValidService);
            contractNoteDomianModels.ToList().ForEach(x => x.RecordStatus = "D");
            var response = ContractNoteService.DeleteContractNote(contractNoteDomianModels[0].ContractNumber, new List<DomModel.ContractNote> { contractNoteDomianModels[0] }, true);

            mockContractNoteRepository.Verify(m => m.Delete(It.IsAny<DbModel.ContractNote>()), Times.AtLeast(1));
        }

        [TestMethod]
        public void ThrowsExceptionWhileSavingNote()
        {
            mockContractRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Contract, bool>>>())).Throws(new Exception("Exception occured while performing some operation."));
            mockContractNoteRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.ContractNote, bool>>>())).Returns((Expression<Func<DbModel.ContractNote, bool>> predicate) => mockContractNoteDbData.Where(predicate));
            ContractNoteService = new Service.ContractNoteService(mockMapper.Object, mockLogger.Object, mockContractNoteRepository.Object, mockContractRepository.Object, NoteValidService);

            contractNoteDomianModels.ToList().ForEach(x => x.RecordStatus = "N");

            var response = ContractNoteService.SaveContractNote(contractNoteDomianModels[0].ContractNumber, new List<DomModel.ContractNote> { contractNoteDomianModels[0] }, true); 

         
            Assert.AreEqual("11", response.Code);
            Assert.IsNotNull(response.Messages);
            Assert.AreEqual("31", response.Messages.FirstOrDefault().Code);
        }

        [TestMethod]
        public void ThrowsExceptionWhileDeletingNote()
        {
            mockContractRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Contract, bool>>>())).Throws(new Exception("Exception occured while performing some operation."));
            mockContractNoteRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.ContractNote, bool>>>())).Returns((Expression<Func<DbModel.ContractNote, bool>> predicate) => mockContractNoteDbData.Where(predicate));
            ContractNoteService = new Service.ContractNoteService(mockMapper.Object, mockLogger.Object, mockContractNoteRepository.Object, mockContractRepository.Object, NoteValidService);
            contractNoteDomianModels.ToList().ForEach(x => x.RecordStatus = "D");
            var response = ContractNoteService.DeleteContractNote(contractNoteDomianModels[0].ContractNumber, new List<DomModel.ContractNote> { contractNoteDomianModels[0] }, true);
            Assert.AreEqual("11", response.Code);
            Assert.IsNotNull(response.Messages);
            Assert.AreEqual("31", response.Messages.FirstOrDefault().Code);

        }

        [TestMethod]
        public void ShouldNotSaveDataWhenContractNumberIsNull()
        {
            mockContractRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Contract, bool>>>())).Returns(mockContractDbData);
            mockContractNoteRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.ContractNote, bool>>>())).Returns((Expression<Func<DbModel.ContractNote, bool>> predicate) => mockContractNoteDbData.Where(predicate));
            ContractNoteService = new Service.ContractNoteService(mockMapper.Object, mockLogger.Object, mockContractNoteRepository.Object, mockContractRepository.Object, NoteValidService);

            contractNoteDomianModels.ToList().ForEach(x => x.RecordStatus = "N");

            var response = ContractNoteService.SaveContractNote(null, new List<DomModel.ContractNote> { contractNoteDomianModels[0] }, true);
            Assert.AreEqual("11", response.Code);

        }

        [TestMethod]
        public void ShouldNotSaveDataWhenContractIsInvalid()
        {
            //mockContractRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Contract, bool>>>())).Returns(mockContractDbData);
            mockContractNoteRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.ContractNote, bool>>>())).Returns((Expression<Func<DbModel.ContractNote, bool>> predicate) => mockContractNoteDbData.Where(predicate));
            ContractNoteService = new Service.ContractNoteService(mockMapper.Object, mockLogger.Object, mockContractNoteRepository.Object, mockContractRepository.Object, NoteValidService);

            contractNoteDomianModels.ToList().ForEach(x => x.RecordStatus = "N");

            var response = ContractNoteService.SaveContractNote(contractNoteDomianModels[0].ContractNumber, new List<DomModel.ContractNote> { contractNoteDomianModels[0] }, true);
            Assert.AreEqual("11", response.Code);

        }

        [TestMethod]
        public void GetAllSavedNotes()
        {
            mockContractRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Contract, bool>>>())).Returns(mockContractDbData);
            mockContractNoteRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.ContractNote, bool>>>())).Returns((Expression<Func<DbModel.ContractNote, bool>> predicate) => mockContractNoteDbData.Where(predicate));
            ContractNoteService = new Service.ContractNoteService(mockMapper.Object, mockLogger.Object, mockContractNoteRepository.Object, mockContractRepository.Object, NoteValidService);

            contractNoteDomianModels.ToList().ForEach(x => x.RecordStatus = "N");

            var response = ContractNoteService.SaveContractNote(contractNoteDomianModels[0].ContractNumber, new List<DomModel.ContractNote> { contractNoteDomianModels[0] }, true,true);

            mockContractNoteRepository.Verify(m => m.Add(It.IsAny<IList<DbModel.ContractNote>>()), Times.AtLeastOnce);
        }



        [TestMethod]
        public void GetAllNotesAfterDelete()
        {
            mockContractRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Contract, bool>>>())).Returns(mockContractDbData);
            mockContractNoteRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.ContractNote, bool>>>())).Returns((Expression<Func<DbModel.ContractNote, bool>> predicate) => mockContractNoteDbData.Where(predicate));
            ContractNoteService = new Service.ContractNoteService(mockMapper.Object, mockLogger.Object, mockContractNoteRepository.Object, mockContractRepository.Object, NoteValidService);
            contractNoteDomianModels.ToList().ForEach(x => x.RecordStatus = "D");
            var response = ContractNoteService.DeleteContractNote(contractNoteDomianModels[0].ContractNumber, new List<DomModel.ContractNote> { contractNoteDomianModels[0] }, true,true);

            mockContractNoteRepository.Verify(m => m.Delete(It.IsAny<DbModel.ContractNote>()), Times.AtLeast(1));
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
