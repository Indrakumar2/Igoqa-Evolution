using AutoMapper;
using Evolution.Admin.Domain.Interfaces.Data;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Common.Models.Messages;
using Evolution.Common.Models.Responses;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using Evolution.Document.Domain.Interfaces.Data;
using Evolution.Document.Domain.Interfaces.Documents;
using Evolution.Logging.Interfaces;
using MongoDB.Driver;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using DomainModel = Evolution.Document.Domain.Models.Document;

namespace Evolution.Document.Core.Services
{
    public class DocumentApprovalService : IDocumentApprovalService
    {
        private readonly IDocumentApprovalRepository _repository = null;
        private readonly IUserRepository _userRepository = null;
        private readonly IAppLogger<DocumentApprovalService> _logger = null;
        private readonly IMapper _mapper = null;
        private readonly JObject _messageDescriptions = null;

        public DocumentApprovalService(IMapper mapper, IDocumentApprovalRepository repository, IUserRepository userRepository, IAppLogger<DocumentApprovalService> logger,JObject messages)
        {
            this._repository = repository;
            this._userRepository = userRepository;
            this._logger = logger;
            this._mapper = mapper;

            this._messageDescriptions = messages;
        }

        public Response GetDocumentForApproval(DomainModel.DocumentApproval searchModel)
        {
            IList<DomainModel.DocumentApproval> result = null;
            Exception exception = null;
            try
            {
                result = this._repository.Search(searchModel);
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), searchModel);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception, result?.Count);
        }

        public Response ModifyDocumentForApproval(DomainModel.DocumentApproval documentApproval)
        {
            return ModifyDocumentApproval(documentApproval);
        }

        public Response SaveDocumentForApproval(IList<DomainModel.DocumentApproval> documentApprovals)
        {
            return AddDocumentForApproval(documentApprovals);
        }

        private Response AddDocumentForApproval(IList<DomainModel.DocumentApproval> documentForApprovals)
        {
            Exception exception = null;
            IList<MessageDetail> errorMessages = null;
            IList<DbModel.User> users = null;
            try
            {
                errorMessages = new List<MessageDetail>();
                _repository.AutoSave = false;
                foreach (var documentForApproval in documentForApprovals?.Where(x => x.RecordStatus.IsRecordStatusNew()).ToList())
                {
                    if (documentForApproval != null && this.ValidateUsers(documentForApproval.DocumentUploadedBy,ref users, ref errorMessages,documentForApproval.DocumentApprovedBy))
                    {
                        var recordsToBeInsert = this._mapper.Map<DbModel.DocumentApproval>(documentForApproval);
                        recordsToBeInsert.Id = 0;
                        //recordsToBeInsert.UploadedUserId = users?.FirstOrDefault(x => x.SamaccountName == documentForApproval.DocumentUploadedBy)?.Id;
                        if (!string.IsNullOrEmpty(documentForApproval.DocumentApprovedBy))
                            recordsToBeInsert.CoordinatorId = users?.FirstOrDefault(x => x.SamaccountName == documentForApproval.DocumentApprovedBy)?.Id;
                        else
                            recordsToBeInsert.IsApproved = false; // As no value for approved by.

                        _repository.Add(recordsToBeInsert);
                    }
                    else
                        break;
                }

                if (errorMessages.Count <= 0)
                    _repository.ForceSave();
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), documentForApprovals);
            }
            finally
            {
                _repository.AutoSave = false;
            }
            return new Response().ToPopulate(ResponseType.Success, null, errorMessages.ToList(), null, null, exception);
        }

        private Response ModifyDocumentApproval(DomainModel.DocumentApproval documentForApproval)
        {
            Exception exception = null;
            IList<MessageDetail> errorMessages = null;
            IList<DbModel.User> users = null;
            try
            {
                errorMessages = new List<MessageDetail>();
                if (documentForApproval != null && documentForApproval.RecordStatus.IsRecordStatusModified())
                {                   
                    if (this.ValidateUsers(documentForApproval.DocumentUploadedBy, ref users, ref errorMessages,documentForApproval.DocumentApprovedBy))
                    {
                        DbModel.DocumentApproval recordsToBeModify = null;
                        if (this.IsRecordValidForProcess(documentForApproval, ref recordsToBeModify, ref errorMessages))
                        {
                            this.SetModifyValue(documentForApproval, users, ref recordsToBeModify);
                            _repository.Update(recordsToBeModify);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), documentForApproval);
            }
            return new Response().ToPopulate(ResponseType.Success, null, errorMessages.ToList(), null, null, exception);
        }

        private bool ValidateUsers(string uploadedBy, ref IList<DbModel.User> users, ref IList<MessageDetail> errorMessages, string approvedBy = null)
        {
            if (errorMessages == null)
                errorMessages = new List<MessageDetail>();
            
            var valdUsers = new List<string>() { uploadedBy };
            if (approvedBy != null)
                valdUsers.Add(approvedBy);

            users = _userRepository.FindBy(x => valdUsers.Contains(x.SamaccountName)).ToList();
            if (!users.Any(x => x.SamaccountName == uploadedBy))
                errorMessages.Add(new MessageDetail(MessageType.DocumentUploaderNotFound.ToId(), string.Format(this._messageDescriptions[MessageType.DocumentUploaderNotFound.ToId()].ToString(), uploadedBy)));

            if (approvedBy != null && !users.Any(x => x.SamaccountName == approvedBy))
                errorMessages.Add(new MessageDetail(MessageType.DocumentCoordinatorNotFound.ToId(), string.Format(this._messageDescriptions[MessageType.DocumentCoordinatorNotFound.ToId()].ToString(), uploadedBy)));
            
            return errorMessages?.Count <= 0;
        }

        private bool IsRecordValidForProcess(DomainModel.DocumentApproval documentForApproval, ref DbModel.DocumentApproval dbDocumentForApproval, ref IList<MessageDetail> errorMessages)
        {
            if (errorMessages == null)
                errorMessages = new List<MessageDetail>();

            dbDocumentForApproval = _repository.FindBy(x => x.Id == documentForApproval.Id).FirstOrDefault();
            if (dbDocumentForApproval == null)
                errorMessages.Add(new MessageDetail(MessageType.DocumentAppovalIdNotExists.ToId(), this._messageDescriptions[MessageType.DocumentAppovalIdNotExists.ToId()].ToString()));

            if (dbDocumentForApproval?.UpdateCount.ToInt() != documentForApproval.UpdateCount.ToInt())
                errorMessages.Add(new MessageDetail(MessageType.DocumentAppovalUpdatedByOtherUser.ToId(), _messageDescriptions[MessageType.DocumentAppovalUpdatedByOtherUser.ToId()].ToString()));

            return errorMessages.Count <= 0;
        }

        private void SetModifyValue(DomainModel.DocumentApproval documentApproval, IList<DbModel.User> users, ref DbModel.DocumentApproval dbDocumentApproval)
        {
            dbDocumentApproval.SourceId = documentApproval.DocumentSourceId;
            dbDocumentApproval.SourceModule = documentApproval.DocumentSourceModule;
            dbDocumentApproval.TargetId = documentApproval.DocumentTargetId;
            dbDocumentApproval.TargetModule = documentApproval.DocumentTargetModule;
            dbDocumentApproval.DocumentName = documentApproval.DocumentName;
            dbDocumentApproval.DocumentType = documentApproval.DocumentType;
            dbDocumentApproval.DocumentSize = documentApproval.DocumentSize;
            dbDocumentApproval.UploadedDate = documentApproval.DocumentUploadedDate;            
           // dbDocumentApproval.UploadedUserId = users?.FirstOrDefault(x => x.SamaccountName == documentApproval.DocumentUploadedBy)?.Id;
            if (!string.IsNullOrEmpty(documentApproval.DocumentApprovedBy)) //If document ApprovedBy doesn't exist then system will not chnaged the Approved status.
            {
                dbDocumentApproval.ApprovedDate = documentApproval.DocumentApprovedDate;
                dbDocumentApproval.IsApproved = documentApproval.IsApproved;
                dbDocumentApproval.CoordinatorId = users?.FirstOrDefault(x => x.SamaccountName == documentApproval.DocumentApprovedBy)?.Id;
            }
            else
            {
                dbDocumentApproval.CoordinatorId = null;
                dbDocumentApproval.IsApproved = false;
                dbDocumentApproval.ApprovedDate = null;
            }

            dbDocumentApproval.IsSpecialistVisible = documentApproval.IsSpecialistVisible;
            dbDocumentApproval.UpdateCount = Convert.ToByte(Convert.ToInt32(documentApproval.UpdateCount) + 1);
        }
    }
}