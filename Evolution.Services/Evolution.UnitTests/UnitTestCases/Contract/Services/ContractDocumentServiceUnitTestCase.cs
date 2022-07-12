//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using System;
//using System.Collections.Generic;
//using System.Text;
//using ServiceDomainData = Evolution.Contract.Domain.Interfaces.Contracts;
//using DomainData = Evolution.Contract.Domain.Interfaces.Data;
//using Moq;
//using DomModel = Evolution.Contract.Domain.Models.Contracts;
//using Evolution.Logging.Interfaces;
//using AutoMapper;
//using DbModel = Evolution.DbRepository.Models;
//using System.Linq;
//using Evolution.UnitTests.Mocks.Data.Contracts.Domain;
//using Evolution.UnitTests.Mocks.Data.Contracts.Db;
//using Masterdata = Evolution.Master.Domain.Interfaces.Data;
//using System.Linq.Expressions;
//using Microsoft.Extensions.Options;
//using MasterService = Evolution.Master.Domain.Interfaces;
//using Evolution.Common.Enums;
//using Evolution.Common.Models.Messages;
//using Evolution.UnitTest.UnitTestCases;
//using ValidService = Evolution.Contract.Domain.Interfaces.Validations;

//namespace Evolution.UnitTests.UnitTestCases.Contract.Services
//{
//    [TestClass]
//    public class ContractDocumentServiceUnitTestCase : BaseTestCase
//    {
//        ServiceDomainData.IContractDocumentService ContractDocumentService = null;
//        Mock<DomainData.IContractDocumentRepository> mockContractDocumentRepository = null;
//        Mock<DomainData.IContractRepository> mockContractRepository = null;
//        Mock<Masterdata.IModuleDocumentTypeRepository> mockDocumentTypeRepo = null;
//        IList<DomModel.ContractDocument> contractDocumentDomianModels = null;
//        Mock<MasterService.Services.IModuleDocumentTypeService> mockDocumentTypeService = null;
//        Mock<IAppLogger<Evolution.Contract.Core.Services.ContractDocumentService>> mockLogger = null;
//        IOptions<DbRepository.MongoModels.Settings> mockMongoContext = null;
//        IQueryable<DbModel.Contract> mockContractDbData = null;
//        IQueryable<DbModel.ContractDocument> mockContractDocumentDbData = null;
//        IQueryable<DbModel.ModuleDocumentType> mockModuleDocumentType = null;
//        ValidService.IContractDocumentValidationService ContractDocumnetValidService = null;

//        [TestInitialize]
//        public void InitializeContractDocumentService()
//        {
//            mockContractDocumentRepository = new Mock<DomainData.IContractDocumentRepository>();
//            mockContractRepository = new Mock<DomainData.IContractRepository>();
//            contractDocumentDomianModels = MockContractDomainModel.GetDomainModelContractDocument();
//            mockLogger = new Mock<IAppLogger<Evolution.Contract.Core.Services.ContractDocumentService>>();
//            mockMongoContext = Options.Create(new DbRepository.MongoModels.Settings() { ConnectionString = "mongodb://192.168.51.10:5300", Database = "EvolutionDocuments" });
//            mockContractDbData = MockContract.GetDbModelContract();
//            mockContractDocumentDbData = MockContract.GetDbModelContractDocuments();
//            mockDocumentTypeService = new Mock<MasterService.Services.IModuleDocumentTypeService>();
//            mockDocumentTypeRepo = new Mock<Masterdata.IModuleDocumentTypeRepository>();
//            mockModuleDocumentType = MockContract.GetmoduleDocumentTypeMockData();
//        }
//        [TestMethod]
//        public void FetchAllContractDocumentWithoutSearchValue()
//        {
//            mockContractDocumentRepository.Setup(x => x.Search(It.IsAny<DomModel.ContractDocument>())).Returns(contractDocumentDomianModels);
//            ContractDocumentService = new Evolution.Contract.Core.Services.ContractDocumentService(Mapper.Instance, mockLogger.Object, mockContractDocumentRepository.Object, mockContractRepository.Object, mockDocumentTypeService.Object, mockMongoContext, ContractDocumnetValidService);
//            var response = ContractDocumentService.GetContractDocument(new DomModel.ContractDocument());
//            Assert.AreEqual("1", response.Code);
//            Assert.AreEqual(2, (response.Result as List<DomModel.ContractDocument>).Count);
//        }
//        [TestMethod]
//        public void FetchAllContractDocumentWithSearchValue()
//        {
//            mockContractDocumentRepository.Setup(x => x.Search(It.Is<DomModel.ContractDocument>(c => c.ContractNumber == "SU0001/0001"))).Returns(new List<DomModel.ContractDocument> { contractDocumentDomianModels[0] });
//            ContractDocumentService = new Evolution.Contract.Core.Services.ContractDocumentService(Mapper.Instance, mockLogger.Object, mockContractDocumentRepository.Object, mockContractRepository.Object, mockDocumentTypeService.Object, mockMongoContext, ContractDocumnetValidService);
//            var response = ContractDocumentService.GetContractDocument(new DomModel.ContractDocument { ContractNumber = "SU0001/0001" });


