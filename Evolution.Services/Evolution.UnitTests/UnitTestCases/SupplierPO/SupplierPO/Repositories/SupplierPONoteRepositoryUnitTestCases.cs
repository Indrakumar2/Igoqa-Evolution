using AutoMapper;
using Evolution.SupplierPO.Domain.Interfaces.Data;
using Evolution.UnitTest.UnitTestCases;
using Evolution.UnitTests.Mocks.Data.SupplierPO.Db;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using DomainModel = Evolution.SupplierPO.Domain.Models.SupplierPO;

namespace Evolution.UnitTests.UnitTestCases.SupplierPO.Repositories
{
    [TestClass]
    public class SupplierPONoteRepositoryUnitTestCases :BaseTestCase
    {
        private ISupplierPONoteRepository _repository = null;
             
        [TestInitialize]
        public void InitializeNotes()
        {
            var mockData = MockDbSupplierPO.GetSupplierPONotesMockData();
            var mockDbSet = MockDbSupplierPO.GetSupplierPONotesMockData(mockData);
            mockContext.Setup(x => x.SupplierPurchaseOrderNote).Returns(mockDbSet.Object);
            _repository = new Evolution.SupplierPO.Infrastructure.Data.SupplierPONoteRepository(mockContext.Object, Mapper.Instance);
        }

        [TestMethod]
        public void GetAllNotes()
        {
            var result = _repository.Search(1, new DomainModel.SupplierPONote());
            Assert.AreEqual(2, result.Count);
        }


        [TestMethod]
        public void GetNotesBySupplierPONumber()
        {
            var result = _repository.Search(1, new DomainModel.SupplierPONote() { SupplierPONumber= "New Supplier PO Number" });
            Assert.AreEqual(2, result.Count);
        }


        [TestMethod]
        public void GetNotesBySupplierPONumberWildCardSearch()
        {
            var result = _repository.Search(1, new DomainModel.SupplierPONote() { SupplierPONumber = "New*" });
            Assert.AreEqual(2, result.Count);
        }

        //[TestMethod]
        //public void GetNotesWithoutSearchValue()
        //{
        //    var result = _repository.Search(1, null);
        //    Assert.AreEqual(2, result.Count);
        //}

    }
}
