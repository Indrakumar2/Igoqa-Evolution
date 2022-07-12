using System;
using System.Collections.Generic;
using System.Text;
using ServiceDomainData = Evolution.Contract.Domain.Interfaces.Contracts;
using DomainData = Evolution.Contract.Domain.Interfaces.Data;
using Moq;
using DomModel = Evolution.Contract.Domain.Models.Contracts;
using Evolution.Logging.Interfaces;
using AutoMapper;
using DbModel = Evolution.DbRepository.Models;
using System.Linq;
using Evolution.UnitTests.Mocks.Data.Contracts.Domain;
using Evolution.UnitTests.Mocks.Data.Contracts.Db;
using Masterdata = Evolution.Master.Domain.Interfaces.Data;
using System.Linq.Expressions;
using Microsoft.Extensions.Options;
using MasterService = Evolution.Master.Domain.Interfaces;
using Evolution.Common.Enums;
using DbRepo = Evolution.DbRepository.Interfaces.Master;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Service = Evolution.Contract.Core.Services;
using Evolution.UnitTest.UnitTestCases;
using ValidService = Evolution.Contract.Domain.Interfaces.Validations;

namespace Evolution.UnitTests.UnitTestCases.Contract.Services
{
    [TestClass]
    public class ContractInvoiceReferenceServiceUnitTestCase : BaseTestCase
    {
        ValidService.IContractInvoiceReferenceTypeValidationService InvoiceReferenceValidService=null;
        ServiceDomainData.IContractInvoiceReferenceTypeService ContractInvoiceReferenceService = null;
        Mock<DomainData.IContractInvoiceReferenceTypeRepository> mockContractinvoiceReferenceRepository = null;
        Mock<DomainData.IContractRepository> mockContractRepository = null;
        Mock<DbRepo.IDataRepository> mockDbRepo = null;
        IList<DomModel.ContractInvoiceReferenceType> contractInvoiceReferenceTypeDomianModels = null;
        Mock<IAppLogger<Service.ContractInvoiceReferenceTypeService>> mockLogger = null;
        Mock<IMapper> mockMapper = null;
        IQueryable<DbModel.Contract> mockContractDbData = null;
        IQueryable<DbModel.ContractInvoiceReference> mockContractInvoiceReferenceDbData = null;
        IQueryable<DbModel.Data> mockAssignmentType = null;
        [TestInitialize]
        public void InitializeContractInvoiceReferencService()
        {
            mockContractinvoiceReferenceRepository = new Mock<DomainData.IContractInvoiceReferenceTypeRepository>();
            mockContractRepository = new Mock<DomainData.IContractRepository>();
            contractInvoiceReferenceTypeDomianModels = MockContractDomainModel.GetDomainModelInvoiceReference();
            mockLogger = new Mock<IAppLogger<Service.ContractInvoiceReferenceTypeService>>();
            mockMapper = new Mock<IMapper>();
            mockContractDbData = MockContract.GetDbModelContract();
            mockContractInvoiceReferenceDbData = MockContract.GetDbModelContractInvoiceReferences();
            mockDbRepo = new Mock<DbRepo.IDataRepository>();
            mockAssignmentType = MockContract.GetmoduleAssignmentTypeMockData();
        }
        [TestMethod]
        public void FetchAllContractInvoiceReferencWithoutSearchValue()
        {
            mockContractinvoiceReferenceRepository.Setup(x => x.Search(It.IsAny<DomModel.ContractInvoiceReferenceType>())).Returns(contractInvoiceReferenceTypeDomianModels);
            ContractInvoiceReferenceService = new Service.ContractInvoiceReferenceTypeService(mockMapper.Object, mockLogger.Object, mockContractinvoiceReferenceRepository.Object, mockContractRepository.Object, mockDbRepo.Object, InvoiceReferenceValidService);
            var response = ContractInvoiceReferenceService.GetContractInvoiceReferenceType(new DomModel.ContractInvoiceReferenceType());
            Assert.AreEqual("1", response.Code);
            Assert.AreEqual(2, (response.Result as List<DomModel.ContractInvoiceReferenceType>).Count);
        }
        [TestMethod]
        public void FetchAllContractInvoiceReferencWithSearchValue()
        {
            mockContractinvoiceReferenceRepository.Setup(x => x.Search(It.Is<DomModel.ContractInvoiceReferenceType>(c => c.ContractNumber == "SU02412/0001"))).Returns(new List<DomModel.ContractInvoiceReferenceType> { contractInvoiceReferenceTypeDomianModels[0] });
            ContractInvoiceReferenceService = new Service.ContractInvoiceReferenceTypeService(mockMapper.Object, mockLogger.Object, mockContractinvoiceReferenceRepository.Object, mockContractRepository.Object, mockDbRepo.Object, InvoiceReferenceValidService);
            var response = ContractInvoiceReferenceService.GetContractInvoiceReferenceType(new DomModel.ContractInvoiceReferenceType { ContractNumber = "SU02412/0001" });


            Assert.AreEqual("1", response.Code);
            Assert.AreEqual(1, (response.Result as List<DomModel.ContractInvoiceReferenceType>).Count);
        }