//            Assert.AreEqual("1", response.Code);
//            Assert.AreEqual(1, (response.Result as List<DomModel.ContractDocument>).Count);
//        }

//        [TestMethod]
//        public void FetchContractDocumentListWithNullSearchModel()
//        {
//            mockContractDocumentRepository.Setup(x => x.Search(It.IsAny<DomModel.ContractDocument>())).Returns(contractDocumentDomianModels);

//            ContractDocumentService = new Evolution.Contract.Core.Services.ContractDocumentService(Mapper.Instance, mockLogger.Object, mockContractDocumentRepository.Object, mockContractRepository.Object, mockDocumentTypeService.Object, mockMongoContext, ContractDocumnetValidService);

//            var response = ContractDocumentService.GetContractDocument(null);

//            Assert.AreEqual("1", response.Code);
//            Assert.AreEqual(2, (response.Result as List<DomModel.ContractDocument>).Count);
//        }
//        [TestMethod]
//        public void FetchContractDocumentListByWildCardSearchWithDocumentNameStartWith()
//        {
//            mockContractDocumentRepository.Setup(x => x.Search(It.Is<DomModel.ContractDocument>(c => c.Name.StartsWith("C")))).Returns(new List<DomModel.ContractDocument> { contractDocumentDomianModels[1] });

//            ContractDocumentService = new Evolution.Contract.Core.Services.ContractDocumentService(Mapper.Instance, mockLogger.Object, mockContractDocumentRepository.Object, mockContractRepository.Object, mockDocumentTypeService.Object, mockMongoContext, ContractDocumnetValidService);

//            var response = ContractDocumentService.GetContractDocument(new DomModel.ContractDocument { Name = "C*" });

//            Assert.AreEqual("1", response.Code);
//            Assert.AreEqual(1, (response.Result as List<DomModel.ContractDocument>).Count);
//        }
//        [TestMethod]
//        public void FetchContractDocumentListByWildCardSearchWithDocumentNameEndWith()
//        {
//            mockContractDocumentRepository.Setup(x => x.Search(It.Is<DomModel.ContractDocument>(c => c.Name.EndsWith("Report")))).Returns(new List<DomModel.ContractDocument> { contractDocumentDomianModels[1] });

//            ContractDocumentService = new Evolution.Contract.Core.Services.ContractDocumentService(Mapper.Instance, mockLogger.Object, mockContractDocumentRepository.Object, mockContractRepository.Object, mockDocumentTypeService.Object, mockMongoContext, ContractDocumnetValidService);
//            var response = ContractDocumentService.GetContractDocument(new DomModel.ContractDocument { Name = "*Report" });

//            Assert.AreEqual("1", response.Code);
//            Assert.AreEqual(1, (response.Result as List<DomModel.ContractDocument>).Count);
//        }
//        [TestMethod]
//        public void FetchContractDocumentListByWildCardSearchWithDocumentNameContains()
//        {
//            mockContractDocumentRepository.Setup(x => x.Search(It.Is<DomModel.ContractDocument>(c => c.Name.Contains("Report")))).Returns(new List<DomModel.ContractDocument> { contractDocumentDomianModels[1] });

//            ContractDocumentService = new Evolution.Contract.Core.Services.ContractDocumentService(Mapper.Instance, mockLogger.Object, mockContractDocumentRepository.Object, mockContractRepository.Object, mockDocumentTypeService.Object, mockMongoContext, ContractDocumnetValidService);

