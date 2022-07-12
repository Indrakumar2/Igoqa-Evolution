using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Common.Models.Messages;
using Evolution.Common.Models.Responses;
using Evolution.Logging.Interfaces;
using Evolution.Master.Domain.Interfaces.Data;
using Evolution.Master.Domain.Interfaces.Services;
using Evolution.Master.Domain.Interfaces.Validations;
using Evolution.Master.Domain.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using dbModel=Evolution.DbRepository.Models.SqlDatabaseContext;

namespace Evolution.Master.Core
{
    public class LanguageReferenceTypeService : ILanguageReferenceTypeService
    {
        private readonly IAppLogger<LanguageReferenceTypeService> _logger = null;
        private readonly IMasterRepository _masterRepository = null;
        private readonly ILanguageReferenceTypeRepository _repository = null;
        private readonly IMapper _mapper = null;
        private readonly JObject _messages = null;
        private readonly ILanguageRefTypeValidationService _languageRefTypeValidationService = null;
        private readonly ILanguageService _languageService = null;
        private readonly IAssignmentReferenceType _RefTypeService=null;

        public LanguageReferenceTypeService(IAppLogger<LanguageReferenceTypeService> logger,
                                        IMasterRepository masterRepository, 
                                        ILanguageReferenceTypeRepository repository,
                                        IMapper mapper, 
                                        JObject messages,
                                        ILanguageRefTypeValidationService languageRefTypeValidationService,
                                        ILanguageService languageService,
                                         IAssignmentReferenceType RefTypeService)
        {
            _logger=logger;
            _masterRepository=masterRepository;
            _repository=repository;
            _mapper=mapper;
            _messages=messages;
            _languageRefTypeValidationService=languageRefTypeValidationService;
            _languageService=languageService;
            _RefTypeService=RefTypeService;




            
        }

        public Response Delete(IList<LanguageReferenceType> datas)
        {
            throw new System.NotImplementedException();
        }

        public Response Modify(IList<LanguageReferenceType> datas)
        {
            throw new System.NotImplementedException();
        }

