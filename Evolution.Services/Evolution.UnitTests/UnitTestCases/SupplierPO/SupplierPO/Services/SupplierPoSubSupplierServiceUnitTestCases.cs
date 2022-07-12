using AutoMapper;
using Evolution.Logging.Interfaces;
using Evolution.Supplier.Domain.Interfaces.Data;
using Evolution.SupplierPO.Core.Services;
using Evolution.SupplierPO.Domain.Interfaces.Data;
using Evolution.SupplierPO.Domain.Interfaces.SupplierPO;
using Evolution.SupplierPO.Domain.Interfaces.Validations;
using Evolution.SupplierPO.Infrastructure.Validations;
using Evolution.UnitTest.UnitTestCases;
using Evolution.UnitTests.Mocks.Data.Supplier.Db;
using Evolution.UnitTests.Mocks.Data.SupplierPO.Db;
using Evolution.UnitTests.Mocks.Data.SupplierPO.Domain;
using Evolution.ValidationService.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using DbModel = Evolution.DbRepository.Models;
using DomainModel = Evolution.SupplierPO.Domain.Models.SupplierPO;

namespace Evolution.UnitTests.UnitTestCases.SupplierPO.Services
{
    [TestClass]
    public class SupplierPoSubSupplierServiceUnitTestCases : BaseTestCase
    {
        Mock<ISupplierPOSubSupplierRepository> mockSubSupplierRepository = null;
        Mock<ISupplierPORepository> mockSupplierPORepository = null;
        Mock<ISupplierRepository> mockSupplierRepository = null;
        Mock<IAppLogger<SupplierPOSubSupplierService>> mockLogger = null;
        ISupplierPoSubSupplierValidationService subSupplierValidationService = null;
        IValidationService validationService = null;
        IList<DomainModel.SupplierPOSubSupplier> mockSubSupplierDomainData = null;
        ISupplierPOSubSupplierService _service = null;
        IQueryable<DbModel.SupplierPurchaseOrder> mockSupplierPODbData = null;
        IQueryable<DbModel.Supplier> mockSupplierDbData = null;
        IQueryable<DbModel.SupplierPurchaseOrderSubSupplier> mockSubSupplierDbData = null;

        [TestInitialize]
        public void InitializeSubSupplierTests()
        {
            mockSubSupplierRepository = new Mock<ISupplierPOSubSupplierRepository>();
            mockSupplierPORepository = new Mock<ISupplierPORepository>();
            mockSupplierRepository = new Mock<ISupplierRepository>();
            mockLogger = new Mock<IAppLogger<SupplierPOSubSupplierService>>();
            validationService = new ValidationService.Services.ValidationService();
            subSupplierValidationService = new SupplierPoSubSupplierValidationService(validationService);
            mockSubSupplierDomainData = MockDomainSupplierPO.GetSubSupplierMockedData();
            mockSupplierPODbData = MockDbSupplierPO.GetSupplierPOMockDbData();
            mockSupplierDbData = MockSupplier.GetSupplierMockedData();
            mockSubSupplierDbData = MockDbSupplierPO.GetSubSupplierMockData();
            _service = new Evolution.SupplierPO.Core.Services.SupplierPOSubSupplierService(mockSubSupplierRepository.Object, mockSupplierRepository.Object, mockLogger.Object, Mapper.Instance, mockSupplierPORepository.Object, subSupplierValidationService);
        }

        #region GetSubSuppliers

        [TestMethod]
        public void GetAllSubSuppliers()
        {
            mockSubSupplierRepository.Setup(x => x.Search(It.IsAny<int>(), It.IsAny<DomainModel.SupplierPOSubSupplier>())).Returns(mockSubSupplierDomainData);
            var result = _service.Get(mockSubSupplierDomainData[0].SupplierPOId, mockSubSupplierDomainData[0]);
            Assert.AreEqual("1", result.Code);
            Assert.AreEqual(2, (result.Result as IList<DomainModel.SupplierPOSubSupplier>).Count);
        }

        [TestMethod]
        public void GetAllSubSuppliersBySupplierPONumber()
        {
            mockSubSupplierRepository.Setup(x => x.Search(It.IsAny<int>(), It.IsAny<DomainModel.SupplierPOSubSupplier>())).Returns(mockSubSupplierDomainData);
            var result = _service.Get(mockSubSupplierDomainData[0].SupplierPOId, new DomainModel.SupplierPOSubSupplier() { SupplierPONumber = "New Supplier PO Number " });
            Assert.AreEqual("1", result.Code);
            Assert.AreEqual(2, (result.Result as IList<DomainModel.SupplierPOSubSupplier>).Count);
        }