        [TestMethod]
        public void FetchContractInvoiceReferencListWithNullSearchModel()
        {
            mockContractinvoiceReferenceRepository.Setup(x => x.Search(It.IsAny<DomModel.ContractInvoiceReferenceType>())).Returns(contractInvoiceReferenceTypeDomianModels);

            ContractInvoiceReferenceService = new Service.ContractInvoiceReferenceTypeService(mockMapper.Object, mockLogger.Object, mockContractinvoiceReferenceRepository.Object, mockContractRepository.Object, mockDbRepo.Object, InvoiceReferenceValidService);

            var response = ContractInvoiceReferenceService.GetContractInvoiceReferenceType(null);

            Assert.AreEqual("1", response.Code);
            Assert.AreEqual(2, (response.Result as List<DomModel.ContractInvoiceReferenceType>).Count);
        }
        [TestMethod]
        public void ThrowsExceptionWhileFetchingInvoiceReferenc()
        {
            mockContractinvoiceReferenceRepository.Setup(x => x.Search(It.IsAny<DomModel.ContractInvoiceReferenceType>())).Throws(new Exception("Exception occured while performing some operation."));
            ContractInvoiceReferenceService = new Service.ContractInvoiceReferenceTypeService(mockMapper.Object, mockLogger.Object, mockContractinvoiceReferenceRepository.Object, mockContractRepository.Object, mockDbRepo.Object, InvoiceReferenceValidService);

            var response = ContractInvoiceReferenceService.GetContractInvoiceReferenceType(new DomModel.ContractInvoiceReferenceType());

            Assert.AreEqual("11", response.Code);
            Assert.IsNotNull(response.Messages);
            Assert.AreEqual("31", response.Messages.FirstOrDefault().Code);
        }
        [TestMethod]
        public void FetchContractInvoiceReferencListByWildCardSearchWithReftypeStartWith()
        {
            mockContractinvoiceReferenceRepository.Setup(x => x.Search(It.Is<DomModel.ContractInvoiceReferenceType>(c => c.ReferenceType.StartsWith("Call")))).Returns(new List<DomModel.ContractInvoiceReferenceType> { contractInvoiceReferenceTypeDomianModels[0] });

            ContractInvoiceReferenceService = new Service.ContractInvoiceReferenceTypeService(mockMapper.Object, mockLogger.Object, mockContractinvoiceReferenceRepository.Object, mockContractRepository.Object, mockDbRepo.Object, InvoiceReferenceValidService);

            var response = ContractInvoiceReferenceService.GetContractInvoiceReferenceType(new DomModel.ContractInvoiceReferenceType { ReferenceType = "Call*" });

            Assert.AreEqual("1", response.Code);
            Assert.AreEqual(1, (response.Result as List<DomModel.ContractInvoiceReferenceType>).Count);
        }
        [TestMethod]
        public void FetchContractInvoiceRefListByWildCardSearchWithReftypeEndWith()
        {
            mockContractinvoiceReferenceRepository.Setup(x => x.Search(It.Is<DomModel.ContractInvoiceReferenceType>(c => c.ReferenceType.EndsWith("Center")))).Returns(new List<DomModel.ContractInvoiceReferenceType> { contractInvoiceReferenceTypeDomianModels[1] });

            ContractInvoiceReferenceService = new Service.ContractInvoiceReferenceTypeService(mockMapper.Object, mockLogger.Object, mockContractinvoiceReferenceRepository.Object, mockContractRepository.Object, mockDbRepo.Object, InvoiceReferenceValidService);

            var response = ContractInvoiceReferenceService.GetContractInvoiceReferenceType(new DomModel.ContractInvoiceReferenceType { ReferenceType = "*Center" });

            Assert.AreEqual("1", response.Code);
            Assert.AreEqual(1, (response.Result as List<DomModel.ContractInvoiceReferenceType>).Count);
        }

