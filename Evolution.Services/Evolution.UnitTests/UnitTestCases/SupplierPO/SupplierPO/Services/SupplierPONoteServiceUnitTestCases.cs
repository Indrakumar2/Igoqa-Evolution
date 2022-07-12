using AutoMapper;
using Evolution.Logging.Interfaces;
using Evolution.SupplierPO.Core.Services;
using Evolution.SupplierPO.Domain.Interfaces.Data;
using Evolution.SupplierPO.Domain.Interfaces.SupplierPO;
using Evolution.SupplierPO.Domain.Interfaces.Validations;
using Evolution.SupplierPO.Infrastructure.Validations;
using Evolution.UnitTest.UnitTestCases;
using Evolution.UnitTests.Mocks.Data.SupplierPO.Domain;
using Evolution.ValidationService.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using DomainModel = Evolution.SupplierPO.Domain.Models.SupplierPO;
using DbModel = Evolution.DbRepository.Models;
using Evolution.UnitTests.Mocks.Data.SupplierPO.Db;

namespace Evolution.UnitTests.UnitTestCases.SupplierPO.Services
{
    [TestClass]
    public class SupplierPONoteServiceUnitTestCases : BaseTestCase
    {
        Mock<ISupplierPONoteRepository> mockNoteRepository = null;
        Mock<ISupplierPORepository> mockSupplierPORepository = null;
        Mock<IAppLogger<SupplierPONoteService>> mockLogger = null;
        IList<DomainModel.SupplierPONote> mockNoteData = null;
        ISupplierPONoteService _service = null;
        ISupplierPONoteValidationService _noteValidationService = null;
        IValidationService _validationService = null;
        IQueryable<DbModel.SupplierPurchaseOrder> mockDbSupplierPOData = null;
        [TestInitialize]
        public void InitializeNoteRepository()
        {
            mockSupplierPORepository = new Mock<ISupplierPORepository>();
            mockLogger = new Mock<IAppLogger<SupplierPONoteService>>();
            mockNoteRepository = new Mock<ISupplierPONoteRepository>();
            _validationService = new ValidationService.Services.ValidationService();
            _noteValidationService = new SuppilerPoNoteValidationService(_validationService);
            mockNoteData = MockDomainSupplierPO.GetSupplierPONotesMockedData();
            mockDbSupplierPOData = MockDbSupplierPO.GetSupplierPOMockDbData();
            _service = new SupplierPONoteService(Mapper.Instance, mockLogger.Object, mockNoteRepository.Object, mockSupplierPORepository.Object,_noteValidationService);
        }

        #region Get

        [TestMethod]
        public void FetchAllSupplierPONotes()
        {
            mockNoteRepository.Setup(x => x.Search(It.IsAny<int>(), It.IsAny<DomainModel.SupplierPONote>())).Returns(mockNoteData);
            var result = _service.GetSupplierPONote(1, new DomainModel.SupplierPONote());
            Assert.AreEqual("1", result.Code);
            Assert.AreEqual(2, (result.Result as List<DomainModel.SupplierPONote>).Count);
        }

        [TestMethod]
        public void FetchSupplierPoNotesBasedONSupplierPONumber()
        {
            mockNoteRepository.Setup(x => x.Search(It.IsAny<int>(), It.IsAny<DomainModel.SupplierPONote>())).Returns(mockNoteData);
            var result = _service.GetSupplierPONote(1, new DomainModel.SupplierPONote() { SupplierPONumber= "New Supplier PO Number "});
            Assert.AreEqual("1", result.Code);
            Assert.AreEqual(2, (result.Result as List<DomainModel.SupplierPONote>).Count);
        }


        [TestMethod]
        public void FetchSupplierPoNoteWithSupplierPONumberWildCardSearch()
        {
            mockNoteRepository.Setup(x => x.Search(It.IsAny<int>(), It.IsAny<DomainModel.SupplierPONote>())).Returns(mockNoteData);
            var result = _service.GetSupplierPONote(1, new DomainModel.SupplierPONote() { SupplierPONumber = "New*" });
            Assert.AreEqual("1", result.Code);
            Assert.AreEqual(2, (result.Result as List<DomainModel.SupplierPONote>).Count);
        }

