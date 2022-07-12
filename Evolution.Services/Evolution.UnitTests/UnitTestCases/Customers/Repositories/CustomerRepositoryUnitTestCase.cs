using AutoMapper;
using Evolution.UnitTest.Mocks.Data.Customers.Db;
using Evolution.UnitTest.UnitTestCases;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using DomainData = Evolution.Customer.Domain.Interfaces.Data;

namespace Evolution.UnitTests.UnitTestCases.Customers.Repositories
{
    [TestClass]
    public class CustomerRepositoryUnitTestCase : BaseTestCase
    {        
        DomainData.ICustomerRepository customerRepository = null;

        [TestInitialize]
        public void InitializeCustomerRepository()
        {   
            var mockData = MockCustomer.GetCustomerMockData();
            var mockDbSet = MockCustomer.GetCustomerMockDbSet(mockData);
            
            mockContext.Setup(c => c.Customer).Returns(mockDbSet.Object);
            customerRepository = new Customer.Infrastructure.Data.CustomerRepository(Mapper.Instance,mockContext.Object);
        }

        [TestMethod]
        public void FetchCustomerListWithoutSearchValue()
        {
            var company= customerRepository.Search(new Customer.Domain.Models.Customers.CustomerSearch());
            Assert.AreEqual(4, company.Count);
        }

        [TestMethod]
        public void FetchCustomerListWithNullSearchModel()
        {
            var company = customerRepository.Search(null);
            Assert.AreEqual(4, company.Count);
        }

        [TestMethod]
        public void FetchCustomerListByCompanyCode()
        {
            var company = customerRepository.Search(new Customer.Domain.Models.Customers.CustomerSearch() { CustomerName = "ABB" });
            Assert.AreEqual(1, company.Count);
        }

        [TestMethod]
        public void FetchCustomerListByNameWildCardSearch()
        {
            var company = customerRepository.Search(new Customer.Domain.Models.Customers.CustomerSearch() { CustomerName = "*AB*" });
            Assert.AreEqual(4, company.Count);
        }

        
    }
}
