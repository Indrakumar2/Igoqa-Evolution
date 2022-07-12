using AutoMapper;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Common.Models.Base;
using Evolution.Common.Models.Responses;
using Evolution.DbRepository.Models.SqlDatabaseContext;
using Evolution.Logging.Interfaces;
using Evolution.Master.Domain.Interfaces.Data;
using Evolution.Master.Domain.Interfaces.Services;
using Evolution.Master.Domain.Models;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Evolution.Master.Core
{
    public class InvoicePaymentTermsService : MasterService, IInvoicePaymentTermsService
    {
        private readonly IAppLogger<MasterService> _logger = null;
        private readonly IMasterRepository _repository = null;
        private readonly IMapper _mapper = null;
        private readonly JObject _messages = null;
        private readonly IMemoryCache _memoryCache = null;
        private readonly AppEnvVariableBaseModel _environment = null;
        public InvoicePaymentTermsService(IMapper mapper, IAppLogger<MasterService> logger, IMasterRepository repository, JObject messages,
                                          IMemoryCache memoryCache, IOptions<AppEnvVariableBaseModel> environment) : base(mapper, logger, repository, messages)
        {
            this._mapper = mapper;
            this._repository = repository;
            this._logger = logger;
            this._messages = messages;
            _memoryCache = memoryCache;
            _environment = environment.Value;
        }

        public Response Search(InvoicePaymentTerms search)
        {
            IList<InvoicePaymentTerms> result = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                var cacheKey = "InvoicePaymentTerms";
                if (search == null)
                    search = new InvoicePaymentTerms();

                if (search.IsFromRefresh)
                {
                    result = InvoicePaymentTerms(search);
                    if (_memoryCache.TryGetValue(cacheKey, out result))
                    {
                        _memoryCache.Remove(cacheKey);
                        _memoryCache.Set(cacheKey, result, ObjectExtension.CacheExpiry(_environment.AbsoluteExpiration, _environment.SlidingExpiration));
                    }
                }
                else
                {
                    if (!_memoryCache.TryGetValue(cacheKey, out result))
                    {
                        result = InvoicePaymentTerms(search);
                        _memoryCache.Set(cacheKey, result, ObjectExtension.CacheExpiry(_environment.AbsoluteExpiration, _environment.SlidingExpiration));
                    }
                }
            }
            catch (Exception ex)
            {
                responseType = ResponseType.Exception;
                this._logger.LogError(responseType.ToId(), this._messages[responseType.ToId()].ToString(), ex);
            }
            return new Response().ToPopulate(responseType, result, result?.Count);
        }

        private List<InvoicePaymentTerms> InvoicePaymentTerms(InvoicePaymentTerms search)
        {
            List<InvoicePaymentTerms> invoicePayment = null;
            var masterData = _mapper.Map<InvoicePaymentTerms, MasterData>(search);
            //If condition added on 24-Sep2020 to find how complied queries perform
            if (search.Name.HasEvoWildCardChar())
                invoicePayment = this._repository.FindBy(_mapper.Map<MasterData, Data>(masterData).ToExpression())
                                            .Where(x => x.MasterDataTypeId == Convert.ToInt32(MasterType.InvoicePaymentTerms) && (search.IsActive == null || x.IsActive == search.IsActive))
                                            .Select(x => _mapper.Map<MasterData, InvoicePaymentTerms>(_mapper.Map<Data, MasterData>(x))).ToList();
            else
            {
                masterData.MasterDataTypeId = Convert.ToInt32(MasterType.InvoicePaymentTerms);
                invoicePayment = _repository.GetMasterData(masterData).Select(x => _mapper.Map<MasterData, InvoicePaymentTerms>(x)).ToList();
            }

            //Added for Bug-587
            return SortedList(invoicePayment);
        }

        public List<InvoicePaymentTerms> SortedList(List<InvoicePaymentTerms> unsoredList)
        {
            var invoicePayment = new List<KeyValuePair<InvoicePaymentTerms, int>>();
            unsoredList?.ForEach(x =>
            {
                if (!string.IsNullOrEmpty(x.Name))
                    invoicePayment.Add(new KeyValuePair<InvoicePaymentTerms, int>(x, (Regex.Split(x.Name, @"\D+")[0]).ToString() == "" ? 0 : Convert.ToInt32(Regex.Split(x.Name, @"\D+")[0])));
            });
            var numInvoicePayment = invoicePayment.Where(x => x.Value != 0).OrderBy(x => x.Value).Select(x => x.Key).ToList();
            var nonNumInvoicePayment = invoicePayment.Where(x => x.Value == 0).OrderBy(x => x.Key.Name).Select(x => x.Key).ToList();

            return numInvoicePayment?.Union(nonNumInvoicePayment)?.ToList();
        }
    }
}
