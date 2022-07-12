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
using dbRepo = Evolution.DbRepository.Interfaces.Master;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Service = Evolution.Contract.Core.Services;
using Evolution.UnitTest.UnitTestCases;
using ValidService = Evolution.Contract.Domain.Interfaces.Validations;

namespace Evolution.UnitTests.UnitTestCases.Contract.Services
{
    [TestClass]
     public class ContractInvoiceAttachmentServiceUnitTestCase : BaseTestCase
    {
        ServiceDomainData.IContractInvoiceAttachmentService ContractInvoiceAttachmentService = null;
        Mock<DomainData.IContractInvoiceAttachmentRepository> mockContractinvoiceAttachmentRepository = null;
        Mock<DomainData.IContractRepository> mockContractRepository = null;
        Mock<Masterdata.IModuleDocumentTypeRepository> mockDocumenttype = null;
        IList<DomModel.ContractInvoiceAttachment> contractInvoiceAttachmentDomianModels = null;
        Mock<IAppLogger<Service.ContractInvoiceAttachmentService>> mockLogger = null;
        Mock<IMapper> mockMapper = null;
        IQueryable<DbModel.Contract> mockContractDbData = null;
        IQueryable<DbModel.ContractInvoiceAttachment> mockContractInvoiceAttachmentDbData = null;
        IQueryable<DbModel.ModuleDocumentType> mockModuleDocumentType = null;
        ValidService.IContractInvoiceAttachmentValidationService InvoiceAttachValidService = null;

        [TestInitialize]
        public void InitializeContractInvoiceAttachmentService()
        {
            mockContractinvoiceAttachmentRepository = new Mock<DomainData.IContractInvoiceAttachmentRepository>();
            mockContractRepository = new Mock<DomainData.IContractRepository>();
            mockDocumenttype = new Mock<Masterdata.IModuleDocumentTypeRepository>();
            contractInvoiceAttachmentDomianModels = MockContractDomainModel.GetDomainModelInvoiceAttachments();
            mockLogger = new Mock<IAppLogger<Service.ContractInvoiceAttachmentService>>();
            mockMapper = new Mock<IMapper>();
            mockContractDbData = MockContract.GetDbModelContract();
            mockContractInvoiceAttachmentDbData = MockContract.GetDbModelInvoiceAttachments();
            mockModuleDocumentType = MockContract.GetmoduleDocumentTypeMockData();

        }
        [TestMethod]
        public void FetchAllContractInvoiceAttachmentWithoutSearchValue()
        {
            mockContractinvoiceAttachmentRepository.Setup(x => x.Search(It.IsAny<DomModel.ContractInvoiceAttachment>())).Returns(contractInvoiceAttachmentDomianModels);
            ContractInvoiceAttachmentService = new Service.ContractInvoiceAttachmentService(mockMapper.Object, mockLogger.Object, mockContractinvoiceAttachmentRepository.Object,  mockContractRepository.Object, mockDocumenttype.Object, InvoiceAttachValidService);
            var response = ContractInvoiceAttachmentService.GetContractInvoiceAttachment(new DomModel.ContractInvoiceAttachment());
            Assert.AreEqual("1", response.Code);
            Assert.AreEqual(2, (response.Result as List<DomModel.ContractInvoiceAttachment>).Count);
        }
        [TestMethod]
        public void FetchAllContractInvoiceAttachmentWithSearchValue()
        {
            mockContractinvoiceAttachmentRepository.Setup(x => x.Search(It.Is<DomModel.ContractInvoiceAttachment>(c => c.ContractNumber == "SU02412/0001"))).Returns(new List<DomModel.ContractInvoiceAttachment> { contractInvoiceAttachmentDomianModels[0] });
            ContractInvoiceAttachmentService = new Service.ContractInvoiceAttachmentService(mockMapper.Object, mockLogger.Object, mockContractinvoiceAttachmentRepository.Object, mockContractRepository.Object, mockDocumenttype.Object, InvoiceAttachValidService);
            var response = ContractInvoiceAttachmentService.GetContractInvoiceAttachment(new DomModel.ContractInvoiceAttachment { ContractNumber = "SU02412/0001" });


            Assert.AreEqual("1", response.Code);
            Assert.AreEqual(1, (response.Result as List<DomModel.ContractInvoiceAttachment>).Count);
        }

