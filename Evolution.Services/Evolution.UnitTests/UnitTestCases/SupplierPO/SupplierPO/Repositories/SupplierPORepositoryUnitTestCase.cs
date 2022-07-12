using AutoMapper;
using Evolution.SupplierPO.Domain.Interfaces.Data;
using Evolution.SupplierPO.Infrastructure.Data;
using Evolution.UnitTest.UnitTestCases;
using Evolution.UnitTests.Mocks.Data.SupplierPO.Db;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DomainModel = Evolution.SupplierPO.Domain.Models.SupplierPO;

namespace Evolution.UnitTests.UnitTestCases.SupplierPO.Repositories
{
    [TestClass]
    public class SupplierPORepositoryUnitTestCase : BaseTestCase
    {
        ISupplierPORepository _repository = null;
        

        [TestInitialize]
        public void InitializeSupplierPoRepository()
        {
            var mockData = MockDbSupplierPO.GetSupplierPOMockDbData();
            var mockDbSet = MockDbSupplierPO.GetSupplierPOMockData(mockData);
            mockContext.Setup(x => x.SupplierPurchaseOrder).Returns(mockDbSet.Object);
            _repository = new SupplierPORepository(mockContext.Object, Mapper.Instance);
        }

        [TestMethod]
        public void GetAllSupplierPO()
        {
            var result = _repository.Search(new DomainModel.SupplierPO());
            Assert.AreEqual(2, result.Count);
        }

        [TestMethod]
        public void GetSupplierPOByProjectNumber()
        {
            var result = _repository.Search(new DomainModel.SupplierPO() { SupplierPOProjectNumber = 1});
            Assert.AreEqual(2, result.Count);
        }

        [TestMethod]
        public void GetSupplierPOBySupplierPoNumberWildCardSearch()
        {
            var result = _repository.Search(new DomainModel.SupplierPO() { SupplierPONumber =  "New *" });
            Assert.AreEqual(2, result.Count);
        }

        //[TestMethod]
        //public void GetAllSupplierPOWithNullSearchValue()
        //{
        //    var result = _repository.Search(null);
        //    Assert.AreEqual(2, result.Count);
        //}

    }
}
