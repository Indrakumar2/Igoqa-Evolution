using Evolution.Master.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Text;
using Evolution.Master.Domain.Models;
using Evolution.Common.Models.Responses;
using Evolution.Logging.Interfaces;
using Evolution.DbRepository.Interfaces.Master;
using AutoMapper;
using System.Linq;
using System.Data.SqlClient;
using Evolution.Common.Extensions;
using Evolution.Common.Enums;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Reflection;

namespace Evolution.Master.Core.Services
{
    public class CustomerCommodityService : ICustomerCommodityService
    {
        private readonly IAppLogger<CustomerCommodityService> _logger = null;
        private readonly ICustomerCommodityRepository _repository = null;
        private readonly IMapper _mapper = null;
        private readonly JObject _messages = null;

        public CustomerCommodityService(IMapper mapper, IAppLogger<CustomerCommodityService> logger, ICustomerCommodityRepository repository)
        {
            this._logger = logger;
            this._repository = repository;
            this._mapper = mapper;
            this._messages = JObject.Parse(File.ReadAllText(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"Message_EN.json")));
        }

        public Response Search(CustomerCommodity search)
        {
            IList<CustomerCommodity> result = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                if (search == null)
                    search = new CustomerCommodity();
                if (search.CustomerName != null)
                {
                    result = _repository.FindBy(x => string.IsNullOrEmpty(search.CustomerName) ||
                                                     x.Customer.Name == search.CustomerName)
                                        .Select(x => new CustomerCommodity()
                                        {
                                            CustomerName = x.Customer.Name.Trim(),
                                            CustomerCode = x.CustomerId.ToString(),
                                            CommodityName = x.Commodity.Name
                                        })
                                        .OrderBy(x => x.CustomerName)
                                        .ToList();
                }
                else if (search.CustomerCode != null)
                {
                    result = _repository.FindBy(x => x.Customer.Code == search.CustomerCode)
                                        .Select(x => new CustomerCommodity()
                                        {
                                            CustomerName = x.Customer.Name.Trim(),
                                            CustomerCode = x.Customer.Code.ToString(),
                                            CommodityName = x.Commodity.Name
                                        })
                                        .OrderBy(x => x.CommodityName)
                                        .ToList();
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
    }

}
