using Evolution.Logging.Interfaces;
using Evolution.Supplier.Core.Services;
using Evolution.Supplier.Domain.Interfaces.Data;
using Evolution.Supplier.Domain.Interfaces.Suppliers;
using Evolution.UnitTest.UnitTestCases;
using Evolution.ValidationService.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DomModel = Evolution.Supplier.Domain.Models.Supplier;
using DbModel = Evolution.DbRepository.Models;
using ValdService = Evolution.Supplier.Domain.Interfaces.Validations;
using Evolution.UnitTests.Mocks.Data.Supplier.Domain;
using Evolution.UnitTests.Mocks.Data.Supplier.Db;
using Service = Evolution.Supplier.Core.Services;
using AutoMapper;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Evolution.UnitTests.UnitTestCases.Suppliers
{
    [TestClass]
   public class SupplierContactUnitTestCaseService:BaseTestCase
   {
        ISupplierContactService supplierContactService = null;
        Mock<ISupplierContactRepository> mockSupplierContactRepo = null;
        Mock<ISupplierRepository> mockSupplierRepo = null;
        Mock<IAppLogger<SupplierContactService>> mockLogger = null;
        IValidationService Validation = null;
        IList<DomModel.SupplierContact> SupplierContactDomianModels = null;
        IQueryable<DbModel.Supplier> mockSupplierDbData = null;
        IQueryable<DbModel.SupplierContact> mockSupplierContactDbData = null;
        ValdService.ISupplierContactValidationService validationService = null;
        //Mock<IMapper> mockMapper = null;

        [TestInitialize]
        public void InitializeSupplierContactService()
        {
            mockSupplierContactRepo = new Mock<ISupplierContactRepository>();
            mockSupplierRepo = new Mock<ISupplierRepository>();
            mockLogger = new Mock<IAppLogger<SupplierContactService>>();
            SupplierContactDomianModels = MockSupplierDomainModel.GetSupplierContactDomModel();
            mockSupplierDbData = MockSupplier.GetSupplierMockedData();
            mockSupplierContactDbData = MockSupplier.GetSupplierContactMockedData();



        }
        [TestMethod]
       
        public void FetchSupplierContactListWithoutSearchValue()
        {
            mockSupplierContactRepo.Setup(x => x.Search(It.IsAny<DomModel.SupplierContact>())).Returns(SupplierContactDomianModels);


            supplierContactService = new Service.SupplierContactService(Mapper.Instance, mockLogger.Object, mockSupplierContactRepo.Object, mockSupplierRepo.Object, validationService);

            var response = supplierContactService.GetSupplierContact(new DomModel.SupplierContact());

            Assert.AreEqual("1", response.Code);
            Assert.AreEqual(2, (response.Result as List<DomModel.SupplierContact>).Count);
        }

        [TestMethod]
        public void FetchSupplierNoteListWithSearchValue()
        {
            mockSupplierContactRepo.Setup(x => x.Search(It.IsAny<DomModel.SupplierContact>())).Returns(new List<DomModel.SupplierContact>() { SupplierContactDomianModels[0] });

            supplierContactService = new Service.SupplierContactService(Mapper.Instance, mockLogger.Object, mockSupplierContactRepo.Object, mockSupplierRepo.Object, validationService);

            var response = supplierContactService.GetSupplierContact(new DomModel.SupplierContact() { SupplierContactId = 1 });

            Assert.AreEqual("1", response.Code);
            Assert.AreEqual(1, (response.Result as List<DomModel.SupplierContact>).Count);
        }
        [TestMethod]
        public void FetchSupplierNoteListBySupplierId()
        {
            mockSupplierContactRepo.Setup(x => x.Search(It.IsAny<DomModel.SupplierContact>())).Returns(new List<DomModel.SupplierContact>() { SupplierContactDomianModels[0] });

            supplierContactService = new Service.SupplierContactService(Mapper.Instance, mockLogger.Object, mockSupplierContactRepo.Object, mockSupplierRepo.Object, validationService);

            var response = supplierContactService.GetSupplierContact(new DomModel.SupplierContact() { SupplierId = 1 });

            Assert.AreEqual("1", response.Code);
            Assert.AreEqual(1, (response.Result as List<DomModel.SupplierContact>).Count);
        }
        [TestMethod]
        public void ThrowsExceptionWhileFetchingContact()
        {
            mockSupplierContactRepo.Setup(x => x.Search(It.IsAny<DomModel.SupplierContact>())).Throws(new Exception("Exception occured while performing some operation."));
            supplierContactService = new Service.SupplierContactService(Mapper.Instance, mockLogger.Object, mockSupplierContactRepo.Object, mockSupplierRepo.Object, validationService);

            var response = supplierContactService.GetSupplierContact(new DomModel.SupplierContact());

            Assert.AreEqual("11", response.Code);
            Assert.IsNotNull(response.Messages);
            Assert.AreEqual("31", response.Messages.FirstOrDefault().Code);
        }
        [TestMethod]
        public void SaveSupplierContact()
        {
            mockSupplierRepo.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Supplier, bool>>>())).Returns(mockSupplierDbData);
            mockSupplierContactRepo.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.SupplierContact, bool>>>())).Returns((Expression<Func<DbModel.SupplierContact, bool>> predicate) => mockSupplierContactDbData.Where(predicate));
            Validation = new Evolution.ValidationService.Services.ValidationService();
            validationService = new Evolution.Supplier.Infrastructure.Validations.SupplierContactValidationService(Validation);
            supplierContactService = new Service.SupplierContactService(Mapper.Instance, mockLogger.Object, mockSupplierContactRepo.Object, mockSupplierRepo.Object, validationService);

            SupplierContactDomianModels.ToList().ForEach(x => x.RecordStatus = "N");

            var response = supplierContactService.SaveSupplierContact( new List<DomModel.SupplierContact> { SupplierContactDomianModels[0] }, true);

            mockSupplierContactRepo.Verify(m => m.Add(It.IsAny<IList<DbModel.SupplierContact>>()), Times.AtLeastOnce);
        }
        [TestMethod]
        public void ModifySupplierContact()
        {
            mockSupplierRepo.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Supplier, bool>>>())).Returns(mockSupplierDbData);
            mockSupplierContactRepo.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.SupplierContact, bool>>>())).Returns((Expression<Func<DbModel.SupplierContact, bool>> predicate) => mockSupplierContactDbData.Where(predicate));
            //mockDbRepo.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Data, bool>>>())).Returns((Expression<Func<DbModel.Data, bool>> predicate) => mockAssignmentType.Where(predicate));
            Validation = new Evolution.ValidationService.Services.ValidationService();
            validationService = new Evolution.Supplier.Infrastructure.Validations.SupplierContactValidationService(Validation);
            supplierContactService = new Service.SupplierContactService(Mapper.Instance, mockLogger.Object, mockSupplierContactRepo.Object, mockSupplierRepo.Object, validationService);
            SupplierContactDomianModels.ToList().ForEach(x => x.RecordStatus = "M");
            mockSupplierContactRepo.Setup(moq => moq.ForceSave());
            SupplierContactDomianModels[0].SupplierContactName = "ABC";

             var response = supplierContactService.ModifySupplierContact(new List<DomModel.SupplierContact> { SupplierContactDomianModels[0] }, true);
            mockSupplierContactRepo.Verify(m => m.ForceSave(), Times.Once);
        }
        [TestMethod]
        public void DeleteSupplierContact()
        {
            mockSupplierRepo.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Supplier, bool>>>())).Returns(mockSupplierDbData);
            mockSupplierContactRepo.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.SupplierContact, bool>>>())).Returns((Expression<Func<DbModel.SupplierContact, bool>> predicate) => mockSupplierContactDbData.Where(predicate));
            Validation = new Evolution.ValidationService.Services.ValidationService();
            validationService = new Evolution.Supplier.Infrastructure.Validations.SupplierContactValidationService(Validation);
            supplierContactService = new Service.SupplierContactService(Mapper.Instance, mockLogger.Object, mockSupplierContactRepo.Object, mockSupplierRepo.Object, validationService);
            SupplierContactDomianModels.ToList().ForEach(x => x.RecordStatus = "D");
            var response = supplierContactService.DeleteSupplierContact( new List<DomModel.SupplierContact> { SupplierContactDomianModels[0] }, true);
            mockSupplierContactRepo.Verify(m => m.ForceSave(), Times.Once);
        }
        [TestMethod]
        public void ThrowsExceptionWhileSavingContact()
        {

            mockSupplierRepo.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Supplier, bool>>>())).Returns(mockSupplierDbData);

            Validation = new Evolution.ValidationService.Services.ValidationService();
            validationService = new Evolution.Supplier.Infrastructure.Validations.SupplierContactValidationService(Validation);
            supplierContactService = new Service.SupplierContactService(Mapper.Instance, mockLogger.Object, mockSupplierContactRepo.Object, mockSupplierRepo.Object, validationService);

            SupplierContactDomianModels.ToList().ForEach(x => x.RecordStatus = "N");

            var response = supplierContactService.SaveSupplierContact(new List<DomModel.SupplierContact> { SupplierContactDomianModels[0] }, true);

            mockSupplierContactRepo.Verify(m => m.Add(It.IsAny<IList<DbModel.SupplierContact>>()), Times.AtLeastOnce);
        }
        [TestMethod]
        public void ThrowsExceptionWhileDeletingContact()
        {
            mockSupplierRepo.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Supplier, bool>>>())).Throws(new Exception("Exception occured while performing some operation."));
            mockSupplierContactRepo.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.SupplierContact, bool>>>())).Returns((Expression<Func<DbModel.SupplierContact, bool>> predicate) => mockSupplierContactDbData.Where(predicate));
           // Validation = new Evolution.ValidationService.Services.ValidationService();
            //validationService = new Evolution.Supplier.Infrastructure.Validations.SupplierContactValidationService(Validation);
            supplierContactService = new Service.SupplierContactService(Mapper.Instance, mockLogger.Object, mockSupplierContactRepo.Object, mockSupplierRepo.Object, validationService);
            SupplierContactDomianModels.ToList().ForEach(x => x.RecordStatus = "D");
            var response = supplierContactService.DeleteSupplierContact(new List<DomModel.SupplierContact> { SupplierContactDomianModels[0] }, true);
            Assert.AreEqual("11", response.Code);
            Assert.IsNotNull(response.Messages);
            Assert.AreEqual("31", response.Messages.FirstOrDefault().Code);

        }



        [TestMethod]
        public void ShouldNotSaveDataWhenSupplierIsInvalid()
        {

            Validation = new Evolution.ValidationService.Services.ValidationService();
            validationService = new Evolution.Supplier.Infrastructure.Validations.SupplierContactValidationService(Validation);
            supplierContactService = new Service.SupplierContactService(Mapper.Instance, mockLogger.Object, mockSupplierContactRepo.Object, mockSupplierRepo.Object, validationService);

            SupplierContactDomianModels.ToList().ForEach(x => x.RecordStatus = "N");
            var response = supplierContactService.SaveSupplierContact(new List<DomModel.SupplierContact> { SupplierContactDomianModels[0] }, true);
            Assert.AreEqual("11", response.Code);

        }

        [TestMethod]
        public void GetAllSavedContacts()
        {
            mockSupplierRepo.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Supplier, bool>>>())).Returns(mockSupplierDbData);

            Validation = new Evolution.ValidationService.Services.ValidationService();
            validationService = new Evolution.Supplier.Infrastructure.Validations.SupplierContactValidationService(Validation);
            supplierContactService = new Service.SupplierContactService(Mapper.Instance, mockLogger.Object, mockSupplierContactRepo.Object, mockSupplierRepo.Object, validationService);

            SupplierContactDomianModels.ToList().ForEach(x => x.RecordStatus = "N");

            var response = supplierContactService.SaveSupplierContact(new List<DomModel.SupplierContact> { SupplierContactDomianModels[0] }, true);

            mockSupplierContactRepo.Verify(m => m.Add(It.IsAny<IList<DbModel.SupplierContact>>()), Times.AtLeastOnce);
        }



       
    }

}