        [TestMethod]
        public void FetchSupplierPoNoteWithNoSearchValue()
        {
            mockNoteRepository.Setup(x => x.Search(It.IsAny<int>(), It.IsAny<DomainModel.SupplierPONote>())).Returns(mockNoteData);
            var result = _service.GetSupplierPONote(1,null);
            Assert.AreEqual("1", result.Code);
            Assert.AreEqual(2, (result.Result as List<DomainModel.SupplierPONote>).Count);
        }

        [TestMethod]
        public void FetchSupplierPOWithExceptionResult()
        {
            mockNoteRepository.Setup(x => x.Search(It.IsAny<int>(), It.IsAny<DomainModel.SupplierPONote>())).Throws(new System.Exception());
            var result = _service.GetSupplierPONote(1, null);
            Assert.AreEqual("11", result.Code);
            Assert.AreEqual("31", (result.Messages.FirstOrDefault().Code));
        }

        #endregion

        #region  AddSupplierPoNotes
        
        [TestMethod]
        public void AddSupplierPoNotesWithValidData()
        {
            mockSupplierPORepository.Setup(x => x.FindBy(It.IsAny<Expression<Func<DbModel.SupplierPurchaseOrder, bool>>>())).Returns(mockDbSupplierPOData);
            mockNoteData.ToList().ForEach(x => x.RecordStatus = "N");
            var result = _service.SaveSupplierPONote(mockNoteData[0].SupplierPOId, mockNoteData,true);
            mockNoteRepository.Verify(x => x.Add(It.IsAny<IList<DbModel.SupplierPurchaseOrderNote>>()), Times.AtLeastOnce);
        }

        [TestMethod]
        public void AddSupplierPONotesWithInvalidJsonFormat()
        {
            mockSupplierPORepository.Setup(x => x.FindBy(It.IsAny<Expression<Func<DbModel.SupplierPurchaseOrder, bool>>>())).Returns(mockDbSupplierPOData);
            mockNoteData.ToList().ForEach(x => { x.RecordStatus = "N"; x.Notes = null; });
            var result = _service.SaveSupplierPONote(mockNoteData[0].SupplierPOId, mockNoteData, true);
            Assert.AreEqual("41", result.Code);
            
        }

        [TestMethod]
        public void AddSupplierPONotesWithInvalidSupplierPONumber()
        {
            mockSupplierPORepository.Setup(x => x.FindBy(It.IsAny<Expression<Func<DbModel.SupplierPurchaseOrder, bool>>>())).Returns<DbModel.SupplierPurchaseOrder>(null);
            mockNoteData.ToList().ForEach(x => { x.RecordStatus = "N"; });
            var result = _service.SaveSupplierPONote(mockNoteData[0].SupplierPOId, mockNoteData, true);
            Assert.IsTrue( result.Messages.Count>0);
        }
        
        [TestMethod]
        public void AddSupplierPONotesWithExceptionResult()
        {
            mockSupplierPORepository.Setup(x => x.FindBy(It.IsAny<Expression<Func<DbModel.SupplierPurchaseOrder, bool>>>())).Throws(new Exception());
            mockNoteData.ToList().ForEach(x => { x.RecordStatus = "N"; });
            var result = _service.SaveSupplierPONote(mockNoteData[0].SupplierPOId, mockNoteData, true);
            Assert.AreEqual("11",result.Code);
        }

        [TestMethod]
        public void AddSupplierPONotesWithInvalidRecordStatus()
        {
            mockSupplierPORepository.Setup(x => x.FindBy(It.IsAny<Expression<Func<DbModel.SupplierPurchaseOrder, bool>>>())).Throws(new Exception());
            mockNoteData.ToList().ForEach(x => { x.RecordStatus = "M"; });
            var result = _service.SaveSupplierPONote(mockNoteData[0].SupplierPOId, mockNoteData, true);
            Assert.AreEqual("1", result.Code);
        }

        #endregion

