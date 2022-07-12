using AutoMapper;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Common.Models.Messages;
using Evolution.Common.Models.Responses;
using Evolution.Logging.Interfaces;
using Evolution.Draft.Domain.Interfaces.Data;
using Evolution.Draft.Domain.Interfaces.Draft;
using Evolution.Draft.Domain.Interfaces.Validations; 
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.Draft.Domain.Models;
using Evolution.Company.Domain.Interfaces.Data;
using Evolution.Home.Domain.Interfaces.Homes;
using Evolution.Security.Domain.Interfaces.Data;

namespace Evolution.Draft.Core.Services
{
    public class DraftService : IDraftService
    {
        private readonly IDraftRepository _draftRepository = null;
        private readonly IAppLogger<DraftService> _logger = null;        
        private readonly JObject _MessageDescriptions = null;
        private readonly IMapper _mapper = null;
        private readonly IDraftValidationService _validationService = null;
        private readonly ICompanyRepository _companyRepository = null;
        private readonly IMyTaskService _myTaskService = null;
        private readonly IUserRepository _userRepository = null;

        public DraftService(IDraftRepository draftRepository, IAppLogger<DraftService> logger, IMapper mapper, IDraftValidationService validationService, ICompanyRepository companyRepository,  IMyTaskService myTaskService, IUserRepository userRepository)
        {
            this._draftRepository = draftRepository;
            this._logger = logger;
            this._mapper = mapper;
            this._validationService = validationService;
            this._companyRepository = companyRepository; //D661 issue1 myTask CR
            this._myTaskService = myTaskService;
            this._userRepository = userRepository;
        }

        public Response DeleteDraft(string DrafId, bool commitChange = true)
        { 
               return DeleteDraft(new List<string> { DrafId }); 
        }

        public Response DeleteDraft(IList<string> DraftIds, bool commitChange = true)
        {
            Exception exception = null;
            List<MessageDetail> errorMessages = null;

            try
            {
                _draftRepository.AutoSave = false;
                errorMessages = new List<MessageDetail>();
                IList<DbRepository.Models.SqlDatabaseContext.Draft> dbDrafts = null;
                if (DraftIds != null && DraftIds?.Count>0)
                {
                    if (IsValidDraft(DraftIds, ref dbDrafts, ref errorMessages))
                    {
                        _draftRepository.Delete(dbDrafts);

                        if (commitChange && !_draftRepository.AutoSave && dbDrafts?.Count > 0 && errorMessages.Count <= 0)
                        {
                            _draftRepository.ForceSave();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), DraftIds);
            }
            return new Response().ToPopulate(ResponseType.Success, null, errorMessages, null, null, exception);

        }

        public Response DeleteDraft(string draftId, DraftType draftType, bool commitChange = true)
        {
            Exception exception = null;
            List<MessageDetail> errorMessages = null;

            try
            {
                _draftRepository.AutoSave = false;
                errorMessages = new List<MessageDetail>();
                IList<DbRepository.Models.SqlDatabaseContext.Draft> dbDrafts = null;
                if (!string.IsNullOrEmpty(draftId))
                {
                    if (IsValidDraft(new List<string> { draftId }, draftType, ref dbDrafts, ref errorMessages))
                    {
                        _draftRepository.Delete(dbDrafts);

                        if (commitChange && !_draftRepository.AutoSave && dbDrafts?.Count > 0 && errorMessages.Count <= 0)
                        {
                            _draftRepository.ForceSave();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), draftId);
            }
            return new Response().ToPopulate(ResponseType.Success, null, errorMessages, null, null, exception);
        }

        public Response GetDraft(DomainModel.Draft searchModel)
        {
            IList<DomainModel.Draft> result = null;
            Exception exception = null;

            try
            {
                result = this._draftRepository.Search(searchModel);
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), searchModel);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception, result?.Count);
        }