        [TestMethod]
        public void GetAllSubSuppliersBySupplierPONumberWildCardSearch()
        {
            mockSubSupplierRepository.Setup(x => x.Search(It.IsAny<int>(), It.IsAny<DomainModel.SupplierPOSubSupplier>())).Returns(mockSubSupplierDomainData);
            var result = _service.Get(mockSubSupplierDomainData[0].SupplierPOId, new DomainModel.SupplierPOSubSupplier() { SupplierPONumber = "New*" });
            Assert.AreEqual("1", result.Code);
            Assert.AreEqual(2, (result.Result as IList<DomainModel.SupplierPOSubSupplier>).Count);
        }

        [TestMethod]
        public void GetAllSubSuppliersWithNoSearchvalue()
        {
            mockSubSupplierRepository.Setup(x => x.Search(It.IsAny<int>(), It.IsAny<DomainModel.SupplierPOSubSupplier>())).Returns(mockSubSupplierDomainData);
            var result = _service.Get(mockSubSupplierDomainData[0].SupplierPOId, null);
            Assert.AreEqual("1", result.Code);
            Assert.AreEqual(2, (result.Result as IList<DomainModel.SupplierPOSubSupplier>).Count);
        }

        [TestMethod]
        public void GetAllSubSuppliersThrowsException()
        {
            mockSubSupplierRepository.Setup(x => x.Search(It.IsAny<int>(), It.IsAny<DomainModel.SupplierPOSubSupplier>())).Throws(new Exception());
            var result = _service.Get(mockSubSupplierDomainData[0].SupplierPOId, null);
            Assert.AreEqual("11", result.Code);
            Assert.AreEqual("31",result.Messages.FirstOrDefault().Code);
        }

        #endregion

        #region AddSubSuppliers

        [TestMethod]
        public void AddSubSuppliersWithValidData()
        {
            mockSupplierPORepository.Setup(x => x.FindBy(It.IsAny<Expression<Func<DbModel.SupplierPurchaseOrder, bool>>>())).Returns(mockSupplierPODbData);
            mockSubSupplierDomainData.ToList().ForEach(x => x.RecordStatus = "N");
            mockSupplierRepository.Setup(x => x.GetAll()).Returns(mockSupplierDbData);
            var result = _service.SaveSubSupplier(mockSubSupplierDomainData[0].SupplierId, mockSubSupplierDomainData);
            mockSubSupplierRepository.Verify(x => x.Add(It.IsAny<IList<DbModel.SupplierPurchaseOrderSubSupplier>>()), Times.AtLeastOnce);
        }

        [TestMethod]
        public void AddSubSuppliersWithInValidJsonFormat()
        {
            mockSupplierPORepository.Setup(x => x.FindBy(It.IsAny<Expression<Func<DbModel.SupplierPurchaseOrder, bool>>>())).Returns(mockSupplierPODbData);
            mockSubSupplierDomainData.ToList().ForEach(x => { x.RecordStatus = "N"; x.SubSupplierName = null; });
            mockSupplierRepository.Setup(x => x.GetAll()).Returns(mockSupplierDbData);
            var result = _service.SaveSubSupplier(mockSubSupplierDomainData[0].SupplierId, mockSubSupplierDomainData);
            Assert.AreEqual("41", result.Code);
        }

        [TestMethod]
        public void AddSubSuppliersWithInValidSupplierPO()
        {
            mockSupplierPORepository.Setup(x => x.FindBy(It.IsAny<Expression<Func<DbModel.SupplierPurchaseOrder, bool>>>())).Returns<DbModel.SupplierPurchaseOrder>(null);
            mockSubSupplierDomainData.ToList().ForEach(x => { x.RecordStatus = "N"; });
            mockSupplierRepository.Setup(x => x.GetAll()).Returns(mockSupplierDbData);
            var result = _service.SaveSubSupplier(mockSubSupplierDomainData[0].SupplierId, mockSubSupplierDomainData);
            Assert.AreEqual("11", result.Code);
        }