        #region DeleteSupplierPONotes

        [TestMethod]
        public void RemoveSupplierPONotesWithValidData()
        {
            var mockDbNoteData = MockDbSupplierPO.GetSupplierPONotesMockData();
            mockNoteRepository.Setup(x => x.FindBy(It.IsAny<Expression<Func<DbModel.SupplierPurchaseOrderNote, bool>>>())).Returns(mockDbNoteData);
            mockSupplierPORepository.Setup(x => x.FindBy(It.IsAny<Expression<Func<DbModel.SupplierPurchaseOrder, bool>>>())).Returns(mockDbSupplierPOData);
            mockNoteData.ToList().ForEach(x => x.RecordStatus = "D");
            var result = _service.DeleteSupplierPONote(mockNoteData[0].SupplierPOId, mockNoteData, true);
            mockNoteRepository.Verify(x => x.Delete(It.IsAny<DbModel.SupplierPurchaseOrderNote>()), Times.AtLeastOnce);
        }


        [TestMethod]
        public void RemoveSupplierPONotesWithInvalidRecordStatus()
        {
            mockSupplierPORepository.Setup(x => x.FindBy(It.IsAny<Expression<Func<DbModel.SupplierPurchaseOrder, bool>>>())).Throws(new Exception());
            mockNoteData.ToList().ForEach(x => { x.RecordStatus = "M"; });
            var result = _service.DeleteSupplierPONote(mockNoteData[0].SupplierPOId, mockNoteData, true);
            Assert.AreEqual("1", result.Code);
        }


        [TestMethod]
        public void RemoveSupplierPONotesWithExceptionResult()
        {
            mockSupplierPORepository.Setup(x => x.FindBy(It.IsAny<Expression<Func<DbModel.SupplierPurchaseOrder, bool>>>())).Throws(new Exception());
            mockNoteData.ToList().ForEach(x => { x.RecordStatus = "D"; });
            var result = _service.DeleteSupplierPONote(mockNoteData[0].SupplierPOId, mockNoteData, true);
            Assert.AreEqual("11", result.Code);
        }

        [TestMethod]
        public void RemoveSupplierPONotesWithInvalidSupplierPONumber()
        {
            mockNoteRepository.Setup(x => x.FindBy(It.IsAny<Expression<Func<DbModel.SupplierPurchaseOrderNote, bool>>>())).Returns<DbModel.SupplierPurchaseOrderNote>(null);
            mockSupplierPORepository.Setup(x => x.FindBy(It.IsAny<Expression<Func<DbModel.SupplierPurchaseOrder, bool>>>())).Returns<DbModel.SupplierPurchaseOrder>(null);
            mockNoteData.ToList().ForEach(x => { x.RecordStatus = "D"; });
            var result = _service.DeleteSupplierPONote(mockNoteData[0].SupplierPOId, mockNoteData, true);
            Assert.IsTrue(result.Messages.Count > 0);
        }


        [TestMethod]
        public void RemoveSupplierPONotesWithInvalidNote()
        {
            mockSupplierPORepository.Setup(x => x.FindBy(It.IsAny<Expression<Func<DbModel.SupplierPurchaseOrder, bool>>>())).Returns(mockDbSupplierPOData);
            mockNoteData.ToList().ForEach(x => { x.RecordStatus = "D"; });
            var result = _service.DeleteSupplierPONote(mockNoteData[0].SupplierPOId, mockNoteData, true);
            Assert.AreEqual("1", result.Code);
        }


        [TestMethod]
        public void RemoveSupplierPONotesWithInvalidJsonFormat()
        {
            mockSupplierPORepository.Setup(x => x.FindBy(It.IsAny<Expression<Func<DbModel.SupplierPurchaseOrder, bool>>>())).Returns(mockDbSupplierPOData);
            mockNoteData.ToList().ForEach(x => { x.RecordStatus = "D"; x.Notes = null; });
            var result = _service.DeleteSupplierPONote(mockNoteData[0].SupplierPOId, mockNoteData, true);
            Assert.AreEqual("41", result.Code);

        }
        #endregion
    }

}