        public Response Save(IList<LanguageReferenceType> languageRefTypes)
        {
            Exception exception = null;
            IList<dbModel.Data> dbRefType = null;
            IList<dbModel.Data> dbLanguage =null;
            try
            {
                if (languageRefTypes != null)
                {

                    IList<dbModel.LanguageReferenceType> dbLanguageRefType = null;
                    var recordToBeAdd = FilterRecord(languageRefTypes, ValidationType.Add);
                    var validResponse = IsRecordValidForProcess(languageRefTypes, ValidationType.Add, 
                                                            recordToBeAdd, ref dbLanguageRefType,
                                                              ref dbLanguage,ref dbRefType);
                 


                    if (Convert.ToBoolean(validResponse.Result) && dbLanguageRefType != null && dbLanguage !=null && dbRefType!=null)
                    {
                        _repository.AutoSave = false;
                        dbLanguageRefType = _mapper.Map<IList<dbModel.LanguageReferenceType>>(recordToBeAdd, opt =>
                        {
                            opt.Items["isLanguageRefId"] = false;
                            opt.Items["isLanguageId"] = dbLanguage;
                              opt.Items["isReferenceTypeId"] = dbRefType;
                        });
                        _repository.Add(dbLanguageRefType);
                        _repository.ForceSave();
                    }
                    else
                        return validResponse;
                }

            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), languageRefTypes);
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, null, null, exception);
        }

        public Response Search(LanguageReferenceType search)
        {
              IList<LanguageReferenceType> result = null;
              Exception exception = null;
              try
              {
                    
                  search= search ?? new LanguageReferenceType();
                  result=_repository.Search(search);
              }
              catch(Exception ex)
              {
                  exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), search);
              }
               return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception, result?.Count);
        }

           private bool IsValidPayload(IList<LanguageReferenceType> datas,
                                    ValidationType validationType,
                                    ref IList<ValidationMessage> validationMessages)
        {
            var messages = new List<ValidationMessage>();
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            var validationResults = _languageRefTypeValidationService.Validate(JsonConvert.SerializeObject(datas), validationType);
            if (validationResults?.Count > 0)
                messages.Add(_messages, ModuleType.Master, validationResults);

            if (validationResults.Count > 0)
                validationMessages.AddRange(messages);

            return validationMessages?.Count <= 0;
        }

         private IList<dbModel.LanguageReferenceType> GetLanguageRefData(IList<LanguageReferenceType> datas)
        {
            IList<dbModel.LanguageReferenceType> dbLanguageRefType= null;
            if (datas?.Count > 0)
            {
                var Id = datas.Select(x => x.LanguageReferenceTypeId).Distinct().ToList();
                dbLanguageRefType = _repository.FindBy(x => Id.Contains(x.Id)).ToList();
            }

            return dbLanguageRefType;
        }

         private bool IsCountryIdExistInDb(IList<int> langRefTypeIds,
                                        IList<dbModel.LanguageReferenceType> dbLanguageRefType,
                                        ref IList<int> languageRefTypeNotExsists,
                                        ref IList<ValidationMessage> validationMessages)
        {
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            if (dbLanguageRefType == null)
                dbLanguageRefType = new List<dbModel.LanguageReferenceType>();

            var validMessages = validationMessages;

            if (langRefTypeIds?.Count > 0)
            {
                languageRefTypeNotExsists = langRefTypeIds.Where(x => !dbLanguageRefType.Select(x1 => x1.Id).ToList().Contains(x)).ToList();
                languageRefTypeNotExsists?.ToList().ForEach(x =>
                {
                    validMessages.Add(_messages, x, MessageType.InvalidLanguageRefType, x);
                });
            }



            if (validMessages.Count > 0)
                validationMessages = validMessages;

            return validationMessages.Count <= 0;
        }


         private bool IsRecordValid(IList<LanguageReferenceType> datas,
                                             ref IList<dbModel.LanguageReferenceType> dbCountries,
                                             ref IList<dbModel.Data> dbLanguage,
                                             ref IList<dbModel.Data> dbRefType,
                                             ref IList<ValidationMessage> validationMessages)
        {
            IList<ValidationMessage> messages = new List<ValidationMessage>();
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            IList<string> languageNames = datas.Select(x => x.Language).ToList();
            IList<string> refNames = datas.Select(x=>x.ReferenceType).ToList();

            if (_languageService.IsValidLanguage(languageNames, ref dbLanguage,ref messages))
                 _RefTypeService.IsValidRefType(refNames, ref dbRefType, ref messages);


            if (messages.Count > 0)
                validationMessages.AddRange(messages);

            return messages?.Count <= 0;
        }

          public  Response IsRecordValidForProcess(IList<LanguageReferenceType> languageRefType, 
                                                 ValidationType validationType,
                                                 IList<LanguageReferenceType> FilteredDatas, 
                                                 ref IList<dbModel.LanguageReferenceType> dbLanguageRefType,
                                                 ref IList<dbModel.Data>dbLanguages,
                                                ref IList<dbModel.Data>dbRefType)
        {
            Exception exception = null;
            bool result = true;
            IList<ValidationMessage> validationMessages = null;
           
            try
            {
                if(languageRefType?.Count>0)
                {
                    if (validationMessages == null)
                        validationMessages = new List<ValidationMessage>();

                    if (FilteredDatas == null || FilteredDatas.Count <= 0)
                        FilteredDatas = FilterRecord(languageRefType, validationType);                

                        if(FilteredDatas != null && FilteredDatas?.Count > 0)
                        {
                            result = IsValidPayload(languageRefType, validationType, ref validationMessages);

                            if(FilteredDatas?.Count>0 && result)
                            {
                                IList<int> moduleNotExists = null;
                                var LanguageRefId = FilteredDatas.Where(x => x.LanguageReferenceTypeId.HasValue).Select(x => x.LanguageReferenceTypeId.Value).Distinct().ToList();

                                if ((dbLanguageRefType == null || dbLanguageRefType.Count <= 0) && validationType != ValidationType.Add)
                                    dbLanguageRefType = GetLanguageRefData(FilteredDatas);


                                if (validationType == ValidationType.Delete || validationType == ValidationType.Update)
                                {
                                    result = IsCountryIdExistInDb(LanguageRefId,
                                                                dbLanguageRefType,
                                                                ref moduleNotExists,
                                                                ref validationMessages);

                                    if (result && validationType!=ValidationType.Delete)

                                    {
                                        
                                        result = IsRecordValid(languageRefType,ref dbLanguageRefType, ref dbLanguages,ref dbRefType, ref validationMessages);
                                    }
                                    else
                                      result = false;
                                }
                               
                                else
                                    result = false;

                            }
                        }
                    }
                
            }
            catch(Exception ex)
            {
                result = false;
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), languageRefType);

            }
            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), result, exception);
        }

         private IList<LanguageReferenceType> FilterRecord(IList<LanguageReferenceType> languagerefTypes,
                                             ValidationType filterType)
        {
            IList<LanguageReferenceType> filteredLanguageRefTypeData = null;

            if (filterType == ValidationType.Add)
                filteredLanguageRefTypeData = languagerefTypes?.Where(x => x.RecordStatus.IsRecordStatusNew()).ToList();
            else if (filterType == ValidationType.Update)
                filteredLanguageRefTypeData = languagerefTypes?.Where(x => x.RecordStatus.IsRecordStatusModified()).ToList();
            else if (filterType == ValidationType.Delete)
                filteredLanguageRefTypeData = languagerefTypes?.Where(x => x.RecordStatus.IsRecordStatusDeleted()).ToList();

            return filteredLanguageRefTypeData;
        }



        
    }
}