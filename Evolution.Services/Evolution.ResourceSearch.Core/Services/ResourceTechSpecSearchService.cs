using AutoMapper;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Common.Models.Responses;
using Evolution.Google.Model.Interfaces;
using Evolution.Logging.Interfaces;
using Evolution.Google.Model.Models;
using Evolution.ResourceSearch.Domain.Interfaces.Data;
using Evolution.ResourceSearch.Domain.Interfaces.ResourceSearch;
using Evolution.ResourceSearch.Domain.Models.ResourceSearch;
using Evolution.TechnicalSpecialist.Domain.Interfaces.Data;
using Newtonsoft.Json.Linq;
using MongoDB.Driver.GeoJsonObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using DomainModel = Evolution.ResourceSearch.Domain.Models.ResourceSearch;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using System.Text.RegularExpressions;
using Evolution.Company.Domain.Interfaces.Companies;
using Evolution.Common.Models.Messages;
using System.Transactions;

namespace Evolution.ResourceSearch.Core.Services
{
    public class ResourceTechSpecSearchService : IResourceTechSpecSearchService
    {
        private readonly IMapper _mapper = null;
        private readonly JObject _messages = null;
        private readonly IAppLogger<ResourceTechSpecSearchService> _logger = null;
        private readonly ITechnicalSpecialistRepository _tsRepository = null;
        private readonly IResourceSearchRepository _resourceSearchRepository = null;
        private readonly IMongoGeoCoordinateService _mongoGeoCoordinateService = null;
        private readonly ICompanyService _companyService = null;
        private readonly IGeoCoordinateService _geoCoordinateService = null;
        private readonly IDistanceService _distanceService = null;

        public ResourceTechSpecSearchService(IMapper mapper,
                               JObject messages,
                               IAppLogger<ResourceTechSpecSearchService> logger,
                               ITechnicalSpecialistRepository tsRepository,
                               IResourceSearchRepository resourceSearchRepository,
                               IMongoGeoCoordinateService mongoGeoCoordinateService,
                               IGeoCoordinateService geoCoordinateService,
                               IDistanceService distanceService,
                               ICompanyService companyService)
        {
            this._mapper = mapper;
            this._messages = messages;
            this._logger = logger;
            this._tsRepository = tsRepository;
            this._resourceSearchRepository = resourceSearchRepository;
            this._mongoGeoCoordinateService = mongoGeoCoordinateService;
            this._geoCoordinateService = geoCoordinateService;
            this._distanceService = distanceService;
            _companyService = companyService;
        }

        private string GetSupplierAddressWithoutSupplierName(string supplierFullAddress)
        {
            string supplierLocation = string.Empty;
            if (!string.IsNullOrEmpty(supplierFullAddress))
            {

                string[] locationArr = supplierFullAddress.Split(',');
                for (int i = 0; i < locationArr.Length; i++)
                {
                    if (!string.IsNullOrEmpty(locationArr[i]))
                    {
                        if (i != 0)
                        {
                            supplierLocation += "" + locationArr[i]?.Trim() + ",";
                        }
                    }
                }
            }
            return !string.IsNullOrEmpty(supplierLocation) ? supplierLocation.TrimEnd(',') : supplierLocation;
        }

