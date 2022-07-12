using AutoMapper;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Common.Models.Messages;
using Evolution.Common.Models.Responses;
using Evolution.Logging.Interfaces;
using Evolution.Master.Domain.Interfaces.Data;
using Evolution.Master.Domain.Interfaces.Services;
using Evolution.Master.Domain.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Transactions;

namespace Evolution.Master.Core
{
    public class ModuleDocumentTypeService : MasterService, IModuleDocumentTypeService
    {
        private readonly IAppLogger<MasterService> _logger = null;
        private readonly IModuleDocumentTypeRepository _repository = null;
        private readonly IMapper _mapper = null;
        private readonly JObject _messages = null;

        public ModuleDocumentTypeService(IMapper mapper,
                                            IAppLogger<MasterService> logger,
                                            IMasterRepository masterRepository,
                                            IModuleDocumentTypeRepository repository,JObject messages) : base(mapper, logger, masterRepository,messages)
        {
            this._logger = logger;
            this._repository = repository;
            this._mapper = mapper;
            this._messages = JObject.Parse(File.ReadAllText(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"Message_EN.json")));
        }

        public Response Search(ModuleDocumentType search)
        {
            IList<ModuleDocumentType> result = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                if (search == null)
                    search = new ModuleDocumentType();
                using (var tranScope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))                
                {
                    result = this._repository.Search(search);
                    tranScope.Complete();
                }
            }
            catch (SqlException sqlE)
            {
                responseType = ResponseType.DbException;
                this._logger.LogError(responseType.ToId(), this._messages[responseType.ToId()].ToString(), sqlE);
            }
            catch (Exception ex)
            {
                responseType = ResponseType.Exception;
                this._logger.LogError(responseType.ToId(), this._messages[responseType.ToId()].ToString(), ex);
            }

            return new Response().ToPopulate(responseType, result, result?.Count);
        }

        public bool IsValidDocumentType(DocumentModuleType documentModuleType, IList<string> documentTypeNames, ModuleType moduleType, ref List<MessageDetail> errorMessages)
        {
            List<MessageDetail> messages = new List<MessageDetail>();
            if (errorMessages == null)
                errorMessages = new List<MessageDetail>();

            var dbDocTypes = this._repository.FindBy(x => x.ModuleId == Convert.ToInt32(documentModuleType)
                                                    && documentTypeNames.Contains(x.DocumentType.Name)).ToList();

            var documentTypeNotExists = documentTypeNames.Where(x => !dbDocTypes.Any(x1 => x1.DocumentType.Name == x)).ToList();

            documentTypeNotExists.ForEach(x =>
            {
                string errorCode = MessageType.DocumentTypeNotExists.ToId();
                messages.Add(new MessageDetail(moduleType, errorCode, string.Format(_messages[errorCode].ToString(), x)));
            });

            if (messages.Count > 0)
                errorMessages.AddRange(messages);

            return errorMessages?.Count <= 0;
        }

    }
}