//            var response = ContractDocumentService.GetContractDocument(new DomModel.ContractDocument { Name = "*Report*" });

//            Assert.AreEqual("1", response.Code);
//            Assert.AreEqual(1, (response.Result as List<DomModel.ContractDocument>).Count);
//        }
//        [TestMethod]
//        public void ThrowsExceptionWhileFetchingCompanyDocumentList()
//        {
//            mockContractDocumentRepository.Setup(x => x.Search(It.IsAny<DomModel.ContractDocument>())).Throws(new Exception("Exception occured while performing some operation."));
//            ContractDocumentService = new Evolution.Contract.Core.Services.ContractDocumentService(Mapper.Instance, mockLogger.Object, mockContractDocumentRepository.Object, mockContractRepository.Object, mockDocumentTypeService.Object, mockMongoContext, ContractDocumnetValidService);

//            var response = ContractDocumentService.GetContractDocument(new DomModel.ContractDocument());

//            Assert.AreEqual("11", response.Code);
//            Assert.IsNotNull(response.Messages);
//            Assert.AreEqual("31", response.Messages.FirstOrDefault().Code);
//        }
//        [TestMethod]
//        public void SaveContrcatDocuments()
//        {

//            mockContractRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Contract, bool>>>())).Returns(mockContractDbData);
//            mockContractDocumentRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.ContractDocument, bool>>>())).Returns((Expression<Func<DbModel.ContractDocument, bool>> predicate) => mockContractDocumentDbData.Where(predicate));
//            mockDocumentTypeRepo.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.ModuleDocumentType, bool>>>())).Returns((Expression<Func<DbModel.ModuleDocumentType, bool>> predicate) => mockModuleDocumentType.Where(predicate));

//            mockDocumentTypeService.Setup(moq => moq.IsValidDocumentType(DocumentModuleType.Contract, It.IsAny<IList<string>>(), ModuleType.Contract, ref It.Ref<List<MessageDetail>>.IsAny)).Returns(true);

//            ContractDocumentService = new Evolution.Contract.Core.Services.ContractDocumentService(Mapper.Instance, mockLogger.Object, mockContractDocumentRepository.Object, mockContractRepository.Object, mockDocumentTypeService.Object, mockMongoContext, ContractDocumnetValidService);
//            contractDocumentDomianModels.ToList().ForEach(x => x.RecordStatus = "N");
//            var response = ContractDocumentService.SaveContractDocument(contractDocumentDomianModels[0].ContractNumber, new List<DomModel.ContractDocument> { contractDocumentDomianModels[0] });
//            mockContractDocumentRepository.Verify(m => m.Add(It.IsAny<IList<DbModel.ContractDocument>>()), Times.AtLeastOnce);
//        }
//        [TestMethod]
//        public void ModifyContractDocument()
//        {
//            mockContractDocumentRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.ContractDocument, bool>>>())).Returns((Expression<Func<DbModel.ContractDocument, bool>> predicate) => mockContractDocumentDbData.Where(predicate));
//            mockContractRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Contract, bool>>>())).Returns(mockContractDbData);
//            mockDocumentTypeService.Setup(moq => moq.IsValidDocumentType(DocumentModuleType.Contract, It.IsAny<IList<string>>(), ModuleType.Contract, ref It.Ref<List<MessageDetail>>.IsAny)).Returns(true);

//            ContractDocumentService = new Evolution.Contract.Core.Services.ContractDocumentService(Mapper.Instance, mockLogger.Object, mockContractDocumentRepository.Object, mockContractRepository.Object, mockDocumentTypeService.Object, mockMongoContext, ContractDocumnetValidService);
//            contractDocumentDomianModels.ToList().ForEach(x => x.RecordStatus = "M");
//            var response = ContractDocumentService.ModifyContractDocument(contractDocumentDomianModels[0].ContractNumber, new List<DomModel.ContractDocument> { contractDocumentDomianModels[0] });

//            mockContractDocumentRepository.Verify(m => m.Update(It.IsAny<DbModel.ContractDocument>()), Times.AtLeast(1));
//        }


