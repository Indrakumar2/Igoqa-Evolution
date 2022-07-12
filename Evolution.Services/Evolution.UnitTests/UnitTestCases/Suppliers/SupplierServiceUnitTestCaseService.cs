using Evolution.DbRepository.Interfaces.Master;
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
using Evolution.UnitTest.Mocks.Data.Masters.Db.Masters;
using Service = Evolution.Supplier.Core.Services;
using AutoMapper;
using Evolution.Master.Domain.Models;
using Evolution.UnitTests.Mocks.Data.Masters.Domain.Masters;
using System.Linq.Expressions;
using Evolution.Common.Models.Filters;

namespace Evolution.UnitTests.UnitTestCases.Suppliers
{
    [TestClass]
    public class SupplierServiceUnitTestCaseService:BaseTestCase
    {
        ISupplierService supplierService = null;
        Mock<ICityRepository> mockCityRepo = null;
        Mock<ISupplierRepository> mockSupplierRepo = null;
       Mock<IAppLogger<SupplierService>> mockLogger = null;
        IValidationService Validation = null;
        IList<DomModel.Supplier> SupplierDomianModels = null;
        IList<City> CityDomainModel = null;
        IQueryable<DbModel.Supplier> mockSupplierDbData = null;
        IQueryable<DbModel.City> mockCityDbData = null;
        ValdService.ISupplierValidationService validationService = null;


        [TestInitialize]
        public void InitializeSupplierService()
        {
            mockCityRepo = new Mock<ICityRepository>();
            mockSupplierRepo = new Mock<ISupplierRepository>();
            mockLogger = new Mock<IAppLogger<SupplierService>>();
            SupplierDomianModels = MockSupplierDomainModel.GetSupplierDomModel();
            mockSupplierDbData = MockSupplier.GetSupplierMockedData();
            mockCityDbData = MockMaster.GetCityMockData();
            CityDomainModel = MockDomainMaster.GetCityMockedDomainData();
        }

        [TestMethod]
        public void FetchSupplierListWithoutSearchValue()
        {
            mockSupplierRepo.Setup(x => x.GetCount(It.IsAny<DomModel.Supplier>())).Returns(SupplierDomianModels.Count);
            mockCityRepo.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.City, bool>>>())).Returns(mockCityDbData);

            supplierService = new Service.SupplierService(Mapper.Instance, mockLogger.Object, mockSupplierRepo.Object, mockCityRepo.Object, validationService);

            var response = supplierService.GetSupplier(new DomModel.Supplier() , new AdditionalFilter { IsRecordCountOnly = true });