        [TestMethod]
        public void AddSubSuppliersWithInValidSupplier()
        {
            mockSupplierPORepository.Setup(x => x.FindBy(It.IsAny<Expression<Func<DbModel.SupplierPurchaseOrder, bool>>>())).Returns(mockSupplierPODbData);
            mockSubSupplierDomainData.ToList().ForEach(x => { x.RecordStatus = "N"; });
            mockSupplierRepository.Setup(x => x.GetAll()).Returns<DbModel.SupplierPurchaseOrder>(null);
            var result = _service.SaveSubSupplier(mockSubSupplierDomainData[0].SupplierId, mockSubSupplierDomainData);
            Assert.AreEqual("11", result.Code);
        }

        [TestMethod]
        public void AddSubSuppliersWithInValidRecordStatus()
        {
            mockSupplierPORepository.Setup(x => x.FindBy(It.IsAny<Expression<Func<DbModel.SupplierPurchaseOrder, bool>>>())).Returns(mockSupplierPODbData);
            mockSubSupplierDomainData.ToList().ForEach(x => { x.RecordStatus = "M"; });
            mockSupplierRepository.Setup(x => x.GetAll()).Returns(mockSupplierDbData);
            var result = _service.SaveSubSupplier(mockSubSupplierDomainData[0].SupplierId, mockSubSupplierDomainData);
            Assert.AreEqual("1", result.Code);
        }
        
        [TestMethod]
        public void AddSubSuppliersWithExceptionResult()
        {
            mockSupplierPORepository.Setup(x => x.FindBy(It.IsAny<Expression<Func<DbModel.SupplierPurchaseOrder, bool>>>())).Returns(mockSupplierPODbData);
            mockSubSupplierDomainData.ToList().ForEach(x => { x.RecordStatus = "N"; });
            mockSupplierRepository.Setup(x => x.GetAll()).Returns(mockSupplierDbData);
            mockSubSupplierRepository.Setup(x => x.Add(It.IsAny<IList<DbModel.SupplierPurchaseOrderSubSupplier>>())).Throws(new Exception());
            var result = _service.SaveSubSupplier(mockSubSupplierDomainData[0].SupplierId, mockSubSupplierDomainData);
            Assert.AreEqual("11", result.Code);
        }

        #endregion

        #region UpdateSubSuppliers

        [TestMethod]
        public void UpdateSubSuplierWithValidData()
        {
            mockSupplierPODbData.ToList().ForEach(x => x.SupplierPurchaseOrderSubSupplier = mockSubSupplierDbData.ToList());
            mockSupplierPORepository.Setup(x => x.FindBy(It.IsAny<Expression<Func<DbModel.SupplierPurchaseOrder, bool>>>())).Returns(mockSupplierPODbData);
            mockSubSupplierDomainData.ToList().ForEach(x => { x.RecordStatus = "M"; });
            mockSupplierRepository.Setup(x => x.GetAll()).Returns(mockSupplierDbData);
            var result = _service.ModifySubSupplier(mockSubSupplierDomainData[0].SupplierId, mockSubSupplierDomainData);
            mockSubSupplierRepository.Verify(x => x.Update(It.IsAny<DbModel.SupplierPurchaseOrderSubSupplier>()), Times.AtLeastOnce);
        }

        [TestMethod]
        public void UpdateSubSuppliersWithInValidJsonFormat()
        {
            mockSupplierPORepository.Setup(x => x.FindBy(It.IsAny<Expression<Func<DbModel.SupplierPurchaseOrder, bool>>>())).Returns(mockSupplierPODbData);
            mockSubSupplierDomainData.ToList().ForEach(x => { x.RecordStatus = "M"; x.SubSupplierName = null; });
            mockSupplierRepository.Setup(x => x.GetAll()).Returns(mockSupplierDbData);
            var result = _service.ModifySubSupplier(mockSubSupplierDomainData[0].SupplierId, mockSubSupplierDomainData);
            Assert.AreEqual("41", result.Code);
        }

        [TestMethod]
        public void UpdateSubSuppliersWithInValidSupplierPO()
        {
            mockSupplierPORepository.Setup(x => x.FindBy(It.IsAny<Expression<Func<DbModel.SupplierPurchaseOrder, bool>>>())).Returns<DbModel.SupplierPurchaseOrder>(null);
            mockSubSupplierDomainData.ToList().ForEach(x => { x.RecordStatus = "M"; });
            mockSupplierRepository.Setup(x => x.GetAll()).Returns(mockSupplierDbData);
            var result = _service.ModifySubSupplier(mockSubSupplierDomainData[0].SupplierId, mockSubSupplierDomainData);
            Assert.AreEqual("11", result.Code);
        }