//        [TestMethod]
//        public void DeleteContractDocument()
//        {
//            mockContractDocumentRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.ContractDocument, bool>>>())).Returns((Expression<Func<DbModel.ContractDocument, bool>> predicate) => mockContractDocumentDbData.Where(predicate));
//            mockContractRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Contract, bool>>>())).Returns(mockContractDbData);
//            mockDocumentTypeService.Setup(moq => moq.IsValidDocumentType(DocumentModuleType.Contract, It.IsAny<IList<string>>(), ModuleType.Contract, ref It.Ref<List<MessageDetail>>.IsAny)).Returns(true);

//            ContractDocumentService = new Evolution.Contract.Core.Services.ContractDocumentService(Mapper.Instance, mockLogger.Object, mockContractDocumentRepository.Object, mockContractRepository.Object, mockDocumentTypeService.Object, mockMongoContext, ContractDocumnetValidService);
//            contractDocumentDomianModels.ToList().ForEach(x => x.RecordStatus = "D");
//            var response = ContractDocumentService.DeleteContractDocument(contractDocumentDomianModels[0].ContractNumber, new List<DomModel.ContractDocument> { contractDocumentDomianModels[0] });

//            mockContractDocumentRepository.Verify(m => m.Delete(It.IsAny<DbModel.ContractDocument>()), Times.AtLeast(1));
//        }

//        [TestMethod]
//        public void ThrowsExceptionWhileSavingDocument()
//        {

//            mockContractRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Contract, bool>>>())).Throws(new Exception("Exception occured while performing some operation."));

//            mockContractDocumentRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.ContractDocument, bool>>>())).Returns((Expression<Func<DbModel.ContractDocument, bool>> predicate) => mockContractDocumentDbData.Where(predicate));
//            mockDocumentTypeRepo.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.ModuleDocumentType, bool>>>())).Returns((Expression<Func<DbModel.ModuleDocumentType, bool>> predicate) => mockModuleDocumentType.Where(predicate));

//            mockDocumentTypeService.Setup(moq => moq.IsValidDocumentType(DocumentModuleType.Contract, It.IsAny<IList<string>>(), ModuleType.Contract, ref It.Ref<List<MessageDetail>>.IsAny)).Returns(true);

//            ContractDocumentService = new Evolution.Contract.Core.Services.ContractDocumentService(Mapper.Instance, mockLogger.Object, mockContractDocumentRepository.Object, mockContractRepository.Object, mockDocumentTypeService.Object, mockMongoContext, ContractDocumnetValidService);
//            contractDocumentDomianModels.ToList().ForEach(x => x.RecordStatus = "N");
//            var response = ContractDocumentService.SaveContractDocument(contractDocumentDomianModels[0].ContractNumber, new List<DomModel.ContractDocument> { contractDocumentDomianModels[0] });
//            Assert.AreEqual("11", response.Code);
//            Assert.IsNotNull(response.Messages);
//            Assert.AreEqual("31", response.Messages.FirstOrDefault().Code);

//        }

//        [TestMethod]
//        public void ThrowsExceptionWhileUpdatingDocument()
//        {

//            mockContractRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Contract, bool>>>())).Throws(new Exception("Exception occured while performing some operation."));

//            mockContractDocumentRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.ContractDocument, bool>>>())).Returns((Expression<Func<DbModel.ContractDocument, bool>> predicate) => mockContractDocumentDbData.Where(predicate));
//            mockDocumentTypeRepo.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.ModuleDocumentType, bool>>>())).Returns((Expression<Func<DbModel.ModuleDocumentType, bool>> predicate) => mockModuleDocumentType.Where(predicate));

//            mockDocumentTypeService.Setup(moq => moq.IsValidDocumentType(DocumentModuleType.Contract, It.IsAny<IList<string>>(), ModuleType.Contract, ref It.Ref<List<MessageDetail>>.IsAny)).Returns(true);

//            ContractDocumentService = new Evolution.Contract.Core.Services.ContractDocumentService(Mapper.Instance, mockLogger.Object, mockContractDocumentRepository.Object, mockContractRepository.Object, mockDocumentTypeService.Object, mockMongoContext, ContractDocumnetValidService);
//            contractDocumentDomianModels.ToList().ForEach(x => x.RecordStatus = "M");
//            var response = ContractDocumentService.ModifyContractDocument(contractDocumentDomianModels[0].ContractNumber, new List<DomModel.ContractDocument> { contractDocumentDomianModels[0] });
//            Assert.AreEqual("11", response.Code);
//            Assert.IsNotNull(response.Messages);
//            Assert.AreEqual("31", response.Messages.FirstOrDefault().Code);

