using AutoMapper;
using Evolution.Logging.Interfaces;
using Evolution.Project.Domain.Interfaces.Data;
using Evolution.Supplier.Domain.Interfaces.Data;
using Evolution.SupplierPO.Core.Services;
using Evolution.SupplierPO.Domain.Interfaces.Data;
using Evolution.SupplierPO.Domain.Interfaces.SupplierPO;
using Evolution.SupplierPO.Domain.Interfaces.Validations;
using Evolution.SupplierPO.Infrastructure.Validations;
using Evolution.UnitTest.UnitTestCases;
using Evolution.UnitTests.Mocks.Data.Projects.Db;
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
    public class SupplierPOServiceUnitTestCases : BaseTestCase
    {

        Mock<ISupplierPORepository> mockSupplierPORepository = null;
        Mock<IProjectRepository> mockProjectRepository = null;
        Mock<ISupplierRepository> mockSupplierRepository = null;
        Mock<IAppLogger<SupplierPOService>> mockLogger = null;
        ISupplierPoValidationService supplierPoValidationService = null;
        IValidationService validationService = null;
        IList<DomainModel.SupplierPO> mockSupplierPODomainData = null;
        ISupplierPOService _service = null;
        IQueryable<DbModel.Project> mockProjectDbData = null;
        IQueryable<DbModel.Supplier> mockSupplierDbData = null;
        IQueryable<DbModel.SupplierPurchaseOrder> mocksupplierPODbData = null;

        [TestInitialize]
        public void InitializeSupplierPOTests()
        {
            mockSupplierPORepository = new Mock<ISupplierPORepository>();
            mockProjectRepository = new Mock<IProjectRepository>();
            mockSupplierRepository = new Mock<ISupplierRepository>();
            mockLogger = new Mock<IAppLogger<SupplierPOService>>();
            validationService = new ValidationService.Services.ValidationService();
            supplierPoValidationService = new SupplierPOValidationService(validationService);
            mockSupplierPODomainData = MockDomainSupplierPO.GetSupplierPoMockedDomainData();
            mockProjectDbData = MockProject.GetprojectMockData();
            _service = new SupplierPOService(mockSupplierPORepository.Object, Mapper.Instance, mockLogger.Object, mockProjectRepository.Object, mockSupplierRepository.Object, supplierPoValidationService);
            mockSupplierDbData = MockSupplier.GetSupplierMockedData();
            mocksupplierPODbData = MockDbSupplierPO.GetSupplierPOMockDbData();
        }
        
        #region GetSupplierPO

        [TestMethod]
        public void GetAllSupplierPO()
        {
            mockSupplierPORepository.Setup(x => x.Search(It.IsAny<DomainModel.SupplierPO>())).Returns(mockSupplierPODomainData.ToList());
            var result = _service.Get(new DomainModel.SupplierPO());
            Assert.AreEqual("1", result.Code);
            Assert.AreEqual(2, (result.Result as IList<DomainModel.SupplierPO>).Count);
        }

        [TestMethod]
        public void GetAllSupplierPOByProjectNumber()
        {
            mockSupplierPORepository.Setup(x => x.Search(It.IsAny<DomainModel.SupplierPO>())).Returns(mockSupplierPODomainData.ToList());
            var result = _service.Get(new DomainModel.SupplierPO() {SupplierPOProjectNumber = 1 });
            Assert.AreEqual("1", result.Code);
            Assert.AreEqual(2, (result.Result as IList<DomainModel.SupplierPO>).Count);
        }

        [TestMethod]
        public void GetAllSupplierPOWithSupplierPONumberWildCardSearch()
        {
            mockSupplierPORepository.Setup(x => x.Search(It.IsAny<DomainModel.SupplierPO>())).Returns(mockSupplierPODomainData.ToList());
            var result = _service.Get(new DomainModel.SupplierPO() { SupplierPONumber = "New*"});
            Assert.AreEqual("1", result.Code);
            Assert.AreEqual(2, (result.Result as IList<DomainModel.SupplierPO>).Count);
        }

        [TestMethod]
        public void GetAllSupplierPOWithNullSearchValue()
        {
            mockSupplierPORepository.Setup(x => x.Search(It.IsAny<DomainModel.SupplierPO>())).Returns(mockSupplierPODomainData.ToList());
            var result = _service.Get(null);
            Assert.AreEqual("1", result.Code);
            Assert.AreEqual(2, (result.Result as IList<DomainModel.SupplierPO>).Count);
        }

        [TestMethod]
        public void GetAllSupplierPOWithExceptionResult()
        {
            mockSupplierPORepository.Setup(x => x.Search(It.IsAny<DomainModel.SupplierPO>())).Throws(new Exception());
            var result = _service.Get(null);
            Assert.AreEqual("11" , result.Code);
        }

        #endregion

        #region AddSupplierPO

        [TestMethod]
        public void AddSupplierPOValidData()
        {
            mockProjectRepository.Setup(x => x.GetAll()).Returns(mockProjectDbData);
            mockSupplierRepository.Setup(x => x.GetAll()).Returns(mockSupplierDbData);
            mockSupplierPODomainData.ToList().ForEach(x => x.RecordStatus = "N");
            var result = _service.SaveSupplierPurchaseOrders(mockSupplierPODomainData, true);
            mockSupplierPORepository.Verify(x => x.Add(It.IsAny<DbModel.SupplierPurchaseOrder>()), Times.AtLeastOnce);

        }

        [TestMethod]
        public void AddSupplierPOWithInvalidRecordStatus()
        {
            mockProjectRepository.Setup(x => x.GetAll()).Returns(mockProjectDbData);
            mockSupplierRepository.Setup(x => x.GetAll()).Returns(mockSupplierDbData);
            mockSupplierPODomainData.ToList().ForEach(x => x.RecordStatus = "D");
            var result = _service.SaveSupplierPurchaseOrders(mockSupplierPODomainData, true);
            Assert.AreEqual("1", result.Code);
        }

        [TestMethod]
        public void AddSupplierPOWithInvalidProjectNumber()
        {
            mockProjectRepository.Setup(x => x.GetAll()).Returns<IQueryable<DbModel.Project>>(null);
            mockSupplierRepository.Setup(x => x.GetAll()).Returns(mockSupplierDbData);
            mockSupplierPODomainData.ToList().ForEach(x => x.RecordStatus = "N");
            var result = _service.SaveSupplierPurchaseOrders(mockSupplierPODomainData, true);
            Assert.AreEqual("11", result.Code);
        }

        [TestMethod]
        public void AddSupplierPOWithInvalidSupplierName()
        {
            mockProjectRepository.Setup(x => x.GetAll()).Returns(mockProjectDbData);
            mockSupplierRepository.Setup(x => x.GetAll()).Returns<IQueryable<DbModel.Supplier>>(null);
            mockSupplierPODomainData.ToList().ForEach(x => x.RecordStatus = "N");
            var result = _service.SaveSupplierPurchaseOrders(mockSupplierPODomainData, true);
            Assert.AreEqual("11", result.Code);
        }

        [TestMethod]
        public void AddSupplierPOWithInvalidJsonFormat()
        {
            mockProjectRepository.Setup(x => x.GetAll()).Returns(mockProjectDbData);
            mockSupplierRepository.Setup(x => x.GetAll()).Returns<IQueryable<DbModel.Supplier>>(null);
            mockSupplierPODomainData.ToList().ForEach(x => { x.RecordStatus = "N";x.SupplierPONumber = null; });
            var result = _service.SaveSupplierPurchaseOrders(mockSupplierPODomainData, true);
            Assert.AreEqual("41", result.Code);
        }

        [TestMethod]
        public void AddSupplierPOWithExceptionResult()
        {
            mockProjectRepository.Setup(x => x.GetAll()).Returns(mockProjectDbData);
            mockSupplierRepository.Setup(x => x.GetAll()).Returns(mockSupplierDbData);
            mockSupplierPODomainData.ToList().ForEach(x => x.RecordStatus = "N");
            mockSupplierPORepository.Setup(x => x.Add(It.IsAny<DbModel.SupplierPurchaseOrder>())).Throws(new Exception());
            var result = _service.SaveSupplierPurchaseOrders(mockSupplierPODomainData, true);
            Assert.AreEqual("11", result.Code);
        }
        #endregion

        #region UpdateSupplierPO

        [TestMethod]
        public void UpdateSupplierPOValidData()
        {
            mockSupplierPORepository.Setup(x => x.GetAll()).Returns(mocksupplierPODbData);
            mockProjectRepository.Setup(x => x.GetAll()).Returns(mockProjectDbData);
            mockSupplierRepository.Setup(x => x.GetAll()).Returns(mockSupplierDbData);
            mockSupplierPORepository.Setup(x => x.FindBy(It.IsAny<Expression<Func<DbModel.SupplierPurchaseOrder, bool>>>())).Returns(mocksupplierPODbData);
            mockSupplierPODomainData.ToList().ForEach(x => x.RecordStatus = "M");
            var result = _service.ModifySupplierPurchaseOrders(mockSupplierPODomainData, true);
            mockSupplierPORepository.Verify(x => x.Update(It.IsAny<DbModel.SupplierPurchaseOrder>()), Times.AtLeastOnce);

        }

        [TestMethod]
        public void UpdateSupplierPOWithInvalidRecordStatus()
        {
            mockSupplierPORepository.Setup(x => x.GetAll()).Returns(mocksupplierPODbData);
            mockProjectRepository.Setup(x => x.GetAll()).Returns(mockProjectDbData);
            mockSupplierRepository.Setup(x => x.GetAll()).Returns(mockSupplierDbData);
            mockSupplierPODomainData.ToList().ForEach(x => x.RecordStatus = "D");
            var result = _service.ModifySupplierPurchaseOrders(mockSupplierPODomainData, true);
            Assert.AreEqual("1", result.Code);
        }

        [TestMethod]
        public void UpdateSupplierPOWithInvalidProjectNumber()
        {
            mockSupplierPORepository.Setup(x => x.GetAll()).Returns(mocksupplierPODbData);
            mockProjectRepository.Setup(x => x.GetAll()).Returns<IQueryable<DbModel.Project>>(null);
            mockSupplierRepository.Setup(x => x.GetAll()).Returns(mockSupplierDbData);
            mockSupplierPODomainData.ToList().ForEach(x => x.RecordStatus = "M");
            var result = _service.ModifySupplierPurchaseOrders(mockSupplierPODomainData, true);
            Assert.AreEqual("11", result.Code);
        }

        [TestMethod]
        public void UpdateSupplierPOWithInvalidSupplierName()
        {
            mockSupplierPORepository.Setup(x => x.GetAll()).Returns(mocksupplierPODbData);
            mockProjectRepository.Setup(x => x.GetAll()).Returns(mockProjectDbData);
            mockSupplierRepository.Setup(x => x.GetAll()).Returns<IQueryable<DbModel.Supplier>>(null);
            mockSupplierPODomainData.ToList().ForEach(x => x.RecordStatus = "M");
            var result = _service.ModifySupplierPurchaseOrders(mockSupplierPODomainData, true);
            Assert.AreEqual("11", result.Code);
        }

        [TestMethod]
        public void UpdateSupplierPOWithInvalidJsonFormat()
        { 
            mockSupplierPORepository.Setup(x => x.GetAll()).Returns(mocksupplierPODbData);
            mockProjectRepository.Setup(x => x.GetAll()).Returns(mockProjectDbData);
            mockSupplierRepository.Setup(x => x.GetAll()).Returns<IQueryable<DbModel.Supplier>>(null);
            mockSupplierPODomainData.ToList().ForEach(x => { x.RecordStatus = "M"; x.SupplierPONumber = null; });
            var result = _service.ModifySupplierPurchaseOrders(mockSupplierPODomainData, true);
            Assert.AreEqual("41", result.Code);
        }

        [TestMethod]
        public void UpdateSupplierPOWithExceptionResult()
        {
            mockSupplierPORepository.Setup(x => x.GetAll()).Returns(mocksupplierPODbData);
            mockProjectRepository.Setup(x => x.GetAll()).Returns(mockProjectDbData);
            mockSupplierRepository.Setup(x => x.GetAll()).Returns(mockSupplierDbData);
            mockSupplierPODomainData.ToList().ForEach(x => x.RecordStatus = "M");
            mockSupplierPORepository.Setup(x => x.Update(It.IsAny<DbModel.SupplierPurchaseOrder>())).Throws(new Exception());
            var result = _service.ModifySupplierPurchaseOrders(mockSupplierPODomainData, true);
            Assert.AreEqual("11", result.Code);
        }

        [TestMethod]
        public void UpdateSupplierWithWrongUpdateCount()
        {
            mockSupplierPORepository.Setup(x => x.GetAll()).Returns(mocksupplierPODbData);
            mockProjectRepository.Setup(x => x.GetAll()).Returns(mockProjectDbData);
            mockSupplierRepository.Setup(x => x.GetAll()).Returns<IQueryable<DbModel.Supplier>>(null);
            mockSupplierPODomainData.ToList().ForEach(x => { x.RecordStatus = "M"; x.UpdateCount = 2; });
            var result = _service.ModifySupplierPurchaseOrders(mockSupplierPODomainData, true);
            Assert.AreEqual("11", result.Code);
        }

        #endregion

        #region DeleteSupplierPO

        [TestMethod]
        public void DeleteSupplierPOValidData()
        {
            mocksupplierPODbData.ToList().ForEach(x => x.Assignment.Clear());
            mockSupplierPORepository.Setup(x => x.GetAll()).Returns(mocksupplierPODbData);
            mockProjectRepository.Setup(x => x.GetAll()).Returns(mockProjectDbData);
            mockSupplierRepository.Setup(x => x.GetAll()).Returns(mockSupplierDbData);
            mockSupplierPORepository.Setup(x => x.FindBy(It.IsAny<Expression<Func<DbModel.SupplierPurchaseOrder, bool>>>())).Returns(mocksupplierPODbData);
            mockSupplierPODomainData.ToList().ForEach(x => x.RecordStatus = "D");
            var result = _service.DeleteSupplierPurchaseOrders(mockSupplierPODomainData, true);
            mockSupplierPORepository.Verify(x => x.Delete(It.IsAny<DbModel.SupplierPurchaseOrder>()), Times.AtLeastOnce);
        }

        [TestMethod]
        public void DeleteSupplierPOWithInvalidRecordStatus()
        {
            mockSupplierPORepository.Setup(x => x.GetAll()).Returns(mocksupplierPODbData);
            mockProjectRepository.Setup(x => x.GetAll()).Returns(mockProjectDbData);
            mockSupplierRepository.Setup(x => x.GetAll()).Returns(mockSupplierDbData);
            mockSupplierPODomainData.ToList().ForEach(x => x.RecordStatus = "N");
            var result = _service.DeleteSupplierPurchaseOrders(mockSupplierPODomainData, true);
            Assert.AreEqual("1", result.Code);
        }

        [TestMethod]
        public void DeleteSupplierPOWithInvalidProjectNumber()
        {
            mockSupplierPORepository.Setup(x => x.GetAll()).Returns(mocksupplierPODbData);
            mockProjectRepository.Setup(x => x.GetAll()).Returns<IQueryable<DbModel.Project>>(null);
            mockSupplierRepository.Setup(x => x.GetAll()).Returns(mockSupplierDbData);
            mockSupplierPODomainData.ToList().ForEach(x => x.RecordStatus = "D");
            var result = _service.DeleteSupplierPurchaseOrders(mockSupplierPODomainData, true);
            Assert.AreEqual("11", result.Code);
        }

        [TestMethod]
        public void DeleteSupplierPOWithInvalidSupplierName()
        {
            mockSupplierPORepository.Setup(x => x.GetAll()).Returns(mocksupplierPODbData);
            mockProjectRepository.Setup(x => x.GetAll()).Returns(mockProjectDbData);
            mockSupplierRepository.Setup(x => x.GetAll()).Returns<IQueryable<DbModel.Supplier>>(null);
            mockSupplierPODomainData.ToList().ForEach(x => x.RecordStatus = "D");
            var result = _service.DeleteSupplierPurchaseOrders(mockSupplierPODomainData, true);
            Assert.AreEqual("11", result.Code);
        }

        [TestMethod]
        public void DeleteSupplierPOWithInvalidJsonFormat()
        {
            mockSupplierPORepository.Setup(x => x.GetAll()).Returns(mocksupplierPODbData);
            mockProjectRepository.Setup(x => x.GetAll()).Returns(mockProjectDbData);
            mockSupplierRepository.Setup(x => x.GetAll()).Returns<IQueryable<DbModel.Supplier>>(null);
            mockSupplierPODomainData.ToList().ForEach(x => { x.RecordStatus = "D"; x.SupplierPONumber = null; });
            var result = _service.DeleteSupplierPurchaseOrders(mockSupplierPODomainData, true);
            Assert.AreEqual("41", result.Code);
        }

        [TestMethod]
        public void DeleteSupplierPOWithExceptionResult()
        {
            mockSupplierPORepository.Setup(x => x.GetAll()).Returns(mocksupplierPODbData);
            mockProjectRepository.Setup(x => x.GetAll()).Returns(mockProjectDbData);
            mockSupplierRepository.Setup(x => x.GetAll()).Returns(mockSupplierDbData);
            mockSupplierPODomainData.ToList().ForEach(x => x.RecordStatus = "D");
            mockSupplierPORepository.Setup(x => x.Delete(It.IsAny<DbModel.SupplierPurchaseOrder>())).Throws(new Exception());
            var result = _service.DeleteSupplierPurchaseOrders(mockSupplierPODomainData, true);
            Assert.AreEqual("11", result.Code);
        }

        [TestMethod]
        public void DeleteSupplierPoWIthAssignmentAlreadyAssociated()
        {
           
            mockSupplierPORepository.Setup(x => x.GetAll()).Returns(mocksupplierPODbData);
            mockProjectRepository.Setup(x => x.GetAll()).Returns(mockProjectDbData);
            mockSupplierRepository.Setup(x => x.GetAll()).Returns(mockSupplierDbData);
            mockSupplierPORepository.Setup(x => x.FindBy(It.IsAny<Expression<Func<DbModel.SupplierPurchaseOrder, bool>>>())).Returns(mocksupplierPODbData);
            mockSupplierPODomainData.ToList().ForEach(x => x.RecordStatus = "D");
            var result = _service.DeleteSupplierPurchaseOrders(mockSupplierPODomainData, true);
            Assert.AreEqual("11", result.Code);
        }
        #endregion
    }
}