        [TestMethod]
        public void UpdateSubSuppliersWithInValidSupplier()
        {
            mockSupplierPORepository.Setup(x => x.FindBy(It.IsAny<Expression<Func<DbModel.SupplierPurchaseOrder, bool>>>())).Returns(mockSupplierPODbData);
            mockSubSupplierDomainData.ToList().ForEach(x => { x.RecordStatus = "M"; });
            mockSupplierRepository.Setup(x => x.GetAll()).Returns<DbModel.SupplierPurchaseOrder>(null);
            var result = _service.ModifySubSupplier(mockSubSupplierDomainData[0].SupplierId, mockSubSupplierDomainData);
            Assert.AreEqual("11", result.Code);
        }

        [TestMethod]
        public void UpdateSubSuppliersWithInValidRecordStatus()
        {
            mockSupplierPORepository.Setup(x => x.FindBy(It.IsAny<Expression<Func<DbModel.SupplierPurchaseOrder, bool>>>())).Returns(mockSupplierPODbData);
            mockSubSupplierDomainData.ToList().ForEach(x => { x.RecordStatus = "N"; });
            mockSupplierRepository.Setup(x => x.GetAll()).Returns(mockSupplierDbData);
            var result = _service.ModifySubSupplier(mockSubSupplierDomainData[0].SupplierId, mockSubSupplierDomainData);
            Assert.AreEqual("1", result.Code);
        }

        [TestMethod]
        public void UpdateSubSuppliersWithExceptionResult()
        {
            mockSupplierPORepository.Setup(x => x.FindBy(It.IsAny<Expression<Func<DbModel.SupplierPurchaseOrder, bool>>>())).Returns(mockSupplierPODbData);
            mockSubSupplierDomainData.ToList().ForEach(x => { x.RecordStatus = "M"; });
            mockSupplierRepository.Setup(x => x.GetAll()).Returns(mockSupplierDbData);
            mockSubSupplierRepository.Setup(x => x.Update(It.IsAny<DbModel.SupplierPurchaseOrderSubSupplier>())).Throws(new Exception());
            var result = _service.ModifySubSupplier(mockSubSupplierDomainData[0].SupplierId, mockSubSupplierDomainData);
            Assert.AreEqual("11", result.Code);
        }

        [TestMethod]
        public void UpdateSubSupplierWithNotAssociatedSubSupplier()
        {
            mockSupplierPORepository.Setup(x => x.FindBy(It.IsAny<Expression<Func<DbModel.SupplierPurchaseOrder, bool>>>())).Returns(mockSupplierPODbData);
            mockSubSupplierDomainData.ToList().ForEach(x => { x.RecordStatus = "M"; });
            mockSupplierRepository.Setup(x => x.GetAll()).Returns(mockSupplierDbData);
            var result = _service.ModifySubSupplier(mockSubSupplierDomainData[0].SupplierId, mockSubSupplierDomainData);
            Assert.AreEqual("11", result.Code);
        }

        [TestMethod]
        public void UpdateSubSupplierWithWrongUpdateCount()
        {
            mockSupplierPORepository.Setup(x => x.FindBy(It.IsAny<Expression<Func<DbModel.SupplierPurchaseOrder, bool>>>())).Returns(mockSupplierPODbData);
            mockSubSupplierDomainData.ToList().ForEach(x => { x.RecordStatus = "M";x.UpdateCount = 5; });
            mockSupplierRepository.Setup(x => x.GetAll()).Returns(mockSupplierDbData);
            var result = _service.ModifySubSupplier(mockSubSupplierDomainData[0].SupplierId, mockSubSupplierDomainData);
            Assert.AreEqual("11", result.Code);
        }
        #endregion

        #region DeleteSubSuppliers

        [TestMethod]
        public void DeleteSubSuplierWithValidData()
        {
            mockSupplierPODbData.ToList().ForEach(x => x.SupplierPurchaseOrderSubSupplier = mockSubSupplierDbData.ToList());
            mockSupplierPORepository.Setup(x => x.FindBy(It.IsAny<Expression<Func<DbModel.SupplierPurchaseOrder, bool>>>())).Returns(mockSupplierPODbData);
            mockSubSupplierDomainData.ToList().ForEach(x => { x.RecordStatus = "D"; });
            mockSupplierRepository.Setup(x => x.GetAll()).Returns(mockSupplierDbData);
            var result = _service.DeleteSubSupplier(mockSubSupplierDomainData[0].SupplierId, mockSubSupplierDomainData);
            mockSubSupplierRepository.Verify(x => x.Delete(It.IsAny<DbModel.SupplierPurchaseOrderSubSupplier>()), Times.AtLeastOnce);
        }