//        }
//        [TestMethod]
//        public void ThrowsExceptionWhileDeletingDocument()
//        {

//            mockContractRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Contract, bool>>>())).Throws(new Exception("Exception occured while performing some operation."));

//            mockContractDocumentRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.ContractDocument, bool>>>())).Returns((Expression<Func<DbModel.ContractDocument, bool>> predicate) => mockContractDocumentDbData.Where(predicate));
//            mockDocumentTypeRepo.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.ModuleDocumentType, bool>>>())).Returns((Expression<Func<DbModel.ModuleDocumentType, bool>> predicate) => mockModuleDocumentType.Where(predicate));

//            mockDocumentTypeService.Setup(moq => moq.IsValidDocumentType(DocumentModuleType.Contract, It.IsAny<IList<string>>(), ModuleType.Contract, ref It.Ref<List<MessageDetail>>.IsAny)).Returns(true);

//            ContractDocumentService = new Evolution.Contract.Core.Services.ContractDocumentService(Mapper.Instance, mockLogger.Object, mockContractDocumentRepository.Object, mockContractRepository.Object, mockDocumentTypeService.Object, mockMongoContext, ContractDocumnetValidService);
//            contractDocumentDomianModels.ToList().ForEach(x => x.RecordStatus = "D");
//            var response = ContractDocumentService.DeleteContractDocument(contractDocumentDomianModels[0].ContractNumber, new List<DomModel.ContractDocument> { contractDocumentDomianModels[0] });
//            Assert.AreEqual("11", response.Code);
//            Assert.IsNotNull(response.Messages);
//            Assert.AreEqual("31", response.Messages.FirstOrDefault().Code);
//        }
//        [TestMethod]
//        public void GetAllDocumentsAfterSavingData()
//        {

//            mockContractRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Contract, bool>>>())).Returns(mockContractDbData);
//            mockContractDocumentRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.ContractDocument, bool>>>())).Returns((Expression<Func<DbModel.ContractDocument, bool>> predicate) => mockContractDocumentDbData.Where(predicate));
//            mockDocumentTypeRepo.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.ModuleDocumentType, bool>>>())).Returns((Expression<Func<DbModel.ModuleDocumentType, bool>> predicate) => mockModuleDocumentType.Where(predicate));

//            mockDocumentTypeService.Setup(moq => moq.IsValidDocumentType(DocumentModuleType.Contract, It.IsAny<IList<string>>(), ModuleType.Contract, ref It.Ref<List<MessageDetail>>.IsAny)).Returns(true);

//            ContractDocumentService = new Evolution.Contract.Core.Services.ContractDocumentService(Mapper.Instance, mockLogger.Object, mockContractDocumentRepository.Object, mockContractRepository.Object, mockDocumentTypeService.Object, mockMongoContext, ContractDocumnetValidService);
//            contractDocumentDomianModels.ToList().ForEach(x => x.RecordStatus = "N");
//            var response = ContractDocumentService.SaveContractDocument(contractDocumentDomianModels[0].ContractNumber, new List<DomModel.ContractDocument> { contractDocumentDomianModels[0] }, true, null, true);
//            mockContractDocumentRepository.Verify(m => m.Add(It.IsAny<IList<DbModel.ContractDocument>>()), Times.AtLeastOnce);
//        }
//        [TestMethod]
//        public void GetAllDocumentAfterModifingContractSavingDocument()
//        {
//            mockContractDocumentRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.ContractDocument, bool>>>())).Returns((Expression<Func<DbModel.ContractDocument, bool>> predicate) => mockContractDocumentDbData.Where(predicate));
//            mockContractRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Contract, bool>>>())).Returns(mockContractDbData);
//            mockDocumentTypeService.Setup(moq => moq.IsValidDocumentType(DocumentModuleType.Contract, It.IsAny<IList<string>>(), ModuleType.Contract, ref It.Ref<List<MessageDetail>>.IsAny)).Returns(true);