        [TestMethod]
        public void FetchcontractInvoiceReferencListByWildCardSearchWithReftypeContains()
        {
            mockContractinvoiceReferenceRepository.Setup(x => x.Search(It.Is<DomModel.ContractInvoiceReferenceType>(c => c.ReferenceType.Contains("Cost")))).Returns(new List<DomModel.ContractInvoiceReferenceType> { contractInvoiceReferenceTypeDomianModels[1] });

            ContractInvoiceReferenceService = new Service.ContractInvoiceReferenceTypeService(mockMapper.Object, mockLogger.Object, mockContractinvoiceReferenceRepository.Object, mockContractRepository.Object, mockDbRepo.Object, InvoiceReferenceValidService);

            var response = ContractInvoiceReferenceService.GetContractInvoiceReferenceType(new DomModel.ContractInvoiceReferenceType { ReferenceType = "*Cost*" });

            Assert.AreEqual("1", response.Code);
            Assert.AreEqual(1, (response.Result as List<DomModel.ContractInvoiceReferenceType>).Count);
        }


        [TestMethod]
        public void SaveContrcatInvoiceRefType()
        {

            mockContractRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Contract, bool>>>())).Returns(mockContractDbData);
            mockContractinvoiceReferenceRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.ContractInvoiceReference, bool>>>())).Returns((Expression<Func<DbModel.ContractInvoiceReference, bool>> predicate) => mockContractInvoiceReferenceDbData.Where(predicate));
            mockDbRepo.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Data, bool>>>())).Returns((Expression<Func<DbModel.Data, bool>> predicate) => mockAssignmentType.Where(predicate));
            ContractInvoiceReferenceService = new Service.ContractInvoiceReferenceTypeService(mockMapper.Object, mockLogger.Object, mockContractinvoiceReferenceRepository.Object, mockContractRepository.Object, mockDbRepo.Object,InvoiceReferenceValidService);
            contractInvoiceReferenceTypeDomianModels.ToList().ForEach(x => x.RecordStatus = "N");
            var response = ContractInvoiceReferenceService.SaveContractInvoiceReferenceType(contractInvoiceReferenceTypeDomianModels[0].ContractNumber, new List<DomModel.ContractInvoiceReferenceType> { contractInvoiceReferenceTypeDomianModels[0] });
            mockContractinvoiceReferenceRepository.Verify(m => m.Add(It.IsAny<IList<DbModel.ContractInvoiceReference>>()), Times.AtLeastOnce);
        }
        [TestMethod]
        public void ModifyContractInvoiceRefType()
        {

            mockContractRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Contract, bool>>>())).Returns(mockContractDbData);
            mockContractinvoiceReferenceRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.ContractInvoiceReference, bool>>>())).Returns((Expression<Func<DbModel.ContractInvoiceReference, bool>> predicate) => mockContractInvoiceReferenceDbData.Where(predicate));
            mockDbRepo.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Data, bool>>>())).Returns((Expression<Func<DbModel.Data, bool>> predicate) => mockAssignmentType.Where(predicate));
            ContractInvoiceReferenceService = new Service.ContractInvoiceReferenceTypeService(mockMapper.Object, mockLogger.Object, mockContractinvoiceReferenceRepository.Object, mockContractRepository.Object, mockDbRepo.Object, InvoiceReferenceValidService);
            contractInvoiceReferenceTypeDomianModels.ToList().ForEach(x => x.RecordStatus = "M");
            var response = ContractInvoiceReferenceService.ModifyContractInvoiceReferenceType(contractInvoiceReferenceTypeDomianModels[0].ContractNumber, new List<DomModel.ContractInvoiceReferenceType> { contractInvoiceReferenceTypeDomianModels[0] });
            mockContractinvoiceReferenceRepository.Verify(m => m.Update(It.IsAny<DbModel.ContractInvoiceReference>()), Times.AtLeastOnce);
        }

        [TestMethod]
        public void DeleteContractInvoiceRefType()
        {

            mockContractRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Contract, bool>>>())).Returns(mockContractDbData);
            mockContractinvoiceReferenceRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.ContractInvoiceReference, bool>>>())).Returns((Expression<Func<DbModel.ContractInvoiceReference, bool>> predicate) => mockContractInvoiceReferenceDbData.Where(predicate));
            mockDbRepo.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Data, bool>>>())).Returns((Expression<Func<DbModel.Data, bool>> predicate) => mockAssignmentType.Where(predicate));

            ContractInvoiceReferenceService = new Service.ContractInvoiceReferenceTypeService(mockMapper.Object, mockLogger.Object, mockContractinvoiceReferenceRepository.Object, mockContractRepository.Object, mockDbRepo.Object, InvoiceReferenceValidService);
            contractInvoiceReferenceTypeDomianModels.ToList().ForEach(x => x.RecordStatus = "D");
            var response = ContractInvoiceReferenceService.DeleteContractInvoiceReferenceType(contractInvoiceReferenceTypeDomianModels[0].ContractNumber, new List<DomModel.ContractInvoiceReferenceType> { contractInvoiceReferenceTypeDomianModels[0] });
            mockContractinvoiceReferenceRepository.Verify(m => m.Delete(It.IsAny<DbModel.ContractInvoiceReference>()), Times.AtLeastOnce);
        }


        [TestMethod]
        public void ThrowExceptionWhileSavingContrcatInvoiceRefType()
        {

            mockContractRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Contract, bool>>>())).Throws(new Exception("Exception occured while performing some operation."));
            mockContractinvoiceReferenceRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.ContractInvoiceReference, bool>>>())).Returns((Expression<Func<DbModel.ContractInvoiceReference, bool>> predicate) => mockContractInvoiceReferenceDbData.Where(predicate));
            mockDbRepo.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Data, bool>>>())).Returns((Expression<Func<DbModel.Data, bool>> predicate) => mockAssignmentType.Where(predicate));
            ContractInvoiceReferenceService = new Service.ContractInvoiceReferenceTypeService(mockMapper.Object, mockLogger.Object, mockContractinvoiceReferenceRepository.Object, mockContractRepository.Object, mockDbRepo.Object, InvoiceReferenceValidService);
            contractInvoiceReferenceTypeDomianModels.ToList().ForEach(x => x.RecordStatus = "N");
            var response = ContractInvoiceReferenceService.SaveContractInvoiceReferenceType(contractInvoiceReferenceTypeDomianModels[0].ContractNumber, new List<DomModel.ContractInvoiceReferenceType> { contractInvoiceReferenceTypeDomianModels[0] });
            Assert.AreEqual("11", response.Code);
            Assert.IsNotNull(response.Messages);
            Assert.AreEqual("31", response.Messages.FirstOrDefault().Code);
        }
        [TestMethod]
        public void ThrowExceptionWhileUpdatingContrcatInvoiceRefType()
        {

            mockContractRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Contract, bool>>>())).Throws(new Exception("Exception occured while performing some operation."));
            mockContractinvoiceReferenceRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.ContractInvoiceReference, bool>>>())).Returns((Expression<Func<DbModel.ContractInvoiceReference, bool>> predicate) => mockContractInvoiceReferenceDbData.Where(predicate));
            mockDbRepo.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Data, bool>>>())).Returns((Expression<Func<DbModel.Data, bool>> predicate) => mockAssignmentType.Where(predicate));
            ContractInvoiceReferenceService = new Service.ContractInvoiceReferenceTypeService(mockMapper.Object, mockLogger.Object, mockContractinvoiceReferenceRepository.Object, mockContractRepository.Object, mockDbRepo.Object, InvoiceReferenceValidService);
            contractInvoiceReferenceTypeDomianModels.ToList().ForEach(x => x.RecordStatus = "M");
            var response = ContractInvoiceReferenceService.ModifyContractInvoiceReferenceType(contractInvoiceReferenceTypeDomianModels[0].ContractNumber, new List<DomModel.ContractInvoiceReferenceType> { contractInvoiceReferenceTypeDomianModels[0] });
            Assert.AreEqual("11", response.Code);
            Assert.IsNotNull(response.Messages);
            Assert.AreEqual("31", response.Messages.FirstOrDefault().Code);
        }

        [TestMethod]
        public void ThrowExceptionWhileDeletingContrcatInvoiceRefType()
        {

            mockContractRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Contract, bool>>>())).Throws(new Exception("Exception occured while performing some operation."));
            mockContractinvoiceReferenceRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.ContractInvoiceReference, bool>>>())).Returns((Expression<Func<DbModel.ContractInvoiceReference, bool>> predicate) => mockContractInvoiceReferenceDbData.Where(predicate));
            mockDbRepo.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Data, bool>>>())).Returns((Expression<Func<DbModel.Data, bool>> predicate) => mockAssignmentType.Where(predicate));

            ContractInvoiceReferenceService = new Service.ContractInvoiceReferenceTypeService(mockMapper.Object, mockLogger.Object, mockContractinvoiceReferenceRepository.Object, mockContractRepository.Object, mockDbRepo.Object, InvoiceReferenceValidService);
            contractInvoiceReferenceTypeDomianModels.ToList().ForEach(x => x.RecordStatus = "D");
            var response = ContractInvoiceReferenceService.DeleteContractInvoiceReferenceType(contractInvoiceReferenceTypeDomianModels[0].ContractNumber, new List<DomModel.ContractInvoiceReferenceType> { contractInvoiceReferenceTypeDomianModels[0] });
            Assert.AreEqual("11", response.Code);
            Assert.IsNotNull(response.Messages);
            Assert.AreEqual("31", response.Messages.FirstOrDefault().Code);
        }
        [TestMethod]
        public void GetAllDataAfterSavingInvoiceRef()
        {

            mockContractRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Contract, bool>>>())).Returns(mockContractDbData);
            mockContractinvoiceReferenceRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.ContractInvoiceReference, bool>>>())).Returns((Expression<Func<DbModel.ContractInvoiceReference, bool>> predicate) => mockContractInvoiceReferenceDbData.Where(predicate));
            mockDbRepo.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Data, bool>>>())).Returns((Expression<Func<DbModel.Data, bool>> predicate) => mockAssignmentType.Where(predicate));
            ContractInvoiceReferenceService = new Service.ContractInvoiceReferenceTypeService(mockMapper.Object, mockLogger.Object, mockContractinvoiceReferenceRepository.Object, mockContractRepository.Object, mockDbRepo.Object, InvoiceReferenceValidService);
            contractInvoiceReferenceTypeDomianModels.ToList().ForEach(x => x.RecordStatus = "N");
            var response = ContractInvoiceReferenceService.SaveContractInvoiceReferenceType(contractInvoiceReferenceTypeDomianModels[0].ContractNumber, new List<DomModel.ContractInvoiceReferenceType> { contractInvoiceReferenceTypeDomianModels[0] });
            mockContractinvoiceReferenceRepository.Verify(m => m.Add(It.IsAny<IList<DbModel.ContractInvoiceReference>>()), Times.AtLeastOnce);
        }
        [TestMethod]
        public void GetAllDataAfterUpdatingInvoiceRef()
        {

            mockContractRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Contract, bool>>>())).Returns(mockContractDbData);
            mockContractinvoiceReferenceRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.ContractInvoiceReference, bool>>>())).Returns((Expression<Func<DbModel.ContractInvoiceReference, bool>> predicate) => mockContractInvoiceReferenceDbData.Where(predicate));
            mockDbRepo.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Data, bool>>>())).Returns((Expression<Func<DbModel.Data, bool>> predicate) => mockAssignmentType.Where(predicate));
            ContractInvoiceReferenceService = new Service.ContractInvoiceReferenceTypeService(mockMapper.Object, mockLogger.Object, mockContractinvoiceReferenceRepository.Object, mockContractRepository.Object, mockDbRepo.Object, InvoiceReferenceValidService);
            contractInvoiceReferenceTypeDomianModels.ToList().ForEach(x => x.RecordStatus = "M");
            var response = ContractInvoiceReferenceService.ModifyContractInvoiceReferenceType(contractInvoiceReferenceTypeDomianModels[0].ContractNumber, new List<DomModel.ContractInvoiceReferenceType> { contractInvoiceReferenceTypeDomianModels[0] }, true, true);
            mockContractinvoiceReferenceRepository.Verify(m => m.Update(It.IsAny<DbModel.ContractInvoiceReference>()), Times.AtLeastOnce);
        }

        [TestMethod]
        public void GetAllDataAfterDeletingInvoiceRef()
        {

            mockContractRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Contract, bool>>>())).Returns(mockContractDbData);
            mockContractinvoiceReferenceRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.ContractInvoiceReference, bool>>>())).Returns((Expression<Func<DbModel.ContractInvoiceReference, bool>> predicate) => mockContractInvoiceReferenceDbData.Where(predicate));
            mockDbRepo.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Data, bool>>>())).Returns((Expression<Func<DbModel.Data, bool>> predicate) => mockAssignmentType.Where(predicate));

            ContractInvoiceReferenceService = new Service.ContractInvoiceReferenceTypeService(mockMapper.Object, mockLogger.Object, mockContractinvoiceReferenceRepository.Object, mockContractRepository.Object, mockDbRepo.Object, InvoiceReferenceValidService);
            contractInvoiceReferenceTypeDomianModels.ToList().ForEach(x => x.RecordStatus = "D");
            var response = ContractInvoiceReferenceService.DeleteContractInvoiceReferenceType(contractInvoiceReferenceTypeDomianModels[0].ContractNumber, new List<DomModel.ContractInvoiceReferenceType> { contractInvoiceReferenceTypeDomianModels[0] }, true, true);
            mockContractinvoiceReferenceRepository.Verify(m => m.Delete(It.IsAny<DbModel.ContractInvoiceReference>()), Times.AtLeastOnce);
        }
        [TestMethod]
        public void ShouldNotSaveDataWhenContractNumberIsNull()
        {

            mockContractRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Contract, bool>>>())).Returns(mockContractDbData);
            mockContractinvoiceReferenceRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.ContractInvoiceReference, bool>>>())).Returns((Expression<Func<DbModel.ContractInvoiceReference, bool>> predicate) => mockContractInvoiceReferenceDbData.Where(predicate));
            mockDbRepo.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Data, bool>>>())).Returns((Expression<Func<DbModel.Data, bool>> predicate) => mockAssignmentType.Where(predicate));
            ContractInvoiceReferenceService = new Service.ContractInvoiceReferenceTypeService(mockMapper.Object, mockLogger.Object, mockContractinvoiceReferenceRepository.Object, mockContractRepository.Object, mockDbRepo.Object, InvoiceReferenceValidService);
            contractInvoiceReferenceTypeDomianModels.ToList().ForEach(x => x.RecordStatus = "N");
            var response = ContractInvoiceReferenceService.SaveContractInvoiceReferenceType(null, new List<DomModel.ContractInvoiceReferenceType> { contractInvoiceReferenceTypeDomianModels[0] });
            Assert.AreEqual("11", response.Code);
        }
        [TestMethod]
        public void ShouldNotModifyDataWhenContractNumberIsNull()
        {

            mockContractRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Contract, bool>>>())).Returns(mockContractDbData);
            mockContractinvoiceReferenceRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.ContractInvoiceReference, bool>>>())).Returns((Expression<Func<DbModel.ContractInvoiceReference, bool>> predicate) => mockContractInvoiceReferenceDbData.Where(predicate));
            mockDbRepo.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Data, bool>>>())).Returns((Expression<Func<DbModel.Data, bool>> predicate) => mockAssignmentType.Where(predicate));
            ContractInvoiceReferenceService = new Service.ContractInvoiceReferenceTypeService(mockMapper.Object, mockLogger.Object, mockContractinvoiceReferenceRepository.Object, mockContractRepository.Object, mockDbRepo.Object, InvoiceReferenceValidService);
            contractInvoiceReferenceTypeDomianModels.ToList().ForEach(x => x.RecordStatus = "M");
            var response = ContractInvoiceReferenceService.ModifyContractInvoiceReferenceType(null, new List<DomModel.ContractInvoiceReferenceType> { contractInvoiceReferenceTypeDomianModels[0] });
            Assert.AreEqual("11", response.Code);
        }
        [TestMethod]
        public void ShouldNotSaveDataWithContractNumberIsInvalid()
        {

            //mockContractRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Contract, bool>>>())).Returns(mockContractDbData);
            mockContractinvoiceReferenceRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.ContractInvoiceReference, bool>>>())).Returns((Expression<Func<DbModel.ContractInvoiceReference, bool>> predicate) => mockContractInvoiceReferenceDbData.Where(predicate));
            mockDbRepo.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Data, bool>>>())).Returns((Expression<Func<DbModel.Data, bool>> predicate) => mockAssignmentType.Where(predicate));
            ContractInvoiceReferenceService = new Service.ContractInvoiceReferenceTypeService(mockMapper.Object, mockLogger.Object, mockContractinvoiceReferenceRepository.Object, mockContractRepository.Object, mockDbRepo.Object, InvoiceReferenceValidService);
            contractInvoiceReferenceTypeDomianModels.ToList().ForEach(x => x.RecordStatus = "N");
            var response = ContractInvoiceReferenceService.SaveContractInvoiceReferenceType(null, new List<DomModel.ContractInvoiceReferenceType> { contractInvoiceReferenceTypeDomianModels[0] });
            Assert.AreEqual("11", response.Code);
        }

        [TestMethod]
        public void ShouldNotModifyDataWhenContractNumberIsInvalid()
        {

            // mockContractRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Contract, bool>>>())).Returns(mockContractDbData);
            mockContractinvoiceReferenceRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.ContractInvoiceReference, bool>>>())).Returns((Expression<Func<DbModel.ContractInvoiceReference, bool>> predicate) => mockContractInvoiceReferenceDbData.Where(predicate));
            mockDbRepo.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Data, bool>>>())).Returns((Expression<Func<DbModel.Data, bool>> predicate) => mockAssignmentType.Where(predicate));
            ContractInvoiceReferenceService = new Service.ContractInvoiceReferenceTypeService(mockMapper.Object, mockLogger.Object, mockContractinvoiceReferenceRepository.Object, mockContractRepository.Object, mockDbRepo.Object, InvoiceReferenceValidService);
            contractInvoiceReferenceTypeDomianModels.ToList().ForEach(x => x.RecordStatus = "M");
            var response = ContractInvoiceReferenceService.ModifyContractInvoiceReferenceType(contractInvoiceReferenceTypeDomianModels[0].ContractNumber, new List<DomModel.ContractInvoiceReferenceType> { contractInvoiceReferenceTypeDomianModels[0] });
            Assert.AreEqual("11", response.Code);
        }
        [TestMethod]
        public void ValidationOfAssignmentRefWhileSave()
        {

            mockContractRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Contract, bool>>>())).Returns(mockContractDbData);
            mockContractinvoiceReferenceRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.ContractInvoiceReference, bool>>>())).Returns((Expression<Func<DbModel.ContractInvoiceReference, bool>> predicate) => mockContractInvoiceReferenceDbData.Where(predicate));
            mockDbRepo.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Data, bool>>>())).Returns((Expression<Func<DbModel.Data, bool>> predicate) => mockAssignmentType.Where(predicate));
            ContractInvoiceReferenceService = new Service.ContractInvoiceReferenceTypeService(mockMapper.Object, mockLogger.Object, mockContractinvoiceReferenceRepository.Object, mockContractRepository.Object, mockDbRepo.Object, InvoiceReferenceValidService);
            contractInvoiceReferenceTypeDomianModels.ToList().ForEach(x => x.RecordStatus = "N");
            contractInvoiceReferenceTypeDomianModels[0].ReferenceType = "Invalid";
            var response = ContractInvoiceReferenceService.SaveContractInvoiceReferenceType(contractInvoiceReferenceTypeDomianModels[0].ContractNumber, new List<DomModel.ContractInvoiceReferenceType> { contractInvoiceReferenceTypeDomianModels[0] });
            Assert.AreEqual("11", response.Code);
        }

        [TestMethod]
        public void ValidationOfAssignmentRefWhileUpdate()
        {

            mockContractRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Contract, bool>>>())).Returns(mockContractDbData);
            mockContractinvoiceReferenceRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.ContractInvoiceReference, bool>>>())).Returns((Expression<Func<DbModel.ContractInvoiceReference, bool>> predicate) => mockContractInvoiceReferenceDbData.Where(predicate));
            mockDbRepo.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Data, bool>>>())).Returns((Expression<Func<DbModel.Data, bool>> predicate) => mockAssignmentType.Where(predicate));
            ContractInvoiceReferenceService = new Service.ContractInvoiceReferenceTypeService(mockMapper.Object, mockLogger.Object, mockContractinvoiceReferenceRepository.Object, mockContractRepository.Object, mockDbRepo.Object, InvoiceReferenceValidService);
            contractInvoiceReferenceTypeDomianModels.ToList().ForEach(x => x.RecordStatus = "M");
            contractInvoiceReferenceTypeDomianModels[0].ReferenceType = "invalid";
            var response = ContractInvoiceReferenceService.ModifyContractInvoiceReferenceType(contractInvoiceReferenceTypeDomianModels[0].ContractNumber, new List<DomModel.ContractInvoiceReferenceType> { contractInvoiceReferenceTypeDomianModels[0] });
            Assert.AreEqual("11", response.Code);
        }
        [TestMethod]
        public void ValidationOfUpdateCount()
        {

            mockContractRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Contract, bool>>>())).Returns(mockContractDbData);
            mockContractinvoiceReferenceRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.ContractInvoiceReference, bool>>>())).Returns((Expression<Func<DbModel.ContractInvoiceReference, bool>> predicate) => mockContractInvoiceReferenceDbData.Where(predicate));
            mockDbRepo.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Data, bool>>>())).Returns((Expression<Func<DbModel.Data, bool>> predicate) => mockAssignmentType.Where(predicate));
            ContractInvoiceReferenceService = new Service.ContractInvoiceReferenceTypeService(mockMapper.Object, mockLogger.Object, mockContractinvoiceReferenceRepository.Object, mockContractRepository.Object, mockDbRepo.Object, InvoiceReferenceValidService);
            contractInvoiceReferenceTypeDomianModels.ToList().ForEach(x => x.RecordStatus = "M");
            contractInvoiceReferenceTypeDomianModels[0].UpdateCount = 5;
            var response = ContractInvoiceReferenceService.ModifyContractInvoiceReferenceType(contractInvoiceReferenceTypeDomianModels[0].ContractNumber, new List<DomModel.ContractInvoiceReferenceType> { contractInvoiceReferenceTypeDomianModels[0] });
            Assert.AreEqual("11", response.Code);
        }
        [TestMethod]
        public void ValidationOfExsistanceOfInvoiceRef()
        {

            mockContractRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Contract, bool>>>())).Returns(mockContractDbData);
            mockContractinvoiceReferenceRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.ContractInvoiceReference, bool>>>())).Returns((Expression<Func<DbModel.ContractInvoiceReference, bool>> predicate) => mockContractInvoiceReferenceDbData.Where(predicate));
            mockDbRepo.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Data, bool>>>())).Returns((Expression<Func<DbModel.Data, bool>> predicate) => mockAssignmentType.Where(predicate));
            ContractInvoiceReferenceService = new Service.ContractInvoiceReferenceTypeService(mockMapper.Object, mockLogger.Object, mockContractinvoiceReferenceRepository.Object, mockContractRepository.Object, mockDbRepo.Object, InvoiceReferenceValidService);
            contractInvoiceReferenceTypeDomianModels.ToList().ForEach(x => x.RecordStatus = "M");
            contractInvoiceReferenceTypeDomianModels[0].ReferenceType = "ABC";
            var response = ContractInvoiceReferenceService.ModifyContractInvoiceReferenceType(contractInvoiceReferenceTypeDomianModels[0].ContractNumber, new List<DomModel.ContractInvoiceReferenceType> { contractInvoiceReferenceTypeDomianModels[0] });
            Assert.AreEqual("11", response.Code);
        }

        [TestMethod]
        public void ISRecordIdIsValidForUpdate()
        {

            mockContractRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Contract, bool>>>())).Returns(mockContractDbData);
            mockContractinvoiceReferenceRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.ContractInvoiceReference, bool>>>())).Returns((Expression<Func<DbModel.ContractInvoiceReference, bool>> predicate) => mockContractInvoiceReferenceDbData.Where(predicate));
            mockDbRepo.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Data, bool>>>())).Returns((Expression<Func<DbModel.Data, bool>> predicate) => mockAssignmentType.Where(predicate));
            ContractInvoiceReferenceService = new Service.ContractInvoiceReferenceTypeService(mockMapper.Object, mockLogger.Object, mockContractinvoiceReferenceRepository.Object, mockContractRepository.Object, mockDbRepo.Object, InvoiceReferenceValidService);
            contractInvoiceReferenceTypeDomianModels.ToList().ForEach(x => x.RecordStatus = "M");
            contractInvoiceReferenceTypeDomianModels[0].ContractInvoiceReferenceTypeId = 5;
            var response = ContractInvoiceReferenceService.ModifyContractInvoiceReferenceType(contractInvoiceReferenceTypeDomianModels[0].ContractNumber, new List<DomModel.ContractInvoiceReferenceType> { contractInvoiceReferenceTypeDomianModels[0] });
            Assert.AreEqual("11", response.Code);




        }
        [TestMethod]
        public void ISRecordIdIsValidForDelete()
        {

            mockContractRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Contract, bool>>>())).Returns(mockContractDbData);
            mockContractinvoiceReferenceRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.ContractInvoiceReference, bool>>>())).Returns((Expression<Func<DbModel.ContractInvoiceReference, bool>> predicate) => mockContractInvoiceReferenceDbData.Where(predicate));
            mockDbRepo.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Data, bool>>>())).Returns((Expression<Func<DbModel.Data, bool>> predicate) => mockAssignmentType.Where(predicate));
            ContractInvoiceReferenceService = new Service.ContractInvoiceReferenceTypeService(mockMapper.Object, mockLogger.Object, mockContractinvoiceReferenceRepository.Object, mockContractRepository.Object, mockDbRepo.Object, InvoiceReferenceValidService);
            contractInvoiceReferenceTypeDomianModels.ToList().ForEach(x => x.RecordStatus = "D");
            contractInvoiceReferenceTypeDomianModels[0].ContractInvoiceReferenceTypeId = 5;
            var response = ContractInvoiceReferenceService.DeleteContractInvoiceReferenceType(contractInvoiceReferenceTypeDomianModels[0].ContractNumber, new List<DomModel.ContractInvoiceReferenceType> { contractInvoiceReferenceTypeDomianModels[0] });
            Assert.AreEqual("11", response.Code);




        }

    }
}