        public Response Get(DomainModel.ResourceSearch searchModel, bool IncludeGeoLocation = false)
        {
            IList<ResourceTechSpecSearchResult> result = new List<ResourceTechSpecSearchResult>();
            IList<ResourceSearchTechSpecInfo> distinctResourceInfo = new List<ResourceSearchTechSpecInfo>();
            Exception exception = null;
            List<string> fromLocations = null;
            List<DistanceSearchResult> distanceSearchResults = null;
            try
            {
                IList<ResourceSearchTechSpecInfo> searchRes = null;
                if (searchModel != null)
                {
                    using (var tranScope = new TransactionScope(TransactionScopeOption.RequiresNew,
                                new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                    {
                        searchRes = this._resourceSearchRepository.SearchTechSpech(searchModel);
                        tranScope.Complete();
                    }
                    if (searchRes?.Count > 0)
                    {
                        result = ProcessSupplierLocationInfo(searchModel, searchRes);

                        if (searchModel.SearchParameter.OptionalSearch != null && searchModel.SearchParameter.OptionalSearch.Radius > 0)
                        {
                            FilterResourceOnRadiusInfo(searchModel.SearchParameter.OptionalSearch.Radius, ref result);
                        }

                        if (result?.Count > 0 && IncludeGeoLocation)
                        {
                            ProcessMongoGeoLocationInfo(ref result); // Fetch Latitude and longitude from mongoDB 
                        }

                        if (searchModel?.SearchType == ResourceSearchType.ARS.ToString() || searchModel?.SearchType == ResourceSearchType.PreAssignment.ToString())
                        {
                            fromLocations = new List<string>();
                            var exceptionRes = this._resourceSearchRepository.GetExceptionTSList(searchModel);
                            // 1328: Added code to calculate distance for Exception List.
                            if (exceptionRes != null)
                            {
                                //string location = !string.IsNullOrEmpty(searchModel.SearchParameter.Supplier)
                                //? searchModel.SearchParameter.Supplier.Replace(",", "") + ", " + searchModel.SearchParameter.SupplierFullAddress.Trim() :
                                //searchModel.SearchParameter.SupplierFullAddress.Trim();
                                string location = searchModel.SearchParameter.SupplierFullAddress.Trim();
                                fromLocations.Add(location);
                                searchModel.SearchParameter.SubSupplierInfos?.ToList().ForEach(subSupp =>
                                {
                                    //string subSupplierLocation = !string.IsNullOrEmpty(subSupp.SubSupplier)
                                    //? subSupp.SubSupplier.Replace(",", "") + ", " + subSupp.SubSupplierFullAddress.Trim() : subSupp.SubSupplierFullAddress.Trim();
                                    string subSupplierLocation = subSupp.SubSupplierFullAddress.Trim();
                                    fromLocations.Add(subSupplierLocation);
                                });
                                distinctResourceInfo = exceptionRes.Where(x => !string.IsNullOrEmpty(x.GoogleAddress))?.GroupBy(x => x.GoogleAddress)?.Select(x => x.FirstOrDefault())?.ToList();
                                distanceSearchResults = CalculateDistanceForExceptionList(fromLocations, distinctResourceInfo);
                                string supplierLocationId = string.Empty;
                                foreach (var groupOrigin in distanceSearchResults.GroupBy(x => x.Origin))
                                {
                                    supplierLocationId = string.Empty;
                                    string formttedSupplierLoc = string.Empty;

                                    foreach (var ts in distinctResourceInfo)
                                    {
                                        foreach (var distance in groupOrigin.Select(x => x).ToList())
                                        {
                                            if (ts.GoogleAddress == distance.Destination)
                                            {
                                                ts.DistanceFromVenderInKm = distance.Distance.Kilometer.Replace("km", "").Replace("1 m", "0");
                                                ts.DistanceFromVenderInMile = Math.Round((distance.Distance.Meter >= 0) ? (distance.Distance.Meter / 1609.344) : -1);//def 902 fix : if Google unbale to find location ,Then set distance to negative Number .So that it will result in exception list.
                                                break;
                                            }
                                            else
                                            {
                                                ts.DistanceFromVenderInKm = string.Empty;
                                                ts.DistanceFromVenderInMile = -1; //def 902 fix : if Google unbale to find location ,Then set distance to negative Number .So that it will result in exception list.
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        if (searchModel?.SearchType == ResourceSearchType.ARS.ToString() || searchModel?.SearchType == ResourceSearchType.PreAssignment.ToString() || searchModel?.SearchType == ResourceSearchType.Quick.ToString()) // D-625
                        {
                            result.ToList().ForEach(x =>
                            {
                                if (!string.IsNullOrEmpty(x.SupplierLocationId))
                                {
                                    if (!string.IsNullOrEmpty(x.SupplierLocationId.Split('_')?[0]))
                                        if (searchModel.SearchParameter.FirstVisitSupplierId == Convert.ToInt64(x.SupplierLocationId?.Split('_')?[0]?.Trim()))
                                            x.IsFirstVisit = true;
                                }

                                if (!string.IsNullOrEmpty(x.SupplierLocationId) && x.SupplierLocationId.Trim() == string.Format("{0}_{1}", searchModel?.SearchParameter?.SupplierId, searchModel?.SearchParameter?.SupplierLocation?.Trim()))
                                {
                                    /*Note - This is added to populate IsSelected flag of ARS properly. Before fixed it was coming as true for all records if one record is true.The code is also optimised */
                                    var epins = searchModel?.SearchParameter?.SelectedTechSpecInfo?.Select(x2 => x2.Epin)?.ToList();
                                    if (epins?.Count > 0)
                                        x.ResourceSearchTechspecInfos?.Where(y => epins.Contains(y.Epin))?.Select(y => { y.IsSelected = true; return y; })?.ToList();
                                    else
                                        x.ResourceSearchTechspecInfos?.Select(y => { y.IsSelected = false; return y; })?.ToList();
                                }
                                if (searchModel?.SearchParameter?.SubSupplierInfos?.Count > 0)
                                {
                                    searchModel?.SearchParameter?.SubSupplierInfos?.ToList().ForEach(subSup =>
                                    {
                                        if (x.SupplierLocationId.Trim() == string.Format("{0}_{1}", subSup?.SubSupplierId, subSup?.SubSupplierLocation?.Trim()))
                                        {
                                            /*Note - This is added to populate IsSelected flag of ARS properly. Before fixed it was coming as true for all records if one record is true.The code is also optimised */
                                            var epins = subSup.SelectedSubSupplierTS?.Select(x2 => x2.Epin)?.ToList();
                                            if (epins?.Count > 0)
                                                x.ResourceSearchTechspecInfos?.Where(y => epins.Contains(y.Epin)).Select(y => { y.IsSelected = true; return y; })?.ToList();
                                            else
                                                x.ResourceSearchTechspecInfos?.Select(y => { y.IsSelected = false; return y; })?.ToList();
                                        }
                                    });
                                }
                            });
                            // Neglect Exception resource search in quick search and override scenario (only for Override Validated Task).
                            // ITK D 1314 Commenting the if condition
                            // As part of 1325 and 1328 the exception List code has been moved inside this condition. 
                            if (searchModel?.SearchType != ResourceSearchType.Quick.ToString()
                                    && !string.IsNullOrEmpty(searchModel.SearchParameter.OPCompanyCode)
                                    //&& !(searchModel?.OverridenPreferredResources?.Count >= 0 // 1326 already we are excluding approved Overriden Resource in Line number 174. Because of this condition all exception list resources are shown in ARS Grid.
                                    && (searchModel?.SearchType?.ToString() == ResourceSearchType.PreAssignment.ToString() || //sanity 213 fix
                                           (searchModel?.SearchType?.ToString() == ResourceSearchType.ARS.ToString() && searchModel?.SearchAction?.ToString() == ResourceSearchAction.PLO.ToString()) //sanity 213 fix
                                        || (searchModel?.SearchType?.ToString() == ResourceSearchType.ARS.ToString() && searchModel?.SearchAction?.ToString() == ResourceSearchAction.ARR.ToString())
                                        || (searchModel?.SearchType.ToString() == ResourceSearchType.ARS.ToString() && string.IsNullOrEmpty(searchModel?.SearchAction?.ToString()))
                                        || (searchModel?.SearchType.ToString() == ResourceSearchType.ARS.ToString() && searchModel?.SearchAction.ToString() == ResourceSearchAction.OPR.ToString())
                                        || (searchModel?.SearchType.ToString() == ResourceSearchType.ARS.ToString() && searchModel?.SearchAction.ToString() == ResourceSearchAction.AR.ToString()) //Added for IGO - D960
                                    ))
                            {
                                var locationsWithNoDistanceFromGoogle = fromLocations.Except(distanceSearchResults.GroupBy(x => x.Origin)?.Select(x => x.Key)?.ToList());
                                FilterResourceOnCompanyDistanceRange(searchModel.SearchParameter.OPCompanyCode, distinctResourceInfo, searchModel, ref result, locationsWithNoDistanceFromGoogle?.ToList());
                            }
                           
                            result = result.OrderByDescending(x => x.ResourceSearchTechspecInfos.Count).ToList();
                            result = result.GroupBy(x => x.Location).Select(x => x.First()).ToList();
                            if (result?.ToList()?.Count > 0)
                                result = result?.OrderByDescending(x => x.IsFirstVisit)?.ToList();
                        }
                    }
                    else
                    {
                        ResourceTechSpecSearchResult supplier = new ResourceTechSpecSearchResult()
                        {
                            Location = searchModel.SearchParameter.Supplier + ", " + searchModel.SearchParameter.SupplierFullAddress,
                            ResourceSearchTechspecInfos = new List<ResourceSearchTechSpecInfo>(),
                            SearchExceptionResourceInfos = new List<ResourceSearchTechSpecInfo>(),
                            SupplierLocationId = searchModel.SearchParameter.SupplierFullAddress,
                        };
                        result.Add(supplier);
                        if (searchModel.SearchParameter?.SubSupplierInfos != null && searchModel.SearchParameter?.SubSupplierInfos.Count > 0)
                        {
                            foreach (var item in searchModel.SearchParameter.SubSupplierInfos)
                            {
                                result.Add(new ResourceTechSpecSearchResult()
                                {
                                    Location = item.SubSupplier + ", " + item.SubSupplierFullAddress,
                                    Address = item.SubSupplierFullAddress,
                                    ResourceSearchTechspecInfos = new List<ResourceSearchTechSpecInfo>(),
                                    SearchExceptionResourceInfos = new List<ResourceSearchTechSpecInfo>(),
                                    SupplierLocationId = item.SubSupplierFullAddress
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), searchModel);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception, result?.Count);

        }

        public Response GetGeoLocationInfo(IList<ResourceTechSpecSearchResult> resourceResultInfo)
        {
            Exception exception = null;
            try
            {
                if (resourceResultInfo?.Count > 0)
                {
                    ProcessMongoGeoLocationInfo(ref resourceResultInfo); // Fetch Latitude and longitude from mongoDB
                    ProcessGoogleGeoLocationInfo(ref resourceResultInfo); // Fetch Latitude and longitude from google Api
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), resourceResultInfo);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, resourceResultInfo, exception, resourceResultInfo?.Count);

        }

        private IList<ResourceTechSpecSearchResult> ProcessSupplierLocationInfo(DomainModel.ResourceSearch searchModel, IList<ResourceSearchTechSpecInfo> resourceInfo)
        {
            var result = new List<ResourceTechSpecSearchResult>();
            var distanceResult = new List<DistanceSearchResult>();

            // Added for PLO scenario
            IList<ResourceSearchTechSpecInfo> mainSupplierResourceInfo = new List<ResourceSearchTechSpecInfo>();
            IList<ResourceSearchTechSpecInfo> subSupplierResourceInfo = new List<ResourceSearchTechSpecInfo>();

            if (searchModel?.SearchAction == ResourceSearchAction.SSSPC.ToString())
            {

                var mainSupplierEpin = searchModel?.SearchParameter?.SelectedTechSpecInfo?.Select(x => x.Epin)?.ToList();
                var subSupplierEpin = searchModel?.SearchParameter?.SubSupplierInfos?.SelectMany(x => x.SelectedSubSupplierTS)?.Select(x => x.Epin)?.ToList();

                mainSupplierResourceInfo = resourceInfo.Where(x => mainSupplierEpin?.Count > 0 && mainSupplierEpin.Contains(x.Epin))?.ToList();
                subSupplierResourceInfo = resourceInfo.Where(x => subSupplierEpin?.Count > 0 && subSupplierEpin.Contains(x.Epin))?.ToList();
            }

            //TODO : Need to Change with better approach for sorting multiple column and values
            /* 1st sorting will be Employment Type – Full time employees, Part Time Employees, Independent Contractors, Third Party Contractors,Office Staff and rest.
               2nd sorting will be based on Schedule Status – Sorted within Employment Type by Available, Tentative, Booked.
               3rd sorting based on distance ascending order. */

            if (searchModel?.SearchParameter != null && (!string.IsNullOrEmpty(searchModel.SearchParameter.SupplierLocation) || searchModel.SearchParameter.SubSupplierInfos?.Count > 0) &&
                resourceInfo?.Count > 0)
            {
                var fromLocations = new List<string>();
                var fromLocation = new List<Tuple<string, string>>();
                var toLocations = new List<string>();
                var supplierLocationId = string.Empty;
                var supplierLocation = string.Empty;
                var supplierAddress = string.Empty; // D - 912
                IList<ResourceSearchTechSpecInfo> distinctResourceInfo = null;

                //def 1323
                distinctResourceInfo = resourceInfo?.Where(x => !string.IsNullOrEmpty(x.GoogleAddress))?.GroupBy(x => x.GoogleAddress)?.Select(x => x.FirstOrDefault())?.ToList();
                //string location = !string.IsNullOrEmpty(searchModel.SearchParameter.Supplier)
                //    ? searchModel.SearchParameter.Supplier.Replace(",", "") + ", " + searchModel.SearchParameter.SupplierFullAddress.Trim() :
                //    searchModel.SearchParameter.SupplierFullAddress.Trim();
                string location = searchModel?.SearchParameter?.SupplierFullAddress?.Trim();
                fromLocations.Add(location);
                fromLocation.Add(new Tuple<string, string>(location, "Main"));
                if (searchModel.SearchType == "ARS")
                {
                    List<int> subSupplierIds = searchModel.SearchParameter.SubSupplierInfos?.Select(x => x.SubSupplierId)?.ToList();
                    if (subSupplierIds?.Count > 0)
                    {
                        var subSupplierPostalCode = _resourceSearchRepository.GetSupplier(subSupplierIds);
                        if (subSupplierPostalCode?.Count > 0)
                        {
                            searchModel.SearchParameter.SubSupplierInfos?.ToList().ForEach(subSupp =>
                            {
                                string postalCode = subSupplierPostalCode?.FirstOrDefault(x => x.Id == subSupp.SubSupplierId)?.PostalCode;
                                subSupp.SubSupplierFullAddress = postalCode == null ? subSupp.SubSupplierFullAddress?.Trim() : string.Join(",", subSupp.SubSupplierFullAddress?.Trim(), postalCode); 
                            });
                        }
                    }
                }
                searchModel.SearchParameter.SubSupplierInfos?.ToList().ForEach(subSupp =>
                {
                    //string subSupplierLocation = !string.IsNullOrEmpty(subSupp.SubSupplier)
                    //? subSupp.SubSupplier.Replace(",", "") + ", " + subSupp.SubSupplierFullAddress.Trim() :
                    //subSupp.SubSupplierFullAddress.Trim();
                    string subSupplierLocation = subSupp?.SubSupplierFullAddress?.Trim();
                    fromLocations.Add(subSupplierLocation);
                    fromLocation.Add(new Tuple<string, string>(subSupplierLocation, "Sub"));
                });

                fromLocations = fromLocations.Where(x => !string.IsNullOrEmpty(x)).ToList();
                /*  DISTANCE MATRIX LIMITATIONS:
                 *  Maximum of 25 origins or 25 destinations per request.
                 *  Maximum 100 elements per server-side request.
                 *  Maximum 100 elements per client-side request.
                 *  1000 elements per second (EPS), calculated as the sum of client-side and server-side queries
                 */
                if (fromLocations.Count > 0)
                {
                    for (int srcCount = 0; srcCount <= (fromLocations.Count / 2); srcCount++)
                    {
                        var sourceLoc = fromLocations.Skip(srcCount * 2).Take(2).ToList();
                        var resourceSplitCount = sourceLoc?.Count > 0 ? 25 - sourceLoc.Count : 25;
                        if (distinctResourceInfo.Count > resourceSplitCount)
                        {
                            for (int page = 0; page <= (distinctResourceInfo.Count / resourceSplitCount); page++)
                            {
                                var pagedSearchRes = distinctResourceInfo.Skip(page * resourceSplitCount).Take(resourceSplitCount).ToList();
                                toLocations = pagedSearchRes.Select(x => x.GoogleAddress)?.Distinct()?.ToList();
                                var pagedDistanceResult = this._distanceService.DrivingDistance(sourceLoc, toLocations);
                                if (pagedDistanceResult?.Count > 0)
                                    distanceResult.AddRange(pagedDistanceResult);
                            }
                        }
                        else
                        {
                            List<string> sourceLocation = sourceLoc.Select(a => a)?.Distinct().ToList();
                            toLocations = distinctResourceInfo.Select(x => x.GoogleAddress)?.Distinct()?.ToList();
                            var nonPagedDistanceResult = this._distanceService.DrivingDistance(sourceLocation, toLocations)?.ToList();
                            if (nonPagedDistanceResult?.Count > 0)
                                distanceResult.AddRange(nonPagedDistanceResult);
                        }
                    }
                }

                if (distanceResult?.Count() > 0)
                {
                    bool isSubSupplierAdded = false;
                    foreach (var groupOrigin in distanceResult.GroupBy(x => x.Origin))
                    {
                        supplierLocationId = string.Empty;
                        supplierLocation = string.Empty;
                        supplierAddress = string.Empty;
                        string formttedSupplierLoc = string.Empty;

                        // Added for PLO scenario
                        var resourceInfoClone = (mainSupplierResourceInfo?.Count > 0 || subSupplierResourceInfo?.Count > 0)
                                                ? string.Equals(searchModel?.SearchParameter?.SupplierFullAddress?.Trim(), groupOrigin.Key.Trim())
                                                ? mainSupplierResourceInfo?.Select(x => x.Clone() as ResourceSearchTechSpecInfo).ToList()
                                                : subSupplierResourceInfo?.Select(x => x.Clone() as ResourceSearchTechSpecInfo).ToList()
                                                : resourceInfo?.Select(x => x.Clone() as ResourceSearchTechSpecInfo).ToList();
                        foreach (var ts in resourceInfoClone)
                        {
                            foreach (var distance in groupOrigin.Select(x => x).ToList())
                            {
                                if (ts.GoogleAddress == distance.Destination)
                                {
                                    ts.DistanceFromVenderInKm = distance.Distance.Kilometer.Replace("km", "").Replace("1 m", "0");
                                    ts.DistanceFromVenderInMile = Math.Round((distance.Distance.Meter >= 0) ? (distance.Distance.Meter / 1609.344) : -1);//def 902 fix : if Google unbale to find location ,Then set distance to negative Number .So that it will result in exception list.
                                    break;
                                }
                                else
                                {
                                    ts.DistanceFromVenderInKm = string.Empty;
                                    ts.DistanceFromVenderInMile = -1; //def 902 fix : if Google unbale to find location ,Then set distance to negative Number .So that it will result in exception list.
                                }
                            }
                        }

                        //Tuple<string, string> tupleLocation = fromLocation?.Where(main => main.Item2 == "Main")?.FirstOrDefault();
                        List<Tuple<string, string>> tupleLocation = fromLocation?.Where(main => main.Item1 == groupOrigin.Key.Trim())?.ToList(); //Changes for Live D693
                        if (tupleLocation != null)
                        {
                            if (tupleLocation.Any(x => x.Item2 == "Main"))
                            {
                                string llocation = searchModel.SearchParameter.SupplierFullAddress?.Trim();
                                if (string.Equals(llocation, groupOrigin.Key.Trim()))
                                {
                                    supplierLocationId = string.Format("{0}_{1}", searchModel?.SearchParameter?.SupplierId, searchModel?.SearchParameter?.SupplierLocation.Trim());
                                    supplierLocation = searchModel?.SearchParameter.Supplier + ", " + searchModel?.SearchParameter?.SupplierFullAddress;
                                    supplierAddress = searchModel?.SearchParameter.SupplierFullAddress; // D - 912
                                }
                                else
                                {
                                    if (searchModel?.SearchParameter != null)
                                    {
                                        supplierLocationId = string.Format("{0}_{1}", searchModel?.SearchParameter?.SupplierId, searchModel?.SearchParameter?.SupplierLocation.Trim());
                                        supplierLocation = searchModel?.SearchParameter.Supplier + ", " + searchModel?.SearchParameter.SupplierFullAddress;
                                        supplierAddress = searchModel?.SearchParameter.SupplierFullAddress; // D - 912 
                                    }
                                }
                                result.Add(new ResourceTechSpecSearchResult
                                {
                                    Location = supplierLocation,// groupOrigin.Key,
                                    Address = supplierAddress, // D - 912
                                    SupplierLocationId = supplierLocationId.Trim(),
                                    ResourceSearchTechspecInfos = AssignResource(resourceInfoClone)
                                });
                            }

                            if (tupleLocation.Any(x => x.Item2 == "Sub"))
                            {
                                if (searchModel.SearchParameter?.SubSupplierInfos?.Count > 0)
                                {
                                    var tupLocation = tupleLocation.FirstOrDefault(x => x.Item2 == "Sub").Item1;
                                    foreach (var item in searchModel.SearchParameter.SubSupplierInfos)
                                    {
                                        if (!string.IsNullOrEmpty(item.SubSupplierFullAddress))
                                        {
                                            string llocation = item.SubSupplierFullAddress?.Trim();
                                            if (tupLocation == llocation)
                                            {
                                                if (llocation.Equals(tupLocation.Trim()))
                                                //if (llocation.Equals(tupleLocation.FirstOrDefault(x => x.Item2 == "Sub").Item1.Trim()))
                                                {
                                                    if (subSupplierResourceInfo?.Count > 0)
                                                    {
                                                        var subSupplierResourceEpin = item?.SelectedSubSupplierTS?.Select(x => x.Epin).ToList();
                                                        if (subSupplierResourceEpin?.Count > 0)
                                                            resourceInfoClone = resourceInfoClone.Where(x => subSupplierResourceEpin.Contains(x.Epin)).ToList();
                                                    }

                                                    // D - 912
                                                    // Added for PLO scenario
                                                    supplierLocationId = string.Format("{0}_{1}", item?.SubSupplierId, item?.SubSupplierLocation.Trim());
                                                    supplierLocation = item.SubSupplier + ", " + llocation;
                                                    supplierAddress = item?.SubSupplierFullAddress;
                                                    isSubSupplierAdded = true;
                                                    result.Add(new ResourceTechSpecSearchResult
                                                    {
                                                        Location = supplierLocation,// groupOrigin.Key,
                                                        Address = supplierAddress, // D - 912
                                                        SupplierLocationId = supplierLocationId.Trim(),
                                                        ResourceSearchTechspecInfos = AssignResource(resourceInfoClone)
                                                    });
                                                }
                                            }
                                            else
                                            {
                                                var SubSupResourceInfo = subSupplierResourceInfo?.Count > 0 ? subSupplierResourceInfo : resourceInfo;

                                                SubSupResourceInfo = SubSupResourceInfo.Select(x =>
                                                {
                                                    x.DistanceFromVenderInKm = string.Empty;
                                                    x.DistanceFromVenderInMile = -1; //def 902 fix : if Google unbale to find location ,Then set distance to negative Number .So that it will result in exception list.
                                                    return x;
                                                }).ToList();
                                                supplierLocationId = string.Format("{0}_{1}", item?.SubSupplierId, item?.SubSupplierLocation.Trim());
                                                supplierLocation = item.SubSupplier + ", " + llocation;
                                                supplierAddress = item?.SubSupplierFullAddress;
                                                isSubSupplierAdded = true;
                                                result.Add(new ResourceTechSpecSearchResult
                                                {
                                                    Location = supplierLocation,// groupOrigin.Key,
                                                    Address = supplierAddress, // D - 912
                                                    SupplierLocationId = supplierLocationId.Trim(),
                                                    ResourceSearchTechspecInfos = AssignResource(SubSupResourceInfo)
                                                });
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                    fromLocations.Except(distanceResult.ToList()
                      .GroupBy(x => x.Origin)
                      .Select(x1 => x1.Key)).ToList().ForEach(y1 =>
                      {
                          var resourceInfoExceptClone = resourceInfo?.Select(x => x.Clone() as ResourceSearchTechSpecInfo).ToList();
                          Tuple<string, string> tupleLocation = fromLocation?.Where(main => main.Item2 == "Main")?.FirstOrDefault();
                          if (tupleLocation != null)
                          {
                              if (tupleLocation.Item1 == y1.Trim()) //Changes for Live D792 
                              {
                                  string llocation = searchModel?.SearchParameter?.Supplier.Replace(",", "") + ", " + searchModel?.SearchParameter?.SupplierFullAddress?.Trim();
                                  if (string.Equals(llocation, y1.Trim()))
                                  {
                                      supplierLocationId = string.Format("{0}_{1}", searchModel?.SearchParameter?.SupplierId, searchModel?.SearchParameter?.SupplierLocation.Trim());
                                      supplierLocation = searchModel?.SearchParameter.Supplier + ", " + searchModel?.SearchParameter.SupplierFullAddress;
                                      supplierAddress = searchModel?.SearchParameter.SupplierFullAddress; // D - 912
                                  }
                                  else
                                  {
                                      if (searchModel?.SearchParameter != null)
                                      {
                                          supplierLocationId = string.Format("{0}_{1}", searchModel?.SearchParameter?.SupplierId, searchModel?.SearchParameter?.SupplierLocation.Trim());
                                          supplierLocation = searchModel?.SearchParameter.Supplier + ", " + searchModel?.SearchParameter.SupplierFullAddress;
                                          supplierAddress = searchModel?.SearchParameter.SupplierFullAddress; // D - 912 
                                      }
                                  }
                                  result.Add(new ResourceTechSpecSearchResult
                                  {
                                      Location = supplierLocation,//y1,
                                      Address = supplierAddress, // D - 912
                                      SupplierLocationId = supplierLocationId.Trim(),
                                      ResourceSearchTechspecInfos = AssignResource(resourceInfoExceptClone)
                                  });
                              }
                              tupleLocation = fromLocation?.Where(main => main.Item2 == "Sub")?.FirstOrDefault();
                              if (tupleLocation != null && !isSubSupplierAdded)
                              {
                                  if (searchModel.SearchParameter?.SubSupplierInfos?.Count > 0)
                                  {
                                      foreach (var item in searchModel.SearchParameter.SubSupplierInfos)
                                      {
                                          if (!string.IsNullOrEmpty(item.SubSupplierFullAddress))
                                          {
                                              string llocation = item.SubSupplierFullAddress?.Trim();
                                              if (llocation.Trim().Equals(tupleLocation.Item1.Trim()))
                                              {
                                                  if (subSupplierResourceInfo?.Count > 0)
                                                  {
                                                      var subSupplierResourceEpin = item?.SelectedSubSupplierTS?.Select(x => x.Epin).ToList();
                                                      if (subSupplierResourceEpin?.Count > 0)
                                                          resourceInfoExceptClone = resourceInfoExceptClone.Where(x => subSupplierResourceEpin.Contains(x.Epin)).ToList();
                                                  }
                                                  supplierLocationId = string.Format("{0}_{1}", item?.SubSupplierId, item?.SubSupplierLocation.Trim());
                                                  supplierLocation = item?.SubSupplier + ", " + item?.SubSupplierFullAddress;
                                                  supplierAddress = item?.SubSupplierFullAddress; // D - 912
                                                  result.Add(new ResourceTechSpecSearchResult
                                                  {
                                                      Location = supplierLocation,//y1,
                                                      Address = supplierAddress, // D - 912
                                                      SupplierLocationId = supplierLocationId.Trim(),
                                                      ResourceSearchTechspecInfos = AssignResource(resourceInfoExceptClone)
                                                  });
                                              }
                                          }
                                      }
                                  }
                              }
                          }

                      });

                }
                else
                { // If distance Api returns nothing
                    if (!string.IsNullOrEmpty(searchModel.SearchParameter.SupplierLocation.Trim()) || searchModel.SearchType == "Quick") //Scenario Fixing (with out supplier detail Quick search should return the TS results)
                    {
                        // Added for PLO scenario
                        var mainSupResourceInfo = mainSupplierResourceInfo?.Count > 0 ? mainSupplierResourceInfo : resourceInfo;

                        mainSupResourceInfo = mainSupResourceInfo.Select(x =>
                        {
                            x.DistanceFromVenderInKm = string.Empty;
                            x.DistanceFromVenderInMile = -1; //def 902 fix : if Google unbale to find location ,Then set distance to negative Number .So that it will result in exception list.
                            return x;
                        }).ToList();

                        /*Note - This is added to populate IsSelected flag of ARS properly. Before fixed it was coming as true for all records if one record is true */
                        result.Add(new ResourceTechSpecSearchResult
                        {
                            Location = searchModel.SearchParameter.SupplierLocation,
                            Address = searchModel.SearchParameter.SupplierFullAddress, // D - 912
                            SupplierLocationId = string.Format("{0}_{1}", searchModel.SearchParameter.SupplierId, searchModel.SearchParameter.SupplierLocation.Trim()),
                            ResourceSearchTechspecInfos = AssignResource(mainSupResourceInfo)
                        });
                    }
                    if (searchModel.SearchParameter.SubSupplierInfos?.Count > 0)
                    {
                        resourceInfo = resourceInfo.Select(x =>
                        {
                            x.DistanceFromVenderInKm = string.Empty;
                            x.DistanceFromVenderInMile = -1; //def 902 fix : if Google unbale to find location ,Then set distance to negative Number .So that it will result in exception list.
                            return x;
                        }).ToList();

                        /*Note - This is added to populate IsSelected flag of ARS properly. Before fixed it was coming as true for all records if one record is true */
                        searchModel.SearchParameter?.SubSupplierInfos?.ToList().ForEach(x3 =>
                        {
                            // Added for PLO scenario
                            var subSupplierresourceInfoClone = resourceInfo;
                            if (subSupplierResourceInfo?.Count > 0)
                            {
                                var subSupplierResourceEpin = x3?.SelectedSubSupplierTS?.Select(x => x.Epin).ToList();
                                subSupplierresourceInfoClone = resourceInfo.Where(x => subSupplierResourceEpin?.Count > 0 && subSupplierResourceEpin.Contains(x.Epin)).ToList();
                            }
                            result.Add(new ResourceTechSpecSearchResult
                            {
                                Location = x3.SubSupplierLocation,
                                Address = x3.SubSupplierFullAddress, // D - 912
                                SupplierLocationId = string.Format("{0}_{1}", x3.SubSupplierId, x3.SubSupplierLocation.Trim()),
                                ResourceSearchTechspecInfos = AssignResource(subSupplierresourceInfoClone)
                            });
                        });
                    }
                }
                PopulateSupplierNameAndType(searchModel, ref result);
            }
            else
            {
                result.Add(new ResourceTechSpecSearchResult
                {
                    Location = searchModel.SearchParameter.FirstVisitLocation ?? string.Empty,
                    Address = searchModel.SearchParameter.SupplierFullAddress ?? string.Empty, // D - 912
                    ResourceSearchTechspecInfos = AssignResource(resourceInfo),
                    SearchExceptionResourceInfos = new List<ResourceSearchTechSpecInfo>()
                });
            }
            return result;
        }

        private List<DistanceSearchResult> CalculateDistanceForExceptionList(List<string> fromLocations, IList<ResourceSearchTechSpecInfo> exceptionList)
        {
            var toLocations = new List<string>();
            List<DistanceSearchResult> distanceResult = new List<DistanceSearchResult>();
            if (fromLocations?.Count(x => !string.IsNullOrEmpty(x)) > 0)
            {
                var distinctResourceInfo = exceptionList?.Where(x => !string.IsNullOrEmpty(x.GoogleAddress))?.GroupBy(x => x.GoogleAddress)?.Select(x => x.FirstOrDefault())?.ToList();
                for (int srcCount = 0; srcCount <= (fromLocations.Count / 2); srcCount++)
                {
                    var sourceLoc = fromLocations?.Skip(srcCount * 2).Take(2).ToList();
                    var resourceSplitCount = sourceLoc?.Count > 0 ? 25 - sourceLoc.Count : 25;
                    if (exceptionList.Count > resourceSplitCount)
                    {
                        for (int page = 0; page <= (exceptionList.Count / resourceSplitCount); page++)
                        {
                            var pagedSearchRes = distinctResourceInfo?.Skip(page * resourceSplitCount).Take(resourceSplitCount).ToList();
                            toLocations = pagedSearchRes?.Where(x => !string.IsNullOrEmpty(x.GoogleAddress))?.Select(x => x.GoogleAddress)?.Distinct()?.ToList();
                            var pagedDistanceResult = this._distanceService.DrivingDistance(sourceLoc, toLocations);
                            if (pagedDistanceResult?.Any() == true)
                                distanceResult?.AddRange(pagedDistanceResult);
                        }
                    }
                    else
                    {
                        toLocations = distinctResourceInfo?.Where(x => !string.IsNullOrEmpty(x.GoogleAddress))?.Select(x => x.GoogleAddress)?.Distinct()?.ToList();
                        var nonPagedDistanceResult = this._distanceService.DrivingDistance(sourceLoc, toLocations)?.ToList();
                        if (nonPagedDistanceResult?.Any() == true)
                            distanceResult?.AddRange(nonPagedDistanceResult);
                    }
                }
            }
            return distanceResult;
        }
        /*Note - This is added to populate IsSelected flag of ARS properly. Before fixed it was coming as true for all records if one record is true */
        private List<ResourceSearchTechSpecInfo> AssignResource(IList<ResourceSearchTechSpecInfo> resourceInfo)
        {
            List<ResourceSearchTechSpecInfo> resources = new List<ResourceSearchTechSpecInfo>();
            var techResources = resourceInfo.OrderBy(x => x.IsSelected)
                                 .ThenBy(x => x.EmploymentTypePrecedence)
                                 .ThenBy(x1 => x1.ScheduleStatusPrecedence)
                                 .ThenBy(x2 => x2.DistanceFromVenderInMile)
                                 .ThenBy(x2 => x2.LastName).ToList();
            foreach (var resource in techResources)
            {
                resources.Add(new ResourceSearchTechSpecInfo
                {
                    City = resource.City,
                    Country = resource.Country,
                    DistanceFromVenderInKm = resource.DistanceFromVenderInKm,
                    DistanceFromVenderInMile = resource.DistanceFromVenderInMile,
                    Email = resource.Email,
                    EmploymentType = resource.EmploymentType,
                    Epin = resource.Epin,
                    ExceptionComment = resource.ExceptionComment,
                    FirstName = resource.FirstName,
                    IsSelected = resource.IsSelected,
                    IsSupplier = resource.IsSupplier,
                    LastName = resource.LastName,
                    MobileNumber = resource.MobileNumber,
                    ProfileStatus = resource.ProfileStatus,
                    ScheduleStatus = resource.ScheduleStatus,
                    State = resource.State,
                    SubDivision = resource.SubDivision,
                    TechSpecGeoLocation = resource.TechSpecGeoLocation,
                    Zip = resource.Zip
                });
            }
            return resources;
        }

        private void ProcessMongoGeoLocationInfo(ref IList<ResourceTechSpecSearchResult> resourceResultInfo)
        {
            IList<MongoGeoCoordinateInfo> mongoGeoCoordinateInfos = null;
            if (resourceResultInfo?.Count > 0)
            {
                var geoSearchModel = resourceResultInfo.Where(x => x.ResourceSearchTechspecInfos?.Count > 0)?
                                         .SelectMany(x => x.ResourceSearchTechspecInfos)?
                                         .Where(x => !string.IsNullOrEmpty(x.Country))?
                                         .Select(x => new MongoGeoCoordinateInfo
                                         {
                                             Country = x.Country,
                                             State = x.State,
                                             City = x.City,
                                             Zip = x.Zip
                                         })?
                                         .GroupBy(x => new { x.Country, x.State, x.City, x.Zip })?
                                         .Select(group => group.First())?
                                         .ToList();

                if (geoSearchModel?.Count > 0)
                {
                    mongoGeoCoordinateInfos = _mongoGeoCoordinateService.Search(geoSearchModel);
                    if (mongoGeoCoordinateInfos?.Count > 0)
                    {
                        resourceResultInfo.ToList().ForEach(x =>
                        {
                            x.ResourceSearchTechspecInfos = x.ResourceSearchTechspecInfos
                                                            .GroupJoin(mongoGeoCoordinateInfos,
                                                                        res => new { res.Country, res.State, res.City, res.Zip },
                                                                        geoLoc => new { geoLoc.Country, geoLoc.State, geoLoc.City, geoLoc.Zip },
                                                                        (res, geoLoc) => new { res, geoLoc })
                                                            .Select(x1 =>
                                                            {
                                                                x1.res.TechSpecGeoLocation = new GeoCoordinateInfo
                                                                {
                                                                    Longitude = x1.geoLoc.FirstOrDefault()?.Location?.Coordinates?.Longitude ?? default(double),
                                                                    Latitude = x1.geoLoc.FirstOrDefault()?.Location?.Coordinates?.Latitude ?? default(double),
                                                                };
                                                                return x1.res;
                                                            }).ToList();
                        });
                    }
                }
            }
        }

        private void ProcessGoogleGeoLocationInfo(ref IList<ResourceTechSpecSearchResult> resourceResultInfo)
        {
            var googleLocationCoordinates = new List<LocationSearchResult>();
            if (resourceResultInfo?.Count > 0)
            {
                var locSearchSup = resourceResultInfo.Where(x => !string.IsNullOrEmpty(x.Address))?
                .Select(x => x.Address) // D - 912
                .Distinct()
                .ToList();

                var locSearchTs = resourceResultInfo.Where(x => x.ResourceSearchTechspecInfos?.Count > 0)?
                                                    .SelectMany(x => x.ResourceSearchTechspecInfos)?
                                                    .Where(x => !string.IsNullOrEmpty(x.Country) &&
                                                                (x.TechSpecGeoLocation == null || (x.TechSpecGeoLocation?.Latitude == default(double) ||
                                                                    x.TechSpecGeoLocation?.Longitude == default(double))))
                                                    ?.GroupBy(x => new { x.Country, x.State, x.City, x.Zip })?
                                                    .Select(x => new MongoGeoCoordinateInfo
                                                    {
                                                        Country = x.Key.Country,
                                                        State = x.Key.State,
                                                        City = x.Key.City,
                                                        Zip = x.Key.Zip
                                                    }).ToList();

                var googleCoordinates = _mongoGeoCoordinateService.SearchAndSyncToMongo(locSearchTs);

                if (locSearchSup?.Count > 0)
                {
                    locSearchSup.ForEach(x =>
                    {
                        var geoLocation = _geoCoordinateService.GetLocationCoordinate(x);
                        if (geoLocation != null)
                            googleLocationCoordinates.Add(geoLocation);
                    });
                }

                resourceResultInfo.ToList().ForEach(x =>
                {
                    var supGeoLocation = googleLocationCoordinates?.FirstOrDefault(x1 => string.Equals(x1.Address, x.Address, StringComparison.OrdinalIgnoreCase));

                    if (supGeoLocation != null)
                    {
                        x.SupplierGeoLocation = new GeoCoordinateInfo
                        {
                            Longitude = supGeoLocation.Location.Coordinates.Longitude,
                            Latitude = supGeoLocation.Location.Coordinates.Latitude,
                        };

                    }
                    if (googleCoordinates?.Count > 0)
                    {
                        x.ResourceSearchTechspecInfos = x.ResourceSearchTechspecInfos
                                                             .GroupJoin(googleCoordinates,
                                                                         res => new { res.Country, res.State, res.City, res.Zip },
                                                                         geoLoc => new { geoLoc.Country, geoLoc.State, geoLoc.City, geoLoc.Zip },
                                                                         (res, geoLoc) => new { res, geoLoc })
                                                             .Select(x1 =>
                                                             {
                                                                 if (x1.res.TechSpecGeoLocation == null || (x1.res.TechSpecGeoLocation?.Latitude == default(double) || x1.res.TechSpecGeoLocation?.Longitude == default(double)))
                                                                 {
                                                                     x1.res.TechSpecGeoLocation = new GeoCoordinateInfo
                                                                     {
                                                                         Longitude = x1.geoLoc.FirstOrDefault() != null ? x1.geoLoc.FirstOrDefault().Location.Coordinates.Longitude : 0,
                                                                         Latitude = x1.geoLoc.FirstOrDefault() != null ? x1.geoLoc.FirstOrDefault().Location.Coordinates.Latitude : 0,
                                                                     };

                                                                 }
                                                                 return x1.res;
                                                             }).ToList();
                    }
                });
            }


        }

        private void PopulateSupplierNameAndType(DomainModel.ResourceSearch searchModel, ref List<ResourceTechSpecSearchResult> resourceResultInfo)
        {
            if (searchModel?.SearchParameter != null && resourceResultInfo?.Count > 0)
            {
                resourceResultInfo.ToList().ForEach(x =>
                {
                    x.SupplierInfo = x.SupplierInfo ?? new List<ResourceSupplierInfo>();
                    if (searchModel?.SearchParameter != null && !string.IsNullOrEmpty(searchModel.SearchParameter.SupplierFullAddress))
                    {
                        string supplierLocation = searchModel?.SearchParameter.Supplier + ", " + searchModel?.SearchParameter.SupplierFullAddress;
                        if (string.Equals(supplierLocation, x.Location))
                        {
                            string[] arrSupplierName = searchModel.SearchParameter?.SupplierFullAddress?.Split(',');
                            x.SupplierInfo.Add(new ResourceSupplierInfo
                            {
                                SupplierName = supplierLocation,
                                SupplierType = "Supplier"
                            });
                        }
                    }
                    if (searchModel.SearchParameter.SubSupplierInfos?.Count > 0)
                    {
                        // searchModel.SearchParameter.SubSupplierInfos.Where(x2 => !string.IsNullOrEmpty(x2.SubSupplierLocation) && string.Equals(Regex.Replace(x2.SubSupplierLocation, "\n|\r", " "), x.Location)).ToList().ForEach(x3 =>
                        searchModel.SearchParameter.SubSupplierInfos.ToList().ForEach(x3 =>
                        {
                            string subSupplierLocation = x3.SubSupplier + ", " + x3.SubSupplierFullAddress;
                            if (string.Equals(x.Location.Trim(), subSupplierLocation.Trim())) //Changes for Live D693
                            {
                                x.SupplierInfo.Add(new ResourceSupplierInfo
                                {
                                    SupplierName = subSupplierLocation,
                                    SupplierType = "SubSupplier"
                                });
                            }
                        });
                    }

                });

            }

        }

        private void FilterResourceOnRadiusInfo(decimal? radius, ref IList<ResourceTechSpecSearchResult> resourceResultInfo)
        {
            resourceResultInfo.ToList().ForEach(x =>
            {
                var tsGrtRadis = x.ResourceSearchTechspecInfos?.Where(x1 => x1.DistanceFromVenderInMile > Convert.ToDouble(radius.Value));
                if (tsGrtRadis != null && tsGrtRadis.Any())
                {
                    x.ResourceSearchTechspecInfos = x.ResourceSearchTechspecInfos.Except(tsGrtRadis).ToList();
                }

            });
        }

        private void FilterResourceOnCompanyDistanceRange(string companyCode, IList<ResourceSearchTechSpecInfo> exceptionTs, DomainModel.ResourceSearch searchModel, ref IList<ResourceTechSpecSearchResult> resourceResultInfo, List<string> locationsWithNoDistanceFromGoogle)
        {
            IList<ResourceTechSpecSearchResult> ExceptionResource = new List<ResourceTechSpecSearchResult>();
            IList<DbModel.Company> dbCompany = null;
            IList<ValidationMessage> validationMessages = null;
            List<int> overrideApprovedEpins = searchModel?.OverridenPreferredResources == null ? new List<int>() : searchModel?.OverridenPreferredResources?.Where(a => a.IsApproved != null && a.IsApproved.Value == true)?.Select(a => a.TechSpecialist.Epin).Distinct().ToList();
            if (!string.IsNullOrEmpty(companyCode))
            {
                bool isValidCompany = _companyService.IsValidCompany(new List<string> { companyCode }, ref dbCompany, ref validationMessages);
                if (isValidCompany && dbCompany != null)
                {
                    resourceResultInfo.ToList().ForEach(x =>
                    {
                        var tsGrtRadis = x.ResourceSearchTechspecInfos?.Where(x1 => (x1.DistanceFromVenderInMile > Convert.ToDouble(dbCompany?.FirstOrDefault()?.ResourceOutsideDistance) || x1.DistanceFromVenderInMile < 0) && !overrideApprovedEpins.Contains(x1.Epin));// Note: (x1.DistanceFromVenderInMile< 0)  added to filter out resources to which distance information NOT FOUND added isSelectd condition to include selected resource beyond configured distance in ARS Grid;
                        if (tsGrtRadis != null)
                        {
                            tsGrtRadis.ToList().ForEach(x1 =>
                            {   //Def902 Fix
                                if (x1.DistanceFromVenderInMile == -1)
                                {
                                    x1.ExceptionComment = "Google distance information not found";
                                }
                                else
                                {
                                    x1.ExceptionComment = !string.IsNullOrEmpty(x1.ExceptionComment) ? string.Format("{0},{1}", x1.ExceptionComment, "Beyond configured distance") : "Beyond configured distance";
                                }
                            });
                            x.SearchExceptionResourceInfos = tsGrtRadis.ToList();
                            if (exceptionTs != null)
                            {
                                var resourceInfoExceptClone = exceptionTs?.Select(x1 => x1.Clone() as ResourceSearchTechSpecInfo).ToList();
                                if (locationsWithNoDistanceFromGoogle.Contains(x?.Address?.Trim()))
                                {
                                    resourceInfoExceptClone?.Select(y =>
                                    {
                                        y.DistanceFromVenderInKm = string.Empty;
                                        y.DistanceFromVenderInMile = -1;
                                        y.ExceptionComment = y.ExceptionComment + "," + "Google distance information not found";
                                        return y;
                                    })?.ToList();
                                }
                                x.SearchExceptionResourceInfos.AddRange(resourceInfoExceptClone);
                                //x.SearchExceptionResourceInfos.AddRange(exceptionTs);
                            }
                        }
                        x.ResourceSearchTechspecInfos = x.ResourceSearchTechspecInfos.Except(tsGrtRadis).ToList();
                    });
                }
            }
        }
    }
}