//            ContractDocumentService = new Evolution.Contract.Core.Services.ContractDocumentService(Mapper.Instance, mockLogger.Object, mockContractDocumentRepository.Object, mockContractRepository.Object, mockDocumentTypeService.Object, mockMongoContext, ContractDocumnetValidService);
//            contractDocumentDomianModels.ToList().ForEach(x => x.RecordStatus = "M");
//            var response = ContractDocumentService.ModifyContractDocument(contractDocumentDomianModels[0].ContractNumber, new List<DomModel.ContractDocument> { contractDocumentDomianModels[0] }, true, true);

//            mockContractDocumentRepository.Verify(m => m.Update(It.IsAny<DbModel.ContractDocument>()), Times.AtLeast(1));
//        }


//        [TestMethod]
//        public void GetAllDocumentAfterDeletingContractDocument()
//        {
//            mockContractDocumentRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.ContractDocument, bool>>>())).Returns((Expression<Func<DbModel.ContractDocument, bool>> predicate) => mockContractDocumentDbData.Where(predicate));
//            mockContractRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Contract, bool>>>())).Returns(mockContractDbData);
//            mockDocumentTypeService.Setup(moq => moq.IsValidDocumentType(DocumentModuleType.Contract, It.IsAny<IList<string>>(), ModuleType.Contract, ref It.Ref<List<MessageDetail>>.IsAny)).Returns(true);

//            ContractDocumentService = new Evolution.Contract.Core.Services.ContractDocumentService(Mapper.Instance, mockLogger.Object, mockContractDocumentRepository.Object, mockContractRepository.Object, mockDocumentTypeService.Object, mockMongoContext, ContractDocumnetValidService);
//            contractDocumentDomianModels.ToList().ForEach(x => x.RecordStatus = "D");
//            var response = ContractDocumentService.DeleteContractDocument(contractDocumentDomianModels[0].ContractNumber, new List<DomModel.ContractDocument> { contractDocumentDomianModels[0] }, true, true);

//            mockContractDocumentRepository.Verify(m => m.Delete(It.IsAny<DbModel.ContractDocument>()), Times.AtLeast(1));
//        }
//        [TestMethod]
//        public void ValidationOfUpdateCountMissmatch()
//        {
//            mockContractDocumentRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.ContractDocument, bool>>>())).Returns((Expression<Func<DbModel.ContractDocument, bool>> predicate) => mockContractDocumentDbData.Where(predicate));
//            mockContractRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Contract, bool>>>())).Returns(mockContractDbData);
//            mockDocumentTypeService.Setup(moq => moq.IsValidDocumentType(DocumentModuleType.Contract, It.IsAny<IList<string>>(), ModuleType.Contract, ref It.Ref<List<MessageDetail>>.IsAny)).Returns(true);

//            ContractDocumentService = new Evolution.Contract.Core.Services.ContractDocumentService(Mapper.Instance, mockLogger.Object, mockContractDocumentRepository.Object, mockContractRepository.Object, mockDocumentTypeService.Object, mockMongoContext, ContractDocumnetValidService);
//            contractDocumentDomianModels.ToList().ForEach(x => x.RecordStatus = "M");
//            contractDocumentDomianModels[0].UpdateCount = 2;
//            var response = ContractDocumentService.ModifyContractDocument(contractDocumentDomianModels[0].ContractNumber, new List<DomModel.ContractDocument> { contractDocumentDomianModels[0] });
//            Assert.AreEqual("11", response.Code);
//        }

//        [TestMethod]
//        public void ShouldNotSaveContractDocuemtWhenContractNumberIsNull()
//        {

//            mockContractRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Contract, bool>>>())).Returns(mockContractDbData);
//            mockContractDocumentRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.ContractDocument, bool>>>())).Returns((Expression<Func<DbModel.ContractDocument, bool>> predicate) => mockContractDocumentDbData.Where(predicate));
//            mockDocumentTypeRepo.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.ModuleDocumentType, bool>>>())).Returns((Expression<Func<DbModel.ModuleDocumentType, bool>> predicate) => mockModuleDocumentType.Where(predicate));

//            mockDocumentTypeService.Setup(moq => moq.IsValidDocumentType(DocumentModuleType.Contract, It.IsAny<IList<string>>(), ModuleType.Contract, ref It.Ref<List<MessageDetail>>.IsAny)).Returns(true);