        public Response GetDraftMyTask(DomainModel.Draft searchModel)
        {
            IList<DomainModel.Draft> result = null;
            Exception exception = null;

            try
            {
                result = this._draftRepository.GetDraftMyTask(searchModel);
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), searchModel);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception, result?.Count);
        }

        public Response ModifyDraft(string jsontext,string drafId, bool commitChange = true)
        {
            Exception exception = null;
            List<MessageDetail> errorMessages = null;
            List<ValidationMessage> validationMessages = null;
            string DraftId = null;
            try
            {
                _draftRepository.AutoSave = false;
                errorMessages = new List<MessageDetail>();
                //IList<Draft> recordToBeModify = null;
                IList<DbRepository.Models.SqlDatabaseContext.Draft> dbDrafts = null;
                if (IsValidDraft(new List<string> { drafId }, ref dbDrafts, ref errorMessages))
                {
                    foreach (var drafinfos in dbDrafts)
                    {
                        var dbDrafs = dbDrafts.FirstOrDefault(x => x.Id == drafinfos.Id && !string.Equals(x.Description, DraftType.ProfileChangeHistory.ToString())); //D661 issue1 myTask CR //Sanity Def 125
                        if (dbDrafs != null)
                        {
                            drafinfos.Description = dbDrafs.Description;
                            drafinfos.Moduletype = dbDrafs.Moduletype;
                            drafinfos.DraftId = dbDrafs.DraftId;
                            drafinfos.SerilizableObject = jsontext;
                            drafinfos.SerilizationType = dbDrafs.SerilizationType;
                            drafinfos.AssignedBy = dbDrafs.AssignedBy;
                            drafinfos.AssignedOn = DateTime.UtcNow;
                            drafinfos.AssignedTo = dbDrafs.AssignedTo;
                            drafinfos.CreatedBy = dbDrafs.CreatedBy;
                            drafinfos.CreatedTo = dbDrafs.CreatedTo;
                            drafinfos.ModifiedBy = dbDrafs.ModifiedBy;
                            drafinfos.LastModification = dbDrafs.LastModification;
                            drafinfos.UpdateCount = dbDrafs.UpdateCount.CalculateUpdateCount();

                            _draftRepository.Update(drafinfos);
                        }
                    }
                    if (commitChange && !_draftRepository.AutoSave)
                    {
                        int value = _draftRepository.ForceSave();
                        if (value > 0)
                        {
                            DraftId = drafId;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), jsontext);

            }
            return new Response().ToPopulate(ResponseType.Success, null, errorMessages, validationMessages, DraftId, exception);
        } 

        public Response SaveDraft(DomainModel.Draft draft, bool commitChange = true)
        {
            Exception exception = null;
            List<MessageDetail> errorMessages = null;
            IList<DbModel.Company> dbCompanies = null;
            List<ValidationMessage> validationMessages = null;
            string DraftId = null;
            DbModel.Draft recordToBeInserted = null;

            try
            {
                _draftRepository.AutoSave = false;
                errorMessages = new List<MessageDetail>();

                if (draft!=null && !string.IsNullOrEmpty(draft.SerilizableObject))
                {
                    recordToBeInserted= _mapper.Map<DbModel.Draft>(draft); 
                    recordToBeInserted.SerilizationType = SerializationType.JSON.ToString(); 
                    recordToBeInserted.Moduletype = recordToBeInserted.Moduletype.ToEnum<ModuleCodeType>().ToString();
                    recordToBeInserted.DraftId = GetRandomNumber(draft.DraftType,draft.DraftId); 
                    recordToBeInserted.AssignedOn = DateTime.UtcNow;
                    recordToBeInserted.CreatedBy = draft.AssignedBy;
                    recordToBeInserted.CreatedTo = DateTime.UtcNow;
                   //D661 issue1 myTask CR
                    dbCompanies = _companyRepository?.FindBy(x => draft.CompanyCode.Contains(x.Code)).ToList();
                    recordToBeInserted.CompanyId = dbCompanies?[0].Id;

                    if (recordToBeInserted.Description == DraftType.ProfileChangeHistory.ToString())
                    {
                        recordToBeInserted.AssignedBy = recordToBeInserted.AssignedBy = string.Empty; //Added to avoid listing of Draft profile in MyTask
                    }
                    //D661 issue1 myTask CR
                    if (!string.IsNullOrEmpty(draft.DraftId))
                    {
                        List<string> moduleType = new List<string> { recordToBeInserted.Moduletype };
                        List<string> taskRefCode = new List<string> { draft.DraftId };
                        IList<Evolution.Home.Domain.Models.Homes.MyTask> myTaskToBeDeleted = null;
                        myTaskToBeDeleted = _myTaskService.Get(moduleType, taskRefCode)
                                                                .Result
                                                                .Populate<IList<Evolution.Home.Domain.Models.Homes.MyTask>>()
                                                                ?.Select(x => { x.RecordStatus = RecordStatus.Delete.FirstChar(); return x; })
                                                                .ToList(); ;
                        if (myTaskToBeDeleted.Any())
                        {
                            _myTaskService.Delete(myTaskToBeDeleted);
                        }
                    }
                     //D661 issue1 myTask CR
                    _draftRepository.Add(recordToBeInserted);

                    if (recordToBeInserted.Description != DraftType.ProfileChangeHistory.ToString() && recordToBeInserted.Description != DraftType.CreateProfile.ToString()) //CreateProfile Desription Check Condition Added For SanityDef 168
                    {
                        var dbUserId = _userRepository?.FindBy(x => draft.PendingWithUser == x.SamaccountName)?.Select(x => x.Id).ToList();
                        if(dbUserId != null) { _draftRepository.UpdateTechSpec(draft, Convert.ToInt32(dbUserId[0])); }
                    }

                    if (commitChange && !_draftRepository.AutoSave)
                    {
                        int value = _draftRepository.ForceSave();
                        if (value > 0)
                        {
                            DraftId = recordToBeInserted.DraftId;   
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), draft);

            }
            return new Response().ToPopulate(ResponseType.Success, null, errorMessages, validationMessages, DraftId, exception);

        }
         
        private string GetRandomNumber(string type,string preDefinedValue=null)
        {
            string number = string.Empty;
            if (string.IsNullOrEmpty(preDefinedValue) || string.Equals(preDefinedValue,"0"))
            {
                Random random = new Random();
                int n = random.Next(0, int.MaxValue);
                //  number = (string.IsNullOrEmpty(type) ? string.Empty : type.ToEnum<DraftType>().ToString()) +  n.ToString("D5");
                number = n.ToString("D5");
            }
            else
            {
                number = preDefinedValue;
            }
            return number;

        }
         

        private bool IsValidDraft(IList<string> DrafIds, ref IList<DbRepository.Models.SqlDatabaseContext.Draft> dbDrafts, ref List<MessageDetail> errorMessages)
        {
            List<MessageDetail> messages = new List<MessageDetail>();
            if (errorMessages == null)
                errorMessages = new List<MessageDetail>();

            var dbDraftInfos = this._draftRepository.FindBy(x => DrafIds.Contains(x.DraftId)).ToList();

            if (dbDrafts?.Count <= 0)
            {
                string errorCode = MessageType.InvalidDraftID.ToId();
                messages.Add(new MessageDetail(errorCode, string.Format(_MessageDescriptions[errorCode].ToString(), DrafIds)));
            }

            dbDrafts = dbDraftInfos;

            if (messages.Count > 0)
                errorMessages.AddRange(messages);

            return errorMessages?.Count <= 0;
        }

        private bool IsValidDraft(IList<string> DrafIds, DraftType draftType, ref IList<DbRepository.Models.SqlDatabaseContext.Draft> dbDrafts, ref List<MessageDetail> errorMessages)
        {
            List<MessageDetail> messages = new List<MessageDetail>();
            if (errorMessages == null)
                errorMessages = new List<MessageDetail>();

            var dbDraftInfos = this._draftRepository.FindBy(x => DrafIds.Contains(x.DraftId) && x.Description == draftType.ToString()).ToList();

            if (dbDrafts?.Count <= 0)
            {
                string errorCode = MessageType.InvalidDraftID.ToId();
                messages.Add(new MessageDetail(errorCode, string.Format(_MessageDescriptions[errorCode].ToString(), DrafIds)));
            }

            dbDrafts = dbDraftInfos;

            if (messages.Count > 0)
                errorMessages.AddRange(messages);

            return errorMessages?.Count <= 0;
        }

        private bool IsValidDraftList(IList<DomainModel.Draft> draftInfos, ref IList<DbRepository.Models.SqlDatabaseContext.Draft> dbDrafts, ref List<MessageDetail> errorMessages)
        {
            List<MessageDetail> messages = new List<MessageDetail>();
            if (errorMessages == null)
                errorMessages = new List<MessageDetail>();

            var Drafts = _draftRepository.FindBy(x => draftInfos.Select(x1 => x1.Id).Contains(x.Id)).ToList();

            var notMatchedRecords = draftInfos.Where(x => !Drafts.Any(x1 => x1.Id == x.Id)).ToList();

            notMatchedRecords.ForEach(x =>
            {
                string errorCode = MessageType.InvalidDraftID.ToId();
                messages.Add(new MessageDetail(ModuleType.Draft, errorCode, string.Format(_MessageDescriptions[errorCode].ToString(), x.Id)));
            });

            dbDrafts = Drafts;

            if (messages.Count > 0)
                errorMessages.AddRange(messages);

            return errorMessages?.Count <= 0;
        }

        private bool IsRecordUpdateCountMatching(IList<DomainModel.Draft> draftInfos, IList<DbRepository.Models.SqlDatabaseContext.Draft> dbDrafts, ref List<MessageDetail> errorMessages)
        {
            List<MessageDetail> messages = new List<MessageDetail>();
            if (errorMessages == null)
                errorMessages = new List<MessageDetail>();

            var notMatchedRecords = draftInfos.Where(x => !dbDrafts.ToList().Any(x1 => x1.UpdateCount.ToInt() == x.UpdateCount.ToInt() && x1.Id == x.Id)).ToList();

            notMatchedRecords.ForEach(x =>
            {
                string errorCode = MessageType.DraftHasBeenUpdatedByOther.ToId();
                messages.Add(new MessageDetail(ModuleType.Draft, errorCode, string.Format(_MessageDescriptions[errorCode].ToString(), x.DraftId)));
            });

            if (messages.Count > 0)
                errorMessages.AddRange(messages);

            return errorMessages?.Count <= 0;

        }

        public Response ModifyListOfDrafts(IList<DomainModel.Draft> drafts, bool commitChange = true)
        {
            Exception exception = null;
            List<MessageDetail> errorMessages = null;
            List<ValidationMessage> validationMessages = null;
            try
            {
                _draftRepository.AutoSave = false;
                errorMessages = new List<MessageDetail>();
                IList<DomainModel.Draft> recordToBeModify = null;
                IList<DbRepository.Models.SqlDatabaseContext.Draft> dbDrafts = null;
                if (this.IsRecordValidForProcess(drafts, ValidationType.Update, ref recordToBeModify, ref errorMessages, ref validationMessages))
                {
                    if (IsValidDraftList(drafts, ref dbDrafts, ref errorMessages))
                    {
                        if(dbDrafts != null) 
                        {
                            foreach (var drafinfos in dbDrafts)
                            {
                                var dbDrafs = dbDrafts?.FirstOrDefault(x => x.DraftId == drafinfos.DraftId);
                                var draftsToModify = drafts?.FirstOrDefault(x => x.Id == drafinfos.Id);
                                if (dbDrafs != null && draftsToModify != null)
                                {
                                    drafinfos.Description = dbDrafs.Description;
                                    drafinfos.Moduletype = dbDrafs.Moduletype;
                                    drafinfos.DraftId = dbDrafs.DraftId;
                                    if(drafinfos.SerilizableObject != null)
                                        drafinfos.SerilizableObject = drafinfos.SerilizableObject;
                                    drafinfos.SerilizationType = dbDrafs.SerilizationType;
                                    drafinfos.AssignedBy = draftsToModify.AssignedBy;
                                    drafinfos.AssignedOn = DateTime.UtcNow;
                                    drafinfos.AssignedTo = draftsToModify.AssignedTo;
                                    drafinfos.CreatedBy = dbDrafs.CreatedBy;
                                    drafinfos.CreatedTo = dbDrafs.CreatedTo;
                                    drafinfos.ModifiedBy = draftsToModify.AssignedBy;
                                    drafinfos.LastModification = DateTime.UtcNow;
                                    drafinfos.UpdateCount = dbDrafs.UpdateCount.CalculateUpdateCount();

                                    _draftRepository.Update(drafinfos);

                                    var dbUserId = _userRepository?.FindBy(x => draftsToModify.AssignedTo == x.SamaccountName)?.Select(x => x.Id).ToList(); //--IGOQC 951 (13-01-2021)
                                    if (dbUserId != null) { _draftRepository.UpdateTechSpec(draftsToModify, Convert.ToInt32(dbUserId[0])); } //--IGOQC 951 (13-01-2021)
                                }
                            }
                        }
                        if (commitChange && !_draftRepository.AutoSave)
                        {
                            _draftRepository.ForceSave();
                           
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), drafts);

            }
            return new Response().ToPopulate(ResponseType.Success, null, errorMessages, validationMessages, null, exception);
        }

        private bool IsRecordValidForProcess(IList<DomainModel.Draft> Drafts, ValidationType validationType, ref IList<DomainModel.Draft> filteredNotes, ref List<MessageDetail> errorMessages, ref List<ValidationMessage> validationMessages)
        {
            if (validationType == ValidationType.Add)
                filteredNotes = Drafts?.Where(x => x.RecordStatus.IsRecordStatusNew()).ToList();
            else if (validationType == ValidationType.Delete)
                filteredNotes = Drafts?.Where(x => x.RecordStatus.IsRecordStatusDeleted()).ToList();
            else if (validationType == ValidationType.Update)
                filteredNotes = Drafts?.Where(x => x.RecordStatus.IsRecordStatusModified()).ToList();

            if (filteredNotes?.Count <= 0)
                return false;

            return IsDraftsHasValidSchema(filteredNotes, validationType, ref errorMessages, ref validationMessages);
        }

        private bool IsDraftsHasValidSchema(IList<DomainModel.Draft> models, ValidationType validationType, ref List<MessageDetail> errorMessages, ref List<ValidationMessage> validationMessages)
        {
            List<ValidationMessage> messages = new List<ValidationMessage>();

            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            var validationResults = _validationService.Validate(JsonConvert.SerializeObject(models), validationType);
            validationResults.ToList().ForEach(x =>
            {
                messages.Add(new ValidationMessage(x.Code, new List<MessageDetail> { new MessageDetail(ModuleType.Draft, x.Code, x.Message) }));
            });

            if (messages.Count > 0)
            {
                validationMessages.AddRange(messages);
            }
            return validationMessages?.Count <= 0;

        }
    }
}