        [TestMethod]
        public void FetchContractInvoiceAttachmentListWithNullSearchModel()
        {
            mockContractinvoiceAttachmentRepository.Setup(x => x.Search(It.IsAny<DomModel.ContractInvoiceAttachment>())).Returns(contractInvoiceAttachmentDomianModels);

            ContractInvoiceAttachmentService = new Service.ContractInvoiceAttachmentService(mockMapper.Object, mockLogger.Object, mockContractinvoiceAttachmentRepository.Object, mockContractRepository.Object, mockDocumenttype.Object, InvoiceAttachValidService);

            var response = ContractInvoiceAttachmentService.GetContractInvoiceAttachment(null);

            Assert.AreEqual("1", response.Code);
            Assert.AreEqual(2, (response.Result as List<DomModel.ContractInvoiceAttachment>).Count);
        }
        [TestMethod]
        public void ThrowsExceptionWhileFetchingInvoiceAttachment()
        {
            mockContractinvoiceAttachmentRepository.Setup(x => x.Search(It.IsAny<DomModel.ContractInvoiceAttachment>())).Throws(new Exception("Exception occured while performing some operation."));
            ContractInvoiceAttachmentService = new Service.ContractInvoiceAttachmentService(mockMapper.Object, mockLogger.Object, mockContractinvoiceAttachmentRepository.Object, mockContractRepository.Object, mockDocumenttype.Object, InvoiceAttachValidService);

            var response = ContractInvoiceAttachmentService.GetContractInvoiceAttachment(new DomModel.ContractInvoiceAttachment());

            Assert.AreEqual("11", response.Code);
            Assert.IsNotNull(response.Messages);
            Assert.AreEqual("31", response.Messages.FirstOrDefault().Code);
        }

        [TestMethod]
        public void FetchContractInvoiceAttachmentListByWildCardSearchWithNameStartWith()
        {
            mockContractinvoiceAttachmentRepository.Setup(x => x.Search(It.Is<DomModel.ContractInvoiceAttachment>(c => c.DocumentType.StartsWith("Con")))).Returns(new List<DomModel.ContractInvoiceAttachment> { contractInvoiceAttachmentDomianModels[0] });

            ContractInvoiceAttachmentService = new Service.ContractInvoiceAttachmentService(mockMapper.Object, mockLogger.Object, mockContractinvoiceAttachmentRepository.Object, mockContractRepository.Object, mockDocumenttype.Object, InvoiceAttachValidService);

            var response = ContractInvoiceAttachmentService.GetContractInvoiceAttachment(new DomModel.ContractInvoiceAttachment { DocumentType = "Con*" });

            Assert.AreEqual("1", response.Code);
            Assert.AreEqual(1, (response.Result as List<DomModel.ContractInvoiceAttachment>).Count);
        }
        [TestMethod]
        public void FetchContractInvoiceAttachmentListByWildCardSearchWithNameEndsWith()
        { 
            mockContractinvoiceAttachmentRepository.Setup(x => x.Search(It.Is<DomModel.ContractInvoiceAttachment>(c => c.DocumentType.EndsWith("Report")))).Returns(new List<DomModel.ContractInvoiceAttachment> { contractInvoiceAttachmentDomianModels[1] });

            ContractInvoiceAttachmentService = new Service.ContractInvoiceAttachmentService(mockMapper.Object, mockLogger.Object, mockContractinvoiceAttachmentRepository.Object, mockContractRepository.Object, mockDocumenttype.Object, InvoiceAttachValidService);

            var response = ContractInvoiceAttachmentService.GetContractInvoiceAttachment(new DomModel.ContractInvoiceAttachment { DocumentType = "*Report" });

            Assert.AreEqual("1", response.Code);
            Assert.AreEqual(1, (response.Result as List<DomModel.ContractInvoiceAttachment>).Count);
        }
        [TestMethod]
        public void FetchInvoiceAttachmentListByWildCardSearchWithDocumentNameContains()
        {
            mockContractinvoiceAttachmentRepository.Setup(x => x.Search(It.Is<DomModel.ContractInvoiceAttachment>(c => c.DocumentType.Contains("Report")))).Returns(new List<DomModel.ContractInvoiceAttachment> { contractInvoiceAttachmentDomianModels[1] });

            ContractInvoiceAttachmentService = new Service.ContractInvoiceAttachmentService(mockMapper.Object, mockLogger.Object, mockContractinvoiceAttachmentRepository.Object, mockContractRepository.Object, mockDocumenttype.Object, InvoiceAttachValidService);

            var response = ContractInvoiceAttachmentService.GetContractInvoiceAttachment(new DomModel.ContractInvoiceAttachment { DocumentType = "*Report*" });

            Assert.AreEqual("1", response.Code);
            Assert.AreEqual(1, (response.Result as List<DomModel.ContractInvoiceAttachment>).Count);
        }