            Assert.AreEqual("1", response.Code);
            Assert.AreEqual(2, response.RecordCount);
        }
        [TestMethod]
        public void FetchSupplierListWithSearchValue()
        {
            mockSupplierRepo.Setup(x => x.Search(It.IsAny<DomModel.Supplier>())).Returns(SupplierDomianModels);
            mockCityRepo.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.City, bool>>>())).Returns(mockCityDbData);

            supplierService = new Service.SupplierService(Mapper.Instance, mockLogger.Object, mockSupplierRepo.Object, mockCityRepo.Object, validationService);

            var response = supplierService.GetSupplier(new DomModel.Supplier { SupplierId=1}, new AdditionalFilter { IsRecordCountOnly = false });

            Assert.AreEqual("1", response.Code);

            Assert.AreEqual(2, (response.Result as List<DomModel.Supplier>).Count);
        }
        [TestMethod]
        public void FetchSupplierWithNullSearchModel()
        {
            mockSupplierRepo.Setup(x => x.Search(It.IsAny<DomModel.Supplier>())).Returns(SupplierDomianModels);

            supplierService = new Service.SupplierService(Mapper.Instance, mockLogger.Object, mockSupplierRepo.Object, mockCityRepo.Object, validationService);

            var response = supplierService.GetSupplier(null, new AdditionalFilter { IsRecordCountOnly = false });

            Assert.AreEqual("1", response.Code);

            Assert.AreEqual(2, (response.Result as List<DomModel.Supplier>).Count);
        }
        [TestMethod]
        public void ThrowsExceptionWhileFetchingSupplierList()
        {
            mockSupplierRepo.Setup(x => x.Search(It.IsAny<DomModel.Supplier>())).Throws(new Exception("Exception occured while performing some operation."));
            supplierService = new Service.SupplierService(Mapper.Instance, mockLogger.Object, mockSupplierRepo.Object, mockCityRepo.Object, validationService);

            var response = supplierService.GetSupplier(null, new AdditionalFilter { IsRecordCountOnly = false });
            Assert.AreEqual("11", response.Code);
            Assert.IsNotNull(response.Messages);
            Assert.AreEqual("31", response.Messages.FirstOrDefault().Code);
        }

        [TestMethod]
        public void SaveSuppliers()
        {
            mockSupplierRepo.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Supplier, bool>>>())).Returns(mockSupplierDbData);

            mockCityRepo.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.City, bool>>>())).Returns((Expression<Func<DbModel.City, bool>> predicate) => mockCityDbData.Where(predicate));
           

            Validation = new Evolution.ValidationService.Services.ValidationService();
            validationService = new Evolution.Supplier.Infrastructure.Validations.SupplierValidationService(Validation);
            supplierService = new Service.SupplierService(Mapper.Instance, mockLogger.Object, mockSupplierRepo.Object, mockCityRepo.Object, validationService);
            SupplierDomianModels.ToList().ForEach(x => x.RecordStatus = "N");

            var response = supplierService.SaveSupplier(new List<DomModel.Supplier> { SupplierDomianModels[0] });
            Assert.AreEqual("1", response.Code);
            mockSupplierRepo.Verify(m => m.ForceSave(), Times.Once);
        }

        [TestMethod]
        public void ModifySuppliers()
        {
            mockSupplierRepo.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Supplier, bool>>>())).Returns(mockSupplierDbData.Where(x=>x.Id==1));

            mockCityRepo.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.City, bool>>>())).Returns((Expression<Func<DbModel.City, bool>> predicate) => mockCityDbData.Where(predicate));


            Validation = new Evolution.ValidationService.Services.ValidationService();
            validationService = new Evolution.Supplier.Infrastructure.Validations.SupplierValidationService(Validation);
            supplierService = new Service.SupplierService(Mapper.Instance, mockLogger.Object, mockSupplierRepo.Object, mockCityRepo.Object, validationService);
            SupplierDomianModels.ToList().ForEach(x => x.RecordStatus = "M");
            SupplierDomianModels[0].ModifiedBy = "Test";
            var response = supplierService.ModifySupplier(new List<DomModel.Supplier> { SupplierDomianModels[0] });
            Assert.AreEqual("1", response.Code);
            mockSupplierRepo.Verify(m => m.ForceSave(), Times.Once);
        }

        [TestMethod]
        public void DeleteSuppliers()
        {
            mockSupplierRepo.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Supplier, bool>>>())).Returns(mockSupplierDbData.Where(x=>x.Id==1));

            mockCityRepo.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.City, bool>>>())).Returns((Expression<Func<DbModel.City, bool>> predicate) => mockCityDbData.Where(predicate));


            Validation = new Evolution.ValidationService.Services.ValidationService();
            validationService = new Evolution.Supplier.Infrastructure.Validations.SupplierValidationService(Validation);
            supplierService = new Service.SupplierService(Mapper.Instance, mockLogger.Object, mockSupplierRepo.Object, mockCityRepo.Object, validationService);
            SupplierDomianModels.ToList().ForEach(x => x.RecordStatus = "D");
           
            var response = supplierService.DeleteSupplier(new List<DomModel.Supplier> { SupplierDomianModels[0] });
            Assert.AreEqual("1", response.Code);
            mockSupplierRepo.Verify(m => m.ForceSave(), Times.Once);
        }

        [TestMethod]
        public void IsValidCityForSupplier()
        {
            mockSupplierRepo.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Supplier, bool>>>())).Returns(mockSupplierDbData.Where(x=>x.Id==1));

            mockCityRepo.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.City, bool>>>())).Returns((Expression<Func<DbModel.City, bool>> predicate) => mockCityDbData.Where(predicate));

            Validation = new Evolution.ValidationService.Services.ValidationService();
            validationService = new Evolution.Supplier.Infrastructure.Validations.SupplierValidationService(Validation);
            supplierService = new Service.SupplierService(Mapper.Instance, mockLogger.Object, mockSupplierRepo.Object, mockCityRepo.Object, validationService);
            SupplierDomianModels.ToList().ForEach(x => x.RecordStatus = "N");

            SupplierDomianModels[0].City = "Canada";
            var response = supplierService.SaveSupplier(new List<DomModel.Supplier> { SupplierDomianModels[0] });
            Assert.AreEqual("11", response.Code);
            Assert.IsNotNull(response.Messages);
            Assert.AreEqual("11001", response.Messages.FirstOrDefault().Code);
        }

        [TestMethod]
        public void UpdateCountMissmatch()
        {
            mockSupplierRepo.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Supplier, bool>>>())).Returns(mockSupplierDbData.Where(x=>x.Id==1));

            mockCityRepo.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.City, bool>>>())).Returns((Expression<Func<DbModel.City, bool>> predicate) => mockCityDbData.Where(predicate));


            Validation = new Evolution.ValidationService.Services.ValidationService();
            validationService = new Evolution.Supplier.Infrastructure.Validations.SupplierValidationService(Validation);
            supplierService = new Service.SupplierService(Mapper.Instance, mockLogger.Object, mockSupplierRepo.Object, mockCityRepo.Object, validationService);
            SupplierDomianModels.ToList().ForEach(x => x.RecordStatus = "M");
            SupplierDomianModels[0].UpdateCount = 5;
            var response = supplierService.ModifySupplier(new List<DomModel.Supplier> { SupplierDomianModels[0] });
           
            Assert.AreEqual("11", response.Code);
            Assert.IsNotNull(response.Messages);
            Assert.AreEqual("11002", response.Messages.FirstOrDefault().Code);
        }

        [TestMethod]
        public void ShouldNotDeleteSupplierWhenItsAlreadyRefered()
        {
            mockSupplierRepo.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Supplier, bool>>>())).Returns(mockSupplierDbData.Where(x=>x.Id==2));

            mockCityRepo.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.City, bool>>>())).Returns((Expression<Func<DbModel.City, bool>> predicate) => mockCityDbData.Where(predicate));


            Validation = new Evolution.ValidationService.Services.ValidationService();
            validationService = new Evolution.Supplier.Infrastructure.Validations.SupplierValidationService(Validation);
            supplierService = new Service.SupplierService(Mapper.Instance, mockLogger.Object, mockSupplierRepo.Object, mockCityRepo.Object, validationService);
            SupplierDomianModels.ToList().ForEach(x => x.RecordStatus = "D");

            var response = supplierService.DeleteSupplier(new List<DomModel.Supplier> { SupplierDomianModels[1] });
            Assert.AreEqual("11", response.Code);
            Assert.IsNotNull(response.Messages);
            Assert.AreEqual("11004", response.Messages.FirstOrDefault().Code);
        }
        [TestMethod]
        public void ThrowsExceptionWhileDeletingSupplier()
        {
            mockSupplierRepo.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Supplier, bool>>>())).Returns(mockSupplierDbData.Where(x => x.Id == 1));

            mockCityRepo.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.City, bool>>>())).Returns((Expression<Func<DbModel.City, bool>> predicate) => mockCityDbData.Where(predicate));


           // Validation = new Evolution.ValidationService.Services.ValidationService();
           // validationService = new Evolution.Supplier.Infrastructure.Validations.SupplierValidationService(Validation);
            supplierService = new Service.SupplierService(Mapper.Instance, mockLogger.Object, mockSupplierRepo.Object, mockCityRepo.Object, validationService);
            SupplierDomianModels.ToList().ForEach(x => x.RecordStatus = "D");

            var response = supplierService.DeleteSupplier(new List<DomModel.Supplier> { SupplierDomianModels[0] });
            Assert.AreEqual("11", response.Code);
            Assert.IsNotNull(response.Messages);
            Assert.AreEqual("31", response.Messages.FirstOrDefault().Code);

        }
        [TestMethod]
        public void ThrowsExceptionWhileSavingSupplier()
        {
            mockSupplierRepo.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Supplier, bool>>>())).Returns(mockSupplierDbData.Where(x => x.Id == 1));

            mockCityRepo.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.City, bool>>>())).Returns((Expression<Func<DbModel.City, bool>> predicate) => mockCityDbData.Where(predicate));


           // Validation = new Evolution.ValidationService.Services.ValidationService();
            //validationService = new Evolution.Supplier.Infrastructure.Validations.SupplierValidationService(Validation);
            supplierService = new Service.SupplierService(Mapper.Instance, mockLogger.Object, mockSupplierRepo.Object, mockCityRepo.Object, validationService);
            SupplierDomianModels.ToList().ForEach(x => x.RecordStatus = "N");

            var response = supplierService.SaveSupplier(new List<DomModel.Supplier> { SupplierDomianModels[0] });
            Assert.AreEqual("11", response.Code);
            Assert.IsNotNull(response.Messages);
            Assert.AreEqual("31", response.Messages.FirstOrDefault().Code);

        }

        [TestMethod]
        public void ThrowsExceptionWhileModifyingSupplier()
        {
            mockSupplierRepo.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Supplier, bool>>>())).Returns(mockSupplierDbData.Where(x => x.Id == 1));

            mockCityRepo.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.City, bool>>>())).Returns((Expression<Func<DbModel.City, bool>> predicate) => mockCityDbData.Where(predicate));


            // Validation = new Evolution.ValidationService.Services.ValidationService();
            //validationService = new Evolution.Supplier.Infrastructure.Validations.SupplierValidationService(Validation);
            supplierService = new Service.SupplierService(Mapper.Instance, mockLogger.Object, mockSupplierRepo.Object, mockCityRepo.Object, validationService);
            SupplierDomianModels.ToList().ForEach(x => x.RecordStatus = "M");

            var response = supplierService.ModifySupplier(new List<DomModel.Supplier> { SupplierDomianModels[0] });
            Assert.AreEqual("11", response.Code);
            Assert.IsNotNull(response.Messages);
            Assert.AreEqual("31", response.Messages.FirstOrDefault().Code);

        }



    }
}
