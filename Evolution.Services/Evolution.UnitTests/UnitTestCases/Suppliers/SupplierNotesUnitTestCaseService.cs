using Evolution.Logging.Interfaces;
using Evolution.Supplier.Core.Services;
using Evolution.Supplier.Domain.Interfaces.Data;
using Evolution.Supplier.Domain.Interfaces.Suppliers;
using Evolution.ValidationService.Interfaces;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DomModel = Evolution.Supplier.Domain.Models.Supplier;
using DbModel = Evolution.DbRepository.Models;
using ValdService = Evolution.Supplier.Domain.Interfaces.Validations;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Evolution.UnitTests.Mocks.Data.Supplier.Domain;
using Evolution.UnitTests.Mocks.Data.Supplier.Db;
using SupplierService = Evolution.Supplier.Core.Services;
using AutoMapper;
using System.Linq.Expressions;
using Evolution.UnitTest.UnitTestCases;

namespace Evolution.UnitTests.UnitTestCases.Suppliers
{
    [TestClass]
   public  class SupplierNotesUnitTestCaseService:BaseTestCase
    {
        ISupplierNoteService SupplierNoteService = null;
        Mock<ISupplierNoteRepository> mockSupplierNoteRepo = null;
        Mock<ISupplierRepository> mockSupplierRepo = null;
        Mock<IAppLogger<SupplierNoteService>> mockLogger = null;
        IValidationService Validation = null;
        IList<DomModel.SupplierNote> SupplierNoteDomianModels = null;
        IQueryable<DbModel.Supplier> mockSupplierDbData = null;
        IQueryable<DbModel.SupplierNote> mockSupplierNoteDbData = null;
        ValdService.ISupplierNoteValidationService validationService = null;
        Mock<IMapper> mockMapper = null;

        [TestInitialize]
        public void InitializeSupplierNoteService()
        {
            mockSupplierNoteRepo = new Mock<ISupplierNoteRepository>();
            mockSupplierRepo = new Mock<ISupplierRepository>();
            mockLogger = new Mock<IAppLogger<SupplierNoteService>>();
            SupplierNoteDomianModels = MockSupplierDomainModel.GetSupplierNoteDomModel();
            mockSupplierDbData = MockSupplier.GetSupplierMockedData();
            mockSupplierNoteDbData = MockSupplier.GetSupplierNoteMockedData();
            mockMapper = new Mock<IMapper>();
        }

        [TestMethod]
        public void FetchSupplierNoteListWithoutSearchValue()
        {
            mockSupplierNoteRepo.Setup(x => x.Search(It.IsAny<DomModel.SupplierNote>())).Returns(SupplierNoteDomianModels);
               

            SupplierNoteService = new SupplierService.SupplierNoteService(mockMapper.Object, mockLogger.Object, mockSupplierNoteRepo.Object, mockSupplierRepo.Object, validationService);

            var response = SupplierNoteService.GetSupplierNote(new DomModel.SupplierNote());

            Assert.AreEqual("1", response.Code);
            Assert.AreEqual(2, (response.Result as List<DomModel.SupplierNote>).Count);
        }