        [TestMethod]
        public void DeleteSubSuppliersWithInValidJsonFormat()
        {
            mockSupplierPORepository.Setup(x => x.FindBy(It.IsAny<Expression<Func<DbModel.SupplierPurchaseOrder, bool>>>())).Returns(mockSupplierPODbData);
            mockSubSupplierDomainData.ToList().ForEach(x => { x.RecordStatus = "D"; x.SubSupplierName = null; });
            mockSupplierRepository.Setup(x => x.GetAll()).Returns(mockSupplierDbData);
            var result = _service.DeleteSubSupplier(mockSubSupplierDomainData[0].SupplierId, mockSubSupplierDomainData);
            Assert.AreEqual("41", result.Code);
        }

        [TestMethod]
        public void DeleteSubSuppliersWithInValidSupplierPO()
        {
            mockSupplierPORepository.Setup(x => x.FindBy(It.IsAny<Expression<Func<DbModel.SupplierPurchaseOrder, bool>>>())).Returns<DbModel.SupplierPurchaseOrder>(null);
            mockSubSupplierDomainData.ToList().ForEach(x => { x.RecordStatus = "D"; });
            mockSupplierRepository.Setup(x => x.GetAll()).Returns(mockSupplierDbData);
            var result = _service.DeleteSubSupplier(mockSubSupplierDomainData[0].SupplierId, mockSubSupplierDomainData);
            Assert.AreEqual("11", result.Code);
        }

        [TestMethod]
        public void DeleteSubSuppliersWithInValidSupplier()
        {
            mockSupplierPORepository.Setup(x => x.FindBy(It.IsAny<Expression<Func<DbModel.SupplierPurchaseOrder, bool>>>())).Returns(mockSupplierPODbData);
            mockSubSupplierDomainData.ToList().ForEach(x => { x.RecordStatus = "D"; });
            mockSupplierRepository.Setup(x => x.GetAll()).Returns<DbModel.SupplierPurchaseOrder>(null);
            var result = _service.DeleteSubSupplier(mockSubSupplierDomainData[0].SupplierId, mockSubSupplierDomainData);
            Assert.AreEqual("11", result.Code);
        }

        [TestMethod]
        public void DeleteSubSuppliersWithInValidRecordStatus()
        {
            mockSupplierPORepository.Setup(x => x.FindBy(It.IsAny<Expression<Func<DbModel.SupplierPurchaseOrder, bool>>>())).Returns(mockSupplierPODbData);
            mockSubSupplierDomainData.ToList().ForEach(x => { x.RecordStatus = "N"; });
            mockSupplierRepository.Setup(x => x.GetAll()).Returns(mockSupplierDbData);
            var result = _service.DeleteSubSupplier(mockSubSupplierDomainData[0].SupplierId, mockSubSupplierDomainData);
            Assert.AreEqual("1", result.Code);
        }

        [TestMethod]
        public void DeleteSubSuppliersWithExceptionResult()
        {
            mockSupplierPORepository.Setup(x => x.FindBy(It.IsAny<Expression<Func<DbModel.SupplierPurchaseOrder, bool>>>())).Returns(mockSupplierPODbData);
            mockSubSupplierDomainData.ToList().ForEach(x => { x.RecordStatus = "D"; });
            mockSupplierRepository.Setup(x => x.GetAll()).Returns(mockSupplierDbData);
            mockSubSupplierRepository.Setup(x => x.Update(It.IsAny<DbModel.SupplierPurchaseOrderSubSupplier>())).Throws(new Exception());
            var result = _service.DeleteSubSupplier(mockSubSupplierDomainData[0].SupplierId, mockSubSupplierDomainData);
            Assert.AreEqual("11", result.Code);
        }

        [TestMethod]
        public void DeleteSubSupplierWithNotAssociatedSubSupplier()
        {
            mockSupplierPORepository.Setup(x => x.FindBy(It.IsAny<Expression<Func<DbModel.SupplierPurchaseOrder, bool>>>())).Returns(mockSupplierPODbData);
            mockSubSupplierDomainData.ToList().ForEach(x => { x.RecordStatus = "D"; });
            mockSupplierRepository.Setup(x => x.GetAll()).Returns(mockSupplierDbData);
            var result = _service.DeleteSubSupplier(mockSubSupplierDomainData[0].SupplierId, mockSubSupplierDomainData);
            Assert.AreEqual("11", result.Code);
        }

        #endregion
    }
}
