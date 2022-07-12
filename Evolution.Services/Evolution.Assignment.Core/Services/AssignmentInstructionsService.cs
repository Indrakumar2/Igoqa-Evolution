using Evolution.Assignment.Domain.Interfaces.Assignments;
using Evolution.Assignment.Domain.Interfaces.Data;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Common.Models.Messages;
using Evolution.Common.Models.Responses;
using Evolution.Logging.Interfaces;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using AssignmentEnum = Evolution.Assignment.Domain.Enums;
using DbModels = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModels = Evolution.Assignment.Domain.Models.Assignments;

namespace Evolution.Assignment.Core.Services
{
    public class AssignmentInstructionsService : IAssignmentInstructionsService
    {
        private readonly IAssignmentInstructionsRepository _repository = null;
        private readonly IAssignmentRepository _assignmentRepository = null;
        private readonly IAppLogger<AssignmentInstructionsService> _logger = null;
        private readonly JObject _messageDescriptions = null;

        public AssignmentInstructionsService(IAssignmentInstructionsRepository repository,
                                             IAppLogger<AssignmentInstructionsService> logger,
                                             JObject messages)
        {
            _repository = repository;
            _logger = logger;
            _messageDescriptions = messages;
        }

        public Response GetAssignmentInstructions(int assignmentId)
        {
            DomainModels.AssignmentInstructions result = null;
            Exception exception = null;
            try
            {
                using (var tranScope = new TransactionScope(TransactionScopeOption.RequiresNew,new TransactionOptions {IsolationLevel = IsolationLevel.ReadUncommitted}))
                {
                    result = _repository.Search(assignmentId);
                    tranScope.Complete();
                }

            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), assignmentId);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception, null);
        }

        public Response Add(int assignmentId,
                            DomainModels.AssignmentInstructions assignmentInstructions,
                            DbModels.Assignment dbAssignment = null)
        {
            return ProcessAssignmentInstructions(assignmentId,
                                                 assignmentInstructions,
                                                 dbAssignment);
        }


        public Response Modify(int assignmentId,
                               DomainModels.AssignmentInstructions assignmentInstructions,
                               DbModels.Assignment dbAssignment = null)
        {
            return ProcessAssignmentInstructions(assignmentId,
                                                 assignmentInstructions,
                                                 dbAssignment);
        }

        private Response ProcessAssignmentInstructions(int assignmentId,
                                                       DomainModels.AssignmentInstructions assignmentInstructions,
                                                       DbModels.Assignment dbAssignment,
                                                       bool commitChange = true)
        {
            Exception exception = null;
            List<MessageDetail> errorMessages = null;
            List<DbModels.AssignmentMessage> msgToBeInsert = null;
            List<DbModels.AssignmentMessage> msgToBeUpdate = null;
            try
            {
                this._repository.AutoSave = false;
                errorMessages = new List<MessageDetail>();

                if (this.IsValidAssignment(assignmentId, ref dbAssignment, ref errorMessages))
                {
                    msgToBeInsert = this.ProcessNewAssignmentInstructions(assignmentInstructions,
                                                                          dbAssignment,
                                                                          ref errorMessages).ToList();
                    msgToBeUpdate = this.ProcessExistingAssignmentInstructions(assignmentInstructions,
                                                                                dbAssignment,
                                                                                ref errorMessages).ToList();

                    if (errorMessages.Count <= 0)
                    {
                        if (msgToBeInsert?.Count > 0)
                            _repository.Add(msgToBeInsert);

                        if (msgToBeUpdate?.Count > 0)
                            _repository.Update(msgToBeUpdate);

                        if (commitChange && errorMessages.Count <= 0 && !_repository.AutoSave)
                            _repository.ForceSave();
                    }
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), assignmentInstructions);
            }
            finally
            {
                _repository.AutoSave = true;
               // _repository.Dispose();
            }

            return new Response().ToPopulate(ResponseType.Success, null, errorMessages, null, null, exception);
        }

        private IList<DbModels.AssignmentMessage> ProcessNewAssignmentInstructions(DomainModels.AssignmentInstructions assignmentInstructions,
                                                                                   DbModels.Assignment dbAssignment,
                                                                                   ref List<MessageDetail> errorMessages)
        {
            List<DbModels.AssignmentMessage> dbAssignmentMessage = new List<DbModels.AssignmentMessage>();
            DbModels.AssignmentMessage assignmentMessage = null;

            if (errorMessages == null)
                errorMessages = new List<MessageDetail>();

            assignmentMessage = dbAssignment?.AssignmentMessage?.FirstOrDefault(x => x.MessageTypeId == (int)AssignmentEnum.AssignmentMessageType.InterCompanyInstructions);
            if (assignmentMessage == null && !string.IsNullOrEmpty(assignmentInstructions.InterCompanyInstructions))
                dbAssignmentMessage.Add(ConvertToDbAssignmentInstructions(Convert.ToInt32(dbAssignment.Id),
                                                                          AssignmentEnum.AssignmentMessageType.InterCompanyInstructions,
                                                                          assignmentInstructions.InterCompanyInstructions));

            assignmentMessage = dbAssignment?.AssignmentMessage?.FirstOrDefault(x => x.MessageTypeId == (int)AssignmentEnum.AssignmentMessageType.OperationalNotes);
            if (assignmentMessage == null && !string.IsNullOrEmpty(assignmentInstructions.TechnicalSpecialistInstructions))
                dbAssignmentMessage.Add(ConvertToDbAssignmentInstructions(Convert.ToInt32(dbAssignment.Id),
                                                                          AssignmentEnum.AssignmentMessageType.OperationalNotes,
                                                                          assignmentInstructions.TechnicalSpecialistInstructions));

            //assignmentMessage = dbAssignment?.AssignmentMessage?.FirstOrDefault(x => x.MessageTypeId == (int)AssignmentEnum.AssignmentMessageType.ReportingRequirements);
            //if (assignmentMessage == null && !string.IsNullOrEmpty(assignmentInstructions.ClientReportingRequirements))
            //    dbAssignmentMessage.Add(ConvertToDbAssignmentInstructions(Convert.ToInt32(dbAssignment.Id), AssignmentEnum.AssignmentMessageType.ReportingRequirements, assignmentInstructions.ClientReportingRequirements));

            return dbAssignmentMessage;

        }

        private IList<DbModels.AssignmentMessage> ProcessExistingAssignmentInstructions(DomainModels.AssignmentInstructions assignmentInstructions,
                                                                                        DbModels.Assignment dbAssignment,
                                                                                        ref List<MessageDetail> errorMessages)
        {
            List<DbModels.AssignmentMessage> recordToUpdate = new List<DbModels.AssignmentMessage>();
            DbModels.AssignmentMessage assignmentMessage = null;

            if (errorMessages == null)
                errorMessages = new List<MessageDetail>();

            assignmentMessage = dbAssignment?.AssignmentMessage?.FirstOrDefault(x => x.MessageTypeId == (int)AssignmentEnum.AssignmentMessageType.InterCompanyInstructions);
            if (assignmentMessage != null)
                recordToUpdate.Add(ConvertToDbAssignmentInstructions(assignmentMessage,
                                                                     AssignmentEnum.AssignmentMessageType.InterCompanyInstructions,
                                                                     assignmentInstructions.InterCompanyInstructions,
                                                                     assignmentInstructions.LastModification,
                                                                     !string.IsNullOrEmpty(assignmentInstructions.InterCompanyInstructions) ? true : false,
                                                                     assignmentInstructions.ModifiedBy,
                                                                     assignmentInstructions.UpdateCount));

            assignmentMessage = dbAssignment?.AssignmentMessage?.FirstOrDefault(x => x.MessageTypeId == (int)AssignmentEnum.AssignmentMessageType.OperationalNotes);
            if (assignmentMessage != null)
                recordToUpdate.Add(ConvertToDbAssignmentInstructions(assignmentMessage,
                                                                    AssignmentEnum.AssignmentMessageType.OperationalNotes,
                                                                    assignmentInstructions.TechnicalSpecialistInstructions,
                                                                    assignmentInstructions.LastModification,
                                                                    !string.IsNullOrEmpty(assignmentInstructions.TechnicalSpecialistInstructions) ? true : false,
                                                                    assignmentInstructions.ModifiedBy,
                                                                    assignmentInstructions.UpdateCount));

            //assignmentMessage = dbAssignment?.AssignmentMessage?.FirstOrDefault(x => x.MessageTypeId == (int)AssignmentEnum.AssignmentMessageType.ReportingRequirements);
            //if (assignmentMessage != null && !string.IsNullOrEmpty(assignmentInstructions.ClientReportingRequirements))
            //    recordToUpdate.Add(ConvertToDbAssignmentInstructions(assignmentMessage, AssignmentEnum.AssignmentMessageType.ReportingRequirements, assignmentInstructions.ClientReportingRequirements, assignmentInstructions.LastModification, false, assignmentInstructions.ModifiedBy, assignmentInstructions.UpdateCount));

            return recordToUpdate;

        }

        private bool IsValidAssignment(int assignmentId,
                                       ref DbModels.Assignment dbAssignment,
                                       ref List<MessageDetail> errorMessages)
        {
            MessageType messageType = MessageType.Success;
            if (errorMessages == null)
                errorMessages = new List<MessageDetail>();
            if (dbAssignment == null)
            {
                dbAssignment = _assignmentRepository.FindBy(x => x.Id == assignmentId)?.FirstOrDefault();
                if (dbAssignment == null)
                    messageType = MessageType.AssignmentRefrenceInvalidAssignment;
            }

            if (messageType != MessageType.Success)
                errorMessages.Add(new MessageDetail(ModuleType.Assignment, messageType.ToId(), string.Format(_messageDescriptions[messageType.ToId()].ToString(), assignmentId.ToString())));

            return messageType == MessageType.Success;
        }

        private DbModels.AssignmentMessage ConvertToDbAssignmentInstructions(int assignmentId,
                                                                            AssignmentEnum.AssignmentMessageType type,
                                                                            string messageText,
                                                                            bool isActive = true)
        {
            return new DbModels.AssignmentMessage()
            {
                Id=0,
                AssignmentId = assignmentId,
                Identifier = null,
                Message = messageText,
                IsActive = isActive,
                MessageTypeId = (int)type,
            };
        }

        private DbModels.AssignmentMessage ConvertToDbAssignmentInstructions(DbModels.AssignmentMessage dbAssignmentMessage,
                                                                            AssignmentEnum.AssignmentMessageType type,
                                                                            string messageText,
                                                                            DateTime? LastModification,
                                                                            bool? isActive = true,
                                                                            string modifiedBy = null,
                                                                            int? updateCount = null)
        {
            dbAssignmentMessage.Message = isActive == true ? messageText : dbAssignmentMessage.Message;
            dbAssignmentMessage.IsActive = isActive;
            dbAssignmentMessage.LastModification = LastModification;
            dbAssignmentMessage.ModifiedBy = modifiedBy;
            dbAssignmentMessage.UpdateCount = Convert.ToByte(updateCount).CalculateUpdateCount();
            return dbAssignmentMessage;
        }
    }
}