        [TestMethod]
        public void SaveContrcatInvoiceAttachment()
        {

            mockContractRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Contract, bool>>>())).Returns(mockContractDbData);
            mockContractinvoiceAttachmentRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.ContractInvoiceAttachment, bool>>>())).Returns((Expression<Func<DbModel.ContractInvoiceAttachment, bool>> predicate) => mockContractInvoiceAttachmentDbData.Where(predicate));
            mockDocumenttype.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.ModuleDocumentType, bool>>>())).Returns((Expression<Func<DbModel.ModuleDocumentType, bool>> predicate) => mockModuleDocumentType.Where(predicate));
            ContractInvoiceAttachmentService = new Service.ContractInvoiceAttachmentService(mockMapper.Object, mockLogger.Object, mockContractinvoiceAttachmentRepository.Object, mockContractRepository.Object, mockDocumenttype.Object, InvoiceAttachValidService);
            contractInvoiceAttachmentDomianModels.ToList().ForEach(x => x.RecordStatus = "N");
              var response = ContractInvoiceAttachmentService.SaveContractInvoiceAttachment(contractInvoiceAttachmentDomianModels[0].ContractNumber, new List<DomModel.ContractInvoiceAttachment> { contractInvoiceAttachmentDomianModels[0] });
            mockContractinvoiceAttachmentRepository.Verify(m => m.Add(It.IsAny<IList<DbModel.ContractInvoiceAttachment>>()), Times.AtLeastOnce);
        }
        [TestMethod]
        public void ModifyContractInvoiceAttachment()
        {

            mockContractRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Contract, bool>>>())).Returns(mockContractDbData);
            mockContractinvoiceAttachmentRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.ContractInvoiceAttachment, bool>>>())).Returns((Expression<Func<DbModel.ContractInvoiceAttachment, bool>> predicate) => mockContractInvoiceAttachmentDbData.Where(predicate));
            mockDocumenttype.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.ModuleDocumentType, bool>>>())).Returns((Expression<Func<DbModel.ModuleDocumentType, bool>> predicate) => mockModuleDocumentType.Where(predicate));
            ContractInvoiceAttachmentService = new Service.ContractInvoiceAttachmentService(mockMapper.Object, mockLogger.Object, mockContractinvoiceAttachmentRepository.Object, mockContractRepository.Object, mockDocumenttype.Object, InvoiceAttachValidService);
            contractInvoiceAttachmentDomianModels.ToList().ForEach(x => x.RecordStatus = "M");
            var response = ContractInvoiceAttachmentService.ModifyContractInvoiceAttachment(contractInvoiceAttachmentDomianModels[0].ContractNumber, new List<DomModel.ContractInvoiceAttachment> { contractInvoiceAttachmentDomianModels[0] });
            mockContractinvoiceAttachmentRepository.Verify(m => m.Update(It.IsAny<DbModel.ContractInvoiceAttachment>()), Times.AtLeastOnce);
        }

        [TestMethod]
        public void DeleteContractInvoiceAttachment()
        {

            mockContractRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Contract, bool>>>())).Returns(mockContractDbData);
            mockContractinvoiceAttachmentRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.ContractInvoiceAttachment, bool>>>())).Returns((Expression<Func<DbModel.ContractInvoiceAttachment, bool>> predicate) => mockContractInvoiceAttachmentDbData.Where(predicate));
            mockDocumenttype.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.ModuleDocumentType, bool>>>())).Returns((Expression<Func<DbModel.ModuleDocumentType, bool>> predicate) => mockModuleDocumentType.Where(predicate));
            ContractInvoiceAttachmentService = new Service.ContractInvoiceAttachmentService(mockMapper.Object, mockLogger.Object, mockContractinvoiceAttachmentRepository.Object, mockContractRepository.Object, mockDocumenttype.Object, InvoiceAttachValidService);
            contractInvoiceAttachmentDomianModels.ToList().ForEach(x => x.RecordStatus = "D");
            var response = ContractInvoiceAttachmentService.DeleteContractInvoiceAttachment(contractInvoiceAttachmentDomianModels[0].ContractNumber, new List<DomModel.ContractInvoiceAttachment> { contractInvoiceAttachmentDomianModels[0] });
            mockContractinvoiceAttachmentRepository.Verify(m => m.Delete(It.IsAny<DbModel.ContractInvoiceAttachment>()), Times.AtLeastOnce);
        }
        [TestMethod]
        public void ThrowingExceptionWhileSavingContrcatInvoiceAttachment()
        {

            mockContractRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Contract, bool>>>())).Throws(new Exception("Exception occured while performing some operation."));
            mockContractinvoiceAttachmentRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.ContractInvoiceAttachment, bool>>>())).Returns((Expression<Func<DbModel.ContractInvoiceAttachment, bool>> predicate) => mockContractInvoiceAttachmentDbData.Where(predicate));
            mockDocumenttype.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.ModuleDocumentType, bool>>>())).Returns((Expression<Func<DbModel.ModuleDocumentType, bool>> predicate) => mockModuleDocumentType.Where(predicate));
            ContractInvoiceAttachmentService = new Service.ContractInvoiceAttachmentService(mockMapper.Object, mockLogger.Object, mockContractinvoiceAttachmentRepository.Object, mockContractRepository.Object, mockDocumenttype.Object, InvoiceAttachValidService);
            contractInvoiceAttachmentDomianModels.ToList().ForEach(x => x.RecordStatus = "N");
            var response = ContractInvoiceAttachmentService.SaveContractInvoiceAttachment(contractInvoiceAttachmentDomianModels[0].ContractNumber, new List<DomModel.ContractInvoiceAttachment> { contractInvoiceAttachmentDomianModels[0] });
            Assert.AreEqual("11", response.Code);
            Assert.IsNotNull(response.Messages);
            Assert.AreEqual("31", response.Messages.FirstOrDefault().Code);
        }

        [TestMethod]
        public void ThrowingExceptionWhileUpdateContrcatInvoiceAttachment()
        {

            mockContractRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Contract, bool>>>())).Throws(new Exception("Exception occured while performing some operation."));
            mockContractinvoiceAttachmentRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.ContractInvoiceAttachment, bool>>>())).Returns((Expression<Func<DbModel.ContractInvoiceAttachment, bool>> predicate) => mockContractInvoiceAttachmentDbData.Where(predicate));
            mockDocumenttype.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.ModuleDocumentType, bool>>>())).Returns((Expression<Func<DbModel.ModuleDocumentType, bool>> predicate) => mockModuleDocumentType.Where(predicate));
            ContractInvoiceAttachmentService = new Service.ContractInvoiceAttachmentService(mockMapper.Object, mockLogger.Object, mockContractinvoiceAttachmentRepository.Object, mockContractRepository.Object, mockDocumenttype.Object, InvoiceAttachValidService);
            contractInvoiceAttachmentDomianModels.ToList().ForEach(x => x.RecordStatus = "M");
            var response = ContractInvoiceAttachmentService.ModifyContractInvoiceAttachment(contractInvoiceAttachmentDomianModels[0].ContractNumber, new List<DomModel.ContractInvoiceAttachment> { contractInvoiceAttachmentDomianModels[0] });
            Assert.AreEqual("11", response.Code);
            Assert.IsNotNull(response.Messages);
            Assert.AreEqual("31", response.Messages.FirstOrDefault().Code);
        }
        [TestMethod]
        public void ThrowingExceptionWhileDeleteContrcatInvoiceAttachment()
        {

            mockContractRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Contract, bool>>>())).Throws(new Exception("Exception occured while performing some operation."));
            mockContractinvoiceAttachmentRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.ContractInvoiceAttachment, bool>>>())).Returns((Expression<Func<DbModel.ContractInvoiceAttachment, bool>> predicate) => mockContractInvoiceAttachmentDbData.Where(predicate));
            mockDocumenttype.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.ModuleDocumentType, bool>>>())).Returns((Expression<Func<DbModel.ModuleDocumentType, bool>> predicate) => mockModuleDocumentType.Where(predicate));
            ContractInvoiceAttachmentService = new Service.ContractInvoiceAttachmentService(mockMapper.Object, mockLogger.Object, mockContractinvoiceAttachmentRepository.Object, mockContractRepository.Object, mockDocumenttype.Object, InvoiceAttachValidService);
            contractInvoiceAttachmentDomianModels.ToList().ForEach(x => x.RecordStatus = "D");
            var response = ContractInvoiceAttachmentService.DeleteContractInvoiceAttachment(contractInvoiceAttachmentDomianModels[0].ContractNumber, new List<DomModel.ContractInvoiceAttachment> { contractInvoiceAttachmentDomianModels[0] });
            Assert.AreEqual("11", response.Code);
            Assert.IsNotNull(response.Messages);
            Assert.AreEqual("31", response.Messages.FirstOrDefault().Code);
        }
        [TestMethod]
        public void GetAllDataAfterSavingContrcatInvoiceAttachment()
        {

            mockContractRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Contract, bool>>>())).Returns(mockContractDbData);
            mockContractinvoiceAttachmentRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.ContractInvoiceAttachment, bool>>>())).Returns((Expression<Func<DbModel.ContractInvoiceAttachment, bool>> predicate) => mockContractInvoiceAttachmentDbData.Where(predicate));
            mockDocumenttype.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.ModuleDocumentType, bool>>>())).Returns((Expression<Func<DbModel.ModuleDocumentType, bool>> predicate) => mockModuleDocumentType.Where(predicate));
            ContractInvoiceAttachmentService = new Service.ContractInvoiceAttachmentService(mockMapper.Object, mockLogger.Object, mockContractinvoiceAttachmentRepository.Object, mockContractRepository.Object, mockDocumenttype.Object, InvoiceAttachValidService);
            contractInvoiceAttachmentDomianModels.ToList().ForEach(x => x.RecordStatus = "N");
            var response = ContractInvoiceAttachmentService.SaveContractInvoiceAttachment(contractInvoiceAttachmentDomianModels[0].ContractNumber, new List<DomModel.ContractInvoiceAttachment> { contractInvoiceAttachmentDomianModels[0] },true,true);
            mockContractinvoiceAttachmentRepository.Verify(m => m.Add(It.IsAny<IList<DbModel.ContractInvoiceAttachment>>()), Times.AtLeastOnce);
        }
        [TestMethod]
        public void GetAllDataAftereModifingContractInvoiceAttachment()
        {

            mockContractRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Contract, bool>>>())).Returns(mockContractDbData);
            mockContractinvoiceAttachmentRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.ContractInvoiceAttachment, bool>>>())).Returns((Expression<Func<DbModel.ContractInvoiceAttachment, bool>> predicate) => mockContractInvoiceAttachmentDbData.Where(predicate));
            mockDocumenttype.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.ModuleDocumentType, bool>>>())).Returns((Expression<Func<DbModel.ModuleDocumentType, bool>> predicate) => mockModuleDocumentType.Where(predicate));
            ContractInvoiceAttachmentService = new Service.ContractInvoiceAttachmentService(mockMapper.Object, mockLogger.Object, mockContractinvoiceAttachmentRepository.Object, mockContractRepository.Object, mockDocumenttype.Object, InvoiceAttachValidService);
            contractInvoiceAttachmentDomianModels.ToList().ForEach(x => x.RecordStatus = "M");
            var response = ContractInvoiceAttachmentService.ModifyContractInvoiceAttachment(contractInvoiceAttachmentDomianModels[0].ContractNumber, new List<DomModel.ContractInvoiceAttachment> { contractInvoiceAttachmentDomianModels[0] },true,true);
            mockContractinvoiceAttachmentRepository.Verify(m => m.Update(It.IsAny<DbModel.ContractInvoiceAttachment>()), Times.AtLeastOnce);
        }

        [TestMethod]
        public void GetAllDataAfterDeletingContractInvoiceAttachment()
        {

            mockContractRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Contract, bool>>>())).Returns(mockContractDbData);
            mockContractinvoiceAttachmentRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.ContractInvoiceAttachment, bool>>>())).Returns((Expression<Func<DbModel.ContractInvoiceAttachment, bool>> predicate) => mockContractInvoiceAttachmentDbData.Where(predicate));
            mockDocumenttype.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.ModuleDocumentType, bool>>>())).Returns((Expression<Func<DbModel.ModuleDocumentType, bool>> predicate) => mockModuleDocumentType.Where(predicate));
            ContractInvoiceAttachmentService = new Service.ContractInvoiceAttachmentService(mockMapper.Object, mockLogger.Object, mockContractinvoiceAttachmentRepository.Object, mockContractRepository.Object, mockDocumenttype.Object, InvoiceAttachValidService);
            contractInvoiceAttachmentDomianModels.ToList().ForEach(x => x.RecordStatus = "D");
            var response = ContractInvoiceAttachmentService.DeleteContractInvoiceAttachment(contractInvoiceAttachmentDomianModels[0].ContractNumber, new List<DomModel.ContractInvoiceAttachment> { contractInvoiceAttachmentDomianModels[0] },true,true);
            mockContractinvoiceAttachmentRepository.Verify(m => m.Delete(It.IsAny<DbModel.ContractInvoiceAttachment>()), Times.AtLeastOnce);
        }

        [TestMethod]
        public void ShouldNotSaveContrcatInvoiceAttachmentWhenContractNumberIsNull()
        {

            mockContractRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Contract, bool>>>())).Returns(mockContractDbData);
            mockContractinvoiceAttachmentRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.ContractInvoiceAttachment, bool>>>())).Returns((Expression<Func<DbModel.ContractInvoiceAttachment, bool>> predicate) => mockContractInvoiceAttachmentDbData.Where(predicate));
            mockDocumenttype.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.ModuleDocumentType, bool>>>())).Returns((Expression<Func<DbModel.ModuleDocumentType, bool>> predicate) => mockModuleDocumentType.Where(predicate));
            ContractInvoiceAttachmentService = new Service.ContractInvoiceAttachmentService(mockMapper.Object, mockLogger.Object, mockContractinvoiceAttachmentRepository.Object, mockContractRepository.Object, mockDocumenttype.Object, InvoiceAttachValidService);
            contractInvoiceAttachmentDomianModels.ToList().ForEach(x => x.RecordStatus = "N");
            var response = ContractInvoiceAttachmentService.SaveContractInvoiceAttachment(null, new List<DomModel.ContractInvoiceAttachment> { contractInvoiceAttachmentDomianModels[0] });
            Assert.AreEqual("11", response.Code);

        }

        [TestMethod]
        public void ShouldNotUpdateContrcatInvoiceAttachmentWhenContractNumberIsNull()
        {

            mockContractRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Contract, bool>>>())).Returns(mockContractDbData);
            mockContractinvoiceAttachmentRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.ContractInvoiceAttachment, bool>>>())).Returns((Expression<Func<DbModel.ContractInvoiceAttachment, bool>> predicate) => mockContractInvoiceAttachmentDbData.Where(predicate));
            mockDocumenttype.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.ModuleDocumentType, bool>>>())).Returns((Expression<Func<DbModel.ModuleDocumentType, bool>> predicate) => mockModuleDocumentType.Where(predicate));
            ContractInvoiceAttachmentService = new Service.ContractInvoiceAttachmentService(mockMapper.Object, mockLogger.Object, mockContractinvoiceAttachmentRepository.Object, mockContractRepository.Object, mockDocumenttype.Object,InvoiceAttachValidService);
            contractInvoiceAttachmentDomianModels.ToList().ForEach(x => x.RecordStatus = "M");
            var response = ContractInvoiceAttachmentService.ModifyContractInvoiceAttachment(null, new List<DomModel.ContractInvoiceAttachment> { contractInvoiceAttachmentDomianModels[0] });
            Assert.AreEqual("11", response.Code);

        }
        [TestMethod]
        public void ShouldNotSaveContrcatInvoiceAttachmentWithInvalidContract()
        {

            //mockContractRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Contract, bool>>>())).Returns(mockContractDbData);
            mockContractinvoiceAttachmentRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.ContractInvoiceAttachment, bool>>>())).Returns((Expression<Func<DbModel.ContractInvoiceAttachment, bool>> predicate) => mockContractInvoiceAttachmentDbData.Where(predicate));
            mockDocumenttype.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.ModuleDocumentType, bool>>>())).Returns((Expression<Func<DbModel.ModuleDocumentType, bool>> predicate) => mockModuleDocumentType.Where(predicate));
            ContractInvoiceAttachmentService = new Service.ContractInvoiceAttachmentService(mockMapper.Object, mockLogger.Object, mockContractinvoiceAttachmentRepository.Object, mockContractRepository.Object, mockDocumenttype.Object, InvoiceAttachValidService);
            contractInvoiceAttachmentDomianModels.ToList().ForEach(x => x.RecordStatus = "N");
            var response = ContractInvoiceAttachmentService.SaveContractInvoiceAttachment(contractInvoiceAttachmentDomianModels[0].ContractNumber, new List<DomModel.ContractInvoiceAttachment> { contractInvoiceAttachmentDomianModels[0] });
            Assert.AreEqual("11", response.Code);

        }

        [TestMethod]
        public void ShouldNotUpdateContrcatInvoiceAttachmentWithInvalidContract()
        {

            //mockContractRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Contract, bool>>>())).Returns(mockContractDbData);
            mockContractinvoiceAttachmentRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.ContractInvoiceAttachment, bool>>>())).Returns((Expression<Func<DbModel.ContractInvoiceAttachment, bool>> predicate) => mockContractInvoiceAttachmentDbData.Where(predicate));
            mockDocumenttype.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.ModuleDocumentType, bool>>>())).Returns((Expression<Func<DbModel.ModuleDocumentType, bool>> predicate) => mockModuleDocumentType.Where(predicate));
            ContractInvoiceAttachmentService = new Service.ContractInvoiceAttachmentService(mockMapper.Object, mockLogger.Object, mockContractinvoiceAttachmentRepository.Object, mockContractRepository.Object, mockDocumenttype.Object, InvoiceAttachValidService);
            contractInvoiceAttachmentDomianModels.ToList().ForEach(x => x.RecordStatus = "M");
            var response = ContractInvoiceAttachmentService.ModifyContractInvoiceAttachment(contractInvoiceAttachmentDomianModels[0].ContractNumber, new List<DomModel.ContractInvoiceAttachment> { contractInvoiceAttachmentDomianModels[0] });
            Assert.AreEqual("11", response.Code);

        }
        [TestMethod]
        public void ValidationOfDocumentTypeWhileSave ()
        {

            mockContractRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Contract, bool>>>())).Returns(mockContractDbData);
            mockContractinvoiceAttachmentRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.ContractInvoiceAttachment, bool>>>())).Returns((Expression<Func<DbModel.ContractInvoiceAttachment, bool>> predicate) => mockContractInvoiceAttachmentDbData.Where(predicate));
            mockDocumenttype.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.ModuleDocumentType, bool>>>())).Returns((Expression<Func<DbModel.ModuleDocumentType, bool>> predicate) => mockModuleDocumentType.Where(predicate));
            ContractInvoiceAttachmentService = new Service.ContractInvoiceAttachmentService(mockMapper.Object, mockLogger.Object, mockContractinvoiceAttachmentRepository.Object, mockContractRepository.Object, mockDocumenttype.Object, InvoiceAttachValidService);
            contractInvoiceAttachmentDomianModels.ToList().ForEach(x => x.RecordStatus = "N");
            contractInvoiceAttachmentDomianModels[0].DocumentType = "Contract1";
            var response = ContractInvoiceAttachmentService.SaveContractInvoiceAttachment(contractInvoiceAttachmentDomianModels[0].ContractNumber, new List<DomModel.ContractInvoiceAttachment> { contractInvoiceAttachmentDomianModels[0] });
            Assert.AreEqual("11", response.Code);

        }
        [TestMethod]
        public void ValidationOfDocumentTypeWhileUpdate()
        {

            mockContractRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Contract, bool>>>())).Returns(mockContractDbData);
            mockContractinvoiceAttachmentRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.ContractInvoiceAttachment, bool>>>())).Returns((Expression<Func<DbModel.ContractInvoiceAttachment, bool>> predicate) => mockContractInvoiceAttachmentDbData.Where(predicate));
            mockDocumenttype.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.ModuleDocumentType, bool>>>())).Returns((Expression<Func<DbModel.ModuleDocumentType, bool>> predicate) => mockModuleDocumentType.Where(predicate));
            ContractInvoiceAttachmentService = new Service.ContractInvoiceAttachmentService(mockMapper.Object, mockLogger.Object, mockContractinvoiceAttachmentRepository.Object, mockContractRepository.Object, mockDocumenttype.Object, InvoiceAttachValidService);
            contractInvoiceAttachmentDomianModels.ToList().ForEach(x => x.RecordStatus = "M");
            contractInvoiceAttachmentDomianModels[0].DocumentType = "Contract1";
            var response = ContractInvoiceAttachmentService.ModifyContractInvoiceAttachment(contractInvoiceAttachmentDomianModels[0].ContractNumber, new List<DomModel.ContractInvoiceAttachment> { contractInvoiceAttachmentDomianModels[0] });
            Assert.AreEqual("11", response.Code);

        }
        [TestMethod]
        public void ValidationOfDocumentTypeWhileDelete()
        {

            mockContractRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Contract, bool>>>())).Returns(mockContractDbData);
            mockContractinvoiceAttachmentRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.ContractInvoiceAttachment, bool>>>())).Returns((Expression<Func<DbModel.ContractInvoiceAttachment, bool>> predicate) => mockContractInvoiceAttachmentDbData.Where(predicate));
            mockDocumenttype.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.ModuleDocumentType, bool>>>())).Returns((Expression<Func<DbModel.ModuleDocumentType, bool>> predicate) => mockModuleDocumentType.Where(predicate));
            ContractInvoiceAttachmentService = new Service.ContractInvoiceAttachmentService(mockMapper.Object, mockLogger.Object, mockContractinvoiceAttachmentRepository.Object, mockContractRepository.Object, mockDocumenttype.Object, InvoiceAttachValidService);
            contractInvoiceAttachmentDomianModels.ToList().ForEach(x => x.RecordStatus = "M");
            contractInvoiceAttachmentDomianModels[0].DocumentType = "Contract1";
            var response = ContractInvoiceAttachmentService.ModifyContractInvoiceAttachment(contractInvoiceAttachmentDomianModels[0].ContractNumber, new List<DomModel.ContractInvoiceAttachment> { contractInvoiceAttachmentDomianModels[0] });
            Assert.AreEqual("11", response.Code);

        }

        [TestMethod]
        public void ValidationOfUpdatingRecordId()
        {

            mockContractRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Contract, bool>>>())).Returns(mockContractDbData);
            mockContractinvoiceAttachmentRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.ContractInvoiceAttachment, bool>>>())).Returns((Expression<Func<DbModel.ContractInvoiceAttachment, bool>> predicate) => mockContractInvoiceAttachmentDbData.Where(predicate));
            mockDocumenttype.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.ModuleDocumentType, bool>>>())).Returns((Expression<Func<DbModel.ModuleDocumentType, bool>> predicate) => mockModuleDocumentType.Where(predicate));
            ContractInvoiceAttachmentService = new Service.ContractInvoiceAttachmentService(mockMapper.Object, mockLogger.Object, mockContractinvoiceAttachmentRepository.Object, mockContractRepository.Object, mockDocumenttype.Object, InvoiceAttachValidService);
            contractInvoiceAttachmentDomianModels.ToList().ForEach(x => x.RecordStatus = "M");
            contractInvoiceAttachmentDomianModels[0].ContractInvoiceAttachmentId = 5;
            var response = ContractInvoiceAttachmentService.ModifyContractInvoiceAttachment(contractInvoiceAttachmentDomianModels[0].ContractNumber, new List<DomModel.ContractInvoiceAttachment> { contractInvoiceAttachmentDomianModels[0] });
            Assert.AreEqual("11", response.Code);

        }
        [TestMethod]
        public void ValidationOfDeletingRecordId()
        {

            mockContractRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Contract, bool>>>())).Returns(mockContractDbData);
            mockContractinvoiceAttachmentRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.ContractInvoiceAttachment, bool>>>())).Returns((Expression<Func<DbModel.ContractInvoiceAttachment, bool>> predicate) => mockContractInvoiceAttachmentDbData.Where(predicate));
            mockDocumenttype.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.ModuleDocumentType, bool>>>())).Returns((Expression<Func<DbModel.ModuleDocumentType, bool>> predicate) => mockModuleDocumentType.Where(predicate));
            ContractInvoiceAttachmentService = new Service.ContractInvoiceAttachmentService(mockMapper.Object, mockLogger.Object, mockContractinvoiceAttachmentRepository.Object, mockContractRepository.Object, mockDocumenttype.Object, InvoiceAttachValidService);
            contractInvoiceAttachmentDomianModels.ToList().ForEach(x => x.RecordStatus = "D");
            contractInvoiceAttachmentDomianModels[0].ContractInvoiceAttachmentId = 5;
            var response = ContractInvoiceAttachmentService.DeleteContractInvoiceAttachment(contractInvoiceAttachmentDomianModels[0].ContractNumber, new List<DomModel.ContractInvoiceAttachment> { contractInvoiceAttachmentDomianModels[0] });
            Assert.AreEqual("11", response.Code);

        }
        [TestMethod]
        public void ValidationOfupdateCountMissMatch()
        {

            mockContractRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Contract, bool>>>())).Returns(mockContractDbData);
            mockContractinvoiceAttachmentRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.ContractInvoiceAttachment, bool>>>())).Returns((Expression<Func<DbModel.ContractInvoiceAttachment, bool>> predicate) => mockContractInvoiceAttachmentDbData.Where(predicate));
            mockDocumenttype.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.ModuleDocumentType, bool>>>())).Returns((Expression<Func<DbModel.ModuleDocumentType, bool>> predicate) => mockModuleDocumentType.Where(predicate));
            ContractInvoiceAttachmentService = new Service.ContractInvoiceAttachmentService(mockMapper.Object, mockLogger.Object, mockContractinvoiceAttachmentRepository.Object, mockContractRepository.Object, mockDocumenttype.Object, InvoiceAttachValidService);
            contractInvoiceAttachmentDomianModels.ToList().ForEach(x => x.RecordStatus = "M");
            contractInvoiceAttachmentDomianModels[0].UpdateCount = 5;
            var response = ContractInvoiceAttachmentService.ModifyContractInvoiceAttachment(contractInvoiceAttachmentDomianModels[0].ContractNumber, new List<DomModel.ContractInvoiceAttachment> { contractInvoiceAttachmentDomianModels[0] });
            Assert.AreEqual("11", response.Code);

        }


    

    }

}
