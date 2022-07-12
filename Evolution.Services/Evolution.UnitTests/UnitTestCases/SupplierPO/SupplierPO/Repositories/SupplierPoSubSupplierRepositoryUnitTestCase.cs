using AutoMapper;
using Evolution.SupplierPO.Domain.Interfaces.Data;
using Evolution.UnitTest.UnitTestCases;
using Evolution.UnitTests.Mocks.Data.SupplierPO.Db;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using DbModel = Evolution.DbRepository.Models;
using DomainModel = Evolution.SupplierPO.Domain.Models.SupplierPO;

namespace Evolution.UnitTests.UnitTestCases.SupplierPO.Repositories
{
    [TestClass]
    public class SupplierPoSubSupplierRepositoryUnitTestCase:BaseTestCase
    {
        ISupplierPOSubSupplierRepository _repository = null;

        [TestInitialize]
        public void InitializeSubSupplier()
        {
            var mockData = MockDbSupplierPO.GetSubSupplierMockData();
            var mockDbSet = MockDbSupplierPO.GetSubSupplierMockedDbSet(mockData);
            mockContext.Setup(x => x.SupplierPurchaseOrderSubSupplier).Returns(mockDbSet.Object);
            _repository = new Evolution.SupplierPO.Infrastructure.Data.SupplierPOSubSupplierRepository(mockContext.Object, Mapper.Instance);
        }

        [TestMethod]
        public void GetAllSubSuppliers()
        {
            var result = _repository.Search(1,new DomainModel.SupplierPOSubSupplier());
            Assert.AreEqual(2, result.Count);
        }

        [TestMethod]
        public void GetSubSupplierBySubSupplierName()
        {
            var result = _repository.Search(1, new DomainModel.SupplierPOSubSupplier() { SubSupplierName = "Main Supplier 1" });
            Assert.AreEqual(2, result.Count);
        }

        [TestMethod]
        public void GetSubSupplierBySubSupplierNameWildCardSeach()
        {
            var result = _repository.Search(1, new DomainModel.SupplierPOSubSupplier() { SubSupplierName = "Main*" });
            Assert.AreEqual(2, result.Count);
        }

        //[TestMethod]
        //public void GetSupplierPOWithNullSearchValue()
        //{
        //    var result = _repository.Search(1,null);
        //    Assert.AreEqual(2, result.Count);
        //}

    }
}