//            ContractDocumentService = new Evolution.Contract.Core.Services.ContractDocumentService(Mapper.Instance, mockLogger.Object, mockContractDocumentRepository.Object, mockContractRepository.Object, mockDocumentTypeService.Object, mockMongoContext, ContractDocumnetValidService);
//            contractDocumentDomianModels.ToList().ForEach(x => x.RecordStatus = "N");
//            var response = ContractDocumentService.SaveContractDocument(null, new List<DomModel.ContractDocument> { contractDocumentDomianModels[0] });
//            Assert.AreEqual("11", response.Code);
//        }


//        [TestMethod]
//        public void ShouldNotSaveContractDocuemtWhenContractNumberIsinvalid()
//        {

//            //mockContractRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Contract, bool>>>())).Returns(mockContractDbData);
//            mockContractDocumentRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.ContractDocument, bool>>>())).Returns((Expression<Func<DbModel.ContractDocument, bool>> predicate) => mockContractDocumentDbData.Where(predicate));
//            mockDocumentTypeRepo.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.ModuleDocumentType, bool>>>())).Returns((Expression<Func<DbModel.ModuleDocumentType, bool>> predicate) => mockModuleDocumentType.Where(predicate));

//            mockDocumentTypeService.Setup(moq => moq.IsValidDocumentType(DocumentModuleType.Contract, It.IsAny<IList<string>>(), ModuleType.Contract, ref It.Ref<List<MessageDetail>>.IsAny)).Returns(true);

//            ContractDocumentService = new Evolution.Contract.Core.Services.ContractDocumentService(Mapper.Instance, mockLogger.Object, mockContractDocumentRepository.Object, mockContractRepository.Object, mockDocumentTypeService.Object, mockMongoContext, ContractDocumnetValidService);
//            contractDocumentDomianModels.ToList().ForEach(x => x.RecordStatus = "N");
//            var response = ContractDocumentService.SaveContractDocument(contractDocumentDomianModels[0].ContractNumber, new List<DomModel.ContractDocument> { contractDocumentDomianModels[0] });
//            Assert.AreEqual("11", response.Code);
//        }
//        [TestMethod]
//        public void ShouldNotUpdateContractDocuemtWhenContractNumberIsNull()
//        {

//            mockContractRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Contract, bool>>>())).Returns(mockContractDbData);
//            mockContractDocumentRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.ContractDocument, bool>>>())).Returns((Expression<Func<DbModel.ContractDocument, bool>> predicate) => mockContractDocumentDbData.Where(predicate));
//            mockDocumentTypeRepo.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.ModuleDocumentType, bool>>>())).Returns((Expression<Func<DbModel.ModuleDocumentType, bool>> predicate) => mockModuleDocumentType.Where(predicate));

//            mockDocumentTypeService.Setup(moq => moq.IsValidDocumentType(DocumentModuleType.Contract, It.IsAny<IList<string>>(), ModuleType.Contract, ref It.Ref<List<MessageDetail>>.IsAny)).Returns(true);

//            ContractDocumentService = new Evolution.Contract.Core.Services.ContractDocumentService(Mapper.Instance, mockLogger.Object, mockContractDocumentRepository.Object, mockContractRepository.Object, mockDocumentTypeService.Object, mockMongoContext, ContractDocumnetValidService);
//            contractDocumentDomianModels.ToList().ForEach(x => x.RecordStatus = "M");
//            var response = ContractDocumentService.ModifyContractDocument(null, new List<DomModel.ContractDocument> { contractDocumentDomianModels[0] });
//            Assert.AreEqual("11", response.Code);
//        }

//        [TestMethod]
//        public void ShouldNotUpdateContractDocuemtWhenContractNumberIsinvalid()
//        {

//            //mockContractRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Contract, bool>>>())).Returns(mockContractDbData);
//            mockContractDocumentRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.ContractDocument, bool>>>())).Returns((Expression<Func<DbModel.ContractDocument, bool>> predicate) => mockContractDocumentDbData.Where(predicate));
//            mockDocumentTypeRepo.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.ModuleDocumentType, bool>>>())).Returns((Expression<Func<DbModel.ModuleDocumentType, bool>> predicate) => mockModuleDocumentType.Where(predicate));