        [TestMethod]
        public void FetchSupplierNoteListWithSearchValue()
        {
            mockSupplierNoteRepo.Setup(x => x.Search(It.IsAny<DomModel.SupplierNote>())).Returns(new List<DomModel.SupplierNote>() { SupplierNoteDomianModels[0] });

            SupplierNoteService = new SupplierService.SupplierNoteService(mockMapper.Object, mockLogger.Object, mockSupplierNoteRepo.Object, mockSupplierRepo.Object, validationService);

            var response = SupplierNoteService.GetSupplierNote(new DomModel.SupplierNote() {SupplierNoteId=1 });

            Assert.AreEqual("1", response.Code);
            Assert.AreEqual(1, (response.Result as List<DomModel.SupplierNote>).Count);
        }
        [TestMethod]
        public void FetchSupplierNoteListBySupplierId()
        {
            mockSupplierNoteRepo.Setup(x => x.Search(It.IsAny<DomModel.SupplierNote>())).Returns(new List<DomModel.SupplierNote>() { SupplierNoteDomianModels[0] });

            SupplierNoteService = new SupplierService.SupplierNoteService(mockMapper.Object, mockLogger.Object, mockSupplierNoteRepo.Object, mockSupplierRepo.Object, validationService);

            var response = SupplierNoteService.GetSupplierNote(new DomModel.SupplierNote() { SupplierId = 1 });

            Assert.AreEqual("1", response.Code);
            Assert.AreEqual(1, (response.Result as List<DomModel.SupplierNote>).Count);
        }
        [TestMethod]
        public void ThrowsExceptionWhileFetchingNote()
        {
            mockSupplierNoteRepo.Setup(x => x.Search(It.IsAny<DomModel.SupplierNote>())).Throws(new Exception("Exception occured while performing some operation."));
           SupplierNoteService = new SupplierService.SupplierNoteService(mockMapper.Object, mockLogger.Object, mockSupplierNoteRepo.Object, mockSupplierRepo.Object, validationService);

            var response = SupplierNoteService.GetSupplierNote(new DomModel.SupplierNote());

            Assert.AreEqual("11", response.Code);
            Assert.IsNotNull(response.Messages);
            Assert.AreEqual("31", response.Messages.FirstOrDefault().Code);
        }
        [TestMethod]
        public void SaveSupplierNote()
        {
            mockSupplierRepo.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Supplier, bool>>>())).Returns(mockSupplierDbData);
            mockSupplierNoteRepo.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.SupplierNote, bool>>>())).Returns((Expression<Func<DbModel.SupplierNote, bool>> predicate) => mockSupplierNoteDbData.Where(predicate));
            Validation = new Evolution.ValidationService.Services.ValidationService();
            validationService = new Evolution.Supplier.Infrastructure.Validations.SupplierNoteValidationService(Validation);
            SupplierNoteService = new SupplierService.SupplierNoteService(Mapper.Instance, mockLogger.Object, mockSupplierNoteRepo.Object, mockSupplierRepo.Object, validationService);

            SupplierNoteDomianModels.ToList().ForEach(x => x.RecordStatus = "N");

            var response = SupplierNoteService.SaveSupplierNote((int)SupplierNoteDomianModels[0].SupplierId, new List<DomModel.SupplierNote> { SupplierNoteDomianModels[0] }, true);

            mockSupplierNoteRepo.Verify(m => m.Add(It.IsAny<IList<DbModel.SupplierNote>>()), Times.AtLeastOnce);
        }
        [TestMethod]
        public void DeleteSupplierNote()
        {
            mockSupplierRepo.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Supplier, bool>>>())).Returns(mockSupplierDbData);
            mockSupplierNoteRepo.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.SupplierNote, bool>>>())).Returns((Expression<Func<DbModel.SupplierNote, bool>> predicate) => mockSupplierNoteDbData.Where(predicate));
            Validation = new Evolution.ValidationService.Services.ValidationService();
            validationService = new Evolution.Supplier.Infrastructure.Validations.SupplierNoteValidationService(Validation);
            SupplierNoteService = new SupplierService.SupplierNoteService(Mapper.Instance, mockLogger.Object, mockSupplierNoteRepo.Object, mockSupplierRepo.Object, validationService);
            SupplierNoteDomianModels.ToList().ForEach(x => x.RecordStatus = "D");
            var response = SupplierNoteService.DeleteSupplierNote((int)SupplierNoteDomianModels[0].SupplierId, new List<DomModel.SupplierNote> { SupplierNoteDomianModels[0] }, true);

            mockSupplierNoteRepo.Verify(m => m.Delete(It.IsAny<DbModel.SupplierNote>()), Times.AtLeast(1));
        }
        [TestMethod]
        public void ThrowsExceptionWhileSavingNote()
        {

            mockSupplierRepo.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Supplier, bool>>>())).Returns(mockSupplierDbData);

            Validation = new Evolution.ValidationService.Services.ValidationService();
            validationService = new Evolution.Supplier.Infrastructure.Validations.SupplierNoteValidationService(Validation);
            SupplierNoteService = new SupplierService.SupplierNoteService(Mapper.Instance, mockLogger.Object, mockSupplierNoteRepo.Object, mockSupplierRepo.Object, validationService);

            SupplierNoteDomianModels.ToList().ForEach(x => x.RecordStatus = "N");

            var response = SupplierNoteService.SaveSupplierNote((int)SupplierNoteDomianModels[0].SupplierId, new List<DomModel.SupplierNote> { SupplierNoteDomianModels[0] }, true,true);

            mockSupplierNoteRepo.Verify(m => m.Add(It.IsAny<IList<DbModel.SupplierNote>>()), Times.AtLeastOnce);
        }
        [TestMethod]
        public void ThrowsExceptionWhileDeletingNote()
        {
            mockSupplierRepo.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Supplier, bool>>>())).Throws(new Exception("Exception occured while performing some operation."));

            Validation = new Evolution.ValidationService.Services.ValidationService();
            validationService = new Evolution.Supplier.Infrastructure.Validations.SupplierNoteValidationService(Validation);
            SupplierNoteService = new SupplierService.SupplierNoteService(Mapper.Instance, mockLogger.Object, mockSupplierNoteRepo.Object, mockSupplierRepo.Object, validationService);
            SupplierNoteDomianModels.ToList().ForEach(x => x.RecordStatus = "D");
            var response = SupplierNoteService.DeleteSupplierNote((int)SupplierNoteDomianModels[0].SupplierId, new List<DomModel.SupplierNote> { SupplierNoteDomianModels[0] }, true,true);
            Assert.AreEqual("11", response.Code);
            Assert.IsNotNull(response.Messages);
            Assert.AreEqual("31", response.Messages.FirstOrDefault().Code);

        }



        [TestMethod]
        public void ShouldNotSaveDataWhenSupplierIsInvalid()
        {

            Validation = new Evolution.ValidationService.Services.ValidationService();
            validationService = new Evolution.Supplier.Infrastructure.Validations.SupplierNoteValidationService(Validation);
            SupplierNoteService = new SupplierService.SupplierNoteService(Mapper.Instance, mockLogger.Object, mockSupplierNoteRepo.Object, mockSupplierRepo.Object, validationService);

            SupplierNoteDomianModels.ToList().ForEach(x => x.RecordStatus = "N");
            var response = SupplierNoteService.SaveSupplierNote((int)SupplierNoteDomianModels[0].SupplierId, new List<DomModel.SupplierNote> { SupplierNoteDomianModels[0] }, true);
            Assert.AreEqual("11", response.Code);

        }

        [TestMethod]
        public void GetAllSavedNotes()
        {
            mockSupplierRepo.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Supplier, bool>>>())).Returns(mockSupplierDbData);

            Validation = new Evolution.ValidationService.Services.ValidationService();
            validationService = new Evolution.Supplier.Infrastructure.Validations.SupplierNoteValidationService(Validation);
            SupplierNoteService = new SupplierService.SupplierNoteService(Mapper.Instance, mockLogger.Object, mockSupplierNoteRepo.Object, mockSupplierRepo.Object, validationService);

            SupplierNoteDomianModels.ToList().ForEach(x => x.RecordStatus = "N");

            var response = SupplierNoteService.SaveSupplierNote((int)SupplierNoteDomianModels[0].SupplierId, new List<DomModel.SupplierNote> { SupplierNoteDomianModels[0] }, true,true);

            mockSupplierNoteRepo.Verify(m => m.Add(It.IsAny<IList<DbModel.SupplierNote>>()), Times.AtLeastOnce);
        }



        [TestMethod]
        public void  GetAllSupplierNoteAfterDeletingSupplier()
        {
            mockSupplierRepo.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Supplier, bool>>>())).Returns(mockSupplierDbData);
            mockSupplierNoteRepo.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.SupplierNote, bool>>>())).Returns((Expression<Func<DbModel.SupplierNote, bool>> predicate) => mockSupplierNoteDbData.Where(predicate));
            Validation = new Evolution.ValidationService.Services.ValidationService();
            validationService = new Evolution.Supplier.Infrastructure.Validations.SupplierNoteValidationService(Validation);
            SupplierNoteService = new SupplierService.SupplierNoteService(Mapper.Instance, mockLogger.Object, mockSupplierNoteRepo.Object, mockSupplierRepo.Object, validationService);
            SupplierNoteDomianModels.ToList().ForEach(x => x.RecordStatus = "D");
            var response = SupplierNoteService.DeleteSupplierNote((int)SupplierNoteDomianModels[0].SupplierId, new List<DomModel.SupplierNote> { SupplierNoteDomianModels[0] }, true,true);

            mockSupplierNoteRepo.Verify(m => m.Delete(It.IsAny<DbModel.SupplierNote>()), Times.AtLeast(1));
        }
    }
   

}