//            mockDocumentTypeService.Setup(moq => moq.IsValidDocumentType(DocumentModuleType.Contract, It.IsAny<IList<string>>(), ModuleType.Contract, ref It.Ref<List<MessageDetail>>.IsAny)).Returns(true);

//            ContractDocumentService = new Evolution.Contract.Core.Services.ContractDocumentService(Mapper.Instance, mockLogger.Object, mockContractDocumentRepository.Object, mockContractRepository.Object, mockDocumentTypeService.Object, mockMongoContext, ContractDocumnetValidService);
//            contractDocumentDomianModels.ToList().ForEach(x => x.RecordStatus = "M");
//            var response = ContractDocumentService.ModifyContractDocument(contractDocumentDomianModels[0].ContractNumber, new List<DomModel.ContractDocument> { contractDocumentDomianModels[0] });
//            Assert.AreEqual("11", response.Code);
//        }

//        [TestMethod]
//        public void ValidationOfDocumentWhileSave()
//        {

//            mockContractRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Contract, bool>>>())).Returns(mockContractDbData);
//            mockContractDocumentRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.ContractDocument, bool>>>())).Returns((Expression<Func<DbModel.ContractDocument, bool>> predicate) => mockContractDocumentDbData.Where(predicate));
//            mockDocumentTypeRepo.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.ModuleDocumentType, bool>>>())).Returns((Expression<Func<DbModel.ModuleDocumentType, bool>> predicate) => mockModuleDocumentType.Where(predicate));

//            mockDocumentTypeService.Setup(moq => moq.IsValidDocumentType(DocumentModuleType.Contract, It.IsAny<IList<string>>(), ModuleType.Contract, ref It.Ref<List<MessageDetail>>.IsAny)).Returns(true);

//            ContractDocumentService = new Evolution.Contract.Core.Services.ContractDocumentService(Mapper.Instance, mockLogger.Object, mockContractDocumentRepository.Object, mockContractRepository.Object, mockDocumentTypeService.Object, mockMongoContext, ContractDocumnetValidService);
//            contractDocumentDomianModels.ToList().ForEach(x => x.RecordStatus = "M");
//            contractDocumentDomianModels[0].Name = "ABC";
//            contractDocumentDomianModels[0].ContractDocumentId = 3;
//            var response = ContractDocumentService.ModifyContractDocument(contractDocumentDomianModels[0].ContractNumber, new List<DomModel.ContractDocument> { contractDocumentDomianModels[0] });
//            Assert.AreEqual("11", response.Code);
//        }

//        [TestMethod]
//        public void ValidationOfDocumentWhileDelete()
//        {

//            mockContractRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.Contract, bool>>>())).Returns(mockContractDbData);
//            mockContractDocumentRepository.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.ContractDocument, bool>>>())).Returns((Expression<Func<DbModel.ContractDocument, bool>> predicate) => mockContractDocumentDbData.Where(predicate));
//            mockDocumentTypeRepo.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<DbModel.ModuleDocumentType, bool>>>())).Returns((Expression<Func<DbModel.ModuleDocumentType, bool>> predicate) => mockModuleDocumentType.Where(predicate));

//            mockDocumentTypeService.Setup(moq => moq.IsValidDocumentType(DocumentModuleType.Contract, It.IsAny<IList<string>>(), ModuleType.Contract, ref It.Ref<List<MessageDetail>>.IsAny)).Returns(true);

//            ContractDocumentService = new Evolution.Contract.Core.Services.ContractDocumentService(Mapper.Instance, mockLogger.Object, mockContractDocumentRepository.Object, mockContractRepository.Object, mockDocumentTypeService.Object, mockMongoContext, ContractDocumnetValidService);
//            contractDocumentDomianModels.ToList().ForEach(x => x.RecordStatus = "D");
//            contractDocumentDomianModels[0].Name = "ABC";
//            contractDocumentDomianModels[0].ContractDocumentId = 3;
//            var response = ContractDocumentService.DeleteContractDocument(contractDocumentDomianModels[0].ContractNumber, new List<DomModel.ContractDocument> { contractDocumentDomianModels[0] });
//            Assert.AreEqual("11", response.Code);
//        }
//    }

//}



